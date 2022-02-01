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
			if (iface != null)
				this.doubleValue = iface.getDoubleValue();
		}

        public static implicit operator double(DoubleTI v)
        {
            return v.doubleValue;
        }

        public static implicit operator DoubleTI(double v)
        {
            return new DoubleTI(v);
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
