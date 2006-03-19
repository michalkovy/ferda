using System;
using System.Collections.Generic;
using System.Text;
public delegate void LongRunCompleted();

namespace Ferda.FrontEnd.AddIns.ResultBrowser.NonGUIClasses
{
    /// <summary>
    /// Event signalizing communication through Ice is running
    /// </summary>
    interface LongRunComplete
    {
        event LongRunCompleted LongRunComplete;
        void OnLongRunComplete();
    }
}