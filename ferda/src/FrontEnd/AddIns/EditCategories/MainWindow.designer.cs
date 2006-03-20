// MainWindow.cs - GUI part
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

namespace Ferda.FrontEnd.AddIns.EditCategories
{
    partial class MainListView
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.CategoriesListView = new System.Windows.Forms.ListView();
            this.ColumnCategoryName = new System.Windows.Forms.ColumnHeader();
            this.ColumnCategoryType = new System.Windows.Forms.ColumnHeader();
            this.ColumnCategoryValue = new System.Windows.Forms.ColumnHeader();
            this.ColumnFrequency = new System.Windows.Forms.ColumnHeader();
            this.CategoriesContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuItemNew = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemNewEnum = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemNewInterval = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemRename = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItemJoin = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItemDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItemSaveAndQuit = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemQuitWithoutSave = new System.Windows.Forms.ToolStripMenuItem();
            this.CategoriesToolStrip = new System.Windows.Forms.ToolStrip();
            this.ButtonSaveAndQuit = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ButtonQuitWithoutSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.ButtonNew = new System.Windows.Forms.ToolStripSplitButton();
            this.ToolMenuItemNewEnumeration = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolMenuItemNewInterval = new System.Windows.Forms.ToolStripMenuItem();
            this.ButtonEdit = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.ButtonJoin = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.ButtonDelete = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.CategoriesContextMenu.SuspendLayout();
            this.CategoriesToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.CategoriesListView);
            this.splitContainer1.Panel1.Controls.Add(this.CategoriesToolStrip);
            this.splitContainer1.Panel1MinSize = 100;
            this.splitContainer1.Panel2Collapsed = true;
            this.splitContainer1.Size = new System.Drawing.Size(846, 558);
            this.splitContainer1.SplitterDistance = 550;
            this.splitContainer1.TabIndex = 0;
            this.splitContainer1.Text = "splitContainer1";
            // 
            // CategoriesListView
            // 
            this.CategoriesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnCategoryName,
            this.ColumnCategoryType,
            this.ColumnCategoryValue,
            this.ColumnFrequency});
            this.CategoriesListView.ContextMenuStrip = this.CategoriesContextMenu;
            this.CategoriesListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CategoriesListView.FullRowSelect = true;
            this.CategoriesListView.GridLines = true;
            this.CategoriesListView.LabelEdit = true;
            this.CategoriesListView.Location = new System.Drawing.Point(0, 25);
            this.CategoriesListView.Name = "CategoriesListView";
            this.CategoriesListView.Size = new System.Drawing.Size(846, 533);
            this.CategoriesListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.CategoriesListView.TabIndex = 17;
            this.CategoriesListView.UseCompatibleStateImageBehavior = false;
            this.CategoriesListView.View = System.Windows.Forms.View.Details;
            // 
            // ColumnCategoryName
            // 
            this.ColumnCategoryName.Width = 178;
            // 
            // ColumnCategoryType
            // 
            this.ColumnCategoryType.Width = 99;
            // 
            // ColumnCategoryValue
            // 
            this.ColumnCategoryValue.Width = 229;
            // 
            // CategoriesContextMenu
            // 
            this.CategoriesContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemNew,
            this.MenuItemEdit,
            this.MenuItemRename,
            this.toolStripSeparator8,
            this.MenuItemJoin,
            this.toolStripSeparator9,
            this.MenuItemDelete,
            this.toolStripSeparator10,
            this.MenuItemSaveAndQuit,
            this.MenuItemQuitWithoutSave});
            this.CategoriesContextMenu.Name = "NewCategoryContextMenu";
            this.CategoriesContextMenu.Size = new System.Drawing.Size(220, 176);
            // 
            // MenuItemNew
            // 
            this.MenuItemNew.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemNewEnum,
            this.MenuItemNewInterval});
            this.MenuItemNew.Name = "MenuItemNew";
            this.MenuItemNew.Size = new System.Drawing.Size(219, 22);
            this.MenuItemNew.Text = "toolStripMenuItem1";
            // 
            // MenuItemNewEnum
            // 
            this.MenuItemNewEnum.Name = "MenuItemNewEnum";
            this.MenuItemNewEnum.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.MenuItemNewEnum.Size = new System.Drawing.Size(217, 22);
            this.MenuItemNewEnum.Text = "toolStripMenuItem1";
            // 
            // MenuItemNewInterval
            // 
            this.MenuItemNewInterval.Name = "MenuItemNewInterval";
            this.MenuItemNewInterval.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.MenuItemNewInterval.Size = new System.Drawing.Size(217, 22);
            this.MenuItemNewInterval.Text = "toolStripMenuItem1";
            // 
            // MenuItemEdit
            // 
            this.MenuItemEdit.Enabled = false;
            this.MenuItemEdit.Name = "MenuItemEdit";
            this.MenuItemEdit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.MenuItemEdit.Size = new System.Drawing.Size(219, 22);
            this.MenuItemEdit.Text = "toolStripMenuItem1";
            // 
            // MenuItemRename
            // 
            this.MenuItemRename.Enabled = false;
            this.MenuItemRename.Name = "MenuItemRename";
            this.MenuItemRename.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.MenuItemRename.Size = new System.Drawing.Size(219, 22);
            this.MenuItemRename.Text = "toolStripMenuItem1";
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(216, 6);
            // 
            // MenuItemJoin
            // 
            this.MenuItemJoin.Enabled = false;
            this.MenuItemJoin.Name = "MenuItemJoin";
            this.MenuItemJoin.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.J)));
            this.MenuItemJoin.Size = new System.Drawing.Size(219, 22);
            this.MenuItemJoin.Text = "toolStripMenuItem1";
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(216, 6);
            // 
            // MenuItemDelete
            // 
            this.MenuItemDelete.Enabled = false;
            this.MenuItemDelete.Name = "MenuItemDelete";
            this.MenuItemDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.MenuItemDelete.Size = new System.Drawing.Size(219, 22);
            this.MenuItemDelete.Text = "toolStripMenuItem1";
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(216, 6);
            // 
            // MenuItemSaveAndQuit
            // 
            this.MenuItemSaveAndQuit.Name = "MenuItemSaveAndQuit";
            this.MenuItemSaveAndQuit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.MenuItemSaveAndQuit.Size = new System.Drawing.Size(219, 22);
            this.MenuItemSaveAndQuit.Text = "toolStripMenuItem1";
            // 
            // MenuItemQuitWithoutSave
            // 
            this.MenuItemQuitWithoutSave.Name = "MenuItemQuitWithoutSave";
            this.MenuItemQuitWithoutSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.MenuItemQuitWithoutSave.Size = new System.Drawing.Size(219, 22);
            this.MenuItemQuitWithoutSave.Text = "toolStripMenuItem1";
            // 
            // CategoriesToolStrip
            // 
            this.CategoriesToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ButtonSaveAndQuit,
            this.toolStripSeparator1,
            this.ButtonQuitWithoutSave,
            this.toolStripSeparator5,
            this.ButtonNew,
            this.ButtonEdit,
            this.toolStripSeparator6,
            this.ButtonJoin,
            this.toolStripSeparator7,
            this.ButtonDelete});
            this.CategoriesToolStrip.Location = new System.Drawing.Point(0, 0);
            this.CategoriesToolStrip.Name = "CategoriesToolStrip";
            this.CategoriesToolStrip.Size = new System.Drawing.Size(846, 25);
            this.CategoriesToolStrip.TabIndex = 16;
            this.CategoriesToolStrip.Text = "toolStrip1";
            // 
            // ButtonSaveAndQuit
            // 
            this.ButtonSaveAndQuit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonSaveAndQuit.Name = "ButtonSaveAndQuit";
            this.ButtonSaveAndQuit.Size = new System.Drawing.Size(106, 22);
            this.ButtonSaveAndQuit.Text = "ButtonSaveAndQuit";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // ButtonQuitWithoutSave
            // 
            this.ButtonQuitWithoutSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonQuitWithoutSave.Name = "ButtonQuitWithoutSave";
            this.ButtonQuitWithoutSave.Size = new System.Drawing.Size(125, 22);
            this.ButtonQuitWithoutSave.Text = "ButtonQuitWithoutSave";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // ButtonNew
            // 
            this.ButtonNew.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolMenuItemNewEnumeration,
            this.ToolMenuItemNewInterval});
            this.ButtonNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonNew.Name = "ButtonNew";
            this.ButtonNew.Size = new System.Drawing.Size(76, 22);
            this.ButtonNew.Text = "ButtonNew";
            // 
            // ToolMenuItemNewEnumeration
            // 
            this.ToolMenuItemNewEnumeration.Name = "ToolMenuItemNewEnumeration";
            this.ToolMenuItemNewEnumeration.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.ToolMenuItemNewEnumeration.Size = new System.Drawing.Size(272, 22);
            this.ToolMenuItemNewEnumeration.Text = "ToolMenuItemNewEnumeration";
            // 
            // ToolMenuItemNewInterval
            // 
            this.ToolMenuItemNewInterval.Name = "ToolMenuItemNewInterval";
            this.ToolMenuItemNewInterval.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.ToolMenuItemNewInterval.Size = new System.Drawing.Size(272, 22);
            this.ToolMenuItemNewInterval.Text = "ToolMenuItemNewInterval";
            // 
            // ButtonEdit
            // 
            this.ButtonEdit.Enabled = false;
            this.ButtonEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonEdit.Name = "ButtonEdit";
            this.ButtonEdit.Size = new System.Drawing.Size(61, 22);
            this.ButtonEdit.Text = "ButtonEdit";
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // ButtonJoin
            // 
            this.ButtonJoin.Enabled = false;
            this.ButtonJoin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonJoin.Name = "ButtonJoin";
            this.ButtonJoin.Size = new System.Drawing.Size(62, 22);
            this.ButtonJoin.Text = "ButtonJoin";
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // ButtonDelete
            // 
            this.ButtonDelete.Enabled = false;
            this.ButtonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonDelete.Name = "ButtonDelete";
            this.ButtonDelete.Size = new System.Drawing.Size(74, 22);
            this.ButtonDelete.Text = "ButtonDelete";
            // 
            // MainListView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(846, 558);
            this.Controls.Add(this.splitContainer1);
            this.Name = "MainListView";
            this.Text = "Smart Datalist";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.CategoriesContextMenu.ResumeLayout(false);
            this.CategoriesToolStrip.ResumeLayout(false);
            this.CategoriesToolStrip.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ToolStripButton ButtonSaveAndQuit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton ButtonQuitWithoutSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem ToolMenuItemNewEnumeration;
        private System.Windows.Forms.ToolStripMenuItem ToolMenuItemNewInterval;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton ButtonJoin;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripButton ButtonDelete;
        private System.Windows.Forms.ToolStripMenuItem MenuItemNewEnum;
        private System.Windows.Forms.ToolStripMenuItem MenuItemNewInterval;
        private System.Windows.Forms.ToolStripMenuItem MenuItemJoin;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem MenuItemSaveAndQuit;
        private System.Windows.Forms.ToolStripMenuItem MenuItemQuitWithoutSave;
        internal System.Windows.Forms.ToolStrip CategoriesToolStrip;
        internal System.Windows.Forms.ListView CategoriesListView;
        internal System.Windows.Forms.ContextMenuStrip CategoriesContextMenu;
        private System.Windows.Forms.ToolStripMenuItem MenuItemDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ColumnHeader ColumnCategoryName;
        private System.Windows.Forms.ColumnHeader ColumnCategoryType;
        private System.Windows.Forms.ColumnHeader ColumnCategoryValue;
        private System.Windows.Forms.ColumnHeader ColumnFrequency;
        internal System.Windows.Forms.SplitContainer splitContainer1;
        internal System.Windows.Forms.ToolStripSplitButton ButtonNew;
        internal System.Windows.Forms.ToolStripMenuItem MenuItemNew;
        internal System.Windows.Forms.ToolStripMenuItem MenuItemEdit;
        internal System.Windows.Forms.ToolStripButton ButtonEdit;
        private System.Windows.Forms.ToolStripMenuItem MenuItemRename;
    }
}