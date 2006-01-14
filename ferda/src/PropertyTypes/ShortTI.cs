using Ice;

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
		
	}
}
