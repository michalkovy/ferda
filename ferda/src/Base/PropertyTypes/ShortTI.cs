using Ice;
using System;

namespace Ferda.Modules
{
	public class ShortTI : ShortT, ShortTInterfaceOperations_, IValue
	{
		public ValueT getValueT()
		{
			ShortValueT result = new ShortValueT();
			result.Value = this;
			return result;
		}
		
		public ShortTI()
		{}
		
		public ShortTI(short shortValue)
		{
			this.shortValue = shortValue;
		}
		
		public ShortTI(ShortTInterfacePrx iface)
		{
			if (iface != null)
				this.shortValue = iface.getShortValue();
		}

        public static implicit operator short(ShortTI v)
        {
            return v.shortValue;
        }

        public static implicit operator ShortTI(short v)
        {
            return new ShortTI(v);
        }
	    
		/// <summary>
		/// Method getShortValue
		/// </summary>
		/// <returns>A short</returns>
		/// <param name="__current">An Ice.Current</param>
		public short getShortValue(Current __current)
		{
			return this.shortValue;
		}

        public int getIntValue(Current __current)
        {
            return this.shortValue;
        }

        public long getLongValue(Current __current)
        {
            return this.shortValue;
        }

        public float getFloatValue(Current __current)
        {
            return this.shortValue;
        }

        public double getDoubleValue(Current __current)
        {
            return this.shortValue;
        }

        public String getStringValue(Current __current)
        {
            return this.shortValue.ToString();
        }
	}
}
