// AttributeNameInLiteralsProvider.cs - cache of the attribute names in literals
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

namespace Ferda.Guha.MiningProcessor.Formulas
{
    /// <summary>
    /// The class represents a simple cache for one 
    /// <see cref="Ferda.Guha.MiningProcessor.AttributeNameProviderPrx"/>.
    /// This way, there is no need to ask the Ice layer once again for the
    /// names of attributes.
    /// </summary>
    public static class AttributeNameInLiteralsProvider
    {
        /// <summary>
        /// The cache
        /// </summary>
        private static Dictionary<string, string> _cache = new Dictionary<string, string>();

        /// <summary>
        /// Initialization of the cache (always has to be done before using).
        /// </summary>
        /// <param name="attributeNameProvider"></param>
        public static void Init(AttributeNameProviderPrx attributeNameProvider)
        {
            GuidAttributeNamePair[] names = attributeNameProvider.GetAttributeNames();
            foreach (GuidAttributeNamePair pair in names)
            {
                _cache[pair.id.value] = pair.attributeName;
            }
        }

        /// <summary>
        /// Method for retrieving the actual names of attributes.
        /// </summary>
        /// <param name="attributeGuid">Identification (GUID) of the attribute</param>
        /// <returns>Actual attribute name</returns>
        public static string GetAttributeNameInLiterals(string attributeGuid)
        {
#if Testing
            string stringId = attributeGuid.ToString();
            return "Attribute" + stringId.Substring(stringId.Length - 1);
#else
            string result;
            if (!_cache.TryGetValue(attributeGuid, out result))
                throw new InvalidOperationException(
                    "AttributeNameProviderPrx has to be set before usage of this method.");
            else
                return _cache[attributeGuid];
#endif
        }
    }
}