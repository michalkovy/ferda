using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Modules.Boxes.DataMiningCommon.Attributes;

namespace Ferda.Modules.Helpers.Data
{
    /// <summary>
    /// This static class provides some methods for working with categories and attributes at all.
    /// </summary>
    public class Attribute
    {
        /// <summary>
        /// Gets the categories names.
        /// </summary>
        /// <param name="categories">The categories.</param>
        /// <param name="maxCategoriesNames">The max categories names.</param>
        /// <returns>
        /// Names of categories. (or empty array of strings if 
        /// count of categories is greater than specified 
        /// <c>maxCategoriesNames</c>)
        /// </returns>
        public static string[] GetCategoriesNames(CategoriesStruct categories, long maxCategoriesNames)
        {
            long count = GetCategoriesCount(categories);
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

        /// <summary>
        /// Gets the categories count.
        /// </summary>
        /// <param name="categories">The categories.</param>
        /// <returns>Categories count.</returns>
        public static long GetCategoriesCount(CategoriesStruct categories)
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

        /// <summary>
        /// Tests the categories count. If there is no category in specified
        /// <c>categories</c> than <see cref="T:Ferda.Modules.BadValueError"/>
        /// is thrown.
        /// </summary>
        /// <param name="categories">The categories.</param>
        /// <param name="boxIdentity">The box identity.</param>
        /// <exception cref="T:Ferda.Modules.BadValueError">Thrown if there is no category in specified <c>categories</c>.</exception>
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
            throw Ferda.Modules.Exceptions.BadValueError(null, boxIdentity, "There is no categoryName in attribute!", new string[] { "Categories" }, restrictionTypeEnum.Other);
        }

        /// <summary>
        /// Tests the categories disjunctivity.
        /// </summary>
        /// <param name="categories">The categories.</param>
        /// <param name="boxIdentity">The box identity.</param>
        /// <exception cref="T:Ferda.Modules.BadValueError">Thrown iff categories are not disjunctive.</exception>
        public static void TestCategoriesDisjunctivity(CategoriesStruct categories, string boxIdentity)
        {//TODO testCategoriesDisjunctivity
            bool allDisjunkt = true;

            if (!allDisjunkt)
            {
                throw Ferda.Modules.Exceptions.BadValueError(null, boxIdentity, "Categories are not disjunktive!", null, Ferda.Modules.restrictionTypeEnum.Other);
            }
        }

        /// <summary>
        /// Tries if the <c>categoryName</c> is in categories.
        /// </summary>
        /// <param name="categories">The categories.</param>
        /// <param name="categoryName">Name of the category.</param>
        /// <returns>True iff <c>categoryName</c> is in categories; otherwise, false.</returns>
        public static bool TryIsCategoryInCategories(CategoriesStruct categories, string categoryName)
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

        /// <summary>
        /// Tests if the categories names are in categories if not than
        /// <see cref="T:Ferda.Modules.BadValueError"/> is thrown.
        /// </summary>
        /// <param name="categories">The categories.</param>
        /// <param name="categoriesNames">The categories names.</param>
        /// <param name="boxIdentity">The box identity.</param>
        /// <exception cref="T:Ferda.Modules.BadValueError">Thrown if specified names (<c>categoriesNames</c>) are not in <c>categories</c>.</exception>
        public static void TestAreCategoriesInCategories(CategoriesStruct categories, string[] categoriesNames, string boxIdentity)
        {
            string notFound = String.Empty;
            foreach (string categoryName in categoriesNames)
            {
                if (TryIsCategoryInCategories(categories, categoryName))
                    continue;
                else
                {
                    notFound = (String.IsNullOrEmpty(notFound)) ? categoryName : notFound + ", " + categoryName;
                }
            }
            if (!String.IsNullOrEmpty(notFound))
            {
                throw Ferda.Modules.Exceptions.BadValueError(null, boxIdentity, "Categories: " + notFound + " is/are not in categories!", new string[0], restrictionTypeEnum.NotInSelectOptions);
            }
        }

        /// <summary>
        /// Gets the setting for new attribute box.
        /// </summary>
        /// <param name="categoriesStruct">The categories struct.</param>
        /// <param name="xCategory">The x category.</param>
        /// <param name="includeNullCategory">The include null category.</param>
        /// <param name="nameInLiterals">The name in literals.</param>
        /// <returns>Property settings for new derived attribute box module.</returns>
        public static PropertySetting[] GetSettingForNewAttributeBox(CategoriesStruct categoriesStruct, string xCategory, string includeNullCategory, string nameInLiterals)
        {
            PropertySetting categoriesProperty = new PropertySetting();
            categoriesProperty.propertyName = "Categories";
            categoriesProperty.value = new CategoriesTI(categoriesStruct);

            /*
            PropertySetting countOfCategoriesProperty = new PropertySetting();
            countOfCategoriesProperty.propertyName = "CountOfCategories";
            countOfCategoriesProperty.result = new Ferda.Modules.LongTI(countOfCategories);
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