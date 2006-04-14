using System;
using Ice;

namespace Ferda.Modules
{
	public class FloatTI : FloatT, IValue
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
			this.floatValue = iface.getFloatValue();
		}
		
		/// <summary>
		/// Method getFloatValue
		/// </summary>
		/// <returns>A float</returns>
		/// <param name="__current">An Ice.Current</param>
		public override float getFloatValue(Current __current)
		{
			return this.floatValue;
		}

        public override double getDoubleValue(Current __current)
        {
            return this.floatValue;
        }

        public override String getStringValue(Current __current)
        {
            return this.floatValue.ToString();
        }
	}
}
