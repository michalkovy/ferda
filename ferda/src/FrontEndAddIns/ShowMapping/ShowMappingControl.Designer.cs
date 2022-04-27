// ExplainTableControl.cs - GUI part
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

namespace Ferda.FrontEnd.AddIns.ShowMapping
{
    partial class ShowMappingControl
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
            this.ShowMappingListView = new System.Windows.Forms.ListView();
            this.ColumnDataTable = new System.Windows.Forms.ColumnHeader();
            this.ColumnColumn = new System.Windows.Forms.ColumnHeader();
            this.ColumnOntologyEntity = new System.Windows.Forms.ColumnHeader();
            this.ColumnAnnotations = new System.Windows.Forms.ColumnHeader();
            this.ColumnCardinality = new System.Windows.Forms.ColumnHeader();
            this.ColumnMinimum = new System.Windows.Forms.ColumnHeader();
            this.ColumnMaximum = new System.Windows.Forms.ColumnHeader();
            this.ColumnDomainDividingValues = new System.Windows.Forms.ColumnHeader();
            this.ColumnDistinctValues = new System.Windows.Forms.ColumnHeader();
            this.ContextMenuData = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItemCopyAll = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemCopySelected = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextMenuData.SuspendLayout();
            this.SuspendLayout();
            // 
            // ShowMappingListView
            // 
            this.ShowMappingListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnDataTable,
            this.ColumnColumn,
            this.ColumnOntologyEntity,
            this.ColumnCardinality,
            this.ColumnMinimum,
            this.ColumnMaximum,
            this.ColumnDomainDividingValues,
            this.ColumnDistinctValues,
            this.ColumnAnnotations});
            this.ShowMappingListView.ContextMenuStrip = this.ContextMenuData;
            this.ShowMappingListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ShowMappingListView.FullRowSelect = true;
            this.ShowMappingListView.GridLines = true;
            this.ShowMappingListView.Location = new System.Drawing.Point(0, 0);
            this.ShowMappingListView.Name = "ShowMappingListView";
            this.ShowMappingListView.ShowItemToolTips = true;
            this.ShowMappingListView.Size = new System.Drawing.Size(730, 262);
            this.ShowMappingListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.ShowMappingListView.TabIndex = 0;
            this.ShowMappingListView.UseCompatibleStateImageBehavior = false;
            this.ShowMappingListView.View = System.Windows.Forms.View.Details;
            // 
            // ColumnDataTable
            // 
            this.ColumnDataTable.Width = 100;
            // 
            // ColumnColumn
            // 
            this.ColumnColumn.Width = 100;
            // 
            // ColumnOntologyEntity
            // 
            this.ColumnOntologyEntity.Width = 100;
            // 
            // ColumnAnnotations
            // 
            this.ColumnAnnotations.Width = 160;
            // 
            // ColumnCardinality
            // 
            this.ColumnCardinality.DisplayIndex = 3;
            // 
            // ColumnMinimum
            // 
            this.ColumnMinimum.DisplayIndex = 4;
            // 
            // ColumnMaximum
            // 
            this.ColumnMaximum.DisplayIndex = 5;
            // 
            // ColumnDomainDividingValues
            // 
            this.ColumnDomainDividingValues.DisplayIndex = 6;
            this.ColumnDomainDividingValues.Width = 100;
            // 
            // ColumnDistinctValues
            // 
            this.ColumnDistinctValues.DisplayIndex = 7;
            this.ColumnDistinctValues.Width = 100;
            // 
            // ContextMenuData
            // 
            this.ContextMenuData.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemCopyAll,
            this.ToolStripMenuItemCopySelected,
            this.toolStripSeparator1,
            this.ToolStripHelp});
            this.ContextMenuData.Name = "ContextMenuData";
            this.ContextMenuData.Size = new System.Drawing.Size(180, 76);
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
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(176, 6);
            // 
            // ToolStripHelp
            // 
            this.ToolStripHelp.Name = "ToolStripHelp";
            this.ToolStripHelp.Size = new System.Drawing.Size(179, 22);
            this.ToolStripHelp.Text = "toolStripMenuItem1";
            this.ToolStripHelp.Click += new System.EventHandler(this.ToolStripHelp_Click);
            // 
            // ShowMappingControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ShowMappingListView);
            this.Name = "ShowMappingControl";
            this.Size = new System.Drawing.Size(730, 262);
            this.ContextMenuData.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView ShowMappingListView;
        private System.Windows.Forms.ColumnHeader ColumnDataTable;
        private System.Windows.Forms.ColumnHeader ColumnOntologyEntity;
        private System.Windows.Forms.ColumnHeader ColumnCardinality;
        private System.Windows.Forms.ColumnHeader ColumnColumn;
        private System.Windows.Forms.ContextMenuStrip ContextMenuData;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemCopyAll;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemCopySelected;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripHelp;
        private System.Windows.Forms.ColumnHeader ColumnMinimum;
        private System.Windows.Forms.ColumnHeader ColumnMaximum;
        private System.Windows.Forms.ColumnHeader ColumnDomainDividingValues;
        private System.Windows.Forms.ColumnHeader ColumnDistinctValues;
        private System.Windows.Forms.ColumnHeader ColumnAnnotations;
    }
}
