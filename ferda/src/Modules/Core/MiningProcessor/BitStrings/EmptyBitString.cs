using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Ferda.Guha.MiningProcessor.Formulas;

namespace Ferda.Guha.MiningProcessor.BitStrings
{
    public static class EmptyBitStringSingleton
    {
        private static EmptyBitString _emptyBitString = new EmptyBitString();
        public static EmptyBitString EmptyBitString
        {
            get
            {
                return _emptyBitString;
            }
        }
    }

    public class EmptyBitString : IEmptyBitString
    {
        public static BooleanAttributeFormula EmptyBitStringId = new AtomFormula(new BitStringIdentifier(new Guid("Empty BitString"), "Empty BitString Category"));
        
        internal EmptyBitString()
        {}

        #region IBitString Members

        public BooleanAttributeFormula Identifier
        {
            get { return EmptyBitStringId; }
        }

        public IBitString And(IBitString source)
        {
            if (source is BitString)
            {
                return ((BitString)source).And(this);
            }
            else if (source is EmptyBitString)
            {
                EmptyBitString other = (EmptyBitString)source;
                if (other.Length != Length)
                    throw Exceptions.BitStringsLengtsAreNotEqualError();
                return this;
            }
            else
                throw new NotImplementedException();
        }

        public IBitString Not()
        {
            return this;
        }

        public IBitString Or(IBitString source)
        {
            if (source is BitString)
            {
                return ((BitString)source).Or(this);
            }
            else if (source is EmptyBitString)
            {
                EmptyBitString other = (EmptyBitString)source;
                if (other.Length != Length)
                    throw Exceptions.BitStringsLengtsAreNotEqualError();
                return this;
            }
            else
                throw new NotImplementedException();
        }

        public int Sum
        {
            get { return Length; }
        }

        #endregion

        #region IBitStringBase Members


        public int Length
        {
            get { return Int32.MinValue; }
        }

        #endregion
    }
}
