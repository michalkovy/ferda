package Ferda.OntologyRelated;

import Ice.Communicator;

public class ObjectFactory extends Ice.LocalObjectImpl implements Ice.ObjectFactory
{
	public Ice.Object create(String type) {
		if (type.equals("::Ferda::OntologyRelated::OWLParser")) {
			return new OWLParserI();
		}
		assert(false);
		return null;
	}
		
	/// <summary>
    /// Creates an object according to its type defined by 
    /// string in the parameter.
    /// </summary>
    /// <param name="type">Type of the object</param>
    /// <returns>Object of a given type</returns>

    public void destroy()
    {
        return;
    }

    /// <summary>
    /// Adds this object factory to the communicator. For further details,
    /// see the ICE documentation. 
    /// </summary>
    /// <param name="communicator">Communicator</param>
    /// <param name="factory">Factory to be added to a communicator</param>
    public static void addFactoryToCommunicator(Communicator communicator,
                                                ObjectFactory factory)
    {
    	if (communicator.findObjectFactory("::Ferda::OntologyRelated::OWLParser") == null)
            communicator.addObjectFactory(factory, "::Ferda::OntologyRelated::OWLParser");
    }
}
