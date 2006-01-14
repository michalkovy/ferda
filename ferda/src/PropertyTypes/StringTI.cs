
using System;
using Ice;

namespace Ferda.Modules
{
	public class StringTI : StringT, IValue
	{
		public ValueT getValueT()
		{
			StringValueT result = new StringValueT();
			result.Value = this;
			return result;
		}
		
		public StringTI()
		{}
		
		public StringTI(string stringValue)
		{
			this.stringValue = stringValue;
		}
		
		public StringTI(StringTInterfacePrx iface)
		{
			this.stringValue = iface.getStringValue();
		}
		
		/// <summary>
		/// Method getStringValue
		/// </summary>
		/// <returns>A string</returns>
		/// <param name="__current">An Ice.Current</param>
		public override String getStringValue(Current __current)
		{
			return this.stringValue;
		}
		
	}
}
