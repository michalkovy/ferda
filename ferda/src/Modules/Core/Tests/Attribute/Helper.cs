using System;
using System.Collections.Generic;
using Ferda.Guha.Attribute;

namespace Tests.Attribute
{
    public static class Helper
    {
        #region categories names

        /// <summary>
        /// "category A"
        /// </summary>
        public const string categoryAName = "category A";

        /// <summary>
        /// "category B"
        /// </summary>
        public const string categoryBName = "category B";

        /// <summary>
        /// "category C"
        /// </summary>
        public const string categoryCName = "category C";

        /// <summary>
        /// "category D"
        /// </summary>
        public const string categoryDName = "category D";

        #endregion

        public static Interval<int> Interval1(IComparer<Interval<int>> comparer)
        {
            return GetIntervalT(-10, BoundaryEnum.Closed, 10, BoundaryEnum.Open, comparer);
        }

        public static int GetT(int input)
        {
            return input;
        }

        public static Interval<int> GetIntervalT(int x, BoundaryEnum xBoundary, int y, BoundaryEnum yBoundary,
                                                 IComparer<Interval<int>> comparer)
        {
            return new Interval<int>(GetT(x), xBoundary, GetT(y), yBoundary, comparer);
        }

        public static bool ExactEqual<T>(T x, T y)
            where T : IComparable
        {
            return (x.CompareTo(y) == 0);
        }

        public static bool ExactEqual<T>(Interval<T> x, Interval<T> y)
            where T : IComparable
        {
            return (
                       x.LeftBoundary == y.LeftBoundary
                       && x.RightBoundary == y.RightBoundary
                       && (x.LeftBoundary == BoundaryEnum.Infinity || ExactEqual<T>(x.LeftValue, y.LeftValue))
                       && (x.RightBoundary == BoundaryEnum.Infinity || ExactEqual<T>(x.RightValue, y.RightValue))
                   );
        }
    }
}