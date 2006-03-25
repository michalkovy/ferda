// DatabaseInfo.cs - GUI part
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

namespace Ferda.FrontEnd.AddIns.DatabaseInfo
{
    partial class DataBaseInfo
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
            this.DataBaseInfoListView = new System.Windows.Forms.ListView();
            this.TableName = new System.Windows.Forms.ColumnHeader();
            this.TableRemarks = new System.Windows.Forms.ColumnHeader();
            this.TableRowCount = new System.Windows.Forms.ColumnHeader();
            this.TableType = new System.Windows.Forms.ColumnHeader();
            this.ContextMenuData = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItemCopyAll = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemCopySelected = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextMenuData.SuspendLayout();
            this.SuspendLayout();
            // 
            // DataBaseInfoListView
            // 
            this.DataBaseInfoListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.TableName,
            this.TableRemarks,
            this.TableRowCount,
            this.TableType});
            this.DataBaseInfoListView.ContextMenuStrip = this.ContextMenuData;
            this.DataBaseInfoListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataBaseInfoListView.FullRowSelect = true;
            this.DataBaseInfoListView.GridLines = true;
            this.DataBaseInfoListView.Location = new System.Drawing.Point(0, 0);
            this.DataBaseInfoListView.Name = "DataBaseInfoListView";
            this.DataBaseInfoListView.Size = new System.Drawing.Size(467, 262);
            this.DataBaseInfoListView.TabIndex = 0;
            this.DataBaseInfoListView.UseCompatibleStateImageBehavior = false;
            this.DataBaseInfoListView.View = System.Windows.Forms.View.Details;
            // 
            // TableName
            // 
            this.TableName.Width = 106;
            // 
            // TableRemarks
            // 
            this.TableRemarks.Width = 109;
            // 
            // TableRowCount
            // 
            this.TableRowCount.Width = 93;
            // 
            // TableType
            // 
            this.TableType.Width = 87;
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
            // DataBaseInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.DataBaseInfoListView);
            this.Name = "DataBaseInfo";
            this.Size = new System.Drawing.Size(467, 262);
            this.ContextMenuData.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView DataBaseInfoListView;
        private System.Windows.Forms.ColumnHeader TableName;
        private System.Windows.Forms.ColumnHeader TableRemarks;
        private System.Windows.Forms.ColumnHeader TableRowCount;
        private System.Windows.Forms.ColumnHeader TableType;
        private System.Windows.Forms.ContextMenuStrip ContextMenuData;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemCopyAll;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemCopySelected;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripHelp;
    }
}