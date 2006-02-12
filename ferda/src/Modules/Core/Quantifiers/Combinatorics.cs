/*
 * 
 * Author: Tomáš Karban <karby@matfyz.cz>
 * 
 */

using System;
using System.Collections;
using System.Diagnostics;

namespace Ferda.Modules.Quantifiers
{
    /// <summary>
    /// Exposes some combinatoric computing as static methods.
    /// </summary>
    /// <remarks>
    /// It is not possible to create instances of this class. All methods are static.
    /// </remarks>
    public sealed class Combinatorics
    {
        private Combinatorics()
        {
            // it is not possible to create instances of this class.
        }


        static Combinatorics()
        {
            #region Initialization of factorials (long)
            long factorial = 1;  // value for 0!
            _factorials[0] = factorial;
            for (int i = 1; i < 21; i++)
            {
                factorial *= i;
                _factorials[i] = factorial;
            }
            #endregion
            
            #region Initialization of factorials (double)
            double factorialDouble = 1.0;  // value for 0!
            _factorialsDouble[0] = factorialDouble;
            for (int i = 1; i < _factorialsDoubleMax; i++)
            {
                factorialDouble *= i;
                _factorialsDouble[i] = factorialDouble;
            }
            #endregion

            #region Initialization of log factorials
            double logFactorial = 0.0;  // value for log(0!)
            _logFactorials[0] = logFactorial;
            for (int i = 1; i < _logFactorialsMax; i++)
            {
                logFactorial += Math.Log(i);
                _logFactorials[i] = logFactorial;
            }
            #endregion

            #region Initialization of binomial coefficients
            const int initRows = 4;
            const int initColumns = 32;
            _pascalTriangle = new long[initRows][];  // jagged array
            _pascalTriangleFilled = new int[initRows];
            
            // fill the first row
            _pascalTriangle[0] = new long[initColumns];
            for (int j = 0; j < initColumns; j++)
            {
                _pascalTriangle[0][j] = j + 2;
            }
            _pascalTriangleFilled[0] = initColumns - 1;

            // fill other rows
            for (int i = 1; i < initRows; i++)
            {
                _pascalTriangle[i] = new long[initColumns];
                _pascalTriangle[i][0] = i + 2;
                for (int j = 1; j < initColumns; j++)
                {
                    _pascalTriangle[i][j] = _pascalTriangle[i - 1][j] + _pascalTriangle[i][j - 1];
                }
                _pascalTriangleFilled[i] = initColumns - 1;
            }
            #endregion
        }


        // factorials
        private const int _factorialsMax = 21;
        private static long[] _factorials = new long[_factorialsMax];
        private const int _factorialsDoubleMax = 171;
        private static double[] _factorialsDouble = new double[_factorialsDoubleMax];
        private const int _logFactorialsMax = 1000;
        private static double[] _logFactorials = new double[_logFactorialsMax];
        
        // pascal triangle for small binnomial coefficients
        private static long[][] _pascalTriangle;  // jagged array
        private static int[] _pascalTriangleFilled;


        /// <summary>
        /// Returns a natural logarithm of a factorial.
        /// </summary>
        /// <param name="input">The number from which to compute natural logarithm of a factorial.</param>
        /// <returns>Natural logarithm of a factorial of the specified number.</returns>
        /// <remarks>This method uses precise computation for inputs less than 1000. For bigger numbers it uses a numerical computation - <see cref="LogGamma">logarithm of a gamma function</see>.</remarks>
        public static double LogFactorial(int input)
        {
            if (input < 0)
                throw new ArgumentOutOfRangeException("taskDescription", input, "LogFactorial can be computed from non-negative integers only.");

            // use table lookup for small values
            if (input < _logFactorialsMax)
                return _logFactorials[input];

            // use gamma function for bigger values
            return LogGamma(input + 1);
        }


        /// <summary>
        /// Returns a factorial of a given number.
        /// </summary>
        /// <param name="input">A number to compute factorial from.</param>
        /// <returns>Factorial of a given number.</returns>
        public static long Factorial(int input)
        {
            if (input < 0)
                throw new ArgumentOutOfRangeException("taskDescription", input, "Factorial can be computed from non-negative integers only.");
            if (input >= _factorialsMax)
                throw new System.OverflowException(String.Format(System.Globalization.CultureInfo.InvariantCulture, "Factorial can be computed from integers less than {0} only.", _factorialsMax));

            return _factorials[input];
        }


        /// <summary>
        /// Returns a factorial of a given number.
        /// </summary>
        /// <param name="input">A number to compute factorial from.</param>
        /// <returns>Factorial of a given number.</returns>
        public static double FactorialDouble(int input)
        {
            if (input < 0)
                throw new ArgumentOutOfRangeException("taskDescription", input, "FactorialDouble can be computed from non-negative integers only.");
            if (input >= _factorialsDoubleMax)
                throw new System.OverflowException(String.Format(System.Globalization.CultureInfo.InvariantCulture, "FactorialDouble can be computed from integers less than {0} only.", _factorialsDoubleMax));

            return _factorialsDouble[input];
        }


        /// <summary>
        /// Computes a binomial coefficient (n choose k)
        /// </summary>
        /// <param name="n">Top number of binomial coefficient (must be greater than or equal to k).</param>
        /// <param name="k">Bottom number of binomial coefficient (must be non-negative).</param>
        /// <returns>Binomial coefficient of numbers n and k.</returns>
        public static long BinomialCoefficient(int n, int k)
        {
            if (k < 0)
                throw new ArgumentOutOfRangeException("k", k, "BinomialCoefficient can be computed for non-negative k only.");
            if (n < k)
                throw new ArgumentOutOfRangeException("n", n, "BinomialCoefficient must have n greater than or equal to k.");

            if (k > n - k)
                k = n - k;

            if (k == 0)
                return 1;
            if (k == 1)
                return n;

            // let's cut memory requirements to ~400kB max... 
            // (4337 6) = 9210884681005865704
            if (n > 4337)
            {
                if (k >= 6)
                    throw new OverflowException("Arithmetic operation resulted in an overflow.");

                // try to compute it using multiplying and GCD
                long numerator = 1;
                int denominator = 1;
                for (int x = k; x > 1; x--)
                {
                    denominator *= x;
                }
                for (int x = n; x > n - k; x--)
                {
                    int d = GreatestCommonDivisorInternal(denominator, x);
                    Debug.Assert(x % d == 0, "Something's wrong with this algorithm.");
                    Debug.Assert(denominator % d == 0, "Something's wrong with this algorithm.");
                    numerator *= (x / d);
                    denominator /= d;
                }
                Debug.Assert(denominator == 1, "Something's wrong with this algorithm.");
                return numerator;
            }

            // get the actual array indexes
            n--;
            k--;

            lock (typeof(Combinatorics))
            {
            
                // resize the array in the first dimension (k) if necessary
                if (k >= _pascalTriangle.GetLength(0))
                {
                    // get new size (power of 2)
                    int newK = _pascalTriangle.GetLength(0);
                    while (newK <= k)
                        newK *= 2;
                
                    // allocate new arrays
                    long [][] newTriangle = new long[newK][];
                    int [] newTriangleFilled = new int[newK];
                
                    // copy existing elements
                    Array.Copy(_pascalTriangle, newTriangle, _pascalTriangle.GetLength(0));
                    Array.Copy(_pascalTriangleFilled, newTriangleFilled, _pascalTriangle.GetLength(0));
                
                    // initialize new elements
                    for (int i = _pascalTriangle.GetLength(0); i < newK; i++)
                    {
                        newTriangleFilled[i] = -1;
                    }

                    // replace the old triangle
                    _pascalTriangle = newTriangle;
                    _pascalTriangleFilled = newTriangleFilled;
                }

                // fill the array if necessary
                int indexInRow = n - k - 1;
                if (indexInRow >= _pascalTriangleFilled[k])
                {
                    int newN = GetPowerOfTwo(indexInRow + 1);

                    for (int i = 0; i <= k; i++)
                    {
                        // resize the row if necessary
                        if (_pascalTriangleFilled[i] == -1)
                        {
                            // the row is not allocated yet
                            _pascalTriangle[i] = new long[newN];
                            _pascalTriangle[i][0] = i + 2;
                            _pascalTriangleFilled[i] = 1;
                        }
                        else if (_pascalTriangle[i].GetLength(0) < newN)
                        {
                            // the row is too short at the moment
                            long [] newRow = new long[newN];
                            Array.Copy(_pascalTriangle[i], newRow, _pascalTriangleFilled[i] + 1);
                            _pascalTriangle[i] = newRow;
                        }

                        // fill the row if necessary
                        if (indexInRow >= _pascalTriangleFilled[i])
                        {
                            if (i == 0)
                            {
                                for (int j = _pascalTriangleFilled[i]; j <= indexInRow; j++)
                                {
                                    _pascalTriangle[0][j] = j + 2;
                                }
                            }
                            else
                            {
                                for (int j = _pascalTriangleFilled[i]; j <= indexInRow; j++)
                                {
                                    _pascalTriangle[i][j] = _pascalTriangle[i - 1][j] + _pascalTriangle[i][j - 1];
                                }
                            }
                            _pascalTriangleFilled[i] = indexInRow;
                        }
                    }
                }

                // return the element
                return _pascalTriangle[k][indexInRow];
            }
        }

        
        /// <summary>
        /// Computes a greatest common divisor (GCD) using Euclidean algorithm.
        /// </summary>
        /// <param name="a">First taskDescription number.</param>
        /// <param name="b">Second taskDescription number.</param>
        /// <returns>Greatest common divisor (GCD) of a and b.</returns>
        public static long GreatestCommonDivisor(long a, long b)
        {
            if (a < 1)
                throw new ArgumentOutOfRangeException("a", a, "Input argument a must be at least 1.");
            if (b < 1)
                throw new ArgumentOutOfRangeException("b", b, "Input argument b must be at least 1.");

            return GreatestCommonDivisorInternal(a, b);
        }


        private static long GreatestCommonDivisorInternal(long a, long b)
        {
            Debug.Assert(a > 0);
            Debug.Assert(b > 0);

            while (b > 0)
            {
                long t = b;
                b = a % b;
                a = t;
            }

            return a;
        }


        /// <summary>
        /// Computes a greatest common divisor (GCD) using Euclidean algorithm.
        /// </summary>
        /// <param name="a">First taskDescription number.</param>
        /// <param name="b">Second taskDescription number.</param>
        /// <returns>Greatest common divisor (GCD) of a and b.</returns>
        public static int GreatestCommonDivisor(int a, int b)
        {
            if (a < 1)
                throw new ArgumentOutOfRangeException("a", a, "Input argument a must be at least 1.");
            if (b < 1)
                throw new ArgumentOutOfRangeException("b", b, "Input argument b must be at least 1.");

            return GreatestCommonDivisorInternal(a, b);
        }


        private static int GreatestCommonDivisorInternal(int a, int b)
        {
            Debug.Assert(a > 0);
            Debug.Assert(b > 0);

            while (b > 0)
            {
                int t = b;
                b = a % b;
                a = t;
            }

            return a;
        }



        /// <summary>
        /// Computes the smallest power of 2 greater than or equal to the given integer.
        /// </summary>
        /// <param name="value">Lower bound for the requested power of 2.</param>
        /// <returns>The smallest power of 2 greater than or equal to the given integer.</returns>
        public static int GetPowerOfTwo(int value)
        {
            if (value < 1)
                throw new ArgumentOutOfRangeException("value", value, "Value must be at least 1.");

            int power = 1;
            while (power < value)
                power *= 2;
            return power;
        }


        /// <summary>
        /// Computes a sum of the sequence of numbers between start and end (inclusive).
        /// </summary>
        /// <param name="start">The first number of the sequence.</param>
        /// <param name="end">The last number of the sequence.</param>
        /// <returns>Sum of the sequence.</returns>
        /// <example>
        /// <para>SequenceSum(3, 6) returns 18 ... (3 + 4 + 5 + 6)</para>
        /// <para>SequenceSum(1, 10) returns 55 ... (1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10)</para>
        /// <para>SequenceSum(4, -2) returns 7 ... (4 + 3 + 2 + 1 + 0 + (-1) + (-2))</para>
        /// <para>SequenceSum(-6, -3) returns -18 ... ((-6) + (-5) + (-4) + (-3))</para>
        /// </example>
        public static int SequenceSum(int start, int end)
        {
            return ((start + end) * (Math.Abs(start - end) + 1)) / 2;
        }


        /// <summary>
        /// Computes a sum of the sequence of numbers between start and end (inclusive).
        /// </summary>
        /// <param name="start">The first number of the sequence.</param>
        /// <param name="end">The last number of the sequence.</param>
        /// <returns>Sum of the sequence.</returns>
        /// <example>
        /// <para>SequenceSum(3, 6) returns 18 ... (3 + 4 + 5 + 6)</para>
        /// <para>SequenceSum(1, 10) returns 55 ... (1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10)</para>
        /// <para>SequenceSum(4, -2) returns 7 ... (4 + 3 + 2 + 1 + 0 + (-1) + (-2))</para>
        /// <para>SequenceSum(-6, -3) returns -18 ... ((-6) + (-5) + (-4) + (-3))</para>
        /// </example>
        public static long SequenceSum(long start, long end)
        {
            return ((start + end) * (Math.Abs(start - end) + 1)) / 2;
        }



        /// <summary>
        /// Computes a natural logarightm of gamma function.
        /// </summary>
        /// <param name="value">Input value.</param>
        /// <returns>A natural logarithm of gamma function.</returns>
        /// <remarks>
        /// <para>For definition of a gamma function see 
        /// <a href="http://en.wikipedia.org/wiki/Gamma_function">http://en.wikipedia.org/wiki/Gamma_function</a>.</para>
        /// <para>This method uses a numeric approximation published in the chapter 6.1 of the book
        /// "Numerical recipes in C: The Art of Scientific Computing", ISBN 0-521-43108-5, 
        /// (C) 1988-1992 by Cambridge University Press and (C) 1988-1992 by Numerical Recipes Software.
        /// It is also available online at the address 
        /// <a href="http://www.library.cornell.edu/nr/bookcpdf.html">http://www.library.cornell.edu/nr/bookcpdf.html</a>.</para>
        /// </remarks>
        public static double LogGamma(double value)
        {
            double x, y, z = 1.000000000190015;

            x = value + 5.5;
            x -= ((value + 0.5) * Math.Log(x));

            y = value;
            for(int j=0; j<=5; j++)
                z += (_gammaHelper[j] / ++y);

            return -x + Math.Log(2.50662827463100005 * z / value);
        }


        // magic constants for gamma function
        private static double[] _gammaHelper = new double[] { 76.18009172947146, -86.50535032941677, 24.01409824083091, -1.231739572450155, 0.1208650973866179e-2, -0.5395239384953e-5 };


        /// <summary>
        /// Generate a subset by its index.
        /// </summary>
        /// <param name="wholeSet">The whole set.</param>
        /// <param name="subsetSize">Required subset size. Must be between 0 and the size of the whole set.</param>
        /// <param name="subsetIndex">Required subset index. Must be between 0 and the total number of subsets with the specified <i>subsetSize</i> minus one.</param>
        /// <returns>A subset specified by its index.</returns>
        /// <exception cref="ArgumentNullException">The reference to the whole set cannot be null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <list type="bullet">
        /// <item>The size of a subset must be non-negative.</item>
        /// <item>The size of a subset must be less than or equal to size of the whole set.</item>
        /// <item>The index of a subset must be non-negative.</item>
        /// <item>The index of a subset must be less than the total number of subsets with the specified size.</item>
        /// </list>
        /// </exception>
        /// <remarks>
        /// <para>This method can accept an empty set, as well as the sumOfRowMax subset size can be zero.</para>
        /// </remarks>
        public static ArrayList GetSubsetByIndex(ArrayList wholeSet, int subsetSize, long subsetIndex)
        {
            if (wholeSet == null)
                throw new ArgumentNullException("wholeSet", "The reference to the whole set cannot be null.");
            if (subsetSize < 0)
                throw new ArgumentOutOfRangeException("subsetSize", subsetSize, "The size of a subset must be non-negative.");
            if (subsetSize > wholeSet.Count)
                throw new ArgumentOutOfRangeException("subsetSize", subsetSize, "The size of a subset must be less than or equal to size of the whole set.");
            if (subsetIndex < 0)
                throw new ArgumentOutOfRangeException("subsetIndex", subsetIndex, "The index of a subset must be non-negative.");
            if (subsetIndex >= Combinatorics.BinomialCoefficient(wholeSet.Count, subsetSize))
                throw new ArgumentOutOfRangeException("subsetIndex", subsetIndex, "The index of a subset must be less than the total number of subsets with the specified size.");

            ArrayList result = new ArrayList(subsetSize);
            int currentIndex = 0;
            while (subsetSize > 0)
            {
                for ( ; ; )
                {
                    long nextSubsets = Combinatorics.BinomialCoefficient(wholeSet.Count - currentIndex - 1, subsetSize - 1);
                    if (subsetIndex >= nextSubsets)
                    {
                        currentIndex++;
                        subsetIndex -= nextSubsets;
                    }
                    else
                    {
                        break;
                    }
                }
                result.Add(wholeSet[currentIndex]);
                subsetSize--;
                currentIndex++;
            }

            return result;
        }


        
        /// <summary>
        /// Generate a cyclic interval by its index.
        /// </summary>
        /// <param name="wholeSet">The whole set.</param>
        /// <param name="cyclicIntervalSize">Required cyclic interval size. Must be between 1 and the size of the whole set.</param>
        /// <param name="cyclicIntervalIndex">Required cyclic interval index. Must be between 0 and the number of items in the whole set minus one.</param>
        /// <returns>A cyclic interval specified by its index.</returns>
        /// <exception cref="ArgumentNullException">The reference to the whole set cannot be null.</exception>
        /// <exception cref="ArgumentException">The whole set cannot be empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <list type="bullet">
        /// <item>The size of a cyclic interval must be at least 1.</item>
        /// <item>The size of a cyclic interval must be less than or equal to size of the whole set.</item>
        /// <item>The index of a cyclic interval must be non-negative.</item>
        /// <item>The index of a cyclic interval must be less than the number of items in the whole set.</item>
        /// </list>
        /// </exception>
        /// <remarks>
        /// <para>This method does not accept empty whole set. The requested interval size must be at least 1.</para>
        /// </remarks>
        public static ArrayList GetCyclicIntervalByIndex(ArrayList wholeSet, int cyclicIntervalSize, int cyclicIntervalIndex)
        {
            if (wholeSet == null)
                throw new ArgumentNullException("wholeSet", "The reference to the whole set cannot be null.");
            if (wholeSet.Count < 1)
                throw new ArgumentException("The whole set cannot be empty.", "wholeSet");
            if (cyclicIntervalSize < 1)
                throw new ArgumentOutOfRangeException("cyclicIntervalSize", cyclicIntervalSize, "The size of a cyclic interval must be at least 1.");
            if (cyclicIntervalSize >= wholeSet.Count)
                throw new ArgumentOutOfRangeException("cyclicIntervalSize", cyclicIntervalSize, "The size of a cyclic interval must be less than or equal to size of the whole set.");
            if (cyclicIntervalIndex < 0)
                throw new ArgumentOutOfRangeException("cyclicIntervalIndex", cyclicIntervalIndex, "The index of a cyclic interval must be non-negative.");
            if (cyclicIntervalIndex >= wholeSet.Count)
                throw new ArgumentOutOfRangeException("cyclicIntervalIndex", cyclicIntervalIndex, "The index of a cyclic interval must be less than the number of items in the whole set.");

            ArrayList result = new ArrayList(cyclicIntervalSize);
            do
            {
                result.Add(wholeSet[cyclicIntervalIndex]);
                cyclicIntervalIndex = (cyclicIntervalIndex + 1) % wholeSet.Count;
                cyclicIntervalSize--;
            } while (cyclicIntervalSize > 0);
            
            return result;
        }

    
        /// <summary>
        /// Generate an interval by its index.
        /// </summary>
        /// <param name="wholeSet">The whole set.</param>
        /// <param name="intervalSize">Required interval size. Must be between 1 and the size of the whole set.</param>
        /// <param name="intervalIndex">Required interval index. Must be between 0 and the total number of intervals of the specified size minus one.</param>
        /// <returns>An interval specified by its index.</returns>
        /// <exception cref="ArgumentNullException">The reference to the whole set cannot be null.</exception>
        /// <exception cref="ArgumentException">The whole set cannot be empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <list type="bullet">
        /// <item>The size of an interval must be at least 1.</item>
        /// <item>The size of an interval must be less than or equal to size of the whole set.</item>
        /// <item>The index of an interval must be non-negative.</item>
        /// <item>The index of an interval must be less than the total number of intervals of the specified size minus one.</item>
        /// </list>
        /// </exception>
        /// <remarks>
        /// <para>This method does not accept empty whole set. The requested interval size must be at least 1.</para>
        /// <para>This method is intended for creating cuts (both left and right) as well. There is no special method for that purpose.</para>
        /// <list type="bullet">
        /// <item>For left cuts, use 0 as an intervalIndex.</item>
        /// <item>For right cuts, use (wholeSet.Count - intervalSize) as an intervalIndex.</item>
        /// </list>
        /// </remarks>
        public static ArrayList GetIntervalByIndex(ArrayList wholeSet, int intervalSize, int intervalIndex)
        {
            if (wholeSet == null)
                throw new ArgumentNullException("wholeSet", "The reference to the whole set cannot be null.");
            if (wholeSet.Count < 1)
                throw new ArgumentException("The whole set cannot be empty.", "wholeSet");
            if (intervalSize < 1)
                throw new ArgumentOutOfRangeException("intervalSize", intervalSize, "The size of an interval must be at least 1.");
            if (intervalSize >= wholeSet.Count)
                throw new ArgumentOutOfRangeException("intervalSize", intervalSize, "The size of an interval must be less than or equal to size of the whole set.");
            if (intervalIndex < 0)
                throw new ArgumentOutOfRangeException("intervalIndex", intervalIndex, "The index of an interval must be non-negative.");
            if (intervalIndex >= wholeSet.Count - intervalSize + 1)
                throw new ArgumentOutOfRangeException("intervalIndex", intervalIndex, "The index of an interval must be less than the total number of intervals of the specified size minus one.");

            ArrayList result = new ArrayList(intervalSize);
            do
            {
                result.Add(wholeSet[intervalIndex]);
                intervalIndex++;
                intervalSize--;
            } while (intervalSize > 0);
            
            return result;
        }


        /// <summary>
        /// Delegate for a float-type function for binary search solution of equation f(x) = 0.
        /// </summary>
        /// <remarks>
        /// <para>The solution is supposed to be exactly one.</para>
        /// <para>The left start point and the right start point must have different signs (i.e. either f(left) &lt; 0 and f(right) &gt; 0, or f(left) &gt; 0 and f(right) &lt; 0).</para>
        /// </remarks>
        public delegate float ExaminedFloatFunction(float point, params object[] hiddenParams);

        
        /// <summary>
        /// Delegate for a double-type function for binary search solution of equation f(x) = 0.
        /// </summary>
        /// <remarks>
        /// <para>The solution is supposed to be exactly one.</para>
        /// <para>The left start point and the right start point must have different signs (i.e. either f(left) &lt; 0 and f(right) &gt; 0, or f(left) &gt; 0 and f(right) &lt; 0).</para>
        /// </remarks>
        public delegate double ExaminedDoubleFunction(double point, params object[] hiddenParams);

        
        /// <summary>
        /// Binary search algorithm to search for solution of f(x) = 0.
        /// </summary>
        /// <param name="left">Left start point.</param>
        /// <param name="right">Right start point.</param>
        /// <param name="function">Delegate of the function f.</param>
        /// <param name="hiddenParams">Parameters to be passed to the examinated function f.</param>
        /// <returns>The point between left and right start point, where f(x) = 0.</returns>
        public static float BinarySearch(float left, float right, ExaminedFloatFunction function, params object[] hiddenParams)
        {
            // check arguments
            if (left >= right)
                throw new ArgumentException("The left point must be less than the right point.");
            
            // verify the left and right points
            float leftValue = function(left, hiddenParams);
            float rightValue = function(right, hiddenParams);
            bool ascendingFunction = (rightValue > leftValue);
            if (Math.Sign(leftValue) == Math.Sign(rightValue))
                throw new ArgumentException("The function must have different signs at the left and right points.");

            // it is ok to start now...
            return BinarySearchInternal(left, right, function, ascendingFunction, hiddenParams);
        }


        /// <summary>
        /// Binary search algorithm to search for solution of f(x) = 0.
        /// </summary>
        /// <param name="left">Left start point.</param>
        /// <param name="right">Right start point.</param>
        /// <param name="function">Delegate of the function f.</param>
        /// <param name="hiddenParams">Parameters to be passed to the examinated function f.</param>
        /// <returns>The point between left and right start point, where f(x) = 0.</returns>
        public static double BinarySearch(double left, double right, ExaminedDoubleFunction function, params object[] hiddenParams)
        {
            // check arguments
            if (left >= right)
                throw new ArgumentException("The left point must be less than the right point.");
            
            // verify the left and right points
            double leftValue = function(left, hiddenParams);
            double rightValue = function(right, hiddenParams);
            bool ascendingFunction = (rightValue > leftValue);
            if (Math.Sign(leftValue) == Math.Sign(rightValue))
                throw new ArgumentException("The function must have different signs at the left and right points.");

            // it is ok to start now...
            return BinarySearchInternal(left, right, function, ascendingFunction, hiddenParams);
        }


        private static float BinarySearchInternal(float left, float right, ExaminedFloatFunction function, bool ascendingFunction, params object[] hiddenParams)
        {
            for ( ; ; )
            {
                float middlePoint = (left + right) / 2.0f;
                if ((middlePoint <= left) || (middlePoint >= right))
                    return middlePoint;

                float middleValue = function(middlePoint, hiddenParams);
                if (ascendingFunction)
                {
                    if (Math.Sign(middleValue) < 0)
                        left = middlePoint;
                    else
                        right = middlePoint;
                }
                else
                {
                    if (Math.Sign(middleValue) > 0)
                        left = middlePoint;
                    else
                        right = middlePoint;
                }
            }
        }


        private static double BinarySearchInternal(double left, double right, ExaminedDoubleFunction function, bool ascendingFunction, params object[] hiddenParams)
        {
            for ( ; ; )
            {
                double middlePoint = (left + right) / 2.0f;
                if ((middlePoint <= left) || (middlePoint >= right))
                    return middlePoint;

                double middleValue = function(middlePoint, hiddenParams);
                if (ascendingFunction)
                {
                    if (Math.Sign(middleValue) < 0)
                        left = middlePoint;
                    else
                        right = middlePoint;
                }
                else
                {
                    if (Math.Sign(middleValue) > 0)
                        left = middlePoint;
                    else
                        right = middlePoint;
                }
            }
        }


        /// <summary>
        /// Computes an incomplete gamma function (regularized).
        /// </summary>
        /// <param name="a">Parameter a.</param>
        /// <param name="x">Parameter x.</param>
        /// <returns>A value of incomplete gamma function in the point (a, x).</returns>
        /// <exception cref="ArgumentOutOfRangeException">Parameter a must be greater than 0.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Parameter x must be greater than or equal to 0.</exception>
        /// <remarks>
        /// <para>For defition of an incomplete gama function (regularized) see
        /// <a href="http://en.wikipedia.org/wiki/Incomplete_Gamma_function">http://en.wikipedia.org/wiki/Incomplete_Gamma_function</a>.</para>
        /// <para>This method uses a numeric computation published in the chapter 6.2 of the book
        /// "Numerical recipes in C: The Art of Scientific Computing", ISBN 0-521-43108-5,
        /// (C) 1988-1992 by Cambridge University Press and (C) 1988-1992 by Numerical Recipes Software.
        /// It is also available online at the address
        /// <a href="http://www.library.cornell.edu/nr/bookcpdf.html">http://www.library.cornell.edu/nr/bookcpdf.html</a>.</para>
        /// </remarks>
        public static double IncompleteGamma(double a, double x)
        {
            if (a <= 0.0)
                throw new ArgumentOutOfRangeException("a", a, "Parameter a must be greater than 0.");
            if (x < 0.0)
                throw new ArgumentOutOfRangeException("x", x, "Parameter x must be greater than or equal to 0.");

            // choose faster convergence strategy
            if (x < (a + 1.0)) 
                return IncompleteGammaInternal1(a, x);
            else 
                return 1.0 - IncompleteGammaInternal2(a, x);
        }


        private const double relativePrecision = 1.0e-12;
        private const double smallPositive = 1.0e-100;
        

        private static double IncompleteGammaInternal1(double a, double x)
        {
            if (x <= 0.0) 
                return 0.0;

            double ap = a;
            double sum = 1.0 / a;
            double del = sum;
            for (int n = 1; n <= 1000; n++) 
            {
                ap += 1.0;
                del *= x / ap;
                sum += del;
                if (Math.Abs(del) < Math.Abs(sum) * relativePrecision)
                    break;
            }
            return sum * Math.Exp(-x + a * Math.Log(x) - LogGamma(a)); 
        }


        private static double IncompleteGammaInternal2(double a, double x)
        {
            double an, b, c, d, del, h;
            b = x + 1.0 - a;
            c = 1.0 / smallPositive;
            d= 1.0 / b;
            h = d;
            for (int i=1; i <= 1000; i++)
            {
                an = -i * (i - a);
                b += 2.0;
                d = an * d + b;
                if (Math.Abs(d) < smallPositive)
                    d = smallPositive;
                c = b + an / c;
                if (Math.Abs(c) < smallPositive)
                    c = smallPositive;
                d = 1.0 / d;
                del = d * c;
                h *= del;
                if (Math.Abs(del - 1.0) < relativePrecision) break;
            }
            
            return Math.Exp(-x + a * Math.Log(x) - LogGamma(a)) * h;
        }


        /// <summary>
        /// Computes the value of chi-square probability density function with k degrees of freedom.
        /// </summary>
        /// <param name="x">Point of function.</param>
        /// <param name="k">Degrees of freedom.</param>
        /// <returns>The value of chi-square probability density function with k degrees of freedom.</returns>
        /// <remarks>
        /// <para>It is computed using gamma function, see
        /// <a href="http://en.wikipedia.org/wiki/Chi_square_distribution">http://en.wikipedia.org/wiki/Chi_square_distribution</a>.</para>
        /// </remarks>
        public static double ChiSquarePdf(double x, int k)
        {
            if (k <= 0)
                throw new ArgumentOutOfRangeException("k", k, "The parameter k (degrees of freedom) must be positive.");
            if (x <= 0.0)
                return 0.0;

            double k2 = ((double) k) / 2.0;
            
            // user precise values for small k instead of calling LogGamma
            double gamma;
            if (k <= 8)
                gamma = _chiSquarePdfHelper[k - 1];
            else
                gamma = Math.Exp(LogGamma(k2));
            return (Math.Pow(0.5, k2) * Math.Pow(x, k2 - 1.0) * Math.Exp(-x / 2.0) / gamma);
        }


        // values of gamma function for small values 0.5, 1.0, 1.5, 2.0, 2.5, 3.0, 3.5, 4.0
        // for use in chi-square probability density function
        private static double[] _chiSquarePdfHelper = new double[]
        {
            1.77245385090552,  // Math.Sqrt(Math.PI)
            1.0,
            0.886226925452758,  // Math.Sqrt(Math.PI) / 2.0
            1.0,
            1.32934038817914,  // 3.0 * Math.Sqrt(Math.PI) / 4.0
            2.0,
            3.32335097044784,  // 15.0 * Math.Sqrt(Math.PI) / 8.0
            6.0 
        };

        
        /// <summary>
        /// Computes the value of chi-square cummulative distribution function with k degrees of freedom.
        /// </summary>
        /// <param name="x">The point of function.</param>
        /// <param name="k">Degrees of freedom.</param>
        /// <returns>The value of chi-square cummulative distribution function with k degrees of freedom.</returns>
        /// <remarks>
        /// <para>It is computed using incomplete gamma function, see
        /// <a href="http://en.wikipedia.org/wiki/Chi_square_distribution">http://en.wikipedia.org/wiki/Chi_square_distribution</a>.</para>
        /// </remarks>
        public static double ChiSquareCdf(double x, int k)
        {
            if (k <= 0)
                throw new ArgumentOutOfRangeException("k", k, "The parameter k (degrees of freedom) must be positive.");
            if (x <= 0.0)
                return 0.0;

            return IncompleteGamma(((double) k) / 2.0, x / 2.0);
        }


        /// <summary>
        /// Inverse function to chi-square cummulative distribution function.
        /// </summary>
        /// <param name="alpha">The percentile in question.</param>
        /// <param name="k">Degrees of freedom.</param>
        /// <returns>The point where </returns>
        public static double ChiSquareCdfInv(double alpha, int k)
        {
            if ((alpha <= 0.0) || (alpha > 0.5))
                throw new ArgumentOutOfRangeException("alpha", alpha, "The parameter alpha must be greater than 0.0 and less then or equal to 0.5.");
            if (k <= 0)
                throw new ArgumentOutOfRangeException("k", k, "The parameter k (degrees of freedom) must be positive.");

            return BinarySearch(0.0, 1.0e20, new ExaminedDoubleFunction(ChiSquareCdfInvHelper), 1.0 - alpha, k);
        }


        // wrapper for ChiSquareCdf to be a "single-variable" function for binary search
        private static double ChiSquareCdfInvHelper(double x, params object[] hiddenParams)
        {
            double alpha = (double) hiddenParams[0];
            int k = (int) hiddenParams[1];
            return ChiSquareCdf(x, k) - alpha;
        }



    }
}
