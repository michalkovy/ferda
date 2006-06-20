using System;
using System.Collections.Generic;
using System.Data;
using Ferda.Guha.Data;
using Ferda.Modules.Helpers.Caching;
using Ice;

namespace Ferda.Modules.Boxes.DataPreparation.Datasource.Column
{
    internal class Functions : ColumnFunctionsDisp_, IFunctions
    {
        /// <summary>
        /// The box module.
        /// </summary>
        protected BoxModuleI _boxModule;

        //protected IBoxInfo _boxInfo;

        #region IFunctions Members

        /// <summary>
        /// Sets the <see cref="T:Ferda.Modules.BoxModuleI">box module</see>
        /// and the <see cref="T:Ferda.Modules.Boxes.IBoxInfo">box info</see>.
        /// </summary>
        /// <param name="boxModule">The box module.</param>
        /// <param name="boxInfo">The box info.</param>
        public void setBoxModuleInfo(BoxModuleI boxModule, IBoxInfo boxInfo)
        {
            _boxModule = boxModule;
            //_boxInfo = boxInfo;
        }

        #endregion

        #region Properties

        public const string PropSelectExpression = "SelectExpression";
        public const string PropDataType = "DataType";
        public const string PropCardinality = "Cardinality";
        public const string PropValueMin = "ValueMin";
        public const string PropValueMax = "ValueMax";
        public const string PropValueAverage = "ValueAverage";
        public const string PropValueVariability = "ValueVariability";
        public const string PropValueStandardDeviation = "ValueStandardDeviation";
        public const string PropValueDistincts = "ValueDistincts";
        public const string SockDataTable = "DataTable";

        public string SelectExpression
        {
            get { return _boxModule.GetPropertyString(PropSelectExpression); }
        }

        public Guha.Data.CardinalityEnum Cardinality
        {
            get
            {
                return (Guha.Data.CardinalityEnum)Enum.Parse(
                                                       typeof(Guha.Data.CardinalityEnum),
                                                       _boxModule.GetPropertyString(PropCardinality)
                                                       );
            }
        }

        public StringTI DataType
        {
            get
            {
                ColumnExplain tmp = GetColumnExplain(false);
                return (tmp != null) ? tmp.dataType.ToString() : null;
            }
        }

        public StringTI ValueMin
        {
            get
            {
                ColumnStatistics tmp = GetColumnStatistics(false);
                return (tmp != null) ? tmp.valueMin : null;
            }
        }

        public StringTI ValueMax
        {
            get
            {
                ColumnStatistics tmp = GetColumnStatistics(false);
                return (tmp != null) ? tmp.valueMax : null;
            }
        }

        public StringTI ValueAverage
        {
            get
            {
                ColumnStatistics tmp = GetColumnStatistics(false);
                return (tmp != null) ? tmp.valueAverage : null;
            }
        }

        public DoubleTI ValueVariability
        {
            get
            {
                ColumnStatistics tmp = GetColumnStatistics(false);
                return (tmp != null) ? tmp.valueVariability : Double.NaN;
            }
        }

        public DoubleTI ValueStandardDeviation
        {
            get
            {
                ColumnStatistics tmp = GetColumnStatistics(false);
                return (tmp != null) ? tmp.valueStandardDeviation : Double.NaN;
            }
        }

        public LongTI ValueDistincts
        {
            get
            {
                ColumnStatistics tmp = GetColumnStatistics(false);
                return (tmp != null) ? tmp.valueDistincts : 0;
            }
        }

        #endregion

        #region Methods

        public DataTableFunctionsPrx GetDataTableFunctionsPrx(bool fallOnError)
        {
            return SocketConnections.GetPrx<DataTableFunctionsPrx>(
                _boxModule,
                SockDataTable,
                DataTableFunctionsPrxHelper.checkedCast,
                fallOnError);
        }

        public string GetSelectExpression(bool fallOnError)
        {
            if (String.IsNullOrEmpty(SelectExpression) && fallOnError)
            {
                throw Exceptions.BadValueError(null, _boxModule.StringIceIdentity,
                                               "Property is not set.", new string[] { PropSelectExpression },
                                               restrictionTypeEnum.Missing);
            }
            else
                return SelectExpression;
        }

        private CacheFlag _cacheFlag = new CacheFlag();
        private GenericColumn _cachedValue = null;

        public GenericColumn GetGenericColumn(bool fallOnError)
        {
            DataTableFunctionsPrx prx = GetDataTableFunctionsPrx(fallOnError);
            if (prx == null)
                return null;

            DataTableInfo tmp =
                ExceptionsHandler.GetResult<DataTableInfo>(
                    fallOnError,
                    prx.getDataTableInfo,
                    delegate
                    {
                        return null;
                    },
                    _boxModule.StringIceIdentity
                    );

            if (tmp == null)
                return null;

            DatabaseConnectionSettingHelper connSetting =
                new DatabaseConnectionSettingHelper(tmp.databaseConnectionSetting);

            Dictionary<string, IComparable> cacheSetting = new Dictionary<string, IComparable>();
            cacheSetting.Add(Database.BoxInfo.typeIdentifier + Database.Functions.PropConnectionString, connSetting);
            cacheSetting.Add(DataTable.BoxInfo.typeIdentifier + DataTable.Functions.PropName, tmp.dataTableName);
            cacheSetting.Add(BoxInfo.typeIdentifier + PropSelectExpression, SelectExpression);

            if (_cacheFlag.IsObsolete(connSetting.LastReloadRequest, cacheSetting)
                || (_cachedValue == null && fallOnError))
            {
                _cachedValue = ExceptionsHandler.GetResult<GenericColumn>(
                    fallOnError,
                    delegate
                    {
                        string selectExpression = GetSelectExpression(fallOnError);
                        return
                            GenericDatabaseCache.GetGenericDatabase(connSetting)[tmp.dataTableName].GetGenericColumn
                                (selectExpression);
                    },
                    delegate
                    {
                        return null;
                    },
                    _boxModule.StringIceIdentity
                    );
            }
            return _cachedValue;
        }

        public string[] GetColumnsNames(bool fallOnError)
        {
            DataTableFunctionsPrx prx = GetDataTableFunctionsPrx(fallOnError);
            return ExceptionsHandler.GetResult<string[]>(
                fallOnError,
                delegate
                {
                    if (prx != null)
                        return prx.getColumnsNames();
                    return new string[0];
                },
                delegate
                {
                    return new string[0];
                },
                _boxModule.StringIceIdentity
                );
        }

        public ColumnExplain GetColumnExplain(bool fallOnError)
        {
            return ExceptionsHandler.GetResult<ColumnExplain>(
                fallOnError,
                delegate
                {
                    GenericColumn tmp = GetGenericColumn(fallOnError);
                    if (tmp != null)
                        return tmp.Explain;
                    return null;
                },
                delegate
                {
                    return null;
                },
                _boxModule.StringIceIdentity
                );
        }

        public ColumnStatistics GetColumnStatistics(bool fallOnError)
        {
            return ExceptionsHandler.GetResult<ColumnStatistics>(
                fallOnError,
                delegate
                {
                    GenericColumn tmp = GetGenericColumn(fallOnError);
                    if (tmp != null)
                        return tmp.Statistics;
                    return null;
                },
                delegate
                {
                    return null;
                },
                _boxModule.StringIceIdentity
                );
        }

        public ValuesAndFrequencies GetDistinctsAndFrequencies(bool fallOnError)
        {
            return ExceptionsHandler.GetResult<ValuesAndFrequencies>(
                fallOnError,
                delegate
                {
                    GenericColumn tmp1 = GetGenericColumn(fallOnError);
                    ColumnExplain tmp2 = GetColumnExplain(fallOnError);
                    if (tmp1 != null && tmp2 != null)
                    {
                        System.Data.DataTable dt = tmp1.GetDistinctsAndFrequencies(null);
                        ValuesAndFrequencies result = new ValuesAndFrequencies();
                        result.dataType = tmp2.dataType;
                        List<ValueFrequencyPair> data = new List<ValueFrequencyPair>();

                        foreach (DataRow row in dt.Rows)
                        {
                            if (row[0] == DBNull.Value)
                            {
                                data.Add(new ValueFrequencyPair(nullValueConstant.value, Convert.ToInt32(row[1])));
                            }
                            else
                            {
                                data.Add(new ValueFrequencyPair(row[0].ToString(), Convert.ToInt32(row[1])));
                            }
                        }

                        result.data = data.ToArray();
                        return result;
                    }
                    return null;
                },
                delegate
                {
                    return null;
                },
                _boxModule.StringIceIdentity
                );
        }

        public ColumnInfo GetColumnInfo(bool fallOnError)
        {
            return ExceptionsHandler.GetResult<ColumnInfo>(
                fallOnError,
                delegate
                {
                    DataTableFunctionsPrx tmp1 = GetDataTableFunctionsPrx(fallOnError);
                    ColumnExplain tmp2 = GetColumnExplain(fallOnError);
                    string selectExpression = GetSelectExpression(fallOnError);
                    if (tmp1 != null && tmp2 != null)
                        return new ColumnInfo(tmp1.getDataTableInfo(),
                                              selectExpression,
                                              tmp2.dataType,
                                              Cardinality
                            );
                    return null;
                },
                delegate
                {
                    return null;
                },
                _boxModule.StringIceIdentity
                );
        }

        #endregion

        #region Ice Functions

        public override ColumnInfo getColumnInfo(Current current__)
        {
            return GetColumnInfo(true);
        }

        public override ColumnStatistics getColumnStatistics(Current current__)
        {
            return GetColumnStatistics(true);
        }

        public override ValuesAndFrequencies getDistinctsAndFrequencies(Current current__)
        {
            return GetDistinctsAndFrequencies(true);
        }

        #endregion
    }
}