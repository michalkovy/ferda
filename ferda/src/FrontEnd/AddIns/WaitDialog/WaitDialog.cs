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


        #endregion

        #region Constructor

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
           // this.ChangeLocale(this.resManager);
           // this.displayer = Displayer;

            this.KeyDown += new KeyEventHandler(WaitDialog_KeyDown);
            this.tprx = tprx;
            this.ownerOfAddIn = ownerOfAddIn;
            InitializeComponent();
            ExceptionCatcher catcher = new ExceptionCatcher(this.ownerOfAddIn,this);
            this.tprx.runAction_async(catcher);
        //    InitializeBackgroundWorker();
        //    this.BackGroundTaskLauncher.RunWorkerAsync();
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


        #region Private methods



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