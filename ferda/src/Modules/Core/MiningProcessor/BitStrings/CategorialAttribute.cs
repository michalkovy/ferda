using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Guha.MiningProcessor.Generation;

namespace Ferda.Guha.MiningProcessor.BitStrings
{
    public class CategorialAttribute
    {
        private readonly Guid _attributeId;
        public Guid AttributeId
        {
            get { return _attributeId; }
        }
        
        private readonly IBitString[] _bitStrings;
        public IBitString[] BitStrings
        {
            get { return _bitStrings; }
        }
        
        public CategorialAttribute(BitStringGeneratorPrx generator)
        {
            _attributeId = new Guid(generator.GetAttributeId().value);
            _bitStrings = Helpers.GetBitStrings(generator, AttributeId);
        }
    }
}
