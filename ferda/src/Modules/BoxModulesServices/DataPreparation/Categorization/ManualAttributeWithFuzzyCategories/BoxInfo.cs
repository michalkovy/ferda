// BoxInfo.cs - BoxInfo class for the Manual Attribute with fuzzy categories box
//
// Author: Martin Ralbovský <martin.ralbovsky@gmail.cz>
//
// Copyright (c) 2009 Martin Ralbovský
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
using Ferda.Modules;
using Ferda.Guha.Data;

namespace Ferda.Modules.Boxes.DataPreparation.Categorization.ManualAttributeWithFuzzyCategories
{
    /// <summary>
    /// This class implements most of functions used by <see cref="T:Ferda.ModulesManager"/>.
    /// For more information <see cref="T:Ferda.Modules.Boxes.BoxInfo"/>.
    /// </summary>
    /// <seealso cref="T:Ferda.Modules.BoxModuleI"/>
    /// <seealso cref="T:Ferda.Modules.BoxModuleFactoryI"/>
    /// <seealso cref="T:Ferda.Modules.BoxModuleFactoryCreatorI"/>
    internal class BoxInfo : Boxes.BoxInfo
    {
        /// <summary>
        /// Functions creates an object of <see cref="T:Ferda.Modules.IFunctions">IFuntions</see>
        /// type that provides functionality of the box
        /// </summary>
        /// <param name="boxModule">Current box module</param>
        /// <param name="iceObject">ICE stuff</param>
        /// <param name="functions">The new created functions object</param>
        public override void CreateFunctions(Ferda.Modules.BoxModuleI boxModule, out Ice.Object iceObject, out Ferda.Modules.IFunctions functions)
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
        /// <see cref="M:Ferda.Modules.Boxes.IBoxInfo.GetBoxModuleFunctionsIceIds()"/>.
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
        public override string GetDefaultUserLabel(Ferda.Modules.BoxModuleI boxModule)
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
        public override Ferda.Modules.ModulesAskingForCreation[] GetModulesAskingForCreation(string[] localePrefs, Ferda.Modules.BoxModuleI boxModule)
        {
            return new ModulesAskingForCreation[] { };
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
        /// Returns read-only property values for displaying in propertygrid
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="boxModule">The boxmodule.</param>
        /// <returns></returns>
        public override PropertyValue GetReadOnlyPropertyValue(string propertyName, BoxModuleI boxModule)
        {
            LongTI result = new LongTI();
            Functions Func = (Functions)boxModule.FunctionsIObj;
            switch (propertyName)
            {
                case Public.SockCountOfCategories:
                    if (Func.FuzzySets != null)
                    {
                        result.longValue = Func.FuzzySets.fuzzySets.Length;
                    }
                    else
                    {
                        result.longValue = 0;
                    }
                    return result;
                default:
                    throw new NotImplementedException();
            }
            return null;
        }

        /// <summary>
        /// Validation of the box
        /// </summary>
        /// <param name="boxModule">The boxmodule</param>
        public override void Validate(BoxModuleI boxModule)
        {
            Functions Func = (Functions)boxModule.FunctionsIObj;
            
            ColumnFunctionsPrx colPrs = Public.GetColumnFunctionsPrx(true, boxModule);

            if (colPrs.getColumnInfo().cardinality != CardinalityEnum.Cardinal)
            {
                throw Exceptions.BadParamsError(null,
                                    boxModule.StringIceIdentity,
                                    "Fuzzy categories can be created only from cardinal columns. Set the semantics of the column to cardinal",
                                    restrictionTypeEnum.OtherReason);
            }

            if (!GenericColumn.GetIsNumericDataType(colPrs.getColumnInfo().dataType))
            {
                throw Exceptions.BadParamsError(null,
                                    boxModule.StringIceIdentity,
                                    "The data type of the column is not supported for creation of fuzzy categories. Currently all numeric data types are supported.",
                                    restrictionTypeEnum.OtherReason);
            }

            if (Func.FuzzySets == null)
            {
                throw Exceptions.BoxRuntimeError(
                    null,
                    boxModule.StringIceIdentity,
                    "There has to be at least one category or interval created in the static attribute");
            }

            if (String.IsNullOrEmpty(Func.NameInBooleanAttributes))
                throw Exceptions.BadValueError(
                    null,
                    boxModule.StringIceIdentity,
                    "Property \"Name in Boolean attributes\" can not be empty string.",
                    new string[] { Public.SockNameInBooleanAttributes },
                    restrictionTypeEnum.OtherReason
                    );

            object dummy = Func.GetCategoriesAndFrequencies(true);
            //dummy = Func.GetBitStrings(true);
        }

        #region Type Identifier

        /// <summary>
        /// This is recomended (not required) to have <c>public const string</c>
        /// field in the BoxInfo implementation which holds the identifier
        /// of type of the box module.
        /// </summary>
        public const string typeIdentifier = "DataPreparation.Categorization.ManualAttributeWithFuzzyCategories";

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

        /* Other functions to override
    * public virtual PropertyValue GetPropertyObjectFromInterface(string propertyName, Ice.ObjectPrx objectPrx)
    * public virtual bool IsPropertySet(string propertyName, PropertyValue propertyValue)
    * public virtual DynamicHelpItem[] GetDynamicHelpItems(string[] localePrefs, BoxModuleI boxModule)
    * public virtual void RunAction(string actionName, BoxModuleI boxModule)
    * */
    }
}
