using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Ferda.ModulesManager;
using Ferda.FrontEnd.AddIns;
using Ferda.Modules.Boxes.DataMiningCommon.DataMatrix;
using Ferda.FrontEnd.AddIns.DatabaseInfo;
using System.Resources;
using System.Reflection;

namespace Ferda
{
    namespace FrontEnd.AddIns.DatabaseInfo.MyIce
    {
        public class DatabaseInfoIce : Ferda.Modules.ModuleForInteractionDisp_
        {
            Ferda.FrontEnd.AddIns.IOwnerOfAddIn ownerOfAddIn;

            private ResourceManager resManager;
            private string localizationString;

            public DatabaseInfoIce(Ferda.FrontEnd.AddIns.IOwnerOfAddIn ownerOfAddIn)
            {
                this.ownerOfAddIn = ownerOfAddIn;

                //setting the ResManager resource manager and localization string
                resManager = new ResourceManager("Ferda.FrontEnd.AddIns.DataBaseInfo.Localization_en-US",
                Assembly.GetExecutingAssembly());
                localizationString = "en-US";
            }

            public override Ferda.Modules.BoxType[] getAcceptedBoxTypes(Ice.Current __current)
            {
                Modules.BoxType boxType = new Modules.BoxType();
                boxType.neededSockets = new Modules.NeededSocket[0];
                boxType.functionIceId = "::Ferda::Modules::Boxes::DataMiningCommon::Database::DatabaseFunctions";
                return new Modules.BoxType[] { boxType };
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

                    locale = "Ferda.FrontEnd.AddIns.DataBaseInfo.Localization_" + locale;

                    resManager = new ResourceManager(locale,
                Assembly.GetExecutingAssembly());

                }

                catch
                {
                }
                return resManager.GetString("DataBaseInfoModule");
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

                    locale = "Ferda.FrontEnd.AddIns.DataBaseInfo.Localization_" + locale;

                    resManager = new ResourceManager(locale,
                Assembly.GetExecutingAssembly());

                }

                catch
                {
                }
                return resManager.GetString("DataBaseInfoModule");

            }

            public override string[] getNeededConnectedSockets(Ice.Current __current)
            {
                return new string[0];
            }

            public override void run(Ferda.Modules.BoxModulePrx boxModuleParam, string[] localePrefs, ManagersEnginePrx manager, Ice.Current __current)
            {
                string locale;
                try
                {
                    locale = localePrefs[0];

                    localizationString = locale;

                    locale = "Ferda.FrontEnd.AddIns.DataBaseInfo.Localization_" + locale;

                    resManager = new ResourceManager(locale,
                Assembly.GetExecutingAssembly());

                }

                catch
                {
                }
                Ferda.Modules.Boxes.DataMiningCommon.Database.DatabaseFunctionsPrx prx =
                    Ferda.Modules.Boxes.DataMiningCommon.Database.DatabaseFunctionsPrxHelper.checkedCast(boxModuleParam.getFunctions());

                try
                {

                    string label = manager.getProjectInformation().getUserLabel(Ice.Util.identityToString(boxModuleParam.ice_getIdentity()));
                    Ferda.FrontEnd.AddIns.DatabaseInfo.DataBaseInfo control = new DataBaseInfo(localePrefs, prx.explainDatabaseStructure());
                    this.ownerOfAddIn.ShowDockableControl(control, label + " " + resManager.GetString("DataBaseInfo"));
                }

                catch (Ferda.Modules.BadParamsError ex)
                {
                    if (ex.restrictionType == Ferda.Modules.restrictionTypeEnum.DbConnectionString)
                    {
                        MessageBox.Show(resManager.GetString("BadConnectionString"), resManager.GetString("Error"),
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }

                catch (Ferda.Modules.NoConnectionInSocketError)
                {
                    MessageBox.Show(resManager.GetString("BoxNotConnected"), resManager.GetString("Error"),
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }
    }
}
