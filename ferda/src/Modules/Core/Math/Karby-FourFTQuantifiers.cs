//using System;

// /*
//"base"  base treshold (support)

//"fui"  founded implication
//"lci"  lower critical implication
//"uci"  upper critical implication

//"dfui"  double founded implication
//"dlci"  double lower critical implication
//"duci"  double upper critical implication

//"fue"  founded equivalence
//"lce"  lower critical equivalence
//"uce"  upper critical equivalence

//"sid"  simple deviaiton
//"fsh"  fisher
//"chi"  chi-square -- GUHA 4.4.23

//"aa"  above average
//"ba"  below average
//"oa"  outisde average

//*/

//namespace RelMiner.FourFTQuantifiers
//{
//#if DEBUG
//    /// <summary>
//    /// RelMiner.FourFTQuantifiers is a namespace that contains a definition of a four-fold contingency table and 4ft-quantifiers.
//    /// </summary>
//    public class NamespaceDoc
//    {
//        private NamespaceDoc()
//        {
//        }
//    }
//#endif


//    /// <summary>
//    /// Represents a 4ft-table.
//    /// </summary>
//    /// <remarks>
//    /// <para>It stores the four frequencies <b>a</b>, <b>b</b>, <b>c</b>, <b>d</b> internally. Missing information is not taken into account. The other frequencies <b>k</b>, <b>l</b>, <b>r</b>, <b>s</b>, <b>n</b> are computed.</para>
//    /// </remarks>
//    public class FourFTTable
//    {
//        /// <summary>
//        /// Implicit constructor that creates an empty four-fold contingency table.
//        /// </summary>
//        public FourFTTable()
//        {
//        }

        
//        /// <summary>
//        /// Initializing constructor.
//        /// </summary>
//        /// <param name="a">Frequency <b>a</b>.</param>
//        /// <param name="b">Frequency <b>b</b>.</param>
//        /// <param name="c">Frequency <b>c</b>.</param>
//        /// <param name="d">Frequency <b>d</b>.</param>
//        /// <exception cref="ArgumentOutOfRangeException">The frequencies in 4ft-table cannot be negative.</exception>
//        public FourFTTable(int a, int b, int c, int d)
//        {
//            A = a;
//            B = b;
//            C = c;
//            D = d;
//        }
//        private int _a, _b, _c, _d;

        
//        /// <summary>
//        /// Gets or sets the frequency <b>a</b> of 4ft-table.
//        /// </summary>
//        /// <exception cref="ArgumentOutOfRangeException">The frequencies in 4ft-table cannot be negative.</exception>
//        public int A
//        {
//            get
//            {
//                return _a; 
//            } 
//            set 
//            { 
//                if (value < 0) throw new ArgumentOutOfRangeException("value", value, "The frequencies in 4ft-table cannot be negative.");
//                _a = value;
//            } 
//        }


//        /// <summary>
//        /// Gets or sets the frequency <b>b</b> of 4ft-table.
//        /// </summary>
//        /// <exception cref="ArgumentOutOfRangeException">The frequencies in 4ft-table cannot be negative.</exception>
//        public int B
//        {
//            get
//            {
//                return _b;
//            }
//            set
//            {
//                if (value < 0) throw new ArgumentOutOfRangeException("value", value, "The frequencies in 4ft-table cannot be negative.");
//                _b = value;
//            }
//        }

        
//        /// <summary>
//        /// Gets or sets the frequency <b>c</b> of 4ft-table.
//        /// </summary>
//        /// <exception cref="ArgumentOutOfRangeException">The frequencies in 4ft-table cannot be negative.</exception>
//        public int C
//        {
//            get
//            {
//                return _c;
//            }
//            set
//            {
//                if (value < 0) throw new ArgumentOutOfRangeException("value", value, "The frequencies in 4ft-table cannot be negative.");
//                _c = value;
//            }
//        }

        
//        /// <summary>
//        /// Gets or sets the frequency <b>d</b> of 4ft-table.
//        /// </summary>
//        /// <exception cref="ArgumentOutOfRangeException">The frequencies in 4ft-table cannot be negative.</exception>
//        public int D
//        {
//            get
//            {
//                return _d;
//            }
//            set
//            {
//                if (value < 0) throw new ArgumentOutOfRangeException("value", value, "The frequencies in 4ft-table cannot be negative.");
//                _d = value;
//            }
//        }

        
//        /// <summary>
//        /// Gets or sets the frequency <b>r</b> of 4ft-table, which is <b>a + b</b>.
//        /// </summary>
//        public int R { get { return _a + _b; } }


//        /// <summary>
//        /// Gets or sets the frequency <b>s</b> of 4ft-table, which is <b>c + d</b>.
//        /// </summary>
//        public int S { get { return _c + _d; } }

        
//        /// <summary>
//        /// Gets or sets the frequency <b>k</b> of 4ft-table, which is <b>a + c</b>.
//        /// </summary>
//        public int K { get { return _a + _c; } }

        
//        /// <summary>
//        /// Gets or sets the frequency <b>l</b> of 4ft-table, which is <b>b + d</b>.
//        /// </summary>
//        public int L { get { return _b + _d; } }


//        /// <summary>
//        /// Gets or sets the frequency <b>n</b> of 4ft-table, which is <b>a + b + c + d</b>.
//        /// </summary>
//        public int N { get { return _a + _b + _c + _d; } }
//    }

    
//    /// <summary>
//    /// An interface mandatory for all 4ft-quantifiers.
//    /// </summary>
//    interface IFourFTQuantifier
//    {
//        /// <summary>
//        /// Computes the real value of the quantifier on the specified 4tf-table.
//        /// </summary>
//        /// <param name="table">A reference to <see cref="FourFTTable">4ft-table</see>.</param>
//        /// <returns>The real value of the quantifier on the specified 4ft-table.</returns>
//        float Value(FourFTTable table);

        
//        /// <summary>
//        /// Returns the validity (<b>true</b>/<b>false</b>) of the quantifier on the specified 4ft-table.
//        /// </summary>
//        /// <param name="table">A reference to <see cref="FourFTTable">4ft-table</see>.</param>
//        /// <returns>The validity of the quantifier on the specified 4ft-table.</returns>
//        bool Validity(FourFTTable table);
//    }


//    /// <summary>
//    /// Base quantifier.
//    /// </summary>
//    /// <remarks>
//    /// <para>Base quantifier imposes a minimum <b>support</b> (a treshold on the <b>a</b> frequency).</para>
//    /// <para>It is valid iff the <b>a</b> frequency in 4ft-table is greater than or equal to specified <b>support</b>.</para>
//    /// </remarks>
//    [Serializable]
//    public class Base : IFourFTQuantifier
//    {
//        /// <summary>
//        /// Constructor.
//        /// </summary>
//        /// <param name="support">A <b>support</b> parameter.</param>
//        /// <remarks>
//        /// <para><b>Support</b> must be greater than or equal to 0.</para>
//        /// </remarks>
//        /// <exception cref="ArgumentOutOfRangeException">Support cannot be negative.</exception>
//        public Base(int support)
//        {
//            Support = support;
//        }


//        /// <summary>
//        /// Gets or sets the value of <b>support</b> parameter.
//        /// </summary>
//        /// <remarks>
//        /// <para><b>Support</b> must be greater than or equal to 0.</para>
//        /// </remarks>
//        /// <exception cref="ArgumentOutOfRangeException">Support cannot be negative.</exception>
//        public int Support
//        {
//            get
//            {
//                return _support;
//            }
//            set
//            {
//                if (value < 0)
//                    throw new ArgumentOutOfRangeException("value", value, "Support cannot be negative.");

//                _support = value;
//            }
//        }
//        private int _support;


//        /// <summary>
//        /// Returns the <b>a</b> frequency from 4ft-table.
//        /// </summary>
//        /// <param name="table">A reference to <see cref="FourFTTable">4ft-table</see>.</param>
//        /// <returns>The <b>a</b> frequency from 4ft-table.</returns>
//        public float Value(FourFTTable table)
//        {
//            return (float) table.A;
//        }

        
//        /// <summary>
//        /// Returns the validity of Base quantifier, i.e. true iff the <b>a</b> frequency in 4ft-table is greater than or equal to the specified <b>support</b>.
//        /// </summary>
//        /// <param name="table">A reference to <see cref="FourFTTable">4ft-table</see>.</param>
//        /// <returns><b>true</b> iff the <b>a</b> frequency in 4ft-table is greater than or equal to the specified <b>support</b>.</returns>
//        public bool Validity(FourFTTable table)
//        {
//            return (table.A >= _support);
//        }
//    }


//    /// <summary>
//    /// Founded implication quantifier.
//    /// </summary>
//    /// <remarks>
//    /// <para>Defined as a condition a / (a + b) &gt;= p.</para>
//    /// </remarks>
//    [Serializable]
//    public class FoundedImplication : IFourFTQuantifier
//    {
//        /// <summary>
//        /// Constructor.
//        /// </summary>
//        /// <param name="p">The <b>confidence</b> parameter.</param>
//        /// <exception cref="ArgumentOutOfRangeException">The parameter p must be between 0 and 1 (inclusive).</exception>
//        public FoundedImplication(float p)
//        {
//            P = p;
//        }

        
//        /// <summary>
//        /// Gets or sets the <b>confidence</b> parameter.
//        /// </summary>
//        /// <exception cref="ArgumentOutOfRangeException">The parameter p must be between 0 and 1 (inclusive).</exception>
//        public float P
//        {
//            get
//            {
//                return _p;
//            }
//            set
//            {
//                if ((value < 0.0f) || (value > 1.0f))
//                    throw new ArgumentOutOfRangeException("value", value, "The parameter p must be between 0 and 1 (inclusive).");

//                _p = value;
//            }
//        }
//        private float _p;

        
//        /// <summary>
//        /// Computes the <b>confidence</b>.
//        /// </summary>
//        /// <param name="table">A reference to <see cref="FourFTTable">4ft-table</see>.</param>
//        /// <returns>The confidence defined as  <b>a / (a + b)</b>.</returns>
//        /// <remarks>
//        /// <para>If (a + b) = 0, returns 0.</para>
//        /// </remarks>
//        public float Value(FourFTTable table)
//        {
//            return (table.R > 0) ? ((float) table.A) / ((float) table.R) : 0.0f;
//        }


//        /// <summary>
//        /// Returns <b>true</b> iff the <b>confidence</b> is greater than or equal to the confidence parameter.
//        /// </summary>
//        /// <param name="table">A reference to <see cref="FourFTTable">4ft-table</see>.</param>
//        /// <returns><b>true</b> iff the <b>confidence</b> defined as <b>a / (a + b)</b> is greater than or equal to the confidence parameter.</returns>
//        /// <remarks>
//        /// <para>If (a + b) = 0, returns false.</para>
//        /// </remarks>
//        public bool Validity(FourFTTable table)
//        {
//            return (table.R > 0) && (((float) table.A) >= P * ((float) table.R));
//        }
//    }


//    /// <summary>
//    /// Lower critical implication quantifier.
//    /// </summary>
//    /// <remarks>
//    /// <para>Lower critical implication quantifier is defined as a condition <b>Sum[i = a..r] r! / (i! * (r - i)!) * p^i * (1 - p)^(r - i) &lt;= alpha</b>.</para>
//    /// <para>See chapter 4.4.9 and 4.4.12 in GUHA-book (likely p-implication quantifier).</para>
//    /// </remarks>
//    [Serializable]
//    public class LowerCriticalImplication : IFourFTQuantifier
//    {
//        /// <summary>
//        /// Constructor.
//        /// </summary>
//        /// <param name="p">Statistical confidence parameter.</param>
//        /// <param name="alpha">Statistical significance (usually 5%).</param>
//        /// <exception cref="ArgumentOutOfRangeException">
//        /// <list type="bullet">
//        /// <item>The parameter p must be between 0 and 1 (inclusive).</item>
//        /// <item>The parameter alpha must be greater than 0.0 and less then or equal to 0.5.</item>
//        /// </list>
//        /// </exception>
//        public LowerCriticalImplication(float p, float alpha)
//        {
//            P = p;
//            Alpha = alpha;
//        }


//        /// <summary>
//        /// Gets or sets the statistical confidence parameter.
//        /// </summary>
//        /// <exception cref="ArgumentOutOfRangeException">The parameter p must be between 0 and 1 (inclusive).</exception>
//        public float P
//        {
//            get
//            {
//                return _p;
//            }
//            set
//            {
//                if ((value < 0.0f) || (value > 1.0f))
//                    throw new ArgumentOutOfRangeException("value", value, "The parameter p must be between 0 and 1 (inclusive).");

//                _p = value;
//            }
//        }
//        private float _p;
        
        
//        /// <summary>
//        /// Gets or sets the statistical significance (usually 5%).
//        /// </summary>
//        /// <exception cref="ArgumentOutOfRangeException">The parameter alpha must be greater than 0.0 and less then or equal to 0.5.</exception>
//        public float Alpha
//        {
//            get
//            {
//                return _alpha;
//            }
//            set
//            {
//                if ((value <= 0.0f) || (value > 0.5f))
//                    throw new ArgumentOutOfRangeException("value", value, "The parameter alpha must be greater than 0.0 and less then or equal to 0.5.");

//                _alpha = value;
//            }
//        }
//        private float _alpha;


//        // computes a value of LCI sum minus alpha
//        private float QuantifierHelper(float p, params object[] hiddenParams)
//        {
//            FourFTTable table = (FourFTTable) hiddenParams[0];
//            if (p <= 0.0f) return (- _alpha);
//            if (p >= 1.0f) return (1.0f - _alpha);
//            int a = table.A; int r = table.R;
//            double sum = 0.0;
//            for (int i = a; i <= r; i++)
//            {
//                sum += Math.Exp(Combinatorics.LogFactorial(r) - Combinatorics.LogFactorial(i) - Combinatorics.LogFactorial(r - i) + ((double) i) * Math.Log(p) + ((double) (r - i)) * Math.Log(1.0 - p));
//            }
//            return ((float) sum - _alpha);
//        }
        

//        /// <summary>
//        /// Computes the statistical confidence value at the specified significance (alpha).
//        /// </summary>
//        /// <param name="table">A reference to <see cref="FourFTTable">4ft-table</see>.</param>
//        /// <returns>Statistical confidence value at the specified significance (alpha).</returns>
//        /// <remarks>
//        /// <para>Computes the numerical solution of the following equation (for variable p):</para>
//        /// <para><b>Sum[i = a..r] r! / (i! * (r - i)!) * p^i * (1.0 - p)^(r - i) - alpha = 0.0</b></para>
//        /// <para>The solution must be between 0.0 and 1.0 (inclusive).</para>
//        /// </remarks>
//        public float Value(FourFTTable table)
//        {
//            return Combinatorics.BinarySearch(0.0f, 1.0f, new Combinatorics.ExaminedFloatFunction(QuantifierHelper), table);
//        }


//        /// <summary>
//        /// Returns <b>true</b> if the statistical confidence value is greater than or equal to the p parameter with the specified statistical significance (alpha).
//        /// </summary>
//        /// <param name="table">A reference to <see cref="FourFTTable">4ft-table</see>.</param>
//        /// <returns><b>true</b> if if the statistical confidence value is greater than or equal to the p parameter with the specified statistical significance (alpha).</returns>
//        /// <remarks>
//        /// <para>It computes the following condition:</para>
//        /// <para><b>Sum[i = a..r] r! / (i! * (r - i)!) * p^i * (1 - p)^(r - i) &lt;= alpha</b>.</para>
//        /// </remarks>
//        public bool Validity(FourFTTable table)
//        {
//            return (QuantifierHelper(_p, table) <= 0.0f);
//        }
//    }


//    /// <summary>
//    /// Upper critical implication quantifier.
//    /// </summary>
//    /// <remarks>
//    /// <para>Upper critical implication quantifier is defined as a condition <b>Sum[i = 0..a] r! / (i! * (r - i)!) * p^i * (1 - p)^(r - i) &gt; alpha</b>.</para>
//    /// <para>See chapter 4.4.9 and 4.4.12 in GUHA-book (suspicious p-implication quantifier).</para>
//    /// </remarks>
//    [Serializable]
//    public class UpperCriticalImplication : IFourFTQuantifier
//    {
//        /// <summary>
//        /// Constructor.
//        /// </summary>
//        /// <param name="p">Statistical confidence parameter.</param>
//        /// <param name="alpha">Statistical significance (usually 5%).</param>
//        /// <exception cref="ArgumentOutOfRangeException">
//        /// <list type="bullet">
//        /// <item>The parameter p must be between 0 and 1 (inclusive).</item>
//        /// <item>The parameter alpha must be greater than 0.0 and less then or equal to 0.5.</item>
//        /// </list>
//        /// </exception>
//        public UpperCriticalImplication(float p, float alpha)
//        {
//            P = p;
//            Alpha = alpha;
//        }


//        /// <summary>
//        /// Gets or sets the statistical strength parameter.
//        /// </summary>
//        /// <exception cref="ArgumentOutOfRangeException">The parameter p must be between 0 and 1 (inclusive).</exception>
//        public float P
//        {
//            get
//            {
//                return _p;
//            }
//            set
//            {
//                if ((value < 0.0f) || (value > 1.0f))
//                    throw new ArgumentOutOfRangeException("value", value, "The parameter p must be between 0 and 1 (inclusive).");

//                _p = value;
//            }
//        }
//        private float _p;
        
        
//        /// <summary>
//        /// Gets or sets the statistical significance (usually 5%).
//        /// </summary>
//        /// <exception cref="ArgumentOutOfRangeException">The parameter alpha must be greater than 0.0 and less then or equal to 0.5.</exception>
//        public float Alpha
//        {
//            get
//            {
//                return _alpha;
//            }
//            set
//            {
//                if ((value <= 0.0f) || (value > 0.5f))
//                    throw new ArgumentOutOfRangeException("value", value, "The parameter alpha must be greater than 0.0 and less then or equal to 0.5.");

//                _alpha = value;
//            }
//        }
//        private float _alpha;


//        // computes a value of LCI sum minus alpha
//        private float QuantifierHelper(float p, params object[] hiddenParams)
//        {
//            FourFTTable table = (FourFTTable) hiddenParams[0];
//            if (p <= 0.0f) return (- _alpha);
//            if (p >= 1.0f) return (1.0f - _alpha);
//            int a = table.A; int r = table.R;
//            double sum = 0.0;
//            for (int i = 0; i <= a; i++)
//            {
//                sum += Math.Exp(Combinatorics.LogFactorial(r) - Combinatorics.LogFactorial(i) - Combinatorics.LogFactorial(r - i) + ((double) i) * Math.Log(p) + ((double) (r - i)) * Math.Log(1.0 - p));
//            }
//            return ((float) sum - _alpha);
//        }
        

//        /// <summary>
//        /// Computes the statistical strength value at the specified significance (alpha).
//        /// </summary>
//        /// <param name="table">A reference to <see cref="FourFTTable">4ft-table</see>.</param>
//        /// <returns>Statistical strength value at the specified significance (alpha).</returns>
//        /// <remarks>
//        /// <para>Computes the numerical solution of the following equation (for variable p):</para>
//        /// <para><b>Sum[i = 0..a] r! / (i! * (r - i)!) * p^i * (1.0 - p)^(r - i) - alpha = 0.0</b></para>
//        /// <para>The solution must be between 0.0 and 1.0 (inclusive).</para>
//        /// </remarks>
//        public float Value(FourFTTable table)
//        {
//            return Combinatorics.BinarySearch(0.0f, 1.0f, new Combinatorics.ExaminedFloatFunction(QuantifierHelper), table);
//        }


//        /// <summary>
//        /// Returns <b>true</b> if the statistical strength value is greater than or equal to the p parameter with the specified statistical significance (alpha).
//        /// </summary>
//        /// <param name="table">A reference to <see cref="FourFTTable">4ft-table</see>.</param>
//        /// <returns><b>true</b> if if the statistical confidence value is greater than or equal to the p parameter with the specified statistical significance (alpha).</returns>
//        /// <remarks>
//        /// <para>It computes the following condition:</para>
//        /// <para><b>Sum[i = 0..a] r! / (i! * (r - i)!) * p^i * (1 - p)^(r - i) &gt; alpha</b>.</para>
//        /// </remarks>
//        public bool Validity(FourFTTable table)
//        {
//            return (QuantifierHelper(_p, table) > 0.0f);
//        }
//    }


//    /// <summary>
//    /// Double founded implication quantifier.
//    /// </summary>
//    /// <remarks>
//    /// <para>Defined as a condition a / (a + b + c) &gt;= p.</para>
//    /// </remarks>
//    [Serializable]
//    public class DoubleFoundedImplication : IFourFTQuantifier
//    {
//        /// <summary>
//        /// Constructor.
//        /// </summary>
//        /// <param name="p">The strength parameter.</param>
//        /// <exception cref="ArgumentOutOfRangeException">The parameter p must be between 0 and 1 (inclusive).</exception>
//        public DoubleFoundedImplication(float p)
//        {
//            P = p;
//        }

        
//        /// <summary>
//        /// Gets or sets the strength parameter.
//        /// </summary>
//        /// <exception cref="ArgumentOutOfRangeException">The parameter p must be between 0 and 1 (inclusive).</exception>
//        public float P
//        {
//            get
//            {
//                return _p;
//            }
//            set
//            {
//                if ((value < 0.0f) || (value > 1.0f))
//                    throw new ArgumentOutOfRangeException("value", value, "The parameter p must be between 0 and 1 (inclusive).");

//                _p = value;
//            }
//        }
//        private float _p;

        
//        /// <summary>
//        /// Computes the <b>strength</b>.
//        /// </summary>
//        /// <param name="table">A reference to <see cref="FourFTTable">4ft-table</see>.</param>
//        /// <returns>The strength defined as <b>a / (a + b + c)</b>.</returns>
//        /// <remarks>
//        /// <para>If (a + b + c) = 0, returns 0.</para>
//        /// </remarks>
//        public float Value(FourFTTable table)
//        {
//            int sum = table.A + table.B + table.C;
//            return (sum > 0) ? ((float) table.A) / ((float) sum) : 0.0f;
//        }


//        /// <summary>
//        /// Returns <b>true</b> iff the strength is greater than or equal to the strength parameter.
//        /// </summary>
//        /// <param name="table">A reference to <see cref="FourFTTable">4ft-table</see>.</param>
//        /// <returns><b>true</b> iff the strength defined as <b>a / (a + b + c)</b> is greater than or equal to the strength parameter.</returns>
//        /// <remarks>
//        /// <para>If (a + b + c) = 0, returns false.</para>
//        /// </remarks>
//        public bool Validity(FourFTTable table)
//        {
//            int sum = table.A + table.B + table.C;
//            return (sum > 0) && (((float) table.A) >= P * ((float) sum));
//        }
//    }


//    /// <summary>
//    /// Double lower critical implication quantifier.
//    /// </summary>
//    /// <remarks>
//    /// <para>Double lower critical implication quantifier is defined as the condition
//    /// <b>Sum[i = a..x] x! / (i! * (x - i)!) * p^i * (1 - p)^(x - i) &lt;= alpha</b>,
//    /// where x = (a + b + c).</para>
//    /// </remarks>
//    [Serializable]
//    public class DoubleLowerCriticalImplication : IFourFTQuantifier
//    {
//        /// <summary>
//        /// Constructor.
//        /// </summary>
//        /// <param name="p">Statistical strength parameter.</param>
//        /// <param name="alpha">Statistical significance (usually 5%).</param>
//        /// <exception cref="ArgumentOutOfRangeException">
//        /// <list type="bullet">
//        /// <item>The parameter p must be between 0 and 1 (inclusive).</item>
//        /// <item>The parameter alpha must be greater than 0.0 and less then or equal to 0.5.</item>
//        /// </list>
//        /// </exception>
//        public DoubleLowerCriticalImplication(float p, float alpha)
//        {
//            P = p;
//            Alpha = alpha;
//        }


//        /// <summary>
//        /// Gets or sets the statistical strength parameter.
//        /// </summary>
//        /// <exception cref="ArgumentOutOfRangeException">The parameter p must be between 0 and 1 (inclusive).</exception>
//        public float P
//        {
//            get
//            {
//                return _p;
//            }
//            set
//            {
//                if ((value < 0.0f) || (value > 1.0f))
//                    throw new ArgumentOutOfRangeException("value", value, "The parameter p must be between 0 and 1 (inclusive).");

//                _p = value;
//            }
//        }
//        private float _p;
        
        
//        /// <summary>
//        /// Gets or sets the statistical significance (usually 5%).
//        /// </summary>
//        /// <exception cref="ArgumentOutOfRangeException">The parameter alpha must be greater than 0.0 and less then or equal to 0.5.</exception>
//        public float Alpha
//        {
//            get
//            {
//                return _alpha;
//            }
//            set
//            {
//                if ((value <= 0.0f) || (value > 0.5f))
//                    throw new ArgumentOutOfRangeException("value", value, "The parameter alpha must be greater than 0.0 and less then or equal to 0.5.");

//                _alpha = value;
//            }
//        }
//        private float _alpha;


//        // computes a value of DLCI sum minus alpha
//        private float QuantifierHelper(float p, params object[] hiddenParams)
//        {
//            FourFTTable table = (FourFTTable) hiddenParams[0];
//            if (p <= 0.0f) return (- _alpha);
//            if (p >= 1.0f) return (1.0f - _alpha);
//            int a = table.A; int x = table.A + table.B + table.C;
//            double sum = 0.0;
//            for (int i = a; i <= x; i++)
//            {
//                sum += Math.Exp(Combinatorics.LogFactorial(x) - Combinatorics.LogFactorial(i) - Combinatorics.LogFactorial(x - i) + ((double) i) * Math.Log(p) + ((double) (x - i)) * Math.Log(1.0 - p));
//            }
//            return ((float) sum - _alpha);
//        }
        

//        /// <summary>
//        /// Computes the statistical strength value at the specified significance (alpha).
//        /// </summary>
//        /// <param name="table">A reference to <see cref="FourFTTable">4ft-table</see>.</param>
//        /// <returns>Statistical strength value at the specified significance (alpha).</returns>
//        /// <remarks>
//        /// <para>Computes the numerical solution of the following equation (for variable p):</para>
//        /// <para><b>Sum[i = a..x] x! / (i! * (x - i)!) * p^i * (1.0 - p)^(x - i) - alpha = 0.0</b>,
//        /// where <b>x = (a + b + c)</b>.</para>
//        /// <para>The solution must be between 0.0 and 1.0 (inclusive).</para>
//        /// </remarks>
//        public float Value(FourFTTable table)
//        {
//            return Combinatorics.BinarySearch(0.0f, 1.0f, new Combinatorics.ExaminedFloatFunction(QuantifierHelper), table);
//        }


//        /// <summary>
//        /// Returns <b>true</b> if the statistical strength value is greater than or equal to the p parameter with the specified statistical significance (alpha).
//        /// </summary>
//        /// <param name="table">A reference to <see cref="FourFTTable">4ft-table</see>.</param>
//        /// <returns><b>true</b> if if the statistical strength value is greater than or equal to the p parameter with the specified statistical significance (alpha).</returns>
//        /// <remarks>
//        /// <para>It computes the following condition:</para>
//        /// <para><b>Sum[i = a..x] x! / (i! * (x - i)!) * p^i * (1 - p)^(x - i) &lt;= alpha</b>,
//        /// where <b>x = (a + b + c)</b>.</para>
//        /// </remarks>
//        public bool Validity(FourFTTable table)
//        {
//            return (QuantifierHelper(_p, table) <= 0.0f);
//        }
//    }


//    /// <summary>
//    /// Double upper critical implication quantifier.
//    /// </summary>
//    /// <remarks>
//    /// <para>Double upper critical implication quantifier is defined as the condition
//    /// <b>Sum[i = 0..a] x! / (i! * (x - i)!) * p^i * (1 - p)^(x - i) &gt; alpha</b>,
//    /// where x = (a + b + c).</para>
//    /// </remarks>
//    [Serializable]
//    public class DoubleUpperCriticalImplication : IFourFTQuantifier
//    {
//        /// <summary>
//        /// Constructor.
//        /// </summary>
//        /// <param name="p">Statistical strength parameter.</param>
//        /// <param name="alpha">Statistical significance (usually 5%).</param>
//        /// <exception cref="ArgumentOutOfRangeException">
//        /// <list type="bullet">
//        /// <item>The parameter p must be between 0 and 1 (inclusive).</item>
//        /// <item>The parameter alpha must be greater than 0.0 and less then or equal to 0.5.</item>
//        /// </list>
//        /// </exception>
//        public DoubleUpperCriticalImplication(float p, float alpha)
//        {
//            P = p;
//            Alpha = alpha;
//        }


//        /// <summary>
//        /// Gets or sets the statistical strength parameter.
//        /// </summary>
//        /// <exception cref="ArgumentOutOfRangeException">The parameter p must be between 0 and 1 (inclusive).</exception>
//        public float P
//        {
//            get
//            {
//                return _p;
//            }
//            set
//            {
//                if ((value < 0.0f) || (value > 1.0f))
//                    throw new ArgumentOutOfRangeException("value", value, "The parameter p must be between 0 and 1 (inclusive).");

//                _p = value;
//            }
//        }
//        private float _p;
        
        
//        /// <summary>
//        /// Gets or sets the statistical significance (usually 5%).
//        /// </summary>
//        /// <exception cref="ArgumentOutOfRangeException">The parameter alpha must be greater than 0.0 and less then or equal to 0.5.</exception>
//        public float Alpha
//        {
//            get
//            {
//                return _alpha;
//            }
//            set
//            {
//                if ((value <= 0.0f) || (value > 0.5f))
//                    throw new ArgumentOutOfRangeException("value", value, "The parameter alpha must be greater than 0.0 and less then or equal to 0.5.");

//                _alpha = value;
//            }
//        }
//        private float _alpha;


//        // computes a value of DLCI sum minus alpha
//        private float QuantifierHelper(float p, params object[] hiddenParams)
//        {
//            FourFTTable table = (FourFTTable) hiddenParams[0];
//            if (p <= 0.0f) return (- _alpha);
//            if (p >= 1.0f) return (1.0f - _alpha);
//            int a = table.A; int x = table.A + table.B + table.C;
//            double sum = 0.0;
//            for (int i = 0; i <= a; i++)
//            {
//                sum += Math.Exp(Combinatorics.LogFactorial(x) - Combinatorics.LogFactorial(i) - Combinatorics.LogFactorial(x - i) + ((double) i) * Math.Log(p) + ((double) (x - i)) * Math.Log(1.0 - p));
//            }
//            return ((float) sum - _alpha);
//        }
        

//        /// <summary>
//        /// Computes the statistical strength value at the specified significance (alpha).
//        /// </summary>
//        /// <param name="table">A reference to <see cref="FourFTTable">4ft-table</see>.</param>
//        /// <returns>Statistical strength value at the specified significance (alpha).</returns>
//        /// <remarks>
//        /// <para>Computes the numerical solution of the following equation (for variable p):</para>
//        /// <para><b>Sum[i = 0..a] x! / (i! * (x - i)!) * p^i * (1.0 - p)^(x - i) - alpha = 0.0</b>,
//        /// where <b>x = (a + b + c)</b>.</para>
//        /// <para>The solution must be between 0.0 and 1.0 (inclusive).</para>
//        /// </remarks>
//        public float Value(FourFTTable table)
//        {
//            return Combinatorics.BinarySearch(0.0f, 1.0f, new Combinatorics.ExaminedFloatFunction(QuantifierHelper), table);
//        }


//        /// <summary>
//        /// Returns <b>true</b> if the statistical strength value is greater than or equal to the p parameter with the specified statistical significance (alpha).
//        /// </summary>
//        /// <param name="table">A reference to <see cref="FourFTTable">4ft-table</see>.</param>
//        /// <returns><b>true</b> if if the statistical strength value is greater than or equal to the p parameter with the specified statistical significance (alpha).</returns>
//        /// <remarks>
//        /// <para>It computes the following condition:</para>
//        /// <para><b>Sum[i = 0..a] x! / (i! * (x - i)!) * p^i * (1 - p)^(x - i) &gt; alpha</b>,
//        /// where <b>x = (a + b + c)</b>.</para>
//        /// </remarks>
//        public bool Validity(FourFTTable table)
//        {
//            return (QuantifierHelper(_p, table) > 0.0f);
//        }
//    }


//    /// <summary>
//    /// Founded equivalence quantifier.
//    /// </summary>
//    /// <remarks>
//    /// <para>Defined as a condition (a + d) / (a + b + c + d) &gt;= p.</para>
//    /// </remarks>
//    [Serializable]
//    public class FoundedEquivalence : IFourFTQuantifier
//    {
//        /// <summary>
//        /// Constructor.
//        /// </summary>
//        /// <param name="p">The strength parameter.</param>
//        /// <exception cref="ArgumentOutOfRangeException">The parameter p must be between 0 and 1 (inclusive).</exception>
//        public FoundedEquivalence(float p)
//        {
//            P = p;
//        }

        
//        /// <summary>
//        /// Gets or sets the strength parameter.
//        /// </summary>
//        /// <exception cref="ArgumentOutOfRangeException">The parameter p must be between 0 and 1 (inclusive).</exception>
//        public float P
//        {
//            get
//            {
//                return _p;
//            }
//            set
//            {
//                if ((value < 0.0f) || (value > 1.0f))
//                    throw new ArgumentOutOfRangeException("value", value, "The parameter p must be between 0 and 1 (inclusive).");

//                _p = value;
//            }
//        }
//        private float _p;

        
//        /// <summary>
//        /// Computes the <b>strength</b> of founded equivalence.
//        /// </summary>
//        /// <param name="table">A reference to <see cref="FourFTTable">4ft-table</see>.</param>
//        /// <returns>The strength defined as <b>(a + d) / (a + b + c + d)</b>.</returns>
//        /// <remarks>
//        /// <para>If (a + b + c + d) = 0, returns 0.</para>
//        /// </remarks>
//        public float Value(FourFTTable table)
//        {
//            return (table.N > 0) ? ((float) table.A) / ((float) table.N) : 0.0f;
//        }


//        /// <summary>
//        /// Returns <b>true</b> iff the strength is greater than or equal to the strength parameter.
//        /// </summary>
//        /// <param name="table">A reference to <see cref="FourFTTable">4ft-table</see>.</param>
//        /// <returns><b>true</b> iff the strength defined as <b>(a + d) / (a + b + c + d)</b> is greater than or equal to the strength parameter.</returns>
//        /// <remarks>
//        /// <para>If (a + b + c + d) = 0, returns false.</para>
//        /// </remarks>
//        public bool Validity(FourFTTable table)
//        {
//            return (table.N > 0) && (((float) table.A) >= P * ((float) table.N));
//        }
//    }


//    /// <summary>
//    /// Lower critical equivalence quantifier.
//    /// </summary>
//    /// <remarks>
//    /// <para>Lower critical equivalence quantifier is defined as a condition
//    /// <b>Sum[i = a..n] n! / (i! * (n - i)!) * p^i * (1 - p)^(n - i) &lt;= alpha</b>.</para>
//    /// </remarks>
//    [Serializable]
//    public class LowerCriticalEquivalence : IFourFTQuantifier
//    {
//        /// <summary>
//        /// Constructor.
//        /// </summary>
//        /// <param name="p">Statistical strength parameter.</param>
//        /// <param name="alpha">Statistical significance (usually 5%).</param>
//        /// <exception cref="ArgumentOutOfRangeException">
//        /// <list type="bullet">
//        /// <item>The parameter p must be between 0 and 1 (inclusive).</item>
//        /// <item>The parameter alpha must be greater than 0.0 and less then or equal to 0.5.</item>
//        /// </list>
//        /// </exception>
//        public LowerCriticalEquivalence(float p, float alpha)
//        {
//            P = p;
//            Alpha = alpha;
//        }


//        /// <summary>
//        /// Gets or sets the statistical strength parameter.
//        /// </summary>
//        /// <exception cref="ArgumentOutOfRangeException">The parameter p must be between 0 and 1 (inclusive).</exception>
//        public float P
//        {
//            get
//            {
//                return _p;
//            }
//            set
//            {
//                if ((value < 0.0f) || (value > 1.0f))
//                    throw new ArgumentOutOfRangeException("value", value, "The parameter p must be between 0 and 1 (inclusive).");

//                _p = value;
//            }
//        }
//        private float _p;
        
        
//        /// <summary>
//        /// Gets or sets the statistical significance (usually 5%).
//        /// </summary>
//        /// <exception cref="ArgumentOutOfRangeException">The parameter alpha must be greater than 0.0 and less then or equal to 0.5.</exception>
//        public float Alpha
//        {
//            get
//            {
//                return _alpha;
//            }
//            set
//            {
//                if ((value <= 0.0f) || (value > 0.5f))
//                    throw new ArgumentOutOfRangeException("value", value, "The parameter alpha must be greater than 0.0 and less then or equal to 0.5.");

//                _alpha = value;
//            }
//        }
//        private float _alpha;


//        // computes a value of DLCI sum minus alpha
//        private float QuantifierHelper(float p, params object[] hiddenParams)
//        {
//            FourFTTable table = (FourFTTable) hiddenParams[0];
//            if (p <= 0.0f) return (- _alpha);
//            if (p >= 1.0f) return (1.0f - _alpha);
//            int a = table.A; int n = table.N;
//            double sum = 0.0;
//            for (int i = a; i <= n; i++)
//            {
//                sum += Math.Exp(Combinatorics.LogFactorial(n) - Combinatorics.LogFactorial(i) - Combinatorics.LogFactorial(n - i) + ((double) i) * Math.Log(p) + ((double) (n - i)) * Math.Log(1.0 - p));
//            }
//            return ((float) sum - _alpha);
//        }
        

//        /// <summary>
//        /// Computes the statistical strength value at the specified significance (alpha).
//        /// </summary>
//        /// <param name="table">A reference to <see cref="FourFTTable">4ft-table</see>.</param>
//        /// <returns>Statistical strength value at the specified significance (alpha).</returns>
//        /// <remarks>
//        /// <para>Computes the numerical solution of the following equation (for variable p):</para>
//        /// <para><b>Sum[i = a..n] n! / (i! * (n - i)!) * p^i * (1.0 - p)^(n - i) - alpha = 0.0</b>.</para>
//        /// <para>The solution must be between 0.0 and 1.0 (inclusive).</para>
//        /// </remarks>
//        public float Value(FourFTTable table)
//        {
//            return Combinatorics.BinarySearch(0.0f, 1.0f, new Combinatorics.ExaminedFloatFunction(QuantifierHelper), table);
//        }


//        /// <summary>
//        /// Returns <b>true</b> if the statistical strength value is greater than or equal to the p parameter with the specified statistical significance (alpha).
//        /// </summary>
//        /// <param name="table">A reference to <see cref="FourFTTable">4ft-table</see>.</param>
//        /// <returns><b>true</b> if if the statistical strength value is greater than or equal to the p parameter with the specified statistical significance (alpha).</returns>
//        /// <remarks>
//        /// <para>It computes the following condition:</para>
//        /// <para><b>Sum[i = a..n] n! / (i! * (n - i)!) * p^i * (1 - p)^(n - i) &lt;= alpha</b>.</para>
//        /// </remarks>
//        public bool Validity(FourFTTable table)
//        {
//            return (QuantifierHelper(_p, table) <= 0.0f);
//        }
//    }


//    /// <summary>
//    /// Upper critical equivalence quantifier.
//    /// </summary>
//    /// <remarks>
//    /// <para>Upper critical equivalence quantifier is defined as a condition
//    /// <b>Sum[i = 0..a] n! / (i! * (n - i)!) * p^i * (1 - p)^(n - i) &gt; alpha</b>.</para>
//    /// </remarks>
//    [Serializable]
//    public class UpperCriticalEquivalence : IFourFTQuantifier
//    {
//        /// <summary>
//        /// Constructor.
//        /// </summary>
//        /// <param name="p">Statistical strength parameter.</param>
//        /// <param name="alpha">Statistical significance (usually 5%).</param>
//        /// <exception cref="ArgumentOutOfRangeException">
//        /// <list type="bullet">
//        /// <item>The parameter p must be between 0 and 1 (inclusive).</item>
//        /// <item>The parameter alpha must be greater than 0.0 and less then or equal to 0.5.</item>
//        /// </list>
//        /// </exception>
//        public UpperCriticalEquivalence(float p, float alpha)
//        {
//            P = p;
//            Alpha = alpha;
//        }


//        /// <summary>
//        /// Gets or sets the statistical strength parameter.
//        /// </summary>
//        /// <exception cref="ArgumentOutOfRangeException">The parameter p must be between 0 and 1 (inclusive).</exception>
//        public float P
//        {
//            get
//            {
//                return _p;
//            }
//            set
//            {
//                if ((value < 0.0f) || (value > 1.0f))
//                    throw new ArgumentOutOfRangeException("value", value, "The parameter p must be between 0 and 1 (inclusive).");

//                _p = value;
//            }
//        }
//        private float _p;
        
        
//        /// <summary>
//        /// Gets or sets the statistical significance (usually 5%).
//        /// </summary>
//        /// <exception cref="ArgumentOutOfRangeException">The parameter alpha must be greater than 0.0 and less then or equal to 0.5.</exception>
//        public float Alpha
//        {
//            get
//            {
//                return _alpha;
//            }
//            set
//            {
//                if ((value <= 0.0f) || (value > 0.5f))
//                    throw new ArgumentOutOfRangeException("value", value, "The parameter alpha must be greater than 0.0 and less then or equal to 0.5.");

//                _alpha = value;
//            }
//        }
//        private float _alpha;


//        // computes a value of UCE sum minus alpha
//        private float QuantifierHelper(float p, params object[] hiddenParams)
//        {
//            FourFTTable table = (FourFTTable) hiddenParams[0];
//            if (p <= 0.0f) return (- _alpha);
//            if (p >= 1.0f) return (1.0f - _alpha);
//            int a = table.A; int n = table.N;
//            double sum = 0.0;
//            for (int i = 0; i <= a; i++)
//            {
//                sum += Math.Exp(Combinatorics.LogFactorial(n) - Combinatorics.LogFactorial(i) - Combinatorics.LogFactorial(n - i) + ((double) i) * Math.Log(p) + ((double) (n - i)) * Math.Log(1.0 - p));
//            }
//            return ((float) sum - _alpha);
//        }
        

//        /// <summary>
//        /// Computes the statistical strength value at the specified significance (alpha).
//        /// </summary>
//        /// <param name="table">A reference to <see cref="FourFTTable">4ft-table</see>.</param>
//        /// <returns>Statistical strength value at the specified significance (alpha).</returns>
//        /// <remarks>
//        /// <para>Computes the numerical solution of the following equation (for variable p):</para>
//        /// <para><b>Sum[i = 0..a] n! / (i! * (n - i)!) * p^i * (1.0 - p)^(n - i) - alpha = 0.0</b>.</para>
//        /// <para>The solution must be between 0.0 and 1.0 (inclusive).</para>
//        /// </remarks>
//        public float Value(FourFTTable table)
//        {
//            return Combinatorics.BinarySearch(0.0f, 1.0f, new Combinatorics.ExaminedFloatFunction(QuantifierHelper), table);
//        }


//        /// <summary>
//        /// Returns <b>true</b> if the statistical strength value is greater than or equal to the p parameter with the specified statistical significance (alpha).
//        /// </summary>
//        /// <param name="table">A reference to <see cref="FourFTTable">4ft-table</see>.</param>
//        /// <returns><b>true</b> if if the statistical strength value is greater than or equal to the p parameter with the specified statistical significance (alpha).</returns>
//        /// <remarks>
//        /// <para>It computes the following condition:</para>
//        /// <para><b>Sum[i = 0..a] n! / (i! * (n - i)!) * p^i * (1 - p)^(n - i) &gt; alpha</b>.</para>
//        /// </remarks>
//        public bool Validity(FourFTTable table)
//        {
//            return (QuantifierHelper(_p, table) > 0.0f);
//        }
//    }


//    /// <summary>
//    /// Simple deviation quantifier.
//    /// </summary>
//    /// <remarks>
//    /// <para>Simple deviation quantifier is defined as the condition <b>(a * d) >= 2^k * (b * c)</b>, where <b>k</b> is a strength parameter.</para>
//    /// <para>See chapter 2.2.4 (e) in GUHA-book (simple association, which is a simpler form without parameter).</para>
//    /// <para>This quantifier behaves strangely when there are some zeros in 4ft-table. For example, for the table (a = 1000, b = 1, c = 1, d = 0),
//    /// we can see strong association between antecedent and succedent and thus strong deviation from their independency; having d = 0 means that the quantifier fails completely.
//    /// Similarly, very small numbers in contrast with large numbers in one 4ft-table are not quite testifying as well. The best case is when all four values are "in balance".</para>
//    /// </remarks>
//    [Serializable]
//    public class SimpleDeviation : IFourFTQuantifier
//    {
//        /// <summary>
//        /// Constructor.
//        /// </summary>
//        /// <param name="k">The parameter of strength.</param>
//        public SimpleDeviation(float k)
//        {
//            K = k;
//        }


//        /// <summary>
//        /// Gets or sets the parameter of strength.
//        /// </summary>
//        /// <remarks>
//        /// <para>Although the parameter can be any real value, it is supposed to be greater than or equal to zero.</para>
//        /// </remarks>
//        public float K
//        {
//            get
//            {
//                return _k;
//            }
//            set
//            {
//                _k = value;
//            }
//        }
//        private float _k;

        
//        /// <summary>
//        /// Computes the simple deviation strength value.
//        /// </summary>
//        /// <param name="table">A reference to <see cref="FourFTTable">4ft-table</see>.</param>
//        /// <returns>Simple deviation strength value defined as <b>ln(ad/bc) / ln(2)</b>.</returns>
//        /// <remarks>
//        /// <para>There are special cases defined explicitly:</para>
//        /// <para>If both <b>(a * d) = 0</b> and <b>(b * c) = 0</b>, return 0.</para>
//        /// <para>If only <b>(a * d) = 0</b>, return -INF.</para>
//        /// <para>If only <b>(b * c) = 0</b>, return +INF.</para>
//        /// </remarks>
//        public float Value(FourFTTable table)
//        {
//            long ad = ((long) table.A) * ((long) table.D);
//            long bc = ((long) table.B) * ((long) table.C);
//            if ((ad == 0) && (bc == 0)) return 0.0f;
//            if (ad == 0) return Single.NegativeInfinity;
//            if (bc == 0) return Single.PositiveInfinity;
//            return (float) (Math.Log(((double) ad) / ((double) bc)) / 0.693147180559945309417);
//        }


//        /// <summary>
//        /// Returns <b>true</b> if the simple deviation strength is greater than or equal to the strength parameter.
//        /// </summary>
//        /// <param name="table">A reference to <see cref="FourFTTable">4ft-table</see>.</param>
//        /// <returns><b>true</b> iff <b>(a * d) &gt;= 2^k * (b * c)</b>.</returns>
//        /// <remarks>
//        /// <para>There are special cases defined explicitly:</para>
//        /// <para>If both <b>(a * d) = 0</b> and <b>(b * c) = 0</b>, return true if <b>k &lt;= 0</b>.</para>
//        /// <para>If only <b>(a * d) = 0</b>, return <b>false</b>.</para>
//        /// <para>If only <b>(b * c) = 0</b>, return <b>true</b>.</para>
//        /// </remarks>
//        public bool Validity(FourFTTable table)
//        {
//            long ad = ((long) table.A) * ((long) table.D);
//            long bc = ((long) table.B) * ((long) table.C);
//            if ((ad == 0) && (bc == 0)) return (_k <= 0.0f);
//            if (ad == 0) return false;
//            if (bc == 0) return true;
//            return ((double) ad) >= Math.Pow(2, _k) * ((double) bc);
//        }
//    }


//    /// <summary>
//    /// Fisher quantifier.
//    /// </summary>
//    /// <remarks>
//    /// <para>Fisher quantifier is a statistical test of independence between antecedent and succedent (null hypothesis) against positive dependence (alternative hypothesis) on the level alpha.</para>
//    /// <para>It is defined as the condition <b>(a * d) &gt; (b * c)  &amp;  Sum[i = a..x] (r! * s! * k! * l!) / (n! * i! * (r-i)! * (k-i)! * (n+i-r-k)!) &lt;= alpha</b>, where <b>x = min(r, k)</b>.</para>
//    /// <para>See chapter 4.4.20 in GUHA-book.</para>
//    /// </remarks>
//    [Serializable]
//    public class Fisher: IFourFTQuantifier
//    {
//        /// <summary>
//        /// Constructor.
//        /// </summary>
//        /// <param name="alpha">Statistical significance (usually 5%).</param>
//        /// <exception cref="ArgumentOutOfRangeException">The parameter alpha must be greater than 0.0 and less then or equal to 0.5.</exception>
//        public Fisher(float alpha)
//        {
//            Alpha = alpha;
//        }


//        /// <summary>
//        /// Gets or sets the statistical significance (usually 5%).
//        /// </summary>
//        /// <exception cref="ArgumentOutOfRangeException">The parameter alpha must be greater than 0.0 and less then or equal to 0.5.</exception>
//        public float Alpha
//        {
//            get
//            {
//                return _alpha;
//            }
//            set
//            {
//                if ((value <= 0.0f) || (value > 0.5f))
//                    throw new ArgumentOutOfRangeException("value", value, "The parameter alpha must be greater than 0.0 and less then or equal to 0.5.");

//                _alpha = value;
//            }
//        }
//        private float _alpha;

        
//        private static float FisherValue(FourFTTable table)
//        {
//            double sum = 0.0;
//            int stop = (table.R < table.K) ? table.R : table.K;
//            for (int i = table.A; i <= stop; i++)
//            {
//                double x = 
//                    + Combinatorics.LogFactorial(table.R) 
//                    + Combinatorics.LogFactorial(table.S)
//                    + Combinatorics.LogFactorial(table.K)
//                    + Combinatorics.LogFactorial(table.L)
//                    - Combinatorics.LogFactorial(table.N)
//                    - Combinatorics.LogFactorial(i)
//                    - Combinatorics.LogFactorial(table.R - i)
//                    - Combinatorics.LogFactorial(table.K - i)
//                    - Combinatorics.LogFactorial(table.N + i - table.R - table.K);
//                sum += Math.Exp(x);
//            }
//            return (float) sum;
//        }

        
//        /// <summary>
//        /// Computes the value of Fisher quantifier.
//        /// </summary>
//        /// <param name="table">A reference to <see cref="FourFTTable">4ft-table</see>.</param>
//        /// <returns>The value <b>Sum[i = a..x] (r! * s! * k! * l!) / (n! * i! * (r-i)! * (k-i)! * (n+i-r-k)!)</b>, where <b>x = min(r, k)</b>.</returns>
//        /// <remarks>
//        /// <para>There is a special case defined explicitly:</para>
//        /// <para>If <b>(a * d) &lt;= (b * c)</b>, return 0.</para>
//        /// </remarks>
//        public float Value(FourFTTable table)
//        {
//            if (((long) table.A) * ((long) table.D) <= ((long) table.B) * ((long) table.C)) return 0.0f;
//            return FisherValue(table);
//        }


//        /// <summary>
//        /// Returns <b>true</b> if the antecedent and succedent are positively associated (in terms of Fisher quantifier).
//        /// </summary>
//        /// <param name="table">A reference to <see cref="FourFTTable">4ft-table</see>.</param>
//        /// <returns><b>true</b> iff <b>(a * d) &gt; (b * c)</b> and <b>Sum[i = a..x] (r! * s! * k! * l!) / (n! * i! * (r-i)! * (k-i)! * (n+i-r-k)!) &lt;= alpha</b>, where <b>x = min(r, k)</b>.</returns>
//        public bool Validity(FourFTTable table)
//        {
//            if (((long) table.A) * ((long) table.D) <= ((long) table.B) * ((long) table.C)) return false;
//            return (FisherValue(table) <= _alpha);
//        }
//    }


//    /// <summary>
//    /// Chi-square quantifier.
//    /// </summary>
//    /// <remarks>
//    /// <para>Chi-square quantifier is defined as a condition <b>(a * d) &gt; (b * c)  &amp;&amp;  n * (a * d - b * c) ^ 2 &gt;= chisq * r * s * k * l</b>,
//    /// where chisq is (1-alpha) quantile of the chi-square distribution function (with 1 degree of freedom, because this is a four-fold contingency table).</para>
//    /// <para>See chapter 4.4.23 in GUHA-book.</para>
//    /// </remarks>
//    [Serializable]
//    public class ChiSquare : IFourFTQuantifier
//    {
//        /// <summary>
//        /// Constructor.
//        /// </summary>
//        /// <param name="alpha">Statistical significance (usually 5%).</param>
//        /// <exception cref="ArgumentOutOfRangeException">The parameter alpha must be greater than 0.0 and less then or equal to 0.5.</exception>
//        public ChiSquare(float alpha)
//        {
//            Alpha = alpha;
//        }


//        /// <summary>
//        /// Gets or sets the statistical significance (usually 5%).
//        /// </summary>
//        /// <exception cref="ArgumentOutOfRangeException">The parameter alpha must be greater than 0.0 and less then or equal to 0.5.</exception>
//        public float Alpha
//        {
//            get
//            {
//                return _alpha;
//            }
//            set
//            {
//                if ((value <= 0.0f) || (value > 0.5f))
//                    throw new ArgumentOutOfRangeException("value", value, "The parameter alpha must be greater than 0.0 and less then or equal to 0.5.");

//                _alpha = value;
//                _criticalValue = Combinatorics.ChiSquareCdfInv(value, 1);  // 1 degree of freedom for four-fold contingency table
//            }
//        }
//        private float _alpha;
//        private double _criticalValue;

        
//        /// <summary>
//        /// Returns the value of alpha that would be neccessary to reject a null hypothesis.
//        /// </summary>
//        /// <param name="table">A reference to <see cref="FourFTTable">4ft-table</see>.</param>
//        /// <returns>The minimum value of alpha that is neccessary to reject a null hypothesis.</returns>
//        public float Value(FourFTTable table)
//        {
//            double x = ((double) table.A) * ((double) table.D) - ((double) table.B) * ((double) table.C);  // ad - bc
//            if (x <= 0)
//                return Single.PositiveInfinity;

//            double y = (((double) table.N) * x * x) / (((double) table.R) * ((double) table.S) * ((double) table.K) * ((double) table.L));
//            return 1.0f - ((float) Combinatorics.ChiSquareCdf(y, 1));
//        }


//        /// <summary>
//        /// The 4ft-quantifier chi-square is valid, if the null hypothesis of independence between antecedent and succedent is rejected,
//        /// thus we say that antecedent and succedent are not independent.
//        /// </summary>
//        /// <param name="table">A reference to <see cref="FourFTTable">4ft-table</see>.</param>
//        /// <returns><b>true</b> if the null hypothesis of independence is rejected.</returns>
//        public bool Validity(FourFTTable table)
//        {
//            double x = ((double) table.A) * ((double) table.D) - ((double) table.B) * ((double) table.C);  // ad - bc
//            if (x <= 0)
//                return false;

//            return ((double) table.N) * x * x >= _criticalValue * ((double) table.R) * ((double) table.S) * ((double) table.K) * ((double) table.L);
//        }
//    }


//    /// <summary>
//    /// Above average quantifier.
//    /// </summary>
//    /// <remarks>
//    /// <para>Defined as a condition <b>a / (a + b) &gt;= k * (a + c) / (a + b + c + d)</b>.</para>
//    /// </remarks>
//    [Serializable]
//    public class AboveAverage : IFourFTQuantifier
//    {
//        /// <summary>
//        /// Constructor.
//        /// </summary>
//        /// <param name="k">The parameter of strength.</param>
//        /// <exception cref="ArgumentOutOfRangeException">The parameter of above average quantifier must be greater than or equal to 0.</exception>
//        public AboveAverage(float k)
//        {
//            K = k;
//        }


//        /// <summary>
//        /// Gets or sets the parameter of strength.
//        /// </summary>
//        /// <exception cref="ArgumentOutOfRangeException">The parameter of above average quantifier must be greater than or equal to 0.</exception>
//        public float K
//        {
//            get
//            {
//                return _k;
//            }
//            set
//            {
//                if (value < 0)
//                    throw new ArgumentOutOfRangeException("value", value, "The parameter of above average quantifier must be greater than or equal to 0.");

//                _k = value;
//            }
//        }
//        private float _k;

        
//        /// <summary>
//        /// Computes the above average strength value.
//        /// </summary>
//        /// <param name="table">A reference to <see cref="FourFTTable">4ft-table</see>.</param>
//        /// <returns>Above average strength value defined as <b>(a / (a + b)) * ((a + b + c + d) / (a + c))</b> if <b>a &gt; 0</b>; otherwise it returns zero.</returns>
//        /// <remarks>
//        /// <para>If a = 0, returns 0.</para>
//        /// </remarks>
//        public float Value(FourFTTable table)
//        {
//            return (table.A > 0) ? (((float) table.A) * ((float) table.N)) / (((float) table.R) * ((float) table.K)) : 0.0f;
//        }


//        /// <summary>
//        /// Returns <b>true</b> if the above average strength is greater than or equal to the strength parameter.
//        /// </summary>
//        /// <param name="table">A reference to <see cref="FourFTTable">4ft-table</see>.</param>
//        /// <returns><b>true</b> if <b>a &gt; 0</b> and the strength defined as <b>(a / (a + b)) * ((a + b + c + d) / (a + c))</b> is greater than or equal to the strength parameter.</returns>
//        /// <remarks>
//        /// <para>If a = 0, returns false.</para>
//        /// </remarks>
//        public bool Validity(FourFTTable table)
//        {
//            return (table.A > 0) && ((((float) table.A) * ((float) table.N)) / (((float) table.R) * ((float) table.K)) >= _k);
//        }
//    }


//    /// <summary>
//    /// Below average quantifier.
//    /// </summary>
//    /// <remarks>
//    /// <para>Defined as a condition <b>a / (a + b) &lt;= (1 / k) * (a + c) / (a + b + c + d)</b>.</para>
//    /// </remarks>
//    [Serializable]
//    public class BelowAverage : IFourFTQuantifier
//    {
//        /// <summary>
//        /// Constructor.
//        /// </summary>
//        /// <param name="k">The parameter of strength.</param>
//        /// <exception cref="ArgumentOutOfRangeException">The parameter of below average quantifier must be greater than 0.</exception>
//        public BelowAverage(float k)
//        {
//            K = k;
//        }


//        /// <summary>
//        /// Gets or sets the parameter of strength.
//        /// </summary>
//        /// <exception cref="ArgumentOutOfRangeException">The parameter of below average quantifier must be greater than or equal to 0.</exception>
//        public float K
//        {
//            get
//            {
//                return _k;
//            }
//            set
//            {
//                if (value <= 0)
//                    throw new ArgumentOutOfRangeException("value", value, "The parameter of below average quantifier must be greater than 0.");

//                _k = value;
//            }
//        }
//        private float _k;

        
//        /// <summary>
//        /// Computes the below average strength value.
//        /// </summary>
//        /// <param name="table">A reference to <see cref="FourFTTable">4ft-table</see>.</param>
//        /// <returns>Below average strength value defined as <b>((a + b) / a) * ((a + c) / (a + b + c + d))</b> with two exceptions: returns zero if <b>(a + c) = 0</b> and returns +INF if <b>a = 0</b>.</returns>
//        /// <remarks>
//        /// <para>The below average quantifier value must be explicitly defined for <b>a = 0</b> as +INF to prevent division by zero.</para>
//        /// <para>Furthermore, if <b>(a + c) = 0</b>, the result +INF (= extremely strong hypothesis) would not make sense, so it is defined as zero (= no hypothesis) instead.</para>
//        /// </remarks>
//        public float Value(FourFTTable table)
//        {
//            if (table.K == 0) return 0.0f;
//            if (table.A == 0) return Single.PositiveInfinity;
//            return (((float) table.R) * ((float) table.K)) / (((float) table.A) * ((float) table.N));
//        }


//        /// <summary>
//        /// Returns <b>true</b> if the below average strength is greater than or equal to the strength parameter.
//        /// </summary>
//        /// <param name="table">A reference to <see cref="FourFTTable">4ft-table</see>.</param>
//        /// <returns><b>true</b> if the strength defined as <b>((a + b) / a) * ((a + c) / (a + b + c + d))</b> is greater than or equal to the strength parameter.</returns>
//        /// <remarks>
//        /// <para>If <b>(a + c) = 0</b>, return false.</para>
//        /// <para>If <b>a = 0</b>, returns true.</para>
//        /// </remarks>
//        public bool Validity(FourFTTable table)
//        {
//            if (table.K == 0) return false;
//            if (table.A == 0) return true;
//            return ((((float) table.R) * ((float) table.K)) / (((float) table.A) * ((float) table.N)) >= _k);
//        }
//    }


//    /// <summary>
//    /// Outside average quantifier.
//    /// </summary>
//    /// <remarks>
//    /// <para>Defined as a compound condition of above and below average, i.e.<br />
//    /// <b>a / (a + b) &gt;= k * (a + c) / (a + b + c + d)</b> <i>or</i><br />
//    /// <b>a / (a + b) &lt;= (1 / k) * (a + c) / (a + b + c + d)</b>.</para>
//    /// </remarks>
//    [Serializable]
//    public class OutsideAverage : IFourFTQuantifier
//    {
//        /// <summary>
//        /// Constructor.
//        /// </summary>
//        /// <param name="k">The parameter of strength.</param>
//        /// <exception cref="ArgumentOutOfRangeException">The parameter of outside average quantifier must be greater than or equal to 1.</exception>
//        public OutsideAverage(float k)
//        {
//            K = k;
//        }


//        /// <summary>
//        /// Gets or sets the parameter of strength.
//        /// </summary>
//        /// <exception cref="ArgumentOutOfRangeException">The parameter of outside average quantifier must be greater than or equal to 1.</exception>
//        public float K
//        {
//            get
//            {
//                return _k;
//            }
//            set
//            {
//                if (value < 1.0f)
//                    throw new ArgumentOutOfRangeException("value", value, "The parameter of outside average quantifier must be greater than or equal to 1.");

//                _k = value;
//            }
//        }
//        private float _k;

        
//        /// <summary>
//        /// Computes the outside average strength value.
//        /// </summary>
//        /// <param name="table">A reference to <see cref="FourFTTable">4ft-table</see>.</param>
//        /// <returns>Outside average strength value defined as maximum of two values:<br />
//        /// <b>(a / (a + b)) * ((a + b + c + d) / (a + c))</b> and<br />
//        /// <b>((a + b) / a) * ((a + c) / (a + b + c + d))</b>.</returns>
//        /// <remarks>
//        /// <para>Note that the two values are reciprocal. Thus we can compute one of it (denote it x) and return max(x, 1/x).</para>
//        /// <para>There are special cases, where the value must be defined explicitly:</para>
//        /// <para>For <b>(a + c) = 0</b>, return 1 (explanation: the succedent does not exist at all, so its occurence is neither above nor below average when antecedent holds true).</para>
//        /// <para>For <b>a = 0</b>, return +INF (explanation: the succedent exists only when antecedent is not true, so its occurence is +INF-times smaller than in average, i.e. the below-average-part of the quantifier holds true).</para>
//        /// </remarks>
//        public float Value(FourFTTable table)
//        {
//            if (table.K == 0) return 1.0f;
//            if (table.A == 0) return Single.PositiveInfinity;
//            float x = (((float) table.A) * ((float) table.N)) / (((float) table.R) * ((float) table.K));
//            return Math.Max(x, (1 / x));
//        }


//        /// <summary>
//        /// Returns <b>true</b> if the outside average strength is greater than or equal to the strength parameter.
//        /// </summary>
//        /// <param name="table">A reference to <see cref="FourFTTable">4ft-table</see>.</param>
//        /// <returns><b>true</b> if the max(x, 1/x) is greater than or equal to k, where <b>x = (a / (a + b)) * ((a + b + c + d) / (a + c))</b>.</returns>
//        /// <remarks>
//        /// <para>There are special cases, where the validity must be defined explicitely (see <see cref="Value">Value()</see> method for explanation):</para>
//        /// <para>For <b>(a + c) = 0</b>, return true only if the parameter k = 1.</para>
//        /// <para>For <b>a = 0</b>, return true.</para>
//        /// </remarks>
//        public bool Validity(FourFTTable table)
//        {
//            if (table.K == 0) return (_k <= 1.0f);
//            if (table.A == 0) return true;
//            float x = (((float) table.A) * ((float) table.N)) / (((float) table.R) * ((float) table.K));
//            return Math.Max(x, (1 / x)) >= _k;
//        }
//    }
//}
