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
        public int ActSize
        {
            get { return _actSize; }
        }

        public int Count
        {
            get { return _cache.Count; }
        }

        private LinkedList<KeyT> _list;
        private Dictionary<KeyT, ValueT> _cache;

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

        public abstract ValueT GetValue(KeyT key);

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
                int removingValueSize = GetSize(removingValue);
                _cache.Remove(lastKey);
                _actSize -= removingValueSize;
            }
        }
        
        public bool Contains(KeyT key)
        {
            return _cache.ContainsKey(key);
        }

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

        public override string ToString()
        {
            return Helpers.Common.Print.SequenceToString(_list, ", ") + " (act size: " + ActSize + ")";
        }
    }
}