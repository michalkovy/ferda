using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ferda.Modules.Boxes.DataMiningCommon.Attributes;
using Ferda.Modules.Boxes.DataMiningCommon.Column;
using Ferda.FrontEnd.AddIns.AttributeFrequency.NonGUIClasses;
using System.Resources;
using Ferda.Modules;

using System.Reflection;
using System.Threading;
using System.Globalization;

namespace Ferda
{
    namespace FrontEnd.AddIns.AttributeFrequency
    {
        public partial class AttributeFrequency : UserControl
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
            /// ColumnStruct for the column of the attribute
            /// </summary>
            private ColumnStruct columnStruct;

            /// <summary>
            /// Categories to count frequences on
            /// </summary>
            private CategoriesStruct categoriesStruct;
            /// <summary>
            /// Datatable containing data from the SQL query
            /// </summary>
            private ArrayList tempTable;       

            #endregion


            #region Constructor
            public AttributeFrequency(AbstractAttributeStruct attributeStruct,string [] localePrefs)
            {
                //setting the ResManager resource manager and localization string
                resManager = new ResourceManager("Ferda.FrontEnd.AddIns.AttributeFrequency.Localization_en-US",
                Assembly.GetExecutingAssembly());
                string locale;
                try
                {
                    locale = localePrefs[0];
                    localizationString = locale;
                  //  Thread.CurrentThread.CurrentCulture = new CultureInfo(locale);
                  //  Thread.CurrentThread.CurrentUICulture = new CultureInfo(locale);
                  //  locale = "Ferda.FrontEnd.AddIns.AttributeFr.Localization." + locale;      
                    locale = "Ferda.FrontEnd.AddIns.AttributeFrequency.Localization_" + locale;
                    resManager = new ResourceManager(locale, Assembly.GetExecutingAssembly());
                }
                catch
                {
                 //   Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                 //   Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                    localizationString = "en-US";
                }
                this.rowCount = attributeStruct.column.dataMatrix.recordsCount;
                this.columnStruct = attributeStruct.column;
                this.categoriesStruct = attributeStruct.categories;
                InitializeComponent();
                DBInteraction myDb = new DBInteraction(columnStruct, this.categoriesStruct, resManager);
                this.ListViewInit();
                this.InitializeGraph();
                this.tempTable = myDb.GetCategoriesFrequences(this.categoriesStruct);
                this.ArrayListToListView(this.tempTable);
                this.DrawBarsFromDataTable(this.tempTable, this.AttributeFrequencyBarChart);
                this.DrawAreaFromDataTable(this.tempTable, this.AttributeFrequencyAreaChart);
                this.DrawPieFromDataTable(this.tempTable, this.AttributeFrequencyPieChart);
                this.ToolStripMenuItemAbsolute.CheckedChanged += new EventHandler(ToolStripMenuItem_AbCheckedChanged);
            }

            #endregion
            #region Context menu handlers
            void ToolStripMenuItem_AbCheckedChanged(object sender, EventArgs e)
            {
                this.DrawBarsFromDataTable(this.tempTable, this.AttributeFrequencyBarChart);
                this.DrawAreaFromDataTable(this.tempTable, this.AttributeFrequencyAreaChart);
                this.DrawPieFromDataTable(this.tempTable, this.AttributeFrequencyPieChart);
            }
            #endregion


            #region Private methods

            /// <summary>
            /// Method to convert Frequences DataTable to Frequences ListView
            /// </summary>
            /// <param name="dataTable"></param>
            /// <returns></returns>
            private void ArrayListToListView(ArrayList categoriesFrequency)
            {
                foreach (CategoryFrequency catFrequency in categoriesFrequency)
                {
                    ListViewItem newItem = new ListViewItem();
                    newItem.Text = catFrequency.key;

                    newItem.SubItems.Add(catFrequency.count.ToString());

                    double temp;
                    try
                    {
                        temp = Convert.ToDouble(catFrequency.count);
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
                    this.AttributeFrListView.Items.Add(newItem);
                }
            }

            /// <summary>
            /// Method for ColumnFrListView init
            /// </summary>
            private void ListViewInit()
            {
                this.ChangeLocale(resManager);
                //adding a handling method for column sorting
                this.AttributeFrListView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.ColumnFrListView_ColumnClick);
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

                if ((columnSorter.bAscending = (AttributeFrListView.Sorting == SortOrder.Ascending)))
                    AttributeFrListView.Sorting = SortOrder.Descending;
                else
                    AttributeFrListView.Sorting = SortOrder.Ascending;

                AttributeFrListView.ListViewItemSorter = columnSorter;
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
            }

            #endregion
        }
    }
}