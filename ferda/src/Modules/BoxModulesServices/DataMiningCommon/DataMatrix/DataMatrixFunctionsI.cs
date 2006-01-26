using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using Ferda.Modules.Boxes.DataMiningCommon.Database;

namespace Ferda.Modules.Boxes.DataMiningCommon.DataMatrix
{
	class DataMatrixFunctionsI : DataMatrixFunctionsDisp_, IFunctions
	{
		protected BoxModuleI boxModule;
		//protected IBoxInfo boxInfo;

		#region IFunctions Members
		public void setBoxModuleInfo(BoxModuleI boxModule, IBoxInfo boxInfo)
		{
			this.boxModule = boxModule;
			//this.boxInfo = boxInfo;
		}
		#endregion

		#region Properties
		protected string dataMatrixName
		{
			get
			{
				return this.boxModule.GetPropertyString("Name");
			}
		}

		protected string[] primaryKeyColumns
		{
			get
			{
                return Ferda.Modules.Helpers.Common.Csv.Csv2Strings(
                    this.boxModule.GetPropertyString("PrimaryKeyColumns"));
			}
		}

		#region Cache: RecordsCount
		private class recordsCountCache : Ferda.Modules.Helpers.Caching.Cache
		{
			private long value;
			public long Value(string boxIdentity, DateTimeT lastReloadTime, string connectionString, string dataMatrixName)
			{
                lock (this)
                {
                    Dictionary<string, IComparable> cacheSetting = new Dictionary<string, IComparable>();
                    cacheSetting.Add("ConnectionString", connectionString);
                    cacheSetting.Add("DataMatrixName", dataMatrixName);
                    if (IsObsolete(lastReloadTime, cacheSetting))
                        value = Ferda.Modules.Helpers.Data.DataMatrix.GetRecordsCount(connectionString, dataMatrixName, boxIdentity);
                    return value;
                }
			}
		}
		private recordsCountCache recordsCountCached = new recordsCountCache();
		#endregion
		private long getRecordsCount()
		{
			DatabaseStruct databaseStruct = getDatabaseFunctionsPrx().getDatabase();
			return recordsCountCached.Value(
                boxModule.StringIceIdentity,
				databaseStruct.lastReloadInfo,
				databaseStruct.connectionString, dataMatrixName);
		}
		#endregion

		#region Functions
		public override DataMatrixStruct getDataMatrix(Ice.Current __current)
		{
			DatabaseStruct databaseStruct = this.getDatabaseFunctionsPrx().getDatabase();
			string myIdentity = Ice.Util.identityToString(this.boxModule.IceIdentity);
			Ferda.Modules.Helpers.Data.DataMatrix.TestDataMatrixExists(databaseStruct.connectionString, dataMatrixName, myIdentity);
			DataMatrixStruct result = new DataMatrixStruct();
			result.database = databaseStruct;
			result.dataMatrixName = dataMatrixName;
			result.recordsCount = this.getRecordsCount();
			result.primaryKeyColumns = this.primaryKeyColumns;
			result.explainDataMatrix = this.explainDataMatrixStructure();

			return result;
		}

		#region Cache: ColumnsNames
		private class columnsNamesCache : Ferda.Modules.Helpers.Caching.Cache
		{
			private string[] value;
			public string[] Value(string boxIdentity, DateTimeT lastReloadTime, string connectionString, string dataMatrixName)
			{
                lock (this)
                {
                    Dictionary<string, IComparable> cacheSetting = new Dictionary<string, IComparable>();
                    cacheSetting.Add("ConnectionString", connectionString);
                    cacheSetting.Add("DataMatrixName", dataMatrixName);
                    if (IsObsolete(lastReloadTime, cacheSetting))
                        value = Ferda.Modules.Helpers.Data.DataMatrix.GetColumns(connectionString, dataMatrixName, boxIdentity);
                    return value;
                }
			}
		}
		private columnsNamesCache columnsNamesCached = new columnsNamesCache();
		#endregion
		public override string[] getColumns(Ice.Current __current)
		{
			DatabaseStruct databaseStruct = getDatabaseFunctionsPrx().getDatabase();
			return columnsNamesCached.Value(
                boxModule.StringIceIdentity,
				databaseStruct.lastReloadInfo,
				databaseStruct.connectionString, dataMatrixName);
		}

		public override ColumnInfo[] explainDataMatrixStructure(Ice.Current __current)
		{
            return Ferda.Modules.Helpers.Data.DataMatrix.Explain(connectionString, dataMatrixName, boxModule.StringIceIdentity);
		}

		#endregion

		#region Sockets
		private DatabaseFunctionsPrx getDatabaseFunctionsPrx()
		{
			return DatabaseFunctionsPrxHelper.checkedCast(
				SocketConnections.GetObjectPrx(boxModule, "Database")
				);
		}
		#endregion

		#region Actions
		public bool RunActionTestPrimaryKeyColumns()
		{
			try
			{
                Ferda.Modules.Helpers.Data.DataMatrix.TestValuesInEnteredPrimaryKeyColumnsAreNotUniqueError(connectionString, dataMatrixName, primaryKeyColumns, boxModule.StringIceIdentity);
				return true;
			}
            catch (Ferda.Modules.BoxRuntimeError) { }
			return false;
		}
		#endregion

		#region BoxInfo
		public string[] GetTablesNames()
		{
			try
			{
				return getDatabaseFunctionsPrx().getTables();
			}
            catch (Ferda.Modules.BoxRuntimeError) { }
			return new string[0];
		}

		public long GetRecordsCount()
		{
			try
			{
				return getRecordsCount();
			}
            catch (Ferda.Modules.BoxRuntimeError) { }
			return 0;
		}
		public string[] GetColumns()
		{
			try
			{
				return getColumns();
			}
            catch (Ferda.Modules.BoxRuntimeError) { }
			return new string[0];
		}
		#endregion

		private string connectionString
		{
			get
			{
				return getDatabaseFunctionsPrx().getDatabase().connectionString;
			}
		}
	}
}
