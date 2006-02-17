// IPropertyProvider.cs - Interface to provide the properties for the Ferda property grid
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
