using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Resources;
using System.Reflection;
using Ferda.Modules.Boxes.DataMiningCommon.DataMatrix;
using Ferda.FrontEnd.AddIns.ShowTable.NonGUIClasses;

namespace Ferda.FrontEnd.AddIns.ShowTable
{
    public partial class ShowTableControl : UserControl
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
        private string[] columns;

        /// <summary>
        /// DataMatrixStruct
        /// </summary>
        private DataMatrixStruct dataMatrix;

        #endregion

        #region Constructor
        public ShowTableControl(string[] localePrefs, string[] columns, DataMatrixStruct dataMatrixName)
        {
            //setting the ResManager resource manager and localization string

            string locale;
            try
            {
                locale = localePrefs[0];

                localizationString = locale;

                locale = "Ferda.FrontEnd.AddIns.ShowTable.Localization_" + locale;

                resManager = new ResourceManager(locale,
            Assembly.GetExecutingAssembly());

            }

            catch
            {
                resManager = new ResourceManager("Ferda.FrontEnd.AddIns.ShowTable.Localization_en-US",
            Assembly.GetExecutingAssembly());
                localizationString = "en-US";
            }

            this.columns = columns;
            this.dataMatrix = dataMatrixName;
            InitializeComponent();
            this.ListViewInit();

            DBInteraction explainTable = new DBInteraction(this.dataMatrix.dataMatrixName, this.dataMatrix);
            this.MakeListView(explainTable.ShowTable());
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

                for(int i = 1; i < dataRow.ItemArray.GetUpperBound(0);i++)
                {
                    newItem.SubItems.Add(dataRow[i].ToString());
                }
                this.ListViewShowTable.Items.Add(newItem);
            }
        }

        #endregion

    }
}