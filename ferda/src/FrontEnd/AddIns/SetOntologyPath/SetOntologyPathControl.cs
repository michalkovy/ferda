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
        /// Resulting Ontology Path
        /// </summary>
        public string ReturnOntologyPath
        {
            get
            {
                return returnOntologyPath;
            }
        }

        #endregion

        /// <summary>
        /// Initializing SetOntologyPathControl
        /// </summary>
        /// <param name="localePrefs">Locale prefs</param>
        /// <param name="currentOntologyPath">Current Ontology Path</param>
        /// <param name="ownerOfAddIn">Owner of AddIn</param>
        public SetOntologyPathControl(string[] localePrefs, string currentOntologyPath, IOwnerOfAddIn ownerOfAddIn)
        {
            /// setting the ResManager resource manager and localization string
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

            /// initializing private variables
            this.ownerOfAddIn = ownerOfAddIn;
            this.path = Assembly.GetExecutingAssembly().Location;
            
            /// initializing the form and localization
            InitializeComponent();
            ChangeLocale(resManager);
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

        /// <summary>
        /// Button to select the file with the ontology
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
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

        /// <summary>
        /// Button to set new path to ontology. Sets the chosen path to ontology for retrieval.
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
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

        /// <summary>
        /// Canceling the setting of the new path to the ontology
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Dispose();
        }

        /// <summary>
        /// User types the path to ontology, it seems that the url path will be chosen
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        private void URLPathTextBox_TextChanged(object sender, EventArgs e)
        {
            radioButton1.Checked = false;
            radioButton2.Checked = true;
        }

        /// <summary>
        /// User sets the curson into URLPathTextBox, it seems that the url path will be chosen
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        private void URLPathTextBox_Click(object sender, EventArgs e)
        {
            radioButton1.Checked = false;
            browseButton.Enabled = false;
            radioButton2.Checked = true;
        }

        /// <summary>
        /// User selected the first button, he wants to set the path to ontology from the network
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        private void radioButton1_Click(object sender, EventArgs e)
        {
            URLPathTextBox.Text = "";
            radioButton1.Checked = true;
            browseButton.Enabled = true;
            radioButton2.Checked = false;
        }

        /// <summary>
        /// User selected the second button, he wants to set the path to internet based ontology
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        private void radioButton2_Click(object sender, EventArgs e)
        {
            radioButton1.Checked = false;
            browseButton.Enabled = false;
            radioButton2.Checked = true;
        }

        /// <summary>
        /// Opens the help document
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        private void helpButton_Click(object sender, EventArgs e)
        {
            ownerOfAddIn.OpenPdf(ownerOfAddIn.GetBinPath() + "\\AddIns\\Help\\SetOntologyPath.pdf");
        }
    }
}