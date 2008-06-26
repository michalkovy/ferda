#ifndef	FERDA_ONTOLOGYRELATED_GENERATED
#define	FERDA_ONTOLOGYRELATED_GENERATED

#include <Modules/OntologyData.ice>  

module Ferda {
  module OntologyRelated {
    module generated {          
      interface OWLParser {
        Ferda::OntologyRelated::generated::OntologyData::OntologyStructure parseOntology(string ontologyURL) throws OntologyData::WrongOntologyURL;
      };
    };
  };
};

#endif
