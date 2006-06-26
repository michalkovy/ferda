using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Guha.MiningProcessor.Formulas;
using Ferda.Guha.MiningProcessor.Generation;

namespace Ferda.Guha.MiningProcessor.BitStrings
{
    public class CategorialAttributeTrace
    {
        private readonly CategorialAttributeFormula _identifier;
        public CategorialAttributeFormula Identifier
        {
            get { return _identifier; }
        }
        
        private readonly IBitString[] _bitStrings;
        public IBitString[] BitStrings
        {
            get { return _bitStrings; }
        }
        
        public CategorialAttributeTrace(BitStringGeneratorPrx generator)
        {
            _identifier = new CategorialAttributeFormula(new Guid(generator.GetAttributeId().value));
            _bitStrings = Helpers.GetBitStrings(generator, _identifier.AttributeId);
        }
    }
}
