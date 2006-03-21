using System;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractQuantifier;
using Ferda.Modules.Quantifiers;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.LISpMinerTasks.CFTask.Quantifiers.Functional.ArithmeticAverage
{
	class ArithmeticAverageFunctionsI : AbstractCFTaskQuantifierFunctionsFunctional
	{
        protected override bool useNumericValues
        {
            get { return true; }
        }

        protected override ContingencyTable.QuantifierValue<OneDimensionalContingencyTable> valueFunctionDelegate
        {
            get { return OneDimensionalContingencyTable.GetArithmeticAverage; }
        }
    }
}