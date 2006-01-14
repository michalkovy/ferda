using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Modules.Boxes.DataMiningCommon.Attributes;

namespace Ferda.Modules.Helpers.Data
{
	public class Attribute
	{
		public static string[] CategoriesNames(CategoriesStruct categories, long maxCategoriesNames)
		{
			long count = CategoriesCount(categories);
			if ((maxCategoriesNames > 0 && count > maxCategoriesNames) || (count == 0))
				return new string[0];
			int actualCount = 0;
			string[] result = new string[count];
			if (categories.dateTimeIntervals != null)
			{
				categories.dateTimeIntervals.Keys.CopyTo(result, actualCount);
				actualCount += categories.dateTimeIntervals.Count;
			}
			if (categories.floatIntervals != null)
			{
				categories.floatIntervals.Keys.CopyTo(result, actualCount);
				actualCount += categories.floatIntervals.Count;
			}
			if (categories.longIntervals != null)
			{
				categories.longIntervals.Keys.CopyTo(result, actualCount);
				actualCount += categories.longIntervals.Count;
			}
			if (categories.enums != null)
			{
				categories.enums.Keys.CopyTo(result, actualCount);
				actualCount += categories.enums.Count;
			}
			return result;
		}
		public static SelectString[] CategoriesNamesSelectString(CategoriesStruct categories, long maxCategoriesNames)
		{
			return StringArrayToSelectStringArray(CategoriesNames(categories, maxCategoriesNames));
		}
		public static SelectString[] StringArrayToSelectStringArray(string[] input)
		{
			if ((input == null) || (input.Length == 0))
				return null;
			List<SelectString> result = new List<SelectString>();
			foreach (string item in input)
			{
				SelectString selectString = new SelectString();
				selectString.name = item;
				selectString.label = item;
				result.Add(selectString);
			}
			return result.ToArray();
		}
		public static long CategoriesCount(CategoriesStruct categories)
		{
			long result = 0;
            if (categories != null)
            {
                if (categories.enums != null)
                    result += categories.enums.Count;
                if (categories.longIntervals != null)
                    result += categories.longIntervals.Count;
                if (categories.floatIntervals != null)
                    result += categories.floatIntervals.Count;
                if (categories.dateTimeIntervals != null)
                    result += categories.dateTimeIntervals.Count;
            }
			return result;
		}
		public static void TestCategoriesCount(long countOfCategories, string myIdentity)
		{
			if (countOfCategories <= 0)
			{
				throw Ferda.Modules.Exceptions.BadValueError(null, myIdentity, "There is no categoryName in attribute!", new string[] { "Categories" }, restrictionTypeEnum.Other);
			}
		}
		public static void TestCategoriesCount(CategoriesStruct categories, string boxIdentity)
		{
			if (categories.enums != null && categories.enums.Count > 0)
				return;
			if (categories.longIntervals != null && categories.longIntervals.Count > 0)
				return;
			if (categories.floatIntervals != null && categories.floatIntervals.Count > 0)
				return;
			if (categories.dateTimeIntervals != null && categories.dateTimeIntervals.Count > 0)
				return;
			TestCategoriesCount(-1, boxIdentity);
		}
		public static void TestCategoriesDisjunctivity(CategoriesStruct categories, string boxIdentity)
		{//TODO testCategoriesDisjunctivity
			bool allDisjunkt = true;

			if (!allDisjunkt)
			{
                throw Ferda.Modules.Exceptions.BadValueError(null, boxIdentity, "Categories are not disjunktive!", null, Ferda.Modules.restrictionTypeEnum.Other);
			}
		}
		public static bool TestIsCategoryInCategories(CategoriesStruct categories, string categoryName)
		{
			if (String.IsNullOrEmpty(categoryName))
				return true;
			if (categories.dateTimeIntervals != null && categories.dateTimeIntervals.Contains(categoryName))
				return true;
			if (categories.enums != null && categories.enums.Contains(categoryName))
				return true;
			if (categories.floatIntervals != null && categories.floatIntervals.Contains(categoryName))
				return true;
			if (categories.longIntervals != null && categories.longIntervals.Contains(categoryName))
				return true;
			return false;
		}
		public static void TestXCategoryAndIncludeNullCategoryAreInCategories(CategoriesStruct categories, string xCategory, string includeNullCategory, string myIdentity)
		{
			bool xCategoryFound = TestIsCategoryInCategories(categories, xCategory);
			bool includeNullCategoryFound = TestIsCategoryInCategories(categories, includeNullCategory);

			if (xCategoryFound && includeNullCategoryFound)
				return;
			else if (!xCategoryFound && !includeNullCategoryFound)
			{
				throw Ferda.Modules.Exceptions.BadValueError(null, myIdentity, "X-Category and Include null Category is not in categories!", new string[] { "XCategory", "IncludeNullCategory" }, restrictionTypeEnum.NotInSelectOptions);
			}
			else if (!xCategoryFound)
			{
				throw Ferda.Modules.Exceptions.BadValueError(null, myIdentity, "X-Category is not in categories!", new string[] { "XCategory" }, restrictionTypeEnum.NotInSelectOptions);
			}
			else// if (!includeNullCategoryFound)
			{
				throw Ferda.Modules.Exceptions.BadValueError(null, myIdentity, "Include null Category is not in categories!", new string[] { "IncludeNullCategory" }, restrictionTypeEnum.NotInSelectOptions);
			}
		}
		public static PropertySetting[] GetSettingForNewAttributeBox(CategoriesStruct categoriesStruct, string xCategory, string includeNullCategory, string nameInLiterals)
		{
			PropertySetting categoriesProperty = new PropertySetting();
			categoriesProperty.propertyName = "Categories";
			categoriesProperty.value = new CategoriesTI(categoriesStruct);

			/*
			PropertySetting countOfCategoriesProperty = new PropertySetting();
			countOfCategoriesProperty.propertyName = "CountOfCategories";
			countOfCategoriesProperty.value = new Ferda.Modules.LongTI(countOfCategories);
			 */

			PropertySetting xCategoryProperty = new PropertySetting();
			xCategoryProperty.propertyName = "XCategory";
			xCategoryProperty.value = new Ferda.Modules.StringTI(xCategory);

			PropertySetting includeNullProperty = new PropertySetting();
			includeNullProperty.propertyName = "IncludeNullCategory";
			includeNullProperty.value = new Ferda.Modules.StringTI(includeNullCategory);

			PropertySetting nameInLiteralsProperty = new PropertySetting();
			nameInLiteralsProperty.propertyName = "NameInLiterals";
			nameInLiteralsProperty.value = new Ferda.Modules.StringTI(nameInLiterals);

			return new PropertySetting[] { categoriesProperty, xCategoryProperty, includeNullProperty, nameInLiteralsProperty };
		}
	}
}