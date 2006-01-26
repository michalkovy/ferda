using System;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractQuantifier;
using System.Collections.Generic;
using Ferda.Modules.Boxes.LISpMinerTasks.KLTask.Quantifiers.AbstractKLQuantifier;
using Ferda.Modules.Quantifiers;

namespace Ferda.Modules.Boxes.LISpMinerTasks.KLTask.Quantifiers
{
	/// <summary>
	/// Defined properties: RowFrom, RowTo, ColumnFrom, ColumnTo, Relation and Treshold.
	/// </summary>
	public abstract class AbstractKLTaskQuantifierFunctions : AbstractKLQuantifierFunctionsDisp_, IFunctions
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
		protected string RowFrom
		{
			get
			{
				return this.boxModule.GetPropertyString("RowFrom");
			}
		}

		protected string RowTo
		{
			get
			{
				return this.boxModule.GetPropertyString("RowTo");
			}
		}

		protected string ColumnFrom
		{
			get
			{
				return this.boxModule.GetPropertyString("ColumnFrom");
			}
		}

		protected string ColumnTo
		{
			get
			{
				return this.boxModule.GetPropertyString("ColumnTo");
			}
		}

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

		protected abstract ContingencyTable.QuantifierValue<TwoDimensionalContingencyTable> valueFunctionDelegate
		{
			get;
		}

		#region Functions
		public override string QuantifierIdentifier(Ice.Current __current)
		{
			return boxModule.BoxInfo.Identifier;
		}

		public override bool Validity(AbstractQuantifierSetting setting, Ice.Current __current)
		{
			return ContingencyTable.Compare(Value(setting), Relation, Treshold);
		}

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
		#endregion
	}

	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// Defined properties: RowFrom, RowTo, ColumnFrom, ColumnTo, Relation, Treshold and Units.
	/// </remarks>
	public abstract class AbstractKLTaskQuantifierFunctionsWithUnits : AbstractKLTaskQuantifierFunctions
	{
		#region Properties
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
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// Defined properties: RowFrom, RowTo, ColumnFrom, ColumnTo, Relation, Treshold and Direction.
	/// </remarks>
	public abstract class AbstractKLTaskQuantifierFunctionsWithDirection : AbstractKLTaskQuantifierFunctions
	{
		#region Properties
		protected DirectionEnum Direction
		{
			get
			{
				return (DirectionEnum)Enum.Parse(typeof(DirectionEnum), this.boxModule.GetPropertyString("Direction"));
			}
		}
		#endregion

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
	}
}
