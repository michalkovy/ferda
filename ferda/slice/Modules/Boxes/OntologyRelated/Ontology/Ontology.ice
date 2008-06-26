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

	$ slice2cs -I../../../slice/ --output-dir ./OntologyRelated/Ontology/generated ./OntologyRelated/Ontology/Ontology.ice

		- Path in argument -I is path to "slice" subdirectory of Ferda project.
		- Path in argument --output-dir is place for generated *.cs files.
		- Following arguments are slice-files to proceed.

	See also
		$ slice2cs --help
*/

#ifndef	FERDA_MODULES_BOXES_ONTOLOGYRELATED_ONTOLOGY
#define FERDA_MODULES_BOXES_ONTOLOGYRELATED_ONTOLOGY

#include <Modules/Common.ice> //some common enumerations and structures
#include <Modules/Exceptions.ice> //some exceptions defined
#include <Modules/BuiltinSequences.ice> //sequences of basic types
#include <Modules/OWLParser.ice> //OWL parser
#include <Modules/OntologyData.ice> //Ontology data


module Ferda {
	module Modules {
		module Boxes {
			module OntologyRelated
			{
				module Ontology
				{
					interface OntologyFunctions						
					{
            bool LoadOntology() throws Ferda::Modules::BoxRuntimeError;
            
            void LoadOntologyWithParameter(string innerOntologyPath) throws Ferda::Modules::BoxRuntimeError;
            
            Ferda::OntologyRelated::generated::OntologyData::OntologyStructure getOntology()
              throws Ferda::Modules::BoxRuntimeError;
						
						idempotent Ferda::OntologyRelated::generated::OntologyData::StrSeqMap getOntologyEntityProperties(string entityName) 
              throws Ferda::Modules::BoxRuntimeError;
              
            idempotent StringSeq getOntologyEntityAnnotations(string entityName) 
							throws Ferda::Modules::BoxRuntimeError;
							
						idempotent StringSeq getOntologyEntitySuperClasses(string entityName) 
							throws Ferda::Modules::BoxRuntimeError;
          };
				};
			};
		};
	};
};
#endif
