//#define FFTTask_MAIN
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Odbc;
using System.Data;
using Ferda.Modules.Boxes.FFTTask;
using Ferda.Modules.Boxes.AbstractLMTask;

namespace Ferda.Modules.MetabaseLayer
{
	public class FFTTask : Task
	{
		protected override TaskTypeEnum taskType
		{
			get
			{
				return TaskTypeEnum.FFT;
			}
		}
		protected override string exeFileName
		{
			get
			{
				return "4ftGen.exe";
			}
		}
		protected override void saveTask(object input, int masterTaskID, string boxIdentity)
		{
			Ferda.Modules.Boxes.FFTTask.TaskStruct taskDescription = (TaskStruct)input;

			string tableName = "taTaskFT";
			string autoIncrementColumn = common.GetAutoIncrementColumnName(tableName);
			long autoIncrementValue = common.GetTableAutoIncrementValue(tableName, 1);
			string query = "INSERT INTO " + tableName + " (" + autoIncrementColumn
				+ ",TaskID,MissingsTypeID,Prolong100A,Prolong100S,IncludeSymetric) VALUES "
				+ "(" + autoIncrementValue + ","
				+ masterTaskID + ","
				+ common.Constants.MissingTypeEnumDictionary[taskDescription.missingType] + ","
				+ common.Constants.GetBool(taskDescription.prolong100A) + ","
				+ common.Constants.GetBool(taskDescription.prolong100S) + ","
				+ common.Constants.GetBool(taskDescription.includeSymetric)
				+ ")";
			common.ExecuteInsertQuery(query, tableName);
			saveQuantifiers(taskDescription.quantifiers, masterTaskID, boxIdentity);
		}

		private void saveQuantifier(int taskID, int quantifierType, Dictionary<string, double> parameters, bool relative)
		{
			string columns = "";
			string values = "";
			foreach (KeyValuePair<string, double> property in parameters)
			{
				columns = (String.IsNullOrEmpty(columns)) ? property.Key : columns + "," + property.Key;
				values = (String.IsNullOrEmpty(values)) ? "'" + property.Value.ToString() + "'" : values + "," + "'" + property.Value.ToString() + "'";
			}
			string tableName = "tdFTQuantifier";
			string autoIncrementColumn = common.GetAutoIncrementColumnName(tableName);
			long autoIncrementValue = common.GetTableAutoIncrementValue(tableName, 1);
			string query = "INSERT INTO " + tableName + " (" + autoIncrementColumn
				+ ",TaskID,FTQuantifierTypeID,ParamRelativ," + columns + ") VALUES "
				+ "(" + autoIncrementValue + ","
				+ taskID + ","
				+ quantifierType + ","
				+ common.Constants.GetBool(relative) + ","
				+ values
				+ ")";
			//ParamP,ParamAlfa,ParamBeta,ParamDelta
			common.ExecuteInsertQuery(query, tableName);
		}

		private void saveQuantifiers(QuantifierSettingStruct[] quantifiers, int taskID, string boxIdentity)
		{
			PropertySetting[] setting;
			Dictionary<string, double> properties = new Dictionary<string, double>();
			PropertySettingHelper propertySettingHelper;
			foreach (QuantifierSettingStruct quantifier in quantifiers)
			{
				setting = quantifier.setting;
				propertySettingHelper = new PropertySettingHelper(setting);
				properties.Clear();
				CoreRelationEnum relation;
				//ParamP,ParamAlfa,ParamBeta,ParamDelta
				switch (quantifier.typeIdentifier)
				{
					case Ferda.Modules.Boxes.FFTTask.Quantifiers.Functional.AboveAverageImplication.AboveAverageImplicationBoxInfo.typeIdentifier:
						//15  Above Average Implication          ;;;;p
						//ParamK
						properties.Add("ParamP", propertySettingHelper.GetDoubleProperty("ParamK"));
						saveQuantifier(taskID, 15, properties, false);
						break;
					case Ferda.Modules.Boxes.FFTTask.Quantifiers.Functional.BaseCeil.BaseCeilBoxInfo.typeIdentifier:
						//18  BASE                               ;;count;p;
						//19  Ceiling                            ;;count;p;
						//17  Support                            p;;;;
						//Relation,Treshold,Units
						relation = (CoreRelationEnum)Enum.Parse(typeof(CoreRelationEnum), propertySettingHelper.GetStringProperty("Relation"));
						CoreUnitsEnum units = (CoreUnitsEnum)Enum.Parse(typeof(CoreUnitsEnum), propertySettingHelper.GetStringProperty("Units"));
						bool relative = false;
						double treshold = propertySettingHelper.GetDoubleProperty("Treshold");
						switch (units)
						{
							case CoreUnitsEnum.AbsoluteNumberCore:
								properties.Add("ParamAlfa", treshold);
								relative = false;
								break;
							case CoreUnitsEnum.RelativeNumberCore:
								properties.Add("ParamBeta", treshold);
								relative = true;
								break;
						}
						switch (relation)
						{
							case CoreRelationEnum.GreaterThanOrEqualCore:
								saveQuantifier(taskID, 18, properties, relative);
								break;
							case CoreRelationEnum.LessThanOrEqualCore:
								saveQuantifier(taskID, 19, properties, relative);
								break;
						}
						break;
					case Ferda.Modules.Boxes.FFTTask.Quantifiers.Functional.BelowAverageImplication.BelowAverageImplicationBoxInfo.typeIdentifier:
						//16  Below Average Implication          ;;;;p
						//ParamK
						properties.Add("ParamP", propertySettingHelper.GetDoubleProperty("ParamK"));
						saveQuantifier(taskID, 16, properties, false);
						break;
					case Ferda.Modules.Boxes.FFTTask.Quantifiers.Functional.DoubleFoundedImplication.DoubleFoundedImplicationBoxInfo.typeIdentifier:
						//4   Double Founded Implication         p;
						//ParamP
						properties.Add("ParamP", propertySettingHelper.GetDoubleProperty("ParamP"));
						saveQuantifier(taskID, 4, properties, false);
						break;
					case Ferda.Modules.Boxes.FFTTask.Quantifiers.Functional.DoubleCriticalImplication.DoubleCriticalImplicationBoxInfo.typeIdentifier:
						//5   Double Lower Critical Implication  p;;Alpha
						//6   Double Upper Critical Implication  p;;Alpha
						//ParamP,ParamAlpha
						relation = (CoreRelationEnum)Enum.Parse(typeof(CoreRelationEnum), propertySettingHelper.GetStringProperty("Relation"));
						properties.Add("ParamP", propertySettingHelper.GetDoubleProperty("ParamP"));
						properties.Add("ParamAlfa", propertySettingHelper.GetDoubleProperty("ParamAlpha"));
						switch (relation)
						{
							case CoreRelationEnum.GreaterThanOrEqualCore:
								saveQuantifier(taskID, 6, properties, false);
								break;
							case CoreRelationEnum.LessThanOrEqualCore:
								saveQuantifier(taskID, 5, properties, false);
								break;
						}
						break;
					case Ferda.Modules.Boxes.FFTTask.Quantifiers.Functional.E.EBoxInfo.typeIdentifier:
						//14  E-quantifier                       ;;Delta
						//ParamP
						properties.Add("ParamDelta", propertySettingHelper.GetDoubleProperty("ParamP"));
						saveQuantifier(taskID, 4, properties, false);
						break;
					case Ferda.Modules.Boxes.FFTTask.Quantifiers.Functional.Fisher.FisherBoxInfo.typeIdentifier:
						//11  Fisher quantifier                  ;;Alpha
						//ParamAlpha
						properties.Add("ParamAlfa", propertySettingHelper.GetDoubleProperty("ParamAlpha"));
						saveQuantifier(taskID, 11, properties, false);
						break;
					case Ferda.Modules.Boxes.FFTTask.Quantifiers.Functional.FoundedEquivalence.FoundedEquivalenceBoxInfo.typeIdentifier:
						//7   Founded Equivalence                p;
						//ParamP
						properties.Add("ParamP", propertySettingHelper.GetDoubleProperty("ParamP"));
						saveQuantifier(taskID, 7, properties, false);
						break;
					case Ferda.Modules.Boxes.FFTTask.Quantifiers.Functional.FoundedImplication.FoundedImplicationBoxInfo.typeIdentifier:
						//1   Founded Implication                p;
						//ParamP
						properties.Add("ParamP", propertySettingHelper.GetDoubleProperty("ParamP"));
						saveQuantifier(taskID, 1, properties, false);
						break;
					case Ferda.Modules.Boxes.FFTTask.Quantifiers.Functional.ChiSquared.ChiSquaredBoxInfo.typeIdentifier:
						//12  Chi-Squared quantifier             ;;Alpha
						//ParamAlpha
						properties.Add("ParamAlfa", propertySettingHelper.GetDoubleProperty("ParamAlpha"));
						saveQuantifier(taskID, 12, properties, false);
						break;
					case Ferda.Modules.Boxes.FFTTask.Quantifiers.Functional.CriticalEquivalence.CriticalEquivalenceBoxInfo.typeIdentifier:
						//8   Lower Critical Equivalence         p;;Alpha
						//9   Upper Critical Equivalence         p;;Alpha
						//ParamP,ParamAlpha
						relation = (CoreRelationEnum)Enum.Parse(typeof(CoreRelationEnum), propertySettingHelper.GetStringProperty("Relation"));
						properties.Add("ParamP", propertySettingHelper.GetDoubleProperty("ParamP"));
						properties.Add("ParamAlfa", propertySettingHelper.GetDoubleProperty("ParamAlpha"));
						saveQuantifier(taskID, 8, properties, false);
						switch (relation)
						{
							case CoreRelationEnum.GreaterThanOrEqualCore:
								saveQuantifier(taskID, 9, properties, false);
								break;
							case CoreRelationEnum.LessThanOrEqualCore:
								saveQuantifier(taskID, 8, properties, false);
								break;
						}
						break;
					case Ferda.Modules.Boxes.FFTTask.Quantifiers.Functional.CriticalImplication.CriticalImplicationBoxInfo.typeIdentifier:
						//2   Lower Critical Implication         p;;Alpha
						//3   Upper Critical Implication         p;;Alpha
						//ParamP,ParamAlpha
						relation = (CoreRelationEnum)Enum.Parse(typeof(CoreRelationEnum), propertySettingHelper.GetStringProperty("Relation"));
						properties.Add("ParamP", propertySettingHelper.GetDoubleProperty("ParamP"));
						properties.Add("ParamAlfa", propertySettingHelper.GetDoubleProperty("ParamAlpha"));
						switch (relation)
						{
							case CoreRelationEnum.GreaterThanOrEqualCore:
								saveQuantifier(taskID, 3, properties, false);
								break;
							case CoreRelationEnum.LessThanOrEqualCore:
								saveQuantifier(taskID, 2, properties, false);
								break;
						}
						break;
					case Ferda.Modules.Boxes.FFTTask.Quantifiers.Functional.SimpleDeviation.SimpleDeviationBoxInfo.typeIdentifier:
						//10  Simple Deviation                   ;;;;Delta
						//ParamK
						properties.Add("ParamDelta", propertySettingHelper.GetDoubleProperty("ParamK"));
						saveQuantifier(taskID, 10, properties, false);
						break;
					default:
						throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(quantifier.typeIdentifier);
				}
			}
		}

		protected override void readResults(int taskID, long allObjectsCount, object taskDescription, out GeneratingStruct generation, out HypothesisStruct[] result)
		{
			generation = common.GetGeneratingStruct(taskID);
			List<HypothesisStruct> hypothesesResult = new List<HypothesisStruct>();
			HypothesisStruct hypothesisStruct;
			AbstractQuantifierSetting quantifierSetting;
			DataTable hypothesis = common.ExecuteSelectQuery("SELECT * FROM tiHypothesis WHERE TaskID=" + taskID);
			foreach (DataRow hypothese in hypothesis.Rows)
			{
				hypothesisStruct = new HypothesisStruct();
				hypothesisStruct.boolenliterals = common.GetBooleanLiterals(taskID, Convert.ToInt32(hypothese["HypothesisID"]));
				hypothesisStruct.literals = new LiteralStruct[0];
				quantifierSetting = new AbstractQuantifierSetting();
				quantifierSetting.firstContingencyTableRows = new int[2][];
				quantifierSetting.firstContingencyTableRows[0] = new int[2] { Convert.ToInt32(hypothese["FreqA"]), Convert.ToInt32(hypothese["FreqB"]) };
				quantifierSetting.firstContingencyTableRows[1] = new int[2] { Convert.ToInt32(hypothese["FreqC"]), Convert.ToInt32(hypothese["FreqD"]) };
				quantifierSetting.secondContingencyTableRows = new int[0][];
				quantifierSetting.allObjectsCount = allObjectsCount;
				hypothesisStruct.quantifierSetting = quantifierSetting;
				hypothesesResult.Add(hypothesisStruct);
			}
			result = hypothesesResult.ToArray();
		}

		protected override BooleanCedent[] getBooleanCedents(object taskDescription)
		{
			Ferda.Modules.Boxes.FFTTask.TaskStruct input = (TaskStruct)taskDescription;
			List<BooleanCedent> result = new List<BooleanCedent>();
			addBooleanCedents(input.antecedentSetting, CedentEnum.Antecedent, ref result);
			addBooleanCedents(input.succedentSetting, CedentEnum.Succedent, ref result);
			addBooleanCedents(input.conditionSetting, CedentEnum.Condition, ref result);
			return result.ToArray();
		}

		protected override CategorialCedent[] getCategorialCedents(object taskDescription)
		{
			return new CategorialCedent[0];
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
