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

            private long rowCount;

            private ResourceManager rm;
            #endregion


            #region Contructor
            public DBInteraction(ColumnInfo columnInfo, ResourceManager rm)
            {
                this.connectionString = columnInfo.dataMatrix.database.odbcConnectionString;

                this.columnSelectExpression = columnInfo.columnSelectExpression;

                this.dataMatrixName = columnInfo.dataMatrix.dataMatrixName;

                this.rowCount = columnInfo.dataMatrix.recordsCount;

                this.rm = rm;
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