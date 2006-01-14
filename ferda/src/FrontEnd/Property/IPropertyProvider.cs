using System;
using System.Collections.Generic;
using System.Text;
using Ferda.FrontEnd.External;

namespace Ferda.FrontEnd.Properties
{
    /// <summary>
    /// This is the interface a control must implement to enable
    /// showing the properties in the Ferda's property grid
    /// </summary>
    public interface IPropertyProvider
    {
        /// <summary>
        /// Sets the displayer to know where to display the properties.
        /// </summary>
        IPropertiesDisplayer Displayer
        {
            set;
        }

        /*
        /// <summary>
        /// Determines if in the provider is one ore more objects selected
        /// </summary>
        bool IsOneObjectSelected
        {
            get;
        }

        /// <summary>
        /// Gets the<see cref="T:Ferda.FrontEnd.External.PropertyTable"/> selected object
        /// that holds the information about the properties
        /// </summary>
        PropertyTable SelectedObject
        {
            get;
        }

        /// <summary>
        /// Gets the<see cref="T:Ferda.FrontEnd.External.PropertyTable"/> selected objects
        /// that holds the information about the properties
        /// </summary>
        PropertyTable[] SelectedObjects
        {
            get;
        }
        */
    }
}
