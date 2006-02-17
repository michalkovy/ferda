// IAsyncPropertyManager.cs - Interface to get asynchronous property getting
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

namespace Ferda.FrontEnd.Properties
{
    /// <summary>
    /// The interface is implemented when a control wants asynchronous
    /// changing of properties
    /// </summary>
    public interface IAsyncPropertyManager
    {
        /// <summary>
        /// The property value is changed, so the the propertyGrid should be
        /// refilled with new values
        /// </summary>
        /// <param name="catcher">Catcher of the connection</param>
        /// <param name="value">New value of the property</param>
        /// <param name="moreBoxes">If the refresh of the property grid is 
        /// from one or more boxes</param>
        void ChangedPropertyValue(AsyncPropertyCatcher catcher, object value, 
            bool moreBoxes);
    }
}
