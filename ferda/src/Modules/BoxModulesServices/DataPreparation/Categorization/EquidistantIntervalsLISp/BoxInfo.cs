// BoxInfo.cs - box functions for Equidistant intervals LISp box
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
using System.Diagnostics;
using System.Collections.Generic;
using Ferda.Guha.Data;
using Object = Ice.Object;
using FixedAtom = Ferda.Modules.Boxes.GuhaMining.FixedAtom;
using AtomSetting = Ferda.Modules.Boxes.GuhaMining.AtomSetting;

namespace Ferda.Modules.Boxes.DataPreparation.Categorization.EquidistantIntervalsLISp
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
            Functions Func = (Functions)boxModule.FunctionsIObj;
            string label = String.Empty;
            if (Func.NameInLiterals != string.Empty && Func.NameInLiterals != null)
            {
                return Func.NameInLiterals;
            }
            else
            {
                try
                {
                    label =
                    Func.GetColumnFunctionsPrx(false).getColumnInfo().columnSelectExpression;
                }
                catch { }
                return label;
            }  
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

            foreach (string moduleAFCname in modulesAFC.Keys)
            {
                singleModule = new ModuleAskingForCreation();
                moduleConnection = new ModulesConnection();
                //no need to set any property
                singleModule.propertySetting = new PropertySetting[] { };

                switch (moduleAFCname)
                {
                    case "FixedAtom":
                        //creating the info about the connections of the new module
                        moduleConnection.socketName =
                            FixedAtom.Functions.SockBitStringGenerator;
                        moduleConnection.boxModuleParam = boxModule.MyProxy;

                        //creating the new (single) module
                        singleModule.modulesConnection =
                            new ModulesConnection[] { moduleConnection };
                        singleModule.newBoxModuleIdentifier =
                            FixedAtom.BoxInfo.typeIdentifier;
                        break;

                    case "AtomSetting":
                        //creating the info about the connections of the new module
                        moduleConnection.socketName =
                            AtomSetting.Functions.SockBitStringGenerator;
                        moduleConnection.boxModuleParam = boxModule.MyProxy;

                        //creating the new (single) module
                        singleModule.modulesConnection =
                            new ModulesConnection[] { moduleConnection };
                        singleModule.newBoxModuleIdentifier =
                            AtomSetting.BoxInfo.typeIdentifier;
                        break;

                    case "StaticAttribute":
                        //creating the info about the connections of the new module
                        moduleConnection.socketName =
                            StaticAttribute.Functions.SockColumn;
                        moduleConnection.boxModuleParam = boxModule.MyProxy;

                        //creating the new (single) module
                        singleModule.modulesConnection =
                            new ModulesConnection[] { moduleConnection };
                        singleModule.newBoxModuleIdentifier =
                            StaticAttribute.BoxInfo.typeIdentifier;

                        //getting and testing the attribute (doens't have to be connected)
                        Guha.Attribute.Attribute<IComparable> attribute =
                            ((Functions)boxModule.FunctionsIObj).GetAttribute(false);
                        if (attribute == null)
                        {
                            break;
                        }

                        PropertySetting editCategories =
                            new PropertySetting(Functions.PropCategories, new StringTI(
                            Guha.Attribute.Serializer.Serialize(
                            (attribute.Export()))));
                      
                        singleModule.propertySetting = new PropertySetting[] { editCategories };
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

        public override PropertyValue GetReadOnlyPropertyValue(string propertyName, BoxModuleI boxModule)
        {
            Functions Func = (Functions)boxModule.FunctionsIObj;
            switch (propertyName)
            {
                case Functions.PropCountOfCategories:
                    return Func.CountOfCategories;
                case Functions.PropIncludeNullCategory:
                    return Func.IncludeNullCategory;
                default:
                    throw new NotImplementedException();
            }
        }

        public override SelectString[] GetPropertyOptions(string propertyName, BoxModuleI boxModule)
        {
            Functions Func = (Functions)boxModule.FunctionsIObj;

            switch (propertyName)
            {
                case Functions.PropXCategory:
                    return BoxInfoHelper.GetSelectStringArray(
                        Func.GetCategoriesNames(false)
                        );
                default:
                    return null;
            }
        }

        public const string typeIdentifier = "DataPreparation.Categorization.EquidistantIntervalsLISp";

        protected override string identifier
        {
            get { return typeIdentifier; }
        }

        public override void Validate(BoxModuleI boxModule)
        {
            Functions Func = (Functions)boxModule.FunctionsIObj;
            if (Func.Length <= 0)
            {
                throw Exceptions.BadValueError(
                    null,
                    boxModule.StringIceIdentity,
                    "Count of intervals has to be greater than 0.",
                    new string[] { Functions.PropCountOfCategories },
                    restrictionTypeEnum.OtherReason
                    );
            }
            // try to invoke methods
            object dummy = Func.GetColumnFunctionsPrx(true);
            dummy = Func.GetAttributeId();
            dummy = Func.GetAttributeNames();
            try
            {
                dummy = Func.GetAttribute(true);
            }
            catch
            {
                throw Exceptions.BadParamsError(
                    null,
                    boxModule.StringIceIdentity,
                    "Requested number of intervals is either 0, exceeds count of values or domain is set incorrectly",
                    restrictionTypeEnum.OtherReason
                    );
            }
            dummy = Func.GetCategoriesNames(true);
            dummy = Func.GetCategoriesAndFrequencies(true);
            dummy = Func.GetBitStrings(true);
            Debug.Assert(dummy == null);

            if (String.IsNullOrEmpty(Func.NameInLiterals))
                throw Exceptions.BadValueError(
                    null,
                    boxModule.StringIceIdentity,
                    "Property \"Name in results\" can not be empty string.",
                    new string[] { Functions.PropNameInLiterals },
                    restrictionTypeEnum.OtherReason
                    );


            CardinalityEnum potentiallyCardinality = Func.PotentiallyCardinality(true);

            if (Common.CompareCardinalityEnums(
                    Func.Cardinality,
                    potentiallyCardinality
                    ) > 1)
            {
                throw Exceptions.BadValueError(
                    null,
                    boxModule.StringIceIdentity,
                    "Unsupported cardinality type for current attribute setting.",
                    new string[] { Functions.PropCardinality },
                    restrictionTypeEnum.OtherReason
                    );
            }

            /*
            if (potentiallyCardinality != CardinalityEnum.Cardinal)
            {
                throw Exceptions.BadValueError(
                    null,
                    boxModule.StringIceIdentity,
                    "Unsupported cardinality type for current attribute setting.",
                    new string[] { Functions.PropCardinality },
                    restrictionTypeEnum.OtherReason
                    );
            }
            */
        }
    }
}