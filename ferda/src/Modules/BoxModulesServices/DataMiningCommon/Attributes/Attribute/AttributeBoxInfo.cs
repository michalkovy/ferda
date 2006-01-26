using System;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.DataMiningCommon.Attributes.Attribute
{
	class AttributeBoxInfo : Ferda.Modules.Boxes.DataMiningCommon.Attributes.AbstractAttributeBoxInfo
	{
		public const string typeIdentifier = 
			"DataMiningCommon.Attributes.Attribute";

		protected override string identifier
		{
			get { return typeIdentifier; }
		}

		public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
		{
			AttributeFunctionsI result = new AttributeFunctionsI();
			iceObject = (Ice.Object)result;
			functions = (IFunctions)result;
		}

		public override string[] GetBoxModuleFunctionsIceIds()
		{
			return AttributeFunctionsI.ids__;
		}

		public override ModulesAskingForCreation[] GetModulesAskingForCreation(string[] localePrefs, BoxModuleI boxModule)
		{
			Dictionary<string, ModulesAskingForCreation> modulesAFC = this.getModulesAskingForCreationNonDynamic(localePrefs);
			List<ModulesAskingForCreation> result = new List<ModulesAskingForCreation>();
			ModulesAskingForCreation moduleAFC;
			ModulesConnection moduleConnection;
			ModuleAskingForCreation singleModuleAFC;
			foreach (string moduleAFCName in modulesAFC.Keys)
			{
				moduleAFC = modulesAFC[moduleAFCName];
				moduleConnection = new ModulesConnection();
				singleModuleAFC = new ModuleAskingForCreation();
				switch (moduleAFCName)
				{
					case "AtomSetting":
						moduleConnection.socketName = "Attribute";
						moduleConnection.boxModuleParam = boxModule.MyProxy;
						singleModuleAFC.newBoxModuleIdentifier =
							Ferda.Modules.Boxes.DataMiningCommon.AtomSetting.AtomSettingBoxInfo.typeIdentifier;
						break;
					case "EquivalenceClass":
						moduleConnection.socketName = "LiteralSettingOrAttribute";
						moduleConnection.boxModuleParam = boxModule.MyProxy;
						singleModuleAFC.newBoxModuleIdentifier =
							Ferda.Modules.Boxes.DataMiningCommon.EquivalenceClass.EquivalenceClassBoxInfo.typeIdentifier;
						break;
					case "CategorialPartialCedentSetting":
						moduleConnection.socketName = "Attribute";
						moduleConnection.boxModuleParam = boxModule.MyProxy;
						singleModuleAFC.newBoxModuleIdentifier =
							Ferda.Modules.Boxes.DataMiningCommon.CategorialPartialCedentSetting.CategorialPartialCedentSettingBoxInfo.typeIdentifier;
						break;
					default:
						throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(moduleAFCName);
				}
				singleModuleAFC.modulesConnection = new ModulesConnection[] { moduleConnection };
				moduleAFC.newModules = new ModuleAskingForCreation[] { singleModuleAFC };
				result.Add(moduleAFC);
			}
			return result.ToArray();
		}

		public override PropertyValue GetReadOnlyPropertyValue(String propertyName, BoxModuleI boxModule)
		{
			AttributeFunctionsI Func = (AttributeFunctionsI)boxModule.FunctionsIObj;
			switch (propertyName)
			{
				case "CountOfCategories":
					return new Ferda.Modules.LongTI(Func.CountOfCategories());
				default:
					throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(propertyName);
			}
		}

		protected override IAbstractAttribute getFuncIAbstractAttribute(BoxModuleI boxModule)
		{
			AttributeFunctionsI Func = (AttributeFunctionsI)boxModule.FunctionsIObj;
			return Func;
		}
	}
}