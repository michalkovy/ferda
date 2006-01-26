//#define FFTTask_MAIN
using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Modules.Boxes.LISpMinerTasks.SDFFTTask;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractQuantifier;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractLMTask;
using System.Data.Odbc;
using System.Data;
using Ferda.Modules.Boxes.DataMiningCommon.BooleanPartialCedentSetting;
using Ferda.Modules.Boxes;
using System.IO;
using System.Diagnostics;

namespace Ferda.Modules.MetabaseLayer
{
	public class SDFFTTask : Task
	{
		protected override TaskTypeEnum taskType
		{
			get
			{
				return TaskTypeEnum.SDFFT;
			}
		}
		protected override string exeFileName
		{
			get
			{
				return "SD4ftGen.exe";
			}
		}

		protected override void saveTask(object input, int masterTaskID, string boxIdentity)
		{
			Ferda.Modules.Boxes.LISpMinerTasks.SDFFTTask.TaskStruct taskDescription = (TaskStruct)input;
			string tableName = "taTaskDF";
			string autoIncrementColumn = common.GetAutoIncrementColumnName(tableName);
			long autoIncrementValue = common.GetTableAutoIncrementValue(tableName, 1);
			string query = "INSERT INTO " + tableName + " (" + autoIncrementColumn
				+ ",TaskID,MissingsTypeID,JoinedSet) VALUES "
				+ "(" + autoIncrementValue + ","
				+ masterTaskID + ","
				+ common.Constants.MissingTypeEnumDictionary[taskDescription.missingType] + ","
				+ common.Constants.SdJoinedSet(taskDescription.secondSet)
				+ ")";
			common.ExecuteInsertQuery(query, tableName);
			saveQuantifiers(taskDescription.quantifiers, masterTaskID, boxIdentity);
		}

		private void saveQuantifiers(QuantifierSettingStruct[] quantifiers, int taskID, string boxIdentity)
		{
			PropertySettingHelper propertySettingHelper;
			string tableName = "tdDFQuantifier";
			string autoIncrementColumn = common.GetAutoIncrementColumnName(tableName);
			foreach (QuantifierSettingStruct quantifier in quantifiers)
			{
				propertySettingHelper = new PropertySettingHelper(quantifier.setting);
				OperationModeEnum operationMode = (OperationModeEnum)Enum.Parse(typeof(OperationModeEnum), propertySettingHelper.GetStringProperty("OperationMode"));
				RelationEnum relation = (RelationEnum)Enum.Parse(typeof(RelationEnum), propertySettingHelper.GetStringProperty("Relation"));
				UnitsEnum units = UnitsEnum.AbsoluteNumber;
				try
				{
					units = (UnitsEnum)Enum.Parse(typeof(UnitsEnum), propertySettingHelper.GetStringProperty("Units"));
				}
				catch
				{
					units = UnitsEnum.AbsoluteNumber;
				}
				double treshold = propertySettingHelper.GetDoubleProperty("Treshold");
				int quantifierTypeID = 0;

                #region Quantifier switch
                switch (quantifier.typeIdentifier)
				{
					case Ferda.Modules.Boxes.LISpMinerTasks.SDFFTTask.Quantifiers.Aggregation.SumOfValues.SumOfValuesBoxInfo.typeIdentifier:
						quantifierTypeID = 1;
						break;
					case Ferda.Modules.Boxes.LISpMinerTasks.SDFFTTask.Quantifiers.Aggregation.MinValue.MinValueBoxInfo.typeIdentifier:
						quantifierTypeID = 2;
						break;
					case Ferda.Modules.Boxes.LISpMinerTasks.SDFFTTask.Quantifiers.Aggregation.MaxValue.MaxValueBoxInfo.typeIdentifier:
						quantifierTypeID = 3;
						break;
					case Ferda.Modules.Boxes.LISpMinerTasks.SDFFTTask.Quantifiers.Aggregation.BaseCeil.BaseCeilBoxInfo.typeIdentifier:
						quantifierTypeID = 20;
						break;
					case Ferda.Modules.Boxes.LISpMinerTasks.SDFFTTask.Quantifiers.Functional.FoundedImplication.FoundedImplicationBoxInfo.typeIdentifier:
						units = UnitsEnum.AbsoluteNumber;
						quantifierTypeID = 4;
						break;
					case Ferda.Modules.Boxes.LISpMinerTasks.SDFFTTask.Quantifiers.Functional.AboveAverageImplication.AboveAverageImplicationBoxInfo.typeIdentifier:
						units = UnitsEnum.AbsoluteNumber;
						quantifierTypeID = 5;
						break;
					case Ferda.Modules.Boxes.LISpMinerTasks.SDFFTTask.Quantifiers.Functional.DoubleFoundedImplication.DoubleFoundedImplicationBoxInfo.typeIdentifier:
						units = UnitsEnum.AbsoluteNumber;
						quantifierTypeID = 9;
						break;
					case Ferda.Modules.Boxes.LISpMinerTasks.SDFFTTask.Quantifiers.Functional.FoundedEquivalence.FoundedEquivalenceBoxInfo.typeIdentifier:
						units = UnitsEnum.AbsoluteNumber;
						quantifierTypeID = 12;
						break;
					default:
						throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(quantifier.typeIdentifier);
                }

                #endregion

                long autoIncrementValue = common.GetTableAutoIncrementValue(tableName, 1);
				string query = "INSERT INTO " + tableName + " (" + autoIncrementColumn
					+ ",TaskID,SDQuantifierSourceTypeID,DFQuantifierTypeID,DFQuantifierValueTypeID,ValuePar,CompareTypeID) VALUES "
					+ "(" + autoIncrementValue + ","
					+ taskID + ","
					+ common.Constants.OperationModeEnumDictionary[operationMode] + ","
					+ quantifierTypeID + ","
					+ common.Constants.UnitsEnumDictionary[units] + ","
					+ "'" + treshold + "',"
					+ common.Constants.RelationEnumDictionary[relation]
					+ ")";
				common.ExecuteInsertQuery(query, tableName);
			}
		}

		protected override void readResults(int taskID, long allObjectsCount, object taskDescription, out GeneratingStruct generation, out HypothesisStruct[] result)
		{
			generation = common.GetGeneratingStruct(taskID);
			List<HypothesisStruct> hypothesesResult = new List<HypothesisStruct>();
			HypothesisStruct hypothesisStruct;
			AbstractQuantifierSetting quantifierSetting;
			DataTable hypothesis = common.ExecuteSelectQuery("SELECT * FROM tiHypothesisDF WHERE TaskID=" + taskID);
			foreach (DataRow hypothese in hypothesis.Rows)
			{
				hypothesisStruct = new HypothesisStruct();
				hypothesisStruct.boolenliterals = common.GetBooleanLiterals(taskID, Convert.ToInt32(hypothese["HypothesisID"]));
				hypothesisStruct.literals = new LiteralStruct[0];
				quantifierSetting = new AbstractQuantifierSetting();
				quantifierSetting.firstContingencyTableRows = new int[2][];
				quantifierSetting.firstContingencyTableRows[0] = new int[2] { Convert.ToInt32(hypothese["FirstFreqA"]), Convert.ToInt32(hypothese["FirstFreqB"]) };
				quantifierSetting.firstContingencyTableRows[1] = new int[2] { Convert.ToInt32(hypothese["FirstFreqC"]), Convert.ToInt32(hypothese["FirstFreqD"]) };
				quantifierSetting.secondContingencyTableRows = new int[2][];
				quantifierSetting.secondContingencyTableRows[0] = new int[2] { Convert.ToInt32(hypothese["SecondFreqA"]), Convert.ToInt32(hypothese["SecondFreqB"]) };
				quantifierSetting.secondContingencyTableRows[1] = new int[2] { Convert.ToInt32(hypothese["SecondFreqC"]), Convert.ToInt32(hypothese["SecondFreqD"]) };
				quantifierSetting.allObjectsCount = allObjectsCount;
				hypothesisStruct.quantifierSetting = quantifierSetting;
				hypothesesResult.Add(hypothesisStruct);
			}
			result = hypothesesResult.ToArray();
		}

		protected override BooleanCedent[] getBooleanCedents(object taskDescription)
		{
			Ferda.Modules.Boxes.LISpMinerTasks.SDFFTTask.TaskStruct input = (TaskStruct)taskDescription;
			List<BooleanCedent> result = new List<BooleanCedent>();
			addBooleanCedents(input.antecedentSetting, CedentEnum.Antecedent, ref result);
			addBooleanCedents(input.succedentSetting, CedentEnum.Succedent, ref result);
			addBooleanCedents(input.conditionSetting, CedentEnum.Condition, ref result);
			addBooleanCedents(input.firstCedentSetting, CedentEnum.FirstSet, ref result);
			addBooleanCedents(input.secondCedentSetting, CedentEnum.SecondSet, ref result);
			return result.ToArray();
		}

		protected override CategorialCedent[] getCategorialCedents(object taskDescription)
		{
			return new CategorialCedent[0];
		}
	}
}
