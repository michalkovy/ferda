using System;
using System.Collections.Generic;
using Object=Ice.Object;

namespace Ferda.Modules.Boxes.DataPreparation.Datasource.DataTable
{
    internal class BoxInfo : Boxes.BoxInfo
    {
        public override void CreateFunctions(BoxModuleI boxModule, out Object iceObject, out IFunctions functions)
        {
            Functions result = new Functions();
            iceObject = result;
            functions = result;
        }

        public override string[] GetBoxModuleFunctionsIceIds()
        {
            var f = new Functions();
            return f.ice_ids();
        }

        public override string GetDefaultUserLabel(BoxModuleI boxModule)
        {
            return ((Functions) boxModule.FunctionsIObj).Name;
        }

        public override ModulesAskingForCreation[] GetModulesAskingForCreation(string[] localePrefs,
                                                                               BoxModuleI boxModule)
        {
            Functions Func = (Functions) boxModule.FunctionsIObj;

            Dictionary<string, ModulesAskingForCreation> modulesAFC = getModulesAskingForCreationNonDynamic(localePrefs);
            List<ModulesAskingForCreation> result = new List<ModulesAskingForCreation>();
            ModulesAskingForCreation moduleAFC;
            ModulesConnection moduleConnection;
            ModuleAskingForCreation singleModuleAFC;
            var allColumnModulesAFC = new Dictionary<string, ModuleAskingForCreation>();

            // I presuppose that item with key "AllColumnsConjunctionHeuristic" is before item with key "AllColumns" and "Column"

            foreach (string moduleAFCName in modulesAFC.Keys)
            {
                moduleAFC = modulesAFC[moduleAFCName];
                switch (moduleAFCName)
                {
                    case "AllColumnsConjunctionHeuristic":
                        var allModulesAFC = new List<ModuleAskingForCreation>();
                        var explainInfos = Func.GetColumnExplainSeq(false);
                        if (explainInfos.Length > 0)
                        {
                            moduleConnection = new ModulesConnection();
                            moduleConnection.socketName = Column.Functions.SockDataTable;
                            moduleConnection.boxModuleParam = boxModule.MyProxy;

                            var conjunctionConnections = new List<ModulesCreatedConnection>();

                            foreach (var explainInfo in explainInfos)
                            {
                                singleModuleAFC = new ModuleAskingForCreation();
                                singleModuleAFC.modulesConnection = new ModulesConnection[] {moduleConnection};
                                singleModuleAFC.newBoxModuleIdentifier = Column.BoxInfo.typeIdentifier;
                                PropertySetting propertySetting = new PropertySetting();
                                propertySetting.propertyName = Column.Functions.PropSelectExpression;
                                propertySetting.value = new StringTI(explainInfo.name);
                                PropertySetting propertySetting2 = new PropertySetting();
                                propertySetting2.propertyName = Column.Functions.PropCardinality;
                                propertySetting2.value = new StringTI(Guha.Data.CardinalityEnum.Ordinal.ToString());
                                singleModuleAFC.propertySetting = new PropertySetting[] {propertySetting, propertySetting2};
                                singleModuleAFC.newBoxModuleId = new string[]{ explainInfo.name };
                                allColumnModulesAFC[explainInfo.name] = singleModuleAFC;
                                allModulesAFC.Add(singleModuleAFC);

                                var columnConnection = new ModulesCreatedConnection();
                                columnConnection.socketName = Categorization.Public.SockColumn;
                                columnConnection.boxModuleCreatedId = explainInfo.name;

                                var categorizationAFC = new ModuleAskingForCreation();
                                categorizationAFC.modulesCreatedConnection = new ModulesCreatedConnection[] { columnConnection };
                                categorizationAFC.newBoxModuleId = new string[] { $"{explainInfo.name}category" };
                                var propSetting = new PropertySetting();
                                propSetting.propertyName = Categorization.Public.SockNameInBooleanAttributes;
                                propSetting.value = new StringTI(explainInfo.name);
                                if (explainInfo.dataType == Guha.Data.DbDataTypeEnum.BooleanType || explainInfo.dataType == Guha.Data.DbDataTypeEnum.ShortIntegerType)
                                {
                                    categorizationAFC.newBoxModuleIdentifier = Categorization.EachValueOneCategory.BoxInfo.typeIdentifier;
                                    categorizationAFC.propertySetting = new PropertySetting[] { propSetting };
                                }
                                else
                                {
                                    categorizationAFC.newBoxModuleIdentifier = Categorization.EquifrequencyIntervals.BoxInfo.typeIdentifier;
                                    var propSetting2 = new PropertySetting();
                                    propSetting2.propertyName = Categorization.Public.SockCountOfCategories;
                                    propSetting2.value = new LongTI(8);
                                    categorizationAFC.propertySetting = new PropertySetting[] { propSetting, propSetting2 };

                                }
                                allModulesAFC.Add(categorizationAFC);

                                var categoryConnection = new ModulesCreatedConnection();
                                categoryConnection.socketName = GuhaMining.AtomSetting.Functions.SockBitStringGenerator;
                                categoryConnection.boxModuleCreatedId = $"{explainInfo.name}category";

                                var atomSettingAFC = new ModuleAskingForCreation();
                                atomSettingAFC.modulesCreatedConnection = new ModulesCreatedConnection[] { categoryConnection };
                                atomSettingAFC.newBoxModuleIdentifier = GuhaMining.AtomSetting.BoxInfo.typeIdentifier;
                                atomSettingAFC.newBoxModuleId = new string[] { $"{explainInfo.name}atom" };
                                var propSettingAt = new PropertySetting();
                                propSettingAt.propertyName = GuhaMining.AtomSetting.Functions.PropCoefficientType;
                                if (explainInfo.dataType == Guha.Data.DbDataTypeEnum.BooleanType)
                                {
                                    propSettingAt.value = new StringTI(Guha.MiningProcessor.CoefficientTypeEnum.Subsets.ToString());
                                    atomSettingAFC.propertySetting = new PropertySetting[] { propSettingAt };
                                }
                                else
                                {
                                    propSettingAt.value = new StringTI(Guha.MiningProcessor.CoefficientTypeEnum.Intervals.ToString());
                                    var propSettingAt2 = new PropertySetting();
                                    propSettingAt2.propertyName = GuhaMining.AtomSetting.Functions.PropMaximalLength;
                                    propSettingAt2.value = new IntTI(3);
                                    atomSettingAFC.propertySetting = new PropertySetting[] { propSettingAt, propSettingAt2 };
                                }
                                    
                                allModulesAFC.Add(atomSettingAFC);

                                var atomConnection = new ModulesCreatedConnection();
                                atomConnection.socketName = GuhaMining.ConjunctionSetting.Functions.SockBooleanAttributeSetting;
                                atomConnection.boxModuleCreatedId = $"{explainInfo.name}atom";
                                conjunctionConnections.Add(atomConnection);
                                //TODO: Conjunction

                            }
                            if (allModulesAFC.Count <= 1)
                                continue;

                            var conjunctionAFC = new ModuleAskingForCreation();
                            conjunctionAFC.modulesCreatedConnection = conjunctionConnections.ToArray();
                            conjunctionAFC.newBoxModuleIdentifier = GuhaMining.ConjunctionSetting.BoxInfo.typeIdentifier;
                            var propSettingCon = new PropertySetting();
                            propSettingCon.propertyName = GuhaMining.ConjunctionSetting.Functions.PropMaximalLength;
                            propSettingCon.value = new IntTI(3);
                            conjunctionAFC.propertySetting = new PropertySetting[] { propSettingCon };
                            allModulesAFC.Add(conjunctionAFC);

                            moduleAFC.newModules = allModulesAFC.ToArray();
                            result.Add(moduleAFC);
                        }
                        break;
                    case "Column":
                        foreach(var pairSingleModuleAFC in allColumnModulesAFC)
                        {
                            ModulesAskingForCreation newMAFC = new ModulesAskingForCreation();
                            newMAFC.label = moduleAFC.label.Replace("@Name", pairSingleModuleAFC.Key);
                            newMAFC.hint = moduleAFC.hint.Replace("@Name", pairSingleModuleAFC.Key);
                            newMAFC.help = moduleAFC.help;
                            newMAFC.newModules = new ModuleAskingForCreation[] { pairSingleModuleAFC.Value };
                            result.Add(newMAFC);
                        }
                        break;
                    case "DerivedColumn":
                        moduleConnection = new ModulesConnection();
                        singleModuleAFC = new ModuleAskingForCreation();
                        moduleConnection.socketName = Column.Functions.SockDataTable;
                        moduleConnection.boxModuleParam = boxModule.MyProxy;
                        singleModuleAFC.modulesConnection = new ModulesConnection[] {moduleConnection};
                        singleModuleAFC.newBoxModuleIdentifier = Column.BoxInfo.typeIdentifier;
                        moduleAFC.newModules = new ModuleAskingForCreation[] {singleModuleAFC};
                        result.Add(moduleAFC);
                        break;
                    case "AllColumns":
                        if (allColumnModulesAFC.Count <= 1)
                            continue;
                        moduleAFC.newModules = allColumnModulesAFC.Values.ToArray();
                        result.Add(moduleAFC);
                        break;
                    
                    default:
                        throw new NotImplementedException();
                }
            }
            return result.ToArray();
        }

        public override SelectString[] GetPropertyOptions(string propertyName, BoxModuleI boxModule)
        {
            Functions Func = (Functions) boxModule.FunctionsIObj;

            switch (propertyName)
            {
                case Functions.PropName:
                    return BoxInfoHelper.GetSelectStringArray(
                        Func.GetDataTablesNames(false)
                        );
                case Functions.PropPrimaryKeyColumns:
                    return BoxInfoHelper.GetSelectStringArray(
                        Func.GetColumnsNames(false)
                        );
                default:
                    return null;
            }
        }

        public const string typeIdentifier = "DataPreparation.DataSource.DataTable";

        protected override string identifier
        {
            get { return typeIdentifier; }
        }

        public override PropertyValue GetReadOnlyPropertyValue(string propertyName, BoxModuleI boxModule)
        {
            Functions Func = (Functions) boxModule.FunctionsIObj;
            switch (propertyName)
            {
                case Functions.PropType:
                    return Func.Type;
                case Functions.PropRemarks:
                    return Func.Remarks;
                case Functions.PropRecordsCount:
                    return Func.RecordsCount;
                default:
                    throw new NotImplementedException();
            }
        }

        public override void Validate(BoxModuleI boxModule)
        {
            Functions Func = (Functions) boxModule.FunctionsIObj;

            // try to invoke methods
            object dummy = Func.GetDatabaseFunctionsPrx(true);
            dummy = Func.GetGenericDataTable(true);
            dummy = Func.GetDataTableExplain(true);
            dummy = Func.GetColumnExplainSeq(true);
            dummy = Func.GetColumnsNames(true);
            dummy = Func.GetDataTablesNames(true);
            DataTableInfo dti = Func.GetDataTableInfo(true);
            long recordsCount = dti.recordsCount;
            if (recordsCount <= 0)
                throw Exceptions.BadValueError(null, boxModule.StringIceIdentity,
                                               "The table has no records. Please select non empty data table for analysis.",
                                               new string[] {Functions.PropName, Functions.PropRecordsCount},
                                               restrictionTypeEnum.Minimum);
            if (recordsCount >= Int32.MaxValue)
                throw Exceptions.BadValueError(null, boxModule.StringIceIdentity,
                                               "The table has more than " + Int32.MaxValue +
                                               " records. Data mining of data tables of such size is not supported.",
                                               new string[] {Functions.PropName, Functions.PropRecordsCount},
                                               restrictionTypeEnum.Maximum);
            Func.TryPrimaryKey(true);
        }
    }
}
