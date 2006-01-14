using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Odbc;
using Ferda.Modules.Boxes.DataMiningCommon.DataMatrix;

namespace Ferda.Modules.Helpers.Data
{
	/// <summary>
	/// This class contains static methods for working with data matrixes in databases.
	/// </summary>
	public static class DataMatrix
	{
		/// <summary>
		/// Tests if <c>dataMatrixName</c> is contained in database given by <c>odbcConnectionString</c>.
		/// </summary>
		/// <param name="odbcConnectionString">An ODBC connection string for connecting to database.</param>
		/// <param name="dataMatrixName">An unescaped name of data matrix for the test.</param>
		/// <param name="boxIdentity">An identity of BoxModule.</param>
		/// <exception cref="T:Ferda.Modules.BadParamsError"/>
		public static void TestDataMatrixExists(string odbcConnectionString, string dataMatrixName, string boxIdentity, bool leaveConnection)
		{
			OdbcConnection conn = Ferda.Modules.Helpers.Data.OdbcConnections.GetConnection(odbcConnectionString, boxIdentity);

			try
			{
				//trim dataMatrixName
				if (!String.IsNullOrEmpty(dataMatrixName))
					dataMatrixName = dataMatrixName.Trim();
				dataMatrixName = SqlSecurity.SafeSqlObjectName(dataMatrixName);

				//try query "SELECT * FROM dataMatrixName WHERE 0"
				OdbcCommand command = new OdbcCommand("SELECT * FROM `" + dataMatrixName + "` WHERE 0", conn);
				command.ExecuteNonQuery();
			}
			catch (Exception e)
			{
				//there is not data matrix (dataMatrixName) in given database
                throw Ferda.Modules.Exceptions.BadParamsError(e, boxIdentity, "Cold not found data matrix in database!", Ferda.Modules.restrictionTypeEnum.DbTable);
			}
			finally
			{
				if (leaveConnection)
					Ferda.Modules.Helpers.Data.OdbcConnections.LeaveConnection(odbcConnectionString);
			}
		}

		/// <summary>
		/// Tests if values in <c>primaryKeyColumns</c> are distinct.
		/// </summary>
		/// <param name="odbcConnectionString">An ODBC connection string for connecting to databse.</param>
		/// <param name="dataMatrixName">An unescaped name of data matrix.</param>
		/// <param name="primaryKeyColumns">An array of column unescaped names for test.</param>
		/// <param name="boxIdentity">An identity of BoxModule.</param>
		/// <exception cref="T:Ferda.Modules.Boxes.DataMiningCommon.DataMatrix.ValuesInEnteredPrimaryKeyColumnsAreNotUniqueError"/>
		/// <exception cref="T:Ferda.Modules.Boxes.DataMiningCommon.DataMatrix.CouldNotFoundDataMatrixInDatabaseError"/>
		/// <exception cref="T:Ferda.Modules.BadParamsError"/>
		public static void TestValuesInEnteredPrimaryKeyColumnsAreNotUniqueError(string odbcConnectionString, string dataMatrixName, string[] primaryKeyColumns, string boxIdentity, bool leaveConnection)
		{
			TestDataMatrixExists(odbcConnectionString, dataMatrixName, boxIdentity, false);
			OdbcConnection conn = Ferda.Modules.Helpers.Data.OdbcConnections.GetConnection(odbcConnectionString, boxIdentity);

			bool result = false;
			if (primaryKeyColumns != null && primaryKeyColumns.Length > 0)
			{
				//make a sequence "PkColumn1, PkColumn2, PkColumn3, ..."
				string subQuery = "";
				foreach (string columnName in primaryKeyColumns)
				{
					string columnNameTmp = "`" + SqlSecurity.SafeSqlObjectName(columnName) + "`";
					subQuery = (string.IsNullOrEmpty(subQuery)) ? columnNameTmp : subQuery + ", " + columnNameTmp;
				}

				//create SQL query " ... GROUP BY PkColumns HAVING COUNT(*) > 1"
				dataMatrixName = SqlSecurity.SafeSqlObjectName(dataMatrixName);
				string mySelectQuery = "SELECT " + subQuery + " FROM `" + dataMatrixName + "` GROUP BY " + subQuery + " HAVING COUNT(*) > 1";
				OdbcDataAdapter myDataAdapter = new OdbcDataAdapter(mySelectQuery, conn);
				DataSet myDataSet = new DataSet();

				if (leaveConnection)
					Ferda.Modules.Helpers.Data.OdbcConnections.LeaveConnection(odbcConnectionString);

				//test if SQL query returned any rows
				try
				{
					myDataAdapter.Fill(myDataSet);
					result = !(myDataSet.Tables[0].Rows.Count > 1);
				}
				catch (OdbcException)
				{
					//also if dataMatrix is not in DB
					//also if columns are not in dataMatrix
				}
			}
			if (!result)
			{
                throw Ferda.Modules.Exceptions.BadParamsError(null, boxIdentity, "Values in entered primary key columns are not unique!", Ferda.Modules.restrictionTypeEnum.DbPrimaryKey);
			}
		}
		
		/// <summary>
		/// Compute number of records/rows in data matrix.
		/// </summary>
		/// <param name="odbcConnectionString">An ODBC connection string for connecting DB.</param>
		/// <param name="dataMatrixName">An unescaped name of data matrix.</param>
		/// <param name="boxIdentity">An identity of BoxModule.</param>
		/// <returns>Number of records/rows in data matrix.</returns>
		/// <exception cref="T:Ferda.Modules.Boxes.DataMiningCommon.DataMatrix.CouldNotFoundDataMatrixInDatabaseError"/>
		/// <exception cref="T:Ferda.Modules.BadParamsError"/>
		public static long ComputeRecordsCount(string odbcConnectionString, string dataMatrixName, string boxIdentity, bool leaveConnection)
		{
			TestDataMatrixExists(odbcConnectionString, dataMatrixName, boxIdentity, false);
			OdbcConnection conn = Ferda.Modules.Helpers.Data.OdbcConnections.GetConnection(odbcConnectionString, boxIdentity);

			//execute query "SELECT COUNT(*) FROM dataMatrixName"
			dataMatrixName = SqlSecurity.SafeSqlObjectName(dataMatrixName);
			OdbcCommand command = new OdbcCommand(
					"SELECT COUNT(*) FROM `" + dataMatrixName + "`",
					conn);
			long result = Convert.ToInt64(command.ExecuteScalar());
			if (leaveConnection)
				Ferda.Modules.Helpers.Data.OdbcConnections.LeaveConnection(odbcConnectionString);

			return result;
		}

		/// <summary>
		/// Get information about columns in current data matrix.
		/// </summary>
		/// <param name="odbcConnectionString">An ODBC connection string for connecting DB.</param>
		/// <param name="dataMatrixName">An unescaped name of data matrix.</param>
		/// <param name="boxIdentity">An identity of BoxModule.</param>
		/// <returns>An array of <see cref="T:Ferda.Modules.Boxes.DataMiningCommon.DataMatrix.ColumnInfo"/>.</returns>
		/// <remarks><see cref="T:Ferda.Modules.Boxes.DataMiningCommon.DataMatrix.ColumnInfo"/> is similar to result of <see cref="M:System.Data.Odbc.OdbcDataReader.GetSchemaTable"/>.</remarks>
		/// <exception cref="T:Ferda.Modules.Boxes.DataMiningCommon.DataMatrix.CouldNotFoundDataMatrixInDatabaseError"/>
		/// <exception cref="T:Ferda.Modules.BadParamsError"/>
		public static ColumnInfo[] ExplainDataMatrixStructure(string odbcConnectionString, string dataMatrixName, string boxIdentity, bool leaveConnection)
		{
			TestDataMatrixExists(odbcConnectionString, dataMatrixName, boxIdentity, false);
			OdbcConnection conn = Ferda.Modules.Helpers.Data.OdbcConnections.GetConnection(odbcConnectionString, boxIdentity);

			List<ColumnInfo> result = new List<ColumnInfo>();
			ColumnInfo columnInfo;

			//create SQL (empty ... WHERE 0) query over dataMatrixName (this select given data matrix)
			dataMatrixName = SqlSecurity.SafeSqlObjectName(dataMatrixName);
			OdbcCommand odbcCommand = new OdbcCommand("SELECT * FROM `" + dataMatrixName + "` WHERE 0", conn);

			//see documentation for System.Data.Odbc.OdbcDataReader.GetSchemaTable()
			foreach (DataRow row in odbcCommand.ExecuteReader().GetSchemaTable().Rows)
			{
				columnInfo = new ColumnInfo();
				columnInfo.name = row["ColumnName"].ToString();
				columnInfo.columnOrdinal = Convert.ToInt32(row["ColumnOrdinal"]);
				columnInfo.columnSize = Convert.ToInt32(row["ColumnSize"]);
				columnInfo.numericPrecision = Convert.ToInt32(row["NumericPrecision"]);
				columnInfo.numericScale = Convert.ToInt32(row["NumericScale"]);
				columnInfo.providerType = Convert.ToInt32(row["ProviderType"]);
				columnInfo.dataType = row["DataType"].ToString();
				columnInfo.isLong = Convert.ToBoolean(row["IsLong"]);
				columnInfo.allowDBNull = Convert.ToBoolean(row["AllowDBNull"]);
				columnInfo.isReadOnly = Convert.ToBoolean(row["IsReadOnly"]);
				columnInfo.isRowVersion = Convert.ToBoolean(row["IsRowVersion"]);
				columnInfo.isUnique = Convert.ToBoolean(row["IsUnique"]);
				columnInfo.isKey = Convert.ToBoolean(row["IsKey"]);
				columnInfo.isAutoIncrement = Convert.ToBoolean(row["IsAutoIncrement"]);
				result.Add(columnInfo);
			}

			if (leaveConnection)
				Ferda.Modules.Helpers.Data.OdbcConnections.LeaveConnection(odbcConnectionString);
			return result.ToArray();
		}

		/// <summary>
		/// Get names of columns in data matrix.
		/// </summary>
		/// <param name="odbcConnectionString">An ODBC connection string for connecting DB.</param>
		/// <param name="dataMatrixName">An unescaped name of data matrix.</param>
		/// <param name="boxIdentity">An identity of BoxModule.</param>
		/// <returns>An array of names of columns in data matrix.</returns>
		/// <exception cref="T:Ferda.Modules.Boxes.DataMiningCommon.DataMatrix.CouldNotFoundDataMatrixInDatabaseError"/>
		/// <exception cref="T:Ferda.Modules.BadParamsError"/>
		public static string[] ColumnsNames(string odbcConnectionString, string dataMatrixName, string boxIdentity, bool leaveConnection)
		{
			TestDataMatrixExists(odbcConnectionString, dataMatrixName, boxIdentity, false);
			OdbcConnection conn = Ferda.Modules.Helpers.Data.OdbcConnections.GetConnection(odbcConnectionString, boxIdentity);

			List<string> result = new List<string>();

			//create SQL (empty ... WHERE 0) query over dataMatrixName
			dataMatrixName = SqlSecurity.SafeSqlObjectName(dataMatrixName);
			OdbcCommand odbcCommand = new OdbcCommand("SELECT * FROM `" + dataMatrixName + "` WHERE 0", conn);

			//see documentation of System.Data.Odbc.OdbcDataReader.GetSchemaTable()
			foreach (DataRow row in odbcCommand.ExecuteReader().GetSchemaTable().Rows)
			{
				result.Add(row["ColumnName"].ToString());
			}

			if (leaveConnection)
				Ferda.Modules.Helpers.Data.OdbcConnections.LeaveConnection(odbcConnectionString);
			return result.ToArray();
		}
	}
}