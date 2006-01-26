using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using Ferda.Modules.Boxes.DataMiningCommon.DataMatrix;

namespace Ferda.Modules.Boxes.DataMiningCommon.Column
{
	class ColumnFunctionsI : ColumnFunctionsDisp_, IFunctions
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
		protected string columnSelectExpression
		{
			get
			{
				return this.boxModule.GetPropertyString("Name");
			}
		}

		#region Cache: StatisticsStruct
		private class statisticsStructCache : Ferda.Modules.Helpers.Caching.Cache
		{
			private StatisticsStruct value;
			public StatisticsStruct Value(string boxIdentity, DateTimeT lastReloadTime, string connectionString, string dataMatrixName, long dataMatrixRecordsCount, string columnSelectExpression, ValueSubTypeEnum columnValueSubType)
			{
                lock (this)
                {
                    Dictionary<string, IComparable> cacheSetting = new Dictionary<string, IComparable>();
                    cacheSetting.Add("ConnectionString", connectionString);
                    cacheSetting.Add("DataMatrixName", dataMatrixName);
                    cacheSetting.Add("DataMatrixRecordsCount", dataMatrixRecordsCount);
                    cacheSetting.Add("ColumnSelectExpression", columnSelectExpression);
                    if (IsObsolete(lastReloadTime, cacheSetting))
                    {
                        value = Ferda.Modules.Helpers.Data.Column.GetStatistics(connectionString, dataMatrixName, columnSelectExpression, columnValueSubType, boxIdentity);
                    }
                    return value;
                }
			}
		}
		private statisticsStructCache statisticsStructCached = new statisticsStructCache();
		#endregion
		protected StatisticsStruct getStatisticsStruct(DataMatrixStruct dataMatrixStruct)
		{
            ValueSubTypeEnum columnValueSubType = valueSubTypeCached.Value(boxModule.StringIceIdentity, dataMatrixStruct.database.lastReloadInfo, dataMatrixStruct.database.connectionString, dataMatrixStruct.dataMatrixName, columnSelectExpression, true);
            return statisticsStructCached.Value(boxModule.StringIceIdentity, dataMatrixStruct.database.lastReloadInfo, dataMatrixStruct.database.connectionString, dataMatrixStruct.dataMatrixName, dataMatrixStruct.recordsCount, columnSelectExpression, columnValueSubType);
		}
		protected StatisticsStruct getStatisticsStruct()
		{
			DataMatrixStruct dataMatrixStruct = getDataMatrixFunctionsPrx().getDataMatrix();
			return getStatisticsStruct(dataMatrixStruct);
		}
		#endregion

		#region Cache: Column Data Type (ValueSubTypeEnum)
		private class valueSubTypeCache : Ferda.Modules.Helpers.Caching.Cache
		{
			private ValueSubTypeEnum value;
			public ValueSubTypeEnum Value(string boxIdentity, DateTimeT lastReloadTime, string connectionString, string dataMatrixName, string columnSelectExpression, bool leaveConnection)
			{
                lock (this)
                {
                    Dictionary<string, IComparable> cacheSetting = new Dictionary<string, IComparable>();
                    cacheSetting.Add("ConnectionString", connectionString);
                    cacheSetting.Add("DataMatrixName", dataMatrixName);
                    cacheSetting.Add("ColumnSelectExpression", columnSelectExpression);
                    if (IsObsolete(lastReloadTime, cacheSetting))
                        value =
                            Ferda.Modules.Helpers.Data.Column.GetColumnSubTypeByDataType(
                                Ferda.Modules.Helpers.Data.Column.GetDataType(connectionString, dataMatrixName, columnSelectExpression, boxIdentity));
                    return value;
                }
			}
		}
		private valueSubTypeCache valueSubTypeCached = new valueSubTypeCache();
		#endregion
		protected ValueSubTypeEnum getColumnValueSubType(DataMatrixStruct dataMatrixStruct)
		{
            return valueSubTypeCached.Value(boxModule.StringIceIdentity, dataMatrixStruct.database.lastReloadInfo, dataMatrixStruct.database.connectionString, dataMatrixStruct.dataMatrixName, columnSelectExpression, true);
		}
		protected ValueSubTypeEnum getColumnValueSubType()
		{
			DataMatrixStruct dataMatrixStruct = getDataMatrixFunctionsPrx().getDataMatrix();
			return getColumnValueSubType(dataMatrixStruct);
		}

		#region Functions
		public override ColumnStruct getColumn(Ice.Current __current)
		{
			DataMatrixStruct dataMatrixStruct = this.getDataMatrixFunctionsPrx().getDataMatrix();
			string connectionString = dataMatrixStruct.database.connectionString;
			string dataMatrixName = dataMatrixStruct.dataMatrixName;
			string columnSelectExpression = this.columnSelectExpression;
			Ferda.Modules.Helpers.Data.Column.TestColumnSelectExpression(
				connectionString,
				dataMatrixName,
				columnSelectExpression,
                boxModule.StringIceIdentity);
			ColumnStruct result = new ColumnStruct();
			result.statistics = getStatisticsStruct(dataMatrixStruct);
			result.dataMatrix = dataMatrixStruct;
			result.columnSelectExpression = columnSelectExpression;
			result.columnSubType = getColumnValueSubType(dataMatrixStruct);
			result.columnType = ColumnTypeEnum.Ordinary;
			return result;
		}

		public override string[] getDistinctValues(Ice.Current __current)
		{
			DataMatrixStruct dataMatrixStruct = getDataMatrixFunctionsPrx().getDataMatrix();
            return Helpers.Data.Column.GetDistinctsStringSeq(dataMatrixStruct.database.connectionString, dataMatrixStruct.dataMatrixName, columnSelectExpression, boxModule.StringIceIdentity);
		}
		#endregion

		#region Sockets
		protected DataMatrixFunctionsPrx getDataMatrixFunctionsPrx()
		{
			return DataMatrixFunctionsPrxHelper.checkedCast(
				SocketConnections.GetObjectPrx(boxModule, "DataMatrixOrMultiColumn")
				);
		}
		#endregion

		#region Actions
		#endregion

		#region BoxInfo
		public string[] GetColumnsNames()
		{
			try
			{
				return this.getDataMatrixFunctionsPrx().getColumns();
			}
            catch (Ferda.Modules.BoxRuntimeError) { }
			return new string[0];
		}
		public ValueSubTypeEnum GetColumnSubType()
		{
			try
			{
				return getColumnValueSubType();
			}
            catch (Ferda.Modules.BoxRuntimeError) { }
			return ValueSubTypeEnum.Unknown;
		}

		public StatisticsStruct GetStatistics()
		{
			try
			{
				return getStatisticsStruct();
			}
            catch (Ferda.Modules.BoxRuntimeError) { }
			return new StatisticsStruct();
		}
		#endregion
	}
}
