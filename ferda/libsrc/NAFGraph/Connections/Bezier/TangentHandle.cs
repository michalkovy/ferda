using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace Netron.GraphLib
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable] public class TangentHandle : BezierEntity
	{
		#region Fields
		[NonSerialized] Pen pen;
		
		BezierHandle handle;

		TangentHandle mCotangent;
		
		protected bool mEnabled = true;
		#endregion

		#region Properties

		public bool Enabled
		{
			get{return mEnabled;}
			set{mEnabled = value;}
		}

		public TangentHandle Cotangent
		{
			get{return mCotangent;}
			set{mCotangent = value;}
		}
		
		#endregion

		#region Constructor
		public TangentHandle(BezierHandle handle, PointF point)
		{
			this.handle = handle;
			this.mCurrentPoint = point;			
			mRectangle = new RectangleF(point,new SizeF(5,5));
			pen = new Pen(Color.Orange,1F);
		}

		#endregion


		#region Methods
		internal override void Paint(Graphics g)
		{
			if(mEnabled)
			{
				g.DrawLine(pen,handle.CurrentPoint,mCurrentPoint);
				if(this.mHovered)
					//g.FillRectangle(Brushes.Red,mRectangle);
					g.DrawRectangle(Pens.Turquoise,Rectangle.Round(new RectangleF(this.mCurrentPoint,new SizeF(10,10))));
				else
					g.FillRectangle(Brushes.Green,mRectangle);
			}
		}

		public override void ChangeLocation(PointF p)
		{
			//Debug.WriteLine(mCurrentPoint.X + "->" + p.X);
			mCurrentPoint = p;

			mRectangle = new RectangleF(mCurrentPoint,new SizeF(5,5));
			
		}

		internal void ChangeCotangent(PointF p)
		{
			if(handle.HandleType == HandleTypes.Symmetric)
			{
				mCotangent.CurrentPoint=new PointF(2*handle.CurrentPoint.X-mCurrentPoint.X,2*handle.CurrentPoint.Y - mCurrentPoint.Y);				
			}
		}

		internal override bool Hit(PointF p)
		{
			RectangleF r = new RectangleF(p, new SizeF(0,0));
			RectangleF env = new RectangleF(this.mCurrentPoint,new SizeF(10,10));
			env.Offset(-5,-5);
			mHovered = env.Contains(r);
			//Debug.WriteLine("(" + p.X + "," + p.Y + ") c " + "(" + mCurrentPoint.X + ","  + mCurrentPoint.Y +")");
			return mHovered;
		}

		#endregion

	}
}
