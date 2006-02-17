// FerdaPropertySpec.cs - a better PropertySpec class with information about the type of the property
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
    /// A standard <see cref="T:Ferda.FrontEnd.External.PropertySpec"/> class
    /// with information about the type of the property - if it is a socket
    /// property or a normal property
    /// </summary>
    internal class FerdaPropertySpec : PropertySpec
    {
        private bool socketProperty;

        /// <summary>
        /// Deterimes if this property belongs to a socket or if it is
        /// a normal property
        /// </summary>
        public bool SocketProperty
        {
            set
            {
                socketProperty = value;
            }
            get
            {
                return socketProperty;
            }
        }

        /// <summary>
        /// Constructor that should be used for the class
        /// </summary>
        /// <param name="propName">Name of the property</param>
        /// <param name="propType">Type of the property</param>
        /// <param name="sockProp">if it is a socket property</param>
        public FerdaPropertySpec(string propName, string propType, bool sockProp)
            : base(propName, propType)
        {
            this.SocketProperty = sockProp;
        }

        /// <summary>
        /// Other constructor that should be used
        /// </summary>
        /// <param name="propName">Name of the property</param>
        /// <param name="type">Type of the property</param>
        /// <param name="sockProp">if it is a socket property</param>
        public FerdaPropertySpec(string propName, Type type, bool sockProp)
            : base(propName, type)
        {
            this.SocketProperty = sockProp;
        }
    }
}
