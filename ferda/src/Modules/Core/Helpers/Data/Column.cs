using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Odbc;
using Ferda.Modules.Boxes.DataMiningCommon.Column;

namespace Ferda.Modules.Helpers.Data
{
    /// <summary>
    /// Provides a set of methods and properties that help work with (DB) column.
    /// </summary>
    public static class Column
    {
        /// <summary>
        /// Test <c>columnSelectExpression</c> over data matrix.
        /// </summary>
        /// <param name="odbcConnectionString">The ODBC connection string.</param>
        /// <param name="dataMatrixName">Name of the data matrix.</param>
        /// <param name="columnSelectExpression">The column select expression.</param>
        /// <param name="boxIdentity">An identity of BoxModule.</param>
        /// <exception cref="T:Ferda.Modules.BadParamsError">Thrown if <c>odbcConnectionString</c> or <c>dataMatrixName</c> or <c>columnSelectExpression</c> parameter is wrong.</exception>
        public static void TestColumnSelectExpression(string odbcConnectionString, string dataMatrixName, string columnSelectExpression, string boxIdentity)
        {
            //throws exception if odbcConnectionString is wrong
            OdbcConnection conn = Ferda.Modules.Helpers.Data.OdbcConnections.GetConnection(odbcConnectionString, boxIdentity);

            try
            {
                OdbcCommand command = new OdbcCommand("SELECT " + "`" + columnSelectExpression + "`" + " FROM " + "`" + dataMatrixName + "`" + " WHERE 0", conn);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //throws exception if dataMatrixName is wrong
                DataMatrix.TestDataMatrixExists(odbcConnectionString, dataMatrixName, boxIdentity);

                //throws exception if columnSelectExpression is wrong
                throw Ferda.Modules.Exceptions.BadParamsError(ex, boxIdentity, "Bad select column sql expression!", Ferda.Modules.restrictionTypeEnum.DbColumn);
            }
        }

        #region Value Types Mischmasch (nominal/ordinal/cardinal, ...)
        /// <summary>
        /// Determines whether colum sub type is cardinal.
        /// </summary>
        /// <param name="columnValueSubType">Value sub type of the column.</param>
        /// <returns>
        /// <c>true</c> if colum sub type is cardinal; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsColumSubTypeCardinal(ValueSubTypeEnum columnValueSubType)
        {
            switch (GetColumnValueTypeByValueSubType(columnValueSubType))
            {
                case ValueType.Integral:
                case ValueType.Floating:
                case ValueType.DateTime:
                    //case ValueType.Time:
                    return true;
                default:
                    return false;
            }
        }
        /// <summary>
        /// Determines whether colum sub type is ordinal.
        /// </summary>
        /// <param name="columnValueSubType">Value sub type of the column.</param>
        /// <returns>
        /// <c>true</c> if colum value sub type is ordinal; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsColumSubTypeOrdinal(ValueSubTypeEnum columnValueSubType)
        {
            switch (GetColumnValueTypeByValueSubType(columnValueSubType))
            {
                case ValueType.Unknown:
                    return false;
                default:
                    return true;
            }
        }
        
        /// <summary>
        /// Value type as simplication of <see cref="T:Ferda.Modules.ValueSubTypeEnum"/>.
        /// </summary>
        public enum ValueType
        {
            /// <summary>
            /// Unknown value type
            /// </summary>
            Unknown,

            /// <summary>
            /// [signed/unsigned]Short/Int/Long value type
            /// </summary>
            Integral,
            
            /// <summary>
            /// [signed/unsigned]Float/Double/Decimal value type
            /// </summary>
            Floating,
            
            /// <summary>
            /// Date/DateTime/Time value type
            /// </summary>
            DateTime,
            //Time,
            
            /// <summary>
            /// Boolean value type
            /// </summary>
            Boolean,
            
            /// <summary>
            /// String value type
            /// </summary>
            String
        }

        /// <summary>
        /// Gets the <see cref="T:Ferda.Modules.Helpers.Data.Column.ValueType">value type</see> 
        /// of the column by <see cref="T:Ferda.Modules.ValueSubTypeEnum">value sub type</see>.
        /// </summary>
        /// <param name="columnValueSubType">Type of the column value sub.</param>
        /// <returns></returns>
        public static ValueType GetColumnValueTypeByValueSubType(ValueSubTypeEnum columnValueSubType)
        {
            switch (columnValueSubType)
            {
                case ValueSubTypeEnum.ShortIntegerType:
                case ValueSubTypeEnum.UnsignedShortIntegerType:
                case ValueSubTypeEnum.IntegerType:
                case ValueSubTypeEnum.UnsignedIntegerType:
                case ValueSubTypeEnum.LongIntegerType:
                case ValueSubTypeEnum.UnsignedLongIntegerType:
                    return ValueType.Integral;
                case ValueSubTypeEnum.FloatType:
                case ValueSubTypeEnum.DoubleType:
                case ValueSubTypeEnum.DecimalType:
                    return ValueType.Floating;
                case ValueSubTypeEnum.DateTimeType:
                    return ValueType.DateTime;
                //case ValueSubTypeEnum.TimeType:
                ////return ValueType.Time;
                //return ValueType.DateTime;
                case ValueSubTypeEnum.BooleanType:
                    return ValueType.Boolean;
                case ValueSubTypeEnum.StringType:
                    return ValueType.String;
                case ValueSubTypeEnum.Unknown:
                    return ValueType.Unknown;
                default:
                    throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(columnValueSubType);
            }
        }

        /// <summary>
        /// Gets the value sub type of the column by data type.
        /// </summary>
        /// <param name="columnDbDataType">Column DB data type.</param>
        /// <returns>Value sub type of the column.</returns>
        public static ValueSubTypeEnum GetColumnSubTypeByDataType(string columnDbDataType)
        {
            switch (columnDbDataType)
            {
                case "System.Int16":
                    return ValueSubTypeEnum.ShortIntegerType;
                case "System.UInt16":
                    return ValueSubTypeEnum.UnsignedShortIntegerType;
                case "System.Int32":
                    return ValueSubTypeEnum.IntegerType;
                case "System.UInt32":
                    return ValueSubTypeEnum.UnsignedIntegerType;
                case "System.Int64":
                    return ValueSubTypeEnum.LongIntegerType;
                case "System.UInt64":
                    return ValueSubTypeEnum.UnsignedLongIntegerType;
                case "System.Single":
                    return ValueSubTypeEnum.FloatType;
                case "System.Double":
                    return ValueSubTypeEnum.DoubleType;
                case "System.Decimal":
                    return ValueSubTypeEnum.DecimalType;
                case "System.Boolean":
                    return ValueSubTypeEnum.BooleanType;
                case "System.String":
                    return ValueSubTypeEnum.StringType;
                case "System.DateTime":
                    return ValueSubTypeEnum.DateTimeType;
                //return ValueSubTypeEnum.DateType;
                case "System.TimeSpan":
                    return ValueSubTypeEnum.TimeType;
                case unknownDbColumnDataType:
                default:
                    System.Diagnostics.Debug.WriteLine("Unknown column data type: " + columnDbDataType);
                    return ValueSubTypeEnum.Unknown;
            }
        }
        private const string unknownDbColumnDataType = "Unknown";
        #endregion

        /// <summary>
        /// Gets the data type of the column values (determined by DB schema).
        /// </summary>
        /// <param name="odbcConnectionString">The ODBC connection string.</param>
        /// <param name="dataMatrixName">Name of the data matrix.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="boxIdentity">The box identity.</param>
        /// <returns>String as name of .NET data type of specified column (e.g. "System.String").</returns>
        /// <exception cref="T:Ferda.Modules.BadParamsError">Thrown if <c>odbcConnectionString</c> or <c>dataMatrixName</c> or <c>columnSelectExpression</c> parameter is wrong.</exception>
        public static string GetDataType(string odbcConnectionString, string dataMatrixName, string columnName, string boxIdentity)
        {
            //throws exception if odbcConnectionString is wrong
            OdbcConnection conn = Ferda.Modules.Helpers.Data.OdbcConnections.GetConnection(odbcConnectionString, boxIdentity);
            try
            {
                OdbcCommand myCommand = new OdbcCommand("SELECT * FROM " + dataMatrixName + " WHERE 0", conn);
                OdbcDataReader myReader = myCommand.ExecuteReader();
                DataTable schemaTable = myReader.GetSchemaTable();
                foreach (DataRow myRow in schemaTable.Rows)
                {
                    if (0 == String.Compare(myRow["ColumnName"].ToString(), columnName.ToLower(), true))
                    {
                        return myRow["DataType"].ToString();
                    }
                }
                return unknownDbColumnDataType;
            }
            catch (Exception ex)
            {
                //throws exception if dataMatrixName or columnName is wrong
                TestColumnSelectExpression(odbcConnectionString, dataMatrixName, columnName, boxIdentity);

                //or other reason for exception
                throw Ferda.Modules.Exceptions.BadParamsUnexpectedReasonError(ex, boxIdentity);
            }
        }

        /// <summary>
        /// Name of column label in query to distinct values of the column.
        /// </summary>
        public const string SelectDistincts = "DistinctsColumn";

        /// <summary>
        /// Name of column label in query to frequencies of the column.
        /// </summary>
        public const string SelectFrequency = "DistinctsFrequency";

        /// <summary>
        /// Gets <see cref="T:System.Data.DataTable"/> selecting distinct 
        /// values and it`s frequencies of specified <c>columnSelectExpression</c>.
        /// </summary>
        /// <param name="odbcConnectionString">The ODBC connection string.</param>
        /// <param name="dataMatrixName">Name of the data matrix.</param>
        /// <param name="columnSelectExpression">The column select expression.</param>
        /// <param name="whereCondition">The where condition.</param>
        /// <param name="boxIdentity">The box identity.</param>
        /// <returns>
        /// <see cref="T:System.Data.DataTable"/> whith two columns. First, named 
        /// <see cref="F:Ferda.Modules.Helpers.Data.Column.SelectDistincts"/>,
        /// selects the "values" of the specified <c>columnSelectExpression</c> 
        /// and the second one, named 
        /// <see cref="F:Ferda.Modules.Helpers.Data.Column.SelectFrequency"/>,
        /// selects the frequencies fo corresponding values.
        /// </returns>
        /// <exception cref="T:Ferda.Modules.BadParamsError">Thrown if <c>odbcConnectionString</c> or <c>dataMatrixName</c> or <c>columnSelectExpression</c> parameter is wrong.</exception>
        /// <seealso cref="F:Ferda.Modules.Helpers.Data.Column.SelectDistincts"/>
        /// <seealso cref="F:Ferda.Modules.Helpers.Data.Column.SelectFrequency"/>
        public static DataTable GetDistinctsAndFrequencies(string odbcConnectionString, string dataMatrixName, string columnSelectExpression, string whereCondition, string boxIdentity)
        {
            //throws exception if odbcConnectionString is wrong
            OdbcConnection conn = Ferda.Modules.Helpers.Data.OdbcConnections.GetConnection(odbcConnectionString, boxIdentity);

            string where = (String.IsNullOrEmpty(whereCondition))
                ? String.Empty
                : " WHERE " + whereCondition;

            string query =
                "SELECT "
                    + "`" + columnSelectExpression + "`" + " AS " + SelectDistincts
                    + ", COUNT(1) AS " + SelectFrequency
                + " FROM " + dataMatrixName
                    + where
                    + " GROUP BY " + "`" + columnSelectExpression + "`"
                    + " ORDER BY " + "`" + columnSelectExpression + "`";

            try
            {

                OdbcDataAdapter odbcDataAdapter = new OdbcDataAdapter(query, conn);
                DataSet dataSet = new DataSet();
                odbcDataAdapter.Fill(dataSet);
                DataTable result = dataSet.Tables[0];
                return result;
            }
            catch (Exception ex)
            {
                //throws exception if dataMatrixName or columnName is wrong
                TestColumnSelectExpression(odbcConnectionString, dataMatrixName, columnSelectExpression, boxIdentity);

                //or other reason for exception
                throw Ferda.Modules.Exceptions.BadParamsUnexpectedReasonError(ex, boxIdentity);
            }
        }

        /// <summary>
        /// Gets <see cref="T:System.Data.DataTable"/> selecting distinct 
        /// values and it`s frequencies of specified <c>columnSelectExpression</c>.
        /// </summary>
        /// <param name="odbcConnectionString">The ODBC connection string.</param>
        /// <param name="dataMatrixName">Name of the data matrix.</param>
        /// <param name="columnSelectExpression">The column select expression.</param>
        /// <param name="boxIdentity">The box identity.</param>
        /// <returns>
        /// <see cref="T:System.Data.DataTable"/> whith two columns. First, named 
        /// <see cref="F:Ferda.Modules.Helpers.Data.Column.SelectDistincts"/>,
        /// selects the "values" of the specified <c>columnSelectExpression</c> 
        /// and the second one, named 
        /// <see cref="F:Ferda.Modules.Helpers.Data.Column.SelectFrequency"/>,
        /// selects the frequencies fo corresponding values.
        /// </returns>
        /// <exception cref="T:Ferda.Modules.BadParamsError">Thrown if <c>odbcConnectionString</c> or <c>dataMatrixName</c> or <c>columnSelectExpression</c> parameter is wrong.</exception>
        public static DataTable GetDistinctsAndFrequencies(string odbcConnectionString, string dataMatrixName, string columnSelectExpression, string boxIdentity)
        {
            return GetDistinctsAndFrequencies(odbcConnectionString, dataMatrixName, columnSelectExpression, null, boxIdentity);
        }

        /// <summary>
        /// Gets the <see cref="T:Ferda.Modules.Boxes.DataMiningCommon.Column.StatisticsInfo">statistics</see>
        /// i.e. count of distinct values, minimal result, maximal result, average result 
        /// (if result type is not cardinal average length is returned) and for cardinal values
        /// also computes variability and standard deviation.
        /// </summary>
        /// <param name="odbcConnectionString">The ODBC connection string.</param>
        /// <param name="dataMatrixName">Name of the data matrix.</param>
        /// <param name="columnSelectExpression">The column select expression.</param>
        /// <param name="columnSubType">Type of the column sub.</param>
        /// <param name="boxIdentity">The box identity.</param>
        /// <returns>
        /// <see cref="T:Ferda.Modules.Boxes.DataMiningCommon.Column.StatisticsInfo"/> 
        /// i.e. count of distinct values, minimal result, maximal result, average result 
        /// (if result type is not cardinal average length is returned) and for cardinal values
        /// also computes variability and standard deviation.
        /// </returns>
        public static StatisticsInfo GetStatistics(string odbcConnectionString, string dataMatrixName, string columnSelectExpression, ValueSubTypeEnum columnSubType, string boxIdentity)
        {

            //throws exception if odbcConnectionString is wrong
            OdbcConnection conn = Ferda.Modules.Helpers.Data.OdbcConnections.GetConnection(odbcConnectionString, boxIdentity);

            StatisticsInfo result = new StatisticsInfo();
            OdbcCommand odbcCommand = new OdbcCommand();
            odbcCommand.Connection = conn;

            bool isCardinal = false;

            try
            {
                OdbcDataAdapter myDataAdapter = new OdbcDataAdapter(
                    "SELECT DISTINCT " + "`" + columnSelectExpression + "`" + " FROM " + "`" + dataMatrixName + "`",
                    conn);
                System.Data.DataSet myDataSet = new System.Data.DataSet();
                myDataAdapter.Fill(myDataSet);
                result.ValueDistincts = Convert.ToInt64(myDataSet.Tables[0].Rows.Count);

                /* much more effective but unsupported 
                 * UNDONE tohle predelat
                odbcCommand.CommandText = "SELECT COUNT(DISTINCT " + "`" + columnSelectExpression + "`" + ") FROM " + "`" + dataMatrixName + "`";
                result.ValueDistincts = Convert.ToInt64(odbcCommand.ExecuteScalar());
                 * */

                isCardinal = IsColumSubTypeCardinal(GetColumnSubTypeByDataType(GetDataType(odbcConnectionString, dataMatrixName, columnSelectExpression, boxIdentity)));
            }
            catch (Exception ex)
            {
                //throws exception if dataMatrixName or columnName is wrong
                TestColumnSelectExpression(odbcConnectionString, dataMatrixName, columnSelectExpression, boxIdentity);

                //or other reason for exception
                throw Ferda.Modules.Exceptions.BadParamsUnexpectedReasonError(ex, boxIdentity);
            }

            string selectMaxExpression = "MAX(" + "`" + columnSelectExpression + "`" + ") AS Maximum";
            string selectMinExpression = "MIN(" + "`" + columnSelectExpression + "`" + ") AS Minimum";
            string selectAvgExpression = (isCardinal)
                ? "AVG(" + "`" + columnSelectExpression + "`" + ") AS Average"
                : "AVG(LEN(" + "`" + columnSelectExpression + "`" + ")) AS Average";

            odbcCommand.CommandText = "SELECT "
                + selectMaxExpression + ","
                + selectMinExpression + ","
                + selectAvgExpression
                + " FROM " + "`" + dataMatrixName + "`";

            //System.Diagnostics.Debug.WriteLine("aggr_s:" + DateTime.Now.ToString());
            OdbcDataReader odbcDataReader = odbcCommand.ExecuteReader();
            //System.Diagnostics.Debug.WriteLine("aggr_e:" + DateTime.Now.ToString());

            if (odbcDataReader.Read())
            {
                result.ValueMax = odbcDataReader["Maximum"].ToString();
                result.ValueMin = odbcDataReader["Minimum"].ToString();
                result.ValueAverage = odbcDataReader["Average"].ToString();
            }
            odbcDataReader.Close();

            //System.Diagnostics.Debug.WriteLine("card_s:" + DateTime.Now.ToString());
            if (isCardinal)
            {
                odbcCommand.CommandText = "SELECT COUNT(1) FROM " + "`" + dataMatrixName + "`";
                long dataMatrixRowsCount = Convert.ToInt64(odbcCommand.ExecuteScalar());

                //TODO optimize this
                odbcCommand.CommandText =
                    "SELECT SUM( "
                        + "(" + "`" + columnSelectExpression + "`" + " - '" + result.ValueAverage + "')"
                        + " * (" + "`" + columnSelectExpression + "`" + " - '" + result.ValueAverage + "')"
                        + ") FROM " + "`" + dataMatrixName + "`";

                result.ValueVariability = Convert.ToDouble(odbcCommand.ExecuteScalar()) / dataMatrixRowsCount;
                result.ValueStandardDeviation = Math.Sqrt(result.ValueVariability);
            }
            //System.Diagnostics.Debug.WriteLine("card_e:" + DateTime.Now.ToString());

            #region old implementation

            /* /
            //begin of old implementation
            object commandResult;

            odbcCommand.CommandText = "SELECT MAX(" + "`" + columnSelectExpression + "`" + ") AS Maximum FROM " + + "`" + dataMatrixName + "`";
            commandResult = odbcCommand.ExecuteScalar();
            result.ValueMax =
                (commandResult != null) ? commandResult.ToString() : null;

            odbcCommand.CommandText = "SELECT MIN(" + "`" + columnSelectExpression + "`" + ") AS Minimum FROM " + "`" + dataMatrixName + "`";
            commandResult = odbcCommand.ExecuteScalar();
            result.ValueMin =
                (commandResult != null) ? commandResult.ToString() : null;

            if (isCardinal)
            {
                odbcCommand.CommandText = "SELECT AVG(" + "`" + columnSelectExpression +"`" + ") AS Average FROM " + "`" + dataMatrixName + "`";
                commandResult = odbcCommand.ExecuteScalar();
                result.ValueAverage = (commandResult != null) ? commandResult.ToString() : null;

                odbcCommand.CommandText = "SELECT COUNT(1) FROM " + "`" + dataMatrixName + "`";
                long dataMatrixRowsCount = Convert.ToInt64(odbcCommand.ExecuteScalar());

                odbcCommand.CommandText =
                    "SELECT SUM( "
                        + "(" + "`" + columnSelectExpression + "`" + " - '" + result.ValueAverage + "')"
                        + "* (" + "`" + columnSelectExpression + "`" + " - '" + result.ValueAverage + "')"
                        + ") AS Variability FROM " + "`" + dataMatrixName + "`";

                result.ValueVariability =
                    Convert.ToDouble(odbcCommand.ExecuteScalar()) / dataMatrixRowsCount;

                result.ValueStandardDeviation = Math.Sqrt(result.ValueVariability);
            }
            else
            {
                odbcCommand.CommandText = "SELECT AVG(LEN(" + "`" + columnSelectExpression + "`" + ")) AS AverageLen FROM " + "`" + dataMatrixName + "`";
                commandResult = odbcCommand.ExecuteScalar();
                result.ValueAverage = (commandResult != null) ? commandResult.ToString() : null;

                result.ValueVariability = 0;

                result.ValueStandardDeviation = 0;
            }

            //end of old implementation
            /* */
            #endregion

            return result;
        }

        /// <summary>
        /// Gets <see cref="T:System.Data.DataTable"/> selecting distinct 
        /// values of specified <c>columnSelectExpression</c>.
        /// </summary>
        /// <param name="odbcConnectionString">The ODBC connection string.</param>
        /// <param name="dataMatrixName">Name of the data matrix.</param>
        /// <param name="columnSelectExpression">The column select expression.</param>
        /// <param name="boxIdentity">The box identity.</param>
        /// <returns>
        /// <see cref="T:System.Data.DataTable"/> whith one column, named 
        /// <see cref="F:Ferda.Modules.Helpers.Data.Column.SelectDistincts"/>,
        /// selects the "values" of the specified <c>columnSelectExpression</c>.
        /// </returns>
        /// <exception cref="T:Ferda.Modules.BadParamsError">Thrown if <c>odbcConnectionString</c> or <c>dataMatrixName</c> or <c>columnSelectExpression</c> parameter is wrong.</exception>
        /// <seealso cref="F:Ferda.Modules.Helpers.Data.Column.SelectDistincts"/>
        public static DataTable GetDistincts(string odbcConnectionString, string dataMatrixName, string columnSelectExpression, string boxIdentity)
        {
            //throws exception if odbcConnectionString is wrong
            OdbcConnection conn = Ferda.Modules.Helpers.Data.OdbcConnections.GetConnection(odbcConnectionString, boxIdentity);

            try
            {
                OdbcDataAdapter myDataAdapter = new OdbcDataAdapter(
                    "SELECT DISTINCT " + columnSelectExpression + " AS " + SelectDistincts + " FROM " + "`" + dataMatrixName + "`",
                    conn);
                DataSet myDataSet = new DataSet();
                myDataAdapter.Fill(myDataSet);
                return myDataSet.Tables[0];
            }
            catch (Exception ex)
            {
                //throws exception if dataMatrixName or columnName is wrong
                TestColumnSelectExpression(odbcConnectionString, dataMatrixName, columnSelectExpression, boxIdentity);

                //or other reason for exception
                throw Ferda.Modules.Exceptions.BadParamsUnexpectedReasonError(ex, boxIdentity);
            }
        }

        /// <summary>
        /// Gets distinct values of specified <c>columnSelectExpression</c> as 
        /// array of <see cref="T:System.String">strings</see>.
        /// </summary>
        /// <param name="odbcConnectionString">The ODBC connection string.</param>
        /// <param name="dataMatrixName">Name of the data matrix.</param>
        /// <param name="columnSelectExpression">The column select expression.</param>
        /// <param name="boxIdentity">The box identity.</param>
        /// <returns>
        /// Array of strings as distinct values, where 
        /// <see cref="F:System.DBNull.Value"/> is replaced by <see cref="F:Ferda.Modules.Helpers.Common.Constants.DbNullCategoryName"/>
        /// and <see cref="F:System.String.Empty"/> is replaced by <see cref="F:Ferda.Modules.Helpers.Common.Constants.EmptyStringCategoryName"/>.
        /// </returns>
        public static string[] GetDistinctsStringSeq(string odbcConnectionString, string dataMatrixName, string columnSelectExpression, string boxIdentity)
        {
            DataTable dataTable = Column.GetDistincts(odbcConnectionString, dataMatrixName, columnSelectExpression, boxIdentity);

            List<string> result = new List<string>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                if (dataRow.ItemArray[0] == System.DBNull.Value)
                    result.Add(Ferda.Modules.Helpers.Common.Constants.DbNullCategoryName);
                else if (dataRow.ItemArray[0].ToString() == String.Empty)
                    result.Add(Ferda.Modules.Helpers.Common.Constants.EmptyStringCategoryName);
                else
                    result.Add(dataRow.ItemArray[0].ToString());
            }
            return result.ToArray();
        }
    }
}