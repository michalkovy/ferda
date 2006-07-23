//using System;
//using Ferda.Guha.Data;

//namespace Ferda.Guha.Math.Quantifiers
//{
//    #region Implications

//    /// <summary>
//    /// Computes lower critical implication.
//    /// </summary>
//    /// <returns>
//    /// <para>
//    /// Lower critical implication quantifier defined as a condition 
//    /// <b>Sum[i = a..r] r! / (i! * (r - i)!) * p^i * (1 - p)^(r - i) &lt;= alpha</b>.
//    /// </para>
//    /// <para>
//    /// See chapter 4.4.9 and 4.4.12 in GUHA-book 
//    /// (likely p-implication quantifier).
//    /// </para>
//    /// </returns>
//    public class LowerCriticalImplication : IQuantifierValidate
//    {
//        // P: Statistical confidence parameter.
//        // The parameter p must be between 0 and 1 (inclusive).

//        // (Treshold)
//        // Alpha: Statistical significance (usually 5%).
//        // The parameter alpha must be greater than 0.0 and less then or equal to 0.5.

//        #region IQuantifierValidate Members

//        // computes a value of LCI sum minus alpha
//        private double QuantifierHelper(double p, params object[] hiddenParams)
//        {
//            FourFoldContingencyTable table = (FourFoldContingencyTable)hiddenParams[0];
//            double alpha = (double)hiddenParams[1];
//            if (p <= 0.0f) return (-alpha);
//            if (p >= 1.0f) return (1.0f - alpha);
//            double a = table.Adiv;
//            double r = table.Rdiv;
//            double sum = 0.0;
//            for (double i = a; i <= r; i++)
//            {
//                sum += System.Math.Exp(
//                    Combinatorics.LogFactorial(r)
//                    - Combinatorics.LogFactorial(i)
//                    - Combinatorics.LogFactorial(r - i)
//                    + i * System.Math.Log(p)
//                    + (r - i) * System.Math.Log(1.0 - p)
//                    );
//            }
//            return ((float)sum - alpha);
//        }

//        public bool Validate(IQuantifierValidateData data, out double value)
//        {
//            //TODO Denominator

//            // get params
//            FourFoldContingencyTable table = data.FourFoldContingencyTable;

//            double p = (double)(data.GetParam("p"));
//            if ((p < 0.0f) || (p > 1.0f))
//                throw new ArgumentOutOfRangeException("The parameter p must be between 0 and 1 (inclusive).");

//            double alpha = data.Treshold;

//            // validate
//            bool valid = Common.Compare(
//                data.Relation, // default <=
//                QuantifierHelper(p, table, alpha),
//                0.0d
//                );
//            if (!valid)
//            {
//                value = Double.NaN;
//                return false;
//            }

//            // compute value


//            value = Double.NaN;
//            return false;
//        }

//        #endregion

//        #region IQuantifierTypeProperties Members

//        public double MinValue
//        {
//            get { return 0.0d; }
//        }

//        public double MaxValue
//        {
//            get { return 0.5d; }
//        }

//        public PerformanceDifficultyEnum QuantifierPerformanceDifficulty
//        {
//            get { return PerformanceDifficultyEnum.Difficult; }
//        }

//        public bool NeedsNumericValues
//        {
//            get { return false; }
//        }

//        public CardinalityEnum SupportedData
//        {
//            get { return CardinalityEnum.Nominal; }
//        }

//        public QuantifierClassEnum[] QuantifierClasses
//        {
//            get { return new QuantifierClassEnum[] { QuantifierClassEnum.Equivalency }; }
//        }

//        public bool IrrelevantOnUnits
//        {
//            get { return false; }
//        }

//        public bool SupportsFloatContingencyTable
//        {
//            get { return false; }
//        }

//        #endregion
//    }

//    //TODO
//    //public class UpperCriticalImplication : IQuantifierValidate{ }

//    #endregion

//    #region Double Implications

//    //TODO
//    //public class DoubleLowerCriticalImplication : IQuantifierValidate{ }
//    //public class DoubleUpperCriticalImplication : IQuantifierValidate{ }

//    #endregion

//    #region Equivalence

//    //TODO
//    //public class LowerCriticalEquivalence : IQuantifierValidate{ }
//    //public class UpperCriticalEquivalence : IQuantifierValidate{ }

//    #endregion

//    #region Distribution significance

//    //TODO
//    public class ChiSquare : IQuantifierValidate
//    {
//        #region IQuantifier Members

//        public bool Validate(IQuantifierValidateData data, out double value)
//        {
//            throw new Exception("The method or operation is not implemented.");
//            // TODO denominator!
//            //ContingencyTable table = data.ContingencyTable;

//            //// dle 1KL_Minr.pdf

//            //// radkove a sloupcove soucty
//            //System.Collections.Generic.List<double> rowSums = new List<double>(table.NumberOfRows);
//            //System.Collections.Generic.List<double> columnSums = new List<double>(table.NumberOfColumns);
//            //for (int r = 0; r < table.NumberOfRows; r++)
//            //    rowSums.Insert(r, 0);
//            //for (int c = 0; c < table.NumberOfColumns; c++)
//            //    columnSums.Insert(c, 0);
//            //double sum = 0;
//            //for (int r = 0; r < table.NumberOfRows; r++)
//            //{
//            //    for (int c = 0; c < table.NumberOfColumns; c++)
//            //    {
//            //        rowSums[r] += table.ContingecyTable[r, c];
//            //        columnSums[c] += table.ContingecyTable[r, c];
//            //    }
//            //    sum += rowSums[r];
//            //}

//            //// dle 1KL_Minr.pdf
//            //double x;
//            //double chisquare = 0;
//            //for (int r = 0; r < table.NumberOfRows; r++)
//            //{
//            //    for (int c = 0; c < table.NumberOfColumns; c++)
//            //    {
//            //        x = rowSums[r] * columnSums[c] / sum;
//            //        chisquare += System.Math.Pow(table.ContingecyTable[r, c] - x, 2) / x;
//            //    }
//            //}

//            ////http://schnoodles.com/cgi-bin/web_chi.cgi
//            //int degreesOfFreedom = (table.NumberOfRows - 1) * (table.NumberOfColumns - 1);

//            //// special functions 
//            ////public static double chisq(double df, double x)
//            ////{
//            ////    if (x < 0.0 || df < 1.0) return 0.0;
//            ////    return igam(df / 2.0, x / 2.0);
//            ////}

//            //double result = SpecialFunction.chisq(degreesOfFreedom, chisquare);

//            //return result;
//        }

//        #endregion

//        #region IQuantifierTypeProperties Members

//        public double MinValue
//        {
//            get { throw new Exception("The method or operation is not implemented."); }
//        }

//        public double MaxValue
//        {
//            get { throw new Exception("The method or operation is not implemented."); }
//        }

//        public PerformanceDifficultyEnum QuantifierPerformanceDifficulty
//        {
//            get { throw new Exception("The method or operation is not implemented."); }
//        }

//        public bool NeedsNumericValues
//        {
//            get { throw new Exception("The method or operation is not implemented."); }
//        }

//        public CardinalityEnum SupportedData
//        {
//            get { throw new Exception("The method or operation is not implemented."); }
//        }

//        public QuantifierClassEnum[] QuantifierClasses
//        {
//            get { throw new Exception("The method or operation is not implemented."); }
//        }

//        public bool IrrelevantOnUnits
//        {
//            get { return false; }
//        }

//        public bool SupportsFloatContingencyTable
//        {
//            get { return false; } //TODO ??
//        }

//        #endregion
//    }

//    /// <summary>
//    /// Computes the simple deviation strength.
//    /// </summary>
//    /// <returns>
//    /// <para>Simple deviation strength result defined as <c>ln(ad/bc) / ln(2)</c>.</para>
//    /// <para>There are special cases defined explicitly:</para>
//    /// <para>If both <c>(a * d) = 0</c> and <c>(b * c) = 0</c>, return 0.</para>
//    /// <para>If only <c>(a * d) = 0</c>, return -INF.</para>
//    /// <para>If only <c>(b * c) = 0</c>, return +INF.</para>
//    /// </returns>
//    public class SimpleDeviation : IQuantifierValue
//    {
//        #region IQuantifier Members

//        public double Value(IQuantifierValueData data)
//        {
//            FourFoldContingencyTable table = data.FourFoldContingencyTable;

//            double ad = table.A * table.D;
//            double bc = table.B * table.C;
//            if ((ad == 0) && (bc == 0))
//                return 0.0D;
//            if (ad == 0)
//                return Double.NegativeInfinity;
//            if (bc == 0)
//                return Double.PositiveInfinity;
//            return (System.Math.Log(ad / bc) / 0.693147180559945309417);
//        }

//        #endregion

//        #region IQuantifierTypeProperties Members

//        public double MinValue
//        {
//            get { return Double.NegativeInfinity; }
//        }

//        public double MaxValue
//        {
//            get { return Double.PositiveInfinity; }
//        }

//        public PerformanceDifficultyEnum QuantifierPerformanceDifficulty
//        {
//            get { return PerformanceDifficultyEnum.Easy; }
//        }

//        public bool NeedsNumericValues
//        {
//            get { return false; }
//        }

//        public CardinalityEnum SupportedData
//        {
//            get { return CardinalityEnum.Nominal; }
//        }

//        public QuantifierClassEnum[] QuantifierClasses
//        {
//            get { return new QuantifierClassEnum[] { }; }
//        }

//        public bool IrrelevantOnUnits
//        {
//            get { return true; }
//        }

//        public bool SupportsFloatContingencyTable
//        {
//            get { return true; }
//        }

//        #endregion
//    }

//    //TODO fisher
//    //public class Fisher : ???

//    #endregion
//}
