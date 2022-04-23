// Helpers.cs - helping methods for the bit string retrieval
//
// Authors: Tomáš Kuchaø <tomas.kuchar@gmail.com>      
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

using System;
using Ferda.Guha.MiningProcessor.BitStrings;

namespace Ferda.Guha.MiningProcessor.Generation
{
    /// <summary>
    /// Possible bitwise operations (now only And and Or)
    /// </summary>
    public enum BitwiseOperation
    {
        And,
        Or
    }

    /// <summary>
    /// Helping methods for the bit string retrieval.
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Retrieves a bit string from a given bit string generator, given attribute
        /// and given categories. The method takes all the bit strings from the attribute
        /// determined by caterogies and binds them with the specific operation. 
        /// </summary>
        /// <param name="bitStringGeneratorPrx">Bit string generator</param>
        /// <param name="attributeGuid">Identification of the attribute</param>
        /// <param name="categoriesIds">Which categories are to be taken</param>
        /// <param name="operation">Operation that binds the categories</param>
        /// <returns>Resulting bit string</returns>
        public static async Task<IBitString> GetBitStringAsync(BitStringGeneratorPrx bitStringGeneratorPrx, string attributeGuid,
                                              string[] categoriesIds, BitwiseOperation operation)
        {
            IBitString resultUsed = null;
            IBitString resultTemp = null; // can be used for in place operations
            IBitStringCache cache = BitStringCache.GetInstance(bitStringGeneratorPrx);
            switch (operation)
            {
                case BitwiseOperation.And:
                    foreach (string var in categoriesIds)
                    {
                        if (resultTemp == null)
                        {
                            if (resultUsed == null)
                                resultUsed = await cache.GetValueAsync(attributeGuid, var).ConfigureAwait(false);
                            else
                                resultTemp = resultUsed.And(await cache.GetValueAsync(attributeGuid, var).ConfigureAwait(false));
                        }
                        else
                            resultTemp = resultTemp.AndInPlace(await cache.GetValueAsync(attributeGuid, var).ConfigureAwait(false));
                    }
                    return resultTemp ?? resultUsed;
                case BitwiseOperation.Or:
                    foreach (string var in categoriesIds)
                    {
                        if (resultTemp == null)
                        {
                            if (resultUsed == null)
                                resultUsed = await cache.GetValueAsync(attributeGuid, var).ConfigureAwait(false);
                            else
                                resultTemp = resultUsed.Or(await cache.GetValueAsync(attributeGuid, var).ConfigureAwait(false));
                        }
                        else
                            resultTemp = resultTemp.OrInPlace(await cache.GetValueAsync(attributeGuid, var).ConfigureAwait(false));
                    }
                    return resultTemp ?? resultUsed;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Retrieves bit strings for a given bit string generator and given
        /// attribute. It uses the bit string cache, see
        /// <see cref="Ferda.Guha.MiningProcessor.BitStrings.BitStringCache"/>.
        /// </summary>
        /// <param name="bitStringGeneratorPrx">Bit string generator</param>
        /// <param name="attributeGuid">Identification of the attribute</param>
        /// <returns>Bits strings for given attribute</returns>
        public static async Task<IBitString[]> GetBitStringsAsync(BitStringGeneratorPrx bitStringGeneratorPrx, string attributeGuid)
        {
            IBitStringCache cache = BitStringCache.GetInstance(bitStringGeneratorPrx);
            string[] categoriesIds = cache.GetCategoriesIds(attributeGuid);
            IBitString[] result = new IBitString[categoriesIds.Length];
            for (int i = 0; i < categoriesIds.Length; i++)
            {
                result[i] = await cache.GetValueAsync(attributeGuid, categoriesIds[i]).ConfigureAwait(false);
            }
            return result;
        }
    }
}