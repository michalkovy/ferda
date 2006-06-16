#define Testing
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
                return "Attribute";
#else
                return attributeId.ToString();
#endif
        }
    }
}
