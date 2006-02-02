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
    /// The converter for all the Ferda OtherProperties.
    /// It converts the property to string.
    /// </summary>
    internal class OtherPropertyConverter : TypeConverter
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
                return base.CanConvertFrom(context, destinationType);
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
                OtherProperty prop = value as OtherProperty;

                return prop.Box.GetPropertyOtherAbout(prop.PropertyName);
            }
            else
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }
    }
}
