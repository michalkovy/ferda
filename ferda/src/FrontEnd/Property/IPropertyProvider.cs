using System;
using System.Collections.Generic;
using System.Text;
using Ferda.FrontEnd.External;

namespace Ferda.FrontEnd.Properties
{
    /// <summary>
    /// This is the interface a control must implement to enable
    /// showing the properties in the Ferda's property grid - 
    /// <see cref="T:Ferda.FrontEnd.Properties.FerdaPropertyGrid"/>
    /// </summary>
    public interface IPropertyProvider
    {
        /// <summary>
        /// Sets the displayer to know where to display the properties.
        /// </summary>
        IOtherObjectDisplayer Displayer
        {
            set;
        }
    }
}
