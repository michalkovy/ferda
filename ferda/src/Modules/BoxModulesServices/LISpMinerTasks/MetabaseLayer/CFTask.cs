using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Odbc;
using System.Data;
using Ferda.Modules.Boxes.LISpMinerTasks.CFTask;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractLMTask;
using Ferda.Modules.Boxes.LISpMinerTasks.CFTask.Quantifiers;


namespace Ferda.Modules.MetabaseLayer
{
    public class CFTask : Task
    {
        protected override string exeFileName
        {
            get
            {
                return "CFGen.exe";
            }
        }

        protected override TaskTypeEnum taskType
        {
            get
            {
                return TaskTypeEnum.CF;
            }
        }

        protected override BooleanCedent[] getBooleanCedents(object taskDescription)
        {
            Ferda.Modules.Boxes.LISpMinerTasks.CFTask.TaskStruct input = (TaskStruct)taskDescription;
            List<BooleanCedent> result = new List<BooleanCedent>();
            addBooleanCedents(input.conditionSetting, CedentEnum.Condition, ref result);
            return result.ToArray();
        }

        protected override CategorialCedent[] getCategorialCedents(object taskDescription)
        {
            Ferda.Modules.Boxes.LISpMinerTasks.CFTask.TaskStruct input = (TaskStruct)taskDescription;

            List<CategorialCedent> result = new List<CategorialCedent>();
            addCategorialCedent(input.antecedentSetting, CedentEnum.Antecedent, ref result);
            return result.ToArray();
        }

        protected override void readResults(int taskID, long allObjectsCount, object taskDescription, out GeneratingStruct generation, out HypothesisStruct[] result)
        {
            generation = common.GetGeneratingStruct(taskID);
            List<HypothesisStruct> hypothesesResult = new List<HypothesisStruct>();
            HypothesisStruct hypothesisStruct;
            AbstractQuantifierSetting quantifierSetting;
            DataTable hypothesis = common.ExecuteSelectQuery("SELECT * FROM tiHypothesisCF WHERE TaskID=" + taskID);
            foreach (DataRow hypothese in hypothesis.Rows)
            {
                int hypothesisID = Convert.ToInt32(hypothese["HypothesisID"]);
                hypothesisStruct = new HypothesisStruct();
                hypothesisStruct.booleanLiterals = common.GetBooleanLiterals(taskID, hypothesisID);
                hypothesisStruct.literals = common.GetCategorialLiterals(TaskTypeEnum.CF, taskID, hypothesisID, taskDescription);
           //     hypothesisStruct.literals = common.Get
                //  hypothesisStruct.literals = common.Get
                LiteralStruct columnLiteral = new LiteralStruct();
                columnLiteral.cedentType = CedentEnum.Antecedent;
                columnLiteral.literalIdentifier = common.CategorialLiteral[Convert.ToInt32(hypothese["CFLiteralDID"])];
               // hypothesisStruct.literals = new LiteralStruct[] { rowLiteral, columnLiteral };

                quantifierSetting = new AbstractQuantifierSetting();
                quantifierSetting.firstContingencyTableRows = common.GetContingecyTable(this.taskType, taskID, hypothesisID, 0, columnLiteral.literalIdentifier);
                quantifierSetting.secondContingencyTableRows = new int[0][];
                quantifierSetting.allObjectsCount = allObjectsCount;
                hypothesisStruct.quantifierSetting = quantifierSetting;
                hypothesesResult.Add(hypothesisStruct);
            }
            result = hypothesesResult.ToArray();
        }

        protected override void saveTask(object taskStruct, int masterTaskID, string boxIdentity)
        {
            Ferda.Modules.Boxes.LISpMinerTasks.CFTask.TaskStruct taskDescription = (TaskStruct)taskStruct;

            string tableName = "taTaskCF";
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
            Dictionary<string, double> properties = new Dictionary<string, double>();
            PropertySettingHelper propertySettingHelper;

            string tableName = "tdCFQuantifier";
            string autoIncrementColumn = common.GetAutoIncrementColumnName(tableName);



            foreach (QuantifierSettingStruct quantifier in quantifiers)
            {
                propertySettingHelper = new PropertySettingHelper(quantifier.setting);
                UnitsEnum units = UnitsEnum.AbsoluteNumber;
                bool quantifierWithUnits = true;
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
                bool aggregationQuantifierFlag = true;
                int fromColumn = 0;
                int toColumn = 0;
                int quantifierTypeID = 0;

                RelationEnum relation = RelationEnum.Equal;

                try
                {
                    relation = (RelationEnum)Enum.Parse(typeof(RelationEnum), propertySettingHelper.GetStringProperty("Relation"));
                }
                catch (ArgumentException) { }
                double threshold = propertySettingHelper.GetDoubleProperty("Treshold");

                #region Quantifiers switch
                switch (quantifier.typeIdentifier)
                {
                    case Boxes.LISpMinerTasks.CFTask.Quantifiers.Aggregation.SumOfValues.SumOfValuesBoxInfo.typeIdentifier:
                        quantifierTypeID = 1;
                        break;

                    case Boxes.LISpMinerTasks.CFTask.Quantifiers.Aggregation.MinValue.MinValueBoxInfo.typeIdentifier:
                        quantifierTypeID = 2;
                        break;

                    case Boxes.LISpMinerTasks.CFTask.Quantifiers.Aggregation.MaxValue.MaxValueBoxInfo.typeIdentifier:
                        quantifierTypeID = 3;
                        break;

                    case Boxes.LISpMinerTasks.CFTask.Quantifiers.Aggregation.AverageValue.AverageValueBoxInfo.typeIdentifier:
                        quantifierTypeID = 4;
                        break;

                    case Boxes.LISpMinerTasks.CFTask.Quantifiers.Aggregation.AnyValue.AnyValueBoxInfo.typeIdentifier:
                        quantifierTypeID = 5;
                        break;

                    case Boxes.LISpMinerTasks.CFTask.Quantifiers.Functional.VariationRatio.VariationRatioBoxInfo.typeIdentifier:
                        quantifierTypeID = 8;
                        break;

                    case Boxes.LISpMinerTasks.CFTask.Quantifiers.Functional.NominalVariation.NominalVariationBoxInfo.typeIdentifier:
                        quantifierTypeID = 9;
                        break;

                    case Boxes.LISpMinerTasks.CFTask.Quantifiers.Functional.DiscreteOrdinaryVariation.DiscreteOrdinaryVariationBoxInfo.typeIdentifier:
                        quantifierTypeID = 10;
                        break;

                    case Boxes.LISpMinerTasks.CFTask.Quantifiers.Functional.ArithmeticAverage.ArithmeticAverageBoxInfo.typeIdentifier:
                        quantifierTypeID = 11;
                        break;

                    case Boxes.LISpMinerTasks.CFTask.Quantifiers.Functional.GeometricAverage.GeometricAverageBoxInfo.typeIdentifier:
                        quantifierTypeID = 12;
                        break;

                    case Boxes.LISpMinerTasks.CFTask.Quantifiers.Functional.Variance.VarianceBoxInfo.typeIdentifier:
                        quantifierTypeID = 13;
                        break;

                    case Boxes.LISpMinerTasks.CFTask.Quantifiers.Functional.StandardDeviation.StandardDeviationBoxInfo.typeIdentifier:
                        quantifierTypeID = 14;
                        break;

                    case Boxes.LISpMinerTasks.CFTask.Quantifiers.Functional.Skewness.SkewnessBoxInfo.typeIdentifier:
                        quantifierTypeID = 15;
                        break;

                    case Boxes.LISpMinerTasks.CFTask.Quantifiers.Functional.Asymetry.AsymetryBoxInfo.typeIdentifier:
                        quantifierTypeID = 16;
                        break;

                    default:
                        throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(quantifier.typeIdentifier);
                }

                #endregion

                long autoIncrementValue = common.GetTableAutoIncrementValue(tableName, 1);
                string unitsColumn, unitsColumnValue, aggregationQuantifier, aggregationQuantifierValue;
                unitsColumn = unitsColumnValue = aggregationQuantifier = aggregationQuantifierValue = String.Empty;

                try
                {
                    units = (UnitsEnum)Enum.Parse(typeof(UnitsEnum), propertySettingHelper.GetStringProperty("Units"));
                }
                catch (ArgumentException)
                {
                    quantifierWithUnits = false;
                }

                try
                {
                    fromColumn = Ferda.Modules.Helpers.Common.Parsing.ZeroBasedBoundFromOneBasedString(propertySettingHelper.GetStringProperty("CategoryRangeFrom"));
                    toColumn = Ferda.Modules.Helpers.Common.Parsing.ZeroBasedBoundFromOneBasedString(propertySettingHelper.GetStringProperty("CategoryRangeTo"));
                }
                catch (ArgumentException)
                {
                    aggregationQuantifierFlag = false;
                }

                if (aggregationQuantifierFlag)
                {
                    //aggregationQuantifier = "FromCol,ToCol,";
                    aggregationQuantifierValue = fromColumn.ToString() + "," + toColumn.ToString() + ",";
                }
                else
                {
                    aggregationQuantifierValue = "1, -1, ";
                }

                if (quantifierWithUnits)
                {
                    unitsColumn = "CFQuantifierValueTypeID,";
                    unitsColumnValue = common.Constants.UnitsEnumDictionary[units] + ",";
                }

                string thresholdColumn;
                if (quantifierWithRelativeTreshold)
                {
                    thresholdColumn = "Threshold,";
                }
                else
                {
                    thresholdColumn = "ValuePar,";
                }

                string query = "INSERT INTO " + tableName + " (" + autoIncrementColumn
                    + ",TaskID,CFQuantifierTypeID,FromCol,ToCol," + unitsColumn + thresholdColumn + "CompareTypeID) VALUES "
                    + "(" + autoIncrementValue + ","
                    + taskID + ","
                    + quantifierTypeID + ","
                    + aggregationQuantifierValue
                    + unitsColumnValue
                    + "'" + threshold.ToString() + "'" + ","
                    + common.Constants.RelationEnumDictionary[relation]
                    + ")";

                common.ExecuteInsertQuery(query, tableName);
            }
        }
    }
}
