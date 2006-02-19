using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Modules.Boxes.DataMiningCommon.DataMatrix
{
    public class ExplainDataMatrixStructureCache : Ferda.Modules.Helpers.Caching.Cache
    {
        private ColumnSchemaInfo[] value = new ColumnSchemaInfo[0];

        /// <summary>
        /// Gets infos about columns in specified data source`s table.
        /// </summary>
        /// <param name="boxIdentity">The box identity.</param>
        /// <param name="lastReloadTime">The last reload time.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="dataMatrixName">Name of the data matrix.</param>
        /// <returns></returns>
        public ColumnSchemaInfo[] Value(string boxIdentity, DateTimeT lastReloadTime, string connectionString, string dataMatrixName)
        {
            lock (this)
            {
                Dictionary<string, IComparable> cacheSetting = new Dictionary<string, IComparable>();
                cacheSetting.Add(Database.DatabaseBoxInfo.typeIdentifier + Database.DatabaseBoxInfo.OdbcConnectionStringPropertyName, connectionString);
                cacheSetting.Add(DataMatrix.DataMatrixBoxInfo.typeIdentifier + DataMatrix.DataMatrixBoxInfo.DataMatrixNamePropertyName, dataMatrixName);
                if (IsObsolete(lastReloadTime, cacheSetting))
                    value = Ferda.Modules.Helpers.Data.DataMatrix.Explain(connectionString, dataMatrixName, boxIdentity);
                if (value == null)
                    value = new ColumnSchemaInfo[0];
                return value;
            }
        }
    }
}
