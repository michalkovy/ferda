using System;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.DataMiningCommon.BooleanPartialCedentSetting
{
	class BooleanPartialCedentSettingBoxInfo : Ferda.Modules.Boxes.BoxInfo
	{
		public const string typeIdentifier = 
			"DataMiningCommon.BooleanPartialCedentSetting";

		protected override string identifier
		{
			get { return typeIdentifier; }
		}

		public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
		{
			BooleanPartialCedentSettingFunctionsI result = new BooleanPartialCedentSettingFunctionsI();
			iceObject = (Ice.Object)result;
			functions = (IFunctions)result;
		}

		public override string[] GetBoxModuleFunctionsIceIds()
		{
			return BooleanPartialCedentSettingFunctionsI.ids__;
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