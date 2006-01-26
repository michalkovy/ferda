using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using Ferda.Modules.Helpers.Common;

namespace Ferda.Modules.Boxes.DataMiningCommon.Attributes.EquifrequencyIntervalsAttribute
{
	public static class EquidistantAlgorithm
	{
		public static GeneratedAttribute Generate(
			AttributeDomainEnum domainType,
			double from,
			double to,
			long countOfCategories,
			Ferda.Modules.Boxes.DataMiningCommon.Column.ColumnStruct columnStruct,
			string boxIdentity)
		{
			Ferda.Modules.Helpers.Data.Column.SimpleTypeEnum simpleColumnType = Ferda.Modules.Helpers.Data.Column.GetColumnSimpleSubTypeBySubType(columnStruct.columnSubType);
			if (simpleColumnType != Ferda.Modules.Helpers.Data.Column.SimpleTypeEnum.Integral
				&& simpleColumnType != Ferda.Modules.Helpers.Data.Column.SimpleTypeEnum.Floating)
				throw Ferda.Modules.Exceptions.BadParamsError(null, boxIdentity, simpleColumnType.ToString(), restrictionTypeEnum.DbColumnDataType);

			//dataArray.Add(new Data(currentValue, currentCount));
			ArrayList dataArray = new ArrayList();
			DataTable frequencies = null;
			float startValue = Convert.ToSingle(columnStruct.statistics.ValueMin);
			float endValue = Convert.ToSingle(columnStruct.statistics.ValueMax);
			switch (domainType)
			{
				case AttributeDomainEnum.WholeDomain:
					frequencies = Ferda.Modules.Helpers.Data.Column.GetDistinctsAndFrequencies(
						columnStruct.dataMatrix.database.connectionString,
						columnStruct.dataMatrix.dataMatrixName,
						columnStruct.columnSelectExpression,
						boxIdentity);
					from = to = 0;
					break;
				case AttributeDomainEnum.SubDomainValueBounds:
                    frequencies = Ferda.Modules.Helpers.Data.Column.GetDistinctsAndFrequencies(
						columnStruct.dataMatrix.database.connectionString,
						columnStruct.dataMatrix.dataMatrixName,
						columnStruct.columnSelectExpression,
						columnStruct.columnSelectExpression + ">=" + from + " AND " + columnStruct.columnSelectExpression + "<=" + to,
						boxIdentity);
					startValue = (float)from;
					endValue = (float)to;
					from = to = 0;
					break;
				case AttributeDomainEnum.SubDomainNumberOfValuesBounds:
                    frequencies = Ferda.Modules.Helpers.Data.Column.GetDistinctsAndFrequencies(
						columnStruct.dataMatrix.database.connectionString,
						columnStruct.dataMatrix.dataMatrixName,
						columnStruct.columnSelectExpression,
						boxIdentity);
					break;
				default:
					throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(domainType);
			}

			if (from < 0)
			{
                throw Ferda.Modules.Exceptions.BadValueError(null, boxIdentity, null, new string[] { "From" }, restrictionTypeEnum.Minimum);
			}
			if (to < 0)
			{
                throw Ferda.Modules.Exceptions.BadValueError(null, boxIdentity, null, new string[] { "To" }, restrictionTypeEnum.Minimum);
			}
			int fromUi = (int)from;
			int toUi = (int)to;
			int frequenciesCount = frequencies.Rows.Count;
			if (frequenciesCount <= from + to)
				return new GeneratedAttribute();
			object item;

			switch (simpleColumnType)
			{
				case Ferda.Modules.Helpers.Data.Column.SimpleTypeEnum.Floating:
					for (int i = fromUi; i < (frequenciesCount - toUi); i++)
					{
						item = frequencies.Rows[i][Ferda.Modules.Helpers.Data.Column.SelectDistincts];
						if (item == System.DBNull.Value)
							continue;
                        dataArray.Add(new EquifrequencyIntervalGenerator.Data(Convert.ToSingle(item), Convert.ToInt32(frequencies.Rows[i][Ferda.Modules.Helpers.Data.Column.SelectFrequency])));
					}
					break;
				case Ferda.Modules.Helpers.Data.Column.SimpleTypeEnum.Integral:
					for (int i = fromUi; i < (frequenciesCount - toUi); i++)
					{
                        item = frequencies.Rows[i][Ferda.Modules.Helpers.Data.Column.SelectDistincts];
						if (item == System.DBNull.Value)
							continue;
                        dataArray.Add(new EquifrequencyIntervalGenerator.Data(Convert.ToInt64(item), Convert.ToInt32(frequencies.Rows[i][Ferda.Modules.Helpers.Data.Column.SelectFrequency])));
					}
					break;
				default:
					throw Ferda.Modules.Exceptions.BadParamsError(null, boxIdentity, simpleColumnType.ToString(), restrictionTypeEnum.DbColumnDataType);
			}
			object[] splitPoints = null;
			try
			{
				splitPoints = EquifrequencyIntervalGenerator.GenerateIntervals((int)countOfCategories, dataArray);
			}
			catch (ArgumentOutOfRangeException ex)
			{
				if (ex.ParamName == "intervals")
				{
					throw Ferda.Modules.Exceptions.BadValueError(ex, boxIdentity, null, new string[] { "CountOfCategories" }, restrictionTypeEnum.Other);
				}
				else
					throw ex;
			}
			if (splitPoints == null)
				return new GeneratedAttribute();

			CategoriesStruct categoriesStruct = new CategoriesStruct();

			string categoryName;
			List<SelectString> categoriesNames = new List<SelectString>();
			SelectString categoriesNamesItem;
			bool computeCategoriesNames = (splitPoints.Length + 1 < Ferda.Modules.Boxes.DataMiningCommon.Attributes.AbstractAttributeConstants.MaxLengthOfCategoriesNamesSelectStringArray);
			switch (simpleColumnType)
			{
				case Ferda.Modules.Helpers.Data.Column.SimpleTypeEnum.Floating:
					categoriesStruct.floatIntervals = new FloatIntervalCategorySeq();
					float beginValueFl = startValue;
					float nextValueFl;
					FloatIntervalStruct intervalFlTemplate = new FloatIntervalStruct();
					intervalFlTemplate.leftBoundType = BoundaryEnum.Sharp;
					intervalFlTemplate.rightBoundType = BoundaryEnum.Round;
					FloatIntervalStruct intervalFl;
					for (int i = 0; i < splitPoints.Length + 1; i++)
					{
						if (i == splitPoints.Length)
						{
							nextValueFl = endValue;
							intervalFlTemplate.rightBoundType = BoundaryEnum.Sharp;
						}
						else
							nextValueFl = (float)splitPoints[i];
						intervalFl = intervalFlTemplate;
						intervalFl.leftBound = beginValueFl;
						intervalFl.rightBound = nextValueFl;
						categoryName = Constants.LeftClosedInterval + beginValueFl.ToString() + Constants.SeparatorInterval + nextValueFl.ToString();
						categoryName += (intervalFl.rightBoundType == BoundaryEnum.Sharp)? Constants.RightClosedInterval : Constants.RightOpenedInterval;

						if (computeCategoriesNames)
						{
							categoriesNamesItem = new SelectString();
							categoriesNamesItem.name = categoryName;
							categoriesNamesItem.label = categoryName;
							categoriesNames.Add(categoriesNamesItem);
						}

						categoriesStruct.floatIntervals.Add(
							categoryName,
							new FloatIntervalStruct[] { intervalFl });
						beginValueFl = nextValueFl;
					}

					return new GeneratedAttribute(
						categoriesStruct,
						null,
						categoriesStruct.floatIntervals.Count,
						categoriesNames.ToArray());
				case Ferda.Modules.Helpers.Data.Column.SimpleTypeEnum.Integral:
					categoriesStruct.longIntervals = new LongIntervalCategorySeq();
					long beginValueLn = (long)startValue;
					long nextValueLn;
					LongIntervalStruct intervalLnTemplate = new LongIntervalStruct();
					intervalLnTemplate.leftBoundType = BoundaryEnum.Sharp;
					intervalLnTemplate.rightBoundType = BoundaryEnum.Round;
					LongIntervalStruct intervalLn;
					for (int i = 0; i < splitPoints.Length + 1; i++)
					{
						if (i == splitPoints.Length)
						{
							nextValueLn = (long)endValue;
							intervalLnTemplate.rightBoundType = BoundaryEnum.Sharp;
						}
						else
							nextValueLn = (long)splitPoints[i];
						intervalLn = intervalLnTemplate;
						intervalLn.leftBound = beginValueLn;
						intervalLn.rightBound = nextValueLn;
						categoryName = Constants.LeftClosedInterval + beginValueLn.ToString() + Constants.SeparatorInterval + nextValueLn.ToString();
						categoryName += (intervalLn.rightBoundType == BoundaryEnum.Sharp)? Constants.RightClosedInterval : Constants.RightOpenedInterval;

						if (computeCategoriesNames)
						{
							categoriesNamesItem = new SelectString();
							categoriesNamesItem.name = categoryName;
							categoriesNamesItem.label = categoryName;
							categoriesNames.Add(categoriesNamesItem);
						}

						categoriesStruct.longIntervals.Add(
							categoryName,
							new LongIntervalStruct[] { intervalLn });
						beginValueLn = nextValueLn;
					}

					return new GeneratedAttribute(
						categoriesStruct,
						null,
						categoriesStruct.longIntervals.Count,
						categoriesNames.ToArray());
				default:
					throw Ferda.Modules.Exceptions.BadParamsError(null, boxIdentity, simpleColumnType.ToString(), restrictionTypeEnum.DbColumnDataType);
			}
		}
	}
}
