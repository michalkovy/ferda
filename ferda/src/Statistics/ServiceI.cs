using System;
using System.Collections.Generic;
using System.Text;
using IceBox;

namespace Ferda.Statistics
{
    public class ServiceI : Ice.LocalObjectImpl, IceBox.Service 
    {
        #region ServiceOperationsNC_ Members

        public void start(string name, Ice.Communicator communicator, string[] args)
        {
            _adapter = communicator.createObjectAdapter(name);
            _adapter.add(new FFT.Support(), Ice.Util.stringToIdentity("FFTSupport"));
            _adapter.activate();

        }

        public void stop()
        {
            _adapter.deactivate();
        }

        private Ice.ObjectAdapter _adapter;

        #endregion
    }
}
