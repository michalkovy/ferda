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
                DataMatrixInfo dataMatrixInfo = GetDataMatrixFunctionsPrx().getDataMatrixInfo();
                return valueSubTypeCached.Value(boxModule.StringIceIdentity, dataMatrixInfo.database.lastReloadInfo, dataMatrixInfo.database.odbcConnectionString, dataMatrixInfo.dataMatrixName, ColumnSelectExpression, true);

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

        #region Cache: StatisticsInfo
        private class statisticsInfoCache : Ferda.Modules.Helpers.Caching.Cache
        {
            private StatisticsInfo value;
            public StatisticsInfo Value(string boxIdentity, DateTimeT lastReloadTime, string connectionString, string dataMatrixName, long dataMatrixRecordsCount, string columnSelectExpression, ValueSubTypeEnum columnValueSubType)
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
                    if (value == null)
                        value = new StatisticsInfo();
                    return value;
                }
            }
        }
        private statisticsInfoCache statisticsInfoCached = new statisticsInfoCache();
        #endregion
        protected StatisticsInfo getStatisticsStruct(DataMatrixInfo dataMatrixInfo)
        {
            return statisticsInfoCached.Value(boxModule.StringIceIdentity, dataMatrixInfo.database.lastReloadInfo, dataMatrixInfo.database.odbcConnectionString, dataMatrixInfo.dataMatrixName, dataMatrixInfo.recordsCount, ColumnSelectExpression, ColumnValueSubType);
        }
        protected StatisticsInfo getStatisticsInfo()
        {
            DataMatrixInfo dataMatrixInfo = GetDataMatrixFunctionsPrx().getDataMatrixInfo();
            return getStatisticsStruct(dataMatrixInfo);
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
        protected ValueSubTypeEnum getColumnValueSubType(DataMatrixInfo dataMatrixInfo)
        {
            return valueSubTypeCached.Value(boxModule.StringIceIdentity, dataMatrixInfo.database.lastReloadInfo, dataMatrixInfo.database.odbcConnectionString, dataMatrixInfo.dataMatrixName, ColumnSelectExpression, true);
        }
        protected ValueSubTypeEnum getColumnValueSubType()
        {
            DataMatrixInfo dataMatrixInfo = GetDataMatrixFunctionsPrx().getDataMatrixInfo();
            return getColumnValueSubType(dataMatrixInfo);
        }

        public bool IsColumnOrdinary()
        {
            DataMatrixInfo dataMatrixInfo = GetDataMatrixFunctionsPrx().getDataMatrixInfo();
            string[] columns = Ferda.Modules.Helpers.Data.DataMatrix.GetColumns(
                dataMatrixInfo.database.odbcConnectionString,
                dataMatrixInfo.dataMatrixName,
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
            DataMatrixInfo dataMatrixInfo = GetDataMatrixFunctionsPrx().getDataMatrixInfo();
            //switch ordinary/derived column

            //ordinary
            return Ferda.Modules.Helpers.Data.Column.GetColumnSubTypeByDataType(
                Ferda.Modules.Helpers.Data.Column.GetDataType(
                dataMatrixInfo.database.odbcConnectionString,
                dataMatrixInfo.dataMatrixName,
                ColumnSelectExpression,
                boxModule.StringIceIdentity));
            //derived
            //return (ValueSubTypeEnum)(Enum.Parse(typeof(ValueSubTypeEnum), this.boxModule.GetPropertyString("ValueSubType")));
        }

        #region Functions
        public override ColumnInfo getColumnInfo(Ice.Current __current)
        {
            DataMatrixInfo dataMatrixInfo = this.GetDataMatrixFunctionsPrx().getDataMatrixInfo();
            string dataMatrixName = dataMatrixInfo.dataMatrixName;
            ColumnInfo result = new ColumnInfo();
            result.statistics = getStatisticsStruct(dataMatrixInfo);
            result.dataMatrix = dataMatrixInfo;
            result.columnSelectExpression = ColumnSelectExpression;
            result.columnSubType = getColumnValueSubType(dataMatrixInfo);
            result.columnType = ColumnType;
            return result;
        }

        public override string[] getDistinctValues(Ice.Current __current)
        {
            DataMatrixInfo dataMatrixInfo = GetDataMatrixFunctionsPrx().getDataMatrixInfo();
            return Helpers.Data.Column.GetDistinctsStringSeq(dataMatrixInfo.database.odbcConnectionString, dataMatrixInfo.dataMatrixName, ColumnSelectExpression, boxModule.StringIceIdentity);
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
                return this.GetDataMatrixFunctionsPrx().getColumnsNames();
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

        public StatisticsInfo GetStatistics()
        {
            try
            {
                return getStatisticsInfo();
            }
            catch (Ferda.Modules.BoxRuntimeError) { }
            return new StatisticsInfo();
        }
        #endregion
    }
}
