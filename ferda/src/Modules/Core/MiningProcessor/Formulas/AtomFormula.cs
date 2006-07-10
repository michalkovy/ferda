using System;
using System.Collections.Generic;
using Ferda.Guha.MiningProcessor.BitStrings;

namespace Ferda.Guha.MiningProcessor.Formulas
{
    [Serializable()]
    public class AtomFormula : BooleanAttributeFormula
    {
        private readonly BitStringIdentifier _bitStringIdentifier;

        public BitStringIdentifier BitStringIdentifier
        {
            get { return _bitStringIdentifier; }
        }

        public AtomFormula(BitStringIdentifier bitStringIdentifier)
        {
            _bitStringIdentifier = bitStringIdentifier;
        }

        public override string ToString()
        {
            if (_bitStringIdentifier == EmptyBitString.EmptyBitStringIdentifier)
                return "";
            return WriteAtom(
                _bitStringIdentifier.AttributeGuid,
                new string[] {_bitStringIdentifier.CategoryId}
                );
        }

        public static string WriteAtom(string attributeGuid, IEnumerable<string> categories)
        {
            return AttributeNameInLiteralsProvider.GetAttributeNameInLiterals(attributeGuid)
                   + "("
                   + FormulaHelper.SequenceToString(
                         categories,
                         FormulaSeparator.AtomMembers,
                         false
                         )
                   + ")";
        }
    }
}