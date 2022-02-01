using System;
using Ice;

namespace Ferda.Modules
{
	public class LongTI : LongT, LongTInterfaceOperations_, IValue
	{
		public ValueT getValueT()
		{
			LongValueT result = new LongValueT();
			result.Value = this;
			return result;
		}
		
		public LongTI()
		{}
		
		public LongTI(long longValue)
		{
			this.longValue = longValue;
		}
		
		public LongTI(LongTInterfacePrx iface)
		{
			if (iface != null)
				this.longValue = iface.getLongValue();
		}

        public static implicit operator long(LongTI v)
        {
            return v.longValue;
        }

        public static implicit operator LongTI(long v)
        {
            return new LongTI(v);
        }
	    
		/// <summary>
		/// Method getLongValue
		/// </summary>
		/// <returns>A long</returns>
		/// <param name="__current">An Ice.Current</param>
		public long getLongValue(Current __current)
		{
			return this.longValue;
		}

        public float getFloatValue(Current __current)
        {
            return this.longValue;
        }

        public double getDoubleValue(Current __current)
        {
            return this.longValue;
        }

        public String getStringValue(Current __current)
        {
            return this.longValue.ToString();
        }
	}
}
