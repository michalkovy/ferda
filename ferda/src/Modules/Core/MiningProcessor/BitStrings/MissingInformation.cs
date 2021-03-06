// MissingInformation.cs - cache for missing information bit strings
//
// Authors: Tomáš Kuchař <tomas.kuchar@gmail.com>
// Commented by: Martin Ralbovský <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2006 Tomáš Kuchař
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Ferda.Modules.Helpers.Caching;
using Ferda.Modules.Helpers.Common;

namespace Ferda.Guha.MiningProcessor.BitStrings
{
    /// <summary>
    /// The class represents missing information bit string cache. It uses
    /// the MostRecentlyUsed cache as a basis. The values of the cache are
    /// bit strings representing missing information of individual attributes.
    /// The keys are sets of identificators of attributes. 
    /// </summary>
    public class MissingInformation : MostRecentlyUsed<Set<string>, IBitString>
    {
        /// <summary>
        /// The one bit string cache in the mining processor
        /// </summary>
        private static IBitStringCache _bitStringCache;
        /// <summary>
        /// Instance of the class
        /// </summary>
		private static readonly MissingInformation _instance = new MissingInformation();

        /// <summary>
        /// Default size of the most recently used cache.
        /// </summary>
        public const int cacheDefaultSize = 1048576; // ~1Mb

        /// <summary>
        /// Default constructor for the class. The constructor is private, because
        /// only way how to use the class should be through the
        /// <see cref="Ferda.Guha.MiningProcessor.BitStrings.MissingInformation.GetInstance()"/>
        /// method. 
        /// </summary>
        private MissingInformation()
            : base(cacheDefaultSize)
        {
            _bitStringCache = BitStringCache.GetInstance();
        }

        /// <summary>
        /// Gets the value of missing information bit string from a specified
        /// key. The key is a set containing attribute identificators. The resulting
        /// bit string is a OR of all the missing information bit strings from the 
        /// individual attribute identificators. 
        /// The value is retrieved from external sources (not from the cache). For using
        /// internal sources (cache itself), please use the <c>this[Set key]</c>
        /// method instead. 
        /// </summary>
        /// <param name="key">The key (set containing attribute identificators)</param>
        /// <returns>A bit string containing missing information</returns>
        /// <remarks>
        /// The method first searches for best matching subset. If there is a best
        /// matching subset, the method retrieves the missing information bitstrings
        /// that are stored in the cache. Otherwise it just takes the missing information
        /// bitstrings from the attributes. 
        /// </remarks>
        public override async Task<IBitString> GetValueExternalAsync(Set<string> key)
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
                        //nechapu proc by se nemohly rovnat
                        if (bestMatch.Count == key.Count)
                            break;
                    }
                }
            }

            if (bestMatch != null)
            {
                //there is a subset that matched with the key.
                IBitString last = await this.GetValueAsync(bestMatch).ConfigureAwait(false);
                IBitString newCached = null;
                foreach (string guid in key)
                {
                    if (!bestMatch.Contains(guid))
                    {
                        IBitString newBitString = await _bitStringCache.GetMissingInformationBitStringAsync(guid).ConfigureAwait(false);
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
                //everything needs to be retrieved from bitstring generators (attributes)
                IBitString newCached = null;
                foreach (string guid in key)
                {
                    IBitString newBitString = await _bitStringCache.GetMissingInformationBitStringAsync(guid).ConfigureAwait(false);
                    if (newCached == null)
                        newCached = newBitString;
                    else
                        newCached = newCached.Or(newBitString);
                }
                return newCached;
            }
        }

        /// <summary>
        /// Gets the size of the bit string in the parameter
        /// <paramref name="itemToMeasure"/>.
        /// </summary>
        /// <param name="itemToMeasure">The item to measure.</param>
        /// <returns>Size of a bit string</returns>
        public override ulong GetSize(IBitString itemToMeasure)
        {
            return (ulong) itemToMeasure.Length;
        }
		
        /// <summary>
        /// Gets a instance of the class. This should be the only way how to
        /// access the class. 
        /// </summary>
        /// <returns></returns>
		public static MissingInformation GetInstance()
        {
            return _instance;
		}
    }
}
