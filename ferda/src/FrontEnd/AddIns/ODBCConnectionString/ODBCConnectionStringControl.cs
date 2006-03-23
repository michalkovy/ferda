// ODBCConnectionStringControl.cs - UserControl for composing the connection string
//
// Author: Alexander Kuzmin <alexander.kuzmin@gmail.com>
//
// Copyright (c) 2005 Alexander Kuzmin
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
using Guy.Utilities.Reg;
using System.Resources;
using System.Reflection;


namespace Ferda.FrontEnd.AddIns.ODBCConnectionString
{
    /// <summary>
    /// User conttrol for composing the connection string
    /// </summary>
    public partial class ODBCConnectionStringControl : Form, Ferda.FrontEnd.IIconProvider
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
        /// Resulting DSN
        /// </summary>
        private string returnString;

        /// <summary>
        /// Current path (for icon loading)
        /// </summary>
        private string path;

        /// <summary>
        /// Dictionary that contains all the icons for the application, ]
        /// that are keyed by string values. See 
        /// <see cref="F:Ferda.FrontEnd.FerdaForm.LoadIcons"/> for their names
        /// </summary>
        private Dictionary<string, Icon> iconProvider;

        /// <summary>
        /// Owner of addin
        /// </summary>
        private IOwnerOfAddIn ownerOfAddIn;


        #endregion


        #region Public properties

        /// <summary>
        /// Resulting DSN
        /// </summary>
        public string ReturnString
        {
            get
            {
                return returnString;
            }
        }

        #endregion


        #region Contructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="localePrefs">localeprefs</param>
        /// <param name="currentDSN">Current DSN</param>
        public ODBCConnectionStringControl(string[] localePrefs, string currentDSN, IOwnerOfAddIn ownerOfAddIn)
        {
            //setting the ResManager resource manager and localization string
            string locale;
            try
            {
                locale = localePrefs[0];
                localizationString = locale;
                locale = "Ferda.FrontEnd.AddIns.ODBCConnectionString.Localization_" + locale;
                resManager = new ResourceManager(locale, Assembly.GetExecutingAssembly());
            }
            catch
            {
                resManager = new ResourceManager("Ferda.FrontEnd.AddIns.ODBCConnectionString.Localization_en-US",
            Assembly.GetExecutingAssembly());
                localizationString = "en-US";
            }
            this.ownerOfAddIn = ownerOfAddIn;
            this.path = Assembly.GetExecutingAssembly().Location;
            InitializeComponent();
            this.ChangeLocale(resManager);
            this.returnString = currentDSN;
            this.FillList();
            this.MyInit();
        }

        #endregion


        #region Initialization

        /// <summary>
        /// Initializing the GUI elements
        /// </summary>
        private void MyInit()
        {
            FillStatusBar();
            this.TextBoxCustomConnectionString.LostFocus += new EventHandler(TextBoxCustomConnectionString_LostFocus);
            this.TextBoxCustomConnectionString.KeyPress += new KeyPressEventHandler(TextBoxCustomConnectionString_KeyPress);
            this.ComboboxExistingSources.SelectionChangeCommitted += new EventHandler(ComboboxExistingSources_SelectionChangeCommitted);
            this.TextBoxUsername.LostFocus += new EventHandler(TextBoxUsername_LostFocus);
            this.TextBoxPassword.LostFocus += new EventHandler(TextBoxPassword_LostFocus);
            this.LoadIcons();
            this.InitIcons();
        }

        /// <summary>
        /// Method to set the status bar
        /// </summary>
        private void FillStatusBar()
        {
            if (this.returnString != String.Empty)
                this.StatusBarCurrent.Text = this.resManager.GetString("CurrentDSN") + ": "
            + this.returnString;
            else
                this.StatusBarCurrent.Text = this.resManager.GetString("CurrentDSN")
                + ": "
                + this.resManager.GetString("None");
        }

        /// <summary>
        /// Method to load all icons
        /// </summary>
        private void InitIcons()
        {
            this.Icon = iconProvider["FerdaIcon"];
            this.ToolStripButtonCancel.Image = iconProvider["CancelIcon"].ToBitmap();
            this.ToolStripButtonOk.Image = iconProvider["OkIcon"].ToBitmap();
            this.ToolStripButtonTest.Image = iconProvider["TestIcon"].ToBitmap();
            this.ToolStripButtonOdbc.Image = iconProvider["NewIcon"].ToBitmap();
        }

        #endregion


        #region Private methods

        /// <summary>
        /// Method for testing the connection string
        /// </summary>
        /// <param name="connectionString">Connection string to test</param>
        /// <returns></returns>
        public bool TestConnectionStringAction(string connectionString)
        {
            try
            {
                Ferda.Modules.Helpers.Data.Database.TestConnectionString(connectionString, null);
                return true;
            }
            catch { }
            return false;
        }

        private void FillList()
        {
            string[] dsns = { "" };
            string[] dsns_user = { "" };
            try
            {
                dsns = ODBCDSN.DsnList(HKEY.LocalMachine);
            }
            catch
            {
            }

            try
            {
                dsns_user = ODBCDSN.DsnList(HKEY.CurrentUser);
            }
            catch
            {
            }
            string currentDsn = SA.Reg.ReadDsn();
            foreach (string str in dsns)
            {
                ComboboxExistingSources.Items.Add(str);
            }

            foreach (string str in dsns_user)
            {
                ComboboxExistingSources.Items.Add(str);
            }

            if (ComboboxExistingSources.Items.Count > 0)
            {
                ComboboxExistingSources.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Loads the icons for the application
        /// </summary>
        /// <remarks>
        /// Sometimes, the program path can change and at this time, no icons
        /// are present
        /// </remarks>
        private void LoadIcons()
        {
            System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex("(\\\\)");
            string[] s = r.Split(this.path);
            string newPath = "";
            for (int j = 0; j < s.GetLength(0) - 3; j++)
            {
                newPath = newPath + s[j];
            }

            Icon i;
            iconProvider = new Dictionary<string, Icon>();

            //loading the program icon
            i = new Icon(newPath + "FerdaFrontEnd.ico");
            iconProvider.Add("FerdaIcon", i);

            i = new Icon(newPath + "\\Icons\\Save project.ico");
            iconProvider.Add("OkIcon", i);

            i = new Icon(newPath + "\\Icons\\Exit.ico");
            iconProvider.Add("CancelIcon", i);

            i = new Icon(newPath + "\\Icons\\New project.ico");
            iconProvider.Add("NewIcon", i);

            i = new Icon(newPath + "\\Icons\\Pack sockets.ico");
            iconProvider.Add("TestIcon", i);
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
        public void ChangeLocale(ResourceManager rm)
        {
            this.Text = rm.GetString("ConnectionString");
            this.ToolStripButtonCancel.Text = rm.GetString("ButtonCancel");
            this.ToolStripButtonOk.Text = rm.GetString("ButtonOk");
           // this.ToolStripButtonRefresh.Text = rm.GetString("ButtonRefresh");
            this.ButtonRefresh.Text = rm.GetString("ButtonRefresh");
            this.ToolStripButtonTest.Text = rm.GetString("ButtonTest");
            this.ToolStripButtonOdbc.Text = rm.GetString("ButtonOdbcConnection");
            this.LabelInputDataSource.Text = rm.GetString("InputDataSource");
            this.LabelInputServerAuth.Text = rm.GetString("InputAuth");
            this.LabelInputUsername.Text = rm.GetString("Username");
            this.LabelPassword.Text = rm.GetString("Password");
            this.LabelResultingString.Text = rm.GetString("ResultingString");
            this.LabelWhatToInput.Text = rm.GetString("InputFollowing");
            this.RadioUseCustomString.Text = rm.GetString("InputCustomConnectionString");
            this.RadioUseDSN.Text = rm.GetString("InputDataSource");
            this.CheckBoxAllowPasswordSave.Text = rm.GetString("SavePassword");
            this.CheckBoxEmptyPassword.Text = rm.GetString("EmptyPassword");
            this.ToolStripHelp.Text = rm.GetString("Help");
        }

        #endregion


        #region Toolbuttons and statusbars handlers

        /// <summary>
        /// Handler which calls windows dialog for ODBC datasource
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripButtonOdbc_Click(object sender, EventArgs e)
        {

            SA.ODBCCP32 obj = new SA.ODBCCP32();
          if (obj.CreateDatasource(this.Handle,""))
          {
              this.ComboboxExistingSources.Items.Clear();
              this.FillList();
          }
          else
          {
                //MessageBox.Show(this.resManager.GetString("DllFail"), this.resManager.GetString("MessageTitleFail"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          //  obj.CreateDatasource(this.Handle,"");
            

        }

        /// <summary>
        /// Method which closes the window without connection string modification.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripButtonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Dispose();
        }

        /// <summary>
        /// Method which closes the window and returns the modified connection string.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripButtonOk_Click(object sender, EventArgs e)
        {
            this.returnString =
                this.ComposeString();
            this.DialogResult = DialogResult.OK;
            this.Dispose();
            return;
        }

        /// <summary>
        /// Method which tests the conneciton string based on current data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripButtonTest_Click(object sender, EventArgs e)
        {

            if (this.TestConnectionStringAction(this.ComposeString()))
            {
                MessageBox.Show(this.resManager.GetString("ConnectionStringOk"), this.resManager.GetString("MessageTitleSuccess"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                this.TextBoxNewConnectionString.Text = this.ComposeString();
            }

            else
            {

                MessageBox.Show(this.resManager.GetString("ConnectionStringFail"), this.resManager.GetString("MessageTitleFail"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// Method which composes the connection string using the input from user.
        /// </summary>
        /// <returns></returns>
        private string ComposeString()
        {
            string connectionString;

            if (RadioUseDSN.Checked == true)
            {
                try
                {
                    connectionString = "DSN=" + ComboboxExistingSources.SelectedItem.ToString();
                    //connectionString = "\"DSN=" + ComboboxExistingSources.SelectedItem.ToString();
                }
                catch
                {
                    return String.Empty;
                }
            }
            else
            {
                if (this.TextBoxCustomConnectionString.Text != String.Empty)
                {
                    connectionString = this.TextBoxCustomConnectionString.Text;
                    return connectionString;
                }
                else
                {
                    return String.Empty;
                }
            }

            if (this.TextBoxUsername.Text.Length > 0)
            {
                connectionString = connectionString +
                    ";UID=" + this.TextBoxUsername.Text;

                if ((!this.CheckBoxEmptyPassword.Checked) && (this.TextBoxPassword.Text.Length > 0))
                {
                    connectionString = connectionString +
                        ";PWD=" + this.TextBoxPassword.Text;
                }


            }

            
            if (this.CheckBoxAllowPasswordSave.Checked == true)
            {
                connectionString = connectionString +
                    ";Persist security info=true";
            }
            else
            {
                connectionString = connectionString +
                        ";Persist security info=false";
            }

            

           // connectionString = connectionString + "\"";

            return connectionString;
        }

        /// <summary>
        /// Handler which enables or disables Test button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxCustomConnectionString_LostFocus(object sender, EventArgs e)
        {
            if (this.TextBoxCustomConnectionString.Text.Length > 0)
            {
                this.TextBoxNewConnectionString.Text = this.TextBoxCustomConnectionString.Text;

                this.ToolStripButtonTest.Enabled = true;
            }
            else
            {
                this.ToolStripButtonTest.Enabled = false;
            }
        }

        /// <summary>
        /// Handler which enables or disables Test button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxCustomConnectionString_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((this.TextBoxCustomConnectionString.Text.Length > 0) || (this.ComboboxExistingSources.SelectedIndex > 0))
            {
                this.ToolStripButtonTest.Enabled = true;
            }
            else
            {
                this.ToolStripButtonTest.Enabled = false;
            }
        }

        /// <summary>
        /// Handler which enables or disables password textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBoxEmptyPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (this.CheckBoxEmptyPassword.Checked)
            {
                this.TextBoxPassword.Enabled = false;
            }
            else
            {
                this.TextBoxPassword.Enabled = true;
            }

            this.TextBoxNewConnectionString.Text = this.ComposeString();
        }


        /// <summary>
        /// Handler which enables or disables custom string textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioUseCustomString_CheckedChanged(object sender, EventArgs e)
        {
            if (this.RadioUseCustomString.Enabled)
            {
                this.ComboboxExistingSources.Enabled = false;
                this.TextBoxCustomConnectionString.Enabled = true;
            }
            else
            {
                this.ComboboxExistingSources.Enabled = true;
                this.TextBoxCustomConnectionString.Enabled = false;
            }
            this.TextBoxNewConnectionString.Text = this.ComposeString();
        }

        private void RadioUseDSN_CheckedChanged(object sender, EventArgs e)
        {
            if (this.RadioUseDSN.Checked == true)
            {
                this.ComboboxExistingSources.Enabled = true;
                this.TextBoxCustomConnectionString.Enabled = false;
            }
            else
            {
                this.ComboboxExistingSources.Enabled = false;
                this.TextBoxCustomConnectionString.Enabled = true;
            }
            this.TextBoxNewConnectionString.Text = this.ComposeString();
        }


        /// <summary>
        /// Handler which changes the value of the actual connection string displayed in the NewConnectionString textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboboxExistingSources_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (this.ComboboxExistingSources.SelectedItem.ToString() != String.Empty)
            {
                this.TextBoxNewConnectionString.Text = this.ComposeString();
                this.ToolStripButtonTest.Enabled = true;
            }
            else
            {
                this.TextBoxNewConnectionString.Text = String.Empty;
                this.ToolStripButtonTest.Enabled = false;
            }
        }     

        /// <summary>
        /// Handler for save password checkpoint
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBoxAllowPasswordSave_CheckedChanged(object sender, EventArgs e)
        {
            this.TextBoxNewConnectionString.Text = this.ComposeString();
        }

        /// <summary>
        /// Handler which composes connection string based on already inputted data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxPassword_LostFocus(object sender, EventArgs e)
        {
            this.TextBoxNewConnectionString.Text = this.ComposeString();
        }

        /// <summary>
        /// Handler which composes connection string based on already inputted data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxUsername_LostFocus(object sender, EventArgs e)
        {
            this.TextBoxNewConnectionString.Text = this.ComposeString();
        }

        /// <summary>
        /// Handler which re-reads datasources to the combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonRefresh_Click(object sender, EventArgs e)
        {
            this.ComboboxExistingSources.Items.Clear();
            this.FillList();
        }

        private void ToolStripHelp_Click(object sender, EventArgs e)
        {
            ownerOfAddIn.OpenPdf(ownerOfAddIn.GetBinPath() + "\\AddIns\\Help\\ODBCConnectionString.pdf");
        }

        #endregion


        #region IIconProvider Members

        /// <summary>
        /// Gets the icon specified by icons string identifier
        /// </summary>
        /// <param name="IconName">Name of the icon</param>
        /// <returns>Icon that is connected to this name</returns>
        public Icon GetIcon(string IconName)
        {
            return iconProvider[IconName];
        }

        #endregion
    }
}
