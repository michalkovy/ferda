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
            return Functions.ids__;
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
            List<ModuleAskingForCreation> allColumnModulesAFC = new List<ModuleAskingForCreation>();

            // I presuppose that item with key "Column" is before item with key "AllColumns"

            foreach (string moduleAFCName in modulesAFC.Keys)
            {
                moduleAFC = modulesAFC[moduleAFCName];
                switch (moduleAFCName)
                {
                    case "Column":
                        string[] columnsNames = Func.GetColumnsNames(false);
                        if (columnsNames.Length > 0)
                        {
                            moduleConnection = new ModulesConnection();
                            moduleConnection.socketName = Column.Functions.SockDataTable;
                            moduleConnection.boxModuleParam = boxModule.MyProxy;
                            foreach (string columnName in columnsNames)
                            {
                                ModulesAskingForCreation newMAFC = new ModulesAskingForCreation();
                                newMAFC.label = moduleAFC.label.Replace("@Name", columnName);
                                newMAFC.hint = moduleAFC.hint.Replace("@Name", columnName);
                                newMAFC.help = moduleAFC.help;
                                singleModuleAFC = new ModuleAskingForCreation();
                                singleModuleAFC.modulesConnection = new ModulesConnection[] {moduleConnection};
                                ;
                                singleModuleAFC.newBoxModuleIdentifier = Column.BoxInfo.typeIdentifier;
                                PropertySetting propertySetting = new PropertySetting();
                                propertySetting.propertyName = Column.Functions.PropSelectExpression;
                                propertySetting.value = new StringTI(columnName);
                                singleModuleAFC.propertySetting = new PropertySetting[] {propertySetting};
                                allColumnModulesAFC.Add(singleModuleAFC);
                                newMAFC.newModules = new ModuleAskingForCreation[] {singleModuleAFC};
                                result.Add(newMAFC);
                            }
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
                        moduleConnection = new ModulesConnection();
                        moduleConnection.socketName = Column.Functions.SockDataTable;
                        moduleConnection.boxModuleParam = boxModule.MyProxy;
                        moduleAFC.newModules = allColumnModulesAFC.ToArray();
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
            dummy = Func.GetDataTableInfo(true);
            dummy = Func.GetDataTablesNames(true);
            Func.TryPrimaryKey(true);
        }
    }
}