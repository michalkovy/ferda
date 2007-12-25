package Ferda.OntologyRelated;

import java.net.URI;
import java.util.HashMap;
import java.util.Iterator;
import java.util.Map;
import java.util.Set;

import Ferda.OntologyRelated.generated.OntologyData.ObjectProperty;
import Ferda.OntologyRelated.generated.OntologyData.OntologyClass;
import Ferda.OntologyRelated.generated.OntologyData.OntologyStructure;
import Ferda.OntologyRelated.generated.OntologyData.WrongOntologyURL;
import Ice.Current;
import Ferda.OntologyRelated.generated.*;

import org.semanticweb.owl.apibinding.OWLManager;
import org.semanticweb.owl.model.OWLAnnotation;
import org.semanticweb.owl.model.OWLClass;
import org.semanticweb.owl.model.OWLConstant;
import org.semanticweb.owl.model.OWLDataPropertyExpression;
import org.semanticweb.owl.model.OWLDescription;
import org.semanticweb.owl.model.OWLIndividual;
import org.semanticweb.owl.model.OWLOntology;

public class OWLParserI extends _OWLParserDisp{
	
	//function for parsing the ontology into predefined structure
	//input parameter ontologyURL is either network or internet location of the ontology file
	public OntologyStructure parseOntology(String ontologyURL, Current __current)
			throws WrongOntologyURL {
		
		//ontology structure is defined in slice
		OntologyStructure FerdaOntology = new OntologyStructure();
		//FerdaOntologyClassMap connects names of classes and other information (properties) of the class 
		java.util.Map<java.lang.String, OntologyClass> FerdaOntologyClassMap = 
			new HashMap<String, OntologyClass>();
		FerdaOntology.OntologyClassMap = FerdaOntologyClassMap; 
		
		//loading and parsing the ontology
		try {
			//replace "\\" with "/" for correct windows paths
			//replace " " with "%20" is needed, bacause the method does't allow space character
			URI physicalURI = URI.create(ontologyURL.replace("\\", "/").replace(" ","%20").toString());
			
			//loading the ontology
			OWLOntology ontology = OWLManager.createOWLOntologyManager().loadOntologyFromPhysicalURI(physicalURI);
			
			//loading particular classes from the ontology
			for(OWLClass cls : ontology.getReferencedClasses()) {
				//structure for storing the data properties (its names and values) of the class
				Map<java.lang.String, String[]> dataPropertiesMap = new HashMap<String, String[]>();
				//structure for storing the individuals (instances - its names and annotations) of the class
				Map<java.lang.String, String[]> instancesMap = new HashMap<String, String[]>();
				
				//loading annotations (labels, comments, ...) of the class
				Set<OWLAnnotation> tmpAnnotations = cls.getAnnotations(ontology);
				String[] annotations = new String[tmpAnnotations.size()];
				int i = 0;
				for (OWLAnnotation annotation : tmpAnnotations) {
					String tmpstr = annotation.getAnnotationValue().toString();
					int from = tmpstr.indexOf("\"");
					int to = tmpstr.indexOf("\"", from + 1);
					annotations[i] = tmpstr.substring(from + 1, to);
					i++;
				}
								
				//loading subclasses of the class
				Set<OWLDescription> tmpSubClasses = cls.getSubClasses(ontology);
				String[] subClasses = new String[tmpSubClasses.size()];
				i = 0;
				for (OWLDescription subClass : tmpSubClasses) {
					subClasses[i] = subClass.toString();
					i++;
				}
				
				//loading superclasses of the class (in the owl ontology there can be more than one superclass for one class
				Set<OWLDescription> tmpSuperClasses = cls.getSuperClasses(ontology);
				String[] superClasses = new String[tmpSuperClasses.size()];
				i = 0;
				for (OWLDescription superClass : tmpSuperClasses) {
					superClasses[i] = superClass.toString();
					i++;
				}
				
				//creating new ontology class
				OntologyClass newFerdaClass = new OntologyClass(
						cls.toString(),
						annotations,
						subClasses,
						superClasses,
						instancesMap,
						dataPropertiesMap
						);
				
				//adding the class into OntologyClassMap
				FerdaOntology.OntologyClassMap.put(cls.toString(), newFerdaClass);
			}
			//assigning instances and data properties to particular classes - 
			//in this context, classes acts as an individuals and 
			//it's needed to assign right values (data properties) to classes in according to name 
			for(OWLIndividual ind : ontology.getReferencedIndividuals()) {
				
				OntologyClass ferdaClass = FerdaOntology.OntologyClassMap.get(ind.toString());
			
				//instance which is not also a class (it is an individual of the class)
				if (ferdaClass == null) {	
					ferdaClass = FerdaOntology.OntologyClassMap.get(ind.getTypes(ontology).toArray()[0].toString());
					//loading of annotations (labels, comments, ...)
					//annotations = new String[cls.getAnnotations(ontology).size()];
					Set<OWLAnnotation> tmpIndividualAnnotations = ind.getAnnotations(ontology);
					String[] individualAnnotations = new String[tmpIndividualAnnotations.size()];
					int i = 0;
					for (OWLAnnotation annotation : tmpIndividualAnnotations) {
						String tmpstr = annotation.getAnnotationValue().toString();
						int from = tmpstr.indexOf("\"");
						int to = tmpstr.indexOf("\"", from + 1);
						individualAnnotations[i] = tmpstr.substring(from + 1, to);
						i++;
					}
					ferdaClass.InstancesAnnotations.put(ind.toString(), individualAnnotations);
				}
				//instance which is also a class (it is a data property of the class)
				else {
					Map<OWLDataPropertyExpression, Set<OWLConstant>> dpMap = ind.getDataPropertyValues(ontology);
					for(OWLDataPropertyExpression dpName : dpMap.keySet()) {
						String[] dpValueArray = new String[dpMap.get(dpName).size()];
						Iterator<OWLConstant> it = dpMap.get(dpName).iterator();
						int i = 0;						
						while (it.hasNext()) {
							dpValueArray[i] = it.next().getLiteral().toString();
							i++;
						}
						ferdaClass.DataPropertiesMap.put(dpName.toString(), dpValueArray);
					}
				}
			}
			
			//loading the object properties of the ontology
			
			//creation of array ObjectProperties, which will be sent as a return value as a part of FerdaOntology
			FerdaOntology.ObjectProperties = new ObjectProperty[ontology.getReferencedObjectProperties().size()];	
			//artificial variable used for positioning of ObjectProperty into an array
			int j = 0;
			for(org.semanticweb.owl.model.OWLObjectProperty op : ontology.getReferencedObjectProperties()) {
				String[] annotations = new String[op.getAnnotations(ontology).size()];
				String[] domains = new String[op.getDomains(ontology).size()];
				String[] ranges = new String[op.getRanges(ontology).size()];
				
				int i = 0;
				for(org.semanticweb.owl.model.OWLAnnotation annotation : op.getAnnotations(ontology)) {
					annotations[i] = annotation.toString();
					i++;
				}
				
				i = 0;
				for(org.semanticweb.owl.model.OWLDescription domain : op.getDomains(ontology)) {
					domains[i] = domain.toString();
					i++;
				}
				
				i = 0;
				for(org.semanticweb.owl.model.OWLDescription range : op.getRanges(ontology)) {
					ranges[i] = range.toString();
					i++;
				}

				ObjectProperty newFerdaObjectProperty = new ObjectProperty(
						op.toString(), 
						annotations, 
						domains, 
						ranges);
				
				//adding new object property struct into the array of object properties
				FerdaOntology.ObjectProperties[j] = newFerdaObjectProperty;
				j++;
			}
			
			//setting the ontology URI
			FerdaOntology.ontologyURI = ontology.getURI().toString();
		}
		
		catch (Exception e) {
			//System.out.println("The ontology could not be created: " + e.getMessage());
			//return null;
		}
		
		return FerdaOntology;
	}
}
