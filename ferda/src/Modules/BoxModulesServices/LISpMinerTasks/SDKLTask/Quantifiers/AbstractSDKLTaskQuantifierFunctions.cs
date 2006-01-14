using System;
using Ferda.Modules.Boxes.AbstractQuantifier;
using System.Collections.Generic;
using Ferda.Modules.Boxes.SDKLTask.Quantifiers.AbstractSDKLQuantifier;
using Ferda.Modules.Quantifiers;

namespace Ferda.Modules.Boxes.SDKLTask.Quantifiers
{
	/// <summary>
	/// Defined properties: RowFrom, RowTo, ColumnFrom, ColumnTo, Relation and Treshold.
	/// </summary>
	public abstract class AbstractSDKLTaskQuantifierFunctions : AbstractSDKLQuantifierFunctionsDisp_, IFunctions
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

		protected UnitsEnum Units
		{
			get
			{
				return (UnitsEnum)Enum.Parse(typeof(UnitsEnum), this.boxModule.GetPropertyString("Units"));
			}
		}

		protected OperationModeEnum OperationMode
		{
			get
			{
				return (OperationModeEnum)Enum.Parse(typeof(OperationModeEnum), this.boxModule.GetPropertyString("OperationMode"));
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

			return ContingencyTable.Value<TwoDimensionalContingencyTable>(
				valueFunctionDelegate,
				tableA,
				tableB,
				OperationMode,
				Units,
				setting.allObjectsCount);
		}
		#endregion
	}
}
