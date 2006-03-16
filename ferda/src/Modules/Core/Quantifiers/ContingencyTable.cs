using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Modules.Helpers.Common;

namespace Ferda.Modules.Quantifiers
{
    /// <summary>
    /// Represents contingency table.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class provides basic functionality for working
    /// with contingency tables, for further functionality, 
    /// please see derived classes.
    /// </para>
    /// <para>
    /// Please notice that sometimes you want to use only submatrix of 
    /// the contingency table so you cen use properties:
    /// <see cref="P:Ferda.Modules.Quantifiers.ContingencyTable.StartRowBound"/>,
    /// <see cref="P:Ferda.Modules.Quantifiers.ContingencyTable.EndoRowBound"/>,
    /// <see cref="P:Ferda.Modules.Quantifiers.ContingencyTable.StartColumnBound"/>,
    /// <see cref="P:Ferda.Modules.Quantifiers.ContingencyTable.EndColumnBound"/>.
    /// </para>
    /// <para>
    /// Sometimes you may to want to divide contingency table by some 
    /// numeber, in that case please see 
    /// <see cref="P:Ferda.Modules.Quantifiers.ContingencyTable.Denominator"/>
    /// (default value is 1).
    /// </para>
    /// </remarks>
    public class ContingencyTable
    {
        #region Core Functions

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ContingencyTable"/> class.
        /// </summary>
        public ContingencyTable()
        {
            PreparedSums = new PreparedSums(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContingencyTable"/> class.
        /// </summary>
        /// <param name="contingencyTable">The contingency table.</param>
        /// <remarks>
        /// For futher information about <c>contingencyTable</c> param please see
        /// <see cref="P:Ferda.Modules.Quantifiers.ContingencyTable.Table"/>.
        /// </remarks>
        public ContingencyTable(int[][] contingencyTable)
        {
            PreparedSums = new PreparedSums(this);

            int numOfRows = contingencyTable.Length;
            if (numOfRows <= 0)
                throw Ferda.Modules.Exceptions.BadParamsError(null, null, "Bad shape of contingecy table!", restrictionTypeEnum.BadFormat);
            int numOfColumns = contingencyTable[0].Length;
            long[,] newTable = new long[numOfRows, numOfColumns];
            int rowNumber = 0;
            foreach (int[] row in contingencyTable)
            {
                if (row.Length != numOfColumns)
                    throw Ferda.Modules.Exceptions.BadParamsError(null, null, "Bad shape of contingecy table!", restrictionTypeEnum.BadFormat);
                int columnNumber = 0;
                foreach (int item in row)
                {
                    newTable[rowNumber, columnNumber] = item;
                    columnNumber++;
                }
                rowNumber++;
            }
            Table = newTable;
            denominator = 1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContingencyTable"/> class.
        /// </summary>
        /// <param name="contingencyTable">The contingency table.</param>
        /// <remarks>
        /// For futher information about <c>contingencyTable</c> param please see
        /// <see cref="P:Ferda.Modules.Quantifiers.ContingencyTable.Table"/>.
        /// </remarks>
        public ContingencyTable(long[][] contingencyTable)
        {
            PreparedSums = new PreparedSums(this);

            int numOfRows = contingencyTable.Length;
            if (numOfRows <= 0)
                throw Ferda.Modules.Exceptions.BadParamsError(null, null, "Bad shape of contingecy table!", restrictionTypeEnum.BadFormat);
            int numOfColumns = contingencyTable[0].Length;
            long[,] newTable = new long[numOfRows, numOfColumns];
            int rowNumber = 0;
            foreach (long[] row in contingencyTable)
            {
                if (row.Length != numOfColumns)
                    throw Ferda.Modules.Exceptions.BadParamsError(null, null, "Bad shape of contingecy table!", restrictionTypeEnum.BadFormat);
                int columnNumber = 0;
                foreach (long item in row)
                {
                    newTable[rowNumber, columnNumber] = item;
                    columnNumber++;
                }
                rowNumber++;
            }
            Table = newTable;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContingencyTable"/> class.
        /// </summary>
        /// <param name="contingencyTable">The contingency table.</param>
        /// <remarks>
        /// For futher information about <c>contingencyTable</c> param please see
        /// <see cref="P:Ferda.Modules.Quantifiers.ContingencyTable.Table"/>.
        /// </remarks>
        public ContingencyTable(long[,] contingencyTable)
        {
            PreparedSums = new PreparedSums(this);

            Table = contingencyTable;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContingencyTable"/> class.
        /// </summary>
        /// <param name="contingencyTable">The contingency table.</param>
        /// <param name="denominator">The denominator.</param>
        /// <remarks>
        /// For futher information about <c>contingencyTable</c> param please see
        /// <see cref="P:Ferda.Modules.Quantifiers.ContingencyTable.Table"/>.
        /// </remarks>
        public ContingencyTable(long[,] contingencyTable, long denominator)
        {
            PreparedSums = new PreparedSums(this);

            Table = contingencyTable;
            this.denominator = denominator;
        }
        #endregion

        #region Fields (table, denominator)

        /// <summary>
        /// Provides some prepared sums. 
        /// Computations are lazy evaluated and results are cached.
        /// </summary>
        public PreparedSums PreparedSums;

        private long denominator = 1;
        /// <summary>
        /// Gets the denominator or (instead of set) multiples the 
        /// denominator by given <c>result</c> (default result of the
        /// denominator is <c>1</c>).
        /// </summary>
        /// <result>The denominator.</result>
        public long Denominator
        {
            get { return denominator; }
            set { denominator *= value; }
        }

        private long[,] table;
        /// <summary>
        /// <para>
        /// Table [r, c], <c>r</c> is index of row, <c>c</c> is index of column.
        /// </para>
        /// <para>
        /// At position [0, 0] is item at first row and first column, 
        /// if you like <b>a-frequency</b>.
        /// </para>
        /// <para>
        /// Please notice that values of the contingecy table are not denominated
        /// therefore, don`t forget to use <see cref="P:Ferda.Modules.Quantifiers.ContingencyTable.Denominator"/>.
        /// </para>
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <exception cref="T:Ferda.Modules.BadParamsError">
        /// Thrown if contingency table has bad shape i.e. no rows or no columns;
        /// or any item of the table is &lt; 0. (Contingency table should have all
        /// items possitive.)
        /// </exception>
        public long[,] Table
        {
            private set
            {
                if (value.GetLength(0) <= 0)
                    throw Ferda.Modules.Exceptions.BadParamsError(null, null, "Bad shape of contingecy table!", restrictionTypeEnum.BadFormat);
                if (value.GetLength(1) <= 0)
                    throw Ferda.Modules.Exceptions.BadParamsError(null, null, "Bad shape of contingecy table!", restrictionTypeEnum.BadFormat);
                foreach (long item in value)
                {
                    if (item < 0)
                        throw Ferda.Modules.Exceptions.BadParamsError(null, null, "All values in contingency table have to be greater than 0!", restrictionTypeEnum.BadFormat);
                }
                table = value;
            }
            get
            {
                return table;
            }
        }
        #endregion

        #region Arithmetic (operator+, operator-, operator==, operator!=, GetHashCode, Div(result))
        /// <summary>
        /// Operator <b>==</b>.
        /// </summary>
        /// <param name="a">First contingecy table.</param>
        /// <param name="b">Second contingecy table.</param>
        /// <returns>
        /// <c>true</c> iff all corresponding items of specified tables (in space 
        /// restricted by their bounds) are equal but also their denominators equals;
        /// otherwise, <c>false</c>. 
        /// </returns>
        /// <exception cref="T:Ferda.Modules.BadParamsError">
        /// Thrown iff specified contingecy tables (more precisely its submatrixes)
        /// has differend shapes.
        /// </exception>
        public static bool operator ==(ContingencyTable a, ContingencyTable b)
        {
            int aFirstRowIndex = a.FirstRowIndex;
            int aLastRowIndex = a.LastRowIndex;
            int aFirstColumnIndex = a.FirstColumnIndex;
            int aLastColumnIndex = a.LastColumnIndex;
            int bFirstRowIndex = b.FirstRowIndex;
            int bLastRowIndex = b.LastRowIndex;
            int bFirstColumnIndex = b.FirstColumnIndex;
            int bLastColumnIndex = b.LastColumnIndex;

            long aMultiplicator = b.Denominator;
            long bMultiplicator = a.Denominator;
            long resultDenominator = aMultiplicator * bMultiplicator;
            if (aMultiplicator == bMultiplicator)
            {
                resultDenominator = aMultiplicator;
                aMultiplicator = bMultiplicator = 1;
            }

            if ((aLastRowIndex - aFirstRowIndex) == (bLastRowIndex - bFirstRowIndex)
                && (aLastColumnIndex - aFirstColumnIndex) == (bLastColumnIndex - bFirstColumnIndex))
            {
                int aToBRowShift = bFirstRowIndex - aFirstRowIndex;
                int aToBColumnShift = bFirstColumnIndex - aFirstColumnIndex;
                for (int rowIndex = aFirstRowIndex; rowIndex <= aLastRowIndex; rowIndex++)
                {
                    for (int columnIndex = aFirstColumnIndex; columnIndex <= aLastColumnIndex; columnIndex++)
                    {
                        if (a.table[rowIndex, columnIndex] * aMultiplicator
                            != b.table[rowIndex - aToBRowShift, columnIndex - aToBColumnShift] * bMultiplicator)
                            return false;
                    }
                }
                return true;
            }
            throw Ferda.Modules.Exceptions.BadParamsError(null, null, "Uncompatible sizes of contingecy tables to combine!", restrictionTypeEnum.BadFormat);
        }

        private enum operatorType
        {
            Plus,
            Minus
        }

        private static ContingencyTable internalOperator(ContingencyTable a, ContingencyTable b, operatorType operatorType)
        {
            int aFirstRowIndex = a.FirstRowIndex;
            int aLastRowIndex = a.LastRowIndex;
            int aFirstColumnIndex = a.FirstColumnIndex;
            int aLastColumnIndex = a.LastColumnIndex;
            int bFirstRowIndex = b.FirstRowIndex;
            int bLastRowIndex = b.LastRowIndex;
            int bFirstColumnIndex = b.FirstColumnIndex;
            int bLastColumnIndex = b.LastColumnIndex;

            long aMultiplicator = b.Denominator;
            long bMultiplicator = a.Denominator;
            long resultDenominator = aMultiplicator * bMultiplicator;
            if (aMultiplicator == bMultiplicator)
            {
                resultDenominator = aMultiplicator;
                aMultiplicator = bMultiplicator = 1;
            }

            if ((aLastRowIndex - aFirstRowIndex) == (bLastRowIndex - bFirstRowIndex)
                && (aLastColumnIndex - aFirstColumnIndex) == (bLastColumnIndex - bFirstColumnIndex))
            {
                int aToBRowShift = bFirstRowIndex - aFirstRowIndex;
                int aToBColumnShift = bFirstColumnIndex - aFirstColumnIndex;
                long[,] result = new long[aLastRowIndex - aFirstRowIndex + 1, aLastColumnIndex - aFirstColumnIndex + 1];
                for (int rowIndex = aFirstRowIndex; rowIndex <= aLastRowIndex; rowIndex++)
                {
                    for (int columnIndex = aFirstColumnIndex; columnIndex <= aLastColumnIndex; columnIndex++)
                    {
                        if (operatorType == operatorType.Plus)
                        {
                            result[rowIndex - aFirstRowIndex, columnIndex - aFirstColumnIndex] =
                                a.table[rowIndex, columnIndex] * aMultiplicator
                                + b.table[rowIndex - aToBRowShift, columnIndex - aToBColumnShift] * bMultiplicator;
                        }
                        else if (operatorType == operatorType.Minus)
                        {
                            result[rowIndex - aFirstRowIndex, columnIndex - aFirstColumnIndex] =
                                Math.Abs(
                                    a.table[rowIndex, columnIndex] * aMultiplicator
                                    - b.table[rowIndex - aToBRowShift, columnIndex - aToBColumnShift] * bMultiplicator);
                        }
                    }
                }
                return new ContingencyTable(result, resultDenominator);
            }
            throw Ferda.Modules.Exceptions.BadParamsError(null, null, "Uncompatible sizes of contingecy tables to combine!", restrictionTypeEnum.BadFormat);
        }

        /// <summary>
        /// Operator <b>!=</b>.
        /// </summary>
        /// <param name="a">First contingecy table.</param>
        /// <param name="b">Second contingecy table.</param>
        /// <returns>
        /// <c>true</c> iff all corresponding items of specified tables (in space 
        /// restricted by their bounds) are not equal and also their denominators 
        /// are equals different; otherwise, <c>false</c>. 
        /// </returns>
        /// <exception cref="T:Ferda.Modules.BadParamsError">
        /// Thrown iff specified contingecy tables (more precisely its submatrixes)
        /// has differend shapes.
        /// </exception>
        public static bool operator !=(ContingencyTable a, ContingencyTable b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"/> is suitable for use in hashing algorithms and data structures like a hash table.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:Ferda.Modules.Quantifiers.ContingencyTable"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return firstColumnIndex
                + 3 * lastColumnIndex
                + 7 * firstRowIndex
                + 11 * lastRowIndex
                + 13 * (int)denominator
                + 17 * (int)SumOfValues;
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"></see> 
        /// is equal to the current <see cref="T:Ferda.Modules.Quantifiers.ContingencyTable"/>.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="T:System.Object"></see> to compare with the current 
        /// <see cref="T:Ferda.Modules.Quantifiers.ContingencyTable"/>.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="T:System.Object"></see> is equal to 
        /// the current <see cref="T:Ferda.Modules.Quantifiers.ContingencyTable"/>; 
        /// otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(Object obj)
        {
            ContingencyTable contingencyTable = obj as ContingencyTable;
            if (obj == null)
                return false;

            return this == contingencyTable;
        }

        /// <summary>
        /// ContingencyTable with corresponding denominator
        /// and the contingecy table, where Table_ij = (A_kl + B_mn). 
        /// (k, l, m, n indexes depends on bounds of input tables, usually
        /// k = m = i and l = n = j)
        /// </summary>
        /// <param name="a">First contingecy table.</param>
        /// <param name="b">Second contingecy table.</param>
        /// <returns>
        /// [Sub]matrix as result of adding by items. 
        /// </returns>
        /// <exception cref="T:Ferda.Modules.BadParamsError">
        /// Thrown iff specified contingecy tables (more precisely its submatrixes)
        /// has differend shapes.
        /// </exception>
        public static ContingencyTable operator +(ContingencyTable a, ContingencyTable b)
        {
            return internalOperator(a, b, operatorType.Plus);
        }

        /// <summary>
        /// ContingencyTable with corresponding denominator
        /// and the contingecy table, where Table_ij = (Abs(A_kl - B_mn)). 
        /// (k, l, m, n indexes depends on bounds of input tables, usually
        /// k = m = i and l = n = j)
        /// </summary>
        /// <param name="a">First contingecy table.</param>
        /// <param name="b">Second contingecy table.</param>
        /// <returns>
        /// [Sub]matrix as result of subtraction by items. 
        /// </returns>
        /// <exception cref="T:Ferda.Modules.BadParamsError">
        /// Thrown iff specified contingecy tables (more precisely its submatrixes)
        /// has differend shapes.
        /// </exception> 
        public static ContingencyTable operator -(ContingencyTable a, ContingencyTable b)
        {
            return internalOperator(a, b, operatorType.Minus);
        }

        /// <summary>
        /// Multiplies the denominator by the specified <c>result</c>.
        /// </summary>
        /// <param name="value">The multiplicator for the denomitor.</param>
        public void Div(long value)
        {
            this.denominator *= value;
        }
        #endregion

        #region Four Fold Table
        /// <summary>
        /// Gets a result indicating whether this instance is basic (i.e. a-frequency
        /// is on [0, 0] and and on the contrary in last row and column is d-frequency [1, 1])
        /// bottom-right) four fold table.
        /// </summary>
        /// <result>
        /// <c>true</c> if this instance is basic four fold table; otherwise, <c>false</c>.
        /// </result>
        public bool IsBasicFourFoldTable
        {
            get
            {
                return (FirstRowIndex == 0
                    && LastRowIndex == 1
                    && FirstColumnIndex == 0
                    && LastColumnIndex == 1);
            }
        }
        /// <summary>
        /// Gets a result indicating whether this instance is four fold table i.e. its shape
        /// is 2x2.
        /// </summary>
        /// <result>
        /// <c>true</c> if this instance is four fold table; otherwise, <c>false</c>.
        /// </result>
        public bool IsFourFoldTable
        {
            get
            {
                return (
                    (LastRowIndex - FirstRowIndex + 1) == 2
                    && (LastColumnIndex - FirstColumnIndex + 1) == 2
                    );
            }
        }
        #endregion

        #region Nine Fold Table
        /// <summary>
        /// Gets a result indicating whether this instance is basic (i.e. a-frequency
        /// is on [0, 0] and and on the contrary in last row and column is d-frequency [2, 2])
        /// bottom-right) nine fold table.
        /// </summary>
        /// <result>
        /// <c>true</c> if this instance is basic nine fold table; otherwise, <c>false</c>.
        /// </result>
        public bool IsBasicNineFoldTable
        {
            get
            {
                return (FirstRowIndex == 0
                    && LastRowIndex == 2
                    && FirstColumnIndex == 0
                    && LastColumnIndex == 2);
            }
        }
        /// <summary>
        /// Gets a result indicating whether this instance is four fold table i.e. its shape
        /// is 3x3.
        /// </summary>
        /// <result>
        /// <c>true</c> if this instance is four fold table; otherwise, <c>false</c>.
        /// </result>
        public bool IsNineFoldTable
        {
            get
            {
                return (
                    (LastRowIndex - FirstRowIndex + 1) == 3
                    && (LastColumnIndex - FirstColumnIndex + 1) == 3
                    );
            }
        }
        #endregion

        #region OneDimensional Table
        /// <summary>
        /// Gets a result indicating whether this instance is basic one dimensional 
        /// table. (first row index equals to last row index equals to zero)
        /// </summary>
        /// <result>
        /// <c>true</c> if this instance is basic one dimensional table; 
        /// otherwise, <c>false</c>.
        /// </result>
        public bool IsBasicOneDimensionalTable
        {
            get
            {
                return (FirstRowIndex == 0
                    && LastRowIndex == 0);
            }
        }
        /// <summary>
        /// Gets a result indicating whether this instance is one dimensional 
        /// table. (first row index equals to last row index)
        /// </summary>
        /// <result>
        /// <c>true</c> if this instance is one dimensional; otherwise, <c>false</c>.
        /// </result>
        public bool IsOneDimensional
        {
            get
            {
                return ((LastRowIndex - FirstRowIndex) == 0);
            }
        }
        #endregion

        #region Bounds (Int32 or RangeEnum)
        /// <summary>
        /// Tries to parse and evaluate <see cref="T:Ferda.Modules.RangeEnum"/> 
        /// or integer from input string (one-based) and returns 
        /// (zero-based) index of column/row.
        /// </summary>
        /// <param name="lastItemIndex">Index of last possible item from range (counted from zero).</param>
        /// <param name="oneBasedBound">String representing <see cref="T:Ferda.Modules.RangeEnum"/> or integer &lt; 0. Bound is counted from 1.</param>
        /// <returns>Bound counted from zero.</returns>
        /// <exception cref="T:Ferda.Modules.BadParamsError">
        /// Thrown iff parsing of param <c>oneBasedBound</c> was unsuccessful.
        /// </exception>
        protected static int bound(int lastItemIndex, string oneBasedBound)
        {
            int zeroBasedBound = Parsing.ZeroBasedBoundFromOneBasedString(oneBasedBound);
            if (zeroBasedBound == -1)
                return lastItemIndex;
            else if (zeroBasedBound == -2)
                return lastItemIndex / 2;
            else
                return (zeroBasedBound <= lastItemIndex) ? zeroBasedBound : lastItemIndex;
        }

        /// <summary>
        /// Set index of first row item (one-based i.e. counted form 1) in contingency table.
        /// </summary>
        /// <remarks>
        /// Allowed values are from <see cref="T:System.Int32"/> domain or 
        /// <see cref="T:Ferda.Modules.RangeEnum"/> enumeration.
        /// </remarks>
        /// <exception cref="T:Ferda.Modules.BadParamsError">
        /// Thrown iff parsing was unsuccessful.
        /// </exception>
        public string StartRowBound
        {
            set
            {
                firstRowIndex = bound(MaxRowIndex, value);
            }
        }

        /// <summary>
        /// Set index of last row item (one-based i.e. counted form 1) in contingency table.
        /// </summary>
        /// <remarks>
        /// Allowed values are from <see cref="T:System.Int32"/> domain or 
        /// <see cref="T:Ferda.Modules.RangeEnum"/> enumeration.
        /// </remarks>
        /// <exception cref="T:Ferda.Modules.BadParamsError">
        /// Thrown iff parsing was unsuccessful.
        /// </exception>
        public string EndRowBound
        {
            set
            {
                lastRowIndex = bound(MaxRowIndex, value);
            }
        }

        /// <summary>
        /// Set index of first column item (one-based i.e. counted form 1) in contingency table.
        /// </summary>
        /// <remarks>
        /// Allowed values are from <see cref="T:System.Int32"/> domain or 
        /// <see cref="T:Ferda.Modules.RangeEnum"/> enumeration.
        /// </remarks>
        /// <exception cref="T:Ferda.Modules.BadParamsError">
        /// Thrown iff parsing was unsuccessful.
        /// </exception>
        public string StartColumnBound
        {
            set
            {
                firstColumnIndex = bound(MaxColumnIndex, value);
            }
        }

        /// <summary>
        /// Set index of last column item (one-based i.e. counted form 1) in contingency table.
        /// </summary>
        /// <remarks>
        /// Allowed values are from <see cref="T:System.Int32"/> domain or 
        /// <see cref="T:Ferda.Modules.RangeEnum"/> enumeration.
        /// </remarks>
        /// <exception cref="T:Ferda.Modules.BadParamsError">
        /// Thrown iff parsing was unsuccessful.
        /// </exception>
        public string EndColumnBound
        {
            set
            {
                lastColumnIndex = bound(MaxColumnIndex, value);
            }
        }
        #endregion

        #region Indexes (fist/last/max row/column item index)
        private int firstRowIndex = -1;
        /// <summary>
        /// Gets the index of the first row.
        /// </summary>
        /// <value>The index of the first row.</value>
        public int FirstRowIndex
        {
            get
            {
                if (firstRowIndex >= 0)
                    return firstRowIndex;
                else
                    return 0;
            }
        }

        private int lastRowIndex = -1;
        /// <summary>
        /// Gets the index of the last row.
        /// </summary>
        /// <value>The index of the last row.</value>
        public int LastRowIndex
        {
            get
            {
                if (lastRowIndex >= 0)
                    return lastRowIndex;
                else
                    return MaxRowIndex;
            }
        }

        /// <summary>
        /// Gets the index of the max row.
        /// </summary>
        /// <value>The index of the max row.</value>
        public int MaxRowIndex
        {
            get
            {
                return table.GetLength(0) - 1;
            }
        }
        
        private int firstColumnIndex = -1;
        /// <summary>
        /// Gets the index of the first column.
        /// </summary>
        /// <value>The index of the first column.</value>
        public int FirstColumnIndex
        {
            get
            {
                if (firstColumnIndex >= 0)
                    return firstColumnIndex;
                else
                    return 0;
            }
        }
        
        private int lastColumnIndex = -1;
        /// <summary>
        /// Gets the index of the last column.
        /// </summary>
        /// <value>The index of the last column.</value>
        public int LastColumnIndex
        {
            get
            {
                if (lastColumnIndex >= 0)
                    return lastColumnIndex;
                else
                    return MaxColumnIndex;
            }
        }

        /// <summary>
        /// Gets the index of the max column.
        /// </summary>
        /// <value>The index of the max column.</value>
        public int MaxColumnIndex
        {
            get
            {
                return table.GetLength(1) - 1;
            }
        }

        /// <summary>
        /// Gets the count of used cells i.e. number of items of the 
        /// contingency tables in space restricted by the bounds.
        /// </summary>
        /// <result>The count of used cells.</result>
        public int CountOfUsedCells
        {
            get
            {
                return (LastRowIndex - FirstRowIndex + 1) * (LastColumnIndex - FirstColumnIndex + 1);
            }
        }
        #endregion

        #endregion

        #region Comparing (2 numbers, RelationEnum)
        /// <summary>
        /// Compares the two numbers by specifiec <c>relation</c>.
        /// </summary>
        /// <param name="leftSideNumber">The left side number.</param>
        /// <param name="relation">The relation.</param>
        /// <param name="rightSideNumber">The right side number.</param>
        /// <returns>True iff both specified numbers are in specified <c>relation</c>.</returns>
        public static bool Compare(double leftSideNumber, RelationEnum relation, double rightSideNumber)
        {
            switch (relation)
            {
                case RelationEnum.Equal:
                    return leftSideNumber == rightSideNumber;
                case RelationEnum.GreaterThan:
                    return leftSideNumber > rightSideNumber;
                case RelationEnum.GreaterThanOrEqual:
                    return leftSideNumber >= rightSideNumber;
                case RelationEnum.LessThan:
                    return leftSideNumber < rightSideNumber;
                case RelationEnum.LessThanOrEqual:
                    return leftSideNumber <= rightSideNumber;
                default:
                    throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(relation);
            }
        }
        #endregion

        #region Units converting (result, [Core]UnitsEnum, ...)

        /// <summary>
        /// Converts specified <c>value</c> by data in <c>table</c> 
        /// and <c>allObjectsCount</c> according to specified 
        /// <c>units</c>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="units">The units.</param>
        /// <param name="table">
        /// The table (used to determine some denominator if 
        /// specified <c>units</c> depends on values in the 
        /// contingency table).
        /// </param>
        /// <param name="allObjectsCount">
        /// All objects count (used iff specified <c>units</c> 
        /// depends on count of all objects in data matrix 
        /// (not the same as count of all objects in contingency 
        /// table)).
        /// </param>
        /// <returns>Converted value.</returns>
        public static double ConvertUnits(double value, UnitsEnum units, ContingencyTable table, long allObjectsCount)
        {
            switch (units)
            {
                case UnitsEnum.AbsoluteNumber:
                    return value;
                case UnitsEnum.RelativeToActCondition:
                    return value / table.SumOfValues;
                case UnitsEnum.RelativeToAllObjects:
                    return value / allObjectsCount;
                case UnitsEnum.RelativeToMaxFrequency:
                    return value / table.MaxValue;
                default:
                    throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(units);
            }
        }
        #endregion

        #region Combining (Operation modes ... 1st set / 2nd set / diff of abs freq / diff of rel freq / diff of [abs] val)
        /// <summary>
        /// Difference of two contingency tables (A - B).
        /// </summary>
        /// <typeparam name="T">Subtype of <see cref="T:Ferda.Modules.Quantifiers.ContingencyTable"/>.</typeparam>
        /// <param name="a">The contingency table A.</param>
        /// <param name="b">The contingency table B.</param>
        /// <returns>Contingency table as result of A - B expression.</returns>
        private static T Minus<T>(T a, T b)
            where T : ContingencyTable, new()
        {
            ContingencyTable result = internalOperator(a, b, operatorType.Minus);
            T resultObject = new T();
            resultObject.Table = result.Table;
            resultObject.Denominator = result.Denominator;
            return resultObject;
        }

        /// <summary>
        /// Combines specified contingency tables (A and B).
        /// </summary>
        /// <typeparam name="T">Subtype of <see cref="T:Ferda.Modules.Quantifiers.ContingencyTable"/>.</typeparam>
        /// <param name="a">The contingency table A.</param>
        /// <param name="b">The contingency table B.</param>
        /// <param name="operationMode">The operation mode.</param>
        /// <returns>One contingency table as result of combination two contingecy tables (A and B).</returns>
        public static T Combine<T>(T a, T b, OperationModeEnum operationMode)
            where T : ContingencyTable, new()
        {
            switch (operationMode)
            {
                case OperationModeEnum.FirstSetFrequencies:
                    return a;
                case OperationModeEnum.SecondSetFrequencies:
                    return b;
                case OperationModeEnum.DifferencesOfAbsoluteFrequencies:
                    return ContingencyTable.Minus<T>(a, b);
                case OperationModeEnum.DifferencesOfRelativeFrequencies:
                    a.Div((long)a.SumOfValues);
                    b.Div((long)b.SumOfValues);
                    return ContingencyTable.Minus<T>(a, b);
                default:
                    throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(operationMode);
            }
        }

        /// <summary>
        /// Combines the specified values of one quantifier 
        /// evaluated over two (first and second) contingency tables.
        /// </summary>
        /// <param name="firstQuantifierValue">The first quantifier value.</param>
        /// <param name="secondQuantifierValue">The second quantifier value.</param>
        /// <param name="operationMode">The operation mode.</param>
        /// <returns>Combined value of two quntifiered values.</returns>
        public static double Combine(double firstQuantifierValue, double secondQuantifierValue, OperationModeEnum operationMode)
        {
            switch (operationMode)
            {
                case OperationModeEnum.AbsoluteDifferenceOfQuantifierValues:
                    return Math.Abs(firstQuantifierValue - secondQuantifierValue);
                case OperationModeEnum.DifferenceOfQuantifierValues:
                    return firstQuantifierValue - secondQuantifierValue;
                default:
                    throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(operationMode);
            }
        }

        /// <summary>
        /// Determines whether the specified <c>operationMode</c>
        /// leads to some operations before or after separated quantifier evaluation
        /// over two contingency tables.
        /// </summary>
        /// <param name="operationMode">The operation mode.</param>
        /// <returns>
        /// 	<c>true</c> if the specivied <c>operationMode</c> leads
        /// to some computation with quangifier values (computed 
        /// separately over two contingecy tables); otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOperationModeOverQuantifierValues(OperationModeEnum operationMode)
        {
            return (operationMode == OperationModeEnum.AbsoluteDifferenceOfQuantifierValues
                || operationMode == OperationModeEnum.DifferenceOfQuantifierValues);
        }

        #endregion

        #region Aggregation Quantifiers
        /// <summary>
        /// Gets the max value.
        /// </summary>
        /// <value>The max value.</value>
        public double MaxValue
        {
            get
            {
                double result = Double.NegativeInfinity;
                int lastColumnIndex = LastColumnIndex;
                for (int rowIndex = FirstRowIndex; rowIndex <= LastRowIndex; rowIndex++)
                {
                    for (int columnIndex = FirstColumnIndex; columnIndex <= lastColumnIndex; columnIndex++)
                    {
                        result = Math.Max(result, table[rowIndex, columnIndex]);
                    }
                }
                return result / (double)denominator;
            }
        }

        /// <summary>
        /// Gets the max value.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public static double GetMaxValue(ContingencyTable table)
        {
            return table.MaxValue;
        }

        /// <summary>
        /// Gets the min value.
        /// </summary>
        /// <value>The min value.</value>
        public double MinValue
        {
            get
            {
                double result = Double.PositiveInfinity;
                int lastColumnIndex = LastColumnIndex;
                for (int rowIndex = FirstRowIndex; rowIndex <= LastRowIndex; rowIndex++)
                {
                    for (int columnIndex = FirstColumnIndex; columnIndex <= lastColumnIndex; columnIndex++)
                    {
                        result = Math.Min(result, table[rowIndex, columnIndex]);
                    }
                }
                return result / (double)denominator;
            }
        }

        /// <summary>
        /// Gets the min value.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public static double GetMinValue(ContingencyTable table)
        {
            return table.MinValue;
        }

        /// <summary>
        /// Gets the sum of values.
        /// </summary>
        /// <value>The sum of values.</value>
        public double SumOfValues
        {
            get
            {
                double result = 0;
                int lastColumnIndex = LastColumnIndex;
                for (int rowIndex = FirstRowIndex; rowIndex <= LastRowIndex; rowIndex++)
                {
                    for (int columnIndex = FirstColumnIndex; columnIndex <= lastColumnIndex; columnIndex++)
                    {
                        result += table[rowIndex, columnIndex];
                    }
                }
                return result / (double)denominator;
            }
        }

        /// <summary>
        /// Gets the sum of values.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public static double GetSumOfValues(ContingencyTable table)
        {
            return table.SumOfValues;
        }

        /// <summary>
        /// Gets the average value.
        /// </summary>
        /// <value>The average value.</value>
        public double AverageValue
        {
            get
            {
                return SumOfValues / (double)CountOfUsedCells * (double)denominator;
            }
        }

        /// <summary>
        /// Gets the average value.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public static double GetAverageValue(ContingencyTable table)
        {
            return table.AverageValue;
        }
        #endregion

        #region AnyValue
        /// <summary>
        /// Test if any value of the contingency table 
        /// (converted to specified <c>units</c>) is in 
        /// specified <c>relation</c> to specified <c>treshold</c>.
        /// If test succedd the (first) successfully found value is returned
        /// in <c>out value</c>.
        /// </summary>
        /// <param name="relation">The relation.</param>
        /// <param name="treshold">The treshold.</param>
        /// <param name="units">The units.</param>
        /// <param name="allObjectsCount">All objects count (needed and used if units are UnitsEnum.RelativeToAllObjects).</param>
        /// <param name="value">The value.</param>
        /// <returns>True if any value of the contingency table satisfies the specified condition.</returns>
        public bool AnyValue(RelationEnum relation, double treshold, UnitsEnum units, long? allObjectsCount, out double value)
        {
            value = 0;
            double multipledTreshold = treshold * denominator;
            switch (units)
            {
                case UnitsEnum.AbsoluteNumber:
                    break;
                case UnitsEnum.RelativeToActCondition:
                    multipledTreshold = multipledTreshold * this.SumOfValues / 100;
                    break;
                case UnitsEnum.RelativeToMaxFrequency:
                    multipledTreshold = multipledTreshold * this.MaxValue / 100;
                    break;
                case UnitsEnum.RelativeToAllObjects:
                    if (allObjectsCount.HasValue)
                        multipledTreshold = multipledTreshold * allObjectsCount.Value / 100;
                    else
                        throw new Exception("optionalNumberForRelativeComparing result needed for UnitsEnum.RelativeToAllObjects");
                    break;
                default:
                    throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(units);
            }
            foreach (long item in table)
                if (Compare(item, relation, multipledTreshold))
                {
                    value = item;
                    return true;
                }
            return false;
        }
        #endregion

        #region Generic "Value" functions with delegates

        /// <summary>
        /// Delegate of quantifier over a subtype of <see cref="T:Ferda.Modules.Quantifiers.ContingencyTable"/>.
        /// </summary>
        /// <typeparam name="T">Subtype of <see cref="T:Ferda.Modules.Quantifiers.ContingencyTable"/>.</typeparam>
        /// <param name="table">Instance of subtype of <see cref="T:Ferda.Modules.Quantifiers.ContingencyTable"/>.</param>
        /// <returns>Double value of the quantifier.</returns>
        public delegate double QuantifierValue<T>(T table);

        /// <summary>
        /// Gets the specified quantifier value.
        /// </summary>
        /// <typeparam name="T">Subtype of <see cref="T:Ferda.Modules.Quantifiers.ContingencyTable"/>.</typeparam>
        /// <param name="quantifierValue">The quantifier value.</param>
        /// <param name="firstTable">The first table.</param>
        /// <param name="secondTable">The second table.</param>
        /// <param name="operationMode">The operation mode.</param>
        /// <returns>The value of specified quantifier.</returns>
        public static double Value<T>(QuantifierValue<T> quantifierValue, T firstTable, T secondTable, OperationModeEnum operationMode)
            where T : ContingencyTable, new()
        {
            if (IsOperationModeOverQuantifierValues(operationMode))
            {
                return Combine(
                    quantifierValue(firstTable),
                    quantifierValue(secondTable),
                    operationMode);
            }
            else
            {
                return quantifierValue(
                    ContingencyTable.Combine<T>(firstTable, secondTable, operationMode)
                    );
            }
        }

        /// <summary>
        /// Gets the specified quantifier value.
        /// </summary>
        /// <typeparam name="T">Subtype of <see cref="T:Ferda.Modules.Quantifiers.ContingencyTable"/>.</typeparam>
        /// <param name="quantifierValue">The quantifier value.</param>
        /// <param name="table">The table.</param>
        /// <param name="units">The units.</param>
        /// <param name="allObjectsCount">All objects count.</param>
        /// <returns>The value of specified quantifier.</returns>
        public static double Value<T>(QuantifierValue<T> quantifierValue, T table, UnitsEnum units, long allObjectsCount)
            where T : ContingencyTable
        {
            return ContingencyTable.ConvertUnits(quantifierValue(table), units, table, allObjectsCount);
        }

        /// <summary>
        /// Gets the specified quantifier value.
        /// </summary>
        /// <typeparam name="T">Subtype of <see cref="T:Ferda.Modules.Quantifiers.ContingencyTable"/>.</typeparam>
        /// <param name="quantifierValue">The quantifier value.</param>
        /// <param name="table">The table.</param>
        /// <returns>The value of specified quantifier.</returns>
        public static double Value<T>(QuantifierValue<T> quantifierValue, T table)
            where T : ContingencyTable
        {
            return quantifierValue(table);
        }

        /// <summary>
        /// Gets the specified quantifier value.
        /// </summary>
        /// <typeparam name="T">Subtype of <see cref="T:Ferda.Modules.Quantifiers.ContingencyTable"/>.</typeparam>
        /// <param name="quantifierValue">The quantifier value.</param>
        /// <param name="firstTable">The first table.</param>
        /// <param name="secondTable">The second table.</param>
        /// <param name="operationMode">The operation mode.</param>
        /// <param name="units">The units.</param>
        /// <param name="allObjectsCount">All objects count.</param>
        /// <returns>The value of specified quantifier.</returns>
        public static double Value<T>(QuantifierValue<T> quantifierValue, T firstTable, T secondTable, OperationModeEnum operationMode, UnitsEnum units, long allObjectsCount)
            where T : ContingencyTable, new()
        {
            if (IsOperationModeOverQuantifierValues(operationMode))
            {
                return Combine(
                    ContingencyTable.ConvertUnits(quantifierValue(firstTable), units, firstTable, allObjectsCount),
                    ContingencyTable.ConvertUnits(quantifierValue(secondTable), units, secondTable, allObjectsCount),
                    operationMode);
            }
            else
            {
                T combinedTable = ContingencyTable.Combine<T>(firstTable, secondTable, operationMode);
                return ContingencyTable.ConvertUnits(
                    quantifierValue(combinedTable), 
                    units, 
                    combinedTable, 
                    allObjectsCount);
            }
        }

        #endregion
    }

    /// <summary>
    /// Provides some computations that are lazy evaluated (when requested)
    /// and cached.
    /// </summary>
    public class PreparedSums
    {
        private long[] rowSums;
        /// <summary>
        /// Sum of all values in row [r]. Denominator is not applied.
        /// </summary>
        public long[] RowSums
        {
            get
            {
                compute();
                return rowSums;
            }
        }

        private long[] columnSums;
        /// <summary>
        /// Sum of all values in column [c]. Denominator is not applied.
        /// </summary>
        public long[] ColumnSums
        {
            get
            {
                compute();
                return columnSums;
            }
        }

        private long totalSum;
        /// <summary>
        /// Sum of all values in table. Denominator is not applied
        /// </summary>
        public long TotalSum
        {
            get
            {
                compute();
                return totalSum;
            }
        }

        private ContingencyTable contingencyTable;

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="T:Ferda.Modules.Quantifiers.PreparedSums"/> class.
        /// </summary>
        /// <param name="table">The table.</param>
        public PreparedSums(ContingencyTable table)
        {
            this.contingencyTable = table;
        }

        private bool computed = false;
        /// <summary>
        /// Computes (prepares) all sums if it is not yet.
        /// </summary>
        private void compute()
        {
            if (!computed)
                Refresh();
        }
        /// <summary>
        /// Refreshes prepared sums.
        /// </summary>
        /// <remarks>
        /// Useful when ContingencyTable.table changed.
        /// </remarks>
        public void Refresh()
        {
            rowSums = new long[contingencyTable.LastRowIndex - contingencyTable.FirstRowIndex + 1];
            rowSums.Initialize();
            columnSums = new long[contingencyTable.LastColumnIndex - contingencyTable.FirstColumnIndex + 1];
            columnSums.Initialize();
            int lastColumnIndex = contingencyTable.LastColumnIndex;
            int firstColumnIndex = contingencyTable.FirstColumnIndex;
            for (int rowIndex = contingencyTable.FirstRowIndex; rowIndex <= contingencyTable.LastRowIndex; rowIndex++)
            {
                for (int columnIndex = firstColumnIndex; columnIndex <= lastColumnIndex; columnIndex++)
                {
                    rowSums[rowIndex] += contingencyTable.Table[rowIndex, columnIndex];
                    columnSums[columnIndex] += contingencyTable.Table[rowIndex, columnIndex];
                }
                totalSum += rowSums[rowIndex];
            }
            this.computed = true;
        }
    }
}
/*
        int firstRowIndex = FirstRowIndex;
        int lastRowIndex = LastRowIndex;

        int firstColumnIndex = FirstColumnIndex;
        int lastColumnIndex = LastColumnIndex;
        for (int rowIndex = FirstRowIndex; rowIndex <= LastRowIndex; rowIndex++)
        {
            for (int columnIndex = firstColumnIndex; columnIndex <= lastColumnIndex; columnIndex++)
            {
                double item = table[rowIndex, columnIndex];

            }
        }
*/