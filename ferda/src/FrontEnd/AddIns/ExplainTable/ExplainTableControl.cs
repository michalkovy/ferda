using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ferda.Modules.Boxes.DataMiningCommon.DataMatrix;
using Ferda.FrontEnd.AddIns.ExplainTable.NonGUIClasses;
using System.Resources;
using System.Reflection;


namespace Ferda
{
    namespace FrontEnd.AddIns.ExplainTable
    {
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
            /// ColumnInfo array
            /// </summary>
            private ColumnInfo[] dataMatrix;

            /// <summary>
            /// DataMatrixStruct
            /// </summary>
            private DataMatrixStruct dataMatrixStruct;

            #endregion

            #region Constructor
            public ExplainTable(string [] localePrefs,ColumnInfo [] dataMatrix, DataMatrixStruct dataMatrixStruct)
            {
                //setting the ResManager resource manager and localization string

                string locale;
                try
                {
                    locale = localePrefs[0];

                    localizationString = locale;

                    locale = "Ferda.FrontEnd.AddIns.ExplainTable.Localization_" + locale;

                    resManager = new ResourceManager(locale,
                Assembly.GetExecutingAssembly());

                }

                catch
                {
                    resManager = new ResourceManager("Ferda.FrontEnd.AddIns.ExplainTable.Localization_en-US",
                Assembly.GetExecutingAssembly());
                    localizationString = "en-US";
                }

                this.dataMatrix = dataMatrix;

                this.dataMatrixStruct = dataMatrixStruct;

                InitializeComponent();

                this.ListViewInit();

                this.MakeListView();

            }

            #endregion


            /// <summary>
            /// Method to fill ListView with ColumnInfo data
            /// </summary>
            private void MakeListView()
            {
                foreach (ColumnInfo columnInfo in this.dataMatrix)
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

                ListViewItemComparer columnSorter = new ListViewItemComparer(e.Column);

                if ((columnSorter.bAscending = (ExplainTableListView.Sorting == SortOrder.Ascending)))
                    ExplainTableListView.Sorting = SortOrder.Descending;
                else
                    ExplainTableListView.Sorting = SortOrder.Ascending;

                ExplainTableListView.ListViewItemSorter = columnSorter;

            }


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
            }

            #endregion
        }
    }
}