using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Modules.Quantifiers
{
    public class ContingencyTable
    {
        #region CASTING
        /* CASTING
		//Resim to, ze spadne pretypovani na radcich (+-) 943 973
		public T Retype<T>(ContingencyTable table)
			where T : ContingencyTable
		{
				if (typeof(T) == typeof(ContingencyTable))
					return (T)table;
				else if (typeof(T) == typeof(FourFoldContingencyTable))
					return (T)ContingencyTable2FourFoldContingencyTable(table);
				else if (typeof(T) == typeof(TwoDimensionalContingencyTable))
					return (T)ContingencyTable2TwoDimensionalContingencyTable(table);
		}
	
		//public static explicit operator FourFoldContingencyTable(ContingencyTable contingecyTable)
		public static FourFoldContingencyTable ContingencyTable2FourFoldContingencyTable(ContingencyTable contingecyTable)
		{
			if (contingecyTable.IsBasicFourFoldTable)
				return new FourFoldContingencyTable(contingecyTable.table, contingecyTable.denominator);
			else if (contingecyTable.IsFourFoldTable)
			{
				int firstColumnIndex = contingecyTable.FirstColumnIndex;
				int firstRowIndex = contingecyTable.FirstRowIndex;
				return new FourFoldContingencyTable(
					contingecyTable.table[firstRowIndex, firstColumnIndex],
					contingecyTable.table[firstRowIndex, firstColumnIndex + 1],
					contingecyTable.table[firstRowIndex + 1, firstColumnIndex],
					contingecyTable.table[firstRowIndex + 1, firstColumnIndex + 1],
					contingecyTable.denominator);
			}
			else
				throw new InvalidCastException();
		}

		public static TwoDimensionalContingencyTable ContingencyTable2TwoDimensionalContingencyTable(ContingencyTable contingecyTable)
		{
			try
			{
				return new TwoDimensionalContingencyTable(contingecyTable.table, contingecyTable.denominator);
			}
			catch (Exception ex)
			{
				throw new InvalidCastException("", ex);
			}
		}
		*/
        #endregion

        #region Constructors
        public ContingencyTable()
        {
            denominator = 1;
        }

        public ContingencyTable(int[][] contingencyTable)
        {
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
            denominator = 1;
        }
        /// <summary>
        /// Constructor (with default denominator=1).
        /// </summary>
        /// <param name="contingencyTable">Array of contingency table rows. Value of table[0][0] is value of A.</param>
        public ContingencyTable(long[,] contingencyTable)
        {
            Table = contingencyTable;
            denominator = 1;
        }

        public ContingencyTable(long[,] contingencyTable, long denominator)
        {
            Table = contingencyTable;
            this.denominator = denominator;
        }
        #endregion

        #region Fields (table, denominator)
        protected long denominator = 1;
        public long Denominator
        {
            get { return denominator; }
            set { denominator *= value; }
        }

        protected long[,] table;
        /// <summary>
        /// Table [x][c], <c>x</c> is index of row, <c>c</c> is index of column.
        /// </summary>
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

        #region Arithmetic (operator+, operator-, operator==, operator!=, Div(value))
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
                aMultiplicator = bMultiplicator = 1;
                resultDenominator = aMultiplicator;
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
                + (int)SumOfValuesAggregation;
        }

        public override bool Equals(Object obj)
        {
            ContingencyTable contingencyTable = obj as ContingencyTable;
            if (obj == null)
                return false;

            return this == contingencyTable;
        }

        /// <summary>
        /// If submatrixes of <c>a</c> and <c>b</c> are not of the same 
        /// size throws Ferda.Modules.BadParamsError() otherwise
        /// returns new ContingencyTable with corresponding denominator
        /// as OutputContingencyTable_ij(Abs(A_kl + B_mn)).
        /// </summary>
        public static ContingencyTable operator +(ContingencyTable a, ContingencyTable b)
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
                aMultiplicator = bMultiplicator = 1;
                resultDenominator = aMultiplicator;
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
                        result[rowIndex - aFirstRowIndex, columnIndex - aFirstColumnIndex] =
                            Math.Abs(
                                a.table[rowIndex, columnIndex] * aMultiplicator
                                + b.table[rowIndex - aToBRowShift, columnIndex - aToBColumnShift] * bMultiplicator);
                    }
                }
                return new ContingencyTable(result, resultDenominator);
            }
            throw Ferda.Modules.Exceptions.BadParamsError(null, null, "Uncompatible sizes of contingecy tables to combine!", restrictionTypeEnum.BadFormat);
        }

        /// <summary>
        /// If submatrixes of <c>a</c> and <c>b</c> are not of the same 
        /// size throws Ferda.Modules.BadParamsError() otherwise
        /// returns new ContingencyTable with corresponding denominator
        /// as OutputContingencyTable_ij(Abs(A_kl - B_mn)).
        /// </summary>
        public static ContingencyTable operator -(ContingencyTable a, ContingencyTable b)
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
                aMultiplicator = bMultiplicator = 1;
                resultDenominator = aMultiplicator;
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
                        result[rowIndex - aFirstRowIndex, columnIndex - aFirstColumnIndex] =
                            Math.Abs(
                                a.table[rowIndex, columnIndex] * aMultiplicator
                                - b.table[rowIndex - aToBRowShift, columnIndex - aToBColumnShift] * bMultiplicator);
                    }
                }
                return new ContingencyTable(result, resultDenominator);
            }
            throw Ferda.Modules.Exceptions.BadParamsError(null, null, "Uncompatible sizes of contingecy tables to combine!", restrictionTypeEnum.BadFormat);
        }

        public void Div(long value)
        {
            this.denominator *= value;
        }
        #endregion

        #region Four Fold Table
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

        #region OneDimensional Table
        public bool IsOneDimensional
        {
            get
            {
                return ((LastRowIndex - FirstRowIndex + 1) == 1);
            }
        }
        #endregion

        #region Bounds (Int32 or RangeEnum)
        /// <summary>
        /// This function try to parse <see cref="T:Ferda.Modules.RangeEnum"/> item or integer value from <c>oneBasedBound</c>. First item has bound equal to 1!
        /// </summary>
        /// <param name="oneBasedBound">String representing <see cref="T:Ferda.Modules.RangeEnum"/> or integer &lt; 0. Bound is counted from 1.</param>
        /// <returns>
        /// <remarks>Input bound is from range 1 .. maxBound but output bound is from range 0 .. maxBound-1.</remarks>
        /// <para>Returns -1 iff RangeEnum.All was entered.</para>
        /// <para>Returns -2 iff RangeEnum.Half was entered.</para>
        /// <para>Otherwise returns parsed integer -1 .</para>.</returns>
        /// <exception cref="T:Ferda.Modules.Exceptions.BadParamsError">If parsing integer or RangeEnum from parameter <c>oneBasedBound</c> was unsuccesfull or parsed integer isn`t &lt; 0.</exception>
        public static int ZeroBasedBoundFromOneBasedString(string oneBasedBound)
        {
            try
            {
                RangeEnum rangeEnum = (RangeEnum)Enum.Parse(typeof(RangeEnum), oneBasedBound);
                if (rangeEnum == RangeEnum.All)
                    return -1;
                if (rangeEnum == RangeEnum.Half)
                    return -2;
            }
            catch (ArgumentException) { }
            try
            {
                int result = Convert.ToInt32(oneBasedBound);
                if (result <= 0)
                    throw Ferda.Modules.Exceptions.BadParamsError(null, null, "Bound have to greater than 0! Current bound is: " + oneBasedBound, restrictionTypeEnum.Minimum);
                return result - 1;
            }
            catch (ArgumentException ex)
            {
                throw Ferda.Modules.Exceptions.BadParamsError(ex, null, "Argument Exception converting bound " + oneBasedBound, restrictionTypeEnum.BadFormat);
            }
            catch (OverflowException ex)
            {
                throw Ferda.Modules.Exceptions.BadParamsError(ex, null, "Overflow Exception converting bound " + oneBasedBound, restrictionTypeEnum.BadFormat);
            }
            catch (FormatException ex)
            {
                throw Ferda.Modules.Exceptions.BadParamsError(ex, null, "Format Exception converting bound " + oneBasedBound, restrictionTypeEnum.BadFormat);
            }
        }
        /// <summary>
        /// This function try to parse <see cref="T:Ferda.Modules.RangeEnum"/> item or integer value from <c>zeroBasedBound</c>. First item has bound equal to 0!
        /// </summary>
        /// <param name="lastItemIndex">Index of last possible item from range (counted from zero).</param>
        /// <param name="zeroBasedBound">String representing <see cref="T:Ferda.Modules.RangeEnum"/> or integer &lt; 0. Bound is counted from 1.</param>
        /// <returns>Bound counted from zero.</returns>
        public static int Bound(int lastItemIndex, int zeroBasedBound)
        {
            if (zeroBasedBound == -1)
                return lastItemIndex;
            else if (zeroBasedBound == -2)
                return lastItemIndex / 2;
            else
                return (zeroBasedBound <= lastItemIndex) ? zeroBasedBound : lastItemIndex;
        }

        /// <summary>
        /// Set index of first row item (counted form 1) in contingency table.
        /// </summary>
        /// <remarks>Allowed values are form <c>Int32</c> domain or <c>RangeEnum</c> enumeration.</remarks>
        public string StartRowBound
        {
            set
            {
                firstRowIndex = Bound(MaxRowIndex, ZeroBasedBoundFromOneBasedString(value));
            }
        }

        /// <summary>
        /// Set index of last row item (counted form 1) in contingency table.
        /// </summary>
        /// <remarks>Allowed values are form <c>Int32</c> domain or <c>RangeEnum</c> enumeration.</remarks>
        public string EndRowBound
        {
            set
            {
                lastRowIndex = Bound(MaxRowIndex, ZeroBasedBoundFromOneBasedString(value));
            }
        }

        /// <summary>
        /// Set index of first column item (counted form 1) in contingency table.
        /// </summary>
        /// <remarks>Allowed values are form <c>Int32</c> domain or <c>RangeEnum</c> enumeration.</remarks>
        public string StartColumnBound
        {
            set
            {
                firstColumnIndex = Bound(MaxColumnIndex, ZeroBasedBoundFromOneBasedString(value));
            }
        }

        /// <summary>
        /// Set index of last column item (counted form 1) in contingency table.
        /// </summary>
        /// <remarks>Allowed values are form <c>Int32</c> domain or <c>RangeEnum</c> enumeration.</remarks>
        public string EndColumnBound
        {
            set
            {
                lastColumnIndex = Bound(MaxColumnIndex, ZeroBasedBoundFromOneBasedString(value));
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
        #endregion

        public int Count
        {
            get
            {
                return (LastRowIndex - FirstRowIndex + 1) * (LastColumnIndex - FirstColumnIndex + 1);
            }
        }

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

        #region Units converting (value, [Core]UnitsEnum, ...)
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
                aMultiplicator = bMultiplicator = 1;
                resultDenominator = aMultiplicator;
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
                        result[rowIndex - aFirstRowIndex, columnIndex - aFirstColumnIndex] =
                            Math.Abs(
                                a.table[rowIndex, columnIndex] * aMultiplicator
                                - b.table[rowIndex - aToBRowShift, columnIndex - aToBColumnShift] * bMultiplicator);
                    }
                }
                T resultObject = new T();
                resultObject.Table = result;
                resultObject.Denominator = resultDenominator;
            }
            throw Ferda.Modules.Exceptions.BadParamsError(null, null, "Uncompatible sizes of contingecy tables to combine!", restrictionTypeEnum.BadFormat);
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
        #endregion

        public static bool IsOperationModeOverQuantifierValues(OperationModeEnum operationMode)
        {
            if (operationMode == OperationModeEnum.AbsoluteDifferenceOfQuantifierValues
                || operationMode == OperationModeEnum.DifferenceOfQuantifierValues)
                return true;
            return false;
        }

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
                return SumOfValuesAggregation / (double)Count * (double)denominator;
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
                        throw new Exception("optionalNumberForRelativeComparing value needed for UnitsEnum.RelativeToAllObjects");
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

        #region Row and Column Sums (rowSum, columnSum, allSum) denominator is not processed
        /// <summary>
        /// Sum of all values in row [x]. Index is number of row.
        /// </summary>
        protected long[] rowSums;
        /// <summary>
        /// Sum of all values in column [c]. Index is number of column.
        /// </summary>
        protected long[] columnSums;
        /// <summary>
        /// Sum of all values in table;
        /// </summary>
        protected long allSum;
        protected bool doneComputeRowAndColumnSums = false;
        public void ComputeRowAndColumnSums()
        {
            if (doneComputeRowAndColumnSums)
                return;
            rowSums = new long[LastRowIndex - FirstRowIndex + 1];
            rowSums.Initialize();
            columnSums = new long[LastColumnIndex - FirstColumnIndex + 1];
            columnSums.Initialize();
            long item;
            int lastColumnIndex = LastColumnIndex;
            for (int rowIndex = FirstRowIndex; rowIndex <= LastRowIndex; rowIndex++)
            {
                for (int columnIndex = FirstColumnIndex; columnIndex <= lastColumnIndex; columnIndex++)
                {
                    item = table[rowIndex, columnIndex];
                    rowSums[rowIndex] += item;
                    columnSums[columnIndex] += item;
                    allSum += item;
                }
            }
            doneComputeRowAndColumnSums = true;
        }
        #endregion
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