using Ice;
using System;

namespace Ferda.Modules
{
	public class BoolTI : BoolT, IValue
	{
		public ValueT getValueT()
		{
			BoolValueT result = new BoolValueT();
			result.Value = this;
			return result;
		}
		
		public BoolTI()
		{}
		
		public BoolTI(bool boolValue)
		{
			this.boolValue = boolValue;
		}
		
		public BoolTI(BoolTInterfacePrx iface)
		{
			this.boolValue = iface.getBoolValue();
		}
		
		/// <summary>
		/// Method getBoolValue
		/// </summary>
		/// <returns>A bool</returns>
		/// <param name="__current">An Ice.Current</param>
		public override bool getBoolValue(Current __current)
		{
			return this.boolValue;
		}

        public override short getShortValue(Current __current)
        {
            return this.boolValue ? (short)1 : (short)0;
        }

        public override int getIntValue(Current __current)
        {
            return this.boolValue ? 1 : 0;
        }

        public override long getLongValue(Current __current)
        {
            return this.boolValue ? 1 : 0;
        }

        public override float getFloatValue(Current __current)
        {
            return this.boolValue ? 1 : 0;
        }

        public override double getDoubleValue(Current __current)
        {
            return this.boolValue ? 1 : 0;
        }

        public override String getStringValue(Current __current)
        {
            return this.boolValue.ToString();
        }
	}
}
