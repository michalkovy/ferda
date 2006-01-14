using System;
using System.Collections;
using System.Drawing;
namespace Netron.GraphLib.Utils
{
	/// <summary>
	/// Collects free arrows
	/// </summary>
	public class FreeArrowCollection : CollectionBase
	{
		public FreeArrowCollection()
		{
			
		}

		public int Add(FreeArrow arrow)
		{
			return this.InnerList.Add(arrow);
		}

		public FreeArrow this[int index]
		{
			get{return this.InnerList[index] as FreeArrow;}
		}

		public FreeArrow this[string name]
		{
			get
			{
				for(int k=0; k<this.InnerList.Count; k++)
				{
					if(this[k].Name == name)
						return this[k];
				}
				return null;
			}
		}

		/// <summary>
		/// Paints the collection of arrows on the given graphics
		/// </summary>
		/// <param name="g"></param>
		public void Paint(Graphics g)
		{
			for(int k=0; k<this.InnerList.Count; k++)
			{
				(this.InnerList[k] as FreeArrow).PaintArrow(g);
			}
		}


	}
}
