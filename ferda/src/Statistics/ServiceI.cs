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

            #region FFTTask - _adapter.add all of the statistics
            
            _adapter.add(new FFTTask.A(), Ice.Util.stringToIdentity("FFTTA"));
            _adapter.add(new FFTTask.AvgDf(), Ice.Util.stringToIdentity("FFTTAvgDf"));
            _adapter.add(new FFTTask.B(), Ice.Util.stringToIdentity("FFTTB"));
            _adapter.add(new FFTTask.C(), Ice.Util.stringToIdentity("FFTTC"));
            _adapter.add(new FFTTask.CmPlt(), Ice.Util.stringToIdentity("FFTTCmPlt"));
            _adapter.add(new FFTTask.Conf(), Ice.Util.stringToIdentity("FFTTConf"));
            _adapter.add(new FFTTask.D(), Ice.Util.stringToIdentity("FFTTD"));
            _adapter.add(new FFTTask.DConf(), Ice.Util.stringToIdentity("FFTTDConf"));
            _adapter.add(new FFTTask.DLBound(), Ice.Util.stringToIdentity("FFTTDLBound"));
            _adapter.add(new FFTTask.DUBound(), Ice.Util.stringToIdentity("FFTTDUBound"));
            _adapter.add(new FFTTask.EConf(), Ice.Util.stringToIdentity("FFTTEConf"));
            _adapter.add(new FFTTask.ELBound(), Ice.Util.stringToIdentity("FFTTELBound"));
            _adapter.add(new FFTTask.EUBound(), Ice.Util.stringToIdentity("FFTTEUBound"));
            _adapter.add(new FFTTask.Fisher(), Ice.Util.stringToIdentity("FFTTFisher"));
            _adapter.add(new FFTTask.ChiSq(), Ice.Util.stringToIdentity("FFTTChiSq"));
            _adapter.add(new FFTTask.LBound(), Ice.Util.stringToIdentity("FFTTLBound"));
            _adapter.add(new FFTTask.N(), Ice.Util.stringToIdentity("FFTTN"));
            _adapter.add(new FFTTask.R(), Ice.Util.stringToIdentity("FFTTR"));
            _adapter.add(new FFTTask.Support(), Ice.Util.stringToIdentity("FFTTSupport"));
            _adapter.add(new FFTTask.UBound(), Ice.Util.stringToIdentity("FFTTUBound"));

            #endregion

            #region CFTask - _adapter.add all of the statistics

            _adapter.add(new CFTask.Sum(), Ice.Util.stringToIdentity("CFSum"));
            _adapter.add(new CFTask.Min(), Ice.Util.stringToIdentity("CFMin"));
            _adapter.add(new CFTask.Max(), Ice.Util.stringToIdentity("CFMax"));

            #endregion

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
