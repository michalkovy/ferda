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
        public string ColumnSelectExpression
        {
            get
            {
                return this.boxModule.GetPropertyString(ColumnBoxInfo.ColumnSelectExpressionPropertyName);
            }
        }

        public ValueSubTypeEnum ColumnValueSubType
        {
            get
            {
                //switch ordinary/derived column

                //ordinary
                DataMatrixStruct dataMatrixStruct = GetDataMatrixFunctionsPrx().getDataMatrix();
                return valueSubTypeCached.Value(boxModule.StringIceIdentity, dataMatrixStruct.database.lastReloadInfo, dataMatrixStruct.database.connectionString, dataMatrixStruct.dataMatrixName, ColumnSelectExpression, true);

                //derived
                //return (ValueSubTypeEnum)(Enum.Parse(typeof(ValueSubTypeEnum), this.boxModule.GetPropertyString("ValueSubType")));

            }
        }

        public ColumnTypeEnum ColumnType
        {
            get
            {
                return ColumnTypeEnum.Ordinary;
                //return ColumnTypeEnum.Derived;
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
            return statisticsStructCached.Value(boxModule.StringIceIdentity, dataMatrixStruct.database.lastReloadInfo, dataMatrixStruct.database.connectionString, dataMatrixStruct.dataMatrixName, dataMatrixStruct.recordsCount, ColumnSelectExpression, ColumnValueSubType);
        }
        protected StatisticsStruct getStatisticsStruct()
        {
            DataMatrixStruct dataMatrixStruct = GetDataMatrixFunctionsPrx().getDataMatrix();
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
                    cacheSetting.Add(Database.DatabaseBoxInfo.typeIdentifier + Database.DatabaseBoxInfo.OdbcConnectionStringPropertyName, connectionString);
                    cacheSetting.Add(DataMatrix.DataMatrixBoxInfo.typeIdentifier + DataMatrix.DataMatrixBoxInfo.DataMatrixNamePropertyName, dataMatrixName);
                    cacheSetting.Add(Column.ColumnBoxInfo.typeIdentifier + Column.ColumnBoxInfo.ColumnSelectExpressionPropertyName, columnSelectExpression);
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
            return valueSubTypeCached.Value(boxModule.StringIceIdentity, dataMatrixStruct.database.lastReloadInfo, dataMatrixStruct.database.connectionString, dataMatrixStruct.dataMatrixName, ColumnSelectExpression, true);
        }
        protected ValueSubTypeEnum getColumnValueSubType()
        {
            DataMatrixStruct dataMatrixStruct = GetDataMatrixFunctionsPrx().getDataMatrix();
            return getColumnValueSubType(dataMatrixStruct);
        }

        public bool IsColumnOrdinary()
        {
            DataMatrixStruct dataMatrixStruct = GetDataMatrixFunctionsPrx().getDataMatrix();
            string[] columns = Ferda.Modules.Helpers.Data.DataMatrix.GetColumns(
                dataMatrixStruct.database.connectionString,
                dataMatrixStruct.dataMatrixName,
                boxModule.StringIceIdentity);
            string columnSelectExpression = ColumnSelectExpression;
            foreach (string columnInDataMatrix in columns)
            {
                if (0 == String.Compare(columnInDataMatrix, columnSelectExpression))
                    return true;
            }
            return false;
        }

        public ValueSubTypeEnum GetColumnValueSubType()
        {
            DataMatrixStruct dataMatrixStruct = GetDataMatrixFunctionsPrx().getDataMatrix();
            //switch ordinary/derived column

            //ordinary
            return Ferda.Modules.Helpers.Data.Column.GetColumnSubTypeByDataType(
                Ferda.Modules.Helpers.Data.Column.GetDataType(
                dataMatrixStruct.database.connectionString,
                dataMatrixStruct.dataMatrixName,
                ColumnSelectExpression,
                boxModule.StringIceIdentity));
            //derived
            //return (ValueSubTypeEnum)(Enum.Parse(typeof(ValueSubTypeEnum), this.boxModule.GetPropertyString("ValueSubType")));
        }

        #region Functions
        public override ColumnStruct getColumn(Ice.Current __current)
        {
            DataMatrixStruct dataMatrixStruct = this.GetDataMatrixFunctionsPrx().getDataMatrix();
            string dataMatrixName = dataMatrixStruct.dataMatrixName;
            ColumnStruct result = new ColumnStruct();
            result.statistics = getStatisticsStruct(dataMatrixStruct);
            result.dataMatrix = dataMatrixStruct;
            result.columnSelectExpression = ColumnSelectExpression;
            result.columnSubType = getColumnValueSubType(dataMatrixStruct);
            result.columnType = ColumnType;
            return result;
        }

        public override string[] getDistinctValues(Ice.Current __current)
        {
            DataMatrixStruct dataMatrixStruct = GetDataMatrixFunctionsPrx().getDataMatrix();
            return Helpers.Data.Column.GetDistinctsStringSeq(dataMatrixStruct.database.connectionString, dataMatrixStruct.dataMatrixName, ColumnSelectExpression, boxModule.StringIceIdentity);
        }
        #endregion

        #region Sockets
        public DataMatrixFunctionsPrx GetDataMatrixFunctionsPrx()
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
                return this.GetDataMatrixFunctionsPrx().getColumns();
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
