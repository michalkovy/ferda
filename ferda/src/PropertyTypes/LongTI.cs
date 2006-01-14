
using Ice;

namespace Ferda.Modules
{
	public class LongTI : LongT, IValue
	{
		public ValueT getValueT()
		{
			LongValueT result = new LongValueT();
			result.Value = this;
			return result;
		}
		
		public LongTI()
		{}
		
		public LongTI(long longValue)
		{
			this.longValue = longValue;
		}
		
		public LongTI(LongTInterfacePrx iface)
		{
			this.longValue = iface.getLongValue();
		}
		
		/// <summary>
		/// Method getLongValue
		/// </summary>
		/// <returns>A long</returns>
		/// <param name="__current">An Ice.Current</param>
		public override long getLongValue(Current __current)
		{
			return this.longValue;
		}
		
	}
}
