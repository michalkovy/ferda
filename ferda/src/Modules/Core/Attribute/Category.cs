using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Ferda.Guha.Attribute
{
    /// <summary>
    /// Represents the category of the interaval. The category can be
    /// quite coplex, it can be composited of collection of intervals and
    /// collection of values (enumeration).
    /// </summary>
    /// <typeparam name="T">Domain type</typeparam>
    public class Category<T> : IEquatable<Category<T>>, IComparable<Category<T>>
        where T : IComparable
    {
        #region Fields

        private Attribute<T> _attribute;
        private Intervals<T> _intervals;
        private Enumeration<T> _enumeration;
        private int _ordNumber;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the parent attribute.
        /// </summary>
        /// <value>The attribute.</value>
        public Attribute<T> Attribute
        {
            get { return _attribute; }
        }

        /// <summary>
        /// Gets the collection of intervals.
        /// </summary>
        /// <value>The intervals.</value>
        public Intervals<T> Intervals
        {
            get { return _intervals; }
        }

        /// <summary>
        /// Gets the enumeration of values.
        /// </summary>
        /// <value>The enumeration.</value>
        public Enumeration<T> Enumeration
        {
            get { return _enumeration; }
        }

        /// <summary>
        /// Gets the name of the category.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return _attribute.GetCategoryName(this); }
        }

        /// <summary>
        /// Gets the default label for the category i.e. category.ToString().
        /// </summary>
        /// <value>The default label.</value>
        public string DefaultLabel
        {
            get { return ToString(); }
        }

        /// <summary>
        /// Gets or sets the ord number of the category.
        /// </summary>
        /// <value>The ord number.</value>
        /// <remarks>
        /// Categories can be sorted by the OrdNubmers. Please notice
        /// that there is no checking if two categories has the same OrdNumber
        /// even the OrdNumbers does not have to corresponds real sort. For 
        /// reals sort you can use <see cref="M:Ferda.Guha.Attribute.Attribute`1.SetDefaultOrds"/> method. 
        /// </remarks>
        public int OrdNumber
        {
            get { return _ordNumber; }
            set { _ordNumber = value; }
        }

        #endregion

        #region Constructors

        private Category()
        {
            _enumeration = new Enumeration<T>(this);
            _intervals = new Intervals<T>(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Ferda.Guha.Attribute.Category`1"/> class.
        /// </summary>
        /// <param name="attributeClass">The parent attribute class.</param>
        public Category(Attribute<T> attributeClass)
            : this()
        {
            _attribute = attributeClass;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Ferda.Guha.Attribute.Category`1"/> class.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="attributeClass">The parent attribute class.</param>
        public Category(CategorySerializable<T> category, Attribute<T> attributeClass)
            : this(attributeClass)
        {
            // process intervals
            foreach (IntervalSerializable<T> interval in category.Intervals)
            {
                _intervals.Add(
                    interval.LeftValue,
                    interval.LeftBoundary,
                    interval.RightValue,
                    interval.RightBoundary,
                    false
                    );
            }

            // process enumeration
            foreach (T val in category.Enumeration)
            {
                _enumeration.Add(
                    val,
                    false);
            }

            OrdNumber = category.OrdNumber;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Ferda.Guha.Attribute.Category`1"/> class.
        /// </summary>
        /// <param name="intervals">The intervals.</param>
        /// <param name="enumeration">The enumeration.</param>
        /// <param name="attributeClass">The parent attribute class.</param>
        public Category(Interval<T>[] intervals, T[] enumeration, Attribute<T> attributeClass)
            : this(attributeClass)
        {
            if (intervals != null)
                foreach (Interval<T> interval in intervals)
                {
                    _intervals.Add(
                        interval.LeftValue,
                        interval.LeftBoundary,
                        interval.RightValue,
                        interval.RightBoundary,
                        false
                        );
                }
            if (enumeration != null)
                for (int i = 0; i < enumeration.Length; i++)
                {
                    _enumeration.Add(enumeration[i], false);
                }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Ferda.Guha.Attribute.Category`1"/> class.
        /// </summary>
        /// <param name="intervals">The intervals.</param>
        /// <param name="enumeration">The enumeration.</param>
        /// <param name="attributeClass">The parent attribute class.</param>
        public Category(List<Interval<T>> intervals, List<T> enumeration, Attribute<T> attributeClass)
            : this(attributeClass)
        {
            if (intervals != null)
                foreach (Interval<T> interval in intervals)
                {
                    _intervals.Add(
                        interval.LeftValue,
                        interval.LeftBoundary,
                        interval.RightValue,
                        interval.RightBoundary,
                        false
                        );
                }
            if (enumeration != null)
                for (int i = 0; i < enumeration.Count; i++)
                {
                    _enumeration.Add(enumeration[i], false);
                }
        }

        #endregion

        internal CategorySerializable<T> Export(string categoryName)
        {
            return new CategorySerializable<T>(
                    categoryName,
                    OrdNumber,
                    _enumeration.Export(),
                    _intervals.Export()
                    );
        }

        /// <summary>
        /// Sorts the enumerations and intervals.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="T:Ferda.Guha.Attribute.NotComparableCollisionException">
        /// Thrown if intervals and enumertions can not be sorted
        /// i.e. they are not disjunctive ... it can appear when
        /// <c>_attribute.LazyReduction</c> is <c>true</c>.
        /// </exception>
        internal ArrayList SortEnumerationsAndIntervals()
        {
            _intervals.Sort();
            _enumeration.Sort();

            ArrayList result = new ArrayList();

            int iIndex = 0;
            int eIndex = 0;
            int compareResult;
            while (iIndex < _intervals.Count && eIndex < _enumeration.Count)
            {
                compareResult = _attribute.Compare(_intervals[iIndex], _enumeration[eIndex]);
                if (compareResult < 0)
                {
                    result.Add(_intervals[iIndex]);
                    iIndex++;
                }
                else if (compareResult > 0)
                {
                    result.Add(_enumeration[eIndex]);
                    eIndex++;
                }
                else //interval and enumItem are not disjunctive
                {
                    throw new NotComparableCollisionException("Critical error: This shold never happend.");
                }
            }
            while (iIndex < _intervals.Count)
            {
                result.Add(_intervals[iIndex]);
                iIndex++;
            }
            while (eIndex < _enumeration.Count)
            {
                result.Add(_enumeration[eIndex]);
                eIndex++;
            }
            return result;
        }

        /// <summary>
        /// Sorts this instance. In DEBUG (and not lazy reduction) mode, the disjunctivity is also checked.
        /// </summary>
        public void Sort()
        {
            _intervals.Sort();
            _enumeration.Sort();

#if DEBUG
            int iIndex = 0;
            int eIndex = 0;
            int compareResult;
            while (iIndex < _intervals.Count && eIndex < _enumeration.Count)
            {
                compareResult = _attribute.Compare(_intervals[iIndex], _enumeration[eIndex]);
                if (compareResult < 0)
                {
                    iIndex++;
                }
                else if (compareResult > 0)
                {
                    eIndex++;
                }
                else //interval and enumItem are not disjunctive
                    throw new Exception("This should never happend.");
            }
#endif
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current category.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current category.
        /// </returns>
        public override string ToString()
        {
            Sort();
            int iIndex = 0;
            int eIndex = 0;
            int compareResult;
            StringBuilder result = new StringBuilder();
            while (iIndex < _intervals.Count || eIndex < _enumeration.Count)
            {
                if (iIndex > 0 && eIndex > 0)
                    result.Append(Common.CategoryMembersSeparator);

                if (iIndex == _intervals.Count)
                    compareResult = 1;
                else if (eIndex == _enumeration.Count)
                    compareResult = -1;
                else
                    compareResult = _attribute.Compare(_intervals[iIndex], _enumeration[eIndex]);

                if (compareResult < 0 || eIndex == _enumeration.Count)
                {
                    result.Append(_intervals[iIndex].ToString());
                    iIndex++;
                }
                else if (compareResult > 0 || iIndex == _intervals.Count)
                {
                    result.Append(_enumeration[eIndex].ToString());
                    eIndex++;
                }
                else
                {
                    Debug.Assert(false); // only if category is not reduced (deprecated)
                    throw Exceptions.AttributeCategoriesDisjunctivityError(null, null);
                    //if (!_attribute.LazyReduction)
                    //{
                    //    Debugger.Break();
                    //    throw new Exception(
                    //        "This should never happend ... only if category is not reduced, but reduction is not lazy.");
                    //}
                    //result.Append(_enumeration[eIndex].ToString());
                    //eIndex++;
                    //result.Append(Common.CategoryMembersSeparator);
                    //result.Append(_intervals[iIndex].ToString());
                    //iIndex++;
                }
            }
            if (_attribute.NullContainingCategory == Name)
            {
                if (result.Length > 0)
                    return Common.NullValue + Common.CategoryMembersSeparator + result.ToString();
                else
                    return Common.NullValue;
            }
            else
                return result.ToString();
        }

        /// <summary>
        /// Determines whether the left interval is direct (i.e. no value 
        /// can be occured between these intervals in other words both
        /// intervals share same value on the neighbour but one is open and 
        /// second is closed there) left sided 
        /// disjunctive neighbour of the right interval.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        private bool directDisjunctiveNeighbourSided(Interval<T> left, Interval<T> right)
        {
            return (
                       _attribute.Compare(left.RightValue, right.LeftValue) == 0
                       && (
                              (left.RightBoundary == BoundaryEnum.Closed && right.LeftBoundary == BoundaryEnum.Open)
                              ||
                              (left.RightBoundary == BoundaryEnum.Open && right.LeftBoundary == BoundaryEnum.Closed)
                          )
                   );
        }

        /// <summary>
        /// Determines whether the specified intervals are 
        /// direct disjunctive neighbours i.e. they can be joined
        /// to one bigger interval.
        /// </summary>
        /// <param name="x">The interval.</param>
        /// <param name="y">The interval.</param>
        private bool directDisjunctiveNeighbour(Interval<T> x, Interval<T> y)
        {
            return directDisjunctiveNeighbourSided(x, y) || directDisjunctiveNeighbourSided(y, x);
        }


        /// <summary>
        /// Determines whether the specified interval and value are
        /// direct disjunctive neighbours i.e. they can be joined
        /// to one bigger interval.
        /// </summary>
        /// <param name="x">The interval.</param>
        /// <param name="y">The value.</param>
        /// <param name="side">The side where the specified value (<c>y</c>) neighbours.</param>
        private bool directDisjunctiveNeighbour(Interval<T> x, T y, out Side side)
        {
            if (x.RightBoundary == BoundaryEnum.Open && _attribute.Compare(x.RightValue, y) == 0)
            {
                side = Side.Right;
                return true;
            }
            if (x.LeftBoundary == BoundaryEnum.Open && _attribute.Compare(x.LeftValue, y) == 0)
            {
                side = Side.Left;
                return true;
            }
            side = 0; // doesnt matter;
            return false;
        }

        // return enumValueExtendedLastInterval;
        private bool reduceEnumeration(IEnumerator<T> enumValuesEnumerator, ref T currentEnumValue,
                                       ref bool currentEnumValueIsSet, List<T> resultEnumeration, ref T updIntLV,
                                       ref BoundaryEnum updIntLB, ref T updIntRV, ref BoundaryEnum updIntRB)
        {
            bool result = false;
            Interval<T> lastInterval;
        NextEnumerationValueEvaluation:
            lastInterval = new Interval<T>(updIntLV, updIntLB, updIntRV, updIntRB, _attribute);
            if (currentEnumValueIsSet)
            {
                Side side;
                if (directDisjunctiveNeighbour(lastInterval, currentEnumValue, out side))
                {
                    result = true;
                    if (side == Side.Right)
                        updIntRB = BoundaryEnum.Closed;
                    else //if (side == Side.Left)
                        updIntLB = BoundaryEnum.Closed;
                    // this currentEnumValue will not be in resultEnumeration
                }
                else
                {
                    int compareResult = _attribute.Compare(currentEnumValue, lastInterval);
                    if (compareResult < 0)
                        // enum value is less than the "lastInterval"
                        resultEnumeration.Add(currentEnumValue);
                    else if (compareResult > 0)
                        // enum value is greater than the "lastInterval"
                        goto StopEnumerationValueEvaluation;
                    // else (compareResult == 0) 
                    //  enum value is covered by the "lastInterval"
                    //  this can come only if lazy reduce is used and axis was disabled
                    //  this currentEnumValue will not be in resultEnumeration
                    //  do nothing
                }

                // not processed if currentEnumValue is > the "lastInterval"
                if (enumValuesEnumerator.MoveNext())
                    currentEnumValue = enumValuesEnumerator.Current;
                else
                    currentEnumValueIsSet = false;
                goto NextEnumerationValueEvaluation;
            }
        StopEnumerationValueEvaluation:
            return result;
        }

        /// <summary>
        /// Reduces the specified category i.e. overlaping interavals are 
        /// joined, duplicits are removed.
        /// </summary>
        public void Reduce()
        {
            if (_intervals.Count == 0)
                return;

            bool lastAxisDisabledValue = _attribute.Axis.Disabled;
            _attribute.Axis.Disabled = true;

            // ENUMERATION
            // if enumeration 
            //  is in disjunctivity collision
            //      with interval ... the value is removed from enumertion
            //      with other enum value ... can not come ... exception should be thrown
            //  is direct neighbour of some interval ... interavl is updated, the value is removed from enumeration
            _enumeration.Sort();
            List<T> resultEnumeration = new List<T>();
            IEnumerator<T> enumValuesEnumerator = _enumeration.GetEnumerator();
            T currentEnumValue = default(T);
            bool currentEnumValueIsSet = false;
            if (enumValuesEnumerator.MoveNext())
            {
                currentEnumValue = enumValuesEnumerator.Current;
                currentEnumValueIsSet = true;
            }

            // INTERVALS
            //
            // intervals init
            _intervals.Sort(true);
            List<Interval<T>> intervals = new List<Interval<T>>();
            intervals.AddRange(_intervals);
            List<Interval<T>> resultIntervals = new List<Interval<T>>();
            Interval<T> lastInterval = null;
            T updIntLV, updIntRV;
            updIntLV = updIntRV = default(T);
            BoundaryEnum updIntLB, updIntRB;
            updIntLB = updIntRB = 0; //default .. BUNO

            // intervals and enumerations are sorted
            // proccess all intervals and enumerations by one walk-through
            foreach (Interval<T> currentInterval in intervals)
            {
            NextIntervalEvaluation:
                if (lastInterval == null) // only in first iteration
                {
                    updIntLV = currentInterval.LeftValue;
                    updIntRV = currentInterval.RightValue;
                    updIntLB = currentInterval.LeftBoundary;
                    updIntRB = currentInterval.RightBoundary;
                    lastInterval = new Interval<T>(updIntLV, updIntLB, updIntRV, updIntRB, _attribute);
                    continue;
                }

            UpdatedIntervalEvaluation:
                lastInterval = new Interval<T>(updIntLV, updIntLB, updIntRV, updIntRB, _attribute);

                if (
                    _attribute.Compare(lastInterval, currentInterval) == 0
                    // this can come only if lazy reduce is used and axis was disabled
                    || directDisjunctiveNeighbour(lastInterval, currentInterval)
                    )
                {
                    #region getting new bounds

                    // "currentInterval" will not be in resultIntervals
                    // enlarge (if needed last interval)
                    if // choose smaller left value & boundary
                        (!
                         (
                             lastInterval.LeftBoundary == BoundaryEnum.Infinity
                             ||
                             (
                                 currentInterval.LeftBoundary != BoundaryEnum.Infinity
                                 && _attribute.Compare(lastInterval.LeftValue, currentInterval.LeftValue) < 0
                             )
                         )
                        )
                    {
                        updIntLV = currentInterval.LeftValue;
                        updIntLB = currentInterval.LeftBoundary;
                    }
                    if // choose bigger right value & boundary
                        (!
                         (
                             lastInterval.RightBoundary == BoundaryEnum.Infinity
                             ||
                             (
                                 currentInterval.RightBoundary != BoundaryEnum.Infinity
                                 && _attribute.Compare(lastInterval.RightValue, currentInterval.RightValue) > 0
                             )
                         )
                        )
                    {
                        updIntRV = currentInterval.RightValue;
                        updIntRB = currentInterval.RightBoundary;
                    }

                    #endregion
                }
                else
                // last interval is smaller than current one
                // processing enumeration is needed
                {
                    if (
                        reduceEnumeration(enumValuesEnumerator, ref currentEnumValue, ref currentEnumValueIsSet,
                                          resultEnumeration, ref updIntLV, ref updIntLB, ref updIntRV, ref updIntRB)
                        )
                        // enumerattion processing may change comparation result of last and current interval
                        goto UpdatedIntervalEvaluation;

                    // interval may not be more extended (extension by intervals and enumerations finnished)
                    resultIntervals.Add(new Interval<T>(updIntLV, updIntLB, updIntRV, updIntRB, _attribute));
                    lastInterval = null;
                    goto NextIntervalEvaluation;
                }
            }

            // update last processed interval
            if (lastInterval != null)
            {
                reduceEnumeration(enumValuesEnumerator, ref currentEnumValue, ref currentEnumValueIsSet,
                                  resultEnumeration, ref updIntLV, ref updIntLB, ref updIntRV, ref updIntRB);
                resultIntervals.Add(
                    new Interval<T>(updIntLV, updIntLB, updIntRV, updIntRB, _attribute)
                    );
            }

            _intervals.Clear();
            if (resultIntervals.Count > 0)
                foreach (Interval<T> interval in resultIntervals)
                    _intervals.AddWhileReducing(interval.LeftValue, interval.LeftBoundary, interval.RightValue,
                                   interval.RightBoundary, true);

            // not processed enumeration members
            //  if _intervals.Count == 0 or members greather than all intervals
            if (currentEnumValueIsSet)
                resultEnumeration.Add(currentEnumValue);
            while (enumValuesEnumerator.MoveNext())
            {
                resultEnumeration.Add(enumValuesEnumerator.Current);
            }
            _enumeration.Clear();
            if (resultEnumeration.Count > 0)
                foreach (T enumValue in resultEnumeration)
                    _enumeration.AddWhileReducing(enumValue, true);

            _attribute.Axis.Disabled = lastAxisDisabledValue;
        }

        #region IEquatable<Category<T>> Members

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
        /// Please see <see cref="T:Ferda.Guha.Attribute.Category`1.Equals(Ferda.Guha.Attribute.Category`1)"/>.
        /// </summary>
        public override bool Equals(object obj)
        {
            Category<T> other = obj as Category<T>;
            return Equals(other);
        }

        /// <summary>
        /// Determines whether the specified category can be 
        /// ordered in front or behind the current category. If not
        /// true is returned.
        /// </summary>
        /// <param name="other">The category to compare with the current category.</param>
        /// <returns>
        /// true if categories blend together 
        /// or both categories are empty 
        /// or even they are not disjunctive; 
        /// otherwise, false.
        /// </returns>
        /// <remarks>
        /// Please note that this equality is not transitional i.e. 
        /// THIS IS NOT TRUE: (x.Equals(y) &amp;&amp; y.Equals(z)) 
        /// returns true if and only if x.Equals(z) returns true.
        /// </remarks>
        public bool Equals(Category<T> other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (other == null) return false;
            int comparationResult = _attribute.Compare(this, other);
            if (comparationResult == 0)
                return true;
            return false;
        }

        /// <summary>
        /// Operator ==.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <remarks>Please see method Equals().</remarks>
        public static bool operator ==(Category<T> x, Category<T> y)
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
        public static bool operator !=(Category<T> x, Category<T> y)
        {
            return !(x == y);
        }

        #endregion

        #region IComparable<Category<T>> Members

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// 0 if categoris overlap each other.
        /// 1 if current is on the right side (on imaginary axis) of other
        /// -1 otherwise
        /// </returns>
        public int CompareTo(Category<T> other)
        {
            return _attribute.Compare(this, other);
        }

        #endregion
    }
}