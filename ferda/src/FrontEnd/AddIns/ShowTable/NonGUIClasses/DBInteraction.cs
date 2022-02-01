// DBInteraction.cs - class for interaction with ODBC database
//
// Authors: Alexander Kuzmin <alexander.kuzmin@gmail.com>
//          Martin Ralbovský <martin.ralbovsky@gmail.com>            
//
// Copyright (c) 2005 Alexander Kuzmin, Martin Ralbovský
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
using System.Data.Odbc;
using System.Data;
using Ferda.Modules.Boxes.DataPreparation;
using Ferda.Guha.Data;

namespace Ferda.FrontEnd.AddIns.ShowTable.NonGUIClasses
{
    /// <summary>
    /// Class for interaction with ODBC database
    /// </summary>
    public class DBInteraction
    {
        #region Private variables

        /// <summary>
        /// Structure that holds information about the data table
        /// </summary>
        private DataTableInfo dataTableStruct;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor for the class
        /// </summary>
        /// <param name="dataTableStruct">
        /// Structure that holds information about the data table
        /// </param>
        public DBInteraction(DataTableInfo dataTableStruct)
        {
            this.dataTableStruct = dataTableStruct;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Gets the actual data of the table
        /// </summary>
        /// <returns>Resulting datatable</returns>
        public DataTable ShowTable()
        {
            DatabaseConnectionSettingHelper helper =
                new DatabaseConnectionSettingHelper(dataTableStruct.databaseConnectionSetting);
            
            //testing the database
            GenericDatabase db = GenericDatabaseCache.GetGenericDatabase(helper);

            //getting the result
            string tableName = dataTableStruct.dataTableName;
            GenericDataTable table = db[dataTableStruct.dataTableName];
            DataTable result = table.Select();

            return result;
        }

        #endregion
    }
}
