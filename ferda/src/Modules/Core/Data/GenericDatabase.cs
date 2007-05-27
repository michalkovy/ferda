using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Diagnostics;

namespace Ferda.Guha.Data
{
    /// <summary>
    /// Provides essential functionality to access generic database,
    /// method for getting shema information, connection properties
    /// and enumerates through data tables in the database.
    /// </summary>
    /// <remarks>
    /// Note that we don't need a CreateDbDataReader class, for example, 
    /// because DbDataReader isn't directly created with the "new" operator; 
    /// instead it is always created via DbCommand.ExecuteReader().
    /// </remarks>
    public class GenericDatabase : IEnumerable<GenericDataTable>
    {
        #region Statics

        private static List<string> _tableTypes = new List<string>(
            new string[]
                {
                    "ALIAS",
                    "TABLE",
                    "SYNONYM",
                    "SYSTEM TABLE",
                    "VIEW",
                    "GLOBAL TEMPORARY",
                    "LOCAL TEMPORARY",
                    "EXTERNAL TABLE",
                    "SYSTEM VIEW"
                }
            );

        /// <summary>
        /// Gets the table types.
        /// </summary>
        /// <value>The table types.</value>
        public static List<string> TableTypes
        {
            get { return _tableTypes; }
        }

        #endregion

        private DateTime _lastAccess;

        /// <summary>
        /// Gets or sets the last access time.
        /// </summary>
        /// <value>The last access time.</value>
        public DateTime LastAccess
        {
            get { return _lastAccess; }
            set
            {
                if (_lastAccess < value)
                    _lastAccess = value;
            }
        }

        #region Setup

        private readonly string _providerInvariantName;

        /// <summary>
        /// Gets the invariant name of the db provider.
        /// </summary>
        /// <value>The invariant name of the db provider.</value>
        public string ProviderInvariantName
        {
            get { return _providerInvariantName; }
        }

        private readonly string _connectionString;

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        public string ConnectionString
        {
            get { return _connectionString; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Ferda.Guha.Data.GenericDatabase"/> class.
        /// </summary>
        /// <param name="providerInvariantName">Name of the provider invariant.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <exception cref="T:Ferda.Modules.BadParamsError">
        /// <b>DbProviderInvariantNameError</b>, <b>DbConnectionStringError</b>
        /// or <b>DbConnectionIsBrokenError</b>
        /// Thrown when unable to make a connection 
        /// </exception>
        internal GenericDatabase(string providerInvariantName, string connectionString)
        {
            _providerInvariantName = providerInvariantName;
            _connectionString = connectionString;

            // test connectivity
            DbConnection tmp = DbConnection;
            Trace.WriteLineIf(tmp.State != ConnectionState.Open, "Connetion not opened yet.");
            _lastAccess = DateTime.Now;
        }

        #endregion

        #region Provider-specific (fields)

        private DbProviderFactory _dbProviderFactory = null;

        /// <summary>
        /// Gets the db provider factory.
        /// </summary>
        /// <value>The db provider factory.</value>
        /// <exception cref="T:Ferda.Modules.BadParamsError">
        /// <b>DbProviderInvariantNameError</b>
        /// Thrown iff specified <c>dataProvider</c> is unknown.
        /// </exception>
        public DbProviderFactory DbProviderFactory
        {
            get
            {
                if (_dbProviderFactory == null)
                    _dbProviderFactory = DataProviderHelper.GetDbProviderFactory(ProviderInvariantName);
                return _dbProviderFactory;
            }
        }

        private DbConnection _dbConnection = null;

        /// <summary>
        /// Gets the db connection.
        /// </summary>
        /// <value>The db connection.</value>
        /// <exception cref="T:Ferda.Modules.BadParamsError">
        /// <b>DbProviderInvariantNameError</b>, <b>DbConnectionStringError</b>
        /// or <b>DbConnectionIsBrokenError</b>
        /// Thrown when unable to make a connection 
        /// </exception>
        public DbConnection DbConnection
        {
            get
            {
                if (_dbConnection == null)
                {
                    try
                    {
                        DbConnection dbConnection = DbProviderFactory.CreateConnection();
                        dbConnection.ConnectionString = ConnectionString;
                        dbConnection.Open();
                        _dbConnection = dbConnection;
                    }
                    catch (Exception e)
                    {
                        throw Exceptions.DbConnectionStringError(e, null);
                    }
                }
                if (_dbConnection.State == ConnectionState.Broken)
                    throw Exceptions.DbConnectionIsBrokenError(null, null);
                if (_dbConnection.State == ConnectionState.Closed)
                    _dbConnection.Open();
                return _dbConnection;
            }
        }

        private DbCommandBuilder _dbCommandBuilder;

        /// <summary>
        /// Gets the db command builder.
        /// </summary>
        /// <value>The db command builder.</value>
        public DbCommandBuilder DbCommandBuilder
        {
            get
            {
                if (_dbCommandBuilder == null)
                {
                    string prefix, suffix;

                    _dbCommandBuilder = DbProviderFactory.CreateCommandBuilder();
                    DataProviderHelper.GetIdentifeirQuotes(
                        ProviderInvariantName,
                        out prefix,
                        out suffix
                        );
                    _dbCommandBuilder.QuotePrefix = prefix;
                    _dbCommandBuilder.QuoteSuffix = suffix;
                }
                return _dbCommandBuilder;
            }
        }

        #endregion

        #region Provider-specific (methods)

        /// <summary>
        /// Creates the db command.
        /// </summary>
        /// <returns></returns>
        public DbCommand CreateDbCommand()
        {
            DbCommand result = DbProviderFactory.CreateCommand();
            result.Connection = DbConnection;
            return result;
        }

        /// <summary>
        /// Creates the db data adapter.
        /// </summary>
        /// <returns></returns>
        public DbDataAdapter CreateDbDataAdapter()
        {
            return DbProviderFactory.CreateDataAdapter();
        }

        /// <summary>
        /// Quotes the query identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>Quoted identifier.</returns>
        public string QuoteQueryIdentifier(string identifier)
        {
            if (identifier == null)
                identifier = String.Empty;
            return DbCommandBuilder.QuoteIdentifier(identifier);
        }

        #endregion

        private ConnectionInfo _connectionInfo = null;

        /// <summary>
        /// Gets the connection info.
        /// </summary>
        /// <value>The connection info.</value>
        public ConnectionInfo ConnectionInfo
        {
            get
            {
                lock (this)
                {
                    if (_connectionInfo == null)
                    {
                        _connectionInfo = new ConnectionInfo();
                        DbConnection conn = DbConnection;
                        _connectionInfo.connectionString = conn.ConnectionString;
                        _connectionInfo.connectionTimeout = conn.ConnectionTimeout;
                        _connectionInfo.databaseName = conn.Database;
                        _connectionInfo.dataSource = conn.DataSource;
                        {
                            // for ODBC sources ... determine driver
                            OdbcConnection odbcConn = conn as OdbcConnection;
                            if (odbcConn != null)
                                _connectionInfo.driver = odbcConn.Driver;
                            else
                                _connectionInfo.driver = String.Empty;
                        }
                        try
                        {
                            _connectionInfo.serverVersion = conn.ServerVersion;
                        }
                        catch (InvalidOperationException)
                        {
                            _connectionInfo.serverVersion = String.Empty;
                        }
                    }
                    return _connectionInfo;
                }
            }
        }

        #region Tables

        private Dictionary<string, GenericDataTable> _tables = null;

        /// <summary>
        /// Gets the tables schema.
        /// </summary>
        /// <value>The tables &lt; string tableName, GenericDataTable.</value>
        private Dictionary<string, GenericDataTable> tables
        {
            get
            {
                lock (this)
                {
                    if (_tables == null)
                    {
                        _tables = new Dictionary<string, GenericDataTable>();
                        DataTable schema = GetSchema("TABLES", null);
                        foreach (DataRow row in schema.Rows)
                        {
                            DataTableExplain resultItem = new DataTableExplain();
                            resultItem.name = row["TABLE_NAME"].ToString();
                            resultItem.type = row["TABLE_TYPE"].ToString();
			    if (schema.Columns.Contains("REMARKS"))
			    {
                            	resultItem.remarks = row["REMARKS"].ToString();
			    }
			    else
			    {
			    	resultItem.remarks = "not supported";
			    }
                            resultItem.recordsCount = -1;
                            _tables.Add(resultItem.name, new GenericDataTable(this, resultItem));
                        }
                    }
                    return _tables;
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="T:Ferda.Guha.Data.GenericDataTable"/> with the specified columnName.
        /// </summary>
        /// <value>Generic data table i.e. cached information about the data table.</value>
        /// <exception cref="T:Ferda.Modules.BadParamsError">
        /// <b>DbDataTableNameError</b>
        /// Thrown when the specified data table name is not found in the database.
        /// </exception>
        public GenericDataTable this[string dataTableName]
        {
            get
            {
                lock (this)
                {
                    if (tables.ContainsKey(dataTableName))
                        return tables[dataTableName];
                    else
                        throw Exceptions.DbDataTableNameError(null, null);
                }
            }
        }

        /// <summary>
        /// Gets names of the data tables in current database.
        /// </summary>
        /// <value>The names of data tables.</value>
        public string[] DataTablesNames
        {
            get
            {
                lock (this)
                {
                    string[] result = new string[tables.Keys.Count];
                    tables.Keys.CopyTo(result, 0);
                    return result;
                }
            }
        }

        /// <summary>
        /// Gets names of the data tables in current database which type is 
        /// one of specified <c>acceptableTypes</c>.
        /// </summary>
        /// <param name="acceptableTypes">The acceptable types.</param>
        /// <returns></returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// Thrown if unknonwn type of table is passed by <c>acceptableTypes</c> parameter.
        /// </exception>
        public string[] GetAcceptableDataTablesNames(string[] acceptableTypes)
        {
            return GetAcceptableDataTablesNames(new List<string>(acceptableTypes));
        }

        /// <summary>
        /// Gets names of the data tables in current database which type is 
        /// one of specified <c>acceptableTypes</c>.
        /// </summary>
        /// <param name="acceptableTypes">The acceptable types.</param>
        /// <returns></returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// Thrown if unknonwn type of table is passed by <c>acceptableTypes</c> parameter.
        /// </exception>
        public string[] GetAcceptableDataTablesNames(List<string> acceptableTypes)
        {
            if (acceptableTypes == null || acceptableTypes.Count == 0)
                return new string[0];
            else
            {
                // test validity of params
                foreach (string var in acceptableTypes)
                {
                    if (!TableTypes.Contains(var))
                        throw new ArgumentOutOfRangeException("acceptableTypes", var, null);
                }

                List<string> result = new List<string>();

                foreach (string var in DataTablesNames)
                {
                    if (this[var].IsAcceptable(acceptableTypes))
                        result.Add(var);
                }

                return result.ToArray();
            }
        }

        #endregion

        #region Schema / explain

        /// <summary>
        /// Gets the schema.
        /// </summary>
        /// <param name="collectionName">Name of the collection.</param>
        /// <param name="restrictionValues">The restriction values. {table catalog/schema, table owner, table name, column name}</param>
        /// <returns></returns>
        public DataTable GetSchema(string collectionName, string[] restrictionValues)
        {
            //if (restrictionValues == null)
            //{
            //    restrictionValues = new string[4];
            //}
            return DbConnection.GetSchema(collectionName, restrictionValues);

            #region restriction example

            /* /
            // restriction string array
            string[] res = new string[4];

            // all columns, all tables owned by dbo
            res[1] = "dbo";
            DataTable t1 = conn.GetSchema("Columns", res);

            // clear collection
            for (int i = 0; i < 4; i++) res[i] = null;
            // all columns, all tables named "authors", any owner/schema
            res[2] = "authors";
            DataTable t2 = conn.GetSchema("Columns", res);

            // clear collection
            for (int i = 0; i < 4; i++) res[i] = null;
            // columns named au_lname 
            // all tables named "authors", any owner/schema
            res[2] = "authors"; res[3] = "au_lname";
            DataTable t3 = conn.GetSchema("Columns", res);

            // clear collection
            for (int i = 0; i < 4; i++) res[i] = null;
            // columns named au_lname 
            // any tables, any owner/schema
            res[3] = "name";
            DataTable t4 = conn.GetSchema("Columns", res);
            /* */

            #endregion
        }

        #endregion

        #region IEnumerable<GenericDataTable> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<GenericDataTable> GetEnumerator()
        {
            return tables.Values.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return tables.Values.GetEnumerator();
        }

        #endregion
    }
}
