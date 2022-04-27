// ProgressBarHandler.cs - handler of progress bars for all GUHA mining processors
//
// Authors: Martin Ralbovský <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2007 Martin Ralbovský 
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
using Ferda.ModulesManager;
using Ferda.Modules;

namespace Ferda.Guha.MiningProcessor.Miners
{
    /// <summary>
    /// Class providing functionality to handle progress bars for
    /// the GUHA mining processors. The class was created because
    /// of the ETree mining processor. This miner does not resemble
    /// the other GUHA miner, yet it needs to communicate with the
    /// progress bar.
    /// </summary>
    public abstract class ProgressBarHandler
    {
        #region Private fields

        /// <summary>
        /// Progress task (a task that has its progress and can be displayed
        /// in the progress bar).
        /// </summary>
        private readonly ProgressTaskListener _progressListener;
        /// <summary>
        /// Proxy to a progress bar.
        /// </summary>
        private readonly ProgressBarPrx _progressBarPrx;

        /// <summary>
        /// The progress value (indicating how far is the computation).
        /// -1 indicates unknown value, otherwise the value is a float
        /// from 0 to 1.
        /// </summary>
        private float _progressValue = -1;
        /// <summary>
        /// The progress message shown to the user
        /// </summary>
        private string _progressMessage = "Loading ...";
        /// <summary>
        /// Ticks (precise time) of the last update of the 
        /// progress bar
        /// </summary>
        private long _progressLastUpdateTicks = DateTime.Now.Ticks;
        /// <summary>
        /// Minimum change in number of ticks for the miner to publish its
        /// progress. 
        /// </summary>
        private const long _progressMinCountOfTicksToPublish = 500000;

        // ticks:
        // 1 tick = 100-nanosecond 
        // nano is 0.000 000 001
        // mili is 0.001
        // 0.05 sec is ticks * 500 000

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor of the class
        /// </summary>
        /// <param name="progressListener">The progress listener.</param>
        /// <param name="progressBarPrx">The progress bar PRX.</param>
        protected ProgressBarHandler(ProgressTaskListener progressListener,
            ProgressBarPrx progressBarPrx)
        {
            _progressListener = progressListener;

            if (_progressListener != null)
                _progressListener.MinningProcessor = this;

            _progressBarPrx = progressBarPrx;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the value of progress of the task.
        /// -1 indicates unknown value, otherwise the value is a float
        /// from 0 to 1
        /// </summary>
        /// <param name="message">In this parameter, the message to the user
        /// is stored</param>
        /// <returns>The progress value</returns>
        public float ProgressGetValue(out string message)
        {
            message = _progressMessage;
            return _progressValue;
        }

        /// <summary>
        /// Sets the progress value to a progress bar proxy. 
        /// -1 indicates unknown value, otherwise the value is a float
        /// from 0 to 1.
        /// </summary>
        /// <param name="value">Value to be set.</param>
        /// <param name="message">Message to be set</param>
        /// <returns>If false, the task was stopped by the user</returns>
        public bool ProgressSetValue(float value, string message)
        {
            if ((_progressBarPrx != null) && (_progressListener != null))
            {
                _progressValue = value;
                _progressMessage = message;

                long actTicks = DateTime.Now.Ticks;
                if (System.Math.Abs(_progressLastUpdateTicks - actTicks) > _progressMinCountOfTicksToPublish)
                {
                    _progressLastUpdateTicks = actTicks;
                    try
                    {
                        _progressBarPrx.setValue(value, message);
                    }
                    catch (Ice.ObjectNotExistException)
                    {
                        // because one thread can destroy progress bar this exception is possible
                    }
                }

                if (_progressListener.Stopped)
                    return false;
            }
            return true;
        }

        #endregion
    }
}
