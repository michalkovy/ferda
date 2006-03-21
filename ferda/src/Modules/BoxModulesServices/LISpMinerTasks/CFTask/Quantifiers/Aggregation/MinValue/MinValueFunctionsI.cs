using System;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractQuantifier;
using Ferda.Modules.Quantifiers;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.LISpMinerTasks.CFTask.Quantifiers.Aggregation.MinValue
{
	class MinValueFunctionsI : AbstractCFTaskQuantifierFunctionsAggregation
	{
        protected override ContingencyTable.QuantifierValue<OneDimensionalContingencyTable> valueFunctionDelegate
        {
            get { return OneDimensionalContingencyTable.GetMinValue; }
        }
    }
}