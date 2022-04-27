using Ice;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

using Object = Ice.Object;

namespace Ferda.Modules.Boxes.Language.Math.IfThenElse
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
        	BoxModulePrx[] conditionBoxPrxs = boxModule.getConnections("if",null);
        	if (conditionBoxPrxs == null || conditionBoxPrxs.Length == 0) return null;
        	BoxModulePrx conditionBoxPrx = conditionBoxPrxs[0];
        	if (conditionBoxPrx == null) return null;
        	Ice.ObjectPrx objectPrx = conditionBoxPrx.getFunctions();
        	Ferda.Modules.BoolTInterfacePrx conditionPrx = Ferda.Modules.BoolTInterfacePrxHelper.checkedCast(objectPrx);
        	if (conditionPrx == null) return null;
        	bool conditionValue = conditionPrx.getBoolValue();
        	
        	BoxModulePrx[] resultBoxPrxs = (conditionValue) ? boxModule.getConnections("then",null) : boxModule.getConnections("else",null);
        	if (resultBoxPrxs == null || resultBoxPrxs.Length == 0) return null;
        	BoxModulePrx resultBoxPrx = resultBoxPrxs[0];
        	if (resultBoxPrx == null) return null;
        	return resultBoxPrx.getFunctions();
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

        public const string typeIdentifier = "Language.Math.IfThenElse";

        protected override string identifier
        {
            get { return typeIdentifier; }
        }

        public override PropertyValue GetReadOnlyPropertyValue(string propertyName, BoxModuleI boxModule)
        {
            switch (propertyName)
            {
                default:
                    throw new NotImplementedException();
            }
        }

        public override void RunAction(string actionName, BoxModuleI boxModule)
        {
            switch (actionName)
            {
                default:
                    throw Exceptions.NameNotExistError(null, actionName);
            }
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
