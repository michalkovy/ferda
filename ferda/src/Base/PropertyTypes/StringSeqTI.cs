
using System;
using Ice;namespace Ferda.Modules
{
	public class StringSeqTI : StringSeqT, StringSeqTInterfaceOperations_, IValue
	{
		public ValueT getValueT()
		{
			StringSeqValueT result = new StringSeqValueT();
			result.Value = this;
			return result;
		}
		
		public StringSeqTI()
		{}
		
		///<summary>
		/// Constructor
		/// </summary>
		/// <param name="stringSeqValue">A  string[]</param>
		public StringSeqTI(string[] stringSeqValue)
		{
			this.stringSeqValue = stringSeqValue;
		}
		
		public StringSeqTI(StringSeqTInterfacePrx stringSeqTInterfacePrx)
		{
			if (stringSeqTInterfacePrx != null)
				this.stringSeqValue = stringSeqTInterfacePrx.getStringSeq();
		}
		
		/// <summary>
		/// Method getStringSeq
		/// </summary>
		/// <returns>A string[]</returns>
		/// <param name="__current">An Ice.Current</param>
		public String[] getStringSeq(Current __current)
		{
			return this.stringSeqValue;
		}
	}
}
