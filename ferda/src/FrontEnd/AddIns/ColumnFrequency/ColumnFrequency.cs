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
namespace Ferda
{
    namespace FrontEnd.AddIns.ColumnFrequency
    {
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
            public ColumnFrequency(string [] localePrefs,ColumnStruct columnStruct)
            {
                //setting the ResManager resource manager and localization string
                string locale;
                try
                {
                    locale = localePrefs[0];
                    localizationString = locale;
                    locale = "Ferda.FrontEnd.AddIns.ColumnFr.Localization_" + locale;
                    resManager = new ResourceManager(locale,Assembly.GetExecutingAssembly());
                }
                catch
                {
                    resManager = new ResourceManager("Ferda.FrontEnd.AddIns.ColumnFr.Localization_en-US",
                Assembly.GetExecutingAssembly());
                    localizationString = "en-US";
                }
                this.rowCount = columnStruct.dataMatrix.recordsCount;
                InitializeComponent();
                DBInteraction myDb = new DBInteraction(columnStruct,resManager);
                this.ListViewInit();
                this.InitializeGraph();
                this.tempTable = myDb.GetAllValuesCount();
                this.FrDataTableToFrListView(this.tempTable);
                this.DrawBarsFromDataTable(this.tempTable, this.ColumnFrequencyChart);
                this.ToolStripMenuItemAbsolute.CheckedChanged += new EventHandler(ToolStripMenuItem_AbCheckedChanged);
                this.ToolStripMenuItemPie.CheckedChanged += new EventHandler(ToolStripMenuItemPie_CheckedChanged);
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
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ListViewItem newItem = new ListViewItem();
                    newItem.Text = dataRow[0].ToString();

                    newItem.SubItems.Add(dataRow[1].ToString());

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
                    }
                    else
                    {
                        newItem.SubItems.Add("0");
                    }

                    

                    this.ColumnFrListView.Items.Add(newItem);
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

            void ToolStripMenuItem_AbCheckedChanged(object sender, EventArgs e)
            {
                this.DrawBarsFromDataTable(this.tempTable, this.ColumnFrequencyChart);
            }

            void ToolStripMenuItemPie_CheckedChanged(object sender, EventArgs e)
            {
                if (this.ColumnFrequencyChart.Series.Count > 0)
                {
                    this.DrawBarsFromDataTable(this.tempTable, this.ColumnFrequencyChart);
                }
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
                this.TabPageGraph.Text = rm.GetString("TabPageGraph");
                this.TabPageText.Text = rm.GetString("TabPageText");
                this.ToolStripMenuItemAbsolute.Text = rm.GetString("ToolStripMenuItemAbsolute");
                this.ToolStripMenuItemPie.Text = rm.GetString("ToolStripMenuItemPie");
            }

            #endregion

        }
    }
}
