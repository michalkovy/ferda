using System;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.DataMiningCommon.Column
{
    class ColumnBoxInfo : Ferda.Modules.Boxes.BoxInfo
    {
        public const string typeIdentifier =
            "DataMiningCommon.Column";

        protected override string identifier
        {
            get { return typeIdentifier; }
        }

        public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
        {
            ColumnFunctionsI result = new ColumnFunctionsI();
            iceObject = (Ice.Object)result;
            functions = (IFunctions)result;
        }

        public override string[] GetBoxModuleFunctionsIceIds()
        {
            return ColumnFunctionsI.ids__;
        }

        public override string GetDefaultUserLabel(BoxModuleI boxModule)
        {
            return boxModule.GetPropertyString("Name");
        }

        public override SelectString[] GetPropertyOptions(String propertyName, BoxModuleI boxModule)
        {
            switch (propertyName)
            {
                case "Name":
                    return BoxInfoHelper.StringArrayToSelectStringArray(
                        ((ColumnFunctionsI)boxModule.FunctionsIObj).GetColumnsNames()
                        );
                default:
                    return null;
            }
        }

        public override ModulesAskingForCreation[] GetModulesAskingForCreation(string[] localePrefs, BoxModuleI boxModule)
        {
            Dictionary<string, ModulesAskingForCreation> modulesAFC = this.getModulesAskingForCreationNonDynamic(localePrefs);
            List<ModulesAskingForCreation> result = new List<ModulesAskingForCreation>();
            ModulesAskingForCreation moduleAFC;
            ModulesConnection moduleConnection;
            ModuleAskingForCreation singleModuleAFC;
            PropertySetting propertySetting;
            Ferda.ModulesManager.BoxModuleProjectInformationPrx projectInfoPrx = boxModule.Manager.getProjectInformation();
            string label = projectInfoPrx.getUserLabel(boxModule.StringIceIdentity);
            ValueSubTypeEnum columnValueSubType = ((ColumnFunctionsI)boxModule.FunctionsIObj).GetColumnSubType();
            foreach (string moduleAFCName in modulesAFC.Keys)
            {
                moduleAFC = modulesAFC[moduleAFCName];
                moduleConnection = new ModulesConnection();
                singleModuleAFC = new ModuleAskingForCreation();
                switch (moduleAFCName)
                {
                    case "MultiColumn":
                        moduleConnection.socketName = "Column";
                        //singleModuleAFC.newBoxModuleIdentifier =
                        //	Ferda.Modules.Boxes.DataMiningCommon.MultiColumn.MultiColumnBoxInfo.typeIdentifier;
                        continue; //TODO MultiColumn is not implemented yet.
                    case "Attribute":
                    case "EachValueOneCategoryAttribute":
                    case "EquifrequencyIntervalsAttribute":
                    case "EquidistantIntervalsAttribute":
                        if (moduleAFCName == "Attribute")
                        {
                            singleModuleAFC.newBoxModuleIdentifier =
                                Ferda.Modules.Boxes.DataMiningCommon.Attributes.Attribute.AttributeBoxInfo.typeIdentifier;
                        }
                        if (moduleAFCName == "EachValueOneCategoryAttribute")
                        {
                            singleModuleAFC.newBoxModuleIdentifier =
                                Ferda.Modules.Boxes.DataMiningCommon.Attributes.EachValueOneCategoryAttribute.EachValueOneCategoryAttributeBoxInfo.typeIdentifier;
                        }
                        if (moduleAFCName == "EquifrequencyIntervalsAttribute")
                        {
                            if (!Ferda.Modules.Helpers.Data.Column.IsColumSubTypeCardinal(columnValueSubType)
                                || columnValueSubType == ValueSubTypeEnum.DateTimeType)
                                continue;
                            singleModuleAFC.newBoxModuleIdentifier =
                                Ferda.Modules.Boxes.DataMiningCommon.Attributes.EquifrequencyIntervalsAttribute.EquifrequencyIntervalsAttributeBoxInfo.typeIdentifier;
                        }
                        if (moduleAFCName == "EquidistantIntervalsAttribute")
                        {
                            if ((columnValueSubType == ValueSubTypeEnum.Unknown)
                                || (columnValueSubType == ValueSubTypeEnum.DateTimeType)
                                //|| (columnValueSubType == ValueSubTypeEnum.TimeType)
                                || (columnValueSubType == ValueSubTypeEnum.BooleanType))
                                continue;
                            singleModuleAFC.newBoxModuleIdentifier =
                                Ferda.Modules.Boxes.DataMiningCommon.Attributes.EquidistantIntervalsAttribute.EquidistantIntervalsAttributeBoxInfo.typeIdentifier;
                        }
                        moduleConnection.socketName = "ColumnOrDerivedColumn";
                        propertySetting = new PropertySetting();
                        propertySetting.propertyName = "NameInLiterals";
                        propertySetting.value = new Ferda.Modules.StringTI(label);
                        singleModuleAFC.propertySetting = new PropertySetting[] { propertySetting };
                        break;
                    default:
                        throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(moduleAFCName);
                }
                moduleConnection.boxModuleParam = boxModule.MyProxy;
                singleModuleAFC.modulesConnection = new ModulesConnection[] { moduleConnection };
                singleModuleAFC.newBoxModuleUserLabel = new string[] { label };
                moduleAFC.newModules = new ModuleAskingForCreation[] { singleModuleAFC };
                result.Add(moduleAFC);
            }
            return result.ToArray();
        }

        public override PropertyValue GetReadOnlyPropertyValue(String propertyName, BoxModuleI boxModule)
        {
            ColumnFunctionsI Func = (ColumnFunctionsI)boxModule.FunctionsIObj;
            switch (propertyName)
            {
                case "ValueSubType":
                    return new Ferda.Modules.StringTI(Func.GetColumnSubType().ToString());
                case "ValueMin":
                    return new Ferda.Modules.StringTI(Func.GetStatistics().ValueMin);
                case "ValueMax":
                    return new Ferda.Modules.StringTI(Func.GetStatistics().ValueMax);
                case "ValueAverage":
                    return new Ferda.Modules.StringTI(Func.GetStatistics().ValueAverage);
                case "ValueVariability":
                    return new Ferda.Modules.DoubleTI(Func.GetStatistics().ValueVariability);
                case "ValueStandardDeviation":
                    return new Ferda.Modules.DoubleTI(Func.GetStatistics().ValueStandardDeviation);
                case "ValueDistincts":
                    return new Ferda.Modules.LongTI(Func.GetStatistics().ValueDistincts);
                default:
                    return null;
                    //throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(propertyName);
            }
        }
    }
}