// BoxModule.cs - BoxInfo class for the database box
//
// Author: Tomáš Kuchaø <tomas.kuchar@gmail.com>
//
// Copyright (c) 2006 Tomáš Kuchaø
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.Collections.Generic;
using Ferda.Guha.Data;
using Object=Ice.Object;

namespace Ferda.Modules.Boxes.DataPreparation.Datasource.Database
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
            Functions Func = (Functions) boxModule.FunctionsIObj;

            string connectionString = Func.ConnectionString;
            if (!String.IsNullOrEmpty(connectionString))
            {
                {
                    string[] items = connectionString.Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries);
                    if (items != null)
                        foreach (string item in items)
                        {
                            if (!String.IsNullOrEmpty(item))
                            {
                                item.Trim();
                                if (item.StartsWith("DSN=", StringComparison.OrdinalIgnoreCase))
                                    return item.Substring(4);
                            }
                        }
                }
                {
                    string databaseName = Func.DatabaseName;
                    string result;
                    if (!String.IsNullOrEmpty(databaseName))
                    {
                        string[] items =
                            databaseName.Split(new char[] {'/', '\\'}, StringSplitOptions.RemoveEmptyEntries);
                        if (items != null && items.Length > 0)
                            for (int i = items.Length - 1; i >= 0; i--)
                            {
                                if (!String.IsNullOrEmpty(items[i]))
                                {
                                    result = items[i].Trim();
                                    if (result.Length <= 20)
                                        return result;
                                    else
                                        return result.Substring(0, 17) + "...";
                                }
                            }
                    }
                }
                if (connectionString.Length <= 20)
                    return connectionString;
                else
                    return connectionString.Substring(0, 17) + "...";
            }
            return null;
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
            List<ModuleAskingForCreation> allDataMatrixModulesAFC = new List<ModuleAskingForCreation>();

            // I presuppose that item with key "DataMatrix" is before item with key "AllDataMatrixes"

            foreach (string moduleAFCName in modulesAFC.Keys)
            {
                moduleAFC = modulesAFC[moduleAFCName];
                switch (moduleAFCName)
                {
                    case "DataMatrix":
                        string[] dataMatrixNames = Func.GetDataTablesNames(false);
                        if (dataMatrixNames.Length > 0)
                        {
                            moduleConnection = new ModulesConnection();
                            moduleConnection.socketName = DataTable.Functions.SockDatabase;
                            moduleConnection.boxModuleParam = boxModule.MyProxy;
                            foreach (string dataMatrixName in dataMatrixNames)
                            {
                                ModulesAskingForCreation newMAFC = new ModulesAskingForCreation();
                                newMAFC.label = moduleAFC.label.Replace("@Name", dataMatrixName);
                                newMAFC.hint = moduleAFC.hint.Replace("@Name", dataMatrixName);
                                newMAFC.help = moduleAFC.help;
                                singleModuleAFC = new ModuleAskingForCreation();
                                singleModuleAFC.modulesConnection = new ModulesConnection[] {moduleConnection};
                                singleModuleAFC.newBoxModuleIdentifier = DataTable.BoxInfo.typeIdentifier;
                                PropertySetting propertySetting = new PropertySetting();
                                propertySetting.propertyName = DataTable.Functions.PropName;
                                propertySetting.value = new StringTI(dataMatrixName);
                                singleModuleAFC.propertySetting = new PropertySetting[] {propertySetting};
                                allDataMatrixModulesAFC.Add(singleModuleAFC);
                                newMAFC.newModules = new ModuleAskingForCreation[] {singleModuleAFC};
                                result.Add(newMAFC);
                            }
                        }
                        break;
                    case "AllDataMatrixes":
                        if (allDataMatrixModulesAFC.Count <= 1)
                            continue;
                        moduleConnection = new ModulesConnection();
                        moduleConnection.socketName = DataTable.Functions.SockDatabase;
                        moduleConnection.boxModuleParam = boxModule.MyProxy;
                        moduleAFC.newModules = allDataMatrixModulesAFC.ToArray();
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
            switch (propertyName)
            {
                case Functions.PropProviderInvariantName:
                    return BoxInfoHelper.GetSelectStringArray(
                        DataProviderHelper.FactoryClassesInvariantNames
                        );
                default:
                    return null;
            }
        }

        public const string typeIdentifier = "DataPreparation.DataSource.Database";

        protected override string identifier
        {
            get { return typeIdentifier; }
        }

        public override PropertyValue GetReadOnlyPropertyValue(string propertyName, BoxModuleI boxModule)
        {
            Functions Func = (Functions) boxModule.FunctionsIObj;
            switch (propertyName)
            {
                case Functions.PropLastReloadRequest:
                    return Func.LastReloadRequest;
                case Functions.PropConnectionTimeout:
                    return Func.ConnectionTimeout;
                case Functions.PropDatabaseName:
                    return Func.DatabaseName;
                case Functions.PropDataSource:
                    return Func.DataSource;
                case Functions.PropDriver:
                    return Func.Driver;
                case Functions.PropServerVersion:
                    return Func.ServerVersion;
                default:
                    throw new NotImplementedException();
            }
        }

        public override void RunAction(string actionName, BoxModuleI boxModule)
        {
            Functions Func = (Functions) boxModule.FunctionsIObj;
            switch (actionName)
            {
                case "ReloadRequest":
                    Func.LastReloadRequest = DateTime.Now;
                    break;
                default:
                    throw Exceptions.NameNotExistError(null, actionName);
            }
        }

        public override void Validate(BoxModuleI boxModule)
        {
            Functions Func = (Functions) boxModule.FunctionsIObj;

            // try to invoke methods
            object dummy = Func.GetConnectionInfo(true);
            dummy = Func.GetGenericDatabase(true);
            dummy = Func.GetDataTableExplainSeq(true);
            dummy = Func.GetDataTablesNames(true);
        }

        public override bool IsPropertySet(string propertyName, PropertyValue propertyValue)
        {
            if (propertyName == "ConnectionString")
            {
                string value = ((StringT)propertyValue).stringValue;
                if (value == string.Empty || value == null)
                {
                    return false;
                }
            }

            return base.IsPropertySet(propertyName, propertyValue);
        }
    }
}