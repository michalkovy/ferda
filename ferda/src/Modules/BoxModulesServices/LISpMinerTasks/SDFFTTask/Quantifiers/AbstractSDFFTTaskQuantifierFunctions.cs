using System;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractQuantifier;
using System.Collections.Generic;
using Ferda.Modules.Boxes.LISpMinerTasks.SDFFTTask.Quantifiers.AbstractSDFFTQuantifier;
using Ferda.Modules.Quantifiers;

namespace Ferda.Modules.Boxes.LISpMinerTasks.SDFFTTask.Quantifiers
{
    /// <summary>
    /// Base for SDFFT quantifiers.	
    /// </summary>
	public abstract class AbstractSDFFTTaskQuantifierFunctions : AbstractSDFFTQuantifierFunctionsDisp_, IFunctions
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
		#endregion

        /// <summary>
        /// Gets the value function delegate.
        /// </summary>
        /// <value>The value function delegate.</value>
		protected abstract ContingencyTable.QuantifierValue<FourFoldContingencyTable> valueFunctionDelegate
		{
			get;
		}
	}

    /// <summary>
    /// Base for SDFFT quantifiers.	
    /// </summary>
	/// <remarks>
	/// Defined properties: OperationMode, Relation and Treshold.
	/// </remarks>
	public abstract class AbstractSDFFTTaskQuantifierFunctionsFunctional : AbstractSDFFTTaskQuantifierFunctions
	{
		#region Properties
        /// <summary>
        /// Gets the operation mode.
        /// </summary>
        /// <value>The operation mode.</value>
		protected OperationModeEnum OperationMode
		{
			get
			{
				//return OperationModeEnum.AbsoluteDifferenceOfQuantifierValues;
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
				//return RelationEnum.LessThanOrEqual;
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
				//return 3;
				return this.boxModule.GetPropertyDouble("Treshold");
			}
		}
		#endregion

		#region Functions
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

        /// <summary>
        /// Gets the value of the quantifier above specified <c>setting</c>.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <param name="__current">The __current.</param>
        /// <returns></returns>
		public override double Value(AbstractQuantifierSetting setting, Ice.Current __current)
		{
			return ContingencyTable.Value<FourFoldContingencyTable>(
				valueFunctionDelegate,
				new FourFoldContingencyTable(setting.firstContingencyTableRows),
				new FourFoldContingencyTable(setting.secondContingencyTableRows),
				OperationMode);
		}
		#endregion
	}

    /// <summary>
    /// Base for SDFFT quantifiers.	
    /// </summary>
	/// <remarks>
	/// Defined properties: OperationMode, Relation, Treshold and Units.
	/// </remarks>
	public abstract class AbstractSDFFTTaskQuantifierFunctionsAggregation : AbstractSDFFTTaskQuantifierFunctionsFunctional
	{
		#region Properties
        /// <summary>
        /// Gets the units.
        /// </summary>
        /// <value>The units.</value>
		protected UnitsEnum Units
		{
			get
			{
				//return UnitsEnum.AbsoluteNumber;
				return (UnitsEnum)Enum.Parse(typeof(UnitsEnum), this.boxModule.GetPropertyString("Units"));
			}
		}
		#endregion

		#region Functions
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

        /// <summary>
        /// Gets the value of the quantifier above specified <c>setting</c>.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <param name="__current">The __current.</param>
        /// <returns></returns>
		public override double Value(AbstractQuantifierSetting setting, Ice.Current __current)
		{
			return ContingencyTable.Value<FourFoldContingencyTable>(
				valueFunctionDelegate,
				new FourFoldContingencyTable(setting.firstContingencyTableRows),
				new FourFoldContingencyTable(setting.secondContingencyTableRows),
				OperationMode,
				Units,
				setting.allObjectsCount);
		}
		#endregion
	}
}
