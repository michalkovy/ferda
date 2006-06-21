using System;
using System.Collections.Generic;
using Ferda.Guha.MiningProcessor.BitStrings;

namespace Ferda.Guha.MiningProcessor.Formulas
{
    public class Atom : IFormula
    {
        private readonly BitStringIdentifier _bitStringIdentifier;

        public BitStringIdentifier BitStringIdentifier
        {
            get { return _bitStringIdentifier; }
        }

        public Atom(BitStringIdentifier bitStringIdentifier)
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
                   + Formula.SequenceToString(
                         categories, 
                         FormulaSeparator.AtomMembers,
                         false
                         ) 
                   + ")";
        }
    }
}