// GenericDataTable.cs - methods for working with data table
//
// Author:  Tomáš Kuchaø <tomas.kuchar@gmail.com>
//          Alexander Kuzmin <alexander.kuzmin@gmail.com> (Virtual column functionality)
//
// Copyright (c) 2007 Tomáš Kuchaø, Alexander Kuzmin
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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Ferda.Modules.Boxes.DataPreparation;

namespace Ferda.Guha.Data
{
    /// <summary>
    /// Provides information about the data table and enumerates
    /// through [derived] columns in the data table.
    /// </summary>
    public class GenericDataTable : IEnumerable<GenericColumn>
    {
        private readonly GenericDatabase _genericDatabase;

        /// <summary>
        /// Gets the parent generic database.
        /// </summary>
        /// <value>The parent generic database.</value>
        public GenericDatabase GenericDatabase
        {
            get { return _genericDatabase; }
        }

        /// <summary>
        /// Gets the type of the data table.
        /// </summary>
        /// <value>The type of the data table.</value>
        public string DataTableType
        {
            get { return _explain.type; }
        }

        private DataTableExplain _explain;

        /// <summary>
        /// Gets the explain.
        /// </summary>
        /// <value>The explain.</value>
        public DataTableExplain Explain
        {
            get
            {
                if (_explain.recordsCount < 0)
                {
                    //prepare command for "SELECT COUNT(1) FROM ..." query
                    DbCommand command = GenericDatabase.CreateDbCommand();
                    command.CommandText = "SELECT COUNT(1) FROM " + GenericDatabase.QuoteQueryIdentifier(_explain.name);
                    _explain.recordsCount = Convert.ToInt64(command.ExecuteScalar());
                }
                return _explain;
            }
        }



        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Ferda.Guha.Data.GenericDataTable"/> class.
        /// </summary>
        /// <param name="genericDatabase">The generic database.</param>
        /// <param name="explain">The explain.</param>
        internal GenericDataTable(GenericDatabase genericDatabase, DataTableExplain explain)
        {
            _genericDatabase = genericDatabase;
            _explain = explain;
        }

        /*
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Ferda.Guha.Data.GenericDataTable"/> class.
        /// </summary>
        /// <param name="genericDatabase">The generic database.</param>
        /// <param name="detailDataTableName"></param>
        /// <param name="joinKey"></param>
        /// /// <param name="explain">The explain.</param>
        internal GenericDataTable(GenericDatabase genericDatabase, string detailDataTableName, string joinKey, DataTableExplain explain)
        {
            _genericDatabase = genericDatabase;
            _detailDataTableName = detailDataTableName;
            _joinKey = joinKey;
            _explain = explain;
        }
        */
        #endregion

        /// <summary>
        /// Gets the quoted columns names i.e.
        /// makes a sequence "`Column1`, `Column2`, `Column3`, ..."
        /// </summary>
        /// <param name="columnsNames">The columns names.</param>
        internal string getQuotedColumnsNames(string[] columnsNames)
        {
            // makes a sequence "`Column1`, `Column2`, `Column3`, ..."
            string subQuery = "";
            foreach (string columnName in columnsNames)
            {
                string columnNameTmp = GenericDatabase.QuoteQueryIdentifier(columnName);
                subQuery = (string.IsNullOrEmpty(subQuery)) ? columnNameTmp : subQuery + ", " + columnNameTmp;
            }
            return subQuery;
        }

        /// <summary>
        /// Tests the unique columns i.e. unicity of values in data in specified
        /// <c>uniqueColumns</c>. (These columns can simulate Primary Key.)
        /// </summary>
        /// <param name="uniqueColumns">The columns to test unicity.</param>
        /// <exception cref="T:Ferda.Modules.BadValueError">
        /// <b>DbUniqueKeyError</b>
        /// Thrown when the specified columns does not satisfy the 
        /// unicity of values in the database.
        /// </exception>
        public void TestUniqueKey(string[] uniqueColumns)
        {
            if (uniqueColumns == null || uniqueColumns.Length == 0)
                throw Exceptions.DbUniqueKeyError(null, null);

            // makes a sequence "`Column1`, `Column2`, `Column3`, ..."
            string subQuery = getQuotedColumnsNames(uniqueColumns);

            //create SQL query " ... GROUP BY PkColumns HAVING COUNT(1) > 1"
            DbCommand myCommand = GenericDatabase.CreateDbCommand();
            myCommand.CommandText = "SELECT " + subQuery + " FROM " +
                                    GenericDatabase.QuoteQueryIdentifier(_explain.name) + " GROUP BY " + subQuery +
                                    " HAVING COUNT(1) > 1";

            DbDataAdapter myDataAdapter = GenericDatabase.CreateDbDataAdapter();
            myDataAdapter.SelectCommand = myCommand;
            DataSet myDataSet = new DataSet();

            //test if SQL query returned any rows
            try
            {
                myDataAdapter.Fill(myDataSet);

                //test uniqueness
                if (myDataSet.Tables[0].Rows.Count > 1)
                    throw Exceptions.DbUniqueKeyError(null, null);
            }
            catch (DbException e)
            {
                //throws innnerException if columns are not in dataMatrix
                throw Exceptions.DbUniqueKeyError(e, null);
            }
        }

        /// <summary>
        /// Determines whether the specified acceptable types is acceptable.
        /// </summary>
        /// <param name="acceptableTypes">The acceptable types.</param>
        /// <returns>
        /// 	<c>true</c> if the specified acceptable types is acceptable; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// Thrown if unknonwn type of table is passed by <c>acceptableTypes</c> parameter.
        /// </exception>
        public bool IsAcceptable(string[] acceptableTypes)
        {
            return IsAcceptable(new List<string>(acceptableTypes));
        }

        /// <summary>
        /// Determines whether the specified acceptable types is acceptable.
        /// </summary>
        /// <param name="acceptableTypes">The acceptable types.</param>
        /// <returns>
        /// 	<c>true</c> if the specified acceptable types is acceptable; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// Thrown if unknonwn type of table is passed by <c>acceptableTypes</c> parameter.
        /// </exception>
        public bool IsAcceptable(List<string> acceptableTypes)
        {
            if (acceptableTypes == null || acceptableTypes.Count == 0)
                return false;

            foreach (string var in acceptableTypes)
            {
                if (!GenericDatabase.TableTypes.Contains(var.ToUpper()))
                    throw new ArgumentOutOfRangeException("acceptableTypes", var, null);
            }

            if (acceptableTypes.Contains(_explain.type.ToUpper()))
                return true;
            return false;
        }

        /// <summary>
        /// Function returns a data from the data table
        /// (equivalent to the "SELECT * FROM tableName" SQL command)
        /// </summary>
        /// <returns>
        /// A DataTable structure filled with the actual data
        /// </returns>
        public DataTable Select()
        {
            try
            {
                //building the command
                DbCommand command = GenericDatabase.CreateDbCommand();
                command.CommandText = "SELECT * FROM " +
                    GenericDatabase.QuoteQueryIdentifier(Explain.name);

                DbDataAdapter dataAdapter = GenericDatabase.CreateDbDataAdapter();
                dataAdapter.SelectCommand = command;
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                return dataSet.Tables[0];
            }
            catch (DbException ex)
            {
                throw Exceptions.DbUnexpectedError(ex, null);
            }
        }

        #region Columns

        private Dictionary<string, GenericColumn> _columns = null;

        /// <summary>
        /// Loads the schema of columns.
        /// </summary>
        /// <exception cref="T:Ferda.Modules.BadParamsError">
        /// <b>DbUnexpectedError</b>
        /// Thrown when the GetSchema() operation is not supperted by
        /// the data table (usually for some "SYSTEM" tables)
        /// </exception>
        private Dictionary<string, GenericColumn> columns
        {
            get
            {
                if (_columns == null)
                {
                    _columns = new Dictionary<string, GenericColumn>();
                    DataTable schema;

                    // There are two ways to get schema information about the column.
                    // Both are described below.

                    // first way
                    // schema = GenericDatabase.GetSchema("COLUMNS", new string[4] { null, null, _explain.name, null });

                    // second (used) way
                    DbCommand command = GenericDatabase.CreateDbCommand();
                    command.CommandText = "SELECT * FROM " + GenericDatabase.QuoteQueryIdentifier(_explain.name) +
                                          " WHERE 0";
                    try
                    {
                        schema = command.ExecuteReader(CommandBehavior.SchemaOnly).GetSchemaTable();
                    }
                    catch (DbException e)
                    {
                        throw Exceptions.DbUnexpectedError(e, null);
                    }

                    foreach (DataRow row in schema.Rows)
                    {
                        #region What can I get?

                        /* Results for 
                         * 
                         *      schema = GenericDatabase.GetSchema("COLUMNS", new string[4] { null, null, _explain.name, null });
                         * 
                         * string COLUMN_NAME
                         * int DATA_TYPE
                         * string TYPE_NAME (DOUBLE, VARCHAR, DATETIME, BIT(==boolean), ...)
                         * int COLUMN_SIZE
                         * int BUFFER_LENGTH
                         * int/null DECIMAL_DIGITS
                         * int/null NUM_PREC_RADIX
                         * 0/1 NULLABLE
                         * st   ring REMARKS
                         * ? COLUMNDEF
                         * int SQL_DATA_TYPE
                         * int/null SQL_DATETIME_SUB
                         * int/null CHAR_OCTET_LENGTH
                         * int ORDINAL_POSITION
                         * YES/NO IS_NULLABLE
                         * int ORDINAL == ORDINAL_POSITION
                         * */

                        /* Results for 
                         * 
                         *      DbCommand command = GenericDatabase.CreateDbCommand();
                         *      command.CommandText = "SELECT * FROM " + GenericDatabase.QuoteQueryIdentifier(_explain.name) + " WHERE 0";
                         *      schema = command.ExecuteReader(CommandBehavior.SchemaOnly).GetSchemaTable();
                         * 
                         * ColumnName
                         * ColumnOrdinal
                         * ColumnSize
                         * NumericPrecision
                         * NumericScale
                         * IsUnique
                         * IsKey
                         * BaseServerName
                         * BaseCatalogName
                         * BaseColumnName
                         * BaseSchemaName
                         * BaseTableName
                         * DataType
                         * AllowDBNull
                         * ProviderType
                         * IsAliased
                         * IsExpression
                         * IsIdentity
                         * IsAutoIncrement
                         * IsRowVersion
                         * IsHidden
                         * IsLong
                         * IsReadOnly
                         * */

                        #endregion

                        ColumnExplain resultItem = new ColumnExplain();
                        resultItem.name = row["ColumnName"].ToString();
                        resultItem.columnOrdinal = Convert.ToInt32(row["ColumnOrdinal"]);
                        resultItem.columnSize = Convert.ToInt32(row["ColumnSize"]);
                        resultItem.numericPrecision = Convert.ToInt32(row["NumericPrecision"]);
                        resultItem.numericScale = Convert.ToInt32(row["NumericScale"]);
                        resultItem.isUnique = Convert.ToBoolean(row["IsUnique"]);
                        resultItem.isKey = Convert.ToBoolean(row["IsKey"]);
                        //BaseServerName
                        //BaseCatalogName
                        //BaseColumnName
                        //BaseSchemaName
                        //BaseTableName
                        resultItem.dataType = GenericColumn.GetDbDataTypeFromFullName(row["DataType"].ToString());
                        resultItem.allowDBNull = Convert.ToBoolean(row["AllowDBNull"]);
                        resultItem.providerType = Convert.ToInt32(row["ProviderType"]);
                        //IsAliased
                        //IsExpression
                        //IsIdentity
                        resultItem.isAutoIncrement = Convert.ToBoolean(row["IsAutoIncrement"]);
                        resultItem.isRowVersion = Convert.ToBoolean(row["IsRowVersion"]);
                        //IsHidden
                        resultItem.isLong = Convert.ToBoolean(row["IsLong"]);
                        resultItem.isReadOnly = Convert.ToBoolean(row["IsReadOnly"]);
                        _columns.Add(resultItem.name, new GenericColumn(this, resultItem, false, false));
                    }
                }
                return _columns;
            }
        }

        private Dictionary<string, GenericColumn> _virtualColumns = null;

        /// <summary>
        /// Virtual columns.
        /// </summary>
        private Dictionary<string, GenericColumn> virtualColumns
        {
            get
            {
                if (_virtualColumns == null)
                {
                    _virtualColumns = new Dictionary<string, GenericColumn>();
                }
                return _virtualColumns;
            }
        }

        /// <summary>
        /// Gets the <see cref="T:Ferda.Guha.Data.GenericColumn"/> with the specified columnName.
        /// </summary>
        /// <value>Generic column i.e. cached information about the column.</value>
        /// <exception cref="T:Ferda.Modules.BadParamsError">
        /// <b>DbDataTableNameError</b>
        /// Thrown when the specified data table name is not found in the database.
        /// </exception>
        /// <exception cref="T:Ferda.Modules.BadParamsError">
        /// <b>DbUnexpectedError</b>
        /// Thrown when the GetSchema() operation is not supperted by
        /// the data table (usually for some "SYSTEM" tables)
        /// </exception>
        public GenericColumn this[string columnName]
        {
            get
            {
                lock (this)
                {
                    if (columns.ContainsKey(columnName))
                        return columns[columnName];
                    else
                        throw Exceptions.DbColumnNameError(null, null);
                }
            }
        }

        /// <summary>
        /// Gets the generic column (even derived).
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="columnInfo">Information about column.</param>
        /// <returns>The generic column.</returns>
        /// <exception cref="T:Ferda.Modules.BadParamsError">
        /// <b>DbColumnNameError</b>
        /// Thrown when the specified columns does is not correct 
        /// SQL query.
        /// </exception>
        public GenericColumn GetGenericColumn(string columnName, ColumnInfo columnInfo)
        {
            GenericColumn result;

            if (columnInfo.columnType == ColumnTypeEnum.SimpleColumn)
            {

                if (columns.TryGetValue(columnName, out result))
                {
                    return result;
                }
                else // requested column is derived and it is not in cached yet
                {
                    AddDerivedColumn(columnName);
                    return columns[columnName];
                }
            }
            else
            {
                if (virtualColumns.TryGetValue(columnName, out result))
                {
                    return result;
                }
                else
                {
                    AddVirtualColumn(columnName, columnInfo.detailTableName, 
                        columnInfo.masterTableIdColumn, columnInfo.detailTableIdColumn);
                    return virtualColumns[columnName];
                }
            }
        }
        /*
        /// <summary>
        /// Gets the virtual column.
        /// </summary>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="detailDataTableName">Name of the detail datatable.</param>
        /// <param name="masterTableIdColumn">Name of the id column of master datatable.</param>
        /// <param name="detailTableIdColumn">Name of the id column of detail datatable.</param>
        /// <returns>The generic column.</returns>
        /// <exception cref="T:Ferda.Modules.BadParamsError">
        /// <b>DbColumnNameError</b>
        /// Thrown when the specified column is not a correct 
        /// SQL query.
        /// </exception>       
        private GenericColumn GetVirtualColumn(string columnName, string detailDataTableName, string masterTableIdColumn, string detailTableIdColumn)
        {
            GenericColumn result;

            if (virtualColumns.TryGetValue(columnName, out result))
            {
                return result;
            }
            else // requested column is derived and it is not in cached yet
            {
                AddVirtualColumn(columnName, detailDataTableName, masterTableIdColumn, detailTableIdColumn);
                return virtualColumns[columnName];
            }
        }*/

        /// <summary>
        /// Gets names of the columns in current data table (derived columns are not
        /// included).
        /// </summary>
        /// <value>The names of columns.</value>
        /// <exception cref="T:Ferda.Modules.BadParamsError">
        /// <b>DbUnexpectedError</b>
        /// Thrown when the GetSchema() operation is not supperted by
        /// the data table (usually for some "SYSTEM" tables)
        /// </exception>
        public string[] BasicColumnsNames
        {
            get
            {
                lock (this)
                {
                    List<string> result = new List<string>();
                    foreach (KeyValuePair<string, GenericColumn> var in columns)
                    {
                        if (!var.Value.IsDerived)
                            result.Add(var.Key);
                    }
                    return result.ToArray();
                }
            }
        }

        #endregion

        #region Derived and virtual Columns

        /// <summary>
        /// Tests the derived column select expression.
        /// </summary>
        /// <param name="columnSelectExpression">The column select expression.</param>
        /// <exception cref="T:Ferda.Modules.BadParamsError">
        /// <b>DbColumnNameError</b>
        /// Thrown when the specified columns does is not correct 
        /// SQL query.
        /// </exception>
        private void testDerivedColumnSelectExpression(string columnSelectExpression)
        {
            try
            {
                DbCommand command = GenericDatabase.CreateDbCommand();
                // TODO possibly security attacs here
                command.CommandText = "SELECT " + columnSelectExpression + " FROM " +
                                      GenericDatabase.QuoteQueryIdentifier(_explain.name) + " WHERE 0";
                command.ExecuteNonQuery();
            }
            catch (DbException e)
            {
                throw Exceptions.DbColumnNameError(e, null);
            }
        }


        /// <summary>
        /// Tests the virtual column select expression
        /// </summary>
        /// <param name="columnSelectExpression">The column select expression.</param>
        /// <param name="detailDataTableName"></param>
        /// <param name="masterDataTableIdColumn"></param>
        /// <param name="detailDataTableIdColumn"></param>
        /// <exception cref="T:Ferda.Modules.BadParamsError">
        /// <b>DbColumnNameError</b>
        /// Thrown when the specified columns does is not correct 
        /// SQL query.
        /// </exception>
        private void testVirtualColumnSelectExpression(string columnSelectExpression,
            string detailDataTableName,
            string masterDataTableIdColumn, string detailDataTableIdColumn)
        {
            try
            {
                DbCommand command = GenericDatabase.CreateDbCommand();
                // TODO possibly security attacs here
                command.CommandText = 
                    "SELECT " + 
                    //GenericDatabase.QuoteQueryIdentifier(detailDataTableName) + 
                      //                "." + 
                                      columnSelectExpression + " FROM " +
                                      GenericDatabase.QuoteQueryIdentifier(detailDataTableName)
                                      + ","
                                      + GenericDatabase.QuoteQueryIdentifier(_explain.name)
                                      + " WHERE "
                                      + GenericDatabase.QuoteQueryIdentifier(_explain.name) + "." 
                                      + GenericDatabase.QuoteQueryIdentifier(masterDataTableIdColumn)
                                      + "="
                                      + GenericDatabase.QuoteQueryIdentifier(detailDataTableName)
                                      + "." + GenericDatabase.QuoteQueryIdentifier(detailDataTableIdColumn);
                command.ExecuteNonQuery();
            }
            catch (DbException e)
            {
                throw Exceptions.DbColumnNameError(e, null);
            }
        }

        /// <summary>
        /// Adds the derived column.
        /// </summary>
        /// <param name="columnSelectExpression">The column select expression.</param>
        /// <exception cref="T:Ferda.Modules.BadParamsError">
        /// <b>DbColumnNameError</b>
        /// Thrown when the specified <c>columnSelectExpression</c> 
        /// is not correct SQL query.
        /// </exception>
        public void AddDerivedColumn(string columnSelectExpression)
        {
            testDerivedColumnSelectExpression(columnSelectExpression);

            GenericColumn column = new GenericColumn(
                this,
                new ColumnExplain(
                    columnSelectExpression,
                    -1,
                    -1,
                    -1,
                    -1,
                    DbDataTypeEnum.UnknownType,
                    -1,
                    true,
                    true,
                    true,
                    false,
                    false,
                    false,
                    false
                    ),
                true,
                false
                );

            _columns.Add(columnSelectExpression, column);
            DbDataTypeEnum dataType =
                column.DetermineDerivedColumnDbDataType();
            ColumnExplain explain = column.Explain;
            explain.dataType = dataType;
            column.Explain = explain;
        }


        /// <summary>
        /// Adds the virtual column.
        /// </summary>
        /// <param name="columnSelectExpression">The column select expression.</param>
        /// <param name="detailDataTableName"></param>
        /// <param name="masterDataTableIdColumn"></param>
        /// <param name="detailDataTableIdColumn"></param>
        /// <exception cref="T:Ferda.Modules.BadParamsError">
        /// <b>DbColumnNameError</b>
        /// Thrown when the specified <c>columnSelectExpression</c> 
        /// is not correct SQL query.
        /// </exception>
        public void AddVirtualColumn(string columnSelectExpression, string detailDataTableName,
            string masterDataTableIdColumn, string detailDataTableIdColumn)
        {
            lock (this)
            {
                testVirtualColumnSelectExpression(columnSelectExpression,
                    detailDataTableName, masterDataTableIdColumn, detailDataTableIdColumn);

                GenericColumn column = new GenericColumn(
                    this,
                    new ColumnExplain(
                        columnSelectExpression,
                        -1,
                        -1,
                        -1,
                        -1,
                        DbDataTypeEnum.UnknownType,
                        -1,
                        true,
                        true,
                        true,
                        false,
                        false,
                        false,
                        false
                        ),
                    false,
                    true
                    );

                _virtualColumns.Add(columnSelectExpression, column);
               
                column.DetailDataTableName = detailDataTableName;
                column.DetailTableIdColumn = detailDataTableIdColumn;
                column.MasterTableIdColumn = masterDataTableIdColumn;

                DbDataTypeEnum dataType =
                    column.DetermineVirtualColumnDbDataType(detailDataTableName, masterDataTableIdColumn, detailDataTableIdColumn);
                ColumnExplain explain = column.Explain;
                explain.dataType = dataType;
                column.Explain = explain;
                
            }
        }

        /// <summary>
        /// Removes the derived column.
        /// </summary>
        /// <param name="columnSelectExpression">The column select expression.</param>
        /// <exception cref="T:Ferda.Modules.BadParamsError">
        /// <b>DbUnexpectedError</b>
        /// Thrown when the GetSchema() operation is not supperted by
        /// the data table (usually for some "SYSTEM" tables)
        /// </exception>
        public void RemoveDerivedColumn(string columnSelectExpression)
        {
            if (!columns.ContainsKey(columnSelectExpression))
                return;
            else if (columns[columnSelectExpression].IsDerived)
                columns.Remove(columnSelectExpression);
            else
                throw new InvalidOperationException();
        }

        #endregion

        #region IEnumerable<GenericColumn> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
        /// </returns>
        /// <exception cref="T:Ferda.Modules.BadParamsError">
        /// <b>DbUnexpectedError</b>
        /// Thrown when the GetSchema() operation is not supperted by
        /// the data table (usually for some "SYSTEM" tables)
        /// </exception>
        public IEnumerator<GenericColumn> GetEnumerator()
        {
            return columns.Values.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
        /// </returns>
        /// <exception cref="T:Ferda.Modules.BadParamsError">
        /// <b>DbUnexpectedError</b>
        /// Thrown when the GetSchema() operation is not supperted by
        /// the data table (usually for some "SYSTEM" tables)
        /// </exception>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return columns.Values.GetEnumerator();
        }

        #endregion
    }
}
