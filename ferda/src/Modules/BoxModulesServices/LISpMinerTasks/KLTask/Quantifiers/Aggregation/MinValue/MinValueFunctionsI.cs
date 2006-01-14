using System;
using Ferda.Modules.Boxes.AbstractQuantifier;
using Ferda.Modules.Quantifiers;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.KLTask.Quantifiers.Aggregation.MinValue
{
	class MinValueFunctionsI : AbstractKLTaskQuantifierFunctionsWithUnits
	{
		protected override ContingencyTable.QuantifierValue<TwoDimensionalContingencyTable> valueFunctionDelegate
		{
			get
			{
				return TwoDimensionalContingencyTable.MinValueAggregationValue;
			}
		}
	}
}