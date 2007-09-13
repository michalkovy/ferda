// IceDesignDocumentation.cs - documentation of Ice design
//
// Author: Martin Ralbovsky <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2007 Martin Ralbovsky
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

using Ferda.Modules;

namespace Ferda.Guha.Data.IceDesignDocumentation
{
    /// <summary>
    /// Structure that holds information about the a connection
    /// setting to a database.
    /// </summary>
    public struct DataBaseConnectionSetting
    {
        /// <summary>
        /// The provider of the connection setting.
        /// See <see cref="System.Data.Common.DbProviderFactory"/>
        /// for details. Provider can be e.g. ODBC or a MS SQL server. 
        /// </summary>
        public string providerInvariantName;

        /// <summary>
        /// Connection string to a database. May be different for 
        /// different dtabase providers. 
        /// </summary>
        public string connectionString;

        /// <summary>
        /// Date and time of last reload request of the database
        /// </summary>
        public DateTimeT lastReloadRequest;
    }

    /// <summary>
    /// Represents a ODBC connection info. For more information, see the
    /// <see cref="System.Data.Odbc.OdbcConncetion"/> for details and
    /// the corresponding fields.
    /// </summary>
    public struct ConnectionInfo
    {
        /// <summary>
        /// Gets or sets the string used to open a data source.
        /// </summary>
        public string connectionString;

        /// <summary>
        /// Gets or sets the time to wait while trying to establish a 
        /// connection before terminating the attempt and generating an error.
        /// </summary>
        public int connectionTimeout;

        /// <summary>
        /// Gets the name of the current database or the database to be 
        /// used after a connection is opened.
        /// </summary>
        public string databaseName;

        /// <summary>
        /// Gets the server name or file name of the data source.
        /// </summary>
        public string dataSource;

        /// <summary>
        /// For ODBC connections only.
        /// Gets the name of the ODBC driver specified for the current connection.
        /// </summary>
        public string driver;

        /// <summary>
        /// Gets a string that contains the version of the server to which
        /// the client is connected.
        /// </summary>
        public string serverVersion;
    }

    /// <summary>
    /// Structure explaining a data table
    /// (for purposes of data mining).
    /// </summary>
    public struct DataTableExplain
    {
        /// <summary>
        /// Name of the data table
        /// </summary>
        public string name;

        /// <summary>
        /// Type of the data table
        /// (table, view...)
        /// </summary>
        public string type;

        /// <summary>
        /// Remarks about the table
        /// </summary>
        public string remarks;

        /// <summary>
        /// How many records are in the table
        /// </summary>
        public long recordsCount;
    }

    /// <summary>
    /// Possible data types in the database.
    /// </summary>
    public enum DbDataTypeEnum
    {
        UnknownType,
        BooleanType,
        //DateType,
        DateTimeType,
        TimeType,
        ShortIntegerType,
        IntegerType,
        LongIntegerType,
        UnsignedShortIntegerType,
        UnsignedIntegerType,
        UnsignedLongIntegerType,
        FloatType,
        DoubleType,
        DecimalType,
        StringType
    }

    /// <summary>
    /// Simple data types in a database.
    /// </summary>
    public enum DbSimpleDataTypeEnum
    {
        UnknownSimpleType,
        BooleanSimpleType,
        //DateSimpleType,
        DateTimeSimpleType,
        TimeSimpleType,
        ShortSimpleType,
        IntegerSimpleType,
        LongSimpleType,
        FloatSimpleType,
        DoubleSimpleType,
        StringSimpleType
    }

    /// <summary>
    /// Represents cardinality of a column. 
    /// </summary>
    public enum CardinalityEnum
    {
        Nominal,
        Ordinal,
        OrdinalCyclic,
        Cardinal
    }

    /// <summary>
    /// Structure explaining a column (for purposes of data mining)
    /// </summary>
    public struct ColumnExplain
    {
        /// <summary>
        /// Name of the column in a data table
        /// </summary>
        public string name;

        /// <summary>
        /// Ordering of columns in the data table
        /// </summary>
        public int columnOrdinal;

        /// <summary>
        /// Size of the column (in Bytes)
        /// </summary>
        public int columnSize;

        /// <summary>
        /// Numerical precision
        /// </summary>
        public int numericPrecision;

        /// <summary>
        /// Numeric Scale
        /// </summary>
        public int numericScale;

        /// <summary>
        /// Data type of the column
        /// </summary>
        public DbDataTypeEnum dataType;

        /// <summary>
        /// Provider type of the column
        /// </summary>
        public int providerType;
        
        /// <summary>
        /// If the column is long
        /// </summary>
        public bool isLong;
        
        /// <summary>
        /// If the column allows nulls
        /// </summary>
        public bool allowDBNull;
        
        /// <summary>
        /// If the column is read only 
        /// </summary>
        public bool isReadOnly;
        
        /// <summary>
        /// ???
        /// </summary>
        public bool isRowVersion;

        /// <summary>
        /// If values of the column are unique
        /// </summary>
        bool isUnique;

        /// <summary>
        /// If the column is a key
        /// </summary>
        bool isKey;

        /// <summary>
        /// If the column uses auto increment
        /// </summary>
        bool isAutoIncrement;
    }

    /// <summary>
    /// Statistical information about a database column
    /// </summary>
    public struct ColumnStatistics
    {
        /// <summary>
        /// Maximal value of the column
        /// </summary>
        public string valueMin;

        /// <summary>
        /// Minimal value of the column
        /// </summary>
        public string valueMax;

        /// <summary>
        /// Average value of the column
        /// </summary>
        public string valueAverage;

        /// <summary>
        /// Variance of a column.
        /// </summary>
        public double valueVariability;

        /// <summary>
        /// Standard deviation for a column
        /// only for numerical columns. 
        /// </summary>
        public double valueStandardDeviation;

        /// <summary>
        /// Number of distinct values in a column
        /// </summary>
        public long valueDistincts;
    }

    /// <summary>
    /// Structure holding information about a value from
    /// column and its frequency.
    /// </summary>
    public struct ValueFrequencyPair
    {
        /// <summary>
        /// Value of a column (or its string representation
        /// </summary>
        public string value;
        /// <summary>
        /// Frequency of that value
        /// </summary>
        public int frequency;
    }

    /// <summary>
    /// Structure holding information about
    /// values and frequencies of a database column
    /// </summary>
    public struct ValuesAndFrequencies
    {
        /// <summary>
        /// Data type of the column
        /// </summary>
        public DbDataTypeEnum dataType;
        /// <summary>
        /// Its values and frequencies
        /// </summary>
        public ValueFrequencyPair[] data;
    };
}