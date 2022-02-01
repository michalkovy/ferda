// BoxInfo.cs - BoxInfo class for the atom setting box
//
// Authors: Tomáš Kuchaø <tomas.kuchar@gmail.com>,
//          Martin Ralbovský <martin.ralbovsky@gmail.com> 
//          (modulesAFC, comments)
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
using System.Diagnostics;
using System.Collections.Generic;
using Ferda.Guha.Data;
using Ferda.Guha.MiningProcessor;
using Object = Ice.Object;

namespace Ferda.Modules.Boxes.GuhaMining.AtomSetting
{
    /// <summary>
    /// Class that provides info about boxes of the AtomSetting type
    /// </summary>
    public class BoxInfo : Boxes.BoxInfo
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
            Functions Func = (Functions) boxModule.FunctionsIObj;
            string attributeName = Func.GetAttributeName(false);
            if (attributeName == null)
                return null;
            else
            {
                //getting the coefficient type and creating the label of the box
                //from the name
                string shortCoefficientType =
                    GetPropertyOptionShortLocalizedLabel(Functions.PropCoefficientType, Func.CoefficientType.ToString(),
                                                         boxModule.LocalePrefs);
                return
                    attributeName + "(" + shortCoefficientType + "[" + Func.MinimalLength + "-" + Func.MaximalLength +
                    "])";
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

            foreach (string moduleAFCname in modulesAFC.Keys)
            {
                singleModule = new ModuleAskingForCreation();
                moduleConnection = new ModulesConnection();
                //no need to set any property
                singleModule.propertySetting = new PropertySetting[] { };

                switch (moduleAFCname)
                {
                    case "ConjunctionSetting":
                        //creating the info about the connections of the new module
                        moduleConnection.socketName =
                            ConjunctionSetting.Functions.SockBooleanAttributeSetting;
                        moduleConnection.boxModuleParam = boxModule.MyProxy;

                        //creating the new (single) module
                        singleModule.modulesConnection =
                            new ModulesConnection[] { moduleConnection };
                        singleModule.newBoxModuleIdentifier =
                            ConjunctionSetting.BoxInfo.typeIdentifier;
                        break;

                    case "DisjunctionSetting":
                        //creating the info about the connections of the new module
                        moduleConnection.socketName =
                            DisjunctionSetting.Functions.SockBooleanAttributeSetting;
                        moduleConnection.boxModuleParam = boxModule.MyProxy;

                        //creating the new (single) module
                        singleModule.modulesConnection =
                            new ModulesConnection[] { moduleConnection };
                        singleModule.newBoxModuleIdentifier =
                            DisjunctionSetting.BoxInfo.typeIdentifier;
                        break;

                    case "Sign":
                        //creating the info about the connections of the new module
                        moduleConnection.socketName =
                            Sign.Functions.SockBooleanAttributeSetting;
                        moduleConnection.boxModuleParam = boxModule.MyProxy;

                        //creating the new (single) module
                        singleModule.modulesConnection =
                            new ModulesConnection[] { moduleConnection };
                        singleModule.newBoxModuleIdentifier =
                            Sign.BoxInfo.typeIdentifier;
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
            return null;
        }

        /// <summary>
        /// Validates the box
        /// </summary>
        /// <param name="boxModule">box instance to be validated</param>
        public override void Validate(BoxModuleI boxModule)
        {
            Functions Func = (Functions) boxModule.FunctionsIObj;

            if (Func.MinimalLength > Func.MaximalLength)
                throw Exceptions.BadValueError(null, boxModule.StringIceIdentity,
                                               "Minimal length is not less or equal to maximal length.",
                                               new string[] {Functions.PropMinimalLength, Functions.PropMaximalLength},
                                               restrictionTypeEnum.Minimum);

            if (Func.MinimalLength < 1)
                throw Exceptions.BadValueError(null, boxModule.StringIceIdentity,
                                               "Minimal length must be greather than 1.",
                                               new string[] {Functions.PropMinimalLength},
                                               restrictionTypeEnum.Minimum);

            string[] categories = Func.GetCategoriesIds(true);
            if (categories != null && categories.Length < Func.MinimalLength)
                throw Exceptions.BadValueError(null, boxModule.StringIceIdentity,
                                               "Count of categories in input attribute must be greater than Minimal length.",
                                               new string[]
                                                   {Functions.SockBitStringGenerator, Functions.PropMinimalLength},
                                               restrictionTypeEnum.OtherReason);

            //conform coefficient type a attribute cardinality type
            CardinalityEnum attributeCardinalityType = Func.GetAttributeCardinality(true);
            CoefficientTypeEnum coefficientType = Func.CoefficientType;
            if (coefficientType == CoefficientTypeEnum.Subsets)
            {
                //OK
            }
            else if (coefficientType == CoefficientTypeEnum.LeftCuts
                     || coefficientType == CoefficientTypeEnum.RightCuts
                     || coefficientType == CoefficientTypeEnum.Cuts
                     || coefficientType == CoefficientTypeEnum.Intervals)
            {
                if (attributeCardinalityType == CardinalityEnum.Ordinal
                    || attributeCardinalityType == CardinalityEnum.Cardinal)
                {
                    //OK
                }
                else
                {
                    // cyclic ordinal / nominal
                    throw Exceptions.BadValueError(null, boxModule.StringIceIdentity,
                                                   "For current cardinality type of source attribute is not allowed to make cuts or intervals.",
                                                   new string[]
                                                       {Functions.SockBitStringGenerator, Functions.PropCoefficientType},
                                                   restrictionTypeEnum.OtherReason);
                }
            }
            else if (coefficientType == CoefficientTypeEnum.CyclicIntervals)
            {
                if (attributeCardinalityType == CardinalityEnum.OrdinalCyclic)
                {
                    //OK
                }
                else
                {
                    // nominal / ordinal / cardinal / numeric cardinal
                    throw Exceptions.BadValueError(null, boxModule.StringIceIdentity,
                                                   "For current cardinality type of source attribute is not allowed to make cyclic intervals.",
                                                   new string[]
                                                       {Functions.SockBitStringGenerator, Functions.PropCoefficientType},
                                                   restrictionTypeEnum.OtherReason);
                }
            }

            // try to invoke methods
            object dummy = Func.GetEntitySetting(true);
            dummy = Func.GetAttributeName(true);
        }

        #region Type Identifier

        /// <summary>
        /// This is recomended (not required) to have <c>public const string</c> 
        /// field in the BoxInfo implementation which holds the identifier 
        /// of type of the box module.
        /// </summary>
        public const string typeIdentifier = "GuhaMining.AtomSetting";

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