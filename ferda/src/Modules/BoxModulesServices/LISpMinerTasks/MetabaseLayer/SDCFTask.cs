using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using Ferda.Modules.LMTasks;
using Ferda.Modules.Boxes.SDCFTask;
using Ferda.Modules.Boxes.AbstractLMTask;
using Ferda.Modules.Boxes.SDCFTask.Quantifiers;

namespace Ferda.Modules.MetabaseLayer
{
    class SDCFTask : Task
    {
        protected override string exeFileName
        {
            get
            {
                return "SDCFGen.exe";
            }
        }

        protected override TaskTypeEnum taskType
        {
            get
            {
                return TaskTypeEnum.SDCF;
            }
        }

        protected override BooleanCedent[] getBooleanCedents(object taskDescription)
        {
            Ferda.Modules.Boxes.SDCFTask.TaskStruct input = (TaskStruct)taskDescription;
            List<BooleanCedent> result = new List<BooleanCedent>();
            addBooleanCedents(input.conditionSetting, CedentEnum.Condition, ref result);
            addBooleanCedents(input.firstCedentSetting, CedentEnum.FirstSet, ref result);
            addBooleanCedents(input.secondCedentSetting, CedentEnum.SecondSet, ref result);
            return result.ToArray();
        }

        protected override CategorialCedent[] getCategorialCedents(object taskDescription)
        {
            Ferda.Modules.Boxes.SDCFTask.TaskStruct input = (TaskStruct)taskDescription;
            List<CategorialCedent> result = new List<CategorialCedent>();
            addCategorialCedent(input.antecedentSetting, CedentEnum.Antecedent, ref result);
            return result.ToArray();
        }


        protected override void saveTask(object taskStruct, int masterTaskID, string boxIdentity)
        {
            Ferda.Modules.Boxes.SDCFTask.TaskStruct taskDescription = (TaskStruct)taskStruct;

            string tableName = "taTaskDC";
            string autoIncrementColumn = common.GetAutoIncrementColumnName(tableName);
            long autoIncrementValue = common.GetTableAutoIncrementValue(tableName, 1);
            string query = "INSERT INTO " + tableName + " (" + autoIncrementColumn
                + ",TaskID,JoinedSet) VALUES "
                + "(" + autoIncrementValue + ","
                + masterTaskID + ","
                + common.Constants.SdJoinedSet(taskDescription.secondSet)
                + ")";
            common.ExecuteInsertQuery(query, tableName);
            saveQuantifiers(taskDescription.quantifiers, masterTaskID, boxIdentity);
        }

        protected override void readResults(int taskID, long allObjectsCount, object taskDescription, out GeneratingStruct generation, out HypothesisStruct[] result)
        {
            generation = common.GetGeneratingStruct(taskID);
            List<HypothesisStruct> hypothesesResult = new List<HypothesisStruct>();
            HypothesisStruct hypothesisStruct;
            AbstractQuantifierSetting quantifierSetting;
            DataTable hypothesis = common.ExecuteSelectQuery("SELECT * FROM tiHypothesisDC WHERE TaskID=" + taskID);
            foreach (DataRow hypothese in hypothesis.Rows)
            {
                int hypothesisID = Convert.ToInt32(hypothese["HypothesisID"]);
                hypothesisStruct = new HypothesisStruct();
                hypothesisStruct.boolenliterals = common.GetBooleanLiterals(taskID, hypothesisID);
                LiteralStruct columnLiteral = new LiteralStruct();
                LiteralStruct rowLiteral = new LiteralStruct();
                columnLiteral.literalDimension = LiteralDimensionEnum.SecondDimension;
                columnLiteral.literalIdentifier = common.CategorialLiteral[Convert.ToInt32(hypothese["CFLiteralDID"])];
                hypothesisStruct.literals = new LiteralStruct[] { rowLiteral, columnLiteral };

                quantifierSetting = new AbstractQuantifierSetting();
                quantifierSetting.firstContingencyTableRows = common.GetContingecyTable(this.taskType, taskID, hypothesisID, rowLiteral.literalIdentifier, columnLiteral.literalIdentifier);
                quantifierSetting.secondContingencyTableRows = common.GetSecondContingecyTable(this.taskType, taskID, hypothesisID, rowLiteral.literalIdentifier, columnLiteral.literalIdentifier);
                quantifierSetting.allObjectsCount = allObjectsCount;
                hypothesisStruct.quantifierSetting = quantifierSetting;
                hypothesesResult.Add(hypothesisStruct);
            }
            result = hypothesesResult.ToArray();
        }

        private void saveQuantifiers(QuantifierSettingStruct[] quantifiers, int taskID, string boxIdentity)
        {
            Dictionary<string, double> properties = new Dictionary<string, double>();
            PropertySettingHelper propertySettingHelper;

            string tableName = "tdDCQuantifier";
            string autoIncrementColumn = common.GetAutoIncrementColumnName(tableName);


            foreach (QuantifierSettingStruct quantifier in quantifiers)
            {
                propertySettingHelper = new PropertySettingHelper(quantifier.setting);
                OperationModeEnum operationMode = (OperationModeEnum)Enum.Parse(typeof(OperationModeEnum), propertySettingHelper.GetStringProperty("OperationMode"));
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

                RelationEnum relation = (RelationEnum)Enum.Parse(typeof(RelationEnum), propertySettingHelper.GetStringProperty("Relation"));
                double threshold = propertySettingHelper.GetDoubleProperty("Treshold");

                #region Quantifiers switch
                switch (quantifier.typeIdentifier)
                {
                    case Boxes.SDCFTask.Quantifiers.Aggregation.SumOfValues.SumOfValuesBoxInfo.typeIdentifier:
                        quantifierTypeID = 1;
                        break;

                    case Boxes.SDCFTask.Quantifiers.Aggregation.MinValue.MinValueBoxInfo.typeIdentifier:
                        quantifierTypeID = 2;
                        break;

                    case Boxes.SDCFTask.Quantifiers.Aggregation.MaxValue.MaxValueBoxInfo.typeIdentifier:
                        quantifierTypeID = 3;
                        break;

                    case Boxes.SDCFTask.Quantifiers.Aggregation.AverageValue.AverageValueBoxInfo.typeIdentifier:
                        quantifierTypeID = 4;
                        break;

                    case Boxes.SDCFTask.Quantifiers.Aggregation.AnyValue.AnyValueBoxInfo.typeIdentifier:
                        quantifierTypeID = 5;
                        break;

                    case Boxes.SDCFTask.Quantifiers.Functional.VariationRatio.VariationRatioBoxInfo.typeIdentifier:
                        quantifierTypeID = 8;
                        break;

                    case Boxes.SDCFTask.Quantifiers.Functional.NominalVariation.NominalVariationBoxInfo.typeIdentifier:
                        quantifierTypeID = 9;
                        break;

                    case Boxes.SDCFTask.Quantifiers.Functional.DiscreteOrdinaryVariation.DiscreteOrdinaryVariationBoxInfo.typeIdentifier:
                        quantifierTypeID = 10;
                        break;

                    case Boxes.SDCFTask.Quantifiers.Functional.ArithmeticAverage.ArithmeticAverageBoxInfo.typeIdentifier:
                        quantifierTypeID = 11;
                        break;

                    case Boxes.SDCFTask.Quantifiers.Functional.GeometricAverage.GeometricAverageBoxInfo.typeIdentifier:
                        quantifierTypeID = 12;
                        break;

                    case Boxes.SDCFTask.Quantifiers.Functional.Variance.VarianceBoxInfo.typeIdentifier:
                        quantifierTypeID = 13;
                        break;

                    case Boxes.SDCFTask.Quantifiers.Functional.StandardDeviation.StandardDeviationBoxInfo.typeIdentifier:
                        quantifierTypeID = 14;
                        break;

                    case Boxes.SDCFTask.Quantifiers.Functional.Skewness.SkewnessBoxInfo.typeIdentifier:
                        quantifierTypeID = 15;
                        break;

                    case Boxes.SDCFTask.Quantifiers.Functional.Asymetry.AsymetryBoxInfo.typeIdentifier:
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
                    fromColumn = Ferda.Modules.Quantifiers.ContingencyTable.ZeroBasedBoundFromOneBasedString(propertySettingHelper.GetStringProperty("CategoryRangeFrom"));
                    toColumn = Ferda.Modules.Quantifiers.ContingencyTable.ZeroBasedBoundFromOneBasedString(propertySettingHelper.GetStringProperty("CategoryRangeTo"));
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
                    aggregationQuantifierValue = "1, -1";
                }

                if (quantifierWithUnits)
                {
                    unitsColumn = "DCQuantifierValueTypeID,";
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
                    + ",TaskID,SDQuantifierSourceTypeID,DCQuantifierTypeID,FromCol,ToCol," + unitsColumn + thresholdColumn + "CompareTypeID) VALUES "
                    + "(" + autoIncrementValue + ","
                    + taskID + ","
                    + common.Constants.OperationModeEnumDictionary[operationMode] + ","
                    + quantifierTypeID + ","
                    + aggregationQuantifierValue
                    + unitsColumnValue
                    + "'" + threshold.ToString() + "'" + ","
                    + common.Constants.RelationEnumDictionary[relation]
                    + ")";
                /*
                Console.WriteLine("AutoIncrement: {0} ; {1}\n", autoIncrementColumn, autoIncrementValue);
                Console.WriteLine("TaskID: {0} \n", taskID);
                Console.WriteLine("SDQuantifierSourceTypeID: {0}\n", common.Constants.OperationModeEnumDictionary[operationMode]);
                Console.WriteLine("DCQuantifierTypeID: {0}\n", quantifierTypeID);
                Console.WriteLine("FromCol,ToCol: {0}\n", aggregationQuantifierValue);
                Console.WriteLine("UnitsColumn: {0} ; {1}\n", unitsColumn, unitsColumnValue);
                Console.WriteLine("Threshold: {0} ; {1}\n", thresholdColumn, threshold);
                Console.WriteLine("ValueTypeID: {0}\n", common.Constants.RelationEnumDictionary[relation]);


                Console.WriteLine(query);*/

                common.ExecuteInsertQuery(query, tableName);
            }
        }
    }
}
