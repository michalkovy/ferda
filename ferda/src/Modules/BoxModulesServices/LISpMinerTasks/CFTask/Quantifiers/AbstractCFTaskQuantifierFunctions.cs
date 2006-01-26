using System;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractQuantifier;
using System.Collections.Generic;
using Ferda.Modules.Boxes.LISpMinerTasks.CFTask.Quantifiers.AbstractCFQuantifier;
using Ferda.Modules.Quantifiers;

namespace Ferda.Modules.Boxes.LISpMinerTasks.CFTask.Quantifiers
{
	/// <summary>
	/// Defined properties: Relation and Treshold.
	/// </summary>
	public abstract class AbstractCFTaskQuantifierFunctions : AbstractCFQuantifierFunctionsDisp_, IFunctions
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

		#region Properties
		protected RelationEnum Relation
		{
			get
			{
				return (RelationEnum)Enum.Parse(typeof(RelationEnum), this.boxModule.GetPropertyString("Relation"));
			}
		}

		protected double Treshold
		{
			get
			{
				return this.boxModule.GetPropertyDouble("Treshold");
			}
		}
		#endregion

		/*
		protected abstract ContingencyTable.QuantifierValue<TwoDimensionalContingencyTable> valueFunctionDelegate
		{
			get;
		}
		 */

		#region Functions
		public override string QuantifierIdentifier(Ice.Current __current)
		{
			return boxModule.BoxInfo.Identifier;
		}

		public override bool Validity(AbstractQuantifierSetting setting, Ice.Current __current)
		{
			return ContingencyTable.Compare(Value(setting), Relation, Treshold);
		}

		/*
		public override double Value(AbstractQuantifierSetting setting, Ice.Current __current)
		{
			TwoDimensionalContingencyTable table = new TwoDimensionalContingencyTable(setting.firstContingencyTableRows);
			table.StartColumnBound = ColumnFrom;
			table.StartRowBound = RowFrom;
			table.EndColumnBound = ColumnTo;
			table.EndRowBound = RowTo;

			return ContingencyTable.Value<TwoDimensionalContingencyTable>(
				valueFunctionDelegate,
				table);
		}
		 */
		#endregion
	}

	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// Defined properties: CategoryRangeFrom, CategoryRangeTo, Units, Relation, Treshold.
	/// </remarks>
	public abstract class AbstractCFTaskQuantifierFunctionsAggregation : AbstractCFTaskQuantifierFunctions
	{
		#region Properties
		protected string CategoryRangeFrom
		{
			get
			{
				return this.boxModule.GetPropertyString("CategoryRangeFrom");
			}
		}

		protected string CategoryRangeTo
		{
			get
			{
				return this.boxModule.GetPropertyString("CategoryRangeTo");
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

		public override double Value(AbstractQuantifierSetting setting, Ice.Current __current)
		{
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
	/// 
	/// </summary>
	/// <remarks>
	/// Defined properties: Relation, Treshold.
	/// </remarks>
	public abstract class AbstractCFTaskQuantifierFunctionsFunctional : AbstractCFTaskQuantifierFunctions
	{
		#region Properties
		#endregion

		public override double Value(AbstractQuantifierSetting setting, Ice.Current __current)
		{
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
