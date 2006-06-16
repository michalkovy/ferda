using System;
using Ferda.Guha.Data;

// See document 093 CF a SDCF Kvantifikátory.doc
namespace Ferda.Guha.Math.Quantifiers
{

    #region Characteristics of nominal variable distribution

    /// <summary>
    /// <para>
    /// Normalized Nominal Variation is defined as 
    /// <c>K * NominalVariation / (K-1)</c> (where K = number of distinct values)
    /// and it takes values from interval [0,1].
    /// Value 0 means that:
    /// I. whole explored set is concentrated in only one value or
    /// II. distribution of frequencies is uniform i.e. <c>Freq i = 1/K</c>.
    /// </para>
    /// <para>
    /// <c>NominalVariation = 1 - Sum(i)((Freq i)^2) 
    /// = Sum(i)((Freq i) * (1 - Freq i))</c>
    /// </para>
    /// </summary>
    public class NominalVariationNormalized : IQuantifierValue
    {
        #region IQuantifierValue Members

        public double Value(IQuantifierValueData data)
        {
            SingleDimensionContingecyTable table = data.SingleDimensionContingecyTable;

            double sumFreq_2 = 0;
            int k = table.NumberOfColumns;
            if (k == 1)
                return 0;

            table.ForEach(delegate(double v)
                              {
                                  sumFreq_2 += System.Math.Pow(v, 2);
                              }
                );

            double nominalVariation = 1 - (sumFreq_2 / System.Math.Pow(table.Denominator, 2));
            return k * nominalVariation / (k - 1);
        }

        #endregion

        #region IQuantifierTypeProperties Members

        public double MinValue
        {
            get { return 0.0d; }
        }

        public double MaxValue
        {
            get { return 1.0d; }
        }

        public PerformanceDifficultyEnum QuantifierPerformanceDifficulty
        {
            get { return PerformanceDifficultyEnum.QuiteEasy; }
        }

        public bool NeedsNumericValues
        {
            get { return false; }
        }

        public CardinalityEnum SupportedData
        {
            get { return CardinalityEnum.Nominal; }
        }

        public QuantifierClassEnum[] QuantifierClasses
        {
            get { return new QuantifierClassEnum[] { }; }
        }

        public bool IrrelevantOnUnits
        {
            get { return false; }
        }

        public bool SupportsFloatContingencyTable
        {
            get { return true; }
        }

        #endregion
    }

    /// <summary>
    /// Variation ratio is defined as <c>1 - Max(i)(Freq i)</c>.
    /// </summary>
    public class VariationRatio : IQuantifierValue
    {
        #region IQuantifierValue Members

        public double Value(IQuantifierValueData data)
        {
            SingleDimensionContingecyTable table = data.SingleDimensionContingecyTable;

            return 1 - (table.Max / table.Denominator);
        }

        #endregion

        #region IQuantifierTypeProperties Members

        public double MinValue
        {
            get { return 0.0d; }
        }

        public double MaxValue
        {
            get { return 1.0d; }
        }

        public PerformanceDifficultyEnum QuantifierPerformanceDifficulty
        {
            get { return PerformanceDifficultyEnum.Easy; }
        }

        public bool NeedsNumericValues
        {
            get { return false; }
        }

        public CardinalityEnum SupportedData
        {
            get { return CardinalityEnum.Nominal; }
        }

        public QuantifierClassEnum[] QuantifierClasses
        {
            get { return new QuantifierClassEnum[] { }; }
        }

        public bool IrrelevantOnUnits
        {
            get { return false; }
        }

        public bool SupportsFloatContingencyTable
        {
            get { return true; }
        }

        #endregion
    }

    #endregion

    #region Characteristics of ordinal variable distribution

    /// <summary>
    /// <para>
    /// Normalized Discrete Ordinary Variation is defined as 
    /// <c>2 * DiscreteOrdinaryVariation / (K-1)</c> (where K = number of distinct values)
    /// and it takes values from interval [0,1].
    /// Value 0 means that whole explored set is concentrated in only one value or.
    /// Value 0.5 means that 50% of objects are in a_1 and 50% of objects are in a_K.
    /// </para>
    /// <para>
    /// <c>DiscreteOrdinaryVariation = 2 * (1 - Sum(i)((Freq i)^2)) 
    /// = 2 * (Sum(i)((Freq i) * (1 - Freq i)))</c>.
    /// </para>
    /// </summary>
    public class DiscreteOrdinaryVariationNormalized : IQuantifierValue
    {
        #region IQuantifierValue Members

        public double Value(IQuantifierValueData data)
        {
            SingleDimensionContingecyTable table = data.SingleDimensionContingecyTable;

            double sumFreq_2 = 0;
            int k = table.NumberOfColumns;
            if (k == 1)
                return 0;

            table.ForEach(delegate(double v)
                              {
                                  sumFreq_2 += System.Math.Pow(v, 2);
                              }
                );

            double discreteOrdinaryVariation = 2 * (1 - (sumFreq_2 / System.Math.Pow(table.Denominator, 2)));
            return 2 * discreteOrdinaryVariation / (k - 1);
        }

        #endregion

        #region IQuantifierTypeProperties Members

        public double MinValue
        {
            get { return 0.0d; }
        }

        public double MaxValue
        {
            get { return 1.0d; }
        }

        public PerformanceDifficultyEnum QuantifierPerformanceDifficulty
        {
            get { return PerformanceDifficultyEnum.Easy; }
        }

        public bool NeedsNumericValues
        {
            get { return false; }
        }

        public CardinalityEnum SupportedData
        {
            get { return CardinalityEnum.Ordinal; }
        }

        public QuantifierClassEnum[] QuantifierClasses
        {
            get { return new QuantifierClassEnum[] { }; }
        }

        public bool IrrelevantOnUnits
        {
            get { return false; }
        }

        public bool SupportsFloatContingencyTable
        {
            get { return true; }
        }

        #endregion
    }

    #endregion

    #region Characteristics of cardinal variable distribution

    public static class SingleDimensionCardinalVariableDistribution
    {
        /// <summary>
        /// Aritmetic average is defined as <c>Sum(i)(Freq i * Value i)</c>
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static double AritmeticAverage(SingleDimensionContingecyTable table, double[] values)
        {
            double result = 0;
            for (int c = 0; c < table.NumberOfColumns; c++)
            {
                result += table[c] * values[c];
            }
            return result / table.Denominator;
        }

        /// <summary>
        /// Geometric average is defined as 
        /// <c>Mul(i)((Value i)^(Freq i)) = e^Sum(i)((Freq i) * Log(Value i))</c>
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static double GeometricAverage(SingleDimensionContingecyTable table, double[] values)
        {
            double pow = 0;
            for (int c = 0; c < table.NumberOfColumns; c++)
            {
                pow += table[c] * System.Math.Log(values[c]);
            }
            return System.Math.Pow(System.Math.E, pow / table.Denominator);
        }

        /// <summary>
        /// Variance is defined as 
        /// <c>Sum(i)( (Freq i) * ((Value i) - AritmeticAverage)^2 )</c>
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static double Variance(SingleDimensionContingecyTable table, double[] values)
        {
            double result = 0;
            double arithmeticAverage = AritmeticAverage(table, values);
            for (int c = 0; c < table.NumberOfColumns; c++)
            {
                result += table[c] * System.Math.Pow(values[c] - arithmeticAverage, 2);
            }
            return result / table.Denominator;
        }

        /// <summary>
        /// Standard deviation is defined as <c>Sqrt(Variance)</c>
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static double StandardDeviation(SingleDimensionContingecyTable table, double[] values)
        {
            return System.Math.Sqrt(Variance(table, values));
        }

        /// <summary>
        /// Asymetry is defined as 
        /// <c>(Sum(i)( (Freq i) * ((Value i) - AritmeticAverage)^3 )) / StandardDeviation^3</c>
        /// </summary>
        /// <remarks>
        /// Value = 0 means that distribution is symetrical along Arithmetic average;
        /// Value &gt; 0 means that predominates deviations to right side;
        /// Value &lt; 0 means that predominates deviations to left side.
        /// </remarks>
        /// <param name="table">The table.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static double Asymetry(SingleDimensionContingecyTable table, double[] values)
        {
            double varianceTmp = 0;
            double result = 0;
            double arithmeticAverage = AritmeticAverage(table, values);
            for (int c = 0; c < table.NumberOfColumns; c++)
            {
                double valueMinusArithmeticAvg = values[c] - arithmeticAverage;
                double varianceAdd = table[c] * System.Math.Pow(valueMinusArithmeticAvg, 2);
                varianceTmp += varianceAdd;
                result += varianceAdd * valueMinusArithmeticAvg;
            }
            double variance = varianceTmp / table.Denominator;
            return (result / table.Denominator) / System.Math.Pow(System.Math.Sqrt(variance), 3);
        }

        /// <summary>
        /// Skewness is defined as 
        /// <c>(N'' - N') / N</c> where 
        /// <c>N''</c> is number of values greater than arithmetic value;
        /// <c>N'</c> is number of values lower than arithmetic value;
        /// <c>N</c> is number of all objects.
        /// </summary>
        /// <remarks>
        /// Value = 0 means that distribution is symetrical along Arithmetic average;
        /// Value &gt; 0 means that predominates deviations to right side;
        /// Value &lt; 0 means that predominates deviations to left side.
        /// </remarks>
        /// <param name="table">The table.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static double Skewness(SingleDimensionContingecyTable table, double[] values)
        {
            double arithmeticAverage = AritmeticAverage(table, values);
            double N_ = 0; // N'
            double N__ = 0; // N''
            for (int c = 0; c < table.NumberOfColumns; c++)
            {
                if (values[c] < arithmeticAverage)
                    N_ += table[c];
                else if (values[c] > arithmeticAverage)
                    N__ += table[c];
            }
            return (N__ - N_);
            // not divided by table.Sum because only relative frequencies are expected
        }
    }

    #endregion
}