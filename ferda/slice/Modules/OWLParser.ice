#ifndef	FERDA_ONTOLOGYRELATED
#define	FERDA_ONTOLOGYRELATED

#include <Modules/OntologyData.ice>

#endif    

module Ferda {
   module OntologyRelated {          
       interface OWLParser {
           OntologyData::OntologyStructure parseOntology(string ontologyURL) throws OntologyData::WrongOntologyURL;
       };
  };
};
