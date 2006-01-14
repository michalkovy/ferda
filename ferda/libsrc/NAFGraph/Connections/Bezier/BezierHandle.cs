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
	[Serializable] public class BezierHandle : BezierEntity
	{
		#region Fields

		protected HandleTypes mHandleType = HandleTypes.Symmetric;

		protected TangentHandle mTangent1;

		protected TangentHandle mTangent2;

		[NonSerialized] protected Pen pen ;

		protected BezierPainter mCurve = null;

		protected bool mVerticalConstraint = false;

		#endregion


		#region Properties

		public BezierPainter Curve
		{
			get{return mCurve;}
			set{mCurve = value;}
		}

		public HandleTypes HandleType
		{
			get{return mHandleType;}
			set{
				mHandleType = value;
				if(value==HandleTypes.Symmetric)
				{
					this.Tangent1.ChangeCotangent(Tangent1.CurrentPoint);
				}
			}
		}

		public TangentHandle Tangent1
		{
			get{return mTangent1;}
			set{mTangent1 = value;}
		}

		public TangentHandle Tangent2
		{
			get{return mTangent2;}
			set{mTangent2 = value;}
		}

		
		public bool VerticalConstraint
		{
			get{return mVerticalConstraint;}
			set{mVerticalConstraint = value;}
		}

		#endregion

		#region Constructor
		public BezierHandle(float x, float y)
		{
			this.mCurrentPoint = new PointF(x,y);
			Init();
		}

		public BezierHandle(float x, float y, HandleTypes type)
		{
			this.mCurrentPoint = new PointF(x,y);
			this.mHandleType = type;
			Init();
			if(type == HandleTypes.Single)
			{
				this.Tangent1.Enabled = true;
				this.Tangent2.Enabled = false;
			}
			
		}
		public BezierHandle()
		{
			mCurrentPoint = PointF.Empty;
			Init();
		}

		public BezierHandle(PointF p)
		{
			this.mCurrentPoint = p;
			Init();
		}
		public BezierHandle(PointF p, HandleTypes type)
		{
			this.mCurrentPoint = p;
			this.mHandleType = type;
			Init();
			if(type == HandleTypes.Single)
			{
				this.Tangent1.Enabled = true;
				this.Tangent2.Enabled = false;
			}
			
		}

		#endregion
		
		#region Methods

		private void Init()
		{
			mRectangle = new RectangleF(mCurrentPoint,new SizeF(6,6));
			mTangent1 = new TangentHandle(this, new PointF(mCurrentPoint.X-40,mCurrentPoint.Y+40));
			mTangent2 = new TangentHandle(this, new PointF(mCurrentPoint.X+40,mCurrentPoint.Y-40));
			
			//this helps the symmetric behavior
			mTangent1.Cotangent = mTangent2;
			mTangent2.Cotangent = mTangent1;

			//mTangent1.Enabled = false;
			pen = new Pen(Color.OrangeRed);

		}

		internal override bool Hit(PointF p)
		{		
			RectangleF r = new RectangleF(p, new SizeF(0,0));
			RectangleF env = new RectangleF(mCurrentPoint,new SizeF(8,8));
			//env.Offset(-4,-4);
			return env.Contains(r);
		}

		public override void ChangeLocation(PointF p)
		{	
			

			
			if(!mVerticalConstraint)
			{
				mTangent1.CurrentPoint=new PointF(p.X + Tangent1.CurrentPoint.X-mCurrentPoint.X, p.Y+Tangent1.CurrentPoint.Y-mCurrentPoint.Y);
				mTangent2.CurrentPoint=new PointF(p.X + Tangent2.CurrentPoint.X-mCurrentPoint.X, p.Y+Tangent2.CurrentPoint.Y-mCurrentPoint.Y);
				this.mCurrentPoint.X = p.X;
			}
			else
			{
				mTangent1.CurrentPoint=new PointF( Tangent1.CurrentPoint.X , p.Y+Tangent1.CurrentPoint.Y-mCurrentPoint.Y);
				mTangent2.CurrentPoint=new PointF( Tangent1.CurrentPoint.X , p.Y+Tangent2.CurrentPoint.Y-mCurrentPoint.Y);
			}
			this.mCurrentPoint.Y = p.Y;
			mRectangle = new RectangleF(mCurrentPoint,new SizeF(5,5));
				
		}


		internal override void Paint(Graphics g)
		{
			
			//mRectangle.Offset(-3,-3);
			if(mHovered) 
				//g.FillRectangle(Brushes.Red,this.mRectangle);
				g.DrawRectangle(pen,Rectangle.Round(new RectangleF(mCurrentPoint,new SizeF(8,8))));
			else
				g.FillRectangle(Brushes.Green,this.mRectangle);

			this.mTangent1.Paint(g);
			this.mTangent2.Paint(g);
		}

		#endregion



		
	}
}
