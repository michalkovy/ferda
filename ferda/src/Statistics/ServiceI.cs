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


            #region SDFFTTask - _adapter.add all of the statistics
            
            _adapter.add(new SDFFTTask.DfAvg(), Ice.Util.stringToIdentity("SFFTDfAvg"));
            _adapter.add(new SDFFTTask.DfConf(), Ice.Util.stringToIdentity("SFFTDfConf"));
            _adapter.add(new SDFFTTask.DfDFUI(), Ice.Util.stringToIdentity("SFFTDfDFUI"));
            _adapter.add(new SDFFTTask.DfFUE(), Ice.Util.stringToIdentity("SFFTDfFUE"));
           
            _adapter.add(new SDFFTTask.OneA(), Ice.Util.stringToIdentity("SFFTOneA"));
            _adapter.add(new SDFFTTask.OneAvgDf(), Ice.Util.stringToIdentity("SFFTOneAvgDf"));
            _adapter.add(new SDFFTTask.OneB(), Ice.Util.stringToIdentity("SFFTOneB"));
            _adapter.add(new SDFFTTask.OneC(), Ice.Util.stringToIdentity("SFFTOneC"));
            _adapter.add(new SDFFTTask.OneD(), Ice.Util.stringToIdentity("SFFTOneD"));
            _adapter.add(new SDFFTTask.OneCmplt(), Ice.Util.stringToIdentity("SFFTOneCmplt"));
            _adapter.add(new SDFFTTask.OneConf(), Ice.Util.stringToIdentity("SFFTOneConf"));
            _adapter.add(new SDFFTTask.OneDConf(), Ice.Util.stringToIdentity("SFFTOneDConf"));
            _adapter.add(new SDFFTTask.OneEConf(), Ice.Util.stringToIdentity("SFFTOneEConf"));
            _adapter.add(new SDFFTTask.OneSupp(), Ice.Util.stringToIdentity("SFFTOneSupp"));

            _adapter.add(new SDFFTTask.TwoA(), Ice.Util.stringToIdentity("SFFTTwoA"));
            _adapter.add(new SDFFTTask.TwoAvgDf(), Ice.Util.stringToIdentity("SFFTTwoAvgDf"));
            _adapter.add(new SDFFTTask.TwoB(), Ice.Util.stringToIdentity("SFFTTwoB"));
            _adapter.add(new SDFFTTask.TwoC(), Ice.Util.stringToIdentity("SFFTTwoC"));
            _adapter.add(new SDFFTTask.TwoD(), Ice.Util.stringToIdentity("SFFTTwoD"));
            _adapter.add(new SDFFTTask.TwoCmplt(), Ice.Util.stringToIdentity("SFFTTwoCmplt"));
            _adapter.add(new SDFFTTask.TwoConf(), Ice.Util.stringToIdentity("SFFTTwoConf"));
            _adapter.add(new SDFFTTask.TwoDConf(), Ice.Util.stringToIdentity("SFFTTwoDConf"));
            _adapter.add(new SDFFTTask.TwoEConf(), Ice.Util.stringToIdentity("SFFTTwoEConf"));
            _adapter.add(new SDFFTTask.TwoSupp(), Ice.Util.stringToIdentity("SFFTTwoSupp"));
            
            #endregion


            #region CFTask - _adapter.add all of the statistics

            _adapter.add(new CFTask.Sum(), Ice.Util.stringToIdentity("CFSum"));
            _adapter.add(new CFTask.Min(), Ice.Util.stringToIdentity("CFMin"));
            _adapter.add(new CFTask.Max(), Ice.Util.stringToIdentity("CFMax"));

            #endregion


            #region SDCFTask - _adapter.add all of the statistics

            _adapter.add(new SDCFTask.OneSum(), Ice.Util.stringToIdentity("SDCFOneSum"));
            _adapter.add(new SDCFTask.OneMin(), Ice.Util.stringToIdentity("SDCFOneMin"));
            _adapter.add(new SDCFTask.OneMax(), Ice.Util.stringToIdentity("SDCFOneMax"));

            _adapter.add(new SDCFTask.TwoSum(), Ice.Util.stringToIdentity("SDCFTwoSum"));
            _adapter.add(new SDCFTask.TwoMin(), Ice.Util.stringToIdentity("SDCFTwoMin"));
            _adapter.add(new SDCFTask.TwoMax(), Ice.Util.stringToIdentity("SDCFTwoMax"));


            #endregion


            #region KLTask - _adapter.add all of the statistics

            _adapter.add(new KLTask.Sum(), Ice.Util.stringToIdentity("KLSum"));
            _adapter.add(new KLTask.Min(), Ice.Util.stringToIdentity("KLMin"));
            _adapter.add(new KLTask.Max(), Ice.Util.stringToIdentity("KLMax"));

            #endregion


            #region SDKLTask - _adapter.add all of the statistics

            _adapter.add(new SDKLTask.OneSum(), Ice.Util.stringToIdentity("SDKLOneSum"));
            _adapter.add(new SDKLTask.OneMin(), Ice.Util.stringToIdentity("SDKLOneMin"));
            _adapter.add(new SDKLTask.OneMax(), Ice.Util.stringToIdentity("SDKLOneMax"));

            _adapter.add(new SDKLTask.TwoSum(), Ice.Util.stringToIdentity("SDKLTwoSum"));
            _adapter.add(new SDKLTask.TwoMin(), Ice.Util.stringToIdentity("SDKLTwoMin"));
            _adapter.add(new SDKLTask.TwoMax(), Ice.Util.stringToIdentity("SDKLTwoMax"));

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
