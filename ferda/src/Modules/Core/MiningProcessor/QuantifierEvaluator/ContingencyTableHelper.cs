// ContingencyTableHelper.cs - classes to help with contingency 
// tables
//
// Authors: Tomáš Kuchaø <tomas.kuchar@gmail.com>      
// Commented by: Martin Ralbovský <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2006 Tomáš Kuchaø
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Ferda.Guha.Math.Quantifiers;

namespace Ferda.Guha.MiningProcessor.QuantifierEvaluator
{
    /// <summary>
    /// Class that holds information about bounds
    /// (when computing not the whole contingency
    /// table, but its part, especially in KL 
    /// procedure).
    /// </summary>
    internal class bounds : IEquatable<bounds>
    {
        /// <summary>
        /// From row
        /// </summary>
        public int FromRow;
        /// <summary>
        /// To row
        /// </summary>
        public int ToRow;
        /// <summary>
        /// From column
        /// </summary>
        public int FromColumn;
        /// <summary>
        /// To column
        /// </summary>
        public int ToColumn;

        #region IEquatable<bounds> Members

        /// <summary>
        /// Returns iff two bounds are 
        /// equal to each other
        /// </summary>
        /// <param name="other">The other bound</param>
        /// <returns>Iff the bounds are equal</returns>
        public bool Equals(bounds other)
        {
            return (FromRow == other.FromRow
                    && ToRow == other.ToRow
                    && FromColumn == other.FromColumn
                    && ToColumn == other.ToColumn);
        }

        /// <summary>
        /// Gets the hash code of a bound
        /// (unique)
        /// </summary>
        /// <returns>Hash code of a bound</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return
                    (FromRow & 0x000000ff)
                    + ((FromColumn & 0x000000ff) << 8)
                    + ((ToRow & 0x000000ff) << 16)
                    + ((ToColumn & 0x000000ff) << 24);
            }
        }

        #endregion
    }

    /// <summary>
    /// A cached sub contingency table
    /// (part of a big contingency table)
    /// </summary>
    internal class cachedSubContingencyTable
    {
        /// <summary>
        /// The contingency table itself
        /// </summary>
        public double[][] ContingencyTable;
        /// <summary>
        /// Denominator "relative to actual condition"
        /// </summary>
        public double RelativeToActConditionDenominator = -1;
        /// <summary>
        /// Denominator "relative to maximal frequency"
        /// </summary>
        public double RelativeToMaxFrequencyDenominator = -1;
    }

    /// <summary>
    /// <para>
    /// Four fold contingency table
    /// </para>
    /// <para>
    /// <![CDATA[
    /// Cond    Succ    n Succ
    /// Ant     a       b
    /// n Ant   c       d
    /// ]]>
    /// </para>
    /// </summary>
    /// <remarks>
    /// Please not that <c>a</c> is at position [0][0].
    /// </remarks>
    internal class FourFoldContingencyTable
    {
        /// <summary>
        /// Internal representation of the
        /// contingency table
        /// </summary>
        private double[][] _cT;

        /// <summary>
        /// The <c>Relative to actual condition denominator</c>
        /// is equal to sum of all count of all items
        /// in the table (known as N).
        /// </summary>
        private double _relativeToActConditionDenominator = -1;

        /// <summary>
        /// The <c>Relative to actual condition denominator</c>
        /// is equal to the highest item of items
        /// (a, b, c, d).
        /// </summary>
        private double _relativeToMaxFrequencyDenominator = -1;

        /// <summary>
        /// Ant and Succ
        /// </summary>
        public double a
        {
            get { return _cT[0][0]; }
            set { _cT[0][0] = value; }
        }

        /// <summary>
        /// Ant and notSucc
        /// </summary>
        public double b
        {
            get { return _cT[0][1]; }
            set { _cT[0][1] = value; }
        }

        /// <summary>
        /// notAnt and Succ
        /// </summary>
        public double c
        {
            get { return _cT[1][0]; }
            set { _cT[1][0] = value; }
        }

        /// <summary>
        /// notAnt and Succ
        /// </summary>
        public double d
        {
            get { return _cT[1][1]; }
            set { _cT[1][1] = value; }
        }

        /// <summary>
        /// The contingency table
        /// in and two-dimensional array
        /// </summary>
        public double[][] ContingencyTable
        {
            get { return _cT; }
        }

        /// <summary>
        /// Default constructor of the class
        /// </summary>
        public FourFoldContingencyTable()
        {
            _cT = new double[2][];
            _cT[0] = new double[2];
            _cT[0].Initialize();
            _cT[1] = new double[2];
            _cT[1].Initialize();
        }

        /// <summary>
        /// Computes the denominators.
        /// The <c>Relative to actual condition denominator</c>
        /// is equal to sum of all count of all items
        /// in the table (known as N).
        /// The <c>Relative to actual condition denominator</c>
        /// is equal to the highest item of items
        /// (a, b, c, d).
        /// </summary>
        private void computeDenominators()
        {
            double sum = 0.0d;
            double max = Double.MinValue;
            foreach (double[] row in _cT)
            {
                foreach (double item in row)
                {
                    max = System.Math.Max(max, item);
                    sum += item;
                }
            }
            _relativeToActConditionDenominator = sum;
            _relativeToMaxFrequencyDenominator = max;
        }

        /// <summary>
        /// The <c>Relative to actual condition denominator</c>
        /// is equal to sum of all count of all items
        /// in the table (known as N).
        /// </summary>
        public double RelativeToActConditionDenominator
        {
            get
            {
                if (_relativeToActConditionDenominator < 0)
                {
                    computeDenominators();
                }
                return _relativeToActConditionDenominator;
            }
        }

        /// <summary>
        /// The <c>Relative to actual condition denominator</c>
        /// is equal to the highest item of items
        /// (a, b, c, d).
        /// </summary>
        public double RelativeToMaxFrequencyDenominator
        {
            get
            {
                if (_relativeToMaxFrequencyDenominator < 0)
                {
                    computeDenominators();
                }
                return _relativeToMaxFrequencyDenominator;
            }
        }
    }

    /// <summary>
    /// The nine-fold condtingency table. It is used for 
    /// 4FT miner and missing information handling. 
    /// <![CDATA[
    /// Cond    Succ    X-Succ  n Succ  | X-Cond    Succ    X-Succ  n Succ
    /// Ant     f111    f1x1    f101    | Ant       f11x    f1xx    f10x
    /// X-Ant   fx11    fxx1    fx01    | X-Ant     fx1x    fxxx    fx0x
    /// n Ant   f011    f0x1    f001    | n Ant     f01x    f0xx    f00x
    /// ]]>
    /// </summary>
    /// <remarks>
    /// Please note that <c>f111</c> is at position [0][0].
    /// Indexes of items has this meaning: [antecedent, succedent, condition]
    /// </remarks>
    internal class NineFoldContingencyTablePair
    {
        /// <summary>
        /// Antecedent AND Succedent AND Condition
        /// </summary>
        public double f111
        {
            get { return _cT[0][0]; }
            set { _cT[0][0] = value; }
        }

        /// <summary>
        /// Antecedent AND missing Succedent AND Condition
        /// </summary>
        public double f1x1
        {
            get { return _cT[0][1]; }
            set { _cT[0][1] = value; }
        }

        /// <summary>
        /// Antecedent AND NOT Succedent AND Condition
        /// </summary>
        public double f101
        {
            get { return _cT[0][2]; }
            set { _cT[0][2] = value; }
        }

        /// <summary>
        /// Antecedent AND Succedent AND missing Condition
        /// </summary>
        public double f11x
        {
            get { return _cT[0][3]; }
            set { _cT[0][3] = value; }
        }

        /// <summary>
        /// Antecedent AND missing Succedent AND missing Condition
        /// </summary>
        public double f1xx
        {
            get { return _cT[0][4]; }
            set { _cT[0][4] = value; }
        }

        /// <summary>
        /// Antecedent AND NOT Succedent AND missing Condition
        /// </summary>
        public double f10x
        {
            get { return _cT[0][5]; }
            set { _cT[0][5] = value; }
        }

        /// <summary>
        /// Missing Antecedent AND Succedent AND Condition
        /// </summary>
        public double fx11
        {
            get { return _cT[1][0]; }
            set { _cT[1][0] = value; }
        }

        /// <summary>
        /// Missing Antecedent AND missing Succedent AND Condition
        /// </summary>
        public double fxx1
        {
            get { return _cT[1][1]; }
            set { _cT[1][1] = value; }
        }

        /// <summary>
        /// Missing Antecedent AND NOT Succedent AND Condition
        /// </summary>
        public double fx01
        {
            get { return _cT[1][2]; }
            set { _cT[1][2] = value; }
        }

        /// <summary>
        /// Missing Antecedent AND Succedent AND Missing Condition
        /// </summary>
        public double fx1x
        {
            get { return _cT[1][3]; }
            set { _cT[1][3] = value; }
        }

        /// <summary>
        /// Missing Antecedent AND missing Succedent AND missing Condition
        /// </summary>
        public double fxxx
        {
            get { return _cT[1][4]; }
            set { _cT[1][4] = value; }
        }

        /// <summary>
        /// Missing Antecedent AND NOT Succedent AND missing Condition
        /// </summary>
        public double fx0x
        {
            get { return _cT[1][5]; }
            set { _cT[1][5] = value; }
        }

        /// <summary>
        /// NOT Antecedent AND Succedent AND Condition
        /// </summary>
        public double f011
        {
            get { return _cT[2][0]; }
            set { _cT[2][0] = value; }
        }

        /// <summary>
        /// NOT Antecedent AND mising Succedent AND Condition
        /// </summary>
        public double f0x1
        {
            get { return _cT[2][1]; }
            set { _cT[2][1] = value; }
        }

        /// <summary>
        /// NOT Antecedent AND NOT Succedent AND Condition
        /// </summary>
        public double f001
        {
            get { return _cT[2][2]; }
            set { _cT[2][2] = value; }
        }

        /// <summary>
        /// NOT Antecedent AND Succedent AND missing Condition
        /// </summary>
        public double f01x
        {
            get { return _cT[2][3]; }
            set { _cT[2][3] = value; }
        }

        /// <summary>
        /// NOT Antecedent AND missing Succedent AND missing Condition
        /// </summary>
        public double f0xx
        {
            get { return _cT[2][4]; }
            set { _cT[2][4] = value; }
        }

        /// <summary>
        /// NOT Antecedent AND NOT Succedent AND missing Condition
        /// </summary>
        public double f00x
        {
            get { return _cT[2][5]; }
            set { _cT[2][5] = value; }
        }

        /// <summary>
        /// Internal representation of the table
        /// </summary>
        private double[][] _cT;

        /// <summary>
        /// Returns the whole contingency table in
        /// form of two-dimensional array
        /// </summary>
        public double[][] ContingencyTable
        {
            get { return _cT; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public NineFoldContingencyTablePair()
        {
            _cT = new double[3][];
            for (int r = 0; r < 3; r++)
            {
                _cT[r] = new double[6];
                _cT[r].Initialize();
            }
        }

        /// <summary>
        /// Constructor creating the table
        /// </summary>
        /// <param name="contingencyTable">Array representation of the table</param>
        public NineFoldContingencyTablePair(double[][] contingencyTable)
        {
            if (contingencyTable.Length != 3 || contingencyTable[0].Length != 6)
                throw new ArgumentException("contingencyTable");
            _cT = contingencyTable;
        }
    }
    
    /// <summary>
    /// Class for easier work with contingency tables
    /// </summary>
    public class ContingencyTableHelper
    {
        #region Fields and Properties

        /// <summary>
        /// Internal representation of contingency table
        /// </summary>
        private double[][] _contingencyTable;
        /// <summary>
        /// Contingency table in form of two-dimensional array
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public double[][] ContingencyTable
        {
            get { return _contingencyTable; }
        }

        /// <summary>
        /// Count of all objects in the contingency table
        /// </summary>
        private long _allObjectsCount;
        /// <summary>
        /// Count of all objects in the contingency table
        /// </summary>
        public long AllObjectsCount
        {
            get { return _allObjectsCount; }
        }

        /// <summary>
        /// The denominator
        /// </summary>
        private double _denominator = 1.0d;
        /// <summary>
        /// The denominator
        /// </summary>
        public double Denominator
        {
            get { return _denominator; }
        }

        /// <summary>
        /// If the contingency table is empty
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                foreach (double[] doubles in _contingencyTable)
                {
                    foreach (double d in doubles)
                    {
                        if (d > 0.0)
                            return false;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Identification of the attribute that contains numeric values
        /// </summary>
        private string _numericValuesAttributeGuid;
        /// <summary>
        /// Identification of the attribute that contains numeric values
        /// </summary>
        public string NumericValuesAttributeGuid
        {
            get { return _numericValuesAttributeGuid; }
        }

        /// <summary>
        /// The <c>Relative to actual condition denominator</c>
        /// is equal to sum of all count of all items
        /// in the table (known as N).
        /// </summary>
        private double _relativeToActConditionDenominator = -1;
        /// <summary>
        /// The <c>Relative to actual condition denominator</c>
        /// is equal to sum of all count of all items
        /// in the table (known as N).
        /// </summary>
        protected double relativeToActConditionDenominator
        {
            get
            {
                if (_relativeToActConditionDenominator < 0)
                {
                    computeDenominators();
                }
                return _relativeToActConditionDenominator * _denominator;
            }
        }

        /// <summary>
        /// The <c>Relative to actual condition denominator</c>
        /// is equal to the maximal value of an item in the table
        /// </summary>
        private double _relativeToMaxFrequencyDenominator = -1;
        /// <summary>
        /// The <c>Relative to actual condition denominator</c>
        /// is equal to the maximal value of an item in the table
        /// </summary>
        protected double relativeToMaxFrequencyDenominator
        {
            get
            {
                if (_relativeToMaxFrequencyDenominator < 0)
                {
                    computeDenominators();
                }
                return _relativeToMaxFrequencyDenominator * _denominator;
            }
        }

        #endregion

        #region Operator Minus (of Absolute/Relative Frequencies)

        /// <summary>
        /// Subtracts the <paramref name="op2"/> contigency table
        /// from <paramref name="op1"/> contingency table. The tables
        /// need to be the same shape and need to have the same numeric values
        /// attributes.
        /// </summary>
        /// <param name="op1">First operand</param>
        /// <param name="op2">Second operand</param>
        /// <returns>Result of the subtraction</returns>
        public static ContingencyTableHelper OperatorMinus(ContingencyTableHelper op1, ContingencyTableHelper op2)
        {
            if (!(
                     (op1.NumericValuesAttributeGuid == null && op2.NumericValuesAttributeGuid == null)
                     ||
                     (op1.NumericValuesAttributeGuid == op2.NumericValuesAttributeGuid)
                 ))
                throw new ArgumentException();
            int numOfRows = op1.ContingencyTable.Length;
            int numOfCols = op1.ContingencyTable[0].Length;
            if ((op2.ContingencyTable.Length != numOfRows)
                || (op2.ContingencyTable[0].Length != numOfCols))
                throw new ArgumentException();
            if (op1.AllObjectsCount != op2.AllObjectsCount)
                throw new ArgumentException();
            double op1Den = op1.relativeToActConditionDenominator;
            double op2Den = op2.relativeToActConditionDenominator;
            double[][] result = new double[numOfRows][];
            for (int r = 0; r < numOfRows; r++)
            {
                result[r] = new double[numOfCols];
                for (int c = 0; c < numOfCols; c++)
                {
                    result[r][c] = System.Math.Abs(
                        op1.ContingencyTable[r][c] * op2Den
                        - op2.ContingencyTable[r][c] * op1Den
                        );
                }
            }
            double newDenominator = op1Den * op2Den;
            if (op1.NumericValuesAttributeGuid == null)
                return new ContingencyTableHelper(result, op1.AllObjectsCount, newDenominator);
            else
                return
                    new ContingencyTableHelper(result, op1.AllObjectsCount, newDenominator,
                                               op1.NumericValuesAttributeGuid);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Basic constructor of the class. Constructs the contingency table from two-dimensional
        /// array of values, all objects count and the denominator
        /// </summary>
        /// <param name="contingencyTable">Contingency table values</param>
        /// <param name="allObjectsCount">All objects coung</param>
        /// <param name="denominator">Denominator</param>
        public ContingencyTableHelper(double[][] contingencyTable, long allObjectsCount, double denominator)
        {
            if (contingencyTable == null)
                throw new ArgumentNullException("contingencyTable");
            if (contingencyTable.Length == 0)
                throw new ArgumentException("Contingency table is degenerative.", "contingencyTable");
            _contingencyTable = contingencyTable;
            _allObjectsCount = allObjectsCount;
            _denominator = denominator;
        }

        /// <summary>
        /// Simplified constuctor of the class
        /// </summary>
        /// <param name="contingencyTable">Contingency table values</param>
        /// <param name="allObjectsCount">All objects coung</param>
        public ContingencyTableHelper(double[][] contingencyTable, long allObjectsCount)
            : this(contingencyTable, allObjectsCount, 1.0d)
        {
        }

        /// <summary>
        /// Basic constructor of the class for tables that contain numeric attribute.
        /// Constructs the contingency table from two-dimensional
        /// array of values, all objects count, the denominator and identification of numeric
        /// values attribute
        /// </summary>
        /// <param name="contingencyTable">Contingency table values</param>
        /// <param name="allObjectsCount">All objects coung</param>
        /// <param name="denominator">Denominator</param>
        /// <param name="numericValuesAttributeGuid">Numeric values attributes identification</param>
        public ContingencyTableHelper(double[][] contingencyTable, long allObjectsCount, double denominator,
                                      string numericValuesAttributeGuid)
            : this(contingencyTable, allObjectsCount, denominator)
        {
            _numericValuesAttributeGuid = numericValuesAttributeGuid;
        }

        /// <summary>
        /// Simplified constructor of the class for tables that contain numeric attribute.
        /// </summary>
        /// <param name="contingencyTable">Contingency table values</param>
        /// <param name="allObjectsCount">All objects coung</param>
        /// <param name="numericValuesAttributeGuid">Numeric values attributes identification</param>
        public ContingencyTableHelper(double[][] contingencyTable, long allObjectsCount,
                                      string numericValuesAttributeGuid)
            : this(contingencyTable, allObjectsCount, 1.0d, numericValuesAttributeGuid)
        {
        }

        #endregion

        #region Sub matrix

        /// <summary>
        /// Gets <c>From</c> bound from the
        /// <see cref="Bound"/> structure.
        /// </summary>
        /// <param name="fromBound">Bound structure</param>
        /// <param name="length">Length of the bound</param>
        /// <returns>From bound as number</returns>
        private int GetBoundFromIndex(Bound fromBound, int length)
        {
            switch (fromBound.boundType)
            {
                case BoundTypeEnum.All:
                    return 0;
                case BoundTypeEnum.Half:
                    return (int)System.Math.Floor((double)length / 2);
                case BoundTypeEnum.Number:
                    if (fromBound.number < length)
                        return length;
                    else
                        return length - 1;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets <c>To</c> bound from the
        /// <see cref="Bound"/> structure.
        /// </summary>
        /// <param name="toBound">Bound structure</param>
        /// <param name="length">Length of the bound</param>
        /// <returns>To bound as number</returns>
        private int GetBoundToIndex(Bound toBound, int length)
        {
            switch (toBound.boundType)
            {
                case BoundTypeEnum.All:
                    return length - 1;
                case BoundTypeEnum.Half:
                    return (int)System.Math.Ceiling((double)length / 2);
                case BoundTypeEnum.Number:
                    if (toBound.number < length)
                        return length;
                    else
                        return length - 1;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Cached subcontingency tables.
        /// </summary>
        private Dictionary<bounds, cachedSubContingencyTable> _cached = null;

        /// <summary>
        /// Makes a new subcontingency table out of the specified
        /// bounds and adds it to the cache.
        /// </summary>
        /// <param name="b">Specified bounds</param>
        private void makeSubContingencyTable(bounds b)
        {
            double sum = 0.0d;
            double max = Double.MinValue;
            double[][] result = new double[b.ToRow - b.FromRow + 1][];
            for (int r = b.FromRow; r <= b.ToRow; r++)
            {
                result[r] = new double[b.ToColumn - b.FromColumn + 1];
                for (int c = b.FromColumn; c <= b.ToColumn; c++)
                {
                    double item = _contingencyTable[r][c];
                    result[r][c] = item;
                    max = System.Math.Max(max, item);
                    sum += item;
                }
            }
            cachedSubContingencyTable val = new cachedSubContingencyTable();
            val.RelativeToActConditionDenominator = sum;
            val.RelativeToMaxFrequencyDenominator = max;
            val.ContingencyTable = result;
            _cached.Add(b, val);
        }

        /// <summary>
        /// Gets a sub-contingency table (computed out of this table and
        /// parameters.
        /// </summary>
        /// <param name="fromRow">From row bound</param>
        /// <param name="toRow">To row bound</param>
        /// <param name="fromColumn">From column bound</param>
        /// <param name="toColumn">To column bound</param>
        /// <param name="units">Units</param>
        /// <returns>Quantifier evaluate setting representing 
        /// this sub-contingency table (contingency table
        /// in form to be evaluated by quantifiers).</returns>
        public QuantifierEvaluateSetting GetSubTable(Bound fromRow, Bound toRow, Bound fromColumn, Bound toColumn,
                                                     UnitsEnum units)
        {
            QuantifierEvaluateSetting result = new QuantifierEvaluateSetting();

            bounds b = null;

            // contingency [sub]table;
            if (
                fromRow.boundType == BoundTypeEnum.All
                && toRow.boundType == BoundTypeEnum.All
                && fromColumn.boundType == BoundTypeEnum.All
                && toColumn.boundType == BoundTypeEnum.All
                )
            {
                result.contingencyTable = _contingencyTable;
            }
            else
            {
                if (_cached == null)
                    _cached = new Dictionary<bounds, cachedSubContingencyTable>();

                int numberOfRows = _contingencyTable.Length;
                int numberOfColumns = _contingencyTable[0].Length;

                b = new bounds();
                b.FromRow = GetBoundFromIndex(fromRow, numberOfRows);
                b.ToRow = GetBoundToIndex(toRow, numberOfRows);
                b.FromColumn = GetBoundFromIndex(fromColumn, numberOfColumns);
                b.ToColumn = GetBoundToIndex(toColumn, numberOfColumns);

                cachedSubContingencyTable cachedSCT;
                if (_cached.TryGetValue(b, out cachedSCT))
                {
                    result.contingencyTable = cachedSCT.ContingencyTable;
                }
                else
                {
                    makeSubContingencyTable(b);
                    result.contingencyTable = _cached[b].ContingencyTable;
                }
            }

            // denominator
            switch (units)
            {
                case UnitsEnum.AbsoluteNumber:
                    result.denominator = _denominator;
                    break;
                case UnitsEnum.Irrelevant:
                    result.denominator = 1;
                    break;
                case UnitsEnum.RelativeToActCondition:
                    if (b != null) // subTable
                    {
                        result.denominator = _cached[b].RelativeToActConditionDenominator * _denominator;
                    }
                    else
                    {
                        result.denominator = relativeToActConditionDenominator;
                    }
                    break;
                case UnitsEnum.RelativeToAllObjects:
                    result.denominator = _allObjectsCount * _denominator;
                    break;
                case UnitsEnum.RelativeToMaxFrequency:
                    if (b != null) // subTable
                    {
                        result.denominator = _cached[b].RelativeToMaxFrequencyDenominator * _denominator;
                    }
                    else
                    {
                        result.denominator = relativeToMaxFrequencyDenominator;
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
            return result;
        }

        #endregion

        #region PureFFTQuantifiers: Quantifier classe, missing informatin handling

        /// <summary>
        /// Determines if a quantifier class (<paramref name="asked"/> belongs
        /// to one of the classes in <paramref name="inClasses"/>.
        /// </summary>
        /// <param name="inClasses">Array of classes</param>
        /// <param name="asked">Quantifier class to determine</param>
        /// <returns>True iff belongs.</returns>
        public static bool IsInQuantifierClass(QuantifierClassEnum[] inClasses, QuantifierClassEnum asked)
        {
            foreach (QuantifierClassEnum inClass in inClasses)
                if (inClass == asked)
                    return true;
            return false;
        }

        /// <summary>
        /// Gets the four fold contingency table out of the specified parameters. 
        /// The method takes the missing information handling into consideration. 
        /// </summary>
        /// <param name="quantifierClasses">Classes of quantifiers that are connected
        /// to the task</param>
        /// <param name="missingInformationHandling">Handling of missing information</param>
        /// <param name="units">Units to count the contingency table</param>
        /// <returns>Quantifier evaluate setting representing 
        /// this sub-contingency table (contingency table
        /// in form to be evaluated by quantifiers).</returns>
        public QuantifierEvaluateSetting GetSubTable(QuantifierClassEnum[] quantifierClasses,
                                                     MissingInformationHandlingEnum missingInformationHandling,
                                                     UnitsEnum units)
        {
            double[][] fourFoldCT;
            bool fromFFCT = false;
            FourFoldContingencyTable fourFCT = null;

            if (_contingencyTable.Length == 2 && _contingencyTable[0].Length == 2)
            {
                fromFFCT = true;
                if (missingInformationHandling == MissingInformationHandlingEnum.Deleting)
                {
                    fourFoldCT = _contingencyTable;
                    goto skipMissingInformationHandlingSwitch;
                }
                else
                    throw new NotImplementedException();
            }

            fourFCT = new FourFoldContingencyTable();
            NineFoldContingencyTablePair nineFCTP = new NineFoldContingencyTablePair(_contingencyTable);
            switch (missingInformationHandling)
            {
                case MissingInformationHandlingEnum.Deleting:
                    fourFCT.a = nineFCTP.f111;
                    fourFCT.b = nineFCTP.f101;
                    fourFCT.c = nineFCTP.f011;
                    fourFCT.d = nineFCTP.f001;
                    break;
                case MissingInformationHandlingEnum.Optimistic:
                    if (IsInQuantifierClass(quantifierClasses, QuantifierClassEnum.Implicational))
                    {
                        fourFCT.a = nineFCTP.f111 + nineFCTP.f1x1 + nineFCTP.fx11 + nineFCTP.fxx1
                                    + nineFCTP.f11x + nineFCTP.f1xx + nineFCTP.fx1x + nineFCTP.fxxx;
                        fourFCT.b = nineFCTP.f101;
                    }
                    else if (IsInQuantifierClass(quantifierClasses, QuantifierClassEnum.SigmaDoubleImplicational))
                    {
                        // we create a, b in the same way as for QuantifierClassEnum.Implicational
                        fourFCT.a = nineFCTP.f111 + nineFCTP.f1x1 + nineFCTP.fx11 + nineFCTP.fxx1
                                    + nineFCTP.f11x + nineFCTP.f1xx + nineFCTP.fx1x + nineFCTP.fxxx;
                        fourFCT.b = nineFCTP.f101;
                        fourFCT.c = nineFCTP.f011;
                    }
                    else if (IsInQuantifierClass(quantifierClasses, QuantifierClassEnum.SigmaEquivalency))
                    {
                        // we create a, b, c in the same way as for QuantifierClassEnum.SigmaDoubleImplicational
                        fourFCT.a = nineFCTP.f111;
                        fourFCT.b = nineFCTP.f101 + nineFCTP.f1x1 + nineFCTP.fx01 + nineFCTP.fxx1
                                    + nineFCTP.f10x + nineFCTP.f1xx + nineFCTP.fx0x + nineFCTP.fxxx;
                        fourFCT.c = nineFCTP.f011 + nineFCTP.fx11 + nineFCTP.f0x1
                                    + nineFCTP.f01x + nineFCTP.fx1x + nineFCTP.f0xx;
                        fourFCT.d = nineFCTP.f001 + nineFCTP.f0x1 + nineFCTP.fx01
                                    + nineFCTP.f00x + nineFCTP.f0xx + nineFCTP.fx0x;
                    }
                    else if (IsInQuantifierClass(quantifierClasses, QuantifierClassEnum.FPropertyQuantifier))
                    {
                        fourFCT.a = nineFCTP.f111 + nineFCTP.f1x1 + nineFCTP.fx11
                                    + nineFCTP.f11x + nineFCTP.f1xx + nineFCTP.fx1x;
                        fourFCT.b = nineFCTP.f101;
                        fourFCT.c = nineFCTP.f011;
                        fourFCT.d = nineFCTP.f001 + nineFCTP.fx01 + nineFCTP.f0x1
                                    + nineFCTP.f00x + nineFCTP.fx0x + nineFCTP.f0xx;

                        #region Add fxxx and fxx1 to a and d as |a-d| is minimal

                        double sH = nineFCTP.fxxx + nineFCTP.fxx1;
                        if (sH != 0)
                        {
                            double dH = System.Math.Abs(nineFCTP.fxxx - nineFCTP.fxx1);
                            double dAB = System.Math.Abs(fourFCT.a - fourFCT.d);
                            if (System.Math.Abs(dAB - sH) < System.Math.Abs(dAB - dH))
                            {
                                if (fourFCT.a < fourFCT.d)
                                    fourFCT.a += sH;
                                else
                                    fourFCT.d += sH;
                            }
                            else
                            {
                                if (nineFCTP.fxxx < nineFCTP.fxx1)
                                {
                                    if (fourFCT.a < fourFCT.d)
                                    {
                                        fourFCT.a += nineFCTP.fxx1;
                                        fourFCT.d += nineFCTP.fxxx;
                                    }
                                    else
                                    {
                                        fourFCT.a += nineFCTP.fxxx;
                                        fourFCT.d += nineFCTP.fxx1;
                                    }
                                }
                                else
                                {
                                    if (fourFCT.a < fourFCT.d)
                                    {
                                        fourFCT.a += nineFCTP.fxxx;
                                        fourFCT.d += nineFCTP.fxx1;
                                    }
                                    else
                                    {
                                        fourFCT.a += nineFCTP.fxx1;
                                        fourFCT.d += nineFCTP.fxxx;
                                    }
                                }
                            }
                        }

                        #endregion
                    }
                    break;
                case MissingInformationHandlingEnum.Secured:
                    if (IsInQuantifierClass(quantifierClasses, QuantifierClassEnum.Implicational))
                    {
                        fourFCT.a = nineFCTP.f111;
                        fourFCT.b = nineFCTP.f101 + nineFCTP.f1x1 + nineFCTP.fx01 + nineFCTP.fxx1
                                    + nineFCTP.f10x + nineFCTP.f1xx + nineFCTP.fx0x + nineFCTP.fxxx;
                    }
                    else if (IsInQuantifierClass(quantifierClasses, QuantifierClassEnum.SigmaDoubleImplicational))
                    {
                        // we create a, b in the same way as for QuantifierClassEnum.Implicational
                        fourFCT.a = nineFCTP.f111;
                        fourFCT.b = nineFCTP.f101 + nineFCTP.f1x1 + nineFCTP.fx01 + nineFCTP.fxx1
                                    + nineFCTP.f10x + nineFCTP.f1xx + nineFCTP.fx0x + nineFCTP.fxxx;
                        fourFCT.c = nineFCTP.f011 + nineFCTP.fx11 + nineFCTP.f0x1
                                    + nineFCTP.f01x + nineFCTP.fx1x + nineFCTP.f0xx;
                        //nineFCTP.fxx1, nineFCTP.fxxx can be placed in b or c (it is equivalent)
                    }
                    else if (IsInQuantifierClass(quantifierClasses, QuantifierClassEnum.SigmaEquivalency))
                    {
                        // we create a, b, c in the same way as for QuantifierClassEnum.SigmaDoubleImplicational
                        fourFCT.a = nineFCTP.f111;
                        fourFCT.b = nineFCTP.f101 + nineFCTP.f1x1 + nineFCTP.fx01 + nineFCTP.fxx1
                                    + nineFCTP.f10x + nineFCTP.f1xx + nineFCTP.fx0x + nineFCTP.fxxx;
                        fourFCT.c = nineFCTP.f011 + nineFCTP.fx11 + nineFCTP.f0x1
                                    + nineFCTP.f01x + nineFCTP.fx1x + nineFCTP.f0xx;
                        fourFCT.d = nineFCTP.f001;
                    }
                    else if (IsInQuantifierClass(quantifierClasses, QuantifierClassEnum.FPropertyQuantifier))
                    {
                        fourFCT.a = nineFCTP.f111;
                        fourFCT.b = nineFCTP.f101 + nineFCTP.f1x1 + nineFCTP.fx01
                                    + nineFCTP.f10x + nineFCTP.f1xx + nineFCTP.fx0x;
                        fourFCT.c = nineFCTP.f011 + nineFCTP.fx11 + nineFCTP.f0x1
                                    + nineFCTP.f01x + nineFCTP.fx1x + nineFCTP.f0xx;
                        fourFCT.d = nineFCTP.f001;

                        #region Add fxxx and fxx1 to b and c as |b-c| is minimal

                        double sH = nineFCTP.fxxx + nineFCTP.fxx1;
                        if (sH != 0)
                        {
                            double dH = System.Math.Abs(nineFCTP.fxxx - nineFCTP.fxx1);
                            double dBC = System.Math.Abs(fourFCT.b - fourFCT.c);
                            if (System.Math.Abs(dBC - sH) < System.Math.Abs(dBC - dH))
                            {
                                if (fourFCT.b < fourFCT.c)
                                    fourFCT.b += sH;
                                else
                                    fourFCT.c += sH;
                            }
                            else
                            {
                                if (nineFCTP.fxxx < nineFCTP.fxx1)
                                {
                                    if (fourFCT.b < fourFCT.c)
                                    {
                                        fourFCT.b += nineFCTP.fxx1;
                                        fourFCT.c += nineFCTP.fxxx;
                                    }
                                    else
                                    {
                                        fourFCT.b += nineFCTP.fxxx;
                                        fourFCT.c += nineFCTP.fxx1;
                                    }
                                }
                                else
                                {
                                    if (fourFCT.b < fourFCT.c)
                                    {
                                        fourFCT.b += nineFCTP.fxxx;
                                        fourFCT.c += nineFCTP.fxx1;
                                    }
                                    else
                                    {
                                        fourFCT.b += nineFCTP.fxx1;
                                        fourFCT.c += nineFCTP.fxxx;
                                    }
                                }
                            }
                        }

                        #endregion
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
            fourFoldCT = fourFCT.ContingencyTable;
        skipMissingInformationHandlingSwitch:
            QuantifierEvaluateSetting result = new QuantifierEvaluateSetting();
            result.contingencyTable = fourFoldCT;
            result.numericValuesProviders = null;
            //result.numericValuesAttributeId = null;

            switch (units)
            {
                case UnitsEnum.AbsoluteNumber:
                    result.denominator = _denominator;
                    break;
                case UnitsEnum.Irrelevant:
                    result.denominator = 1;
                    break;
                case UnitsEnum.RelativeToActCondition:
                    if (fromFFCT)
                        result.denominator = relativeToActConditionDenominator * _denominator;
                    else
                        result.denominator = fourFCT.RelativeToActConditionDenominator * _denominator;
                    break;
                case UnitsEnum.RelativeToAllObjects:
                    result.denominator = _allObjectsCount * _denominator;
                    break;
                case UnitsEnum.RelativeToMaxFrequency:
                    if (fromFFCT)
                        result.denominator = relativeToMaxFrequencyDenominator * _denominator;
                    else
                        result.denominator = fourFCT.RelativeToMaxFrequencyDenominator * _denominator;
                    break;
                default:
                    throw new NotImplementedException();
            }

            return result;
        }

        #endregion

        /// <summary>
        /// Computes the denominators
        /// </summary>
        private void computeDenominators()
        {
            double sum = 0.0d;
            double max = Double.MinValue;
            foreach (double[] row in _contingencyTable)
            {
                foreach (double item in row)
                {
                    max = System.Math.Max(max, item);
                    sum += item;
                }
            }
            _relativeToActConditionDenominator = sum;
            _relativeToMaxFrequencyDenominator = max;
        }
    }
}