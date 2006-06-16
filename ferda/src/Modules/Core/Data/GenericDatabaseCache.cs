using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;

namespace Ferda.Guha.Data
{
#if DEBUG
    /// <summary>
    /// Ferda.Guha.Data is a namespace providing DAL (Data Access Layer) 
    /// to other components of Ferda system. It is provider-independent
    /// (generic) DAL based on ADO.NET 2.0 (for more information please 
    /// see section "see also" below) it means that any data provider even 
    /// Odbc, OleDb, MS Sql, Oracle can be accesed. The only condition 
    /// to use the data source by the provider-independent data access 
    /// layer is that the provider must exists for the current data source 
    /// and it must be installed on the .NET Framework (or Mono) where the 
    /// module should run.
    /// </summary>
    /// <remarks>
    /// <b>Introduction</b>
    /// <para>
    /// Many (box) modules of Ferda system needs to access to database
    /// and gets some information. The problem is that each such module
    /// should refresh the information about datasource every time it is 
    /// asked on the information. It leads to negative effects on efficiency
    /// of the system. More over if each such module should in each such 
    /// itteration create the connection to the data source, gets the information
    /// and than again leave the connection (close and dispose), it is 
    /// really big performance dependencies becase intialization of (e.g. Odbc)
    /// connection is relatively slow.
    /// </para>
    /// <para>
    /// Throgh reasons mentioned above there is requirement to cache the 
    /// connection and even other information about datasource.
    /// </para>
    /// <b>Description</b>
    /// <para>
    /// The only recommended entry point to this library should be through
    /// static class <see cref="T:Ferda.Guha.Data.GenericDatabaseCache"/>
    /// which holds (and create or refresh if necessary) connections 
    /// (<see cref="M:Ferda.Guha.Data.GenericDatabaseCache.GetGenericDatabase(Ferda.Guha.Data.DatabaseConnectionSettingHelper)"/>)
    /// to all data sources. For equal connection parameters are connections 
    /// shared. To each cached connection is assigned cache of basic information
    /// that are provided by the DAL i.e. if the DAL is asked twice for schema
    /// of database of specified connection it is evaluated most highly once
    /// and if the information was cached earlier, it is not computed again.
    /// <c>
    /// The only exception of this is when connection parameters contains 
    /// request on reload (it is specified by DateTime i.e. if the specified
    /// date time is higher than last reload request than cache is dropped and
    /// new reload request is memorized.)
    /// </c>
    /// </para>
    /// </remarks>
    /// <seealso href="http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnvs05/html/vsgenerics.asp?frame=true&amp;hidetoc=true">
    /// Generic Coding with the ADO.NET 2.0 Base Classes and Factories (MSDN Library)
    /// </seealso>
    /// <seealso href="http://msdn2.microsoft.com/en-us/library/t9f29wbk.aspx">
    /// Writing Provider-Independent Code in ADO.NET (MSDN Library)
    /// </seealso>
    /// <seealso href="http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnvs05/html/adonet2schemas.asp?frame=true&amp;hidetoc=true">
    /// Schemas in ADO.NET 2.0 (MSDN Library)
    /// </seealso>
    public class NamespaceDoc
    {
        private NamespaceDoc()
        {
        }
    }
#endif


    /// <summary>
    /// Holds cache of opened connection and information corresponding to it.
    /// </summary>
    public static class GenericDatabaseCache
    {
        /// <summary>
        /// Key: connection parameters
        /// Value: cached generic databases (shared database connection and cached information about the data source)
        /// </summary>
        internal static Dictionary<DatabaseConnectionSettingHelper, GenericDatabase> _connections =
            new Dictionary<DatabaseConnectionSettingHelper, GenericDatabase>();

        private static GarbageThread _garbage = new GarbageThread();
        private static Thread _garbageThread;

        /// <summary>
        /// Gets the generic database (shared database connection and 
        /// cached information about the data source).
        /// </summary>
        /// <param name="databaseConnectionSetting">The database connection setting.</param>
        /// <remarks>
        /// If <c>LastReloadRequest</c> in <c>databaseConnectionSetting</c>
        /// is greater than last reload request associated with cached generic database
        /// than the generic database is dropped and created again (i.e. cache is deleted).
        /// </remarks>
        /// <returns>
        /// Shared database connection and cached information about the data source
        /// </returns>
        /// <exception cref="T:Ferda.Modules.BadParamsError">
        /// <b>DbProviderInvariantNameError</b>, <b>DbConnectionStringError</b>
        /// or <b>DbConnectionIsBrokenError</b>
        /// Thrown when unable to make a connection 
        /// </exception>
        public static GenericDatabase GetGenericDatabase(DatabaseConnectionSettingHelper databaseConnectionSetting)
        {
            lock ("gargageThread")
            {
                if (_garbageThread == null)
                {
                    _garbageThread = new Thread(_garbage.Run);
                    _garbageThread.Start();
                }
            }
            lock (_connections)
            {
                //iff cached
                if (_connections.ContainsKey(databaseConnectionSetting))
                {
                    foreach (KeyValuePair<DatabaseConnectionSettingHelper, GenericDatabase> kvp in _connections)
                    {
                        if (kvp.Key.Equals(databaseConnectionSetting))
                        {
                            //test if cached value is actual
                            if (kvp.Key.LastReloadRequest >= databaseConnectionSetting.LastReloadRequest) //actual
                            {
                                kvp.Value.LastAccess = DateTime.Now;
                                return kvp.Value;
                            }
                            else //not actual
                            {
                                //delete cached value ... continue with
                                _connections.Remove(kvp.Key);
                                break;
                            }
                        }
                    }
                }

                //iff connection doesn`t exist (wasn`t saved)
                {
                    GenericDatabase result = new GenericDatabase(
                        databaseConnectionSetting.ProviderInvariantName,
                        databaseConnectionSetting.ConnectionString
                        );

                    //save opened connection if connection setting is valid
                    _connections.Add(databaseConnectionSetting, result);
                    return result;
                }
            }
        }

        /// <summary>
        /// Removes the generic database.
        /// </summary>
        /// <param name="databaseConnectionSetting">The database connection setting.</param>
        public static void RemoveGenericDatabase(DatabaseConnectionSettingHelper databaseConnectionSetting)
        {
            lock (_connections)
            {
                DbConnection conn = null;
                try
                {
                    GenericDatabase gd;
                    if (_connections.TryGetValue(databaseConnectionSetting, out gd))
                    {
                        conn = gd.DbConnection;
                        _connections.Remove(databaseConnectionSetting);
                    }
                }
                catch
                {
                }
                finally
                {
                    if (conn != null)
                        conn.Dispose();
                }
            }
        }
    }
}