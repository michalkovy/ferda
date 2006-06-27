using System;
using System.Collections.Generic;
using System.Text;
using Ferda.ModulesManager;

namespace Ferda.FrontEnd
{
    public class ProgressBarI : ProgressBarDisp_
    {
        protected ProgressTaskPrx task;
        protected string name;
        protected string hint;

        public ProgressBarI(ProgressTaskPrx task, string name, string hint)
        {
            this.task = task;
            this.name = name;
            this.hint = hint;
        }

        /// <summary>
        /// This is called on end. After calling this, proxy of this object is not usable
        /// </summary>
        /// <param name="current__"></param>
        public override void done(Ice.Current current__)
        {
            current__.adapter.remove(current__.id);
        }

        public override void setValue(float value, string message, Ice.Current current__)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
