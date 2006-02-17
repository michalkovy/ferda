// IBoxSelector.cs - Control implementing this interface should be able to select a box
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

namespace Ferda.FrontEnd
{
    /// <summary>
    /// Control implementing this interface should be able to select a box
    /// </summary>
    /// <example>
    /// FerdaDesktop implements this interface and is able to select a box 
    /// on the desktop with a red rectangle around.
    /// </example>
    public interface IBoxSelector
    {
        /// <summary>
        /// When a box is selected in the archive, it should also be selected on the 
        /// view. This function selects the box in the desktop
        /// </summary>
        /// <param name="box">Box to be selected</param>
        void SelectBox(IBoxModule box);
    }
}
