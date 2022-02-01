// ShowTableControl.cs - UserControl class for displaying database table
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
using Ferda.FrontEnd.AddIns.Common.ListView;
using Ferda.FrontEnd.AddIns.ShowTable.NonGUIClasses;
using Ferda.Modules.Boxes.DataPreparation;


namespace Ferda.FrontEnd.AddIns.ShowTable
{
    /// <summary>
    /// Usercontrol class for the module for interaction displaying database table
    /// </summary>
    public partial class ShowTableControl : UserControl
    {
        #region Private variables

        /// <summary>
        /// Resource manager
        /// </summary>
        private ResourceManager resManager;

        /// <summary>
        /// ColumnInfo array
        /// </summary>
        private string[] columns;

        /// <summary>
        /// Owner of addin
        /// </summary>
        private IOwnerOfAddIn ownerOfAddIn;

        /// <summary>
        /// Listview items comparer
        /// </summary>
        private ListViewItemComparer comparer = new ListViewItemComparer();

        /// <summary>
        /// Structure that holds the information about the data table
        /// </summary>
        private DataTableInfo dataTableInfo;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor of the class
        /// </summary>
        /// <param name="resManager">Resource manager for the module</param>
        /// <param name="ownerOfAddIn">
        /// Owner of the addin (usually the FrontEnd environment)</param>
        /// <param name="columns">Names of the columns</param>
        /// <param name="dataTableInfo">
        /// Structure that holds the information about the data table</param>
        public ShowTableControl(ResourceManager resManager, string[] columns, 
            DataTableInfo dataTableInfo, IOwnerOfAddIn ownerOfAddIn)
        {
            this.ownerOfAddIn = ownerOfAddIn;
            this.resManager = resManager;
            this.columns = columns;
            this.dataTableInfo = dataTableInfo;

            comparer.column = 0;
            
            InitializeComponent();
            
            this.ChangeLocale();
            this.ListViewInit();
            DBInteraction explainTable = new DBInteraction(dataTableInfo);
            this.MakeListView(explainTable.ShowTable());
            this.ToolStripMenuItemCopyAll.Click += new EventHandler(ToolStripMenuItemCopyAll_Click);
            this.ToolStripMenuItemCopySelected.Click += new EventHandler(ToolStripMenuItemCopySelected_Click);
        }

        #endregion

        #region Context menu handlers

        /// <summary>
        /// Copy selected to clipboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ToolStripMenuItemCopySelected_Click(object sender, EventArgs e)
        {
            StringBuilder copyString = new StringBuilder();

            foreach (ColumnHeader header in this.ListViewShowTable.Columns)
            {
                copyString.Append(header.Text + "\t");
            }
            //deleting last tab
            copyString.Remove(copyString.Length - 1, 1);
            copyString.AppendLine();
            foreach (ListViewItem item in this.ListViewShowTable.SelectedItems)
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
        /// Copy all to clipboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ToolStripMenuItemCopyAll_Click(object sender, EventArgs e)
        {
            StringBuilder copyString = new StringBuilder();

            foreach (ColumnHeader header in this.ListViewShowTable.Columns)
            {
                copyString.Append(header.Text + "\t");
            }
            //deleting last tab
            copyString.Remove(copyString.Length - 1, 1);
            copyString.AppendLine();
            foreach (ListViewItem item in this.ListViewShowTable.Items)
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
        /// Showing pdf file with help
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripHelp_Click(object sender, EventArgs e)
        {
            ownerOfAddIn.OpenPdf(ownerOfAddIn.GetBinPath() + "\\AddIns\\Help\\ShowTable.pdf");
        }


        void ListViewShowTable_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            comparer.column = e.Column;
            if (ListViewShowTable.Sorting == SortOrder.Ascending)
            {
                comparer.bAscending = false;
                ListViewShowTable.Sorting = SortOrder.Descending;
            }
            else
            {
                comparer.bAscending = true;
                ListViewShowTable.Sorting = SortOrder.Ascending;
            }

            ListViewShowTable.ListViewItemSorter = comparer;
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Method for listview initialization
        /// </summary>
        private void ListViewInit()
        {
            foreach (string headerText in this.columns)
            {
                ColumnHeader header = new ColumnHeader();
                header.Text = headerText;
                this.ListViewShowTable.Columns.Add(header);
            }
            this.ListViewShowTable.ColumnClick += new ColumnClickEventHandler(ListViewShowTable_ColumnClick);
        }
       
        /// <summary>
        /// Method to fill listview with DataTable data
        /// </summary>
        /// <param name="table">Table to convert to listview</param>
        private void MakeListView(DataTable table)
        {
            foreach (DataRow dataRow in table.Rows)
            {
                ListViewItem newItem = new ListViewItem();
                newItem.Text = dataRow[0].ToString();

                for(int i = 1; i <= dataRow.ItemArray.GetUpperBound(0);i++)
                {
                    newItem.SubItems.Add(dataRow[i].ToString());
                }
                this.ListViewShowTable.Items.Add(newItem);
            }
        }

        #endregion

        #region Localization

        /// <summary>
        /// Method to change l10n.
        /// </summary>
        private void ChangeLocale()
        {
            this.ToolStripMenuItemCopySelected.Text = 
                resManager.GetString("CopySelectedToClipboard");
            this.ToolStripMenuItemCopyAll.Text = 
                resManager.GetString("CopyAllToClipboard");
            this.ToolStripHelp.Text = resManager.GetString("Help");
        }

        #endregion
    }
}