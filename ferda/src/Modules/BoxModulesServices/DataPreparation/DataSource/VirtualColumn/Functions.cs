// Functions.cs - functionality for Virtual column box
//
// Author: Alexander Kuzmin <alexander.kuzmin@gmail.com>
//
// Copyright (c) 2007 Alexander Kuzmin
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA


using System;
using System.Collections.Generic;
using System.Data;
using Ferda.Guha.Data;
using Ferda.Modules.Helpers.Caching;
using Ice;

namespace Ferda.Modules.Boxes.DataPreparation.Datasource.VirtualColumn
{
    internal class Functions : ColumnFunctionsDisp_, IFunctions
    {
        /// <summary>
        /// The box module.
        /// </summary>
        protected BoxModuleI _boxModule;

        private CacheFlag _cacheFlag = new CacheFlag();
        private GenericColumn _cachedValue = null;


        #region Properties

        public const string PropName = "Name";
        public const string PropMasterIdColumn = "MasterIdColumn";
        public const string PropDetailIdColumn = "DetailIdColumn";
        public const string PropDetailResultColumn = "DetailResultColumn";
        public const string PropSelectExpression = "SelectExpression";
        //   public const string PropJoinKey = "JoinKey";
        public const string PropDataType = "DataType";
        public const string PropCardinality = "Cardinality";
        public const string PropValueMin = "ValueMin";
        public const string PropValueMax = "ValueMax";
        public const string PropValueAverage = "ValueAverage";
        public const string PropValueVariability = "ValueVariability";
        public const string PropValueStandardDeviation = "ValueStandardDeviation";
        public const string PropValueDistincts = "ValueDistincts";
        public const string SockMasterDataTable = "MasterDataTable";
        public const string SockDetailDataTable = "DetailDataTable";

        public string DetailTableIdColumn
        {
            get { return _boxModule.GetPropertyString(PropDetailIdColumn); }
        }

        public string DetailTableResultColumn
        {
            get { return _boxModule.GetPropertyString(PropDetailResultColumn); }
        }


        public string Name
        {
            get { return _boxModule.GetPropertyString(PropName); }
        }


        public string SelectExpression
        {
            get { return _boxModule.GetPropertyString(PropSelectExpression); }
        }

        /*
        public StringTI MasterIdColumn
        {
            get
            {
                return 
            }
        }*/

        public DataTableFunctionsPrx GetDataTableFunctionsPrx(bool fallOnError)
        {
            return SocketConnections.GetPrx<DataTableFunctionsPrx>(
                _boxModule,
                SockMasterDataTable,
                DataTableFunctionsPrxHelper.checkedCast,
                fallOnError);
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
        /*
        public string[] GetColumnsNames(bool fallOnError)
        {

            return ExceptionsHandler.GetResult<string[]>(
                fallOnError,
                delegate
                {
                    GenericDataTable tmp = GetGenericDataTable(fallOnError);
                    if (tmp != null)
                        return tmp.BasicColumnsNames;
                    return new string[0];
                },
                delegate
                {
                    return new string[0];
                },
                _boxModule.StringIceIdentity
                );



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
        }*/

        public CardinalityEnum Cardinality
        {
            get
            {
                return (CardinalityEnum)Enum.Parse(
                                             typeof(CardinalityEnum),
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


        public override string GetSourceDataTableId(Current current__)
        {
            DataTableFunctionsPrx prx = GetMasterDataTableFunctionsPrx(true);
            if (prx != null)
                return prx.GetSourceDataTableId();
            return null;
        }

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


        #region Functions


        /// <summary>
        /// Gets proxy of master datatable
        /// </summary>
        /// <param name="fallOnError"></param>
        /// <returns>Datatable proxy</returns>
        public DataTableFunctionsPrx GetMasterDataTableFunctionsPrx(bool fallOnError)
        {
            return SocketConnections.GetPrx<DataTableFunctionsPrx>(
                _boxModule,
                SockMasterDataTable,
                DataTableFunctionsPrxHelper.checkedCast,
                fallOnError);
        }

        /// <summary>
        /// Gets proxy of detail datatable
        /// </summary>
        /// <param name="fallOnError"></param>
        /// <returns>Datatable proxy</returns>
        public DataTableFunctionsPrx GetDetailDataTableFunctionsPrx(bool fallOnError)
        {
            return SocketConnections.GetPrx<DataTableFunctionsPrx>(
                _boxModule,
                SockDetailDataTable,
                DataTableFunctionsPrxHelper.checkedCast,
                fallOnError);
        }

        /// <summary>
        /// Gets column statistics
        /// </summary>
        /// <param name="fallOnError"></param>
        /// <returns>column statistics</returns>
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

        /// <summary>
        /// Gets distinct values and their frequencies
        /// </summary>
        /// <param name="fallOnError"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets virtual column
        /// </summary>
        /// <param name="fallOnError"></param>
        /// <returns></returns>
        public GenericColumn GetGenericColumn(bool fallOnError)
        {
            DataTableFunctionsPrx prx = GetMasterDataTableFunctionsPrx(fallOnError);
            if (prx == null)
                return null;

            DataTableFunctionsPrx prx1 = GetDetailDataTableFunctionsPrx(fallOnError);
            if (prx1 == null)
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

            DataTableInfo tmp1 =
                ExceptionsHandler.GetResult<DataTableInfo>(
                    fallOnError,
                    prx1.getDataTableInfo,
                    delegate
                    {
                        return null;
                    },
                    _boxModule.StringIceIdentity
                    );

            if (tmp1 == null)
                return null;

            DatabaseConnectionSettingHelper connSetting =
                new DatabaseConnectionSettingHelper(tmp.databaseConnectionSetting);

            Dictionary<string, IComparable> cacheSetting = new Dictionary<string, IComparable>();
            cacheSetting.Add(Database.BoxInfo.typeIdentifier + Database.Functions.PropConnectionString, connSetting);
            cacheSetting.Add(DataTable.BoxInfo.typeIdentifier + DataTable.Functions.PropName + "Master", tmp.dataTableName);
            cacheSetting.Add(DataTable.BoxInfo.typeIdentifier + DataTable.Functions.PropName + "Detail", tmp1.dataTableName);
            //     cacheSetting.Add(BoxInfo.typeIdentifier + PropSelectExpression, SelectExpression);

            //           if (_cacheFlag.IsObsolete(connSetting.LastReloadRequest, cacheSetting)
            //               || (_cachedValue == null && fallOnError))
            //           {
            _cachedValue = ExceptionsHandler.GetResult<GenericColumn>(
                fallOnError,
                delegate
                {
                    // FOR FUTURE IMPLEMENTATION: result column here does not have to be
                    // given as a name, a select expression can be applied
                    string selectExpression = GetSelectExpression(fallOnError);
                    ColumnInfo info = new ColumnInfo(
                        tmp1,
                        //   DetailTableResultColumn,
                     selectExpression,
                        DbDataTypeEnum.UnknownType, CardinalityEnum.Nominal, ColumnTypeEnum.VirtualColumn,
                        tmp1.dataTableName, DetailTableIdColumn, MasterTableIdColumn);
                    return
                        GenericDatabaseCache.GetGenericDatabase(connSetting)[tmp.dataTableName].GetGenericColumn
                        //  (DetailTableResultColumn, info);
                    (selectExpression, info);
                },
                delegate
                {
                    return null;
                },
                _boxModule.StringIceIdentity
                );
            //        }
            return _cachedValue;
        }

        /// <summary>
        /// Gets column EXPLAIN information
        /// </summary>
        /// <param name="fallOnError"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Id column of master datatable (for JOIN)
        /// </summary>
        public string MasterTableIdColumn
        {
            get { return _boxModule.GetPropertyString(PropMasterIdColumn); }
        }

        /// <summary>
        /// Id column of detail datatable (for JOIN)
        /// </summary>
        public string DetailIdColumn
        {
            get { return _boxModule.GetPropertyString(PropDetailIdColumn); }
        }


        /// <summary>
        /// Gets column names for master datatable
        /// </summary>
        /// <param name="fallOnError"></param>
        /// <returns>Array with column names</returns>
        public string[] GetMasterColumnsNames(bool fallOnError)
        {
            return ExceptionsHandler.GetResult<string[]>(
                fallOnError,
                delegate
                {
                    DataTableFunctionsPrx tmp1 = GetMasterDataTableFunctionsPrx(fallOnError);

                    if (tmp1 != null)
                        return tmp1.getColumnsNames();
                    return null;
                },
                delegate
                {
                    return null;
                },
                _boxModule.StringIceIdentity
                );
        }

        /// <summary>
        /// Gets column names for detail datatable
        /// </summary>
        /// <param name="fallOnError"></param>
        /// <returns>Array with column names</returns>
        public string[] GetDetailColumnsNames(bool fallOnError)
        {
            return ExceptionsHandler.GetResult<string[]>(
                fallOnError,
                delegate
                {
                    DataTableFunctionsPrx tmp1 = GetDetailDataTableFunctionsPrx(fallOnError);

                    if (tmp1 != null)
                    {
                        return tmp1.getColumnsNames();
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

        /// <summary>
        /// Gets full column names (table.column) for detail datatable
        /// </summary>
        /// <param name="fallOnError"></param>
        /// <returns>Array with column names</returns>
        public string[] GetDetailColumnsFullNames(bool fallOnError)
        {
            return ExceptionsHandler.GetResult<string[]>(
                fallOnError,
                delegate
                {
                    DataTableFunctionsPrx tmp1 = GetDetailDataTableFunctionsPrx(fallOnError);

                    if (tmp1 != null)
                    {
                        string[] detailCols =
                            tmp1.getColumnsNames();

                        DataTableFunctionsPrx prx = GetDetailDataTableFunctionsPrx(fallOnError);
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

                        for (int i = 0; i < detailCols.Length; i++)
                        {
                            detailCols[i] = tmp.dataTableName + "." + detailCols[i];
                        }
                        return detailCols;
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

        public string GetSelectExpression(bool fallOnError)
        {

            if (String.IsNullOrEmpty(SelectExpression))
            {
                if (fallOnError)
                {
                    throw Exceptions.BadValueError(null, _boxModule.StringIceIdentity,
                                               "Property is not set.", new string[] { PropSelectExpression },
                                               restrictionTypeEnum.Missing);
                }
                else
                    return String.Empty;
            }
            else
                return SelectExpression;
        }

        /*
        public string GetJoinKey(bool fallOnError)
        {
            
            if (String.IsNullOrEmpty(MasterTableIdColumn) && fallOnError)
            {
                throw Exceptions.BadValueError(null, _boxModule.StringIceIdentity,
                                               "Property is not set.", new string[] { PropJoinKey },
                                               restrictionTypeEnum.Missing);
            }
            else
                return MasterTableIdColumn;
        }*/

        /// <summary>
        /// Gets ColumnInfo information
        /// </summary>
        /// <param name="fallOnError"></param>
        /// <returns></returns>
        public ColumnInfo GetColumnInfo(bool fallOnError)
        {
            return ExceptionsHandler.GetResult<ColumnInfo>(
                fallOnError,
                delegate
                {
                    DataTableFunctionsPrx tmp1 = GetMasterDataTableFunctionsPrx(fallOnError);

                    DataTableFunctionsPrx prx1 = GetDetailDataTableFunctionsPrx(fallOnError);
                    if (prx1 == null)
                        return null;

                    DataTableInfo tmp =
                        ExceptionsHandler.GetResult<DataTableInfo>(
                            fallOnError,
                            prx1.getDataTableInfo,
                            delegate
                            {
                                return null;
                            },
                            _boxModule.StringIceIdentity
                            );

                    ColumnExplain tmp2 = GetColumnExplain(fallOnError);
                    // string selectExpression = GetSelectExpression(fallOnError);
                    if (tmp1 != null && tmp2 != null)
                        return new ColumnInfo(tmp1.getDataTableInfo(),
                                              DetailTableResultColumn,
                                              tmp2.dataType,
                                              Cardinality,
                                              ColumnTypeEnum.VirtualColumn,
                                              tmp.dataTableName,
                                              DetailTableIdColumn,
                                              MasterTableIdColumn);
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
        }

        #endregion
    }
}