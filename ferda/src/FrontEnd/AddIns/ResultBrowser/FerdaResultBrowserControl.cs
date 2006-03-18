using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using FrontEnd.AddIns.ResultBrowser.NonGUIClasses;
using Ferda;
using Ferda.Modules;
using Ferda.FrontEnd.AddIns;
using Ferda.FrontEnd.External;
using Ferda.FrontEnd.Properties;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractLMTask;
using System.Resources;
using System.Reflection;
using Ferda.ModulesManager;


namespace Ferda.FrontEnd.AddIns.ResultBrowser
{
    public partial class FerdaResultBrowserControl : UserControl
    {
        #region Private variables

        /// <summary>
        /// Ferda result browser class, will be displayed in the listview
        /// </summary>
        FerdaResult resultBrowser;


        /// <summary>
        /// Resource manager
        /// </summary>
        private ResourceManager resManager;


        /// <summary>
        /// Localization string, en-US or cs-CZ for now.
        /// </summary>
        private string localizationString;


        /// <summary>
        /// Implementation of propertygrid
        /// </summary>
        IOtherObjectDisplayer displayer;

        /// <summary>
        /// Auxiliary index for previously selected hypothesis
        /// </summary>
        private int previousIndex = -1;

        /// <summary>
        /// Sorter for the listview
        /// </summary>
        Sorter columnSorter = new Sorter();

        /// <summary>
        /// Counter for loading hypotheses
        /// </summary>
        private int loadingCounter = 0;

        /// <summary>
        /// Count of hypotheses
        /// </summary>
        private int hypothesesCount = 0;

        #endregion

        #region Constructor

        public FerdaResultBrowserControl(string[] localePrefs, HypothesisStruct[] hypotheses, QuantifierProvider[] used_quantifiers, IOtherObjectDisplayer Displayer, List<Ferda.Statistics.StatisticsProviderPrx> statisticsProxies)
        {
            //setting the ResManager resource manager and localization string
            string locale;
            try
            {
                locale = localePrefs[0];
                localizationString = locale;
                locale = "Ferda.FrontEnd.AddIns.ResultBrowser.Localization_" + locale;
                resManager = new ResourceManager(locale, Assembly.GetExecutingAssembly());
            }
            catch
            {
                resManager = new ResourceManager("Ferda.FrontEnd.AddIns.ResultBrowser.Localization_en-US",
            Assembly.GetExecutingAssembly());
                localizationString = "en-US";
            }
            InitializeComponent();
            InitializeGraph();
            this.hypothesesCount = hypotheses.Length;
            resultBrowser = new FerdaResult(resManager);
            resultBrowser.IceTicked += new IceTick(resultBrowser_IceTicked);
            resultBrowser.Initialize(hypotheses, used_quantifiers, statisticsProxies);
            this.displayer = Displayer;
            this.displayer.Reset();
            this.Initialize();
        }

        #endregion


        #region Private methods

        /// <summary>
        /// Method to refresh resultbrowser
        /// </summary>
        private void RefreshBrowser()
        {
            this.columnSorter.column = 0;
            //disabling the column click handler (sorter)
            this.HypothesesListView.ColumnClick -= new ColumnClickEventHandler(ClickOnColumn);
            //columnSorter.
            HypothesesListView.ListViewItemSorter = null;

            //clearing all the items
            this.HypothesesListView.Items.Clear();

            while (this.HypothesesListView.Columns.Count > 4)
            {
                this.HypothesesListView.Columns.RemoveAt(4);
            }

            //adding handler to display hypothesis details in richtextbox
            HypothesesListView.ItemActivate += new EventHandler(ItemSelectHandler);
            HypothesesListView.ItemCheck += new ItemCheckEventHandler(ItemSelectHandler);
            HypothesesListView.MouseClick += new MouseEventHandler(ItemSelectHandler);
            HypothesesListView.KeyDown += new KeyEventHandler(ItemSelectHandler);
            HypothesesListView.KeyUp += new KeyEventHandler(ItemSelectHandler);

            //adding the column names selected in the context menu
            foreach (String name in resultBrowser.GetSelectedColumnNames())
            {
                ColumnHeader header = new ColumnHeader();
                header.Text = name;
                header.Name = name;
                header.Width = 200;
                header.TextAlign = HorizontalAlignment.Right;
                this.HypothesesListView.Columns.Add(header);
            }
            int precision = Convert.ToInt32(this.NumericUpDownDecimals.Value);
           
            //adding hypotheses
            int i = 0;
            foreach (HypothesisStruct hypothese in resultBrowser.GetAllHypotheses())
            {
                ListViewItem item = new ListViewItem(FerdaResult.GetHypothesisName(hypothese));

                //antecedent
                item.SubItems.Add(FerdaResult.GetAntecedent(hypothese));

                //succedent
                item.SubItems.Add(FerdaResult.GetSuccedent(hypothese));

                //Condition
                item.SubItems.Add(FerdaResult.GetCondition(hypothese));

                //quantifiers
                foreach (double value in resultBrowser.ReadSelectedQuantifiersFromCache(i, precision))
                {
                    item.SubItems.Add(value.ToString("N" + precision.ToString()));
                }

                item.Tag = i;
                HypothesesListView.Items.Add(item);
                i++;
            }
            this.LabelCurrentlySorted.Text = resManager.GetString("SortedByNone");
            //adding the sorter
            HypothesesListView.ColumnClick += new ColumnClickEventHandler(ClickOnColumn);
        }

        /// <summary>
        /// Method for Result listview initialization
        /// </summary>
        private void Initialize()
        {
            //setting locale
            this.ChangeLocale(this.resManager);

            //adding unused quantifiers in the context menu
            ToolStripMenuItem tempitem;
            foreach (String item in resultBrowser.GetUsedColumnNames())
            {
                tempitem = (ToolStripMenuItem)this.QuantifiersListContextMenu.Items.Add(item);
                tempitem.Click += new EventHandler(ItemClick);
                tempitem.CheckOnClick = true;
            }

            foreach (String item in resultBrowser.GetUnusedColumnNames())
            {
                tempitem = (ToolStripMenuItem)this.QuantifiersListContextMenu.Items.Add(item);
                tempitem.Click += new EventHandler(ItemClick);
                tempitem.CheckOnClick = true;
            }

            //filling the listbox for sorting by statistics
            foreach (string name in resultBrowser.ReadStatisticsNamesFromCache())
            {
                this.ComboBoxSortStatistics.Items.Add(name);
            }

            this.RefreshBrowser();
            this.ToolStripShowGraphEdit.Click += new EventHandler(ToolStripShowGraphEdit_Click);
            this.ToolStripCopyChart.Click += new EventHandler(ToolStripCopyChart_Click);
        }

        #endregion


        #region Handlers

        /// <summary>
        /// Method for handling one IceTick, refreshes the progress bar
        /// </summary>
        void resultBrowser_IceTicked()
        {
            if (hypothesesCount > 0)
            {
                this.ProgressBarIceTicks.Value = (int)(((float)loadingCounter / (float)hypothesesCount) * 100);
            }
        }

        /// <summary>
        /// Right-click on chart handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ToolStripShowGraphEdit_Click(object sender, EventArgs e)
        {
            if (!ToolStripShowGraphEdit.Checked)
            {
                this.GroupBoxChangeGraph.Visible = true;
                this.GroupBoxChangeGraph.BringToFront();
                this.ToolStripShowGraphEdit.Checked = true;
            }

            else
            {
                this.GroupBoxChangeGraph.Visible = false;
                this.ToolStripShowGraphEdit.Checked = false;
            }
        }

        /// <summary>
        /// Method which handles selection change in the combobox and re-sorts the listview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxSortStatistics_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int index = 0;
            try
            {
                index = Convert.ToInt32(this.ComboBoxSortStatistics.SelectedIndex);
            }
            catch
            {
                return;
            }
            string statisticsName = String.Empty;

            try
            {
                statisticsName = this.resultBrowser.ReadStatisticsNamesFromCache()[index];
            }
            catch
            {
                return;
            }
            Tuple[] tuples = new Tuple[this.hypothesesCount];
            int precision = Convert.ToInt32(this.NumericUpDownDecimals.Value);
            
            for (int k = 0; k < this.hypothesesCount; k++)
            {
              //  Tuple tempTuple = new Tuple();
                tuples[k].HypId = k;
                tuples[k].Value = this.resultBrowser.ReadStatisticsFromCache(k, precision)[index].Value;
            }
            List<Tuple> tuples1 = new List<Tuple>();
            tuples1.AddRange(tuples);
            tuples1.Sort();
            tuples = tuples1.ToArray();
            this.columnSorter.column = -1;
            //disabling the column click handler (sorter)
            //this.HypothesesListView.ColumnClick -= new ColumnClickEventHandler(ClickOnColumn);
            HypothesesListView.ListViewItemSorter = null;
            this.HypothesesListView.Items.Clear();
            for (int i = 0; i < tuples.Length; i++)
            {
                HypothesisStruct hypothese = resultBrowser.GetHypothese(tuples[i].HypId);
                ListViewItem item = new ListViewItem(FerdaResult.GetHypothesisName(hypothese));

                //antecedent
                item.SubItems.Add(FerdaResult.GetAntecedent(hypothese));

                //succedent
                item.SubItems.Add(FerdaResult.GetSuccedent(hypothese));

                //Condition
                item.SubItems.Add(FerdaResult.GetCondition(hypothese));

                //quantifiers
                foreach (double value in resultBrowser.ReadSelectedQuantifiersFromCache(tuples[i].HypId, precision))
                {
                    item.SubItems.Add(value.ToString("N" + precision.ToString()));
                }
                item.Tag = tuples[i].HypId;
                HypothesesListView.Items.Add(item);
            }
            this.LabelCurrentlySorted.Text = statisticsName;
        //    this.HypothesesListView.ColumnClick += new ColumnClickEventHandler(ClickOnColumn);
        }

        /// <summary>
        /// Function handling the selection of the quantifiers from the dropdown context menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemClick(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            if (item.Checked)
            {
                this.resultBrowser.AddColumn(item.Text);
            }
            else
            {
                this.resultBrowser.RemoveColumn(item.Text);
                this.HypothesesListView.ColumnClick -= new ColumnClickEventHandler(ClickOnColumn);
                this.HypothesesListView.Columns.RemoveByKey(item.Text);
            }
            this.RefreshBrowser();
        }


        /// <summary>
        /// Method for filling the chart and propertygrid with the hypothese data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemSelectHandler(object sender, EventArgs e)
        {
            ListView view = (ListView)sender;
            int index = 0;
            HypothesisStruct hypothesis;
            try
            {
                index = (int)view.SelectedItems[0].Tag;
                if (index == previousIndex)
                {
                    return;
                }
                else
                {
                    hypothesis = this.resultBrowser.GetHypothese(index);
                    previousIndex = index;
                }
            }

            catch
            {
                return;
            }
            this.FillPropertyGrid(index);
            this.DrawBarsFromFirstTable(hypothesis, this.ContingencyTableChart);
        }

        /// <summary>
        /// Method for sorting by the column
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickOnColumn(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            columnSorter.column = e.Column;
            if ((columnSorter.bAscending = (HypothesesListView.Sorting == SortOrder.Ascending)))
                HypothesesListView.Sorting = SortOrder.Descending;
            else
                HypothesesListView.Sorting = SortOrder.Ascending;

            HypothesesListView.ListViewItemSorter = columnSorter;
        }

        /// <summary>
        /// Fills propertgrid with the data of the selected hypothesis
        /// </summary>
        /// <param name="hypothesisId">Id of the hypothesis to take data from</param>
        private void FillPropertyGrid(int hypothesisId)
        {
            HypothesisStruct hypothesis = this.resultBrowser.GetHypothese(hypothesisId);
            PropertyTable table = new PropertyTable();


            #region Filling in name, antecedent, succedent, condition

            //name
            PropertySpec hName = new PropertySpec(
                resManager.GetString("ColumnHypothesisName"),
                typeof(string),
                resManager.GetString("HypothesisData"),
                resManager.GetString("HypothesisData"),
                FerdaResult.GetHypothesisName(hypothesis)
                );
            hName.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            table.Properties.Add(hName);
            table[resManager.GetString("ColumnHypothesisName")] = FerdaResult.GetHypothesisName(hypothesis);

            //antecedent
            PropertySpec hAntecedent = new PropertySpec(
                resManager.GetString("ColumnAntecedent"),
                typeof(string),
                resManager.GetString("HypothesisData"),
                resManager.GetString("HypothesisData"),
                FerdaResult.GetAntecedent(hypothesis)
                );
            hAntecedent.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            table.Properties.Add(hAntecedent);
            table[resManager.GetString("ColumnAntecedent")] = FerdaResult.GetAntecedent(hypothesis);

            //succedent
            PropertySpec hSuccedent = new PropertySpec(
                resManager.GetString("ColumnSuccedent"),
                typeof(string),
                resManager.GetString("HypothesisData"),
                resManager.GetString("HypothesisData"),
                FerdaResult.GetSuccedent(hypothesis)
                );
            hSuccedent.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            table.Properties.Add(hSuccedent);
            table[resManager.GetString("ColumnSuccedent")] = FerdaResult.GetSuccedent(hypothesis);

            //condition
            PropertySpec hCondition = new PropertySpec(
                resManager.GetString("ColumnCondition"),
                typeof(string),
                resManager.GetString("HypothesisData"),
                resManager.GetString("HypothesisData"),
                FerdaResult.GetCondition(hypothesis)
                );
            hCondition.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            table.Properties.Add(hCondition);
            table[resManager.GetString("ColumnCondition")] = FerdaResult.GetCondition(hypothesis);

            #endregion


            #region Used quantifiers and their values

            //used quantifiers and their values
            List<string> names = this.resultBrowser.GetAllQuantifierNames();
            double[] quantifiers = this.resultBrowser.ReadAllQuantifiersFromCache(hypothesisId, Convert.ToInt32(this.NumericUpDownDecimals.Value));

            for (int i = 0; i < quantifiers.Length; i++)
            {
                PropertySpec hQuantifier = new PropertySpec(
                names[i],
                typeof(double),
                resManager.GetString("AppliedQuantifiers"),
                resManager.GetString("AppliedQuantifiers"),
                quantifiers[i]
                );
                hQuantifier.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
                table.Properties.Add(hQuantifier);
                table[names[i]] = quantifiers[i];
            }

            #endregion


            #region Contingency tables

            int i1 = 1;
            int j1 = 1;
            foreach (int[] row in hypothesis.quantifierSetting.firstContingencyTableRows)
            {
                foreach (int value in row)
                {
                    PropertySpec hValue = new PropertySpec(
                    i1.ToString() + "-" + j1.ToString(),
                    typeof(int),
                    "1. " + resManager.GetString("ContingencyTable"),
                    "1. " + resManager.GetString("ContingencyTable"),
                    value
                    );
                    hValue.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
                    table.Properties.Add(hValue);
                    table[i1.ToString() + "-" + j1.ToString()] = value;
                    j1++;
                }
                j1 = 1;
                i1++;
            }

            i1 = 1;
            j1 = 1;
            foreach (int[] row in hypothesis.quantifierSetting.secondContingencyTableRows)
            {
                foreach (int value in row)
                {
                    PropertySpec hValue = new PropertySpec(
                    i1.ToString() + "-" + j1.ToString(),
                    typeof(int),
                    "2. " + resManager.GetString("ContingencyTable"),
                    "2. " + resManager.GetString("ContingencyTable"),
                    value
                    );
                    hValue.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
                    table.Properties.Add(hValue);
                    table[i1.ToString() + "-" + j1.ToString()] = value;
                    j1++;
                }
                j1 = 1;
                i1++;
            }

            #endregion


            #region Statistics functions

            foreach (CountedValues value in this.resultBrowser.ReadStatisticsFromCache(hypothesisId, Convert.ToInt32(this.NumericUpDownDecimals.Value)))
            {
                string temp = String.Empty;
                temp = value.Value.ToString();
                PropertySpec statistics = new PropertySpec(
                    value.Name,
                    typeof(string),
                    this.resManager.GetString("Statistics"),
                    this.resManager.GetString("Statistics"),
                    temp
                    );
                statistics.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
                table.Properties.Add(statistics);
                table[value.Name] = temp;
            }

            #endregion

            this.displayer.Reset();
            this.displayer.OtherObjectAdapt(table);
        }

        /// <summary>
        /// Handler for changing numeric precision for quantifiers values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumericUpDownDecimals_ValueChanged(object sender, EventArgs e)
        {
            this.RefreshBrowser();
        }

        #endregion


        #region Localization

        /// <summary>
        /// Resource manager of the application, it is filled according to the
        /// current localization
        /// </summary>
        public ResourceManager ResManager
        {
            get
            {
                return resManager;
            }
        }


        /// <summary>
        /// Localization string of the application, possible values are "en-US" and "cs-CZ"
        /// </summary>
        public string LocalizationString
        {
            get
            {
                return localizationString;
            }
        }

        /// <summary>
        /// Method to change l10n.
        /// </summary>
        /// <param name="rm">Resource manager to handle new l10n resource</param>
        private void ChangeLocale(ResourceManager rm)
        {
            this.ColumnAntecedent.Text = rm.GetString("ColumnAntecedent");
            this.ColumnCondition.Text = rm.GetString("ColumnCondition");
            this.ColumnHypotheseName.Text = rm.GetString("ColumnHypothesisName");
            this.ColumnSuccedent.Text = rm.GetString("ColumnSuccedent");
            this.GroupBoxChangeGraph.Text = rm.GetString("GraphViewOptions");
            this.ToolStripShowGraphEdit.Text = rm.GetString("GraphViewOptions");
            this.Label3dpercent.Text = rm.GetString("3dpercent");
            this.ContingencyTableChart.Header.Lines = new string[] { rm.GetString("ContingencyTable") };
            this.LabelHOffset.Text = rm.GetString("LabelHOffset");
            this.LabelVOffset.Text = rm.GetString("LabelVOffset");
            this.LabelZoom.Text = rm.GetString("LabelZoom");
            this.ToolStripCopyChart.Text = rm.GetString("CopyChart");
            this.CheckBoxShowLabels.Text = rm.GetString("ShowLabels");

            this.LabelSortHypotheses.Text = rm.GetString("SortBy");
            this.LabelSortedBy.Text = rm.GetString("SortedBy");

            this.LabelCurrentlySorted.Text = rm.GetString("SortedByNone");
            this.LabelNumeric.Text = rm.GetString("LabelNumeric");
            this.LabelProgressBar.Text = rm.GetString("HypothesesLoading");
        }

        #endregion
    }
}

