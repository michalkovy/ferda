using System;
using Ferda.Guha.Data;
using Ferda.Guha.MiningProcessor.BitStrings;
using Ferda.Guha.MiningProcessor.Formulas;

namespace Ferda.Guha.MiningProcessor.Generation
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

        private bool _supportsNumericValues = false;
        private bool _supportsNumericValuesInitialized = false;
        public bool SupportsNumericValues
        {
            get
            {
                if (!_supportsNumericValuesInitialized)
                {
                    double[] numericValues = _generator.GetCategoriesNumericValues();
                    _supportsNumericValues = !(numericValues == null || numericValues.Length == 0);
                }
                return _supportsNumericValues;
            }
        }

        private CardinalityEnum _attributeCardinality = CardinalityEnum.Nominal;
        private bool _attributeCardinalityInitialized = false;
        public CardinalityEnum AttributeCardinality
        {
            get
            {
                if (!_attributeCardinalityInitialized)
                {
                    _attributeCardinality = _generator.GetAttributeCardinality();
                }
                return _attributeCardinality;
            }
        }

        private readonly BitStringGeneratorPrx _generator;
        
        public CategorialAttributeTrace(BitStringGeneratorPrx generator)
        {
            if (generator == null)
                throw new ArgumentNullException("generator");
            _generator = generator;
            _identifier = new CategorialAttributeFormula(generator.GetAttributeId().value);
            _bitStrings = Helpers.GetBitStrings(generator, _identifier.AttributeGuid);
        }
    }
}