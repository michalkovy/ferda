using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Modules.Boxes.DataMiningCommon.DataMatrix
{
    public class RecordsCountCache : Ferda.Modules.Helpers.Caching.Cache
    {
        private long value;

        /// <summary>
        /// Gets count of all records in specified <c>dataMatrixName</c>.
        /// </summary>
        /// <param name="boxIdentity">The box identity.</param>
        /// <param name="lastReloadTime">The last reload time.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="dataMatrixName">Name of the data matrix.</param>
        /// <returns></returns>
        public long Value(string boxIdentity, DateTimeT lastReloadTime, string connectionString, string dataMatrixName)
        {
            lock (this)
            {
                Dictionary<string, IComparable> cacheSetting = new Dictionary<string, IComparable>();
                cacheSetting.Add(Database.DatabaseBoxInfo.typeIdentifier + Database.DatabaseBoxInfo.OdbcConnectionStringPropertyName, connectionString);
                cacheSetting.Add(DataMatrix.DataMatrixBoxInfo.typeIdentifier + DataMatrix.DataMatrixBoxInfo.DataMatrixNamePropertyName, dataMatrixName);
                if (IsObsolete(lastReloadTime, cacheSetting))
                    value = Ferda.Modules.Helpers.Data.DataMatrix.GetRecordsCount(connectionString, dataMatrixName, boxIdentity);
                return value;
            }
        }
    }
}
