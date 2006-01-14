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
namespace Ferda
{
    namespace FrontEnd.AddIns.DatabaseInfo
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
            private DataMatrixInfo[] dataMatrix;

            #endregion

            #region Constructor
            public DataBaseInfo(string [] localePrefs,DataMatrixInfo [] dataMatrix)
            {
                //setting the ResManager resource manager and localization string

                string locale;
                try
                {
                    locale = localePrefs[0];

                    localizationString = locale;

                    locale = "Ferda.FrontEnd.AddIns.DataBaseInfo.Localization_" + locale;

                    resManager = new ResourceManager(locale,
                Assembly.GetExecutingAssembly());

                }

                catch
                {
                    resManager = new ResourceManager("Ferda.FrontEnd.AddIns.DataBaseInfo.Localization_en-US",
                Assembly.GetExecutingAssembly());
                    localizationString = "en-US";
                }

                
                this.dataMatrix = dataMatrix;

                InitializeComponent();
                this.ListViewInit();
                this.FillDBInfoListView();
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
                foreach (DataMatrixInfo value in this.dataMatrix)
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
            }
        }
    }
}