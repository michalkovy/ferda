// GenericColumn.cs - methods for working with data column
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
using System.Data;
using System.Data.Common;
using System.Diagnostics;

namespace Ferda.Guha.Data
{
    /// <summary>
    /// Provides information about the column even schema, statistics, 
    /// possibly cardinality and data type discussion. Please note
    /// that the column can be derived see <see cref="P:Ferda.Guha.Data.GenericColumn.IsDerived"/>.
    /// </summary>
    public class GenericColumn
    {
        private readonly GenericDataTable _genericDataTable;

        /// <summary>
        /// Gets the generic data table.
        /// </summary>
        /// <value>The generic data table.</value>
        public GenericDataTable GenericDataTable
        {
            get { return _genericDataTable; }
        }

        private ColumnExplain _explain;

        /// <summary>
        /// Gets the explain.
        /// </summary>
        /// <value>The explain.</value>
        public ColumnExplain Explain
        {
            get { return _explain; }
            internal set { _explain = value; }
        }

        private readonly bool _isDerived;

        /// <summary>
        /// Gets a value indicating whether this instance is derived.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is derived; otherwise, <c>false</c>.
        /// </value>
        public bool IsDerived
        {
            get { return _isDerived; }
        }

        private readonly bool _isVirtual;

        /// <summary>
        /// Gets a value indicating whether this instance is virtual.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is virtual; otherwise, <c>false</c>.
        /// </value>
        public bool IsVirtual
        {
            get { return _isVirtual; }
        }

        private string _detailDataTableName;

        /// <summary>
        /// Detail DataTable name
        /// </summary>
        public string DetailDataTableName
        {
            get { return _detailDataTableName; }
            set { _detailDataTableName=value; }
        }

        private string _masterTableIdColumn = String.Empty;

        /// <summary>
        /// Master table id column
        /// </summary>
        public string MasterTableIdColumn
        {
            get { return _masterTableIdColumn; }
            set { _masterTableIdColumn=value; }
        }

        private string _detailTableIdColumn = String.Empty;

        /// <summary>
        /// Detail table id column
        /// </summary>
        public string DetailTableIdColumn
        {
            get { return _detailTableIdColumn; }
            set { _detailTableIdColumn = value; }
        }


        private ColumnStatistics _statistics = null;

        /// <summary>
        /// Gets the statistics.
        /// </summary>
        /// <value>The statistics.</value>
        /// <exception cref="T:Ferda.Modules.BadParamsError">
        /// <b>DbUnexpectedError</b>
        /// Thrown when the specifiec column does not support
        /// basic SQL queries (DISTINCT, MIN, MAX, ...) e.g.
        /// the column is of type "BLOB", "Memo", ... (often
        /// DbSimpleDataTypeEnum.UnknownSimpleType)
        /// </exception>
        public ColumnStatistics Statistics
        {
            get
            {
                lock (this)
                {
                    if (_statistics == null)
                    {
                        ColumnStatistics result = new ColumnStatistics();

                        DbCommand command = GenericDataTable.GenericDatabase.CreateDbCommand();

                        string columnQuotedIdentifier;
                        string dataTableQuotedIdentifier;
                        string whereQuotedIdentifier = "1";
                        if(!IsVirtual)
                        {
                            dataTableQuotedIdentifier = GenericDataTable.GenericDatabase.QuoteQueryIdentifier(GenericDataTable.Explain.name);
                            columnQuotedIdentifier = GetQuotedQueryIdentifier();
                        }
                        else
                        {

                            columnQuotedIdentifier = 
                               // GenericDataTable.GenericDatabase.QuoteQueryIdentifier(
                             //   DetailDataTableName)
                            //    + "." + 
                                GetQuotedQueryIdentifier();


                            dataTableQuotedIdentifier = GenericDataTable.GenericDatabase.QuoteQueryIdentifier(DetailDataTableName)
                                + ","
                                + GenericDataTable.GenericDatabase.QuoteQueryIdentifier(GenericDataTable.Explain.name);
               
                            whereQuotedIdentifier =
                                GenericDataTable.GenericDatabase.QuoteQueryIdentifier(DetailDataTableName)
                                + "."
                                + GenericDataTable.GenericDatabase.QuoteQueryIdentifier(DetailTableIdColumn)
                                + "="
                                + GenericDataTable.GenericDatabase.QuoteQueryIdentifier(GenericDataTable.Explain.name)
                                + "."
                                + GenericDataTable.GenericDatabase.QuoteQueryIdentifier(MasterTableIdColumn);
         
                        }

                        try
                        {
                            #region Count of DISTINCTs

                            {
                                command.CommandText =
                                    "SELECT DISTINCT " + columnQuotedIdentifier + " FROM " 
                                    + dataTableQuotedIdentifier + " WHERE " + whereQuotedIdentifier;

                                DbDataAdapter dataAdapter = GenericDataTable.GenericDatabase.CreateDbDataAdapter();
                                dataAdapter.SelectCommand = command;
                                DataSet dataSet = new DataSet();
                                dataAdapter.Fill(dataSet);
                                result.valueDistincts = Convert.ToInt64(dataSet.Tables[0].Rows.Count);

                                /* much more effective but unsupported 
                                 * UNDONE tohle predelat
                                odbcCommand.CommandText = "SELECT COUNT(DISTINCT " + SqlSecurity.ColumnQuote + columnSelectExpression + SqlSecurity.ColumnQuote + ") FROM " + "`" + dataMatrixName + "`";
                                result.ValueDistincts = Convert.ToInt64(odbcCommand.ExecuteScalar());
                                 * */
                            }

                            #endregion

                            #region Min, Max, Avg[Len]

                            {
                                string selectAvgExpression;
                                if (Explain.dataType == DbDataTypeEnum.StringType)
                                    selectAvgExpression = "AVG(LEN(" + columnQuotedIdentifier + ")) AS Average";
                                else if (PotentiallyCardinality == CardinalityEnum.Cardinal
                                    || PotentiallyCardinality == CardinalityEnum.OrdinalCyclic
                                    || PotentiallyCardinality == CardinalityEnum.Ordinal
                                    || Explain.dataType == DbDataTypeEnum.BooleanType)
                                    selectAvgExpression = "AVG(" + columnQuotedIdentifier + ") AS Average";

                                else //if (Explain.dataType == DbDataTypeEnum.UnknownType)
                                    selectAvgExpression = "'N/A' AS Average";

                                command.CommandText = "SELECT "
                                                      + "MAX(" + columnQuotedIdentifier + ") AS Maximum, "
                                                      + "MIN(" + columnQuotedIdentifier + ") AS Minimum, "
                                                      + selectAvgExpression
                                                      + " FROM " + dataTableQuotedIdentifier;

                                DbDataAdapter dataAdapter = GenericDataTable.GenericDatabase.CreateDbDataAdapter();
                                dataAdapter.SelectCommand = command;
                                DataSet dataSet = new DataSet();
                                dataAdapter.Fill(dataSet);
                                Debug.Assert(dataSet.Tables[0].Rows.Count == 1,
                                             "Unexpected count of returned rows from select query.");
                                DataRow dataRow = dataSet.Tables[0].Rows[0];
                                result.valueMax = dataRow["Maximum"].ToString();
                                result.valueMin = dataRow["Minimum"].ToString();
                                result.valueAverage = dataRow["Average"].ToString();
                            }

                            #endregion

                            #region Variability, Standard deviation

                            if (IsNumericDataType)
                            {
                                long dataTableRowsCount = GenericDataTable.Explain.recordsCount;

                                //UNDONE optimize this
                                command.CommandText =
                                    "SELECT SUM( "
                                    + "(" + columnQuotedIdentifier + " - '" + result.valueAverage + "')"
                                    + " * (" + columnQuotedIdentifier + " - '" + result.valueAverage + "')"
                                    + ") FROM " + dataTableQuotedIdentifier;

                                result.valueVariability = Convert.ToDouble(command.ExecuteScalar()) / dataTableRowsCount;
                                result.valueStandardDeviation = System.Math.Sqrt(result.valueVariability);
                            }
                            else
                            {
                                result.valueVariability = double.NaN;
                                result.valueStandardDeviation = double.NaN;
                            }

                            #endregion
                        }
                        catch (DbException ex)
                        {
                            throw Exceptions.DbUnexpectedError(ex, null);
                        }
                        _statistics = result;
                    }
                    return _statistics;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Ferda.Guha.Data.GenericColumn"/> class.
        /// </summary>
        /// <param name="genericDataTable">The generic data table.</param>
        /// <param name="explain">The explain.</param>
        /// <param name="isDerived">if set to <c>true</c> [is derived].</param>
        /// <param name="isVirtual">if set to <c>true</c> [is virtual].</param>
        internal GenericColumn(GenericDataTable genericDataTable, ColumnExplain explain, bool isDerived, bool isVirtual)
        {
            _genericDataTable = genericDataTable;
            _explain = explain;
            _isDerived = isDerived;
            _isVirtual = isVirtual;
        }

        /// <summary>
        /// Changes the type of the db data.
        /// </summary>
        /// <param name="dbDataType">Type of the db data.</param>
        /// <exception cref="T:System.InvalidOperationException">
        /// Thrown if method is called on not derived column.
        /// </exception>
        public void ChangeDbDataType(DbDataTypeEnum dbDataType)
        {
            if (IsDerived)
                _explain.dataType = dbDataType;
            else
                throw new InvalidOperationException();
        }

        #region Try to test selected dbDataType on [derived] column

        /// <summary>
        /// Tries the get value from db value.
        /// </summary>
        /// <param name="dbValue">The db value.</param>
        /// <param name="dbDataType">Type of the db data.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="T:System.InvalidOperationException">
        /// Thrown iff <c>dbDataType</c> is unknown.
        /// </exception>
        /// <exception cref="T:System.InvalidCastException">
        /// Thrown if casting on specified <c>dbDataType</c> was unsuccessful.
        /// </exception>
        /// <exception cref="T:System.OverflowException">Thrown when converting faild.</exception>
        /// <exception cref="T:System.FormatException">Thrown when converting faild.</exception>
        /// <exception cref="T:System.ArgumentNullException">Thrown when converting faild.</exception>
        /// <remarks>
        /// For <c>DbDataTypeEnum.TimeType</c> returns <c>TimeSpan</c> (time of day) not <c>DateTime</c>.
        /// </remarks>
        /// <returns>false if parsed value is DBNull.Value; otherwise, return true</returns>
        public static bool ParseValue(object dbValue, DbDataTypeEnum dbDataType, out IComparable value)
        {
            if (dbValue == DBNull.Value
                || dbValue.ToString() == Ferda.Guha.Data.nullValueConstant.value)
            {
                value = null;
                return false;
            }
            else
            {
                switch (dbDataType)
                {
                    case DbDataTypeEnum.BooleanType:
                        value = Convert.ToBoolean(dbValue);
                        break;
                    case DbDataTypeEnum.DateTimeType:
                        value = Convert.ToDateTime(dbValue);
                        break;
                    case DbDataTypeEnum.DecimalType:
                        value = Convert.ToDecimal(dbValue);
                        break;
                    case DbDataTypeEnum.DoubleType:
                        value = Convert.ToDouble(dbValue);
                        break;
                    case DbDataTypeEnum.FloatType:
                        value = Convert.ToSingle(dbValue);
                        break;
                    case DbDataTypeEnum.IntegerType:
                        value = Convert.ToInt32(dbValue);
                        break;
                    case DbDataTypeEnum.LongIntegerType:
                        value = Convert.ToInt64(dbValue);
                        break;
                    case DbDataTypeEnum.ShortIntegerType:
                        value = Convert.ToInt16(dbValue);
                        break;
                    case DbDataTypeEnum.StringType:
                        value = Convert.ToString(dbValue);
                        break;
                    case DbDataTypeEnum.TimeType:
                        // UNDONE, FIX possibly mistake
                        value = (Convert.ToDateTime(dbValue)).TimeOfDay;
                        break;
                    case DbDataTypeEnum.UnsignedIntegerType:
                        value = Convert.ToUInt32(dbValue);
                        break;
                    case DbDataTypeEnum.UnsignedLongIntegerType:
                        value = Convert.ToUInt64(dbValue);
                        break;
                    case DbDataTypeEnum.UnsignedShortIntegerType:
                        value = Convert.ToUInt16(dbValue);
                        break;
                    case DbDataTypeEnum.UnknownType:
                        throw new InvalidOperationException();
                    default:
                        throw new NotImplementedException();
                }
                return true;
            }
        }

        /*
        /// <summary>
        /// Tries the get value from db value.
        /// </summary>
        /// <param name="dbValue">The db value.</param>
        /// <param name="dbDataType">Type of the db data.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="T:System.InvalidOperationException">
        /// Thrown iff <c>dbDataType</c> is unknown.
        /// </exception>
        /// <exception cref="T:System.InvalidCastException">
        /// Thrown if casting on specified <c>dbDataType</c> was unsuccessful.
        /// </exception>
        /// <exception cref="T:System.OverflowException">Thrown when converting faild.</exception>
        /// <exception cref="T:System.FormatException">Thrown when converting faild.</exception>
        /// <exception cref="T:System.ArgumentNullException">Thrown when converting faild.</exception>
        /// <remarks>
        /// For <c>DbSimpleDataTypeEnum.TimeSimpleType</c> returns <c>TimeSpan</c> (time of day) not <c>DateTime</c>.
        /// </remarks>
        public static bool TryParseValue(object dbValue, DbSimpleDataTypeEnum dbDataType, out IComparable value)
        {
            if (dbValue == DBNull.Value)
            {
                value = null;
                return false;
            }
            else
            {
                switch (dbDataType)
                {
                    case DbSimpleDataTypeEnum.BooleanSimpleType:
                        value = Convert.ToBoolean(dbValue);
                        break;
                    case DbSimpleDataTypeEnum.DateTimeSimpleType:
                        value = Convert.ToDateTime(dbValue);
                        break;
                    case DbSimpleDataTypeEnum.DoubleSimpleType:
                        // decimal or double
                        value = Convert.ToDouble(dbValue);
                        break;
                    case DbSimpleDataTypeEnum.FloatSimpleType:
                        value = Convert.ToSingle(dbValue);
                        break;
                    case DbSimpleDataTypeEnum.ShortSimpleType:
                        value = Convert.ToInt16(dbValue);
                        break;
                    case DbSimpleDataTypeEnum.IntegerSimpleType:
                        // uint16 or int32
                        value = Convert.ToInt32(dbValue);
                        break;
                    case DbSimpleDataTypeEnum.LongSimpleType:
                        // uint32, int64, uint64
                        value = Convert.ToInt64(dbValue);
                        break;
                    case DbSimpleDataTypeEnum.StringSimpleType:
                        value = Convert.ToString(dbValue);
                        break;
                    case DbSimpleDataTypeEnum.TimeSimpleType:
                        // TODO possibly mistake
                        value = (Convert.ToDateTime(dbValue)).TimeOfDay;
                        break;
                    case DbSimpleDataTypeEnum.UnknownSimpleType:
                        throw new InvalidOperationException();
                    default:
                        throw new NotImplementedException();
                }
                return true;
            }
        }

        /// <summary>
        /// Tests the db data type of the derived column.
        /// </summary>
        /// <param name="dbDataType">Type of the db data.</param>
        /// <exception cref="T:System.InvalidOperationException">
        /// Thrown iff <c>dbDataType</c> is unknown.
        /// </exception>
        /// <exception cref="T:System.InvalidCastException">
        /// Thrown if casting on specified <c>dbDataType</c> was unsuccessful.
        /// </exception>
        /// <exception cref="T:System.OverflowException">Thrown when converting faild.</exception>
        /// <exception cref="T:System.FormatException">Thrown when converting faild.</exception>
        /// <exception cref="T:System.ArgumentNullException">Thrown when converting faild.</exception>
        public void TestDerivedColumnDbDataType(DbDataTypeEnum dbDataType)
        {
            string columnQuotedIdentifier = GetQuotedQueryIdentifier();
            string dataTableQuotedIdentifier =
                GenericDataTable.GenericDatabase.QuoteQueryIdentifier(GenericDataTable.Explain.name);

            DbCommand command = GenericDataTable.GenericDatabase.CreateDbCommand();

            command.CommandText =
                "SELECT DISTINCT " + columnQuotedIdentifier
                + " FROM " + dataTableQuotedIdentifier;

            DataTable data;
            try
            {
                DbDataAdapter dataAdapter = GenericDataTable.GenericDatabase.CreateDbDataAdapter();
                dataAdapter.SelectCommand = command;
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                data = dataSet.Tables[0];
            }
            catch (DbException ex)
            {
                throw Exceptions.DbUnexpectedError(ex, null);
            }

            try
            {
                object dbValue;
                object value;
                foreach (DataRow var in data.Rows)
                {
                    dbValue = var[0];
                    TryParseValue(dbValue, dbDataType, out value);
                }
            }
            catch (InvalidCastException)
            {
                throw;
            } // means that the dbDataType is not good
            catch (OverflowException)
            {
                throw;
            } // means that the dbDataType is not enough
            catch (FormatException)
            {
                throw;
            } // menas that the dbDataType is not good
            catch (ArgumentNullException)
            {
                throw;
            } // this should be never thrown
            catch (InvalidOperationException)
            {
                throw;
            } // specified dbDataType is unknown
        }
        */

        #endregion

        #region DbDataType, potentially cardinality nominal/ordinal/cardinal, is numeric

        /// <summary>
        /// Determines the db datat type of the [derived] column.
        /// </summary>
        public DbDataTypeEnum DetermineDerivedColumnDbDataType()
        {
            string columnQuotedIdentifier = GetQuotedQueryIdentifier();
            string dataTableQuotedIdentifier =
                GenericDataTable.GenericDatabase.QuoteQueryIdentifier(GenericDataTable.Explain.name);

            DbCommand command = GenericDataTable.GenericDatabase.CreateDbCommand();

            command.CommandText =
                String.Format("SELECT DISTINCT {0} FROM {1}", columnQuotedIdentifier, dataTableQuotedIdentifier);

            try
            {
                DbDataReader reader = command.ExecuteReader();
                Type type = reader.GetProviderSpecificFieldType(0);
                DbDataTypeEnum result = GetDbDataTypeFromFullName(type.FullName);
                return result;
            }
            catch (DbException ex)
            {
                throw Exceptions.DbUnexpectedError(ex, null);
            }
        }

        /// <summary>
        /// Determines the db data type of the virtual column.
        /// </summary>
        public DbDataTypeEnum DetermineVirtualColumnDbDataType(string detailTableName, string masterTableIdColumn, string detailTableIdColumn)
        {
            string columnQuotedIdentifier = 
                //GenericDataTable.GenericDatabase.QuoteQueryIdentifier(detailTableName)
              //  + "." + 
                GetQuotedQueryIdentifier();
            string dataTableQuotedIdentifier =
                GenericDataTable.GenericDatabase.QuoteQueryIdentifier(detailTableName)
            + ","
            + GenericDataTable.GenericDatabase.QuoteQueryIdentifier(GenericDataTable.Explain.name);

            string whereQuotedIdentifier =
                GenericDataTable.GenericDatabase.QuoteQueryIdentifier(detailTableName)
            + "."
            + GenericDataTable.GenericDatabase.QuoteQueryIdentifier(detailTableIdColumn)
            + "="
            + GenericDataTable.GenericDatabase.QuoteQueryIdentifier(GenericDataTable.Explain.name)
            + "."
            + GenericDataTable.GenericDatabase.QuoteQueryIdentifier(masterTableIdColumn);

            DbCommand command = GenericDataTable.GenericDatabase.CreateDbCommand();

            command.CommandText =
                String.Format("SELECT DISTINCT {0} FROM {1} WHERE {2}", columnQuotedIdentifier, dataTableQuotedIdentifier, whereQuotedIdentifier);

            try
            {
                DbDataReader reader = command.ExecuteReader();
                Type type = reader.GetProviderSpecificFieldType(0);
                DbDataTypeEnum result = GetDbDataTypeFromFullName(type.FullName);
                return result;
            }
            catch (DbException ex)
            {
                throw Exceptions.DbUnexpectedError(ex, null);
            }
        }

        /// <summary>
        /// Gets the type of the db data.
        /// </summary>
        /// <param name="fullNameOfDataType">The full name of .NET data type.</param>
        public static DbDataTypeEnum GetDbDataTypeFromFullName(string fullNameOfDataType)
        {
            switch (fullNameOfDataType)
            {
                case "System.Int16":
                    return DbDataTypeEnum.ShortIntegerType;
                case "System.UInt16":
                    return DbDataTypeEnum.UnsignedShortIntegerType;
                case "System.Int32":
                    return DbDataTypeEnum.IntegerType;
                case "System.UInt32":
                    return DbDataTypeEnum.UnsignedIntegerType;
                case "System.Int64":
                    return DbDataTypeEnum.LongIntegerType;
                case "System.UInt64":
                    return DbDataTypeEnum.UnsignedLongIntegerType;
                case "System.Single":
                    return DbDataTypeEnum.FloatType;
                case "System.Double":
                    return DbDataTypeEnum.DoubleType;
                case "System.Decimal":
                    return DbDataTypeEnum.DecimalType;
                case "System.Boolean":
                    return DbDataTypeEnum.BooleanType;
                case "System.String":
                    return DbDataTypeEnum.StringType;
                case "System.DateTime":
                    return DbDataTypeEnum.DateTimeType;
                //return ValueSubTypeEnum.DateType;
                case "System.TimeSpan":
                    return DbDataTypeEnum.TimeType;
                case "System.Byte[]": //BLOB, Memo, ...
                default:
                    Debug.WriteLine("Unknown column data type: " + fullNameOfDataType);
                    return DbDataTypeEnum.UnknownType;
            }
        }

        /// <summary>
        /// Gets the simple data type of the specified (<c>dbDataType</c>) data type.
        /// </summary>
        /// <param name="dbDataType">(Complex) type of the db data.</param>
        public static DbSimpleDataTypeEnum GetSimpleDataType(DbDataTypeEnum dbDataType)
        {
            switch (dbDataType)
            {
                case DbDataTypeEnum.BooleanType:
                    return DbSimpleDataTypeEnum.BooleanSimpleType;
                case DbDataTypeEnum.DateTimeType:
                    return DbSimpleDataTypeEnum.DateTimeSimpleType;
                case DbDataTypeEnum.DecimalType:
                case DbDataTypeEnum.DoubleType:
                    return DbSimpleDataTypeEnum.DoubleSimpleType;
                case DbDataTypeEnum.FloatType:
                    return DbSimpleDataTypeEnum.FloatSimpleType;
                case DbDataTypeEnum.ShortIntegerType:
                    return DbSimpleDataTypeEnum.ShortSimpleType;
                case DbDataTypeEnum.UnsignedShortIntegerType:
                case DbDataTypeEnum.IntegerType:
                    return DbSimpleDataTypeEnum.IntegerSimpleType;
                case DbDataTypeEnum.UnsignedIntegerType:
                case DbDataTypeEnum.LongIntegerType:
                case DbDataTypeEnum.UnsignedLongIntegerType:
                    return DbSimpleDataTypeEnum.LongSimpleType;
                case DbDataTypeEnum.StringType:
                    return DbSimpleDataTypeEnum.StringSimpleType;
                case DbDataTypeEnum.TimeType:
                    return DbSimpleDataTypeEnum.TimeSimpleType;
                case DbDataTypeEnum.UnknownType:
                    return DbSimpleDataTypeEnum.UnknownSimpleType;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the type of the db simple data.
        /// </summary>
        /// <value>The type of the db simple data.</value>
        public DbSimpleDataTypeEnum DbSimpleDataType
        {
            get { return GetSimpleDataType(Explain.dataType); }
        }

        /// <summary>
        /// Determines whether the specified data type is numeric.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        /// <returns>
        /// 	<c>true</c> if the specified data type is numeric; otherwise, <c>false</c>.
        /// </returns>
        public static bool GetIsNumericDataType(DbDataTypeEnum dataType)
        {
            switch (dataType)
            {
                case DbDataTypeEnum.DecimalType:
                case DbDataTypeEnum.DoubleType:
                case DbDataTypeEnum.FloatType:
                case DbDataTypeEnum.IntegerType:
                case DbDataTypeEnum.LongIntegerType:
                case DbDataTypeEnum.ShortIntegerType:
                case DbDataTypeEnum.UnsignedIntegerType:
                case DbDataTypeEnum.UnsignedLongIntegerType:
                case DbDataTypeEnum.UnsignedShortIntegerType:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is of numeric data type.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is numeric of data type; otherwise, <c>false</c>.
        /// </value>
        public bool IsNumericDataType
        {
            get
            {
                return GetIsNumericDataType(Explain.dataType);
            }
        }

        /// <summary>
        /// Gets the potentially cardinality.
        /// </summary>
        /// <value>The potentially cardinality.</value>
        public CardinalityEnum PotentiallyCardinality
        {
            get
            {
                switch (DbSimpleDataType)
                {
                    case DbSimpleDataTypeEnum.DoubleSimpleType:
                    case DbSimpleDataTypeEnum.FloatSimpleType:
                    case DbSimpleDataTypeEnum.IntegerSimpleType:
                    case DbSimpleDataTypeEnum.LongSimpleType:
                    case DbSimpleDataTypeEnum.ShortSimpleType:
                        return CardinalityEnum.Cardinal;
                    case DbSimpleDataTypeEnum.DateTimeSimpleType:
                    case DbSimpleDataTypeEnum.TimeSimpleType:
                    case DbSimpleDataTypeEnum.StringSimpleType:
                        return CardinalityEnum.OrdinalCyclic;
                    case DbSimpleDataTypeEnum.BooleanSimpleType:
                    case DbSimpleDataTypeEnum.UnknownSimpleType:
                        return CardinalityEnum.Nominal;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        #endregion

        /// <summary>
        /// Gets the quoted query identifier of the column i.e.
        /// if the column is derived or virtual, returns its name
        /// (does not make any quotation) otherwise quotes the name of the column.
        /// </summary>
        /// <returns>Quoted query identifier of the column</returns>
        public string GetQuotedQueryIdentifier()
        {
            if ((IsDerived) || (IsVirtual))
                return _explain.name;
            else
                return GenericDataTable.GenericDatabase.QuoteQueryIdentifier(_explain.name);

        }

        /// <summary>
        /// Gets the distincts and frequencies.
        /// </summary>
        /// <param name="whereCondition">The where condition.</param>
        /// <returns>
        /// <see cref="T:System.Data.DataTable"/> where in first column is 
        /// (distinct) value and in sencond column is frequency of this 
        /// value in the database.
        /// </returns>
        /// <exception cref="T:Ferda.Modules.BadParamsError">
        /// <b>DbUnexpectedError</b>
        /// Thrown when the specifiec column does not support
        /// basic SQL queries (DISTINCT) e.g.
        /// the column is of type "BLOB", "Memo", ... (often
        /// DbSimpleDataTypeEnum.UnknownSimpleType)
        /// </exception>
        public DataTable GetDistinctsAndFrequencies(string whereCondition)
        {
            string where = (String.IsNullOrEmpty(whereCondition))
                               ? String.Empty
                               : " WHERE " + whereCondition;
            string tableName;
            string columnQuotedIdentifier;

            if (IsVirtual)
            {
                if (String.IsNullOrEmpty(whereCondition))
                {
                    where = " WHERE ";
                }
                else
                {
                    where = where + " AND ";
                }
                where = where
                    + GenericDataTable.GenericDatabase.QuoteQueryIdentifier(DetailDataTableName)
                    + "."
                    + GenericDataTable.GenericDatabase.QuoteQueryIdentifier(DetailTableIdColumn)
                    + "="
                    + GenericDataTable.GenericDatabase.QuoteQueryIdentifier(GenericDataTable.Explain.name)
                    + "."
                    + GenericDataTable.GenericDatabase.QuoteQueryIdentifier(MasterTableIdColumn);

                tableName = GenericDataTable.GenericDatabase.QuoteQueryIdentifier(DetailDataTableName)
                + ","
                + GenericDataTable.GenericDatabase.QuoteQueryIdentifier(GenericDataTable.Explain.name);
                columnQuotedIdentifier = 
                   // GenericDataTable.GenericDatabase.QuoteQueryIdentifier(
                  //  DetailDataTableName) + "." + 
                    GetQuotedQueryIdentifier();
            }
            else
            {
                tableName = GenericDataTable.GenericDatabase.QuoteQueryIdentifier(GenericDataTable.Explain.name);
                columnQuotedIdentifier = GetQuotedQueryIdentifier();
            }

            //string columnQuotedIdentifier = GetQuotedQueryIdentifier();

            DbCommand command = GenericDataTable.GenericDatabase.CreateDbCommand();
            command.CommandText =
                "SELECT " + columnQuotedIdentifier + ", COUNT(1) "
                + " FROM " + tableName
                + where
                + " GROUP BY " + columnQuotedIdentifier
                + " ORDER BY " + columnQuotedIdentifier;

            DbDataAdapter dataAdapter = GenericDataTable.GenericDatabase.CreateDbDataAdapter();
            dataAdapter.SelectCommand = command;

            try
            {
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                return dataSet.Tables[0];
            }
            catch (DbException e)
            {
                throw Exceptions.DbUnexpectedError(e, null);
            }
        }

        /// <summary>
        /// Gets the distincts.
        /// </summary>
        /// <param name="whereCondition">The where condition.</param>
        /// <returns></returns>
        /// <exception cref="T:Ferda.Modules.BadParamsError">
        /// <b>DbUnexpectedError</b>
        /// Thrown when the specifiec column does not support
        /// basic SQL queries (DISTINCT) e.g.
        /// the column is of type "BLOB", "Memo", ... (often
        /// DbSimpleDataTypeEnum.UnknownSimpleType)
        /// </exception>
        public DataTable GetDistincts(string whereCondition)
        {
            string where = (String.IsNullOrEmpty(whereCondition))
                               ? String.Empty
                               : " WHERE " + whereCondition;
            string tableName;
            string columnQuotedIdentifier;

            if (IsVirtual)
            {
                if (String.IsNullOrEmpty(whereCondition))
                {
                    where = " WHERE ";
                }
                else
                {
                    where = where + " AND ";
                }
                where = where
                    + GenericDataTable.GenericDatabase.QuoteQueryIdentifier(DetailDataTableName)
                    + "."
                    + GenericDataTable.GenericDatabase.QuoteQueryIdentifier(DetailTableIdColumn)
                    + "="
                    + GenericDataTable.GenericDatabase.QuoteQueryIdentifier(GenericDataTable.Explain.name)
                    + "."
                    + GenericDataTable.GenericDatabase.QuoteQueryIdentifier(MasterTableIdColumn);
                tableName = GenericDataTable.GenericDatabase.QuoteQueryIdentifier(DetailDataTableName)
                    + ","
                    + GenericDataTable.GenericDatabase.QuoteQueryIdentifier(GenericDataTable.Explain.name);
                columnQuotedIdentifier = 
                   // GenericDataTable.GenericDatabase.QuoteQueryIdentifier(
                 //   DetailDataTableName) + "." + 
                    GetQuotedQueryIdentifier();
            }
            else
            {
                tableName = GenericDataTable.GenericDatabase.QuoteQueryIdentifier(GenericDataTable.Explain.name);
                columnQuotedIdentifier = GetQuotedQueryIdentifier();
            }


             

            DbCommand command = GenericDataTable.GenericDatabase.CreateDbCommand();
            command.CommandText =
                "SELECT " + columnQuotedIdentifier
                + " FROM " + tableName
                + where
                + " GROUP BY " + columnQuotedIdentifier
                + " ORDER BY " + columnQuotedIdentifier;

            DbDataAdapter dataAdapter = GenericDataTable.GenericDatabase.CreateDbDataAdapter();
            dataAdapter.SelectCommand = command;

            try
            {
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                return dataSet.Tables[0];
            }
            catch (DbException e)
            {
                throw Exceptions.DbUnexpectedError(e, null);
            }
        }


        
        /// <summary>
        /// Gets the count vector for the bitstring generation for relational datamining
        /// </summary>
        /// <param name="masterIdColumn">Join key column from master table</param>
        /// <param name="masterDatatableName">Master datatable name</param>
        /// <param name="detailIdColumn">Join key column from detail table</param>
        /// <returns>Countvector table</returns>
        public DataTable GetCountVector(
            string masterIdColumn,
            string masterDatatableName,
            string detailIdColumn)
        {
            string _detailTableName =
                GenericDataTable.GenericDatabase.QuoteQueryIdentifier(GenericDataTable.Explain.name);
            string _masterTableIdQuoted = 
                GenericDataTable.GenericDatabase.QuoteQueryIdentifier(masterIdColumn);
            string _detailTableIdQuoted =
                GenericDataTable.GenericDatabase.QuoteQueryIdentifier(detailIdColumn);
            string _masterDatatableName = 
                GenericDataTable.GenericDatabase.QuoteQueryIdentifier(masterDatatableName);

            if (masterDatatableName.CompareTo(GenericDataTable.Explain.name)==0)
            {
                throw Exceptions.DbDataTableNameError(new Exception("Master and detail datatables cannot be same"),null);
            }
            //select count(Loans1.loan_id) from Loans left join Loans1 on Loans.loan_id = Loans1.loan_id group by Loans.loan_id
            DbCommand command = GenericDataTable.GenericDatabase.CreateDbCommand();
            command.CommandText =
                "SELECT COUNT(" + _detailTableName + "." 
                + _masterTableIdQuoted + ") FROM "
                + _masterDatatableName
                + " LEFT JOIN " + _detailTableName
                + " ON "
                + _masterDatatableName + "." + _masterTableIdQuoted
                + "="
                + _detailTableName + "." + _detailTableIdQuoted
                + " GROUP BY " + _masterDatatableName + "." + _masterTableIdQuoted
                + " ORDER BY " + _masterDatatableName + "." + _masterTableIdQuoted;

            DbDataAdapter dataAdapter = GenericDataTable.GenericDatabase.CreateDbDataAdapter();
            dataAdapter.SelectCommand = command;

            try
            {
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                return dataSet.Tables[0];
            }
            catch (DbException e)
            {
                throw Exceptions.DbUnexpectedError(e, null);
            }
        }

        /// <summary>
        /// Gets the "SELECT `columnName` FROM `tableName` ORDER BY `<c>uniqueKeyForSort</c>`"
        /// query result. (Specified unique key is tested for unicity.)
        /// </summary>
        /// <param name="uniqueKeyForSort">The unique key for sort.</param>
        /// <returns></returns>
        /// <exception cref="T:Ferda.Modules.BadParamsError">
        /// <b>DbUniqueKeyError</b> or <b>DbUnexpectedError</b>
        /// Thrown when the specified columns does not satisfy the 
        /// unicity of values in the database.
        /// </exception>
        public DataTable GetSelect(string[] uniqueKeyForSort)
        {
            // test the unique key
          //  GenericDataTable.TestUniqueKey(uniqueKeyForSort);

            if (uniqueKeyForSort.Length == 0)
            {
                throw Exceptions.DbUniqueKeyError(null, null);
            }

            string columnQuotedIdentifier;

            string dataTableQuotedIdentifier;
            string uniqueKeyQuotedIdentifier;
            if (!IsVirtual)
            {
                dataTableQuotedIdentifier = GenericDataTable.GenericDatabase.QuoteQueryIdentifier(GenericDataTable.Explain.name);
                columnQuotedIdentifier = GetQuotedQueryIdentifier();
                uniqueKeyQuotedIdentifier = GenericDataTable.getQuotedColumnsNames(uniqueKeyForSort);
            }
            else
            {
                uniqueKeyQuotedIdentifier = 
                    GenericDataTable.GenericDatabase.QuoteQueryIdentifier(DetailDataTableName)
                    + "." +
                    GenericDataTable.getQuotedColumnsNames(uniqueKeyForSort);

                dataTableQuotedIdentifier = 
                    GenericDataTable.GenericDatabase.QuoteQueryIdentifier(DetailDataTableName)
                    + ","
                    + GenericDataTable.GenericDatabase.QuoteQueryIdentifier(GenericDataTable.Explain.name)
                    + " WHERE "
                    + GenericDataTable.GenericDatabase.QuoteQueryIdentifier(DetailDataTableName)
                    + "."
                    + GenericDataTable.GenericDatabase.QuoteQueryIdentifier(DetailTableIdColumn)
                    + "="
                    + GenericDataTable.GenericDatabase.QuoteQueryIdentifier(GenericDataTable.Explain.name)
                    + "."
                    + GenericDataTable.GenericDatabase.QuoteQueryIdentifier(MasterTableIdColumn);
                columnQuotedIdentifier = 
                  //  GenericDataTable.GenericDatabase.QuoteQueryIdentifier(
                 //   DetailDataTableName) + "." + 
                    GetQuotedQueryIdentifier();
            }

            DbCommand command = GenericDataTable.GenericDatabase.CreateDbCommand();

            command.CommandText =
                "SELECT " + columnQuotedIdentifier
                + " FROM " + dataTableQuotedIdentifier
                + " ORDER BY " + uniqueKeyQuotedIdentifier;

            DbDataAdapter dataAdapter = GenericDataTable.GenericDatabase.CreateDbDataAdapter();
            dataAdapter.SelectCommand = command;
            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            return dataSet.Tables[0];
        }
    }
}