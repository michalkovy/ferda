using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Odbc;
using Ferda.Modules.Boxes.DataMiningCommon.Database;

namespace Ferda.Modules.Helpers.Data
{
	/// <summary>
	/// This class contains static methods for working with databases.
	/// </summary>
	public static class Database
	{
		/// <summary>
		/// Tests if it is possible to create <see cref="T:System.Data.Odbc.OdbcConnection"/> with <c>odbcConnectionString</c>.
		/// </summary>
		/// <param name="odbcConnectionString">An ODBC connection string for the test.</param>
		/// <param name="boxIdentity">An identity of BoxModule.</param>
		/// <exception cref="T:Ferda.Modules.BadParamsError"/>
		public static void TestConnectionString(string odbcConnectionString, string boxIdentity, bool leaveConnection)
		{
			//throws Ferda.Modules.Boxes.DataMiningCommon.Database.BadConnectionStringError
			Ferda.Modules.Helpers.Data.OdbcConnections.GetConnection(odbcConnectionString, boxIdentity);
			if (leaveConnection)
				Ferda.Modules.Helpers.Data.OdbcConnections.LeaveConnection(odbcConnectionString);
		}

		/// <summary>
		/// Gets names of publishable (<see cref="M:Ferda.Modules.Helpers.Data.Database.IsTableTypePublishable(System.String)"/>) tables in database given by <c>odbcConnectionString</c>.
		/// </summary>
		/// <param name="odbcConnectionString">An ODBC connection string for test.</param>
		/// <param name="boxIdentity">An identity of BoxModule.</param>
		/// <returns>Array of names of tables in current DB.</returns>
		/// <exception cref="T:Ferda.Modules.BadParamsError"/>
		public static string[] GetTables(string odbcConnectionString, string boxIdentity, bool leaveConnection)
		{
			//get connection
			OdbcConnection conn = Ferda.Modules.Helpers.Data.OdbcConnections.GetConnection(odbcConnectionString, boxIdentity);

			//get schema
			DataTable dataTable = conn.GetSchema("TABLES");
			if (leaveConnection)
				Ferda.Modules.Helpers.Data.OdbcConnections.LeaveConnection(odbcConnectionString);

			//result variable
			List<string> dataMatrixNames = new List<string>();

			foreach (DataRow row in dataTable.Rows)
			{
				//only publishable tables and views are added to result
				if (IsTableTypePublishable(row["TABLE_TYPE"].ToString()))
					dataMatrixNames.Add(row["TABLE_NAME"].ToString());
			}
			return dataMatrixNames.ToArray();
		}

		/// <summary>
		/// Tests if table type is publishable i.e. the table (or view) is not system or temporary.
		/// </summary>
		/// <param name="tableType">A table type.</param>
		/// <returns>False iff table (or view) is of system or temporary type.</returns>
		/// <remarks>
		/// There are many types of tables: ALIAS, TABLE, SYNONYM, SYSTEM TABLE, VIEW, 
		/// GLOBAL TEMPORARY, LOCAL TEMPORARY, EXTERNAL TABLE, or SYSTEM VIEW
		/// but we don`t want system and temporary tables or views.
		/// </remarks>
		public static bool IsTableTypePublishable(string tableType)
		{
			//temporary and system tables (or views) are not publishable
			if (tableType == "SYSTEM TABLE"
				|| tableType == "SYSTEM VIEW"
				|| tableType == "GLOBAL TEMPORARY"
				|| tableType == "LOCAL TEMPORARY")
				return false;
			return true;
		}

		/// <summary>
		/// Gets information about publishable (<see cref="M:Ferda.Modules.Helpers.Data.Database.IsTableTypePublishable(System.String)"/>) tables in database given by <c>odbcConnectionString</c>.
		/// </summary>
		/// <param name="odbcConnectionString">An ODBC connection string for test.</param>
		/// <param name="boxIdentity">An identity of BoxModule.</param>
		/// <returns>Array of <see cref="T:Ferda.Modules.Boxes.DataMiningCommon.Database.DataMatrixInfo"/>.</returns>
		/// <exception cref="T:Ferda.Modules.BadParamsError"/>
		public static DataMatrixInfo[] ExplainDatabaseStructure(string odbcConnectionString, string boxIdentity, bool leaveConnection)
		{
			//get connection
			OdbcConnection conn = Ferda.Modules.Helpers.Data.OdbcConnections.GetConnection(odbcConnectionString, boxIdentity);

			//get schema
			DataTable schema = conn.GetSchema("TABLES");

			//prepare OdbcCommand for "SELECT COUNT(*) FROM ..." query
			OdbcCommand odbcCommand = new OdbcCommand();
			odbcCommand.Connection = conn;

			//result variable
			List<DataMatrixInfo> result = new List<DataMatrixInfo>();

			foreach (DataRow row in schema.Rows)
			{
				//only publishable tables or views are added to result
				if (IsTableTypePublishable(row["TABLE_TYPE"].ToString()))
				{
					DataMatrixInfo dataMatrixInfo = new DataMatrixInfo();
					dataMatrixInfo.name = row["TABLE_NAME"].ToString();
					dataMatrixInfo.type = row["TABLE_TYPE"].ToString();
					dataMatrixInfo.remarks = row["REMARKS"].ToString();

					//complete OdbcCommand and execute
					odbcCommand.CommandText = "SELECT COUNT(*) FROM " + dataMatrixInfo.name;
					dataMatrixInfo.rowCount = Convert.ToInt32(odbcCommand.ExecuteScalar());

					result.Add(dataMatrixInfo);
				}
			}
			if (leaveConnection)
				Ferda.Modules.Helpers.Data.OdbcConnections.LeaveConnection(odbcConnectionString);
			return result.ToArray();
		}
	}
}