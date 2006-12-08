// FerdaResultBrowserControl.cs - UserControl class for displaying results
//
// Author:   Alexander Kuzmin <alexander.kuzmin@gmail.com>
//           Martin Ralbovsky <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2005 Alexander Kuzmin, Martin Ralbovsky
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
using System.Resources;
using System.Reflection;
using Ferda.ModulesManager;
using Ferda.FrontEnd.AddIns.Common.ListView;
using Ferda.Guha.Math.Quantifiers;
using Ferda.Guha.MiningProcessor;
using Ferda.Guha.MiningProcessor.Formulas;
using Ferda.Guha.MiningProcessor.Results;
using Ferda.Guha.MiningProcessor.QuantifierEvaluator;

namespace Ferda.FrontEnd.AddIns.ResultBrowser
{
    
    /// <summary>
    /// UserControl class for displaying results
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
        /// Implementation of propertygrid
        /// </summary>
        IOtherObjectDisplayer displayer;

        /// <summary>
        /// Sorter for the listview
        /// </summary>
        ListViewItemComparer columnSorter = new ListViewItemComparer();

        /// <summary>
        /// Counter for loading hypotheses
        /// </summary>
        private int loadingCounter = 0;

        /// <summary>
        /// Count of hypotheses
        /// </summary>
        private long hypothesesCount = 0;

        /// <summary>
        /// Owner of addin
        /// </summary>
        private IOwnerOfAddIn ownerOfAddIn;

        /// <summary>
        /// Columns
        /// </summary>
        private UsedMark[] columns;

        /// <summary>
        /// Quantifiers
        /// </summary>
        private UsedQuantifier[] quantifiers;

        /// <summary>
        /// Previously selected index
        /// </summary>
        int previousIndex = 1;

        /// <summary>
        /// Taskproxy for getting names of the attribues
        /// </summary>
        BitStringGeneratorProviderPrx taskProxy;

        /// <summary>
        /// Determines the space between two filters when resizing in pixels.
        /// </summary>
        private int resizeFilterOffset = 5;

        /// <summary>
        /// Determines number of filter controls in the upper part
        /// of the component
        /// </summary>
        private int numberOfFilters = 5;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="localePrefs">Localeprefs</param>
        /// <param name="result">Result to display</param>
        /// <param name="quantifiers">quantifiers</param>
        /// <param name="Displayer">Propertygrid</param>
        /// <param name="ownerOfAddIn">Ownerofaddin</param>
        public FerdaResultBrowserControl(ResourceManager resManager, string result, 
            Quantifiers quantifiers, BitStringGeneratorProviderPrx taskProxy, 
            IOtherObjectDisplayer Displayer, IOwnerOfAddIn ownerOfAddIn)
        {
            this.resManager = resManager;

            this.taskProxy = taskProxy;
            this.ownerOfAddIn = ownerOfAddIn;
            columnSorter.column = 0;
            InitializeComponent();
            InitializeGraph();

            if (String.IsNullOrEmpty(result))
            {
                throw new Ferda.Modules.BadValueError();
            }
            resultBrowser = new FerdaResult(resManager, result, quantifiers);
            resultBrowser.IceTicked += new LongRunTick(resultBrowser_IceTicked);
            resultBrowser.IceComplete += new LongRunCompleted(resultBrowser_IceComplete);
            resultBrowser.Initialize();
            this.hypothesesCount = resultBrowser.AllHypothesesCount;

            this.Resize += new EventHandler(FerdaResultBrowserControl_Resize);
            this.ChangeLocale();
            this.displayer = Displayer;
            this.displayer.Reset();
        }

        #endregion

        #region Initialization methods

        /// <summary>
        /// Inits column display checkbox and internal columns list
        /// </summary>
        /// <param name="marks"></param>
        private void ColumnsInit(MarkEnum[] marks)
        {
            int i = 1;
            columns = new UsedMark[marks.Length + 1];
            UsedMark usedColumn1 = new UsedMark();
            usedColumn1.id = 0;
            usedColumn1.ColumnType = MarkEnum.Antecedent;
            usedColumn1.ColumnName = this.resManager.GetString("HypothesisId");
            usedColumn1.Selected = true;
            usedColumn1.width = 200;
            this.columns[0] = usedColumn1;
            this.CHLMarks.Items.Add(this.resManager.GetString("HypothesisId"), true);
            foreach (MarkEnum mark in marks)
            {
                this.CHLMarks.Items.Add(mark.ToString(), false);
                UsedMark usedColumn = new UsedMark();
                usedColumn.id = i;
                usedColumn.ColumnType = mark;
                usedColumn.ColumnName = mark.ToString();
                usedColumn.Selected = false;
                usedColumn.width = 200;
                this.columns[i] = usedColumn;
                i++;
            }
        }

        /// <summary>
        /// Inits quantifiers display checkbox and internal quantifiers list
        /// </summary>
        /// <param name="labels"></param>
        /// <param name="userLabels"></param>
        private void QuantifiersInit(string[] labels, string [] userLabels)
        {
            quantifiers = new UsedQuantifier[labels.Length];
            for (int i = 0; i < labels.Length; i++)
            {
                this.CHLQuantifiers.Items.Add(labels[i] + "(" + userLabels[i] + ")", false);
                UsedQuantifier usedQuantifier = new UsedQuantifier();
                usedQuantifier.id = i;
                usedQuantifier.QuantifierLabel = labels[i];
                usedQuantifier.QuantifierUserLabel = userLabels[i];
                usedQuantifier.Selected = false;
                usedQuantifier.width = 200;
                quantifiers[i] = usedQuantifier;
            }
        }

        /// <summary>
        /// Inits the control when the loading loading of the hypotheses is
        /// finished
        /// </summary>
        private void AllInit()
        {
            ColumnsInit(resultBrowser.SemanticMarks);
            QuantifiersInit(resultBrowser.QuantifiersLabels, resultBrowser.QuantifiersUserLabels);
            //TODO: Filters init
            //TODO: Hypotheses count init
            
            HypothesesListView.ColumnClick += new ColumnClickEventHandler(ClickOnColumn);
            
            //adding handler to display hypothesis details in PG
            HypothesesListView.ItemActivate += new EventHandler(ItemSelectHandler);
            HypothesesListView.ItemCheck += new ItemCheckEventHandler(ItemSelectHandler);
            HypothesesListView.MouseClick += new MouseEventHandler(ItemSelectHandler);
            HypothesesListView.KeyDown += new KeyEventHandler(ItemSelectHandler);
            HypothesesListView.KeyUp += new KeyEventHandler(ItemSelectHandler);

            HypothesesListView.ColumnWidthChanged += new ColumnWidthChangedEventHandler(HypothesesListView_ColumnWidthChanged);

            for (int i = 0; i < columns.Length; i++)
            {
                if (columns[i].Selected)
                {
                    ColumnHeader markHeader = new ColumnHeader();
                    markHeader.Text = columns[i].ColumnName;
                    markHeader.Width = columns[i].width;
                    markHeader.Tag = "c" + i.ToString();
                    this.HypothesesListView.Columns.Add(markHeader);
                }
            }

            for (int i = 0; i < quantifiers.Length; i++)
            {
                if (quantifiers[i].Selected)
                {
                    ColumnHeader quantifierHeader = new ColumnHeader();
                    quantifierHeader.Text = quantifiers[i].QuantifierLabel +
                        "(" + quantifiers[i].QuantifierUserLabel + ")";
                    quantifierHeader.Width = quantifiers[i].width;
                    quantifierHeader.Tag = "q" + i.ToString();
                    this.HypothesesListView.Columns.Add(quantifierHeader);
                }
            }
            int j = AddAllHypothesesToListView();
            //displaying the first hypothese (if there is any)
            if (j > 0)
            {
                Hypothesis hypothesis = this.resultBrowser.GetHypothese(0);
                DrawBarsFromFirstTable(hypothesis);
            }

            this.LabelCount.Text = "(" + j + "/" + hypothesesCount + ")";
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Adds a single hypothesis to listview
        /// </summary>
        /// <param name="hypothesisId">Id of hypothesis to add</param>
        private void AddHypothesisToListView(int hypothesisId)
        {
            int j = 0;
            bool itemSet = false;
            ListViewItem item = new ListViewItem();
            for (int i = 0; i < this.columns.Length; i++ )
            {
                if (this.columns[i].Selected)
                {
                    if (this.columns[i].id == 0)
                    {
                        item.Text = hypothesisId.ToString();
                    }
                    else
                    {
                        item.Text = resultBrowser.GetFormulaString(this.columns[i].ColumnType, hypothesisId);
                    }
                    item.Tag = hypothesisId;
                    j = i;
                    itemSet = true;
                    break;
                }
            }

            //TODO: Check filters

            for (int i = j + 1; i < this.columns.Length; i++)
            {
                if (this.columns[i].Selected)
                {
                    item.SubItems.Add(resultBrowser.GetFormulaString(this.columns[i].ColumnType, hypothesisId));
                }
            }
            if (!itemSet)
            {
                return;
            }
            double [] quantifiersValues;
            quantifiersValues = resultBrowser.ReadQuantifiersFromCache(hypothesisId, Convert.ToInt32(this.NumericUpDownDecimals.Value));
            
            for (int i = 0; i < quantifiersValues.Length; i++)
            {
                if (quantifiers[i].Selected)
                {
                    item.SubItems.Add(quantifiersValues[i].ToString());
                }
            }
            HypothesesListView.Items.Add(item);
        }

        /// <summary>
        /// Adds all hypotheses to listview
        /// </summary>
        private int AddAllHypothesesToListView()
        {
            int j = 0;
            for (int i = 0; i < this.hypothesesCount; i++)
            {
                AddHypothesisToListView(i);
                j++;
            }
            return j;
        }

        /// <summary>
        /// Map the column checkbox to internal column structure
        /// </summary>
        private void ChangeColumnCheck()
        {
            for (int i = 0; i < CHLMarks.Items.Count; i++)
            {
                if (CHLMarks.CheckedItems.IndexOf(CHLMarks.Items[i]) != -1)
                {
                    columns[i].Selected = true;
                }
                else
                {
                    columns[i].Selected = false;
                }
            }
        }

        /// <summary>
        /// Map the quantifier checkbox to internal quantifier structure
        /// </summary>
        private void ChangeQuantifierCheck()
        {
            for (int i = 0; i < CHLQuantifiers.Items.Count; i++)
            {
                if (CHLQuantifiers.CheckedItems.IndexOf(CHLQuantifiers.Items[i]) != -1)
                {
                    quantifiers[i].Selected = true;
                }
                else
                {
                    quantifiers[i].Selected = false;
                }
            }
        }

        /// <summary>
        /// Method which temporarily disables all of the user controls until initialization is done
        /// </summary>
        private void PreloadDisable()
        {
            // this.HypothesesListView.Visible = false;
            this.CHLMarks.Enabled = false;
            this.CHLQuantifiers.Enabled = false;
            this.CHLBoxAntecedents.Enabled = false;
            this.CHLBoxConditions.Enabled = false;
            this.CHLBoxSuccedents.Enabled = false;
            this.ButtonSubmitFilter.Enabled = false;
            this.NumericUpDownDecimals.Enabled = false;
        }

        /// <summary>
        /// Method which enables the disabled controls
        /// </summary>
        private void AfterLoadEnable()
        {
            this.HypothesesListView.Enabled = true;
            this.HypothesesListView.Visible = true;
            this.CHLMarks.Enabled = true;
            this.CHLQuantifiers.Enabled = true;
            this.CHLBoxAntecedents.Enabled = true;
            this.CHLBoxConditions.Enabled = true;
            this.CHLBoxSuccedents.Enabled = true;
            this.ButtonSubmitFilter.Enabled = true;
            this.NumericUpDownDecimals.Enabled = true;
        }

        #region Commented
        /*
         
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
        */
        #endregion
        #endregion

        #region Handlers

        /// <summary>
        /// Method to complete initialization after ice communication has been completed
        /// </summary>
        void resultBrowser_IceComplete()
        {
            this.AllInit();
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
        /// Method to check the chart displaying
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioFirstTable_CheckedChanged(object sender, EventArgs e)
        {
            /*
            int index = 0;
            HypothesisStruct hypothesis;
            try
            {
                index = (int)HypothesesListView.SelectedItems[0].Tag;
                hypothesis = this.resultBrowser.GetHypothese(index);
            }

            catch
            {
                return;
            }
            if (this.RadioFirstTable.Checked)
            {
                this.DrawBarsFromFirstTable(hypothesis, this.ContingencyTableChart);
            }
            else
            {
                this.DrawBarsFromSecondTable(hypothesis, this.ContingencyTableChart);
            }
             * */
        }

        /// <summary>
        /// Method to check the chart displaying
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioSecondTable_CheckedChanged(object sender, EventArgs e)
        {
            /*
            int index = 0;
            HypothesisStruct hypothesis;
            try
            {
                
                index = (int)HypothesesListView.SelectedItems[0].Tag;
                hypothesis = this.resultBrowser.GetHypothese(index);
            }

            catch
            {
                return;
            }
            if (this.RadioFirstTable.Checked)
            {
                this.DrawBarsFromFirstTable(hypothesis, this.ContingencyTableChart);
            }
            else
            {
                this.DrawBarsFromSecondTable(hypothesis, this.ContingencyTableChart);
            }
             * */
        }

        /// <summary>
        /// Method for filling the chart and propertygrid with the hypothese data.
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        private void ItemSelectHandler(object sender, EventArgs e)
        {
            ListView view = (ListView)sender;
            int index = 0;

            //getting the index of the new selected row
            index = (int)view.SelectedItems[0].Tag;
            if (index == previousIndex)
            {
                return;
            }
            else
            {
                // hypothesis = this.resultBrowser.GetHypothese(index);
                previousIndex = index;
            }

            //getting the actual hypothesis from the index of the hypothesis
            Hypothesis hypothesis = this.resultBrowser.GetHypothese(index);
            //filling the property grid
            this.FillPropertyGrid(hypothesis, index);

            if (this.RadioFirstTable.Checked)
            {
                this.DrawBarsFromFirstTable(hypothesis);
            }
            else
            {
                //this.DrawBarsFromSecondTable(hypothesis);
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
            if (HypothesesListView.Sorting == SortOrder.Ascending)
            {
                columnSorter.bAscending = false;
                HypothesesListView.Sorting = SortOrder.Descending;
            }
            else
            {
                columnSorter.bAscending = true;
                HypothesesListView.Sorting = SortOrder.Ascending;
            }
            HypothesesListView.ListViewItemSorter = columnSorter;
        }

        /// <summary>
        /// Fills propertgrid with the data of the selected hypothesis
        /// </summary>
        /// <param name="hypothesis">Hypothesis to take the data from</param>
        /// <param name="hypothesisId">Index of the hypothese</param>
        private void FillPropertyGrid(Hypothesis hypothesis, int hypothesisId)
        {
            PropertyTable table = new PropertyTable();
            string antecedentText = string.Empty;
            string succedentText = string.Empty;

            #region Filling in marks according to columns selected

            foreach (UsedMark column in this.columns)
            {
                if (column.Selected)
                {
                    //if it's id column, don't run getformula
                    if (column.id == 0)
                    {
                        PropertySpec tName = new PropertySpec(
                            column.ColumnName,
                            typeof(string),
                            column.ColumnName,
                            resManager.GetString("IDDescription"),
                            String.Empty);
                        tName.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
                        table.Properties.Add(tName);
                        table[column.ColumnName] = hypothesisId.ToString();
                    }
                    else
                    {
                        PropertySpec tName = new PropertySpec(
                            column.ColumnName,
                            typeof(string),
                            column.ColumnName,
                            resManager.GetString("IDDescription"),
                            String.Empty);
                        tName.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
                        table.Properties.Add(tName);
                        table[column.ColumnName] = resultBrowser.GetFormulaString(column.ColumnType, hypothesisId);
                    }
                }
            }

            #endregion

            #region Used quantifiers and their values

            //used quantifiers and their values
            double[] quantifiers;

            quantifiers = resultBrowser.ReadQuantifiersFromCache(hypothesisId,
                    Convert.ToInt32(this.NumericUpDownDecimals.Value));

            for (int i = 0; i < quantifiers.Length; i++)
            {
                PropertySpec hQuantifier = new PropertySpec(
                resultBrowser.QuantifiersLabels[i]
                    + "(" + resultBrowser.QuantifiersUserLabels[i] + ")",
                typeof(double),
                resManager.GetString("AppliedQuantifiers"),
                resManager.GetString("AppliedQuantifiers"),
                quantifiers[i]
                );
                hQuantifier.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
                table.Properties.Add(hQuantifier);
                table[resultBrowser.QuantifiersLabels[i]
                    + "(" + resultBrowser.QuantifiersUserLabels[i]+")"] = quantifiers[i];
            }

            #endregion

            #region Contingency tables

            //for miners with boolean antecedents and succedents - for now only 4FT
            Formula form = hypothesis.GetFormula(MarkEnum.Antecedent);
            if (form != null)
            {
                //antecedent AND succedent
                PropertySpec value = new PropertySpec("AntSucc", typeof(double),
                    resManager.GetString("ContingencyTable"),
                    resManager.GetString("AntSuccDescription"),
                    0);
                value.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
                table.Properties.Add(value);
                table["AntSucc"] = hypothesis.ContingencyTableA[0][0];

                //antecedent AND NOT succedent
                value = new PropertySpec("Ant" + '\u00AC' + "Succ", typeof(double),
                    resManager.GetString("ContingencyTable"),
                    resManager.GetString("AntNOTSuccDescription"),
                    0);
                value.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
                table.Properties.Add(value);
                table["Ant" + '\u00AC' + "Succ"] = hypothesis.ContingencyTableA[0][2];

                //NOT antecedent AND succedent
                value = new PropertySpec('\u00AC' + "AntSucc", typeof(double),
                    resManager.GetString("ContingencyTable"),
                    resManager.GetString("NOTAntSuccDescription"),
                    0);
                value.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
                table.Properties.Add(value);
                table['\u00AC' + "AntSucc"] = hypothesis.ContingencyTableA[2][0];

                //NOT antecedent AND NOT succedent
                value = new PropertySpec('\u00AC' + "Ant" + '\u00AC' + "Succ", typeof(double),
                    resManager.GetString("ContingencyTable"),
                    resManager.GetString("NOTAntNOTSuccDescription"),
                    0);
                value.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
                table.Properties.Add(value);
                table['\u00AC' + "Ant" + '\u00AC' + "Succ"] = 
                    hypothesis.ContingencyTableA[2][2];
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
            // this.RefreshBrowser();
        }

        /// <summary>
        /// Method which handles re-filtering of hypotheses
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSubmitFilter_Click(object sender, EventArgs e)
        {
            /*
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
            */
        }

        /// <summary>
        /// Opens a PDF file with the help about the ResultBrowser module
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        private void ButtonHelp_Click(object sender, EventArgs e)
        {
            ownerOfAddIn.OpenPdf(ownerOfAddIn.GetBinPath() + "\\AddIns\\Help\\ResultBrowser.pdf");
        }

        /// <summary>
        /// Re-displaying listview with new settings for columns display
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSubmitColumnChange_Click(object sender, EventArgs e)
        {
            ChangeColumnCheck();
            ChangeQuantifierCheck();
            this.HypothesesListView.Items.Clear();
            this.columnSorter.column = 0;
            HypothesesListView.ColumnClick -= new ColumnClickEventHandler(ClickOnColumn);
            HypothesesListView.ListViewItemSorter = null;
            this.HypothesesListView.Columns.Clear();

            for (int i = 0; i < columns.Length; i++)
            {
                if (columns[i].Selected)
                {
                    ColumnHeader markHeader = new ColumnHeader();
                    markHeader.Text = columns[i].ColumnName;
                    markHeader.Width = columns[i].width;
                    markHeader.Tag = "c" + i.ToString();
                    this.HypothesesListView.Columns.Add(markHeader);
                }
            }

            for (int i = 0; i < quantifiers.Length; i++)
            {
                if (quantifiers[i].Selected)
                {
                    ColumnHeader quantifierHeader = new ColumnHeader();
                    quantifierHeader.Text = quantifiers[i].QuantifierLabel +
                        "(" + quantifiers[i].QuantifierUserLabel + ")";
                    quantifierHeader.Width = quantifiers[i].width;
                    quantifierHeader.Tag = "q" + i.ToString();
                    this.HypothesesListView.Columns.Add(quantifierHeader);
                }
            }
            AddAllHypothesesToListView();
            HypothesesListView.ColumnClick += new ColumnClickEventHandler(ClickOnColumn);
            HypothesesListView.ListViewItemSorter = columnSorter;
        }

        void HypothesesListView_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            string key = (string)HypothesesListView.Columns[e.ColumnIndex].Tag;
            int index = Convert.ToInt32(key.Substring(1));
            
            //it is a column
            if (key.Substring(0, 1).CompareTo("c") == 0)
            {
                this.columns[index].width = HypothesesListView.Columns[e.ColumnIndex].Width;
            }

            //it is a quantifier column
            if (key.Substring(0, 1).CompareTo("q") == 0)
            {
                this.quantifiers[index].width = HypothesesListView.Columns[e.ColumnIndex].Width;
            } 
        }

        /// <summary>
        /// Forces the whole control to resize
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        void FerdaResultBrowserControl_Resize(object sender, EventArgs e)
        {
            Control control = (Control)sender;
            int newControlWidth = control.Width;
            //magic constant :) This is where the area to place all the
            //filters starts. It should not be moved.
            int zero = 225;

            //computing the size of individual filters after resize
            int newSize = (newControlWidth - zero - numberOfFilters * 
                resizeFilterOffset) / numberOfFilters;

            //resizing the individual filters
            CHLMarks.Left = zero;
            CHLMarks.Width = newSize;
            LQuantifiersToDisplay.Left = CHLMarks.Right + resizeFilterOffset;
            CHLQuantifiers.Left = CHLMarks.Right + resizeFilterOffset;
            CHLQuantifiers.Width = newSize;
            CHLBoxAntecedents.Left = CHLQuantifiers.Right + resizeFilterOffset;
            LAntecedentFilter.Left = CHLQuantifiers.Right + resizeFilterOffset;
            CHLBoxAntecedents.Width = newSize;
            LSuccedentFilter.Left = CHLBoxAntecedents.Right + resizeFilterOffset;
            CHLBoxSuccedents.Left = CHLBoxAntecedents.Right + resizeFilterOffset;
            CHLBoxSuccedents.Width = newSize;
            LConditionFilter.Left = CHLBoxSuccedents.Right + resizeFilterOffset;
            CHLBoxConditions.Left = CHLBoxSuccedents.Right + resizeFilterOffset;
            CHLBoxConditions.Width = newSize;
        }

        #endregion

        #region Localization

        /// <summary>
        /// Method to change l10n.
        /// </summary>
        /// <param name="rm">Resource manager to handle new l10n resource</param>
        private void ChangeLocale()
        {
            GroupBoxChangeGraph.Text = resManager.GetString("GraphViewOptions");
            ButtonSubmitColumnChange.Text = resManager.GetString("SubmitColumnChange");
            ToolStripShowGraphEdit.Text = resManager.GetString("GraphViewOptions");
            this.Label3dpercent.Text = resManager.GetString("3dpercent");
            //     this.ContingencyTableChart.Header.Lines = new string[] { rm.GetString("ContingencyTable") };
            LabelHOffset.Text = resManager.GetString("LabelHOffset");
            LabelVOffset.Text = resManager.GetString("LabelVOffset");
            LabelZoom.Text = resManager.GetString("LabelZoom");
            ToolStripCopyChart.Text = resManager.GetString("CopyChart");
            CheckBoxShowLabels.Text = resManager.GetString("ShowLabels");

            LabelNumeric.Text = resManager.GetString("LabelNumeric");
            LabelProgressBar.Text = resManager.GetString("HypothesesLoading");
            LAntecedentFilter.Text = resManager.GetString("AntecedentFilter");
            LSuccedentFilter.Text = resManager.GetString("SuccedentFilter");
            LConditionFilter.Text = resManager.GetString("ConditionFilter");
            LColumnsToDisplay.Text = resManager.GetString("LabelColumnsToDisplay");
            LQuantifiersToDisplay.Text = resManager.GetString("LabelQuantifiersToDisplay");
            ButtonSubmitFilter.Text = resManager.GetString("ButtonFilter");
            LabelHypothesesTotal.Text = resManager.GetString("HypothesesCount");
            RadioFirstTable.Text = resManager.GetString("RadioFirst");
            RadioSecondTable.Text = resManager.GetString("RadioSecond");
            ButtonHelp.Text = resManager.GetString("Help");
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

    public class UsedColumn
    {
        public int id;
        public bool Selected;
        public int width;
    }
    public class UsedMark : UsedColumn
    {
        public MarkEnum ColumnType;
        public string ColumnName;
    }
    public class UsedQuantifier : UsedColumn
    {
        public string QuantifierLabel;
        public string QuantifierUserLabel;
    }
}