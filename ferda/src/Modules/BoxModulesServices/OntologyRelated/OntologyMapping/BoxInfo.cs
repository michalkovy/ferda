// BoxInfo.cs - BoxInfo class for the Ontology Mapping box
//
// Author: Martin Zeman <martin.zeman@email.cz>
//
// Copyright (c) 2007 Martin Zeman
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
using System.Text;
using Ferda.Modules.Boxes;
using Ferda.Guha.Data;
using Object = Ice.Object;
using Ferda.Modules.Boxes.DataPreparation;

namespace Ferda.Modules.Boxes.OntologyRelated.OntologyMapping
{
    /// <summary>
    /// Class that provides info about boxes of the OntologyMapping type
    /// </summary>
    internal class BoxInfo : Boxes.BoxInfo
    {
        /// <summary>
        /// Functions creates an object of <see cref="T:Ferda.Modules.IFunctions">IFuntions</see>
        /// type that provides functionality of the box
        /// </summary>
        /// <param name="boxModule">Current box module</param>
        /// <param name="iceObject">ICE stuff</param>
        /// <param name="functions">The new created functions object</param>
        public override void CreateFunctions(BoxModuleI boxModule,
            out Object iceObject, out IFunctions functions)
        {
            Functions result = new Functions();
            iceObject = result;
            functions = result;
        }

        /// <summary>
        /// Gets function`s Ice identifiers of the box module.
        /// </summary>
        /// <returns>
        /// An array of strings representing Ice identifiers
        /// of the box module`s functions.
        /// </returns>
        /// <example>
        /// Please see an example for <see cref="T:Ferda.Modules.Boxes.IBoxInfo">IBoxInfo`s</see>
        /// 	<see cref="M:Ferda.Modules.Boxes.IBoxInfo.GetBoxModuleFunctionsIceIds()"/>.
        /// </example>
        public override string[] GetBoxModuleFunctionsIceIds()
        {
            return Functions.ids__;
        }

        /// <summary>
        /// Gets default value for box module user label.
        /// </summary>
        /// <param name="boxModule">A module that returns the label</param>
        /// <returns>The user label</returns>
        public override string GetDefaultUserLabel(BoxModuleI boxModule)
        {
            return "Mapping";
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
        public override ModulesAskingForCreation[] GetModulesAskingForCreation(string[] localePrefs,
                                                                               BoxModuleI boxModule)
        {
            Functions Func = (Functions)boxModule.FunctionsIObj;

            Dictionary<string, ModulesAskingForCreation> modulesAFC = getModulesAskingForCreationNonDynamic(localePrefs);
            List<ModulesAskingForCreation> result = new List<ModulesAskingForCreation>();
            ModulesAskingForCreation moduleAFC;
            ModulesConnection moduleConnection;
            ModuleAskingForCreation singleModuleAFC;
            List<ModuleAskingForCreation> allColumnModulesAFC = new List<ModuleAskingForCreation>();

            // I presuppose that item with key "Column" is before item with key "AllColumns"

            foreach (string moduleAFCName in modulesAFC.Keys)
            {
                /*moduleAFC = modulesAFC[moduleAFCName];
                switch (moduleAFCName)
                {
                    case "Column":
                        //TODO vybrat v�echny mo�nosti DataTable/Columns
                        string[] columnsNames = Func.GetColumnsNames(false);
                        if (columnsNames.Length > 0)
                        {
                            moduleConnection = new ModulesConnection();
                            //TODO n�jak to o�et�it - asi nejl�p tim, �e p�esunu OM do datapreparation
                            //moduleConnection.socketName = Column.Functions.SockDataTable;
                            moduleConnection.socketName = "OntologyMapping";
                            moduleConnection.boxModuleParam = boxModule.MyProxy;
                            foreach (string columnName in columnsNames)
                            {
                                ModulesAskingForCreation newMAFC = new ModulesAskingForCreation();
                                newMAFC.label = moduleAFC.label.Replace("@Name", columnName);
                                newMAFC.hint = moduleAFC.hint.Replace("@Name", columnName);
                                newMAFC.help = moduleAFC.help;
                                singleModuleAFC = new ModuleAskingForCreation();
                                singleModuleAFC.modulesConnection = new ModulesConnection[] { moduleConnection };
                                ;
                                singleModuleAFC.newBoxModuleIdentifier = Column.BoxInfo.typeIdentifier;
                                PropertySetting propertySetting = new PropertySetting();
                                propertySetting.propertyName = Column.Functions.PropSelectExpression;
                                propertySetting.value = new StringTI(columnName);
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
                        //TODO - o�et�it viz v��e
                        //moduleConnection.socketName = Column.Functions.SockDataTable;
                        moduleConnection.socketName = "OntologyMapping";
                        moduleConnection.boxModuleParam = boxModule.MyProxy;
                        singleModuleAFC.modulesConnection = new ModulesConnection[] { moduleConnection };
                        singleModuleAFC.newBoxModuleIdentifier = Column.BoxInfo.typeIdentifier;
                        moduleAFC.newModules = new ModuleAskingForCreation[] { singleModuleAFC };
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
                }*/
            }
            //return result.ToArray();
            return null;
        }
        /// <summary>
        /// Gets array of <see cref="T:Ferda.Modules.SelectString"/> as
        /// options for property, whose options are dynamically variable.
        /// </summary>
        /// <param name="boxModule">The current module</param>
        /// <param name="propertyName">Name of the property</param>
        /// <returns>String options of the property</returns>
        public override SelectString[] GetPropertyOptions(string propertyName, BoxModuleI boxModule)
        {
            Functions Func = (Functions)boxModule.FunctionsIObj;

            switch (propertyName)
            {
                default:
                    return null;
            }
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
        public override PropertyValue GetReadOnlyPropertyValue(string propertyName, BoxModuleI boxModule)
        {
            Functions Func = (Functions)boxModule.FunctionsIObj;
            switch (propertyName)
            {
                case Functions.PropNumberOfMappedPairs:
                    return Func.NumberOfMappedPairs;
                
                default:
                    throw new NotImplementedException();
            }
            return null; 
        }

        /// <summary>
        /// Executes (runs) action specified by <c>actionName</c>.
        /// </summary>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="boxModule">The Box module.</param>
        /// <exception cref="T:Ferda.Modules.NameNotExistError">
        /// Thrown if action named <c>actionName</c> doesn`t exist.
        /// </exception>
        /// <exception cref="T:Ferda.Modules.BoxRuntimeError">
        /// Thrown if any runtime error occured while executing the action.
        /// </exception>
        public override void RunAction(string actionName, BoxModuleI boxModule)
        {
            /* TODO doplnit/smazat
             * Functions Func = (Functions)boxModule.FunctionsIObj;
            switch (actionName)
            {
                case "ReloadRequest":
                    Func.LastReloadRequest = DateTime.Now;
                    break;
                default:
                    throw Exceptions.NameNotExistError(null, actionName);
            }
             */
        }

        /// <summary>
        /// Determines if user has already set a value of a property
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="propertyValue">Value of the property</param>
        /// <returns>If the property is set.</returns>
        public override bool IsPropertySet(string propertyName, PropertyValue propertyValue)
        {
            if (propertyName == "Mapping")
            {
                string value = ((StringT)propertyValue).stringValue;
                if (value == string.Empty || value == null)
                {
                    return false;
                }
            }

            return base.IsPropertySet(propertyName, propertyValue);
        }

        #region Type Identifier

        /// <summary>
        /// This is recomended (not required) to have <c>public const string</c> 
        /// field in the BoxInfo implementation which holds the identifier 
        /// of type of the box module.
        /// </summary>
        public const string typeIdentifier = "OntologyRelated.OntologyMapping";

        /// <summary>
        /// Unique identifier of type of Box module
        /// </summary>
        /// <value></value>
        /// <remarks>
        /// This string identifier is parsed i.e. dots (".") are
        /// replaced by <see cref="P:System.IO.Path.DirectorySeparatorChar"/>.
        /// Returned path is combined with application directory`s
        /// <see cref="F:Ferda.Modules.Boxes.BoxInfo.configFilesFolderName">subdirectory</see>
        /// and final path is used for getting stored configuration [localization] XML files.
        /// </remarks>
        protected override string identifier
        {
            get { return typeIdentifier; }
        }

        #endregion
    }
}
