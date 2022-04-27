// SetOntologyMappingControl.cs - database - ontology mapping setting module
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
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ferda.Guha.Data;
using Ferda.OntologyRelated.generated.OntologyData;
using Ferda.Guha.MiningProcessor.Results;
using Ice;

namespace Ferda.FrontEnd.AddIns.SetOntologyMapping
{
    /// <summary>
    /// The SetOntology mapping (DatabaseOntology Mapping) setting module
    /// </summary>
    public partial class SetOntologyMappingControl : Form
    {
        #region Private variables

        /// <summary>
        /// Localization string - for now, en-US or cs-CZ
        /// </summary>
        private string localizationString;

        /// <summary>
        /// Resulting Mapping
        /// </summary>
        private string returnMapping;

        /// <summary>
        /// Array of structures that holds inrofmation about datatables, its columns and ontology
        /// </summary>
        Dictionary<string, string[]> dataTableColumnsDictionary;

        /// <summary>
        /// Information about ontology
        /// </summary>
        private OntologyStructure ontology;

        /// <summary>
        /// Char which separates strings of mapped pairs (triples if datatable name is count separately)
        /// </summary>
        private string separatorOuter;

        /// <summary>
        /// Char which separates datatable name, column name and ontology entity name
        /// </summary>
        private string separatorInner;

        /// <summary>
        /// Path to Ferda ontology mapping (.xml) file, which was most recently used for Load or Save
        /// </summary>
        private string pathToOntologyMappingFile = "";

        /// <summary>
        /// Owner of addin
        /// </summary>
        private IOwnerOfAddIn ownerOfAddIn;

        /// <summary>
        /// Colors which are used in ontologyTreeView
        /// </summary>
        private Color colorDefaultNotSelectedBack = Color.White;
        private Color colorDefaultNotSelectedFore = Color.Black;
        private Color colorDefaultSelectedBack = Color.FromName("Highlight");
        private Color colorDefaultSelectedFore = Color.White;
        private Color colorMappedBack = Color.Gray;
        private Color colorMappedFore = Color.White;

        #endregion

        #region Public properties

        /// <summary>
        /// Resulting DSN
        /// </summary>
        public string ReturnMapping
        {
            get
            {
                return returnMapping;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor of the class
        /// </summary>
        /// <param name="dataTableColumnsDictionary">
        /// Array of structures that holds inrofmation about datatables, its columns and ontology
        /// </param>
        /// <param name="ontology">Information about ontology</param>
        /// <param name="separatorInner">
        /// Char which separates datatable name, column name and ontology entity name
        /// </param>
        /// <param name="separatorOuter">
        /// Char which separates datatable name, column name and ontology entity name
        /// </param>
        /// <param name="inputMapping">
        /// The mapping that already exist
        /// </param>
        /// <param name="localePrefs">
        /// Localization preferencees
        /// </param>
        /// <param name="ownerOfAddIn">
        /// Owner of add in (FrontEnd)
        /// </param>
        public SetOntologyMappingControl(Dictionary<string, string[]> dataTableColumnsDictionary, OntologyStructure ontology, string separatorInner, string separatorOuter, string inputMapping, string[] localePrefs, IOwnerOfAddIn ownerOfAddIn)
        {
            this.dataTableColumnsDictionary = dataTableColumnsDictionary;
            this.ontology = ontology;
            this.separatorInner = separatorInner;
            this.separatorOuter = separatorOuter;
            this.ownerOfAddIn = ownerOfAddIn;

            InitializeComponent();

            this.RefreshMappingViews(inputMapping);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Method to fill ColumnsTree with DataTable names and its columns
        /// </summary>
        private void RefreshMappingViews(string mapping)
        {
            dataTablesTreeView.Nodes.Clear();
            ontologyTreeView.Nodes.Clear();
            MappingListBox.Items.Clear();

            this.MakeDataTablesTreeView();
            this.MakeOntologyTreeView();
            
            if (mapping != null && mapping != "") {
                string[] tmpMappedPairs = mapping.Split(new string[] { this.separatorOuter }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string tmpMappedPair in tmpMappedPairs)
                {
                    string[] DataTable_Column_OntEnt = tmpMappedPair.Split(new string[] { this.separatorInner }, StringSplitOptions.RemoveEmptyEntries);
                    //all three parts of the mapped pair must exist
                    if (DataTable_Column_OntEnt.Length == 3)
                        mapPair(DataTable_Column_OntEnt[0], DataTable_Column_OntEnt[1], DataTable_Column_OntEnt[2]);
                }
            }
            return;

        }

        /// <summary>
        /// Method to fill ColumnsTree with DataTable names and its columns
        /// </summary>
        private void MakeDataTablesTreeView()
        {
            TreeNode rootNode;

            foreach (string dataTableName in this.dataTableColumnsDictionary.Keys)
            {
                rootNode = dataTablesTreeView.Nodes.Add(dataTableName);
                AddColumnsNodes(dataTableColumnsDictionary[dataTableName], rootNode);
            }
            return;

        }

        /// <summary>
        /// Method to add columns names into dataTablesTreeView
        /// </summary>
        private void AddColumnsNodes(string[] columnsNames, TreeNode node)
        {
            TreeNode newNode;
            foreach (string columnName in columnsNames)
            {
                newNode = node.Nodes.Add(columnName);
            }
            return;
        }

        /// <summary>
        /// Method to fill OntologyTree with Ontology entities
        /// </summary>
        private void MakeOntologyTreeView()
        {
            TreeNode rootNode;

            foreach (OntologyClass ontologyClass in this.ontology.OntologyClassMap.Values)
            {
                if (ontologyClass.SuperClasses.Length == 0)
                {
                    rootNode = ontologyTreeView.Nodes.Add(ontologyClass.name.ToString());

                    AddSubClassesNodes(ontologyClass, rootNode);
                }
            }
            return;

        }

        /// <summary>
        /// Method to add subclasses of ontology class to OntologyTreeView
        /// </summary>
        private void AddSubClassesNodes(OntologyClass ontologyClass, TreeNode node)
        {
            TreeNode newNode;

            //adding the instances of class into the treeview
            AddInstancesNodes(ontologyClass, node);

            foreach (string subClassName in ontologyClass.SubClasses)
            {
                newNode = (node.Nodes.Add(subClassName));

                OntologyClass subClass = this.ontology.OntologyClassMap[subClassName];
                AddSubClassesNodes(subClass, newNode);
            }
            return;
        }

        /// <summary>
        /// Method to add instances of ontology class to OntologyTreeView
        /// </summary>
        private void AddInstancesNodes(OntologyClass ontologyClass, TreeNode node)
        {
            TreeNode newNodeInstance;

            //adding the instances of class into the treeview
            foreach (string instance in ontologyClass.InstancesAnnotations.Keys)
            {
                if (instance != "")
                {
                    newNodeInstance = (node.Nodes.Add(instance));
                    newNodeInstance.NodeFont = new Font(this.dataTablesTreeView.Font, FontStyle.Italic);
                }
            }

            return;
        }

        /// <summary>
        /// Method which maps data table column name and ontology entity name
        /// </summary>
        private void mapPair(string dataTableName, string columnName, string ontologyEntityName)
        {
            TreeNode soughtColumnNode = new TreeNode();
            foreach (TreeNode node in this.dataTablesTreeView.Nodes)
            {
                if (node.Text == dataTableName)
                {
                    foreach (TreeNode columnNode in node.Nodes)
                    {
                        if (columnNode.Text == columnName)
                            soughtColumnNode = columnNode;
                    }
                }
            }
            //test if the column was found during loading of mapping it is possible, that some columns does't exist
            if (soughtColumnNode.Text != "")
            {
                this.MappingListBox.Items.Add(new MappedPair(dataTableName, columnName, soughtColumnNode, ontologyEntityName));
                this.infoBox.Text = "You have mapped pair: " + dataTableName + "." + columnName + " - " + ontologyEntityName + ".";
                soughtColumnNode.NodeFont = new Font(this.dataTablesTreeView.Font, FontStyle.Strikeout);
            }
        }

        /// <summary>
        /// Generates XML mapping from the list view
        /// </summary>
        /// <returns>string that contains the XML mapping</returns>
        private string generateXMLMappingFromListView()
        {
            string tmpMapping = "";
            /// <summary>
            /// Serializable ontology mapping
            /// </summary>
            SerializableOntologyMapping ontologyMapping = new SerializableOntologyMapping();

            ontologyMapping.FerdaOntologyMapping = new Ferda.Guha.MiningProcessor.Results.MappedPair[this.MappingListBox.Items.Count];

            int i = 0;
            foreach (MappedPair mappedPair in this.MappingListBox.Items)
            {
                ontologyMapping.FerdaOntologyMapping[i].DataTableName = mappedPair.DataTableName;
                ontologyMapping.FerdaOntologyMapping[i].DataTableColumnName = mappedPair.DataTableColumnName;
                ontologyMapping.FerdaOntologyMapping[i].OntologyEntityName = mappedPair.OntologyEntityName;
                i++;
            }
            tmpMapping = SerializableOntologyMapping.Serialize(ontologyMapping);
            return tmpMapping;
        }

        /// <summary>
        /// Generates the mapping that is passed to the mapping box. This mapping string is different to the
        /// XML mapping that is being serialized. 
        /// </summary>
        /// <returns>Mapping in a string format</returns>
        private string generateMappingFromListView()
        {
            string tmpMapping = "";

            foreach (MappedPair mappedPair in this.MappingListBox.Items)
            {
                tmpMapping += mappedPair.DataTableName + this.separatorInner + mappedPair.DataTableColumnName + this.separatorInner + mappedPair.OntologyEntityName + this.separatorOuter;
            }

            return tmpMapping;
        }

        #endregion

        #region Events

        /// <summary>
        /// Method which tests if it is possible to map data table column name and ontology entity name and maps them
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        private void mapButton_Click(object sender, EventArgs e)
        {
            if (this.dataTablesTreeView.SelectedNode != null && this.ontologyTreeView.SelectedNode != null)
            {
                if (this.dataTablesTreeView.SelectedNode.NodeFont != null && this.dataTablesTreeView.SelectedNode.NodeFont.Strikeout)
                {
                    this.infoBox.Text = "You are trying to map a column which is already mapped.";
                }
                else if (this.dataTablesTreeView.SelectedNode.Parent == null)
                {
                    this.infoBox.Text = "You can only map the columns names of data tables, not data tables names themselves.";
                }
                else
                {
                    this.MappingListBox.Items.Add(new MappedPair(this.dataTablesTreeView.SelectedNode.Parent.Text, this.dataTablesTreeView.SelectedNode.Text, this.dataTablesTreeView.SelectedNode, this.ontologyTreeView.SelectedNode.Text));
                    this.infoBox.Text = "You have mapped pair: " + this.dataTablesTreeView.SelectedNode.Parent.Text + "." + this.dataTablesTreeView.SelectedNode.Text + " - " + this.ontologyTreeView.SelectedNode.Text + ".";
                    this.dataTablesTreeView.SelectedNode.NodeFont = new Font(this.dataTablesTreeView.Font, FontStyle.Strikeout);
                }
            }
            else
            {
                this.infoBox.Text = "One datatable column and one ontology entity must be selected to add the pair into mapping.";
            }
            return;
        }

        /// <summary>
        /// Method which unmaps mapped pair
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        private void unmapButton_Click(object sender, EventArgs e)
        {
            if (this.MappingListBox.SelectedItem != null)
            {
                MappedPair tmpPair = (MappedPair)this.MappingListBox.SelectedItem;
                tmpPair.ColumnPositionInTreeView.NodeFont = this.dataTablesTreeView.Font;
                this.MappingListBox.Items.Remove(tmpPair);
                this.infoBox.Text = "You have ummapped pair: " + tmpPair.ToString() + ".";
            }
            else
            {
                this.infoBox.Text = "You must select a mapped pair to remove (unmap) it.";
            }
        }

        /// <summary>
        /// Method which returns node normal behaviour (colors) to the selected when focus is returned to the treeView
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        private void dataTablesTreeView_Enter(object sender, EventArgs e)
        {
            if (this.dataTablesTreeView.SelectedNode != null)
            {
                this.dataTablesTreeView.SelectedNode.BackColor = this.colorDefaultNotSelectedBack;
                this.dataTablesTreeView.SelectedNode.ForeColor = this.colorDefaultNotSelectedFore;
            }
        }

        /// <summary>
        /// Method which ensures, that a selected node will be always visible even if the treeView lost the focus
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        private void dataTablesTreeView_Leave(object sender, EventArgs e)
        {
            if (this.dataTablesTreeView.SelectedNode != null)
            {
                this.dataTablesTreeView.SelectedNode.BackColor = this.colorDefaultSelectedBack;
                this.dataTablesTreeView.SelectedNode.ForeColor = this.colorDefaultSelectedFore;
            }
        }

        /// <summary>
        /// Method which returns node normal behaviour (colors) to the selected when focus is returned to the treeView
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        private void ontologyTreeView_Enter(object sender, EventArgs e)
        {
            if (this.ontologyTreeView.SelectedNode != null)
            {
                this.ontologyTreeView.SelectedNode.BackColor = this.colorDefaultNotSelectedBack;
                this.ontologyTreeView.SelectedNode.ForeColor = this.colorDefaultNotSelectedFore;
            }
        }

        /// <summary>
        /// Method which ensures, that a selected node will be always visible even if the treeView lost the focus
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        private void ontologyTreeView_Leave(object sender, EventArgs e)
        {
            if (this.ontologyTreeView.SelectedNode != null)
            {
                this.ontologyTreeView.SelectedNode.BackColor = this.colorDefaultSelectedBack;
                this.ontologyTreeView.SelectedNode.ForeColor = this.colorDefaultSelectedFore;
            }
        }

        /// <summary>
        /// Method which removes selected node from the dataTablesTreeView for better lucidity
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        private void ColumnsRemoveButton_Click(object sender, EventArgs e)
        {
            if (this.dataTablesTreeView.SelectedNode != null)
            {
                string tmpString = this.dataTablesTreeView.SelectedNode.Text;
                bool isDataTable = (this.dataTablesTreeView.SelectedNode.Parent == null) ? true : false;
                this.dataTablesTreeView.SelectedNode.Remove();
                if (isDataTable)
                    this.infoBox.Text = "You have removed datatable name " + tmpString + " and its columns from the tree.";
                else
                    this.infoBox.Text = "You have removed column name " + tmpString + " from the tree.";
            }
            else
            {
                this.infoBox.Text = "You must select datatable or column to remove it from the list.";
            }
        }

        /// <summary>
        /// Method which removes selected node from the ontologyTreeView for better lucidity
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        private void OntologyRemoveButton_Click(object sender, EventArgs e)
        {
            if (this.ontologyTreeView.SelectedNode != null)
            {
                string tmpString = this.ontologyTreeView.SelectedNode.Text;
                this.ontologyTreeView.SelectedNode.Remove();
                this.infoBox.Text = "You have removed ontology entity " + tmpString + " and its siblings from the tree.";
            }
            else
            {
                this.infoBox.Text = "You must select an ontology entity name to remove it from the list.";
            }
        }

        /// <summary>
        /// Loanding mapping from an XML file
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        private void loadButton_Click(object sender, EventArgs e)
        {
            if (LoadMappingDialog.ShowDialog() == DialogResult.OK)
            {
                TextReader tr = new StreamReader(LoadMappingDialog.OpenFile());
                string tmpMapping = tr.ReadToEnd();
                
                /// convert from xml to string form
                
                /// <summary>
                /// Serializable ontology mapping
                /// </summary>
                SerializableOntologyMapping ontologyMapping = new SerializableOntologyMapping();

                ontologyMapping = SerializableOntologyMapping.Deserialize(tmpMapping);

                tmpMapping = "";

                foreach (Ferda.Guha.MiningProcessor.Results.MappedPair mappedPair in ontologyMapping.FerdaOntologyMapping)
                {
                    tmpMapping += mappedPair.DataTableName + this.separatorInner + mappedPair.DataTableColumnName + this.separatorInner + mappedPair.OntologyEntityName + this.separatorOuter;
                }
                
                RefreshMappingViews(tmpMapping);
                tr.Close();
                pathToOntologyMappingFile = LoadMappingDialog.FileName;
            }
        }

        /// <summary>
        /// Saving the mapping to a XML file
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>        
        private void saveButton_Click(object sender, EventArgs e)
        {
            saveMappingDialog.FileName = pathToOntologyMappingFile;
            if (saveMappingDialog.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(saveMappingDialog.OpenFile());
                sw.Write(generateXMLMappingFromListView());
                sw.Close();
            }
        }

        /// <summary>
        /// The OK button click. It returnes the mapping to the mapping box. 
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>  
        private void okButton_Click(object sender, EventArgs e)
        {
            returnMapping = generateMappingFromListView();
            this.DialogResult = DialogResult.OK;
            this.Dispose();
            return;
        }

        /// <summary>
        /// Displays the PDF help for the module.
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        private void helpButton_Click(object sender, EventArgs e)
        {
            ownerOfAddIn.OpenPdf(ownerOfAddIn.GetBinPath() + "\\AddIns\\Help\\DatabaseOntologyMappingUser.pdf");
        }

        #endregion
    }
}