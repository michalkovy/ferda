using Ice;
using System;

namespace Ferda.Modules
{
	public class ShortTI : ShortT, IValue
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
			this.shortValue = iface.getShortValue();
		}
		
		/// <summary>
		/// Method getShortValue
		/// </summary>
		/// <returns>A short</returns>
		/// <param name="__current">An Ice.Current</param>
		public override short getShortValue(Current __current)
		{
			return this.shortValue;
		}

        public override int getIntValue(Current __current)
        {
            return this.shortValue;
        }

        public override long getLongValue(Current __current)
        {
            return this.shortValue;
        }

        public override float getFloatValue(Current __current)
        {
            return this.shortValue;
        }

        public override double getDoubleValue(Current __current)
        {
            return this.shortValue;
        }

        public override String getStringValue(Current __current)
        {
            return this.shortValue.ToString();
        }
	}
}
