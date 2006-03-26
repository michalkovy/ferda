// ExplainTableControl.cs - User Control class for displaying explain table
// results of SQL query
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
using Ferda.Modules.Boxes.DataMiningCommon.DataMatrix;
using Ferda.FrontEnd.AddIns.Common.ListView;
using System.Resources;
using System.Reflection;


namespace Ferda.FrontEnd.AddIns.ExplainTable
{
    /// <summary>
    /// UserControl class for displaying SQL query explain table results
    /// </summary>
    public partial class ExplainTable : UserControl
    {
        #region Private variables

        /// <summary>
        /// Resource manager
        /// </summary>
        private ResourceManager resManager;

        /// <summary>
        /// Localization string, en-US or cs-CZ for now.
        /// </summary>
        private string localizationString;

        /// <summary>
        /// ColumnSchemaInfo array
        /// </summary>
        private ColumnSchemaInfo[] dataMatrix;

        /// <summary>
        /// DataMatrixInfo
        /// </summary>
        private DataMatrixInfo dataMatrixInfo;

        /// <summary>
        /// Owner of addin
        /// </summary>
        private IOwnerOfAddIn ownerOfAddIn;

        /// <summary>
        /// Comparer for the listview items
        /// </summary>
        private ListViewItemComparer comparer = new ListViewItemComparer();

        #endregion


        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="localePrefs">Localeprefs</param>
        /// <param name="dataMatrix">Datamatrix</param>
        /// <param name="dataMatrixInfo">Datamatrix info</param>
        public ExplainTable(string[] localePrefs, ColumnSchemaInfo[] dataMatrix, DataMatrixInfo dataMatrixInfo, IOwnerOfAddIn ownerOfAddIn)
        {
            //setting the ResManager resource manager and localization string
            string locale;
            try
            {
                locale = localePrefs[0];
                localizationString = locale;
                locale = "Ferda.FrontEnd.AddIns.ExplainTable.Localization_" + locale;
                resManager = new ResourceManager(locale, Assembly.GetExecutingAssembly());
            }
            catch
            {
                resManager = new ResourceManager("Ferda.FrontEnd.AddIns.ExplainTable.Localization_en-US",
            Assembly.GetExecutingAssembly());
                localizationString = "en-US";
            }
            this.ownerOfAddIn = ownerOfAddIn;
            comparer.column = 0;
            this.dataMatrix = dataMatrix;
            this.dataMatrixInfo = dataMatrixInfo;
            InitializeComponent();
            this.ListViewInit();
            this.MakeListView();
            this.ToolStripMenuItemCopyAll.Click += new EventHandler(ToolStripMenuItemCopyAll_Click);
            this.ToolStripMenuItemCopySelected.Click += new EventHandler(ToolStripMenuItemCopySelected_Click);
        }

        private void ToolStripHelp_Click(object sender, EventArgs e)
        {
            ownerOfAddIn.OpenPdf(ownerOfAddIn.GetBinPath() + "\\AddIns\\Help\\ExplainTable.pdf");
        }

        #endregion


        #region Context menu handlers

        /// <summary>
        /// Copy selected items to clipboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ToolStripMenuItemCopySelected_Click(object sender, EventArgs e)
        {
            StringBuilder copyString = new StringBuilder();
            copyString.Append(this.resManager.GetString("ColumnName") + "\t" +
                this.resManager.GetString("ColumnAllowDBNull") + "\t" +
                this.resManager.GetString("ColumnOrdinal") + "\t" +
                this.resManager.GetString("ColumnSize") + "\t" +
                this.resManager.GetString("ColumnDataType") + "\t" +
                this.resManager.GetString("ColumnAutoIncrement") + "\t" +
                this.resManager.GetString("ColumnIsKey") + "\t" +
                this.resManager.GetString("ColumnIsLong") + "\t" +
                this.resManager.GetString("ColumnIsReadOnly") + "\t" +
                this.resManager.GetString("ColumnIsRowVersion") + "\t" +
                this.resManager.GetString("ColumnIsUnique") + "\t" +
                this.resManager.GetString("ColumnNumericalPrecision") + "\t" +
                this.resManager.GetString("ColumnNumericalScale") + "\t" +
                this.resManager.GetString("ColumnProviderType"));
            copyString.AppendLine();

            foreach (ListViewItem item in this.ExplainTableListView.SelectedItems)
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
        /// Copy all items to clipboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ToolStripMenuItemCopyAll_Click(object sender, EventArgs e)
        {
            StringBuilder copyString = new StringBuilder();
            copyString.Append(this.resManager.GetString("ColumnName") + "\t" +
                this.resManager.GetString("ColumnAllowDBNull") + "\t" +
                this.resManager.GetString("ColumnOrdinal") + "\t" +
                this.resManager.GetString("ColumnSize") + "\t" +
                this.resManager.GetString("ColumnDataType") + "\t" +
                this.resManager.GetString("ColumnAutoIncrement") + "\t" +
                this.resManager.GetString("ColumnIsKey") + "\t" +
                this.resManager.GetString("ColumnIsLong") + "\t" +
                this.resManager.GetString("ColumnIsReadOnly") + "\t" +
                this.resManager.GetString("ColumnIsRowVersion") + "\t" +
                this.resManager.GetString("ColumnIsUnique") + "\t" +
                this.resManager.GetString("ColumnNumericalPrecision") + "\t" +
                this.resManager.GetString("ColumnNumericalScale") + "\t" +
                this.resManager.GetString("ColumnProviderType"));
            copyString.AppendLine();

            foreach (ListViewItem item in this.ExplainTableListView.Items)
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

        #endregion


        #region Other private methods

        /// <summary>
        /// Method to fill ListView with ColumnSchemaInfo data
        /// </summary>
        private void MakeListView()
        {
            foreach (ColumnSchemaInfo columnInfo in this.dataMatrix)
            {
                ListViewItem newItem = new ListViewItem();
                newItem.Text = columnInfo.name;
                newItem.SubItems.Add(columnInfo.allowDBNull.ToString());
                newItem.SubItems.Add(columnInfo.columnOrdinal.ToString());
                newItem.SubItems.Add(columnInfo.columnSize.ToString());
                newItem.SubItems.Add(columnInfo.dataType.ToString());
                newItem.SubItems.Add(columnInfo.isAutoIncrement.ToString());
                newItem.SubItems.Add(columnInfo.isKey.ToString());
                newItem.SubItems.Add(columnInfo.isLong.ToString());
                newItem.SubItems.Add(columnInfo.isReadOnly.ToString());
                newItem.SubItems.Add(columnInfo.isRowVersion.ToString());
                newItem.SubItems.Add(columnInfo.isUnique.ToString());
                newItem.SubItems.Add(columnInfo.numericPrecision.ToString());
                newItem.SubItems.Add(columnInfo.numericScale.ToString());
                newItem.SubItems.Add(columnInfo.providerType.ToString());
                this.ExplainTableListView.Items.Add(newItem);
            }
        }

        /// <summary>
        /// Method for ColumnFrListView init
        /// </summary>
        private void ListViewInit()
        {
            this.ChangeLocale(resManager);

            //adding a handling method for column sorting
            this.ExplainTableListView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.ExplainTableListView_ColumnClick);
        }

        /// <summary>
        /// Handler for column click - sorts a listview.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExplainTableListView_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            comparer.column = e.Column;
            if (ExplainTableListView.Sorting == SortOrder.Ascending)
            {
                comparer.bAscending = false;
                ExplainTableListView.Sorting = SortOrder.Descending;
            }
            else
            {
                comparer.bAscending = true;
                ExplainTableListView.Sorting = SortOrder.Ascending;
            }
            ExplainTableListView.ListViewItemSorter = comparer;
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
        private void ChangeLocale(ResourceManager rm)
        {
            this.ColumnAutoIncrement.Text = rm.GetString("ColumnAutoIncrement");
            this.ColumnAllowDBNull.Text = rm.GetString("ColumnAllowDBNull");
            this.ColumnDataType.Text = rm.GetString("ColumnDataType");
            this.ColumnIsKey.Text = rm.GetString("ColumnIsKey");
            this.ColumnIsLong.Text = rm.GetString("ColumnIsLong");
            this.ColumnIsReadOnly.Text = rm.GetString("ColumnIsReadOnly");
            this.ColumnIsRowVersion.Text = rm.GetString("ColumnIsRowVersion");
            this.ColumnIsUnique.Text = rm.GetString("ColumnIsUnique");
            this.ColumnName.Text = rm.GetString("ColumnName");
            this.ColumnNumericalPrecision.Text = rm.GetString("ColumnNumericalPrecision");
            this.ColumnNumericalScale.Text = rm.GetString("ColumnNumericalScale");
            this.ColumnOrdinal.Text = rm.GetString("ColumnOrdinal");
            this.ColumnProviderType.Text = rm.GetString("ColumnProviderType");
            this.ColumnSize.Text = rm.GetString("ColumnSize");
            this.ToolStripMenuItemCopySelected.Text = rm.GetString("CopySelectedToClipboard");
            this.ToolStripMenuItemCopyAll.Text = rm.GetString("CopyAllToClipboard");
            this.ToolStripHelp.Text = rm.GetString("Help");
        }

        #endregion
    }
}