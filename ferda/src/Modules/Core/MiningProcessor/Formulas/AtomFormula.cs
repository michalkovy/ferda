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
            return WriteAtom(
                _bitStringIdentifier.AttributeId,
                new string[] { _bitStringIdentifier.CategoryId }
                );
        }

        public static string WriteAtom(Guid attribute, IEnumerable<string> categories)
        {
            return AttributeNameInLiteralsProvider.GetAttributeNameInLiterals(attribute)
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