package Ferda.OntologyRelated;

public class Service extends Ice.LocalObjectImpl implements IceBox.Service
{
	private Ice.ObjectAdapter _adapter;
	
	public void start(String name, Ice.Communicator communicator, String[] args)
	{
	    _adapter = communicator.createObjectAdapter(name);
	    Ice.Object object = new OWLParserI();
	    _adapter.add(object, Ice.Util.stringToIdentity("Ferda.OntologyRelated.OWLParser"));
	    
	    _adapter.activate();
	}

/* OWLParserServer
 * ic = Ice.Util.initialize(args);
            Ice.ObjectAdapter adapter = ic.createObjectAdapterWithEndpoints
            	("OWLParserAdapter", "default -p 10000");
            Ice.Object object = new OWLParserI();
            adapter.add(object, Ice.Util.stringToIdentity("OWLParser"));
            adapter.activate();
            ic.waitForShutdown();
 */

/* C# .NET
public void start(string name, Communicator communicator, string[] args)
{
    _adapter = communicator.createObjectAdapter(name);
    _adapter.add(new MiningProcessorFunctionsI(),
                 Util.stringToIdentity("Ferda.Guha.MiningProcessor.MiningProcessorFunctions"));
    ObjectFactory factory = new ObjectFactory();
    ObjectFactory.addFactoryToCommunicator(communicator, factory);
    _adapter.activate();
}
*/

	public void stop()
	{
		_adapter.deactivate();
	}

}