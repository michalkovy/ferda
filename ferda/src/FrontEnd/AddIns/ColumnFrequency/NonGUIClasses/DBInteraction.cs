// DBInteraction.cs - class interacts with ODBC database
//
// Author: Alexander Kuzmin <alexander.kuzmin@gmail.com>
//
// Copyright (c) 2005 Alexander Kuzmin
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
using Ferda.Modules.Boxes.DataMiningCommon.Column;
using Ferda.Modules;
using System.Data.Odbc;
using System.Data;
using System.Resources;

using System.Reflection;

namespace Ferda.FrontEnd.AddIns.ColumnFrequency.NonGUIClasses
{
    /// <summary>
    /// Class for interaction with ODBC connection
    /// </summary>
    public class DBInteraction
    {
        #region Private variables

        /// <summary>
        /// Connection string
        /// </summary>
        private String connectionString;

        /// <summary>
        /// Column selecet expression
        /// </summary>
        private String columnSelectExpression;

        /// <summary>
        /// Datamatrix name
        /// </summary>
        private String dataMatrixName;

        /// <summary>
        /// Datamatrix row count
        /// </summary>
        private long rowCount;

        #endregion


        #region Contructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="columnInfo">Columninfo</param>
        public DBInteraction(ColumnInfo columnInfo)
        {
            this.connectionString = columnInfo.dataMatrix.database.odbcConnectionString;
            this.columnSelectExpression = columnInfo.columnSelectExpression;
            this.dataMatrixName = columnInfo.dataMatrix.dataMatrixName;
            this.rowCount = columnInfo.dataMatrix.recordsCount;
        }

        #endregion


        #region Public methods
        /// <summary>
        /// Method which returns the DataTable with (value,count) records.
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllValuesCount()
        {
            if (this.rowCount == 0)
            {
                return new DataTable();
            }
            else
            {
                DataTable returnTable = Ferda.Modules.Helpers.Data.Column.GetDistinctsAndFrequencies(
                    this.connectionString,
                    this.dataMatrixName,
                    this.columnSelectExpression,
                    null);
                return returnTable;
            }
        }

        #endregion
    }
}