using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Modules.Boxes.SDFFTTask.Quantifiers
{
	public abstract class AbstractSDFFTTaskQuantifierBoxInfo : BoxInfo
	{
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
			Dictionary<string, ModulesAskingForCreation> modulesAFC = this.getModulesAskingForCreationNonDynamic(localePrefs);
			List<ModulesAskingForCreation> result = new List<ModulesAskingForCreation>();
			ModulesAskingForCreation moduleAFC;
			ModulesConnection moduleConnection;
			ModuleAskingForCreation singleModuleAFC;
			//PropertySetting propertySetting;
			//Ferda.ModulesManager.BoxModuleProjectInformationPrx projectInfoPrx = boxModule.Manager.getProjectInformation();
			//string label = projectInfoPrx.getUserLabel(boxModule.StringIceIdentity);
			foreach (string moduleAFCName in modulesAFC.Keys)
			{
				moduleAFC = modulesAFC[moduleAFCName];
				moduleConnection = new ModulesConnection();
				singleModuleAFC = new ModuleAskingForCreation();
				switch (moduleAFCName)
				{
					case "SDFFTTask":
						moduleConnection.socketName = "SDFFTQuantifier";
						singleModuleAFC.newBoxModuleIdentifier =
							Ferda.Modules.Boxes.SDFFTTask.SDFFTTaskBoxInfo.typeIdentifier;
						break;
					default:
						throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(moduleAFCName);
				}
				moduleConnection.boxModuleParam = boxModule.MyProxy;
				singleModuleAFC.modulesConnection = new ModulesConnection[] { moduleConnection };
				moduleAFC.newModules = new ModuleAskingForCreation[] { singleModuleAFC };
				result.Add(moduleAFC);
			}
			return result.ToArray();
		}
	}
}