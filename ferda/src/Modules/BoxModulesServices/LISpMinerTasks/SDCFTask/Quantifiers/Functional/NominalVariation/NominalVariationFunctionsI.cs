using System;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractQuantifier;
using Ferda.Modules.Quantifiers;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.LISpMinerTasks.SDCFTask.Quantifiers.Functional.NominalVariation
{
	class NominalVariationFunctionsI : AbstractSDCFTaskQuantifierFunctionsFunctional
	{
        protected override bool useNumericValues
        {
            get { return false; }
        }

        protected override ContingencyTable.QuantifierValue<OneDimensionalContingencyTable> valueFunctionDelegate
        {
            get { return OneDimensionalContingencyTable.GetNominalVariation; }
        }
	}
}