using Ice;

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
	}
}
