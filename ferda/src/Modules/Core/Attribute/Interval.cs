using System;
using System.Collections.Generic;

namespace Ferda.Guha.Attribute
{
    /// <summary>
    /// The Interval (only for ordinal (or cardinal) data).
    /// </summary>
    /// <typeparam name="T">Domain type</typeparam>
    public class Interval<T> : IEquatable<Interval<T>>, IComparable<Interval<T>>
        where T : IComparable
    {
        #region Fields

        private T _leftValue;
        private T _rightValue;
        private BoundaryEnum _leftBoundary;
        private BoundaryEnum _rightBoundary;
        private IComparer<Interval<T>> _comparer;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the left value of the interval.
        /// </summary>
        /// <value>The left value.</value>
        public T LeftValue
        {
            get { return _leftValue; }
        }

        /// <summary>
        /// Gets the right value of the interval.
        /// </summary>
        /// <value>The right value.</value>
        public T RightValue
        {
            get { return _rightValue; }
        }

        /// <summary>
        /// Gets the left boundary of the interval.
        /// </summary>
        /// <value>The left boundary.</value>
        public BoundaryEnum LeftBoundary
        {
            get { return _leftBoundary; }
        }

        /// <summary>
        /// Gets the right boundary of the interval.
        /// </summary>
        /// <value>The right boundary.</value>
        public BoundaryEnum RightBoundary
        {
            get { return _rightBoundary; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Ferda.Guha.Attribute.Intervals`1"/> class.
        /// </summary>
        private Interval()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Ferda.Guha.Attribute.Intervals`1"/> class.
        /// </summary>
        /// <param name="leftValue">The left value.</param>
        /// <param name="leftBoundary">The left boundary.</param>
        /// <param name="rightValue">The right value.</param>
        /// <param name="rightBoundary">The right boundary.</param>
        /// <param name="comparer">The interval comparer.</param>
        public Interval(T leftValue, BoundaryEnum leftBoundary, T rightValue, BoundaryEnum rightBoundary,
                        IComparer<Interval<T>> comparer)
        {
            _leftBoundary = leftBoundary;
            _leftValue = leftValue;
            _rightBoundary = rightBoundary;
            _rightValue = rightValue;
            _comparer = comparer;
        }

        #endregion

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current interval.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current interval.
        /// </returns>
        public override string ToString()
        {
            return
                (
                    (_leftBoundary == BoundaryEnum.Infinity)
                        ? Common.BoundaryToString(Side.Left, _leftBoundary)
                        : Common.BoundaryToString(Side.Left, _leftBoundary) + _leftValue.ToString()
                )
                + Common.IntervalValuesSeparator +
                (
                    (_rightBoundary == BoundaryEnum.Infinity)
                        ? Common.BoundaryToString(Side.Right, _rightBoundary)
                        : _rightValue.ToString() + Common.BoundaryToString(Side.Right, _rightBoundary)
                );
        }

        #region IEquatable<Interval<T>> Members

        /// <summary>
        /// Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override int GetHashCode()
        {
            return 0;
        }

        /// <summary>
        /// Please see <see cref="T:Ferda.Guha.Attribute.Interval`1.Equals(Ferda.Guha.Attribute.Interval`1)"/>.
        /// </summary>
        public override bool Equals(object obj)
        {
            Interval<T> other = obj as Interval<T>;
            return Equals(other);
        }

        /// <summary>
        /// Determines whether the specified interval covers part 
        /// (even a point) of current interval.
        /// </summary>
        /// <param name="other">The interval to compare with the current interval.</param>
        /// <returns>
        /// true if the specified interval is covers part 
        /// (even a point) of current interval; otherwise, false.
        /// </returns>
        /// <remarks>
        /// Please note that this equality is not transitional i.e. 
        /// THIS IS NOT TRUE: (x.Equals(y) &amp;&amp; y.Equals(z)) 
        /// returns true if and only if x.Equals(z) returns true.
        /// </remarks>
        public bool Equals(Interval<T> other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (other == null) return false;
            if (_comparer.Compare(this, other) == 0)
                return true;
            return false;
        }

        /// <summary>
        /// Operator ==.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <remarks>Please see method Equals().</remarks>
        public static bool operator ==(Interval<T> x, Interval<T> y)
        {
            if (ReferenceEquals(x, y))
                return true;
            else if (ReferenceEquals(x, null))
                return false;
            else 
                return x.Equals(y);
        }

        /// <summary>
        /// Operator !=.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <remarks>Please see method Equals().</remarks>
        public static bool operator !=(Interval<T> x, Interval<T> y)
        {
            return !(x == y); 
        }

        #endregion

        #region IComparable<Interval<T>> Members

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// 0 if intervals overlap each other.
        /// 1 if current is on the right side (on imaginary axis) of other
        /// -1 otherwise
        /// </returns>
        public int CompareTo(Interval<T> other)
        {
            return _comparer.Compare(this, other);
        }

        #endregion
    }
}