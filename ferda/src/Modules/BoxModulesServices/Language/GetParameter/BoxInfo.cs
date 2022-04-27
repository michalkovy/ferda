using Ice;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

using Object = Ice.Object;

namespace Ferda.Modules.Boxes.Language.GetParameter
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
        	if (mainBoxModulePrxs.Length == 0) return null;
        	BoxModulePrx mainBoxModulePrx = mainBoxModulePrxs[0];
        	if (mainBoxModulePrx == null) return null;

			string parameterName = boxModule.GetPropertyString("ParameterName");
			try
			{
				BoxModulePrx[] connectedBoxModulePrxs = mainBoxModulePrx.getConnections(parameterName);
				if (connectedBoxModulePrxs.Length > 1) return null;
				if (connectedBoxModulePrxs.Length == 1)
				{
					BoxModulePrx connectedBoxModulePrx = connectedBoxModulePrxs[0];
					if (connectedBoxModulePrx == null) return null;
					return connectedBoxModulePrx.getFunctions();
				}
				else
				{
                    return null;
                    // TODO: this was original but Ice stopped supporting Classes to implement interface, what did it break?
                    //PropertyValue propertyValue = mainBoxModulePrx.getProperty(parameterName);
                    //return (propertyValue != null) ? boxModule.Adapter.addWithUUID(propertyValue) : null;
                }
            }
			catch
			{
				return null;
			}
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

        public const string typeIdentifier = "Language.GetParameter";

        protected override string identifier
        {
            get { return typeIdentifier; }
        }

        public override void Validate(BoxModuleI boxModule)
        {
        }

        public override string[] GetFunctionsIceIds(BoxModuleI boxModule)
        {
            return new string[0];
        }
    }
}
