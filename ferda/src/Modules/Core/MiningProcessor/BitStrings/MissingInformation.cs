using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Ferda.Modules.Helpers.Caching;
using Ferda.Modules.Helpers.Common;

namespace Ferda.Guha.MiningProcessor.BitStrings
{
    public class MissingInformation : MostRecentlyUsed<Set<Guid>, IBitString>
    {
        private readonly IBitStringCache _bitStringCache;

        public const int cacheDefaultSize = 1048576; // ~1Mb

        public MissingInformation()
            : base(cacheDefaultSize)
        {
            _bitStringCache = BitStringCache.GetInstance();
        }

        public override IBitString GetValue(Set<Guid> key)
        {
            if (key.Count == 0)
                return new EmptyBitString();

            // try get subset
            Set<Guid> bestMatch = null;
            foreach (Set<Guid> set in Keys)
            {
                if (set.Count > key.Count)
                    continue;
                if (bestMatch == null || bestMatch.Count < set.Count)
                {
                    // test subset
                    if (set.IsSubsetOf(key))
                        bestMatch = set;
                    if (bestMatch.Count == key.Count - 1)
                        break;
                }
            }
            if (bestMatch !=  null)
            {
                IBitString last = this[bestMatch];
                IBitString newCached = null;
                foreach (Guid guid in key)
                {
                    if (!bestMatch.Contains(guid))
                    {
                        IBitString newBitString = _bitStringCache.GetMissingInformationBitString(guid);
                        if (newCached == null)
                            newCached = last.Or(newBitString);
                        else
                            newCached = newCached.Or(newBitString);
                    }
                }
                return newCached;
            }
            else
            {
                IBitString newCached = null;
                foreach (Guid guid in key)
                {
                    if (!bestMatch.Contains(guid))
                    {
                        IBitString newBitString = _bitStringCache.GetMissingInformationBitString(guid);
                        if (newCached == null)
                            newCached = newBitString;
                        else
                            newCached = newCached.Or(newBitString);
                    }
                }
                return newCached;
            }
        }

        public override int GetSize(IBitString itemToMeasure)
        {
            return itemToMeasure.Length;
        }
    }
}
