using System;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractQuantifier;
using Ferda.Modules.Quantifiers;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.LISpMinerTasks.KLTask.Quantifiers.Aggregation.AnyValue
{
	class AnyValueFunctionsI : AbstractKLTaskQuantifierFunctionsWithUnits
	{
		protected override ContingencyTable.QuantifierValue<TwoDimensionalContingencyTable> valueFunctionDelegate
		{
			get
			{
				return null;
			}
		}

		#region Functions
		public override bool Validity(AbstractQuantifierSetting setting, Ice.Current __current)
		{
			TwoDimensionalContingencyTable table = new TwoDimensionalContingencyTable(setting.firstContingencyTableRows);
			table.StartColumnBound = ColumnFrom;
			table.StartRowBound = RowFrom;
			table.EndColumnBound = ColumnTo;
			table.EndRowBound = RowTo;

			double result;

			return table.AnyValue(Relation, Treshold, Units, setting.allObjectsCount, out result);
		}

		public override double Value(AbstractQuantifierSetting setting, Ice.Current __current)
		{
			TwoDimensionalContingencyTable table = new TwoDimensionalContingencyTable(setting.firstContingencyTableRows);
			table.StartColumnBound = ColumnFrom;
			table.StartRowBound = RowFrom;
			table.EndColumnBound = ColumnTo;
			table.EndRowBound = RowTo;

			double result;

			table.AnyValue(Relation, Treshold, Units, setting.allObjectsCount, out result);

			return result;
		}
		#endregion
	}
}