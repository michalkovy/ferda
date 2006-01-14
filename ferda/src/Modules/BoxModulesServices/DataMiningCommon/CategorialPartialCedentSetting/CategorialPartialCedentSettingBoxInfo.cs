using System;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.DataMiningCommon.CategorialPartialCedentSetting
{
	class CategorialPartialCedentSettingBoxInfo : Ferda.Modules.Boxes.BoxInfo
	{
		public const string typeIdentifier = 
			"DataMiningCommon.CategorialPartialCedentSetting";

		protected override string identifier
		{
			get { return typeIdentifier; }
		}

		public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
		{
			CategorialPartialCedentSettingFunctionsI result = new CategorialPartialCedentSettingFunctionsI();
			iceObject = (Ice.Object)result;
			functions = (IFunctions)result;
		}

		public override string[] GetBoxModuleFunctionsIceIds()
		{
			return CategorialPartialCedentSettingFunctionsI.ids__;
		}

		public override string GetDefaultUserLabel(BoxModuleI boxModule)
		{
			return null;
		}

		public override SelectString[] GetPropertyOptions(string propertyName, BoxModuleI boxModule)
		{
            return null;
		}

		public override ModulesAskingForCreation[] GetModulesAskingForCreation(string[] localePrefs, BoxModuleI boxModule)
		{
			return new ModulesAskingForCreation[0] { };
		}
	}
}