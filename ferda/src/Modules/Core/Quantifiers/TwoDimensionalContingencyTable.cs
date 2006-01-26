using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Modules.Quantifiers
{
    public class TwoDimensionalContingencyTable : ContingencyTable
    {
        #region Constructors
        public TwoDimensionalContingencyTable()
            : base()
        { }
        public TwoDimensionalContingencyTable(int[][] contingencyTable)
            : base(contingencyTable)
        { }
        public TwoDimensionalContingencyTable(long[][] contingencyTable)
            : base(contingencyTable)
        { }
        public TwoDimensionalContingencyTable(long[,] contingencyTable)
            : base(contingencyTable)
        { }
        public TwoDimensionalContingencyTable(long[,] contingencyTable, long denominator)
            : base(contingencyTable, denominator)
        { }
        #endregion

        public static double SumOfRowMaximumsValue(TwoDimensionalContingencyTable table)
        {
            return table.SumOfRowMaximums;
        }
        /// <summary>
        /// SumOfMaximumsOfRows (sometimes also FunctionSumOfRows)
        /// </summary>
        /// <remarks>See [032 Zadání pro KL-Miner.doc] chapter 4.1.1.</remarks>
        public double SumOfRowMaximums
        {
            get
            {
                long sumOfRowMax = 0;
                long rowMax;
                long sum = 0;
                int firstColumnIndex = FirstColumnIndex;
                int lastColumnIndex = LastColumnIndex;
                for (int rowIndex = FirstRowIndex; rowIndex <= LastRowIndex; rowIndex++)
                {
                    rowMax = long.MinValue;
                    for (int columnIndex = firstColumnIndex; columnIndex <= lastColumnIndex; columnIndex++)
                    {
                        sum += table[rowIndex, columnIndex];
                        rowMax = Math.Max(rowMax, table[rowIndex, columnIndex]);
                    }
                    sumOfRowMax += rowMax;
                }
                return sumOfRowMax / ((double)denominator * sum);
            }
        }

        public static double MinOfRowMaximumsValue(TwoDimensionalContingencyTable table)
        {
            return table.MinOfRowMaximums;
        }
        /// <summary>
        /// SumOfMaximumsOfRows (sometimes also FunctionEachRow)
        /// </summary>
        /// <remarks>See [032 Zadání pro KL-Miner.doc] chapter 4.1.1.</remarks>
        public double MinOfRowMaximums
        {
            get
            {
                long minOfRowMax = long.MaxValue;
                long rowMax;
                long sum = 0;
                int firstColumnIndex = FirstColumnIndex;
                int lastColumnIndex = LastColumnIndex;
                for (int rowIndex = FirstRowIndex; rowIndex <= LastRowIndex; rowIndex++)
                {
                    rowMax = long.MinValue;
                    for (int columnIndex = firstColumnIndex; columnIndex <= lastColumnIndex; columnIndex++)
                    {
                        sum += table[rowIndex, columnIndex];
                        rowMax = Math.Max(rowMax, table[rowIndex, columnIndex]);
                    }
                    minOfRowMax = Math.Min(minOfRowMax, rowMax);
                }
                return minOfRowMax / ((double)denominator * sum);
            }
        }

        /// <summary>
        /// Symetric Wallis quantifier (sometimes also InformationDependency)
        /// </summary>
        /// <remarks>See [053 Definice KL-kvantifikátorů.pdf] chapter 5.1.</remarks>
        public double SymetricWallis
        {
            get
            {
                double numerator, sumK, sumL, denominator;
                numerator = sumK = sumL = 0;
                denominator = this.denominator;

                int firstRowIndex = FirstRowIndex;
                int firstColumnIndex = FirstColumnIndex;
                int lastColumnIndex = LastColumnIndex;
                long[] rowSums = PreparedSums.RowSums;
                long[] columnSums = PreparedSums.ColumnSums;
                for (int rowIndex = firstRowIndex; rowIndex <= LastRowIndex; rowIndex++)
                {
                    double rowSum = rowSums[rowIndex - firstRowIndex] / denominator;
                    for (int columnIndex = firstColumnIndex; columnIndex <= lastColumnIndex; columnIndex++)
                    {
                        double columnSum = columnSums[columnIndex - firstColumnIndex] / denominator;
                        numerator += Math.Pow((table[rowIndex, columnIndex] / denominator) - (rowSum * columnSum), 2) * (rowSum + columnSum / (rowSum * columnSum));
                        sumK += Math.Pow(rowSum, 2);
                        sumL += Math.Pow(columnSum, 2);
                    }
                }
                double result = (0.5f * numerator) / (1 - 0.5f * (sumK + sumL));
                return result;
            }
        }

        public static double KendalValue(TwoDimensionalContingencyTable table)
        {
            return table.Kendal;
        }
        /// <summary>
        /// See [053 Definice KL-kvantifikátorů.pdf] chapter 5.4.
        /// </summary>
        public double Kendal
        {
            get
            {
                double denominator = this.denominator;
                double p, q, sum, sumOfSecondPowerOfRowSums, sumOfSecondPowerOfColumnSums;
                p = q = sum = sumOfSecondPowerOfRowSums = sumOfSecondPowerOfColumnSums = 0;

                int firstRowIndex = FirstRowIndex;
                int lastRowIndex = LastRowIndex;
                int firstColumnIndex = FirstColumnIndex;
                int lastColumnIndex = LastColumnIndex;
                long[] columnSums = new long[lastColumnIndex - firstColumnIndex + 1];
                columnSums.Initialize();

                #region Initialize qTmp (Q table, P table can be computed from qTmp)

                // initializes Q table (qTmp) (P table can be computed from Q table)
                long[,] qTmp = new long[lastRowIndex + 1, firstColumnIndex + 1];
                qTmp.Initialize();
                //Sum(i>=k,j<=l)Nij where 
                // .. k is row index, 
                // .. l is column index and 
                // .. Nij is number in contingecy table on ij position
                for (int rowIndex = lastRowIndex; rowIndex >= firstRowIndex; rowIndex--)
                {
                    for (int columnIndex = firstColumnIndex; columnIndex <= lastColumnIndex; columnIndex++)
                    {
                        if (columnIndex > firstColumnIndex)
                        {
                            // gets left rectangle
                            long leftRectangle = qTmp[rowIndex, columnIndex - 1];

                            // gets lower rectangle
                            long lowerRectangle = 0;
                            // gets left lower rectangle
                            long leftLowerRectangle = 0;
                            if (rowIndex < lastRowIndex)
                            {
                                lowerRectangle = qTmp[rowIndex + 1, columnIndex];
                                leftLowerRectangle = qTmp[rowIndex + 1, columnIndex - 1];
                            }

                            qTmp[rowIndex, columnIndex] =
                                leftRectangle
                                + lowerRectangle
                                - leftLowerRectangle
                                + table[rowIndex, columnIndex];
                        }
                        else // i.e. (columnIndex == firstColumnIndex)
                        {
                            if (rowIndex < lastRowIndex)
                                qTmp[rowIndex, columnIndex] = qTmp[rowIndex + 1, columnIndex] + table[rowIndex, columnIndex];
                            else // i.e.(rowIndex == lastRowIndex)
                                qTmp[rowIndex, columnIndex] = table[rowIndex, columnIndex];
                        }
                    }
                }
                #endregion

                for (int rowIndex = firstRowIndex; rowIndex <= LastRowIndex; rowIndex++)
                {
                    double rowSum = 0;
                    for (int columnIndex = firstColumnIndex; columnIndex <= lastColumnIndex; columnIndex++)
                    {
                        long item = table[rowIndex, columnIndex];
                        rowSum += item;
                        columnSums[columnIndex] += item;

                        if (rowIndex < lastRowIndex)
                        {
                            //qSum = Sum(i>k,j<l)Nij where k is row index, l is column index and Nij is number in contingecy table on ij position
                            long qSum = (columnIndex > firstColumnIndex) ? qTmp[rowIndex + 1, columnIndex - 1] : 0;
                            q += item * qSum;

                            //pSum = Sum(i>k,j>l)Nij where k is row index, l is column index and Nij is number in contingecy table on ij position
                            long pSum = qTmp[rowIndex + 1, lastColumnIndex] - qTmp[rowIndex + 1, columnIndex];
                            p += item * pSum;
                        }
                    }
                    sumOfSecondPowerOfRowSums += Math.Pow(rowSum, 2);
                    sum += rowSum;
                }
                for (int columnIndex = firstColumnIndex; columnIndex <= lastColumnIndex; columnIndex++)
                {
                    sumOfSecondPowerOfColumnSums += Math.Pow(columnSums[columnIndex], 2);
                }
                double secondPowerOfSum = Math.Pow(sum, 2);
                double resultDenominator = Math.Sqrt(
                    (secondPowerOfSum - sumOfSecondPowerOfRowSums) * (sum - sumOfSecondPowerOfColumnSums)
                    );
                double resultNumerator = 2 * (p - q);
                double result = (resultNumerator / resultDenominator) / Math.Pow(denominator, 2);
                return result;
            }
        }

        public float ChiSquareTest
        {//TODO ChiSquareTest over KL 053, 055, dmw3.pdf
            get { return 0; }
        }

        #region Entropies
        /// <summary>
        /// H(C)
        /// </summary>
        /// <remarks>See [055 Náměty pro další vývoj KL-Miner.doc] or [080 Náměty pro další vývoj KL-Miner II.doc].</remarks>
        public double MarginalColumnEntropy
        {
            get
            {
                double denominator = this.denominator;
                double result = 0;

                foreach (long number in PreparedSums.ColumnSums)
                {
                    double item = number / denominator;
                    result += item * Math.Log(item, 2);
                }
                return -result;
                /*
                int firstColumnIndex = FirstColumnIndex;
                int lastColumnIndex = LastColumnIndex;
                for (int columnIndex = firstColumnIndex; columnIndex <= lastColumnIndex; columnIndex++)
                {
                    double number = columnSums[columnIndex - firstColumnIndex] / allSum;
                    result += number * Math.Log(number, 2);
                }
                 */
            }
        }

        /// <summary>
        /// H(R)
        /// </summary>
        /// <remarks>See [055 Náměty pro další vývoj KL-Miner.doc] or [080 Náměty pro další vývoj KL-Miner II.doc].</remarks>
        public double MarginalRowEntropy
        {
            get
            {
                double denominator = this.denominator;
                double result = 0;

                foreach (long number in PreparedSums.RowSums)
                {
                    double item = number / denominator;
                    result += item * Math.Log(item, 2);
                }
                return -result;
            }
        }

        /// <summary>
        /// H(C,R) = H(R,C)
        /// </summary>
        /// <remarks>See [055 Náměty pro další vývoj KL-Miner.doc] or [080 Náměty pro další vývoj KL-Miner II.doc].</remarks>
        public double JointEntropy
        {
            get
            {
                double denominator = this.denominator;
                double result = 0;

                int firstColumnIndex = FirstColumnIndex;
                int lastColumnIndex = LastColumnIndex;
                for (int rowIndex = FirstRowIndex; rowIndex <= LastRowIndex; rowIndex++)
                {
                    for (int columnIndex = firstColumnIndex; columnIndex <= lastColumnIndex; columnIndex++)
                    {
                        double item = table[rowIndex, columnIndex] / denominator;
                        result += item * Math.Log(item, 2);
                    }
                }
                return -result;
            }
        }

        public static double ConditionalCREntropyValue(TwoDimensionalContingencyTable table)
        {
            return table.ConditionalCREntropy;
        }
        /// <summary>
        /// H(C|R) = H(C,R) - H(R)
        /// </summary>
        /// <remarks>See [055 Náměty pro další vývoj KL-Miner.doc] or [080 Náměty pro další vývoj KL-Miner II.doc].</remarks>
        public double ConditionalCREntropy
        {
            get
            {
                return JointEntropy - MarginalRowEntropy;
            }
        }

        public static double ConditionalRCEntropyValue(TwoDimensionalContingencyTable table)
        {
            return table.ConditionalRCEntropy;
        }
        /// <summary>
        /// H(R|C) = H(C,R) - H(C)
        /// </summary>
        /// <remarks>See [055 Náměty pro další vývoj KL-Miner.doc] or [080 Náměty pro další vývoj KL-Miner II.doc].</remarks>
        public double ConditionalRCEntropy
        {
            get
            {
                return JointEntropy - MarginalColumnEntropy;
            }
        }

        /// <summary>
        /// MI(C,R) = MI(R,C) = H(C) + H(R) - H(C,R) = H(C) - H(C|R)
        /// Sometimes as I(C,R) or I(R,C)
        /// </summary>
        /// <remarks>See [055 Náměty pro další vývoj KL-Miner.doc] or [080 Náměty pro další vývoj KL-Miner II.doc].</remarks>
        public double MutualInformation
        {
            get
            {
                return MarginalColumnEntropy + MarginalRowEntropy - JointEntropy;
            }
        }

        public static double MutualInformationNormalizedValue(TwoDimensionalContingencyTable table)
        {
            return table.MutualInformationNormalized;
        }
        /// <summary>
        /// MI*(C,R) = MI(C,R) / min{ H(C), H(R) }
        /// </summary>
        /// <remarks>See [055 Náměty pro další vývoj KL-Miner.doc] or [080 Náměty pro další vývoj KL-Miner II.doc].</remarks>
        public double MutualInformationNormalized
        {
            get
            {
                double marginalColumnEntropy = MarginalColumnEntropy;
                double marginalRowEntropy = MarginalRowEntropy;
                return marginalColumnEntropy + marginalRowEntropy - JointEntropy
                    / Math.Min(marginalColumnEntropy, marginalRowEntropy);
            }
        }

        public static double InformationDependenceCRValue(TwoDimensionalContingencyTable table)
        {
            return table.InformationDependenceCR;
        }
        /// <summary>
        /// Information dependence of C on R
        /// ID(C|R) = 1 - ( H(C|R) / H(C) )
        /// </summary>
        /// <remarks>See [055 Náměty pro další vývoj KL-Miner.doc] or [080 Náměty pro další vývoj KL-Miner II.doc].</remarks>
        public double InformationDependenceCR
        {
            get
            {
                return 1 - (ConditionalCREntropy / MarginalColumnEntropy);
            }
        }

        public static double InformationDependenceRCValue(TwoDimensionalContingencyTable table)
        {
            return table.InformationDependenceRC;
        }
        /// <summary>
        /// Information dependence of R on C
        /// ID(R|C) = 1 - ( H(R|C) / H(R) ) = 1 - ( ( H(C,R) - H(C) ) / H(R))
        /// </summary>
        /// <remarks>See [055 Náměty pro další vývoj KL-Miner.doc] or [080 Náměty pro další vývoj KL-Miner II.doc].</remarks>
        public double InformationDependenceRC
        {
            get
            {
                return 1 - (ConditionalRCEntropy / MarginalRowEntropy);
            }
        }

        /* OBSOLETE *
        /// <summary>
        /// OBSOLETE
        /// Je nahrazen InformationDependenceCR a InformationDependenceRC
        /// See [053 Definice KL-kvantifikátorů.pdf] chapter 4.2.
        /// </summary>
        /// <remarks>Also known as Information dependency quantifiers.</remarks>
        public double AsymetricInformationCoefficient
        {
            get
            {
                double denominator = this.denominator;
                double sum, sumOfNLogNRows, sumOfNLogNColumns, sumOfNLogNAll, sumOfNLogNSum;
                sum = sumOfNLogNRows = sumOfNLogNColumns = sumOfNLogNAll = sumOfNLogNSum = 0;

                int firstRowIndex = FirstRowIndex;
                int lastRowIndex = LastRowIndex;
                int firstColumnIndex = FirstColumnIndex;
                int lastColumnIndex = LastColumnIndex;
                double[] columnSums = new double[lastColumnIndex - firstColumnIndex + 1];
                columnSums.Initialize();
                for (int rowIndex = firstRowIndex; rowIndex <= LastRowIndex; rowIndex++)
                {
                    double rowSum = 0;
                    for (int columnIndex = firstColumnIndex; columnIndex <= lastColumnIndex; columnIndex++)
                    {
                        double item = table[rowIndex, columnIndex] / denominator;
                        rowSum += item;
                        columnSums[columnIndex] += item;
                        sumOfNLogNAll += item * Math.Log(item, 2);
                    }
                    sumOfNLogNRows += rowSum * Math.Log(rowSum, 2);
                    sum += rowSum;
                }
                for (int columnIndex = firstColumnIndex; columnIndex <= lastColumnIndex; columnIndex++)
                {
                    sumOfNLogNColumns += columnSums[columnIndex] * Math.Log(columnSums[columnIndex], 2);
                }
                sumOfNLogNSum = sum * Math.Log(sum, 2);

                double result = 1 - ((sumOfNLogNRows - sumOfNLogNAll) / (sumOfNLogNSum - sumOfNLogNColumns));
                return result;
            }
        }
         */
        #endregion
    }
}
