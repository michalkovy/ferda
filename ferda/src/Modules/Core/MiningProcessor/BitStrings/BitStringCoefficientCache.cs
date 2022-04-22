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
    public class BitStringCoefficientCache : MostRecentlyUsed<KeyValuePair<string,HashSet<string>>, IBitString>
    {
        public class CustomEqualityComparer : IEqualityComparer<KeyValuePair<string, HashSet<string>>>
        {
            public bool Equals(KeyValuePair<string, HashSet<string>> x, KeyValuePair<string, HashSet<string>> y)
            {
                if (!StringComparer.InvariantCulture.Equals(x.Key,y.Key))
                    return false;

                if (ReferenceEquals(x.Value,y.Value))
                    return true;

                if (ReferenceEquals(x.Value, null))
                    return false;

                return x.Value.SetEquals(y.Value);
            }

            public int GetHashCode(KeyValuePair<string, HashSet<string>> keyValue)
            {
                int hashCode = StringComparer.InvariantCulture.GetHashCode(keyValue.Key);


                if (keyValue.Value != null)
                {
                    hashCode ^= keyValue.Value.GetHashCode() & 0x7FFFFFFF;
                    //    foreach (var t in keyValue.Value)
                    //    {
                    //        hashCode ^= StringComparer.InvariantCulture.GetHashCode(t) & 0x7FFFFFFF;
                    //    }
                }

                return hashCode;
            }
        }

        #region Private fields

        /// <summary>
        /// The singleton (one) object that can be created
        /// </summary>
        private static readonly BitStringCoefficientCache _instance = new BitStringCoefficientCache(cacheDefaultSize);
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
        private const ulong cacheDefaultSize = 137438953472;//137438953472; // ~16GB

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
        static BitStringCoefficientCache()
        {
        }

        /// <summary>
        /// Initializes an empty cache. Cache has a default size 1MB.
        /// </summary>
        private BitStringCoefficientCache(ulong maxItems)
            : base(maxItems, new CustomEqualityComparer())
        {
        }

        #endregion

        public static BitStringCoefficientCache GetInstance()
        {
            return _instance;
        }

        #region IBitStringCache Members

        /// <summary>
        /// Allows to obtain bit string identified by its guid.
        /// Internally distinguish between two possible cases.
        /// First case - bit string is stored in cache, leads to simple handling. Stored bit string is returned to the caller.
        /// Second case - bit string is not found in cache, leads to request to data preprocesor. Obtained string is first stored in cache and then returned to the caller.
        /// Cache is transparent for any exceptions from data preprocesor.
        /// </summary>
        /// <value>
        /// Bit string responding to given guid that is stored in cache or obtained from data preprocesor.
        /// </value>
        public new IBitString this[KeyValuePair<string, HashSet<string>> bitStringIdSet]
        {
            get
            {
#if Testing
                return BitStringCacheTest.GetBitString(bitStringId);
#else
                return base[bitStringIdSet];
#endif
            }
        }
        #endregion


        /// <summary>
        /// Gets the bit string of specified <c>key</c>. From the functionality of
        /// the MostRecentlyUsed cache, this method is invoked only when the value
        /// specified by <paramref name="key"/> is not present in the cache. 
        /// </summary>
        /// <param name="key">Bit string identifier</param>
        /// <returns></returns>
        public override IBitString GetValue(KeyValuePair<string, HashSet<string>> key)
        {
            throw new NotImplementedException();
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
