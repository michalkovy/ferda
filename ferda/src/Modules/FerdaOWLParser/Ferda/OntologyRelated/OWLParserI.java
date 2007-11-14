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
	
	public static final int DEFAULT_ARRAY_SIZE = 5;
	
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
				
				String[] annotations = new String[DEFAULT_ARRAY_SIZE];
				String[] subClasses = new String[DEFAULT_ARRAY_SIZE];
				String[] superClasses = new String[DEFAULT_ARRAY_SIZE];
				String[] instances = new String[DEFAULT_ARRAY_SIZE];
				Map<java.lang.String, String[]> dataPropertiesMap = new HashMap<String, String[]>();
				
				//loading of annotations (labels, comments, ...)
				int i = 0;
				for (OWLAnnotation annotation : cls.getAnnotations(ontology)) {
					annotations = addToArray(annotations, annotation.toString(), i);
					i++;
				}
								
				//loading of subclasses
				i = 0;
				for (OWLDescription subClass : cls.getSubClasses(ontology)) {
					subClasses = addToArray(subClasses, subClass.toString(), i);
					i++;
				}
				
				//loading of superclasses
				i = 0; 
				for (OWLDescription superClass : cls.getSuperClasses(ontology)) {
					superClasses = addToArray(superClasses, superClass.toString(), i);
					i++;
				}
				
				OntologyClass newFerdaClass = new OntologyClass(
						cls.toString(),
						annotations,
						subClasses,
						superClasses,
						instances,
						dataPropertiesMap
						);
				
				FerdaOntology.OntologyClassMap.put(cls.toString(), newFerdaClass);
			}
			//assigning of data properties to particular classes - 
			//in this context, classes acts as an individuals and 
			//it's needed to assign right values (data properties) to classes in according to name 
			for(OWLIndividual ind : ontology.getReferencedIndividuals()) {
				
				OntologyClass ferdaClass = FerdaOntology.OntologyClassMap.get(ind.toString());
				
				if (ferdaClass == null) {	//instance which is not also a class
					ferdaClass = FerdaOntology.OntologyClassMap.get(ind.getTypes(ontology).toArray()[0].toString());
					ferdaClass.Instances = addToArray(ferdaClass.Instances, ind.toString(), -1);
				}
				else {
					Map<OWLDataPropertyExpression, Set<OWLConstant>> dpMap = ind.getDataPropertyValues(ontology);
					for(OWLDataPropertyExpression dpName : dpMap.keySet()) {
						String[] dpValueArray = new String[DEFAULT_ARRAY_SIZE];
						int i = 0;
						Iterator<OWLConstant> it = dpMap.get(dpName).iterator();
						while (it.hasNext()) {
							dpValueArray = (String[])addToArray(dpValueArray, it.next().getLiteral().toString(), i);
							i++;
						}
						ferdaClass.DataPropertiesMap.put(dpName.toString(), dpValueArray);
					}
				}
			}
			
			int j = 0;	//artificial variable used for positioning of ObjectProperty into an array
			ObjectProperty[] ArrayFerdaObjectProperty = new ObjectProperty[DEFAULT_ARRAY_SIZE];
			for(org.semanticweb.owl.model.OWLObjectProperty op : ontology.getReferencedObjectProperties()) {
				String[] annotations = new String[DEFAULT_ARRAY_SIZE];
				String[] domains = new String[DEFAULT_ARRAY_SIZE];
				String[] ranges = new String[DEFAULT_ARRAY_SIZE];
				
				int i = 0;
				for(org.semanticweb.owl.model.OWLAnnotation annotation : op.getAnnotations(ontology)) {
					annotations = addToArray(annotations, annotation.toString(), i);
					i++;
				}
				
				i = 0;
				for(org.semanticweb.owl.model.OWLDescription domain : op.getDomains(ontology)) {
					domains = addToArray(domains, domain.toString(), i);
					i++;
				}
				
				i = 0;
				for(org.semanticweb.owl.model.OWLDescription range : op.getRanges(ontology)) {
					ranges = addToArray(ranges, range.toString(), i);
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
				else {	//needed to enlarge the array ArrayFerdaObjectProperty
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
	
	public String[] addToArray(String[] array, String value, int position) {
		if (position < 0) {	//if position is negative it is a sign, that the value is to be placed on the first unoccupied position
			position = 0;
			while ((position < array.length) && (array[position]!= null)) {
				position++;
			}
		}
		if (position < array.length) {	//a capacity of an array is not exceeded
			array[position] = value;
			return array;
		}
		else {
			int length2 = array.length*2;
			int longer = length2 > position ? length2 : position + DEFAULT_ARRAY_SIZE; 
			String[] array2 = new String[longer];
			System.arraycopy(array, 0, array2, 0, array.length);
			array2[position] = value;
			return array2;
		}
	}
}
