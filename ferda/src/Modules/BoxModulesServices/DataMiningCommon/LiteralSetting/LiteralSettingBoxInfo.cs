using System;
using System.Collections.Generic;
using Ferda.Modules.Helpers.Common;

namespace Ferda.Modules.Boxes.DataMiningCommon.LiteralSetting
{
	class LiteralSettingBoxInfo : Ferda.Modules.Boxes.BoxInfo
	{
		public const string typeIdentifier = 
			"DataMiningCommon.LiteralSetting";

		protected override string identifier
		{
			get { return typeIdentifier; }
		}

		public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
		{
			LiteralSettingFunctionsI result = new LiteralSettingFunctionsI();
			iceObject = (Ice.Object)result;
			functions = (IFunctions)result;
		}

		public override string[] GetBoxModuleFunctionsIceIds()
		{
			return LiteralSettingFunctionsI.ids__;
		}

        /// <summary>
        /// Gets default value for box module user label.
        /// </summary>
		public override string GetDefaultUserLabel(BoxModuleI boxModule)
		{
			//get atom`s userLabel
			string atomsUserLabel = "???";
			BoxModulePrx atomBoxModulePrx;
			if (Ferda.Modules.Boxes.SocketConnections.TryGetBoxModulePrx(boxModule, "AtomSetting", out atomBoxModulePrx))
			{
				string[] atomDefaultUserLabel = atomBoxModulePrx.getDefaultUserLabel();
				if (atomDefaultUserLabel.Length > 0)
					atomsUserLabel = atomDefaultUserLabel[0];
			}

			GaceTypeEnum gaceType = (GaceTypeEnum)Enum.Parse(typeof(GaceTypeEnum), boxModule.GetPropertyString("GaceType"));
			string gaceTypeShort = "";
			switch (gaceType)
			{
				case GaceTypeEnum.Positive:
					break;
				case GaceTypeEnum.Negative:
					gaceTypeShort = Constants.Negation.ToString();
					break;
				case GaceTypeEnum.Both:
					gaceTypeShort = Constants.LeftFunctionBracket + Constants.Negation.ToString() + Constants.RightFunctionBracket;
					break;
				default:
					throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(gaceType);
			}
			LiteralTypeEnum literalType = (LiteralTypeEnum)Enum.Parse(typeof(LiteralTypeEnum), boxModule.GetPropertyString("LiteralType"));
			string shortLiteralType = this.GetPropertyOptionShortLocalizedLabel("LiteralType", literalType.ToString(), boxModule.LocalePrefs);
			return gaceTypeShort + atomsUserLabel + shortLiteralType;
		}

        /// <summary>
        /// Gets array of <see cref="T:Ferda.Modules.SelectString"/> as
        /// options for property, whose options are dynamically variable.
        /// </summary>
		public override SelectString[] GetPropertyOptions(string propertyName, BoxModuleI boxModule)
		{
            return null;
		}

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
					case "BooleanPartialCedentSetting":
						moduleConnection.socketName = "LiteralSetting";
						singleModuleAFC.newBoxModuleIdentifier =
							Ferda.Modules.Boxes.DataMiningCommon.BooleanPartialCedentSetting.BooleanPartialCedentSettingBoxInfo.typeIdentifier;
						break;
					case "EquivalenceClass":
						moduleConnection.socketName = "LiteralSettingOrAttribute";
						singleModuleAFC.newBoxModuleIdentifier =
							Ferda.Modules.Boxes.DataMiningCommon.EquivalenceClass.EquivalenceClassBoxInfo.typeIdentifier;
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