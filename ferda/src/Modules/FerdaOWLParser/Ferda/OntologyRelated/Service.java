package Ferda.OntologyRelated;

// class for setting up the ICE communication between Ferda box ontology and FerdaOWLParser
public class Service extends Ice.LocalObjectImpl implements IceBox.Service
{
	private Ice.ObjectAdapter _adapter;
	
	//function needed by ICE
	public void start(String name, Ice.Communicator communicator, String[] args)
	{
	    _adapter = communicator.createObjectAdapter(name + "Adapter");
	    _adapter.add(new OWLParserI(), Ice.Util.stringToIdentity("Ferda.OntologyRelated.OWLParser"));
	    
	    _adapter.activate();
	}

	//function needed by ICE
	public void stop()
	{
		_adapter.deactivate();
	}

}
