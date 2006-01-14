using System;
using System.Collections;
namespace Netron.GraphLib
{
	/// <summary>
	/// Collection of shape objects
	/// </summary>
	[Serializable] public class ShapeCollection : CollectionBase
	{	
		#region Constructor
		public ShapeCollection()
		{
			
		}
		#endregion

		#region Methods
		public object Clone()
		{
			return this.InnerList.Clone();
		}
		public int Add(Shape shape)
		{
			return this.InnerList.Add(shape);
		}

		public Shape this[int index]
		{
			get{
				if(index>-1 && index<this.InnerList.Count)
					return this.InnerList[index] as Shape;
				else
					return null;
			}
		}
		public bool Contains(object shape)
		{
			if(shape is Shape)
				return this.InnerList.Contains(shape);
			else
				return false;
		}

		

		public void Remove(Shape shape)
		{
			this.InnerList.Remove(shape);
		}

		#endregion
	
	}
}
