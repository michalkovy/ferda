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
        /// Path to Ferda ontology mapping (.fom) file, which was most recently used for Load or Save
        /// </summary>
        string pathToOntologyMappingFile = "";

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
        public SetOntologyMappingControl(Dictionary<string, string[]> dataTableColumnsDictionary, OntologyStructure ontology, string separatorInner, string separatorOuter, string inputMapping, string[] localePrefs, IOwnerOfAddIn ownerOfAddIn)
        {
            this.dataTableColumnsDictionary = dataTableColumnsDictionary;
            this.ontology = ontology;
            this.separatorInner = separatorInner;
            this.separatorOuter = separatorOuter;

            InitializeComponent();

            this.RefreshMappingViews(inputMapping);
        }

        #region Other private methods

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
        /// Method which tests if it is possible to map data table column name and ontology entity name and maps them
        /// </summary>
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

        private void loadButton_Click(object sender, EventArgs e)
        {
            if (LoadMappingDialog.ShowDialog() == DialogResult.OK)
            {
                TextReader tr = new StreamReader(LoadMappingDialog.OpenFile());
                string tmpMapping = tr.ReadToEnd();
                RefreshMappingViews(tmpMapping);
                tr.Close();
                pathToOntologyMappingFile = LoadMappingDialog.FileName;
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            saveMappingDialog.FileName = pathToOntologyMappingFile;
            if (saveMappingDialog.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(saveMappingDialog.OpenFile());
                sw.Write(generateMappingFromListView());
                sw.Close();
            }
        }

        private string generateMappingFromListView()
        {
            string tmpMapping = "";
            foreach (MappedPair mappedPair in this.MappingListBox.Items)
            {
                tmpMapping += mappedPair.DataTableName + this.separatorInner + mappedPair.DataTableColumnName + this.separatorInner + mappedPair.OntologyEntityName + this.separatorOuter;
            }
            return tmpMapping;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            returnMapping = generateMappingFromListView();
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

        public MappedPair() { }

        public MappedPair(string dataTableName, string dataTableColumnName, TreeNode columnPositionInTreeView,  string ontologyEntityName)
        {
            DataTableName = dataTableName;
            DataTableColumnName = dataTableColumnName;
            ColumnPositionInTreeView = columnPositionInTreeView;
            OntologyEntityName = ontologyEntityName;
        }

        public override string ToString()
        {
            return DataTableName + "." + DataTableColumnName + " - " + OntologyEntityName;
        }
    }
}