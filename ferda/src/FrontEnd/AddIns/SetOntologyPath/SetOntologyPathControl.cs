using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Resources;

namespace Ferda.FrontEnd.AddIns.SetOntologyPath
{
    public partial class SetOntologyPathControl : Form
    {

        #region Private variables

        /// <summary>
        /// Localization resource manager
        /// </summary>
        private ResourceManager resManager;

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
        
        /// <summary>
        /// Owner of addin
        /// </summary>
        private IOwnerOfAddIn ownerOfAddIn;
        
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
            //setting the ResManager resource manager and localization string
            string locale;
            try
            {
                locale = localePrefs[0];
                localizationString = locale;
                locale = "Ferda.FrontEnd.AddIns.SetOntologyPath.Localization_" + locale;
                resManager = new ResourceManager(locale, Assembly.GetExecutingAssembly());
            }
            catch
            {
                resManager = new ResourceManager("Ferda.FrontEnd.AddIns.SetOntologyPath.Localization_en-US",
            Assembly.GetExecutingAssembly());
                localizationString = "en-US";
            }
            this.ownerOfAddIn = ownerOfAddIn;
            this.path = Assembly.GetExecutingAssembly().Location;
            
            InitializeComponent();
            if (currentOntologyPath.StartsWith("file:/"))
            {
                radioButton1.Checked = true;
                browseButton.Enabled = true;
                string tmpstr = "file:/";
                diskPathTextBox.Text = currentOntologyPath.Remove(0,tmpstr.Length).ToString();
            }
            else if (currentOntologyPath.ToString() != "") {
                radioButton2.Checked = true;
                URLPathTextBox.Text = currentOntologyPath.ToString();
            }
        }
        
        #region Localization
        //// <summary>
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
        public void ChangeLocale(ResourceManager rm)
        {
            this.Text = rm.GetString("SetOntologyPath");
            this.browseButton.Text = rm.GetString("ButtonBrowse");
            this.cancelButton.Text = rm.GetString("ButtonCancel");
            this.okButton.Text = rm.GetString("ButtonOk");
            this.radioButton1.Text = rm.GetString("SelectOntologyNetwork");
            this.radioButton2.Text = rm.GetString("SelectOntologyInternet");
            this.helpButton.Text = rm.GetString("Help");
        }

        #endregion

        private void browseButton_Click(object sender, EventArgs e)
        {
            URLPathTextBox.Text = "";
            radioButton1.Checked = true;

            openFileDialog1.FileName = diskPathTextBox.Text;
                        
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                diskPathTextBox.Text = openFileDialog1.FileName;
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                if (diskPathTextBox.Text != "")
                    this.returnOntologyPath = "file:/" + diskPathTextBox.Text;
                else
                    this.returnOntologyPath = "";
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
            browseButton.Enabled = false;
            radioButton2.Checked = true;
        }

        private void radioButton1_Click(object sender, EventArgs e)
        {
            URLPathTextBox.Text = "";
            radioButton1.Checked = true;
            browseButton.Enabled = true;
            radioButton2.Checked = false;
        }

        private void radioButton2_Click(object sender, EventArgs e)
        {
            radioButton1.Checked = false;
            browseButton.Enabled = false;
            radioButton2.Checked = true;
        }

        private void helpButton_Click(object sender, EventArgs e)
        {
            ownerOfAddIn.OpenPdf(ownerOfAddIn.GetBinPath() + "\\AddIns\\Help\\SetOntologyPath.pdf");
        }
    }
}