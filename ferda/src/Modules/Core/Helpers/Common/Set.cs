// Set.cs - Strongly typed set of items
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
using System.Collections.Generic;

namespace Ferda.Modules.Helpers.Common
{
    /// <summary>
    /// Represents a strongly typed set of items. Provides methods
    /// Contains, Add, Remove.
    /// </summary>
    /// <typeparam name="T">is T</typeparam>
    public class Set<T> : IEquatable<Set<T>>, IEnumerable<T>
        where T : IEquatable<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Ferda.Modules.Helpers.Common.Set`1"/> class.
        /// </summary>
        public Set()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Ferda.Modules.Helpers.Common.Set`1"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        public Set(T item)
        {
            _set.Add(item);
        }

        private List<T> _set = new List<T>();
        /// <summary>
        /// Determines whether the set contains the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        /// 	<c>true</c> if the set contains the specified item; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(T item)
        {
            return _set.Contains(item);
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Add(T item)
        {
            if (item != null)
                if (!Contains(item))
                    _set.Add(item);
        }

        /// <summary>
        /// Adds the specified items.
        /// </summary>
        /// <param name="items">The items.</param>
        public void AddRange(IEnumerable<T> items)
        {
            if (items != null)
                foreach (T item in items)
                {
                    Add(item);
                }
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public bool Remove(T item)
        {
            return _set.Remove(item);
        }

        /// <summary>
        /// Gets the number of elements actually contained in the set.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get { return _set.Count; }
        }



        /// <summary>
        /// Determines whether the specified set is subset of this set.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <returns>
        /// 	<c>true</c> if the sepecified set is subset of this set; otherwise, <c>false</c>.
        /// </returns>
        public bool IsSubsetOf(Set<T> set)
        {
            if (set == null)
                return false;
            if (set.Count < Count)
                return false;
            else
                foreach (T item in _set)
                {
                    if (!set.Contains(item))
                        return false;
                }
            return true;
        }

        #region IEquatable<Set<T>> Members

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the other parameter; otherwise, false.
        /// </returns>
        public bool Equals(Set<T> other)
        {
            if (other == null)
                return false;
            if (other.Count != Count)
                return false;
            foreach (T item in other._set)
            {
                if (!Contains(item))
                    return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            if (Count == 0)
                return 0;
            else
            {
                int result = 0;
                foreach (T item in _set)
                {
                    result |= item.GetHashCode();
                }
                result ^= Count;
                return result;
            }
        }

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            Set<T> set = obj as Set<T>;
            if (set == null) return false;
            if (!Equals(set)) return false;
            return true;
        }

        #endregion

        /// <summary>
        /// Joins the specified set.
        /// </summary>
        /// <param name="op2">The set.</param>
        /// <returns></returns>
        public Set<T> Join(Set<T> op2)
        {
            return Join(this, op2);
        }

        /// <summary>
        /// Joins the specified sets.
        /// </summary>
        /// <param name="op1">The set 1.</param>
        /// <param name="op2">The set 2.</param>
        /// <returns></returns>
        public static Set<T> Join(Set<T> op1, Set<T> op2)
        {
            Set<T> result = new Set<T>();
            if (op1 != null)
                foreach (T item in op1._set)
                {
                    result.Add(item);
                }
            if (op2 != null)
                foreach (T item in op2._set)
                {
                    result.Add(item);
                }
            return result;
        }

        /// <summary>
        /// Joins the specified sets.
        /// </summary>
        /// <param name="ops">The sets.</param>
        /// <returns></returns>
        public static Set<T> Join(Set<T>[] ops)
        {
            Set<T> result = new Set<T>();
            if (ops != null)
                foreach (Set<T> set in ops)
                {
                    if (set != null)
                        foreach (T item in set._set)
                        {
                            result.Add(item);
                        }

                }
            return result;
        }

        #region IEnumerable<Set<T>> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _set.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
