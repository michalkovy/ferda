// EachValueOneCategoryAlgorithm.cs - algorithm for generating "each value one category" attributes
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
using System.Text;
using System.Data;

namespace Ferda.Modules.Boxes.DataMiningCommon.Attributes.EachValueOneCategoryAttribute
{
	/// <summary>
	/// Algorithm for each value ~ one category attribute.
	/// </summary>
    public static class EachValueOneCategoryAlgorithm
	{
        /// <summary>
        /// Generates the attribute.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="dataMatrixName">Name of the data matrix.</param>
        /// <param name="columnSelectExpression">The column select expression.</param>
        /// <param name="boxIdentity">The box identity.</param>
        /// <returns>Generated attribute.</returns>
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
