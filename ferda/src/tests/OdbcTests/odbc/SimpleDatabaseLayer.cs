#define TRACE

using System;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;

namespace odbc
{
	/// <summary>
	/// Summary description for ODBC.
	/// </summary>
	public class SimpleDatabaseLayer
	{
		private OdbcConnection Connection;
		private string connectionString;

		public SimpleDatabaseLayer(string ConnectionString)
		{
			this.connectionString = ConnectionString;
			this.GetConnection(ConnectionString);
		}
		~SimpleDatabaseLayer()
		{
			try
			{
				this.Connection.Dispose();
			}
			catch {}
		}

		public void MyOdbcCommand(string tableName, string columnName, string value)
		{
			string query = "INSERT INTO " + tableName+ " SET " + columnName + "='" + value + "'";
			OdbcCommand myCommand = new OdbcCommand(query, this.Connection);
			//System.Windows.Forms.MessageBox.Show(query);
			myCommand.ExecuteNonQuery();
		}

		public void GetConnection(string ConnectionString)
		{
			//string sConnString = "Driver={SQL Server};Server=(local);Database=Northwind;Uid=sa;Pwd=;";
			try
			{
				this.Connection = new OdbcConnection(ConnectionString);
				this.Connection.Open();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		public int QueryRowCount(string TableName)
		{
			string myScalarQuery = "SELECT COUNT(*) FROM " + TableName;
			return Convert.ToInt32(this.ExecuteScalar(myScalarQuery));
		}

		struct TableInfo
		{
			public string Name;
			public string Type;
			public string Remarks;
			public int RowCount;
		}

		public void TablesModuleForInteraction()
		{
			List<TableInfo> result = new List<TableInfo>();
			TableInfo tableInfo;
			OdbcConnection conn = new OdbcConnection(connectionString);
			conn.Open();
			OdbcCommand odbcCommand = new OdbcCommand();
			odbcCommand.Connection = conn;
			DataTable schemaTable = conn.GetSchema("TABLES");
			foreach (DataRow myRow in schemaTable.Rows)
			{
				if (myRow["TABLE_TYPE"].ToString() == "SYSTEM TABLE")
					continue;
				tableInfo = new TableInfo();
				tableInfo.Name = myRow["TABLE_NAME"].ToString();
				tableInfo.Type = myRow["TABLE_TYPE"].ToString();
				tableInfo.Remarks = myRow["REMARKS"].ToString();
				//try
				{
					odbcCommand.CommandText = "SELECT COUNT(*) FROM " + tableInfo.Name;
					tableInfo.RowCount = Convert.ToInt32(odbcCommand.ExecuteScalar());
				}
				//catch { }
				result.Add(tableInfo);
			}
			conn.Close();
		}
		struct ColumnInfo
		{
			public string Name;
			public int ColumnOrdinal;
			public int ColumnSize;
			public int NumericPrecision;
			public int NumericScale;
			public string DataType;
			public int ProviderType; 
			public bool IsLong;
			public bool AllowDBNull;
			public bool IsReadOnly;
			public bool IsRowVersion;
			public bool IsUnique;
			public bool IsKey;
			public bool IsAutoIncrement;
		}
		public void ColumnsModuleForInteraction(string tableName)
		{
			List<ColumnInfo> result = new List<ColumnInfo>();
			ColumnInfo columnInfo;
			OdbcConnection conn = new OdbcConnection(connectionString);
			conn.Open();
			OdbcCommand odbcCommand = new OdbcCommand();
			odbcCommand.Connection = conn;
			odbcCommand.CommandText = "SELECT * FROM " + tableName + " WHERE 0";
			foreach (DataRow myRow in odbcCommand.ExecuteReader().GetSchemaTable().Rows)
			{
				columnInfo = new ColumnInfo();
				columnInfo.Name = myRow["ColumnName"].ToString();
				columnInfo.ColumnOrdinal = Convert.ToInt32(myRow["ColumnOrdinal"]);
				columnInfo.ColumnSize = Convert.ToInt32(myRow["ColumnSize"]);
				columnInfo.NumericPrecision = Convert.ToInt32(myRow["NumericPrecision"]);
				columnInfo.NumericScale = Convert.ToInt32(myRow["NumericScale"]);
				columnInfo.ProviderType = Convert.ToInt32(myRow["ProviderType"]);
				columnInfo.DataType = myRow["DataType"].ToString();
				columnInfo.IsLong = Convert.ToBoolean(myRow["IsLong"]);
				columnInfo.AllowDBNull = Convert.ToBoolean(myRow["AllowDBNull"]);
				columnInfo.IsReadOnly = Convert.ToBoolean(myRow["IsReadOnly"]);
				columnInfo.IsRowVersion = Convert.ToBoolean(myRow["IsRowVersion"]);
				columnInfo.IsUnique = Convert.ToBoolean(myRow["IsUnique"]);
				columnInfo.IsKey = Convert.ToBoolean(myRow["IsKey"]);
				columnInfo.IsAutoIncrement = Convert.ToBoolean(myRow["IsAutoIncrement"]);
				result.Add(columnInfo);
			}
			conn.Close();
		}

		public DataTable SelectAllModuleForInteraction(string tableName)
		{
			OdbcConnection conn = new OdbcConnection(connectionString);
			conn.Open();
			OdbcDataAdapter odbcDataAdapter = new OdbcDataAdapter("SELECT * FROM " + tableName, conn);
			DataSet dataSet = new DataSet();
			odbcDataAdapter.Fill(dataSet);
			return dataSet.Tables[0];
		}

		public DataTable ColumnFrequenciesModuleForInteraction(string tableName, string columnName)
		{
			OdbcConnection conn = new OdbcConnection(connectionString);
			conn.Open();
			string query =
				"SELECT "
				+ columnName + " AS TmpName "
				+ ", COUNT(" + columnName + ") AS Frequency "
				+ " FROM " + tableName + " GROUP BY " + columnName + " ORDER BY " + columnName;

			OdbcDataAdapter odbcDataAdapter = new OdbcDataAdapter(query, conn);
			DataSet dataSet = new DataSet();
			odbcDataAdapter.Fill(dataSet);
			return dataSet.Tables[0];
		}
/*
		public DataTable ColumnFrequenciesModuleForInteraction(string tableName, string columnName)
		{
			OdbcConnection conn = new OdbcConnection(connectionString);
			conn.Open();
			OdbcCommand odbcCommand = new OdbcCommand();
			odbcCommand.Connection = conn;
			odbcCommand.CommandText = "SELECT COUNT(*) FROM " + tableName;
			int rowCount = Convert.ToInt32(odbcCommand.ExecuteScalar());

			string query = 
				"SELECT " + columnName
				+ ", COUNT(" + columnName + ") AS Frequency "
				+ ", (Frequency / " + rowCount + ") AS RelativeFrequency "
				+ " FROM " + tableName + " GROUP BY " + columnName + " ORDER BY " + columnName;

			OdbcDataAdapter odbcDataAdapter = new OdbcDataAdapter(query, conn);
			DataSet dataSet = new DataSet();
			odbcDataAdapter.Fill(dataSet);
			return dataSet.Tables[0];
		}
*/
		public System.Data.DataTable QuerySystables()
		{
			List<string> tableNames = new List<string>();
			string myConnString = connectionString;
			//System.Windows.Forms.MessageBox.Show(myConnString);
			string message = "";
			OdbcConnection conn = new OdbcConnection(myConnString);
			conn.Open();
			DataTable schemaTable = conn.GetSchema("TABLES");
			foreach (DataRow myRow in schemaTable.Rows)
			{
				if (myRow["TABLE_TYPE"].ToString() == "TABLE")
				{
					tableNames.Add(myRow["TABLE_NAME"].ToString());
					message += "\n " + myRow["TABLE_NAME"];
				}
			}
			//System.Windows.Forms.MessageBox.Show(message);
			conn.Close();
			return schemaTable;
		}
		public System.Data.DataTable QuerySyscolumns(string tableName)
		{
			List<string> columnNames = new List<string>();
			string myConnString = connectionString;
			string mySelectQuery = "SELECT * FROM " + tableName + " WHERE 0";
			OdbcConnection conn = new OdbcConnection(myConnString);
			OdbcCommand myCommand = new OdbcCommand(mySelectQuery, conn);
			conn.Open();
			OdbcDataReader myReader = myCommand.ExecuteReader();
			DataTable schemaTable = myReader.GetSchemaTable();
			string message = null;
			foreach (DataRow myRow in schemaTable.Rows)
			{
				columnNames.Add(myRow["ColumnName"].ToString());
				message += "\n " + myRow["ColumnName"];
			}
			conn.Close();
			//System.Windows.Forms.MessageBox.Show(message);
			return schemaTable;
		}
		public string GetColumnSubType(string tableName, string columnName)
		{
			DataTable schemaTable = this.QuerySyscolumns(tableName);
			foreach (DataRow myRow in schemaTable.Rows)
			{
				if (myRow["ColumnName"].ToString() == columnName)
				{
					return myRow["DataType"].ToString();
				}
			}
			return null;
		}
		public bool TestPrimaryKeysUnique(string dataMatrixName, string[] primaryKeyColumns)
		{
			bool result = false;
			if (primaryKeyColumns != null && primaryKeyColumns.Length > 0)
			{
				string subQuery = "";
				foreach (string columnName in primaryKeyColumns)
				{
					subQuery = (string.IsNullOrEmpty(subQuery)) ? columnName : subQuery + ", " + columnName;
				}
				string mySelectQuery = "SELECT " + subQuery + " FROM " + dataMatrixName + " GROUP BY " + subQuery + " HAVING COUNT(*) > 1";
				System.Data.Odbc.OdbcConnection conn = new System.Data.Odbc.OdbcConnection("DSN=LM Barbora.mdb");
				conn.Open();
				OdbcDataAdapter myDataAdapter = new OdbcDataAdapter(mySelectQuery, conn);
				System.Data.DataSet myDataSet = new System.Data.DataSet();
				myDataAdapter.Fill(myDataSet);
				result = !(myDataSet.Tables[0].Rows.Count > 1);

				//result = !(this.ExecuteTable(mySelectQuery).Rows.Count > 1);
				System.Windows.Forms.MessageBox.Show("TestPrimaryKeysUnique(" + dataMatrixName + ", [" + subQuery + "]) " + result.ToString());
			}
			return result;
		}
		public object QuerySelectMin(string TableName, string ColumnName)
		{
			string myScalarQuery = "SELECT MIN(" + ColumnName + ") AS Minimum FROM " + TableName;
			//string myScalarQuery = "SELECT MIN(LEN(" + ColumnName + ")) AS MinLen FROM " + dataMatrixName;
			return this.ExecuteScalar(myScalarQuery);
		}
		public object QuerySelectMax(string TableName, string ColumnName)
		{
			string myScalarQuery = "SELECT MAX(" + ColumnName + ") AS Maximum FROM " + TableName;
			//string myScalarQuery = "SELECT MAX(LEN(" + ColumnName + ")) AS MaxLen FROM " + dataMatrixName;
			return this.ExecuteScalar(myScalarQuery);
		}
		public double QuerySelectAvg(string TableName, string ColumnName)
		{
			string myScalarQuery;
			if (this.GetColumnSubType(TableName, ColumnName) == "System.String")
				myScalarQuery = "SELECT AVG(LEN(" + ColumnName + ")) AS AverageLen FROM " + TableName;
			else
				myScalarQuery = "SELECT AVG(" + ColumnName + ") AS Average FROM " + TableName;
			return Convert.ToDouble(this.ExecuteScalar(myScalarQuery));
		}
		//TODO: je spravne?, otestovat
		public double QuerySelectVariability(string TableName, string ColumnName)
		{
			string average = Convert.ToString(Convert.ToDouble(this.QuerySelectAvg(TableName, ColumnName)));
			string myScalarQuery = "SELECT SUM((" + ColumnName + " - '" + average + "') * (" + ColumnName + " - '" + average + "')) AS Variability FROM " + TableName;
			//System.Windows.Forms.MessageBox.Show(myScalarQuery);
			double preVar = Convert.ToDouble(this.ExecuteScalar(myScalarQuery));
			//System.Windows.Forms.MessageBox.Show(preVar.ToString());
			double result = preVar / Convert.ToDouble(this.QueryRowCount(TableName));
			//System.Windows.Forms.MessageBox.Show(result.ToString());
			return result;
		}

		public double QuerySelectStandardDeviation(string TableName, string ColumnName)
		{
			return Math.Sqrt(this.QuerySelectVariability(TableName, ColumnName));
		}
		//TODO: je spravne?, otestovat
		public int QuerySelectDistincts(string TableName, string ColumnName)
		{
			string myScalarQuery = "SELECT DISTINCT " + ColumnName + " FROM " + TableName;
			System.Data.DataTable myTable = this.ExecuteTable(myScalarQuery);
			return myTable.Rows.Count;
		}
		//TODO: je spravne?, otestovat
		public bool CheckFormula(string TableName, string Formula, out System.Exception myException)
		{
			bool result = true;
			myException = null;
			string myTableQuery = "SELECT " + Formula + " FROM "
				+ TableName + " LIMIT 1";
			try
			{
				this.ExecuteTable(myTableQuery);
			}
			catch (Exception ex)
			{
				myException = ex;
				result = false;
			}
			return result;
		}
		private object ExecuteScalar(string ScalarQuery)
		{
			OdbcCommand myCommand = new OdbcCommand(ScalarQuery, this.Connection);
			return myCommand.ExecuteScalar();
		}
		private System.Data.DataTable ExecuteTable(string TableQuery)
		{
			OdbcDataAdapter myDataAdapter = new OdbcDataAdapter(TableQuery, this.Connection);
			System.Data.DataSet myDataSet = new System.Data.DataSet();
			myDataAdapter.Fill(myDataSet);
			return myDataSet.Tables[0];
		}
		private void Log(string logItem)
		{
			;
		}
	}
}
