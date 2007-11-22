// SelectTablesControl.cs - UserControl class for selecting tables to display
//
// Author: Alexander Kuzmin <alexander.kuzmin@gmail.com>
//
// Copyright (c) 2006 Alexander Kuzmin
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
using Ferda.Modules;
using Ferda.Modules.Boxes;
using Ferda.ModulesManager;
using Ferda.FrontEnd.AddIns;
using System.Resources;
using System.Reflection;

namespace Ferda.FrontEnd.AddIns.MultiSelectStrings
{
    public partial class MultiSelectStringsControl : System.Windows.Forms.Form, Ferda.FrontEnd.IIconProvider
    {
        #region Private variables

        /// <summary>
        /// All available strings
        /// </summary>
        SelectString[] allStrings;

        /// <summary>
        /// Strings already selected
        /// </summary>
        string[] selectedStrings;

        /// <summary>
        /// Resourcemanager
        /// </summary>
        ResourceManager resManager;

        /// <summary>
        /// Localization string - for now, en-US or cs-CZ
        /// </summary>
        private string localizationString;

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

        #endregion


        #region Public variables

        public string[] ReturnStrings;

        #endregion


        #region Constructor

        public MultiSelectStringsControl(string [] localePrefs, IOwnerOfAddIn ownerOfAddIn, SelectString [] allStrings, string [] selectedStrings)
        {
            //setting the ResManager resource manager and localization string
            string locale;
            try
            {
                locale = localePrefs[0];
                localizationString = locale;
                locale = "Ferda.FrontEnd.AddIns.MultiSelectStrings.Localization_" + locale;
                resManager = new ResourceManager(locale, Assembly.GetExecutingAssembly());
            }
            catch
            {
                resManager = new ResourceManager("Ferda.FrontEnd.AddIns.MultiSelectStrings.Localization_en-US",
            Assembly.GetExecutingAssembly());
                localizationString = "en-US";
            }
            this.selectedStrings = selectedStrings;
            this.path = Assembly.GetExecutingAssembly().Location;
            this.allStrings = allStrings;
            InitializeComponent();
            this.InitList();
            this.LoadIcons();
            this.InitIcons();
            this.ChangeLocale(this.resManager);
        }

        #endregion


        #region Initialization

        private void InitList()
        {
            List<string> tempList = new List<string>();
            tempList.AddRange(this.selectedStrings);
            foreach (Ferda.Modules.SelectString var in this.allStrings)
            {
                if (tempList.IndexOf(var.name) != -1)
                {
                    this.CheckedListBox.Items.Add(var.label, true);
                }
                else
                {
                    this.CheckedListBox.Items.Add(var.label, false);
                }
            }
        }

        /// <summary>
        /// Method to load all icons
        /// </summary>
        private void InitIcons()
        {
            this.Icon = iconProvider["FerdaIcon"];
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
//            System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex("(\\\\)");
//            string[] s = r.Split(this.path);
//            string newPath = "";
//            for (int j = 0; j < s.GetLength(0) - 3; j++)
//            {
//                newPath = newPath + s[j];
//            }
            
            // get the directory where FerdaFrontEnd.exe resides
            string assemblyDir = Assembly.GetExecutingAssembly().Location;
	    	System.IO.FileInfo fileInfo = new System.IO.FileInfo(assemblyDir);
	    	assemblyDir = fileInfo.Directory.Parent.ToString();
	    	
	    	string iconPath = System.IO.Path.Combine(assemblyDir, "FerdaFrontEnd.ico");

            Icon i;
            iconProvider = new Dictionary<string, Icon>();

            //loading the program icon
            i = new Icon(iconPath);
            iconProvider.Add("FerdaIcon", i);
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
            this.Text = rm.GetString("MultiSelectStrings");
            this.Button2Submit.Text = rm.GetString("Submit");
            this.Button1Cancel.Text = rm.GetString("Cancel");
        }

        #endregion


        #region Button handlers

        /// <summary>
        /// Returns selected items as a string array
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button2Submit_Click(object sender, EventArgs e)
        {
            this.ReturnStrings = new string[this.CheckedListBox.CheckedIndices.Count];

            for(int i = 0; i < this.CheckedListBox.CheckedItems.Count;i++ )
            {
                this.ReturnStrings[i] = this.allStrings[this.CheckedListBox.CheckedIndices[i]].name;
            }
            this.DialogResult = DialogResult.OK;
            this.Dispose();
            return;
        }

        #endregion

        #region IIconProvider Members

        public Icon GetIcon(string IconName)
        {
            return iconProvider[IconName];
        }

        #endregion
    }
}