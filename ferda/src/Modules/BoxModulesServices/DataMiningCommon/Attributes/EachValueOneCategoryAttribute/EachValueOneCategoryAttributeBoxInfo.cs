// EachValueOneCategoryAttributeBoxInfo.cs - box info for "each value one category" attribute box module
//
// Author: Tomáš Kuchař <tomas.kuchar@gmail.com>
//
// Copyright (c) 2005 Tomáš Kuchař
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

namespace Ferda.Modules.Boxes.DataMiningCommon.Attributes.EachValueOneCategoryAttribute
{
    class EachValueOneCategoryAttributeBoxInfo : Ferda.Modules.Boxes.DataMiningCommon.Attributes.AbstractDynamicAttributeBoxInfo
    {
        public const string typeIdentifier =
            "DataMiningCommon.Attributes.EachValueOneCategoryAttribute";

        protected override string identifier
        {
            get { return typeIdentifier; }
        }

        public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
        {
            EachValueOneCategoryAttributeFunctionsI result = new EachValueOneCategoryAttributeFunctionsI();
            iceObject = (Ice.Object)result;
            functions = (IFunctions)result;
        }

        public override string[] GetBoxModuleFunctionsIceIds()
        {
            return EachValueOneCategoryAttributeFunctionsI.ids__;
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
            EachValueOneCategoryAttributeFunctionsI Func = (EachValueOneCategoryAttributeFunctionsI)boxModule.FunctionsIObj;
            switch (propertyName)
            {
                //case "Categories":
                //    return new Ferda.Modules.CategoriesTI(Func.GetGeneratedAttribute().CategoriesStruct);
                case "CountOfCategories":
                    return new Ferda.Modules.LongTI(Func.GetGeneratedAttribute().CategoriesCount);
                case "IncludeNullCategory":
                    return new Ferda.Modules.StringTI(Func.GetGeneratedAttribute().IncludeNullCategoryName);
                default:
                    throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(propertyName);
            }
        }

        /// <summary>
        /// Gets the function object of abstract attribute.
        /// </summary>
        /// <param name="boxModule">The box module.</param>
        /// <returns>FunctionsI object.</returns>
        public override IAbstractDynamicAttribute getFuncIAbstractDynamicAttribute(BoxModuleI boxModule)
        {
            EachValueOneCategoryAttributeFunctionsI Func = (EachValueOneCategoryAttributeFunctionsI)boxModule.FunctionsIObj;
            return Func;
        }

        protected override IAbstractAttribute getFuncIAbstractAttribute(BoxModuleI boxModule)
        {
            EachValueOneCategoryAttributeFunctionsI Func = (EachValueOneCategoryAttributeFunctionsI)boxModule.FunctionsIObj;
            return Func;
        }
    }
}