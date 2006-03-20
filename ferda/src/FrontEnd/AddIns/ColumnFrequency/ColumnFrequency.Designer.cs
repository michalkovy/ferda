// ColumnFrequency.cs - GUI part
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

namespace Ferda.FrontEnd.AddIns.ColumnFrequency
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
            this.ContextMenuDataTab = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItemCopyAll = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemCopySelected = new System.Windows.Forms.ToolStripMenuItem();
            this.TabPageAreaChart = new System.Windows.Forms.TabPage();
            this.ContextMenuGraph = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItemAbsolute = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripMenuToggleMarks = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemCopyChart = new System.Windows.Forms.ToolStripMenuItem();
            this.TabPageBarChart = new System.Windows.Forms.TabPage();
            this.TabPagePieChart = new System.Windows.Forms.TabPage();
            this.TabControlColumnFrequency.SuspendLayout();
            this.TabPageText.SuspendLayout();
            this.ContextMenuDataTab.SuspendLayout();
            this.ContextMenuGraph.SuspendLayout();
            this.SuspendLayout();
            // 
            // TabControlColumnFrequency
            // 
            this.TabControlColumnFrequency.Controls.Add(this.TabPageText);
            this.TabControlColumnFrequency.Controls.Add(this.TabPageAreaChart);
            this.TabControlColumnFrequency.Controls.Add(this.TabPageBarChart);
            this.TabControlColumnFrequency.Controls.Add(this.TabPagePieChart);
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
            this.ColumnFrListView.ContextMenuStrip = this.ContextMenuDataTab;
            this.ColumnFrListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ColumnFrListView.FullRowSelect = true;
            this.ColumnFrListView.GridLines = true;
            this.ColumnFrListView.Location = new System.Drawing.Point(3, 3);
            this.ColumnFrListView.Name = "ColumnFrListView";
            this.ColumnFrListView.ShowItemToolTips = true;
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
            // ContextMenuDataTab
            // 
            this.ContextMenuDataTab.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemCopyAll,
            this.ToolStripMenuItemCopySelected});
            this.ContextMenuDataTab.Name = "ContextMenuDataTab";
            this.ContextMenuDataTab.Size = new System.Drawing.Size(180, 48);
            // 
            // ToolStripMenuItemCopyAll
            // 
            this.ToolStripMenuItemCopyAll.Name = "ToolStripMenuItemCopyAll";
            this.ToolStripMenuItemCopyAll.Size = new System.Drawing.Size(179, 22);
            this.ToolStripMenuItemCopyAll.Text = "toolStripMenuItem1";
            // 
            // ToolStripMenuItemCopySelected
            // 
            this.ToolStripMenuItemCopySelected.Name = "ToolStripMenuItemCopySelected";
            this.ToolStripMenuItemCopySelected.Size = new System.Drawing.Size(179, 22);
            this.ToolStripMenuItemCopySelected.Text = "toolStripMenuItem1";
            // 
            // TabPageAreaChart
            // 
            this.TabPageAreaChart.ContextMenuStrip = this.ContextMenuGraph;
            this.TabPageAreaChart.Location = new System.Drawing.Point(4, 22);
            this.TabPageAreaChart.Name = "TabPageAreaChart";
            this.TabPageAreaChart.Size = new System.Drawing.Size(459, 236);
            this.TabPageAreaChart.TabIndex = 3;
            this.TabPageAreaChart.Text = "tabPage1";
            this.TabPageAreaChart.UseVisualStyleBackColor = true;
            // 
            // ContextMenuGraph
            // 
            this.ContextMenuGraph.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemAbsolute,
            this.toolStripSeparator1,
            this.ToolStripMenuToggleMarks,
            this.ToolStripMenuItemCopyChart});
            this.ContextMenuGraph.Name = "ContextMenuGraphParams";
            this.ContextMenuGraph.Size = new System.Drawing.Size(214, 98);
            // 
            // ToolStripMenuItemAbsolute
            // 
            this.ToolStripMenuItemAbsolute.CheckOnClick = true;
            this.ToolStripMenuItemAbsolute.Name = "ToolStripMenuItemAbsolute";
            this.ToolStripMenuItemAbsolute.Size = new System.Drawing.Size(213, 22);
            this.ToolStripMenuItemAbsolute.Text = "Absolute";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(210, 6);
            // 
            // ToolStripMenuToggleMarks
            // 
            this.ToolStripMenuToggleMarks.CheckOnClick = true;
            this.ToolStripMenuToggleMarks.Name = "ToolStripMenuToggleMarks";
            this.ToolStripMenuToggleMarks.Size = new System.Drawing.Size(213, 22);
            this.ToolStripMenuToggleMarks.Text = "ToolStripMenuToggleMarks";
            this.ToolStripMenuToggleMarks.CheckedChanged += new System.EventHandler(this.ToolStripMenuToggleMarks_CheckChanged);
            // 
            // ToolStripMenuItemCopyChart
            // 
            this.ToolStripMenuItemCopyChart.Name = "ToolStripMenuItemCopyChart";
            this.ToolStripMenuItemCopyChart.Size = new System.Drawing.Size(213, 22);
            this.ToolStripMenuItemCopyChart.Text = "toolStripMenuItem1";
            // 
            // TabPageBarChart
            // 
            this.TabPageBarChart.ContextMenuStrip = this.ContextMenuGraph;
            this.TabPageBarChart.Location = new System.Drawing.Point(4, 22);
            this.TabPageBarChart.Name = "TabPageBarChart";
            this.TabPageBarChart.Padding = new System.Windows.Forms.Padding(3);
            this.TabPageBarChart.Size = new System.Drawing.Size(459, 236);
            this.TabPageBarChart.TabIndex = 1;
            this.TabPageBarChart.Text = "Chart";
            this.TabPageBarChart.UseVisualStyleBackColor = true;
            // 
            // TabPagePieChart
            // 
            this.TabPagePieChart.ContextMenuStrip = this.ContextMenuGraph;
            this.TabPagePieChart.Location = new System.Drawing.Point(4, 22);
            this.TabPagePieChart.Name = "TabPagePieChart";
            this.TabPagePieChart.Padding = new System.Windows.Forms.Padding(3);
            this.TabPagePieChart.Size = new System.Drawing.Size(459, 236);
            this.TabPagePieChart.TabIndex = 2;
            this.TabPagePieChart.Text = "tabPage1";
            this.TabPagePieChart.UseVisualStyleBackColor = true;
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
            this.ContextMenuDataTab.ResumeLayout(false);
            this.ContextMenuGraph.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl TabControlColumnFrequency;
        private System.Windows.Forms.TabPage TabPageText;
        private System.Windows.Forms.ListView ColumnFrListView;
        private System.Windows.Forms.ColumnHeader ValuesColumn;
        private System.Windows.Forms.ColumnHeader FrequencyColumn;
        private System.Windows.Forms.ColumnHeader PercentageColumn;
        private System.Windows.Forms.TabPage TabPageBarChart;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemAbsolute;
        private System.Windows.Forms.ContextMenuStrip ContextMenuGraph;
        private System.Windows.Forms.TabPage TabPagePieChart;
        private System.Windows.Forms.TabPage TabPageAreaChart;
        private System.Windows.Forms.ContextMenuStrip ContextMenuDataTab;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemCopyAll;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemCopySelected;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemCopyChart;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuToggleMarks;
    }
}
