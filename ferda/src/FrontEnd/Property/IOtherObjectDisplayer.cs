// IOtherObjectDisplayer.cs - Interface to display other boxes that the IBoxModule in
// the property grid
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
