// ContingencyTable.cs - Mathematical core for dealing with contingency table
//
// Author: Tomáš Kuchaø <tomas.kuchar@gmail.com>
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
using Ferda.Guha.Math.Quantifiers;

namespace Ferda.Guha.Math
{
    /// <summary>
    /// Kontingencni tabulka 
    /// Bude obsahovat i denominator (implicitnì = 1), tak se Units zpracují uz v mineru.
    /// </summary>
    public class ContingencyTable
    {
        /// <summary>
        /// Contingency table
        /// (rectangular matrix of positive float/double values
        /// [rows,columns]; the field [0,0] contains paramenter <c>a</c> 
        /// (known from four fold contingency tables)
        /// </summary>
        protected readonly double[][] _contingecyTable;


        /// <summary>
        /// Gets the item with the specified row and column index.
        /// At position [0, 0] is item a (known from four-fold contingency tables).
        /// Returned values are not denominated by the denominator.
        /// </summary>
        /// <value></value>
        public double this[int rowIndex, int columnIndex]
        {
            get { return _contingecyTable[rowIndex][columnIndex]; }
        }

        protected readonly double _denominator;

        /// <summary>
        /// Dìlitel, kterym budou v kvantifikatoru vydìleny všechny 
        /// položky kontingenèní tabulky. Navíc v kvantifikatoru mùže
        /// být toto dìlení odloženo (a mùže se tak ušetøit nìkolik operací dìlení).
        /// 
        /// viz Units, ale ne vsechny kvantifikatory pouzivaji Units proto do 
        /// Units pridat NotSupported==AbsoluteValues.
        /// </summary>
        public double Denominator
        {
            get { return _denominator; }
        }

        /// <summary>
        /// Basic constructor for the class
        /// </summary>
        /// <param name="contingencyTable">Contingency table data</param>
        /// <param name="denominator">Denominator</param>
        public ContingencyTable(double[][] contingencyTable, double denominator)
        {
            if (contingencyTable == null)
                throw new ArgumentNullException("contingencyTable");
            if (contingencyTable.Length == 0)
                throw new ArgumentException("Contingency table is degenerative.", "contingencyTable");
            // Test shape
#if DEBUG
            int rowLength = contingencyTable[0].Length;
            foreach (double[] row in contingencyTable)
            {
                if (row.Length != rowLength)
                    throw new ArgumentException("Contingecy table is not in shape of regular rectangle.",
                                                "contingecyTable");
            }            
#endif
            _contingecyTable = contingencyTable;
            _denominator = denominator;
        }
        
        /// <summary>
        /// Other possible constructor of the class
        /// </summary>
        /// <param name="quantifierEvaluateSetting">Setting for quantifiers to be
        /// evaluated</param>
        public ContingencyTable(QuantifierEvaluateSetting quantifierEvaluateSetting)
            : this(quantifierEvaluateSetting.contingencyTable, quantifierEvaluateSetting.denominator)
        {}
        
        private void prepareSums()
        {
            _rowSums = new double[NumberOfRows];
            _rowSums.Initialize();
            _columnSums = new double[NumberOfColumns];
            _columnSums.Initialize();
            _sum = 0;
            for (int r = 0; r < NumberOfRows; r++)
            {
                for (int c = 0; c < NumberOfColumns; c++)
                {
                    double item = this[r, c];
                    _rowSums[r] += item;
                    _columnSums[c] += item;
                    _max = System.Math.Max(_max, item);
                    _min = System.Math.Min(_min, item);
                }
                _sum += _rowSums[r];
            }
        }

        private double _sum = -1;
        /// <summary>
        /// Gets the sum of all vaules in contingency table. This number is not
        /// denominted by the Denominator.
        /// </summary>
        /// <value>The sum.</value>
        public double Sum
        {
            get
            {
                lock (this)
                {
                    if (_sum < 0)
                        prepareSums();
                    return _sum;
                }
            }
        }

        private double _max = Double.NegativeInfinity;

        public double Max
        {
            get
            {
                lock (this)
                {
                    if (_max == Double.NegativeInfinity)
                        prepareSums();
                    return _max;
                }
            }
        }

        private double _min = Double.PositiveInfinity;

        public double Min
        {
            get
            {
                lock (this)
                {
                    if (_min == Double.PositiveInfinity)
                        prepareSums();
                    return _min;
                }
            }
        }
        
        private double[] _rowSums = null;
        /// <summary>
        /// Sum of all values in row [r]. Denominator is not applied.
        /// </summary>
        public double[] RowSums
        {
            get
            {
                lock (this)
                {
                    if (_rowSums == null)
                        prepareSums();
                    return _rowSums;
                }
            }
        }

        private double[] _columnSums = null;
        /// <summary>
        /// Sum of all values in column [c]. Denominator is not applied.
        /// </summary>
        public double[] ColumnSums
        {
            get
            {
                lock (this)
                {
                    if (_columnSums == null)
                        prepareSums();
                    return _columnSums;
                }
            }
        }

        public int NumberOfRows
        {
            get { return _contingecyTable.Length; }
        }

        public int NumberOfColumns
        {
            get { return _contingecyTable[0].Length; }
        }
        
        public void ForEach(Action<double> action)
        {
            foreach (double[] row in _contingecyTable)
            {
                foreach (double var in row)
                {
                    action(var);
                }
            }
        }
    }

    /// <summary>
    /// The four fold contingency table. This tables are constructed by the
    /// 4FT and SD4FT procedure. 
    /// </summary>
    public class FourFoldContingencyTable : ContingencyTable
    {
        /// <summary>
        /// Basic constructor for the class
        /// </summary>
        /// <param name="contingencyTable">Contingency table data</param>
        /// <param name="denominator">Denominator</param>
        public FourFoldContingencyTable(double[][] contingencyTable, double denominator)
            : base(contingencyTable, denominator)
        {
            if (NumberOfColumns != 2 || NumberOfRows != 2)
                throw new ArgumentException("Bad size of contingency table", "contingencyTable");
        }

        /// <summary>
        /// Other possible constructor of the class
        /// </summary>
        /// <param name="quantifierEvaluateSetting">Setting for quantifiers to be
        /// evaluated</param>
        public FourFoldContingencyTable(QuantifierEvaluateSetting quantifierEvaluateSetting)
            : this(quantifierEvaluateSetting.contingencyTable, quantifierEvaluateSetting.denominator)
        {}
        
        #region Fields (A, B, C, D, R, S, K, L, N)

        /// <summary>
        /// Gets the <c>a</c> frequency of the 4f-table (four fold table). (always absolute number)
        /// </summary>
        /// <result>The <c>a</c> frequency of the 4f-table (four fold table).</result>
        public double A
        {
            get { return this[0, 0]; }
        }

        /// <summary>
        /// Gets the <c>a</c> frequency of the 4f-table (four fold table). (divided by the 
        /// <see cref="P:Ferda.Guha.Math.ContingencyTable.Denominator"/>)
        /// </summary>
        /// <result>The <c>a</c> frequency of the 4f-table (four fold table).</result>
        public double Adiv
        {
            get { return A / Denominator; }
        }

        /// <summary>
        /// Gets the <c>b</c> frequency of the 4f-table (four fold table). (always absolute number)
        /// </summary>
        /// <result>The <c>b</c> frequency of the 4f-table (four fold table).</result>
        public double B
        {
            get { return this[0, 1]; }
        }

        /// <summary>
        /// Gets the <c>b</c> frequency of the 4f-table (four fold table). (divided by the 
        /// <see cref="P:Ferda.Guha.Math.ContingencyTable.Denominator"/>)
        /// </summary>
        /// <result>The <c>b</c> frequency of the 4f-table (four fold table).</result>
        public double Bdiv
        {
            get { return B / Denominator; }
        }

        /// <summary>
        /// Gets the <c>c</c> frequency of the 4f-table (four fold table). (always absolute number)
        /// </summary>
        /// <result>The <c>c</c> frequency of the 4f-table (four fold table).</result>
        public double C
        {
            get { return this[1, 0]; }
        }

        /// <summary>
        /// Gets the <c>c</c> frequency of the 4f-table (four fold table). (divided by the 
        /// <see cref="P:Ferda.Guha.Math.ContingencyTable.Denominator"/>)
        /// </summary>
        /// <result>The <c>c</c> frequency of the 4f-table (four fold table).</result>
        public double Cdiv
        {
            get { return C / Denominator; }
        }

        /// <summary>
        /// Gets the <c>d</c> frequency of the 4f-table (four fold table). (always absolute number)
        /// </summary>
        /// <result>The <c>d</c> frequency of the 4f-table (four fold table).</result>
        public double D
        {
            get { return this[1, 1]; }
        }

        /// <summary>
        /// Gets the <c>d</c> frequency of the 4f-table (four fold table). (divided by the 
        /// <see cref="P:Ferda.Guha.Math.ContingencyTable.Denominator"/>)
        /// </summary>
        /// <result>The <c>d</c> frequency of the 4f-table (four fold table).</result>
        public double Ddiv
        {
            get { return D / Denominator; }
        }

        /// <summary>
        /// Gets the <c>r = a + b</c> frequency of the 4f-table (four fold table). (always absolute number)
        /// </summary>
        /// <result>The <c>r = a + b</c> frequency of the 4f-table (four fold table).</result>
        public double R
        {
            get { return A + B; }
        }

        /// <summary>
        /// Gets the <c>r = a + b</c> frequency of the 4f-table (four fold table). (divided by the 
        /// <see cref="P:Ferda.Guha.Math.ContingencyTable.Denominator"/>)
        /// </summary>
        /// <result>The <c>r = a + b</c> frequency of the 4f-table (four fold table).</result>
        public double Rdiv
        {
            get { return R / Denominator; }
        }

        /// <summary>
        /// Gets the <c>s = c + d</c> frequency of the 4f-table (four fold table). (always absolute number)
        /// </summary>
        /// <result>The <c>s = c + d</c> frequency of the 4f-table (four fold table).</result>
        public double S
        {
            get { return C + D; }
        }

        /// <summary>
        /// Gets the <c>s = c + d</c> frequency of the 4f-table (four fold table). (divided by the 
        /// <see cref="P:Ferda.Guha.Math.ContingencyTable.Denominator"/>)
        /// </summary>
        /// <result>The <c>s = c + d</c> frequency of the 4f-table (four fold table).</result>
        public double Sdiv
        {
            get { return S / Denominator; }
        }

        /// <summary>
        /// Gets the <c>k = a + c</c> frequency of the 4f-table (four fold table). (always absolute number)
        /// </summary>
        /// <result>The <c>k = a + c</c> frequency of the 4f-table (four fold table).</result>
        public double K
        {
            get { return A + C; }
        }

        /// <summary>
        /// Gets the <c>k = a + c</c> frequency of the 4f-table (four fold table). (divided by the 
        /// <see cref="P:Ferda.Guha.Math.ContingencyTable.Denominator"/>)
        /// </summary>
        /// <result>The <c>k = a + c</c> frequency of the 4f-table (four fold table).</result>
        public double Kdiv
        {
            get { return K / Denominator; }
        }

        /// <summary>
        /// Gets the <c>l = b + d</c> frequency of the 4f-table (four fold table). (always absolute number)
        /// </summary>
        /// <result>The <c>l = b + d</c> frequency of the 4f-table (four fold table).</result>
        public double L
        {
            get { return B + D; }
        }

        /// <summary>
        /// Gets the <c>l = b + d</c> frequency of the 4f-table (four fold table). (divided by the 
        /// <see cref="P:Ferda.Guha.Math.ContingencyTable.Denominator"/>)
        /// </summary>
        /// <result>The <c>l = b + d</c> frequency of the 4f-table (four fold table).</result>
        public double Ldiv
        {
            get { return L / Denominator; }
        }

        /// <summary>
        /// Gets the <c>n = a + b + c + d</c> frequency of the 4f-table (four fold table). (always absolute number)
        /// </summary>
        /// <result>The <c>n = a + b + c + d</c> frequency of the 4f-table (four fold table).</result>
        public double N
        {
            get { return A + B + C + D; }
        }

        /// <summary>
        /// Gets the <c>n = a + b + c + d</c> frequency of the 4f-table (four fold table). (divided by the 
        /// <see cref="P:Ferda.Guha.Math.ContingencyTable.Denominator"/>)
        /// </summary>
        /// <result>The <c>n = a + b + c + d</c> frequency of the 4f-table (four fold table).</result>
        public double Ndiv
        {
            get { return N / Denominator; }
        }

        #endregion
    }
    
    /// <summary>
    /// A single dimension contingency table used in the CF and SDCF procedures.
    /// </summary>
    public class SingleDimensionContingecyTable : ContingencyTable
    {
        /// <summary>
        /// Basic constructor for the class
        /// </summary>
        /// <param name="contingencyTable">Contingency table data</param>
        /// <param name="denominator">Denominator</param>
        public SingleDimensionContingecyTable(double[][] contingencyTable, double denominator)
            : base(contingencyTable, denominator)
        {
            if (NumberOfRows != 1)
                throw new ArgumentException("Bad size of contingency table", "contingencyTable");
        }

        /// <summary>
        /// Other possible constructor of the class
        /// </summary>
        /// <param name="quantifierEvaluateSetting">Setting for quantifiers to be
        /// evaluated</param>
        public SingleDimensionContingecyTable(QuantifierEvaluateSetting quantifierEvaluateSetting)
            : this(quantifierEvaluateSetting.contingencyTable, quantifierEvaluateSetting.denominator)
        {}
        
        /// <summary>
        /// Gets the item with the specified column index.
        /// Returned values are not denominated by the denominator.
        /// </summary>
        /// <value></value>
        public double this[int columnIndex]
        {
            get { return this[0, columnIndex]; }
        }
    }
}