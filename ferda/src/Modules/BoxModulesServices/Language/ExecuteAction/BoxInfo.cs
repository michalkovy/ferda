using Ice;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

using Object = Ice.Object;

namespace Ferda.Modules.Boxes.Language.ExecuteAction
{
	internal class BoxInfo : Boxes.BoxInfo
	{
		public override void CreateFunctions(BoxModuleI boxModule, out Object iceObject, out IFunctions functions)
		{
	 	   iceObject = null;
	 	   functions = null;
	    }

        public override string[] GetBoxModuleFunctionsIceIds()
        {
            return new string[0];
        }
        
        public override ObjectPrx GetFunctionsObjPrx(BoxModuleI boxModule)
        {
        	BoxModulePrx[] mainBoxModulePrxs = boxModule.getConnections("Box",null);
        	if (mainBoxModulePrxs.Length != 1) return null;
        	BoxModulePrx mainBoxModulePrx = mainBoxModulePrxs[0];
        	if (mainBoxModulePrx == null) return null;
        	
        	try
        	{
        		string actionName = boxModule.GetPropertyString("ActionName");
        		mainBoxModulePrx.runAction(actionName);
        	}
        	catch(Ferda.Modules.BoxRuntimeError e)
        	{
        		boxModule.Manager.getOutputInterface().writeMsg(Ferda.ModulesManager.MsgType.Error, "BoxRuntimeError " + e.boxIdentity, e.userMessage);
        		return null;
        	}
        	catch(System.Exception e)
        	{
        		boxModule.Manager.getOutputInterface().writeMsg(Ferda.ModulesManager.MsgType.Error, e.GetType().FullName, e.Message);
        		return null;
        	}
        	
        	mainBoxModulePrxs = boxModule.getConnections("Function",null);
        	if (mainBoxModulePrxs.Length != 1) return null;
        	mainBoxModulePrx = mainBoxModulePrxs[0];
        	if (mainBoxModulePrx == null) return null;
        	return mainBoxModulePrx.getFunctions();
		}

        public override string GetDefaultUserLabel(BoxModuleI boxModule)
        {
            return null;
        }

        public override ModulesAskingForCreation[] GetModulesAskingForCreation(string[] localePrefs,
                                                                               BoxModuleI boxModule)
        {
            return new ModulesAskingForCreation[0];
        }

        public override SelectString[] GetPropertyOptions(string propertyName, BoxModuleI boxModule)
        {
            return null;
        }

        public const string typeIdentifier = "Language.ExecuteAction";

        protected override string identifier
        {
            get { return typeIdentifier; }
        }

        public override void Validate(BoxModuleI boxModule)
        {
        }
    }
}
