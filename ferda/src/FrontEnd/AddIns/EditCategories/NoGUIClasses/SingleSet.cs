// Interval.cs - class for working with enumerations
//
// Author: Alexander Kuzmin <alexander.kuzmin@gmail.com>
//
// Copyright (c) 2005 Alexander Kuzmin
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
using System.Data;
using System.Collections;
using Ferda;

namespace Ferda.FrontEnd.AddIns.EditCategories
{
    /// <summary>
    /// Class for a single set of values.
    /// </summary>
    public class SingleSet
    {
        #region Constructor

        /// <summary>
        /// Constructor which fills in the values for a set.
        /// </summary>
        /// <param name="Values">Values to be added</param>
        public SingleSet(ArrayList Values)
        {
            this.values = Values;
            this.values.Sort();
        }

        #endregion


        #region Private variables

        /// <summary>
        /// Values in the set.
        /// </summary>
        private ArrayList values = new ArrayList();

        public ArrayList Values
        {
            get
            {
                return this.values;
            }
        }

        #endregion


        #region Private methods

        /// <summary>
        /// Method to detect whether set contains the required value
        /// </summary>
        /// <param name="value">Value to check for</param>
        /// <returns></returns>
        public bool IsInSet(int value)
        {
            if (this.values.Contains(value))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Method to detect whether set contains the required value
        /// </summary>
        /// <param name="value">Value to check for</param>
        /// <returns></returns>
        public bool IsInSet(object value)
        {
            if (this.values.Contains(value))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Method to add the value to set
        /// </summary>
        /// <param name="value">Value to add</param>
        public void AddValue(object value)
        {
            this.values.Add(value);
        }

        #endregion
    }
}