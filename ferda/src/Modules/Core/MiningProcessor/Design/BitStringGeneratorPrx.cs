using System;
using Ferda.Guha.Data;
using Ferda.Guha.MiningProcessor.BitStrings;

namespace Design
{
    public class BitStringGeneratorPrx
    {
        public Guid AttributeGuid
        {
            get { throw new NotImplementedException(); }
        }

        public CardinalityEnum AttributeCardinality
        {
            get { throw new NotImplementedException(); }
        }

        public string[] GetCategoriesIds()
        {
            throw new NotImplementedException();
        }

        public IBitString GetBitString(string categoryId)
        {
            throw new NotImplementedException();
        }

        public string GetMissingInformationCategoryId()
        {
            throw new NotImplementedException();
        }
    }
}