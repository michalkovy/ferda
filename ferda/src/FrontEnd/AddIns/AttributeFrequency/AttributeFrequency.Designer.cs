namespace Ferda
{
    namespace FrontEnd.AddIns.AttributeFrequency
    {
        partial class AttributeFrequency
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
                this.TabControlAttributeFrequency = new System.Windows.Forms.TabControl();
                this.TabPageText = new System.Windows.Forms.TabPage();
                this.AttributeFrListView = new System.Windows.Forms.ListView();
                this.ValuesColumn = new System.Windows.Forms.ColumnHeader();
                this.FrequencyColumn = new System.Windows.Forms.ColumnHeader();
                this.PercentageColumn = new System.Windows.Forms.ColumnHeader();
                this.TabPageAreaChart = new System.Windows.Forms.TabPage();
                this.TabPagePieChart = new System.Windows.Forms.TabPage();
                this.TabPageBarChart = new System.Windows.Forms.TabPage();
                this.ContextMenuGraphParams = new System.Windows.Forms.ContextMenuStrip(this.components);
                this.ToolStripMenuItemAbsolute = new System.Windows.Forms.ToolStripMenuItem();
                this.TabControlAttributeFrequency.SuspendLayout();
                this.TabPageText.SuspendLayout();
                this.ContextMenuGraphParams.SuspendLayout();
                this.SuspendLayout();
                // 
                // TabControlAttributeFrequency
                // 
                this.TabControlAttributeFrequency.Controls.Add(this.TabPageText);
                this.TabControlAttributeFrequency.Controls.Add(this.TabPageAreaChart);
                this.TabControlAttributeFrequency.Controls.Add(this.TabPageBarChart);
                this.TabControlAttributeFrequency.Controls.Add(this.TabPagePieChart);
                this.TabControlAttributeFrequency.Dock = System.Windows.Forms.DockStyle.Fill;
                this.TabControlAttributeFrequency.Location = new System.Drawing.Point(0, 0);
                this.TabControlAttributeFrequency.Name = "TabControlAttributeFrequency";
                this.TabControlAttributeFrequency.SelectedIndex = 0;
                this.TabControlAttributeFrequency.Size = new System.Drawing.Size(467, 262);
                this.TabControlAttributeFrequency.TabIndex = 0;
                // 
                // TabPageText
                // 
                this.TabPageText.Controls.Add(this.AttributeFrListView);
                this.TabPageText.Location = new System.Drawing.Point(4, 22);
                this.TabPageText.Name = "TabPageText";
                this.TabPageText.Padding = new System.Windows.Forms.Padding(3);
                this.TabPageText.Size = new System.Drawing.Size(459, 236);
                this.TabPageText.TabIndex = 0;
                this.TabPageText.Text = "Data";
                this.TabPageText.UseVisualStyleBackColor = true;
                // 
                // AttributeFrListView
                // 
                this.AttributeFrListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ValuesColumn,
            this.FrequencyColumn,
            this.PercentageColumn});
                this.AttributeFrListView.Dock = System.Windows.Forms.DockStyle.Fill;
                this.AttributeFrListView.FullRowSelect = true;
                this.AttributeFrListView.GridLines = true;
                this.AttributeFrListView.Location = new System.Drawing.Point(3, 3);
                this.AttributeFrListView.Name = "AttributeFrListView";
                this.AttributeFrListView.Size = new System.Drawing.Size(453, 230);
                this.AttributeFrListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
                this.AttributeFrListView.TabIndex = 2;
                this.AttributeFrListView.UseCompatibleStateImageBehavior = false;
                this.AttributeFrListView.View = System.Windows.Forms.View.Details;
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
                // TabPageAreaChart
                // 
                this.TabPageAreaChart.ContextMenuStrip = this.ContextMenuGraphParams;
                this.TabPageAreaChart.Location = new System.Drawing.Point(4, 22);
                this.TabPageAreaChart.Name = "TabPageAreaChart";
                this.TabPageAreaChart.Size = new System.Drawing.Size(459, 236);
                this.TabPageAreaChart.TabIndex = 2;
                this.TabPageAreaChart.Text = "tabPage1";
                this.TabPageAreaChart.UseVisualStyleBackColor = true;
                // 
                // TabPagePieChart
                // 
                this.TabPagePieChart.ContextMenuStrip = this.ContextMenuGraphParams;
                this.TabPagePieChart.Location = new System.Drawing.Point(4, 22);
                this.TabPagePieChart.Name = "TabPagePieChart";
                this.TabPagePieChart.Size = new System.Drawing.Size(459, 236);
                this.TabPagePieChart.TabIndex = 3;
                this.TabPagePieChart.Text = "tabPage1";
                this.TabPagePieChart.UseVisualStyleBackColor = true;
                // 
                // TabPageBarChart
                // 
                this.TabPageBarChart.ContextMenuStrip = this.ContextMenuGraphParams;
                this.TabPageBarChart.Location = new System.Drawing.Point(4, 22);
                this.TabPageBarChart.Name = "TabPageBarChart";
                this.TabPageBarChart.Padding = new System.Windows.Forms.Padding(3);
                this.TabPageBarChart.Size = new System.Drawing.Size(459, 236);
                this.TabPageBarChart.TabIndex = 1;
                this.TabPageBarChart.Text = "Chart";
                this.TabPageBarChart.UseVisualStyleBackColor = true;
                // 
                // ContextMenuGraphParams
                // 
                this.ContextMenuGraphParams.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemAbsolute});
                this.ContextMenuGraphParams.Name = "ContextMenuGraphParams";
                this.ContextMenuGraphParams.Size = new System.Drawing.Size(128, 26);
                // 
                // ToolStripMenuItemAbsolute
                // 
                this.ToolStripMenuItemAbsolute.CheckOnClick = true;
                this.ToolStripMenuItemAbsolute.Name = "ToolStripMenuItemAbsolute";
                this.ToolStripMenuItemAbsolute.Size = new System.Drawing.Size(127, 22);
                this.ToolStripMenuItemAbsolute.Text = "Absolute";
                // 
                // AttributeFrequency
                // 
                this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.TabControlAttributeFrequency);
                this.Name = "AttributeFrequency";
                this.Size = new System.Drawing.Size(467, 262);
                this.TabControlAttributeFrequency.ResumeLayout(false);
                this.TabPageText.ResumeLayout(false);
                this.ContextMenuGraphParams.ResumeLayout(false);
                this.ResumeLayout(false);

            }

            #endregion

            private System.Windows.Forms.TabControl TabControlAttributeFrequency;
            private System.Windows.Forms.TabPage TabPageText;
            private System.Windows.Forms.ListView AttributeFrListView;
            private System.Windows.Forms.ColumnHeader ValuesColumn;
            private System.Windows.Forms.ColumnHeader FrequencyColumn;
            private System.Windows.Forms.ColumnHeader PercentageColumn;
            private System.Windows.Forms.TabPage TabPageBarChart;
            private System.Windows.Forms.ContextMenuStrip ContextMenuGraphParams;

            private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemAbsolute;
            private System.Windows.Forms.TabPage TabPageAreaChart;
            private System.Windows.Forms.TabPage TabPagePieChart;


        }
    }
}