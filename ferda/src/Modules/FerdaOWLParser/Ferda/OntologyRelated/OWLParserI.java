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
import java.io.*;

public class OWLParserI extends _OWLParserDisp{
	
	public static final int DEFAULT_ARRAY_SIZE = 10;
	
	public OntologyStructure parseOntology(String ontologyURL, Current __current)
			throws WrongOntologyURL {
		
		OntologyStructure FerdaOntology = new OntologyStructure();
		java.util.Map<java.lang.String, OntologyClass> FerdaOntologyClassMap = 
			new HashMap<String, OntologyClass>();
		FerdaOntology.OntologyClassMap = FerdaOntologyClassMap; 
		
		try {
			//loading ontology
			
			//replace "\\" with "/" for correct windows paths
			//replace " " with "%20" is needed, bacause the method does't allow space character
			URI physicalURI = URI.create(ontologyURL.replace("\\", "/").replace(" ","%20").toString());
			
			OWLOntology ontology = OWLManager.createOWLOntologyManager().loadOntologyFromPhysicalURI(physicalURI);
			
			//loading of particular classes from the ontology
			for(OWLClass cls : ontology.getReferencedClasses()) {
				
				Map<java.lang.String, String[]> dataPropertiesMap = new HashMap<String, String[]>();
				Map<java.lang.String, String[]> instancesMap = new HashMap<String, String[]>();
				
				//loading of annotations (labels, comments, ...)
				//annotations = new String[cls.getAnnotations(ontology).size()];
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
								
				//loading of subclasses
				Set<OWLDescription> tmpSubClasses = cls.getSubClasses(ontology);
				String[] subClasses = new String[tmpSubClasses.size()];
				i = 0;
				for (OWLDescription subClass : tmpSubClasses) {
					subClasses[i] = subClass.toString();
					i++;
				}
				
				//loading of superclasses
				Set<OWLDescription> tmpSuperClasses = cls.getSuperClasses(ontology);
				String[] superClasses = new String[tmpSuperClasses.size()];
				i = 0;
				for (OWLDescription superClass : tmpSuperClasses) {
					superClasses[i] = superClass.toString();
					i++;
				}
				
				OntologyClass newFerdaClass = new OntologyClass(
						cls.toString(),
						annotations,
						subClasses,
						superClasses,
						instancesMap,
						dataPropertiesMap
						);
				
				FerdaOntology.OntologyClassMap.put(cls.toString(), newFerdaClass);
			}
			//assigning instances and data properties to particular classes - 
			//in this context, classes acts as an individuals and 
			//it's needed to assign right values (data properties) to classes in according to name 
			for(OWLIndividual ind : ontology.getReferencedIndividuals()) {
				
				OntologyClass ferdaClass = FerdaOntology.OntologyClassMap.get(ind.toString());
				
				if (ferdaClass == null) {	//instance which is not also a class (individuals of the classes)
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
			
			int j = 0;	//artificial variable used for positioning of ObjectProperty into an array
			ObjectProperty[] ArrayFerdaObjectProperty = new ObjectProperty[ontology.getReferencedObjectProperties().size()];
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
				
				if (ArrayFerdaObjectProperty.length > j) {	//no need to enlarge the array
					ArrayFerdaObjectProperty[j] = newFerdaObjectProperty;
				}
				else {	//it is needed to enlarge the array ArrayFerdaObjectProperty
					ObjectProperty[] ArrayFerdaObjectProperty2 = new ObjectProperty[2*ArrayFerdaObjectProperty.length];
					System.arraycopy(ArrayFerdaObjectProperty, 0, ArrayFerdaObjectProperty2, 0, ArrayFerdaObjectProperty.length);
					ArrayFerdaObjectProperty2[j] = newFerdaObjectProperty;
					ArrayFerdaObjectProperty = ArrayFerdaObjectProperty2;
				}
				j++;	
			}
			
			FerdaOntology.ObjectProperties = new ObjectProperty[j];	//creation of array ObjectProperties, which will be sent as a return value as a part of FerdaOntology 
			j = 0;
			for(ObjectProperty objectProperty : ArrayFerdaObjectProperty) {
				if (objectProperty != null) {
					FerdaOntology.ObjectProperties[j] = objectProperty;
					j++;
				}
			}
			
			FerdaOntology.ontologyURI = ontology.getURI().toString();
		
		}
		
		catch (Exception e) {
			//System.out.println("The ontology could not be created: " + e.getMessage());
			//return null;
		}
		
		return FerdaOntology;
	}
}
