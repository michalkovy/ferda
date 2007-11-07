package Ferda.OntologyRelated;

//import Ice.ObjectFactory;

public class Service extends Ice.LocalObjectImpl implements IceBox.Service
{
	private Ice.ObjectAdapter _adapter;
	
	public void start(String name, Ice.Communicator communicator, String[] args)
	{
	    _adapter = communicator.createObjectAdapter(name);
	    _adapter.add(new OWLParserI(), Ice.Util.stringToIdentity("Ferda.OntologyRelated.OWLParser"));
	    //ObjectFactory factory = new ObjectFactory();
	    //ObjectFactory.addFactoryToCommunicator(communicator, factory);
	    
	    _adapter.activate();
	}

	public void stop()
	{
		_adapter.deactivate();
	}

}