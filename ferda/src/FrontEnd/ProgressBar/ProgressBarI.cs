// ProgressBarI.cs - clas that handles displaying progress of actions of boxes
//
// Author: Martin Ralbovský <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2006 Martin Ralbovský
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Ferda.ModulesManager;
using Ferda.Modules;

namespace Ferda.FrontEnd.ProgressBar
{
    /// <summary>
    /// This class handles a progress bar of one particular box.
    /// It can display the progress of a boxe's action
    /// </summary>
    public class ProgressBarI : ProgressBarDisp_
    {
        //float mezi 0 a 1, jestlize zaporne, tak progress bar "bezi"
        //task ma metodu stop, pomoci ktere se stopuje metoda.
        //kdyz stopnu, tak nic nedelam, treba disablenu progress bar
        //(nebo napsat stopping)
        //dalsi tlacitko hide bude davat visible = false

        /// <summary>
        /// The task connected to this progress bar
        /// </summary>
        protected ProgressTaskPrx task;
        /// <summary>
        /// Name of this progress bar
        /// </summary>
        protected string name;
        /// <summary>
        /// Hint for this progress bar
        /// </summary>
        protected string hint;
        private BoxProgressBar myControl;
        private ProgressBarsManager parentControl;

        private DateTime counter;

        /// <summary>
        /// Default constructor for the class
        /// </summary>
        /// <param name="task">proxy of the task</param>
        /// <param name="name">name of the task</param>
        /// <param name="hint">hint to be displayed</param>
        /// <param name="parentControl">parent control where all the viewing should
        /// be done</param>
        public ProgressBarI(ProgressTaskPrx task, string name, string hint, 
            ProgressBarsManager parentControl)
        {
            this.task = task;
            this.name = name;
            this.hint = hint;
            this.parentControl = parentControl;

            //creating a new progress bar control
            myControl = new BoxProgressBar(this);
            myControl.BoxName = name;
            myControl.SetHint(hint);

            parentControl.AddBoxProgressBar(myControl);

            //setting counter to time the hypotheses
            counter = DateTime.Now;
        }

        /// <summary>
        /// This is called on end. After calling this, proxy of this object is not usable
        /// </summary>
        /// <param name="current__">Some ICE stuff</param>
        public override void done(Ice.Current current__)
        {
            //vzdy kdyz rusim objekt, tohle musim zavolat
            current__.adapter.remove(current__.id);

            //removing the control from the 
            parentControl.RemoveBoxProgressBar(myControl);

            //timing the hypotheses
            if (parentControl.preferences.DisplayTiming)
            {
                TimeSpan timing = DateTime.Now - counter;
                MessageBox.Show(timing.Minutes.ToString() + "Minutes, " +
                    timing.Seconds.ToString() + "Seconds, " +
                    timing.Milliseconds.ToString() + "Miliseconds", "Time elapsed");
            }
        }

        /// <summary>
        /// Adapts the progress bar control with respect to new values
        /// </summary>
        /// <param name="value">value between 0 a 1 indicating progres of the box action,
        /// -1 stands for scrolling progress bar</param>
        /// <param name="message">message??</param>
        /// <param name="current__">some ICE stuff</param>
        public override void setValue(float value, string message, 
            Ice.Current current__)
        {
            if (value < 0) // == -1
            {
                myControl.SetProgressBarStyle(ProgressBarStyle.Marquee);
            }
            else
            {
                myControl.SetProgressBarStyle(ProgressBarStyle.Continuous);
                myControl.SetValue(value);
            }

            myControl.SetHint(message);
            //asyncAdapt();
        }

        /// <summary>
        /// Stops the current task
        /// </summary>
        public void stop()
        {
            if (task != null)
            {
                task.stop();
            }
        }

        //co kdyz uzivatel klikne 2x na stejnou akci - jak se to bude resit?
    }
}
