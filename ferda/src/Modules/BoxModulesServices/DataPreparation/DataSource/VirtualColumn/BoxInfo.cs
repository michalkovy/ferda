// BoxInfo.cs - box functions for Virtual column box
//
// Author: Alexander Kuzmin <alexander.kuzmin@gmail.com>
//
// Copyright (c) 2007 Alexander Kuzmin
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
using Object=Ice.Object;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.DataPreparation.Datasource.VirtualColumn
{
    internal class BoxInfo : Boxes.BoxInfo
    {
        /// <summary>
        /// Creates Functions object
        /// </summary>
        /// <param name="boxModule">The boxmodule</param>
        /// <param name="iceObject">The ice object</param>
        /// <param name="functions">Functions</param>
        public override void CreateFunctions(BoxModuleI boxModule, out Object iceObject, out IFunctions functions)
        {
            Functions result = new Functions();
            iceObject = result;
            functions = result;
        }

        /// <summary>
        /// Gets functions ids
        /// </summary>
        /// <returns></returns>
        public override string[] GetBoxModuleFunctionsIceIds()
        {
            return Functions.ids__;
        }

        /// <summary>
        /// Displaying default user label for the box, if it is not customized
        /// </summary>
        /// <param name="boxModule">The boxmodule</param>
        /// <returns>Box label</returns>
        public override string GetDefaultUserLabel(BoxModuleI boxModule)
        {
            return ((Functions)boxModule.FunctionsIObj).DetailTableResultColumn;
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
            //getting the information what is in the config files
            Dictionary<string, ModulesAskingForCreation> modulesAFC =
                getModulesAskingForCreationNonDynamic(localePrefs);
            //creating the structure that will be returned
            List<ModulesAskingForCreation> result =
                new List<ModulesAskingForCreation>();

            ModulesConnection moduleConnection;
            ModuleAskingForCreation singleModule;
            PropertySetting propSetting;

            StringTI nameInLiteralsValue =
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
                            Categorization.EachValueOneCategory.Functions.SockColumn;
                        moduleConnection.boxModuleParam = boxModule.MyProxy;

                        //the property setting - here setting the "nameInLiterals" 
                        //property of the attribute to the name of the column
                        propSetting.propertyName =
                            Categorization.EachValueOneCategory.Functions.PropNameInLiterals;
                        propSetting.value = nameInLiteralsValue;

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
                            Categorization.EquidistantIntervals.Functions.SockColumn;
                        moduleConnection.boxModuleParam = boxModule.MyProxy;

                        //the property setting - here setting the "nameInLiterals" 
                        //property of the attribute to the name of the column
                        propSetting.propertyName =
                            Categorization.EquidistantIntervals.Functions.PropNameInLiterals;
                        propSetting.value = nameInLiteralsValue;

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
                            Categorization.EquidistantIntervalsLISp.Functions.SockColumn;
                        moduleConnection.boxModuleParam = boxModule.MyProxy;

                        //the property setting - here setting the "nameInLiterals" 
                        //property of the attribute to the name of the column
                        propSetting.propertyName =
                            Categorization.EquidistantIntervalsLISp.Functions.PropNameInLiterals;
                        propSetting.value = nameInLiteralsValue;

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
                            Categorization.EquifrequencyIntervals.Functions.SockColumn;
                        moduleConnection.boxModuleParam = boxModule.MyProxy;

                        //the property setting - here setting the "nameInLiterals" 
                        //property of the attribute to the name of the column
                        propSetting.propertyName =
                            Categorization.EquifrequencyIntervals.Functions.PropNameInLiterals;
                        propSetting.value = nameInLiteralsValue;

                        //creating the new (single) module
                        singleModule.modulesConnection =
                            new ModulesConnection[] { moduleConnection };
                        singleModule.newBoxModuleIdentifier =
                            Categorization.EquifrequencyIntervals.BoxInfo.typeIdentifier;
                        singleModule.propertySetting =
                            new PropertySetting[] { propSetting };
                        break;

                    case "StaticAttribute":
                        //creating the info about the connections of the new module
                        moduleConnection.socketName =
                            Categorization.StaticAttribute.Functions.SockColumn;
                        moduleConnection.boxModuleParam = boxModule.MyProxy;

                        //the property setting - here setting the "nameInLiterals" 
                        //property of the attribute to the name of the column
                        propSetting.propertyName =
                            Categorization.StaticAttribute.Functions.PropNameInLiterals;
                        propSetting.value = nameInLiteralsValue;

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
            return result.ToArray();;
        }

        /// <summary>
        /// Gets property options - here it is master datatable, detail datatable and the result column
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="boxModule"></param>
        /// <returns></returns>
        public override SelectString[] GetPropertyOptions(string propertyName, BoxModuleI boxModule)
        {
            Functions Func = (Functions)boxModule.FunctionsIObj;

            switch (propertyName)
            {
                case Functions.PropMasterIdColumn:
                    return BoxInfoHelper.GetSelectStringArray(
                        Func.GetMasterColumnsNames(false)
                        );

                case Functions.PropDetailIdColumn:
                    return BoxInfoHelper.GetSelectStringArray(
                        Func.GetDetailColumnsNames(false)
                        );

                case Functions.PropDetailResultColumn:
                    return BoxInfoHelper.GetSelectStringArray(
                        Func.GetDetailColumnsNames(false)
                        );

                case Functions.PropSelectExpression:
                    return BoxInfoHelper.GetSelectStringArray(
                        Func.GetDetailColumnsFullNames(false)
                        );

                default:
                    return null;
            }
        }

        /// <summary>
        /// Returns read-only property values for displaying in propertygrid
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="boxModule">The boxmodule.</param>
        /// <returns></returns>
        public override PropertyValue GetReadOnlyPropertyValue(string propertyName, BoxModuleI boxModule)
        {
            Functions Func = (Functions)boxModule.FunctionsIObj;
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
                case Functions.PropValueVariability:
                    return Func.ValueVariability;
                case Functions.PropValueStandardDeviation:
                    return Func.ValueStandardDeviation;
                case Functions.PropValueDistincts:
                    return Func.ValueDistincts;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Box identifier
        /// </summary>
        public const string typeIdentifier = "DataPreparation.DataSource.VirtualColumn";

        /// <summary>
        /// Validation of the box
        /// </summary>
        /// <param name="boxModule">The boxmodule</param>
        public override void Validate(BoxModuleI boxModule)
        {
            Functions Func = (Functions)boxModule.FunctionsIObj;

            // try to invoke methods
            object dummy = Func.GetDataTableFunctionsPrx(true);
            GenericColumn tmp = Func.GetGenericColumn(true);
            dummy = Func.GetColumnsNames(true);
            dummy = Func.GetColumnStatistics(true);
            dummy = Func.GetColumnExplain(true);
            dummy = Func.GetDistinctsAndFrequencies(true);
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
                    new string[] { Functions.PropCardinality },
                    restrictionTypeEnum.OtherReason
                    );
            }
        }

        /// <summary>
        /// Box identifier
        /// </summary>
        protected override string identifier
        {
            get { return typeIdentifier; }
        }

    }
}