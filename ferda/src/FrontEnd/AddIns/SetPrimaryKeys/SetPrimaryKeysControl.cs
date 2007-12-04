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

namespace Ferda.FrontEnd.AddIns.SetPrimaryKeys
{
    public partial class SetPrimaryKeysControl : System.Windows.Forms.Form, Ferda.FrontEnd.IIconProvider
    {
        #region Private variables

        /// <summary>
        /// All available strings
        /// </summary>
        SelectString[] allStrings;

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
        /// String which separates particular string values
        /// </summary>
        private string separatorInner = "\t";

        /// <summary>
        /// String which separates the name of string and its values
        /// </summary>
        private string separatorOuter = "\n";

        /// <summary>
        /// Array of list boxes
        /// </summary>
        private CheckedListBox[] checkedListBoxArray;

        /// <summary>
        /// Dictionary that contains selected values (column names) for particular datatable name
        /// </summary>
        private Dictionary<string, string[]> selectedStrings = new Dictionary<string, string[]>();

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

        public SetPrimaryKeysControl(string[] localePrefs, IOwnerOfAddIn ownerOfAddIn, SelectString[] allStrings, string[] selectedStrings)
        {
            //setting the ResManager resource manager and localization string
            string locale;
            try
            {
                locale = localePrefs[0];
                localizationString = locale;
                locale = "Ferda.FrontEnd.AddIns.SetPrimaryKeys.Localization_" + locale;
                resManager = new ResourceManager(locale, Assembly.GetExecutingAssembly());
            }
            catch
            {
                resManager = new ResourceManager("Ferda.FrontEnd.AddIns.SetPrimaryKeys.Localization_en-US",
            Assembly.GetExecutingAssembly());
                localizationString = "en-US";
            }

            this.path = Assembly.GetExecutingAssembly().Location;
            this.allStrings = allStrings;

            foreach (string selString in selectedStrings)
            {
                string[] firstSplitString = selString.Split(new string[] { separatorOuter }, StringSplitOptions.RemoveEmptyEntries);
                
                string[] parsedStrings = firstSplitString[1].Split(new string[] { separatorInner }, StringSplitOptions.RemoveEmptyEntries);

                this.selectedStrings.Add(firstSplitString[0], parsedStrings);
            }

            InitializeComponent();

            this.checkedListBoxArray = new System.Windows.Forms.CheckedListBox[allStrings.Length];

            this.InitListView();
            this.LoadIcons();
            this.InitIcons();
            this.ChangeLocale(this.resManager);
        }

        #endregion


        #region Initialization

        /// <summary>
        /// Method which initializes the list view - it creates the checked list boxes and populate them with data
        /// </summary>
        private void InitListView()
        {
            int i = 0;
            int j = 0;
            int maxListsInRow = 5;  //maximal count of listviews in a row
            int labelWidth = 120;
            int labelHeight = 15;
            int labelMarginTop = 10;
            int listBoxWidth = 120;
            int listBoxHeight = 200;
            int listBoxMarginTop = 30;
            int rowHeight = 250;
            int bottomHeight = 50;
            int buttonWidth = 75;
            int buttonCount = 3;
            
            int listBoxesCount = this.allStrings.Length;
            int listViewWidth = new int();
            int listViewHeight = new int();
            
            //setting the width of listview dependant on count of displaying listboxes
            if (listBoxesCount > maxListsInRow)
            {
                listViewWidth = maxListsInRow * listBoxWidth;
            }
            else
            {
                listViewWidth = listBoxesCount * listBoxWidth;
                if (listViewWidth < buttonCount * buttonWidth)
                    listViewWidth = buttonCount * buttonWidth;
            }

            //setting height of listview dependant on count of displaying listboxes
            listViewHeight = ((listBoxesCount - 1) / maxListsInRow) + 1;  //getting integer from the division
            listViewHeight = (listViewHeight * rowHeight) + bottomHeight;

            this.ClientSize = new System.Drawing.Size(listViewWidth, listViewHeight);

            foreach (SelectString tmpString in allStrings)
            {
                string[] firstSplitString = tmpString.name.Split(new string[] { separatorOuter }, StringSplitOptions.RemoveEmptyEntries);
                
                Label newLabel = new Label();
                // 
                // newLabel
                // 
                newLabel.AutoSize = true;
                newLabel.Location = new System.Drawing.Point(i*listBoxWidth, j*rowHeight + labelMarginTop);
                newLabel.Name = "label" + i.ToString();
                newLabel.Size = new System.Drawing.Size(labelHeight, labelHeight);
                newLabel.Text = firstSplitString[0];
                this.Controls.Add(newLabel);

                CheckedListBox newCheckedListBox = new CheckedListBox();

                // 
                // newCheckedListBox
                // 
                newCheckedListBox.Name = firstSplitString[0];
                newCheckedListBox.CheckOnClick = true;
                newCheckedListBox.FormattingEnabled = true;
                newCheckedListBox.Location = new System.Drawing.Point(i*listBoxWidth, j*rowHeight+listBoxMarginTop);
                newCheckedListBox.Size = new System.Drawing.Size(listBoxWidth, listBoxHeight);

                //real index of the listbox
                int realIndex = j * maxListsInRow + i;

                checkedListBoxArray[realIndex] = newCheckedListBox;


                this.Controls.Add(checkedListBoxArray[realIndex]);

                string[] secondSplit = firstSplitString[1].Split(new string[] { separatorInner }, StringSplitOptions.RemoveEmptyEntries);

                List<string> tmpList = new List<string>();
                try
                {
                    tmpList.AddRange(this.selectedStrings[firstSplitString[0]]);
                }
                catch {}

                foreach (string strValue in secondSplit)
                {
                    if (tmpList.IndexOf(strValue) != -1)
                    {
                        checkedListBoxArray[realIndex].Items.Add(strValue, true);
                    }
                    else
                    {
                        checkedListBoxArray[realIndex].Items.Add(strValue, false);
                    }
                }
                i++;
                if (i == maxListsInRow)
                {
                    i = 0;
                    j++;
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
            // get the directory where FerdaFrontEnd.exe resides
            string assemblyDir = Assembly.GetExecutingAssembly().Location;
	    	System.IO.FileInfo fileInfo = new System.IO.FileInfo(assemblyDir);
	    	assemblyDir = fileInfo.Directory.Parent.FullName.ToString();
	    	
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
            this.Text = rm.GetString("SetPrimaryKeys");
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
            //counter for listboxes where at least one value is checked
            int checkedListBoxesCount = 0;
            foreach (CheckedListBox tmpCheckedListBox in this.checkedListBoxArray)
            {
                if (tmpCheckedListBox.CheckedIndices.Count > 0)
                {
                    checkedListBoxesCount++;
                }
            }
            this.ReturnStrings = new string[checkedListBoxesCount];
            int j = 0;
            foreach (CheckedListBox tmpCheckedListBox in this.checkedListBoxArray)
            {
                //only listboxes where at least one value is checked
                if (tmpCheckedListBox.CheckedIndices.Count > 0)
                {
                    string returnRow = tmpCheckedListBox.Name + this.separatorOuter;
                    for (int i = 0; i < tmpCheckedListBox.CheckedIndices.Count; i++)
                    {
                        returnRow += tmpCheckedListBox.Items[tmpCheckedListBox.CheckedIndices[i]].ToString() + this.separatorInner;
                    }
                    returnRow = returnRow.Remove(returnRow.Length - this.separatorInner.Length);
                    this.ReturnStrings[j] = returnRow;
                    j++;
                }
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