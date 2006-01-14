using System;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Runtime.CompilerServices;

namespace Netron.GraphLib
{
	/// <summary>
	/// A Bezier curve
	/// </summary>
	public class BezierPainter : ConnectionPainter
	{
		#region Fields
		
		protected bool showManips = false;

	
		protected readonly int division = 25;
		protected int step;
		protected BezierHandleCollection mHandles ;
		protected bool mTracking = false;
	

	

		#endregion

		#region Properties

		public PointF From 
		{
			get{return mHandles[0].CurrentPoint;}
			set{mHandles[0].CurrentPoint = value;}
		}

		public PointF To
		{
			get{return mHandles[mHandles.Count-1].CurrentPoint;}
			set{mHandles[mHandles.Count-1].CurrentPoint = value;}
		}

	
	
		public BezierHandleCollection Handles
		{
			get{return mHandles;}
			set
			{
				mHandles = value;
				for(int k =0; k<mHandles.Count; k++)
				{mHandles[k].Curve = this;}
			}
		}

		/// <summary>
		/// Overrides the default since this Bezier thing is based on handles rather than simple points
		/// </summary>
		public override PointF[] Points
		{
			get{return mPoints;}
			set{
				if(value.Length!=mHandles.Count)
					throw new Exception("Length of given handle locations must be equal to the amount of handles.");
				for(int k=0;k<value.Length;k++)
				{
					mHandles[k].ChangeLocation( value[k]);
				}
			}
		}
		#endregion

		#region Constructors
		//		public BezierPainter()
		//		{
		//			
		//			mHandles = new BezierHandleCollection(this);
		//			mHandles.Add(new BezierHandle(10,10));
		//			mHandles.Add( new BezierHandle(50,50));
		//			mHandles.Add( new BezierHandle(100,100));
		//			mHandles.Add(new BezierHandle(150,150));
		//			Init();
		//		}

//		public BezierPainter(BezierHandleCollection mHandles)
//		{
//		
//			if(mHandles.Count>1)
//			{
//				this.mHandles = mHandles;
//				for(int k =0; k<mHandles.Count; k++)
//				{mHandles[k].Curve = this;}
//			}
//			else
//				throw new Exception("A curve requires at least two handles");
//
//			Init();
//		}
//		public BezierPainter(PointF[] points)
//		{
//			if(points.Length>1)
//			{
//				mHandles = new BezierHandleCollection();
//				BezierHandle hd;
//				for(int k=0; k<points.Length; k++)
//				{
//					if(k==0 || k==points.Length-1) //start and final handle should be single-type					
//						hd = new BezierHandle(points[k],HandleTypes.Single);					
//					else
//						hd = new BezierHandle(points[k],HandleTypes.Symmetric);
//
//					hd.Curve = this;
//					mHandles.Add(hd);
//				}
//			}
//			else
//				throw new Exception("A curve requires at least two handles");
//
//			Init();
//		}

		public BezierPainter(Connection connection) : base(connection)
		{
			mPoints = mConnection.GetConnectionPoints();
			if(mPoints.Length>1)
			{
				mHandles = new BezierHandleCollection();
				BezierHandle hd;
					
				for(int k=1; k<mPoints.Length-1; k++)
				{
					if(k==1 || k==mPoints.Length-2) //start and final handle should be single-type					
						hd = new BezierHandle(mPoints[k],HandleTypes.Single);					
					else
						hd = new BezierHandle(mPoints[k],HandleTypes.Symmetric);

					hd.Curve = this;
					mHandles.Add(hd);
				}
			}
			else
				throw new Exception("A curve requires at least two handles");

			Init();
		}
		#endregion

		#region Methods

		internal override void AddConnectionPoint(PointF p)
		{

			//figure out where the new point is in the series
			int before = 0;
			for(int h =0;h<mHandles.Count-1; h++)
			{
				RectangleF rec = RectangleF.Union(new RectangleF(mHandles[h].CurrentPoint,new Size(5,5)),new RectangleF(mHandles[h+1].CurrentPoint,new Size(5,5)));
				if(rec.Contains(p))
				{
					before = h; break;
				}

			}
			BezierHandle hd = new BezierHandle(p);
			hd.Curve = this;
			this.mHandles.Insert(before+1,hd);
			Init();
		}


		/// <summary>
		/// Removes a connection point
		/// </summary>
		/// <param name="p"></param>
		internal override void RemoveConnectionPoint(PointF p)
		{
			for(int k=0; k<this.mHandles.Count; k++)
			{
				if(mHandles[k].Hit(p)) RemoveHandle(mHandles[k]);
			}
		}

		/// <summary>
		/// Removes an handle
		/// </summary>
		/// <param name="handle"></param>
		internal void RemoveHandle(BezierHandle handle)
		{
			if(mHandles.Count>2)
			{
				mHandles.Remove(handle);
				Init();
			}
			else
				throw new Exception("A curves requires at least to handles");		
		}
		private void Init()
		{
			pen =this.mConnection.pen;
			mPoints = new PointF[(mHandles.Count-1) * (division+2) ];
			step = Convert.ToInt32(100F/(division+1));
		}

		

		public override void Paint(Graphics g)
		{
			float perc;
		
			if(mConnection.From.ConnectorLocation!=ConnectorLocations.Unknown) 
				g.DrawLine(pen,mConnection.From.Location,mHandles[0].CurrentPoint);
	
			for(int h =0;h<mHandles.Count-1; h++)
			{
				//define the subpoints
				for(int k =0; k<division+2; k++)				
				{
					if(k==division+1) 
						perc = 1F;
					else
						perc = ((float) k * step)/100F;

					mPoints[h*(division+2) + k] = GetBezier(perc,mHandles[h+1],mHandles[h]);									
					//Debug.WriteLine(perc);
				}
			}
			
			//g.FillPolygon(Brushes.LightSlateGray,points);
			//draw the Bezier line			
			g.DrawLines(pen,mPoints);
			
//			if(mHover || showManips  || mSelected)
//			{
//				for(int m =0;m <mHandles.Count; m++)
//				{
//					mHandles[m].Paint(g);
//				}
//			}

			if(mConnection.To.ConnectorLocation!=ConnectorLocations.Unknown) 
				g.DrawLine(pen,mConnection.To.Location,mHandles[mHandles.Count-1].CurrentPoint);
		}

		private float B1(float t) { return t*t*t; }
		private float B2(float t) { return  3*t*t*(1-t); }
		private float B3(float t) { return   3*t*(1-t)*(1-t) ; }
		private float B4(float t) { return   (1-t)*(1-t)*(1-t) ; }

		private PointF GetBezier(float percent, BezierHandle handle1, BezierHandle handle2)
		{
			TangentHandle tangent1 = null, tangent2=null;
			if(handle1.HandleType==HandleTypes.Single)
			{
				if(handle1.Tangent1.Enabled) 
					tangent1 = handle1.Tangent1;
				else
					tangent1 = handle1.Tangent2;
			}
			else
				tangent1 = handle1.Tangent2;
			if(handle2.HandleType==HandleTypes.Single)
			{
				if(handle2.Tangent1.Enabled) 
					tangent2 = handle2.Tangent1;
				else
					tangent2 = handle2.Tangent2;
			}
			else
				tangent2 = handle2.Tangent1;
			return GetBezier(percent,handle1.CurrentPoint,tangent1.CurrentPoint,tangent2.CurrentPoint,handle2.CurrentPoint);
		}

		private PointF GetBezier(float percent,PointF C1, PointF C2,PointF C3,PointF C4) 
		{
			 
			float X = C1.X*B1(percent) + C2.X*B2(percent) + C3.X*B3(percent) + C4.X*B4(percent);
			float Y = C1.Y*B1(percent) + C2.Y*B2(percent) + C3.Y*B3(percent) + C4.Y*B4(percent);
			return new PointF(X,Y);
		}

	
	

			
			
		


		public override bool Hit(PointF p)
		{
			bool join = false;
			PointF p1, p2;
			RectangleF r1, r2;
			float o, u;
			PointF s;
			for(int v = 0; v<mPoints.Length-1; v++)
			{
						
				//this is the usual segment test
				//you can do this because the PointF object is a value type!
				p1 = mPoints[v]; p2 = mPoints[v+1];
	
				// p1 must be the leftmost point.
				if (p1.X > p2.X) { s = p2; p2 = p1; p1 = s; }

				r1 = new RectangleF(p1.X, p1.Y, 0, 0);
				r2 = new RectangleF(p2.X, p2.Y, 0, 0);
				r1.Inflate(3, 3);
				r2.Inflate(3, 3);
				//this is like a topological neighborhood
				//the connection is shifted left and right
				//and the point under consideration has to be in between.						
				if (RectangleF.Union(r1, r2).Contains(p))
				{
					if (p1.Y < p2.Y) //SWNE
					{
						o = r1.Left + (((r2.Left - r1.Left) * (p.Y - r1.Bottom)) / (r2.Bottom - r1.Bottom));
						u = r1.Right + (((r2.Right - r1.Right) * (p.Y - r1.Top)) / (r2.Top - r1.Top));
						join |= ((p.X > o) && (p.X < u));
					}
					else //NWSE
					{
						o = r1.Left + (((r2.Left - r1.Left) * (p.Y - r1.Top)) / (r2.Top - r1.Top));
						u = r1.Right + (((r2.Right - r1.Right) * (p.Y - r1.Bottom)) / (r2.Bottom - r1.Bottom));
						join |= ((p.X > o) && (p.X < u));
					}
				}


			}
			return join;
		}
		

	


//		internal BezierEntity Hit(PointF p)
//		{
//			for(int m =0;m <mHandles.Count; m++)
//			{
//				if(mHandles[m].Hit(p))
//				{
//					return mHandles[m];	
//				}
//
//				if(mHandles[m].Tangent1.Hit(p))
//				{
//					return mHandles[m].Tangent1; 
//				}
//				if(mHandles[m].Tangent2.Hit(p))
//				{
//					return mHandles[m].Tangent2; 
//				}
//			}
//			return null;
//		}

		#endregion
	}
}
