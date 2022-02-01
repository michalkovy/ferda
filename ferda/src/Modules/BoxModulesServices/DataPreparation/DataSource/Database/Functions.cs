// Functions.cs - Function objects for the database box module
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
using System.Diagnostics;
using Ferda.Guha.Data;
using Ferda.Modules.Helpers.Caching;
using Ice;

namespace Ferda.Modules.Boxes.DataPreparation.Datasource.Database
{
    /// <summary>
    /// Class is providing ICE functionality of the Database
    /// box module
    /// </summary>
    public class Functions : DatabaseFunctionsDisp_, IFunctions
    {
        /// <summary>
        /// The current box module
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
            //_boxInfo = boxInfo;
        }

        #endregion

        #region Properties

        //names of the properties
        public const string PropProviderInvariantName = "ProviderInvariantName";
        public const string PropConnectionString = "ConnectionString";
        public const string PropLastReloadRequest = "LastReloadRequest";
        public const string PropAcceptableDataTableTypes = "AcceptableDataTableTypes";
        public const string PropConnectionTimeout = "ConnectionTimeout";
        public const string PropDatabaseName = "DatabaseName";
        public const string PropDataSource = "DataSource";
        public const string PropDriver = "Driver";
        public const string PropServerVersion = "ServerVersion";

        /// <summary>
        /// The Provider invariant name property
        /// </summary>
        public string ProviderInvariantName
        {
            get { return _boxModule.GetPropertyString(PropProviderInvariantName); }
        }

        /// <summary>
        /// The Connection string property
        /// </summary>
        public string ConnectionString
        {
            get { return _boxModule.GetPropertyString(PropConnectionString); }
        }

        private DateTimeTI _lastReloadRequest = null;

        /// <summary>
        /// The Last Reload Request property
        /// </summary>
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

        /// <summary>
        /// The Acceptable Data Types property
        /// </summary>
        public string[] AcceptableDataTableTypes
        {
            get { return _boxModule.GetPropertyStringSeq(PropAcceptableDataTableTypes); }
        }

        /// <summary>
        /// The Connection timeout property
        /// </summary>
        public IntTI ConnectionTimeout
        {
            get
            {
                Ferda.Guha.Data.ConnectionInfo tmp = GetConnectionInfo(false);
                return (tmp != null) ? tmp.connectionTimeout : 0;
            }
        }

        /// <summary>
        /// The Database name property
        /// </summary>
        public StringTI DatabaseName
        {
            get
            {
                Ferda.Guha.Data.ConnectionInfo tmp = GetConnectionInfo(false);
                return (tmp != null) ? GetConnectionInfo(false).databaseName : null;
            }
        }

        /// <summary>
        /// The Data source property
        /// </summary>
        public StringTI DataSource
        {
            get
            {
                Ferda.Guha.Data.ConnectionInfo tmp = GetConnectionInfo(false);
                return (tmp != null) ? GetConnectionInfo(false).dataSource : null;
            }
        }

        /// <summary>
        /// The Driver property
        /// </summary>
        public StringTI Driver
        {
            get
            {
                Ferda.Guha.Data.ConnectionInfo tmp = GetConnectionInfo(false);
                return (tmp != null) ? GetConnectionInfo(false).driver : null;
            }
        }

        /// <summary>
        /// The Server version property
        /// </summary>
        public StringTI ServerVersion
        {
            get
            {
                Ferda.Guha.Data.ConnectionInfo tmp = GetConnectionInfo(false);
                return (tmp != null) ? GetConnectionInfo(false).serverVersion : null;
            }
        }

        #endregion

        #region Methods

        private CacheFlag _cacheFlag = new CacheFlag();
        private GenericDatabase _cachedValue = null;

        /// <summary>
        /// Gets the generic database with aid of cashing
        /// </summary>
        /// <param name="fallOnError">If to fall on error</param>
        /// <returns>The generic database</returns>
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

        /// <summary>
        /// Function returns the <see cref="T:Ferda.Guha.Data.ConnectionInfo">
        /// Connection Info</see> structure
        /// </summary>
        /// <param name="fallOnError">If to fall on error</param>
        /// <returns>The connection info</returns>
        public Ferda.Guha.Data.ConnectionInfo GetConnectionInfo(bool fallOnError)
        {
            return ExceptionsHandler.GetResult<Ferda.Guha.Data.ConnectionInfo>(
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

        /// <summary>
        /// Gets the array of<see cref="T:Ferda.Guha.Data.DataTableExplain">DataTableExplain
        /// </see> structures that hold information about the tables in the database
        /// </summary>
        /// <param name="fallOnError">If to fall on error</param>
        /// <returns>Information about data tables</returns>
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

        /// <summary>
        /// Gets the names of data tables in the database
        /// </summary>
        /// <param name="fallOnError">If to fail on error</param>
        /// <returns>The names of data tables in the database</returns>
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

        /// <summary>
        /// Gets the <see cref="T:Ferda.Guha.Data.DatabaseConnectionSetting">Connection
        /// setting</see> object of the database
        /// </summary>
        /// <param name="current__">Ice stuff</param>
        /// <returns>The database connection setting</returns>
        public override DatabaseConnectionSetting getDatabaseConnectionSetting(Current current__)
        {
            return new DatabaseConnectionSetting(ProviderInvariantName, ConnectionString, LastReloadRequest);
        }

        /// <summary>
        /// Gets the <see cref="T:Ferda.Guha.Data.ConnectionInfo">ConnectionInfo</see>
        /// structure that holds information about the connection
        /// </summary>
        /// <param name="current__">Ice stuff</param>
        /// <returns>Connection info</returns>
        public override Ferda.Guha.Data.ConnectionInfo getConnectionInfo(Current current__)
        {
            return GetConnectionInfo(true);
        }

        /// <summary>
        /// Gets information about tables in the database
        /// </summary>
        /// <param name="current__">Ice stuff</param>
        /// <returns>Information about the tables in the database</returns>
        public override DataTableExplain[] getDataTableExplainSeq(Current current__)
        {
            return GetDataTableExplainSeq(true);
        }

        /// <summary>
        /// Gets names of tables in the database
        /// </summary>
        /// <param name="current__">Ice stuff</param>
        /// <returns>Names of tables</returns>
        public override string[] getDataTablesNames(Current current__)
        {
            return GetDataTablesNames(true);
        }

        #endregion
    }
}