using System;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractQuantifier;
using System.Collections.Generic;
using Ferda.Modules.Boxes.LISpMinerTasks.SDCFTask.Quantifiers.AbstractSDCFQuantifier;
using Ferda.Modules.Quantifiers;

namespace Ferda.Modules.Boxes.LISpMinerTasks.SDCFTask.Quantifiers
{
	/// <summary>
    /// Base for SDCF quantifiers.
	/// </summary>
    /// <remarks>
    /// Defined properties: OperationMode, Relation and Treshold.
    /// </remarks>
	public abstract class AbstractSDCFTaskQuantifierFunctions : AbstractSDCFQuantifierFunctionsDisp_, IFunctions
	{
        /// <summary>
        /// The box module.
        /// </summary>
		protected BoxModuleI boxModule;
		//protected IBoxInfo boxInfo;

		#region IFunctions Members
        /// <summary>
        /// Sets the <see cref="T:Ferda.Modules.BoxModuleI">box module</see>
        /// and the <see cref="T:Ferda.Modules.Boxes.IBoxInfo">box info</see>.
        /// </summary>
        /// <param name="boxModule">The box module.</param>
        /// <param name="boxInfo">The box info.</param>
        public void setBoxModuleInfo(BoxModuleI boxModule, IBoxInfo boxInfo)
		{
			this.boxModule = boxModule;
			//this.boxInfo = boxInfo;
		}
		#endregion

		#region Properties
        /// <summary>
        /// Gets the operation mode.
        /// </summary>
        /// <value>The operation mode.</value>
		protected OperationModeEnum OperationMode
		{
			get
			{
				return (OperationModeEnum)Enum.Parse(typeof(OperationModeEnum), this.boxModule.GetPropertyString("OperationMode"));
			}
		}

        /// <summary>
        /// Gets the relation.
        /// </summary>
        /// <value>The relation.</value>
		protected RelationEnum Relation
		{
			get
			{
				return (RelationEnum)Enum.Parse(typeof(RelationEnum), this.boxModule.GetPropertyString("Relation"));
			}
		}

        /// <summary>
        /// Gets the treshold.
        /// </summary>
        /// <value>The treshold.</value>
		protected double Treshold
		{
			get
			{
				return this.boxModule.GetPropertyDouble("Treshold");
			}
		}
		#endregion

        /* TODO uncomment
        /// <summary>
        /// Gets the value function delegate.
        /// </summary>
        /// <value>The value function delegate.</value>
        protected abstract ContingencyTable.QuantifierValue<TwoDimensionalContingencyTable> valueFunctionDelegate
		{
			get;
		}
		 */

        #region Functions
        /// <summary>
        /// Gets the quantifier box identifier.
        /// </summary>
        /// <param name="__current">The Ice __current.</param>
        /// <returns>Box type identifier.</returns>
		public override string QuantifierIdentifier(Ice.Current __current)
		{
			return boxModule.BoxInfo.Identifier;
		}

        /// <summary>
        /// Gets the validity of the quantifier.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <param name="__current">The __current.</param>
        /// <returns></returns>
		public override bool Validity(AbstractQuantifierSetting setting, Ice.Current __current)
		{
			return ContingencyTable.Compare(Value(setting), Relation, Treshold);
		}
		#endregion
	}

	/// <summary>
    /// Base for SDCF quantifiers.
	/// </summary>
	/// <remarks>
	/// Defined properties: CategoryRangeFrom, CategoryRangeTo, Units, OperationMode, Relation, Treshold.
	/// </remarks>
	public abstract class AbstractSDCFTaskQuantifierFunctionsAggregation : AbstractSDCFTaskQuantifierFunctions
	{
		#region Properties
        /// <summary>
        /// Gets the category range from.
        /// </summary>
        /// <value>The category range from.</value>
		protected string CategoryRangeFrom
		{
			get
			{
				return this.boxModule.GetPropertyString("CategoryRangeFrom");
			}
		}

        /// <summary>
        /// Gets the category range to.
        /// </summary>
        /// <value>The category range to.</value>
		protected string CategoryRangeTo
		{
			get
			{
				return this.boxModule.GetPropertyString("CategoryRangeTo");
			}
		}

        /// <summary>
        /// Gets the units.
        /// </summary>
        /// <value>The units.</value>
		protected UnitsEnum Units
		{
			get
			{
				return (UnitsEnum)Enum.Parse(typeof(UnitsEnum), this.boxModule.GetPropertyString("Units"));
			}
		}
		#endregion

        /// <summary>
        /// Gets the value of the quantifier above specified <c>setting</c>.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <param name="__current">The __current.</param>
        /// <returns></returns>
		public override double Value(AbstractQuantifierSetting setting, Ice.Current __current)
		{
            //TODO SDCF Quantifiers
			return 0;
			/*
			TwoDimensionalContingencyTable table = new TwoDimensionalContingencyTable(setting.firstContingencyTableRows);
			table.StartColumnBound = ColumnFrom;
			table.StartRowBound = RowFrom;
			table.EndColumnBound = ColumnTo;
			table.EndRowBound = RowTo;

			return ContingencyTable.Value<TwoDimensionalContingencyTable>(
				valueFunctionDelegate,
				table,
				Units,
				setting.allObjectsCount);
			 */
		}
	}

	/// <summary>
    /// Base for SDCF quantifiers.
	/// </summary>
	/// <remarks>
	/// Defined properties: OperationMode, Relation, Treshold.
	/// </remarks>
	public abstract class AbstractSDCFTaskQuantifierFunctionsFunctional : AbstractSDCFTaskQuantifierFunctions
	{
		#region Properties
		#endregion

        /// <summary>
        /// Gets the value of the quantifier above specified <c>setting</c>.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <param name="__current">The __current.</param>
        /// <returns></returns>
		public override double Value(AbstractQuantifierSetting setting, Ice.Current __current)
		{
            //TODO SDCF Quantifiers
			return 0;
			/*
			TwoDimensionalContingencyTable table = new TwoDimensionalContingencyTable(setting.firstContingencyTableRows);
			table.StartColumnBound = ColumnFrom;
			table.StartRowBound = RowFrom;
			table.EndColumnBound = ColumnTo;
			table.EndRowBound = RowTo;

			return ContingencyTable.Value<TwoDimensionalContingencyTable>(
				valueFunctionDelegate,
				table);
			 */
		}
	}
}
