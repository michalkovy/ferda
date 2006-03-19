using System;
using System.Collections.Generic;
using System.Text;
public delegate void ThreadCompleted();

namespace Ferda.FrontEnd.AddIns.ResultBrowser.NonGUIClasses
{
    /// <summary>
    /// Event signalizing communication through Ice is running
    /// </summary>
    interface ThreadComplete
    {
        event ThreadCompleted ThreadCompleted;
        void OnThreadCompleted();
    }
}