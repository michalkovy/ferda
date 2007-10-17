module Ferda {
   module OntologyRelated {
       module OntologyData {
           exception WrongOntologyURL {
                   string ontologyURL;
           };
                    
           sequence<string> StrSeq;    //typ seznam stringu
           dictionary<string, StrSeq> StrSeqMap;
                   
           struct OntologyClass {
             string name;           //jmeno tridy ontologie
           	StrSeq Annotations;    //seznam pro komentare/labely/atd.
           	StrSeq SubClasses;     //seznam s nazvy trid primych potomku
           	StrSeq SuperClasses;   //seznam s nazvy trid primych predchudcu
           	StrSeq Instances;      //seznam s nazvy instanci teto tridy
           	StrSeqMap DataPropertiesMap;    //"slovnik" - nazev vlastnosti a seznam hodnot jakych nabyva
           };
           
           dictionary<string, OntologyClass> dictionaryStringOntologyClass; //mapa - nazev tridy -> trida ontologie
           
           struct ObjectProperty {
             string name;       //nazev relaci
           	StrSeq Annotations;    //seznam pro komentare/labely/atd.
             StrSeq Domains;    //seznam trid, ktere jsou pro tuto relaci definicnim oborem
           	StrSeq Ranges;	   //seznam trid, ktere jsou pro tuto relaci oborem hodnot
           };
           
           sequence<ObjectProperty> sequenceObjectProperty;
           
           struct OntologyStructure {
             dictionaryStringOntologyClass OntologyClassMap; //vsechny tridy v ontologii
             sequenceObjectProperty ObjectProperties;    //seznam relaci ontologie
           };
       };
   };
};
