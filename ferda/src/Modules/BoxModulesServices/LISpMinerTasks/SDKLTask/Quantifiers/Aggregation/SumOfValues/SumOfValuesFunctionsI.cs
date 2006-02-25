using System;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractQuantifier;
using Ferda.Modules.Quantifiers;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.LISpMinerTasks.SDKLTask.Quantifiers.Aggregation.SumOfValues
{
	class SumOfValuesFunctionsI : AbstractSDKLTaskQuantifierFunctions
	{
		protected override ContingencyTable.QuantifierValue<TwoDimensionalContingencyTable> valueFunctionDelegate
		{
			get
			{
				return TwoDimensionalContingencyTable.GetSumOfValues;
			}
		}
	}
}