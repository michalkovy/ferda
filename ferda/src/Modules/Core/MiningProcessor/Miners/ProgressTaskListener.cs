// ProgressTaskListener.cs - implementation of a progress task
//
// Authors: Tomáš Kuchaø <tomas.kuchar@gmail.com>      
// Commented by: Martin Ralbovský <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2006 Tomáš Kuchaø
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
using Ferda.Modules;

namespace Ferda.Guha.MiningProcessor.Miners
{
    /// <summary>
    /// Implementation of a progress task (task that takes long time
    /// to complete and shows its progress in a progress bar. 
    /// </summary>
    public class ProgressTaskListener : ProgressTaskDisp_
    {
        /// <summary>
        /// The associated mining processor (set only)
        /// </summary>
        public ProgressBarHandler MinningProcessor
        {
            set { _minningProcessor = value; }
        }

        /// <summary>
        /// The associated mining processor
        /// </summary>
        ProgressBarHandler _minningProcessor = null;

        /// <summary>
        /// Gets the progress value. -1 stands for unknown
        /// progress, otherwise it is a float from 0 to 1.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="current__">Ice stuff</param>
        /// <returns>Value of the progress</returns>
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

        /// <summary>
        /// If the task was stopped
        /// </summary>        
        private bool _stopped = false;

        /// <summary>
        /// If the task was stopped
        /// </summary>
        public bool Stopped
        {
            get { return _stopped; }
        }

        /// <summary>
        /// Stops the progress task, in this case stops the verification
        /// of relevant questions. The method is called from the outside
        /// (user initiated).
        /// </summary>
        /// <param name="current__">Ice stuff</param>
        public override void stop(Ice.Current current__)
        {
            _stopped = true;
        }
    }
}
