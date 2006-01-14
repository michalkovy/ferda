using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Modules.Boxes.DataMiningCommon.Attributes
{
	public class GeneratedAttribute
	{
        public GeneratedAttribute()
        {
            this.categoriesStruct = new CategoriesStruct();
            this.categoriesCount = 0;
            this.includeNullCategoryName = String.Empty;
            this.categoriesNames = new SelectString[0];
        }

		public GeneratedAttribute(CategoriesStruct categoriesStruct, string includeNullCategoryName, long categoriesCount)
		{
			this.categoriesStruct = categoriesStruct;
			this.categoriesCount = categoriesCount;
			this.includeNullCategoryName = includeNullCategoryName;
			this.categoriesNames = Ferda.Modules.Helpers.Data.Attribute.CategoriesNamesSelectString(
				categoriesStruct,
				Ferda.Modules.Boxes.DataMiningCommon.Attributes.AbstractAttributeConstants.MaxLengthOfCategoriesNamesSelectStringArray);
		}

		public GeneratedAttribute(CategoriesStruct categoriesStruct, string includeNullCategoryName)
		{
			this.categoriesStruct = categoriesStruct;
			this.categoriesCount = Ferda.Modules.Helpers.Data.Attribute.CategoriesCount(categoriesStruct);
			this.includeNullCategoryName = includeNullCategoryName;
			this.categoriesNames = Ferda.Modules.Helpers.Data.Attribute.CategoriesNamesSelectString(
				categoriesStruct,
				Ferda.Modules.Boxes.DataMiningCommon.Attributes.AbstractAttributeConstants.MaxLengthOfCategoriesNamesSelectStringArray);
		}

		public GeneratedAttribute(CategoriesStruct categoriesStruct, string includeNullCategoryName, long categoriesCount, SelectString[] categoriesNames)
		{
			this.categoriesStruct = categoriesStruct;
			this.categoriesCount = categoriesCount;
			this.includeNullCategoryName = includeNullCategoryName;
			this.categoriesNames = categoriesNames;
		}

		private CategoriesStruct categoriesStruct;
		public CategoriesStruct CategoriesStruct
		{
			get { return categoriesStruct; }
			set { categoriesStruct = value; }
		}

		private long categoriesCount;
		public long CategoriesCount
		{
			get { return categoriesCount; }
			set { categoriesCount = value; }
		}
		
		private string includeNullCategoryName;
		public string IncludeNullCategoryName
		{
			get { return includeNullCategoryName; }
			set { includeNullCategoryName = value; }
		}

		private SelectString[] categoriesNames;
		public SelectString[] CategoriesNames
		{
			get { return categoriesNames; }
			set { categoriesNames = value; }
		}
	}
}
