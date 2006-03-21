using System;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractQuantifier;
using Ferda.Modules.Quantifiers;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.LISpMinerTasks.CFTask.Quantifiers.Aggregation.AverageValue
{
	class AverageValueFunctionsI : AbstractCFTaskQuantifierFunctionsAggregation
	{
        protected override ContingencyTable.QuantifierValue<OneDimensionalContingencyTable> valueFunctionDelegate
        {
            get { return OneDimensionalContingencyTable.GetAverageValue; }
        }
    }
}