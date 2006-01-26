using System;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractQuantifier;
using Ferda.Modules.Boxes.LISpMinerTasks.SDFFTTask.Quantifiers.AbstractSDFFTQuantifier;
using Ferda.Modules.Quantifiers;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.LISpMinerTasks.SDFFTTask.Quantifiers.Functional.FoundedImplication
{
	class FoundedImplicationFunctionsI : AbstractSDFFTTaskQuantifierFunctionsFunctional
	{
		protected override ContingencyTable.QuantifierValue<FourFoldContingencyTable> valueFunctionDelegate
		{
			get
			{
				return FourFoldContingencyTable.FoundedImplicationValue;
			}
		}
	}
}