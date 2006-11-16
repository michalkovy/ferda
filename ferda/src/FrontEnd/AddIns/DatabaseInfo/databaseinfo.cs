// DatabaseInfo.cs - UserControl class for displaying info about a database
//
// Authors: Alexander Kuzmin <alexander.kuzmin@gmail.com>
//          Martin Ralbovský <martin.ralbovsky@gmail.com>            
//
// Copyright (c) 2005 Alexander Kuzmin, Martin Ralbovsky
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using System.Reflection;

using Ferda.FrontEnd.AddIns;
using Ferda.FrontEnd.AddIns.Common.ListView;
using Ferda.Guha.Data;

namespace Ferda.FrontEnd.AddIns.DatabaseInfo
{
    /// <summary>
    /// Usercontrol class for the module for interaction displaying info about
    /// a database
    /// </summary>
    public partial class DataBaseInfo : UserControl
    {
        #region Private variables

        /// <summary>
        /// Localization resource manager
        /// </summary>
        private ResourceManager resManager;

        /// <summary>
        /// Owner of addin
        /// </summary>
        private IOwnerOfAddIn ownerOfAddIn;

        /// <summary>
        /// Listview items comparer
        /// </summary>
        private ListViewItemComparer comparer = new ListViewItemComparer();

        /// <summary>
        /// Array of datatable informations
        /// </summary>
        private DataTableExplain[] explainSeq;

        #endregion
        
        #region Constructor

        /// <summary>
        /// Default class constructor
        /// </summary>
        /// <param name="explainSeq">Array of information about data tables in
        /// the database</param>
        /// <param name="ownerOfAddIn">
        /// Owner of the addin (usually the FrontEnd environment)</param>
        /// <param name="resManager">Resource manager for the module</param>
        public DataBaseInfo(ResourceManager resManager, DataTableExplain[] explainSeq, 
            IOwnerOfAddIn ownerOfAddIn)
        {
            this.resManager = resManager;
            this.ownerOfAddIn = ownerOfAddIn;
            this.explainSeq = explainSeq;

            comparer.column = 0;
            InitializeComponent();
            this.ListViewInit();
            this.FillDBInfoListView();
            this.ToolStripMenuItemCopyAll.Click += new EventHandler(ToolStripMenuItemCopyAll_Click);
            this.ToolStripMenuItemCopySelected.Click += new EventHandler(ToolStripMenuItemCopySelected_Click);
        }

        #endregion

        #region Context menu handlers

        /// <summary>
        /// Copy all to clipboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ToolStripMenuItemCopyAll_Click(object sender, EventArgs e)
        {
            StringBuilder copyString = new StringBuilder();
            copyString.Append(this.resManager.GetString("TableName") + "\t" +
                this.resManager.GetString("Remarks") + "\t" +
                this.resManager.GetString("RowCound") + "\t" +
                this.resManager.GetString("Type"));
            copyString.AppendLine();

            foreach (ListViewItem item in this.DataBaseInfoListView.Items)
            {
                foreach (ListViewItem.ListViewSubItem subItem in item.SubItems)
                {
                    copyString.Append(subItem.Text + "\t");
                }

                //deleting last tab
                copyString.Remove(copyString.Length - 1, 1);
                copyString.AppendLine();
            }
            Clipboard.SetDataObject(copyString.ToString(), true);
        }

        /// <summary>
        /// Copy selected to clipboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ToolStripMenuItemCopySelected_Click(object sender, EventArgs e)
        {
            StringBuilder copyString = new StringBuilder();
            copyString.Append(this.resManager.GetString("TableName") + "\t" +
                this.resManager.GetString("Remarks") + "\t" +
                this.resManager.GetString("RowCound") + "\t" +
                this.resManager.GetString("Type"));
            copyString.AppendLine();

            foreach (ListViewItem item in this.DataBaseInfoListView.SelectedItems)
            {
                foreach (ListViewItem.ListViewSubItem subItem in item.SubItems)
                {
                    copyString.Append(subItem.Text + "\t");
                }

                //deleting last tab
                copyString.Remove(copyString.Length - 1, 1);
                copyString.AppendLine();
            }
            Clipboard.SetDataObject(copyString.ToString(), true);
        }

        /// <summary>
        /// Method for opening help
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripHelp_Click(object sender, EventArgs e)
        {
            this.ownerOfAddIn.OpenPdf(ownerOfAddIn.GetBinPath() + "\\AddIns\\Help\\DatabaseInfo.pdf");
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Method for ColumnFrListView init
        /// </summary>
        private void ListViewInit()
        {
            this.ChangeLocale();
            //adding a handling method for column sorting
            this.DataBaseInfoListView.ColumnClick += 
                new ColumnClickEventHandler(this.DataBaseInfoListView_ColumnClick);
        }

        /// <summary>
        /// Method to fill the listview with datamatrix info.
        /// </summary>
        private void FillDBInfoListView()
        {
            foreach (DataTableExplain explain in explainSeq)
            {
                ListViewItem newItem = new ListViewItem();
                newItem.Text = explain.name;
                newItem.SubItems.Add(explain.remarks);
                newItem.SubItems.Add(explain.recordsCount.ToString());
                newItem.SubItems.Add(explain.type);
                this.DataBaseInfoListView.Items.Add(newItem);
            }
        }

        /// <summary>
        /// Handler for column click - sorts a listview.
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        private void DataBaseInfoListView_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            comparer.column = e.Column;
            if (DataBaseInfoListView.Sorting == SortOrder.Ascending)
            {
                comparer.bAscending = false;
                DataBaseInfoListView.Sorting = SortOrder.Descending;
            }
            else
            {
                comparer.bAscending = true;
                DataBaseInfoListView.Sorting = SortOrder.Ascending;
            }

            DataBaseInfoListView.ListViewItemSorter = comparer;
        }


        #endregion

        #region Localization

        /// <summary>
        /// Method to change l10n.
        /// </summary>
        public void ChangeLocale()
        {
            this.TableName.Text = resManager.GetString("TableName");
            this.TableRemarks.Text = resManager.GetString("Remarks");
            this.TableRowCount.Text = resManager.GetString("RowCount");
            this.TableType.Text = resManager.GetString("Type");
            this.ToolStripMenuItemCopyAll.Text = 
                resManager.GetString("CopyAllToClipboard");
            this.ToolStripMenuItemCopySelected.Text = 
                resManager.GetString("CopySelectedToClipboard");
            this.ToolStripHelp.Text = resManager.GetString("Help");
        }

        #endregion
    }
}