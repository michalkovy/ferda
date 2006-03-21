using System;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractQuantifier;
using Ferda.Modules.Quantifiers;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.LISpMinerTasks.SDCFTask.Quantifiers.Aggregation.AnyValue
{
	class AnyValueFunctionsI : AbstractSDCFTaskQuantifierFunctionsAggregation
	{
        protected override ContingencyTable.QuantifierValue<OneDimensionalContingencyTable> valueFunctionDelegate
        {
            get
            {
                return null;
            }
        }

        private bool ComputeValue(AbstractQuantifierSetting setting, out double value)
        {
            OneDimensionalContingencyTable tableA = new OneDimensionalContingencyTable(setting.firstContingencyTableRows);
            tableA.StartColumnBound = CategoryRangeFrom;
            tableA.EndColumnBound = CategoryRangeTo;

            OneDimensionalContingencyTable tableB = new OneDimensionalContingencyTable(setting.secondContingencyTableRows);
            tableB.StartColumnBound = CategoryRangeFrom;
            tableB.EndColumnBound = CategoryRangeTo;

            if (ContingencyTable.IsOperationModeOverQuantifierValues(OperationMode))
            {
                double valueA;
                bool resultA = tableA.AnyValue(Relation, Treshold, Units, setting.allObjectsCount, out valueA);
                double valueB;
                bool resultB = tableB.AnyValue(Relation, Treshold, Units, setting.allObjectsCount, out valueB);
                value = OneDimensionalContingencyTable.Combine(valueA, valueB, OperationMode);

                if (resultA && resultB)
                    return true;
                else
                    return false;
            }
            else
            {
                OneDimensionalContingencyTable combinedTable =
                    ContingencyTable.Combine<OneDimensionalContingencyTable>(tableA, tableB, OperationMode);
                return combinedTable.AnyValue(Relation, Treshold, Units, setting.allObjectsCount, out value);
            }
        }
        /// <summary>
        /// Gets the validity of the quantifier.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <param name="__current">The __current.</param>
        /// <returns></returns>
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