#ifndef	FERDA_MODULES_BOXES_DATAPREPARATION
#define	FERDA_MODULES_BOXES_DATAPREPARATION

#include <Modules/BuiltinSequences.ice>
#include <Modules/BasicPropertyTypes.ice>
#include <Modules/Exceptions.ice>
#include <Modules/Guha.Data.ice>
#include <Modules/Guha.MiningProcessor.ice>

module Ferda {
	module Modules {
		module Boxes {
			module DataPreparation {
				
					// DATABASE
					
					interface DatabaseFunctions {
						nonmutating	Ferda::Guha::Data::DatabaseConnectionSetting	getDatabaseConnectionSetting()
							throws Ferda::Modules::BoxRuntimeError;
						nonmutating	Ferda::Guha::Data::ConnectionInfo getConnectionInfo()
							throws Ferda::Modules::BoxRuntimeError;
						nonmutating Ferda::Guha::Data::DataTableExplainSeq getDataTableExplainSeq()
							throws Ferda::Modules::BoxRuntimeError;
						nonmutating StringSeq getDataTablesNames()
							throws Ferda::Modules::BoxRuntimeError;
					};


					// DATATABLE
					
					struct DataTableInfo {
						Ferda::Guha::Data::DatabaseConnectionSetting databaseConnectionSetting;
						string dataTableName;
						string type;
						string remarks;
						StringSeq primaryKeyColumns;
						long recordsCount;
					};

					interface DataTableFunctions extends Ferda::Guha::MiningProcessor::SourceDataTableIdProvider {
						nonmutating	DataTableInfo	getDataTableInfo()
							throws Ferda::Modules::BoxRuntimeError;
						nonmutating Ferda::Guha::Data::ColumnExplainSeq getColumnExplainSeq()
							throws Ferda::Modules::BoxRuntimeError;
						nonmutating StringSeq getColumnsNames()
							throws Ferda::Modules::BoxRuntimeError;
					};
					
					
					// COLUMN
					
					struct ColumnInfo {
						DataTableInfo dataTable;
						string columnSelectExpression; // normally it is the name of the column but for derived columns it is stroger
						Ferda::Guha::Data::DbDataTypeEnum dataType;
						Ferda::Guha::Data::CardinalityEnum cardinality;
					};
					
					interface ColumnFunctions extends Ferda::Guha::MiningProcessor::SourceDataTableIdProvider {
						nonmutating	ColumnInfo getColumnInfo()
							throws Ferda::Modules::BoxRuntimeError;
						nonmutating Ferda::Guha::Data::ColumnStatistics getColumnStatistics()
							throws Ferda::Modules::BoxRuntimeError;
						nonmutating Ferda::Guha::Data::ValuesAndFrequencies getDistinctsAndFrequencies()
							throws Ferda::Modules::BoxRuntimeError;
					};
					
					// ATTRIBUTE
					
					enum DomainEnum
					{
						WholeDomain,
						SubDomain
					};
					
					interface AttributeFunctions extends Ferda::Guha::MiningProcessor::BitStringGenerator {
						nonmutating	string getAttribute()
							throws Ferda::Modules::BoxRuntimeError;
						nonmutating Ferda::Guha::Data::ValuesAndFrequencies getCategoriesAndFrequencies()
							throws Ferda::Modules::BoxRuntimeError;
					};

			};
		};
	};
};

#endif