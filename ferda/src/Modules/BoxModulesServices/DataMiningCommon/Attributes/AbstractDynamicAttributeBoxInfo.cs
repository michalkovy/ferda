using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Modules.Boxes.DataMiningCommon.Attributes
{
	public interface IAbstractDynamicAttribute
	{
		BoxModulePrx GetColumnBoxModulePrx();
		PropertySetting[] GetSettingForNewAttributeBox();
	}

	public abstract class AbstractDynamicAttributeBoxInfo : AbstractAttributeBoxInfo
	{
		public abstract IAbstractDynamicAttribute getFuncIAbstractDynamicAttribute(BoxModuleI boxModule);
		//EachValueOneCategoryAttributeFunctionsI Func = (EachValueOneCategoryAttributeFunctionsI)boxModule.FunctionsIObj;

		public override ModulesAskingForCreation[] GetModulesAskingForCreation(string[] localePrefs, BoxModuleI boxModule)
		{
			Dictionary<string, ModulesAskingForCreation> modulesAFC = this.getModulesAskingForCreationNonDynamic(localePrefs);
			List<ModulesAskingForCreation> result = new List<ModulesAskingForCreation>();
			ModulesAskingForCreation moduleAFC;
			ModulesConnection moduleConnection;
			ModuleAskingForCreation singleModuleAFC;
			//Ferda.ModulesManager.BoxModuleProjectInformationPrx projectInfoPrx = boxModule.Manager.getProjectInformation();
			foreach (string moduleAFCName in modulesAFC.Keys)
			{
				moduleAFC = modulesAFC[moduleAFCName];
				moduleConnection = new ModulesConnection();
				singleModuleAFC = new ModuleAskingForCreation();
				switch (moduleAFCName)
				{
					case "AtomSetting":
						moduleConnection.socketName = "Attribute";
						//moduleAFC.newBoxModuleUserLabel = new string[] { projectInfoPrx.getUserLabel(boxModule.StringIceIdentity) };
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
						//moduleAFC.newBoxModuleUserLabel = new string[] { projectInfoPrx.getUserLabel(boxModule.StringIceIdentity) };
						moduleConnection.boxModuleParam = boxModule.MyProxy;
						singleModuleAFC.newBoxModuleIdentifier =
							Ferda.Modules.Boxes.DataMiningCommon.CategorialPartialCedentSetting.CategorialPartialCedentSettingBoxInfo.typeIdentifier;
						break;
					case "Attribute":
						moduleConnection.socketName = "ColumnOrDerivedColumn";
						//moduleAFC.newBoxModuleUserLabel = new string[] { projectInfoPrx.getUserLabel(boxModule.StringIceIdentity) };
						singleModuleAFC.newBoxModuleIdentifier =
							Ferda.Modules.Boxes.DataMiningCommon.Attributes.Attribute.AttributeBoxInfo.typeIdentifier;
						try
						{
							IAbstractDynamicAttribute func = this.getFuncIAbstractDynamicAttribute(boxModule);
							moduleConnection.boxModuleParam = func.GetColumnBoxModulePrx();
							singleModuleAFC.propertySetting = func.GetSettingForNewAttributeBox();
						}
                        catch (Ferda.Modules.BoxRuntimeError) { continue; }
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
	}
}
