using System;
using System.Collections.Generic;
using System.Text;
using Ferda.ModulesManager;


namespace Ferda.FrontEnd.NewBox
{
    /// <summary>
    /// The class compares two IBoxModuleFactoryCreator classes. It compares them
    /// according to their labels
    /// </summary>
    class IBoxModuleFactoryCreatorComparer : IComparer<IBoxModuleFactoryCreator>
    {
        #region Interface methods

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less 
        /// than, equal to or greater than the other. 
        /// </summary>
        /// <param name="x">The first object to compare.
        ///</param>
        /// <param name="y">The second object to compare.
        ///</param>
        /// <returns>Value</returns>
        public int Compare(IBoxModuleFactoryCreator x, IBoxModuleFactoryCreator y)
        {
            return String.Compare(x.Label, y.Label);
        }

        /// <summary>
        /// Determines whether the specified objects are equal. 
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>true if the specified objects are equal; otherwise, false. </returns>
        public bool Equals(IBoxModuleFactoryCreator x, IBoxModuleFactoryCreator y)
        {
            return String.Equals(x.Identifier, y.Identifier);
        }

        #endregion
    }
}
