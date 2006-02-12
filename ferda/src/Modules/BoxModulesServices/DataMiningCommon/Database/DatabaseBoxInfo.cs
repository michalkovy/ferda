using System;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.DataMiningCommon.Database
{
    class DatabaseBoxInfo : Ferda.Modules.Boxes.BoxInfo
    {
        public const string typeIdentifier =
            "DataMiningCommon.Database";

        protected override string identifier
        {
            get { return typeIdentifier; }
        }

        public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
        {
            DatabaseFunctionsI result = new DatabaseFunctionsI();
            iceObject = (Ice.Object)result;
            functions = (IFunctions)result;
        }

        public override string[] GetBoxModuleFunctionsIceIds()
        {
            return DatabaseFunctionsI.ids__;
        }

        public override string GetDefaultUserLabel(BoxModuleI boxModule)
        {
            string odbcConnectionString = boxModule.GetPropertyString(OdbcConnectionStringPropertyName);
            if (!String.IsNullOrEmpty(odbcConnectionString))
            {
                string[] itemsOfConnectionString = odbcConnectionString.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (itemsOfConnectionString != null)
                    foreach (string item in itemsOfConnectionString)
                    {
                        if (!String.IsNullOrEmpty(item))
                        {
                            item.Trim();
                            if (item.StartsWith("DSN=", StringComparison.OrdinalIgnoreCase))
                                return item.Substring(4);
                        }
                    }
                if (odbcConnectionString.Length <= 20)
                    return odbcConnectionString;
                else
                    return odbcConnectionString.Substring(0, 17) + "...";
            }
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
            List<ModuleAskingForCreation> allDataMatrixModulesAFC = new List<ModuleAskingForCreation>();
            // I presuppose that item with key "DataMatrix" is before item with key "AllDataMatrixes"
            foreach (string moduleAFCName in modulesAFC.Keys)
            {
                moduleAFC = modulesAFC[moduleAFCName];
                switch (moduleAFCName)
                {
                    case "DataMatrix":
                        DatabaseFunctionsI Func = (DatabaseFunctionsI)boxModule.FunctionsIObj;
                        string[] dataMatrixNames = Func.GetTables();
                        if (dataMatrixNames != null && dataMatrixNames.Length > 0)
                        {
                            moduleConnection = new ModulesConnection();
                            moduleConnection.socketName = "Database";
                            moduleConnection.boxModuleParam = boxModule.MyProxy;
                            foreach (string dataMatrixName in dataMatrixNames)
                            {
                                ModulesAskingForCreation newMAFC = new ModulesAskingForCreation();
                                newMAFC.label = moduleAFC.label.Replace("@Name", dataMatrixName);
                                newMAFC.hint = moduleAFC.hint.Replace("@Name", dataMatrixName);
                                newMAFC.help = moduleAFC.help;
                                singleModuleAFC = new ModuleAskingForCreation();
                                singleModuleAFC.modulesConnection = new ModulesConnection[] { moduleConnection };
                                singleModuleAFC.newBoxModuleIdentifier = Ferda.Modules.Boxes.DataMiningCommon.DataMatrix.DataMatrixBoxInfo.typeIdentifier;
                                PropertySetting propertySetting = new PropertySetting();
                                propertySetting.propertyName = "Name";
                                propertySetting.value = new Ferda.Modules.StringTI(dataMatrixName);
                                singleModuleAFC.propertySetting = new PropertySetting[] { propertySetting };
                                allDataMatrixModulesAFC.Add(singleModuleAFC);
                                newMAFC.newModules = new ModuleAskingForCreation[] { singleModuleAFC };
                                result.Add(newMAFC);
                            }
                        }
                        break;
                    case "AllDataMatrixes":
                        if (allDataMatrixModulesAFC.Count <= 1)
                            continue;
                        moduleConnection = new ModulesConnection();
                        moduleConnection.socketName = "Database";
                        moduleConnection.boxModuleParam = boxModule.MyProxy;
                        moduleAFC.newModules = allDataMatrixModulesAFC.ToArray();
                        result.Add(moduleAFC);
                        break;
                    default:
                        throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(moduleAFCName);
                }
            }
            return result.ToArray();
        }

        public override void RunAction(string actionName, BoxModuleI boxModule)
        {
            switch (actionName)
            {
                case "TestConnectionString":
                    this.TestConnectionStringAction(boxModule);
                    break;
                case "ReloadInfo":
                    this.ReloadInfoAction(boxModule);
                    break;
                default:
                    throw Ferda.Modules.Exceptions.NameNotExistError(null, null, null, actionName);
            }
        }

        private void TestConnectionStringAction(BoxModuleI boxModule)
        {
            bool isConnectionStringValid = false;
            try
            {
                Ferda.Modules.Helpers.Data.Database.TestConnectionString(
                    boxModule.GetPropertyString(OdbcConnectionStringPropertyName),
                    boxModule.StringIceIdentity);
                isConnectionStringValid = true;
            }
            catch (BadParamsError ex)
            {
                if (ex.restrictionType != restrictionTypeEnum.DbConnectionString)
                    throw ex;
            }
           
            if (isConnectionStringValid)
                // test succeed
                boxModule.OutputMessage(
                    Ferda.ModulesManager.MsgType.Info,
                    "ActionTestConnectionString",
                    "ActionTestConnectionStringSucceed");
            else
                // test failed
                boxModule.OutputMessage(
                    Ferda.ModulesManager.MsgType.Warning,
                    "ActionTestConnectionString",
                    "ActionTestConnectionStringFailed");
        }

        private void ReloadInfoAction(BoxModuleI boxModule)
        {
            DatabaseFunctionsI functionsIObj = (DatabaseFunctionsI)boxModule.FunctionsIObj;
            functionsIObj.LastReloadInfo = new DateTimeTI(DateTime.Now);
        }

        public override PropertyValue GetReadOnlyPropertyValue(String propertyName, BoxModuleI boxModule)
        {
            DatabaseFunctionsI Func = (DatabaseFunctionsI)boxModule.FunctionsIObj;
            switch (propertyName)
            {
                case "DatabaseName":
                    return new Ferda.Modules.StringTI(Func.GetConnectionInfo().databaseName);
                case "DataSource":
                    return new Ferda.Modules.StringTI(Func.GetConnectionInfo().dataSource);
                case "Driver":
                    return new Ferda.Modules.StringTI(Func.GetConnectionInfo().driver);
                case "ServerVersion":
                    return new Ferda.Modules.StringTI(Func.GetConnectionInfo().serverVersion);
                default:
                    throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(propertyName);
            }
        }

        public override void Validate(BoxModuleI boxModule)
        {
            Ferda.Modules.Helpers.Data.Database.TestConnectionString(
                boxModule.GetPropertyString(OdbcConnectionStringPropertyName),
                boxModule.StringIceIdentity);
        }

        #region Names of Properties
        public const string OdbcConnectionStringPropertyName = "ConnectionString";
        public const string LastReloadInfoPropertyName = "LastReloadInfo";
        public const string AcceptableTypesOfTablesPropertyName = "AcceptableTypesOfTables";
        #endregion
    }
}