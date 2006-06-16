using System;
using System.Collections.Generic;
using System.Diagnostics;
using Ferda.Guha.Data;
using Ferda.Modules.Helpers.Caching;
using Ice;

namespace Ferda.Modules.Boxes.DataPreparation.Datasource.Database
{
    internal class Functions : DatabaseFunctionsDisp_, IFunctions
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

        public const string PropProviderInvariantName = "ProviderInvariantName";
        public const string PropConnectionString = "ConnectionString";
        public const string PropLastReloadRequest = "LastReloadRequest";
        public const string PropAcceptableDataTableTypes = "AcceptableDataTableTypes";
        public const string PropConnectionTimeout = "ConnectionTimeout";
        public const string PropDatabaseName = "DatabaseName";
        public const string PropDataSource = "DataSource";
        public const string PropDriver = "Driver";
        public const string PropServerVersion = "ServerVersion";

        public string ProviderInvariantName
        {
            get { return _boxModule.GetPropertyString(PropProviderInvariantName); }
        }

        public string ConnectionString
        {
            get { return _boxModule.GetPropertyString(PropConnectionString); }
        }

        private DateTimeTI _lastReloadRequest = null;

        public DateTimeTI LastReloadRequest
        {
            get
            {
                if (_lastReloadRequest == null)
                    _lastReloadRequest = DateTime.MinValue;
                return _lastReloadRequest;
            }
            set
            {
                Debug.Assert((DateTime) _lastReloadRequest <= (DateTime) value);
                _lastReloadRequest = value;
            }
        }

        public string[] AcceptableDataTableTypes
        {
            get { return _boxModule.GetPropertyStringSeq(PropAcceptableDataTableTypes); }
        }

        public IntTI ConnectionTimeout
        {
            get
            {
                ConnectionInfo tmp = GetConnectionInfo(false);
                return (tmp != null) ? tmp.connectionTimeout : 0;
            }
        }

        public StringTI DatabaseName
        {
            get
            {
                ConnectionInfo tmp = GetConnectionInfo(false);
                return (tmp != null) ? GetConnectionInfo(false).databaseName : null;
            }
        }

        public StringTI DataSource
        {
            get
            {
                ConnectionInfo tmp = GetConnectionInfo(false);
                return (tmp != null) ? GetConnectionInfo(false).dataSource : null;
            }
        }

        public StringTI Driver
        {
            get
            {
                ConnectionInfo tmp = GetConnectionInfo(false);
                return (tmp != null) ? GetConnectionInfo(false).driver : null;
            }
        }

        public StringTI ServerVersion
        {
            get
            {
                ConnectionInfo tmp = GetConnectionInfo(false);
                return (tmp != null) ? GetConnectionInfo(false).serverVersion : null;
            }
        }

        #endregion

        #region Methods

        private CacheFlag _cacheFlag = new CacheFlag();
        private GenericDatabase _cachedValue = null;

        public GenericDatabase GetGenericDatabase(bool fallOnError)
        {
            DatabaseConnectionSettingHelper connSetting =
                new DatabaseConnectionSettingHelper(getDatabaseConnectionSetting());

            Dictionary<string, IComparable> cacheSetting = new Dictionary<string, IComparable>();
            cacheSetting.Add(BoxInfo.typeIdentifier + PropConnectionString, connSetting);

            if (_cacheFlag.IsObsolete(connSetting.LastReloadRequest, cacheSetting)
                || (_cachedValue == null && fallOnError))
            {
                _cachedValue = ExceptionsHandler.GetResult<GenericDatabase>(
                    fallOnError,
                    delegate
                        {
                            return GenericDatabaseCache.GetGenericDatabase(connSetting);
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

        public ConnectionInfo GetConnectionInfo(bool fallOnError)
        {
            return ExceptionsHandler.GetResult<ConnectionInfo>(
                fallOnError,
                delegate
                    {
                        GenericDatabase tmp = GetGenericDatabase(fallOnError);
                        if (tmp != null)
                            return tmp.ConnectionInfo;
                        return null;
                    },
                delegate
                    {
                        return null;
                    },
                _boxModule.StringIceIdentity
                );
        }

        public DataTableExplain[] GetDataTableExplainSeq(bool fallOnError)
        {
            string[] accepts = AcceptableDataTableTypes;

            return ExceptionsHandler.GetResult<DataTableExplain[]>(
                fallOnError,
                delegate
                    {
                        GenericDatabase tmp = GetGenericDatabase(fallOnError);
                        if (tmp != null)
                        {
                            List<DataTableExplain> result = new List<DataTableExplain>();
                            foreach (GenericDataTable table in tmp)
                            {
                                if (table.IsAcceptable(accepts))
                                    result.Add(table.Explain);
                            }
                            return result.ToArray();
                        }
                        return new DataTableExplain[0];
                    },
                delegate
                    {
                        return new DataTableExplain[0];
                    },
                _boxModule.StringIceIdentity
                );
        }

        public string[] GetDataTablesNames(bool fallOnError)
        {
            return ExceptionsHandler.GetResult<string[]>(
                fallOnError,
                delegate
                    {
                        GenericDatabase tmp = GetGenericDatabase(fallOnError);
                        if (tmp != null)
                            return tmp.GetAcceptableDataTablesNames(AcceptableDataTableTypes);
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

        public override DatabaseConnectionSetting getDatabaseConnectionSetting(Current current__)
        {
            return new DatabaseConnectionSetting(ProviderInvariantName, ConnectionString, LastReloadRequest);
        }

        public override ConnectionInfo getConnectionInfo(Current current__)
        {
            return GetConnectionInfo(true);
        }

        public override DataTableExplain[] getDataTableExplainSeq(Current current__)
        {
            return GetDataTableExplainSeq(true);
        }

        public override string[] getDataTablesNames(Current current__)
        {
            return GetDataTablesNames(true);
        }

        #endregion
    }
}