using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Modules;
using Ferda.ModulesManager;

namespace Ferda
{
    namespace ShowTable.MyIce
    {
        class MyIceGeneric: Ferda.Modules.ModuleForInteractionDisp_
        {
            public override BoxType[] getAcceptedBoxTypes(Ice.Current __current)
            {
                BoxType a = new BoxType();
                a.neededSockets = null;
                a.functionIceId = "::Ferda::Modules::Boxes::Database::TableNames::Provider";
                return new BoxType[] { a };

                //throw new Exception("The method or operation is not implemented.");
            }

            public override DynamicHelpItem[] getDynamicHelpItems(string[] localePrefs, Ice.Current __current)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public override byte[] getHelpFile(string identifier, Ice.Current __current)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public override HelpFileInfo[] getHelpFileInfoSeq(string[] localePrefs, Ice.Current __current)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public override string getHint(string[] localePrefs, Ice.Current __current)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public override byte[] getIcon(Ice.Current __current)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public override string getLabel(string[] localePrefs, Ice.Current __current)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public override string[] getNeededConnectedSockets(Ice.Current __current)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            public override void run(BoxModulePrx boxModuleParam, string[] localePrefs, ManagersEnginePrx manager, Ice.Current __current)
            {
                throw new Exception("The method or operation is not implemented.");
            }
}
    }
}
