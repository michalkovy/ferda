// MostRecentlyUsed.cs - Implementation of the most recently used cache
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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Ferda.Modules.Helpers.Common;

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
        private ulong _maxSize = 50;
        private readonly AsyncLock _gate = new AsyncLock();
        /// <summary>
        /// Gets or sets the maximal size of the cache.
        /// </summary>
        /// <value>The maximal size of the cache.</value>
        public ulong MaxSize
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

        private ulong _actSize = 0;
        /// <summary>
        /// Gets the actual size of the cache.
        /// </summary>
        /// <value>The actual size of the cache.</value>
        public ulong ActSize
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
        public MostRecentlyUsed(ulong maxItems)
        {
            _maxSize = maxItems;
            _list = new LinkedList<KeyT>();
            _cache = new Dictionary<KeyT, ValueT>();
        }

        /// <summary>
        /// Retrieves the value from external sources. The value can be then
        /// added to the cache. This is why the method is abstract.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public abstract Task<ValueT> GetValueExternalAsync(KeyT key);

        /// <summary>
        /// Gets the size of the value item.
        /// </summary>
        /// <param name="itemToMeasure">The item to measure.</param>
        /// <returns></returns>
        public virtual ulong GetSize(ValueT itemToMeasure)
        {
            return 1;
        }

        private void shrinkCache(ulong newSize)
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
        /// Gets the value with the specified key. Note that if the
        /// value represented by key is not present in the cache, the
        /// cache calls the GetValue method which calls some external
        /// sources and, retrieves the value. This method adds the value
        /// to the dictionary (which serves as a cache).
        /// </summary>
        /// <returns>Value that is returned.</returns>
        public async Task<ValueT> GetValueAsync(KeyT key)
        {
            using (await _gate.LockAsync().ConfigureAwait(false))
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
                    ValueT newValue = await GetValueExternalAsync(key).ConfigureAwait(false);
                    ulong newValueSize = GetSize(newValue);

                    shrinkCache(_maxSize - newValueSize);

                    Debug.Assert(_actSize >= 0);

                    _actSize += newValueSize;
                    _list.AddFirst(key);
                    _cache.Add(key, newValue);
                    return newValue;
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
