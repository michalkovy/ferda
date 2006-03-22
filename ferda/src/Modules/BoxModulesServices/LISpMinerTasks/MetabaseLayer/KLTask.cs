//#define FFTTask_MAIN
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Odbc;
using System.Data;
using Ferda.Modules.Boxes.LISpMinerTasks.KLTask;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractLMTask;

namespace Ferda.Modules.MetabaseLayer
{
	public class KLTask : Task
	{
		protected override TaskTypeEnum taskType
		{
			get
			{
				return TaskTypeEnum.KL;
			}
		}
		protected override string exeFileName
		{
			get
			{
				return "KLGen.exe";
			}
		}
		protected override void saveTask(object input, int masterTaskID, string boxIdentity)
		{
			Ferda.Modules.Boxes.LISpMinerTasks.KLTask.TaskStruct taskDescription = (TaskStruct)input;

			string tableName = "taTaskKL";
			string autoIncrementColumn = common.GetAutoIncrementColumnName(tableName);
			long autoIncrementValue = common.GetTableAutoIncrementValue(tableName, 1);
			string query = "INSERT INTO " + tableName + " (" + autoIncrementColumn
				+ ",TaskID) VALUES "
				+ "(" + autoIncrementValue + ","
				+ masterTaskID
				+ ")";
			common.ExecuteInsertQuery(query, tableName);
			saveQuantifiers(taskDescription.quantifiers, masterTaskID, boxIdentity);
		}

		private void saveQuantifiers(QuantifierSettingStruct[] quantifiers, int taskID, string boxIdentity)
		{
			PropertySettingHelper propertySettingHelper;
			string tableName = "tdKLQuantifier";
			string autoIncrementColumn = common.GetAutoIncrementColumnName(tableName);
			foreach (QuantifierSettingStruct quantifier in quantifiers)
			{
				propertySettingHelper = new PropertySettingHelper(quantifier.setting);
				int fromRow = Ferda.Modules.Helpers.Common.Parsing.ZeroBasedBoundFromOneBasedString(propertySettingHelper.GetStringProperty("RowFrom"));
				int toRow = Ferda.Modules.Helpers.Common.Parsing.ZeroBasedBoundFromOneBasedString(propertySettingHelper.GetStringProperty("RowTo"));
				int fromColumn = Ferda.Modules.Helpers.Common.Parsing.ZeroBasedBoundFromOneBasedString(propertySettingHelper.GetStringProperty("ColumnFrom"));
				int toColumn = Ferda.Modules.Helpers.Common.Parsing.ZeroBasedBoundFromOneBasedString(propertySettingHelper.GetStringProperty("ColumnTo"));
				RelationEnum relation = (RelationEnum)Enum.Parse(typeof(RelationEnum), propertySettingHelper.GetStringProperty("Relation"));
				double treshold = propertySettingHelper.GetDoubleProperty("Treshold");
				bool quantifierWithUnits = true;
				UnitsEnum units = UnitsEnum.AbsoluteNumber;
				try
				{
					units = (UnitsEnum)Enum.Parse(typeof(UnitsEnum), propertySettingHelper.GetStringProperty("Units"));
				}
				catch (ArgumentException)
				{
					quantifierWithUnits = false;
				}
				DirectionEnum direction = DirectionEnum.ColumnsOnRows;
				try
				{
					direction = (DirectionEnum)Enum.Parse(typeof(DirectionEnum), propertySettingHelper.GetStringProperty("Direction"));
				}
				catch (ArgumentException) { }
				bool quantifierWithRelativeTreshold = false;
				bool kendalQuantifier = false;
				int quantifierTypeID = 0;
				switch (quantifier.typeIdentifier)
				{
					case Ferda.Modules.Boxes.LISpMinerTasks.KLTask.Quantifiers.Aggregation.SumOfValues.SumOfValuesBoxInfo.typeIdentifier:
						quantifierTypeID = 1;
						break;
					case Ferda.Modules.Boxes.LISpMinerTasks.KLTask.Quantifiers.Aggregation.MinValue.MinValueBoxInfo.typeIdentifier:
						quantifierTypeID = 2;
						break;
					case Ferda.Modules.Boxes.LISpMinerTasks.KLTask.Quantifiers.Aggregation.MaxValue.MaxValueBoxInfo.typeIdentifier:
						quantifierTypeID = 3;
						break;
					case Ferda.Modules.Boxes.LISpMinerTasks.KLTask.Quantifiers.Aggregation.AverageValue.AverageValueBoxInfo.typeIdentifier:
						quantifierTypeID = 4;
						break;
					case Ferda.Modules.Boxes.LISpMinerTasks.KLTask.Quantifiers.Aggregation.AnyValue.AnyValueBoxInfo.typeIdentifier:
						quantifierTypeID = 5;
						break;
					case Ferda.Modules.Boxes.LISpMinerTasks.KLTask.Quantifiers.Functional.FunctionSumOfRows.FunctionSumOfRowsBoxInfo.typeIdentifier:
						quantifierWithRelativeTreshold = true;
						quantifierTypeID = 8;
						break;
					case Ferda.Modules.Boxes.LISpMinerTasks.KLTask.Quantifiers.Functional.FunctionEachRow.FunctionEachRowBoxInfo.typeIdentifier:
						quantifierWithRelativeTreshold = true;
						quantifierTypeID = 9;
						break;
					case Ferda.Modules.Boxes.LISpMinerTasks.KLTask.Quantifiers.Functional.ChiSquareTest.ChiSquareTestBoxInfo.typeIdentifier:
						quantifierTypeID = 7;
						break;
					case Ferda.Modules.Boxes.LISpMinerTasks.KLTask.Quantifiers.Functional.ConditionalEntropy.ConditionalEntropyBoxInfo.typeIdentifier:
						quantifierTypeID = 10;
						break;
					case Ferda.Modules.Boxes.LISpMinerTasks.KLTask.Quantifiers.Functional.MutualInformationNormalized.MutualInformationNormalizedBoxInfo.typeIdentifier:
						quantifierTypeID = 13;
						break;
					case Ferda.Modules.Boxes.LISpMinerTasks.KLTask.Quantifiers.Functional.InformationDependency.InformationDependencyBoxInfo.typeIdentifier:
						quantifierTypeID = 11;
						break;
					case Ferda.Modules.Boxes.LISpMinerTasks.KLTask.Quantifiers.Functional.Kendal.KendalBoxInfo.typeIdentifier:
						kendalQuantifier = true;
						quantifierTypeID = 14;
						break;
					default:
						throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(quantifier.typeIdentifier);
				}
				long autoIncrementValue = common.GetTableAutoIncrementValue(tableName, 1);
				string unitsColumn, unitsColumnValue;
				unitsColumn = unitsColumnValue = String.Empty;
				if (quantifierWithUnits)
				{
					unitsColumn = "KLQuantifierValueTypeID,";
					unitsColumnValue = common.Constants.UnitsEnumDictionary[units] + ",";
				}
                /*
                 * // this direction is not implemented by LMGens
                 * if (direction == DirectionEnum.RowsOnColumns)
                 *      ;
                 * */
				string tresholdColumn;
				if (quantifierWithRelativeTreshold)
				{
					tresholdColumn = "Threshold,";
				}
				else
				{
					tresholdColumn = "ValuePar,";
				}
				string kendalColumn, kendalColumnValue;
				kendalColumn = kendalColumnValue = String.Empty;
				if (kendalQuantifier)
				{
					kendalColumn = "KendalAbsValueTauB,";
					kendalColumnValue = common.Constants.TrueValue + ",";
				}
				string query = "INSERT INTO " + tableName + " (" + autoIncrementColumn
					+ ",TaskID,KLQuantifierTypeID,FromRow,ToRow,FromCol,ToCol," + unitsColumn + tresholdColumn + kendalColumn + "CompareTypeID) VALUES "
					+ "(" + autoIncrementValue + ","
					+ taskID + ","
					+ quantifierTypeID + ","
					+ fromRow + ","
					+ toRow + ","
					+ fromColumn + ","
					+ toColumn + ","
					+ unitsColumnValue
					+ "'" +treshold.ToString() + "'" + ","
					+ kendalColumnValue
					+ common.Constants.RelationEnumDictionary[relation]
					+ ")";
				common.ExecuteInsertQuery(query, tableName);
			}
		}

		protected override void readResults(int taskID, long allObjectsCount, object taskDescription, out GeneratingStruct generation, out HypothesisStruct[] result)
		{
			//Ferda.Modules.Boxes.LISpMinerTasks.KLTask.TaskStruct input = (TaskStruct)taskDescription;
			generation = common.GetGeneratingStruct(taskID);
			List<HypothesisStruct> hypothesesResult = new List<HypothesisStruct>();
			HypothesisStruct hypothesisStruct;
			AbstractQuantifierSetting quantifierSetting;
			DataTable hypothesis = common.ExecuteSelectQuery("SELECT * FROM tiHypothesisKL WHERE TaskID=" + taskID);
            LiteralStruct[] literals = common.GetCategorialLiterals(TaskTypeEnum.KL, taskID, taskDescription);
			foreach (DataRow hypothese in hypothesis.Rows)
			{
				int hypothesisID = Convert.ToInt32(hypothese["HypothesisID"]);
				hypothesisStruct = new HypothesisStruct();
				hypothesisStruct.booleanLiterals = common.GetBooleanLiterals(taskID, hypothesisID);
                int rowLiteralId = common.CategorialLiteral[Convert.ToInt32(hypothese["KLLiteralDRowID"])];
                LiteralStruct rowLiteral = new LiteralStruct();
                foreach(LiteralStruct literal in literals)
                {
                    if (literal.cedentType == CedentEnum.Antecedent && literal.literalIdentifier == rowLiteralId)
                        rowLiteral = literal;
                }
                int columnLiteralId = common.CategorialLiteral[Convert.ToInt32(hypothese["KLLiteralDColID"])];
				LiteralStruct columnLiteral = new LiteralStruct();
                foreach(LiteralStruct literal in literals)
                {
                    if (literal.cedentType == CedentEnum.Succedent && literal.literalIdentifier == columnLiteralId)
                        columnLiteral = literal;
                }
                hypothesisStruct.literals = new LiteralStruct[] { rowLiteral, columnLiteral};
                quantifierSetting = new AbstractQuantifierSetting();
				quantifierSetting.firstContingencyTableRows = common.GetContingecyTable(this.taskType, taskID, hypothesisID, rowLiteral.literalIdentifier, columnLiteral.literalIdentifier);
				quantifierSetting.secondContingencyTableRows = new int[0][];
				quantifierSetting.allObjectsCount = allObjectsCount;
				hypothesisStruct.quantifierSetting = quantifierSetting;
				hypothesesResult.Add(hypothesisStruct);
			}
			result = hypothesesResult.ToArray();
		}

		protected override BooleanCedent[] getBooleanCedents(object taskDescription)
		{
			Ferda.Modules.Boxes.LISpMinerTasks.KLTask.TaskStruct input = (TaskStruct)taskDescription;
			List<BooleanCedent> result = new List<BooleanCedent>();
			addBooleanCedents(input.conditionSetting, CedentEnum.Condition, ref result);
			return result.ToArray();
		}

		protected override CategorialCedent[] getCategorialCedents(object taskDescription)
		{
			Ferda.Modules.Boxes.LISpMinerTasks.KLTask.TaskStruct input = (TaskStruct)taskDescription;
			List<CategorialCedent> result = new List<CategorialCedent>();
			addCategorialCedent(input.antecedentSetting, CedentEnum.Antecedent, ref result);
			addCategorialCedent(input.succedentSetting, CedentEnum.Succedent, ref result);
			return result.ToArray();
		}

#if FFTTask_MAIN
            public static void Main(string[] args)
            {
				FFTTask task = new FFTTask();
				task.Prepare();
				task.FinalizeMe();
            }
#endif
	}
}
