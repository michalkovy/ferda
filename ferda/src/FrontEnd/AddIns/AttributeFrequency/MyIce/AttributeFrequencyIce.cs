using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Ferda.ModulesManager;
using Ferda.FrontEnd.AddIns;
using Ferda.Modules.Boxes.DataMiningCommon.DataMatrix;
using Ferda.FrontEnd.AddIns.AttributeFrequency;

using System.Resources;

using System.Reflection;
using System.Threading;
using System.Globalization;


namespace Ferda
{
    namespace FrontEnd.AddIns.AttributeFrequency.MyIce
    {
        public class AttributeFrequencyIce : Ferda.Modules.ModuleForInteractionDisp_
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
            public AttributeFrequencyIce(Ferda.FrontEnd.AddIns.IOwnerOfAddIn ownerOfAddIn)
            {
                this.ownerOfAddIn = ownerOfAddIn;

                //setting the ResManager resource manager and localization string
             //   resManager = new ResourceManager("Ferda.FrontEnd.AddIns.AttributeFrequency.Localization",
          //      Assembly.GetExecutingAssembly());
              //  Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
           //     Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                

                resManager = new ResourceManager("Ferda.FrontEnd.AddIns.AttributeFr.Localization_en-US",
                Assembly.GetExecutingAssembly());
                localizationString = "en-US";

            }
            #endregion


            #region Other ice
            public override Ferda.Modules.BoxType[] getAcceptedBoxTypes(Ice.Current __current)
            {
                Modules.BoxType boxType = new Modules.BoxType();
                boxType.neededSockets = new Modules.NeededSocket[0];
                boxType.functionIceId = "::Ferda::Modules::Boxes::DataMiningCommon::Attributes::AbstractAttributeFunctions";

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
                  //  Thread.CurrentThread.CurrentCulture = new CultureInfo(locale);
               //     Thread.CurrentThread.CurrentUICulture = new CultureInfo(locale);
                    locale = "Ferda.FrontEnd.AddIns.AttributeFrequency.Localization_" + locale;

                    resManager = new ResourceManager(locale,
                Assembly.GetExecutingAssembly());

                }

                catch
                {
                }
                return resManager.GetString("AttributeFrequency");
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
                //    Thread.CurrentThread.CurrentCulture = new CultureInfo(locale);
                 //   Thread.CurrentThread.CurrentUICulture = new CultureInfo(locale);

                    locale = "Ferda.FrontEnd.AddIns.AttributeFrequency.Localization_" + locale;

                    resManager = new ResourceManager(locale,
                Assembly.GetExecutingAssembly());
                }

                catch
                {
                }
                return resManager.GetString("AttributeFrequencyModule");
            }

            public override string[] getNeededConnectedSockets(Ice.Current __current)
            {
                return new string[0];
            }

            #endregion


            #region Run
            public override void run(Ferda.Modules.BoxModulePrx boxModuleParam, string[] localePrefs, ManagersEnginePrx manager, Ice.Current __current)
            {
                
                string locale;
                try
                {
                    locale = localePrefs[0];

                    localizationString = locale;
                    Thread.CurrentThread.CurrentCulture = new CultureInfo(locale);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(locale);

                }

                catch
                {
                }
                
                

                Ferda.Modules.Boxes.DataMiningCommon.Attributes.AbstractAttributeFunctionsPrx prx =
                    Ferda.Modules.Boxes.DataMiningCommon.Attributes.AbstractAttributeFunctionsPrxHelper.checkedCast(boxModuleParam.getFunctions());

                try
                {
                    string label = manager.getProjectInformation().getUserLabel(Ice.Util.identityToString(boxModuleParam.ice_getIdentity()));
                    Ferda.FrontEnd.AddIns.AttributeFrequency.AttributeFrequency control = new AttributeFrequency(prx.getAbstractAttribute(), localePrefs);
                    this.ownerOfAddIn.ShowDockableControl(control, label + " " + resManager.GetString("AttributeFrequency"));
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
                    else
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
