/*
SLICE DOCUMENTATION
-------------------
	www.zeroc.com

	Some general keywords
	---------------------
	idempotent
		Operations that use the Slice idempotent keyword must not modify object state.
	idempotent
		Operations that use the Slice Idempotent keyword can modify object state, but invoking an operation twice in a row must result in the same object state as invoking it once.

HOW TO GENERATE *.cs FROM *.ice
-------------------------------
	Path to Ice/bin directory (e.g. "c:\Ice-2.1.2\bin\") is in the PATH.
	Execute following command in solution root directory.

	$ slice2cs -I../../../slice/ --output-dir ./OntologyRelated/OntologyMapping/generated ./OntologyRelated/OntologyMapping/OntologyMapping.ice

		- Path in argument -I is path to "slice" subdirectory of Ferda project.
		- Path in argument --output-dir is place for generated *.cs files.
		- Following arguments are slice-files to proceed.

	See also
		$ slice2cs --help
*/

#ifndef	OntologyRelated_OntologyMapping
#define OntologyRelated_OntologyMapping

#include <Modules/Common.ice> //some common enumerations and structures
#include <Modules/BasicPropertyTypes.ice>
#include <Modules/Exceptions.ice> //some exceptions defined
#include <Modules/BuiltinSequences.ice> //sequences of basic types
#include <Modules/OntologyData.ice> //Ontology data
#include <Modules/Modules.ice> //Modules
#include <Modules/Guha.Data.ice>
#include <Modules/Guha.MiningProcessor.ice>

module Ferda {
	module Modules {
		module Boxes {
			module OntologyRelated
			{
				module OntologyMapping
				{
				  struct DataTableInfo {
						Ferda::Guha::Data::DatabaseConnectionSetting databaseConnectionSetting;
						string dataTableName;
						string type;
						string remarks;
						StringSeq primaryKeyColumns;
						long recordsCount;
					};

					interface OntologyMappingFunctions
					{
            idempotent Ferda::OntologyRelated::generated::OntologyData::StrSeqMap getOntologyEntityProperties(string dataTableColumnName) 
            throws Ferda::Modules::BoxRuntimeError;

            idempotent	DataTableInfo getDataTableInfo()
							throws Ferda::Modules::BoxRuntimeError;
							
						idempotent StringSeq getDataTablesNames()
							throws Ferda::Modules::BoxRuntimeError;

						idempotent StringSeq getColumnsNames()
							throws Ferda::Modules::BoxRuntimeError;
							
						idempotent string GetSourceDataTableId(string DataTableColumnName)
						  throws Ferda::Modules::BoxRuntimeError;
          };
				};
			};
		};
	};
};
#endif;
