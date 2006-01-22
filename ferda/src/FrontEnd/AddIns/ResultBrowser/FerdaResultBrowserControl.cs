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
using Ferda.Modules.Boxes.AbstractLMTask;
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

        #endregion


        #region Constructor

        public FerdaResultBrowserControl(string[] localePrefs, HypothesisStruct[] hypotheses, QuantifierProvider[] used_quantifiers, IOtherObjectDisplayer Displayer)
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
            resultBrowser = new FerdaResult(resManager);
            resultBrowser.IceRun(hypotheses, used_quantifiers);
            this.displayer = Displayer;
            this.displayer.Reset();
            
            //test
          //  this.FillPropertyGrid(2);
            
            this.Initialize();
        }

        #endregion


        #region Private methods

        /// <summary>
        /// Method to refresh resultbrowser
        /// </summary>
        private void RefreshBrowser()
        {

            //disabling the column click handler (sorter)
            this.HypothesesListView.ColumnClick -= new ColumnClickEventHandler(ClickOnColumn);

            //clearing all the items
            this.HypothesesListView.Items.Clear();


            //clearing all the columns
            this.HypothesesListView.Columns.Clear();

            //adding 4 main columns
            this.HypothesesListView.Columns.Add(this.ColumnHypotheseName);
            this.HypothesesListView.Columns.Add(this.ColumnAntecedent);
            this.HypothesesListView.Columns.Add(this.ColumnSuccedent);
            this.HypothesesListView.Columns.Add(this.ColumnCondition);

            //setting locale
            this.ChangeLocale(this.resManager);

            //adding handler to display hypothesis details in richtextbox
            HypothesesListView.ItemActivate += new EventHandler(ItemSelectHandler);
            HypothesesListView.ItemCheck += new ItemCheckEventHandler(ItemSelectHandler);
            HypothesesListView.MouseClick += new MouseEventHandler(ItemSelectHandler);
            HypothesesListView.KeyDown += new KeyEventHandler(ItemSelectHandler);
            HypothesesListView.KeyUp += new KeyEventHandler(ItemSelectHandler);

             //adding the sorter
            HypothesesListView.ColumnClick += new ColumnClickEventHandler(ClickOnColumn);

            //adding the column names selected in the context menu
            foreach (String name in resultBrowser.GetSelectedColumnNames())
            {
                ColumnHeader header = new ColumnHeader();
                header.Text = name;
                header.Width = 100;
                this.HypothesesListView.Columns.Add(header);
            }


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
                foreach (object value in resultBrowser.QuantifierValues(hypothese))
                {
                   item.SubItems.Add(value.ToString());
                }

                item.Tag = i;
                HypothesesListView.Items.Add(item);
                i++;
            }
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

            this.RefreshBrowser();
            this.ToolStripShowGraphEdit.Click += new EventHandler(ToolStripShowGraphEdit_Click);
        }

        #endregion


        #region Handlers

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
                hypothesis = this.resultBrowser.GetHypothese(index);
            }

            catch
            {
                return;
            }
            this.FillPropertyGrid(hypothesis);
            this.DrawBarsFromFirstTable(hypothesis, this.ContingencyTableChart);
        }

        /// <summary>
        /// Method for sorting by the column
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickOnColumn(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            Sorter columnSorter = new Sorter();
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
        /// <param name="hypotheseId">Id of the hypothesis to take data from</param>
        private void FillPropertyGrid(HypothesisStruct hypothesis)
        {
            PropertyTable table = new PropertyTable();

            #region Filling in name, antecedent, succedent, condition

            //name
            PropertySpec hName = new PropertySpec(
                resManager.GetString("ColumnHypotheseName"),
                typeof(string),
                resManager.GetString("HypothesisData"),
                resManager.GetString("HypothesisData"),
                FerdaResult.GetHypothesisName(hypothesis)
                );
            hName.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            table.Properties.Add(hName);
            table[resManager.GetString("ColumnHypotheseName")] = FerdaResult.GetHypothesisName(hypothesis);

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
            table[resManager.GetString("ColumnAntecedent")] = FerdaResult.GetHypothesisName(hypothesis);

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
            table[resManager.GetString("ColumnSuccedent")] = FerdaResult.GetHypothesisName(hypothesis);

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
            table[resManager.GetString("ColumnCondition")] = FerdaResult.GetHypothesisName(hypothesis);

            #endregion


            #region Used quantifiers and their values

            //used quantifiers and their values
            List<string> quantifierNames = this.resultBrowser.GetUsedQuantifiersNames();
            List<double> quantifierValues = this.resultBrowser.QuantifierValues(hypothesis);

            //if the count is not the same, something must be very wrong...
            if (quantifierNames.Count == quantifierValues.Count)
            {
                for (int i = 0; i < quantifierValues.Count; i++)
                {
                    PropertySpec hQuantifier = new PropertySpec(
                    quantifierNames[i].ToString(),
                    typeof(double),
                    resManager.GetString("AppliedQuantifiers"),
                    resManager.GetString("AppliedQuantifiers"),
                    quantifierValues[i]
                    );
                    hQuantifier.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
                    table.Properties.Add(hQuantifier);
                    table[quantifierNames[i].ToString()] = quantifierValues[i];
                }
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
                i1++;
            }

            #endregion

            this.displayer.Reset();
            this.displayer.OtherObjectAdapt(table);
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
            this.ColumnHypotheseName.Text = rm.GetString("ColumnHypotheseName");
            this.ColumnSuccedent.Text = rm.GetString("ColumnSuccedent");
            this.GroupBoxChangeGraph.Text = rm.GetString("GraphViewOptions");
            this.ToolStripShowGraphEdit.Text = rm.GetString("GraphViewOptions");
            this.Label3dpercent.Text = rm.GetString("3dpercent");
            this.ContingencyTableChart.Header.Lines = new string[] { rm.GetString("ContingencyTable") };
            this.LabelHOffset.Text = rm.GetString("LabelHOffset");
            this.LabelVOffset.Text = rm.GetString("LabelVOffset");
            this.LabelZoom.Text = rm.GetString("LabelZoom");
            this.LabelRotation.Text = rm.GetString("LabelRotation");
        }

        #endregion

    }
}

