using System;
using System.Collections.Generic;
using Ferda.Modules.Helpers.Common;

namespace Ferda.Modules.Boxes.DataMiningCommon.AtomSetting
{
    class AtomSettingBoxInfo : Ferda.Modules.Boxes.BoxInfo
    {
        public const string typeIdentifier =
            "DataMiningCommon.AtomSetting";

        protected override string identifier
        {
            get { return typeIdentifier; }
        }

        public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
        {
            AtomSettingFunctionsI result = new AtomSettingFunctionsI();
            iceObject = (Ice.Object)result;
            functions = (IFunctions)result;
        }

        public override string[] GetBoxModuleFunctionsIceIds()
        {
            return AtomSettingFunctionsI.ids__;
        }

        /// <summary>
        /// Gets default value for box module user label.
        /// </summary>
        public override string GetDefaultUserLabel(BoxModuleI boxModule)
        {
            CoefficientTypeEnum coefficientType = (CoefficientTypeEnum)Enum.Parse(typeof(CoefficientTypeEnum), boxModule.GetPropertyString("CoefficientType"));
            string shortCoefficientType = this.GetPropertyOptionShortLocalizedLabel("CoefficientType", coefficientType.ToString(), boxModule.LocalePrefs);

            //get attribute`s userLabel
            string attributesUserLabel = "???";
            BoxModulePrx attributeBoxModulePrx;
            if (Ferda.Modules.Boxes.SocketConnections.TryGetBoxModulePrx(boxModule, "Attribute", out attributeBoxModulePrx))
            {
                string[] attributeDefaultUserLabel = attributeBoxModulePrx.getDefaultUserLabel();
                if (attributeDefaultUserLabel.Length > 0)
                    attributesUserLabel = attributeDefaultUserLabel[0];
            }

            string result;
            switch (coefficientType)
            {
                case CoefficientTypeEnum.OneParticularCategory:
                    result = (attributesUserLabel != "???") ? attributesUserLabel : shortCoefficientType;
                    result += Constants.LeftFunctionBracket
                        + boxModule.GetPropertyString("Category")
                        + Constants.RightFunctionBracket;
                    break;
                default:
                    result =
                        attributesUserLabel
                        + Constants.LeftFunctionBracket
                        + shortCoefficientType
                        + Constants.LeftEnum
                        + boxModule.GetPropertyLong("MinLen").ToString()
                        + Constants.RangeSeparator
                        + boxModule.GetPropertyLong("MaxLen").ToString()
                        + Constants.RightEnum
                        + Constants.RightFunctionBracket;
                    break;
            }
            return result;
        }

        /// <summary>
        /// Gets array of <see cref="T:Ferda.Modules.SelectString"/> as
        /// options for property, whose options are dynamically variable.
        /// </summary>
        public override SelectString[] GetPropertyOptions(string propertyName, BoxModuleI boxModule)
        {
            switch (propertyName)
            {
                case "Category":
                    return ((AtomSettingFunctionsI)boxModule.FunctionsIObj).GetPropertyCategoriesNames();
                default:
                    return null;
            }
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
                    case "LiteralSetting":
                        moduleConnection.socketName = "AtomSetting";
                        singleModuleAFC.newBoxModuleIdentifier =
                            Ferda.Modules.Boxes.DataMiningCommon.LiteralSetting.LiteralSettingBoxInfo.typeIdentifier;
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