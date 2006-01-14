using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using Ferda.Modules.Boxes.DataMiningCommon.Column;
using Ferda.Modules.Boxes.DataMiningCommon.Attributes;

namespace Ferda.Modules.Boxes.DataMiningCommon.Attributes.Attribute
{
	class AttributeFunctionsI : AttributeFunctionsDisp_, IFunctions, IAbstractAttribute
	{
		protected BoxModuleI boxModule;
		//protected IBoxInfo boxInfo;

		#region IFunctions Members
		public void setBoxModuleInfo(BoxModuleI boxModule, IBoxInfo boxInfo)
		{
			this.boxModule = boxModule;
			//this.boxInfo = boxInfo;
		}
		#endregion

		#region Properties
		protected string IncludeNullCategory
		{
			get
			{
				return this.boxModule.GetPropertyString("IncludeNullCategory");
			}
		}

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

		protected CategoriesStruct Categories
		{
			get
			{
                return ((CategoriesTI)this.boxModule.GetPropertyOther("Categories")).getCategories();
			}
		}
		#endregion

		#region Functions
		public override AbstractAttributeStruct getAbstractAttribute(Ice.Current __current)
		{
			AbstractAttributeStruct result = new AbstractAttributeStruct();
			result.column = getColumnFunctionsPrx().getColumn();
			result.categories = Categories;
            Ferda.Modules.Helpers.Data.Attribute.TestCategoriesDisjunctivity(result.categories, boxModule.StringIceIdentity);
			result.countOfCategories = Ferda.Modules.Helpers.Data.Attribute.CategoriesCount(result.categories);

			//Ferda.Modules.Helpers.Data.Attribute.TestCategoriesCount(result.countOfCategories, boxIdentity);

            result.identifier = boxModule.PersistentIdentity;
			result.includeNullCategory = IncludeNullCategory;
			result.xCategory = XCategory;
            Ferda.Modules.Helpers.Data.Attribute.TestXCategoryAndIncludeNullCategoryAreInCategories(result.categories, result.xCategory, result.includeNullCategory, boxModule.StringIceIdentity);
			result.nameInLiterals = NameInLiterals;
			return result;
		}
		#endregion

		#region Sockets
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
		public long CountOfCategories()
		{
			return Ferda.Modules.Helpers.Data.Attribute.CategoriesCount(Categories);
		}
		#endregion

		#region IAbstractAttribute Members

		public SelectString[] GetPropertyCategoriesNames()
		{
			return Ferda.Modules.Helpers.Data.Attribute.CategoriesNamesSelectString(Categories, AbstractAttributeConstants.MaxLengthOfCategoriesNamesSelectStringArray);
		}

		#endregion
}
}
