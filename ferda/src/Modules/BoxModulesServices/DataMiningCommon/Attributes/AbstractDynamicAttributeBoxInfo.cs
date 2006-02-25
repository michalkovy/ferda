using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Modules.Boxes.DataMiningCommon.Attributes
{
    /// <summary>
    /// Interface which should be implemented by all dynamic attribute
    /// functionsI objects.
    /// </summary>
	public interface IAbstractDynamicAttribute
	{

        /// <summary>
        /// Gets the column box module proxy.
        /// </summary>
        /// <returns>Proxy of Column box module.</returns>
        BoxModulePrx GetColumnBoxModulePrx();

        /// <summary>
        /// Gets the setting for new attribute box.
        /// </summary>
        /// <returns>Setting for new attribute box.</returns>
        PropertySetting[] GetSettingForNewAttributeBox();
	}

    /// <summary>
    /// Abstract <see cref="T:Ferda.Modules.Boxes.BoxInfo"/> 
    /// class for all dynamic attribute box modules.
    /// </summary>
	public abstract class AbstractDynamicAttributeBoxInfo : AbstractAttributeBoxInfo
	{
        /// <summary>
        /// Gets the function object of abstract attribute.
        /// </summary>
        /// <param name="boxModule">The box module.</param>
        /// <returns>FunctionsI object.</returns>
		public abstract IAbstractDynamicAttribute getFuncIAbstractDynamicAttribute(BoxModuleI boxModule);
		//EachValueOneCategoryAttributeFunctionsI Func = (EachValueOneCategoryAttributeFunctionsI)boxModule.FunctionsIObj;

        /// <summary>
        /// Gets the box modules asking for creation.
        /// </summary>
        /// <param name="localePrefs">The localization preferences.</param>
        /// <param name="boxModule">The box module.</param>
        /// <returns>
        /// Array of <see cref="T:Ferda.Modules.ModuleAskingForCreation">
        /// Modules Asking For Creation</see>.
        /// </returns>
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
