using System;
using System.Collections.Generic;
using System.Data.Odbc;
using Ferda.Modules.Boxes.DataMiningCommon.DataMatrix;
using Ferda.Modules.Boxes.DataMiningCommon.Column;

namespace Ferda.Modules.Boxes.DataMiningCommon.DerivedColumn
{
	class DerivedColumnFunctionsI : DerivedColumnFunctionsDisp_, IFunctions
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
				return this.boxModule.GetPropertyString("Formula");
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
                    cacheSetting.Add(Database.DatabaseBoxInfo.typeIdentifier + Database.DatabaseBoxInfo.OdbcConnectionStringPropertyName, connectionString);
                    cacheSetting.Add(DataMatrix.DataMatrixBoxInfo.typeIdentifier + DataMatrix.DataMatrixBoxInfo.DataMatrixNamePropertyName, dataMatrixName);
                    cacheSetting.Add(DataMatrix.DataMatrixBoxInfo.typeIdentifier + DataMatrix.DataMatrixBoxInfo.RecordCountPropertyName, dataMatrixRecordsCount);
                    cacheSetting.Add(Column.ColumnBoxInfo.typeIdentifier + Column.ColumnBoxInfo.ColumnSelectExpressionPropertyName, columnSelectExpression);
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
            return statisticsStructCached.Value(boxModule.StringIceIdentity, dataMatrixStruct.database.lastReloadInfo, dataMatrixStruct.database.connectionString, dataMatrixStruct.dataMatrixName, dataMatrixStruct.recordsCount, columnSelectExpression, columnValueSubType);
		}
		protected StatisticsStruct getStatisticsStruct()
		{
			DataMatrixStruct dataMatrixStruct = getDataMatrixFunctionsPrx().getDataMatrix();
			return getStatisticsStruct(dataMatrixStruct);
		}

		protected ValueSubTypeEnum columnValueSubType
		{
			get
			{
				return (ValueSubTypeEnum)(Enum.Parse(typeof(ValueSubTypeEnum), this.boxModule.GetPropertyString("ValueSubType")));
			}
		}
		#endregion

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
			result.columnSubType = columnValueSubType;
			result.columnType = ColumnTypeEnum.Derived;
			return result;
		}

		public override string[] getDistinctValues(Ice.Current __current)
		{
			DataMatrixStruct dataMatrixStruct = getDataMatrixFunctionsPrx().getDataMatrix();
            return Helpers.Data.Column.GetDistinctsStringSeq(dataMatrixStruct.database.connectionString, dataMatrixStruct.dataMatrixName, columnSelectExpression, boxModule.StringIceIdentity);
		}
		#endregion

		#region Sockets
		private DataMatrixFunctionsPrx getDataMatrixFunctionsPrx()
		{
			return DataMatrixFunctionsPrxHelper.checkedCast(
				SocketConnections.GetObjectPrx(boxModule, "DataMatrix")
			);
		}
		#endregion

		#region Actions
		public bool TestColumnSelectExpressionAction()
		{
			try
			{
				DataMatrixStruct dataMatrixStruct = getDataMatrixFunctionsPrx().getDataMatrix();
				Ferda.Modules.Helpers.Data.Column.TestColumnSelectExpression(
					dataMatrixStruct.database.connectionString,
					dataMatrixStruct.dataMatrixName,
					columnSelectExpression,
                    boxModule.StringIceIdentity);
				return true;
			}
            catch (Ferda.Modules.BoxRuntimeError) { }
			return false;
		}
		#endregion

		#region BoxInfo
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