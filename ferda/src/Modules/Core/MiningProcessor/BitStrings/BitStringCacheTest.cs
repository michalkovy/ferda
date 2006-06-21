#define Testing
#if Testing

using System;
using System.Collections.Generic;
using Ferda.Guha.MiningProcessor.BitStrings;

namespace Ferda.Guha.MiningProcessor.BitStrings
{
    public static class BitStringCacheTest
    {
        private static Dictionary<Guid, Dictionary<string, string>> _bitStringsSetting;
        private static Dictionary<Guid, Dictionary<string, IBitString>> _bitStrings;

        public const string Attr1Id = "CA761232-ED42-11CE-BACD-00AA0057B223";
        public const string Attr2Id = "CA761232-ED42-11CE-BACD-00AA0057B224";
        public const string Attr3Id = "CA761232-ED42-11CE-BACD-00AA0057B225";
        
        static BitStringCacheTest()
        {
            _bitStringsSetting = new Dictionary<Guid, Dictionary<string, string>>();

            Guid attr1Name = new Guid(Attr1Id);
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

            Guid attr2Name = new Guid(Attr2Id);
            _bitStringsSetting.Add(attr2Name, new Dictionary<string, string>());
            _bitStringsSetting[attr2Name].Add("A", "00001");

            Guid attr3Name = new Guid(Attr3Id);
            _bitStringsSetting.Add(attr3Name, new Dictionary<string, string>());
            _bitStringsSetting[attr3Name].Add("1", "0000000000000001");
            _bitStringsSetting[attr3Name].Add("2", "0000000000000010");
            _bitStringsSetting[attr3Name].Add("3", "0000000000000100");
            _bitStringsSetting[attr3Name].Add("4", "0000000000001000");
            _bitStringsSetting[attr3Name].Add("5", "0000000000010000");
            _bitStringsSetting[attr3Name].Add("6", "0000000000100000");
            _bitStringsSetting[attr3Name].Add("7", "0000000001000000");
            _bitStringsSetting[attr3Name].Add("8", "0000000010000000");

            _bitStrings = new Dictionary<Guid, Dictionary<string, IBitString>>();
            foreach (KeyValuePair<Guid, Dictionary<string, string>> pair in _bitStringsSetting)
            {
                _bitStrings.Add(pair.Key, new Dictionary<string, IBitString>());
                foreach (KeyValuePair<string, string> valuePair in pair.Value)
                {
                    _bitStrings[pair.Key].Add(
                        valuePair.Key, 
                        new BitString(
                            valuePair.Value, 
                            new Formulas.Atom(
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

        public static IBitString GetBitString(BitStringIdentifier bitStringId)
        {
            return _bitStrings[bitStringId.AttributeId][bitStringId.CategoryId];
        }

        public static string[] GetCategoriesIds(Guid attributeId)
        {
            List<string> result = new List<string>(_bitStrings[attributeId].Keys);
            return result.ToArray();
        }
    }
}

#endif