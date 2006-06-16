using System;
using Ferda.Guha.Data;

namespace Ferda.Guha.Math.Quantifiers
{

    #region Base treshold (support)

    /// <summary>
    /// Computes the <c>base</c> or <c>support</c> in dependence on units (absolute/relative).
    /// </summary>
    /// <remarks>
    /// The base is defined as <c>a</c>. Support is defined as "relative base".
    /// </remarks>
    public class Base : IQuantifierValue
    {
        #region IQuantifier Members

        public double Value(IQuantifierValueData data)
        {
            FourFoldContingencyTable table = data.FourFoldContingencyTable;

            return table.Adiv;
        }

        #endregion

        #region IQuantifierTypeProperties Members

        public double MinValue
        {
            get { return 0.0d; }
        }

        public double MaxValue
        {
            get { return Double.PositiveInfinity; }
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

    #region Implications

    /// <summary>
    /// Computes the <c>confidence</c>.
    /// </summary>
    /// <returns>
    /// The confidence defined as <c>a / (a + b)</c>.
    /// If (a + b) = 0, returns NaN.
    /// </returns>
    public class FoundedImplication : IQuantifierValue
    {
        #region IQuantifier Members

        public double Value(IQuantifierValueData data)
        {
            FourFoldContingencyTable table = data.FourFoldContingencyTable;

            double r = table.R;
            return (r > 0) ? (table.A) / r : Double.NaN;
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
            get { return new QuantifierClassEnum[] { QuantifierClassEnum.Implicational }; }
        }

        public bool IrrelevantOnUnits
        {
            get { return true; }
        }

        public bool SupportsFloatContingencyTable
        {
            get { return true; }
        }

        #endregion
    }

    /// <summary>
    /// Computes lower critical implication.
    /// </summary>
    /// <returns>
    /// <para>
    /// Lower critical implication quantifier defined as a condition 
    /// <b>Sum[i = a..r] r! / (i! * (r - i)!) * p^i * (1 - p)^(r - i) &lt;= alpha</b>.
    /// </para>
    /// <para>
    /// See chapter 4.4.9 and 4.4.12 in GUHA-book 
    /// (likely p-implication quantifier).
    /// </para>
    /// </returns>
    public class LowerCriticalImplication : IQuantifierValidate
    {
        // P: Statistical confidence parameter.
        // The parameter p must be between 0 and 1 (inclusive).
        
        // (Treshold)
        // Alpha: Statistical significance (usually 5%).
        // The parameter alpha must be greater than 0.0 and less then or equal to 0.5.

        #region IQuantifierValidate Members

        // computes a value of LCI sum minus alpha
        private double QuantifierHelper(double p, params object[] hiddenParams)
        {
            FourFoldContingencyTable table = (FourFoldContingencyTable)hiddenParams[0];
            double alpha = (double)hiddenParams[1];
            if (p <= 0.0f) return (-alpha);
            if (p >= 1.0f) return (1.0f - alpha);
            double a = table.Adiv;
            double r = table.Rdiv;
            double sum = 0.0;
            for (double i = a; i <= r; i++)
            {
                sum += System.Math.Exp(
                    Combinatorics.LogFactorial(r)
                    - Combinatorics.LogFactorial(i)
                    - Combinatorics.LogFactorial(r - i)
                    + i * System.Math.Log(p)
                    + (r - i) * System.Math.Log(1.0 - p)
                    );
            }
            return ((float)sum - alpha);
        }

        public bool Validate(IQuantifierValidateData data, out double value)
        {
            //TODO Denominator
            
            // get params
            FourFoldContingencyTable table = data.FourFoldContingencyTable;

            double p = (double)(data.GetParam("p"));
            if ((p < 0.0f) || (p > 1.0f))
                throw new ArgumentOutOfRangeException("The parameter p must be between 0 and 1 (inclusive).");

            double alpha = data.Treshold;

            // validate
            bool valid = Common.Compare(
                data.Relation, // default <=
                QuantifierHelper(p, table, alpha),
                0.0d
                );
            if (!valid)
            {
                value = Double.NaN;
                return false;
            }

            // compute value


            value = Double.NaN;
            return false;
        }

        #endregion

        #region IQuantifierTypeProperties Members

        public double MinValue
        {
            get { return 0.0d; }
        }

        public double MaxValue
        {
            get { return 0.5d; }
        }

        public PerformanceDifficultyEnum QuantifierPerformanceDifficulty
        {
            get { return PerformanceDifficultyEnum.Difficult; }
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
            get { return new QuantifierClassEnum[] { QuantifierClassEnum.Equivalency }; }
        }

        public bool IrrelevantOnUnits
        {
            get { return false; }
        }

        public bool SupportsFloatContingencyTable
        {
            get { return false; }
        }

        #endregion
    }
    
    //TODO
    //public class UpperCriticalImplcation : IQuantifierValidate{ }

    #endregion

    #region Double Implications

    /// <summary>
    /// Computes the <c>strength</c> of double founded implication.
    /// </summary>
    /// <returns>
    /// The strength defined as <c>a / (a + b + c)</c>.
    /// If (a + b + c) = 0, returns NaN.
    /// </returns>
    public class DoubleFoundedImplication : IQuantifierValue
    {
        #region IQuantifier Members

        public double Value(IQuantifierValueData data)
        {
            FourFoldContingencyTable table = data.FourFoldContingencyTable;

            double a = table.A;
            double sum = a + table.B + table.C;
            return (sum > 0) ? (a / sum) : Double.NaN;
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
            get { return new QuantifierClassEnum[] { QuantifierClassEnum.SigmaDoubleImplicational }; }
        }

        public bool IrrelevantOnUnits
        {
            get { return true; }
        }

        public bool SupportsFloatContingencyTable
        {
            get { return true; }
        }

        #endregion
    }

    
    //TODO
    //public class DoubleLowerCriticalImplcation : IQuantifierValidate{ }
    //public class DoubleUpperCriticalImplcation : IQuantifierValidate{ }

    #endregion

    #region Equivalence

    /// <summary>
    /// Computes the <c>strength</c> of founded equivalence.
    /// </summary>
    /// <returns>
    /// The strength defined as <c>(a + d) / (a + b + c + d)</c>.
    /// If (a + b + c + d) = 0, returns NaN.
    /// </returns>
    public class FoundedEquivalence : IQuantifierValue
    {
        #region IQuantifier Members

        public double Value(IQuantifierValueData data)
        {
            FourFoldContingencyTable table = data.FourFoldContingencyTable;

            double n = table.N;
            return (n > 0) ? (table.A + table.D) / n : Double.NaN;
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
            get { return new QuantifierClassEnum[] { QuantifierClassEnum.SigmaEquivalency }; }
        }

        public bool IrrelevantOnUnits
        {
            get { return true; }
        }

        public bool SupportsFloatContingencyTable
        {
            get { return true; }
        }

        #endregion
    }

    //TODO
    //public class LowerCriticalEquivalence : IQuantifierValidate{ }
    //public class UpperCriticalEquivalence : IQuantifierValidate{ }

    #endregion

    #region Distribution significance

    //TODO
    public class ChiSquare : IQuantifierValidate
    {
        #region IQuantifier Members

        public bool Validate(IQuantifierValidateData data, out double value)
        {
            throw new Exception("The method or operation is not implemented.");
            // TODO denominator!
            /*
            ContingencyTable table = data.ContingencyTable;

            // dle 1KL_Minr.pdf

            // radkove a sloupcove soucty
            System.Collections.Generic.List<double> rowSums = new List<double>(table.NumberOfRows);
            System.Collections.Generic.List<double> columnSums = new List<double>(table.NumberOfColumns);
            for (int r = 0; r < table.NumberOfRows; r++)
                rowSums.Insert(r, 0);
            for (int c = 0; c < table.NumberOfColumns; c++)
                columnSums.Insert(c, 0);
            double sum = 0;
            for (int r = 0; r < table.NumberOfRows; r++)
            {
                for (int c = 0; c < table.NumberOfColumns; c++)
                {
                    rowSums[r] += table.ContingecyTable[r, c];
                    columnSums[c] += table.ContingecyTable[r, c];
                }
                sum += rowSums[r];
            }

            // dle 1KL_Minr.pdf
            double x;
            double chisquare = 0;
            for (int r = 0; r < table.NumberOfRows; r++)
            {
                for (int c = 0; c < table.NumberOfColumns; c++)
                {
                    x = rowSums[r] * columnSums[c] / sum;
                    chisquare += System.Math.Pow(table.ContingecyTable[r, c] - x, 2) / x;
                }
            }

            //http://schnoodles.com/cgi-bin/web_chi.cgi
            int degreesOfFreedom = (table.NumberOfRows - 1) * (table.NumberOfColumns - 1);

            // special functions 
            //public static double chisq(double df, double x)
            //{
            //    if (x < 0.0 || df < 1.0) return 0.0;
            //    return igam(df / 2.0, x / 2.0);
            //}

            double result = SpecialFunction.chisq(degreesOfFreedom, chisquare);

            return result;
             */
        }

        #endregion

        #region IQuantifierTypeProperties Members

        public double MinValue
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public double MaxValue
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public PerformanceDifficultyEnum QuantifierPerformanceDifficulty
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public bool NeedsNumericValues
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public CardinalityEnum SupportedData
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public QuantifierClassEnum[] QuantifierClasses
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public bool IrrelevantOnUnits
        {
            get { return false; }
        }

        public bool SupportsFloatContingencyTable
        {
            get { return false; } //TODO ??
        }

        #endregion
    }

    /// <summary>
    /// Computes the simple deviation strength.
    /// </summary>
    /// <returns>
    /// <para>Simple deviation strength result defined as <c>ln(ad/bc) / ln(2)</c>.</para>
    /// <para>There are special cases defined explicitly:</para>
    /// <para>If both <c>(a * d) = 0</c> and <c>(b * c) = 0</c>, return 0.</para>
    /// <para>If only <c>(a * d) = 0</c>, return -INF.</para>
    /// <para>If only <c>(b * c) = 0</c>, return +INF.</para>
    /// </returns>
    public class SimpleDeviation : IQuantifierValue
    {
        #region IQuantifier Members

        public double Value(IQuantifierValueData data)
        {
            FourFoldContingencyTable table = data.FourFoldContingencyTable;

            double ad = table.A * table.D;
            double bc = table.B * table.C;
            if ((ad == 0) && (bc == 0))
                return 0.0D;
            if (ad == 0)
                return Double.NegativeInfinity;
            if (bc == 0)
                return Double.PositiveInfinity;
            return (System.Math.Log(ad / bc) / 0.693147180559945309417);
        }

        #endregion

        #region IQuantifierTypeProperties Members

        public double MinValue
        {
            get { return Double.NegativeInfinity; }
        }

        public double MaxValue
        {
            get { return Double.PositiveInfinity; }
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
            get { return true; }
        }

        public bool SupportsFloatContingencyTable
        {
            get { return true; }
        }

        #endregion
    }

    //TODO fisher
    //public class Fisher : ???
    
    #endregion

    #region Above/Below/Outside average

    /// <summary>
    /// Computes the above/below average difference.
    /// </summary>
    /// <returns>
    /// <para>
    /// This quiantifier computes value <c>(a / (a + b)) / ((a + c) / (a + b + c + d))</c>
    /// i.e. <c>(a * (a + b + c + d)) / ((a + b) * (a + c))</c> and its semantics depends
    /// on specified <c>Relation</c> and <c>Treshold</c> where 0 &lt; Treshold &lt; +INF. 
    /// </para>
    /// <para>
    /// E.g. if Relation is &gt;= and Treshold is 1.1 than contingency table 
    /// satisfies Above Average condition with p = 0.1. 
    /// Or if Relation is &lt;= and Treshold is 0.8 than contingency table 
    /// satisfies Below Average condition with p = 0.2.
    /// </para>
    /// <para>
    /// If (a + c) = 0, returns NaN. (explanation: the succedent does 
    /// not exist at all, so its occurence is neither above nor below 
    /// average when antecedent holds true); 
    /// </para>
    /// <para>
    /// If a = 0, returns -INF (explanation: the succedent exists only 
    /// when antecedent is not true, so its occurence is +INF-times smaller 
    /// than in average, i.e. the below-average-part should be always true)
    /// </para>
    /// </returns>
    /// <remarks>
    /// <para>
    /// Above average difference is defined as 
    /// <c>(a / (a + b)) &gt;= (1 + p) ((a + c) / (a + b + c + d))</c> where
    /// <c>0 &lt; p</c>
    /// It says that P(succ|ant) is p-times greather than P(succ)
    /// i.e. P(succ|ant)/P(succ) >= 1+p.
    /// </para>
    /// <para>
    /// Below average difference is defined as 
    /// <c>(a / (a + b)) &lt;= (1 - p) ((a + c) / (a + b + c + d))</c> where
    /// <c>0 &lt; p &lt; 1</c>
    /// It says that P(succ|ant) is p-times lesser than P(succ)
    /// i.e. P(succ|ant)/P(succ) >= 1-p.
    /// </para>
    /// <para>
    /// Outside average difference is defined as 
    /// <c>(a / (a + b)) &lt;= (1 - p) ((a + c) / (a + b + c + d))</c> where
    /// <c>0 &lt; p &lt; 1</c>
    /// It says that P(succ|ant) is p-times lesser than P(succ)
    /// i.e. P(succ|ant)/P(succ) >= 1-p.
    /// </para>
    /// <para>
    /// There can be defined outside average also.<br />
    /// Outside average strength value defined as maximum of two values:<br />
    /// <c>(a / (a + b)) * ((a + b + c + d) / (a + c))</c> and<br />
    /// <c>((a + b) / a) * ((a + c) / (a + b + c + d))</c>.
    /// This can be simulated by two calls of this quantifier with 
    /// corresponding setting.
    /// </para>
    /// </remarks>
    public class AboveBelowAverageImplication : IQuantifierValue
    {
        #region IQuantifier Members

        public double Value(IQuantifierValueData data)
        {
            FourFoldContingencyTable table = data.FourFoldContingencyTable;

            if ((table.A + table.C) == 0)
                return Double.NaN;
            else if (table.A == 0)
                return Double.NegativeInfinity;
            else
                return (table.A * table.N) / (table.R * table.K);
        }

        #endregion

        #region IQuantifierTypeProperties Members

        public double MinValue
        {
            get { return 0.0d; }
        }

        public double MaxValue
        {
            get { return Double.PositiveInfinity; }
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
            get { return true; }
        }

        public bool SupportsFloatContingencyTable
        {
            get { return true; }
        }

        #endregion
    }

    #endregion

    #region Others

    /// <summary>
    /// Computes the <c>strenght</c> of E-Quantifier
    /// </summary>
    /// <returns>
    /// The strength defined as <c>b / (a + b) ~ delta</c> and 
    /// <c>c / (c + d) ~ delta</c> where <c>delta</c> is treshold 
    /// and <c>~</c> is relation (usually &lt;=).
    /// If (a + b) = 0 or (c + d) = 0, returns false.
    /// </returns>
    public class E : IQuantifierValid
    {
        #region IQuantifierValid Members

        public bool Validate(IQuantifierValidateData data)
        {
            FourFoldContingencyTable table = data.FourFoldContingencyTable;

            double ab = table.A + table.B;
            double cd = table.C + table.D;
            if (ab * cd == 0)
                return false; //NaN
            else
            {
                return Common.Compare(data.Relation, table.B / ab, data.Treshold)
                       && Common.Compare(data.Relation, table.C / cd, data.Treshold);
            }
        }

        #endregion

        #region IQuantifierTypeProperties Members

        public double MinValue
        {
            get { return 0; }
        }

        public double MaxValue
        {
            get { return 1; }
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
            get { return true; }
        }

        public bool SupportsFloatContingencyTable
        {
            get { return true; }
        }

        #endregion
    }

    #endregion
}