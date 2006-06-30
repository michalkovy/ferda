//#define Testing
using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Guha.MiningProcessor.Formulas
{
    public static class AttributeNameInLiteralsProvider
    {
        public static string GetAttributeNameInLiterals(Guid attributeId)
        {
#if Testing
            string stringId = attributeId.ToString();
            return "Attribute" + stringId.Substring(stringId.Length - 1);
#else
                return attributeId.ToString();
#endif
        }
    }
}
