using System;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractQuantifier;
using System.Collections.Generic;
using Ferda.Modules.Boxes.LISpMinerTasks.FFTTask.Quantifiers.AbstractFFTQuantifier;
using Ferda.Modules.Quantifiers;

namespace Ferda.Modules.Boxes.LISpMinerTasks.FFTTask.Quantifiers.Functional.BaseCeil
{
	/// <summary>
	/// Base/Ceil quantifier.
	/// </summary>
	/// <remarks>
	/// <para>Base quantifier prescribe a minimum/maximum absolute/relative <c>support</c> (a treshold on the <c>a</c> frequency).</para>
	/// <para>Type of comparsion is set by <c>relation</c> property. Test to minimum is applied if <c>relation</c> is set to <c>greater than or equal</c> otherwise is applied test to maximum value.</para>
	/// </remarks>
	class BaseCeilFunctionsI : AbstractFFTTaskQuantifierFunctions
	{
		#region Properties
		protected RelationEnum Relation
		{
			get
			{
                return (RelationEnum)Enum.Parse(typeof(RelationEnum), this.boxModule.GetPropertyString("Relation"));
			}
		}

		protected float Treshold
		{
			get
			{
				float value = this.boxModule.GetPropertyFloat("Treshold");
				if (value <= 0.0f)
				{
                    throw Ferda.Modules.Exceptions.BadValueError(null, boxModule.StringIceIdentity, "The parameter Treshold must greather than 0!", new string[] { "Treshold" }, restrictionTypeEnum.Minimum);
				}
				return value;
			}
		}

		protected UnitsEnum Units
		{
			get
			{
                return (UnitsEnum)Enum.Parse(typeof(UnitsEnum), this.boxModule.GetPropertyString("Units"));
			}
		}
		#endregion

		#region Functions
        /// <summary>
        /// Returns the validity of Base/Ceil quantifier, e.g. true iff the <c>a</c> frequency in 4ft-table is greater than or equal to the specified <c>treshold</c>.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <param name="__current">The __current.</param>
        /// <returns>
        /// 	<c>true</c> iff the relative/absolute (<c>units</c>) frequency of <c>a</c> in 4ft-table is in <c>relation</c> to the specified <c>treshold</c>.
        /// </returns>
		public override bool Validity(AbstractQuantifierSetting setting, Ice.Current __current)
		{
            return ContingencyTable.Compare(Value(setting), Relation, Treshold);
		}

		/// <summary>
		/// Returns the relative/absolute frequency <c>a</c> from 4ft-table.
		/// </summary>
		/// <returns>
		/// If property <c>units</c> is set to absolute number
		/// returns the <c>a</c> frequency from 4ft-table. 
		/// Else if property <c>units</c> is set to relative number
		/// returns the <c>a</c> frequency in percents from 4ft-table.
		/// </returns>
		public override double Value(AbstractQuantifierSetting setting, Ice.Current __current)
		{
            return ContingencyTable.Value<FourFoldContingencyTable>(
                FourFoldContingencyTable.BaseCeilValue,
                new FourFoldContingencyTable(setting.firstContingencyTableRows),
                Units,
                setting.allObjectsCount);            
		}
		#endregion
	}
}