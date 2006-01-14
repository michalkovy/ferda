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

					struct ColumnInfo
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
					sequence<ColumnInfo> ColumnInfoSeq;

					struct DataMatrixStruct {
						Ferda::Modules::Boxes::DataMiningCommon::Database::DatabaseStruct database;
						string dataMatrixName;
						StringSeq primaryKeyColumns;
						long recordsCount;
						ColumnInfoSeq explainDataMatrix;
					};

					interface	ColumnsNamesProvider {
						nonmutating StringSeq getColumns()
							throws
								Ferda::Modules::BoxRuntimeError,
								Ferda::Modules::BadValueError,
								Ferda::Modules::BadParamsError,
								Ferda::Modules::NoConnectionInSocketError;
						nonmutating ColumnInfoSeq explainDataMatrixStructure()
							throws
								Ferda::Modules::BoxRuntimeError,
								Ferda::Modules::BadValueError,
								Ferda::Modules::BadParamsError,
								Ferda::Modules::NoConnectionInSocketError;
					};

					interface DataMatrixFunctions extends ColumnsNamesProvider {
						nonmutating DataMatrixStruct getDataMatrix()
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