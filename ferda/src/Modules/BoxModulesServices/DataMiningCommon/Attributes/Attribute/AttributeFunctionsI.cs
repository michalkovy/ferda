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
			result.column = getColumnFunctionsPrx().getColumnInfo();
			result.categories = Categories;
            Ferda.Modules.Helpers.Data.Attribute.TestCategoriesDisjunctivity(result.categories, boxModule.StringIceIdentity);
            result.countOfCategories = Ferda.Modules.Helpers.Data.Attribute.GetCategoriesCount(result.categories);

			//Ferda.Modules.Helpers.Data.Attribute.TestCategoriesCount(result.countOfCategories, boxIdentity);

            result.identifier = boxModule.PersistentIdentity;
			result.includeNullCategory = IncludeNullCategory;
			result.xCategory = XCategory;
            Ferda.Modules.Helpers.Data.Attribute.TestAreCategoriesInCategories(result.categories, new string[] { result.xCategory, result.includeNullCategory } , boxModule.StringIceIdentity);
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
			return Ferda.Modules.Helpers.Data.Attribute.GetCategoriesCount(Categories);
		}
		#endregion

		#region IAbstractAttribute Members

		public SelectString[] GetPropertyCategoriesNames()
		{
            return Ferda.Modules.Boxes.BoxInfoHelper.StringArrayToSelectStringArray(
                Ferda.Modules.Helpers.Data.Attribute.GetCategoriesNames(
                    Categories,
                    Ferda.Modules.Boxes.DataMiningCommon.Attributes.AbstractAttributeConstants.MaxLengthOfCategoriesNamesSelectStringArray
                    )
                );
		}

		#endregion
}
}
