using Ice;
using System.Collections.Generic;
using System.Collections;

namespace Ferda.Modules
{
	public class HypothesesTI : HypothesesT, IValue
	{
		public HypothesesTI()
		{
			this.hypothesesValue = new HypothesisStruct[0];
		}

		public HypothesesTI(HypothesesTInterfacePrx iface)
		{
			this.hypothesesValue = iface.getHypothesesValue();
		}

		public HypothesesTI(HypothesisStruct[] hypothesisStructSeq)
		{
			this.hypothesesValue = hypothesisStructSeq;
		}

		ValueT IValue.getValueT()
		{
			HypothesesValueT result = new HypothesesValueT();
			result.Value = this.hypothesesValue;
			return result;
		}

		public override HypothesisStruct[] getHypothesesValue(Current __current)
		{
			return this.hypothesesValue;
		}
	}
}
