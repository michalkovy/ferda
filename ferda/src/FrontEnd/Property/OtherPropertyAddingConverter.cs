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
    /// The converter for all the Ferda OtherProperties
    /// It converts the property to and from a string.
    /// </summary>
    public class OtherPropertyAddingConverter : TypeConverter
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

        /// <summary>
        /// Determines if the class can be converted from other class (string)
        /// </summary>
        /// <param name="context">Context of the conversion</param>
        /// <param name="destinationType">Source type</param>
        /// <returns>true if it can be converted, false otherwise</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, 
            Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            else
            {
                return base.CanConvertFrom(context, sourceType);
            }
        }

        /// <summary>
        /// Converts from an object
        /// </summary>
        /// <param name="context">Context of the conversion</param>
        /// <param name="culture">Culture info</param>
        /// <param name="value">Value to be converted</param>
        /// <returns>Object after conversion (a OtherProperty object)</returns>
        public override object ConvertFrom(ITypeDescriptorContext context, 
            System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                string result = value as string;
                OtherProperty op = new OtherProperty(result);

                return op;
            }
            else
            {
                return base.ConvertFrom(context, culture, value);
            }
        }

    }
}
