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
                resManager = new ResourceManager(locale, Assembly.GetExecutingAssembly());
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
            this.ChangeLocale(this.resManager);
            this.ListViewInit();
            DBInteraction explainTable = new DBInteraction(this.dataMatrix.dataMatrixName, this.dataMatrix);
            this.MakeListView(explainTable.ShowTable());
            this.ToolStripMenuItemCopyAll.Click += new EventHandler(ToolStripMenuItemCopyAll_Click);
            this.ToolStripMenuItemCopySelected.Click += new EventHandler(ToolStripMenuItemCopySelected_Click);
        }

        #endregion


        #region Context menu handlers

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
            this.ToolStripMenuItemCopySelected.Text = rm.GetString("CopySelectedToClipboard");
            this.ToolStripMenuItemCopyAll.Text = rm.GetString("CopyAllToClipboard");
        }

        #endregion
    }
}