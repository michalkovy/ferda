using System;

namespace Ferda.FrontEnd.AddIns
{
    /// <summary>
    /// A control should implement this interface to
    /// dynamically show properties of some other control
    /// </summary>
    public interface IFerdaPropertiesDisplayer
    {
        /// <summary>
        /// Adapts the property grid with a new object
        /// </summary>
        void AdaptPropertyGrid();
    }
}
