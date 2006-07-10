using System;
using Ferda.Guha.MiningProcessor.Formulas;

namespace Ferda.Guha.MiningProcessor.BitStrings
{
    public class EmptyBitString : IEmptyBitString
    {
        #region Singleton

        private static readonly EmptyBitString _instance = new EmptyBitString();
        private static readonly object padlock = new object();

        static EmptyBitString()
        {
        }

        public static EmptyBitString GetInstance()
        {
            lock (padlock)
            {
                return _instance;
            }
        }

        #endregion

        public static readonly BitStringIdentifier EmptyBitStringIdentifier =
            new BitStringIdentifier(null, "Empty BitString Category");

        public static readonly BooleanAttributeFormula EmptyBitStringId = new AtomFormula(EmptyBitStringIdentifier);

        private EmptyBitString()
        {
        }

        #region IBitString Members

        public BooleanAttributeFormula Identifier
        {
            get { return EmptyBitStringId; }
        }

        public IBitString And(IBitString source)
        {
            BitString bs = source as BitString;
            if (bs != null)
            {
                return bs.And(this);
            }

            EmptyBitString ebs = source as EmptyBitString;
            if (ebs != null)
            {
                //EmptyBitString other = (EmptyBitString)source;
                //if (other.Length != Length)
                //    throw Exceptions.BitStringsLengtsAreNotEqualError();
                return this;
            }

            throw new NotImplementedException();
        }

        public IBitString Not()
        {
            return this;
        }

        public IBitString Or(IBitString source)
        {
            BitString bs = source as BitString;
            if (bs != null)
            {
                return bs.Or(this);
            }

            EmptyBitString ebs = source as EmptyBitString;
            if (ebs != null)
            {
                //EmptyBitString other = (EmptyBitString)source;
                //if (other.Length != Length)
                //    throw Exceptions.BitStringsLengtsAreNotEqualError();
                return this;
            }

            throw new NotImplementedException();
        }

        public int Sum
        {
            get { return Int32.MinValue; }
        }

        #endregion

        #region IBitStringBase Members

        public int Length
        {
            get { return 1; }
        }

        #endregion
    }
}