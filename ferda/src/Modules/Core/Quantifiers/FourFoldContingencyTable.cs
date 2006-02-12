/*
 * 
 * Vychází z kódu Tomáše Karbana <karby@matfyz.cz>
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Modules.Quantifiers
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Notice that only some 4ft-quantifiers support denominator.
    /// </remarks>
    public class FourFoldContingencyTable : ContingencyTable
    {
        #region Constructors
        public FourFoldContingencyTable()
            : base()
        { }
        public FourFoldContingencyTable(int[][] contingencyTable)
            : base(contingencyTable)
        {
            if (!this.IsBasicFourFoldTable)
                throw Ferda.Modules.Exceptions.BadParamsError(null, null, "Contingecy table has to be four fold!", restrictionTypeEnum.BadFormat);
        }

        public FourFoldContingencyTable(long[][] contingencyTable)
            : base(contingencyTable)
        {
            if (!this.IsBasicFourFoldTable)
                throw Ferda.Modules.Exceptions.BadParamsError(null, null, "Contingecy table has to be four fold!", restrictionTypeEnum.BadFormat);
        }

        public FourFoldContingencyTable(long[,] contingencyTable)
            : base(contingencyTable)
        {
            if (!this.IsBasicFourFoldTable)
                throw Ferda.Modules.Exceptions.BadParamsError(null, null, "Contingecy table has to be four fold!", restrictionTypeEnum.BadFormat);
        }

        public FourFoldContingencyTable(long[,] contingencyTable, long denominator)
            : base(contingencyTable, denominator)
        {
            if (!this.IsBasicFourFoldTable)
                throw Ferda.Modules.Exceptions.BadParamsError(null, null, "Contingecy table has to be four fold!", restrictionTypeEnum.BadFormat);
        }

        public FourFoldContingencyTable(long a, long b, long c, long d)
            : base(new long[2, 2] { { a, b }, { c, d } })
        {
        }

        public FourFoldContingencyTable(long a, long b, long c, long d, long denominator)
            : base(new long[2, 2] { { a, b }, { c, d } }, denominator)
        {
        }
        #endregion

        #region Fields (A, B, C, D, R, S, K, L, N)

        /// <summary>
        /// Gets the <c>a</c> frequency of the 4f-table (four fold table). (always absolute number)
        /// </summary>
        /// <value>The <c>a</c> frequency of the 4f-table (four fold table).</value>
        private long a
        {
            get { return table[0, 0]; }
        }

        /// <summary>
        /// Gets the <c>a</c> frequency of the 4f-table (four fold table). (divided by the 
        /// <see cref="F:Ferda.Modules.Quantifiers.ContingencyTable.denominator"/>)
        /// </summary>
        /// <value>The <c>a</c> frequency of the 4f-table (four fold table).</value>
        public double A
        {
            get { return a / (double)denominator; }
        }

        /// <summary>
        /// Gets the <c>b</c> frequency of the 4f-table (four fold table). (always absolute number)
        /// </summary>
        /// <value>The <c>b</c> frequency of the 4f-table (four fold table).</value>
        private long b
        {
            get { return table[0, 1]; }
        }

        /// <summary>
        /// Gets the <c>b</c> frequency of the 4f-table (four fold table). (divided by the 
        /// <see cref="F:Ferda.Modules.Quantifiers.ContingencyTable.denominator"/>)
        /// </summary>
        /// <value>The <c>b</c> frequency of the 4f-table (four fold table).</value>
        public double B
        {
            get { return b / (double)denominator; }
        }

        /// <summary>
        /// Gets the <c>c</c> frequency of the 4f-table (four fold table). (always absolute number)
        /// </summary>
        /// <value>The <c>c</c> frequency of the 4f-table (four fold table).</value>
        private long c
        {
            get { return table[1, 0]; }
        }

        /// <summary>
        /// Gets the <c>c</c> frequency of the 4f-table (four fold table). (divided by the 
        /// <see cref="F:Ferda.Modules.Quantifiers.ContingencyTable.denominator"/>)
        /// </summary>
        /// <value>The <c>c</c> frequency of the 4f-table (four fold table).</value>
        public double C
        {
            get { return c / (double)denominator; }
        }

        /// <summary>
        /// Gets the <c>d</c> frequency of the 4f-table (four fold table). (always absolute number)
        /// </summary>
        /// <value>The <c>d</c> frequency of the 4f-table (four fold table).</value>
        private long d
        {
            get { return table[1, 1]; }
        }

        /// <summary>
        /// Gets the <c>d</c> frequency of the 4f-table (four fold table). (divided by the 
        /// <see cref="F:Ferda.Modules.Quantifiers.ContingencyTable.denominator"/>)
        /// </summary>
        /// <value>The <c>d</c> frequency of the 4f-table (four fold table).</value>
        public double D
        {
            get { return d / (double)denominator; }
        }

        /// <summary>
        /// Gets the <c>r = a + b</c> frequency of the 4f-table (four fold table). (always absolute number)
        /// </summary>
        /// <value>The <c>r = a + b</c> frequency of the 4f-table (four fold table).</value>
        private long r
        {
            get { return a + b; }
        }

        /// <summary>
        /// Gets the <c>r = a + b</c> frequency of the 4f-table (four fold table). (divided by the 
        /// <see cref="F:Ferda.Modules.Quantifiers.ContingencyTable.denominator"/>)
        /// </summary>
        /// <value>The <c>r = a + b</c> frequency of the 4f-table (four fold table).</value>
        public double R
        {
            get { return r / (double)denominator; }
        }

        /// <summary>
        /// Gets the <c>s = c + d</c> frequency of the 4f-table (four fold table). (always absolute number)
        /// </summary>
        /// <value>The <c>s = c + d</c> frequency of the 4f-table (four fold table).</value>
        private long s
        {
            get { return c + d; }
        }

        /// <summary>
        /// Gets the <c>s = c + d</c> frequency of the 4f-table (four fold table). (divided by the 
        /// <see cref="F:Ferda.Modules.Quantifiers.ContingencyTable.denominator"/>)
        /// </summary>
        /// <value>The <c>s = c + d</c> frequency of the 4f-table (four fold table).</value>
        public double S
        {
            get { return s / (double)denominator; }
        }

        /// <summary>
        /// Gets the <c>k = a + c</c> frequency of the 4f-table (four fold table). (always absolute number)
        /// </summary>
        /// <value>The <c>k = a + c</c> frequency of the 4f-table (four fold table).</value>
        private long k
        {
            get { return a + c; }
        }

        /// <summary>
        /// Gets the <c>k = a + c</c> frequency of the 4f-table (four fold table). (divided by the 
        /// <see cref="F:Ferda.Modules.Quantifiers.ContingencyTable.denominator"/>)
        /// </summary>
        /// <value>The <c>k = a + c</c> frequency of the 4f-table (four fold table).</value>
        public double K
        {
            get { return k / (double)denominator; }
        }

        /// <summary>
        /// Gets the <c>l = b + d</c> frequency of the 4f-table (four fold table). (always absolute number)
        /// </summary>
        /// <value>The <c>l = b + d</c> frequency of the 4f-table (four fold table).</value>
        private long l
        {
            get { return b + d; }
        }

        /// <summary>
        /// Gets the <c>l = b + d</c> frequency of the 4f-table (four fold table). (divided by the 
        /// <see cref="F:Ferda.Modules.Quantifiers.ContingencyTable.denominator"/>)
        /// </summary>
        /// <value>The <c>l = b + d</c> frequency of the 4f-table (four fold table).</value>
        public double L
        {
            get { return l / (double)denominator; }
        }

        /// <summary>
        /// Gets the <c>n = a + b + c + d</c> frequency of the 4f-table (four fold table). (always absolute number)
        /// </summary>
        /// <value>The <c>n = a + b + c + d</c> frequency of the 4f-table (four fold table).</value>
        private long n
        {
            get { return a + b + c + d; }
        }

        /// <summary>
        /// Gets the <c>n = a + b + c + d</c> frequency of the 4f-table (four fold table). (divided by the 
        /// <see cref="F:Ferda.Modules.Quantifiers.ContingencyTable.denominator"/>)
        /// </summary>
        /// <value>The <c>n = a + b + c + d</c> frequency of the 4f-table (four fold table).</value>
        public double N
        {
            get { return n / (double)denominator; }
        }
        #endregion

        #region BaseCeil
        /// <summary>
        /// Returns the validity of Base/Ceil quantifier, e.g. true iff the <c>a</c> frequency in 4ft-table is greater than or equal to the specified <c>treshold</c>.
        /// </summary>
        /// <returns><c>true</c> iff the relative/absolute (<c>units</c>) frequency of <c>a</c> in 4ft-table is in <c>relation</c> to the specified <c>treshold</c>.</returns>
        public bool BaseCeilValidity(double treshold, CoreUnitsEnum units, CoreRelationEnum relation)
        {
            return Compare(BaseCeilValue(units), relation, treshold);
        }

        /// <summary>
        /// Returns the relative/absolute frequency <c>a</c> from 4ft-table.
        /// </summary>
        /// <returns>
        /// If property <c>units</c> is set to absolute number
        /// returns the <c>a</c> frequency from 4ft-table. 
        /// Else if property <c>units</c> is set to relative number
        /// returns the <c>a</c> frequency in percents from 4ft-table.
        /// </returns>
        public double BaseCeilValue(CoreUnitsEnum units)
        {
            return this.ConvertUnits(A, units);
        }
        public static double BaseCeilValue(FourFoldContingencyTable table)
        {
            return table.A;
        }
        #endregion

        #region AboveAverageImplication
        /// <summary>
        /// Computes the above average strength value.
        /// </summary>
        /// <returns>Above average strength value defined as <c>(a / (a + b)) * ((a + b + c + d) / (a + c))</c> if <c>a &gt; 0</c>; otherwise it returns zero.</returns>
        /// <remarks>
        /// <para>If a = 0, returns 0.</para>
        /// </remarks>
        public static double AboveAverageImplicationValue(FourFoldContingencyTable table)
        {
            return (table.A > 0) ? ((table.A * table.N) / (table.R * table.K)) : 0.0D;
        }

        /// <summary>
        /// Returns <c>true</c> if the above average strength is greater than or equal to the strength parameter.
        /// </summary>
        /// <returns><c>true</c> if <c>a &gt; 0</c> and the strength defined as <c>(a / (a + b)) * ((a + b + c + d) / (a + c))</c> is greater than or equal to the strength parameter.</returns>
        /// <remarks>
        /// <para>If a = 0, returns false.</para>
        /// </remarks>
        public bool AboveAverageImplicationValidity(double k)
        {
            return (A > 0) && ((A * N) / (R * K) >= k);
        }
        #endregion

        #region BelowAverageImplication
        /// <summary>
        /// Computes the below average strength value.
        /// </summary>
        /// <returns>Below average strength value defined as <c>((a + b) / a) * ((a + c) / (a + b + c + d))</c> with two exceptions: returns zero if <c>(a + c) = 0</c> and returns +INF if <c>a = 0</c>.</returns>
        /// <remarks>
        /// <para>The below average quantifier value must be explicitly defined for <c>a = 0</c> as +INF to prevent division by zero.</para>
        /// <para>Furthermore, if <c>(a + c) = 0</c>, the result +INF (= extremely strong hypothesis) would not make sense, so it is defined as zero (= no hypothesis) instead.</para>
        /// </remarks>
        public double BelowAverageImplicationValue()
        {
            if (K == 0)
                return 0.0D;
            if (A == 0)
                return Double.PositiveInfinity;
            return (R * K) / (A * N);
        }

        /// <summary>
        /// Returns <c>true</c> if the below average strength is greater than or equal to the strength parameter.
        /// </summary>
        /// <returns><c>true</c> if the strength defined as <c>((a + b) / a) * ((a + c) / (a + b + c + d))</c> is greater than or equal to the strength parameter.</returns>
        /// <remarks>
        /// <para>If <c>(a + c) = 0</c>, return false.</para>
        /// <para>If <c>a = 0</c>, returns true.</para>
        /// </remarks>
        public bool BelowAverageImplicationValidity(double k)
        {
            if (K == 0)
                return false;
            if (A == 0)
                return true;
            return ((R * K) / (A * N) >= k);
        }
        #endregion

        #region ChiSquare
        public static double ChiSquareCriticalValue(double alpha)
        {
            return Combinatorics.ChiSquareCdfInv(alpha, 1);
            // 1 degree of freedom for four-fold contingency table
        }

        /// <summary>
        /// Returns the value of alpha that would be neccessary to reject a null hypothesis.
        /// </summary>
        /// <returns>The minimum value of alpha that is neccessary to reject a null hypothesis.</returns>
        public double ChiSquareValue()
        {
            //TODO: Karbyho ChiSquare quantifier asi ne uplne funguje
            double x = (A * D) - (B * C);
            if (x <= 0)
                return Single.PositiveInfinity;
            double y = (N * x * x) / (R * S * K * L);
            double r2 = Combinatorics.ChiSquareCdf(y, 1);
            double result = 1.0D - r2;
            return result;
        }

        /// <summary>
        /// The 4ft-quantifier chi-square is valid, if the null hypothesis of independence between antecedent and succedent is rejected,
        /// thus we say that antecedent and succedent are not independent.
        /// </summary>
        /// <returns><c>true</c> if the null hypothesis of independence is rejected.</returns>
        public bool ChiSquareValidity(double alpha)
        {
            double x = (A * D) - (B * C);
            if (x <= 0)
                return false;
            return (N) * x * x >= ChiSquareCriticalValue(alpha) * R * S * K * L;
        }
        #endregion

        #region DoubleFoundedImplication
        /// <summary>
        /// Computes the <c>strength</c>.
        /// </summary>
        /// <returns>The strength defined as  <c>a / (a + b + c)</c>.</returns>
        /// <remarks>
        /// <para>If (a + b + c) = 0, returns 0.</para>
        /// </remarks>
        public static double DoubleFoundedImplicationValue(FourFoldContingencyTable table)
        {
            double a = table.A;
            double sum = a + table.B + table.C;
            return (sum > 0) ? (a / sum) : 0.0D;
        }

        /// <summary>
        /// Returns <c>true</c> iff the strength is greater than or equal to the strength parameter.
        /// </summary>
        /// <returns><c>true</c> iff the strength defined as <c>a / (a + b + c)</c> is greater than or equal to the strength parameter.</returns>
        /// <remarks>
        /// <para>If (a + b + c) = 0, returns false.</para>
        /// </remarks>
        public bool DoubleFoundedImplicationValidity(double p)
        {
            double a = A;
            double sum = a + B + C;
            return (sum > 0) && (a >= p * sum);
        }
        #endregion

        #region Double(Lower/Upper)CriticalImplication
        /// <summary>
        /// Computes a value of double critical implication sum minus alpha
        /// </summary>
        public double DoubleCriticalImplicationSumMinusAlpha(double p, params object[] parameters)
        {
            if (denominator != 1)
                throw Ferda.Modules.Exceptions.BadParamsError(null, null, "Denominator have to be equal to 1", restrictionTypeEnum.Other);
            double alpha = (double)parameters[0];
            if (p <= 0.0D)
                return -alpha;
            if (p >= 1.0D)
                return (1 - alpha);
            int a = (int)this.a;
            int x = (int)(this.a + this.b + this.c);
            double sum = 0;
            for (int i = 0; i <= a; i++)
            {
                sum += Math.Exp(Combinatorics.LogFactorial(x) - Combinatorics.LogFactorial(i) - Combinatorics.LogFactorial(x - i) + ((double)i) * Math.Log(p) + ((double)(x - i)) * Math.Log(1.0 - p));
            }
            return (sum - alpha);
        }

        /// <summary>
        /// Computes the statistical strength value at the specified significance (alpha).
        /// </summary>
        /// <returns>Statistical strength value at the specified significance (alpha).</returns>
        /// <remarks>
        /// <para>Computes the numerical solution of the following equation (for variable p):</para>
        /// <para><c>Sum[i = 0..a] x! / (i! * (x - i)!) * p^i * (1.0 - p)^(x - i) - alpha = 0.0</c>,
        /// where <c>x = (a + b + c)</c>.</para>
        /// <para>The solution must be between 0.0 and 1.0 (inclusive).</para>
        /// </remarks>
        public double DoubleCriticalImplicationValue(double alpha)
        {
            return Combinatorics.BinarySearch(0.0D, 1.0D, new Combinatorics.ExaminedDoubleFunction(DoubleCriticalImplicationSumMinusAlpha), alpha);
        }

        /// <summary>
        /// Returns <c>true</c> if the statistical strength value is greater than or equal to the p parameter with the specified statistical significance (alpha).
        /// </summary>
        /// <returns><c>true</c> if if the statistical strength value is greater than or equal to the p parameter with the specified statistical significance (alpha).</returns>
        /// <remarks>
        /// <para>It computes the following condition:</para>
        /// <para><c>Sum[i = a..x] x! / (i! * (x - i)!) * p^i * (1 - p)^(x - i) &lt;= (or &gt;=) alpha</c>,
        /// where <c>x = (a + b + c)</c>.</para>
        /// <para>lower ~ relation == LessThanOrEqualCore</para>
        /// <para>upper ~ relation == GreaterThanOrEqualCore</para>
        /// </remarks>
        public bool DoubleCriticalImplicationValidity(double p, double alpha, CoreRelationEnum relation)
        {
            return Compare(DoubleCriticalImplicationSumMinusAlpha(p, alpha), relation, 0);
        }
        #endregion

        #region E
        /// <summary>
        /// Returns value of expression (<c>a</c> + <c>d</c>) / (<c>a</c> + <c>b</c> + <c>c</c> + <c>d</c>) from 4ft-table.
        /// </summary>
        /// <returns>The value of expression (<c>a</c> + <c>d</c>) / (<c>a</c> + <c>b</c> + <c>c</c> + <c>d</c>) 4ft-table.</returns>
        public double EValue()
        {
            return (this.A + this.D) / this.N;
        }

        /// <summary>
        /// See [054 Redundance vystupu 4ftMiner.doc]
        /// Returns the validity of E-quantifier, i.e. true iff the (<c>a</c> + <c>d</c>) / (<c>a</c> + <c>b</c> + <c>c</c> + <c>d</c>) from 4ft-table is greater than or equal to the specified param <c>p</c>.
        /// </summary>
        /// <returns><c>true</c> iff the value of E-quantifier is greater than or equal to the specified param <c>p</c>.</returns>
        public bool EValidity(double p)
        {
            return (EValue() >= p);
        }
        #endregion

        #region Fisher
        private double FisherHelper()
        {
            if (denominator != 1)
                throw Ferda.Modules.Exceptions.BadParamsError(null, null, "Denominator have to be equal to 1", restrictionTypeEnum.Other);
            double sum = 0.0D;
            int r = (int)this.r;
            int k = (int)this.k;
            int n = (int)this.n;
            int s = (int)this.s;
            int l = (int)this.l;
            int stop = (r < k) ? r : k;
            double tmp = Combinatorics.LogFactorial(r)
                + Combinatorics.LogFactorial(s)
                + Combinatorics.LogFactorial(k)
                + Combinatorics.LogFactorial(l)
                - Combinatorics.LogFactorial(n);
            for (int i = (int)this.a; i <= stop; i++)
            {
                double x = tmp
                    - Combinatorics.LogFactorial(i)
                    - Combinatorics.LogFactorial(r - i)
                    - Combinatorics.LogFactorial(k - i)
                    - Combinatorics.LogFactorial(n + i - r - k);
                sum += Math.Exp(x);
            }
            return sum;
        }

        /// <summary>
        /// Computes the value of Fisher quantifier.
        /// </summary>
        /// <returns>The value <c>Sum[i = a..x] (r! * s! * k! * l!) / (n! * i! * (r-i)! * (k-i)! * (n+i-r-k)!)</c>, where <c>x = min(r, k)</c>.</returns>
        /// <remarks>
        /// <para>There is a special case defined explicitly:</para>
        /// <para>If <c>(a * d) &lt;= (b * c)</c>, return 0.</para>
        /// </remarks>
        public double FisherValue()
        {
            if (a * d <= b * c)
                return 0.0D;
            return FisherHelper();
        }

        /// <summary>
        /// Returns <c>true</c> if the antecedent and succedent are positively associated (in terms of Fisher quantifier).
        /// </summary>
        /// <returns><c>true</c> iff <c>(a * d) &lt; (b * c)</c> and <c>Sum[i = a..x] (r! * s! * k! * l!) / (n! * i! * (r-i)! * (k-i)! * (n+i-r-k)!) &lt;= alpha</c>, where <c>x = min(r, k)</c>.</returns>
        public bool FisherValidity(double alpha)
        {
            if (a * d <= b * c)
                return false;
            return (FisherHelper() <= alpha);
        }
        #endregion

        #region FoundedEquivalence
        /// <summary>
        /// Computes the <c>strength</c> of founded equivalence.
        /// </summary>
        /// <returns>The strength defined as  <c>(a + d) / (a + b + c + d)</c>.</returns>
        /// <remarks>
        /// <para>If (a + b + c + d) = 0, returns 0.</para>
        /// </remarks>
        public static double FoundedEquivalenceValue(FourFoldContingencyTable table)
        {
            double n = table.N;
            return (n > 0) ? (table.A) / n : 0.0D;
        }

        /// <summary>
        /// Returns <c>true</c> iff the strength is greater than or equal to the strength parameter.
        /// </summary>
        /// <returns><c>true</c> iff the strength defined as <c>(a + d) / (a + b + c + d)</c> is greater than or equal to the strength parameter.</returns>
        /// <remarks>
        /// <para>If (a + b + c + d) = 0, returns false.</para>
        /// </remarks>
        public bool FoundedEquivalenceValidity(double p)
        {
            double n = this.N;
            return (n > 0) && (this.A >= p * n);
        }
        #endregion

        #region FoundedImplication
        /// <summary>
        /// Computes the <c>confidence</c>.
        /// </summary>
        /// <returns>The confidence defined as  <c>a / (a + b)</c>.</returns>
        /// <remarks>
        /// <para>If (a + b) = 0, returns 0.</para>
        /// </remarks>
        public static double FoundedImplicationValue(FourFoldContingencyTable table)
        {
            double r = table.R;
            return (r > 0) ? (table.A) / r : 0.0D;
        }

        /// <summary>
        /// Returns <c>true</c> iff the <c>confidence</c> is greater than or equal to the confidence parameter.
        /// </summary>
        /// <returns><c>true</c> iff the <c>confidence</c> defined as <c>a / (a + b)</c> is greater than or equal to the confidence parameter.</returns>
        /// <remarks>
        /// <para>If (a + b) = 0, returns false.</para>
        /// </remarks>
        public bool FoundedImplicationValidity(double p)
        {
            double r = this.R;
            return (r > 0) && (this.A >= p * r);
        }
        #endregion

        #region (Lower/Upper)CriticalEquivalence
        /// <summary>
        /// Computes a value of critical equivalence sum minus alpha
        /// </summary>
        public double CriticalEquivalenceSumMinusAlpha(double p, params object[] parameters)
        {
            if (denominator != 1)
                throw Ferda.Modules.Exceptions.BadParamsError(null, null, "Denominator have to be equal to 1", restrictionTypeEnum.Other);
            double alpha = (double)parameters[0];
            if (p <= 0.0f)
                return -alpha;
            if (p >= 1.0f)
                return (1 - alpha);
            int a = (int)this.a;
            int x = (int)(this.n);
            double sum = 0;
            for (int i = 0; i <= a; i++)
            {
                sum += Math.Exp(Combinatorics.LogFactorial(x) - Combinatorics.LogFactorial(i) - Combinatorics.LogFactorial(x - i) + ((double)i) * Math.Log(p) + ((double)(x - i)) * Math.Log(1.0 - p));
            }
            return (sum - alpha);
        }

        /// <summary>
        /// Computes the statistical strength value at the specified significance (alpha).
        /// </summary>
        /// <returns>Statistical strength value at the specified significance (alpha).</returns>
        /// <remarks>
        /// <para>Computes the numerical solution of the following equation (for variable p):</para>
        /// <para><c>Sum[i = a..n] n! / (i! * (n - i)!) * p^i * (1.0 - p)^(n - i) - alpha = 0.0</c>.</para>
        /// <para>The solution must be between 0.0 and 1.0 (inclusive).</para>
        /// </remarks>
        public double CriticalEquivalenceValue(double alpha)
        {
            return Combinatorics.BinarySearch(0.0D, 1.0D, new Combinatorics.ExaminedDoubleFunction(CriticalEquivalenceSumMinusAlpha), alpha);
        }

        /// <summary>
        /// Returns <c>true</c> if the statistical strength value is greater than or equal to the p parameter with the specified statistical significance (alpha).
        /// </summary>
        /// <returns><c>true</c> if if the statistical strength value is greater than or equal to the p parameter with the specified statistical significance (alpha).</returns>
        /// <remarks>
        /// <para>It computes the following condition:</para>
        /// <para><c>Sum[i = a..n] n! / (i! * (n - i)!) * p^i * (1 - p)^(n - i) &lt;= (or &gt;=) alpha</c>.</para>
        /// <para>lower ~ relation == LessThanOrEqualCore</para>
        /// <para>upper ~ relation == GreaterThanOrEqualCore</para>
        /// </remarks>
        public bool CriticalEquivalenceValidity(double p, double alpha, CoreRelationEnum relation)
        {
            return Compare(CriticalEquivalenceSumMinusAlpha(p, alpha), relation, 0);
        }
        #endregion

        #region (Lower/Upper)CriticalImplication
        /// <summary>
        /// Computes a value of critical implication sum minus alpha
        /// </summary>
        public double CriticalImplicationSumMinusAlpha(double p, params object[] parameters)
        {
            if (denominator != 1)
                throw Ferda.Modules.Exceptions.BadParamsError(null, null, "Denominator have to be equal to 1", restrictionTypeEnum.Other);
            double alpha = (double)parameters[0];
            if (p <= 0.0D)
                return -alpha;
            if (p >= 1.0D)
                return (1 - alpha);
            int a = (int)this.a;
            int x = (int)this.r;
            double sum = 0;
            for (int i = a; i <= x; i++)
            {
                sum += Math.Exp(Combinatorics.LogFactorial(x) - Combinatorics.LogFactorial(i) - Combinatorics.LogFactorial(x - i) + ((double)i) * Math.Log(p) + ((double)(x - i)) * Math.Log(1.0 - p));
            }
            return (sum - alpha);
        }

        /// <summary>
        /// Computes the statistical confidence value at the specified significance (alpha).
        /// </summary>
        /// <returns>Statistical confidence value at the specified significance (alpha).</returns>
        /// <remarks>
        /// <para>Computes the numerical solution of the following equation (for variable p):</para>
        /// <para><c>Sum[i = a..r] r! / (i! * (r - i)!) * p^i * (1.0 - p)^(r - i) - alpha = 0.0</c></para>
        /// <para>The solution must be between 0.0 and 1.0 (inclusive).</para>
        /// </remarks>
        public double CriticalImplicationValue(double alpha)
        {
            return Combinatorics.BinarySearch(0.0D, 1.0D, new Combinatorics.ExaminedDoubleFunction(CriticalImplicationSumMinusAlpha), alpha);
        }

        /// <summary>
        /// Returns <c>true</c> if the statistical confidence value is greater than or equal to the p parameter with the specified statistical significance (alpha).
        /// </summary>
        /// <returns><c>true</c> if if the statistical confidence value is greater than or equal to the p parameter with the specified statistical significance (alpha).</returns>
        /// <remarks>
        /// <para>It computes the following condition:</para>
        /// <para><c>Sum[i = a..r] r! / (i! * (r - i)!) * p^i * (1 - p)^(r - i) &lt;= (or &gt;=) alpha</c>.</para>
        /// <para>lower ~ relation == LessThanOrEqualCore</para>
        /// <para>upper ~ relation == GreaterThanOrEqualCore</para>
        /// </remarks>
        public bool CriticalImplicationValidity(double p, double alpha, CoreRelationEnum relation)
        {
            return Compare(CriticalImplicationSumMinusAlpha(p, alpha), relation, 0);
        }
        #endregion

        #region SimpleDeviation
        /// <summary>
        /// Computes the simple deviation strength value.
        /// </summary>
        /// <returns>Simple deviation strength value defined as <c>ln(ad/bc) / ln(2)</c>.</returns>
        /// <remarks>
        /// <para>There are special cases defined explicitly:</para>
        /// <para>If both <c>(a * d) = 0</c> and <c>(b * c) = 0</c>, return 0.</para>
        /// <para>If only <c>(a * d) = 0</c>, return -INF.</para>
        /// <para>If only <c>(b * c) = 0</c>, return +INF.</para>
        /// </remarks>
        public double SimpleDeviationValue()
        {
            double ad = this.A * this.D;
            double bc = this.B * this.C;
            if ((ad == 0) && (bc == 0))
                return 0.0D;
            if (ad == 0)
                return Double.NegativeInfinity;
            if (bc == 0)
                return Double.PositiveInfinity;
            return (Math.Log(ad / bc) / 0.693147180559945309417);
        }

        /// <summary>
        /// Returns <c>true</c> if the simple deviation strength is greater than or equal to the strength parameter.
        /// </summary>
        /// <returns><c>true</c> iff <c>(a * d) &gt;= 2^k * (b * c)</c>.</returns>
        /// <remarks>
        /// <para>There are special cases defined explicitly:</para>
        /// <para>If both <c>(a * d) = 0</c> and <c>(b * c) = 0</c>, return true if <c>k &lt;= 0</c>.</para>
        /// <para>If only <c>(a * d) = 0</c>, return <c>false</c>.</para>
        /// <para>If only <c>(b * c) = 0</c>, return <c>true</c>.</para>
        /// </remarks>
        public bool SimpleDeviationValidity(double k)
        {
            double ad = this.A * this.D;
            double bc = this.B * this.C;
            if ((ad == 0) && (bc == 0))
                return (k <= 0.0f);
            if (ad == 0)
                return false;
            if (bc == 0)
                return true;
            return (ad >= Math.Pow(2, k) * bc);
        }
        #endregion
    }
}
