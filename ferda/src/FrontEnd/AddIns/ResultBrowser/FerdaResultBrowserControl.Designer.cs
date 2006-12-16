// FerdaResultBrowserControl.cs - GUI part
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


namespace Ferda.FrontEnd.AddIns.ResultBrowser
{
    partial class FerdaResultBrowserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code


        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ResultBrowserSplit = new System.Windows.Forms.SplitContainer();
            this.HypothesesListView = new System.Windows.Forms.ListView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.contingencyTablesPanel = new System.Windows.Forms.Panel();
            this.RadioSecondTable = new System.Windows.Forms.RadioButton();
            this.RadioFirstTable = new System.Windows.Forms.RadioButton();
            this.LQuantifiersToDisplay = new System.Windows.Forms.Label();
            this.CHLQuantifiers = new System.Windows.Forms.CheckedListBox();
            this.ButtonSubmitColumnChange = new System.Windows.Forms.Button();
            this.LColumnsToDisplay = new System.Windows.Forms.Label();
            this.CHLMarks = new System.Windows.Forms.CheckedListBox();
            this.ButtonHelp = new System.Windows.Forms.Button();
            this.LabelCount = new System.Windows.Forms.Label();
            this.LabelHypothesesTotal = new System.Windows.Forms.Label();
            this.ButtonSubmitFilter = new System.Windows.Forms.Button();
            this.LConditionFilter = new System.Windows.Forms.Label();
            this.LSuccedentFilter = new System.Windows.Forms.Label();
            this.LAntecedentFilter = new System.Windows.Forms.Label();
            this.CHLBoxConditions = new System.Windows.Forms.CheckedListBox();
            this.CHLBoxSuccedents = new System.Windows.Forms.CheckedListBox();
            this.CHLBoxAntecedents = new System.Windows.Forms.CheckedListBox();
            this.LabelNumeric = new System.Windows.Forms.Label();
            this.NumericUpDownDecimals = new System.Windows.Forms.NumericUpDown();
            this.StatusStrip = new System.Windows.Forms.StatusStrip();
            this.LabelProgressBar = new System.Windows.Forms.ToolStripStatusLabel();
            this.ProgressBarIceTicks = new System.Windows.Forms.ToolStripProgressBar();
            this.GroupBoxChangeGraph = new System.Windows.Forms.GroupBox();
            this.CHBShowLabels = new System.Windows.Forms.CheckBox();
            this.LabelVOffset = new System.Windows.Forms.Label();
            this.LabelHOffset = new System.Windows.Forms.Label();
            this.TrackBarVOffset = new System.Windows.Forms.TrackBar();
            this.TrackBarHOffset = new System.Windows.Forms.TrackBar();
            this.LabelZoom = new System.Windows.Forms.Label();
            this.TrackBarZoom = new System.Windows.Forms.TrackBar();
            this.Label3dpercent = new System.Windows.Forms.Label();
            this.TrackBar3d = new System.Windows.Forms.TrackBar();
            this.ContextMenuGraphRightClick = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripShowGraphEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripCopyChart = new System.Windows.Forms.ToolStripMenuItem();
            this.ResultBrowserSplit.Panel1.SuspendLayout();
            this.ResultBrowserSplit.Panel2.SuspendLayout();
            this.ResultBrowserSplit.SuspendLayout();
            this.panel1.SuspendLayout();
            this.contingencyTablesPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownDecimals)).BeginInit();
            this.StatusStrip.SuspendLayout();
            this.GroupBoxChangeGraph.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBarVOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBarHOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBarZoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBar3d)).BeginInit();
            this.ContextMenuGraphRightClick.SuspendLayout();
            this.SuspendLayout();
            // 
            // ResultBrowserSplit
            // 
            this.ResultBrowserSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResultBrowserSplit.Location = new System.Drawing.Point(0, 0);
            this.ResultBrowserSplit.Name = "ResultBrowserSplit";
            this.ResultBrowserSplit.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // ResultBrowserSplit.Panel1
            // 
            this.ResultBrowserSplit.Panel1.Controls.Add(this.HypothesesListView);
            this.ResultBrowserSplit.Panel1.Controls.Add(this.panel1);
            // 
            // ResultBrowserSplit.Panel2
            // 
            this.ResultBrowserSplit.Panel2.Controls.Add(this.StatusStrip);
            this.ResultBrowserSplit.Panel2.Controls.Add(this.GroupBoxChangeGraph);
            this.ResultBrowserSplit.Size = new System.Drawing.Size(1064, 517);
            this.ResultBrowserSplit.SplitterDistance = 287;
            this.ResultBrowserSplit.TabIndex = 1;
            this.ResultBrowserSplit.Text = "splitContainer1";
            // 
            // HypothesesListView
            // 
            this.HypothesesListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HypothesesListView.FullRowSelect = true;
            this.HypothesesListView.GridLines = true;
            this.HypothesesListView.Location = new System.Drawing.Point(0, 99);
            this.HypothesesListView.MultiSelect = false;
            this.HypothesesListView.Name = "HypothesesListView";
            this.HypothesesListView.Size = new System.Drawing.Size(1064, 188);
            this.HypothesesListView.TabIndex = 3;
            this.HypothesesListView.UseCompatibleStateImageBehavior = false;
            this.HypothesesListView.View = System.Windows.Forms.View.Details;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.contingencyTablesPanel);
            this.panel1.Controls.Add(this.LQuantifiersToDisplay);
            this.panel1.Controls.Add(this.CHLQuantifiers);
            this.panel1.Controls.Add(this.ButtonSubmitColumnChange);
            this.panel1.Controls.Add(this.LColumnsToDisplay);
            this.panel1.Controls.Add(this.CHLMarks);
            this.panel1.Controls.Add(this.ButtonHelp);
            this.panel1.Controls.Add(this.LabelCount);
            this.panel1.Controls.Add(this.LabelHypothesesTotal);
            this.panel1.Controls.Add(this.ButtonSubmitFilter);
            this.panel1.Controls.Add(this.LConditionFilter);
            this.panel1.Controls.Add(this.LSuccedentFilter);
            this.panel1.Controls.Add(this.LAntecedentFilter);
            this.panel1.Controls.Add(this.CHLBoxConditions);
            this.panel1.Controls.Add(this.CHLBoxSuccedents);
            this.panel1.Controls.Add(this.CHLBoxAntecedents);
            this.panel1.Controls.Add(this.LabelNumeric);
            this.panel1.Controls.Add(this.NumericUpDownDecimals);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1064, 99);
            this.panel1.TabIndex = 2;
            // 
            // contingencyTablesPanel
            // 
            this.contingencyTablesPanel.Controls.Add(this.RadioSecondTable);
            this.contingencyTablesPanel.Controls.Add(this.RadioFirstTable);
            this.contingencyTablesPanel.Location = new System.Drawing.Point(6, 48);
            this.contingencyTablesPanel.Name = "contingencyTablesPanel";
            this.contingencyTablesPanel.Size = new System.Drawing.Size(213, 45);
            this.contingencyTablesPanel.TabIndex = 21;
            // 
            // RadioSecondTable
            // 
            this.RadioSecondTable.AutoSize = true;
            this.RadioSecondTable.Location = new System.Drawing.Point(3, 25);
            this.RadioSecondTable.Name = "RadioSecondTable";
            this.RadioSecondTable.Size = new System.Drawing.Size(85, 17);
            this.RadioSecondTable.TabIndex = 1;
            this.RadioSecondTable.Text = "radioButton2";
            this.RadioSecondTable.UseVisualStyleBackColor = true;
            // 
            // RadioFirstTable
            // 
            this.RadioFirstTable.AutoSize = true;
            this.RadioFirstTable.Checked = true;
            this.RadioFirstTable.Location = new System.Drawing.Point(3, 6);
            this.RadioFirstTable.Name = "RadioFirstTable";
            this.RadioFirstTable.Size = new System.Drawing.Size(85, 17);
            this.RadioFirstTable.TabIndex = 0;
            this.RadioFirstTable.TabStop = true;
            this.RadioFirstTable.Text = "radioButton1";
            this.RadioFirstTable.UseVisualStyleBackColor = true;
            // 
            // LQuantifiersToDisplay
            // 
            this.LQuantifiersToDisplay.AutoSize = true;
            this.LQuantifiersToDisplay.Location = new System.Drawing.Point(348, 5);
            this.LQuantifiersToDisplay.Name = "LQuantifiersToDisplay";
            this.LQuantifiersToDisplay.Size = new System.Drawing.Size(104, 13);
            this.LQuantifiersToDisplay.TabIndex = 20;
            this.LQuantifiersToDisplay.Text = "Quantifiers to display";
            // 
            // CHLQuantifiers
            // 
            this.CHLQuantifiers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CHLQuantifiers.CheckOnClick = true;
            this.CHLQuantifiers.FormattingEnabled = true;
            this.CHLQuantifiers.Location = new System.Drawing.Point(350, 22);
            this.CHLQuantifiers.Name = "CHLQuantifiers";
            this.CHLQuantifiers.Size = new System.Drawing.Size(120, 47);
            this.CHLQuantifiers.TabIndex = 19;
            // 
            // ButtonSubmitColumnChange
            // 
            this.ButtonSubmitColumnChange.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonSubmitColumnChange.Location = new System.Drawing.Point(225, 70);
            this.ButtonSubmitColumnChange.Name = "ButtonSubmitColumnChange";
            this.ButtonSubmitColumnChange.Size = new System.Drawing.Size(75, 23);
            this.ButtonSubmitColumnChange.TabIndex = 18;
            this.ButtonSubmitColumnChange.Text = "Re-display";
            this.ButtonSubmitColumnChange.UseVisualStyleBackColor = true;
            this.ButtonSubmitColumnChange.Click += new System.EventHandler(this.ButtonSubmitColumnChange_Click);
            // 
            // LColumnsToDisplay
            // 
            this.LColumnsToDisplay.AutoSize = true;
            this.LColumnsToDisplay.Location = new System.Drawing.Point(222, 5);
            this.LColumnsToDisplay.Name = "LColumnsToDisplay";
            this.LColumnsToDisplay.Size = new System.Drawing.Size(94, 13);
            this.LColumnsToDisplay.TabIndex = 17;
            this.LColumnsToDisplay.Text = "Columns to display";
            // 
            // CHLMarks
            // 
            this.CHLMarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CHLMarks.CheckOnClick = true;
            this.CHLMarks.FormattingEnabled = true;
            this.CHLMarks.Location = new System.Drawing.Point(225, 22);
            this.CHLMarks.Name = "CHLMarks";
            this.CHLMarks.Size = new System.Drawing.Size(120, 47);
            this.CHLMarks.TabIndex = 16;
            // 
            // ButtonHelp
            // 
            this.ButtonHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonHelp.Location = new System.Drawing.Point(387, 71);
            this.ButtonHelp.Name = "ButtonHelp";
            this.ButtonHelp.Size = new System.Drawing.Size(75, 22);
            this.ButtonHelp.TabIndex = 15;
            this.ButtonHelp.Text = "Help";
            this.ButtonHelp.UseVisualStyleBackColor = true;
            this.ButtonHelp.Click += new System.EventHandler(this.ButtonHelp_Click);
            // 
            // LabelCount
            // 
            this.LabelCount.AutoSize = true;
            this.LabelCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.LabelCount.Location = new System.Drawing.Point(160, 32);
            this.LabelCount.Name = "LabelCount";
            this.LabelCount.Size = new System.Drawing.Size(24, 12);
            this.LabelCount.TabIndex = 14;
            this.LabelCount.Text = "(0/0)";
            this.LabelCount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // LabelHypothesesTotal
            // 
            this.LabelHypothesesTotal.AutoSize = true;
            this.LabelHypothesesTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.LabelHypothesesTotal.Location = new System.Drawing.Point(3, 32);
            this.LabelHypothesesTotal.Name = "LabelHypothesesTotal";
            this.LabelHypothesesTotal.Size = new System.Drawing.Size(151, 12);
            this.LabelHypothesesTotal.TabIndex = 13;
            this.LabelHypothesesTotal.Text = "Hypotheses count (displayed/total):";
            // 
            // ButtonSubmitFilter
            // 
            this.ButtonSubmitFilter.Enabled = false;
            this.ButtonSubmitFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonSubmitFilter.Location = new System.Drawing.Point(306, 71);
            this.ButtonSubmitFilter.Name = "ButtonSubmitFilter";
            this.ButtonSubmitFilter.Size = new System.Drawing.Size(75, 22);
            this.ButtonSubmitFilter.TabIndex = 12;
            this.ButtonSubmitFilter.Text = "Re-filter";
            this.ButtonSubmitFilter.UseVisualStyleBackColor = true;
            this.ButtonSubmitFilter.Click += new System.EventHandler(this.ButtonSubmitFilter_Click);
            // 
            // LConditionFilter
            // 
            this.LConditionFilter.AutoSize = true;
            this.LConditionFilter.Location = new System.Drawing.Point(471, 5);
            this.LConditionFilter.Name = "LConditionFilter";
            this.LConditionFilter.Size = new System.Drawing.Size(73, 13);
            this.LConditionFilter.TabIndex = 11;
            this.LConditionFilter.Text = "Condition filter";
            // 
            // LSuccedentFilter
            // 
            this.LSuccedentFilter.AutoSize = true;
            this.LSuccedentFilter.Location = new System.Drawing.Point(725, 5);
            this.LSuccedentFilter.Name = "LSuccedentFilter";
            this.LSuccedentFilter.Size = new System.Drawing.Size(81, 13);
            this.LSuccedentFilter.TabIndex = 10;
            this.LSuccedentFilter.Text = "Succedent filter";
            // 
            // LAntecedentFilter
            // 
            this.LAntecedentFilter.AutoSize = true;
            this.LAntecedentFilter.Location = new System.Drawing.Point(600, 5);
            this.LAntecedentFilter.Name = "LAntecedentFilter";
            this.LAntecedentFilter.Size = new System.Drawing.Size(84, 13);
            this.LAntecedentFilter.TabIndex = 9;
            this.LAntecedentFilter.Text = "Antecedent filter";
            // 
            // CHLBoxConditions
            // 
            this.CHLBoxConditions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CHLBoxConditions.CheckOnClick = true;
            this.CHLBoxConditions.Enabled = false;
            this.CHLBoxConditions.FormattingEnabled = true;
            this.CHLBoxConditions.Location = new System.Drawing.Point(475, 22);
            this.CHLBoxConditions.Name = "CHLBoxConditions";
            this.CHLBoxConditions.Size = new System.Drawing.Size(120, 47);
            this.CHLBoxConditions.TabIndex = 8;
            // 
            // CHLBoxSuccedents
            // 
            this.CHLBoxSuccedents.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CHLBoxSuccedents.CheckOnClick = true;
            this.CHLBoxSuccedents.Enabled = false;
            this.CHLBoxSuccedents.FormattingEnabled = true;
            this.CHLBoxSuccedents.Location = new System.Drawing.Point(725, 22);
            this.CHLBoxSuccedents.Name = "CHLBoxSuccedents";
            this.CHLBoxSuccedents.Size = new System.Drawing.Size(120, 47);
            this.CHLBoxSuccedents.TabIndex = 7;
            // 
            // CHLBoxAntecedents
            // 
            this.CHLBoxAntecedents.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CHLBoxAntecedents.CheckOnClick = true;
            this.CHLBoxAntecedents.Enabled = false;
            this.CHLBoxAntecedents.FormattingEnabled = true;
            this.CHLBoxAntecedents.Location = new System.Drawing.Point(600, 22);
            this.CHLBoxAntecedents.Name = "CHLBoxAntecedents";
            this.CHLBoxAntecedents.Size = new System.Drawing.Size(120, 47);
            this.CHLBoxAntecedents.TabIndex = 6;
            // 
            // LabelNumeric
            // 
            this.LabelNumeric.AutoSize = true;
            this.LabelNumeric.Location = new System.Drawing.Point(3, 9);
            this.LabelNumeric.Name = "LabelNumeric";
            this.LabelNumeric.Size = new System.Drawing.Size(50, 13);
            this.LabelNumeric.TabIndex = 5;
            this.LabelNumeric.Text = "Precision";
            // 
            // NumericUpDownDecimals
            // 
            this.NumericUpDownDecimals.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NumericUpDownDecimals.Enabled = false;
            this.NumericUpDownDecimals.Location = new System.Drawing.Point(99, 7);
            this.NumericUpDownDecimals.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.NumericUpDownDecimals.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumericUpDownDecimals.Name = "NumericUpDownDecimals";
            this.NumericUpDownDecimals.Size = new System.Drawing.Size(120, 20);
            this.NumericUpDownDecimals.TabIndex = 2;
            this.NumericUpDownDecimals.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.NumericUpDownDecimals.ValueChanged += new System.EventHandler(this.NumericUpDownDecimals_ValueChanged);
            // 
            // StatusStrip
            // 
            this.StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LabelProgressBar,
            this.ProgressBarIceTicks});
            this.StatusStrip.Location = new System.Drawing.Point(0, 204);
            this.StatusStrip.Name = "StatusStrip";
            this.StatusStrip.Size = new System.Drawing.Size(1064, 22);
            this.StatusStrip.TabIndex = 1;
            this.StatusStrip.Text = "statusStrip1";
            // 
            // LabelProgressBar
            // 
            this.LabelProgressBar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.LabelProgressBar.Name = "LabelProgressBar";
            this.LabelProgressBar.Size = new System.Drawing.Size(105, 17);
            this.LabelProgressBar.Text = "Hypotheses loading:";
            // 
            // ProgressBarIceTicks
            // 
            this.ProgressBarIceTicks.Name = "ProgressBarIceTicks";
            this.ProgressBarIceTicks.Size = new System.Drawing.Size(300, 16);
            this.ProgressBarIceTicks.Step = 1;
            // 
            // GroupBoxChangeGraph
            // 
            this.GroupBoxChangeGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupBoxChangeGraph.Controls.Add(this.CHBShowLabels);
            this.GroupBoxChangeGraph.Controls.Add(this.LabelVOffset);
            this.GroupBoxChangeGraph.Controls.Add(this.LabelHOffset);
            this.GroupBoxChangeGraph.Controls.Add(this.TrackBarVOffset);
            this.GroupBoxChangeGraph.Controls.Add(this.TrackBarHOffset);
            this.GroupBoxChangeGraph.Controls.Add(this.LabelZoom);
            this.GroupBoxChangeGraph.Controls.Add(this.TrackBarZoom);
            this.GroupBoxChangeGraph.Controls.Add(this.Label3dpercent);
            this.GroupBoxChangeGraph.Controls.Add(this.TrackBar3d);
            this.GroupBoxChangeGraph.Location = new System.Drawing.Point(752, 3);
            this.GroupBoxChangeGraph.Name = "GroupBoxChangeGraph";
            this.GroupBoxChangeGraph.Size = new System.Drawing.Size(311, 194);
            this.GroupBoxChangeGraph.TabIndex = 0;
            this.GroupBoxChangeGraph.TabStop = false;
            this.GroupBoxChangeGraph.Text = "View options";
            this.GroupBoxChangeGraph.Visible = false;
            // 
            // CHBShowLabels
            // 
            this.CHBShowLabels.AutoSize = true;
            this.CHBShowLabels.Location = new System.Drawing.Point(9, 145);
            this.CHBShowLabels.Name = "CHBShowLabels";
            this.CHBShowLabels.Size = new System.Drawing.Size(83, 17);
            this.CHBShowLabels.TabIndex = 8;
            this.CHBShowLabels.Text = "Show labels";
            this.CHBShowLabels.UseVisualStyleBackColor = true;
            this.CHBShowLabels.CheckedChanged += new System.EventHandler(this.CheckBoxShowLabels_CheckedChanged);
            // 
            // LabelVOffset
            // 
            this.LabelVOffset.AutoSize = true;
            this.LabelVOffset.Location = new System.Drawing.Point(6, 116);
            this.LabelVOffset.Name = "LabelVOffset";
            this.LabelVOffset.Size = new System.Drawing.Size(55, 13);
            this.LabelVOffset.TabIndex = 7;
            this.LabelVOffset.Text = "Ver. offset";
            // 
            // LabelHOffset
            // 
            this.LabelHOffset.AutoSize = true;
            this.LabelHOffset.Location = new System.Drawing.Point(6, 86);
            this.LabelHOffset.Name = "LabelHOffset";
            this.LabelHOffset.Size = new System.Drawing.Size(56, 13);
            this.LabelHOffset.TabIndex = 6;
            this.LabelHOffset.Text = "Hor. offset";
            // 
            // TrackBarVOffset
            // 
            this.TrackBarVOffset.LargeChange = 500;
            this.TrackBarVOffset.Location = new System.Drawing.Point(63, 106);
            this.TrackBarVOffset.Maximum = 1500;
            this.TrackBarVOffset.Minimum = -1500;
            this.TrackBarVOffset.Name = "TrackBarVOffset";
            this.TrackBarVOffset.Size = new System.Drawing.Size(216, 42);
            this.TrackBarVOffset.SmallChange = 100;
            this.TrackBarVOffset.TabIndex = 7;
            this.TrackBarVOffset.TickFrequency = 100;
            this.TrackBarVOffset.Scroll += new System.EventHandler(this.TrackBarVOffset_Scroll);
            // 
            // TrackBarHOffset
            // 
            this.TrackBarHOffset.LargeChange = 500;
            this.TrackBarHOffset.Location = new System.Drawing.Point(63, 76);
            this.TrackBarHOffset.Maximum = 1500;
            this.TrackBarHOffset.Minimum = -1500;
            this.TrackBarHOffset.Name = "TrackBarHOffset";
            this.TrackBarHOffset.Size = new System.Drawing.Size(216, 42);
            this.TrackBarHOffset.SmallChange = 100;
            this.TrackBarHOffset.TabIndex = 6;
            this.TrackBarHOffset.TickFrequency = 100;
            this.TrackBarHOffset.Scroll += new System.EventHandler(this.TrackBarHOffset_Scroll);
            // 
            // LabelZoom
            // 
            this.LabelZoom.AutoSize = true;
            this.LabelZoom.Location = new System.Drawing.Point(6, 56);
            this.LabelZoom.Name = "LabelZoom";
            this.LabelZoom.Size = new System.Drawing.Size(34, 13);
            this.LabelZoom.TabIndex = 3;
            this.LabelZoom.Text = "Zoom";
            // 
            // TrackBarZoom
            // 
            this.TrackBarZoom.LargeChange = 200;
            this.TrackBarZoom.Location = new System.Drawing.Point(63, 46);
            this.TrackBarZoom.Maximum = 1000;
            this.TrackBarZoom.Minimum = 5;
            this.TrackBarZoom.Name = "TrackBarZoom";
            this.TrackBarZoom.Size = new System.Drawing.Size(216, 42);
            this.TrackBarZoom.SmallChange = 50;
            this.TrackBarZoom.TabIndex = 5;
            this.TrackBarZoom.TickFrequency = 50;
            this.TrackBarZoom.Value = 100;
            this.TrackBarZoom.Scroll += new System.EventHandler(this.TrackBarZoom_Scroll);
            // 
            // Label3dpercent
            // 
            this.Label3dpercent.AutoSize = true;
            this.Label3dpercent.Location = new System.Drawing.Point(6, 26);
            this.Label3dpercent.Name = "Label3dpercent";
            this.Label3dpercent.Size = new System.Drawing.Size(30, 13);
            this.Label3dpercent.TabIndex = 1;
            this.Label3dpercent.Text = "3d %";
            // 
            // TrackBar3d
            // 
            this.TrackBar3d.LargeChange = 20;
            this.TrackBar3d.Location = new System.Drawing.Point(63, 16);
            this.TrackBar3d.Maximum = 100;
            this.TrackBar3d.Name = "TrackBar3d";
            this.TrackBar3d.Size = new System.Drawing.Size(216, 42);
            this.TrackBar3d.SmallChange = 10;
            this.TrackBar3d.TabIndex = 4;
            this.TrackBar3d.TickFrequency = 5;
            this.TrackBar3d.Value = 15;
            this.TrackBar3d.Scroll += new System.EventHandler(this.TrackBar3d_Scroll);
            // 
            // ContextMenuGraphRightClick
            // 
            this.ContextMenuGraphRightClick.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripShowGraphEdit,
            this.toolStripSeparator1,
            this.ToolStripCopyChart});
            this.ContextMenuGraphRightClick.Name = "ContextMenuGraphRightClick";
            this.ContextMenuGraphRightClick.Size = new System.Drawing.Size(180, 54);
            // 
            // ToolStripShowGraphEdit
            // 
            this.ToolStripShowGraphEdit.Name = "ToolStripShowGraphEdit";
            this.ToolStripShowGraphEdit.Size = new System.Drawing.Size(179, 22);
            this.ToolStripShowGraphEdit.Text = "toolStripMenuItem1";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(176, 6);
            // 
            // ToolStripCopyChart
            // 
            this.ToolStripCopyChart.Name = "ToolStripCopyChart";
            this.ToolStripCopyChart.Size = new System.Drawing.Size(179, 22);
            this.ToolStripCopyChart.Text = "toolStripMenuItem1";
            // 
            // FerdaResultBrowserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.ResultBrowserSplit);
            this.Name = "FerdaResultBrowserControl";
            this.Size = new System.Drawing.Size(1064, 517);
            this.ResultBrowserSplit.Panel1.ResumeLayout(false);
            this.ResultBrowserSplit.Panel2.ResumeLayout(false);
            this.ResultBrowserSplit.Panel2.PerformLayout();
            this.ResultBrowserSplit.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.contingencyTablesPanel.ResumeLayout(false);
            this.contingencyTablesPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownDecimals)).EndInit();
            this.StatusStrip.ResumeLayout(false);
            this.StatusStrip.PerformLayout();
            this.GroupBoxChangeGraph.ResumeLayout(false);
            this.GroupBoxChangeGraph.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBarVOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBarHOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBarZoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBar3d)).EndInit();
            this.ContextMenuGraphRightClick.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion

        private System.Windows.Forms.SplitContainer ResultBrowserSplit;
        private System.Windows.Forms.ListView HypothesesListView;
        private System.Windows.Forms.GroupBox GroupBoxChangeGraph;
        private System.Windows.Forms.TrackBar TrackBar3d;
        private System.Windows.Forms.ContextMenuStrip ContextMenuGraphRightClick;
        private System.Windows.Forms.ToolStripMenuItem ToolStripShowGraphEdit;
        private System.Windows.Forms.Label Label3dpercent;
        private System.Windows.Forms.Label LabelZoom;
        private System.Windows.Forms.TrackBar TrackBarZoom;
        private System.Windows.Forms.Label LabelVOffset;
        private System.Windows.Forms.Label LabelHOffset;
        private System.Windows.Forms.TrackBar TrackBarVOffset;
        private System.Windows.Forms.TrackBar TrackBarHOffset;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripCopyChart;
        private System.Windows.Forms.CheckBox CHBShowLabels;
        private System.Windows.Forms.StatusStrip StatusStrip;
        private System.Windows.Forms.ToolStripProgressBar ProgressBarIceTicks;
        private System.Windows.Forms.ToolStripStatusLabel LabelProgressBar;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button ButtonHelp;
        private System.Windows.Forms.Label LabelCount;
        private System.Windows.Forms.Label LabelHypothesesTotal;
        private System.Windows.Forms.Button ButtonSubmitFilter;
        private System.Windows.Forms.Label LConditionFilter;
        private System.Windows.Forms.Label LSuccedentFilter;
        private System.Windows.Forms.Label LAntecedentFilter;
        private System.Windows.Forms.CheckedListBox CHLBoxConditions;
        private System.Windows.Forms.CheckedListBox CHLBoxSuccedents;
        private System.Windows.Forms.CheckedListBox CHLBoxAntecedents;
        private System.Windows.Forms.Label LabelNumeric;
        private System.Windows.Forms.NumericUpDown NumericUpDownDecimals;
        private System.Windows.Forms.Label LColumnsToDisplay;
        private System.Windows.Forms.CheckedListBox CHLMarks;
        private System.Windows.Forms.Button ButtonSubmitColumnChange;
        private System.Windows.Forms.Label LQuantifiersToDisplay;
        private System.Windows.Forms.CheckedListBox CHLQuantifiers;
        private System.Windows.Forms.Panel contingencyTablesPanel;
        private System.Windows.Forms.RadioButton RadioSecondTable;
        private System.Windows.Forms.RadioButton RadioFirstTable;
    }
}

