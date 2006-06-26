using System;
using System.Collections.Generic;
using System.Text;

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
        public Set()
        {
        }

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

        #endregion

        public Set<T> Join(Set<T> op2)
        {
            return Join(this, op2);
        }

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
