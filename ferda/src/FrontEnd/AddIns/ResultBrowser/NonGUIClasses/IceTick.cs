using System;
using System.Collections.Generic;
using System.Text;
public delegate void LongRunTick();

namespace Ferda.FrontEnd.AddIns.ResultBrowser.NonGUIClasses
{
    /// <summary>
    /// Event signalizing communication through Ice is running
    /// </summary>
    interface LongRunTicked
    {
        event LongRunTick LongRunTicked;
        void OnLongRunTick();
    }
}