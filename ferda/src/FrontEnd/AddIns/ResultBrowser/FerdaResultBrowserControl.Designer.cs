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
            this.QuantifiersListContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ResultBrowserSplit = new System.Windows.Forms.SplitContainer();
            this.HypothesesListView = new System.Windows.Forms.ListView();
            this.ColumnHypotheseName = new System.Windows.Forms.ColumnHeader();
            this.ColumnAntecedent = new System.Windows.Forms.ColumnHeader();
            this.ColumnSuccedent = new System.Windows.Forms.ColumnHeader();
            this.ColumnCondition = new System.Windows.Forms.ColumnHeader();
            this.panel1 = new System.Windows.Forms.Panel();
            this.LabelNumeric = new System.Windows.Forms.Label();
            this.NumericUpDownDecimals = new System.Windows.Forms.NumericUpDown();
            this.LabelCurrentlySorted = new System.Windows.Forms.Label();
            this.LabelSortedBy = new System.Windows.Forms.Label();
            this.ComboBoxSortStatistics = new System.Windows.Forms.ComboBox();
            this.LabelSortHypotheses = new System.Windows.Forms.Label();
            this.GroupBoxChangeGraph = new System.Windows.Forms.GroupBox();
            this.CheckBoxShowLabels = new System.Windows.Forms.CheckBox();
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
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownDecimals)).BeginInit();
            this.GroupBoxChangeGraph.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBarVOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBarHOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBarZoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBar3d)).BeginInit();
            this.ContextMenuGraphRightClick.SuspendLayout();
            this.SuspendLayout();
            // 
            // QuantifiersListContextMenu
            // 
            this.QuantifiersListContextMenu.Name = "contextMenuStrip1";
            this.QuantifiersListContextMenu.Size = new System.Drawing.Size(61, 4);
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
            this.ResultBrowserSplit.Panel2.Controls.Add(this.GroupBoxChangeGraph);
            this.ResultBrowserSplit.Size = new System.Drawing.Size(1064, 517);
            this.ResultBrowserSplit.SplitterDistance = 287;
            this.ResultBrowserSplit.TabIndex = 1;
            this.ResultBrowserSplit.Text = "splitContainer1";
            // 
            // HypothesesListView
            // 
            this.HypothesesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnHypotheseName,
            this.ColumnAntecedent,
            this.ColumnSuccedent,
            this.ColumnCondition});
            this.HypothesesListView.ContextMenuStrip = this.QuantifiersListContextMenu;
            this.HypothesesListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HypothesesListView.FullRowSelect = true;
            this.HypothesesListView.GridLines = true;
            this.HypothesesListView.Location = new System.Drawing.Point(0, 29);
            this.HypothesesListView.MultiSelect = false;
            this.HypothesesListView.Name = "HypothesesListView";
            this.HypothesesListView.Size = new System.Drawing.Size(1064, 258);
            this.HypothesesListView.TabIndex = 3;
            this.HypothesesListView.UseCompatibleStateImageBehavior = false;
            this.HypothesesListView.View = System.Windows.Forms.View.Details;
            // 
            // ColumnHypotheseName
            // 
            this.ColumnHypotheseName.Width = 250;
            // 
            // ColumnAntecedent
            // 
            this.ColumnAntecedent.Width = 180;
            // 
            // ColumnSuccedent
            // 
            this.ColumnSuccedent.Width = 180;
            // 
            // ColumnCondition
            // 
            this.ColumnCondition.Width = 180;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.LabelNumeric);
            this.panel1.Controls.Add(this.NumericUpDownDecimals);
            this.panel1.Controls.Add(this.LabelCurrentlySorted);
            this.panel1.Controls.Add(this.LabelSortedBy);
            this.panel1.Controls.Add(this.ComboBoxSortStatistics);
            this.panel1.Controls.Add(this.LabelSortHypotheses);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1064, 29);
            this.panel1.TabIndex = 2;
            // 
            // LabelNumeric
            // 
            this.LabelNumeric.AutoSize = true;
            this.LabelNumeric.Dock = System.Windows.Forms.DockStyle.Right;
            this.LabelNumeric.Location = new System.Drawing.Point(894, 0);
            this.LabelNumeric.Name = "LabelNumeric";
            this.LabelNumeric.Size = new System.Drawing.Size(50, 13);
            this.LabelNumeric.TabIndex = 5;
            this.LabelNumeric.Text = "Precision";
            // 
            // NumericUpDownDecimals
            // 
            this.NumericUpDownDecimals.Dock = System.Windows.Forms.DockStyle.Right;
            this.NumericUpDownDecimals.Location = new System.Drawing.Point(944, 0);
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
            // LabelCurrentlySorted
            // 
            this.LabelCurrentlySorted.AutoSize = true;
            this.LabelCurrentlySorted.Location = new System.Drawing.Point(300, 7);
            this.LabelCurrentlySorted.Name = "LabelCurrentlySorted";
            this.LabelCurrentlySorted.Size = new System.Drawing.Size(37, 13);
            this.LabelCurrentlySorted.TabIndex = 3;
            this.LabelCurrentlySorted.Text = "(none)";
            // 
            // LabelSortedBy
            // 
            this.LabelSortedBy.AutoSize = true;
            this.LabelSortedBy.Location = new System.Drawing.Point(239, 7);
            this.LabelSortedBy.Name = "LabelSortedBy";
            this.LabelSortedBy.Size = new System.Drawing.Size(55, 13);
            this.LabelSortedBy.TabIndex = 2;
            this.LabelSortedBy.Text = "Sorted by:";
            // 
            // ComboBoxSortStatistics
            // 
            this.ComboBoxSortStatistics.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBoxSortStatistics.FormattingEnabled = true;
            this.ComboBoxSortStatistics.Location = new System.Drawing.Point(61, 3);
            this.ComboBoxSortStatistics.Name = "ComboBoxSortStatistics";
            this.ComboBoxSortStatistics.Size = new System.Drawing.Size(141, 21);
            this.ComboBoxSortStatistics.TabIndex = 1;
            this.ComboBoxSortStatistics.SelectionChangeCommitted += new System.EventHandler(this.ComboBoxSortStatistics_SelectionChangeCommitted);
            // 
            // LabelSortHypotheses
            // 
            this.LabelSortHypotheses.AutoSize = true;
            this.LabelSortHypotheses.Location = new System.Drawing.Point(3, 7);
            this.LabelSortHypotheses.Name = "LabelSortHypotheses";
            this.LabelSortHypotheses.Size = new System.Drawing.Size(43, 13);
            this.LabelSortHypotheses.TabIndex = 0;
            this.LabelSortHypotheses.Text = "Sort by:";
            // 
            // GroupBoxChangeGraph
            // 
            this.GroupBoxChangeGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupBoxChangeGraph.Controls.Add(this.CheckBoxShowLabels);
            this.GroupBoxChangeGraph.Controls.Add(this.LabelVOffset);
            this.GroupBoxChangeGraph.Controls.Add(this.LabelHOffset);
            this.GroupBoxChangeGraph.Controls.Add(this.TrackBarVOffset);
            this.GroupBoxChangeGraph.Controls.Add(this.TrackBarHOffset);
            this.GroupBoxChangeGraph.Controls.Add(this.LabelZoom);
            this.GroupBoxChangeGraph.Controls.Add(this.TrackBarZoom);
            this.GroupBoxChangeGraph.Controls.Add(this.Label3dpercent);
            this.GroupBoxChangeGraph.Controls.Add(this.TrackBar3d);
            this.GroupBoxChangeGraph.Location = new System.Drawing.Point(830, 57);
            this.GroupBoxChangeGraph.Name = "GroupBoxChangeGraph";
            this.GroupBoxChangeGraph.Size = new System.Drawing.Size(233, 168);
            this.GroupBoxChangeGraph.TabIndex = 0;
            this.GroupBoxChangeGraph.TabStop = false;
            this.GroupBoxChangeGraph.Text = "View options";
            this.GroupBoxChangeGraph.Visible = false;
            // 
            // CheckBoxShowLabels
            // 
            this.CheckBoxShowLabels.AutoSize = true;
            this.CheckBoxShowLabels.Location = new System.Drawing.Point(9, 145);
            this.CheckBoxShowLabels.Name = "CheckBoxShowLabels";
            this.CheckBoxShowLabels.Size = new System.Drawing.Size(83, 17);
            this.CheckBoxShowLabels.TabIndex = 8;
            this.CheckBoxShowLabels.Text = "Show labels";
            this.CheckBoxShowLabels.UseVisualStyleBackColor = true;
            this.CheckBoxShowLabels.CheckedChanged += new System.EventHandler(this.CheckBoxShowLabels_CheckedChanged);
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
            this.TrackBarVOffset.Size = new System.Drawing.Size(173, 42);
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
            this.TrackBarHOffset.Size = new System.Drawing.Size(173, 42);
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
            this.TrackBarZoom.Size = new System.Drawing.Size(173, 42);
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
            this.TrackBar3d.Size = new System.Drawing.Size(173, 42);
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
            this.ResultBrowserSplit.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownDecimals)).EndInit();
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

        private System.Windows.Forms.ContextMenuStrip QuantifiersListContextMenu;
        private System.Windows.Forms.SplitContainer ResultBrowserSplit;
        private System.Windows.Forms.ListView HypothesesListView;
        private System.Windows.Forms.ColumnHeader ColumnHypotheseName;
        private System.Windows.Forms.ColumnHeader ColumnAntecedent;
        private System.Windows.Forms.ColumnHeader ColumnSuccedent;
        private System.Windows.Forms.ColumnHeader ColumnCondition;
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
        private System.Windows.Forms.CheckBox CheckBoxShowLabels;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox ComboBoxSortStatistics;
        private System.Windows.Forms.Label LabelSortHypotheses;
        private System.Windows.Forms.Label LabelCurrentlySorted;
        private System.Windows.Forms.Label LabelSortedBy;
        private System.Windows.Forms.NumericUpDown NumericUpDownDecimals;
        private System.Windows.Forms.Label LabelNumeric;
    }
}

