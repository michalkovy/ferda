using System;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractQuantifier;
using Ferda.Modules.Quantifiers;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.LISpMinerTasks.KLTask.Quantifiers.Functional.FunctionEachRow
{
	class FunctionEachRowFunctionsI : AbstractKLTaskQuantifierFunctionsWithUnits
	{
		protected override ContingencyTable.QuantifierValue<TwoDimensionalContingencyTable> valueFunctionDelegate
		{
			get
			{
				return TwoDimensionalContingencyTable.MinOfRowMaximumsValue;
			}
		}
	}
}