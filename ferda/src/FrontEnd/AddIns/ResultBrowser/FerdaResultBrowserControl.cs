// FerdaResultBrowserControl.cs - UserControl class for displaying results
//
// Author: Alexander Kuzmin <alexander.kuzmin@gmail.com>
//
// Copyright (c) 2005 Alexander Kuzmin
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

#region using...

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using Ferda.FrontEnd.AddIns.ResultBrowser.NonGUIClasses;
using Ferda;
using Ferda.Modules;
using Ferda.FrontEnd.AddIns;
using Ferda.FrontEnd.External;
using Ferda.FrontEnd.Properties;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractLMTask;
using System.Resources;
using System.Reflection;
using Ferda.ModulesManager;

#endregion


namespace Ferda.FrontEnd.AddIns.ResultBrowser
{
    /// <summary>
    /// USerControl class for displaying results
    /// </summary>
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
        
        /// <summary>
        /// TaskType
        /// </summary>
        private string taskType = String.Empty;

        #endregion


        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="localePrefs">Localeprefs</param>
        /// <param name="hypotheses">Hypotheses to display</param>
        /// <param name="used_quantifiers">Used quantifiers</param>
        /// <param name="Displayer">Propertygrid</param>
        /// <param name="statisticsProxies">Statistics proxies</param>
        public FerdaResultBrowserControl(string[] localePrefs, HypothesisStruct[] hypotheses, QuantifierProvider[] used_quantifiers, IOtherObjectDisplayer Displayer, List<Ferda.Statistics.StatisticsProviderPrx> statisticsProxies, string taskType)
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
            this.taskType = taskType;
            resultBrowser = new FerdaResult(resManager);
            resultBrowser.IceTicked += new LongRunTick(resultBrowser_IceTicked);
            resultBrowser.IceComplete += new LongRunCompleted(resultBrowser_IceComplete);
            //setting locale
            this.ChangeLocale(this.resManager);
            this.displayer = Displayer;
            this.displayer.Reset();
            this.PreloadDisable();
            resultBrowser.Initialize(hypotheses, used_quantifiers, statisticsProxies);
        }
      
        #endregion


        #region Private methods

        /// <summary>
        /// Method which temporarily disables all of the user controls until initialization is done
        /// </summary>
        private void PreloadDisable()
        {
           // this.HypothesesListView.Visible = false;
            this.CheckedListBoxAntecedents.Enabled = false;
            this.CheckedListBoxConditions.Enabled = false;
            this.CheckedListBoxSuccedents.Enabled = false;
            this.ButtonSubmitFilter.Enabled = false;
            this.ComboBoxSortStatistics.Enabled = false;
            this.NumericUpDownDecimals.Enabled = false;
        }

        /// <summary>
        /// Method which enables the disabled controls
        /// </summary>
        private void AfterLoadEnable()
        {
            this.HypothesesListView.Enabled = true;
            this.HypothesesListView.Visible = true;
            this.CheckedListBoxAntecedents.Enabled = true;
            this.CheckedListBoxConditions.Enabled = true;
            this.CheckedListBoxSuccedents.Enabled = true;
            this.ButtonSubmitFilter.Enabled = true;
            this.ComboBoxSortStatistics.Enabled = true;
            this.NumericUpDownDecimals.Enabled = true;
        }

        /// <summary>
        /// Method to refresh resultbrowser
        /// </summary>
        private void RefreshBrowser()
        {
            //disabling the column click handler (sorter)
            this.columnSorter.column = 0;
            this.HypothesesListView.ColumnClick -= new ColumnClickEventHandler(ClickOnColumn);
            HypothesesListView.Sorting = SortOrder.None;
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
            Dictionary<int,HypothesisStruct> hypoTemp = resultBrowser.AllFilteredHypotheses;
            foreach (KeyValuePair<int, HypothesisStruct> hypothesis in hypoTemp)
            {
                ListViewItem item = new ListViewItem(FerdaResult.GetHypothesisName(hypothesis.Value));

                //antecedent
                item.SubItems.Add(FerdaResult.GetAntecedentString(hypothesis.Value));

                //succedent
                item.SubItems.Add(FerdaResult.GetSuccedentString(hypothesis.Value));

                //Condition
                item.SubItems.Add(FerdaResult.GetConditionString(hypothesis.Value));

                //quantifiers
                double [] doubleTemp = resultBrowser.ReadSelectedQuantifiersFromCache(i, precision);
                foreach (double value in doubleTemp)
                {
                    item.SubItems.Add(value.ToString("N" + precision.ToString()));
                }

                item.Tag = hypothesis.Key;
                HypothesesListView.Items.Add(item);
                i++;
            }
            this.LabelCount.Text = "(" + i + "/" + hypothesesCount + ")";
            this.LabelCurrentlySorted.Text = resManager.GetString("SortedByNone");
            //adding the sorter
            HypothesesListView.ColumnClick += new ColumnClickEventHandler(ClickOnColumn);
        }

        /// <summary>
        /// Method for Result listview initialization
        /// </summary>
        private void Initialize()
        {
            //no succedent for cf or sdcf
            if ((this.taskType == "LISpMinerTasks.CFTask") || (this.taskType == "LISpMinerTasks.SDCFTask"))
            {
                this.HypothesesListView.Columns[2].Width = 0;
            }
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
            this.InitFiltersCombos();
            this.RefreshBrowser();
            this.ToolStripShowGraphEdit.Click += new EventHandler(ToolStripShowGraphEdit_Click);
            this.ToolStripCopyChart.Click += new EventHandler(ToolStripCopyChart_Click);
            this.HypothesesListView.Visible = true;
            this.HypothesesListView.Enabled = true;
        }

        /// <summary>
        /// Method to re-read items to listview
        /// </summary>
        private void ReReadItems()
        {
            //disabling the column click handler (sorter)
            this.columnSorter.column = 0;
            this.HypothesesListView.ColumnClick -= new ColumnClickEventHandler(ClickOnColumn);
            HypothesesListView.Sorting = SortOrder.None;
            HypothesesListView.ListViewItemSorter = null;

            //clearing all the items
            this.HypothesesListView.Items.Clear();

            int precision = Convert.ToInt32(this.NumericUpDownDecimals.Value);

            //adding hypotheses
            int i = 0;
            foreach (KeyValuePair<int, HypothesisStruct> hypothesis in resultBrowser.AllFilteredHypotheses)
            {
                ListViewItem item = new ListViewItem(FerdaResult.GetHypothesisName(hypothesis.Value));

                //antecedent
                item.SubItems.Add(FerdaResult.GetAntecedentString(hypothesis.Value));

                //succedent
                item.SubItems.Add(FerdaResult.GetSuccedentString(hypothesis.Value));

                //Condition
                item.SubItems.Add(FerdaResult.GetConditionString(hypothesis.Value));

                //quantifiers
                foreach (double value in resultBrowser.ReadSelectedQuantifiersFromCache(i, precision))
                {
                    item.SubItems.Add(value.ToString("N" + precision.ToString()));
                }

                item.Tag = hypothesis.Key;
                HypothesesListView.Items.Add(item);
                i++;
            }
            this.LabelCount.Text = "(" + i + "/" + hypothesesCount + ")";
            this.LabelCurrentlySorted.Text = resManager.GetString("SortedByNone");
            //adding the sorter
            HypothesesListView.ColumnClick += new ColumnClickEventHandler(ClickOnColumn);
        }

        /// <summary>
        /// Method which fills in all the available values for filters
        /// </summary>
        private void InitFiltersCombos()
        {
            foreach (KeyValuePair<string, LiteralFilter> filter in this.resultBrowser.AntecedentFilter)
            {
                this.CheckedListBoxAntecedents.Items.Add(filter.Key, true);
            }

            foreach (KeyValuePair<string, LiteralFilter> filter in this.resultBrowser.SuccedentFilter)
            {
                this.CheckedListBoxSuccedents.Items.Add(filter.Key, true);
            }

            foreach (KeyValuePair<string, LiteralFilter> filter in this.resultBrowser.ConditionFilter)
            {
                this.CheckedListBoxConditions.Items.Add(filter.Key, true);
            }
        }

        #endregion


        #region Handlers

        /// <summary>
        /// Method to complete initialization after ice communication has been completed
        /// </summary>
        void resultBrowser_IceComplete()
        {
            this.Initialize();
            this.LabelProgressBar.Visible = false;
            this.ProgressBarIceTicks.Visible = false;
            this.StatusStrip.Visible = false;
            this.AfterLoadEnable();
        }


        /// <summary>
        /// Method for handling one IceTick, refreshes the progress bar
        /// </summary>
        void resultBrowser_IceTicked()
        {
            if (hypothesesCount > 0)
            {
                this.ProgressBarIceTicks.Value = (int)(((float)loadingCounter / (float)hypothesesCount) * 100);
                loadingCounter++;
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
            HypothesesListView.Sorting = SortOrder.None;
            this.HypothesesListView.Items.Clear();
            for (int i = 0; i < tuples.Length; i++)
            {
                HypothesisStruct hypothese = resultBrowser.GetHypothese(tuples[i].HypId);
                ListViewItem item = new ListViewItem(FerdaResult.GetHypothesisName(hypothese));

                //antecedent
                item.SubItems.Add(FerdaResult.GetAntecedentString(hypothese));

                //succedent
                item.SubItems.Add(FerdaResult.GetSuccedentString(hypothese));

                //Condition
                item.SubItems.Add(FerdaResult.GetConditionString(hypothese));

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
            if (this.RadioFirstTable.Checked)
            {
                this.DrawBarsFromFirstTable(hypothesis, this.ContingencyTableChart);
            }
            else
            {
                this.DrawBarsFromSecondTable(hypothesis, this.ContingencyTableChart);
            }
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

            #region Filling in name, antecedent, succedent, condition, first set, second set

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
                FerdaResult.GetAntecedentString(hypothesis)
                );
            hAntecedent.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            table.Properties.Add(hAntecedent);
            table[resManager.GetString("ColumnAntecedent")] = FerdaResult.GetAntecedentString(hypothesis);

            //succedent
            PropertySpec hSuccedent = new PropertySpec(
                resManager.GetString("ColumnSuccedent"),
                typeof(string),
                resManager.GetString("HypothesisData"),
                resManager.GetString("HypothesisData"),
                FerdaResult.GetSuccedentString(hypothesis)
                );
            hSuccedent.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            table.Properties.Add(hSuccedent);
            table[resManager.GetString("ColumnSuccedent")] = FerdaResult.GetSuccedentString(hypothesis);

            //condition
            PropertySpec hCondition = new PropertySpec(
                resManager.GetString("ColumnCondition"),
                typeof(string),
                resManager.GetString("HypothesisData"),
                resManager.GetString("HypothesisData"),
                FerdaResult.GetConditionString(hypothesis)
                );
            hCondition.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            table.Properties.Add(hCondition);
            table[resManager.GetString("ColumnCondition")] = FerdaResult.GetConditionString(hypothesis);

            //first set
            string temp1 = FerdaResult.GetFirstSetString(hypothesis);
            if(!(temp1.CompareTo(String.Empty) == 0))
            {
                PropertySpec hFirstSet = new PropertySpec(
                "1" + resManager.GetString("FirstSet"),
                typeof(string),
                resManager.GetString("HypothesisData"),
                resManager.GetString("HypothesisData"),
                temp1
                );
            hFirstSet.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            table.Properties.Add(hFirstSet);
            table["1" + resManager.GetString("FirstSet")] = temp1;
            }
            
            //second set
            temp1 = FerdaResult.GetSecondSetString(hypothesis);
            if (!(temp1.CompareTo(String.Empty) == 0))
            {
                PropertySpec hSecondSet = new PropertySpec(
                "2" + resManager.GetString("SecondSet"),
                typeof(string),
                resManager.GetString("HypothesisData"),
                resManager.GetString("HypothesisData"),
                temp1
                );
                hSecondSet.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
                table.Properties.Add(hSecondSet);
                table["2" + resManager.GetString("SecondSet")] = temp1;
            }
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

            if (FerdaResult.IsFFT(hypothesis))
            {
                foreach (int[] row in hypothesis.quantifierSetting.firstContingencyTableRows)
                {
                    foreach (int value in row)
                    {
                        string temp = String.Empty;
                        switch (i1)
                        {
                            case 1:
                                if (j1 == 1)
                                {
                                    temp = "a";
                                }
                                else
                                {
                                    temp = "b";
                                }
                                break;

                            default:
                                if (j1 == 1)
                                {
                                    temp = "c";
                                }
                                else
                                {
                                    temp = "d";
                                }

                                break;
                        }
                        PropertySpec hValue = new PropertySpec(
                        temp,
                        typeof(int),
                        "1. " + resManager.GetString("ContingencyTable"),
                        "1. " + resManager.GetString("ContingencyTable"),
                        value
                        );
                        hValue.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
                        table.Properties.Add(hValue);
                        table[temp] = value;
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
                        string temp = String.Empty;
                        switch (i1)
                        {
                            case 1:
                                if (j1 == 1)
                                {
                                    temp = "a";
                                }
                                else
                                {
                                    temp = "b";
                                }
                                break;

                            default:
                                if (j1 == 1)
                                {
                                    temp = "c";
                                }
                                else
                                {
                                    temp = "d";
                                }

                                break;
                        }
                        PropertySpec hValue = new PropertySpec(
                        temp,
                        typeof(int),
                        "2. " + resManager.GetString("ContingencyTable"),
                        "2. " + resManager.GetString("ContingencyTable"),
                        value
                        );
                        hValue.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
                        table.Properties.Add(hValue);
                        table[temp] = value;
                        j1++;
                    }
                    j1 = 1;
                    i1++;
                }
            }
            else
            {
                foreach (int[] row in hypothesis.quantifierSetting.firstContingencyTableRows)
                {
                    foreach (int value in row)
                    {
                        string antName = i1.ToString();
                        string sucName = j1.ToString();
                        if ((this.taskType == "LISpMinerTasks.SDKLTask") || (this.taskType == "LISpMinerTasks.KLTask"))
                        {
                            foreach (LiteralStruct literal in hypothesis.literals)
                            {
                                if (literal.cedentType == CedentEnum.Antecedent)
                                {
                                    if (literal.categoriesNames.Length > (i1-1))
                                    {
                                        antName = literal.categoriesNames[(i1-1)];
                                    }
                                    break;
                                }
                            }

                            foreach (LiteralStruct literal in hypothesis.literals)
                            {
                                if (literal.cedentType == CedentEnum.Succedent)
                                {
                                    if (literal.categoriesNames.Length > (j1-1))
                                    {
                                        sucName = literal.categoriesNames[(j1-1)];
                                    }
                                    break;
                                }
                            }
                        }
                        else if ((this.taskType == "LISpMinerTasks.SDCFTask") || (this.taskType == "LISpMinerTasks.CFTask"))
                        {
                            foreach (LiteralStruct literal in hypothesis.literals)
                            {
                                if (literal.cedentType == CedentEnum.Antecedent)
                                {
                                    if (literal.categoriesNames.Length > (j1-1))
                                    {
                                        antName = literal.categoriesNames[(j1-1)];
                                    }
                                    break;
                                }
                            }
                            sucName = String.Empty;
                        }
                        PropertySpec hValue = new PropertySpec(
                        antName.ToString() + "-" + sucName.ToString(),
                        typeof(int),
                        "1. " + resManager.GetString("ContingencyTable"),
                        "1. " + resManager.GetString("ContingencyTable"),
                        value
                        );
                        hValue.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
                        table.Properties.Add(hValue);
                        table[antName.ToString() + "-" + sucName.ToString()] = value;
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
                        string antName = i1.ToString();
                        string sucName = j1.ToString();
                        if ((this.taskType == "LISpMinerTasks.SDKLTask") || (this.taskType == "LISpMinerTasks.KLTask"))
                        {
                            foreach (LiteralStruct literal in hypothesis.literals)
                            {
                                if (literal.cedentType == CedentEnum.Antecedent)
                                {
                                    if (literal.categoriesNames.Length > (i1 - 1))
                                    {
                                        antName = literal.categoriesNames[(i1 - 1)];
                                    }
                                    break;
                                }
                            }

                            foreach (LiteralStruct literal in hypothesis.literals)
                            {
                                if (literal.cedentType == CedentEnum.Succedent)
                                {
                                    if (literal.categoriesNames.Length > (j1 - 1))
                                    {
                                        sucName = literal.categoriesNames[(j1 - 1)];
                                    }
                                    break;
                                }
                            }
                        }
                        else if ((this.taskType == "LISpMinerTasks.SDCFTask") || (this.taskType == "LISpMinerTasks.CFTask"))
                        {
                            foreach (LiteralStruct literal in hypothesis.literals)
                            {
                                if (literal.cedentType == CedentEnum.Antecedent)
                                {
                                    if (literal.categoriesNames.Length > (j1 - 1))
                                    {
                                        antName = literal.categoriesNames[(j1 - 1)];
                                    }
                                    break;
                                }
                            }
                            sucName = String.Empty;
                        }
                        PropertySpec hValue = new PropertySpec(
                        antName.ToString() + "-" + sucName.ToString(),
                        typeof(int),
                        "2. " + resManager.GetString("ContingencyTable"),
                        "2. " + resManager.GetString("ContingencyTable"),
                        value
                        );
                        hValue.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
                        table.Properties.Add(hValue);
                        table[antName.ToString() + "-" + sucName.ToString()] = value;
                        j1++;
                    }
                    j1 = 1;
                    i1++;
                }
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

        /// <summary>
        /// Method which handles re-filtering of hypotheses
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSubmitFilter_Click(object sender, EventArgs e)
        {
            //antecedent
            Dictionary<string, LiteralFilter> antFilter = new Dictionary<string, LiteralFilter>();
            foreach (object literalName in this.CheckedListBoxAntecedents.Items)
            {
                LiteralFilter filter = new LiteralFilter();
                filter.Gace = GaceTypeEnum.Both;
                filter.LowLength = 0;
                filter.HighLength = 99;
                if (CheckedListBoxAntecedents.GetItemChecked(CheckedListBoxAntecedents.Items.IndexOf(literalName)))
                {
                    filter.Selected = true;
                }
                else
                {
                    filter.Selected = false;
                }
                antFilter.Add(literalName.ToString(), filter);
            }
            //succedent
            Dictionary<string, LiteralFilter> sucFilter = new Dictionary<string, LiteralFilter>();
            foreach (object literalName in this.CheckedListBoxSuccedents.Items)
            {
                LiteralFilter filter = new LiteralFilter();
                filter.Gace = GaceTypeEnum.Both;
                filter.LowLength = 0;
                filter.HighLength = 99;
                if (CheckedListBoxSuccedents.GetItemChecked(CheckedListBoxSuccedents.Items.IndexOf(literalName)))
                {
                    filter.Selected = true;
                }
                else
                {
                    filter.Selected = false;
                }
                sucFilter.Add(literalName.ToString(), filter);
            }
            //condition
            Dictionary<string, LiteralFilter> condFilter = new Dictionary<string, LiteralFilter>();
            foreach (object literalName in this.CheckedListBoxConditions.Items)
            {
                LiteralFilter filter = new LiteralFilter();
                filter.Gace = GaceTypeEnum.Both;
                filter.LowLength = 0;
                filter.HighLength = 99;
                if (CheckedListBoxConditions.GetItemChecked(CheckedListBoxConditions.Items.IndexOf(literalName)))
                {
                    filter.Selected = true;
                }
                else
                {
                    filter.Selected = false;
                }
                condFilter.Add(literalName.ToString(), filter);
            }
            this.resultBrowser.AntecedentFilter = antFilter;
            this.resultBrowser.SuccedentFilter = sucFilter;
            this.resultBrowser.ConditionFilter = condFilter;
            this.ReReadItems();
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
            if ((this.taskType == "LISpMinerTasks.SDKLTask") || (this.taskType == "LISpMinerTasks.CFTask") || (this.taskType == "LISpMinerTasks.SDCFTask") || (this.taskType == "LISpMinerTasks.KLTask"))
            {
                this.ColumnAntecedent.Text = rm.GetString("RowAttribute");
            }
            else
            {
                this.ColumnAntecedent.Text = rm.GetString("ColumnAntecedent");
            }
            if ((this.taskType == "LISpMinerTasks.SDKLTask") || (this.taskType == "LISpMinerTasks.KLTask"))
            {
                this.ColumnSuccedent.Text = rm.GetString("ColumnAttribute");
            }
            else
            {
                if ((this.taskType == "LISpMinerTasks.CFTask") || (this.taskType == "LISpMinerTasks.SDCFTask"))
                {
                    this.ColumnSuccedent.Text = "";
                }
                else
                {
                    this.ColumnSuccedent.Text = rm.GetString("ColumnSuccedent");
                }
            }
            this.ColumnCondition.Text = rm.GetString("ColumnCondition");
            this.ColumnHypotheseName.Text = rm.GetString("ColumnHypothesisName");
            
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
            this.LabelAntecedentFilter.Text = rm.GetString("AntecedentFilter");
            this.LabelSuccedentFilter.Text = rm.GetString("SuccedentFilter");
            this.LabelConditionFilter.Text = rm.GetString("ConditionFilter");
            this.ButtonSubmitFilter.Text = rm.GetString("ButtonFilter");
            this.LabelHypothesesTotal.Text = rm.GetString("HypothesesCount");
            this.RadioFirstTable.Text = rm.GetString("RadioFirst");
            this.RadioSecondTable.Text = rm.GetString("RadioSecond");
        }

        #endregion


        #region Events

        public event LongRunTick IceTicked;
        public void OnIceTick()
        {
            if (IceTicked != null)
            {
                IceTicked();
            }
        }

        public event LongRunCompleted IceComplete;
        public void OnIceComplete()
        {
            if (IceComplete != null)
            {
                IceComplete();
            }
        }

        #endregion
    }
}