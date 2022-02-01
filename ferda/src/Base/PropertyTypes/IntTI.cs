using System;
using Ice;

namespace Ferda.Modules
{
	public class IntTI : IntT, IntTInterfaceOperations_, IValue
	{
		public ValueT getValueT()
		{
			IntValueT result = new IntValueT();
			result.Value = this;
			return result;
		}
		
		public IntTI()
		{}
		
		public IntTI(int intValue)
		{
			this.intValue = intValue;
		}
		
		public IntTI(IntTInterfacePrx iface)
		{
			if (iface != null)
				this.intValue = iface.getIntValue();
		}

        public static implicit operator int(IntTI v)
        {
            return v.intValue;
        }

        public static implicit operator IntTI(int v)
        {
            return new IntTI(v);
        }
	    
		/// <summary>
		/// Method getIntValue
		/// </summary>
		/// <returns>An int</returns>
		/// <param name="__current">An Ice.Current</param>
		public int getIntValue(Current __current)
		{
			return this.intValue;
		}

        public long getLongValue(Current __current)
        {
            return this.intValue;
        }

        public float getFloatValue(Current __current)
        {
            return this.intValue;
        }

        public double getDoubleValue(Current __current)
        {
            return this.intValue;
        }

        public String getStringValue(Current __current)
        {
            return this.intValue.ToString();
        }
	}
}
