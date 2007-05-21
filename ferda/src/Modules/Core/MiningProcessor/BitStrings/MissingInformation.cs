using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Ferda.Modules.Helpers.Caching;
using Ferda.Modules.Helpers.Common;

namespace Ferda.Guha.MiningProcessor.BitStrings
{
    public class MissingInformation : MostRecentlyUsed<Set<string>, IBitString>
    {
        private static IBitStringCache _bitStringCache;
		private static readonly MissingInformation _instance = new MissingInformation();

        public const int cacheDefaultSize = 1048576; // ~1Mb

        private MissingInformation()
            : base(cacheDefaultSize)
        {
            _bitStringCache = BitStringCache.GetInstance();
        }

        public override IBitString GetValue(Set<string> key)
        {
            if (key.Count == 0)
                return FalseBitString.GetInstance();

            // try get subset
            Set<string> bestMatch = null;
            foreach (Set<string> set in Keys)
            {
                if (set.Count > key.Count)
                    continue;
                if (bestMatch == null || bestMatch.Count < set.Count)
                {
                    // test subset
                    if (set.IsSubsetOf(key))
                    {
                        bestMatch = set;
                        if (bestMatch.Count == key.Count - 1)
                            break;
                    }
                }
            }
            if (bestMatch != null)
            {
                IBitString last = this[bestMatch];
                IBitString newCached = null;
                foreach (string guid in key)
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
                foreach (string guid in key)
                {
                    IBitString newBitString = _bitStringCache.GetMissingInformationBitString(guid);
                    if (newCached == null)
                        newCached = newBitString;
                    else
                        newCached = newCached.Or(newBitString);
                }
                return newCached;
            }
        }

        public override int GetSize(IBitString itemToMeasure)
        {
            return itemToMeasure.Length;
        }
		
		public static MissingInformation GetInstance()
        {
            return _instance;
		}
    }
}
