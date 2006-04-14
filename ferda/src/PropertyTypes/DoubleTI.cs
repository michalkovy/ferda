using System;
using Ice;

namespace Ferda.Modules
{
	public class DoubleTI : DoubleT, IValue
	{
		public ValueT getValueT()
		{
			DoubleValueT result = new DoubleValueT();
			result.Value = this;
			return result;
		}
		
		public DoubleTI()
		{}
		
		public DoubleTI(double doubleValue)
		{
			this.doubleValue = doubleValue;
		}
		
		public DoubleTI(DoubleTInterfacePrx iface)
		{
			this.doubleValue = iface.getDoubleValue();
		}
		
		/// <summary>
		/// Method getDoubleValue
		/// </summary>
		/// <returns>A double</returns>
		/// <param name="__current">An Ice.Current</param>
		public override double getDoubleValue(Current __current)
		{
			return this.doubleValue;
		}

        public override String getStringValue(Current __current)
        {
            return this.doubleValue.ToString();
        }
	}
}
