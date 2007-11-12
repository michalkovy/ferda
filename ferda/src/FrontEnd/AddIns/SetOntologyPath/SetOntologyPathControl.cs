using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ferda.FrontEnd.AddIns.SetOntologyPath
{
    public partial class SetOntologyPathControl : Form
    {

        #region Private variables

        /// <summary>
        /// Localization string - for now, en-US or cs-CZ
        /// </summary>
        private string localizationString;

        /// <summary>
        /// Resulting Ontology Path
        /// </summary>
        private string returnOntologyPath;

        /// <summary>
        /// Current path (for icon loading)
        /// </summary>
        private string path;
        /*
        /// <summary>
        /// Owner of addin
        /// </summary>
        private IOwnerOfAddIn ownerOfAddIn;
        */
        #endregion

        #region Public properties

        /// <summary>
        /// Resulting DSN
        /// </summary>
        public string ReturnOntologyPath
        {
            get
            {
                return returnOntologyPath;
            }
        }

        #endregion

        public SetOntologyPathControl(string[] localePrefs, string currentOntologyPath, IOwnerOfAddIn ownerOfAddIn)
        {
            InitializeComponent();
            if (currentOntologyPath.StartsWith("file:/"))
            {
                radioButton1.Checked = true;
                string tmpstr = "file:/";
                diskPathTextBox.Text = currentOntologyPath.Remove(0,tmpstr.Length).ToString();
            }
            else if (currentOntologyPath.ToString() != "") {
                radioButton1.Checked = true;
                URLPathTextBox.Text = currentOntologyPath.ToString();
            }
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = diskPathTextBox.Text;
            
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                diskPathTextBox.Text = openFileDialog1.FileName;
                URLPathTextBox.Text = "";
                radioButton1.Checked = true;
                radioButton2.Checked = false;
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                this.returnOntologyPath = "file:/" + diskPathTextBox.Text;
                this.DialogResult = DialogResult.OK;
                this.Dispose();
                return;
            }
            else if (radioButton2.Checked)
            {
                this.returnOntologyPath = URLPathTextBox.Text;
                this.DialogResult = DialogResult.OK;
                this.Dispose();
                return;
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
                this.Dispose();
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Dispose();
        }

        private void URLPathTextBox_TextChanged(object sender, EventArgs e)
        {
            radioButton1.Checked = false;
            radioButton2.Checked = true;
        }

        private void URLPathTextBox_Click(object sender, EventArgs e)
        {
            radioButton1.Checked = false;
            radioButton2.Checked = true;
        }
    }
}