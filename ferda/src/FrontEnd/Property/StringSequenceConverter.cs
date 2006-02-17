// StringSequenceConverter.cs - editor for StringSequence
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
using System.ComponentModel;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace Ferda.FrontEnd.Properties
{
    /// <summary>
    /// This class converts the StringSequence class to and from string.
    /// It does not allow adding a new value
    /// </summary>
    internal class StringSequenceConverter : TypeConverter
    {
        /// <summary>
        /// Determines if the class can be converted to other class
        /// </summary>
        /// <param name="context">Context of the conversion</param>
        /// <param name="destinationType">Destination type</param>
        /// <returns>true if it can be converted, false otherwise</returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, 
            Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return true;
            }
            else
            {
                return base.CanConvertTo(context, destinationType);
            }
        }

        /// <summary>
        /// Converts the object into other object with selected type
        /// </summary>
        /// <param name="context">Context of the conversion</param>
        /// <param name="culture">Culture info</param>
        /// <param name="value">Value of the object</param>
        /// <param name="destinationType">Type to be converted to</param>
        /// <returns>New object in designated type</returns>
        public override object ConvertTo(ITypeDescriptorContext context, 
            CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                if (value == null)
                {
                    return string.Empty;
                }

                StringSequence seq = value as StringSequence;
                return seq.selectedLabel;
            }
            else
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }
    }
}
