// Intervals.cs - functionality for collections of intervals
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
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USAusing System;

using System;
using System.Collections;
using System.Collections.Generic;

namespace Ferda.Guha.Attribute
{
    /// <summary>
    /// Collection of intervals.
    /// (Intervals are allowed only for ordinal or cardinal data)
    /// </summary>
    /// <typeparam name="T">Domain type</typeparam>
    public class Intervals<T> : IEnumerable<Interval<T>> // partially IList<interval<T>>, ICollection<interval<T>>
        where T : IComparable
    {
        #region Fields

        private List<Interval<T>> _intervals = new List<Interval<T>>();
        private Category<T> _category;

        #endregion

        #region Constructors

        private Intervals()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Ferda.Guha.Attribute.Intervals`1"/> class.
        /// </summary>
        /// <param name="category">The category.</param>
        public Intervals(Category<T> category)
        {
            _category = category;
        }

        #endregion

        internal IntervalSerializable<T>[] Export()
        {
            List<IntervalSerializable<T>> result = new List<IntervalSerializable<T>>();
            foreach (Interval<T> interval in _intervals)
            {
                result.Add(
                    new IntervalSerializable<T>(
                        interval.LeftValue,
                        interval.RightValue,
                        interval.LeftBoundary,
                        interval.RightBoundary
                        )
                    );
            }
            return result.ToArray();
        }

        /// <summary>
        /// Determines if the item belongs to this set of intervals, i.e.
        /// if it is inside one of the intervals. 
        /// </summary>
        /// <param name="item">Item to be examined</param>
        /// <returns>Iff the item belongs to intervals</returns>
        public bool BelongsToIntervals(T item)
        {
            foreach (Interval<T> interval in _intervals)
            {
                if (interval.BelongsToInterval(item))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Sorts this instance. In DEBUG (and not lazy reduction) 
        /// mode, the disjunctivity is also checked.
        /// </summary>
        public void Sort()
        {
            Sort(false);
        }
        
        internal void Sort(bool nowReducing)
        {
            _intervals.Sort(_category.Attribute);

#if DEBUG
            if (nowReducing)
                return;
            Interval<T> lastInterval = null;
            foreach (Interval<T> item in _intervals)
            {
                if (lastInterval != null)
                    if (_category.Attribute.Compare(lastInterval, item) >= 0)
                        throw new ApplicationException("This should never happend.");
                lastInterval = item;
            }
#endif
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the collection of intervals.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the collection of intervals.
        /// </returns>
        public override string ToString()
        {
            return Ferda.Modules.Helpers.Common.Print.SequenceToString(_intervals, Common.CategoryMembersSeparator);
        }

        #region IList<interval<T>> Members

        /// <summary>
        /// Indexes the of the interval (<c>item</c>).
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public int IndexOf(Interval<T> item)
        {
            return _intervals.IndexOf(item);
        }

        /// <summary>
        /// Removes the interval at specified <c>index</c>.
        /// </summary>
        /// <param name="index">The index.</param>
        public void RemoveAt(int index)
        {
            _intervals.RemoveAt(index);
            _category.Attribute.Axis.NotValid(false, true);
        }

        /// <summary>
        /// Gets the <see cref="T:Ferda.Guha.Attribute.Intervals`1"/> at the specified <c>index</c>.
        /// </summary>
        public Interval<T> this[int index]
        {
            get { return _intervals[index]; }
        }

        #endregion

        #region ICollection<interval<T>> Members

        /// <summary>
        /// Determines whether the interval is degenerative.
        /// The interval is degenerative iff it is closed from 
        /// both sides and left boundary value is equal to right 
        /// boundary value. If interval is degenerative than it is 
        /// usefull to convert it to enumeration value.
        /// </summary>
        /// <param name="leftValue">The left value.</param>
        /// <param name="leftBoundary">The left boundary.</param>
        /// <param name="rightValue">The right value.</param>
        /// <param name="rightBoundary">The right boundary.</param>
        /// <returns>
        /// 	<c>true</c> if the interval is degenerative; otherwise, <c>false</c>.
        /// </returns>
        private bool isIntervalDegenerative(T leftValue, BoundaryEnum leftBoundary, T rightValue,
                                            BoundaryEnum rightBoundary)
        {
            int comparationResult = _category.Attribute.Compare(leftValue, rightValue);
            if (
                comparationResult == 0
                && leftBoundary == BoundaryEnum.Closed
                && rightBoundary == BoundaryEnum.Closed
                )
                return true;
            return false;
        }

        /// <summary>
        /// Adds the specified left value.
        /// </summary>
        /// <param name="leftValue">The left value.</param>
        /// <param name="leftBoundary">The left boundary.</param>
        /// <param name="rightValue">The right value.</param>
        /// <param name="rightBoundary">The right boundary.</param>
        /// <param name="force">if set to <c>true</c> the interval is
        /// updated even if it is in collision with another category
        /// (intersection will be removed from the another category).</param>
        public void Add(T leftValue, BoundaryEnum leftBoundary, T rightValue, BoundaryEnum rightBoundary, bool force)
        {
            add(-1, leftValue, leftBoundary, rightValue, rightBoundary, force, false);
        }

        internal void AddWhileReducing(T leftValue, BoundaryEnum leftBoundary, T rightValue, BoundaryEnum rightBoundary, bool force)
        {
            add(-1, leftValue, leftBoundary, rightValue, rightBoundary, force, true);
        }

        /// <summary>
        /// Adds the interval at specified index.
        /// </summary>
        /// <param name="index">The index (if index is less than zero, interaval is simply added to the end of the collection).</param>
        /// <param name="interval">The interval.</param>
        protected void add(int index, Interval<T> interval)
        {
            if (index >= 0)
                _intervals.Insert(index, interval);
            else
                _intervals.Add(interval);
        }

        /// <summary>
        /// Tests the interval shape. <see cref="T:System.ArgumentException"/>
        /// is thrown iff left side of the interval is not less than right side.
        /// </summary>
        /// <param name="leftValue">The left value.</param>
        /// <param name="leftBoundary">The left boundary.</param>
        /// <param name="rightValue">The right value.</param>
        /// <param name="rightBoundary">The right boundary.</param>
        private void testIntervalShape(T leftValue, BoundaryEnum leftBoundary, T rightValue, BoundaryEnum rightBoundary)
        {
            if (leftBoundary == BoundaryEnum.Infinity || rightBoundary == BoundaryEnum.Infinity)
                return;
            int comparerResult = _category.Attribute.Compare(leftValue, rightValue);
            if (comparerResult > 0 ||
                (comparerResult == 0 && (leftBoundary != BoundaryEnum.Closed || rightBoundary != BoundaryEnum.Closed)))
            {
                // the interval has bad shape
                throw new ArgumentException("Left value is not less than right value of the interval.");
            }
        }

        private void add(int index, T leftValue, BoundaryEnum leftBoundary, T rightValue, BoundaryEnum rightBoundary,
                         bool force, bool nowReducing)
        {
            if (!_category.Attribute.IntervalsAllowed)
                throw new IntervalsNotAllowedException();

            if (isIntervalDegenerative(leftValue, leftBoundary, rightValue, rightBoundary))
            {
                // the interval is degenerative (e.g. <X,X>)
                // it will be added as single value to enumeration
                _category.Enumeration.Add(leftValue, force);
                return;
            }

            testIntervalShape(leftValue, leftBoundary, rightValue, rightBoundary);

            Interval<T> newInterval = new Interval<T>(leftValue, leftBoundary, rightValue, rightBoundary, _category.Attribute);

            string[] categoriesInCollision = _category.Attribute.Axis.CategoriesInCollision(newInterval);
            if (categoriesInCollision.Length == 0)
            {
                // there is no category (even an own) in disjunctivity collision
                // -> simple addition of interval
                add(index, newInterval);
                _category.Attribute.Axis.NotValid(false, true);
                return;
            }
            else if (categoriesInCollision.Length == 1 || categoriesInCollision[0] == _category.Name)
            {
                // disjunctivity collision only with own category 
                // interval is already [partially] contained in this category
                // (it can [partially/full] overlap with some interval or enumeration itemToExclude)
                // -> add the interavl and reduce the category
                add(index, newInterval);
                if (!nowReducing)
                    _category.Reduce();
                _category.Attribute.Axis.NotValid(true, true);
                return;
            }
            else
            {
                // interval is in some disjunctivity collision with other categories
                // the is [partially/full] covering other category(ies) (enumeration(s) or interval(s))
                if (force)
                {
                    _category.Attribute.Exclude(newInterval, categoriesInCollision);
                    add(index, newInterval);
                    if (!nowReducing)
                        _category.Reduce();
                    // the interval could be excluded even from own category (if it was in categoriesInCollision)
                    _category.Attribute.Axis.NotValid(true, true);
                    return;
                }
                else
                {
                    throw Exceptions.AttributeCategoriesDisjunctivityError(null, null, categoriesInCollision);
                }
            }
        }

        /// <summary>
        /// Updates the interval at specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="leftValue">The left value.</param>
        /// <param name="leftBoundary">The left boundary.</param>
        /// <param name="rightValue">The right value.</param>
        /// <param name="rightBoundary">The right boundary.</param>
        /// <param name="force">if set to <c>true</c> the interval is 
        /// updated even if it is in collision with another category 
        /// (intersection will be removed from the another category).
        /// </param>
        public void Update(int index, T leftValue, BoundaryEnum leftBoundary, T rightValue, BoundaryEnum rightBoundary,
                           bool force)
        {
            testIntervalShape(leftValue, leftBoundary, rightValue, rightBoundary);

            Interval<T> tmpInterval = new Interval<T>(leftValue, leftBoundary, rightValue, rightBoundary, _category.Attribute);

            string[] categoriesInCollision = _category.Attribute.Axis.CategoriesInCollision(tmpInterval);

            if (categoriesInCollision.Length == 0)
            {
                // no disjuctivity collision even with previous interval setting
                // -> previous interval is removed, new is added
                RemoveAt(index);
                add(index, leftValue, leftBoundary, rightValue, rightBoundary, force, true);
                return;
            }
            else if (categoriesInCollision.Length == 1 || categoriesInCollision[0] == _category.Name)
            {
                // interval is already [partially] contained in this category
                // but there is no collision with other categories
                // -> previous interval is removed, new is added 
                // + reduction if needed ... not if in collision only if previous interval setting (in add(...) method)
                RemoveAt(index);
                add(index, leftValue, leftBoundary, rightValue, rightBoundary, force, false);
                return;
            }
            // interval is in some disjunctivity collision 
            else
            {
                if (force)
                {
                    // this will make all needed (exclusion, addition, reduction, ...)
                    RemoveAt(index);
                    add(index, leftValue, leftBoundary, rightValue, rightBoundary, force, false);
                    return;
                }
                else
                {
                    throw Exceptions.AttributeCategoriesDisjunctivityError(null, null, categoriesInCollision);
                }
            }
        }

        /// <summary>
        /// Clears the collection of intervals.
        /// </summary>
        public void Clear()
        {
            _intervals.Clear();
        }

        /// <summary>
        /// Determines whether contains the specified interval (<c>item</c>).
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        /// 	<c>true</c> if contains the specified interval (<c>item</c>); otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(Interval<T> item)
        {
            return _intervals.Contains(item);
        }

        /// <summary>
        /// Gets the count of intervals.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get { return _intervals.Count; }
        }

        /// <summary>
        /// Removes the specified interval (<c>item</c>).
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public bool Remove(Interval<T> item)
        {
            _category.Attribute.Axis.NotValid(false, true);
            return _intervals.Remove(item);
        }

        #endregion

        #region IEnumerable<interval<T>> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection of intervals.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection of intervals.
        /// </returns>
        public IEnumerator<Interval<T>> GetEnumerator()
        {
            return _intervals.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a collection of intervals.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection of intervals.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _intervals.GetEnumerator();
        }

        #endregion
    }
}