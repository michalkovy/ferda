using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Modules.Boxes.DataMiningCommon.Database
{
    public class ConnectionInfoCache : Ferda.Modules.Helpers.Caching.Cache
    {
        private ConnectionInfo value = new ConnectionInfo();

        /// <summary>
        /// Gets the connection info. (some fields of <see cref="T:System.Data.Odbc.OdbcConnection"/>)
        /// If specified <c>odbcConnectionString</c> doesn`t work empty strings are returned in
        /// result fields.
        /// </summary>
        /// <param name="boxIdentity">The box identity.</param>
        /// <param name="lastReloadTime">The last reload time (for force reload of the cached value).</param>
        /// <param name="connectionString">The connection string.</param>
        public ConnectionInfo Value(string boxIdentity, DateTimeT lastReloadTime, string connectionString)
        {
            lock (this)
            {
                Dictionary<string, IComparable> cacheSetting = new Dictionary<string, IComparable>();
                cacheSetting.Add(Database.DatabaseBoxInfo.typeIdentifier + DatabaseBoxInfo.OdbcConnectionStringPropertyName, connectionString);
                if (IsObsolete(lastReloadTime, cacheSetting))
                {
                    value = Ferda.Modules.Helpers.Data.Database.GetConnectionInfo(connectionString, boxIdentity);
                }
                return value;
            }
        }
    }
}
