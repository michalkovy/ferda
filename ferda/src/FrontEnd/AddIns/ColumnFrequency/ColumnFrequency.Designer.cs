namespace Ferda
{
    namespace FrontEnd.AddIns.ColumnFrequency
    {
        partial class ColumnFrequency
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
                this.TabControlColumnFrequency = new System.Windows.Forms.TabControl();
                this.TabPageText = new System.Windows.Forms.TabPage();
                this.ColumnFrListView = new System.Windows.Forms.ListView();
                this.ValuesColumn = new System.Windows.Forms.ColumnHeader();
                this.FrequencyColumn = new System.Windows.Forms.ColumnHeader();
                this.PercentageColumn = new System.Windows.Forms.ColumnHeader();
                this.TabPageGraph = new System.Windows.Forms.TabPage();
                this.ContextMenuGraphParams = new System.Windows.Forms.ContextMenuStrip(this.components);
                this.ToolStripMenuItemAbsolute = new System.Windows.Forms.ToolStripMenuItem();
                this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
                this.ToolStripMenuItemPie = new System.Windows.Forms.ToolStripMenuItem();
                this.TabControlColumnFrequency.SuspendLayout();
                this.TabPageText.SuspendLayout();
                this.ContextMenuGraphParams.SuspendLayout();
                this.SuspendLayout();
                // 
                // TabControlColumnFrequency
                // 
                this.TabControlColumnFrequency.Controls.Add(this.TabPageText);
                this.TabControlColumnFrequency.Controls.Add(this.TabPageGraph);
                this.TabControlColumnFrequency.Dock = System.Windows.Forms.DockStyle.Fill;
                this.TabControlColumnFrequency.Location = new System.Drawing.Point(0, 0);
                this.TabControlColumnFrequency.Name = "TabControlColumnFrequency";
                this.TabControlColumnFrequency.SelectedIndex = 0;
                this.TabControlColumnFrequency.Size = new System.Drawing.Size(467, 262);
                this.TabControlColumnFrequency.TabIndex = 0;
                // 
                // TabPageText
                // 
                this.TabPageText.Controls.Add(this.ColumnFrListView);
                this.TabPageText.Location = new System.Drawing.Point(4, 22);
                this.TabPageText.Name = "TabPageText";
                this.TabPageText.Padding = new System.Windows.Forms.Padding(3);
                this.TabPageText.Size = new System.Drawing.Size(459, 236);
                this.TabPageText.TabIndex = 0;
                this.TabPageText.Text = "Data";
                this.TabPageText.UseVisualStyleBackColor = true;
                // 
                // ColumnFrListView
                // 
                this.ColumnFrListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ValuesColumn,
            this.FrequencyColumn,
            this.PercentageColumn});
                this.ColumnFrListView.Dock = System.Windows.Forms.DockStyle.Fill;
                this.ColumnFrListView.FullRowSelect = true;
                this.ColumnFrListView.GridLines = true;
                this.ColumnFrListView.Location = new System.Drawing.Point(3, 3);
                this.ColumnFrListView.Name = "ColumnFrListView";
                this.ColumnFrListView.Size = new System.Drawing.Size(453, 230);
                this.ColumnFrListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
                this.ColumnFrListView.TabIndex = 2;
                this.ColumnFrListView.UseCompatibleStateImageBehavior = false;
                this.ColumnFrListView.View = System.Windows.Forms.View.Details;
                // 
                // ValuesColumn
                // 
                this.ValuesColumn.Width = 154;
                // 
                // FrequencyColumn
                // 
                this.FrequencyColumn.Width = 98;
                // 
                // PercentageColumn
                // 
                this.PercentageColumn.Width = 90;
                // 
                // TabPageGraph
                // 
                this.TabPageGraph.ContextMenuStrip = this.ContextMenuGraphParams;
                this.TabPageGraph.Location = new System.Drawing.Point(4, 22);
                this.TabPageGraph.Name = "TabPageGraph";
                this.TabPageGraph.Padding = new System.Windows.Forms.Padding(3);
                this.TabPageGraph.Size = new System.Drawing.Size(459, 236);
                this.TabPageGraph.TabIndex = 1;
                this.TabPageGraph.Text = "Chart";
                this.TabPageGraph.UseVisualStyleBackColor = true;
                // 
                // ContextMenuGraphParams
                // 
                this.ContextMenuGraphParams.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemAbsolute,
            this.toolStripSeparator1,
            this.ToolStripMenuItemPie});
                this.ContextMenuGraphParams.Name = "ContextMenuGraphParams";
                this.ContextMenuGraphParams.Size = new System.Drawing.Size(128, 54);
                // 
                // ToolStripMenuItemAbsolute
                // 
                this.ToolStripMenuItemAbsolute.CheckOnClick = true;
                this.ToolStripMenuItemAbsolute.Name = "ToolStripMenuItemAbsolute";
                this.ToolStripMenuItemAbsolute.Size = new System.Drawing.Size(127, 22);
                this.ToolStripMenuItemAbsolute.Text = "Absolute";
                // 
                // toolStripSeparator1
                // 
                this.toolStripSeparator1.Name = "toolStripSeparator1";
                this.toolStripSeparator1.Size = new System.Drawing.Size(124, 6);
                // 
                // ToolStripMenuItemPie
                // 
                this.ToolStripMenuItemPie.CheckOnClick = true;
                this.ToolStripMenuItemPie.Name = "ToolStripMenuItemPie";
                this.ToolStripMenuItemPie.Size = new System.Drawing.Size(127, 22);
                this.ToolStripMenuItemPie.Text = "Pie";
                // 
                // ColumnFrequency
                // 
                this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.TabControlColumnFrequency);
                this.Name = "ColumnFrequency";
                this.Size = new System.Drawing.Size(467, 262);
                this.TabControlColumnFrequency.ResumeLayout(false);
                this.TabPageText.ResumeLayout(false);
                this.ContextMenuGraphParams.ResumeLayout(false);
                this.ResumeLayout(false);

            }

            #endregion

            private System.Windows.Forms.TabControl TabControlColumnFrequency;
            private System.Windows.Forms.TabPage TabPageText;
            private System.Windows.Forms.ListView ColumnFrListView;
            private System.Windows.Forms.ColumnHeader ValuesColumn;
            private System.Windows.Forms.ColumnHeader FrequencyColumn;
            private System.Windows.Forms.ColumnHeader PercentageColumn;
            private System.Windows.Forms.TabPage TabPageGraph;
            private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemAbsolute;
            private System.Windows.Forms.ContextMenuStrip ContextMenuGraphParams;
            private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
            private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemPie;


        }
    }
}
