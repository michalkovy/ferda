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

        /// <summary>
        /// Gets default value for box module user label.
        /// </summary>
        public override string GetDefaultUserLabel(BoxModuleI boxModule)
        {
            return boxModule.GetPropertyString(DataMatrixNamePropertyName);
        }

        /// <summary>
        /// Gets <see cref="T:Ferda.Modules.PropertyValue"/> from
        /// <see cref="T:Ice.ObjectPrx">objectPrx</see> parameter.
        /// </summary>
        public override PropertyValue GetPropertyObjectFromInterface(String propertyName, Ice.ObjectPrx objectPrx)
        {
            /*
            if (propertyName == "PrimaryKeyColumns")
                return new Ferda.Modules.StringSeqTI(StringSeqTPrxHelper.checkedCast(objectPrx));
             */
            throw Ferda.Modules.Exceptions.NameNotExistError(null, null, null, propertyName);
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
                        try
                        {
                            columnsNames = Func.getColumnsNames();
                        }
                        catch (Ferda.Modules.BoxRuntimeError) { }
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

        /// <summary>
        /// Gets value of readonly property value.
        /// </summary>
        /// <param name="propertyName">Name of readonly property.</param>
        /// <param name="boxModule">Box module.</param>
        /// <returns>
        /// A <see cref="T:Ferda.Modules.PropertyValue"/> of
        /// readonly property named <c>propertyName</c>.
        /// </returns>
        public override PropertyValue GetReadOnlyPropertyValue(String propertyName, BoxModuleI boxModule)
        {
            switch (propertyName)
            {
                case RecordCountPropertyName:
                    DataMatrixFunctionsI Func = (DataMatrixFunctionsI)boxModule.FunctionsIObj;
                    long recordsCount = 0;
                    try
                    {
                        recordsCount = Func.RecordsCount;
                    }
                    catch (Ferda.Modules.BoxRuntimeError) { }
                    return new Ferda.Modules.LongTI(recordsCount);
                default:
                    throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(propertyName);
            }
        }

        /// <summary>
        /// Gets array of <see cref="T:Ferda.Modules.SelectString"/> as
        /// options for property, whose options are dynamically variable.
        /// </summary>
        public override SelectString[] GetPropertyOptions(String propertyName, BoxModuleI boxModule)
        {
            switch (propertyName)
            {
                case DataMatrixNamePropertyName:
                    string[] dataMatrixNames = null;
                    try
                    {
                        dataMatrixNames = ((DataMatrixFunctionsI)boxModule.FunctionsIObj).GetDatabaseFunctionsPrx().getDataMatrixNames();
                    }
                    catch (Ferda.Modules.BoxRuntimeError) {}
                    return BoxInfoHelper.StringArrayToSelectStringArray(dataMatrixNames);
                case PrimaryKeyColumnsPropertyName:
                    string[] columnNames = null;
                    try
                    {
                        columnNames = ((DataMatrixFunctionsI)boxModule.FunctionsIObj).getColumnsNames();
                    }
                    catch (Ferda.Modules.BoxRuntimeError) { }
                    return BoxInfoHelper.StringArrayToSelectStringArray(columnNames);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Executes (runs) action specified by <c>actionName</c>.
        /// </summary>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="boxModule">The Box module.</param>
        /// <exception cref="T:Ferda.Modules.NameNotExistError">Thrown if action named <c>actionName</c> doesn`t exist.</exception>
        /// <exception cref="T:Ferda.Modules.BoxRuntimeError">Thrown if any runtime error occured while executing the action.</exception>
        public override void RunAction(string actionName, BoxModuleI boxModule)
        {
            switch (actionName)
            {
                case "TestPrimaryKeyColumns":
                    this.TestPrimaryKeyColumnsAction(boxModule);
                    break;
                default:
                    throw Ferda.Modules.Exceptions.NameNotExistError(null, null, null, actionName);
            }
        }

        private void TestPrimaryKeyColumnsAction(BoxModuleI boxModule)
        {
            bool isPrimaryKey = false;
            DataMatrixFunctionsI functionsIObj = (DataMatrixFunctionsI)boxModule.FunctionsIObj;
            try
            {
                Ferda.Modules.Helpers.Data.DataMatrix.TestValuesInEnteredPrimaryKeyColumnsAreNotUniqueError(
                    functionsIObj.GetDatabaseFunctionsPrx().getDatabaseInfo().odbcConnectionString,
                    functionsIObj.DataMatrixName,
                    functionsIObj.PrimaryKeyColumns,
                    boxModule.StringIceIdentity
                    );
                isPrimaryKey = true;
            }
            catch (Ferda.Modules.BoxRuntimeError) { }

            if (isPrimaryKey)
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

        public override void Validate(BoxModuleI boxModule)
        {
            DataMatrixFunctionsI functionsIObj = (DataMatrixFunctionsI)boxModule.FunctionsIObj;
            Database.DatabaseInfo databaseInfo = functionsIObj.GetDatabaseFunctionsPrx().getDatabaseInfo();
            Ferda.Modules.Helpers.Data.DataMatrix.TestDataMatrixExists(
                databaseInfo.odbcConnectionString,
                functionsIObj.DataMatrixName,
                boxModule.StringIceIdentity);
            Ferda.Modules.Helpers.Data.DataMatrix.TestValuesInEnteredPrimaryKeyColumnsAreNotUniqueError(
                databaseInfo.odbcConnectionString,
                functionsIObj.DataMatrixName,
                functionsIObj.PrimaryKeyColumns,
                boxModule.StringIceIdentity);
        }

        #region Names of Properties
        public const string DataMatrixNamePropertyName = "Name";
        public const string PrimaryKeyColumnsPropertyName = "PrimaryKeyColumns";
        public const string RecordCountPropertyName = "RecordCount";
        #endregion
    }
}