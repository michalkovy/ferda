using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Modules.MetabaseLayer
{
	/// <summary>
	/// 
	/// </summary>
    public class PropertySettingHelper
	{
		private Dictionary<string, PropertySetting> properties;
        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <value>The properties.</value>
		public Dictionary<string, PropertySetting> Properties
		{
			get { return properties; }
		}

        private PropertySettingHelper()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertySettingHelper"/> class.
        /// </summary>
        /// <param name="properties">The properties.</param>
		public PropertySettingHelper(PropertySetting[] properties)
		{
			this.properties = new Dictionary<string, PropertySetting>();
			foreach (PropertySetting property in properties)
			{
				this.properties.Add(property.propertyName, property);
			}
		}

        /// <summary>
        /// Gets the property of specified <c>propertyName</c>.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>Setting of property of specified <c>propertyName</c>.</returns>
		public PropertySetting GetProperty(string propertyName)
		{
			if (properties.ContainsKey(propertyName))
				return properties[propertyName];
			throw badPropertyNameError(propertyName);
		}

        /// <summary>
        /// Gets the double property specified <c>propertyName</c>.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>Double poperty value.</returns>
		public double GetDoubleProperty(string propertyName)
		{
			if (properties.ContainsKey(propertyName))
				return ((Ferda.Modules.DoubleT)properties[propertyName].value).doubleValue;
			throw badPropertyNameError(propertyName);
		}

        /// <summary>
        /// Gets the float property specified <c>propertyName</c>.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>Float poperty value.</returns>
		public float GetFloatProperty(string propertyName)
		{
			if (properties.ContainsKey(propertyName))
				return ((Ferda.Modules.FloatT)properties[propertyName].value).floatValue;
			throw badPropertyNameError(propertyName);
		}

        /// <summary>
        /// Gets the int property specified <c>propertyName</c>.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>Int poperty value.</returns>
		public int GetIntProperty(string propertyName)
		{
			if (properties.ContainsKey(propertyName))
				return ((Ferda.Modules.IntT)properties[propertyName].value).intValue;
			throw badPropertyNameError(propertyName);
		}

        /// <summary>
        /// Gets the long property specified <c>propertyName</c>.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>Long poperty value.</returns>
		public long GetLongProperty(string propertyName)
		{
			if (properties.ContainsKey(propertyName))
				return ((Ferda.Modules.LongT)properties[propertyName].value).longValue;
			throw badPropertyNameError(propertyName);
		}

        /// <summary>
        /// Gets the string property specified <c>propertyName</c>.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>String poperty value.</returns>
		public string GetStringProperty(string propertyName)
		{
			if (properties.ContainsKey(propertyName))
				return ((Ferda.Modules.StringT)properties[propertyName].value).stringValue;
			throw badPropertyNameError(propertyName);
		}

		private Exception badPropertyNameError(string propertyName)
		{
			return new ArgumentException("There is no property named " + propertyName,
				propertyName);
		}
	}
}
