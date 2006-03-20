using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using Ferda.Modules.Boxes.LISpMinerTasks.SDKLTask;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractLMTask;
using Ferda.Modules.Boxes.LISpMinerTasks.SDKLTask.Quantifiers;

namespace Ferda.Modules.MetabaseLayer
{
    class SDKLTask : Task
    {
        protected override string exeFileName
        {
            get
            {
                return "SDKLGen.exe";
            }
        }

        protected override TaskTypeEnum taskType
        {
            get
            {
                return TaskTypeEnum.SDKL;
            }
        }

        protected override BooleanCedent[] getBooleanCedents(object taskDescription)
        {
            Ferda.Modules.Boxes.LISpMinerTasks.SDKLTask.TaskStruct input = (TaskStruct)taskDescription;
            List<BooleanCedent> result = new List<BooleanCedent>();
            addBooleanCedents(input.conditionSetting, CedentEnum.Condition, ref result);
            addBooleanCedents(input.firstCedentSetting, CedentEnum.FirstSet, ref result);
            return result.ToArray();
        }

        protected override CategorialCedent[] getCategorialCedents(object taskDescription)
        {
            Ferda.Modules.Boxes.LISpMinerTasks.SDKLTask.TaskStruct input = (TaskStruct)taskDescription;
            List<CategorialCedent> result = new List<CategorialCedent>();
            addCategorialCedent(input.antecedentSetting, CedentEnum.Antecedent, ref result);
            addCategorialCedent(input.succedentSetting, CedentEnum.Succedent, ref result);
            return result.ToArray();
        }

        protected override void readResults(int taskID, long allObjectsCount, object taskDescription, out GeneratingStruct generation, out HypothesisStruct[] result)
        {
            List<HypothesisStruct> hypothesesResult = new List<HypothesisStruct>();
            HypothesisStruct hypothesisStruct;
            AbstractQuantifierSetting quantifierSetting;
            generation = common.GetGeneratingStruct(taskID);
            DataTable hypothesis = common.ExecuteSelectQuery("SELECT * FROM tiHypothesisDK WHERE TaskID=" + taskID);
            foreach (DataRow hypothese in hypothesis.Rows)
            {
                int hypothesisID = Convert.ToInt32(hypothese["HypothesisID"]);
                hypothesisStruct = new HypothesisStruct();
                hypothesisStruct.booleanLiterals = common.GetBooleanLiterals(taskID, hypothesisID);
                
                hypothesisStruct.literals = common.GetCategorialLiterals(TaskTypeEnum.SDKL, taskID, hypothesisID, taskDescription);
                LiteralStruct rowLiteral = new LiteralStruct();
                rowLiteral.cedentType = CedentEnum.Antecedent;
                rowLiteral.literalIdentifier = common.CategorialLiteral[Convert.ToInt32(hypothese["KLLiteralDRowID"])];
                LiteralStruct columnLiteral = new LiteralStruct();
                columnLiteral.cedentType = CedentEnum.Succedent;
                columnLiteral.literalIdentifier = common.CategorialLiteral[Convert.ToInt32(hypothese["KLLiteralDColID"])];
                quantifierSetting = new AbstractQuantifierSetting();
                quantifierSetting.firstContingencyTableRows = common.GetContingecyTable(this.taskType, taskID, hypothesisID, rowLiteral.literalIdentifier, columnLiteral.literalIdentifier);

                quantifierSetting.secondContingencyTableRows = common.GetSecondContingecyTable(this.taskType, taskID, hypothesisID, rowLiteral.literalIdentifier, columnLiteral.literalIdentifier);
                quantifierSetting.allObjectsCount = allObjectsCount;
                hypothesisStruct.quantifierSetting = quantifierSetting;
                hypothesesResult.Add(hypothesisStruct);
            }
            result = hypothesesResult.ToArray();
        }

        protected override void saveTask(object taskStruct, int masterTaskID, string boxIdentity)
        {
            Ferda.Modules.Boxes.LISpMinerTasks.SDKLTask.TaskStruct taskDescription = (TaskStruct)taskStruct;

            string tableName = "taTaskDK";
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

        private void saveQuantifiers(QuantifierSettingStruct[] quantifiers, int taskID, string boxIdentity)
        {
            PropertySettingHelper propertySettingHelper;
            string tableName = "tdDKQuantifier";
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
                OperationModeEnum operationMode = (OperationModeEnum)Enum.Parse(typeof(OperationModeEnum), propertySettingHelper.GetStringProperty("OperationMode"));

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
                    case Ferda.Modules.Boxes.LISpMinerTasks.SDKLTask.Quantifiers.Aggregation.SumOfValues.SumOfValuesBoxInfo.typeIdentifier:
                        quantifierTypeID = 1;
                        break;
                    case Ferda.Modules.Boxes.LISpMinerTasks.SDKLTask.Quantifiers.Aggregation.MinValue.MinValueBoxInfo.typeIdentifier:
                        quantifierTypeID = 4;
                        break;
                    case Ferda.Modules.Boxes.LISpMinerTasks.SDKLTask.Quantifiers.Aggregation.MaxValue.MaxValueBoxInfo.typeIdentifier:
                        quantifierTypeID = 5;
                        break;
                    case Ferda.Modules.Boxes.LISpMinerTasks.SDKLTask.Quantifiers.Aggregation.AverageValue.AverageValueBoxInfo.typeIdentifier:
                        quantifierTypeID = 6;
                        break;
                    case Ferda.Modules.Boxes.LISpMinerTasks.SDKLTask.Quantifiers.Aggregation.AnyValue.AnyValueBoxInfo.typeIdentifier:
                        quantifierTypeID = 7;
                        break;
                    /* 
                     * (not implemented yet)
                     * usefull in future if somebody will be implementing functional quatnifiers for SDKL Miner
                     * no guarantee for rightness of the list of quantifiers below
                     * 
                    case Ferda.Modules.Boxes.LISpMinerTasks.SDKLTask.Quantifiers.Functional.FunctionSumOfRows.FunctionSumOfRowsBoxInfo.typeIdentifier:
                        quantifierWithRelativeTreshold = true;
                        quantifierTypeID = 9;
                        break;
                    case Ferda.Modules.Boxes.LISpMinerTasks.SDKLTask.Quantifiers.Functional.FunctionEachRow.FunctionEachRowBoxInfo.typeIdentifier:
                        quantifierWithRelativeTreshold = true;
                        quantifierTypeID = 10;
                        break;
                    case Ferda.Modules.Boxes.LISpMinerTasks.SDKLTask.Quantifiers.Functional.ConditionalEntropy.ConditionalEntropyBoxInfo.typeIdentifier:
                        quantifierTypeID = 10;
                        break;
                    case Ferda.Modules.Boxes.LISpMinerTasks.SDKLTask.Quantifiers.Functional.MutualInformationNormalized.MutualInformationNormalizedBoxInfo.typeIdentifier:
                        quantifierTypeID = 13;
                        break;
                    case Ferda.Modules.Boxes.LISpMinerTasks.SDKLTask.Quantifiers.Functional.InformationDependency.InformationDependencyBoxInfo.typeIdentifier:
                        quantifierTypeID = 11;
                        break;
                    case Ferda.Modules.Boxes.LISpMinerTasks.SDKLTask.Quantifiers.Functional.Kendal.KendalBoxInfo.typeIdentifier:
                        kendalQuantifier = true;
                        quantifierTypeID = 14;
                        break;
                    */
                    default:
                        throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(quantifier.typeIdentifier);
                }
                long autoIncrementValue = common.GetTableAutoIncrementValue(tableName, 1);
                string unitsColumn, unitsColumnValue;
                unitsColumn = unitsColumnValue = String.Empty;
                if (quantifierWithUnits)
                {
                    unitsColumn = "DKQuantifierValueTypeID,";
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
                    + ",TaskID,SDQuantifierSourceTypeID,DKQuantifierTypeID,FromRow,ToRow,FromCol,ToCol," + unitsColumn + tresholdColumn + kendalColumn + "CompareTypeID) VALUES "
                    + "(" + autoIncrementValue + ","
                    + taskID + ","
                    + common.Constants.OperationModeEnumDictionary[operationMode] + ","
                    + quantifierTypeID + ","
                    + fromRow + ","
                    + toRow + ","
                    + fromColumn + ","
                    + toColumn + ","
                    + unitsColumnValue
                    + "'" + treshold.ToString() + "'" + ","
                    + kendalColumnValue
                    + common.Constants.RelationEnumDictionary[relation]
                    + ")";
      
               // Console.WriteLine(query);
                common.ExecuteInsertQuery(query, tableName);
            }
        }

}
}
