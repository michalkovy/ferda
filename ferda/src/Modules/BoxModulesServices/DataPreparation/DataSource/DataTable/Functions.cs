using System;
using System.Collections.Generic;
using System.Diagnostics;
using Ferda.Guha.Data;
using Ferda.Modules.Helpers.Caching;
using Ice;

namespace Ferda.Modules.Boxes.DataPreparation.Datasource.DataTable
{
    internal class Functions : DataTableFunctionsDisp_, IFunctions
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

        public const string PropName = "Name";
        public const string PropType = "Type";
        public const string PropRemarks = "Remarks";
        public const string PropPrimaryKeyColumns = "PrimaryKeyColumns";
        public const string PropRecordsCount = "RecordsCount";
        public const string SockDatabase = "Database";

        public string Name
        {
            get { return _boxModule.GetPropertyString(PropName); }
        }

        public StringTI Type
        {
            get
            {
                DataTableExplain tmp = GetDataTableExplain(false);
                return (tmp != null) ? tmp.type : null;
            }
        }

        public StringTI Remarks
        {
            get
            {
                DataTableExplain tmp = GetDataTableExplain(false);
                return (tmp != null) ? tmp.remarks : null;
            }
        }

        public string[] PrimaryKeyColumns
        {
            get { return _boxModule.GetPropertyStringSeq(PropPrimaryKeyColumns); }
        }

        public LongTI RecordsCount
        {
            get
            {
                DataTableExplain tmp = GetDataTableExplain(false);
                return (tmp != null) ? tmp.recordsCount : 0;
            }
        }

        #endregion

        #region Methods

        public DatabaseFunctionsPrx GetDatabaseFunctionsPrx(bool fallOnError)
        {
            return SocketConnections.GetPrx<DatabaseFunctionsPrx>(
                _boxModule,
                SockDatabase,
                DatabaseFunctionsPrxHelper.checkedCast,
                fallOnError);
        }

        public string GetName(bool fallOnError)
        {
            if (String.IsNullOrEmpty(Name) && fallOnError)
            {
                throw Exceptions.BadValueError(null, _boxModule.StringIceIdentity,
                                               "Property is not set.", new string[] {PropName},
                                               restrictionTypeEnum.Missing);
            }
            else
                return Name;
        }

        private CacheFlag _cacheFlag = new CacheFlag();
        private GenericDataTable _cachedValue = null;

        public GenericDataTable GetGenericDataTable(bool fallOnError)
        {
            DatabaseFunctionsPrx prx = GetDatabaseFunctionsPrx(fallOnError);
            if (prx == null)
                return null;

            DatabaseConnectionSetting connSettingTmp =
                ExceptionsHandler.GetResult<DatabaseConnectionSetting>(
                    fallOnError,
                    prx.getDatabaseConnectionSetting,
                    delegate
                        {
                            return null;
                        },
                    _boxModule.StringIceIdentity
                    );

            if (connSettingTmp == null)
                return null;

            DatabaseConnectionSettingHelper connSetting = new DatabaseConnectionSettingHelper(connSettingTmp);

            Dictionary<string, IComparable> cacheSetting = new Dictionary<string, IComparable>();
            cacheSetting.Add(Database.BoxInfo.typeIdentifier + Database.Functions.PropConnectionString, connSetting);
            cacheSetting.Add(BoxInfo.typeIdentifier + PropName, Name);

            if (_cacheFlag.IsObsolete(connSetting.LastReloadRequest, cacheSetting)
                || (_cachedValue == null && fallOnError))
            {
                _cachedValue = ExceptionsHandler.GetResult<GenericDataTable>(
                    fallOnError,
                    delegate
                        {
                            string name = GetName(fallOnError);
                            return GenericDatabaseCache.GetGenericDatabase(connSetting)[Name];
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
        /// Tests the unique columns i.e. unicity of values in data in specified
        /// <c>uniqueColumns</c>. (These columns can simulate Primary Key.)
        /// </summary>
        /// <param name="fallOnError">if set to <c>true</c> the method will fall on error.</param>
        /// <exception cref="T:Ferda.Modules.BadValueError">
        /// <b>DbUniqueKeyError</b>
        /// Thrown when the specified columns does not satisfy the
        /// unicity of values in the database.
        /// </exception>
        public void TryPrimaryKey(bool fallOnError)
        {
            Debug.Assert(fallOnError);
            ExceptionsHandler.GetResult<object>(
                fallOnError,
                delegate
                    {
                        GenericDataTable tmp = GetGenericDataTable(fallOnError);
                        if (tmp != null)
                            tmp.TestUniqueKey(PrimaryKeyColumns);
                        return null;
                    },
                delegate
                    {
                        return null;
                    },
                _boxModule.StringIceIdentity
                );
        }

        public string[] GetDataTablesNames(bool fallOnError)
        {
            DatabaseFunctionsPrx prx = GetDatabaseFunctionsPrx(fallOnError);
            return ExceptionsHandler.GetResult<string[]>(
                fallOnError,
                delegate
                    {
                        if (prx != null)
                            return prx.getDataTablesNames();
                        return new string[0];
                    },
                delegate
                    {
                        return new string[0];
                    },
                _boxModule.StringIceIdentity
                );
        }

        public DataTableExplain GetDataTableExplain(bool fallOnError)
        {
            return ExceptionsHandler.GetResult<DataTableExplain>(
                fallOnError,
                delegate
                    {
                        GenericDataTable tmp = GetGenericDataTable(fallOnError);
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

        public DataTableInfo GetDataTableInfo(bool fallOnError)
        {
            return ExceptionsHandler.GetResult<DataTableInfo>(
                fallOnError,
                delegate
                    {
                        DataTableExplain tmp1 = GetDataTableExplain(fallOnError);
                        DatabaseFunctionsPrx tmp2 = GetDatabaseFunctionsPrx(fallOnError);
                        string name = GetName(fallOnError);
                        if (tmp1 != null && tmp2 != null)
                            return new DataTableInfo(tmp2.getDatabaseConnectionSetting(),
                                                     name,
                                                     tmp1.type,
                                                     tmp1.remarks,
                                                     PrimaryKeyColumns,
                                                     tmp1.recordsCount);
                        return null;
                    },
                delegate
                    {
                        return null;
                    },
                _boxModule.StringIceIdentity
                );
        }

        public ColumnExplain[] GetColumnExplainSeq(bool fallOnError)
        {
            return ExceptionsHandler.GetResult<ColumnExplain[]>(
                fallOnError,
                delegate
                    {
                        List<ColumnExplain> result = new List<ColumnExplain>();
                        GenericDataTable tmp = GetGenericDataTable(fallOnError);
                        if (tmp != null)
                        {
                            foreach (GenericColumn column in tmp)
                            {
                                if (!column.IsDerived)
                                    result.Add(column.Explain);
                            }
                            return result.ToArray();
                        }
                        return new ColumnExplain[0];
                    },
                delegate
                    {
                        return new ColumnExplain[0];
                    },
                _boxModule.StringIceIdentity
                );
        }

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
        }

        #endregion

        #region Ice Functions

        public override ColumnExplain[] getColumnExplainSeq(Current current__)
        {
            return GetColumnExplainSeq(true);
        }

        public override string[] getColumnsNames(Current current__)
        {
            return GetColumnsNames(true);
        }

        public override DataTableInfo getDataTableInfo(Current current__)
        {
            return GetDataTableInfo(true);
        }

        public override string GetSourceDataTableId(Current current__)
        {
            return _boxModule.StringIceIdentity;
        }

        #endregion
    }
}