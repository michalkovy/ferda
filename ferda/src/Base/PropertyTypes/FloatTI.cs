using System;
using Ice;

namespace Ferda.Modules
{
	public class FloatTI : FloatT, FloatTInterfaceOperations_, IValue
	{
		public ValueT getValueT()
		{
			FloatValueT result = new FloatValueT();
			result.Value = this;
			return result;
		}
		
		public FloatTI()
		{}
		
		public FloatTI(float floatValue)
		{
			this.floatValue = floatValue;
		}
		
		public FloatTI(FloatTInterfacePrx iface)
		{
			if (iface != null)
				this.floatValue = iface.getFloatValue();
		}

        public static implicit operator float(FloatTI v)
        {
            return v.floatValue;
        }

        public static implicit operator FloatTI(float v)
        {
            return new FloatTI(v);
        }
		
		/// <summary>
		/// Method getFloatValue
		/// </summary>
		/// <returns>A float</returns>
		/// <param name="__current">An Ice.Current</param>
		public float getFloatValue(Current __current)
		{
			return this.floatValue;
		}

        public double getDoubleValue(Current __current)
        {
            return this.floatValue;
        }

        public String getStringValue(Current __current)
        {
            return this.floatValue.ToString();
        }
	}
}
