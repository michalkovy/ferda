using System;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractQuantifier;
using Ferda.Modules.Quantifiers;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.LISpMinerTasks.SDCFTask.Quantifiers.Aggregation.MinValue
{
	class MinValueFunctionsI : AbstractSDCFTaskQuantifierFunctionsAggregation
	{
        protected override ContingencyTable.QuantifierValue<OneDimensionalContingencyTable> valueFunctionDelegate
        {
            get { return OneDimensionalContingencyTable.GetMinValue; }
        }
	}
}