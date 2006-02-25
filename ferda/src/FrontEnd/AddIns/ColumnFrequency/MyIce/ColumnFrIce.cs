using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Ferda.ModulesManager;
using Ferda.FrontEnd.AddIns;
using Ferda.Modules.Boxes.DataMiningCommon.DataMatrix;
using Ferda.Modules.Boxes.DataMiningCommon.Column;
using Ferda.FrontEnd.AddIns.ColumnFrequency;

using System.Resources;

using System.Reflection;


namespace Ferda
{
    namespace FrontEnd.AddIns.ColumnFrequency.MyIce
    {
        public class ColumnFrICe : Ferda.Modules.ModuleForInteractionDisp_
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
            public ColumnFrICe(Ferda.FrontEnd.AddIns.IOwnerOfAddIn ownerOfAddIn)
            {
                this.ownerOfAddIn = ownerOfAddIn;

                //setting the ResManager resource manager and localization string
                resManager = new ResourceManager("Ferda.FrontEnd.AddIns.ColumnFr.Localization_en-US",
                Assembly.GetExecutingAssembly());
                localizationString = "en-US";

            }
            #endregion


            #region Other ice



            public override Ferda.Modules.BoxType[] getAcceptedBoxTypes(Ice.Current __current)
            {
                Modules.BoxType boxType = new Modules.BoxType();
                boxType.neededSockets = new Modules.NeededSocket[0];
                boxType.functionIceId = "::Ferda::Modules::Boxes::DataMiningCommon::Column::ColumnFunctions";

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

                    locale = "Ferda.FrontEnd.AddIns.ColumnFr.Localization_" + locale;

                    resManager = new ResourceManager(locale,
                Assembly.GetExecutingAssembly());

                }

                catch
                {
                }
                return resManager.GetString("ColumnFrequency");
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

                    locale = "Ferda.FrontEnd.AddIns.ColumnFr.Localization_" + locale;

                    resManager = new ResourceManager(locale,
                Assembly.GetExecutingAssembly());

                }

                catch
                {
                }
                return resManager.GetString("ColumnFrequencyModule");
            }

            public override string[] getNeededConnectedSockets(Ice.Current __current)
            {
                return new string[0];

            }



            #endregion


            #region IceRun


            public override void run(Ferda.Modules.BoxModulePrx boxModuleParam, string[] localePrefs, ManagersEnginePrx manager, Ice.Current __current)
            {

                string locale;
                try
                {
                    locale = localePrefs[0];

                    localizationString = locale;

                    locale = "Ferda.FrontEnd.AddIns.ColumnFr.Localization_" + locale;

                    resManager = new ResourceManager(locale,
                Assembly.GetExecutingAssembly());

                }

                catch
                {
                }

                Ferda.Modules.Boxes.DataMiningCommon.Column.ColumnFunctionsPrx prx =

                   Ferda.Modules.Boxes.DataMiningCommon.Column.ColumnFunctionsPrxHelper.checkedCast(boxModuleParam.getFunctions());

                Ferda.Modules.Boxes.DataMiningCommon.Attributes.Attribute.AttributeFunctionsPrx prx1 =
                    Ferda.Modules.Boxes.DataMiningCommon.Attributes.Attribute.AttributeFunctionsPrxHelper.checkedCast(boxModuleParam.getFunctions());


                //prx1.getAbstractAttribute().categories.

                try
                {
                    ColumnInfo columnInfo = prx.getColumnInfo();
                    string label = manager.getProjectInformation().getUserLabel(Ice.Util.identityToString(boxModuleParam.ice_getIdentity()));
                    Ferda.FrontEnd.AddIns.ColumnFrequency.ColumnFrequency control = new ColumnFrequency(localePrefs, columnInfo);
                    this.ownerOfAddIn.ShowDockableControl(control, label + " " + resManager.GetString("ColumnFrequency"));
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
                    else if (ex.restrictionType == Ferda.Modules.restrictionTypeEnum.DbColumn)
                    {
                        MessageBox.Show(resManager.GetString("BadColumnSelectExpression"), resManager.GetString("Error"),
                                   MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    //else
                    {
                        MessageBox.Show(resManager.GetString("InvalidParameters"), resManager.GetString("Error"),
                                   MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }

                catch (Ferda.Modules.NoConnectionInSocketError)
                {
                    MessageBox.Show(resManager.GetString("BoxNotConnected"), resManager.GetString("Error"),
                               MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

            }

            #endregion

        }
    }
}
