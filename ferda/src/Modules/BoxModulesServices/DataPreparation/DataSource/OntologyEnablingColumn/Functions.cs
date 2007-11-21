// Functions.cs - Function objects for the column box module
//
// Author: Tomáš Kuchaø <tomas.kuchar@gmail.com>
// Documented by: Martin Ralbovský <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2006 Tomáš Kuchaø, Martin Ralbovský
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
using Ferda.Modules.Boxes.OntologyRelated.OntologyMapping;

namespace Ferda.Modules.Boxes.DataPreparation.Datasource.OntologyEnablingColumn
{
    /// <summary>
    /// Class is providing ICE functionality of the Ontology Enabling Column
    /// box module
    /// </summary>
    /// <remarks>
    /// This box has same functionality as Column box, the difference is:
    /// 1) input socket - Column box is connected directly to DataTable box, 
    /// Ontology Enabling Column is connected to Ontology Mapping box, which 
    /// is connected into DataTable box.
    /// 2) modules asking for creation - Ontology Enabling Column offers user
    /// one additional box type (comparing to Column box) to create - it is
    /// Ontology Derived Attribute
    /// </remarks>
    internal class Functions : ColumnFunctionsDisp_, IFunctions
    {
        /// <summary>
        /// The box module.
        /// </summary>
        protected BoxModuleI _boxModule;

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

        #region Properties

        //names of the properties
        public const string PropSelectExpression = "SelectExpression";
        public const string PropDataType = "DataType";
        public const string PropCardinality = "Cardinality";
        public const string PropValueMin = "ValueMin";
        public const string PropValueMax = "ValueMax";
        public const string PropValueAverage = "ValueAverage";
        public const string PropValueVariance = "ValueVariance";
        public const string PropValueStandardDeviation = "ValueStandardDeviation";
        public const string PropValueDistincts = "ValueDistincts";
        public const string SockMapping = "OntologyMapping";
        /// this box is connected to DataTable box through OntologyMapping box, 
        /// so SockDataTable is the name of OntologyMapping box socket
        public const string SockDataTable = "DataTable";
        

        /// <summary>
        /// The select expression of the column
        /// </summary>
        public string SelectExpression
        {
            get { return _boxModule.GetPropertyString(PropSelectExpression); }
        }

        /// <summary>
        /// Column cardinality
        /// </summary>
        public CardinalityEnum Cardinality
        {
            get
            {
                return (CardinalityEnum) Enum.Parse(
                                             typeof (CardinalityEnum),
                                             _boxModule.GetPropertyString(PropCardinality)
                                             );
            }
        }

        /// <summary>
        /// Column data type
        /// </summary>
        public StringTI DataType
        {
            get
            {
                ColumnExplain tmp = GetColumnExplain(false);
                return (tmp != null) ? tmp.dataType.ToString() : null;
            }
        }

        /// <summary>
        /// Minimal value of the column
        /// </summary>
        public StringTI ValueMin
        {
            get
            {
                ColumnStatistics tmp = GetColumnStatistics(false);
                return (tmp != null) ? tmp.valueMin : null;
            }
        }

        /// <summary>
        /// Maximal value of the column
        /// </summary>
        public StringTI ValueMax
        {
            get
            {
                ColumnStatistics tmp = GetColumnStatistics(false);
                return (tmp != null) ? tmp.valueMax : null;
            }
        }

        /// <summary>
        /// Average value of the column (iff supported)
        /// </summary>
        public StringTI ValueAverage
        {
            get
            {
                ColumnStatistics tmp = GetColumnStatistics(false);
                return (tmp != null) ? tmp.valueAverage : null;
            }
        }

        /// <summary>
        /// Variance of values of the column
        /// </summary>
        public DoubleTI ValueVariance
        {
            get
            {
                ColumnStatistics tmp = GetColumnStatistics(false);
                return (tmp != null) ? tmp.valueVariability : Double.NaN;
            }
        }

        /// <summary>
        /// Standard deviation of the values of the column
        /// </summary>
        public DoubleTI ValueStandardDeviation
        {
            get
            {
                ColumnStatistics tmp = GetColumnStatistics(false);
                return (tmp != null) ? tmp.valueStandardDeviation : Double.NaN;
            }
        }

        /// <summary>
        /// Distinct values of the column
        /// </summary>
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

        /// <summary>
        /// Gets the proxy of Ontology Mapping box (which is connected
        /// to this Ontology Enabling Column box)
        /// </summary>
        /// <param name="fallOnError">Iff the method should fall on error</param>
        /// <returns>Ontology Mapping proxy</returns>
        public OntologyMappingFunctionsPrx GetOntologyMappingFunctionsPrx(bool fallOnError)
        {
            return SocketConnections.GetPrx<OntologyMappingFunctionsPrx>(
                _boxModule,
                SockMapping,
                OntologyMappingFunctionsPrxHelper.checkedCast,
                fallOnError);
        }

        /// <summary>
        /// Gets the proxy of the Data Table (DataTable box is connected
        /// to OntologyMapping box, which is connected to this 
        /// OntologyEnablingColumn box)
        /// </summary>
        /// <param name="fallOnError">Iff the method should fall on error</param>
        /// <returns>Data table proxy</returns>
        public DataTableFunctionsPrx GetDataTableFunctionsPrx(bool fallOnError)
        {
            /*OntologyMappingFunctionsPrx ontologyMappingPrx = GetOntologyMappingFunctionsPrx(fallOnError);

            if (ontologyMappingPrx == null)
                return null;
            else
            {
                BoxModuleI ontologyMappingBoxModule = (BoxModuleI)ontologyMappingPrx.getBoxModule();

                return SocketConnections.GetPrx<DataTableFunctionsPrx>(
                    ontologyMappingBoxModule,
                    SockDataTable,
                    DataTableFunctionsPrxHelper.checkedCast,
                    fallOnError);
            }*/
            return null;
        }

        /// <summary>
        /// Gets the select expression of the data table. A column can be
        /// created from a SQL select expression over individual data table.
        /// </summary>
        /// <param name="fallOnError">Iff the method should fall on error</param>
        /// <returns>Select expression</returns>
        public string GetSelectExpression(bool fallOnError)
        {
            if (String.IsNullOrEmpty(SelectExpression) && fallOnError)
            {
                throw Exceptions.BadValueError(null, _boxModule.StringIceIdentity,
                                               "Property is not set.", new string[] {PropSelectExpression},
                                               restrictionTypeEnum.Missing);
            }
            else
                return SelectExpression;
        }

        /// <summary>
        /// The cache flag (determines if the data in the cache are actual).
        /// </summary>
        private CacheFlag _cacheFlag = new CacheFlag();
        /// <summary>
        /// The generic column, providing data about the column
        /// </summary>
        private GenericColumn _cachedValue = null;

        /// <summary>
        /// Gets the generic column (structure providing actual data).
        /// The method uses cache and access the data only if needed.
        /// </summary>
        /// <param name="fallOnError">Iff the method should fall on error</param>
        /// <returns>Generic column</returns>
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
                            ColumnInfo info = new ColumnInfo(
                                tmp,
                                selectExpression,
                                DbDataTypeEnum.UnknownType, CardinalityEnum.Nominal, ColumnTypeEnum.SimpleColumn,
                                String.Empty, String.Empty, String.Empty);
                            return
                                GenericDatabaseCache.GetGenericDatabase(connSetting)[tmp.dataTableName].GetGenericColumn
                                    (selectExpression, info);
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

        /// <summary>
        /// Returns column names of the data table that is connected
        /// to this column box.
        /// </summary>
        /// <param name="fallOnError">Iff the method should fall on error</param>
        /// <returns>Column names</returns>
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

        /// <summary>
        /// Gets information about the column (derived from the
        /// SQL EXPLAIN expreession.
        /// </summary>
        /// <param name="fallOnError">Iff the method should fall on error</param>
        /// <returns>Explanation of the column</returns>
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
        /// Gets statistical information about the column
        /// </summary>
        /// <param name="fallOnError">Iff the method should fall on error</param>
        /// <returns></returns>
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
        /// Gets values and frequencies of the column 
        /// (in an Ice defined structure)
        /// </summary>
        /// <param name="fallOnError">Iff the method should fall on error</param>
        /// <returns>Values and frequencies of the column</returns>
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

                            //fetching values via dictionary to ensure uniqueness
                            //because sql query in GenericColumn fails to return unique rows
                            //with complex expression (e.g. including arithmetic operations)
                            Dictionary<string, int> data1 = new Dictionary<string, int>();

                            foreach (DataRow row in dt.Rows)
                            {
                                if (row[0] == DBNull.Value)
                                {
                                    try
                                    {
                                        data1.Add(nullValueConstant.value, Convert.ToInt32(row[1]));
                                    }
                                    catch
                                    {
                                    }
                                //    data.Add(new ValueFrequencyPair(nullValueConstant.value, Convert.ToInt32(row[1])));
                                }
                                else
                                {
                                    try
                                    {
                                        data1.Add(row[0].ToString(), Convert.ToInt32(row[1]));
                                    }
                                    catch
                                    {
                                    }
                                 //   data.Add(new ValueFrequencyPair(row[0].ToString(), Convert.ToInt32(row[1])));
                                }
                            }

                            foreach (KeyValuePair<string, int> pair in data1)
                            {
                                data.Add(new ValueFrequencyPair(pair.Key, pair.Value));
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
        /// Gets information about the column (in am Ice defined structure)
        /// </summary>
        /// <param name="fallOnError">Iff the method should fall on error</param>
        /// <returns>Information about the column</returns>
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
                                                  Cardinality,
                                                  ColumnTypeEnum.SimpleColumn,
                                                  String.Empty,
                                                  String.Empty,
                                                  String.Empty);
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

        /// <summary>
        /// Gets information about the column (in am Ice defined structure)
        /// </summary>
        /// <param name="current__">Ice stuff</param>
        /// <returns>Information about the column</returns>
        public override ColumnInfo getColumnInfo(Current current__)
        {
            return GetColumnInfo(true);
        }

        /// <summary>
        /// Gets the column statistics (in an Ice defined structure)
        /// </summary>
        /// <param name="current__">Ice stuff</param>
        /// <returns>Statistics of the column</returns>
        public override ColumnStatistics getColumnStatistics(Current current__)
        {
            return GetColumnStatistics(true);
        }

        /// <summary>
        /// Gets values and frequencies of the column 
        /// (in an Ice defined structure)
        /// </summary>
        /// <param name="current__">Ice stuff</param>
        /// <returns>Values and frequencies of the column</returns>
        public override ValuesAndFrequencies getDistinctsAndFrequencies(Current current__)
        {
            return GetDistinctsAndFrequencies(true);
        }

        /// <summary>
        /// Gets the identification of the source data table
        /// </summary>
        /// <param name="current__">Ice stuff</param>
        /// <returns>String representing the identification of the source table</returns>
        public override string GetSourceDataTableId(Current current__)
        {
            DataTableFunctionsPrx prx = GetDataTableFunctionsPrx(true);
            if (prx != null)
                return prx.GetSourceDataTableId();
            return null;
        }

        #endregion
    }
}