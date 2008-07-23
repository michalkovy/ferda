// FrequencyDisplayer.cs - user control class for displaying the
// frequencies of a column
//
// Authors:   Alexander Kuzmin <alexander.kuzmin@gmail.com>
//           Martin Ralbovsky <martin.ralbovsky@gmail.com>
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
using Ferda.FrontEnd.AddIns.Common.ListView;
using Ferda.Modules.Boxes.DataPreparation;
using Ferda.Guha.Data;
using System.Resources;
using System.Reflection;

namespace Ferda.FrontEnd.AddIns.FrequencyDisplayer
{
    /// <summary>
    /// Class for displaying column frequencies
    /// </summary>
    public partial class FrequencyDisplayer : UserControl
    {
        #region Private variables

        /// <summary>
        /// Resource manager
        /// </summary>
        private ResourceManager resManager;

        /// <summary>
        /// Total count of rows in the table.
        /// </summary>
        private long rowCount;

        /// <summary>
        /// Datatable containing data from the SQL query
        /// </summary>
        private DataTable tempTable;

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
        /// <param name="resManager">Resource manager for the module</param>
        /// <param name="ownerOfAddIn">
        /// Owner of the addin (usually the FrontEnd environment)</param>
        /// <param name="valfreq">Pairs of values and frequencies to form
        /// the tables and graphs</param>
        public FrequencyDisplayer(ResourceManager resManager, ValuesAndFrequencies valfreq,
            IOwnerOfAddIn ownerOfAddIn)
        {
            this.resManager = resManager;
            this.ownerOfAddIn = ownerOfAddIn;

            //implicitly sorting by the first column
            comparer.column = 0;
            //compute the row count
            ComputeRowCount(valfreq);

            InitializeComponent();
            ListViewInit();
            InitializeGraph();

            //Creating the temporary DataTable
            CreateTable(valfreq);

            this.FrDataTableToFrListView(tempTable);
            this.DrawAreaFromDataTable(tempTable,
                this.ColumnFrequencyAreaChart);
            this.DrawBarsFromDataTable(tempTable,
                this.ColumnFrequencyBarChart);
            this.DrawPieFromDataTable(tempTable,
                this.ColumnFrequencyPieChart);

            //event handling
            this.ToolStripMenuItemAbsolute.CheckedChanged += 
                new EventHandler(ToolStripMenuItem_AbCheckedChanged);
            this.ToolStripMenuItemCopyAll.Click += 
                new EventHandler(ToolStripMenuItemCopyAll_Click);
            this.ToolStripMenuItemCopySelected.Click += 
                new EventHandler(ToolStripMenuItemCopySelected_Click);
            this.ToolStripMenuItemCopyChart.Click += 
                new EventHandler(ToolStripMenuItemCopyChart_Click);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Computes the number of rows in the data table
        /// (from the frequencies of attributes)
        /// </summary>
        /// <param name="valfreq">Values and Frequency pair</param>
        private void ComputeRowCount(ValuesAndFrequencies valfreq)
        {
            long count = 0;
            foreach (ValueFrequencyPair pair in valfreq.data)
            {
                count += pair.frequency;
            }

            rowCount = count;
        }

        /// <summary>
        /// Creates the temporary table that is used for the graph creation
        /// and stores it to the local variable
        /// </summary>
        /// <param name="valfreq">Values and frequency pairs to create
        /// the datatable from</param>
        private void CreateTable(ValuesAndFrequencies valfreq)
        {
            DataTable table = new DataTable("Table");
            DataColumn column;
            DataRow row;

            //adding the first column
            column = new DataColumn();
            column.DataType = typeof(string);
            column.ColumnName = "DistinctsColumn";
            column.ReadOnly = true;
            column.Unique = true;
            table.Columns.Add(column);

            //adding the second column
            column = new DataColumn();
            column.DataType = typeof(System.Int32);
            column.ColumnName = "DistinctsFrequency";
            column.ReadOnly = true;
            column.Unique = false;
            table.Columns.Add(column);

            //adding the rows to the table
            foreach (ValueFrequencyPair pair in valfreq.data)
            {
                row = table.NewRow();
                row["DistinctsColumn"] = pair.value;
                row["DistinctsFrequency"] = pair.frequency;
                table.Rows.Add(row);
            }

            tempTable = table;
        }

        /// <summary>
        /// Method to convert Frequences DataTable to Frequences ListView
        /// </summary>
        /// <param name="dataTable"></param>
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
            this.ChangeLocale();
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
            comparer.column = e.Column;
            if (ColumnFrListView.Sorting == SortOrder.Ascending)
            {
                comparer.bAscending = false;
                ColumnFrListView.Sorting = SortOrder.Descending;
            }
            else
            {
                comparer.bAscending = true;
                ColumnFrListView.Sorting = SortOrder.Ascending;
            }
            ColumnFrListView.ListViewItemSorter = comparer;
        }

        #endregion

        #region Context menu handlers

        /// <summary>
        /// Viewing absolute values
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        void ToolStripMenuItem_AbCheckedChanged(object sender, EventArgs e)
        {
            this.DrawBarsFromDataTable(this.tempTable, this.ColumnFrequencyBarChart);
            this.DrawAreaFromDataTable(this.tempTable, this.ColumnFrequencyAreaChart);
            this.DrawPieFromDataTable(this.tempTable, this.ColumnFrequencyPieChart);
        }

        /// <summary>
        /// Toggling labels or legend on chart
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        private void ToolStripMenuToggleMarks_CheckChanged(object sender, EventArgs e)
        {
            if (this.ToolStripMenuItemShowLegend.Checked)
            {
                ColumnFrequencyAreaChart.Legend.Visible = true;
                ColumnFrequencyBarChart.Legend.Visible = true;
                ColumnFrequencyPieChart.Legend.Visible = true;
            }
            else
            {
                ColumnFrequencyAreaChart.Legend.Visible = false;
                ColumnFrequencyBarChart.Legend.Visible = false;
                ColumnFrequencyPieChart.Legend.Visible = false;
            }
            this.DrawBarsFromDataTable(this.tempTable, this.ColumnFrequencyBarChart);
            this.DrawAreaFromDataTable(this.tempTable, this.ColumnFrequencyAreaChart);
            this.DrawPieFromDataTable(this.tempTable, this.ColumnFrequencyPieChart);
        }

        /// <summary>
        /// Copy all to clipboard
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
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
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
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

        /// <summary>
        /// Opens the help document
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        private void ToolStripHelp_Click(object sender, EventArgs e)
        {
            ownerOfAddIn.OpenPdf(ownerOfAddIn.GetBinPath() + "\\AddIns\\Help\\FrequencyDisplayer.pdf");
        }

        /// <summary>
        /// Opens the help document
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        private void ToolStripChartHelp_Click(object sender, EventArgs e)
        {
            ownerOfAddIn.OpenPdf(ownerOfAddIn.GetBinPath() + "\\AddIns\\Help\\FrequencyDisplayer.pdf");
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
        /// Method to change l10n.
        /// </summary>
        private void ChangeLocale()
        {
            this.FrequencyColumn.Text = resManager.GetString("FrequencyColumn");
            this.ValuesColumn.Text = resManager.GetString("ValueColumn");
            this.PercentageColumn.Text = resManager.GetString("PercentageColumn");
            this.TabPageBarChart.Text = resManager.GetString("TabPageBarChart");
            this.TabPageAreaChart.Text = resManager.GetString("TabPageAreaChart");
            this.TabPagePieChart.Text = resManager.GetString("TabPagePieChart");
            this.TabPageText.Text = resManager.GetString("TabPageText");
            this.ToolStripMenuItemAbsolute.Text = resManager.GetString("ToolStripMenuItemAbsolute");
            this.ToolStripMenuItemCopyAll.Text = resManager.GetString("CopyAllToClipboard");
            this.ToolStripMenuItemCopySelected.Text = resManager.GetString("CopySelectedToClipboard");
            this.ToolStripMenuItemCopyChart.Text = resManager.GetString("CopyChart");
            this.ToolStripMenuToggleMarks.Text = resManager.GetString("ToggleMarks");
            this.ToolStripMenuItemShowLegend.Text = resManager.GetString("ShowLegend");
            this.ToolStripHelp.Text = resManager.GetString("Help");
            this.ToolStripChartHelp.Text = resManager.GetString("Help");
        }
        #endregion
    }
}
