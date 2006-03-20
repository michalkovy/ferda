// ColumnFrequency.cs - Usercontrol class
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
using Ferda.Modules.Boxes.DataMiningCommon.Column;
using Ferda.FrontEnd.AddIns.ColumnFrequency.NonGUIClasses;
using System.Resources;
using System.Reflection;

namespace Ferda.FrontEnd.AddIns.ColumnFrequency
{
    /// <summary>
    /// Class for displaying column frequencies
    /// </summary>
    public partial class ColumnFrequency : UserControl
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
        /// Total count of rows in the table.
        /// </summary>
        private long rowCount;


        /// <summary>
        /// Datatable containing data from the SQL query
        /// </summary>
        private DataTable tempTable;


        #endregion


        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="localePrefs">Localeprefs</param>
        /// <param name="columnInfo">Columninfo</param>
        public ColumnFrequency(string[] localePrefs, ColumnInfo columnInfo)
        {
            //setting the ResManager resource manager and localization string
            string locale;
            try
            {
                locale = localePrefs[0];
                localizationString = locale;
                locale = "Ferda.FrontEnd.AddIns.ColumnFr.Localization_" + locale;
                resManager = new ResourceManager(locale, Assembly.GetExecutingAssembly());
            }
            catch
            {
                resManager = new ResourceManager("Ferda.FrontEnd.AddIns.ColumnFr.Localization_en-US", Assembly.GetExecutingAssembly());
                localizationString = "en-US";
            }
            this.rowCount = columnInfo.dataMatrix.recordsCount;
            InitializeComponent();
            DBInteraction myDb = new DBInteraction(columnInfo);
            this.ListViewInit();
            this.InitializeGraph();
            this.tempTable = myDb.GetAllValuesCount();
            this.FrDataTableToFrListView(this.tempTable);
            this.DrawAreaFromDataTable(this.tempTable, this.ColumnFrequencyAreaChart);
            this.DrawBarsFromDataTable(this.tempTable, this.ColumnFrequencyBarChart);
            this.DrawPieFromDataTable(this.tempTable, this.ColumnFrequencyPieChart);
            this.ToolStripMenuItemAbsolute.CheckedChanged += new EventHandler(ToolStripMenuItem_AbCheckedChanged);
            this.ToolStripMenuItemCopyAll.Click += new EventHandler(ToolStripMenuItemCopyAll_Click);
            this.ToolStripMenuItemCopySelected.Click += new EventHandler(ToolStripMenuItemCopySelected_Click);
            this.ToolStripMenuItemCopyChart.Click += new EventHandler(ToolStripMenuItemCopyChart_Click);
        }

        #endregion


        #region Private methods

        /// <summary>
        /// Method to convert Frequences DataTable to Frequences ListView
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        private void FrDataTableToFrListView(DataTable dataTable)
        {
            StringBuilder toolTip = new StringBuilder();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                ListViewItem newItem = new ListViewItem();
                newItem.Text = dataRow[0].ToString();
                toolTip.Append(dataRow[0].ToString());
                newItem.SubItems.Add(dataRow[1].ToString());
                toolTip.Append("\t" + dataRow[1].ToString());
                double temp;
                try
                {
                    temp = Convert.ToDouble(dataRow[1]);
                }

                catch
                {
                    temp = 0;
                }

                if ((temp != 0) && (this.rowCount != 0))
                {
                    temp = Math.Round(((temp / (double)this.rowCount) * 100), 2);
                    newItem.SubItems.Add(temp.ToString());
                    toolTip.Append("\t" + temp.ToString());
                }
                else
                {
                    newItem.SubItems.Add("0");
                    toolTip.Append("\t" + "0");
                }
                toolTip.AppendLine();
                toolTip.Append(" (" + this.resManager.GetString("RightClickToCopySelected") + ")");
                newItem.ToolTipText = toolTip.ToString();
                this.ColumnFrListView.Items.Add(newItem);
                toolTip.Remove(0, toolTip.Length);
            }
        }

        /// <summary>
        /// Method for ColumnFrListView init
        /// </summary>
        private void ListViewInit()
        {
            this.ChangeLocale(resManager);
            //adding a handling method for column sorting
            this.ColumnFrListView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.ColumnFrListView_ColumnClick);
        }

        /// <summary>
        /// Handler for column click - sorts a listview.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ColumnFrListView_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            ListViewItemComparer columnSorter = new ListViewItemComparer();
            columnSorter.column = e.Column;
            if ((columnSorter.bAscending = (ColumnFrListView.Sorting == SortOrder.Ascending)))
                ColumnFrListView.Sorting = SortOrder.Descending;
            else
                ColumnFrListView.Sorting = SortOrder.Ascending;

            ColumnFrListView.ListViewItemSorter = columnSorter;
        }


        #endregion


        #region Context menu handlers

        /// <summary>
        /// Viewing absolute values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ToolStripMenuItem_AbCheckedChanged(object sender, EventArgs e)
        {
            this.DrawBarsFromDataTable(this.tempTable, this.ColumnFrequencyBarChart);
            this.DrawAreaFromDataTable(this.tempTable, this.ColumnFrequencyAreaChart);
            this.DrawPieFromDataTable(this.tempTable, this.ColumnFrequencyPieChart);
        }

        /// <summary>
        /// Copy all to clipboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ToolStripMenuItemCopyAll_Click(object sender, EventArgs e)
        {
            StringBuilder copyString = new StringBuilder();
            copyString.Append(this.resManager.GetString("ValueColumn") + "\t" +
                this.resManager.GetString("FrequencyColumn") + "\t" +
                this.resManager.GetString("PercentageColumn"));
            copyString.AppendLine();

            foreach (ListViewItem item in this.ColumnFrListView.Items)
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
            copyString.Append(this.resManager.GetString("ValueColumn") + "\t" +
                this.resManager.GetString("FrequencyColumn") + "\t" +
                this.resManager.GetString("PercentageColumn"));
            copyString.AppendLine();

            foreach (ListViewItem item in this.ColumnFrListView.SelectedItems)
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
            this.FrequencyColumn.Text = rm.GetString("FrequencyColumn");
            this.ValuesColumn.Text = rm.GetString("ValueColumn");
            this.PercentageColumn.Text = rm.GetString("PercentageColumn");
            this.TabPageBarChart.Text = rm.GetString("TabPageBarChart");
            this.TabPageAreaChart.Text = rm.GetString("TabPageAreaChart");
            this.TabPagePieChart.Text = rm.GetString("TabPagePieChart");
            this.TabPageText.Text = rm.GetString("TabPageText");
            this.ToolStripMenuItemAbsolute.Text = rm.GetString("ToolStripMenuItemAbsolute");
            this.ToolStripMenuItemCopyAll.Text = rm.GetString("CopyAllToClipboard");
            this.ToolStripMenuItemCopySelected.Text = rm.GetString("CopySelectedToClipboard");
            this.ToolStripMenuItemCopyChart.Text = rm.GetString("CopyChart");
        }
        #endregion
    }
}
