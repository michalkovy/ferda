using Ferda.Guha.MiningProcessor.Miners;
using Ice;

namespace Ferda.Guha.MiningProcessor
{
    public class Service : LocalObjectImpl, IceBox.Service
    {
        private ObjectAdapter _adapter;

        #region ServiceOperationsNC_ Members

        public void start(string name, Communicator communicator, string[] args)
        {
            _adapter = communicator.createObjectAdapter(name);
            _adapter.add(new MiningProcessorFunctionsI(),
                         Util.stringToIdentity("Ferda.Guha.MiningProcessor.MiningProcessorFunctions"));
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