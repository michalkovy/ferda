using System;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractQuantifier;
using System.Collections.Generic;
using Ferda.Modules.Boxes.LISpMinerTasks.SDFFTTask.Quantifiers.AbstractSDFFTQuantifier;
using Ferda.Modules.Quantifiers;

namespace Ferda.Modules.Boxes.LISpMinerTasks.SDFFTTask.Quantifiers
{
	public abstract class AbstractSDFFTTaskQuantifierFunctions : AbstractSDFFTQuantifierFunctionsDisp_, IFunctions
	{
		protected BoxModuleI boxModule;
		//protected IBoxInfo boxInfo;

		#region IFunctions Members
		public void setBoxModuleInfo(BoxModuleI boxModule, IBoxInfo boxInfo)
		{
			this.boxModule = boxModule;
			//this.boxInfo = boxInfo;
		}
		#endregion

		#region Functions
		public override string QuantifierIdentifier(Ice.Current __current)
		{
			return boxModule.BoxInfo.Identifier;
		}
		#endregion

		protected abstract ContingencyTable.QuantifierValue<FourFoldContingencyTable> valueFunctionDelegate
		{
			get;
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// Defined properties: OperationMode, Relation and Treshold.
	/// </remarks>
	public abstract class AbstractSDFFTTaskQuantifierFunctionsFunctional : AbstractSDFFTTaskQuantifierFunctions
	{
		#region Properties
		protected OperationModeEnum OperationMode
		{
			get
			{
				//return OperationModeEnum.AbsoluteDifferenceOfQuantifierValues;
				return (OperationModeEnum)Enum.Parse(typeof(OperationModeEnum), this.boxModule.GetPropertyString("OperationMode"));
			}
		}

		protected RelationEnum Relation
		{
			get
			{
				//return RelationEnum.LessThanOrEqual;
				return (RelationEnum)Enum.Parse(typeof(RelationEnum), this.boxModule.GetPropertyString("Relation"));
			}
		}

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
		public override bool Validity(AbstractQuantifierSetting setting, Ice.Current __current)
		{
			return ContingencyTable.Compare(Value(setting), Relation, Treshold);
		}

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
	/// 
	/// </summary>
	/// <remarks>
	/// Defined properties: OperationMode, Relation, Treshold and Units.
	/// </remarks>
	public abstract class AbstractSDFFTTaskQuantifierFunctionsAggregation : AbstractSDFFTTaskQuantifierFunctionsFunctional
	{
		#region Properties
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
		public override bool Validity(AbstractQuantifierSetting setting, Ice.Current __current)
		{
			return ContingencyTable.Compare(Value(setting), Relation, Treshold);
		}

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
