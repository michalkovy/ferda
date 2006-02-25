#define CONNECTIONS_SHARING_DEBUG

using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Odbc;
using Ferda.Modules.Boxes.DataMiningCommon.Database;

namespace Ferda.Modules.Helpers.Data
{
    /// <summary>
    /// Manages OdbcConnections.
    /// </summary>
    public static class OdbcConnections
    {
        /// <summary>
        /// Bads the connection string error (Exception).
        /// </summary>
        /// <param name="e">The e.</param>
        /// <param name="boxIdentity">The box identity.</param>
        /// <returns>Exception <see cref="T:Ferda.Modules.BadParamsError"/>.</returns>
        public static Ferda.Modules.BadParamsError BadConnectionStringError(Exception e, string boxIdentity)
        {
            return Exceptions.BadParamsError(e, boxIdentity, "Bad ODBC connection string specified. Could not connect to database.", Ferda.Modules.restrictionTypeEnum.DbConnectionString);
        }

        private class CachedConnection
        {
            public CachedConnection(OdbcConnection odbcConnection)
            {
                connection = odbcConnection;
            }

            private OdbcConnection connection;
            public OdbcConnection Connection
            {
                get { return connection; }
            }
        }

        /// <summary>
        /// OdbcConnections
        /// </summary>
        /// <remarks>
        /// <para>Key of a dictionary is an <c>ODBC connection string</c>.</para>
        /// <para>Value of a dictionary is an <c><see cref="T:System.Data.Odbc.OdbcConnection"/></c>.</para>
        /// </remarks>
        private static Dictionary<string, CachedConnection> odbcConnections = new Dictionary<string, CachedConnection>();

        /// <summary>
        /// Close and destroy an OdbcConnection.
        /// </summary>
        /// <param name="odbcConnectionString">An ODBC connection string representing an OdbcConnection to be removed.</param>
        public static void RemoveConnection(string odbcConnectionString)
        {
            if (String.IsNullOrEmpty(odbcConnectionString))
                return;

            //trim odbcConnectionString
            odbcConnectionString = odbcConnectionString.Trim();

            //close and remove OdbcConnection (by odbcConnectionString)
            if (odbcConnections.ContainsKey(odbcConnectionString))
            {
                odbcConnections[odbcConnectionString].Connection.Close();
                odbcConnections.Remove(odbcConnectionString);
            }
        }

        private struct stackInfo
        {
            public stackInfo(string stack, string boxIdentity)
            {
                Stack = stack;
                BoxIdentity = boxIdentity;
            }
            public string Stack;
            public string BoxIdentity;
        }
        // Key: OdbcConnectionString
        // Value: stackInfo i.e. Last stack dump for this connection string and last boxIdentity
        private static Dictionary<string, stackInfo> connectionsSharingDebug = new Dictionary<string, stackInfo>();

        /// <summary>
        /// Get an ODBC connection matching <c>odbcConnectionString</c> or throw <see cref="T:Ferda.Modules.BadParamsError"/>.
        /// </summary>
        /// <param name="odbcConnectionString">An ODBC connection string.</param>
        /// <param name="boxIdentity">An identity of BoxModule witch wants to work with this ODBC connection.</param>
        /// <returns>Typically opened ODBC connection.</returns>
        /// <exception cref="T:Ferda.Modules.BadParamsError"/>
        public static OdbcConnection GetConnection(string odbcConnectionString, string boxIdentity)
        {
            if (String.IsNullOrEmpty(odbcConnectionString))
                throw BadConnectionStringError(null, boxIdentity);

            //trim odbcConnectionString
            odbcConnectionString = odbcConnectionString.Trim();

            CachedConnection cachedConnection;
            OdbcConnection result = null;

            //iff connection exist (was saved)
            if (odbcConnections.TryGetValue(odbcConnectionString, out cachedConnection))
            {
                result = cachedConnection.Connection;
                //connection is not opened
                if ((result.State == System.Data.ConnectionState.Closed)
                    || (result.State == System.Data.ConnectionState.Broken))
                    try
                    {
                        result.Open();
                    }
                    catch (Exception e)
                    {
                        throw BadConnectionStringError(e, boxIdentity);
                    }
            }
            //iff connection doesn`t exist (wasn`t saved)
            else if (!odbcConnections.ContainsKey(odbcConnectionString))
            {
                try
                {
                    result = new OdbcConnection(odbcConnectionString);
                    result.Open();
                }
                catch (Exception e)
                {
                    throw BadConnectionStringError(e, boxIdentity);
                }
                //save opened connection if odbcConnectionString is valid
                odbcConnections.Add(odbcConnectionString, new CachedConnection(result));
            }

            //test if connection was opened
            if (result != null && result.State == System.Data.ConnectionState.Open)
                return result;
            else
            {
                throw BadConnectionStringError(null, boxIdentity);
            }
        }
    }
}