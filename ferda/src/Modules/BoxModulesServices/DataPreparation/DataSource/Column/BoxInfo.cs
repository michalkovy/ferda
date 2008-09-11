// BoxInfo.cs - BoxInfo class for the column box
//
// Authors: Tomáš Kuchaø <tomas.kuchar@gmail.com>,
//          Martin Ralbovský <martin.ralbovsky@gmail.com> (modulesAFC)
//
// Copyright (c) 2006 Tomáš Kuchaø, Martin Ralbovský
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
using Ferda.Guha.Data;
using System.Collections.Generic;
using Object=Ice.Object;

namespace Ferda.Modules.Boxes.DataPreparation.Datasource.Column
{
    /// <summary>
    /// Class that provides info about boxes of the Column type
    /// </summary>
    /// <remarks>
    /// Older version of the code used "variability" instead of 
    /// "variance", which is the right term. Usage of these two
    /// terms is equal.
    /// </remarks>    /// 
    internal class BoxInfo : Boxes.BoxInfo
    {
        /// <summary>
        /// Functions creates an object of <see cref="T:Ferda.Modules.IFunctions">IFuntions</see>
        /// type that provides functionality of the box
        /// </summary>
        /// <param name="boxModule">Current box module</param>
        /// <param name="iceObject">ICE stuff</param>
        /// <param name="functions">The new created functions object</param>
        public override void CreateFunctions(BoxModuleI boxModule, out Object iceObject, out IFunctions functions)
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
            return ((Functions) boxModule.FunctionsIObj).SelectExpression;
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
            //getting the information what is in the config files
            Dictionary<string, ModulesAskingForCreation> modulesAFC = 
                getModulesAskingForCreationNonDynamic(localePrefs);
            //creating the structure that will be returned
            List<ModulesAskingForCreation> result = 
                new List<ModulesAskingForCreation>();
            
            ModulesConnection moduleConnection;
            ModuleAskingForCreation singleModule;
            PropertySetting propSetting;

            StringTI nameInBooleanAttributesValue = 
                new StringTI(boxModule.BoxInfo.GetDefaultUserLabel(boxModule));
            
            foreach (string moduleAFCname in modulesAFC.Keys)
            {
                singleModule = new ModuleAskingForCreation();
                moduleConnection = new ModulesConnection();
                propSetting = new PropertySetting();
                switch (moduleAFCname)
                {
                    case "EachValueOneCategoryAttribute":
                        //creating the info about the connections of the new module
                        moduleConnection.socketName = 
                            Categorization.Public.SockColumn;
                        moduleConnection.boxModuleParam = boxModule.MyProxy;

                        //the property setting - here setting the "nameInLiterals" 
                        //property of the attribute to the name of the column
                        propSetting.propertyName =
                            Categorization.EachValueOneCategory.Functions.PropNameInBooleanAttributes;
                        propSetting.value = nameInBooleanAttributesValue;

                        //creating the new (single) module
                        singleModule.modulesConnection = 
                            new ModulesConnection[] { moduleConnection };
                        singleModule.newBoxModuleIdentifier =
                            Categorization.EachValueOneCategory.BoxInfo.typeIdentifier;
                        singleModule.propertySetting =
                            new PropertySetting[] { propSetting };
                        break;

                    case "EquidistantIntervalsAttribute":
                        //creating the info about the connections of the new module
                        moduleConnection.socketName =
                            Categorization.Public.SockColumn;
                        moduleConnection.boxModuleParam = boxModule.MyProxy;

                        //the property setting - here setting the "nameInLiterals" 
                        //property of the attribute to the name of the column
                        propSetting.propertyName =
                            Categorization.EquidistantIntervals.Functions.PropNameInBooleanAttributes;
                        propSetting.value = nameInBooleanAttributesValue;

                        //creating the new (single) module
                        singleModule.modulesConnection = 
                            new ModulesConnection[] { moduleConnection };
                        singleModule.newBoxModuleIdentifier =
                            Categorization.EquidistantIntervals.BoxInfo.typeIdentifier;
                        singleModule.propertySetting =
                            new PropertySetting[] { propSetting };
                        break;

                    case "EquidistantIntervalsLISpAttribute":
                        //creating the info about the connections of the new module
                        moduleConnection.socketName =
                            Categorization.Public.SockColumn;
                        moduleConnection.boxModuleParam = boxModule.MyProxy;

                        //the property setting - here setting the "nameInLiterals" 
                        //property of the attribute to the name of the column
                        propSetting.propertyName =
                            Categorization.EquidistantIntervalsLISp.Functions.PropNameInBooleanAttributes;
                        propSetting.value = nameInBooleanAttributesValue;

                        //creating the new (single) module
                        singleModule.modulesConnection =
                            new ModulesConnection[] { moduleConnection };
                        singleModule.newBoxModuleIdentifier =
                            Categorization.EquidistantIntervalsLISp.BoxInfo.typeIdentifier;
                        singleModule.propertySetting =
                            new PropertySetting[] { propSetting };
                        break;

                    case "EquifrequencyIntervalsAttribute":
                        //creating the info about the connections of the new module
                        moduleConnection.socketName =
                            Categorization.Public.SockColumn;
                        moduleConnection.boxModuleParam = boxModule.MyProxy;

                        //the property setting - here setting the "nameInLiterals" 
                        //property of the attribute to the name of the column
                        propSetting.propertyName =
                            Categorization.EquifrequencyIntervals.Functions.PropNameInBooleanAttributes;
                        propSetting.value = nameInBooleanAttributesValue;

                        //creating the new (single) module
                        singleModule.modulesConnection = 
                            new ModulesConnection[] { moduleConnection };
                        singleModule.newBoxModuleIdentifier =
                            Categorization.EquifrequencyIntervals.BoxInfo.typeIdentifier;
                        singleModule.propertySetting =
                            new PropertySetting[] { propSetting };
                        break;

                    case "OntologyDerivedAttribute":
                        //creating the info about the connections of the new module
                        moduleConnection.socketName =
                            Categorization.Public.SockColumn;
                        moduleConnection.boxModuleParam = boxModule.MyProxy;

                        //the property setting - here setting the "nameInLiterals" 
                        //property of the attribute to the name of the column
                        propSetting.propertyName =
                            Categorization.OntologyDerivedAttribute.Functions.PropNameInLiterals;
                        propSetting.value = nameInBooleanAttributesValue;

                        //creating the new (single) module
                        singleModule.modulesConnection =
                            new ModulesConnection[] { moduleConnection };
                        singleModule.newBoxModuleIdentifier =
                            Categorization.OntologyDerivedAttribute.BoxInfo.typeIdentifier;
                        singleModule.propertySetting =
                            new PropertySetting[] { propSetting };
                        break;
                    
                    case "StaticAttribute":
                        //creating the info about the connections of the new module
                        moduleConnection.socketName =
                            Categorization.Public.SockColumn;
                        moduleConnection.boxModuleParam = boxModule.MyProxy;

                        //the property setting - here setting the "nameInLiterals" 
                        //property of the attribute to the name of the column
                        propSetting.propertyName =
                            Categorization.StaticAttribute.Functions.PropNameInBooleanAttributes;
                        propSetting.value = nameInBooleanAttributesValue;

                        //creating the new (single) module
                        singleModule.modulesConnection =
                            new ModulesConnection[] { moduleConnection };
                        singleModule.newBoxModuleIdentifier =
                            Categorization.StaticAttribute.BoxInfo.typeIdentifier;
                        singleModule.propertySetting =
                            new PropertySetting[] { propSetting };
                        break;

                    default:
                        throw new NotImplementedException();
                }

                //setting the newModules property of each modules for intearction
                modulesAFC[moduleAFCname].newModules =
                    new ModuleAskingForCreation[] { singleModule };
                result.Add(modulesAFC[moduleAFCname]);
            }

            return result.ToArray();
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
            Functions Func = (Functions) boxModule.FunctionsIObj;

            switch (propertyName)
            {
                case Functions.PropSelectExpression:
                    return BoxInfoHelper.GetSelectStringArray(
                        Func.GetColumnsNames(false)
                        );
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
            Functions Func = (Functions) boxModule.FunctionsIObj;
            switch (propertyName)
            {
                case Functions.PropDataType:
                    return Func.DataType;
                case Functions.PropValueMin:
                    return Func.ValueMin;
                case Functions.PropValueMax:
                    return Func.ValueMax;
                case Functions.PropValueAverage:
                    return Func.ValueAverage;
                case Functions.PropValueVariance:
                    return Func.ValueVariance;
                case Functions.PropValueStandardDeviation:
                    return Func.ValueStandardDeviation;
                case Functions.PropValueDistincts:
                    return Func.ValueDistincts;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Validates the box
        /// </summary>
        /// <param name="boxModule">box instance to be validated</param>
        public override void Validate(BoxModuleI boxModule)
        {
            Functions Func = (Functions) boxModule.FunctionsIObj;

            // try to invoke methods
            object dummy = Func.GetDataTableFunctionsPrx(true);
            GenericColumn tmp = Func.GetGenericColumn(true);
            dummy = Func.GetColumnsNames(true);
            dummy = Func.GetColumnStatistics(true);
            dummy = Func.GetColumnExplain(true);
            //dummy = Func.GetDistinctsAndFrequencies(true);
            dummy = Func.GetColumnInfo(true);

            if (Common.CompareCardinalityEnums(
                    Func.Cardinality,
                    tmp.PotentiallyCardinality
                    ) > 1)
            {
                throw Exceptions.BadValueError(
                    null,
                    boxModule.StringIceIdentity,
                    "Unsupported cardinality type for current data type.",
                    new string[] {Functions.PropCardinality},
                    restrictionTypeEnum.OtherReason
                    );
            }
        }

        #region Type Identifier

        /// <summary>
        /// This is recomended (not required) to have <c>public const string</c> 
        /// field in the BoxInfo implementation which holds the identifier 
        /// of type of the box module.
        /// </summary>
        public const string typeIdentifier = "DataPreparation.DataSource.Column";

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