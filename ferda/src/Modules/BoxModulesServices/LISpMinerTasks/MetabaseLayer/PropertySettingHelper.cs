using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Modules.MetabaseLayer
{
	public class PropertySettingHelper
	{
		private Dictionary<string, PropertySetting> properties;
		public Dictionary<string, PropertySetting> Properties
		{
			get { return properties; }
		}

		public PropertySettingHelper(PropertySetting[] properties)
		{
			this.properties = new Dictionary<string, PropertySetting>();
			foreach (PropertySetting property in properties)
			{
				this.properties.Add(property.propertyName, property);
			}
		}

		public PropertySetting GetProperty(string propertyName)
		{
			if (properties.ContainsKey(propertyName))
				return properties[propertyName];
			throw badPropertyNameError(propertyName);
		}

		public double GetDoubleProperty(string propertyName)
		{
			if (properties.ContainsKey(propertyName))
				return ((Ferda.Modules.DoubleT)properties[propertyName].value).doubleValue;
			throw badPropertyNameError(propertyName);
		}

		public float GetFloatProperty(string propertyName)
		{
			if (properties.ContainsKey(propertyName))
				return ((Ferda.Modules.FloatT)properties[propertyName].value).floatValue;
			throw badPropertyNameError(propertyName);
		}

		public int GetIntProperty(string propertyName)
		{
			if (properties.ContainsKey(propertyName))
				return ((Ferda.Modules.IntT)properties[propertyName].value).intValue;
			throw badPropertyNameError(propertyName);
		}

		public long GetLongProperty(string propertyName)
		{
			if (properties.ContainsKey(propertyName))
				return ((Ferda.Modules.LongT)properties[propertyName].value).longValue;
			throw badPropertyNameError(propertyName);
		}

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
