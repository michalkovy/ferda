using System;
using Ferda.Modules.Boxes.LISpMinerTasks.KLTask.Quantifiers;
using Ferda.Modules.Quantifiers;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.LISpMinerTasks.KLTask.Quantifiers.Functional.ChiSquareTest
{
	class ChiSquareTestFunctionsI : AbstractKLTaskQuantifierFunctions
	{
		protected override ContingencyTable.QuantifierValue<TwoDimensionalContingencyTable> valueFunctionDelegate
		{
			get
			{
                return TwoDimensionalContingencyTable.ChiSquare;
			}
		}
	}
}