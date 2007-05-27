using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Ferda.Modules.Helpers.Caching
{
    /// <summary>
    /// Many times during the usage of an algorithm, a list of the last (n) most recently used 
    /// items comes to be usefull. Sometimes this is referred to the least recently used (LRU)
    /// cache, but this simply implies which e elements that fall out of the list (i.e. the 
    /// least recently used ones).  
    /// 
    /// The usage of the class is the same of a dictionary. Keys and values are insert into
    /// the dictionary. If a new key is inserted into the table and the cache has exceeded the 
    /// set size, then the least recently used item is removed from the cache.
    /// 
    /// Basically, as items are added to the list, they are appended
    /// to a doubly linked list. This list is usefull in the fact that it allows deletion and
    /// insertion at O(1) time.  Since if a reference element (n) is not in the list of most
    /// recently used items, then at least one value falls out of the cache.
    /// </summary>
    public abstract class MostRecentlyUsed<KeyT, ValueT> : DictionaryBase
        where ValueT : class
        where KeyT : IEquatable<KeyT>
    {
        private int _maxSize = 50;
        /// <summary>
        /// Gets or sets the maximal size of the cache.
        /// </summary>
        /// <value>The maximal size of the cache.</value>
        public int MaxSize
        {
            get { return _maxSize; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value", value, "The maximum cache size must be non-negative.");

                if (value < _maxSize)
                    shrinkCache(value);

                _maxSize = value;
            }
        }

        private int _actSize = 0;
        /// <summary>
        /// Gets the actual size of the cache.
        /// </summary>
        /// <value>The actual size of the cache.</value>
        public int ActSize
        {
            get { return _actSize; }
        }

        /// <summary>
        /// Gets the number of elements contained in the cache instance.
        /// </summary>
        /// <value></value>
        /// <returns>The number of elements contained in the cache instance.</returns>
        public new int Count
        {
            get { return _cache.Count; }
        }

        private LinkedList<KeyT> _list;
        private Dictionary<KeyT, ValueT> _cache;

        /// <summary>
        /// Gets an <see cref="T:System.Collections.ICollection"></see> object containing the keys of the <see cref="T:System.Collections.IDictionary"></see> object.
        /// </summary>
        /// <value></value>
        /// <returns>An <see cref="T:System.Collections.ICollection"></see> object containing the keys of the <see cref="T:System.Collections.IDictionary"></see> object.</returns>
        public Dictionary<KeyT, ValueT>.KeyCollection Keys
        {
            get
            {
                return _cache.Keys;
            }
        }
        
        /// <summary>
        /// Construct a most recently used items list with the maximum number of items
        /// allowed in the list.
        /// </summary>
        /// <param name="maxItems">Maximum number of items allowed</param>
        public MostRecentlyUsed(int maxItems)
        {
            _maxSize = maxItems;
            _list = new LinkedList<KeyT>();
            _cache = new Dictionary<KeyT, ValueT>(_maxSize);
        }

        /// <summary>
        /// Gets the value of specified <c>key</c>.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public abstract ValueT GetValue(KeyT key);

        /// <summary>
        /// Gets the size of the value item.
        /// </summary>
        /// <param name="itemToMeasure">The item to measure.</param>
        /// <returns></returns>
        public virtual int GetSize(ValueT itemToMeasure)
        {
            return 1;
        }

        private void shrinkCache(int newSize)
        {
            while (_actSize > newSize && _list.Count > 0)
            {
                KeyT lastKey = _list.Last.Value;
                _list.RemoveLast();
                ValueT removingValue = _cache[lastKey];
                _actSize -= GetSize(removingValue);
                _cache.Remove(lastKey);
            }
        }

        /// <summary>
        /// Determines whether the cache contains the value with specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// 	<c>true</c> if the cache contains the value with specified key; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(KeyT key)
        {
            return _cache.ContainsKey(key);
        }

        /// <summary>
        /// Gets the value with the specified key.
        /// </summary>
        /// <value></value>
        public ValueT this[KeyT key]
        {
            get
            {
                lock (this)
                {
                    ValueT item;
                    if (_cache.TryGetValue(key, out item))
                    {
                        if (!_list.First.Value.Equals(key))
                        {
                            _list.Remove(key);
                            _list.AddFirst(key);
                        }
                        return item;
                    }
                    else
                    {
                        ValueT newValue = GetValue(key);
                        int newValueSize = GetSize(newValue);

                        shrinkCache(_maxSize - newValueSize);

                        Debug.Assert(_actSize >= 0);

                        _actSize += newValueSize;
                        _list.AddFirst(key);
                        _cache.Add(key, newValue);
                        return newValue;
                    }
                }
            }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return Helpers.Common.Print.SequenceToString(_list, ", ") + " (act size: " + ActSize + ")";
        }
    }
}
