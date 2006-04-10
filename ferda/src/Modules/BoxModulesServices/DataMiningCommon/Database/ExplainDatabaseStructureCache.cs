// ExplainDatabaseStructureCache.cs - cache for (like explain) database structure description
//
// Author: Tomáš Kuchař <tomas.kuchar@gmail.com>
//
// Copyright (c) 2005 Tomáš Kuchař
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
using System.Text;

namespace Ferda.Modules.Boxes.DataMiningCommon.Database
{
    /// <summary>
    /// Cache for database structure (like EXPLAIN query).
    /// </summary>
    public class ExplainDatabaseStructureCache : Ferda.Modules.Helpers.Caching.Cache
    {
        private DataMatrixSchemaInfo[] value = new DataMatrixSchemaInfo[0];

        /// <summary>
        /// Gets infos about tables in specified data source.
        /// </summary>
        /// <param name="boxIdentity">The box identity.</param>
        /// <param name="lastReloadTime">The last reload time (for force reload of the cached value).</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="acceptableTypesOfTables">The acceptable types of tables.</param>
        /// <returns></returns>
        public DataMatrixSchemaInfo[] Value(string boxIdentity, DateTimeT lastReloadTime, string connectionString, string[] acceptableTypesOfTables)
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
                    value = new DataMatrixSchemaInfo[0];
                    value = Ferda.Modules.Helpers.Data.Database.Explain(connectionString, acceptableTypesOfTables, boxIdentity);
                }
                if (value == null)
                    value = new DataMatrixSchemaInfo[0];
                return value;
            }
        }
    }
}
