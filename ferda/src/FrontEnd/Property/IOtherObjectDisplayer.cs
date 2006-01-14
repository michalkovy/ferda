using System;
using System.Collections.Generic;
using System.Text;
using Ferda.FrontEnd.External;

namespace Ferda.FrontEnd.Properties
{
    /// <summary>
    /// An interface for the FerdaPropertyGrid to display properties other than 
    /// properties of boxes. It uses the <see cref="T:Ferda.FrontEnd.External.PropertyTable"/>
    /// object to display its properties. There can be only one object displayed in
    /// the PropertyGrid at one time.
    /// </summary>
    public interface IOtherObjectDisplayer
    {
        /// <summary>
        /// Adapts the PropertyGrid when another object than a box is selected.
        /// Uses the SelectedObject as its object.
        /// </summary>
        /// <param name="objectProperties">The 
        /// <see cref="T:Ferda.FrontEnd.External.PropertyTable"/> object
        /// that contains information about the object.
        /// </param>
        void OtherObjectAdapt(PropertyTable objectProperties);

        /// <summary>
        /// Resets the propetry grid to be without properties
        /// </summary>
        void Reset();
    }
}
