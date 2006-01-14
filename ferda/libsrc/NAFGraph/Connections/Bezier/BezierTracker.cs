using System;
using System.Collections;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

namespace Netron.GraphLib
{
	/// <summary>
	/// Tracker for connection objects.
	/// </summary>
	[Serializable]
	public class BezierTracker : ConnectionTracker
	{

		protected BezierPainter mCurve = null;

		protected BezierHandleCollection mHandles = null;

		public BezierTracker(ArrayList l, Boolean resizable) :base(l,resizable)
		{}
		public BezierTracker(ArrayList l):base(l)
		{}
		public BezierTracker(BezierPainter curve) : base(curve.Points)
		{
			mCurve = curve;
			mHandles = curve.Handles;
		}
		public BezierTracker(Connection connection): base(connection.GetConnectionPoints())
		{
			mCurve = connection.mPainter as BezierPainter;
			mHandles = mCurve.Handles;
		}
		public override void Paint(Graphics g)
		{
			
			if (!Resizable) 
				return;
			for(int m =0;m <mHandles.Count; m++)
			{
				mHandles[m].Paint(g);
			}
		}

		public override Point Hit(PointF p)
		{
			for(int m =0;m <mHandles.Count; m++)
			{
				if(mHandles[m].Hit(p))
				{
					mHandles[m].Tracking = true; 
					mTrack = true;
					//return Point.Round(p);		
					return new Point(m,0);
				}
				if(mHandles[m].Tangent1.Hit(p))
				{
					mHandles[m].Tangent1.Tracking = true; 
					mTrack = true;
					//return Point.Round(p);		
					return new Point(m,1);
				}
				if(mHandles[m].Tangent2.Hit(p))
				{
					mHandles[m].Tangent2.Tracking = true; 
					mTrack = true;
					//return Point.Round(p);
					return new Point(m,2);
				}
			}
			mTrack = false;
//			if(HitCurve(p))
//				showManips = true;			
//			else
//				showManips = false;
			return Point.Empty;
		}

		public override void Start(PointF p, Point h)
		{
			mCurrentHandle = h;
			mCurrentPoint = p;
			mTrack = true;
			return;
		}

		public override void Move(PointF p, Size maxsize, bool snap, int snapsize)
		{	
			if(!mTrack) return;
			if(mCurrentHandle==Point.Empty) return;

			if(mCurrentHandle.Y==0)
			{
					mHandles[mCurrentHandle.X].ChangeLocation(p);
					//Debug.WriteLine("(" + p.X + "," + p.Y + ") ");
			}
			else if (mCurrentHandle.Y ==1)
			{
				mHandles[mCurrentHandle.X].Tangent1.ChangeLocation(p);
				mHandles[mCurrentHandle.X].Tangent1.ChangeCotangent(p);
			}
			else if (mCurrentHandle.Y ==2)
			{
				mHandles[mCurrentHandle.X].Tangent2.ChangeLocation(p);
				mHandles[mCurrentHandle.X].Tangent2.ChangeCotangent(p);
			}


			return;

		}

		public override void MoveAll(PointF p)
		{
			

			for( int i=0; i<this.mHandles.Count; i++) 
			{
				PointF pt = mHandles[i].CurrentPoint; 

				PointF delta = new PointF( p.X - mCurrentPoint.X, p.Y - mCurrentPoint.Y);

				pt.X += delta.X;
				pt.Y += delta.Y;
				mHandles[i].ChangeLocation( pt);
			}			
			base.MoveAll (p); //in case the user switches back to the base connection tracker
		}


	}
}
