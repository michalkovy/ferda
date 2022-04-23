// BitStringCache.cs - Cache of bit strings
//
// Author: Tomáš Kuchaø <tomas.kuchar@gmail.com>
// Commented by: Martin Ralbovský <martin.ralbovsky@gmail.com>
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
    /// category ID (string)). It also provides information about
    /// attributes and their categories. 
    /// </remarks>
    /// <seealso href="http://www.yoda.arachsys.com/csharp/singleton.html">Singleton in .NET C#</seealso>
    public class BitStringCache : MostRecentlyUsed<BitStringIdentifier, IBitString>, IBitStringCache
    {
        #region Private fields

        /// <summary>
        /// The singleton (one) object that can be created
        /// </summary>
        private static readonly BitStringCache _instance = new BitStringCache(cacheDefaultSize);
        /// <summary>
        /// Object used for thread-safe access to the bit string cache
        /// </summary>
        private static readonly object padlock = new object();

        //private const int cacheDefaultSize = 1048576; // ~1Mb
        //private const int cacheDefaultSize = 1310720; // ~10MB
        //private const int cacheDefaultSize = 32768; // ~32MB
        //private const int cacheDefaultSize = 65536; // ~64MB

        // size is in BitString units i.e. for strings of
        // bits its in bits, for strings of floats (fuzzy bit strings)
        // it is in floats
        private const ulong cacheDefaultSize = 137438953472; // ~16GB

        /// <summary>
        /// Dictionary of bit string generators that are present in the bit string.
        /// Key to the bit string is attributeGuid.
        /// </summary>
        private Dictionary<string, BitStringGeneratorPrx> _bitStringGenerators =
            new Dictionary<string, BitStringGeneratorPrx>();

        /// <summary>
        /// Cache of missing information bit strings IDs.
        /// </summary>
        private MissingInformationBitStringsIdsCache _missingInformationBitStringsIdsCache;

        #endregion

        #region Constructors

        /// <summary>
        /// Explicit static constructor to tell C# compiler
        /// not to mark type as beforefieldinit
        /// </summary>
        static BitStringCache()
        {
        }

        /// <summary>
        /// Initializes an empty cache. Cache has a default size 1MB.
        /// </summary>
        private BitStringCache(ulong maxItems)
            : base(maxItems)
        {
            _missingInformationBitStringsIdsCache = new MissingInformationBitStringsIdsCache(this);
        }

        #endregion

        internal static IBitStringCache GetInstance()
        {
            return _instance;
            // !!! only for constructor of MissingInformation
            // (MI class is initialized before cache is fisrttime
            // touched by MI instance is firstime used after some
            // usage the cache (rightway inicialization of cache))
        }

        /// <summary>
        /// Gets the (singleton) instance of the bit string cache. If the
        /// instance does not contain bit string generator specified by parameter
        /// <paramref name="prx"/>, the generator is added to the cache.
        /// </summary>
        /// <param name="prx">Proxy of the bit string generator</param>
        /// <returns>Instance of the bit string cache</returns>
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

        /// <summary>
        /// Returns a bit string generator proxy with the attribute identification
        /// specified by <paramref name="attributeGuid"/>. 
        /// </summary>
        /// <param name="attributeGuid">Identifier of the attribute</param>
        /// <returns>Bit string generator proxy</returns>
        public BitStringGeneratorPrx GetBitStringGeneratorPrx(string attributeGuid)
        {
            BitStringGeneratorPrx prx;
            if (_bitStringGenerators.TryGetValue(attributeGuid, out prx))
            {
                return prx;
            }
            else
                //   throw new ArgumentException(
                //       "There is not reference to bit string generator (proxy) in bit string cache.");
                return null;
        }

        #region IBitStringCache Members

        /// <summary>
        /// Allows to obtain bit string identified by its guid.
        /// Internally distinguish between two possible cases.
        /// First case - bit string is stored in cache, leads to simple handling. Stored bit string is returned to the caller.
        /// Second case - bit string is not found in cache, leads to request to data preprocesor. Obtained string is first stored in cache and then returned to the caller.
        /// Cache is transparent for any exceptions from data preprocesor.
        /// </summary>
        /// <returns>The Bit string with the specified attribute GUID (string) and category ID (i.e. name) (string).</returns>
        public Task<IBitString> GetValueAsync(string attributeGuid, string categoryId)
        {
            return GetValueAsync(new BitStringIdentifier(attributeGuid, categoryId));
        }

        /// <summary>
        /// Gets bit string representing the missing information bits for the 
        /// attribute in parameter
        /// </summary>
        /// <param name="attributeGuid">Identification of the attribute</param>
        /// <returns>Bit string of missing information</returns>
        public async Task<IBitString> GetMissingInformationBitStringAsync(string attributeGuid)
        {
            string categoryId = await _missingInformationBitStringsIdsCache.GetValueAsync(attributeGuid).ConfigureAwait(false);
            if (categoryId != null)
            {
                return await base.GetValueAsync(new BitStringIdentifier(attributeGuid, categoryId)).ConfigureAwait(false);
            }
            else
            {
                return FalseBitString.GetInstance();
            }
        }

        /// <summary>
        /// Gets categories of a specified attribute
        /// </summary>
        /// <param name="attributeGuid">Attribute identification</param>
        /// <returns>Categories of an attribute</returns>
        public string[] GetCategoriesIds(string attributeGuid)
        {
#if Testing
            return BitStringCacheTest.GetCategoriesIds(attributeGuid);
#else
            return GetBitStringGeneratorPrx(attributeGuid).GetCategoriesIds();
#endif
        }

        #endregion

        private static long _iceTicks;
        /// <summary>
        /// Ice ticks
        /// </summary>
        public static long IceTicks
        {
            get { return _iceTicks; }
            set { _iceTicks = value; }
        }

        private static long _iceCalls;
        /// <summary>
        /// Ice calls
        /// </summary>
        public static long IceCalls
        {
            get { return _iceCalls; }
            set { _iceCalls = value; }
        }

        /// <summary>
        /// Gets the bit string of specified <c>key</c>. From the functionality of
        /// the MostRecentlyUsed cache, this method is invoked only when the value
        /// specified by <paramref name="key"/> is not present in the cache. 
        /// </summary>
        /// <param name="key">Bit string identifier</param>
        /// <returns></returns>
        public override async Task<IBitString> GetValueExternalAsync(BitStringIdentifier key)
        {
            long before = DateTime.Now.Ticks;
            _iceCalls++;
            
            BitStringIce bs = await GetBitStringGeneratorPrx(key.AttributeGuid).GetBitStringAsync(key.CategoryId).ConfigureAwait(false);
            
            _iceTicks += DateTime.Now.Ticks - before;
            if (bs is CrispBitStringIce)
            {
                CrispBitStringIce crisp = bs as CrispBitStringIce;
                return new BitString(key, crisp.length, crisp.value);
            }
            else
            {
                FuzzyBitStringIce fuzzy = bs as FuzzyBitStringIce;
                return new FuzzyBitString(key, fuzzy.value, false);
            }
        }

        /// <summary>
        /// Gets size of a bitstring
        /// </summary>
        /// <param name="itemToMeasure">Bit string to be measured</param>
        /// <returns>Size of a bitstring</returns>
        public override ulong GetSize(IBitString itemToMeasure)
        {
            return (ulong) itemToMeasure.Length;
        }
    }
}
