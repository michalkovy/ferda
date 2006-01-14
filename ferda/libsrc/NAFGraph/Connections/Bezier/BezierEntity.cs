using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace Netron.GraphLib
{
	/// <summary>
	/// Summary description for Entity.
	/// </summary>
	public abstract class BezierEntity
	{

		#region Fields
		protected bool mTracking = false;

		protected PointF mCurrentPoint;

		protected RectangleF mRectangle ;


		protected bool mHovered = false;

		#endregion

		#region Properties

		public bool Hovered
		{
			get{return mHovered;}
			set{mHovered = value;}
		}


		public bool Tracking
		{
			get{return mTracking;}
			set{mTracking = value;}		
		}

		public PointF CurrentPoint
		{
			get{return mCurrentPoint;}
			set{
				mCurrentPoint = value;
				mRectangle = new RectangleF(mCurrentPoint,new SizeF(5,5));
			}
		}

		#endregion

		#region Constructor
		public BezierEntity()
		{			
		}
		#endregion

		#region Methods

		internal abstract bool Hit(PointF p);

		internal abstract void Paint(Graphics g);

		public abstract void ChangeLocation(PointF p);


		#endregion
	}
}
