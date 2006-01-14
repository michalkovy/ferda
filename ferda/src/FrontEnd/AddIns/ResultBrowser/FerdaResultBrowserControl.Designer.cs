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
            this.GroupBoxChangeGraph = new System.Windows.Forms.GroupBox();
            this.LabelElevation = new System.Windows.Forms.Label();
            this.TrackBarElevation = new System.Windows.Forms.TrackBar();
            this.LabelPerspective = new System.Windows.Forms.Label();
            this.TrackBarPerspective = new System.Windows.Forms.TrackBar();
            this.LabelRotation = new System.Windows.Forms.Label();
            this.TrackBarRotation = new System.Windows.Forms.TrackBar();
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
            this.ResultBrowserSplit.Panel1.SuspendLayout();
            this.ResultBrowserSplit.Panel2.SuspendLayout();
            this.ResultBrowserSplit.SuspendLayout();
            this.GroupBoxChangeGraph.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBarElevation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBarPerspective)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBarRotation)).BeginInit();
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
            // 
            // ResultBrowserSplit.Panel2
            // 
            this.ResultBrowserSplit.Panel2.Controls.Add(this.GroupBoxChangeGraph);
            this.ResultBrowserSplit.Size = new System.Drawing.Size(1064, 517);
            this.ResultBrowserSplit.SplitterDistance = 321;
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
            this.HypothesesListView.Location = new System.Drawing.Point(0, 0);
            this.HypothesesListView.MultiSelect = false;
            this.HypothesesListView.Name = "HypothesesListView";
            this.HypothesesListView.Size = new System.Drawing.Size(1064, 321);
            this.HypothesesListView.TabIndex = 1;
            this.HypothesesListView.UseCompatibleStateImageBehavior = false;
            this.HypothesesListView.View = System.Windows.Forms.View.Details;
            // 
            // ColumnHypotheseName
            // 
            this.ColumnHypotheseName.Width = 107;
            // 
            // ColumnAntecedent
            // 
            this.ColumnAntecedent.Width = 104;
            // 
            // ColumnSuccedent
            // 
            this.ColumnSuccedent.Width = 104;
            // 
            // ColumnCondition
            // 
            this.ColumnCondition.Width = 101;
            // 
            // GroupBoxChangeGraph
            // 
            this.GroupBoxChangeGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupBoxChangeGraph.Controls.Add(this.LabelElevation);
            this.GroupBoxChangeGraph.Controls.Add(this.TrackBarElevation);
            this.GroupBoxChangeGraph.Controls.Add(this.LabelPerspective);
            this.GroupBoxChangeGraph.Controls.Add(this.TrackBarPerspective);
            this.GroupBoxChangeGraph.Controls.Add(this.LabelRotation);
            this.GroupBoxChangeGraph.Controls.Add(this.TrackBarRotation);
            this.GroupBoxChangeGraph.Controls.Add(this.LabelVOffset);
            this.GroupBoxChangeGraph.Controls.Add(this.LabelHOffset);
            this.GroupBoxChangeGraph.Controls.Add(this.TrackBarVOffset);
            this.GroupBoxChangeGraph.Controls.Add(this.TrackBarHOffset);
            this.GroupBoxChangeGraph.Controls.Add(this.LabelZoom);
            this.GroupBoxChangeGraph.Controls.Add(this.TrackBarZoom);
            this.GroupBoxChangeGraph.Controls.Add(this.Label3dpercent);
            this.GroupBoxChangeGraph.Controls.Add(this.TrackBar3d);
            this.GroupBoxChangeGraph.Location = new System.Drawing.Point(708, 3);
            this.GroupBoxChangeGraph.Name = "GroupBoxChangeGraph";
            this.GroupBoxChangeGraph.Size = new System.Drawing.Size(355, 188);
            this.GroupBoxChangeGraph.TabIndex = 0;
            this.GroupBoxChangeGraph.TabStop = false;
            this.GroupBoxChangeGraph.Text = "View options";
            this.GroupBoxChangeGraph.Visible = false;
            // 
            // LabelElevation
            // 
            this.LabelElevation.AutoSize = true;
            this.LabelElevation.Location = new System.Drawing.Point(209, 56);
            this.LabelElevation.Name = "LabelElevation";
            this.LabelElevation.Size = new System.Drawing.Size(51, 13);
            this.LabelElevation.TabIndex = 13;
            this.LabelElevation.Text = "Elevation";
            this.LabelElevation.Visible = false;
            // 
            // TrackBarElevation
            // 
            this.TrackBarElevation.LargeChange = 10;
            this.TrackBarElevation.Location = new System.Drawing.Point(278, 46);
            this.TrackBarElevation.Maximum = 100;
            this.TrackBarElevation.Name = "TrackBarElevation";
            this.TrackBarElevation.Size = new System.Drawing.Size(71, 42);
            this.TrackBarElevation.TabIndex = 12;
            this.TrackBarElevation.TickFrequency = 10;
            this.TrackBarElevation.Value = 15;
            this.TrackBarElevation.Visible = false;
            this.TrackBarElevation.Scroll += new System.EventHandler(this.TrackBarElevation_Scroll);
            // 
            // LabelPerspective
            // 
            this.LabelPerspective.AutoSize = true;
            this.LabelPerspective.Location = new System.Drawing.Point(209, 26);
            this.LabelPerspective.Name = "LabelPerspective";
            this.LabelPerspective.Size = new System.Drawing.Size(63, 13);
            this.LabelPerspective.TabIndex = 11;
            this.LabelPerspective.Text = "Perspective";
            this.LabelPerspective.Visible = false;
            // 
            // TrackBarPerspective
            // 
            this.TrackBarPerspective.LargeChange = 10;
            this.TrackBarPerspective.Location = new System.Drawing.Point(278, 16);
            this.TrackBarPerspective.Maximum = 100;
            this.TrackBarPerspective.Name = "TrackBarPerspective";
            this.TrackBarPerspective.Size = new System.Drawing.Size(71, 42);
            this.TrackBarPerspective.TabIndex = 10;
            this.TrackBarPerspective.TickFrequency = 10;
            this.TrackBarPerspective.Value = 15;
            this.TrackBarPerspective.Visible = false;
            this.TrackBarPerspective.Scroll += new System.EventHandler(this.TrackBarPerspective_Scroll);
            // 
            // LabelRotation
            // 
            this.LabelRotation.AutoSize = true;
            this.LabelRotation.Location = new System.Drawing.Point(6, 146);
            this.LabelRotation.Name = "LabelRotation";
            this.LabelRotation.Size = new System.Drawing.Size(47, 13);
            this.LabelRotation.TabIndex = 9;
            this.LabelRotation.Text = "Rotation";
            this.LabelRotation.Visible = false;
            // 
            // TrackBarRotation
            // 
            this.TrackBarRotation.LargeChange = 30;
            this.TrackBarRotation.Location = new System.Drawing.Point(63, 136);
            this.TrackBarRotation.Maximum = 360;
            this.TrackBarRotation.Name = "TrackBarRotation";
            this.TrackBarRotation.Size = new System.Drawing.Size(71, 42);
            this.TrackBarRotation.SmallChange = 10;
            this.TrackBarRotation.TabIndex = 8;
            this.TrackBarRotation.TickFrequency = 30;
            this.TrackBarRotation.Value = 345;
            this.TrackBarRotation.Visible = false;
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
            this.TrackBarVOffset.Size = new System.Drawing.Size(71, 42);
            this.TrackBarVOffset.SmallChange = 100;
            this.TrackBarVOffset.TabIndex = 5;
            this.TrackBarVOffset.TickFrequency = 300;
            this.TrackBarVOffset.Scroll += new System.EventHandler(this.TrackBarVOffset_Scroll);
            // 
            // TrackBarHOffset
            // 
            this.TrackBarHOffset.LargeChange = 500;
            this.TrackBarHOffset.Location = new System.Drawing.Point(63, 76);
            this.TrackBarHOffset.Maximum = 1500;
            this.TrackBarHOffset.Minimum = -1500;
            this.TrackBarHOffset.Name = "TrackBarHOffset";
            this.TrackBarHOffset.Size = new System.Drawing.Size(71, 42);
            this.TrackBarHOffset.SmallChange = 100;
            this.TrackBarHOffset.TabIndex = 4;
            this.TrackBarHOffset.TickFrequency = 300;
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
            this.TrackBarZoom.Size = new System.Drawing.Size(71, 42);
            this.TrackBarZoom.SmallChange = 50;
            this.TrackBarZoom.TabIndex = 2;
            this.TrackBarZoom.TickFrequency = 100;
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
            this.TrackBar3d.Size = new System.Drawing.Size(71, 42);
            this.TrackBar3d.SmallChange = 10;
            this.TrackBar3d.TabIndex = 0;
            this.TrackBar3d.TickFrequency = 10;
            this.TrackBar3d.Value = 15;
            this.TrackBar3d.Scroll += new System.EventHandler(this.TrackBar3d_Scroll);
            // 
            // ContextMenuGraphRightClick
            // 
            this.ContextMenuGraphRightClick.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripShowGraphEdit});
            this.ContextMenuGraphRightClick.Name = "ContextMenuGraphRightClick";
            this.ContextMenuGraphRightClick.Size = new System.Drawing.Size(180, 26);
            // 
            // ToolStripShowGraphEdit
            // 
            this.ToolStripShowGraphEdit.Name = "ToolStripShowGraphEdit";
            this.ToolStripShowGraphEdit.Size = new System.Drawing.Size(179, 22);
            this.ToolStripShowGraphEdit.Text = "toolStripMenuItem1";
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
            this.GroupBoxChangeGraph.ResumeLayout(false);
            this.GroupBoxChangeGraph.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBarElevation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBarPerspective)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBarRotation)).EndInit();
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
        private System.Windows.Forms.TrackBar TrackBarRotation;
        private System.Windows.Forms.Label LabelRotation;
        private System.Windows.Forms.Label LabelPerspective;
        private System.Windows.Forms.TrackBar TrackBarPerspective;
        private System.Windows.Forms.Label LabelElevation;
        private System.Windows.Forms.TrackBar TrackBarElevation;
    }
}
