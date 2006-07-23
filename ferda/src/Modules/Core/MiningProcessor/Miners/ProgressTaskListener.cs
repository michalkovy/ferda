using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Modules;

namespace Ferda.Guha.MiningProcessor.Miners
{
    public class ProgressTaskListener : ProgressTaskDisp_
    {
        public MiningProcessorBase MinningProcessor
        {
            set { _minningProcessor = value; }
        }
        MiningProcessorBase _minningProcessor = null;

        public override float getValue(out string message, Ice.Current current__)
        {
            if (_minningProcessor == null)
            {
                message = "Loading ...";
                return -1;
            }
            else
            {
                return _minningProcessor.ProgressGetValue(out message);
            }
        }

        private bool _stopped = false;
        public bool Stopped
        {
            get { return _stopped; }
        }

        public override void stop(Ice.Current current__)
        {
            _stopped = true;
        }
    }
}
