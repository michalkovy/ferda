#ifndef	FERDA_ONTOLOGYRELATED_GENERATED_ONTOLOGYDATA
#define	FERDA_ONTOLOGYRELATED_GENERATED_ONTOLOGYDATA

module Ferda {
  module OntologyRelated {
    module generated {
      module OntologyData {
        exception WrongOntologyURL {
          string ontologyURL;
        };
  
        sequence<string> StrSeq;    //type list of strings
        ["clr:collection"] dictionary<string, StrSeq> StrSeqMap;
        
        struct OntologyClass {
          string name;           //name of the class
          StrSeq Annotations;    //list for comments,labels,...
          StrSeq SubClasses;     //list with names of lineal descentdants
          StrSeq SuperClasses;   //list with name of lineal predecessors
          StrSeqMap InstancesAnnotations; //"dictionary" - name of an instance and list of its annotations
          StrSeqMap DataPropertiesMap;    //"dictionary" - name of a property and list of values it takes for this class
        };
        
        ["clr:collection"] dictionary<string, OntologyClass> dictionaryStringOntologyClass; //"a map" - name of class -> class of ontology
  
        struct ObjectProperty {
          string name;       //name of relation
          StrSeq Annotations;    //list for comments,labels,...
          StrSeq Domains;    //list of domains of the relation
          StrSeq Ranges;	   //list of ranges of the relation
        };
  
        sequence<ObjectProperty> sequenceObjectProperty;
  
        struct OntologyStructure {
          string ontologyURI; //ontology URI
          dictionaryStringOntologyClass OntologyClassMap; //all the classes in the ontology
          sequenceObjectProperty ObjectProperties;    //list of all the relations in ontology
        };
      };
    };
  };
};

#endif
