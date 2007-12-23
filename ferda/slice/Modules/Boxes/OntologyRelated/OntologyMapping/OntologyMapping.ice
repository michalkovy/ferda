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
#include <Modules/Boxes/DataPreparation/DataPreparation.ice>

module Ferda {
	module Modules {
		module Boxes {
			module OntologyRelated
			{
				module OntologyMapping
				{
				  
					interface OntologyMappingFunctions
					{
						idempotent Ferda::OntologyRelated::generated::OntologyData::StrSeqMap getOntologyEntityProperties(string dataTableName, string columnName) 
							throws Ferda::Modules::BoxRuntimeError;
							
						idempotent StringSeq getOntologyEntityAnnotations(string dataTableName, string columnName) 
							throws Ferda::Modules::BoxRuntimeError;
							
						idempotent StringSeq getOntologyEntitySuperClasses(string entityName) 
							throws Ferda::Modules::BoxRuntimeError;

						idempotent Ferda::Modules::Boxes::DataPreparation::DataTableInfo getDataTableInfo(string dataTableName)
							throws Ferda::Modules::BoxRuntimeError;
							
						idempotent StringSeq getDataTablesNames()
							throws Ferda::Modules::BoxRuntimeError;

						idempotent StringSeq getColumnsNames(string dataTableName)
							throws Ferda::Modules::BoxRuntimeError;
							
						idempotent string getMapping();
						
						idempotent string getMappingSeparatorInner();
						
						idempotent string getMappingSeparatorOuter();
							
						idempotent string GetSourceDataTableId(string dataTableColumnName)
							throws Ferda::Modules::BoxRuntimeError;
					};
				};
				
				interface OntologyEnablingColumnFunctions extends Ferda::Modules::Boxes::DataPreparation::ColumnFunctions
				{
					idempotent Ferda::OntologyRelated::generated::OntologyData::StrSeqMap getOntologyEntityProperties() 
						throws Ferda::Modules::BoxRuntimeError;
				};
			};
		};
	};
};
#endif;
