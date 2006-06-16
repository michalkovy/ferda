using System;
using System.Collections;
using System.Collections.Generic;

namespace Ferda.Guha.Attribute
{
    /// <summary>
    /// The enumeration of values (set of values) (for nominal, ordinal or cardinal data).
    /// </summary>
    /// <typeparam name="T">Domain type</typeparam>
    public class Enumeration<T> : IEnumerable<T> // partially IList<T>, ICollection<T>
        where T : IComparable
    {
        #region Fields

        private List<T> _enumeration = new List<T>();
        private Category<T> _category;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Ferda.Guha.Attribute.Enumeration{T}"/> class.
        /// </summary>
        private Enumeration()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Ferda.Guha.Attribute.Enumeration{T}"/> class.
        /// </summary>
        /// <param name="category">The category where the enumeration belongs to.</param>
        public Enumeration(Category<T> category)
        {
            _category = category;
        }

        #endregion
        
        internal T[] Export()
        {
            return _enumeration.ToArray();
        }

        /// <summary>
        /// Sorts this instance. In DEBUG (and not lazy reduction) mode, the disjunctivity is also checked.
        /// </summary>
        public void Sort()
        {
            _enumeration.Sort(_category.Attribute);

#if DEBUG
            if (_category.Attribute.LazyReduction)
                return;
            bool initialized = false;
            T lastEnumItem = default(T);
            foreach (T item in _enumeration)
            {
                if (initialized)
                {
                    if (_category.Attribute.Compare(lastEnumItem, item) >= 0)
                        throw new Exception("This should never happend.");
                }
                else
                    initialized = true;
                lastEnumItem = item;
            }
#endif
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current enumeration.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current enumeration.
        /// </returns>
        public override string ToString()
        {
            string result = String.Empty;
            for (int i = 0; i < Count; i++)
            {
                if (i > 0)
                    result += Common.CategoryMembersSeparator;

                result += _enumeration[i].ToString();
            }
            return result;
        }

        #region IList<T> Members

        /// <summary>
        /// Indexes the of the specified <c>item</c>.
        /// </summary>
        /// <param name="item">The item.</param>
        public int IndexOf(T item)
        {
            return _enumeration.IndexOf(item);
        }

        /// <summary>
        /// Removes item at specified <c>index</c>.
        /// </summary>
        /// <param name="index">The index.</param>
        public void RemoveAt(int index)
        {
            _enumeration.RemoveAt(index);
            _category.Attribute.Axis.NotValid(true, false);
        }

        /// <summary>
        /// Gets the item at the specified <c>index</c>.
        /// </summary>
        /// <value></value>
        public T this[int index]
        {
            get { return _enumeration[index]; }
        }

        #endregion

        #region ICollection<T> Members

        /// <summary>
        /// Adds the specified item to the enumeration.
        /// If <c>force</c> mode is enabled than collisisons are resolved by
        /// exclusion of the <c>item</c> from the other category in collisions.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="force">If set to <c>true</c> force addition is enambled (on); otherwise, exception is thrown when collision is detected.</param>
        /// <exception cref="T:Ferda.Guha.Attribute.DisjunctivityCollisionException">
        /// Thrown in not forced mode (force is off) and collision with any other 
        /// category is detected.
        /// </exception>
        public void Add(T item, bool force)
        {
            // already contained in the enumeration
            // -> finish
            if (Contains(item))
                return;

            // test disjunctivity collisions of not contained itemToExclude - throw an exception
            string[] categoryInCollision = _category.Attribute.Axis.CategoriesInCollision(item);
            if (categoryInCollision.Length == 0)
            {
                // no disjuntivity collisions
                // the value is not covered by no category
                _enumeration.Add(item);
                _category.Reduce(false); // can be joined to some interval (value of the open boundary of some interval)
                _category.Attribute.Axis.NotValid(true, false);
                return;
            }
            else if (categoryInCollision.Length == 1 && categoryInCollision[0] == _category.Name)
            {
                // disjunctivity collision only with own category 
                // i.e. already contained in own interval
                // -> finish
                return;
            }
            else
            {
                // disjunctivity collisions
                // the value is in other category (enumeration xor interval)
                if (force)
                {
                    _category.Attribute.Exclude(item, categoryInCollision);
                    _enumeration.Add(item);
                    _category.Reduce(false);
                    // can be joined to some interval (value of the open boundary of some interval)
                    _category.Attribute.Axis.NotValid(true, true);
                    return;
                }
                else
                {
                    throw new DisjunctivityCollisionException(categoryInCollision);
                }
            }
        }

        /// <summary>
        /// Clears the enumeration.
        /// </summary>
        public void Clear()
        {
            _enumeration.Clear();
            _category.Attribute.Axis.NotValid(true, false);
        }

        /// <summary>
        /// Determines whether the enumeration contains the specified <c>item</c>.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        /// 	<c>true</c> if enumeration contains the specified <c>item</c>; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(T item)
        {
            return _enumeration.Contains(item);
        }

        /// <summary>
        /// Gets the count of values in the enumeration.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get { return _enumeration.Count; }
        }

        /// <summary>
        /// Removes the specified <c>item</c>.
        /// </summary>
        /// <param name="item">The item.</param>
        public bool Remove(T item)
        {
            if (_enumeration.Contains(item))
                _category.Attribute.Axis.NotValid(true, false);
            return _enumeration.Remove(item);
        }

        #endregion

        #region IEnumerable<T> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection of values in enumeration.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection of values in enumeration.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _enumeration.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a collection of values in enumeration.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection of values in enumeration.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _enumeration.GetEnumerator();
        }

        #endregion
    }
}