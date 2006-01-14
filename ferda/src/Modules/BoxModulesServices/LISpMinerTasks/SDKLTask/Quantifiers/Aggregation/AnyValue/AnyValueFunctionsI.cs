using System;
using Ferda.Modules.Boxes.AbstractQuantifier;
using Ferda.Modules.Quantifiers;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.SDKLTask.Quantifiers.Aggregation.AnyValue
{
	class AnyValueFunctionsI : AbstractSDKLTaskQuantifierFunctions
	{
		protected override ContingencyTable.QuantifierValue<TwoDimensionalContingencyTable> valueFunctionDelegate
		{
			get
			{
				return null;
			}
		}

		private bool ComputeValue(AbstractQuantifierSetting setting, out double value)
		{
			TwoDimensionalContingencyTable tableA = new TwoDimensionalContingencyTable(setting.firstContingencyTableRows);
			tableA.StartColumnBound = ColumnFrom;
			tableA.StartRowBound = RowFrom;
			tableA.EndColumnBound = ColumnTo;
			tableA.EndRowBound = RowTo;

			TwoDimensionalContingencyTable tableB = new TwoDimensionalContingencyTable(setting.secondContingencyTableRows);
			tableB.StartColumnBound = ColumnFrom;
			tableB.StartRowBound = RowFrom;
			tableB.EndColumnBound = ColumnTo;
			tableB.EndRowBound = RowTo;

			if (ContingencyTable.IsOperationModeOverQuantifierValues(OperationMode))
			{
				double valueA;
				bool resultA = tableA.AnyValue(Relation, Treshold, Units, setting.allObjectsCount, out valueA);
				double valueB;
				bool resultB = tableB.AnyValue(Relation, Treshold, Units, setting.allObjectsCount, out valueB);
				value = TwoDimensionalContingencyTable.Combine(valueA, valueB, OperationMode);

				if (resultA && resultB)
					return true;
				else
					return false;
			}
			else
			{
				TwoDimensionalContingencyTable combinedTable =
					(TwoDimensionalContingencyTable)TwoDimensionalContingencyTable.Combine(tableA, tableB, OperationMode);
				return combinedTable.AnyValue(Relation, Treshold, Units, setting.allObjectsCount, out value);
			}
		}
		public override bool Validity(AbstractQuantifierSetting setting, Ice.Current __current)
		{
			double value;
			return ComputeValue(setting, out value);
		}

		public override double Value(AbstractQuantifierSetting setting, Ice.Current __current)
		{
			double value;
			if (ComputeValue(setting, out value))
				return value;
			else
				return 0D;
		}
	}
}