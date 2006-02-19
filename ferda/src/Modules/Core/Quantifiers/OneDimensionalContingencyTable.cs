using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Modules;

namespace Ferda.Modules.Quantifiers
{
	public class OneDimensionalContingencyTable : ContingencyTable
	{
		#region Constructors
		public OneDimensionalContingencyTable()
			: base()
		{
			if (!this.IsOneDimensional)
                throw Ferda.Modules.Exceptions.BadParamsError(null, null, "Contingecy table has to be onedimensional!", restrictionTypeEnum.BadFormat);
		}
		public OneDimensionalContingencyTable(int[][] contingencyTable)
			: base(contingencyTable)
		{
			if (!this.IsOneDimensional)
                throw Ferda.Modules.Exceptions.BadParamsError(null, null, "Contingecy table has to be onedimensional!", restrictionTypeEnum.BadFormat);
		}
		public OneDimensionalContingencyTable(long[][] contingencyTable)
			: base(contingencyTable)
		{
			if (!this.IsOneDimensional)
                throw Ferda.Modules.Exceptions.BadParamsError(null, null, "Contingecy table has to be onedimensional!", restrictionTypeEnum.BadFormat);
		}
		public OneDimensionalContingencyTable(long[,] contingencyTable)
			: base(contingencyTable)
		{
			if (!this.IsOneDimensional)
                throw Ferda.Modules.Exceptions.BadParamsError(null, null, "Contingecy table has to be onedimensional!", restrictionTypeEnum.BadFormat);
		}
		public OneDimensionalContingencyTable(long[,] contingencyTable, long denominator)
			: base(contingencyTable, denominator)
		{
			if (!this.IsOneDimensional)
                throw Ferda.Modules.Exceptions.BadParamsError(null, null, "Contingecy table has to be onedimensional!", restrictionTypeEnum.BadFormat);
		}
		#endregion

		private double[] numericValues;
		public double[] NumericValues
		{
			get
			{
				if (numericValues == null)
					throw new Exception("Numeric values were not entered.");
				return numericValues;
			}

			set
			{
				if (value.Length != this.MaxColumnIndex + 1)
                    throw Ferda.Modules.Exceptions.BadParamsError(null, null, "Length of onedimensional contingency table is different from length of numericValues array", restrictionTypeEnum.BadFormat);
				this.numericValues = value;
			}
		}

		/// <summary>
		/// See document 093 CF a SDCF Kvantifikátory.doc
		/// </summary>
		public double ArithmeticAverage
		{
			get
			{
				double result = 0;

				int rowIndex = FirstRowIndex;
				int firstColumnIndex = FirstColumnIndex;
				int lastColumnIndex = LastColumnIndex;
				for (int columnIndex = firstColumnIndex; columnIndex <= lastColumnIndex; columnIndex++)
				{
					result += table[rowIndex, columnIndex] * numericValues[columnIndex];
				}
				return result / (double)denominator;
			}
		}

		/// <summary>
		/// See document 093 CF a SDCF Kvantifikátory.doc
		/// ("koeficient asymetrie kolem stredu rozlození")
		/// In statistic literature mentioned as skewness ("šikmost").
		/// </summary>
		public double Skewness
		{
			get
			{
				double arithmeticAverage = ArithmeticAverage;
				double result = 0;

				int rowIndex = FirstRowIndex;
				int firstColumnIndex = FirstColumnIndex;
				int lastColumnIndex = LastColumnIndex;
				for (int columnIndex = firstColumnIndex; columnIndex <= lastColumnIndex; columnIndex++)
				{
					result += table[rowIndex, columnIndex] * Math.Pow(numericValues[columnIndex] - arithmeticAverage, 3);
				}
				return result / Math.Pow(StandardDeviation, 3) * (double)denominator;
			}
		}

		/// <summary>
		/// See document 093 CF a SDCF Kvantifikátory.doc
		/// </summary>
		public double Asymentry
		{
			get
			{
				//result = (NG - NL) / N
				long numberOfAllRecords = 0;
				long numberOfValuesLessThanAverage = 0;
				long numberOfValuesGreaterThanAverage = 0;
				double arithmeticAverage = ArithmeticAverage;

				int rowIndex = FirstRowIndex;
				int firstColumnIndex = FirstColumnIndex;
				int lastColumnIndex = LastColumnIndex;
				for (int columnIndex = firstColumnIndex; columnIndex <= lastColumnIndex; columnIndex++)
				{
                    if (numericValues[columnIndex] > arithmeticAverage)
						numberOfValuesGreaterThanAverage += table[rowIndex, columnIndex];
					else if (numericValues[columnIndex] < arithmeticAverage)
						numberOfValuesLessThanAverage += table[rowIndex, columnIndex];
					numberOfAllRecords += table[rowIndex, columnIndex];
				}
				if (numberOfAllRecords == 0)
					return 0;
				return (numberOfValuesGreaterThanAverage - numberOfValuesLessThanAverage) / (numberOfAllRecords * (double)denominator);
			}
		}

		/// <summary>
		/// See document 093 CF a SDCF Kvantifikátory.doc
		/// </summary>
		/// <remarks>Only for ordinal attributes.</remarks>
		public double DiscreteOrdinaryVariation
		{
			get
			{
				double result = 0;

				int rowIndex = FirstRowIndex;
				int firstColumnIndex = FirstColumnIndex;
				int lastColumnIndex = LastColumnIndex;

				double cumulativeFrequency = 0;
				for (int columnIndex = firstColumnIndex; columnIndex <= lastColumnIndex; columnIndex++)
				{
					cumulativeFrequency += table[rowIndex, columnIndex] / (double)denominator;
					result += cumulativeFrequency * (1 - cumulativeFrequency);
				}
				return 2 * result;
			}
		}

		/// <summary>
		/// See document 093 CF a SDCF Kvantifikátory.doc
		/// </summary>
		/// <remarks>
		/// Numeric values may not be less than or equal to zero.
		/// </remarks>
		public double GeometricAverage
		{
			get
			{
				double result = 0;

				int rowIndex = FirstRowIndex;
				int firstColumnIndex = FirstColumnIndex;
				int lastColumnIndex = LastColumnIndex;
				for (int columnIndex = firstColumnIndex; columnIndex <= lastColumnIndex; columnIndex++)
				{
					try
					{
						result += table[rowIndex, columnIndex] * Math.Log(numericValues[columnIndex]);
					}
					catch (Exception ex)
					{
						if (numericValues[columnIndex] <= 0)
							throw Ferda.Modules.Exceptions.BadParamsError(ex, null, "Numeric values may not be less than or equal to zero.", restrictionTypeEnum.Minimum);
						else
							throw ex;
					}
				}
				return result / (double)denominator;
			}
		}

		/// <summary>
		/// See document 093 CF a SDCF Kvantifikátory.doc
		/// </summary>
		public double NominalVariation
		{
			get
			{
				double result = 0;

				int rowIndex = FirstRowIndex;
				int firstColumnIndex = FirstColumnIndex;
				int lastColumnIndex = LastColumnIndex;

				for (int columnIndex = firstColumnIndex; columnIndex <= lastColumnIndex; columnIndex++)
				{
					result += Math.Pow(table[rowIndex, columnIndex], 2);
				}
				return 1 - (result / (double)denominator);
			}
		}

		/// <summary>
		/// See document 093 CF a SDCF Kvantifikátory.doc
		/// </summary>
		public double StandardDeviation
		{
			get
			{
				return Math.Sqrt(Variance);
			}
		}

		/// <summary>
		/// See document 093 CF a SDCF Kvantifikátory.doc
		/// </summary>
		public double Variance // Rozptyl
		{
			get
			{
				double result = 0;

				int rowIndex = FirstRowIndex;
				int firstColumnIndex = FirstColumnIndex;
				int lastColumnIndex = LastColumnIndex;
				for (int columnIndex = firstColumnIndex; columnIndex <= lastColumnIndex; columnIndex++)
				{
					result += table[rowIndex, columnIndex] * Math.Pow(numericValues[columnIndex], 2);
				}
				return (result / (double)denominator) - (lastColumnIndex - firstColumnIndex + 1) * Math.Pow(ArithmeticAverage, 2);
			}
		}

		/// <summary>
		/// See document 093 CF a SDCF Kvantifikátory.doc
		/// </summary>
		/// <remarks>1 - Modal frequency. Modal frequency is maximal frequency of contingecy table.</remarks>
		public double VariationRatio //Variační poměr
		{
			get
			{
				return 1 - this.MaxValueAggregation;
			}
		}
	}
}