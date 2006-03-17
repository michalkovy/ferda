using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Modules.Boxes.DataMiningCommon.Database
{
    /// <summary>
    /// Cache for names of data matrixes in the database.
    /// </summary>
    public class DataMatrixNamesCache : Ferda.Modules.Helpers.Caching.Cache
    {
        private string[] value = new string[0];

        /// <summary>
        /// Gets names of (acceptable) data matrixes (tables) in the specified data source.
        /// </summary>
        /// <param name="boxIdentity">The box identity.</param>
        /// <param name="lastReloadTime">The last reload time (for force reload of the cached value).</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="acceptableTypesOfTables">The acceptable types of tables.</param>
        /// <returns>
        /// Names of tables in specified (<c>connectionString</c>) data source.
        /// </returns>
        public string[] Value(string boxIdentity, DateTimeT lastReloadTime, string connectionString, string[] acceptableTypesOfTables)
        {
            lock (this)
            {
                Dictionary<string, IComparable> cacheSetting = new Dictionary<string, IComparable>();

                cacheSetting.Add(Database.DatabaseBoxInfo.typeIdentifier + DatabaseBoxInfo.OdbcConnectionStringPropertyName, connectionString);

                //string[] to string (IComparable needed)
                string comparableAcceptableTypesOfTables =
                    Ferda.Modules.Helpers.Common.Others.StringArray2String(acceptableTypesOfTables);
                cacheSetting.Add(Database.DatabaseBoxInfo.typeIdentifier + DatabaseBoxInfo.AcceptableTypesOfTablesPropertyName, comparableAcceptableTypesOfTables);

                if (IsObsolete(lastReloadTime, cacheSetting))
                {
                    value = new string[0];
                    value = Ferda.Modules.Helpers.Data.Database.GetTables(connectionString, null, boxIdentity);
                }
                if (value == null)
                    value = new string[0];
                return value;
            }
        }
    }
}
