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
        /// Array of structures that holds inrofmation about datatable columns
        /// </summary>
        private ColumnExplain[] columnExplains;
        private OntologyStructure ontology;

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
            this.MakeOntologyListBox();
        }

        #region Other private methods

        /// <summary>
        /// Method to fill ListView with ColumnSchemaInfo data
        /// </summary>
        private void MakeDataTableListBox()
        {
            foreach (ColumnExplain columnInfo in columnExplains)
            {
                this.DataTableListBox.Items.Add(columnInfo.name);
            }
        }

        /// <summary>
        /// Method to fill ListView with ColumnSchemaInfo data
        /// </summary>
        private void MakeOntologyListBox()
        {
            foreach (String className in this.ontology.OntologyClassMap.Keys)
            {
                this.OntologyListBox.Items.Add(className.ToString());
            }
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

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            //this.Dispose();
            return;
        }
    }
}