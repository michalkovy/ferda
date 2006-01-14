
using Ice;

namespace Ferda.Modules
{
	public class IntTI : IntT, IValue
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
			this.intValue = iface.getIntValue();
		}
		
		/// <summary>
		/// Method getIntValue
		/// </summary>
		/// <returns>An int</returns>
		/// <param name="__current">An Ice.Current</param>
		public override int getIntValue(Current __current)
		{
			return this.intValue;
		}
	}
}
