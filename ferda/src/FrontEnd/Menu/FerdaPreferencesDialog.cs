// FerdaPreferencesDialog.cs - dialog where user can edit the application settings
// and preferences
//
// Author: Martin Ralbovský <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2005 Martin Ralbovský
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
using System.Windows.Forms;
using System.Resources;

namespace Ferda.FrontEnd.Menu
{
    /// <summary>
	/// Class represents all the settings available in the project.
	/// </summary>
    /// <remarks>
    /// There are plans to add functionality for this tab. Right now it contains
    /// only the localization and little other options,
    /// </remarks>
	/// <stereotype>control</stereotype>
    internal class FerdaPreferencesDialog : Form
    {
        #region Internal class fields

        /// <summary>
        /// A tab control
        /// </summary>
        private TabControl tabControl1;
        /// <summary>
        /// Page that concerns localization
        /// </summary>
        private TabPage TPLocalization;
        /// <summary>
        /// Tab that contains property grid settings
        /// </summary>
        private TabPage TPPropertyGrid;
        /// <summary>
        /// The OK button
        /// </summary>
        private Button BOK;
        /// <summary>
        /// The Cancel button
        /// </summary>
        private Button BCancel;
        /// <summary>
        /// Label informing about localization
        /// </summary>
        private Label LLocalization;
        /// <summary>
        /// List box where user can select his localization
        /// </summary>
        private ListBox LBLocalization;
        /// <summary>
        /// With this checkbox user determines whether to show the
        /// <code>Visible sockets</code> group in the property grid.
        /// </summary>
        private CheckBox CHBShowVisibleSockets;
        /// <summary>
        /// With this checkbox user determines whether to use display
        /// the timing of indivdual progress bars' timing.
        /// </summary>
        private CheckBox CHBDisplayTiming;

        //Resource manager from the FerdaForm
        private ResourceManager resManager;

        #endregion

        #region Properties

        /// <summary>
        /// Retrieves an array of localization strings,
        /// the first one being the new string for which
        /// the localization should be done.
        /// </summary>
        public string [] LocalePrefs
        {
            get
            {
                if (LBLocalization.SelectedIndex != 0)
                {
                    int j = 0;
                    //rearanging the localePrefs - the new localization is the 0th categoriesIndex,
                    //others stay the same
                    string[] result = new string[LBLocalization.Items.Count];
                    result[j] = LBLocalization.Items[LBLocalization.SelectedIndex].ToString();
                    j++;
                    for (int i = 0; i < result.Length; i++)
                    {
                        if (i == LBLocalization.SelectedIndex)
                        {
                            continue;
                        }
                        else
                        {
                            result[j] = LBLocalization.Items[i].ToString();
                            j++;
                        }
                    }

                    return result;
                }
                else
                {
                    string[] result = new string[LBLocalization.Items.Count];
                    for (int i = 0; i < result.Length; i++)
                    {
                        result[i] = LBLocalization.Items[i].ToString();
                    }
                    return result;
                }
            }
        }

        /// <summary>
        /// Resource manager of the application, it is filled according to the
        /// current localization
        /// </summary>
        public ResourceManager ResManager
        {
            set
            {
                resManager = value;
            }
            get
            {
                if (resManager == null)
                {
                    throw new ApplicationException(
                        "Menu.ResManager cannot be null");
                }
                return resManager;
            }
        }

        /// <summary>
        /// User determines whether to show the
        /// <code>Visible sockets</code> group in the property grid.
        /// </summary>
        public bool ShowVisibleSockets
        {
            get
            {
                return CHBShowVisibleSockets.Checked;
            }
        }

        /// <summary>
        /// User determines if the progress bars should display a 
        /// dialog showing exact time elapsed for each progress bar
        /// running. This can be useful i.e. when timing hypotheses 
        /// generation.
        /// </summary>
        public bool DisplayTiming
        {
            get
            {
                return CHBDisplayTiming.Checked;
            }
        }

        #endregion

        #region Constructor

        ///<summary>
        /// Default constructor for FerdaView class.
        ///</summary>
        public FerdaPreferencesDialog(ResourceManager resources, IPreferencesManager manager)
            : base()
        {
            resManager = resources;
            InitializeComponent();
            FillPreferencesValues(manager);

            //localizing the application
            Text = ResManager.GetString("PreferencesDialogCaption");
            BOK.Text = ResManager.GetString("OKButton");
            BCancel.Text = ResManager.GetString("CancelButton");
            TPLocalization.Text = ResManager.GetString("LocalizationTab");
            LLocalization.Text = ResManager.GetString("LocalizationLabel");
            TPPropertyGrid.Text = ResManager.GetString("OtherTab");
            CHBShowVisibleSockets.Text = ResManager.GetString("ShowVisibleSockets");
            CHBDisplayTiming.Text = ResManager.GetString("ShowDisplayTiming");
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fills the control with the preferences values
        /// </summary>
        /// <param name="manager">Preferences manager that contains
        /// the preferences details</param>
        private void FillPreferencesValues(IPreferencesManager manager)
        {
            foreach (string s in manager.LocalePrefs)
            {
                LBLocalization.Items.Add(s);
            }

            //selects the first value on the list
            LBLocalization.SelectedIndex = 0;
            CHBShowVisibleSockets.Checked = manager.ShowVisibleSockets;
            CHBDisplayTiming.Checked = manager.DisplayTiming;
        }

        /// <summary>
        /// Initializes the component
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.TPLocalization = new System.Windows.Forms.TabPage();
            this.LBLocalization = new System.Windows.Forms.ListBox();
            this.LLocalization = new System.Windows.Forms.Label();
            this.TPPropertyGrid = new System.Windows.Forms.TabPage();
            this.CHBShowVisibleSockets = new System.Windows.Forms.CheckBox();
            this.BOK = new System.Windows.Forms.Button();
            this.BCancel = new System.Windows.Forms.Button();
            this.CHBDisplayTiming = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.TPLocalization.SuspendLayout();
            this.TPPropertyGrid.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.TPLocalization);
            this.tabControl1.Controls.Add(this.TPPropertyGrid);
            this.tabControl1.Location = new System.Drawing.Point(4, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(371, 224);
            this.tabControl1.TabIndex = 0;
            // 
            // TPLocalization
            // 
            this.TPLocalization.Controls.Add(this.LBLocalization);
            this.TPLocalization.Controls.Add(this.LLocalization);
            this.TPLocalization.Location = new System.Drawing.Point(4, 22);
            this.TPLocalization.Name = "TPLocalization";
            this.TPLocalization.Padding = new System.Windows.Forms.Padding(3);
            this.TPLocalization.Size = new System.Drawing.Size(363, 198);
            this.TPLocalization.TabIndex = 0;
            this.TPLocalization.Text = "tabPage1";
            this.TPLocalization.UseVisualStyleBackColor = true;
            // 
            // LBLocalization
            // 
            this.LBLocalization.FormattingEnabled = true;
            this.LBLocalization.Location = new System.Drawing.Point(7, 48);
            this.LBLocalization.Name = "LBLocalization";
            this.LBLocalization.Size = new System.Drawing.Size(350, 134);
            this.LBLocalization.TabIndex = 1;
            // 
            // LLocalization
            // 
            this.LLocalization.AutoSize = true;
            this.LLocalization.Location = new System.Drawing.Point(9, 3);
            this.LLocalization.Name = "LLocalization";
            this.LLocalization.Size = new System.Drawing.Size(35, 13);
            this.LLocalization.TabIndex = 0;
            this.LLocalization.Text = "label1";
            // 
            // TPPropertyGrid
            // 
            this.TPPropertyGrid.Controls.Add(this.CHBDisplayTiming);
            this.TPPropertyGrid.Controls.Add(this.CHBShowVisibleSockets);
            this.TPPropertyGrid.Location = new System.Drawing.Point(4, 22);
            this.TPPropertyGrid.Name = "TPPropertyGrid";
            this.TPPropertyGrid.Size = new System.Drawing.Size(363, 198);
            this.TPPropertyGrid.TabIndex = 1;
            this.TPPropertyGrid.Text = "TPPropertyGrid";
            this.TPPropertyGrid.UseVisualStyleBackColor = true;
            // 
            // CHBShowVisibleSockets
            // 
            this.CHBShowVisibleSockets.AutoSize = true;
            this.CHBShowVisibleSockets.Location = new System.Drawing.Point(5, 13);
            this.CHBShowVisibleSockets.Name = "CHBShowVisibleSockets";
            this.CHBShowVisibleSockets.Size = new System.Drawing.Size(80, 17);
            this.CHBShowVisibleSockets.TabIndex = 0;
            this.CHBShowVisibleSockets.Text = "checkBox1";
            this.CHBShowVisibleSockets.UseVisualStyleBackColor = true;
            // 
            // BOK
            // 
            this.BOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.BOK.Location = new System.Drawing.Point(215, 238);
            this.BOK.Name = "BOK";
            this.BOK.Size = new System.Drawing.Size(75, 23);
            this.BOK.TabIndex = 1;
            this.BOK.Text = "OK";
            // 
            // BCancel
            // 
            this.BCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BCancel.Location = new System.Drawing.Point(296, 238);
            this.BCancel.Name = "BCancel";
            this.BCancel.Size = new System.Drawing.Size(75, 23);
            this.BCancel.TabIndex = 2;
            this.BCancel.Text = "Storno";
            // 
            // CHBDisplayTiming
            // 
            this.CHBDisplayTiming.AutoSize = true;
            this.CHBDisplayTiming.Location = new System.Drawing.Point(5, 37);
            this.CHBDisplayTiming.Name = "CHBDisplayTiming";
            this.CHBDisplayTiming.Size = new System.Drawing.Size(80, 17);
            this.CHBDisplayTiming.TabIndex = 1;
            this.CHBDisplayTiming.Text = "checkBox1";
            this.CHBDisplayTiming.UseVisualStyleBackColor = true;
            // 
            // FerdaPreferencesDialog
            // 
            this.AcceptButton = this.BOK;
            this.CancelButton = this.BCancel;
            this.ClientSize = new System.Drawing.Size(387, 273);
            this.Controls.Add(this.BCancel);
            this.Controls.Add(this.BOK);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FerdaPreferencesDialog";
            this.ShowInTaskbar = false;
            this.Text = "Zatim prefDialog";
            this.tabControl1.ResumeLayout(false);
            this.TPLocalization.ResumeLayout(false);
            this.TPLocalization.PerformLayout();
            this.TPPropertyGrid.ResumeLayout(false);
            this.TPPropertyGrid.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
    }
}
