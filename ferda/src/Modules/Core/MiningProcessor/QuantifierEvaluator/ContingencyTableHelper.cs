using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Ferda.Guha.Math.Quantifiers;

namespace Ferda.Guha.MiningProcessor.QuantifierEvaluator
{
    internal class bounds : IEquatable<bounds>
    {
        public int FromRow;
        public int ToRow;
        public int FromColumn;
        public int ToColumn;

        #region IEquatable<bounds> Members

        public bool Equals(bounds other)
        {
            return (FromRow == other.FromRow
                    && ToRow == other.ToRow
                    && FromColumn == other.FromColumn
                    && ToColumn == other.ToColumn);
        }

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

    internal class cachedSubContingencyTable
    {
        public double[][] ContingencyTable;
        public double RelativeToActConditionDenominator = -1;
        public double RelativeToMaxFrequencyDenominator = -1;
    }

    /// <summary>
    /// <![CDATA[
    /// Cond    Succ    n Succ
    /// Ant     a       b
    /// n Ant   c       d
    /// ]]>
    /// </summary>
    /// <remarks>
    /// Please not that <c>a</c> is at position [0][0].
    /// </remarks>
    internal class FourFoldContingencyTable
    {
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

        private double[][] _cT;

        public double[][] ContingencyTable
        {
            get { return _cT; }
        }

        public FourFoldContingencyTable()
        {
            _cT = new double[2][];
            _cT[0] = new double[2];
            _cT[0].Initialize();
            _cT[1] = new double[2];
            _cT[1].Initialize();
        }

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

        private double _relativeToActConditionDenominator = -1;

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

        private double _relativeToMaxFrequencyDenominator = -1;

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
    /// <![CDATA[
    /// Cond    Succ    X-Succ  n Succ  | X-Cond    Succ    X-Succ  n Succ
    /// Ant     f111    f1x1    f101    | Ant       f11x    f1xx    f10x
    /// X-Ant   fx11    fxx1    fx01    | X-Ant     fx1x    fxxx    fx0x
    /// n Ant   f011    f0x1    f001    | n Ant     f01x    f0xx    f00x
    /// ]]>
    /// </summary>
    /// <remarks>
    /// Please not that <c>f111</c> is at position [0][0].
    /// </remarks>
    internal class NineFoldContingencyTablePair
    {
        public double f111
        {
            get { return _cT[0][0]; }
            set { _cT[0][0] = value; }
        }

        public double f1x1
        {
            get { return _cT[0][1]; }
            set { _cT[0][1] = value; }
        }

        public double f101
        {
            get { return _cT[0][2]; }
            set { _cT[0][2] = value; }
        }

        public double f11x
        {
            get { return _cT[0][3]; }
            set { _cT[0][3] = value; }
        }

        public double f1xx
        {
            get { return _cT[0][4]; }
            set { _cT[0][4] = value; }
        }

        public double f10x
        {
            get { return _cT[0][5]; }
            set { _cT[0][5] = value; }
        }

        public double fx11
        {
            get { return _cT[1][0]; }
            set { _cT[1][0] = value; }
        }

        public double fxx1
        {
            get { return _cT[1][1]; }
            set { _cT[1][1] = value; }
        }

        public double fx01
        {
            get { return _cT[1][2]; }
            set { _cT[1][2] = value; }
        }

        public double fx1x
        {
            get { return _cT[1][3]; }
            set { _cT[1][3] = value; }
        }

        public double fxxx
        {
            get { return _cT[1][4]; }
            set { _cT[1][4] = value; }
        }

        public double fx0x
        {
            get { return _cT[1][5]; }
            set { _cT[1][5] = value; }
        }

        public double f011
        {
            get { return _cT[2][0]; }
            set { _cT[2][0] = value; }
        }

        public double f0x1
        {
            get { return _cT[2][1]; }
            set { _cT[2][1] = value; }
        }

        public double f001
        {
            get { return _cT[2][2]; }
            set { _cT[2][2] = value; }
        }

        public double f01x
        {
            get { return _cT[2][3]; }
            set { _cT[2][3] = value; }
        }

        public double f0xx
        {
            get { return _cT[2][4]; }
            set { _cT[2][4] = value; }
        }

        public double f00x
        {
            get { return _cT[2][5]; }
            set { _cT[2][5] = value; }
        }

        private double[][] _cT;

        public double[][] ContingencyTable
        {
            get { return _cT; }
        }

        public NineFoldContingencyTablePair()
        {
            _cT = new double[3][];
            for (int r = 0; r < 3; r++)
            {
                _cT[r] = new double[6];
                _cT[r].Initialize();
            }
        }

        public NineFoldContingencyTablePair(double[][] contingencyTable)
        {
            if (contingencyTable.Length != 3 || contingencyTable[0].Length != 6)
                throw new ArgumentException("contingencyTable");
            _cT = contingencyTable;
        }
    }

    public class ContingencyTableHelper
    {
        #region Fields and Properties

        private double[][] _contingencyTable;
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public double[][] ContingencyTable
        {
            get { return _contingencyTable; }
        }

        private long _allObjectsCount;
        public long AllObjectsCount
        {
            get { return _allObjectsCount; }
        }

        private double _denominator = 1.0d;
        public double Denominator
        {
            get { return _denominator; }
        }
        
        private string _numericValuesAttributeGuid;
        public string NumericValuesAttributeGuid
        {
            get { return _numericValuesAttributeGuid; }
        }

        private double _relativeToActConditionDenominator = -1;
        protected double relativeToActConditionDenominator
        {
            get
            {
                if (_relativeToActConditionDenominator < 0)
                {
                    computeDenominators();
                }
                return _relativeToActConditionDenominator*_denominator;
            }
        }

        private double _relativeToMaxFrequencyDenominator = -1;
        protected double relativeToMaxFrequencyDenominator
        {
            get
            {
                if (_relativeToMaxFrequencyDenominator < 0)
                {
                    computeDenominators();
                }
                return _relativeToMaxFrequencyDenominator*_denominator;
            }
        }

        #endregion

        #region Operator Minus (of Absolute/Relative Frequencies)

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
                        op1.ContingencyTable[r][c]*op2Den
                        - op2.ContingencyTable[r][c]*op1Den
                        );
                }
            }
            double newDenominator = op1Den*op2Den;
            if (op1.NumericValuesAttributeGuid == null)
                return new ContingencyTableHelper(result, op1.AllObjectsCount, newDenominator);
            else
                return
                    new ContingencyTableHelper(result, op1.AllObjectsCount, newDenominator,
                                               op1.NumericValuesAttributeGuid);
        }

        #endregion

        #region Constructors

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

        public ContingencyTableHelper(double[][] contingencyTable, long allObjectsCount)
            : this(contingencyTable, allObjectsCount, 1.0d)
        {
        }

        public ContingencyTableHelper(double[][] contingencyTable, long allObjectsCount, double denominator,
                                      string numericValuesAttributeGuid)
            : this(contingencyTable, allObjectsCount, denominator)
        {
            _numericValuesAttributeGuid = numericValuesAttributeGuid;
        }

        public ContingencyTableHelper(double[][] contingencyTable, long allObjectsCount,
                                      string numericValuesAttributeGuid)
            : this(contingencyTable, allObjectsCount, 1.0d, numericValuesAttributeGuid)
        {
        }

        #endregion

        #region Sub matrix

        private int GetBoundFromIndex(Bound fromBound, int length)
        {
            switch (fromBound.boundType)
            {
                case BoundTypeEnum.All:
                    return 0;
                case BoundTypeEnum.Half:
                    return (int) System.Math.Floor((double) length/2);
                case BoundTypeEnum.Number:
                    if (fromBound.number < length)
                        return length;
                    else
                        return length - 1;
                default:
                    throw new NotImplementedException();
            }
        }

        private int GetBoundToIndex(Bound toBound, int length)
        {
            switch (toBound.boundType)
            {
                case BoundTypeEnum.All:
                    return length - 1;
                case BoundTypeEnum.Half:
                    return (int) System.Math.Ceiling((double) length/2);
                case BoundTypeEnum.Number:
                    if (toBound.number < length)
                        return length;
                    else
                        return length - 1;
                default:
                    throw new NotImplementedException();
            }
        }

        private Dictionary<bounds, cachedSubContingencyTable> _cached = null;

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
                        result.denominator = _cached[b].RelativeToActConditionDenominator*_denominator;
                    }
                    else
                    {
                        result.denominator = relativeToActConditionDenominator;
                    }
                    break;
                case UnitsEnum.RelativeToAllObjects:
                    result.denominator = _allObjectsCount*_denominator;
                    break;
                case UnitsEnum.RelativeToMaxFrequency:
                    if (b != null) // subTable
                    {
                        result.denominator = _cached[b].RelativeToMaxFrequencyDenominator*_denominator;
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

        public static bool IsInQuantifierClass(QuantifierClassEnum[] inClasses, QuantifierClassEnum asked)
        {
            foreach (QuantifierClassEnum inClass in inClasses)
                if (inClass == asked)
                    return true;
            return false;
        }

        public QuantifierEvaluateSetting GetSubTable(QuantifierClassEnum[] quantifierClasses,
                                                     MissingInformationHandlingEnum missingInformationHandling,
                                                     UnitsEnum units)
        {
            FourFoldContingencyTable fourFCT = new FourFoldContingencyTable();
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
            QuantifierEvaluateSetting result = new QuantifierEvaluateSetting();
            result.contingencyTable = fourFCT.ContingencyTable;
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
                    result.denominator = fourFCT.RelativeToActConditionDenominator*_denominator;
                    break;
                case UnitsEnum.RelativeToAllObjects:
                    result.denominator = _allObjectsCount*_denominator;
                    break;
                case UnitsEnum.RelativeToMaxFrequency:
                    result.denominator = fourFCT.RelativeToMaxFrequencyDenominator*_denominator;
                    break;
                default:
                    throw new NotImplementedException();
            }

            return result;
        }

        #endregion

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