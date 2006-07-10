using System;
using Ferda.Guha.MiningProcessor.Formulas;

namespace Ferda.Guha.MiningProcessor.BitStrings
{
    public class FalseBitString : IEmptyBitString
    {
        #region Singleton

        private static readonly FalseBitString _instance = new FalseBitString();
        private static readonly object padlock = new object();

        private FalseBitString()
        {
        }

        public static FalseBitString GetInstance()
        {
            lock (padlock)
            {
                return _instance;
            }
        }

        #endregion

        public static readonly BitStringIdentifier FalseBitStringIdentifier =
            new BitStringIdentifier(null, "False BitString");

        public static readonly BooleanAttributeFormula FalseBitStringId = new AtomFormula(FalseBitStringIdentifier);

        #region IBitString Members

        public BooleanAttributeFormula Identifier
        {
            get { return FalseBitStringId; }
        }

        public IBitString And(IBitString source)
        {
            return this;
        }

        public IBitString Not()
        {
            throw new NotSupportedException("Negation of false bit string is not supported.");
        }

        public IBitString Or(IBitString source)
        {
            return source;
        }

        public int Sum
        {
            get { return 0; }
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