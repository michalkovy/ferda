using System;
using System.Collections.Generic;
using System.Text;
using System.Resources;
using System.Reflection;
using System.Windows.Forms;

namespace Ferda.FrontEnd.AddIns.ShowTable.MyIce
{
    class ShowTableIce : Ferda.Modules.ModuleForInteractionDisp_
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
        public ShowTableIce(Ferda.FrontEnd.AddIns.IOwnerOfAddIn ownerOfAddIn)
        {
            this.ownerOfAddIn = ownerOfAddIn;

            //setting the ResManager resource manager and localization string
            resManager = new ResourceManager("Ferda.FrontEnd.AddIns.ShowTable.Localization_en-US",
            Assembly.GetExecutingAssembly());
            localizationString = "en-US";

        }
        #endregion

        public override Ferda.Modules.BoxType[] getAcceptedBoxTypes(Ice.Current current__)
        {
            Modules.BoxType boxType = new Modules.BoxType();
            boxType.neededSockets = new Modules.NeededSocket[0];
            boxType.functionIceId = "::Ferda::Modules::Boxes::DataMiningCommon::DataMatrix::DataMatrixFunctions";
            return new Modules.BoxType[] { boxType };
        }

        public override Ferda.Modules.DynamicHelpItem[] getDynamicHelpItems(string[] localePrefs, Ice.Current current__)
        {
            return null;
        }

        public override byte[] getHelpFile(string identifier, Ice.Current current__)
        {
            return null;
        }

        public override Ferda.Modules.HelpFileInfo[] getHelpFileInfoSeq(string[] localePrefs, Ice.Current current__)
        {
            return null;
        }

        public override string getHint(string[] localePrefs, Ice.Current current__)
        {
            string locale;
            try
            {
                locale = localePrefs[0];

                localizationString = locale;

                locale = "Ferda.FrontEnd.AddIns.ShowTable.Localization_" + locale;

                resManager = new ResourceManager(locale,
            Assembly.GetExecutingAssembly());

            }

            catch
            {
            }


            return resManager.GetString("ShowTableModule");
        }

        public override byte[] getIcon(Ice.Current current__)
        {
            return null;
        }

        public override string getLabel(string[] localePrefs, Ice.Current current__)
        {
            string locale;
            try
            {
                locale = localePrefs[0];

                localizationString = locale;

                locale = "Ferda.FrontEnd.AddIns.ShowTable.Localization_" + locale;

                resManager = new ResourceManager(locale,
            Assembly.GetExecutingAssembly());

            }

            catch
            {
            }
            return resManager.GetString("ShowTableModule");
        }

        public override string[] getNeededConnectedSockets(Ice.Current current__)
        {
            return new string[0];
        }

        public override void run(Ferda.Modules.BoxModulePrx boxModuleParam, string[] localePrefs, Ferda.ModulesManager.ManagersEnginePrx manager, Ice.Current current__)
        {
            string locale;
            try
            {
                locale = localePrefs[0];

                localizationString = locale;

                locale = "Ferda.FrontEnd.AddIns.ShowTable.Localization_" + locale;

                resManager = new ResourceManager(locale,
            Assembly.GetExecutingAssembly());

            }

            catch
            {
            }

            Ferda.Modules.Boxes.DataMiningCommon.DataMatrix.DataMatrixFunctionsPrx prx =
                    Ferda.Modules.Boxes.DataMiningCommon.DataMatrix.DataMatrixFunctionsPrxHelper.checkedCast(boxModuleParam.getFunctions());
            try
            {

                string label = manager.getProjectInformation().getUserLabel(Ice.Util.identityToString(boxModuleParam.ice_getIdentity()));
                Ferda.FrontEnd.AddIns.ShowTable.ShowTableControl control = new ShowTableControl(localePrefs, prx.getColumns(),prx.getDataMatrix());
                this.ownerOfAddIn.ShowDockableControl(control, resManager.GetString("Show") + " " + label);
            }

            catch (Ferda.Modules.NoConnectionInSocketError)
            {
                MessageBox.Show(resManager.GetString("BoxNotConnected"), resManager.GetString("Error"),
                           MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            catch (Ferda.Modules.BadParamsError ex)
            {
                if (ex.restrictionType == Ferda.Modules.restrictionTypeEnum.DbConnectionString)
                {
                    MessageBox.Show(resManager.GetString("BadConnectionString"), resManager.GetString("Error"),
                               MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (ex.restrictionType == Ferda.Modules.restrictionTypeEnum.DbTable)
                {
                    MessageBox.Show(resManager.GetString("NoDataMatrix"), resManager.GetString("Error"),
                               MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }
    }
}
