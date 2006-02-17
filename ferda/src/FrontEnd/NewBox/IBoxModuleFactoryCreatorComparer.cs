// IBoxModuleFactoryCreatorComparer.cs - class that compares 2 IBoxModules
//
// Author: Martin Ralbovský <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2005 Martin Ralbovský
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


namespace Ferda.FrontEnd.NewBox
{
    /// <summary>
    /// The class compares two IBoxModuleFactoryCreator classes. It compares them
    /// according to their labels
    /// </summary>
    internal class IBoxModuleFactoryCreatorComparer : IComparer<IBoxModuleFactoryCreator>
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
