//#define Testing
using System;
using System.Collections.Generic;

namespace Ferda.Guha.MiningProcessor.Formulas
{
    public static class AttributeNameInLiteralsProvider
    {
        private static Dictionary<string, string> _cache = new Dictionary<string, string>();

        public static void Init(AttributeNameProviderPrx attributeNameProvider)
        {
            GuidAttributeNamePair[] names = attributeNameProvider.GetAttributeNames();
            foreach (GuidAttributeNamePair pair in names)
            {
                _cache[pair.id.value] = pair.attributeName;
            }
        }

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