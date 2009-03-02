/*
Functions.cs - Function objects for the database box module

Author: Tomáš Kuchaø <tomas.kuchar@gmail.com>
Documented by: Martin Ralbovský <martin.ralbovsky@gmail.com>

Copyright (c) 2006 Tomáš Kuchaø, Martin Ralbovský

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation; either version 2 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
*/
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
					     //Returns the connection string
						idempotent	Ferda::Guha::Data::DatabaseConnectionSetting	getDatabaseConnectionSetting()
							throws Ferda::Modules::BoxRuntimeError;
						//Returns the connection info
						idempotent	Ferda::Guha::Data::ConnectionInfo getConnectionInfo()
							throws Ferda::Modules::BoxRuntimeError;
						//Returns an array of structures describing the tables in the database
						idempotent Ferda::Guha::Data::DataTableExplainSeq getDataTableExplainSeq()
							throws Ferda::Modules::BoxRuntimeError;
						//Returns the names of tables in the database
						idempotent StringSeq getDataTablesNames()
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
						idempotent	DataTableInfo getDataTableInfo()
							throws Ferda::Modules::BoxRuntimeError;
						idempotent Ferda::Guha::Data::ColumnExplainSeq getColumnExplainSeq()
							throws Ferda::Modules::BoxRuntimeError;
						idempotent StringSeq getColumnsNames()
							throws Ferda::Modules::BoxRuntimeError;
					};
					
					
					
					// COLUMN
					
					enum ColumnTypeEnum
					{
					   SimpleColumn,
					   VirtualColumn					   					 
					};
					
					struct ColumnInfo {
						DataTableInfo dataTable;
						string columnSelectExpression; // normally it is the name of the column but for derived columns it is stroger
						Ferda::Guha::Data::DbDataTypeEnum dataType;
						Ferda::Guha::Data::CardinalityEnum cardinality;
						ColumnTypeEnum columnType;
						string detailTableName;
						string detailTableIdColumn;
						string masterTableIdColumn;
					};
					
					interface ColumnFunctions extends Ferda::Guha::MiningProcessor::SourceDataTableIdProvider {
						idempotent	ColumnInfo getColumnInfo()
							throws Ferda::Modules::BoxRuntimeError;
						idempotent Ferda::Guha::Data::ColumnStatistics getColumnStatistics()
							throws Ferda::Modules::BoxRuntimeError;
						idempotent Ferda::Guha::Data::ValuesAndFrequencies getDistinctsAndFrequencies()
							throws Ferda::Modules::BoxRuntimeError;
					};
					
					// ATTRIBUTE
					
					enum DomainEnum
					{
						WholeDomain,
						SubDomain
					};
					
					//THE AttributeFunctions WAS REMOVED, BECAUSE ALL ITS FUNCTIONALITY WAS NEEDED
					//IN THE BitStringGenerator INTERFACE FOR PMML DOCUMENTS
					interface AttributeFunctions extends Ferda::Guha::MiningProcessor::BitStringGenerator {
					};

          interface FuzzyAttributeFunctions
          {
            string HelloWorld();
          };
			};
		};
	};
};

#endif
