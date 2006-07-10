using System;
using System.Collections.Generic;
using System.Text;
using Ferda.ModulesManager;

namespace Ferda.FrontEnd
{
    public class ProgressBarI : ProgressBarDisp_
    {
        //float mezi 0 a 1, jestlize zaporne, tak progress bar "bezi"
        protected ProgressTaskPrx task;
        //task ma metodu stop, pomoci ktere se stopuje metoda.
        //kdyz stopnu, tak nic nedelam, treba disablenu progress bar
        //(nebo napsat stopping)
        //dalsi tlacitko hide bude davat visible = false
        protected string name;
        protected string hint;

        public ProgressBarI(ProgressTaskPrx task, string name, string hint)
        {
            this.task = task;
            this.name = name;
            this.hint = hint;

            //prida se novy progress bar
        }

        /// <summary>
        /// This is called on end. After calling this, proxy of this object is not usable
        /// </summary>
        /// <param name="current__"></param>
        public override void done(Ice.Current current__)
        {
            //vzdy kdyz rusim objekt, tohle musim zavolat
            current__.adapter.remove(current__.id);
        }

        public override void setValue(float value, string message, Ice.Current current__)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        //na refreshovani z frontendu se 
    }
}
