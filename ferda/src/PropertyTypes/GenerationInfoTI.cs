using Ice;
using System.Collections.Generic;
using System.Collections;

namespace Ferda.Modules
{
	public class GenerationInfoTI : GenerationInfoT, IValue
	{
		public GenerationInfoTI()
		{
			this.generationInfoValue = new GeneratingStruct();
			this.generationInfoValue.generationStartTime = new DateTimeTI(0, 0, 0, 0, 0, 0);
			this.generationInfoValue.generationTotalTime = new TimeTI(0, 0, 0);
		}

		public GenerationInfoTI(GenerationInfoTInterfacePrx iface)
		{
			this.generationInfoValue = iface.getGenerationInfo();
		}

		public GenerationInfoTI(GeneratingStruct generatingStruct)
		{
			this.generationInfoValue = generatingStruct;
		}


		public override GeneratingStruct getGenerationInfo(Current __current)
		{
			return this.generationInfoValue;
		}

		ValueT IValue.getValueT()
		{
			GenerationInfoValueT result = new GenerationInfoValueT();
			result.Value = this.generationInfoValue;
			return result;
		}
	}
}
