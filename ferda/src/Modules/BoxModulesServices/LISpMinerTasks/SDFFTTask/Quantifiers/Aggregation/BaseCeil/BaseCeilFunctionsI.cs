using System;
using Ferda.Modules.Boxes.AbstractQuantifier;
using Ferda.Modules.Boxes.SDFFTTask.Quantifiers.AbstractSDFFTQuantifier;
using Ferda.Modules.Quantifiers;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.SDFFTTask.Quantifiers.Aggregation.BaseCeil
{
	class BaseCeilFunctionsI : AbstractSDFFTTaskQuantifierFunctionsAggregation
	{
		protected override ContingencyTable.QuantifierValue<FourFoldContingencyTable> valueFunctionDelegate
		{
			get
			{
				return FourFoldContingencyTable.BaseCeilValue;
			}
		}
	}
}