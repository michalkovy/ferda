//#define Testing

using System;
using System.Collections.Generic;
using Ferda.Modules.Helpers.Caching;

namespace Ferda.Guha.MiningProcessor.BitStrings
{
    /// <summary>
    /// This class is a Singleton class that serves as a provider of 
    /// bit strings with local in-memory storage. It uses LRU strategy, 
    /// when memory consumption grows over a given limit.
    /// </summary>
    /// <remarks>
    /// BitStringCache does not care about the data mining tasks, 
    /// settings and other complexities. It only stores BitStrings 
    /// identified by BitStringIdentifier (i.e. pair attribute ID (Guid), 
    /// category ID (string)).
    /// </remarks>
    /// <seealso href="http://www.yoda.arachsys.com/csharp/singleton.html">Singleton in .NET C#</seealso>
    public class BitStringCache : MostRecentlyUsed<BitStringIdentifier, IBitString>, IBitStringCache
    {
        private static readonly BitStringCache _instance = new BitStringCache(cacheDefaultSize);
        private static readonly object padlock = new object();

        private const int cacheDefaultSize = 1048576; // ~1Mb
        // private const int cacheDefaultSize = 1310720; // ~10MB
        // size is in BitString units i.e. for strings of 
        // bits its in bits, for strings of floats (fuzzy bit strings) 
        // it is in floats

        // key is attributeGuid
        private Dictionary<string, BitStringGeneratorPrx> _bitStringGenerators =
            new Dictionary<string, BitStringGeneratorPrx>();

        private MissingInformationBitStringsIdsCache _missingInformationBitStringsIdsCache;

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static BitStringCache()
        {
        }

        /// <summary>
        /// Initializes an empty cache. Cache has a default size 1MB.
        /// </summary>
        private BitStringCache(int maxItems)
            : base(maxItems)
        {
            _missingInformationBitStringsIdsCache = new MissingInformationBitStringsIdsCache(this);
        }

        internal static IBitStringCache GetInstance()
        {
            return _instance;
            // !!! only for constructor of MissingInformation 
            // (MI class is initialized before cache is fisrttime 
            // touched by MI instance is firstime used after some 
            // usage the cache (rightway inicialization of cache))
        }

        public static IBitStringCache GetInstance(BitStringGeneratorPrx prx)
        {
#if Testing
            return _instance;
#else
            lock (padlock)
            {
                string attributeGuid = prx.GetAttributeId().value;
                if (!_instance._bitStringGenerators.ContainsKey(attributeGuid))
                    _instance._bitStringGenerators.Add(attributeGuid, prx);

                return _instance;
            }
#endif
        }

        public BitStringGeneratorPrx GetBitStringGeneratorPrx(string attributeGuid)
        {
            BitStringGeneratorPrx prx;
            if (_bitStringGenerators.TryGetValue(attributeGuid, out prx))
            {
                return prx;
            }
            else
                throw new ArgumentException(
                    "There is not reference to bit string generator (proxy) in bit string cache.");
        }

        #region IBitStringCache Members

        public new IBitString this[BitStringIdentifier bitStringId]
        {
            get
            {
#if Testing
                return BitStringCacheTest.GetBitString(bitStringId);
#else
                return base[bitStringId];
#endif
            }
        }

        public IBitString this[string attributeGuid, string categoryId]
        {
            get { return this[new BitStringIdentifier(attributeGuid, categoryId)]; }
        }

        public IBitString GetMissingInformationBitString(string attributeGuid)
        {
            string categoryId = _missingInformationBitStringsIdsCache[attributeGuid];
            if (categoryId != null)
            {
                return base[new BitStringIdentifier(attributeGuid, categoryId)];
            }
            else
            {
                return FalseBitString.GetInstance();
            }
        }

        public string[] GetCategoriesIds(string attributeGuid)
        {
#if Testing
            return BitStringCacheTest.GetCategoriesIds(attributeGuid);
#else
            return GetBitStringGeneratorPrx(attributeGuid).GetCategoriesIds();
#endif
        }

        #endregion

        public override IBitString GetValue(BitStringIdentifier key)
        {
            BitStringIce bs = GetBitStringGeneratorPrx(key.AttributeGuid).GetBitString(key.CategoryId);
            return new BitString(key, bs.length, bs.value);
        }

        public override int GetSize(IBitString itemToMeasure)
        {
            return itemToMeasure.Length;
        }
    }
}