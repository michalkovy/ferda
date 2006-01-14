using System;
using Ferda.Modules.Boxes.AbstractQuantifier;
using Ferda.Modules.Quantifiers;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.KLTask.Quantifiers.Functional.MutualInformationNormalized
{
	class MutualInformationNormalizedFunctionsI : AbstractKLTaskQuantifierFunctions
	{
		protected override ContingencyTable.QuantifierValue<TwoDimensionalContingencyTable> valueFunctionDelegate
		{
			get {
				return TwoDimensionalContingencyTable.MutualInformationNormalizedValue;			
			}
		}
	}
}