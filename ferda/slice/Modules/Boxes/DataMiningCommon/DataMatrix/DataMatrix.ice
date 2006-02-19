#ifndef	FERDA_MODULES_BOXES_DataMiningCommon_DataMatrix
#define FERDA_MODULES_BOXES_DataMiningCommon_DataMatrix

#include <Modules/BuiltinSequences.ice>
#include <Modules/Modules.ice>
#include <Modules/Common.ice>
#include <Modules/Boxes/DataMiningCommon/Database/Database.ice>

module Ferda {
	module Modules {
		module Boxes {
			module DataMiningCommon {
				module DataMatrix	{

					struct ColumnSchemaInfo
					{
						string name;
						int columnOrdinal;
						int columnSize;
						int numericPrecision;
						int numericScale;
						string dataType;
						int providerType;
						bool isLong;
						bool allowDBNull;
						bool isReadOnly;
						bool isRowVersion;
						bool isUnique;
						bool isKey;
						bool isAutoIncrement;
					};
					sequence<ColumnSchemaInfo> ColumnSchemaInfoSeq;

					struct DataMatrixInfo {
						Ferda::Modules::Boxes::DataMiningCommon::Database::DatabaseInfo database;
						string dataMatrixName;
						StringSeq primaryKeyColumns;
						long recordsCount;
						ColumnSchemaInfoSeq explainDataMatrix;
					};

					interface	ColumnsNamesProviderFunctions {
						nonmutating StringSeq getColumnsNames()
							throws
								Ferda::Modules::BoxRuntimeError,
								Ferda::Modules::BadValueError,
								Ferda::Modules::BadParamsError,
								Ferda::Modules::NoConnectionInSocketError;
					};

					interface DataMatrixFunctions extends ColumnsNamesProviderFunctions {
						nonmutating DataMatrixInfo getDataMatrixInfo()
							throws
								Ferda::Modules::BoxRuntimeError,
								Ferda::Modules::BadValueError,
								Ferda::Modules::BadParamsError,
								Ferda::Modules::NoConnectionInSocketError;
						nonmutating ColumnSchemaInfoSeq explain()
							throws
								Ferda::Modules::BoxRuntimeError,
								Ferda::Modules::BadValueError,
								Ferda::Modules::BadParamsError,
								Ferda::Modules::NoConnectionInSocketError;
					};
				};
			};
		};
	};
};
#endif;