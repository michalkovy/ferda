using System;
using System.Collections.Generic;
using System.Text;
public delegate void IceTick();

namespace Ferda.FrontEnd.AddIns.ResultBrowser.NonGUIClasses
{
    /// <summary>
    /// Event signalizing communication through Ice is running
    /// </summary>
    interface IceTicked
    {
        event IceTick IceTicked;
        void OnIceTick();
    }
}