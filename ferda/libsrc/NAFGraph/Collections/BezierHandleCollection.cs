using System;
using System.Collections;
using System.Drawing;
namespace Netron.GraphLib
{
	/// <summary>
	/// 
	/// </summary>
	public class BezierHandleCollection : CollectionBase
	{

		#region Fields
		protected BezierPainter mCurve = null;

		#endregion

		#region Properties

		public BezierPainter Curve
		{
			get{return mCurve;}
			set{mCurve = value;}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Constructs a collection and assigns the collection to the given curve
		/// </summary>
		/// <param name="curve"></param>
		public BezierHandleCollection(BezierPainter curve)
		{
			this.mCurve = curve;
		}
		public BezierHandleCollection()
		{			
		}
		/// <summary>
		/// Constrcuts a collection on the basis of a PointF collection
		/// </summary>
		/// <param name="list">An ArrayList of PointF's</param>
		public BezierHandleCollection(ArrayList list)
		{
			BezierHandle hl;
			for(int k=0;k<list.Count; k++)
			{
				hl = new BezierHandle((PointF) list[k]);
				Add(hl);
			}
		}
		#endregion

		#region Methods
		public void Remove(BezierHandle handle)
		{
			this.InnerList.Remove(handle);
		}
		public int Add(BezierHandle handle)
		{
			return this.InnerList.Add(handle);
		}

		public void Insert(int index, BezierHandle handle)
		{
			this.InnerList.Insert(index, handle);
		}

		public BezierHandle this[int index]
		{
			get{return this.InnerList[index] as BezierHandle;}
		}

		#endregion
	}
}
