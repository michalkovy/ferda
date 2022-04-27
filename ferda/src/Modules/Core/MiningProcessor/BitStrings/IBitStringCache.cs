// IBitStringCache.cs - Bit string cache
//
// Authors: Tomáš Kuchař <tomas.kuchar@gmail.com>
//          Michal Kováč <michal.kovac.develop@centrum.cz>         
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

namespace Ferda.Guha.MiningProcessor.BitStrings
{
    /// <summary>
    /// Gets bit strings by the identifier form the cache (cache i.e. bitStringCollection)
    /// or from corresponding generator (BitStringGeneratorPrxSeq)
    /// </summary>
    public interface IBitStringCache
    {
        //static IBitStringCache GetInstance(BitStringGeneratorPrx prx);

        /// <summary>
        /// Gets or the count of bit strings in the cache.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets or the actual cache size.
        /// </summary>
        ulong ActSize { get; }

        /// <summary>
        /// Gets or sets the maximum cache size.
        /// </summary>
        ulong MaxSize { get; set; }

        /// <summary>
        /// Allows to check whether the cache contains some concrete bit string (identified by guid)
        /// </summary>
        /// <param name="bitStringId">Bit string identification</param>
        /// <returns>True if cache contains specified bit string, otherwise false</returns>
        bool Contains(BitStringIdentifier bitStringId);

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
        Task<IBitString> GetValueAsync(BitStringIdentifier bitStringId);

        /// <summary>
        /// Allows to obtain bit string identified by its guid.
        /// Internally distinguish between two possible cases.
        /// First case - bit string is stored in cache, leads to simple handling. Stored bit string is returned to the caller.
        /// Second case - bit string is not found in cache, leads to request to data preprocesor. Obtained string is first stored in cache and then returned to the caller.
        /// Cache is transparent for any exceptions from data preprocesor.
        /// </summary>
        /// <value>The Bit string with the specified attribute GUID (string) and category ID (i.e. name) (string).</value>
        Task<IBitString> GetValueAsync(string attributeGuid, string categoryId);

        /// <summary>
        /// Gets bit string representing the missing information bits for the 
        /// attribute in parameter
        /// </summary>
        /// <param name="attributeGuid">Identification of the attribute</param>
        /// <returns>Bit string of missing information</returns>
        Task<IBitString> GetMissingInformationBitStringAsync(string attributeGuid);

        /// <summary>
        /// Gets categories of a specified attribute
        /// </summary>
        /// <param name="attributeGuid">Attribute identification</param>
        /// <returns>Categories of an attribute</returns>
        string[] GetCategoriesIds(string attributeGuid);
    }
}