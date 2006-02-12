using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Modules.Helpers.Common;

namespace Ferda.Modules.Quantifiers
{
    public class ContingencyTable
    {
        #region Core Functions

        #region Constructors
        public ContingencyTable()
        {
            PreparedSums = new PreparedSums(this);
        }
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
        /// Constructor (with default denominator=1).
        /// </summary>
        /// <param name="contingencyTable">Array of contingency table rows. 
        /// Value of table[0][0] is result of A.</param>
        public ContingencyTable(long[,] contingencyTable)
        {
            PreparedSums = new PreparedSums(this);

            Table = contingencyTable;
        }

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

        protected long denominator = 1;
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

        protected long[,] table;
        /// <summary>
        /// Table [r, c], <c>r</c> is index of row, <c>c</c> is index of column.
        /// At position [0, 0] is item at first row and first column, 
        /// if you like <b>a-frequency</b>.
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

        public override int GetHashCode()
        {
            return firstColumnIndex
                + 3 * lastColumnIndex
                + 7 * firstRowIndex
                + 11 * lastRowIndex
                + 13 * (int)denominator
                + 17 * (int)SumOfValuesAggregation;
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
        /// <param name="result">The multiplicator for the denomitor.</param>
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
        int firstRowIndex = -1;
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

        int lastRowIndex = -1;
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
        public int MaxRowIndex
        {
            get
            {
                return table.GetLength(0) - 1;
            }
        }
        int firstColumnIndex = -1;
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
        int lastColumnIndex = -1;
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

        #region Comparing (2 numbers, [Core]RelationEnum)
        public static bool Compare(double leftSideNumber, CoreRelationEnum relation, double rightSideNumber)
        {
            switch (relation)
            {
                case CoreRelationEnum.GreaterThanOrEqualCore:
                    return leftSideNumber >= rightSideNumber;
                case CoreRelationEnum.LessThanOrEqualCore:
                    return leftSideNumber <= rightSideNumber;
                default:
                    throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(relation);
            }
        }
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
        public double ConvertUnits(double value, CoreUnitsEnum units)
        {
            switch (units)
            {
                case CoreUnitsEnum.AbsoluteNumberCore:
                    return value;
                case CoreUnitsEnum.RelativeNumberCore:
                    return value / this.SumOfValuesAggregation;
                default:
                    throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(units);
            }
        }
        public static double ConvertUnits(double value, CoreUnitsEnum units, long sum)
        {
            switch (units)
            {
                case CoreUnitsEnum.AbsoluteNumberCore:
                    return value;
                case CoreUnitsEnum.RelativeNumberCore:
                    return value / sum;
                default:
                    throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(units);
            }
        }
        public static double ConvertUnits(ContingencyTable table, double value, UnitsEnum units, long allObjectsCount)
        {
            switch (units)
            {
                case UnitsEnum.AbsoluteNumber:
                    return value;
                case UnitsEnum.RelativeToActCondition:
                    return value / table.SumOfValuesAggregation;
                case UnitsEnum.RelativeToAllObjects:
                    return value / allObjectsCount;
                case UnitsEnum.RelativeToMaxFrequency:
                    return value / table.MaxValueAggregation;
                default:
                    throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(units);
            }
        }
        public double ConvertUnits(double value, UnitsEnum units, long allObjectsCount)
        {
            return ContingencyTable.ConvertUnits(this, value, units, allObjectsCount);
        }
        #endregion

        #region Combining (Operation modes ... 1st set / 2nd set / diff of abs freq / diff of rel freq / diff of [abs] val)
        private static T Minus<T>(T a, T b)
            where T : ContingencyTable, new()
        {
            ContingencyTable result = internalOperator(a, b, operatorType.Minus);
            T resultObject = new T();
            resultObject.Table = result.Table;
            resultObject.Denominator = result.Denominator;
            return resultObject;
        }

        private static T Combine<T>(T a, T b, OperationModeEnum operationMode)
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
                    a.Div((long)a.SumOfValuesAggregation);
                    b.Div((long)b.SumOfValuesAggregation);
                    return ContingencyTable.Minus<T>(a, b);
                default:
                    throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(operationMode);
            }
        }

        public static ContingencyTable Combine(ContingencyTable a, ContingencyTable b, OperationModeEnum operationMode)
        {
            switch (operationMode)
            {
                case OperationModeEnum.FirstSetFrequencies:
                    return a;
                case OperationModeEnum.SecondSetFrequencies:
                    return b;
                case OperationModeEnum.DifferencesOfAbsoluteFrequencies:
                    return a - b;
                case OperationModeEnum.DifferencesOfRelativeFrequencies:
                    a.Div((long)a.SumOfValuesAggregation);
                    b.Div((long)b.SumOfValuesAggregation);
                    return a - b;
                default:
                    throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(operationMode);
            }
        }

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

        public static bool IsOperationModeOverQuantifierValues(OperationModeEnum operationMode)
        {
            return (operationMode == OperationModeEnum.AbsoluteDifferenceOfQuantifierValues
                || operationMode == OperationModeEnum.DifferenceOfQuantifierValues);
        }

        #endregion

        #region Aggregation Quantifiers
        public static double MaxValueAggregationValue(ContingencyTable table)
        {
            return table.MaxValueAggregation;
        }
        public double MaxValueAggregation
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
        public static double MinValueAggregationValue(ContingencyTable table)
        {
            return table.MinValueAggregation;
        }
        public double MinValueAggregation
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
        public static double SumOfValuesAggregationValue(ContingencyTable table)
        {
            return table.SumOfValuesAggregation;
        }
        public double SumOfValuesAggregation
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
        public static double AverageValueAggregationValue(ContingencyTable table)
        {
            return table.AverageValueAggregation;
        }
        public double AverageValueAggregation
        {
            get
            {
                return SumOfValuesAggregation / (double)CountOfUsedCells * (double)denominator;
            }
        }
        #endregion

        #region AnyValue
        public bool AnyValue(RelationEnum relation, double treshold, UnitsEnum units, long? allObjectsCount, out double value)
        {
            value = 0;
            double multipledTreshold = treshold * denominator;
            switch (units)
            {
                case UnitsEnum.AbsoluteNumber:
                    break;
                case UnitsEnum.RelativeToActCondition:
                    multipledTreshold = multipledTreshold * this.SumOfValuesAggregation / 100;
                    break;
                case UnitsEnum.RelativeToMaxFrequency:
                    multipledTreshold = multipledTreshold * this.MaxValueAggregation / 100;
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

        public delegate double QuantifierValue<T>(T table);

        public delegate double QuantifierValueWithDirection<T>(T table, DirectionEnum direction);

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

        public static double Value<T>(QuantifierValue<T> quantifierValue, T table, UnitsEnum units, long allObjectsCount)
            where T : ContingencyTable
        {
            return ContingencyTable.ConvertUnits(table, quantifierValue(table), units, allObjectsCount);
        }

        public static double Value<T>(QuantifierValue<T> quantifierValue, T table)
            where T : ContingencyTable
        {
            return quantifierValue(table);
        }

        public static double Value<T>(QuantifierValue<T> quantifierValue, T firstTable, T secondTable, OperationModeEnum operationMode, UnitsEnum units, long allObjectsCount)
            where T : ContingencyTable, new()
        {
            if (IsOperationModeOverQuantifierValues(operationMode))
            {
                return Combine(
                    ContingencyTable.ConvertUnits(firstTable, quantifierValue(firstTable), units, allObjectsCount),
                    ContingencyTable.ConvertUnits(secondTable, quantifierValue(secondTable), units, allObjectsCount),
                    operationMode);
            }
            else
            {
                T combinedTable = ContingencyTable.Combine<T>(firstTable, secondTable, operationMode);
                //Michale: tohle pretypovani spadne
                return ContingencyTable.ConvertUnits(
                    combinedTable,
                    quantifierValue(combinedTable),
                    units,
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
        protected long[] rowSums;
        /// <summary>
        /// Sum of all values in row [r]. Denominator is not applied.
        /// </summary>
        public long[] RowSums
        {
            get
            {
                Compute();
                return rowSums;
            }
        }

        protected long[] columnSums;
        /// <summary>
        /// Sum of all values in column [c]. Denominator is not applied.
        /// </summary>
        public long[] ColumnSums
        {
            get
            {
                Compute();
                return columnSums;
            }
        }

        protected long totalSum;
        /// <summary>
        /// Sum of all values in table. Denominator is not applied
        /// </summary>
        public long TotalSum
        {
            get
            {
                Compute();
                return totalSum;
            }
        }

        protected ContingencyTable contingencyTable;

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
        protected void Compute()
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