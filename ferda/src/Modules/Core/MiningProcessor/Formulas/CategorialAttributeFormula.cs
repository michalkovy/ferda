using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Guha.MiningProcessor.Formulas
{
    public class CategorialAttributeFormula : Formula
    {
        private Guid _attributeId;
        
        public Guid AttributeId
        {
            get { return _attributeId; }
            set { _attributeId = value; }
        }

        public CategorialAttributeFormula()
        {
        }
        
        public CategorialAttributeFormula(Guid attributeId)
        {
            _attributeId = attributeId;
        }

        public override string ToString()
        {
            return AttributeNameInLiteralsProvider.GetAttributeNameInLiterals(_attributeId);
        }
    }
}
