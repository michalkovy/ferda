#define Testing

using System;
using System.Collections.Generic;
using System.Diagnostics;

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
    public class BitStringCache : IBitStringCache
    {
        private static readonly BitStringCache _instance = new BitStringCache();
        private static readonly object padlock = new object();

        private int _maxSize;
        private int _actualSize;

        private Dictionary<BitStringIdentifier, LRUBitStringContainer> _storedBitStrings =
            new Dictionary<BitStringIdentifier, LRUBitStringContainer>();

        private LRUBitStringContainer _lastUsedBitString;
        private LRUBitStringContainer _leastUsedBitString;

        //key is GUID of attribute
        private Dictionary<Guid, BitStringGeneratorPrx> _bitStringGenerators =
            new Dictionary<Guid, BitStringGeneratorPrx>();

        //private DataPreprocessor _sourceProcesor;

        private const int cacheDefaultSize = 1048576; // ~1Mb
        //private const int cacheDefaultSize = 1310720; // ~10MB
        // size is in BitString units i.e. for strings of 
        // bits its in bits, for strings of floats (fuzzy bit strings) 
        // it is in floats

        /// <summary>
        /// Gets or sets the maximum cache size.
        /// </summary>
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

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static BitStringCache()
        {
        }

        /// <summary>
        /// Initializes an empty cache. Cache has a default size 1MB.
        /// </summary>
        private BitStringCache()
        {
            _maxSize = cacheDefaultSize;
        }

        public static IBitStringCache GetInstance(BitStringGeneratorPrx prx)
        {
#if Testing
            return _instance;
#endif
            lock (padlock)
            {
                Guid prxAttributeID = new Guid(prx.GetAttributeId().value);
                if (!_instance._bitStringGenerators.ContainsKey(prxAttributeID))
                    _instance._bitStringGenerators.Add(prxAttributeID, prx);

                return _instance;
            }
        }

        private BitStringGeneratorPrx getBitStringGeneratorPrx(Guid attributeId)
        {
            BitStringGeneratorPrx prx;
            if (_bitStringGenerators.TryGetValue(attributeId, out prx))
            {
                return prx;
            }
            else
                throw new ArgumentException(
                    "There is not reference to bit string generator (proxy) in bit string cache.");
        }

        private IBitString loadBitStringFromGeneratorPrx(BitStringIdentifier bitStringId)
        {
            //TODO
            //return getBitStringGeneratorPrx(bitStringId.AttributeId).GetBitString(bitStringId._categoryId);
            return null;
        }

        private void shrinkCache(int requiredSize)
        {
            while (_actualSize > requiredSize)
            {
                LRUBitStringContainer bitStringToRemove = _leastUsedBitString;

                _leastUsedBitString = _leastUsedBitString.NextBitString;

                if (_leastUsedBitString != null)
                    _leastUsedBitString.PrevBitString = null;
                else
                    _lastUsedBitString = null;

                _actualSize -= bitStringToRemove.BitString.Length;
                _storedBitStrings.Remove((BitStringIdentifier) bitStringToRemove.BitString.Identifier);
            }
        }

        private class LRUBitStringContainer
        {
            private LRUBitStringContainer _prevBitString;

            public LRUBitStringContainer PrevBitString
            {
                get { return _prevBitString; }
                set { _prevBitString = value; }
            }

            private LRUBitStringContainer _nextBitString;

            public LRUBitStringContainer NextBitString
            {
                get { return _nextBitString; }
                set { _nextBitString = value; }
            }

            private IBitString _bitString;

            public IBitString BitString
            {
                get { return _bitString; }
            }

            public LRUBitStringContainer(IBitString bitString)
            {
                _bitString = bitString;
            }
        }

        #region IBitStringCache Members

        /// <summary>
        /// Allows to obtain bit string identified by its guid. 
        /// Internally distinguish between two possible cases.
        /// First case - bit string is stored in cache, leads to simple handling. Stored bit string is returned to the caller.
        /// Second case - bit string is not found in cache, leads to request to data preprocesor. Obtained string is first stored in cache and then returned to the caller.
        /// Cache is transparent for any exceptions from data preprocesor.
        /// </summary>
        /// <param name="bitStringId">Identification of required bit string</param>
        /// <returns>Bit string responding to given guid that is stored in cache or obtained from data preprocesor.</returns>
        public IBitString GetBitString(BitStringIdentifier bitStringId)
        {
#if Testing
            return BitStringCacheTest.GetBitString(bitStringId);
#endif
            LRUBitStringContainer storedBitString;
            if (_storedBitStrings.TryGetValue(bitStringId, out storedBitString))
            {
                // LRU list reorganization
                if (storedBitString != _lastUsedBitString)
                {
                    // if we have accessed the least used BitString we need change the variable
                    if (storedBitString == _leastUsedBitString)
                        if (_leastUsedBitString.NextBitString != null)
                            _leastUsedBitString = _leastUsedBitString.NextBitString;

                    if (storedBitString.NextBitString != null)
                        storedBitString.NextBitString.PrevBitString = storedBitString.PrevBitString;
                    if (storedBitString.PrevBitString != null)
                        storedBitString.PrevBitString.NextBitString = storedBitString.NextBitString;

                    storedBitString.NextBitString = null;

                    storedBitString.PrevBitString = _lastUsedBitString;
                    _lastUsedBitString.NextBitString = storedBitString;
                    _lastUsedBitString = storedBitString;
                }

                return storedBitString.BitString;
            }
            else
            {
                IBitString newBitString = loadBitStringFromGeneratorPrx(bitStringId);

                Debug.Assert(newBitString.Identifier.Equals(bitStringId));

                _actualSize += newBitString.Length;

                // REVIEW: what if the cache size is too small to contain one BitString?
                if (_actualSize > _maxSize)
                    shrinkCache(_maxSize);

                LRUBitStringContainer newBitStringContainer = new LRUBitStringContainer(newBitString);
                newBitStringContainer.PrevBitString = _lastUsedBitString;

                if (_lastUsedBitString != null)
                    _lastUsedBitString.NextBitString = newBitStringContainer;

                _lastUsedBitString = newBitStringContainer;
                if (_leastUsedBitString == null)
                    _leastUsedBitString = newBitStringContainer;

                _storedBitStrings.Add(bitStringId, newBitStringContainer);

                return newBitString;
            }
        }

        /// <summary>
        /// Gets actual amount of bit strings stored in cache.
        /// </summary>
        public int BitStringsCount
        {
            get { return _storedBitStrings.Count; }
        }

        /// <summary>
        /// Allows to check whether the cache contains some concrete bit string (identified by guid)
        /// </summary>
        /// <param name="bitStringId">Bit string identification</param>
        /// <returns>True if cache contains specified bit string, otherwise false</returns>
        public bool ContainsBitString(BitStringIdentifier bitStringId)
        {
            return _storedBitStrings.ContainsKey(bitStringId);
        }

        public IBitString GetMissingInformationBitString(Guid attributeId)
        {
            string missingInformationCategoryId =
                getBitStringGeneratorPrx(attributeId).GetMissingInformationCategoryId();
            if (missingInformationCategoryId != null)
            {
                return GetBitString(new BitStringIdentifier(attributeId, missingInformationCategoryId));
            }
            else
            {
                // TODO
                // attribute has no missing values, "false" bit string should be returned
                throw new NotImplementedException();
            }
        }

        public string[] GetCategoriesIds(Guid attributeId)
        {
#if Testing
            return BitStringCacheTest.GetCategoriesIds(attributeId);
#endif
            return getBitStringGeneratorPrx(attributeId).GetCategoriesIds();
        }

        #endregion
    }
}