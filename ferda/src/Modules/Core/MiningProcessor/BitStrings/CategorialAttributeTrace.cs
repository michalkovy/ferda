using System;
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
            if (generator == null)
                throw new ArgumentNullException("generator");
            _identifier = new CategorialAttributeFormula(generator.GetAttributeId().value);
            _bitStrings = Helpers.GetBitStrings(generator, _identifier.AttributeGuid);
        }
    }
}