package Ferda.OntologyRelated;

import Ferda.OntologyRelated.generated.OntologyData.ObjectProperty;
import Ferda.OntologyRelated.generated.OntologyData.OntologyStructure;
import Ferda.OntologyRelated.generated.*;

public class OWLParserClient {
	public static void
	main(String[] args)
	{
		int status = 0;
		Ice.Communicator ic = null;
		try {
			ic = Ice.Util.initialize(args);
			Ice.ObjectPrx base = ic.stringToProxy(
			"OWLParser:default -p 10000");
			OWLParserPrx parser = OWLParserPrxHelper.checkedCast(base);
			if (parser == null)
				throw new Error("Invalid proxy");
			
			OntologyStructure FerdaOntology = new OntologyStructure();
						
			FerdaOntology = parser.parseOntology("file:/D:/Marthin/skola/diplomova_prace/pokusne_ontologie/umls_stulong_2/umls_stulong_2.owl");
			
			System.out.println("-----FerdaOntology-------");
			
			for(String className : FerdaOntology.OntologyClassMap.keySet()) {
				System.out.println(className.toString() + ": ");
				
				System.out.println("\tannotations: ");
				for(String annotation : FerdaOntology.OntologyClassMap.get(className).Annotations) {
					if (!annotation.isEmpty()) System.out.println("\t\t" + annotation.toString());
				}
				System.out.println("\tsubclasses: ");
				for(String subClass : FerdaOntology.OntologyClassMap.get(className).SubClasses) {
					if (!subClass.isEmpty()) System.out.println("\t\t" + subClass.toString());
				}
				System.out.println("\tsuperclasses: ");
				for(String superClass : FerdaOntology.OntologyClassMap.get(className).SuperClasses) {
					if (!superClass.isEmpty()) System.out.println("\t\t" + superClass.toString());
				}
				
				System.out.println("\tdataproperties: ");
				
				for(String dataPropertyName : FerdaOntology.OntologyClassMap.get(className).DataPropertiesMap.keySet()) {
					System.out.println("\t\t" + dataPropertyName.toString() + ": ");
					for(String dataProperty : FerdaOntology.OntologyClassMap.get(className).DataPropertiesMap.get(dataPropertyName)) {
						if (!dataProperty.isEmpty()) System.out.println("\t\t\t" + dataProperty.toString());
					}
				}
				System.out.println("---------");
			}
			
			System.out.println("OBJECT PROPERTIES");
			for(ObjectProperty objectProperty : FerdaOntology.ObjectProperties) {
				System.out.println(objectProperty.name.toString());
				System.out.println("\tannotations: ");
				for(String annotation : objectProperty.Annotations) {
					if (!annotation.isEmpty()) System.out.println("\t\t" + annotation.toString());
				}
				System.out.println("\tdomains: ");
				for(String domain : objectProperty.Domains) {
					if (!domain.isEmpty()) System.out.println("\t\t" + domain.toString());
				}
				System.out.println("\tranges: ");
				for(String range : objectProperty.Ranges) {
					if (!range.isEmpty()) System.out.println("\t\t" + range.toString());
				}
				System.out.println("---------");
			}
			System.out.println("everything OK");
			
		} catch (Ice.LocalException e) {
			e.printStackTrace();
			status = 1;
		} catch (Exception e) {
			System.err.println(e.getMessage());
			status = 1;
		}
		if (ic != null) {
		// Clean up
		//
			try {
				ic.destroy();
			} catch (Exception e) {
				System.err.println(e.getMessage());
				status = 1;
			}
		}
		System.exit(status);
	}
}