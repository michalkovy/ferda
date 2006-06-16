using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Ferda.Guha.MiningProcessor.BitStrings
{
    public class MissingInformation
    {
        private readonly IBitStringCache _bitStringCache;

        public MissingInformation(IBitStringCache bitStringCache)
        {
            _bitStringCache = bitStringCache;
        }

        private class pair
        {
            private readonly ReadOnlyCollection<Guid> _key;
            private readonly IBitString _value;
            public ReadOnlyCollection<Guid> Key
            {
                get { return _key; }
            }
            public IBitString Value
            {
                get { return _value; }
            }

            public pair(ReadOnlyCollection<Guid> key, IBitString value)
            {
                _key = key;
                _value = value;
            }
        }

        private Stack<pair> _cache = new Stack<pair>();


        /// <summary>
        /// Compares the specified collections of Guids. (If cached &gt; asked returns integer &gt; 0)
        /// </summary>
        /// <param name="cached">The cached.</param>
        /// <param name="asked">The asked.</param>
        /// <returns>1 ... uncomparable or greater</returns>
        private static int Compare(ReadOnlyCollection<Guid> cached, ReadOnlyCollection<Guid> asked, out List<Guid> notInCached)
        {
            notInCached = null;
            int sizeComparation = cached.Count - asked.Count;
            if (sizeComparation > 0)
                return 1;
            else if (sizeComparation == 0)
            {
                // test equality
                foreach (Guid guid in asked)
                {
                    if (cached.Contains(guid))
                        continue;
                    else
                        return 1;
                }
                return 0;
            }
            else //if (sizeComparation < 0)
            {
                // test subset
                foreach (Guid guid in cached)
                {
                    if (cached.Contains(guid))
                        continue;
                    else
                        return 1;
                }
                // collect notInCached
                notInCached = new List<Guid>();
                foreach (Guid guid in asked)
                {
                    if (cached.Contains(guid))
                        continue;
                    else
                        notInCached.Add(guid);
                }
                return -1;
            }
        }

        public IBitString GetMissingInformation(ReadOnlyCollection<Guid> attributesIds)
        {
        restart:
            if (_cache.Count > 0)
            {
                pair last = _cache.Peek();
                List<Guid> notInCached = null;
                int comparationResult = Compare(last.Key, attributesIds, out notInCached);
                if (comparationResult == 0)
                    return last.Value;
                else if (comparationResult == 1)
                {
                    _cache.Pop();
                    goto restart;
                }
                else if (comparationResult == -1)
                {
                    IBitString newCached = null;
                    foreach (Guid guid in notInCached)
                    {
                        IBitString newBitString = _bitStringCache.GetMissingInformationBitString(guid);
                        if (newCached == null)
                            newCached = last.Value.Or(newBitString);
                        else
                            newCached = newCached.Or(newBitString);
                    }
                    _cache.Push(new pair(attributesIds, newCached));
                    return _cache.Peek().Value;
                }
                throw new ApplicationException();
            }
            else
            {
                IBitString newCached = null;
                foreach (Guid guid in attributesIds)
                {
                    IBitString newBitString = _bitStringCache.GetMissingInformationBitString(guid);
                    if (newCached == null)
                        newCached = newBitString;
                    else
                        newCached = newCached.Or(newBitString);
                }
                _cache.Push(new pair(attributesIds, newCached));
                return _cache.Peek().Value;
            }
            /*
             * zasobnik 
             * je-li posledni mensi pak prodluz, uloz a pouzij
             * je-li stejny pak nech ulozeny a pouzij
             * je-li delsi jiny pak iterativnì zkracuj
             * */
        }
    }
}
