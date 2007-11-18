using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ferda.Guha.Data;
using Ferda.OntologyRelated.generated.OntologyData;

namespace Ferda.FrontEnd.AddIns.OntologyMapping
{
    public partial class OntologyMappingControl : Form
    {
        #region Private variables

        /// <summary>
        /// Localization string - for now, en-US or cs-CZ
        /// </summary>
        private string localizationString;

        /// <summary>
        /// Resulting Ontology Mapping
        /// </summary>
        private string returnOntologyMapping;

        /// <summary>
        /// Array of structures that holds inrofmation about datatable columns and ontology
        /// </summary>
        private ColumnExplain[] columnExplains;
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
        public string ReturnOntologyMapping
        {
            get
            {
                return returnOntologyMapping;
            }
        }

        #endregion

        //public OntologyMappingControl()
        public OntologyMappingControl(ColumnExplain[] columnExplain, OntologyStructure ontology)
        {
            this.columnExplains = columnExplain;
            this.ontology = ontology;

            InitializeComponent();

            this.MakeDataTableListBox();
            this.MakeOntologyTreeView();
        }

        #region Other private methods

        /// <summary>
        /// Method to fill ListView with ColumnSchemaInfo data
        /// </summary>
        private void MakeDataTableListBox()
        {
            foreach (ColumnExplain columnInfo in columnExplains)
            {
                this.DataTableListBox.Items.Add(new DataTableColumn(columnInfo.name.ToString()));
            }
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
        /// Method to add subclasses of ontology class to OntologyTree
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

        private void mapButton_Click(object sender, EventArgs e)
        {
            if (this.DataTableListBox.SelectedItem != null && this.ontologyTreeView.SelectedNode != null)
            {
                if (this.ontologyTreeView.SelectedNode.NodeFont != null && this.ontologyTreeView.SelectedNode.NodeFont.Strikeout)
                {
                    this.infoBox.Text = "You are trying to map an entity which is already mapped.";
                }
                else
                {
                    DataTableColumn tmpColumn = (DataTableColumn)this.DataTableListBox.SelectedItem;
                    this.MappingListBox.Items.Add(new MappedPair(tmpColumn.ToString(), this.ontologyTreeView.SelectedNode.Text, this.ontologyTreeView.SelectedNode));
                    this.infoBox.Text = "You mapped pair: " + tmpColumn.ToString() + " - " + this.ontologyTreeView.SelectedNode.Text + ".";
                    this.DataTableListBox.Items.Remove(tmpColumn);
                    this.ontologyTreeView.SelectedNode.NodeFont = new Font(this.ontologyTreeView.Font, FontStyle.Strikeout);
                }
            }
            else
            {
                this.infoBox.Text = "One datatable column and one ontology class must be selected to add the pair into mapping.";
            }
            return;
        }

        private void ontologyTreeView_Leave(object sender, EventArgs e)
        {
            if (this.ontologyTreeView.SelectedNode != null)
            {
                this.ontologyTreeView.SelectedNode.BackColor = this.colorDefaultSelectedBack;
                this.ontologyTreeView.SelectedNode.ForeColor = this.colorDefaultSelectedFore;
            }
        }

        private void ontologyTreeView_Enter(object sender, EventArgs e)
        {
            if (this.ontologyTreeView.SelectedNode != null)
            {
                this.ontologyTreeView.SelectedNode.BackColor = this.colorDefaultNotSelectedBack;
                this.ontologyTreeView.SelectedNode.ForeColor = this.colorDefaultNotSelectedFore;
            }
        }

        private void unmapButton_Click(object sender, EventArgs e)
        {
            if (this.MappingListBox.SelectedItem != null)
            {
                MappedPair tmpPair = (MappedPair)this.MappingListBox.SelectedItem;
                this.DataTableListBox.Items.Add(new DataTableColumn(tmpPair.DataTableColumnName.ToString()));
                tmpPair.OntologyPositionInTreeView.NodeFont = this.ontologyTreeView.Font;
                this.MappingListBox.Items.Remove(tmpPair);
                this.infoBox.Text = "You have ummapped pair: " + tmpPair.ToString() + ".";
            }
            else
            {
                this.infoBox.Text = "You must select a mapped pair to remove (unmap) it.";
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            //TODO ulozeni vsech MappedPairs z MappingListBox do parametru krabicky
            //this.returnOntologyPath = URLPathTextBox.Text;
            this.DialogResult = DialogResult.OK;
            this.Dispose();
            return;
        }

        private void ColumnsRemoveButton_Click(object sender, EventArgs e)
        {
            if (this.DataTableListBox.SelectedItem != null)
            {
                DataTableColumn tmpColumn = (DataTableColumn)this.DataTableListBox.SelectedItem;
                string tmpString = tmpColumn.Name;
                this.DataTableListBox.Items.Remove(tmpColumn);
                this.infoBox.Text = "You have removed datatable column " + tmpString + " from the list.";
            }
            else
            {
                this.infoBox.Text = "You must select a column name to remove it from the list.";
            }
        }

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
        public string DataTableColumnName;
        public string OntologyEntityName;
        public TreeNode OntologyPositionInTreeView;

        public MappedPair() { }

        public MappedPair(string dataTableColumnName, string ontologyEntityName, TreeNode ontologyPositionInTreeView)
        {
            DataTableColumnName = dataTableColumnName;
            OntologyEntityName = ontologyEntityName;
            OntologyPositionInTreeView = ontologyPositionInTreeView;
        }

        public override string ToString()
        {
            return DataTableColumnName + " - " + OntologyEntityName;
        }
    }
}