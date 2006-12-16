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
        /// Columns to be displayed in the CHLMarks list
        /// (for 4FT the marks are Antecedent, Succedent and Condition).
        /// </summary>
        private UsedMark[] marks;

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
        /// of the component for the FFT tasks
        /// </summary>
        private int numberOfFFTFilters = 5;

        /// <summary>
        /// Determines number of filter controls in the upper part
        /// of the component for the non FFT tasks
        /// </summary>
        private int numberOfOtherFilters = 3;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="localePrefs">Localeprefs</param>
        /// <param name="result">Identifier of the result to display</param>
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
            resultBrowser = new FerdaResult(result, quantifiers);
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
        /// The method fills the ColumnList (CHLMarks) with the names of
        /// the marks that are present in the task type
        /// (for 4FT the marks are Antecedent, Succedent and Condition).
        /// The method also fills the internal <code>columns</code> structure 
        /// that holds these marks.
        /// </summary>
        private void ColumnsInit()
        {
            //The marks to be displayed (for 4FT the marks are Antecedent, 
            //Succedent and Condition)
            MarkEnum[] enums = resultBrowser.SemanticMarks;

            //The ID mark
            int i = 1;
            marks = new UsedMark[enums.Length + 1];
            UsedMark usedColumn1 = new UsedMark();
            usedColumn1.id = 0;
            usedColumn1.MarkType = MarkEnum.Antecedent;
            usedColumn1.ColumnName = this.resManager.GetString("HypothesisId");
            usedColumn1.Selected = true;
            usedColumn1.width = 200;
            this.marks[0] = usedColumn1;
            this.CHLMarks.Items.Add(this.resManager.GetString("HypothesisId"), true);

            //The rest of the marks (for 4FT the marks are Antecedent, Succedent and
            //Condition)
            foreach (MarkEnum mark in enums)
            {
                this.CHLMarks.Items.Add(mark.ToString(), false);
                UsedMark usedColumn = new UsedMark();
                usedColumn.id = i;
                usedColumn.MarkType = mark;
                usedColumn.ColumnName = mark.ToString();
                usedColumn.Selected = false;
                usedColumn.width = 200;
                this.marks[i] = usedColumn;
                i++;
            }
        }

        /// <summary>
        /// The method fills the quantifiers list (CHLQuantifiers) with
        /// the names of the quantifiers that are connected to the task.
        /// It also fills the internal <code>quantifiers</code> structure
        /// that holds the information
        /// </summary>
        private void QuantifiersInit()
        {
            string[] labels = resultBrowser.QuantifiersLabels;
            string[] userLabels = resultBrowser.QuantifiersUserLabels;

            quantifiers = new UsedQuantifier[labels.Length];
            for (int i = 0; i < labels.Length; i++)
            {
                this.CHLQuantifiers.Items.Add(userLabels[i], false);
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
        /// For the FFT and SD4FT tasks, the method displays a list of
        /// atoms, that are contained in the antecedent. Otherwise, it
        /// hides the <code>LAntecedentFilter</code> and the
        /// <code>CHLBoxAntecedents</code> components.
        /// </summary>
        private void AntecedentInit()
        {
            if (resultBrowser.TaskType == TaskTypeEnum.FourFold ||
                resultBrowser.TaskType == TaskTypeEnum.SDFourFold)
            {
                //showing the controls
                LAntecedentFilter.Visible = true;
                CHLBoxAntecedents.Visible = true;
            }
            else
            {
                //hiding the controls
                LAntecedentFilter.Visible = false;
                CHLBoxAntecedents.Visible = false;
            }
        }

        /// <summary>
        /// For the FFT and SD4FT tasks, the method displays a list of
        /// atoms, that are contained in the succedent. Otherwise, it
        /// hides the <code>LSuccedentFilter</code> and the
        /// <code>CHLBoxSuccedents</code> components.
        /// </summary>
        private void SuccedentInit()
        {
            if (resultBrowser.TaskType == TaskTypeEnum.FourFold ||
                resultBrowser.TaskType == TaskTypeEnum.SDFourFold)
            {
                //showing the controls
                LSuccedentFilter.Visible = true;
                CHLBoxSuccedents.Visible = true;
            }
            else
            {
                //hiding the controls
                LSuccedentFilter.Visible = false;
                CHLBoxSuccedents.Visible = false;
            }
        }

        /// <summary>
        /// Inits the control when the loading loading of the hypotheses is
        /// finished
        /// </summary>
        private void AllInit()
        {
            ColumnsInit();
            QuantifiersInit();
            AntecedentInit();
            SuccedentInit();
            //TODO: Filters init
            
            HypothesesListView.ColumnClick += new ColumnClickEventHandler(ClickOnColumn);
            
            //adding handler to display hypothesis details in PG
            HypothesesListView.ItemActivate += new EventHandler(ItemSelectHandler);
            HypothesesListView.ItemCheck += new ItemCheckEventHandler(ItemSelectHandler);
            HypothesesListView.MouseClick += new MouseEventHandler(ItemSelectHandler);
            HypothesesListView.KeyDown += new KeyEventHandler(ItemSelectHandler);
            HypothesesListView.KeyUp += new KeyEventHandler(ItemSelectHandler);

            HypothesesListView.ColumnWidthChanged += new ColumnWidthChangedEventHandler(HypothesesListView_ColumnWidthChanged);

            for (int i = 0; i < marks.Length; i++)
            {
                if (marks[i].Selected)
                {
                    ColumnHeader markHeader = new ColumnHeader();
                    markHeader.Text = marks[i].ColumnName;
                    markHeader.Width = marks[i].width;
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
            for (int i = 0; i < this.marks.Length; i++ )
            {
                if (this.marks[i].Selected)
                {
                    if (this.marks[i].id == 0)
                    {
                        item.Text = hypothesisId.ToString();
                    }
                    else
                    {
                        item.Text = resultBrowser.GetFormulaString(
                            this.marks[i].MarkType, hypothesisId);
                    }
                    item.Tag = hypothesisId;
                    j = i;
                    itemSet = true;
                    break;
                }
            }

            for (int i = j + 1; i < this.marks.Length; i++)
            {
                if (this.marks[i].Selected)
                {
                    item.SubItems.Add(resultBrowser.GetFormulaString(
                        this.marks[i].MarkType, hypothesisId));
                }
            }
            if (!itemSet)
            {
                return;
            }

            //the quantifiers
            double [] quantifiersValues;
            quantifiersValues = resultBrowser.ReadQuantifiersFromCache(
                hypothesisId, Convert.ToInt32(this.NumericUpDownDecimals.Value));
            
            for (int i = 0; i < quantifiers.Length; i++)
            {
                if (quantifiers[i].Selected)
                {
                    //we use the positive infinity when the quantifier does
                    //not provide numerical values
                    if (Double.IsNaN(quantifiersValues[i]))
                    {
                        item.SubItems.Add(resManager.GetString("NotProvidingValues"));
                    }
                    else
                    {
                        item.SubItems.Add(quantifiersValues[i].ToString());
                    }
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
                    marks[i].Selected = true;
                }
                else
                {
                    marks[i].Selected = false;
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

            foreach (UsedMark column in this.marks)
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
                        table[column.ColumnName] = resultBrowser.GetFormulaString(column.MarkType, hypothesisId);
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
                    + "(" + resultBrowser.QuantifiersUserLabels[i] + ")"] = quantifiers[i];
            }

            #endregion

            #region Contingency tables

            //for miners with boolean antecedents and succedents - for now only 4FT
            if (resultBrowser.TaskType == TaskTypeEnum.FourFold)
            {
                //antecedent AND succedent
                PropertySpec value = new PropertySpec("a", typeof(double),
                    resManager.GetString("ContingencyTable"),
                    resManager.GetString("AntSuccDescription"),
                    0);
                value.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
                table.Properties.Add(value);
                table["a"] = hypothesis.ContingencyTableA[0][0];

                //antecedent AND NOT succedent
                value = new PropertySpec("b", typeof(double),
                    resManager.GetString("ContingencyTable"),
                    resManager.GetString("AntNOTSuccDescription"),
                    0);
                value.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
                table.Properties.Add(value);
                table["b"] = hypothesis.ContingencyTableA[0][2];

                //NOT antecedent AND succedent
                value = new PropertySpec("c", typeof(double),
                    resManager.GetString("ContingencyTable"),
                    resManager.GetString("NOTAntSuccDescription"),
                    0);
                value.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
                table.Properties.Add(value);
                table["c"] = hypothesis.ContingencyTableA[2][0];

                //NOT antecedent AND NOT succedent
                value = new PropertySpec("d", typeof(double),
                    resManager.GetString("ContingencyTable"),
                    resManager.GetString("NOTAntNOTSuccDescription"),
                    0);
                value.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
                table.Properties.Add(value);
                table["d"] = hypothesis.ContingencyTableA[2][2];
            }

            if (resultBrowser.TaskType == TaskTypeEnum.SDFourFold)
            {
                //antecedent AND succedent
                PropertySpec value = new PropertySpec("a", typeof(double),
                    resManager.GetString("ContingencyTable"),
                    resManager.GetString("AntSuccDescription"),
                    0);
                value.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
                table.Properties.Add(value);
                table["a"] = hypothesis.ContingencyTableA[0][0];

                //antecedent AND NOT succedent
                value = new PropertySpec("b", typeof(double),
                    resManager.GetString("ContingencyTable"),
                    resManager.GetString("AntNOTSuccDescription"),
                    0);
                value.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
                table.Properties.Add(value);
                table["b"] = hypothesis.ContingencyTableA[0][2];

                //NOT antecedent AND succedent
                value = new PropertySpec("c", typeof(double),
                    resManager.GetString("ContingencyTable"),
                    resManager.GetString("NOTAntSuccDescription"),
                    0);
                value.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
                table.Properties.Add(value);
                table["c"] = hypothesis.ContingencyTableA[2][0];

                //NOT antecedent AND NOT succedent
                value = new PropertySpec("d", typeof(double),
                    resManager.GetString("ContingencyTable"),
                    resManager.GetString("NOTAntNOTSuccDescription"),
                    0);
                value.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
                table.Properties.Add(value);
                table["d"] = hypothesis.ContingencyTableA[2][2];
            }

            if (resultBrowser.TaskType == TaskTypeEnum.KL)
            {
                double[][] transposed = Transpose(hypothesis.ContingencyTableA);

                for (int i = 0; i <= transposed.GetUpperBound(0); i++)
                {
                    for (int j = 0; j < transposed[i].Length; j++)
                    {
                        PropertySpec value = new PropertySpec(
                            i.ToString() + '-' + j.ToString(), 
                            typeof(double),
                            resManager.GetString("ContingencyTable"),
                            resManager.GetString("KLTableDescription") + ' ' + i.ToString() + ", " + j.ToString(),
                            0);
                        value.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
                        table.Properties.Add(value);
                        table[i.ToString() + '-' + j.ToString()] = transposed[i][j];
                    }
                }
            }

            if (resultBrowser.TaskType == TaskTypeEnum.CF)
            {
                for (int i = 0; i < hypothesis.ContingencyTableA[0].Length; i++)
                {
                    PropertySpec value = new PropertySpec(
                        i.ToString(),
                        typeof(double),
                        resManager.GetString("ContingencyTable"),
                        resManager.GetString("KLTableDescription") + ' ' + i.ToString(),
                        0);
                    value.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
                    table.Properties.Add(value);
                    table[i.ToString()] = hypothesis.ContingencyTableA[0][i];
                }
            }

            #endregion

            this.displayer.Reset();
            this.displayer.OtherObjectAdapt(table);
        }

        /// <summary>
        /// Fills the property table <paramref name="table"/> with the
        /// one of the contingency tables of the SD4FT task. Which contingency
        /// table is determined by the <paramref name="firstTable"/>
        /// </summary>
        /// <param name="hypothesis">Hypothesis containing the contingency
        /// tables</param>
        /// <param name="firstTable">Which table should be filled</param>
        private void FillPropertySDFFTContingency(Hypothesis hypothesis, 
            PropertyTable table, bool firstTable)
        {
            string category = firstTable ?
                resManager.GetString("FirstContingencyTable") :
                resManager.GetString("SecondContingencyTable");

            //antecedent AND succedent
            PropertySpec value = new PropertySpec("a", typeof(double),
                category,
                resManager.GetString("AntSuccDescription"),
                0);
            value.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            table.Properties.Add(value);
            table["a"] = firstTable ?
                hypothesis.ContingencyTableA[0][0] :
                hypothesis.ContingencyTableB[0][0];

            //antecedent AND NOT succedent
            value = new PropertySpec("b", typeof(double),
                category,
                resManager.GetString("AntNOTSuccDescription"),
                0);
            value.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            table.Properties.Add(value);
            table["b"] = firstTable ?
                hypothesis.ContingencyTableA[0][1] :
                hypothesis.ContingencyTableB[0][1];

            //NOT antecedent AND succedent
            value = new PropertySpec("c", typeof(double),
                category,
                resManager.GetString("NOTAntSuccDescription"),
                0);
            value.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            table.Properties.Add(value);
            table["c"] = firstTable ?
                hypothesis.ContingencyTableA[1][0] :
                hypothesis.ContingencyTableB[1][0];

            //NOT antecedent AND NOT succedent
            value = new PropertySpec("d", typeof(double),
                category,
                resManager.GetString("NOTAntNOTSuccDescription"),
                0);
            value.Attributes = new Attribute[] { ReadOnlyAttribute.Yes };
            table.Properties.Add(value);
            table["d"] = firstTable ?
                hypothesis.ContingencyTableA[1][1] :
                hypothesis.ContingencyTableB[1][1];
        }

        /// <summary>
        /// Displays the graph of the fiest hypothesis
        /// </summary>
        private void LoadFirstHypothesis()
        {
            if (resultBrowser.AllHypothesesCount > 0)
            {
                DrawBars(resultBrowser.AllHypotheses[0]);
            }
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
            this.LabelProgressBar.Visible = false;
            this.ProgressBarIceTicks.Visible = false;
            this.StatusStrip.Visible = false;
            this.AfterLoadEnable();
            this.AllInit();
            this.LoadFirstHypothesis();
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
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
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
                hypothesis = this.resultBrowser.GetHypothesis(index);
            }

            catch
            {
                return;
            }
            if (this.RadioFirstTable.Checked)
            {
                this.DrawBars(hypothesis, this.ContingencyTableChart);
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
                hypothesis = this.resultBrowser.GetHypothesis(index);
            }

            catch
            {
                return;
            }
            if (this.RadioFirstTable.Checked)
            {
                this.DrawBars(hypothesis, this.ContingencyTableChart);
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
                // hypothesis = this.resultBrowser.GetHypothesis(index);
                previousIndex = index;
            }

            //getting the actual hypothesis from the index of the hypothesis
            Hypothesis hypothesis = this.resultBrowser.AllHypotheses[index];
            //filling the property grid
            this.FillPropertyGrid(hypothesis, index);
            this.DrawBars(hypothesis);
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

            for (int i = 0; i < marks.Length; i++)
            {
                if (marks[i].Selected)
                {
                    ColumnHeader markHeader = new ColumnHeader();
                    markHeader.Text = marks[i].ColumnName;
                    markHeader.Width = marks[i].width;
                    markHeader.Tag = "c" + i.ToString();
                    this.HypothesesListView.Columns.Add(markHeader);
                }
            }

            for (int i = 0; i < quantifiers.Length; i++)
            {
                if (quantifiers[i].Selected)
                {
                    ColumnHeader quantifierHeader = new ColumnHeader();
                    quantifierHeader.Text = quantifiers[i].QuantifierUserLabel;
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
                this.marks[index].width = HypothesesListView.Columns[e.ColumnIndex].Width;
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
            int newSize;
            CHLMarks.Left = zero;

            //resizing the filters for the 4FT and SD4FT tasks
            if (resultBrowser != null)
            {
                if (resultBrowser.TaskType != TaskTypeEnum.FourFold &&
                    resultBrowser.TaskType != TaskTypeEnum.SDFourFold)
                {
                    //computing the size of individual filters after resize
                    newSize = (newControlWidth - zero - numberOfFFTFilters *
                        resizeFilterOffset) / numberOfOtherFilters;

                    CHLMarks.Width = newSize;
                    LQuantifiersToDisplay.Left = CHLMarks.Right + resizeFilterOffset;
                    CHLQuantifiers.Left = CHLMarks.Right + resizeFilterOffset;
                    CHLQuantifiers.Width = newSize;
                    LConditionFilter.Left = CHLQuantifiers.Right + resizeFilterOffset;
                    CHLBoxConditions.Left = CHLQuantifiers.Right + resizeFilterOffset;
                    CHLBoxConditions.Width = newSize;
                    return;
                }
            }

            //computing the size of individual filters after resize
            newSize = (newControlWidth - zero - numberOfFFTFilters *
                resizeFilterOffset) / numberOfFFTFilters;

            //resizing the individual filters for the rest of the procedures
            CHLMarks.Width = newSize;
            LQuantifiersToDisplay.Left = CHLMarks.Right + resizeFilterOffset;
            CHLQuantifiers.Left = CHLMarks.Right + resizeFilterOffset;
            CHLQuantifiers.Width = newSize;
            LConditionFilter.Left = CHLQuantifiers.Right + resizeFilterOffset;
            CHLBoxConditions.Left = CHLQuantifiers.Right + resizeFilterOffset;
            CHLBoxConditions.Width = newSize;
            CHLBoxAntecedents.Left = CHLBoxConditions.Right + resizeFilterOffset;
            LAntecedentFilter.Left = CHLBoxConditions.Right + resizeFilterOffset;
            CHLBoxAntecedents.Width = newSize;
            LSuccedentFilter.Left = CHLBoxAntecedents.Right + resizeFilterOffset;
            CHLBoxSuccedents.Left = CHLBoxAntecedents.Right + resizeFilterOffset;
            CHLBoxSuccedents.Width = newSize;
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
            CHBShowLabels.Text = resManager.GetString("ShowLabels");

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
}