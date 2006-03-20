//#define TRACE_SQL_QUERIES

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data.Odbc;
using System.Data;
using Ferda.Modules.Boxes.DataMiningCommon.BooleanPartialCedentSetting;
using Ferda.Modules.Boxes.DataMiningCommon.CategorialPartialCedentSetting;
using Ferda.Modules.Boxes.DataMiningCommon.EquivalenceClass;
using Ferda.Modules.Boxes.DataMiningCommon.LiteralSetting;
using Ferda.Modules.Boxes;
using Ferda.Modules;
using Ferda.Modules.Boxes.DataMiningCommon.Attributes;
using Ferda.Modules.Boxes.DataMiningCommon.Column;
using Ferda.Modules.Boxes.DataMiningCommon.DataMatrix;
using Ferda.Modules.Boxes.LISpMinerTasks.AbstractLMTask;

namespace Ferda.Modules.MetabaseLayer
{
    public struct BooleanCedent
    {
        public BooleanCedent(
            CedentEnum cedentType,
            BooleanPartialCedentSettingStruct cedent)
        {
            CedentType = cedentType;
            Cedent = cedent;
        }
        public CedentEnum CedentType;
        public BooleanPartialCedentSettingStruct Cedent;
    }

    public struct CategorialCedent
    {
        public CategorialCedent(
            CedentEnum cedentType,
            CategorialPartialCedentSettingStruct cedent)
        {
            CedentType = cedentType;
            Cedent = cedent;
        }
        public CedentEnum CedentType;
        public CategorialPartialCedentSettingStruct Cedent;
    }

    public enum TaskTypeEnum
    {
        FFT,
        KL,
        CF,
        SDFFT,
        SDKL,
        SDCF
    }

    public enum CategoryTypeEnum
    {
        Enumeration,
        Interval
    }

    public enum InfinityTypeEnum
    {
        None,
        PlusInfinity,
        MinusInfinity
    }

    public class Common
    {
        private OdbcConnection connection;

        private Constants constants;
        public Constants Constants
        {
            get { return constants; }
        }

        public Common(OdbcConnection openedConnection)
        {
            constants = new Constants();
            connection = openedConnection;

            autoIncrements = new Dictionary<string, string>();
            //key: tableName, value: autoIncrementColumnName
            autoIncrements.Add("taTask", "TaskID");
            autoIncrements.Add("taTaskCF", "TaskCFID");
            autoIncrements.Add("taTaskDC", "TaskDCID");
            autoIncrements.Add("taTaskDF", "TaskDFID");
            autoIncrements.Add("taTaskDK", "TaskDKID");
            autoIncrements.Add("taTaskFT", "TaskFTID");
            autoIncrements.Add("taTaskKL", "TaskKLID");
            autoIncrements.Add("tdKLCedentD", "KLCedentDID");
            autoIncrements.Add("tdCFCedentD", "CFCedentDID");
            autoIncrements.Add("tdKLLiteralD", "KLLiteralDID");
            autoIncrements.Add("tdCFLiteralD", "CFLiteralDID");
            autoIncrements.Add("tdCedentD", "CedentDID");
            autoIncrements.Add("tdEquivalenceClass", "EquivalenceClassID");
            autoIncrements.Add("tdLiteralD", "LiteralDID");
            autoIncrements.Add("tmQuantity", "QuantityID");
            autoIncrements.Add("tmCategory", "CategoryID");
            autoIncrements.Add("tmCategoryEnumValue", "CategoryEnumValueID");
            autoIncrements.Add("tmInterval", "IntervalID");
            autoIncrements.Add("tmAttribute", "AttributeID");
            autoIncrements.Add("tmMatrix", "MatrixID");
            autoIncrements.Add("tmValue", "ValueID");
            autoIncrements.Add("tdFTQuantifier", "FTQuantifierID");
            autoIncrements.Add("tdDFQuantifier", "DFQuantifierID");
            autoIncrements.Add("tdKLQuantifier", "KLQuantifierID");
            autoIncrements.Add("tdDKQuantifier", "DKQuantifierID");
            autoIncrements.Add("tdCFQuantifier", "CFQuantifierID");
            autoIncrements.Add("tdDCQuantifier", "DCQuantifierID");
            //autoIncrements.Add();
        }

        public static bool testArrayIsNullOrEmpty(System.Array array)
        {
            if (array == null || array.Length == 0)
                return true;
            return false;
        }

        #region TaskInterpretation
        public int SaveTask(string boxIdentity, TaskTypeEnum taskType)
        {
            string tableName = "taTask";
            string autoIncrementColumn = GetAutoIncrementColumnName(tableName);
            int autoIncrementValue = GetTableAutoIncrementValue(tableName, 1);
            string query = "INSERT INTO " + tableName + " (" + autoIncrementColumn
                + ",Name,TaskSubTypeID,UserID,ReadOnly) VALUES "
                + "(" + autoIncrementValue + ","
                + "'FerdaTask" + boxIdentity + "',"
                + Constants.TaskTypeEnumDictionary[taskType] + ","
                + Constants.UserID + ","
                + Constants.FalseValue
                + ")";
            ExecuteInsertQuery(query, tableName);
            return autoIncrementValue;
        }

        public GeneratingStruct GetGeneratingStruct(int taskID)
        {
            string generationQuery = "SELECT GenerationNrOfTests,GenerationStartTime,GenerationTotalTime,HypothesisGenerated,GenerationInterrupted FROM taTask WHERE TaskID=" + taskID;
            DataTable generationDataTable = ExecuteSelectQuery(generationQuery);
            if (generationDataTable.Rows.Count != 1)
                return new GeneratingStruct();// throw new Exception();
            DataRow generationDataRow = generationDataTable.Rows[0];
            GeneratingStruct result = new GeneratingStruct();
            result.generationNrOfTests = Convert.ToInt32(generationDataRow["GenerationNrOfTests"]);
            if (generationDataRow["GenerationStartTime"] != System.DBNull.Value)
                result.generationStartTime = new DateTimeTI(Convert.ToDateTime(generationDataRow["GenerationStartTime"]));
            else
                result.generationStartTime = new DateTimeTI(0, 0, 0, 0, 0, 0);
            result.generationTotalTime = new TimeTI(new TimeSpan(100000 * Convert.ToInt64(generationDataRow["GenerationTotalTime"])));
            if (Convert.ToBoolean(generationDataRow["HypothesisGenerated"]))
                result.generationState = GenerationStateEnum.Completed;
            if (Convert.ToBoolean(generationDataRow["GenerationInterrupted"]))
                result.generationState = GenerationStateEnum.Interrupted;
            string generationNrOfHypothesesQuery = "SELECT * FROM tiHypothesis WHERE TaskID=" + taskID;
            result.generationNrOfHypotheses = ExecuteSelectQuery(generationNrOfHypothesesQuery).Rows.Count;
            return result;
        }

        private DataTable _tmCategoryBooleanCache = null;
        public string[] GetBooleanLiteralCategoriesNames(int taskID, int literalInterpretationDbID)
        {
            if (_tmCategoryBooleanCache == null)
            {
                _tmCategoryBooleanCache = ExecuteSelectQuery("SELECT tmCategory.Name, tiCoefficient.LiteralIID FROM tiCoefficient, tmCategory WHERE tiCoefficient.TaskID=" + taskID + " AND tiCoefficient.CategoryID=tmCategory.CategoryID");
            }
            DataRow[] categories = _tmCategoryBooleanCache.Select("LiteralIID=" + literalInterpretationDbID);
            //"SELECT tmCategory.* FROM tiCoefficient, tmCategory WHERE tiCoefficient.TaskID=" + taskID + " AND tiCoefficient.LiteralIID=" + literalInterpretationDbID + " AND tiCoefficient.CategoryID=tmCategory.CategoryID"

            List<string> result = new List<string>();
            foreach (DataRow category in categories)
            {
                result.Add(category["Name"].ToString());
            }
            return result.ToArray();
        }

        private Dictionary<int, string[]> _tmCategoryCategorialCache = null;
        private DataTable _tmCategoryCache = null;
        public string[] GetCategorialLiteralCategoriesNames(int QuantityID)
        {
            if (_tmCategoryCategorialCache == null)
            {
                _tmCategoryCategorialCache = new Dictionary<int, string[]>();
            }
            string[] result;
            if (_tmCategoryCategorialCache.TryGetValue(QuantityID, out result))
                return result;
            else
            {
                if (_tmCategoryCache == null)
                    _tmCategoryCache = ExecuteSelectQuery("SELECT Name, QuantityID, Ord FROM tmCategory");
                DataRow[] categories = _tmCategoryCache.Select("QuantityID=" + QuantityID, "Ord");
                List<string> resultList = new List<string>();
                foreach (DataRow category in categories)
                {
                    resultList.Add(category["Name"].ToString());
                }
                result = resultList.ToArray();
                _tmCategoryCategorialCache.Add(QuantityID, result);
                return result;
            }
        }

        private DataTable _tiLiteralsCache = null;
        //tiLiteralI
        public BooleanLiteralStruct[] GetBooleanLiterals(int taskID, int hypothesisID)
        {
            if (_tiLiteralsCache == null)
            {
                _tiLiteralsCache = ExecuteSelectQuery("SELECT * FROM tiLiteralI WHERE TaskID=" + taskID);
            }
            DataRow[] literals = _tiLiteralsCache.Select("HypothesisID=" + hypothesisID);

            List<BooleanLiteralStruct> result = new List<BooleanLiteralStruct>();
            BooleanLiteralStruct booleanLiteralStruct;
            foreach (DataRow literal in literals)
            {
                booleanLiteralStruct = new BooleanLiteralStruct();
                booleanLiteralStruct.cedentType =
                    Constants.CedentEnumDictionaryBackward[
                        Convert.ToInt32(literal["CedentTypeID"])];
                booleanLiteralStruct.negation = Convert.ToBoolean(literal["Negation"]);
                //maybe mistake, but probably not
                int literalDbID = Convert.ToInt32(literal["LiteralDID"]);
                int literalIID = Convert.ToInt32(literal["LiteralIID"]);
                booleanLiteralStruct.literalIdentifier = this.literals[literalDbID];
                booleanLiteralStruct.literalName = this.attributeNameInLiterals[literalDbID];
                booleanLiteralStruct.categoriesNames = GetBooleanLiteralCategoriesNames(taskID, literalIID);
                result.Add(booleanLiteralStruct);
            }
            return result.ToArray();
        }

        private AbstractAttributeStruct GetAttribute(TaskTypeEnum taskType, object taskDescription, string attributeName, CedentEnum cedentType)
        {
            switch (taskType)
            {
                case TaskTypeEnum.FFT:
                    break;
                case TaskTypeEnum.KL:
                    Ferda.Modules.Boxes.LISpMinerTasks.KLTask.TaskStruct input = (Ferda.Modules.Boxes.LISpMinerTasks.KLTask.TaskStruct)taskDescription;
                    if (cedentType == CedentEnum.Antecedent)
                    {                        
                        foreach (CategorialPartialCedentSettingStruct cedent in input.antecedentSetting)
                        {
                            foreach (AbstractAttributeStruct attribute in cedent.attributes)
                            {
                                if (attribute.nameInLiterals == attributeName)
                                    return attribute;
                            }
                        }                        
                    }
                    else if (cedentType == CedentEnum.Succedent)
                    {
                        foreach (CategorialPartialCedentSettingStruct cedent in input.succedentSetting)
                        {
                            foreach (AbstractAttributeStruct attribute in cedent.attributes)
                            {
                                if (attribute.nameInLiterals == attributeName)
                                    return attribute;
                            }
                        }
                    }
                    break;
                case TaskTypeEnum.CF:
                    Ferda.Modules.Boxes.LISpMinerTasks.CFTask.TaskStruct input1 = (Ferda.Modules.Boxes.LISpMinerTasks.CFTask.TaskStruct)taskDescription;
                    foreach (CategorialPartialCedentSettingStruct cedent in input1.antecedentSetting)
                    {
                        foreach (AbstractAttributeStruct attribute in cedent.attributes)
                        {
                            if (attribute.nameInLiterals == attributeName)
                                return attribute;
                        }
                    }
                    break;
                case TaskTypeEnum.SDFFT:
                    break;
                case TaskTypeEnum.SDKL:
                    Ferda.Modules.Boxes.LISpMinerTasks.SDKLTask.TaskStruct input2 = (Ferda.Modules.Boxes.LISpMinerTasks.SDKLTask.TaskStruct)taskDescription;
                    if (cedentType == CedentEnum.Antecedent)
                    {
                        foreach (CategorialPartialCedentSettingStruct cedent in input2.antecedentSetting)
                        {
                            foreach (AbstractAttributeStruct attribute in cedent.attributes)
                            {
                                if (attribute.nameInLiterals == attributeName)
                                    return attribute;
                            }
                        }
                    }
                    else if (cedentType == CedentEnum.Succedent)
                    {
                        foreach (CategorialPartialCedentSettingStruct cedent in input2.succedentSetting)
                        {
                            foreach (AbstractAttributeStruct attribute in cedent.attributes)
                            {
                                if (attribute.nameInLiterals == attributeName)
                                    return attribute;
                            }
                        }
                    }
                    break;
                case TaskTypeEnum.SDCF:
                    Ferda.Modules.Boxes.LISpMinerTasks.SDCFTask.TaskStruct input3 = (Ferda.Modules.Boxes.LISpMinerTasks.SDCFTask.TaskStruct)taskDescription;
                    foreach (CategorialPartialCedentSettingStruct cedent in input3.antecedentSetting)
                    {
                        foreach (AbstractAttributeStruct attribute in cedent.attributes)
                        {
                            if (attribute.nameInLiterals == attributeName)
                                return attribute;
                        }
                    }
                    break;
                default:
                    break;
            }
            return null;
        }

        public LiteralStruct[] GetCategorialLiterals(TaskTypeEnum taskType, int taskID, int hypothesisID, object taskDescription)
        {
            string tdLiteralTableName = String.Empty;
            string tdLiteralIDColumn = String.Empty;

            string tdCedentDTableName = String.Empty;
            string tdCedentDIDColumn = String.Empty;
            switch (taskType)
            {
                case TaskTypeEnum.CF:
                case TaskTypeEnum.SDCF:
                    tdLiteralTableName = "tdCFLiteralD";
                    tdLiteralIDColumn = "CFLiteralDID";

                    tdCedentDTableName = "tdCFCedentD";
                    tdCedentDIDColumn = "CFCedentDID";

                    break;

                case TaskTypeEnum.KL:
                case TaskTypeEnum.SDKL:
                    tdLiteralTableName = "tdKLLiteralD";
                    tdLiteralIDColumn = "KLLiteralDID";

                    tdCedentDTableName = "tdKLCedentD";
                    tdCedentDIDColumn = "KLCedentDID";
                    break;

                default:
                    throw new Exception("SwitchBranchNotImplemented");

            }
            List<LiteralStruct> result = new List<LiteralStruct>();
            LiteralStruct literalStruct;
            DataTable literals = ExecuteSelectQuery(
                "SELECT " +
                tdLiteralTableName + "." + tdLiteralIDColumn + ", " +
                tdCedentDTableName + "." + tdCedentDIDColumn + ", " +
                tdLiteralTableName + ".QuantityID, " +
                "tmQuantity.Name, CedentTypeID" +
                " FROM `"
                + tdCedentDTableName + "`, `" + tdLiteralTableName + "`, `tmQuantity` " +
                "WHERE TaskID=" + taskID + " AND " +
                tdCedentDTableName + "." + tdCedentDIDColumn + "=" +
                tdLiteralTableName + "." + tdCedentDIDColumn +
                " AND tmQuantity.QuantityID=" +
                tdLiteralTableName + "." + "QuantityID"
                );

            foreach (DataRow literal in literals.Rows)
            {
                literalStruct = new LiteralStruct();
                literalStruct.cedentType = // tenhle cedent type
                    Constants.CedentEnumDictionaryBackward[
                        Convert.ToInt32(literal["CedentTypeID"])];

                literalStruct.literalIdentifier = Convert.ToInt32(literal[tdLiteralIDColumn]);
                literalStruct.literalName = literal["Name"].ToString();
                AbstractAttributeStruct attribute = this.GetAttribute(taskType, taskDescription, literalStruct.literalName, literalStruct.cedentType);
                bool canPass = true;
                List<double> valueList = new List<double>();
                if (attribute.categories.enums.Count > 0)
                {                    
                    foreach (DictionaryEntry value in attribute.categories.enums)
                    {
                        String[] StringSeq = (String[])value.Value;
                        if (StringSeq.Length == 1)
                        {
                            double doubleResult;
                            if (Double.TryParse(StringSeq[0], out doubleResult))
                            {
                                valueList.Add(doubleResult);
                            }
                            else
                            {
                                canPass = false;
                                break;
                            }
                        }
                        else
                        {
                            canPass = false;
                        }
                    }
                }
                else
                {
                    canPass = false;
                }
                if (canPass)
                {
                    literalStruct.numericValues = valueList.ToArray(); ;
                }
                else
                {
                    literalStruct.numericValues = null;
                }
                literalStruct.categoriesNames = GetCategorialLiteralCategoriesNames(Convert.ToInt32(literal["QuantityID"]));
                result.Add(literalStruct);
            }
            return result.ToArray();
        }

        public int[][] GetContingecyTable(TaskTypeEnum taskType, int taskID, int hypothesisID, int rowAttributeIdentifier, int columnAttributeIdentifier)
        {
            string tableName = String.Empty;
            string hypothesisColumnName = String.Empty;
            bool kl = false;
            string setType = String.Empty;

            switch (taskType)
            {
                case TaskTypeEnum.KL:
                    tableName = "tiKLFrequencyI";
                    hypothesisColumnName = "HypothesisKLID";
                    kl = true;
                    break;

                case TaskTypeEnum.SDKL:
                    tableName = "tiDKFrequencyI";
                    hypothesisColumnName = "HypothesisDKID";
                    kl = true;
                    setType = " AND CedentTypeID=" + constants.CedentEnumDictionary[CedentEnum.FirstSet];
                    break;

                case TaskTypeEnum.CF:
                    tableName = "tiCFFrequencyI";
                    hypothesisColumnName = "HypothesisCFID";
                    break;

                case TaskTypeEnum.SDCF:
                    tableName = "tiDCFrequencyI";
                    hypothesisColumnName = "HypothesisDCID";
                    setType = " AND CedentTypeID=" + constants.CedentEnumDictionary[CedentEnum.FirstSet];
                    break;

                default:
                    throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(taskType);
            }
            DataTable table = ExecuteSelectQuery("SELECT * FROM " + tableName + " WHERE TaskID=" + taskID + " AND " + hypothesisColumnName + "=" + hypothesisID + setType);
            AbstractAttributeStruct rowAttribute;
            AbstractAttributeStruct columnAttribute = GetAttributeStruct(columnAttributeIdentifier);
            List<int[]> resultList = new List<int[]>();
            if (kl)
            {
                rowAttribute = GetAttributeStruct(rowAttributeIdentifier);
                for (int i = 0; i < rowAttribute.countOfCategories; i++)
                    resultList.Add(new int[columnAttribute.countOfCategories]);
            }
            else
            {
                resultList.Add(new int[columnAttribute.countOfCategories]);
            }
            int[][] result = resultList.ToArray();
            foreach (DataRow row in table.Rows)
            {
                if (kl)
                    result[Convert.ToInt32(row["Row"])][Convert.ToInt32(row["Col"])] = Convert.ToInt32(row["Frequency"]);
                else
                    result[0][Convert.ToInt32(row["Col"])] = Convert.ToInt32(row["Frequency"]);
            }
            return result;
        }

        public int[][] GetSecondContingecyTable(TaskTypeEnum taskType, int taskID, int hypothesisID, int rowAttributeIdentifier, int columnAttributeIdentifier)
        {
            string tableName = String.Empty;
            string hypothesisColumnName = String.Empty;
            bool kl = false;
            switch (taskType)
            {
                case TaskTypeEnum.KL:
                    return new int[0][];

                case TaskTypeEnum.SDKL:
                    tableName = "tiDKFrequencyI";
                    hypothesisColumnName = "HypothesisDKID";
                    kl = true;
                    break;

                case TaskTypeEnum.CF:
                    return new int[0][];

                case TaskTypeEnum.SDCF:
                    tableName = "tiDCFrequencyI";
                    hypothesisColumnName = "HypothesisDCID";
                    break;

                default:
                    throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(taskType);
            }
            DataTable table = ExecuteSelectQuery("SELECT * FROM " + tableName + " WHERE TaskID=" + taskID + " AND " + hypothesisColumnName + "=" + hypothesisID + " AND CedentTypeID=" + constants.CedentEnumDictionary[CedentEnum.FirstSet]);

            AbstractAttributeStruct rowAttribute;
            AbstractAttributeStruct columnAttribute = GetAttributeStruct(columnAttributeIdentifier);
            List<int[]> resultList = new List<int[]>();
            if (kl)
            {
                rowAttribute = GetAttributeStruct(rowAttributeIdentifier);
                for (int i = 0; i < rowAttribute.countOfCategories; i++)
                    resultList.Add(new int[columnAttribute.countOfCategories]);
            }
            else
            {
                resultList.Add(new int[columnAttribute.countOfCategories]);
            }

            int[][] result = resultList.ToArray();
            foreach (DataRow row in table.Rows)
            {
                if (kl)
                    result[Convert.ToInt32(row["Row"])][Convert.ToInt32(row["Col"])] = Convert.ToInt32(row["Frequency"]);
                else
                    result[0][Convert.ToInt32(row["Col"])] = Convert.ToInt32(row["Frequency"]);
            }
            return result;
        }

        #endregion

        private Dictionary<string, string> autoIncrements;

        public string GetAutoIncrementColumnName(string tableName)
        {
            string result;
            if (autoIncrements.TryGetValue(tableName, out result))
                return result;
            else
                throw new Exception("Missing name of autoincrement column in table " + tableName);
        }

        private Dictionary<string, int> tableAutoIncrementValues = new Dictionary<string, int>();
        /// <summary>
        /// Call it before INSERT query on table with autoincrement column. It will return value of this column of next inserted row.
        /// </summary>
        /// <returns>Autoincremented number for the newly adding row.</returns>
        public int GetTableAutoIncrementValue(string tableName, int affectedRowsCount)
        {
            if (tableAutoIncrementValues.ContainsKey(tableName))
            {
                tableAutoIncrementValues[tableName] += affectedRowsCount;
                return tableAutoIncrementValues[tableName];
            }
            else
            {
                string autoIncrementColumnName = GetAutoIncrementColumnName(tableName);
                int result = affectedRowsCount + getMaxValueOfColumnInTable(tableName, autoIncrementColumnName);
                tableAutoIncrementValues.Add(tableName, result);
                return result;
            }
        }

        private int getMaxValueOfColumnInTable(string tableName, string columnName)
        {
            OdbcCommand command = new OdbcCommand(
                    "SELECT MAX(" + columnName + ") AS Maximum FROM " + tableName,
                    connection);
            object commandResult = command.ExecuteScalar();
            return (commandResult == System.DBNull.Value || commandResult == null) ? 0 : Convert.ToInt32(commandResult);
        }

        public void ExecuteInsertQuery(string query, string tableName)
        {
#if TRACE_SQL_QUERIES
            System.Diagnostics.Debug.WriteLine(tableName + ": " + query);
#endif
            OdbcCommand command = new OdbcCommand(query, connection);
            int affectedRowsCount = command.ExecuteNonQuery();
            //if (affectedRowsCount == 0) //maybe bug
            if (affectedRowsCount != 1)
                System.Diagnostics.Debug.WriteLine("WARNING!: MetabaseLayer.Common.ExecuteInsertQuery(" + query + ") : " + affectedRowsCount.ToString());
            //return this.GetTableAutoIncrementValue(tableName, affectedRowsCount);
        }

        public int ExecuteUpdateQuery(string query)
        {
#if TRACE_SQL_QUERIES
            System.Diagnostics.Debug.WriteLine(query);
#endif
            OdbcCommand command = new OdbcCommand(query, connection);
            return command.ExecuteNonQuery();
        }

        public DataTable ExecuteSelectQuery(string query)
        {
#if TRACE_SQL_QUERIES
            System.Diagnostics.Debug.WriteLine(query);
#endif
            OdbcCommand odbcCommand = new OdbcCommand(query, this.connection);
            OdbcDataAdapter dataAdapter = new OdbcDataAdapter(query, this.connection);
            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            return dataSet.Tables[0];
        }

        /// <summary>
        /// tdKLCedentD or tdCFCedentD
        /// </summary>
        /// <returns>ID of new row inserted to database.</returns>
        private int SetCategorialPartialCedent(CedentEnum cedentType, long minLength, long maxLength, int taskDBID, TaskTypeEnum taskType)
        {
            string tableName;
            switch (taskType)
            {
                case TaskTypeEnum.KL:
                case TaskTypeEnum.SDKL:
                    tableName = "tdKLCedentD";
                    break;
                case TaskTypeEnum.CF:
                case TaskTypeEnum.SDCF:
                    tableName = "tdCFCedentD";
                    break;
                default:
                    throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(taskType);
            }
            string autoIncrementColumn = GetAutoIncrementColumnName(tableName);
            int autoIncrementValue = GetTableAutoIncrementValue(tableName, 1);
            string query = "INSERT INTO " + tableName + " (" + autoIncrementColumn
                + ",TaskID,CedentTypeID,MinLen,MaxLen) VALUES "
                + "(" + autoIncrementValue + ","
                + taskDBID + ","
                + constants.CedentEnumDictionary[cedentType] + ","
                + minLength + ","
                + maxLength
                + ")";
            ExecuteInsertQuery(query, tableName);
            return autoIncrementValue;
        }
        /// <summary>
        /// Pair &lt;identifier, dbID&gt;
        /// </summary>
        private Dictionary<CedentIdentifierAndType, int> categorialPartialCedent = new Dictionary<CedentIdentifierAndType, int>();

        /// <summary>
        /// Pair &lt;literalDbId, attributeIdentifier&gt;
        /// </summary>
        private Dictionary<int, int> categorialLiteral = new Dictionary<int, int>();
        /// <summary>
        /// Pair &lt;literalDbId, attributeIdentifier&gt;
        /// </summary>
        public Dictionary<int, int> CategorialLiteral
        {
            get { return categorialLiteral; }
        }

        /// <summary>
        /// tdKLLiteralD or tdCFLiteralD
        /// </summary>
        /// <returns>ID of new row inserted to database.</returns>
        private int SetCategorialLiteralSetting(int cedentDBID, int attributeDBID, TaskTypeEnum taskType, int attributeIdentifier)
        {
            string tableName;
            string column1Name;
            switch (taskType)
            {
                case TaskTypeEnum.KL:
                case TaskTypeEnum.SDKL:
                    tableName = "tdKLLiteralD";
                    column1Name = "KLCedentDID";
                    break;
                case TaskTypeEnum.CF:
                case TaskTypeEnum.SDCF:
                    tableName = "tdCFLiteralD";
                    column1Name = "CFCedentDID";
                    break;
                default:
                    throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(taskType);
            }
            string autoIncrementColumn = GetAutoIncrementColumnName(tableName);
            int autoIncrementValue = GetTableAutoIncrementValue(tableName, 1);
            string query = "INSERT INTO " + tableName + " (" + autoIncrementColumn
                + "," + column1Name + ",QuantityID) VALUES "
                + "(" + autoIncrementValue + ","
                + cedentDBID + ","
                + attributeDBID
                + ")";
            categorialLiteral.Add(autoIncrementValue, attributeIdentifier);
            ExecuteInsertQuery(query, tableName);
            return autoIncrementValue;
        }

        /// <summary>
        /// tdCedentD
        /// </summary>
        /// <returns>ID of new row inserted to database.</returns>
        private int SetBooleanPartialCedent(CedentEnum cedentType, long minLength, long maxLength, int taskDBID)
        {
            string tableName = "tdCedentD";
            string autoIncrementColumn = GetAutoIncrementColumnName(tableName);
            int autoIncrementValue = GetTableAutoIncrementValue(tableName, 1);
            string query = "INSERT INTO " + tableName + " (" + autoIncrementColumn
                + ",Name,TaskID,CedentTypeID,MinLen,MaxLen) VALUES "
                + "(" + autoIncrementValue + ","
                + "'" + cedentType.ToString() + "',"
                + taskDBID + ","
                + constants.CedentEnumDictionary[cedentType] + ","
                + minLength + ","
                + maxLength
                + ")";
            ExecuteInsertQuery(query, tableName);
            return autoIncrementValue;
        }
        private struct CedentIdentifierAndType : IComparable
        {
            public CedentIdentifierAndType(int cedentIdentifier, CedentEnum cedentType)
            {
                this.cedentIdentifier = cedentIdentifier;
                this.cedentType = cedentType;
            }
            public int cedentIdentifier;
            public CedentEnum cedentType;
            public int CompareTo(object obj)
            {
                if (!(obj is CedentIdentifierAndType))
                {
                    throw new ArgumentException();
                }
                if (((CedentIdentifierAndType)obj).cedentIdentifier == this.cedentIdentifier
                    && ((CedentIdentifierAndType)obj).cedentType == this.cedentType)
                    return 0;
                else
                    return -1; //BUNO
            }
            public override bool Equals(Object obj)
            {
                if (!(obj is CedentIdentifierAndType))
                    return false;
                return (this.CompareTo(obj) == 0);
            }
            public override int GetHashCode()
            {
                return (int)cedentType + cedentIdentifier;
            }
        }
        /// <summary>
        /// Pair &lt;identifier + socketName, dbID&gt;
        /// </summary>
        private Dictionary<CedentIdentifierAndType, int> booleanPartialCedents = new Dictionary<CedentIdentifierAndType, int>();

        /// <summary>
        /// tdEquivalenceClass
        /// </summary>
        /// <returns>ID of new row inserted to database.</returns>
        private int SetEquivalenceClass(int booleanPartialCedentDBID, int identifier)
        {
            string tableName = "tdEquivalenceClass";
            string autoIncrementColumn = GetAutoIncrementColumnName(tableName);
            int autoIncrementValue = GetTableAutoIncrementValue(tableName, 1);
            string query = "INSERT INTO " + tableName + " (" + autoIncrementColumn
                + ",Name,CedentDID) VALUES "
                + "(" + autoIncrementValue + ","
                + "'" + identifier.ToString() + "',"
                + booleanPartialCedentDBID
                + ")";
            ExecuteInsertQuery(query, tableName);
            return autoIncrementValue;
        }
        /// <summary>
        /// Pair &lt;identifier, dbID&gt;
        /// </summary>
        private Dictionary<int, int> equivalenceClasses = new Dictionary<int, int>();

        /// <summary>
        /// tdLiteralD
        /// </summary>
        /// <returns>ID of new row inserted to database.</returns>
        private int SetLiteralSetting(long minLength, long maxLength, CoefficientTypeEnum coefficientType,
            int equivalenceClassDBID, GaceTypeEnum gaceType, LiteralTypeEnum literalType,
            int oneParticularCategoryDBID, int booleanPartialCedentDBID, int attributeDBID)
        {
            string equivalenceClassColumn = "EquivalenceClassID,";
            string equivalenceClassValue = "NULL,";
            if (equivalenceClassDBID != 0)
            {
                equivalenceClassColumn = "EquivalenceClassID,";
                equivalenceClassValue = equivalenceClassDBID.ToString() + ",";
            }

            string categoryDBID = (oneParticularCategoryDBID == 0) ? "NULL" : oneParticularCategoryDBID.ToString();

            string tableName = "tdLiteralD";
            string autoIncrementColumn = GetAutoIncrementColumnName(tableName);
            int autoIncrementValue = GetTableAutoIncrementValue(tableName, 1);
            string query = "INSERT INTO " + tableName + " (" + autoIncrementColumn
                + ",CedentDID,QuantityID,CategoryID,LiteralTypeID,GaceTypeID," + equivalenceClassColumn
                + "CoefficientTypeID,MinLen,MaxLen) VALUES "
                + "(" + autoIncrementValue + ","
                + booleanPartialCedentDBID + ","
                + attributeDBID + ","
                + categoryDBID + ","
                + constants.LiteralTypeEnumDictionary[literalType] + ","
                + constants.GaceTypeEnumDictionary[gaceType] + ","
                + equivalenceClassValue +
                +constants.CoefficientTypeEnumDictionary[coefficientType] + ","
                + minLength + ","
                + maxLength
                + ")";
            ExecuteInsertQuery(query, tableName);
            return autoIncrementValue;
        }
        /// <summary>
        /// Pair &lt;dbID, identifier&gt;
        /// </summary>
        private Dictionary<int, int> literals = new Dictionary<int, int>();
        /// <summary>
        /// Pair &lt;dbID, attributeNameInLiterals&gt;
        /// </summary>
        private Dictionary<int, string> attributeNameInLiterals = new Dictionary<int, string>();
        /// <summary>
        /// Pair &lt;dbID, identifier&gt;
        /// </summary>
        public Dictionary<int, int> Literals
        {
            get { return literals; }
        }

        /// <summary>
        /// tmQuantity
        /// </summary>
        /// <returns>ID of new row inserted to database.</returns>
        private int SetAttribute(string nameInLiterals, int attributeIdentifier, int columnDBID)
        {
            if (String.IsNullOrEmpty(nameInLiterals))
                nameInLiterals = attributeIdentifier.ToString();

            string tableName = "tmQuantity";
            string autoIncrementColumn = GetAutoIncrementColumnName(tableName);
            int autoIncrementValue = GetTableAutoIncrementValue(tableName, 1);
            string query = "INSERT INTO " + tableName + " (" + autoIncrementColumn
                + ",Name,AttributeID,ItemShift,ShortName,ShowName,ParentGroupID"
                + ",UserID,wSavedCountUsed,wUpdateVer) VALUES "
                + "(" + autoIncrementValue + ","
                + "'" + nameInLiterals + "',"
                + columnDBID + ","
                + "0," //ItemShift
                + "'" + nameInLiterals + "',"
                + constants.TrueValue + ","
                + "1," //ParentGroupID
                + constants.UserID + "," //UserID
                + constants.wSavedCountUsed + "," //wSavedCountUsed
                + constants.wUpdateVer //wUpdateVer
                + ")";
            ExecuteInsertQuery(query, tableName);
            return autoIncrementValue;
        }

        /// <summary>
        /// Pair &lt;identifier, AbstractAttributeStruct&gt;
        /// </summary>
        private Dictionary<int, Ferda.Modules.Boxes.DataMiningCommon.Attributes.AbstractAttributeStruct> attributesCache = new Dictionary<int, AbstractAttributeStruct>();
        public AbstractAttributeStruct GetAttributeStruct(int identifier)
        {
            AbstractAttributeStruct result;
            if (this.attributesCache.TryGetValue(identifier, out result))
                return result;
            throw new KeyNotFoundException();
        }

        /// <summary>
        /// Pair &lt;identifier, dbID&gt;
        /// </summary>
        private Dictionary<int, int> attributes = new Dictionary<int, int>();

        /// <summary>
        /// tmCategory
        /// </summary>
        /// <returns>ID of new row inserted to database.</returns>
        private int SetCategory(string name, int attributeDBID, CategoryTypeEnum categoryType, bool xCategory, bool includeNullCategory, long categoryOrd, int attributeIdentifier)
        {
            string tableName = "tmCategory";
            string autoIncrementColumn = GetAutoIncrementColumnName(tableName);
            int autoIncrementValue = GetTableAutoIncrementValue(tableName, 1);
            string query = "INSERT INTO " + tableName + " (" + autoIncrementColumn
                + ",Name,QuantityID,CategorySubTypeID,BoolTypeID"
                + ",XCategory,IncludeNULL,wSavedCountUsed,Ord) VALUES "
                + "(" + autoIncrementValue + ","
                + "'" + name + "',"
                + attributeDBID + ","
                + constants.CategoryTypeEnumDictionary[categoryType] + ","
                + "1," //BoolTypeID = No bool
                + constants.GetBool(xCategory) + ","
                + constants.GetBool(includeNullCategory) + ","
                + constants.wSavedCountUsed + "," //wSavedCountUsed
                + categoryOrd
                + ")";
            this.categories[attributeIdentifier].Add(name, autoIncrementValue);
            this.categoriesOrds[attributeIdentifier].Add(categoryOrd, name);
            ExecuteInsertQuery(query, tableName);
            return autoIncrementValue;
        }
        /// <summary>
        /// Pair &lt;attributeIdentifier, Dictionary&lt;rowValue, categoryDBID&gt;&gt;
        /// </summary>
        private Dictionary<int, Dictionary<string, int>> categories = new Dictionary<int, Dictionary<string, int>>();
        /// <summary>
        /// Pair &lt;attributeIdentifier, Dictionary&lt;categoryOrd, rowValue&gt;&gt;
        /// </summary>
        private Dictionary<int, Dictionary<long, string>> categoriesOrds = new Dictionary<int, Dictionary<long, string>>();


        /// <summary>
        /// tmCategoryEnumValue
        /// </summary>
        /// <returns>ID of new row inserted to database.</returns>
        private int SetCategoryEnum(int categoryDBID, int valueDBID)
        {
            string tableName = "tmCategoryEnumValue";
            string autoIncrementColumn = GetAutoIncrementColumnName(tableName);
            int autoIncrementValue = GetTableAutoIncrementValue(tableName, 1);
            string query = "INSERT INTO " + tableName + " (" + autoIncrementColumn
                + ",CategoryID,ValueID) VALUES "
                + "(" + autoIncrementValue + ","
                + categoryDBID + ","
                + valueDBID
                + ")";
            ExecuteInsertQuery(query, tableName);
            return autoIncrementValue;
        }

        /// <summary>
        /// tmInterval
        /// </summary>
        /// <returns>ID of new row inserted to database.</returns>
        private int SetCategoryInterval(int categoryDBID, int fromValueDBID, int toValueDBID, BoundaryEnum leftBracketType, BoundaryEnum rightBracketType)
        {
            string tableName = "tmInterval";
            string autoIncrementColumn = GetAutoIncrementColumnName(tableName);
            int autoIncrementValue = GetTableAutoIncrementValue(tableName, 1);
            string query = "INSERT INTO " + tableName + " (" + autoIncrementColumn
                + ",CategoryID,FromValueID,ToValueID,LeftBracketTypeID,RightBracketTypeID) VALUES "
                + "(" + autoIncrementValue + ","
                + categoryDBID + ","
                + fromValueDBID + ","
                + toValueDBID + ","
                + constants.BoundaryEnumDictionary[leftBracketType] + ","
                + constants.BoundaryEnumDictionary[rightBracketType]
                + ")";
            ExecuteInsertQuery(query, tableName);
            return autoIncrementValue;
        }

        #region SetValue

        /// <summary>
        /// tmValue
        /// </summary>
        /// <returns>ID of new row inserted to database.</returns>
        private int SetValue(long value)
        {
            string tableName = "tmValue";
            string autoIncrementColumn = GetAutoIncrementColumnName(tableName);
            int autoIncrementValue = GetTableAutoIncrementValue(tableName, 1);
            string query = "INSERT INTO " + tableName + " (" + autoIncrementColumn
                + ",ValueSubTypeID,InfinityTypeID,ValueLong) VALUES "
                + "(" + autoIncrementValue + ","
                + constants.ValueSubTypeEnumDictionary[ValueSubTypeEnum.LongIntegerType] + ","
                + constants.InfinityTypeEnumDictionary[InfinityTypeEnum.None] + ","
                + value
                + ")";
            ExecuteInsertQuery(query, tableName);
            return autoIncrementValue;
        }
        private int SetValue(double value)
        {
            string tableName = "tmValue";
            string autoIncrementColumn = GetAutoIncrementColumnName(tableName);
            int autoIncrementValue = GetTableAutoIncrementValue(tableName, 1);
            string query = "INSERT INTO " + tableName + " (" + autoIncrementColumn
                + ",ValueSubTypeID,InfinityTypeID,ValueFloat) VALUES "
                + "(" + autoIncrementValue + ","
                + constants.ValueSubTypeEnumDictionary[ValueSubTypeEnum.DoubleType] + ","
                + constants.InfinityTypeEnumDictionary[InfinityTypeEnum.None] + ","
                + "'" + value + "'"
                + ")";
            ExecuteInsertQuery(query, tableName);
            return autoIncrementValue;
        }
        private int SetValue(DateTime value)
        {
            string tableName = "tmValue";
            string autoIncrementColumn = GetAutoIncrementColumnName(tableName);
            int autoIncrementValue = GetTableAutoIncrementValue(tableName, 1);
            string query = "INSERT INTO " + tableName + " (" + autoIncrementColumn
                + ",ValueSubTypeID,InfinityTypeID,ValueDate) VALUES "
                + "(" + autoIncrementValue + ","
                + constants.ValueSubTypeEnumDictionary[ValueSubTypeEnum.DoubleType] + ","
                + constants.InfinityTypeEnumDictionary[InfinityTypeEnum.None] + ","
                + "'" + constants.DateTimeToString(value) + "'"
                + ")";
            ExecuteInsertQuery(query, tableName);
            return autoIncrementValue;
        }
        private int SetValue(string value)
        {
            if (value == Ferda.Modules.Helpers.Common.Constants.DbNullCategoryName
                || value == null /* nemelo by nastat */ )
                return this.SetValue();
            if (value == Ferda.Modules.Helpers.Common.Constants.EmptyStringCategoryName)
                value = String.Empty;
            string tableName = "tmValue";
            string autoIncrementColumn = GetAutoIncrementColumnName(tableName);
            int autoIncrementValue = GetTableAutoIncrementValue(tableName, 1);
            string query = "INSERT INTO " + tableName + " (" + autoIncrementColumn
                + ",ValueSubTypeID,InfinityTypeID,ValueString) VALUES "
                + "(" + autoIncrementValue + ","
                + constants.ValueSubTypeEnumDictionary[ValueSubTypeEnum.StringType] + ","
                + constants.InfinityTypeEnumDictionary[InfinityTypeEnum.None] + ","
                + "'" + value + "'"
                + ")";
            ExecuteInsertQuery(query, tableName);
            return autoIncrementValue;
        }
        private int SetValue(bool value)
        {
            string tableName = "tmValue";
            string autoIncrementColumn = GetAutoIncrementColumnName(tableName);
            int autoIncrementValue = GetTableAutoIncrementValue(tableName, 1);
            string query = "INSERT INTO " + tableName + " (" + autoIncrementColumn
                + ",ValueSubTypeID,InfinityTypeID,ValueBool) VALUES "
                + "(" + autoIncrementValue + ","
                + constants.ValueSubTypeEnumDictionary[ValueSubTypeEnum.BooleanType] + ","
                + constants.InfinityTypeEnumDictionary[InfinityTypeEnum.None] + ","
                + "'" + constants.GetBool(value) + "'"
                + ")";
            ExecuteInsertQuery(query, tableName);
            return autoIncrementValue;
        }
        private int SetValue(DateTimeStruct value)
        {
            string tableName = "tmValue";
            string autoIncrementColumn = GetAutoIncrementColumnName(tableName);
            int autoIncrementValue = GetTableAutoIncrementValue(tableName, 1);
            string query = "INSERT INTO " + tableName + " (" + autoIncrementColumn
                + ",ValueSubTypeID,InfinityTypeID,ValueDate) VALUES "
                + "(" + autoIncrementValue + ","
                + constants.ValueSubTypeEnumDictionary[ValueSubTypeEnum.DateTimeType] + ","
                + constants.InfinityTypeEnumDictionary[InfinityTypeEnum.None] + ","
                + "'" + constants.DateTimeStructToString(value) + "'"
                + ")";
            ExecuteInsertQuery(query, tableName);
            return autoIncrementValue;
        }
        private int SetValue() //null
        {
            string tableName = "tmValue";
            string autoIncrementColumn = GetAutoIncrementColumnName(tableName);
            int autoIncrementValue = GetTableAutoIncrementValue(tableName, 1);
            string query = "INSERT INTO " + tableName + " (" + autoIncrementColumn
                + ",ValueSubTypeID,InfinityTypeID,IsNULL) VALUES "
                + "(" + autoIncrementValue + ","
                + constants.ValueSubTypeEnumDictionary[ValueSubTypeEnum.FloatType] + ","
                + constants.InfinityTypeEnumDictionary[InfinityTypeEnum.None] + ","
                + constants.TrueValue
                + ")";
            ExecuteInsertQuery(query, tableName);
            return autoIncrementValue;
        }
        private int SetValue(InfinityTypeEnum infinityType)
        {
            string tableName = "tmValue";
            string autoIncrementColumn = GetAutoIncrementColumnName(tableName);
            int autoIncrementValue = GetTableAutoIncrementValue(tableName, 1);
            string query = "INSERT INTO " + tableName + " (" + autoIncrementColumn
                + ",ValueSubTypeID,InfinityTypeID) VALUES "
                + "(" + autoIncrementValue + ","
                + constants.ValueSubTypeEnumDictionary[ValueSubTypeEnum.FloatType] + ","
                + constants.InfinityTypeEnumDictionary[infinityType]
                + ")";
            ExecuteInsertQuery(query, tableName);
            return autoIncrementValue;
        }

        #endregion

        /// <summary>
        /// tmAttribute
        /// </summary>
        /// <returns>ID of new row inserted to database.</returns>
        private int SetColumn(string name, int matrixDBID, ColumnTypeEnum columnSubType, ValueSubTypeEnum valueSubType, string formula, int primaryKeyPosition)
        {
            string tableName = "tmAttribute";
            /* MultiColumns are not implemented so if you want to implement it
             * you should implement appropriate box module and somewhere here
             * you should implement storing setting for the MultiColumn i.e.
             * MCPosition,MCLength,MCDelimeter
             * */
            //ValueMin,ValueMax,ValueAvg,ValueModus,ValueVariability are not stored its needless
            string autoIncrementColumn = GetAutoIncrementColumnName(tableName);
            int autoIncrementValue = GetTableAutoIncrementValue(tableName, 1);
            string query = "INSERT INTO " + tableName + " (" + autoIncrementColumn
                + ",Name,MatrixID,AttributeSubTypeID,ValueSubTypeID"
                + ",Formula,PrimaryKeyPosition,wSavedCountUsed) VALUES "
                + "(" + autoIncrementValue + ","
                + "'" + name + "',"
                + matrixDBID + ","
                + constants.ColumnSubTypeEnumDictionary[columnSubType] + ","
                + constants.ValueSubTypeEnumDictionary[valueSubType] + ","
                + "'" + formula + "',"
                + primaryKeyPosition + ","
                + constants.wSavedCountUsed
                + ")";
            ExecuteInsertQuery(query, tableName);
            return autoIncrementValue;
        }
        /// <summary>
        /// Pair &lt;column select expression, dbID&gt;
        /// </summary>
        private Dictionary<string, int> columns = new Dictionary<string, int>();

        /// <summary>
        /// tmMatrix
        /// </summary>
        /// <returns>ID of new row inserted to database.</returns>
        private int SetDataMatrix(string name, long recordCount)
        {
            string tableName = "tmMatrix";
            string autoIncrementColumn = GetAutoIncrementColumnName(tableName);
            int autoIncrementValue = GetTableAutoIncrementValue(tableName, 1);
            string query = "INSERT INTO " + tableName + " (" + autoIncrementColumn
                + ",Name,Initialised,RecordCount,wSavedCountUsed) VALUES "
                + "(" + autoIncrementValue + ","
                + "'" + name + "',"
                + constants.TrueValue + ","
                + recordCount + ","
                + constants.wSavedCountUsed
                + ")";
            ExecuteInsertQuery(query, tableName);
            return autoIncrementValue;
        }

        #region Insert Value Interval
        private void insertValueInterval(int categoryID, LongIntervalStruct intervalStruct)
        {
            int leftCategoryValueID;
            int rightCategoryValueID;
            BoundaryEnum leftBracketType;
            BoundaryEnum rightBracketType;

            if (intervalStruct.leftBoundType == BoundaryEnum.Infinity)
            {
                leftCategoryValueID = SetValue(InfinityTypeEnum.MinusInfinity);
                leftBracketType = BoundaryEnum.Round;
            }
            else
            {
                leftCategoryValueID = SetValue(intervalStruct.leftBound);
                leftBracketType = intervalStruct.leftBoundType;
            }
            if (intervalStruct.rightBoundType == BoundaryEnum.Infinity)
            {
                rightCategoryValueID = SetValue(intervalStruct.leftBound);
                rightBracketType = BoundaryEnum.Round;
            }
            else
            {
                rightCategoryValueID = SetValue(intervalStruct.rightBound);
                rightBracketType = intervalStruct.rightBoundType;
            }
            this.SetCategoryInterval(
                categoryID,
                leftCategoryValueID,
                rightCategoryValueID,
                leftBracketType,
                rightBracketType);
        }
        private void insertValueInterval(int categoryID, FloatIntervalStruct intervalStruct)
        {
            int leftCategoryValueID;
            int rightCategoryValueID;
            BoundaryEnum leftBracketType;
            BoundaryEnum rightBracketType;

            if (intervalStruct.leftBoundType == BoundaryEnum.Infinity)
            {
                leftCategoryValueID = SetValue(InfinityTypeEnum.MinusInfinity);
                leftBracketType = BoundaryEnum.Round;
            }
            else
            {
                leftCategoryValueID = SetValue(intervalStruct.leftBound);
                leftBracketType = intervalStruct.leftBoundType;
            }
            if (intervalStruct.rightBoundType == BoundaryEnum.Infinity)
            {
                rightCategoryValueID = SetValue(intervalStruct.leftBound);
                rightBracketType = BoundaryEnum.Round;
            }
            else
            {
                rightCategoryValueID = SetValue(intervalStruct.rightBound);
                rightBracketType = intervalStruct.rightBoundType;
            }
            this.SetCategoryInterval(
                categoryID,
                leftCategoryValueID,
                rightCategoryValueID,
                leftBracketType,
                rightBracketType);
        }
        private void insertValueInterval(int categoryID, DateTimeIntervalStruct intervalStruct)
        {
            int leftCategoryValueID;
            int rightCategoryValueID;
            BoundaryEnum leftBracketType;
            BoundaryEnum rightBracketType;

            if (intervalStruct.leftBoundType == BoundaryEnum.Infinity)
            {
                leftCategoryValueID = SetValue(InfinityTypeEnum.MinusInfinity);
                leftBracketType = BoundaryEnum.Round;
            }
            else
            {
                leftCategoryValueID = SetValue(intervalStruct.leftBound);
                leftBracketType = intervalStruct.leftBoundType;
            }
            if (intervalStruct.rightBoundType == BoundaryEnum.Infinity)
            {
                rightCategoryValueID = SetValue(intervalStruct.leftBound);
                rightBracketType = BoundaryEnum.Round;
            }
            else
            {
                rightCategoryValueID = SetValue(intervalStruct.rightBound);
                rightBracketType = intervalStruct.rightBoundType;
            }
            this.SetCategoryInterval(
                categoryID,
                leftCategoryValueID,
                rightCategoryValueID,
                leftBracketType,
                rightBracketType);
        }
        #endregion

        private void updateTask(int taskID, int dataMatrixDBID)
        {
            ExecuteUpdateQuery(
                "UPDATE taTask SET MatrixID=" + dataMatrixDBID
                + " WHERE TaskID=" + taskID);
        }

        public void Remap(
            BooleanCedent[] booleanCedents,
            CategorialCedent[] categorialCedents,
            int taskID,
            TaskTypeEnum taskType,
            string boxIdentity,
            out long allRowsInTaskDataMatrixCount)
        {
            List<AbstractAttributeStruct> abstractAttributes = new List<AbstractAttributeStruct>();
            string categoryName;

            #region EQUIVALENCE CLASS PROBLEM VARIABLES
            int attributeIdentifier;
            int literalIdentifier;
            int equivalenceClassIdentifier;
            int rewrite;
            //Pair <attribute.identifier, List Of [literal.identifier]>
            Dictionary<int, List<int>> attributesAndLiterals = new Dictionary<int, List<int>>();
            //Pair <attribute.identifier, equivalenceClass.identifier>
            Dictionary<int, int> attributesAndEquivalenceClasses = new Dictionary<int, int>();
            //Pair <literal.identifier, equivalenceClass.identifier>
            Dictionary<int, int> literalsAndEquivalenceClasses = new Dictionary<int, int>();
            #endregion

            #region BooleanCedent (tdCedentD), EquivalenceClass (tdEquivalenceClass), Literal (tdLiteralD)

            Dictionary<CedentEnum, bool> savedCedents = new Dictionary<CedentEnum, bool>();

            if (booleanCedents != null)
            {
                int booleanCedentID;
                int equivalenceClassID;
                EquivalenceClassStruct[] equivalenceClasses;

                foreach (BooleanCedent booleanCedent in booleanCedents)
                {
                    #region BooleanPartialCedent (tdCedentD)
                    CedentIdentifierAndType cedentIdentifierAndType = new CedentIdentifierAndType(booleanCedent.Cedent.identifier, booleanCedent.CedentType);
                    if (this.booleanPartialCedents.ContainsKey(cedentIdentifierAndType))
                    {
                        booleanCedentID = this.booleanPartialCedents[cedentIdentifierAndType];
                        continue;
                    }
                    else
                    {
                        booleanCedentID = this.SetBooleanPartialCedent(
                            booleanCedent.CedentType,
                            booleanCedent.Cedent.minLen,
                            booleanCedent.Cedent.maxLen,
                            taskID);
                        this.booleanPartialCedents.Add(cedentIdentifierAndType, booleanCedentID);
                    }

                    savedCedents[booleanCedent.CedentType] = true;

                    #endregion

                    #region EQUIVALENCE CLASS PROBLEM

                    equivalenceClasses = booleanCedent.Cedent.equivalenceClasses;

                    if (equivalenceClasses != null &&
                        equivalenceClasses.Length > 0)
                    {
                        //This makes attributesAndLiterals Dictionary<int, List<int>> Pair <attribute.identifier, List Of [literal.identifier]>
                        if (booleanCedent.Cedent.literalSettings != null)
                            foreach (LiteralSettingStruct literalSettingStruct in booleanCedent.Cedent.literalSettings)
                            {
                                //literalSettingStruct.identifier;
                                attributeIdentifier = literalSettingStruct.atomSetting.abstractAttribute.identifier;
                                literalIdentifier = literalSettingStruct.identifier;
                                if (attributesAndLiterals.ContainsKey(attributeIdentifier))
                                    attributesAndLiterals[attributeIdentifier].Add(literalIdentifier);
                                else
                                    attributesAndLiterals.Add(attributeIdentifier, new List<int>(new int[] { literalIdentifier }));
                                attributesAndEquivalenceClasses[attributeIdentifier] = 0;
                                literalsAndEquivalenceClasses[literalIdentifier] = 0;
                            }
                        //This makes attributesAndEquivalenceClasses Dictionary<int, int> Pair <attribute.identifier, equivalenceClass.identifier>
                        foreach (EquivalenceClassStruct equivalenceClassStruct in equivalenceClasses)
                        {
                            equivalenceClassIdentifier = equivalenceClassStruct.identifier;
                            foreach (int identifierOfAttributeInClass in equivalenceClassStruct.attributesIdentifiers)
                            {
                                rewrite = 0;
                                if (attributesAndEquivalenceClasses.ContainsKey(identifierOfAttributeInClass))
                                {
                                    if (attributesAndEquivalenceClasses[identifierOfAttributeInClass] != 0)
                                        rewrite = attributesAndEquivalenceClasses[identifierOfAttributeInClass];
                                    attributesAndEquivalenceClasses[identifierOfAttributeInClass] = equivalenceClassIdentifier;
                                }
                                else
                                    attributesAndEquivalenceClasses.Add(identifierOfAttributeInClass, equivalenceClassIdentifier);
                                if (rewrite != 0)
                                    foreach (KeyValuePair<int, int> kvp in attributesAndEquivalenceClasses)
                                    {
                                        if (attributesAndEquivalenceClasses[kvp.Key] == rewrite)
                                            attributesAndEquivalenceClasses[kvp.Key] = equivalenceClassIdentifier;
                                    }
                            }
                        }
                        //This makes literalsAndEquivalenceClasses Dictionary<int, int> Pair <literal.identifier, equivalenceClass.identifier>
                        //Process equivalence classes of attributes to literals
                        foreach (KeyValuePair<int, int> kvp in attributesAndEquivalenceClasses)
                        {
                            if (attributesAndEquivalenceClasses[kvp.Key] != 0)
                            {
                                rewrite = attributesAndEquivalenceClasses[kvp.Key];
                                foreach (int currentLiteralKey in attributesAndLiterals[kvp.Key])
                                {
                                    literalsAndEquivalenceClasses[currentLiteralKey] = rewrite;
                                }
                            }
                        }
                        //This process equivalence classes of literals
                        foreach (EquivalenceClassStruct equivalenceClassStruct in equivalenceClasses)
                        {
                            equivalenceClassIdentifier = equivalenceClassStruct.identifier;
                            foreach (int identifierOfLiteralInClass in equivalenceClassStruct.literalsIdentifiers)
                            {
                                rewrite = 0;
                                if (literalsAndEquivalenceClasses.ContainsKey(identifierOfLiteralInClass))
                                {
                                    if (literalsAndEquivalenceClasses[identifierOfLiteralInClass] != 0)
                                        rewrite = literalsAndEquivalenceClasses[identifierOfLiteralInClass];
                                    literalsAndEquivalenceClasses[identifierOfLiteralInClass] = equivalenceClassIdentifier;
                                }
                                else
                                {
                                    literalsAndEquivalenceClasses.Add(identifierOfLiteralInClass, equivalenceClassIdentifier);
                                }
                                if (rewrite != 0)
                                    foreach (KeyValuePair<int, int> kvp in literalsAndEquivalenceClasses)
                                    {
                                        if (literalsAndEquivalenceClasses[kvp.Key] == rewrite)
                                            literalsAndEquivalenceClasses[kvp.Key] = equivalenceClassIdentifier;
                                    }
                            }
                        }
                    }

                    #endregion

                    #region EquivalenceClass (tdEquivalenceClass)

                    equivalenceClasses = booleanCedent.Cedent.equivalenceClasses;
                    if (booleanCedent.Cedent.equivalenceClasses != null)
                        foreach (EquivalenceClassStruct equivalenceClassStruct in equivalenceClasses)
                        {
                            //test if equivalence class is used or if it was absorb by other equivalence class
                            if (!literalsAndEquivalenceClasses.ContainsValue(equivalenceClassStruct.identifier))
                                continue;

                            equivalenceClassID = this.SetEquivalenceClass(
                                booleanCedentID,
                                equivalenceClassStruct.identifier);
                            this.equivalenceClasses.Add(equivalenceClassStruct.identifier, equivalenceClassID);
                        }

                    #endregion

                    #region  Prepare list of Attributes from BooleanCedents
                    if (booleanCedent.Cedent.literalSettings != null)
                        foreach (LiteralSettingStruct literalSettingStruct in booleanCedent.Cedent.literalSettings)
                        {
                            abstractAttributes.Add(literalSettingStruct.atomSetting.abstractAttribute);
                        }
                    #endregion
                }
            }
            #endregion
            #region Prepare list of Attributes from CategorialCedents
            if (categorialCedents != null)
                foreach (CategorialCedent categorialCedent in categorialCedents)
                {
                    savedCedents.Add(categorialCedent.CedentType, true);
                    abstractAttributes.AddRange(categorialCedent.Cedent.attributes);
                }
            #endregion

            #region Saved cedents?
            //pokud totiz neni nastaven nejaky cedent (chybi treba prazdny radek), tak nejde pustit gen
            switch (taskType)
            {
                case TaskTypeEnum.FFT:
                    if (!savedCedents.ContainsKey(CedentEnum.Antecedent) || savedCedents[CedentEnum.Antecedent] == false)
                        this.SetBooleanPartialCedent(CedentEnum.Antecedent, 0, 0, taskID);
                    if (!savedCedents.ContainsKey(CedentEnum.Succedent) || savedCedents[CedentEnum.Succedent] == false)
                        this.SetBooleanPartialCedent(CedentEnum.Succedent, 0, 0, taskID);
                    if (!savedCedents.ContainsKey(CedentEnum.Condition) || savedCedents[CedentEnum.Condition] == false)
                        this.SetBooleanPartialCedent(CedentEnum.Condition, 0, 0, taskID);
                    break;
                case TaskTypeEnum.SDFFT:
                    if (!savedCedents.ContainsKey(CedentEnum.Antecedent) || savedCedents[CedentEnum.Antecedent] == false)
                        this.SetBooleanPartialCedent(CedentEnum.Antecedent, 0, 0, taskID);
                    if (!savedCedents.ContainsKey(CedentEnum.Succedent) || savedCedents[CedentEnum.Succedent] == false)
                        this.SetBooleanPartialCedent(CedentEnum.Succedent, 0, 0, taskID);
                    if (!savedCedents.ContainsKey(CedentEnum.Condition) || savedCedents[CedentEnum.Condition] == false)
                        this.SetBooleanPartialCedent(CedentEnum.Condition, 0, 0, taskID);
                    if (!savedCedents.ContainsKey(CedentEnum.FirstSet) || savedCedents[CedentEnum.FirstSet] == false)
                        this.SetBooleanPartialCedent(CedentEnum.FirstSet, 0, 0, taskID);
                    if (!savedCedents.ContainsKey(CedentEnum.SecondSet) || savedCedents[CedentEnum.SecondSet] == false)
                        this.SetBooleanPartialCedent(CedentEnum.SecondSet, 0, 0, taskID);
                    break;
                case TaskTypeEnum.KL:
                    if (!savedCedents.ContainsKey(CedentEnum.Antecedent) || savedCedents[CedentEnum.Antecedent] == false)
                        this.SetCategorialPartialCedent(CedentEnum.Antecedent, 0, 0, taskID, taskType);
                    if (!savedCedents.ContainsKey(CedentEnum.Succedent) || savedCedents[CedentEnum.Succedent] == false)
                        this.SetCategorialPartialCedent(CedentEnum.Succedent, 0, 0, taskID, taskType);
                    if (!savedCedents.ContainsKey(CedentEnum.Condition) || savedCedents[CedentEnum.Condition] == false)
                        this.SetBooleanPartialCedent(CedentEnum.Condition, 0, 0, taskID);
                    break;

                case TaskTypeEnum.CF:
                    if (!savedCedents.ContainsKey(CedentEnum.Antecedent) || savedCedents[CedentEnum.Antecedent] == false)
                        this.SetCategorialPartialCedent(CedentEnum.Antecedent, 0, 0, taskID, taskType);
                    if (!savedCedents.ContainsKey(CedentEnum.Condition) || savedCedents[CedentEnum.Condition] == false)
                        this.SetBooleanPartialCedent(CedentEnum.Condition, 0, 0, taskID);
                    break;

                case TaskTypeEnum.SDCF:
                    if (!savedCedents.ContainsKey(CedentEnum.Antecedent) || savedCedents[CedentEnum.Antecedent] == false)
                        this.SetCategorialPartialCedent(CedentEnum.Antecedent, 0, 0, taskID, taskType);
                    if (!savedCedents.ContainsKey(CedentEnum.Condition) || savedCedents[CedentEnum.Condition] == false)
                        this.SetBooleanPartialCedent(CedentEnum.Condition, 0, 0, taskID);
                    if (!savedCedents.ContainsKey(CedentEnum.FirstSet) || savedCedents[CedentEnum.FirstSet] == false)
                        this.SetBooleanPartialCedent(CedentEnum.FirstSet, 0, 0, taskID);
                    if (!savedCedents.ContainsKey(CedentEnum.SecondSet) || savedCedents[CedentEnum.SecondSet] == false)
                        this.SetBooleanPartialCedent(CedentEnum.SecondSet, 0, 0, taskID);
                    break;

                case TaskTypeEnum.SDKL:
                    if (!savedCedents.ContainsKey(CedentEnum.Antecedent) || savedCedents[CedentEnum.Antecedent] == false)
                        this.SetCategorialPartialCedent(CedentEnum.Antecedent, 0, 0, taskID, taskType);
                    if (!savedCedents.ContainsKey(CedentEnum.Succedent) || savedCedents[CedentEnum.Succedent] == false)
                        this.SetCategorialPartialCedent(CedentEnum.Succedent, 0, 0, taskID, taskType);
                    if (!savedCedents.ContainsKey(CedentEnum.Condition) || savedCedents[CedentEnum.Condition] == false)
                        this.SetBooleanPartialCedent(CedentEnum.Condition, 0, 0, taskID);
                    if (!savedCedents.ContainsKey(CedentEnum.FirstSet) || savedCedents[CedentEnum.FirstSet] == false)
                        this.SetBooleanPartialCedent(CedentEnum.FirstSet, 0, 0, taskID);
                    if (!savedCedents.ContainsKey(CedentEnum.SecondSet) || savedCedents[CedentEnum.SecondSet] == false)
                        this.SetBooleanPartialCedent(CedentEnum.SecondSet, 0, 0, taskID);
                    break;


                default:
                    throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(taskType);
            }
            #endregion

            #region DataMatrix (tmMatrix), Columns (tmAttribute), AbstractAttributes (tmQuantity), Category (tmCategory, tmCategoryEnumValue, tmInterval, tmValue)

            string dataMatrixName = "";
            int dataMatrixDBID = 0;
            string[] primaryKeyColumns = null; //must be sorted (for BinarySearch)
            string connectionString = null;
            allRowsInTaskDataMatrixCount = 0;
            ColumnSchemaInfo[] columnSchemaInfoSeq = null;
            Ferda.Modules.Boxes.DataMiningCommon.DataMatrix.DataMatrixInfo dataMatrixInfo;

            #region DataMatrix (tmMatrix)
            {
                AbstractAttributeStruct abstractAttributeStruct = abstractAttributes[0];
                dataMatrixInfo = abstractAttributeStruct.column.dataMatrix;

                //only for first attribute/column ... each attribute in one task must be in one and only one data matrix
                columnSchemaInfoSeq = dataMatrixInfo.explainDataMatrix;
                dataMatrixName = dataMatrixInfo.dataMatrixName;
                dataMatrixDBID = SetDataMatrix(dataMatrixName, dataMatrixInfo.recordsCount);
                updateTask(taskID, dataMatrixDBID);

                allRowsInTaskDataMatrixCount = dataMatrixInfo.recordsCount;

                string connectionStringForSave = dataMatrixInfo.database.odbcConnectionString.Trim();
                if (connectionStringForSave.StartsWith("DSN=", StringComparison.InvariantCultureIgnoreCase))
                    connectionStringForSave = connectionStringForSave.Substring(4);

                ExecuteUpdateQuery(
                    "UPDATE tpParamsDB SET "
                    + " strValue='" + connectionStringForSave + "' "
                    + " WHERE Name='DSN'");

                connectionString = dataMatrixInfo.database.odbcConnectionString;
                primaryKeyColumns = dataMatrixInfo.primaryKeyColumns;
                Array.Sort(primaryKeyColumns);
            }

            //Test values of primary key columns for duplicits (also test connection string and existence of data matrix)
            Ferda.Modules.Helpers.Data.DataMatrix.TestValuesInEnteredPrimaryKeyColumnsAreNotUniqueError(connectionString, dataMatrixName, primaryKeyColumns, boxIdentity);
            #endregion

            ColumnInfo columnInfo;
            CategoriesStruct categories;
            int columnID;
            int columnThereforeAlsoAttributeValueSubTypeDBID = 0;
            string columnSelectExpression;
            int primaryKeyColumnPosition;
            int categoryID;
            int enumCategoryValueID;
            int attributeID;
            //int attributeIdentifier; --allready defined
            string xCategory;
            string includeNullCategory;

            foreach (AbstractAttributeStruct abstractAttributeStruct in abstractAttributes)
            {
                //Protect from multiple saving columns or attributesAndEquivalenceClasses or caregories
                if (this.attributes.ContainsKey(abstractAttributeStruct.identifier))
                    continue;

                #region Columns (tmAttribute)

                if (dataMatrixName != abstractAttributeStruct.column.dataMatrix.dataMatrixName)
                {
                    throw Ferda.Modules.Exceptions.BoxRuntimeError(null, boxIdentity, "LM Task can not run over more than one datamatrix!");
                }
                columnSelectExpression = abstractAttributeStruct.column.columnSelectExpression;

                //Protect from multiple saving columns
                if (this.columns.ContainsKey(columnSelectExpression))
                {
                    columnID = this.columns[columnSelectExpression];
                }
                else
                {
                    columnInfo = abstractAttributeStruct.column;
                    primaryKeyColumnPosition = Array.BinarySearch(primaryKeyColumns, abstractAttributeStruct.column.columnSelectExpression);
                    columnThereforeAlsoAttributeValueSubTypeDBID = this.constants.ValueSubTypeEnumDictionary[columnInfo.columnSubType];
                    columnID = SetColumn(
                        columnInfo.columnSelectExpression,
                        dataMatrixDBID,
                        columnInfo.columnType,
                        columnInfo.columnSubType,
                        columnSelectExpression,
                        (primaryKeyColumnPosition >= 0) ? primaryKeyColumnPosition + 1 : -1);
                    this.columns.Add(columnInfo.columnSelectExpression, columnID);
                }

                int i = 0;
                foreach (string primaryKeyColumn in primaryKeyColumns)
                {
                    i++;
                    if (String.IsNullOrEmpty(primaryKeyColumn))
                        continue;
                    if (this.columns.ContainsKey(primaryKeyColumn))
                        continue;
                    foreach (ColumnSchemaInfo columnSchemaInfo in columnSchemaInfoSeq)
                    {
                        if (columnSchemaInfo.name == primaryKeyColumn)
                        {
                            int tmpColumnID = SetColumn(
                                columnSchemaInfo.name,
                                dataMatrixDBID,
                                ColumnTypeEnum.Ordinary,
                                Ferda.Modules.Helpers.Data.Column.GetColumnSubTypeByDataType(columnSchemaInfo.dataType),
                                columnSchemaInfo.name,
                                i);
                            this.columns.Add(columnSchemaInfo.name, tmpColumnID);
                            break;
                        }
                    }
                }

                #endregion

                #region Attribute (tmQuantity)

                attributeID = SetAttribute(
                        abstractAttributeStruct.nameInLiterals,
                        abstractAttributeStruct.identifier,
                        columnID);
                attributeIdentifier = abstractAttributeStruct.identifier;
                xCategory = abstractAttributeStruct.xCategory;
                includeNullCategory = abstractAttributeStruct.includeNullCategory;
                this.attributesCache.Add(abstractAttributeStruct.identifier, abstractAttributeStruct);
                this.attributes.Add(attributeIdentifier, attributeID);
                this.categories.Add(attributeIdentifier, new Dictionary<string, int>());
                this.categoriesOrds.Add(attributeIdentifier, new Dictionary<long, string>());

                #endregion

                #region Categories (tmCategory, tmCategoryEnumValue, tmInterval, tmValue)

                long categoryOrd = 0;
                categories = abstractAttributeStruct.categories;
                //if (categories.dateTimeIntervals != null)
                foreach (DictionaryEntry category in categories.dateTimeIntervals)
                {
                    categoryName = (string)category.Key;
                    //tmCategory
                    categoryOrd++;
                    categoryID = SetCategory(
                        categoryName,
                        attributeID,
                        CategoryTypeEnum.Interval,
                        (xCategory == categoryName),
                        (includeNullCategory == categoryName),
                        categoryOrd,
                        attributeIdentifier);
                    //tmValue, tmInterval
                    foreach (DateTimeIntervalStruct intervalStruct in (DateTimeIntervalStruct[])category.Value)
                    {
                        insertValueInterval(categoryID, intervalStruct);
                    }
                }
                //if (categories.floatIntervals != null)
                foreach (DictionaryEntry category in categories.floatIntervals)
                {
                    categoryName = (string)category.Key;
                    //tmCategory
                    categoryOrd++;
                    categoryID = SetCategory(
                        categoryName,
                        attributeID,
                        CategoryTypeEnum.Interval,
                        (xCategory == categoryName),
                        (includeNullCategory == categoryName),
                        categoryOrd,
                        attributeIdentifier);
                    //tmValue, tmInterval
                    foreach (FloatIntervalStruct intervalStruct in (FloatIntervalStruct[])category.Value)
                    {
                        insertValueInterval(categoryID, intervalStruct);
                    }
                }
                //if (categories.longIntervals != null)
                foreach (DictionaryEntry category in categories.longIntervals)
                {
                    categoryName = (string)category.Key;
                    //tmCategory
                    categoryOrd++;
                    categoryID = SetCategory(
                        categoryName,
                        attributeID,
                        CategoryTypeEnum.Interval,
                        (xCategory == categoryName),
                        (includeNullCategory == categoryName),
                        categoryOrd,
                        attributeIdentifier);
                    //tmValue, tmInterval
                    foreach (LongIntervalStruct intervalStruct in (LongIntervalStruct[])category.Value)
                    {
                        insertValueInterval(categoryID, intervalStruct);
                    }
                }
                //if (categories.enums != null)
                foreach (DictionaryEntry category in categories.enums)
                {
                    categoryName = (string)category.Key;
                    //tmCategory
                    categoryOrd++;
                    categoryID = SetCategory(
                        categoryName,
                        attributeID,
                        CategoryTypeEnum.Enumeration,
                        (xCategory == categoryName),
                        (includeNullCategory == categoryName),
                        categoryOrd,
                        attributeIdentifier);
                    foreach (string enumItem in (string[])category.Value)
                    {
                        //tmValue
                        if (categoryName == includeNullCategory && String.IsNullOrEmpty(enumItem))
                        {
                            enumCategoryValueID = SetValue();
                            continue;
                        }
                        else if (String.IsNullOrEmpty(enumItem))
                        {
                            //DEBUG NOTES: for debugin purposes it can be usevull if this else-if branch is removed (commented)
                            enumCategoryValueID = SetValue();
                            continue;
                        }
                        switch (columnThereforeAlsoAttributeValueSubTypeDBID)
                        {
                            case 1: //long
                                enumCategoryValueID = SetValue(Convert.ToInt64(enumItem));
                                break;
                            case 2: //double
                                enumCategoryValueID = SetValue(Convert.ToDouble(enumItem));
                                break;
                            case 3: //string
                                enumCategoryValueID = SetValue(enumItem);
                                break;
                            case 4: //bool
                                enumCategoryValueID = SetValue(Convert.ToBoolean(enumItem));
                                break;
                            case 5: //date
                                enumCategoryValueID = SetValue(Convert.ToDateTime(enumItem));
                                break;
                            default:
                                throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(columnThereforeAlsoAttributeValueSubTypeDBID);
                        }
                        //tmCategoryEnumValue
                        this.SetCategoryEnum(
                            categoryID,
                            enumCategoryValueID);
                    }
                }

                #endregion
            }

            #endregion

            #region CategorialCedent (tdKLCedentD, tdCFCedentD, tdKLLiteralD, tdCFLiteralD)

            int categorialCedentID;
            int categorialLiteralID;

            if (categorialCedents != null)
                foreach (CategorialCedent categorialCedent in categorialCedents)
                {
                    CedentIdentifierAndType cedentIdentifierAndType = new CedentIdentifierAndType(categorialCedent.Cedent.identifier, categorialCedent.CedentType);
                    if (this.categorialPartialCedent.ContainsKey(cedentIdentifierAndType))
                    {
                        categorialCedentID = this.categorialPartialCedent[cedentIdentifierAndType];
                    }
                    else
                    {
                        categorialCedentID = SetCategorialPartialCedent(
                            categorialCedent.CedentType,
                            categorialCedent.Cedent.minLen,
                            categorialCedent.Cedent.maxLen,
                            taskID,
                            taskType);
                        this.categorialPartialCedent.Add(cedentIdentifierAndType, categorialCedentID);

                        if (categorialCedent.Cedent.attributes != null)
                            foreach (AbstractAttributeStruct abstractAttributeStruct in categorialCedent.Cedent.attributes)
                            {
                                categorialLiteralID = SetCategorialLiteralSetting(
                                    categorialCedentID,
                                    this.attributes[abstractAttributeStruct.identifier],
                                    taskType,
                                    abstractAttributeStruct.identifier);
                            }
                    }
                }

            #endregion

            #region BooleanCedent (tdLiteralD)

            int literalID;

            if (booleanCedents != null)
            {
                foreach (BooleanCedent booleanCedent in booleanCedents)
                {
                    if (booleanCedent.Cedent.literalSettings != null)
                        foreach (LiteralSettingStruct literalSettingStruct in booleanCedent.Cedent.literalSettings)
                        {
                            int literalOneCategoryDBID = (literalSettingStruct.atomSetting.category.Length > 0)
                                ? this.categories[literalSettingStruct.atomSetting.abstractAttribute.identifier][literalSettingStruct.atomSetting.category[0]]
                                : 0;
                            int literalEquivalenceClass = 0;
                            try
                            {
                                literalEquivalenceClass = this.equivalenceClasses[literalsAndEquivalenceClasses[literalSettingStruct.identifier]];
                            }
                            catch (KeyNotFoundException) { }
                            literalID = this.SetLiteralSetting(
                                    literalSettingStruct.atomSetting.minLen,
                                    literalSettingStruct.atomSetting.maxLen,
                                    literalSettingStruct.atomSetting.coefficientType,
                                    literalEquivalenceClass,
                                    literalSettingStruct.gaceType,
                                    literalSettingStruct.literalType,
                                    literalOneCategoryDBID,
                                    this.booleanPartialCedents[new CedentIdentifierAndType(booleanCedent.Cedent.identifier, booleanCedent.CedentType)],
                                    this.attributes[literalSettingStruct.atomSetting.abstractAttribute.identifier]);
                            this.literals.Add(literalID, literalSettingStruct.identifier);
                            this.attributeNameInLiterals.Add(literalID, literalSettingStruct.atomSetting.abstractAttribute.nameInLiterals);
                            //abstractAttributes.Add(literalSettingStruct.atomSetting.abstractAttribute);
                        }
                }
            }

            #endregion
        }
    }
}
