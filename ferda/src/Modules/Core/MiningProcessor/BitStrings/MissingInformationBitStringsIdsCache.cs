// MissingInformationBitStringIdsCache.cs - cache for missing information bit
// string identifiers
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

using Ferda.Modules.Helpers.Caching;

namespace Ferda.Guha.MiningProcessor.BitStrings
{
    /// <summary>
    /// The class represents missing information bit string identificators cache.
    /// In other words, it is a cache that stores names of missing information
    /// categories of attributes. The values of the cache are these names, the 
    /// keys are identificators of attributes. 
    /// </summary>
    internal class MissingInformationBitStringsIdsCache : MostRecentlyUsed<string, string>
    {
        #region Fields

        /// <summary>
        /// Bit string cache of the mining processor
        /// </summary>
        private readonly BitStringCache _bitStringCache;

        /// <summary>
        /// Default size of the most recently used cache.
        /// </summary>
        private const int cacheDefaultSize = 32;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor for the class
        /// </summary>
        /// <param name="bitStringCache">The only one bit string cache</param>
        public MissingInformationBitStringsIdsCache(BitStringCache bitStringCache)
            : base(cacheDefaultSize)
        {
            _bitStringCache = bitStringCache;
        }

        #endregion

        #region MostRecentlyUsed overrides

        /// <summary>
        /// Gets the value of missing information category from a specified attribute.
        /// The value is retrieved from external sources (not from the cache). For using
        /// internal sources (cache itself), please use the <c>this[string key]</c>
        /// method instead. 
        /// </summary>
        /// <param name="key">The key represents identification of an attribute</param>
        /// <returns>String representing missing information category in the
        /// attribute.</returns>
        public override async Task<string> GetValueExternalAsync(string key)
        {
            BitStringGeneratorPrx proxy =
                _bitStringCache.GetBitStringGeneratorPrx(key);

            if (proxy != null)
            {
                string[] missingInformationCategoryId =
                    await _bitStringCache.GetBitStringGeneratorPrx(key).GetMissingInformationCategoryIdAsync().ConfigureAwait(false);
                if (missingInformationCategoryId.Length == 1)
                {
                    return missingInformationCategoryId[0];
                }
            }
            return null;
        }
        
        /// <summary>
        /// Gets the size of the item in the parameter. In this case, 
        /// the size is not interpretable. 
        /// <paramref name="itemToMeasure"/>.
        /// </summary>
        /// <param name="itemToMeasure">The item to measure.</param>
        /// <returns>Size of the item.</returns>
        public override ulong GetSize(string itemToMeasure)
        {
            return 1;
        }

        #endregion
    }
}
