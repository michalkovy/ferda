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

namespace Ferda.FrontEnd.AddIns.ExplainTable
{
    partial class ExplainTable
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
            this.ExplainTableListView = new System.Windows.Forms.ListView();
            this.ColumnName = new System.Windows.Forms.ColumnHeader();
            this.ColumnAllowDBNull = new System.Windows.Forms.ColumnHeader();
            this.ColumnOrdinal = new System.Windows.Forms.ColumnHeader();
            this.ColumnSize = new System.Windows.Forms.ColumnHeader();
            this.ColumnDataType = new System.Windows.Forms.ColumnHeader();
            this.ColumnAutoIncrement = new System.Windows.Forms.ColumnHeader();
            this.ColumnIsKey = new System.Windows.Forms.ColumnHeader();
            this.ColumnIsLong = new System.Windows.Forms.ColumnHeader();
            this.ColumnIsReadOnly = new System.Windows.Forms.ColumnHeader();
            this.ColumnIsRowVersion = new System.Windows.Forms.ColumnHeader();
            this.ColumnIsUnique = new System.Windows.Forms.ColumnHeader();
            this.ColumnNumericalPrecision = new System.Windows.Forms.ColumnHeader();
            this.ColumnNumericalScale = new System.Windows.Forms.ColumnHeader();
            this.ColumnProviderType = new System.Windows.Forms.ColumnHeader();
            this.ContextMenuData = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItemCopyAll = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemCopySelected = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextMenuData.SuspendLayout();
            this.SuspendLayout();
            // 
            // ExplainTableListView
            // 
            this.ExplainTableListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnName,
            this.ColumnAllowDBNull,
            this.ColumnOrdinal,
            this.ColumnSize,
            this.ColumnDataType,
            this.ColumnAutoIncrement,
            this.ColumnIsKey,
            this.ColumnIsLong,
            this.ColumnIsReadOnly,
            this.ColumnIsRowVersion,
            this.ColumnIsUnique,
            this.ColumnNumericalPrecision,
            this.ColumnNumericalScale,
            this.ColumnProviderType});
            this.ExplainTableListView.ContextMenuStrip = this.ContextMenuData;
            this.ExplainTableListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ExplainTableListView.FullRowSelect = true;
            this.ExplainTableListView.GridLines = true;
            this.ExplainTableListView.Location = new System.Drawing.Point(0, 0);
            this.ExplainTableListView.Name = "ExplainTableListView";
            this.ExplainTableListView.ShowItemToolTips = true;
            this.ExplainTableListView.Size = new System.Drawing.Size(730, 262);
            this.ExplainTableListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.ExplainTableListView.TabIndex = 0;
            this.ExplainTableListView.UseCompatibleStateImageBehavior = false;
            this.ExplainTableListView.View = System.Windows.Forms.View.Details;
            // 
            // ColumnName
            // 
            this.ColumnName.Width = 144;
            // 
            // ContextMenuData
            // 
            this.ContextMenuData.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemCopyAll,
            this.ToolStripMenuItemCopySelected,
            this.toolStripSeparator1,
            this.ToolStripHelp});
            this.ContextMenuData.Name = "ContextMenuData";
            this.ContextMenuData.Size = new System.Drawing.Size(180, 98);
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
            // ExplainTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ExplainTableListView);
            this.Name = "ExplainTable";
            this.Size = new System.Drawing.Size(730, 262);
            this.ContextMenuData.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView ExplainTableListView;
        private System.Windows.Forms.ColumnHeader ColumnName;
        private System.Windows.Forms.ColumnHeader ColumnOrdinal;
        private System.Windows.Forms.ColumnHeader ColumnSize;
        private System.Windows.Forms.ColumnHeader ColumnDataType;
        private System.Windows.Forms.ColumnHeader ColumnAutoIncrement;
        private System.Windows.Forms.ColumnHeader ColumnIsKey;
        private System.Windows.Forms.ColumnHeader ColumnIsLong;
        private System.Windows.Forms.ColumnHeader ColumnIsReadOnly;
        private System.Windows.Forms.ColumnHeader ColumnIsRowVersion;
        private System.Windows.Forms.ColumnHeader ColumnIsUnique;
        private System.Windows.Forms.ColumnHeader ColumnNumericalPrecision;
        private System.Windows.Forms.ColumnHeader ColumnProviderType;
        private System.Windows.Forms.ColumnHeader ColumnAllowDBNull;
        private System.Windows.Forms.ColumnHeader ColumnNumericalScale;
        private System.Windows.Forms.ContextMenuStrip ContextMenuData;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemCopyAll;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemCopySelected;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripHelp;
    }
}