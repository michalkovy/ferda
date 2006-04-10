// RecordsCountCahce.cs - cache for number of records in data matrix
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

namespace Ferda.Modules.Boxes.DataMiningCommon.DataMatrix
{
    /// <summary>
    /// Cache for count of all records in the data matrix.
    /// </summary>
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
                {
                    value = 0;
                    value = Ferda.Modules.Helpers.Data.DataMatrix.GetRecordsCount(connectionString, dataMatrixName, boxIdentity);
                }
                return value;
            }
        }
    }
}
