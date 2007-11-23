using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ferda.Guha.Data;
using Ferda.OntologyRelated.generated.OntologyData;

namespace Ferda.FrontEnd.AddIns.SetOntologyMapping
{
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
        private OntologyStructure ontology;

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

        //public SetOntologyMappingControl()
        public SetOntologyMappingControl(Dictionary<string, string[]> dataTableColumnsDictionary, OntologyStructure ontology, string[] localePrefs, IOwnerOfAddIn ownerOfAddIn)
        {
            this.dataTableColumnsDictionary = dataTableColumnsDictionary;
            this.ontology = ontology;

            InitializeComponent();

            this.MakeDataTablesTreeView();
            this.MakeOntologyTreeView();
        }

        #region Other private methods

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
                if (ontologyClass.SuperClasses[0] == "")
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
            foreach (string subClassName in ontologyClass.SubClasses)
            {
                if (subClassName != "")
                {
                    newNode = (node.Nodes.Add(subClassName));
                    OntologyClass subClass = this.ontology.OntologyClassMap[subClassName];
                    AddSubClassesNodes(subClass, newNode);
                }
            }
            return;
        }

        /*
         * for(String className : FerdaOntology.OntologyClassMap.keySet()) {
				System.out.println(className.toString() + ": ");
				
				System.out.println("\tannotations: ");
				for(String annotation : FerdaOntology.OntologyClassMap.get(className).Annotations) {
					if (!annotation.isEmpty()) System.out.println("\t\t" + annotation.toString());
				}
				System.out.println("\tsubclasses: ");
				for(String subClass : FerdaOntology.OntologyClassMap.get(className).SubClasses) {
					if (!subClass.isEmpty()) System.out.println("\t\t" + subClass.toString());
				}
				System.out.println("\tsuperclasses: ");
				for(String superClass : FerdaOntology.OntologyClassMap.get(className).SuperClasses) {
					if (!superClass.isEmpty()) System.out.println("\t\t" + superClass.toString());
				}
				
				System.out.println("\tdataproperties: ");
				
				for(String dataPropertyName : FerdaOntology.OntologyClassMap.get(className).DataPropertiesMap.keySet()) {
					System.out.println("\t\t" + dataPropertyName.toString() + ": ");
					for(String dataProperty : FerdaOntology.OntologyClassMap.get(className).DataPropertiesMap.get(dataPropertyName)) {
						if (!dataProperty.isEmpty()) System.out.println("\t\t\t" + dataProperty.toString());
					}
				}
				System.out.println("---------");
			}
         * */

        #endregion

        /// <summary>
        /// Method which maps data table column name and ontology entity name
        /// </summary>
        private void mapButton_Click(object sender, EventArgs e)
        {
            if (this.dataTablesTreeView.SelectedNode != null && this.ontologyTreeView.SelectedNode != null)
            {
                if (this.ontologyTreeView.SelectedNode.NodeFont != null && this.ontologyTreeView.SelectedNode.NodeFont.Strikeout)
                {
                    this.infoBox.Text = "You are trying to map an entity which is already mapped.";
                }
                else if (this.dataTablesTreeView.SelectedNode.Parent == null)
                {
                    this.infoBox.Text = "You can only map the columns names of data tables, not data tables names themselves.";
                }
                else
                {
                    this.MappingListBox.Items.Add(new MappedPair(this.dataTablesTreeView.SelectedNode.Parent.Text, this.dataTablesTreeView.SelectedNode.Text, this.dataTablesTreeView.SelectedNode, this.ontologyTreeView.SelectedNode.Text, this.ontologyTreeView.SelectedNode));
                    this.infoBox.Text = "You have mapped pair: " + this.dataTablesTreeView.SelectedNode.Parent.Text + "." + this.dataTablesTreeView.SelectedNode.Text + " - " + this.ontologyTreeView.SelectedNode.Text + ".";
                    this.dataTablesTreeView.SelectedNode.NodeFont = new Font(this.ontologyTreeView.Font, FontStyle.Strikeout);
                    this.ontologyTreeView.SelectedNode.NodeFont = new Font(this.ontologyTreeView.Font, FontStyle.Strikeout);
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
        private void unmapButton_Click(object sender, EventArgs e)
        {
            if (this.MappingListBox.SelectedItem != null)
            {
                MappedPair tmpPair = (MappedPair)this.MappingListBox.SelectedItem;
                tmpPair.ColumnPositionInTreeView.NodeFont = this.dataTablesTreeView.Font;
                tmpPair.OntologyPositionInTreeView.NodeFont = this.ontologyTreeView.Font;
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

        private void okButton_Click(object sender, EventArgs e)
        {
            returnMapping = "";
            foreach (MappedPair mappedPair in this.MappingListBox.Items)
            {
                returnMapping += mappedPair.DataTableName + "." + mappedPair.DataTableColumnName + "\t" + mappedPair.OntologyEntityName + "\n";
            }
            this.DialogResult = DialogResult.OK;
            this.Dispose();
            return;
        }
    }

    public class DataTableColumn
    {
        public string Name;

        public DataTableColumn() { }

        public DataTableColumn(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class MappedPair
    {
        public string DataTableName;
        public string DataTableColumnName;
        public string OntologyEntityName;
        public TreeNode ColumnPositionInTreeView;
        public TreeNode OntologyPositionInTreeView;

        public MappedPair() { }

        public MappedPair(string dataTableName, string dataTableColumnName, TreeNode columnPositionInTreeView,  string ontologyEntityName, TreeNode ontologyPositionInTreeView)
        {
            DataTableName = dataTableName;
            DataTableColumnName = dataTableColumnName;
            ColumnPositionInTreeView = columnPositionInTreeView;
            OntologyEntityName = ontologyEntityName;
            OntologyPositionInTreeView = ontologyPositionInTreeView;
        }

        public override string ToString()
        {
            return DataTableName + "." + DataTableColumnName + " - " + OntologyEntityName;
        }
    }
}