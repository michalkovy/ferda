// EachValueOneCategoryattributeFunctionsI.cs - functions object for "each value one category" attribute box module
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
using System.Data.Odbc;
using System.Data;
using Ferda.Modules.Boxes.DataMiningCommon.Column;
using Ferda.Modules.Boxes.DataMiningCommon.Attributes;
using Ferda.Modules.Boxes.DataMiningCommon.Attributes.Attribute;

namespace Ferda.Modules.Boxes.DataMiningCommon.Attributes.EachValueOneCategoryAttribute
{
	class EachValueOneCategoryAttributeFunctionsI : EachValueOneCategoryAttributeFunctionsDisp_, IFunctions, IAbstractDynamicAttribute, IAbstractAttribute
	{
        /// <summary>
        /// The box module.
        /// </summary>
		protected BoxModuleI boxModule;
		//protected IBoxInfo boxInfo;

		#region IFunctions Members
        /// <summary>
        /// Sets the <see cref="T:Ferda.Modules.BoxModuleI">box module</see>
        /// and the <see cref="T:Ferda.Modules.Boxes.IBoxInfo">box info</see>.
        /// </summary>
        /// <param name="boxModule">The box module.</param>
        /// <param name="boxInfo">The box info.</param>
        public void setBoxModuleInfo(BoxModuleI boxModule, IBoxInfo boxInfo)
		{
			this.boxModule = boxModule;
			//this.boxInfo = boxInfo;
		}
		#endregion

		#region Properties

		protected string NameInLiterals
		{
			get
			{
				return this.boxModule.GetPropertyString("NameInLiterals");
			}
		}

		protected string XCategory
		{
			get
			{
				return this.boxModule.GetPropertyString("XCategory");
			}
		}

		#endregion

		#region Categories (CategoriesStruct, Names, Count, includeNullCategoryname)
		private class categoriesCache : Ferda.Modules.Helpers.Caching.Cache
		{
			private GeneratedAttribute value;
			/// <summary>
			/// It is strongly recommended to call this functions before calling any other function in this class.
			/// </summary>
            public GeneratedAttribute Value(string boxIdentity, BoxModuleI boxModule, ColumnInfo columnInfo)
			{
                lock (this)
                {
                    Dictionary<string, IComparable> cacheSetting = new Dictionary<string, IComparable>();
                    cacheSetting.Add(Database.DatabaseBoxInfo.typeIdentifier + Database.DatabaseBoxInfo.OdbcConnectionStringPropertyName, columnInfo.dataMatrix.database.odbcConnectionString);
                    cacheSetting.Add(DataMatrix.DataMatrixBoxInfo.typeIdentifier + DataMatrix.DataMatrixBoxInfo.DataMatrixNamePropertyName, columnInfo.dataMatrix.dataMatrixName);
                    cacheSetting.Add(DataMatrix.DataMatrixBoxInfo.typeIdentifier + DataMatrix.DataMatrixBoxInfo.RecordCountPropertyName, columnInfo.dataMatrix.recordsCount);
                    cacheSetting.Add(Column.ColumnBoxInfo.typeIdentifier + Column.ColumnBoxInfo.ColumnSelectExpressionPropertyName, columnInfo.columnSelectExpression);
                    if (IsObsolete(columnInfo.dataMatrix.database.lastReloadInfo, cacheSetting))
                    {
                        value = new GeneratedAttribute();
                        value = EachValueOneCategoryAlgorithm.Generate(
                            columnInfo.dataMatrix.database.odbcConnectionString,
                            columnInfo.dataMatrix.dataMatrixName,
                            columnInfo.columnSelectExpression,
                            boxIdentity);
                    }
                    if (value == null)
                        value = new GeneratedAttribute();
                    return value;
                }
			}
		}
		private categoriesCache categoriesCached = new categoriesCache();
		private GeneratedAttribute getCategoriesInfo()
		{
			ColumnInfo columnInfo = getColumnFunctionsPrx().getColumnInfo();
            return categoriesCached.Value(boxModule.StringIceIdentity, this.boxModule, columnInfo);
		}
        private GeneratedAttribute getCategoriesInfo(ColumnInfo columnInfo)
		{
            return categoriesCached.Value(boxModule.StringIceIdentity, this.boxModule, columnInfo);
		}
		#endregion

		#region Functions
		public override AbstractAttributeStruct getAbstractAttribute(Ice.Current __current)
		{
			ColumnInfo columnInfo = this.getColumnFunctionsPrx().getColumnInfo();
			AbstractAttributeStruct result = new AbstractAttributeStruct();
			Ferda.Modules.Helpers.Data.Column.TestColumnSelectExpression(
				columnInfo.dataMatrix.database.odbcConnectionString,
				columnInfo.dataMatrix.dataMatrixName,
				columnInfo.columnSelectExpression,
                boxModule.StringIceIdentity);
			GeneratedAttribute categoriesInfo = getCategoriesInfo(columnInfo);
			result.column = columnInfo;
			result.categories = categoriesInfo.CategoriesStruct;
			//AttributeFunctionsI.TestCategoriesDisjunctivity(sumOfRowMax.categories, boxIdentity);
            //This test is useless here (vain / effort / wastage)
            result.identifier = boxModule.PersistentIdentity;
			result.countOfCategories = categoriesInfo.CategoriesCount;
			result.includeNullCategory = categoriesInfo.IncludeNullCategoryName;
			result.xCategory = XCategory;
            Ferda.Modules.Helpers.Data.Attribute.TestAreCategoriesInCategories(result.categories, new string[] { result.xCategory, result.includeNullCategory } , boxModule.StringIceIdentity);
			result.nameInLiterals = NameInLiterals;

            return result;
		}
		#endregion

		#region Sockets
		public BoxModulePrx GetColumnBoxModulePrx()
		{
			return SocketConnections.GetBoxModulePrx(boxModule, "ColumnOrDerivedColumn");
		}
		protected ColumnFunctionsPrx getColumnFunctionsPrx()
		{
			return ColumnFunctionsPrxHelper.checkedCast(
				SocketConnections.GetObjectPrx(boxModule, "ColumnOrDerivedColumn")
				);
		}
		#endregion

		#region Actions
		#endregion

        #region BoxInfo

        public GeneratedAttribute GetGeneratedAttribute()
        {
            try
            {
                return getCategoriesInfo();
            }
            catch (Ferda.Modules.BoxRuntimeError)
            {
                return new GeneratedAttribute();
            }
        }

        public SelectString[] GetPropertyCategoriesNames()
        {
            return this.GetGeneratedAttribute().CategoriesNames;
        }

        public PropertySetting[] GetSettingForNewAttributeBox()
        {
            try
            {
                GeneratedAttribute categoriesInfo = getCategoriesInfo();
                return Ferda.Modules.Helpers.Data.Attribute.GetSettingForNewAttributeBox(
                    categoriesInfo.CategoriesStruct,
                    XCategory,
                    categoriesInfo.IncludeNullCategoryName,
                    NameInLiterals);
            }
            catch (Ferda.Modules.BoxRuntimeError) { }
            return new PropertySetting[0];
        }

        #endregion
	}
}