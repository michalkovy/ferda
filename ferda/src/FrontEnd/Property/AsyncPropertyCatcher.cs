// AsyncPropertyCatcher.cs - class that provides asynchronous prpoerty catching
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
using Ferda.Modules;

namespace Ferda.FrontEnd.Properties
{
    /// <summary>
    /// This class provides functionality for asynchronous property catching
    /// (getting). Through the ice_response method it notifies the propertyGrid, 
    /// that a property has been changed.
    /// </summary>
    public class AsyncPropertyCatcher : AMI_BoxModule_getProperty
    {
        #region Fields

        private IAsyncPropertyManager myManager;
        private string propertyName;
        private string propertyType;
        private bool moreBoxes = false;

        #endregion

        #region Properties

        /// <summary>
        /// Name (not the label) of the property that is beeing changed
        /// </summary>
        public string PropertyName
        {
            get { return propertyName; }
            set { propertyName = value; }
        }

        /// <summary>
        /// Type of the property (for good type conversion in the 
        /// propertyGrid.Temporary values
        /// </summary>
        public string PropertyType
        {
            get { return propertyType; }
            set { propertyType = value; }
        }

        /// <summary>
        /// Determines, if on the desktop there are more than one
        /// boxes selected
        /// </summary>
        public bool MoreBoxes
        {
            get { return moreBoxes; }
            set { moreBoxes = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="man">Manager of this property</param>
        /// <param name="propName">Name of the property</param>
        /// <param name="propertyType">Type of the property</param>
        public AsyncPropertyCatcher(IAsyncPropertyManager man, string propName,
            string propertyType)
        {
            myManager = man;
            propertyName = propName;
            this.propertyType = propertyType;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="man">Manager of this property</param>
        /// <param name="propName">Name of the property</param>
        /// <param name="propertyType">Type of the property</param>
        /// <param name="moreBoxes">
        /// Determines, if on the desktop there are more than one
        /// boxes selected        
        /// </param>
        public AsyncPropertyCatcher(IAsyncPropertyManager man, string propName,
            string propertyType, bool moreBoxes)
            : this(man, propName, propertyType)
        {
            this.moreBoxes = moreBoxes;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Ice has thrown a message, that a value of a property has been changed
        /// </summary>
        /// <param name="value">value of the property</param>
        public override void ice_response(Ferda.Modules.PropertyValue value)
        {
            myManager.ChangedPropertyValue(this, value, moreBoxes);
        }

        /// <summary>
        /// Ice has thrown an exception
        /// </summary>
        /// <param name="ex">ICE exception</param>
        public override void ice_exception(Ice.Exception ex)
        {
        }

        #endregion
    }
}
