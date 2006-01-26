using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Ferda.Modules.Boxes.DataMiningCommon.Attributes.EachValueOneCategoryAttribute
{
	public static class EachValueOneCategoryAlgorithm
	{
		public static GeneratedAttribute Generate(
			string connectionString,
			string dataMatrixName,
			string columnSelectExpression,
			string boxIdentity)
		{
			DataTable dataTable = Ferda.Modules.Helpers.Data.Column.GetDistincts(connectionString, dataMatrixName, columnSelectExpression, boxIdentity);
			CategoriesStruct categoriesStructValue = new CategoriesStruct();
			List<SelectString> categoriesNames = new List<SelectString>();
			categoriesStructValue.enums = new EnumCategorySeq();
			string rowValue;
			string categoryName;
			SelectString item;
			bool getNames = (dataTable.Rows.Count <= AbstractAttributeConstants.MaxLengthOfCategoriesNamesSelectStringArray);
			string includeNullCategoryNameValue = null;
			foreach (DataRow dataRow in dataTable.Rows)
			{
				rowValue = dataRow.ItemArray[0].ToString();
				if (String.IsNullOrEmpty(rowValue))
				{
					categoryName = Ferda.Modules.Helpers.Common.Constants.DbNullCategoryName;
					includeNullCategoryNameValue = categoryName;
				}
				else
				{
					categoryName = rowValue;
				}
				categoriesStructValue.enums.Add(categoryName, new string[1] { rowValue });
				if (getNames)
				{
					item = new SelectString();
					item.label = categoryName;
					item.name = categoryName;
					categoriesNames.Add(item);
				}
			}
			GeneratedAttribute output = new GeneratedAttribute(
				categoriesStructValue,
				includeNullCategoryNameValue,
				categoriesStructValue.enums.Count,
				categoriesNames.ToArray());
			return output;
		}
	}
}
