using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Modules;

namespace Ferda.Modules.Quantifiers
{
	/// <summary>
    /// Represents specific kind of <see cref="T:Ferda.Modules.Quantifiers.ContingencyTable"/>
    /// which has only one dimension i. e. shape of the contingency is 
    /// (one row) x (<c>n</c> columns).
	/// </summary>
    /// <remarks>
    /// Please notice, that <see cref="P:Ferda.Modules.Quantifiers.OneDimensionalContingencyTable.NumericValues"/> property
    /// is must be set for computation fo some one-dimensional quantifiers.
    /// </remarks>
    public class OneDimensionalContingencyTable : ContingencyTable
	{
		#region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="OneDimensionalContingencyTable"/> class.
        /// </summary>
		public OneDimensionalContingencyTable()
			: base()
		{
			if (!this.IsOneDimensional)
                throw Ferda.Modules.Exceptions.BadParamsError(null, null, "Contingecy table has to be onedimensional!", restrictionTypeEnum.BadFormat);
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="OneDimensionalContingencyTable"/> class.
        /// </summary>
        /// <param name="contingencyTable">The contingency table.</param>
        /// <remarks>
        /// For futher information about <c>contingencyTable</c> param please see
        /// <see cref="P:Ferda.Modules.Quantifiers.ContingencyTable.Table"/>.
        /// </remarks>
        public OneDimensionalContingencyTable(int[][] contingencyTable)
			: base(contingencyTable)
		{
			if (!this.IsOneDimensional)
                throw Ferda.Modules.Exceptions.BadParamsError(null, null, "Contingecy table has to be onedimensional!", restrictionTypeEnum.BadFormat);
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="OneDimensionalContingencyTable"/> class.
        /// </summary>
        /// <param name="contingencyTable">The contingency table.</param>
        /// <remarks>
        /// For futher information about <c>contingencyTable</c> param please see
        /// <see cref="P:Ferda.Modules.Quantifiers.ContingencyTable.Table"/>.
        /// </remarks>
		public OneDimensionalContingencyTable(long[][] contingencyTable)
			: base(contingencyTable)
		{
			if (!this.IsOneDimensional)
                throw Ferda.Modules.Exceptions.BadParamsError(null, null, "Contingecy table has to be onedimensional!", restrictionTypeEnum.BadFormat);
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="OneDimensionalContingencyTable"/> class.
        /// </summary>
        /// <param name="contingencyTable">The contingency table.</param>
        /// <remarks>
        /// For futher information about <c>contingencyTable</c> param please see
        /// <see cref="P:Ferda.Modules.Quantifiers.ContingencyTable.Table"/>.
        /// </remarks>
		public OneDimensionalContingencyTable(long[,] contingencyTable)
			: base(contingencyTable)
		{
			if (!this.IsOneDimensional)
                throw Ferda.Modules.Exceptions.BadParamsError(null, null, "Contingecy table has to be onedimensional!", restrictionTypeEnum.BadFormat);
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="OneDimensionalContingencyTable"/> class.
        /// </summary>
        /// <param name="contingencyTable">The contingency table.</param>
        /// <param name="denominator">The denominator.</param>
        /// <remarks>
        /// For futher information about <c>contingencyTable</c> param please see
        /// <see cref="P:Ferda.Modules.Quantifiers.ContingencyTable.Table"/>.
        /// </remarks>
		public OneDimensionalContingencyTable(long[,] contingencyTable, long denominator)
			: base(contingencyTable, denominator)
		{
			if (!this.IsOneDimensional)
                throw Ferda.Modules.Exceptions.BadParamsError(null, null, "Contingecy table has to be onedimensional!", restrictionTypeEnum.BadFormat);
		}
		#endregion

		private double[] numericValues;
        /// <summary>
        /// Gets or sets the numeric values. 
        /// Numerical values are needed for computation of some 
        /// specific quantifier values (e. g. average value needs 
        /// numerical values (from this property) and its 
        /// frequencies (from contingency table)).
        /// </summary>
        /// <value>The numeric values.</value>
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
                    result += Table[rowIndex, columnIndex] * numericValues[columnIndex];
				}
                return result / (double)Denominator;
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
                    result += Table[rowIndex, columnIndex] * Math.Pow(numericValues[columnIndex] - arithmeticAverage, 3);
				}
                return result / Math.Pow(StandardDeviation, 3) * (double)Denominator;
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
                        numberOfValuesGreaterThanAverage += Table[rowIndex, columnIndex];
					else if (numericValues[columnIndex] < arithmeticAverage)
                        numberOfValuesLessThanAverage += Table[rowIndex, columnIndex];
                    numberOfAllRecords += Table[rowIndex, columnIndex];
				}
				if (numberOfAllRecords == 0)
					return 0;
                return (numberOfValuesGreaterThanAverage - numberOfValuesLessThanAverage) / (numberOfAllRecords * (double)Denominator);
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
                    cumulativeFrequency += Table[rowIndex, columnIndex] / (double)Denominator;
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
                        result += Table[rowIndex, columnIndex] * Math.Log(numericValues[columnIndex]);
					}
					catch (Exception ex)
					{
						if (numericValues[columnIndex] <= 0)
							throw Ferda.Modules.Exceptions.BadParamsError(ex, null, "Numeric values may not be less than or equal to zero.", restrictionTypeEnum.Minimum);
						else
							throw ex;
					}
				}
                return result / (double)Denominator;
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
                    result += Math.Pow(Table[rowIndex, columnIndex], 2);
				}
                return 1 - (result / (double)Denominator);
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
                    result += Table[rowIndex, columnIndex] * Math.Pow(numericValues[columnIndex], 2);
				}
                return (result / (double)Denominator) - (lastColumnIndex - firstColumnIndex + 1) * Math.Pow(ArithmeticAverage, 2);
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
				return 1 - this.MaxValue;
			}
		}
	}
}