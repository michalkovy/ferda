using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Odbc;
using System.Data;
using Ferda.Modules.Boxes.DataMiningCommon.DataMatrix;

namespace Ferda.FrontEnd.AddIns.ShowTable.NonGUIClasses
{
    public class DBInteraction
    {
        #region Private variables
        private string connectionString;

        private string dataMatrixName;

        private OdbcConnection connection;

        #endregion


        #region Contructor
        public DBInteraction(string dataMatrixName, DataMatrixInfo dataMatrixStruct)
        {
            this.connectionString = dataMatrixStruct.database.odbcConnectionString;

            this.dataMatrixName = dataMatrixName;

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

        public DataTable ShowTable()
        {
            this.GetConnection();
            return this.GetQueryResultTable(this.ComposeShowTableQuery());
        }

        #endregion
    }
}
