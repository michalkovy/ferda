using System;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractQuantifier;
using Ferda.Modules.Quantifiers;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.LISpMinerTasks.SDCFTask.Quantifiers.Functional.Skewness
{
	class SkewnessFunctionsI : AbstractSDCFTaskQuantifierFunctionsFunctional
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