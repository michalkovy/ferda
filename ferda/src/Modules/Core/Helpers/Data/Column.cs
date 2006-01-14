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
		/// <param name="odbcConnectionString">An ODBC connection string for connecting DB.</param>
		/// <param name="dataMatrixName">A name of data matrix.</param>
		/// <param name="columnSelectExpression">A SELECT expression (as column) for test.</param>
		/// <param name="boxIdentity">An identity of BoxModule.</param>
		/// <exception cref="T:Ferda.Modules.Boxes.DataMiningCommon.Column.BadSelectColumnExpressionError"/>
		/// <exception cref="T:Ferda.Modules.Boxes.DataMiningCommon.DataMatrix.CouldNotFoundDataMatrixInDatabaseError"/>
		/// <exception cref="T:Ferda.Modules.BadParamsError"/>
		public static void TestColumnSelectExpression(string odbcConnectionString, string dataMatrixName, string columnSelectExpression, string boxIdentity, bool leaveConnection)
		{
			DataMatrix.TestDataMatrixExists(odbcConnectionString, dataMatrixName, boxIdentity, false);
			OdbcConnection conn = Ferda.Modules.Helpers.Data.OdbcConnections.GetConnection(odbcConnectionString, boxIdentity);

			try
			{
				OdbcCommand command = new OdbcCommand("SELECT " + columnSelectExpression + " FROM " + dataMatrixName + " WHERE 0", conn);
				command.ExecuteNonQuery();
				if (leaveConnection)
					Ferda.Modules.Helpers.Data.OdbcConnections.LeaveConnection(odbcConnectionString);
			}
			catch (Exception e)
			{
                throw Ferda.Modules.Exceptions.BadParamsError(e, boxIdentity, "Bad select column sql expression!", Ferda.Modules.restrictionTypeEnum.DbColumn);
			}
		}
		public static bool IsColumSubTypeCardinal(ValueSubTypeEnum columnValueSubType)
		{
			switch (ColumnSimpleSubType(columnValueSubType))
			{
				case SimpleTypeEnum.Integral:
				case SimpleTypeEnum.Floating:
				case SimpleTypeEnum.DateTime:
				//case SimpleTypeEnum.Time:
					return true;
				default:
					return false;
			}
		}
		public static bool IsColumSubTypeOrdinal(ValueSubTypeEnum columnValueSubType)
		{
			switch (ColumnSimpleSubType(columnValueSubType))
			{
				case SimpleTypeEnum.Unknown:
					return false;
				default:
					return true;
			}
		}
		public enum SimpleTypeEnum
		{
			Unknown,
			Integral,
			Floating,
			DateTime,
			//Time,
			Boolean,
			String			
		}
		public static SimpleTypeEnum ColumnSimpleSubType(ValueSubTypeEnum columnValueSubType)
		{
			switch (columnValueSubType)
			{
				case ValueSubTypeEnum.ShortIntegerType:
				case ValueSubTypeEnum.UnsignedShortIntegerType:
				case ValueSubTypeEnum.IntegerType:
				case ValueSubTypeEnum.UnsignedIntegerType:
				case ValueSubTypeEnum.LongIntegerType:
				case ValueSubTypeEnum.UnsignedLongIntegerType:
					return SimpleTypeEnum.Integral;
				case ValueSubTypeEnum.FloatType:
				case ValueSubTypeEnum.DoubleType:
				case ValueSubTypeEnum.DecimalType:
					return SimpleTypeEnum.Floating;
				case ValueSubTypeEnum.DateTimeType:
					return SimpleTypeEnum.DateTime;
				//case ValueSubTypeEnum.TimeType:
					////return SimpleTypeEnum.Time;
					//return SimpleTypeEnum.DateTime;
				case ValueSubTypeEnum.BooleanType:
					return SimpleTypeEnum.Boolean;
				case ValueSubTypeEnum.StringType:
					return SimpleTypeEnum.String;
				case ValueSubTypeEnum.Unknown:
					return SimpleTypeEnum.Unknown;
				default:
					throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(columnValueSubType);
			}
		}
		public static ValueSubTypeEnum ComputeColumnSubType(string columnDBDataType)
		{
			switch (columnDBDataType)
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
				default:
					System.Diagnostics.Debug.WriteLine("Unknown column data type: " + columnDBDataType);
					return ValueSubTypeEnum.Unknown;
			}
		}
		public static string ComputeColumnType(string odbcConnectionString, string dataMatrixName, string columnName, string boxIdentity, bool leaveConnection)
		{
			TestColumnSelectExpression(odbcConnectionString, dataMatrixName, columnName, boxIdentity, false);
			OdbcConnection conn = Ferda.Modules.Helpers.Data.OdbcConnections.GetConnection(odbcConnectionString, boxIdentity);
			OdbcCommand myCommand =
				new OdbcCommand("SELECT * FROM " + dataMatrixName + " WHERE 0", conn);
			OdbcDataReader myReader = myCommand.ExecuteReader();
			DataTable schemaTable = myReader.GetSchemaTable();
			string type = null;
			foreach (DataRow myRow in schemaTable.Rows)
			{
				if (myRow["ColumnName"].ToString().ToLower() == columnName.ToLower())
				{
					type = myRow["DataType"].ToString();
					break;
				}
			}
			if (leaveConnection)
				Ferda.Modules.Helpers.Data.OdbcConnections.LeaveConnection(odbcConnectionString);
			return type;
		}

		public static DataTable ComputeColumnDistinctsAndItsFrequencies(string odbcConnectionString, string dataMatrixName, string columnSelectExpression, string whereCondition, string boxIdentity, bool leaveConnection)
		{
			if (String.IsNullOrEmpty(whereCondition))
				return ComputeColumnDistinctsAndItsFrequencies(odbcConnectionString, dataMatrixName, columnSelectExpression, boxIdentity, false);
			TestColumnSelectExpression(odbcConnectionString, dataMatrixName, columnSelectExpression, boxIdentity, false);
			OdbcConnection conn = Ferda.Modules.Helpers.Data.OdbcConnections.GetConnection(odbcConnectionString, boxIdentity);
			string query =
				"SELECT "
				+ columnSelectExpression + " AS TmpName "
				+ ", COUNT(" + columnSelectExpression + ") AS Frequency "
				+ " FROM " + dataMatrixName + " WHERE " + whereCondition + " GROUP BY " + columnSelectExpression + " ORDER BY " + columnSelectExpression;
			OdbcDataAdapter odbcDataAdapter = new OdbcDataAdapter(query, conn);
			DataSet dataSet = new DataSet();
			odbcDataAdapter.Fill(dataSet);
			if (leaveConnection)
				Ferda.Modules.Helpers.Data.OdbcConnections.LeaveConnection(odbcConnectionString);
			return dataSet.Tables[0];
		}

		public static DataTable ComputeColumnDistinctsAndItsFrequencies(string odbcConnectionString, string dataMatrixName, string columnSelectExpression, string boxIdentity, bool leaveConnection)
		{
			TestColumnSelectExpression(odbcConnectionString, dataMatrixName, columnSelectExpression, boxIdentity, false);
			OdbcConnection conn = Ferda.Modules.Helpers.Data.OdbcConnections.GetConnection(odbcConnectionString, boxIdentity);
			string query =
				"SELECT "
				+ columnSelectExpression + " AS TmpName "
				+ ", COUNT(" + columnSelectExpression + ") AS Frequency "
				+ " FROM " + dataMatrixName + " GROUP BY " + columnSelectExpression + " ORDER BY " + columnSelectExpression;
			OdbcDataAdapter odbcDataAdapter = new OdbcDataAdapter(query, conn);
			DataSet dataSet = new DataSet();
			odbcDataAdapter.Fill(dataSet);
			if (leaveConnection)
				Ferda.Modules.Helpers.Data.OdbcConnections.LeaveConnection(odbcConnectionString);
			return dataSet.Tables[0];
		}
		public static StatisticsStruct ComputeStatistics(string odbcConnectionString, string dataMatrixName, string columnSelectExpression, ValueSubTypeEnum columnSubType, string boxIdentity, bool leaveConnection)
		{
			TestColumnSelectExpression(odbcConnectionString, dataMatrixName, columnSelectExpression, boxIdentity, false);
			OdbcConnection conn = Ferda.Modules.Helpers.Data.OdbcConnections.GetConnection(odbcConnectionString, boxIdentity);
			OdbcCommand odbcCommand = new OdbcCommand();
			odbcCommand.Connection = conn;
			object commandResult;

			StatisticsStruct statisticsStruct = new StatisticsStruct();

			OdbcDataAdapter myDataAdapter = new OdbcDataAdapter("SELECT DISTINCT " + columnSelectExpression + " FROM " + dataMatrixName, conn);
			System.Data.DataSet myDataSet = new System.Data.DataSet();
			myDataAdapter.Fill(myDataSet);
			statisticsStruct.ValueDistincts =
				Convert.ToInt64(myDataSet.Tables[0].Rows.Count);

			odbcCommand.CommandText = "SELECT MAX(" + columnSelectExpression + ") AS Maximum FROM " + dataMatrixName;
			commandResult = odbcCommand.ExecuteScalar();
			statisticsStruct.ValueMax =
				(commandResult != null) ? commandResult.ToString() : null;

			odbcCommand.CommandText = "SELECT MIN(" + columnSelectExpression + ") AS Minimum FROM " + dataMatrixName;
			commandResult = odbcCommand.ExecuteScalar();
			statisticsStruct.ValueMin =
				(commandResult != null) ? commandResult.ToString() : null;

			if (IsColumSubTypeCardinal(ComputeColumnSubType(ComputeColumnType(odbcConnectionString, dataMatrixName, columnSelectExpression, boxIdentity, false))))
			{
				odbcCommand.CommandText = "SELECT AVG(" + columnSelectExpression + ") AS Average FROM " + dataMatrixName;
				commandResult = odbcCommand.ExecuteScalar();
				statisticsStruct.ValueAverage = (commandResult != null) ? commandResult.ToString() : null;

				odbcCommand.CommandText = "SELECT COUNT(1) FROM " + dataMatrixName;
				long dataMatrixRowsCount = Convert.ToInt64(odbcCommand.ExecuteScalar());

				odbcCommand.CommandText =
					"SELECT SUM( "
						+ "(" + columnSelectExpression + " - '" + statisticsStruct.ValueAverage + "')"
						+ "* (" + columnSelectExpression + " - '" + statisticsStruct.ValueAverage + "')"
						+ ") AS Variability FROM " + dataMatrixName;
			
				statisticsStruct.ValueVariability =
					Convert.ToDouble(odbcCommand.ExecuteScalar()) / dataMatrixRowsCount;

				statisticsStruct.ValueStandardDeviation = Math.Sqrt(statisticsStruct.ValueVariability);
			}
			else
			{
				odbcCommand.CommandText = "SELECT AVG(LEN(" + columnSelectExpression + ")) AS AverageLen FROM " + dataMatrixName;
				commandResult = odbcCommand.ExecuteScalar();
				statisticsStruct.ValueAverage = (commandResult != null) ? commandResult.ToString() : null;

				statisticsStruct.ValueVariability = 0;

				statisticsStruct.ValueStandardDeviation = 0;
			}
			if (leaveConnection)
				Ferda.Modules.Helpers.Data.OdbcConnections.LeaveConnection(odbcConnectionString);
			return statisticsStruct;
		}
		public static DataTable ColumnDistinctValues(string odbcConnectionString, string dataMatrixName, string columnSelectExpression, string boxIdentity, bool leaveConnection)
		{
			TestColumnSelectExpression(odbcConnectionString, dataMatrixName, columnSelectExpression, boxIdentity, false);
			OdbcConnection conn = Ferda.Modules.Helpers.Data.OdbcConnections.GetConnection(odbcConnectionString, boxIdentity);
			OdbcDataAdapter myDataAdapter = new OdbcDataAdapter(
				"SELECT DISTINCT " + columnSelectExpression + " AS myColumn FROM " + dataMatrixName,
				conn);
			DataSet myDataSet = new DataSet();
			myDataAdapter.Fill(myDataSet);
			if (leaveConnection)
				Ferda.Modules.Helpers.Data.OdbcConnections.LeaveConnection(odbcConnectionString);
			DataTable result = myDataSet.Tables[0];
			return result;
		}

		public static string[] ColumnDistinctValuesStringSeq(string odbcConnectionString, string dataMatrixName, string columnSelectExpression, string boxIdentity)
		{
			List<string> result = new List<string>();
			DataTable dataTable = Column.ColumnDistinctValues(odbcConnectionString, dataMatrixName, columnSelectExpression, boxIdentity, false);
			foreach (DataRow dataRow in dataTable.Rows)
			{
				string rowValue = dataRow.ItemArray[0].ToString();
				if (String.IsNullOrEmpty(rowValue))
				{
                    result.Add(Ferda.Modules.Helpers.Common.Constants.DbNullCategoryName);
					continue;
				}
				result.Add(rowValue);
			}
			return result.ToArray();
		}
	}
}