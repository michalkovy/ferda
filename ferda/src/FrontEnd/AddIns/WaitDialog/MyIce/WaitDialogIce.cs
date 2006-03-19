using System;
using System.Collections.Generic;
using System.Text;
using System.Resources;
using System.Reflection;
using System.Windows.Forms;
using Ferda.Modules;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractLMTask;

namespace Ferda.FrontEnd.AddIns.WaitDialog
{
    class WaitDialogIce : Ferda.Modules.ModuleForInteractionDisp_
    {
        #region Private variables

        Ferda.FrontEnd.AddIns.IOwnerOfAddIn ownerOfAddIn;

        /// <summary>
        /// L10n resource manager
        /// </summary>
        private ResourceManager resManager;

        /// <summary>
        /// L10n string, for now en-US or cs-CZ
        /// </summary>
        private string localizationString;

        #endregion


        #region Constructor

        public WaitDialogIce(Ferda.FrontEnd.AddIns.IOwnerOfAddIn ownerOfAddIn)
        {
            this.ownerOfAddIn = ownerOfAddIn;
            //setting the ResManager resource manager and localization string
            resManager = new ResourceManager("Ferda.FrontEnd.AddIns.WaitDialog.Localization_en-US",
            Assembly.GetExecutingAssembly());
            localizationString = "en-US";
          //  this.Displayer = Displayer;
        }

        #endregion

        #region Other ice

        public override Ferda.Modules.BoxType[] getAcceptedBoxTypes(Ice.Current __current)
        {
            BoxType a = new BoxType();
            a.neededSockets = new NeededSocket[0];
            a.functionIceId = "::Ferda::Modules::Boxes::LISpMinerTasks::AbstractLMTask::AbstractLMTaskFunctions";
            return new BoxType[] { a };
        }

        public override Ferda.Modules.DynamicHelpItem[] getDynamicHelpItems(string[] localePrefs, Ice.Current __current)
        {
            return null;
        }

        public override byte[] getHelpFile(string identifier, Ice.Current __current)
        {
            return null;
        }

        public override Ferda.Modules.HelpFileInfo[] getHelpFileInfoSeq(string[] localePrefs, Ice.Current __current)
        {
            return null;
        }

        public override string getHint(string[] localePrefs, Ice.Current __current)
        {
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
            }
            return resManager.GetString("WaitDialogModule");
        }
        public override byte[] getIcon(Ice.Current __current)
        {
            return null;
        }

        public override string getLabel(string[] localePrefs, Ice.Current __current)
        {
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
            }
            return resManager.GetString("WaitDialogModule");
        }

        public override string[] getNeededConnectedSockets(Ice.Current __current)
        {
            return new string[0];
        }

        #endregion


        public override void run(Ferda.Modules.BoxModulePrx boxModuleParam, string[] localePrefs, Ferda.ModulesManager.ManagersEnginePrx manager, Ice.Current current__)
        {
            string locale;
            try
            {
                locale = localePrefs[0];
                localizationString = locale;
                locale = "Ferda.FrontEnd.AddIns.ResultBrowser.Localization_" + locale;
                resManager = new ResourceManager(locale, Assembly.GetExecutingAssembly());
            }
            catch
            {
            }

            //getting proxy for task identifier
            Ice.ObjectPrx prx2 = boxModuleParam.getMyFactory();

            Modules.BoxModuleFactoryPrx tprx2 =
            Modules.BoxModuleFactoryPrxHelper.checkedCast(prx2);


            //getting proxy for hypotheses and quantifiers
            Ice.ObjectPrx prx = boxModuleParam.getFunctions();
            //manager.getProjectInformation().getUserLabel

            AbstractLMTaskFunctionsPrx tprx = AbstractLMTaskFunctionsPrxHelper.checkedCast(prx);

            Ferda.FrontEnd.AddIns.WaitDialog.WaitDialog control = new Ferda.FrontEnd.AddIns.WaitDialog.WaitDialog(localePrefs, tprx, this.ownerOfAddIn);
            this.ownerOfAddIn.ShowForm(control);    
        }
    }
}
