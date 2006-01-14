using Ice;
using System.Collections.Generic;
using System.Collections;

namespace Ferda.Modules
{
	public class CategoriesTI : CategoriesT, IValue
	{
		public struct Categories
		{
			public struct DateTimeCategory
			{
				public DateTimeCategory(string name, DateTimeIntervalStruct[] value)
				{
					Name = name;
					Value = value;
				}
				public string Name;
				public DateTimeIntervalStruct[] Value;
			}
			public struct FloatCategory
			{
				public FloatCategory(string name, FloatIntervalStruct[] value)
				{
					Name = name;
					Value = value;
				}
				public string Name;
				public FloatIntervalStruct[] Value;
			}
			public struct LongCategory
			{
				public LongCategory(string name, LongIntervalStruct[] value)
				{
					Name = name;
					Value = value;
				}
				public string Name;
				public LongIntervalStruct[] Value;
			}
			public struct EnumCategory
			{
				public EnumCategory(string name, string[] value)
				{
					Name = name;
					Value = value;
				}
				public string Name;
				public string[] Value;
			}
			public DateTimeCategory[] DateTimes;
			public FloatCategory[] Floats;
			public LongCategory[] Longs;
			public EnumCategory[] Enums;

			public static Categories Dict2Struct(CategoriesStruct categories)
			{
				Categories result = new Categories();
				List<Categories.DateTimeCategory> dateTimeCategories = new List<Categories.DateTimeCategory>();
				List<Categories.FloatCategory> floatCategories = new List<Categories.FloatCategory>();
				List<Categories.LongCategory> longCategories = new List<Categories.LongCategory>();
				List<Categories.EnumCategory> enumCategories = new List<Categories.EnumCategory>();
				if (categories.dateTimeIntervals != null)
					foreach (DictionaryEntry category in categories.dateTimeIntervals)
					{
						dateTimeCategories.Add(new Categories.DateTimeCategory((string)category.Key, (DateTimeIntervalStruct[])category.Value));
					}
				if (categories.floatIntervals != null)
					foreach (DictionaryEntry category in categories.floatIntervals)
					{
						floatCategories.Add(new Categories.FloatCategory((string)category.Key, (FloatIntervalStruct[])category.Value));
					}
				if (categories.longIntervals != null)
					foreach (DictionaryEntry category in categories.longIntervals)
					{
						/*
						 * System.InvalidCastException: Specified cast is not valid.
						 *    at Ferda.Modules.CategoriesTI.Categories.Dict2Struct(CategoriesStruct categories) in e:\Saves\Projekt\svn\src\PropertyTypes\CategoriesTI.cs:line 73
						 *    at Ferda.Modules.CategoriesTI.getValueT() in e:\Saves\Projekt\svn\src\PropertyTypes\CategoriesTI.cs:line 122
						 *    at Ferda.ProjectManager.ProjectManager.SaveProject(Stream stream) in e:\Saves\Projekt\svn\src\ProjectManager\ProjectManager.cs:line 356
						 * */
						longCategories.Add(new Categories.LongCategory((string)category.Key, (LongIntervalStruct[])category.Value));
					}
				
				if (categories.enums != null)
					foreach (DictionaryEntry category in categories.enums)
					{
						enumCategories.Add(new Categories.EnumCategory((string)category.Key, (string[])category.Value));
					}
				result.DateTimes = dateTimeCategories.ToArray();
				result.Floats = floatCategories.ToArray();
				result.Longs = longCategories.ToArray();
				result.Enums = enumCategories.ToArray();
				return result;
			}

			public static CategoriesStruct Struct2Dict(Categories categories)
			{
				CategoriesStruct result = new CategoriesStruct();
				result.dateTimeIntervals = new DateTimeIntervalCategorySeq();
				result.floatIntervals = new FloatIntervalCategorySeq();
				result.longIntervals = new LongIntervalCategorySeq();
				result.enums = new EnumCategorySeq();
				foreach (DateTimeCategory category in categories.DateTimes)
				{
					result.dateTimeIntervals.Add(category.Name, category.Value);
				}
				foreach (FloatCategory category in categories.Floats)
				{
					result.floatIntervals.Add(category.Name, category.Value);
				}
				foreach (LongCategory category in categories.Longs)
				{
					result.longIntervals.Add(category.Name, category.Value);
				}
				foreach (EnumCategory category in categories.Enums)
				{
					result.enums.Add(category.Name, category.Value);
				}
				return result;
			}
		}

		ValueT IValue.getValueT()
		{
			CategoriesValueT result = new CategoriesValueT();
			result.Value = Categories.Dict2Struct(this.categoriesValue);
			//result.Value.categoriesValue = Categories.Dict2Struct(this.categoriesValue);
			//result.Value = this(dict) -> struct
			//TODO
			return result;
		}

		public CategoriesTI()
        { this.categoriesValue = new CategoriesStruct(); }

		public CategoriesTI(CategoriesTInterfacePrx iface)
		{
			this.categoriesValue = iface.getCategories();
		}

		public CategoriesTI(CategoriesStruct categoriesStruct)
		{
			this.categoriesValue = categoriesStruct;
		}

		public override CategoriesStruct getCategories(Current __current)
		{
			return this.categoriesValue;
		}
	}
}
