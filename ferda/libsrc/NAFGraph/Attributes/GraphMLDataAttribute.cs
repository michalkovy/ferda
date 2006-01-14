
using System;
using System.Collections;
using System.Reflection;

namespace Netron.GraphLib.Attributes
{
	/// <summary>
	/// Attribute class for designating which properties will be serialized
	/// by the GraphML serializer.
	/// </summary>
	/// 
	[AttributeUsage(AttributeTargets.Property,AllowMultiple = false)]
	public class GraphMLDataAttribute : System.Attribute
	{
		private delegate void ToStringDelegate();
		private delegate void FromStringDelegate();

		public GraphMLDataAttribute()
		{

		}

		public static Hashtable GetValuesOfTaggedFields(object value) 
		{
			Hashtable vs = new Hashtable();

			foreach (PropertyInfo pi in value.GetType().GetProperties()) 
			{	
				if (Attribute.IsDefined(pi, typeof(GraphMLDataAttribute))) 
				{
					vs.Add(pi.Name,pi.GetValue(value,null));
				}
			}

			return vs;
		}
	}
}
