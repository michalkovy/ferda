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

        #region AND
        
        public IBitString andSpecial(IBitString source, bool cloned)
        {
            if (source is EmptyBitString)
                return this;
            else if (source is FalseBitString)
                return source;
            else
                throw new NotImplementedException();
        }

        public IBitString And(IBitString source)
        {
            BitString bs = source as BitString;
            if (bs != null)
                return bs.And(this);
            else
                return andSpecial(source, false);
        }

        //public IBitString AndCloned(IBitString source)
        //{
        //    BitString bs = source as BitString;
        //    if (bs != null)
        //        return bs.AndCloned(this);
        //    else
        //        return andSpecial(source, false);
        //} 
        
        #endregion

        #region NOT
        
        public IBitString Not()
        {
            return this;
        }

        public IBitString NotCloned()
        {
            return this;
        } 
        
        #endregion

        #region OR
        
        public IBitString orSpecial(IBitString source, bool cloned)
        {
            if (source is EmptyBitString)
                return this;
            else if (source is FalseBitString)
                return this;
            else
                throw new NotImplementedException();
        }

        public IBitString Or(IBitString source)
        {
            BitString bs = source as BitString;
            if (bs != null)
                return bs.Or(this);
            else
                return andSpecial(source, false);
        }

        //public IBitString OrCloned(IBitString source)
        //{
        //    BitString bs = source as BitString;
        //    if (bs != null)
        //        return bs.OrCloned(this);
        //    else
        //        return andSpecial(source, false);
        //} 
        
        #endregion

        public int Sum
        {
            get { return Int32.MaxValue; }
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