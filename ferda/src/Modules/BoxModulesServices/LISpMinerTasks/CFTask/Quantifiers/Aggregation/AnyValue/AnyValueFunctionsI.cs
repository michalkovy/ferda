using System;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractQuantifier;
using Ferda.Modules.Quantifiers;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.LISpMinerTasks.CFTask.Quantifiers.Aggregation.AnyValue
{
	class AnyValueFunctionsI : AbstractCFTaskQuantifierFunctionsAggregation
	{
        protected override ContingencyTable.QuantifierValue<OneDimensionalContingencyTable> valueFunctionDelegate
        {
            get
            {
                return null;
            }
        }

        #region Functions
        /// <summary>
        /// Gets the validity of the quantifier.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <param name="__current">The __current.</param>
        /// <returns></returns>
        public override bool Validity(AbstractQuantifierSetting setting, Ice.Current __current)
        {
            OneDimensionalContingencyTable table = new OneDimensionalContingencyTable(setting.firstContingencyTableRows);
            table.StartColumnBound = CategoryRangeFrom;
            table.EndColumnBound = CategoryRangeTo;

            double result;

            return table.AnyValue(Relation, Treshold, Units, setting.allObjectsCount, out result);
        }

        /// <summary>
        /// Gets the value of the quantifier above specified <c>setting</c>.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <param name="__current">The __current.</param>
        /// <returns></returns>
        public override double Value(AbstractQuantifierSetting setting, Ice.Current __current)
        {
            OneDimensionalContingencyTable table = new OneDimensionalContingencyTable(setting.firstContingencyTableRows);
            table.StartColumnBound = CategoryRangeFrom;
            table.EndColumnBound = CategoryRangeTo;

            double result;

            table.AnyValue(Relation, Treshold, Units, setting.allObjectsCount, out result);

            return result;
        }
        #endregion
    }
}