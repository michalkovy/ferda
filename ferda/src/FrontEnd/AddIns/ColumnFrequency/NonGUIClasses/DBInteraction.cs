using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Modules.Boxes.DataMiningCommon.Column;
using Ferda.Modules;
using System.Data.Odbc;
using System.Data;
using System.Resources;

using System.Reflection;

namespace Ferda
{
    namespace FrontEnd.AddIns.ColumnFrequency.NonGUIClasses
    {
        /// <summary>
        /// Class for interaction with ODBC connection
        /// </summary>
        public class DBInteraction
        {
            #region Private variables
            private String connectionString;

            private String columnSelectExpression;

            private String dataMatrixName;

            private OdbcConnection connection;

            private long rowCount;

            private ResourceManager rm;
            #endregion


            #region Contructor
            public DBInteraction(ColumnStruct columnStruct, ResourceManager rm)
            {
                this.connectionString = columnStruct.dataMatrix.database.connectionString;

                this.columnSelectExpression = columnStruct.columnSelectExpression;

                this.dataMatrixName = columnStruct.dataMatrix.dataMatrixName;

                this.rowCount = columnStruct.dataMatrix.recordsCount;

                this.rm = rm;

                this.connection = this.GetConnection();

            }
            #endregion


            #region Initializing ODBC connection
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
            /// Method which composes the ODBC SQL query to get the table
            /// with distinct values and their counts.
            /// </summary>
            /// <returns>Query string.</returns>
            private string GetValueCountQuery()
            {
                return "SELECT "
                + "`" + this.columnSelectExpression + "`" + " AS `Value`"
                + ", COUNT(" + this.columnSelectExpression + ") AS `Cnt`"
                + " FROM " + this.dataMatrixName
                + " GROUP BY " + this.columnSelectExpression
                + " ORDER BY " + this.columnSelectExpression;
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
                    DataTable returnTable = new DataTable();
                    returnTable = this.GetQueryResultTable(this.GetValueCountQuery());
                    this.connection.Close();
                    return returnTable;
                }
            }

            /// <summary>
            /// Method for changing the localization manager.
            /// </summary>
            /// <param name="rm"></param>
            public void ChangeRm(ResourceManager rm)
            {
                this.rm = rm;
            }

            #endregion
        }
    }
}