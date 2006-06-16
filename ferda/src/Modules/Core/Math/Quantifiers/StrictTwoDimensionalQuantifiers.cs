using System;
using Ferda.Guha.Data;

namespace Ferda.Guha.Math.Quantifiers
{
    #region Functional Dependence

    /// <summary>
    /// <para>
    /// Tests functional dependence of columns on rows. Test
    /// if number of object that fault dependence Col = F(Row)
    /// is for each value (row category) relatively "small". 
    /// Number of object which fault this depencence
    /// can be for each <c>i</c> computed as 
    /// <c>(Sum(j)(Freq i,j) - Max(j)(Freq i,j))</c>.
    /// </para>
    /// <para>
    /// Tests are following: for each <c>i</c> is 
    /// <c>Max(j)(Freq i,j) [Relation(default &gt;=)] p * Sum(j)(Freq i,j)</c>;
    /// and for each <c>i</c> is 
    /// <c>Sum(j)(Freq i,j) - Max(j)(Freq i,j) [Opposite! Relation] Tr</c>.
    /// </para>
    /// <para>
    /// This quantifier has not defined value. Returned value 
    /// depends on specified relation. For &gt; is returned Min(i) of <c>Xi</c>
    /// and for &lt; is returned Max(i) of <c>Xi</c>, where 
    /// <c>Xi = (Max(j)(Freq i,j) / Sum(j)(Freq i,j))</c>.
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// See [032 Zadání pro KL-Miner.doc] chapter 4.1.3.
    /// </para>
    /// <para>
    /// Sometimes also FunctionEachRow
    /// </para>
    /// </remarks>
    public class FunctionOfRowEachRow : IQuantifierValidateWithValueOfInterest
    {
        #region IQuantifierValue Members

        public bool Validate(IQuantifierValidateData data, out double value)
        {
            ContingencyTable table = data.ContingencyTable;
            double p = data.Treshold;
            double absTr = (double)data.GetParam("Tr");
            RelationEnum relation = data.Relation;

            bool watchForResult = (0 != Common.GetRelationOrientation(relation));
            value = Double.NaN;

            for (int r = 0; r < table.NumberOfRows; r++)
            {
                //row maximum
                double rowMaxFreq = Double.MinValue;
                double rowSum = 0;

                for (int c = 0; c < table.NumberOfColumns; c++)
                {
                    rowMaxFreq = System.Math.Max(rowMaxFreq, table[r, c]);
                    rowSum += table[r, c];
                }
                //Tests:
                if (Common.Compare(relation, rowMaxFreq, p * rowSum) //test on p
                    && Common.Compare(relation, (absTr * table.Denominator) - rowSum, -rowMaxFreq)) //test on Tr (opposite relation)
                {
                    if (watchForResult)
                        value = Common.GetOrientationBetterValue(relation, value, rowMaxFreq / rowSum);
                }
                else
                {
                    value = Double.NaN;
                    return false;
                }
            }
            return true;
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
            // through "Tr"
        }

        public bool SupportsFloatContingencyTable
        {
            get { return true; }
        }

        #endregion
    }

    /// <summary>
    /// <para>
    /// Tests functional dependence of columns on rows. Test
    /// if Col = F(Row). Number of object which fault this depencence
    /// can be computed as <c>Sum(i)(Sum(j)(Freq i,j) - Max(j)(Freq i,j))</c>
    /// i.e. <c>M - Sum(i)(Max(j)(Freq i,j))</c> where <c>M = Sum(i,j)(Freq i,j)</c>.
    /// </para>
    /// <para>
    /// Tests are following: <c>Sum(i)(Max(j)(Freq i,j)) [Relation(default &gt;=)] p * M</c>;
    /// and <c>M - Sum(i)(Max(j)(Freq i,j)) [Opposite! Relation] Tr</c>.
    /// </para>
    /// <para>
    /// This quantifier has not defined value. Returned value is 
    /// <c>Sum(i)(Max(j)(Freq i,j)) / M</c> as good understandable 
    /// value for ordering/comparing hypotheses.
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// See [032 Zadání pro KL-Miner.doc] chapter 4.1.1.
    /// </para>
    /// <para>
    /// Sometimes also FunctionSumOfRows
    /// </para>
    /// </remarks>
    public class FunctionOfRow : IQuantifierValidateWithValueOfInterest
    {
        #region IQuantifierValue Members

        public bool Validate(IQuantifierValidateData data, out double value)
        {
            ContingencyTable table = data.ContingencyTable;
            double p = data.Treshold;
            double absTr = (double)data.GetParam("Tr");
            RelationEnum relation = data.Relation;

            double m = data.ContingencyTable.Sum;

            // sum of row maximums
            double sumRowMaxFreq = 0;

            for (int r = 0; r < table.NumberOfRows; r++)
            {
                //row maximum
                double rowMaxFreq = Double.MinValue;

                for (int c = 0; c < table.NumberOfColumns; c++)
                {
                    rowMaxFreq = System.Math.Max(rowMaxFreq, table[r, c]);
                }
                sumRowMaxFreq += rowMaxFreq;
            }

            //Tests:
            if (Common.Compare(relation, sumRowMaxFreq, p * m) //test on p
                && Common.Compare(relation, (absTr * table.Denominator) - m, -sumRowMaxFreq)) //test on Tr (opposite relation)
            {
                value = sumRowMaxFreq / m;
                return true;
            }
            else
            {
                value = Double.NaN;
                return false;
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
            // through "Tr"
        }

        public bool SupportsFloatContingencyTable
        {
            get { return true; }
        }

        #endregion
    }
    
    #endregion

    #region Ordinal Dependence

    /// <summary>
    /// <para>
    /// Kendall quantifier value (from interval [0,1]) is defined as
    /// <c>Abs(TauB)</c> where 
    /// <c>TauB = 2(P-Q) / Sqrt((n^2 * Sum(r)((Freq r,*)^2)) * (n^2 * Sum(c)((Freq *,c)^2)))</c>
    /// where <c>n = Sum(r,c)(Freq r,c)</c>; <c>Freq r,* = Sum(c)(Freq r,c)</c>;
    /// <c>P = Sum(r,c)(Freq r,c * Sum(i&gt;r,j&gt;c)(Freq i,j))</c> and
    /// <c>Q = Sum(r,c)(Freq r,c * Sum(i&gt;r,j&lt;c)(Freq i,j))</c>.
    /// </para>
    /// <para>
    /// In statistics, rank correlation is the study of relationships 
    /// between different rankings on the same set of items. It deals 
    /// with measuring correspondence between two rankings, and assessing 
    /// the significance of this correspondence. The statistic described 
    /// here is also known as Kendall's Tau, which is different from 
    /// Spearman's rank correlation coefficient.
    /// </para>
    /// </summary>
    /// <remarks>
    /// See [053 Definice KL-kvantifikátorů.pdf] chapter 5.4.
    /// See [052 Two-dimensional Contingency Tables.pdf] chapter 4.1.5.
    /// See [Mining for Patterns Based on Contingency Tables by KL–Miner - First Experience]
    /// </remarks>
    /// <seealso href="http://en.wikipedia.org/wiki/Kendall's_tau"/>
    public class Kendall : IQuantifierValue
    {
        #region IQuantifierValue Members


        public double Value(IQuantifierValueData data)
        {
            ContingencyTable table = data.ContingencyTable;

            // qTmp[r,c] = Sum(i>=r,j<=c)(Freq i,j)
            #region Initialize qTmp (Q table, P table can be computed from qTmp)

            // qTmp[r,c] = Sum(i>=r,j<=c)(Freq i,j)
            double[,] qTmp = new double[table.NumberOfRows, table.NumberOfColumns];
            qTmp.Initialize();
            for (int r = table.NumberOfRows - 1; r >= 0; r--)
            {
                for (int c = 0; c < table.NumberOfColumns; c++)
                {
                    if (c > 0)
                    {
                        // gets left rectangle
                        double leftRectangle = qTmp[r, c - 1];

                        // gets lower rectangle
                        double lowerRectangle;
                        // gets left lower rectangle
                        double leftLowerRectangle;

                        if (r == table.NumberOfRows - 1)
                        {
                            lowerRectangle = 0;
                            leftLowerRectangle = 0;
                        }
                        else
                        {
                            lowerRectangle = qTmp[r + 1, c];
                            leftLowerRectangle = qTmp[r + 1, c - 1];
                        }

                        qTmp[r, c] =
                            leftRectangle
                            + lowerRectangle
                            - leftLowerRectangle
                            + table[r, c];
                    }
                    else // i.e. (columnIndex == 0)
                    {
                        if (r == table.NumberOfRows - 1)
                            qTmp[r, c] = table[r, c];
                        else
                            qTmp[r, c] = qTmp[r + 1, c] + table[r, c];
                    }
                }
            }
            #endregion

            double p = 0;
            //P = Sum(r,c)(Freq r,c * Sum(i>r,j>c)(Freq i,j))
            double q = 0;
            //Q = Sum(r,c)(Freq r,c * Sum(i>r,j<c)(Freq i,j))

            double sumRowSum_2 = 0;
            //Sum(r)((Freq r,*)^2)
            double sumColSum_2 = 0;
            //Sum(c)((Freq *,c)^2)

            for (int r = 0; r < table.NumberOfRows; r++)
            {
                for (int c = 0; c < table.NumberOfColumns; c++)
                {
                    double item = table[r, c];

                    /* please note that
                     * qTmp[r,c] = Sum(i>=r,j<=c)(Freq i,j)
                     * */

                    // pSum = Sum(i>r,j>c)(Freq i,j)
                    // => pSum[r,c] = qTmp[r+1,*] - qTmp[r+1,c] (! be careful to overflow)
                    if (r < table.NumberOfRows - 1)
                        p += item * qTmp[r + 1, table.NumberOfColumns - 1] - qTmp[r + 1, c];

                    // qSum = Sum(i>r,j<c)(Freq i,j)
                    // => qSum[r,c] = qTmp[r+1,c-1] (! be careful to overflow)
                    if (c > 0 && r < table.NumberOfRows - 1)
                        q += item * qTmp[r + 1, c - 1];
                }
                sumRowSum_2 += System.Math.Pow(table.RowSums[r], 2);
            }
            for (int c = 0; c < table.NumberOfColumns; c++)
            {
                sumColSum_2 += System.Math.Pow(table.ColumnSums[c], 2);
            }

            double n_2 = System.Math.Pow(table.Sum, 2);
            //n^2 where n = Sum(r,c)(Freq r,c)

            return
                2 * System.Math.Abs(p - q)
                /
                System.Math.Sqrt((n_2 - sumRowSum_2) * (n_2 - sumColSum_2));
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
            get { return PerformanceDifficultyEnum.QuiteDifficult; }
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