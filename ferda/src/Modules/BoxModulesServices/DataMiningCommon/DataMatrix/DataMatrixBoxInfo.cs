using System;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.DataMiningCommon.DataMatrix
{
    class DataMatrixBoxInfo : Ferda.Modules.Boxes.BoxInfo
    {
        public const string typeIdentifier =
            "DataMiningCommon.DataMatrix";

        protected override string identifier
        {
            get { return typeIdentifier; }
        }

        public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
        {
            DataMatrixFunctionsI result = new DataMatrixFunctionsI();
            iceObject = (Ice.Object)result;
            functions = (IFunctions)result;
        }

        public override string[] GetBoxModuleFunctionsIceIds()
        {
            return DataMatrixFunctionsI.ids__;
        }

        public override string GetDefaultUserLabel(BoxModuleI boxModule)
        {
            return boxModule.GetPropertyString("Name");
        }

        public override PropertyValue GetPropertyObjectFromInterface(String propertyName, Ice.ObjectPrx objectPrx)
        {
            /*
            if (propertyName == "PrimaryKeyColumns")
                return new Ferda.Modules.StringSeqTI(StringSeqTPrxHelper.checkedCast(objectPrx));
             */
            throw Ferda.Modules.Exceptions.NameNotExistError(null, null, null, propertyName);
        }

        public override ModulesAskingForCreation[] GetModulesAskingForCreation(string[] localePrefs, BoxModuleI boxModule)
        {
            Dictionary<string, ModulesAskingForCreation> modulesAFC = this.getModulesAskingForCreationNonDynamic(localePrefs);
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
                        DataMatrixFunctionsI Func = (DataMatrixFunctionsI)boxModule.FunctionsIObj;
                        string[] columnsNames = null;
                        columnsNames = Func.GetColumns();
                        if (columnsNames != null && columnsNames.Length > 0)
                        {
                            moduleConnection = new ModulesConnection();
                            moduleConnection.socketName = "DataMatrixOrMultiColumn";
                            moduleConnection.boxModuleParam = boxModule.MyProxy;
                            foreach (string columnName in columnsNames)
                            {
                                ModulesAskingForCreation newMAFC = new ModulesAskingForCreation();
                                newMAFC.label = moduleAFC.label.Replace("@Name", columnName);
                                newMAFC.hint = moduleAFC.hint.Replace("@Name", columnName);
                                newMAFC.help = moduleAFC.help;
                                singleModuleAFC = new ModuleAskingForCreation();
                                singleModuleAFC.modulesConnection = new ModulesConnection[] { moduleConnection }; ;
                                singleModuleAFC.newBoxModuleIdentifier =
                                    Ferda.Modules.Boxes.DataMiningCommon.Column.ColumnBoxInfo.typeIdentifier;
                                PropertySetting propertySetting = new PropertySetting();
                                propertySetting.propertyName = "Name";
                                propertySetting.value = new Ferda.Modules.StringTI(columnName);
                                singleModuleAFC.propertySetting = new PropertySetting[] { propertySetting };
                                allColumnModulesAFC.Add(singleModuleAFC);
                                newMAFC.newModules = new ModuleAskingForCreation[] { singleModuleAFC };
                                result.Add(newMAFC);
                            }
                        }
                        break;
                    case "DerivedColumn":
                        moduleConnection = new ModulesConnection();
                        singleModuleAFC = new ModuleAskingForCreation();
                        moduleConnection.socketName = "DataMatrix";
                        moduleConnection.boxModuleParam = boxModule.MyProxy;
                        singleModuleAFC.modulesConnection = new ModulesConnection[] { moduleConnection };
                        singleModuleAFC.newBoxModuleIdentifier =
                            Ferda.Modules.Boxes.DataMiningCommon.DerivedColumn.DerivedColumnBoxInfo.typeIdentifier;
                        moduleAFC.newModules = new ModuleAskingForCreation[] { singleModuleAFC };
                        result.Add(moduleAFC);
                        break;
                    case "AllColumns":
                        if (allColumnModulesAFC.Count <= 1)
                            continue;
                        moduleConnection = new ModulesConnection();
                        moduleConnection.socketName = "DataMatrixOrMultiColumn";
                        moduleConnection.boxModuleParam = boxModule.MyProxy;
                        moduleAFC.newModules = allColumnModulesAFC.ToArray();
                        result.Add(moduleAFC);
                        break;
                    default:
                        throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(moduleAFCName);
                }
            }
            return result.ToArray();
        }

        public override PropertyValue GetReadOnlyPropertyValue(String propertyName, BoxModuleI boxModule)
        {
            switch (propertyName)
            {
                case "RecordCount":
                    DataMatrixFunctionsI Func = (DataMatrixFunctionsI)boxModule.FunctionsIObj;
                    return new Ferda.Modules.LongTI(Func.GetRecordsCount());
                default:
                    throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(propertyName);
            }
        }

        public override SelectString[] GetPropertyOptions(String propertyName, BoxModuleI boxModule)
        {
            switch (propertyName)
            {
                case "Name":
                    return BoxInfoHelper.StringArrayToSelectStringArray(
                        ((DataMatrixFunctionsI)boxModule.FunctionsIObj).GetTablesNames()
                        );
                case "PrimaryKeyColumns":
                    return BoxInfoHelper.StringArrayToSelectStringArray(
                        ((DataMatrixFunctionsI)boxModule.FunctionsIObj).GetColumns()
                        );
                default:
                    return null;
            }
        }

        public override void RunAction(string actionName, BoxModuleI boxModule)
        {
            switch (actionName)
            {
                case "TestPrimaryKeyColumns":
                    this.RunActionTestPrimaryKeyColumns(boxModule);
                    break;
                default:
                    throw Ferda.Modules.Exceptions.NameNotExistError(null, null, null, actionName);
            }
        }

        private void RunActionTestPrimaryKeyColumns(BoxModuleI boxModule)
        {
            DataMatrixFunctionsI Func = (DataMatrixFunctionsI)boxModule.FunctionsIObj;
            if (Func.RunActionTestPrimaryKeyColumns())
                // test succeed
                boxModule.OutputMessage(
                    Ferda.ModulesManager.MsgType.Info,
                    "TestPrimaryKey",
                    "ActionTestTestPrimaryKeySucceed");
            else
                // test failed
                boxModule.OutputMessage(
                    Ferda.ModulesManager.MsgType.Warning,
                    "TestPrimaryKey",
                    "ActionTestTestPrimaryKeyFailed");
        }
    }
}