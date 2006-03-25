// DatabaseInfo.cs - Usercontrol class
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ferda.Modules.Boxes.DataMiningCommon.Database;
using Ferda.FrontEnd.AddIns.DatabaseInfo.NonGUIClasses;
using System.Resources;
using System.Reflection;
using Ferda.FrontEnd.AddIns;

namespace Ferda.FrontEnd.AddIns.DatabaseInfo
{
    public partial class DataBaseInfo : UserControl
    {
        #region Private variables

        /// <summary>
        /// Localization resource manager
        /// </summary>
        private ResourceManager resManager;

        /// <summary>
        /// Localization string - for now, en-US or cs-CZ
        /// </summary>
        private string localizationString;

        /// <summary>
        /// Datamatrix array to convert to listview
        /// </summary>
        private DataMatrixSchemaInfo[] dataMatrix;

        /// <summary>
        /// Owner of addin
        /// </summary>
        private IOwnerOfAddIn ownerOfAddIn;

        #endregion


        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="localePrefs">localeprefs</param>
        /// <param name="dataMatrix">Datamatrix</param>
        public DataBaseInfo(string[] localePrefs, DataMatrixSchemaInfo[] dataMatrix, IOwnerOfAddIn ownerOfAddIn)
        {
            //setting the ResManager resource manager and localization string
            string locale;
            try
            {
                locale = localePrefs[0];
                localizationString = locale;
                locale = "Ferda.FrontEnd.AddIns.DataBaseInfo.Localization_" + locale;
                resManager = new ResourceManager(locale, Assembly.GetExecutingAssembly());
            }
            catch
            {
                resManager = new ResourceManager("Ferda.FrontEnd.AddIns.DataBaseInfo.Localization_en-US",
            Assembly.GetExecutingAssembly());
                localizationString = "en-US";
            }
            this.ownerOfAddIn = ownerOfAddIn;
            this.dataMatrix = dataMatrix;
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
            this.ChangeLocale(resManager);
            //adding a handling method for column sorting
            this.DataBaseInfoListView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.DataBaseInfoListView_ColumnClick);
        }

        /// <summary>
        /// Method to fill the listview with datamatrix info.
        /// </summary>
        private void FillDBInfoListView()
        {
            foreach (DataMatrixSchemaInfo value in this.dataMatrix)
            {
                ListViewItem newItem = new ListViewItem();
                newItem.Text = value.name;
                newItem.SubItems.Add(value.remarks);
                newItem.SubItems.Add(value.rowCount.ToString());
                newItem.SubItems.Add(value.type);
                this.DataBaseInfoListView.Items.Add(newItem);
            }
        }

        /// <summary>
        /// Handler for column click - sorts a listview.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataBaseInfoListView_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {

            ListViewItemComparer columnSorter = new ListViewItemComparer();
            columnSorter.column = e.Column;

            if ((columnSorter.bAscending = (DataBaseInfoListView.Sorting == SortOrder.Ascending)))
                DataBaseInfoListView.Sorting = SortOrder.Descending;
            else
                DataBaseInfoListView.Sorting = SortOrder.Ascending;

            DataBaseInfoListView.ListViewItemSorter = columnSorter;

        }


        #endregion


        #region Localization

        /// <summary>
        /// Resource manager of the application, it is filled according to the
        /// current localization
        /// </summary>
        public ResourceManager ResManager
        {
            get
            {
                return resManager;
            }
        }

        /// <summary>
        /// Localization string of the application, possible values are "en-US" and "cs-CZ"
        /// </summary>
        public string LocalizationString
        {
            get
            {
                return localizationString;
            }
        }

        /// <summary>
        /// Method to change l10n.
        /// </summary>
        /// <param name="rm">Resource manager to handle new l10n resource</param>
        public void ChangeLocale(ResourceManager rm)
        {
            this.TableName.Text = rm.GetString("TableName");
            this.TableRemarks.Text = rm.GetString("Remarks");
            this.TableRowCount.Text = rm.GetString("RowCount");
            this.TableType.Text = rm.GetString("Type");
            this.ToolStripMenuItemCopyAll.Text = rm.GetString("CopyAllToClipboard");
            this.ToolStripMenuItemCopySelected.Text = rm.GetString("CopySelectedToClipboard");
            this.ToolStripHelp.Text = rm.GetString("Help");
        }

        #endregion
    }
}