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

        /// <summary>
        /// Gets default value for box module user label.
        /// </summary>
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
            List<ModuleAskingForCreation> allDataMatrixModulesAFC = new List<ModuleAskingForCreation>();
            // I presuppose that item with key "DataMatrix" is before item with key "AllDataMatrixes"
            foreach (string moduleAFCName in modulesAFC.Keys)
            {
                moduleAFC = modulesAFC[moduleAFCName];
                switch (moduleAFCName)
                {
                    case "DataMatrix":
                        DatabaseFunctionsI Func = (DatabaseFunctionsI)boxModule.FunctionsIObj;
                        string[] dataMatrixNames;
                        try
                        {
                            dataMatrixNames = Func.getDataMatrixNames();
                        }
                        catch (Ferda.Modules.BoxRuntimeError)
                        {
                            dataMatrixNames = new string[0];
                        }
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
            DatabaseFunctionsI Func = (DatabaseFunctionsI)boxModule.FunctionsIObj;
            switch (propertyName)
            {
                case "DatabaseName":
                    return new Ferda.Modules.StringTI(Func.getConnectionInfo().databaseName);
                case "DataSource":
                    return new Ferda.Modules.StringTI(Func.getConnectionInfo().dataSource);
                case "Driver":
                    return new Ferda.Modules.StringTI(Func.getConnectionInfo().driver);
                case "ServerVersion":
                    return new Ferda.Modules.StringTI(Func.getConnectionInfo().serverVersion);
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
        public const string OdbcConnectionStringPropertyName = "OdbcConnectionString";
        public const string LastReloadInfoPropertyName = "LastReloadInfo";
        public const string AcceptableTypesOfTablesPropertyName = "AcceptableTypesOfTables";
        #endregion
    }
}