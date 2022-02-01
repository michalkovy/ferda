
using System;
using Ice;

namespace Ferda.Modules
{
	public class StringTI : StringT, StringTInterfaceOperations_, IValue
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
			if (iface != null)
				this.stringValue = iface.getStringValue();
		}

        public static implicit operator string(StringTI v)
        {
            return v.stringValue;
        }

        public static implicit operator StringTI(string v)
        {
            return new StringTI(v);
        }

		/// <summary>
		/// Method getStringValue
		/// </summary>
		/// <returns>A string</returns>
		/// <param name="__current">An Ice.Current</param>
		public String getStringValue(Current __current)
		{
			return this.stringValue;
		}
	}
}
