// ShowMappingControl.cs - User Control class for displaying explain table
// results of SQL query
//
// Author: Martin Zeman <martinzeman@email.cz>
//
// Copyright (c) 2007 Martin Zeman
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
using Ferda.Modules.Boxes.DataPreparation;
using Ferda.Guha.Data;
using Ferda.FrontEnd.AddIns.Common.ListView;
using System.Resources;
using System.Reflection;
using Ferda.Modules.Boxes.OntologyRelated.OntologyMapping;
using Ferda.OntologyRelated.generated.OntologyData;


namespace Ferda.FrontEnd.AddIns.ShowMapping
{
    /// <summary>
    /// UserControl class for displaying SQL query explain table results
    /// </summary>
    public partial class ShowMappingControl : UserControl
    {
        #region Private variables

        /// <summary>
        /// Resource manager
        /// </summary>
        private ResourceManager resManager;

        /// <summary>
        /// Owner of addin
        /// </summary>
        private IOwnerOfAddIn ownerOfAddIn;

        /// <summary>
        /// Comparer for the listview items
        /// </summary>
        private ListViewItemComparer comparer = new ListViewItemComparer();

        /// <summary>
        /// Proxy to ontologyMapping box
        /// </summary>
        private OntologyMappingFunctionsPrx ontologyMappingPrx;

        /// <summary>
        /// String which contains mapping betweeen datatables columns and ontology entities
        /// structure of the mapping string is
        /// dataTableName(separatorInner)columnName(separatorInner)ontologyEntityName(separatorOuter)
        /// </summary>
        private string mapping;

        /// <summary>
        /// String which is used for separating particular mapping rows
        /// </summary>
        private string separatorOuter;

        /// <summary>
        /// String which is used for separating particular parts of mapping row
        /// </summary>
        private string separatorInner;

        /// <summary>
        /// String which is used for separating particular data properties values
        /// </summary>
        private string dataPropertyValuesSeparator = ", ";

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor of the class
        /// </summary>
        /// <param name="resManager">Resource manager for the module</param>
        /// <param name="columnExplain">Structure that holds information
        /// about the columns of the data table</param>
        /// <param name="ownerOfAddIn">
        /// Owner of the addin (usually the FrontEnd environment)</param>
        public ShowMappingControl(ResourceManager resManager, IOwnerOfAddIn ownerOfAddIn, OntologyMappingFunctionsPrx prx)
        {
            this.ownerOfAddIn = ownerOfAddIn;
            this.resManager = resManager;
            comparer.column = 0;

            this.ontologyMappingPrx = prx;
                        
            this.mapping = ontologyMappingPrx.getMapping();
            this.separatorInner = ontologyMappingPrx.getMappingSeparatorInner();
            this.separatorOuter = ontologyMappingPrx.getMappingSeparatorOuter();

            InitializeComponent();
            this.ListViewInit();
            this.MakeListView();
            
            this.ToolStripMenuItemCopyAll.Click += new EventHandler(ToolStripMenuItemCopyAll_Click);
            this.ToolStripMenuItemCopySelected.Click += new EventHandler(ToolStripMenuItemCopySelected_Click);
        }

        #endregion

        #region Context menu handlers

        /// <summary>
        /// Copy selected items to clipboard
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void ToolStripMenuItemCopySelected_Click(object sender, EventArgs e)
        {
            StringBuilder copyString = new StringBuilder();
            copyString.Append(this.resManager.GetString("ColumnDataTable") + "\t" +
                this.resManager.GetString("ColumnColumn") + "\t" +
                this.resManager.GetString("ColumnOntologyEntity") + "\t" +
                this.resManager.GetString("ColumnCardinality") + "\t" +
                this.resManager.GetString("ColumnMinimum") + "\t" +
                this.resManager.GetString("ColumnMaximum") + "\t" +
                this.resManager.GetString("ColumnDomainDividingValues") + "\t" +
                this.resManager.GetString("ColumnDistinctValues"));
            copyString.AppendLine();

            foreach (ListViewItem item in this.ShowMappingListView.SelectedItems)
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
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void ToolStripMenuItemCopyAll_Click(object sender, EventArgs e)
        {
            StringBuilder copyString = new StringBuilder();
            copyString.Append(this.resManager.GetString("ColumnDataTable") + "\t" +
                this.resManager.GetString("ColumnColumn") + "\t" +
                this.resManager.GetString("ColumnOntologyEntity") + "\t" +
                this.resManager.GetString("ColumnCardinality") + "\t" +
                this.resManager.GetString("ColumnMinimum") + "\t" +
                this.resManager.GetString("ColumnMaximum") + "\t" +
                this.resManager.GetString("ColumnDomainDividingValues") + "\t" +
                this.resManager.GetString("ColumnDistinctValues"));

            copyString.AppendLine();

            foreach (ListViewItem item in this.ShowMappingListView.Items)
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
        /// Shows the help file for this module for interaction
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        private void ToolStripHelp_Click(object sender, EventArgs e)
        {
            ownerOfAddIn.OpenPdf(ownerOfAddIn.GetBinPath() + "\\AddIns\\Help\\ShowMapping.pdf");
        }

        #endregion

        #region Other private methods

        /// <summary>
        /// Method to fill ListView with ColumnSchemaInfo data
        /// </summary>
        private void MakeListView()
        {
            string[] tmpMappedPairs = mapping.Split(new string[] { this.separatorOuter }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string tmpMappedPair in tmpMappedPairs)
            {
                string[] DataTable_Column_OntEnt = tmpMappedPair.Split(new string[] { this.separatorInner }, StringSplitOptions.RemoveEmptyEntries);
                ListViewItem newItem = new ListViewItem();
                newItem.Text = DataTable_Column_OntEnt[0];
                newItem.SubItems.Add(DataTable_Column_OntEnt[1]);
                newItem.SubItems.Add(DataTable_Column_OntEnt[2]);

                Dictionary<string, string> dataPropertyValues = new Dictionary<string, string>();
                /// order must copy the order of columns in listview
                /// names (keys) must be the same as the standard names in ontology
                dataPropertyValues.Add("Cardinality", "");
                dataPropertyValues.Add("Minimum", "");
                dataPropertyValues.Add("Maximum", "");
                dataPropertyValues.Add("DomainDividingValues", "");
                dataPropertyValues.Add("DistinctValues", "");

                StrSeqMap dataProperties = ontologyMappingPrx.getOntologyEntityProperties(DataTable_Column_OntEnt[0], DataTable_Column_OntEnt[1]);

                if (dataProperties != null)
                {
                    foreach (string tmpStr in dataProperties.Keys)
                    {
                        int i = 1;
                        if (dataProperties[tmpStr] != null)
                        {
                            string tmpAllValues = "";
                            foreach (string tmpValue in dataProperties[tmpStr])
                            {
                                //empty values are ignored
                                if (tmpValue != "")
                                {
                                    tmpAllValues += tmpValue;
                                    if (dataProperties[tmpStr].Length > i++)
                                        tmpAllValues += dataPropertyValuesSeparator;
                                }
                            }
                            /// removing unwanted separator at the end of the string
                            if (tmpAllValues != "")
                            {
                                tmpAllValues = tmpAllValues.Remove(tmpAllValues.Length - dataPropertyValuesSeparator.Length);
                            }

                            /// inserting the value of data property into dictionary
                            dataPropertyValues[tmpStr] = tmpAllValues;
                        }
                    }
                }

                /// adding data properties into listView row
                /// I assume, that the order of the keys is same as the order of columns in listview (beginning with the 4th column)
                foreach (string tmpstr in dataPropertyValues.Keys)
                {
                    newItem.SubItems.Add(dataPropertyValues[tmpstr]);
                }

                /// adding annotations into listView row
                string[] annotations = ontologyMappingPrx.getOntologyEntityAnnotations(DataTable_Column_OntEnt[0], DataTable_Column_OntEnt[1]);
                string annotationAll = "";
                foreach (string annotation in annotations)
                {
                    if (annotation != "")
                        annotationAll += annotation + '\n';
                }
                //removing last unwanted char
                annotationAll = annotationAll.Remove(annotationAll.Length - 1);
                newItem.SubItems.Add(annotationAll);

                /// adding new row into listview
                this.ShowMappingListView.Items.Add(newItem);
            }
        }

        /// <summary>
        /// Method for ColumnFrListView init
        /// </summary>
        private void ListViewInit()
        {
            this.ChangeLocale(resManager);

            //adding a handling method for column sorting
            this.ShowMappingListView.ColumnClick += 
                new ColumnClickEventHandler(this.ShowMappingListView_ColumnClick);
        }

        /// <summary>
        /// Handler for column click - sorts a listview.
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        private void ShowMappingListView_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            comparer.column = e.Column;
            if (ShowMappingListView.Sorting == SortOrder.Ascending)
            {
                comparer.bAscending = false;
                ShowMappingListView.Sorting = SortOrder.Descending;
            }
            else
            {
                comparer.bAscending = true;
                ShowMappingListView.Sorting = SortOrder.Ascending;
            }
            ShowMappingListView.ListViewItemSorter = comparer;
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
        /// <param name="rm">Resource manager to handle new l10n resource</param>
        private void ChangeLocale(ResourceManager rm)
        {
            this.ColumnDataTable.Text = rm.GetString("ColumnDataTable");
            this.ColumnColumn.Text = rm.GetString("ColumnColumn");
            this.ColumnOntologyEntity.Text = rm.GetString("ColumnOntologyEntity");
            this.ColumnCardinality.Text = rm.GetString("ColumnCardinality");
            this.ColumnMinimum.Text = rm.GetString("ColumnMinimum");
            this.ColumnMaximum.Text = rm.GetString("ColumnMaximum");
            this.ColumnDomainDividingValues.Text = rm.GetString("ColumnDomainDividingValues");
            this.ColumnDistinctValues.Text = rm.GetString("ColumnDistinctValues");
            this.ColumnAnnotations.Text = rm.GetString("ColumnAnnotations");
            this.ToolStripMenuItemCopySelected.Text = rm.GetString("CopySelectedToClipboard");
            this.ToolStripMenuItemCopyAll.Text = rm.GetString("CopyAllToClipboard");
            this.ToolStripHelp.Text = rm.GetString("Help");
        }

        #endregion
    }
}