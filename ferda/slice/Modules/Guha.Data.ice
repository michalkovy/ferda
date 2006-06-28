#ifndef	FERDA_GUHA_DATA
#define	FERDA_GUHA_DATA

#include <Modules/BuiltinSequences.ice>
#include <Modules/BasicPropertyTypes.ice>
#include <Modules/Exceptions.ice>

module Ferda {
	module Guha {
		module Data {
		
			const string nullValueConstant = "Null";

			struct DatabaseConnectionSetting
			{
				string providerInvariantName; // see System.Data.Common.DbProviderFactories.GetFactory Method (String)
				string connectionString;
				Ferda::Modules::DateTimeT	lastReloadRequest;
			};

			struct ConnectionInfo
			{
				//see	corresponding	fields of	System.Data.Odbc.OdbcConnection	(.NET	Framework)
				string connectionString;
				int	connectionTimeout;
				string databaseName;
				string dataSource;
				string driver; //for odbc	sources
				string serverVersion;
			};

			struct DataTableExplain
			{
				string name;
				string type;
				string remarks;
				long recordsCount;
			};
			sequence<DataTableExplain> DataTableExplainSeq;

			enum DbDataTypeEnum
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
			};
			
			enum DbSimpleDataTypeEnum
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
			};
			
			enum CardinalityEnum
			{
				Nominal,
				Ordinal,
				OrdinalCyclic,
				Cardinal
			};

			struct ColumnExplain
			{
				string name;
				int	columnOrdinal;
				int	columnSize;
				int	numericPrecision;
				int	numericScale;
				DbDataTypeEnum dataType;
				int	providerType;
				bool isLong;
				bool allowDBNull;
				bool isReadOnly;
				bool isRowVersion;
				bool isUnique;
				bool isKey;
				bool isAutoIncrement;
			};
			sequence<ColumnExplain>	ColumnExplainSeq;

			struct ColumnStatistics
			{
				string valueMin;
				string valueMax;
				string valueAverage;
				double valueVariability;
				double valueStandardDeviation;
				long valueDistincts;
			};
			
			struct ValueFrequencyPair 
			{
				string value;
				int frequency;
			};
			sequence<ValueFrequencyPair> ValueFrequencyPairSeq;
					
			struct ValuesAndFrequencies {
				DbDataTypeEnum dataType; // distincts data type
				ValueFrequencyPairSeq data;
			};

		};
	};
};

#endif