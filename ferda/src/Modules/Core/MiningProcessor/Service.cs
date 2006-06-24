using System;
using System.Collections.Generic;
using Ferda.Guha.MiningProcessor.Generation;
using Ice;
using System.Text;
using IceBox;

namespace Ferda.Guha.MiningProcessor
{
    public class Service : LocalObjectImpl, IceBox.Service
    {
        private Ice.ObjectAdapter _adapter; 
        
        #region ServiceOperationsNC_ Members

        public void start(string name, Communicator communicator, string[] args)
        {
            _adapter = communicator.createObjectAdapter(name);
            _adapter.add(new MiningProcessorFunctionsI(), Ice.Util.stringToIdentity("Ferda.Guha.MiningProcessor.MiningProcessorFunctions"));
            ObjectFactory factory = new ObjectFactory();
            ObjectFactory.addFactoryToCommunicator(communicator, factory);
            _adapter.activate(); 
        }

        public void stop()
        {
            _adapter.deactivate(); 
        }

        #endregion
    }
}
