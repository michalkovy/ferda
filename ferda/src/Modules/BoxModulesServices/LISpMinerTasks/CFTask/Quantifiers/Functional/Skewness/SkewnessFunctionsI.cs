using System;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractQuantifier;
using Ferda.Modules.Quantifiers;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.LISpMinerTasks.CFTask.Quantifiers.Functional.Skewness
{
	class SkewnessFunctionsI : AbstractCFTaskQuantifierFunctionsFunctional
	{
        protected override bool useNumericValues
        {
            get { return true; }
        }

        protected override ContingencyTable.QuantifierValue<OneDimensionalContingencyTable> valueFunctionDelegate
        {
            get { return OneDimensionalContingencyTable.GetSkewness; }
        }
    }
}