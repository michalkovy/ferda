// DBInteraction.cs - class for interaction with ODBC database
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
using System.Data.Odbc;
using System.Data;
using Ferda.Modules.Boxes.DataMiningCommon.DataMatrix;

namespace Ferda.FrontEnd.AddIns.ShowTable.NonGUIClasses
{
    /// <summary>
    /// Class for interaction with ODBC database
    /// </summary>
    public class DBInteraction
    {
        #region Private variables

        /// <summary>
        /// Database connection string
        /// </summary>
        private string connectionString;

        /// <summary>
        /// Datamatrix name
        /// </summary>
        private string dataMatrixName;

        /// <summary>
        /// ODBC connection
        /// </summary>
        private OdbcConnection connection;

        #endregion


        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="dataMatrixName">Datamatrix name</param>
        /// <param name="dataMatrixStruct">Datamatrix struct</param>
        public DBInteraction(string dataMatrixName, DataMatrixInfo dataMatrixStruct)
        {
            this.connectionString = dataMatrixStruct.database.odbcConnectionString;
            this.dataMatrixName = dataMatrixName;
            this.connection = this.GetConnection();

        }

        #endregion


        #region Initializing ODBC connection

        /// <summary>
        /// Initializing ODBC conenction
        /// </summary>
        /// <returns>ODBC connection</returns>
        public OdbcConnection GetConnection()
        {
            try
            {
                return new System.Data.Odbc.OdbcConnection(this.connectionString);
            }

            catch (OdbcException e)
            {
                throw Ferda.Modules.Exceptions.BadParamsError(e, null, "Bad ODBC connection string specified. Could not connect to database.", Ferda.Modules.restrictionTypeEnum.DbConnectionString);
            }
        }

        #endregion


        #region Private methods

        /// <summary>
        /// Method to compose query for showing the whole table
        /// </summary>
        /// <returns></returns>
        private string ComposeShowTableQuery()
        {
            return "SELECT * FROM "
            + "`" + this.dataMatrixName + "`";
        }

        /// <summary>
        /// Method which queries the ODBC connection for a given query.
        /// </summary>
        /// <param name="query"></param>
        /// <returns>DataTable with result</returns>
        private DataTable GetQueryResultTable(string query)
        {
            try
            {
                OdbcDataAdapter odbcDataAdapter = new OdbcDataAdapter(query, this.connection);

                DataSet dataSet = new DataSet();
                odbcDataAdapter.Fill(dataSet);

                return dataSet.Tables[0];
            }

            catch (OdbcException)
            {
                this.connection.Close();
                throw (new Ferda.Modules.BadParamsError());

            }
            catch
            {
                this.connection.Close();
                throw (new Ferda.Modules.BadParamsError());
            }
        }

        #endregion


        #region Public methods

        /// <summary>
        /// Gets table
        /// </summary>
        /// <returns>Resulting datatable</returns>
        public DataTable ShowTable()
        {
            this.GetConnection();
            return this.GetQueryResultTable(this.ComposeShowTableQuery());
        }

        #endregion
    }
}
