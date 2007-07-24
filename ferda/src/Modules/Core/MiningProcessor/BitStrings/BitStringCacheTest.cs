// BitStringCacheTest.cs - Test of bit string cache
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

#if Testing

using System;
using System.Collections.Generic;
using Ferda.Guha.MiningProcessor.BitStrings;

namespace Ferda.Guha.MiningProcessor.BitStrings
{
    /// <summary>
    /// Test of bit string cache
    /// </summary>
    public static class BitStringCacheTest
    {
        private static Dictionary<string, Dictionary<string, string>> _bitStringsSetting;
        private static Dictionary<string, Dictionary<string, IBitString>> _bitStrings;

        public const string Attr1Id = "CA761232-ED42-11CE-BACD-00AA0057B221";
        public const string Attr2Id = "CA761232-ED42-11CE-BACD-00AA0057B222";
        public const string Attr3Id = "CA761232-ED42-11CE-BACD-00AA0057B223";
        public const string Attr4Id = "CA761232-ED42-11CE-BACD-00AA0057B224";

        /// <summary>
        /// Initializes the <see cref="T:BitStringCacheTest"/> class.
        /// </summary>
        static BitStringCacheTest()
        {
            _bitStringsSetting = new Dictionary<string, Dictionary<string, string>>();

            string attr1Name = Attr1Id;
            _bitStringsSetting.Add(attr1Name, new Dictionary<string, string>());
            _bitStringsSetting[attr1Name].Add("A", "0000000000000001");
            _bitStringsSetting[attr1Name].Add("B", "0000000000000010");
            _bitStringsSetting[attr1Name].Add("C", "0000000000000100");
            _bitStringsSetting[attr1Name].Add("D", "0000000000001000");
            _bitStringsSetting[attr1Name].Add("E", "0000000000010000");
            _bitStringsSetting[attr1Name].Add("F", "0000000000100000");
            _bitStringsSetting[attr1Name].Add("G", "0000000001000000");
            _bitStringsSetting[attr1Name].Add("H", "0000000010000000");
            _bitStringsSetting[attr1Name].Add("I", "0000000100000000");
            _bitStringsSetting[attr1Name].Add("J", "0000001000000000");
            _bitStringsSetting[attr1Name].Add("K", "0000010000000000");
            _bitStringsSetting[attr1Name].Add("L", "0000100000000000");
            _bitStringsSetting[attr1Name].Add("M", "0001000000000000");
            _bitStringsSetting[attr1Name].Add("N", "0010000000000000");
            _bitStringsSetting[attr1Name].Add("O", "0100000000000000");
            _bitStringsSetting[attr1Name].Add("P", "1000000000000000");

            string attr2Name = Attr2Id;
            _bitStringsSetting.Add(attr2Name, new Dictionary<string, string>());
            _bitStringsSetting[attr2Name].Add("A", "00001");

            string attr3Name = Attr3Id;
            _bitStringsSetting.Add(attr3Name, new Dictionary<string, string>());
            _bitStringsSetting[attr3Name].Add("1", "0000000000000001");
            _bitStringsSetting[attr3Name].Add("2", "0000000000000010");
            _bitStringsSetting[attr3Name].Add("3", "0000000000000100");
            _bitStringsSetting[attr3Name].Add("4", "0000000000001000");
            _bitStringsSetting[attr3Name].Add("5", "0000000000010000");
            _bitStringsSetting[attr3Name].Add("6", "0000000000100000");
            _bitStringsSetting[attr3Name].Add("7", "0000000001000000");
            _bitStringsSetting[attr3Name].Add("8", "0000000010000000");

            string attr4Name = Attr4Id;
            _bitStringsSetting.Add(attr4Name, new Dictionary<string, string>());
            _bitStringsSetting[attr4Name].Add("alpha", "0000000000000001");
            _bitStringsSetting[attr4Name].Add("beta", "0000000000000100");
            _bitStringsSetting[attr4Name].Add("gamma", "0000000000010000");
            _bitStringsSetting[attr4Name].Add("delta", "0000000001000000");
            _bitStringsSetting[attr4Name].Add("epsilon", "0000000100000000");
            _bitStringsSetting[attr4Name].Add("zeta", "0000010000000000");
            _bitStringsSetting[attr4Name].Add("eta", "0001000000000000");
            _bitStringsSetting[attr4Name].Add("theta", "0100000000000000");
            
            _bitStrings = new Dictionary<string, Dictionary<string, IBitString>>();
            foreach (KeyValuePair<string, Dictionary<string, string>> pair in _bitStringsSetting)
            {
                _bitStrings.Add(pair.Key, new Dictionary<string, IBitString>());
                foreach (KeyValuePair<string, string> valuePair in pair.Value)
                {
                    _bitStrings[pair.Key].Add(
                        valuePair.Key, 
                        new BitString(
                            valuePair.Value, 
                            new Formulas.AtomFormula(
                                new BitStringIdentifier(
                                    pair.Key, 
                                    valuePair.Key
                                    )
                                )
                            )
                        );
                }
            }
        }

        /// <summary>
        /// Gets the bit string
        /// </summary>
        /// <param name="bitStringId">Identifier of the bit string</param>
        /// <returns>Bit string</returns>
        public static IBitString GetBitString(BitStringIdentifier bitStringId)
        {
            return _bitStrings[bitStringId.AttributeGuid][bitStringId.CategoryId];
        }

        /// <summary>
        /// Gets categories of a specified attribute
        /// </summary>
        /// <param name="attributeGuid">Attribute identification</param>
        /// <returns>Categories of an attribute</returns>
        public static string[] GetCategoriesIds(string attributeGuid)
        {
            List<string> result = new List<string>(_bitStrings[attributeGuid].Keys);
            return result.ToArray();
        }
    }
}

#endif