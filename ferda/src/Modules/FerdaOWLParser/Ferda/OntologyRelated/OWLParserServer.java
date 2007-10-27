package Ferda.OntologyRelated;

import Ferda.OntologyRelated.OWLParserI;

public class OWLParserServer {
	public static void
    main(String[] args)
    {
        int status = 0;
        Ice.Communicator ic = null;
        try {
            ic = Ice.Util.initialize(args);
            Ice.ObjectAdapter adapter = ic.createObjectAdapterWithEndpoints
            	("OWLParserAdapter", "default -p 10000");
            Ice.Object object = new OWLParserI();
            adapter.add(object, Ice.Util.stringToIdentity("OWLParser"));
            adapter.activate();
            ic.waitForShutdown();
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