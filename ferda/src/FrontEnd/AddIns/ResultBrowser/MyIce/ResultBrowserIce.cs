using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Modules;
using System.Resources;
using System.Reflection;
using System.Windows.Forms;


namespace Ferda
{
    namespace FrontEnd.AddIns.ResultBrowser
    {
        class ResultBrowserIce : Ferda.Modules.ModuleForInteractionDisp_
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

            private Ferda.FrontEnd.Properties.IPropertiesDisplayer Displayer;

            #endregion


            #region Constructor
            public ResultBrowserIce(Ferda.FrontEnd.AddIns.IOwnerOfAddIn ownerOfAddIn, Ferda.FrontEnd.Properties.IPropertiesDisplayer Displayer)
		    {
			    this.ownerOfAddIn = ownerOfAddIn;

                //setting the ResManager resource manager and localization string
                resManager = new ResourceManager("Ferda.FrontEnd.AddIns.ResultBrowser.Localization_en-US",
                Assembly.GetExecutingAssembly());
                localizationString = "en-US";

                this.Displayer = Displayer;

            }
            #endregion


            #region Other ice
            public override Ferda.Modules.BoxType[] getAcceptedBoxTypes(Ice.Current __current)
            {
                BoxType a = new BoxType();
                a.neededSockets = new NeededSocket[0];
                a.functionIceId = "::Ferda::Modules::Boxes::AbstractLMTask::AbstractLMTaskFunctions";
                return new BoxType [] {a};
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

                    locale = "Ferda.FrontEnd.AddIns.ResultBrowser.Localization_" + locale;

                    resManager = new ResourceManager(locale,
                Assembly.GetExecutingAssembly());

                }

                catch
                {
                }
                return resManager.GetString("ResultBrowserModule");
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

                    locale = "Ferda.FrontEnd.AddIns.ResultBrowser.Localization_" + locale;

                    resManager = new ResourceManager(locale,
                Assembly.GetExecutingAssembly());

                }

                catch
                {
                }
                return resManager.GetString("ResultBrowserModule");
            }

            public override string[] getNeededConnectedSockets(Ice.Current __current)
            {
                return new string[0];
            }

            #endregion


            #region Run
            public override void run(Ferda.Modules.BoxModulePrx boxModuleParam, string[] localePrefs, Ferda.ModulesManager.ManagersEnginePrx manager, Ice.Current __current)
            {
                string locale;
                try
                {
                    locale = localePrefs[0];

                    localizationString = locale;

                    locale = "Ferda.FrontEnd.AddIns.ResultBrowser.Localization_" + locale;

                    resManager = new ResourceManager(locale,
                Assembly.GetExecutingAssembly());

                }

                catch
                {
                }

                Ice.ObjectPrx prx = boxModuleParam.getFunctions();
                Ice.ObjectPrx prx1 = prx;
                
                Modules.Boxes.AbstractLMTask.AbstractLMTaskFunctionsPrx tprx =
                    Modules.Boxes.AbstractLMTask.AbstractLMTaskFunctionsPrxHelper.checkedCast(prx);
                Modules.HypothesisStruct [] hypotheses = tprx.getResult();

                Modules.Boxes.AbstractLMTask.QuantifierProvider[] used_quantifiers =
                    tprx.getQuantifierProviders();

                try
                {
                    FrontEnd.AddIns.ResultBrowser.FerdaResultBrowserControl control = new FrontEnd.AddIns.ResultBrowser.FerdaResultBrowserControl(localePrefs, hypotheses, used_quantifiers, this.Displayer);
                    this.ownerOfAddIn.ShowDockableControl(control, resManager.GetString("ResultBrowserControl"));
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
