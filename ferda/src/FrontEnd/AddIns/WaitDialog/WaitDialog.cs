// WaitDialog.cs - Form for displaying task in progress message
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
using System.Resources;
using System.Reflection;
using Ferda;
using Ferda.Modules;
using Ferda.FrontEnd.AddIns;
using Ferda.FrontEnd.External;
using Ferda.FrontEnd.Properties;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractLMTask;

namespace Ferda.FrontEnd.AddIns.WaitDialog
{
    /// <summary>
    /// Form for displaying task in progress message
    /// </summary>
    public partial class WaitDialog : Form
    {
        #region Private variables

        /// <summary>
        /// Resource manager
        /// </summary>
        private ResourceManager resManager;


        /// <summary>
        /// Localization string, en-US or cs-CZ for now.
        /// </summary>
        private string localizationString;

        AbstractLMTaskFunctionsPrx tprx;

        Ferda.FrontEnd.AddIns.IOwnerOfAddIn ownerOfAddIn;

        delegate void CloseDelegate();

        #endregion


        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="localePrefs">localeprefs</param>
        /// <param name="tprx">Proxy</param>
        /// <param name="ownerOfAddIn">Owner of addin</param>
        public WaitDialog(string[] localePrefs, AbstractLMTaskFunctionsPrx tprx, Ferda.FrontEnd.AddIns.IOwnerOfAddIn ownerOfAddIn)
        {
            //setting the ResManager resource manager and localization string
            string locale;
            try
            {
                locale = localePrefs[0];
                localizationString = locale;
                locale = "Ferda.FrontEnd.AddIns.WaitDialog.Localization_" + locale;
                resManager = new ResourceManager(locale, Assembly.GetExecutingAssembly());
            }
            catch
            {
                resManager = new ResourceManager("Ferda.FrontEnd.AddIns.WaitDialog.Localization_en-US",
            Assembly.GetExecutingAssembly());
                localizationString = "en-US";
            }
            this.KeyDown += new KeyEventHandler(WaitDialog_KeyDown);
            this.tprx = tprx;
            this.ownerOfAddIn = ownerOfAddIn;
            InitializeComponent();
            this.ChangeLocale(this.resManager);
            ExceptionCatcher catcher = new ExceptionCatcher(this.ownerOfAddIn, this);
            this.tprx.runAction_async(catcher);

        }    

        /// <summary>
        /// Disabling Alt+F4 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void WaitDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.F4) && (e.Modifiers == Keys.Alt))
            {
                e.Handled = true;
            }
        }

        #endregion


        #region Public methods

        public void AsyncClose()
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.InvokeRequired)
            {
                CloseDelegate d = new CloseDelegate(AsyncClose);
                this.Invoke(d);
            }
            else
            {
                this.Close();
            }
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
        private void ChangeLocale(ResourceManager rm)
        {
            this.Text = rm.GetString("WaitDialogModuleTitle");
            this.LabelDescription.Text = rm.GetString("WaitDescription");
        }

        #endregion
    }
}