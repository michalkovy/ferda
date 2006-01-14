using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

using System.ComponentModel.Design;
using System.Drawing.Design;
using Netron.GraphLib.Interfaces;
using Netron.GraphLib.Configuration;
using Netron.GraphLib.Attributes;
namespace Netron.GraphLib
{
	/// <summary>
	/// The connection class captures a connection between two connectors and is derived mFrom an entity.
	/// </summary>
	
	[Serializable] public class Connection : Entity
	{
			
		#region Fields
		/// <summary>
		/// the label near the ends of the connection
		/// </summary>
		protected internal bool showEndsLabel = false;

		/// <summary>
		/// Holds the polyline data
		/// </summary>
		protected ArrayList insertionPoints;

		protected float lineWidth = 1F;

		protected float arrowOffset = 22F;

		/// <summary>
		/// default rest length of the connection
		/// </summary>
		protected int mRestLength = 100;

		/// <summary>
		/// This is a public floating point assigned by the canvascontrol in the MouseMove and 
		/// MouseDown events. It makes it possible mTo show a drawn line before there is an actual link between
		/// two connectors.
		/// </summary>
		protected PointF mToPoint;
		/// <summary>
		/// The starting connector
		/// </summary>
		protected Connector mFrom = null;
		/// <summary>
		/// the destination connector
		/// </summary>
		protected Connector mTo = null;
		/// <summary>
		/// The line color
		/// </summary>
		protected Color mLineColor=Color.Black;
		/// <summary>
		/// The line style (Solid, Dashed...)
		/// </summary>
        protected DashStyle mLineStyle = DashStyle.Solid;
		/// <summary>
		/// The line weight; thin, medium or fat. Could be set mTo arbitrary size.
		/// </summary>
		protected ConnectionWeight mLineWeight = ConnectionWeight.Thin;
		/// <summary>
		/// The type of arrow or line end
		/// </summary>
        protected ConnectionEnds mLineEnd = ConnectionEnds.NoEnds;

		/// <summary>
		/// the shape of the connection
		/// </summary>
		protected string mLinePath = "Default";
			
		/// <summary>
		/// the painter class used to paint the connection
		/// </summary>
		protected internal ConnectionPainter mPainter = null;
		/// <summary>
		/// Tracker used for the connection
		/// </summary>
		protected ConnectionTracker   mTracker = null;
		#endregion
			
		#region Properties

		protected internal ConnectionPainter Painter
		{
			get{return mPainter;}			
		}

		/// <summary>
		/// Gets or sets whether theconnection is selected
		/// </summary>
		protected internal override bool IsSelected
		{
			get { return base.IsSelected; }
			set 
			{
				base.IsSelected = value; 

				if(this.mLinePath == "Bezier")
				{
					mPainter.Selected = value;
					mTracker = new BezierTracker(this);
				}
				else
					mTracker = new ConnectionTracker(insertionPoints,true);
				
				Invalidate(); 
			}
		}


		/// <summary>
		/// Gets or set the tracker associated with the connection
		/// </summary>
		public ConnectionTracker Tracker
		{
			get { return mTracker; }
			set { mTracker = value; }
		}

		public void AddConnectionPoint(PointF point)
		{
			this.insertionPoints.Add(point);
			//the test is necessary if the developer doesn't implement a separate painter but paints in this class (which is bad bad)
			if(mPainter!=null) mPainter.AddConnectionPoint(point); 
		}

		public void RemoveConnectionPoint(PointF point)
		{
			RectangleF r = new RectangleF(point.X,point.Y,0,0);
			RectangleF s;
			
			for(int m=0;m<this.insertionPoints.Count;m++)
			{
				s = new RectangleF((PointF) insertionPoints[m],new SizeF(50,50));
				s.Offset(-25,-25);//center around the point
				if(s.Contains(r))
				{
					insertionPoints.RemoveAt(m);
					mPainter.RemoveConnectionPoint(point);
					return;
				}
			}
		}

		/// <summary>
		/// Gets or sets the type of connection or shape of the path.
		/// </summary>
		public string LinePath
		{
			get{return mLinePath;}
			[Browsable(false)] set
							   {
								   mLinePath = value;
								   switch(value)
								   {
									   case "Bezier":
										   //this now creates a Bezier curve with the given points and a default set of tangent handles
										   mPainter = new BezierPainter(this);
										   mTracker = new BezierTracker(this);
										   break;
									   case "Default":
										   mPainter = new DefaultPainter(this);
										   mTracker = new ConnectionTracker(insertionPoints,true);
										   break;
									   case "Rectangular":
										   mPainter = new RectangularPainter(this);
										   mTracker = new ConnectionTracker(insertionPoints,true);
										   break;
									   default:
											mTracker = new ConnectionTracker(insertionPoints,true);
										   //we have a custom connection
										   ConnectionSummary consum = this.mSite.Libraries.GetConnectionSummary(value);
										   if(consum==null)
										   {
											   Debug.WriteLine("Couldn't find the custom connection called '" + value +"'");
										   }
										   else
										   {
											   try
											   {												   
												   Assembly ass = Assembly.LoadFrom(consum.LibPath);
												   
												   Directory.SetCurrentDirectory(Path.GetDirectoryName(Application.ExecutablePath));
												   object[] ins = new object[]{this};
												   //handle = ass.CreateInstance(consum.ReflectionName,true,BindingFlags.Public | BindingFlags.Instance | BindingFlags.CreateInstance , null, new object[]{this}, null, new object[]{}) as ObjectHandle;
												   mPainter = ass.CreateInstance(consum.ReflectionName,true,BindingFlags.Public | BindingFlags.Instance | BindingFlags.CreateInstance , null, new object[]{this}, null, new object[]{}) as ConnectionPainter;
												   if(mPainter==null)
												   {
													   mPainter = new DefaultPainter(this);
													   Debug.WriteLine("Couldn't instantiate the painter called '" + value + "'");
												   }
													//Debug.WriteLine(o.ToString());
												   //handle = Activator.CreateInstance(consum.LibPath,consum.ReflectionName,ins);
												   //mPainter = handle.Unwrap() as ConnectionPainter;
											   }
											   catch(Exception exc)
											   {
													Debug.WriteLine(exc.Message);
											   }
											   //shape.Site = this;											   
											//mTracker = new ConnectionTracker(insertionPoints,true);

										   }
										   
										   
										   break;
								   }

								   Invalidate();
							   }
		}
		/// <summary>
		/// Gets or sets the rest length of the connection (used by the layour algorithms)
		/// </summary>
		public int RestLength
		{
			get{return mRestLength;}
			[Browsable(false)] set{mRestLength = value;}
		}
		/// <summary>
		/// Returns the length of the connection
		/// </summary>
		public double length
		{
			get
			{
				PointF s = From.BelongsTo.ConnectionPoint(From); //start point
				PointF e = (To != null) ? To.BelongsTo.ConnectionPoint(To) : mToPoint; //endpoint
				return Math.Sqrt((s.X-e.X)*(s.X-e.X) + (s.Y-e.Y)*(s.Y-e.Y));
			}
		}
		/// <summary>
		/// Gets the rectangle corresponding mTo or embedding the connection
		/// </summary>
		public SizeF ConnectionSize
		{

			get
			{
				SizeF ret=new SizeF(0,0);
				try
				{
					PointF s = From.BelongsTo.ConnectionPoint(From); //start point
					PointF e = (To != null) ? To.BelongsTo.ConnectionPoint(To) : mToPoint; //endpoint
					RectangleF sr=new RectangleF(s,new SizeF(0,0));
					RectangleF er=new RectangleF(e,new SizeF(0,0));
					RectangleF rec= RectangleF.Union(sr,er);
					ret= rec.Size;
				}
				catch (Exception)
				{					
				}
				return ret;
			}
		}
			
		
		/// <summary>
		/// Gets or sets the line style
		/// </summary>
		public DashStyle LineStyle
		{
			get{return mLineStyle;}
			[Browsable(false)] set
							   {
								   mLineStyle = value;
								   this.pen = new Pen(new SolidBrush(mLineColor),lineWidth);
								   this.pen.DashStyle = mLineStyle;		
								   this.Invalidate();
							   }
		}
		/// <summary>
		/// Gets or sets the line end
		/// </summary>
		public ConnectionEnds LineEnd
		{
			get{return mLineEnd;}
			set{mLineEnd=value;}
		}
		/// <summary>
		/// Gets or sets the temporary To point when drawing and connecting mTo a To connector.
		/// Holds normally the mouse coordinate.
		/// </summary>
		public PointF ToPoint
		{
			get{return mToPoint;}
			set{mToPoint =value;}
		}

		/// <summary>
		/// Gets or sets the line color
		/// </summary>
		public Color LineColor
		{
			get{return mLineColor;}
			set
			{
				mLineColor = value; 
				this.pen = new Pen(new SolidBrush(mLineColor),this.lineWidth);
				this.Invalidate();
			
			}
		}

		/// <summary>
		/// Gets or sets the line weight
		/// </summary>
		public ConnectionWeight LineWeight
		{
			get{return mLineWeight;}
			set
			{
				mLineWeight = value;				
				switch (mLineWeight)
				{
					case ConnectionWeight.Thin:
						lineWidth= 1F;break;
					case ConnectionWeight.Medium:
						lineWidth= 2F;break;
					case ConnectionWeight.Fat:
						lineWidth= 3F;break;
				}
				this.pen = new Pen(new SolidBrush(mLineColor),lineWidth);
				this.Invalidate();
			
			}
		}

		/// <summary>
		/// Gets or sets where the connection originates
		/// </summary>
		public Connector From
		{
			get{return mFrom;}
			set{mFrom = value;}
		}
		/// <summary>
		/// Gets or sets where the connection ends
		/// </summary>
		public Connector To
		{
			get{return mTo;}
			set
			{
				mTo = value;
				
			}
		}
			
		#endregion
			
		#region Constructor
		/// <summary>
		/// Default constructor, assigns null connectors and so a null connection
		/// </summary>
		public Connection()
		{
			InitConnection();
		}
		public Connection(IGraphSite site): base(site)
		{
			InitConnection();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Common constructors initialization
		/// </summary>
		public void InitConnection()
		{
			//From = null;
			//To = null;
			this.mShowLabel = false;
			this.pen = this.blackpen;
			mPainter  = new DefaultPainter(this);
			mTracker = new ConnectionTracker(insertionPoints,true);
			insertionPoints = new ArrayList();
		}

		/// <summary>
		/// Called by deserialization
		/// </summary>
		protected internal override void InitEntity()
		{
			base.InitEntity ();
			InitConnection();

		}

		/// <summary>
		/// Returns wether or not the given rectangle is contained in the object
		/// </summary>
		/// <param name="r"></param>
		/// <returns></returns>
		public override Boolean Hit(RectangleF r)
		{
			if ((From == null) || (To == null)) return false;

			PointF p1 = From.AdjacentPoint;
			PointF p2 = To.AdjacentPoint; 
			RectangleF r1=RectangleF.Empty, r2=RectangleF.Empty, r3=RectangleF.Empty;
	

		
			if ((r.Width == 0) && (r.Height == 0))
			{
				PointF p = r.Location;

				if(mPainter.Selected)
				{
					return mTracker.Hit(p)!=Point.Empty || mPainter.Hit(p);
				}
				return mPainter.Hit(p);	
			}

			return (r.Contains(p1) && r.Contains(p2));
		}

		/// <summary>
		/// Returns the points of the connection
		/// </summary>
		/// <returns>An array of PointF structs</returns>
		public PointF[] GetConnectionPoints()
		{
			if(From==null) return null;
			PointF[] points = new PointF[4+insertionPoints.Count];
			
			PointF s = From.Location;
			PointF e = (To != null) ? To.Location : mToPoint;
	
			points[0] = s; //the From connector

			//if there an adjacent point, use it
			if(mFrom.ConnectorLocation == ConnectorLocations.Unknown)
				points[1] = s;
			else
				points[1] = mFrom.AdjacentPoint;

			//loop over the intermediate points
			for(int k=0; k<insertionPoints.Count; k++)
			{
				points[2+k] =(PointF) 	insertionPoints[k];				
			}

			//use the adjacent point of the To connector
			if(mTo==null || mTo.ConnectorLocation == ConnectorLocations.Unknown)
				points[4+insertionPoints.Count-2] = e;
			else
				points[4+insertionPoints.Count-2] = mTo.AdjacentPoint;
			//the To connector
			points[4+insertionPoints.Count-1] = e;

			return points;
		}
		/// <summary>
		/// The painting of the connection
		/// </summary>
		/// <param name="g">The graphics object</param>
		protected internal override void Paint(Graphics g)
		{
			if (!Hover) //the mouse is NOT over this connection
			{
				PaintPolyline(g);
				if(this.mShowLabel) PaintLabel(g);

			}
			else
			{//the mouse is over this connection			
				PaintPolyline(g);
					
					
			}
		}

		/// <summary>
		/// Paints the label
		/// </summary>
		/// <param name="g"></param>
		protected void PaintLabel(Graphics g)
		{
			try
			{	
				RectangleF r = RectangleF.Union(this.mFrom.ConnectionGrip(),this.mTo.ConnectionGrip());
				Size s = g.MeasureString(this.mText, mFont).ToSize();
				RectangleF a = RectangleF.Empty;
				switch(this.mLinePath)
				{
					case "Default":
						a = new RectangleF(r.X+r.Width/2 , r.Y + r.Height/2 + 6, s.Width, s.Height + 1);
						break;
					case "Rectangular":
					switch(this.mFrom.ConnectorLocation)
					{
						case ConnectorLocations.South: case ConnectorLocations.North:
							a=new RectangleF(mFrom.BelongsTo.ConnectionPoint(mFrom).X+5,mTo.BelongsTo.ConnectionPoint(mTo).Y+5,s.Width,s.Height+1);
							break;
						case ConnectorLocations.East: case ConnectorLocations.West:
							a=new RectangleF(mTo.BelongsTo.ConnectionPoint(mTo).X+5,mFrom.BelongsTo.ConnectionPoint(mFrom).Y+5,s.Width,s.Height+1);
							break;

					}
						break;

				}
				RectangleF b = a;
				a.Inflate(+3, +2);      
				g.FillRectangle(new SolidBrush(Color.FromArgb(255, 255, 231)), a);
				g.DrawRectangle(new Pen(Color.Black, 1), Rectangle.Round(a));
				g.DrawString(this.mText, mFont, new SolidBrush(Color.Black), b.Location);
			}
			catch(Exception)
			{				
			}
		}
		/// <summary>
		/// Paints the tracker object
		/// </summary>
		/// <param name="g"></param>
		public void PaintTrack(Graphics g)
		{
			//pen = blackpen;
			//				p.DashStyle = DashStyle.Dash;
			PaintPolyline(g);
		}
		/// <summary>
		/// Draws a line between the To and From connectors
		/// </summary>
		/// <param name="g">The graphics</param>
		/// <param name="p">The pen</param>
		public void PaintPolyline(Graphics g)
		{
			
			
			//style
			if (IsSelected)
				this.pen.DashStyle = DashStyle.Dash;
			else
			{
				if(mLineStyle != DashStyle.Custom)
					this.pen.DashStyle=mLineStyle;
				else
					this.pen.DashStyle=DashStyle.Solid;
			}
			if (From == null) return;
			PointF s = From.BelongsTo.ConnectionPoint(From);
			PointF e = (To != null) ? To.BelongsTo.ConnectionPoint(To) : mToPoint;
			
			//this is for the Omni connector, a central connector
			double len=Math.Sqrt((e.X-s.X)*(e.X-s.X)+(e.Y-s.Y)*(e.Y-s.Y));						
			if(len>0 && To!=null && this.To.ConnectorLocation==ConnectorLocations.Omni)
			{					
				e.X-=Convert.ToSingle((e.X-s.X)*10/len);
				e.Y-=Convert.ToSingle((e.Y-s.Y)*10/len);
			}
			if(len>0 && From!=null && this.From.ConnectorLocation==ConnectorLocations.Omni)
			{					
				s.X-=Convert.ToSingle((e.X-s.X)*10/len);
				s.Y-=Convert.ToSingle((e.Y-s.Y)*10/len);
			}
			//now, for the normal connectors
			if ((s.X != e.X) || (s.Y != e.Y)) 
			{

				mPainter.Hover=Hover;
				switch(this.mLinePath)
				{
					case "Default":
					{					
						mPainter.Points = this.GetConnectionPoints(); //update points to reflect the user actions/motions
						break;
					}
					case "Bezier":
					{					

						if(mFrom.ConnectorLocation == ConnectorLocations.Unknown)
							(mPainter as BezierPainter).Handles[0].ChangeLocation(s);
						else
							(mPainter as BezierPainter).Handles[0].ChangeLocation(mFrom.AdjacentPoint);

						if(mTo==null || mTo.ConnectorLocation == ConnectorLocations.Unknown)
							(mPainter as BezierPainter).Handles[(mPainter as BezierPainter).Handles.Count-1].ChangeLocation(e);
						else
							(mPainter as BezierPainter).Handles[(mPainter as BezierPainter).Handles.Count-1].ChangeLocation(mTo.AdjacentPoint);
							
						break;
					}
					case "Rectangular":
					{
						mPainter.Points = GetConnectionPoints(); //update points to reflect the user actions/motions						
						break;
					}
					default:
						mPainter.Points = GetConnectionPoints();				
						break;
				}

				mPainter.Paint(g);
				if(mHover || mIsSelected) this.mTracker.Paint(g);
			}
			

			PointF left = PointF.Empty, right = PointF.Empty;

			#region the end arrow
			if (LineEnd==ConnectionEnds.BothFilledArrow  || 
				LineEnd==ConnectionEnds.BothOpenArrow  || 
				LineEnd==ConnectionEnds.RightFilledArrow  || 
				LineEnd==ConnectionEnds.RightOpenArrow)
			{	
	
                /* Menim jenom west locations, ostatni ve Ferdovi stejne nemaji smysl
                 */
				switch(this.mTo.ConnectorLocation)
				{
					case ConnectorLocations.North:
						left= new PointF(e.X+4,e.Y-7);
						right = new PointF(e.X-4, e.Y-7); break;
					case ConnectorLocations.South:
						left= new PointF(e.X-4,e.Y+7);
						right = new PointF(e.X+4, e.Y+7); break;
					case ConnectorLocations.West:
						left= new PointF(e.X-12,e.Y-4);
						right = new PointF(e.X-12, e.Y+4); break;
					case ConnectorLocations.East:
						left= new PointF(e.X+7,e.Y+4);
						right = new PointF(e.X+7, e.Y-4);
                        break;
					case ConnectorLocations.Unknown:
						return;
				}
                //2. parametr (hrot sipky)
                //3. parametr (left - horni cast sipky)
                //4. parametr (right - dolni cast sipky)

                if (LineEnd == ConnectionEnds.RightFilledArrow || LineEnd == ConnectionEnds.BothFilledArrow)
                    //PaintArrow(g, e, left, right, true);
                    PaintArrow(g, new PointF(e.X - 5, e.Y), left, right, true);
                else
                    PaintArrow(g, e, left, right, false);

				//the omni arrow is a bit more difficult
				if(this.mTo.ConnectorLocation==ConnectorLocations.Omni)
					if(LineEnd==ConnectionEnds.RightFilledArrow || LineEnd==ConnectionEnds.BothFilledArrow)
						PaintArrow(g,s,e,this.mLineColor,true,false);
					else
						PaintArrow(g,s,e,this.mLineColor,false,false);
					
			}
			#endregion

			#region the start or From arrow
			if (LineEnd==ConnectionEnds.BothFilledArrow  ||
				LineEnd==ConnectionEnds.BothOpenArrow ||
				LineEnd==ConnectionEnds.LeftFilledArrow  ||
				LineEnd==ConnectionEnds.LeftOpenArrow)
			{

				switch(this.mFrom.ConnectorLocation)
				{
					case ConnectorLocations.North:
						left= new PointF(s.X+4,s.Y-7);
						right = new PointF(s.X-4, s.Y-7); break;
					case ConnectorLocations.South:
						left= new PointF(s.X-4,s.Y+7);
						right = new PointF(s.X+4, s.Y+7); break;
					case ConnectorLocations.West:
						left= new PointF(s.X-7,s.Y-4);
						right = new PointF(s.X-7, s.Y+4); break;
					case ConnectorLocations.East:
						left= new PointF(s.X+7,s.Y+4);
						right = new PointF(s.X+7, s.Y-4); break;
					case ConnectorLocations.Unknown:
						return;

				}

				if(LineEnd==ConnectionEnds.LeftFilledArrow || LineEnd==ConnectionEnds.BothFilledArrow)
					PaintArrow(g,s,left,right,true);
				else
					PaintArrow(g,s,left,right,false);
				//the omni arrow is a bit more difficult
				if(this.mTo.ConnectorLocation==ConnectorLocations.Omni)
					if(LineEnd==ConnectionEnds.LeftFilledArrow || LineEnd==ConnectionEnds.BothFilledArrow)
						PaintArrow(g,e,s,this.mLineColor,true,false);
					else
						PaintArrow(g,e,s,this.mLineColor,false,false);
			}
			#endregion
		}

		/// <summary>
		/// Paints an arrow
		/// </summary>
		/// <param name="g"></param>
		/// <param name="tip"></param>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <param name="filled"></param>
		internal protected void PaintArrow(Graphics g, PointF tip, PointF left,PointF right,bool filled)
		{
			try
			{
				SolidBrush brush=new SolidBrush(mLineColor);

				PointF[] points={left, tip, right};
				if (filled)
					g.FillPolygon(brush,points);
				else
				{
					Pen p=new Pen(brush);
					switch(mLineWeight)
					{
						case ConnectionWeight.Thin:
							p.Width=1F;break;
						case ConnectionWeight.Medium:
							p.Width=1.5F;break;
						case ConnectionWeight.Fat:
							p.Width=2F;break;
					}
					g.DrawLines(p,points);
				}
			}
			catch(Exception)
			{				
			}
				
		}
		/// <summary>
		/// Paints an arrow
		/// </summary>
		/// <param name="g"></param>
		/// <param name="tip"></param>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <param name="filled"></param>
		public  void PaintArrow(Graphics g, PointF startPoint, PointF endPoint,Color mLineColor, bool filled, bool showLabel)
		{
			try
			{
				//g.DrawLine(new Pen(mLineColor,1F),startPoint,endPoint);

				SolidBrush brush=new SolidBrush(mLineColor);
				double angle = Math.Atan2(endPoint.Y - startPoint.Y,endPoint.X-startPoint.X);
				double length = Math.Sqrt((endPoint.X - startPoint.X)*(endPoint.X - startPoint.X)+(endPoint.Y - startPoint.Y)*(endPoint.Y - startPoint.Y))-10;
				double delta = Math.Atan2(7,length);
				PointF left = new PointF(Convert.ToSingle(startPoint.X + length*Math.Cos(angle-delta)),Convert.ToSingle(startPoint.Y+length*Math.Sin(angle-delta)));
				PointF right = new PointF(Convert.ToSingle(startPoint.X+length*Math.Cos(angle+delta)),Convert.ToSingle(startPoint.Y+length*Math.Sin(angle+delta)));

				PointF[] points={left, endPoint, right};
				if (filled)
					g.FillPolygon(brush,points);
				else
				{
					Pen p=new Pen(brush,1F);
					g.DrawLines(p,points);
				}
				if(this.showEndsLabel)
				{
					g.DrawString("(" + endPoint.X + "," + endPoint.Y +")",new Font("Arial",10),brush,new PointF(endPoint.X-20,endPoint.Y-20));
				}
			}
			catch(Exception )
			{
				//Trace.WriteLine(exc.Message);
			}
				
		}
			

			

		/// <summary>
		/// Overrides the invalidate (refresh)
		/// </summary>
		/// <remarks>
		/// Note the smart way to invalidate the region arround the start and end points
		/// by inflating the endpoints and then to make a union rectangle
		/// </remarks>
		public override void Invalidate()
		{
			if (From != null)
			{
				PointF s = From.BelongsTo.ConnectionPoint(From); //start point
				PointF e = (To != null) ? To.BelongsTo.ConnectionPoint(To) : mToPoint; //endpoint
				RectangleF sr = new RectangleF(s.X, s.Y, 0, 0);
				RectangleF er = new RectangleF(e.X, e.Y, 0, 0);
				sr.Inflate(+4, +4);
				er.Inflate(+4, +4);

				RectangleF sum = RectangleF.Union(sr, er);
				foreach( PointF p in insertionPoints )
				{
					RectangleF pr = new RectangleF(p.X, p.Y, 0, 0);

					// Point size is determined by tracker object.
					//pr.Inflate(ShapeTracker.HandleWidth/2+1, ShapeTracker.HandleHeight/2+1);
					sum = RectangleF.Union(pr, sum);
				}
				
				// Invalidate the bounding rectangle
				if ((From.BelongsTo != null) && (From.BelongsTo.Site != null))
				{
					Rectangle r = Rectangle.Round(sum);

					r.Offset( From.BelongsTo.Site.AutoScrollPosition.X, 
						From.BelongsTo.Site.AutoScrollPosition.Y );

					r = From.BelongsTo.Site.ZoomRectangle(r);
					r.Inflate(2,2);

					From.BelongsTo.Site.InvalidateRectangle(r);
				}

				From.Invalidate();
				if (To != null) To.Invalidate();								
			}
				
		}

		/// <summary>
		/// Adds a connection on the basis of two connectors
		/// </summary>
		/// <param name="f">The start connector</param>
		/// <param name="t">The end connector</param>
		public void Insert(Connector f, Connector t)
		{
			From = f;
			To = t;
			From.Connections.Add(this);
			From.Invalidate();
			To.Connections.Add(this);
			To.Invalidate();
			this.mSite.Abstract.mConnections.Add(this);
			Invalidate();
		}
		/// <summary>
		/// Deletes the current connection and removes it also mFrom the list in the
		/// From and/or To connectors.
		/// </summary>
		internal protected override void Delete()
		{
			Invalidate();
			this.mSite.Abstract.mConnections.Remove(this);
			if ((From != null) && (From.Connections.Contains(this))) From.Connections.Remove(this);
			if ((To != null) && (To.Connections.Contains(this))) To.Connections.Remove(this);
			From = null;
			To = null;
		}
		/// <summary>
		/// For the given point p it gets the appropriate cursor.
		/// </summary>
		/// <param name="p"> The floating-point point</param>
		/// <returns>A cursor.</returns>
		public override Cursor GetCursor(PointF p)
		{
			if (Control.ModifierKeys == Keys.Shift) return MouseCursors.Add; //why would one do this?
			return MouseCursors.Select;
		}
		#endregion
			
		#region Automata related
		/// <summary>
		/// Overridable Transmit method that hands over values mFrom the Sender mTo the Receiver.
		/// </summary>
		public virtual void Transmit()
		{
			if ((From == null) || (To == null)) return;
   	
			foreach (object o in To.Sends)
				From.Receives.Add(o);
			foreach (object o in From.Sends)
				To.Receives.Add(o);
		}

		#endregion
		
		#region PropertyGrid related
		/// <summary>
		/// Adds property grid accessible properties mTo the connection
		/// </summary>
		public override void AddProperties()
		{
			base.AddProperties ();
			bag.Properties.Add(new PropertySpec("LineColor",typeof(Color),"Appearance","Gets or sets the backcolor of the label."));
			bag.Properties.Add(new PropertySpec("ShowLabel",typeof(bool),"Appearance","Gets or sets whether the label will be shown."));
			bag.Properties.Add(new PropertySpec("LineWeight",typeof(ConnectionWeight),"Appearance","Gets or sets the line weight."));
			bag.Properties.Add(new PropertySpec("LineStyle",typeof(DashStyle),"Appearance","Gets or sets the line style."));
			bag.Properties.Add(new PropertySpec("LineEnd",typeof(ConnectionEnds),"Appearance","Gets or sets the line end type."));
			//bag.Properties.Add(new PropertySpec("LinePath",typeof(string),"Appearance","Gets or sets the line shape.","Default",typeof(ConnectionStyleEditor),typeof(TypeConverter)));
			PropertySpec spec = new PropertySpec("LinePath",typeof(string),"Appearance","Gets or sets the line shape.","Default",typeof(ConnectionStyleEditor),typeof(TypeConverter));
			ArrayList list = new ArrayList();
			for(int k=0; k<this.Site.Libraries.Count;k++)
			{
				for(int m=0; m<this.Site.Libraries[k].ConnectionSummaries.Count; m++)
					list.Add(Site.Libraries[k].ConnectionSummaries[m].ConnectionName);
			}
		
			spec.Attributes = new Attribute[]{ new ConnectionStyleAttribute(list)};
			bag.Properties.Add(spec);
		}
		/// <summary>
		/// Gets the value of the requested property
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void GetPropertyBagValue(object sender, PropertySpecEventArgs e)
		{
			base.GetPropertyBagValue (sender, e);
			switch(e.Property.Name)
			{
				case "Text": e.Value=this.mText;break;
				case "LineColor": e.Value=this.mLineColor; break;
				case "ShowLabel": e.Value=this.mShowLabel; break;
				case "LineWeight": e.Value = this.mLineWeight; break;
				case "LineStyle": e.Value = this.mLineStyle; break;
				case "LineEnd": e.Value = this.mLineEnd; break;
				case "LinePath": e.Value = this.mLinePath; break;
				//case "test": e.Value =test; break;
			}
		}

	
		/// <summary>
		/// Sets the value of the given property
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void SetPropertyBagValue(object sender, PropertySpecEventArgs e)
		{
			base.SetPropertyBagValue (sender, e);
			switch(e.Property.Name)
			{				
				case "LineColor": this.LineColor=(Color) e.Value; break;
				case "ShowLabel": this.mShowLabel=(bool) e.Value; break;
				case "LineWeight": this.LineWeight = (ConnectionWeight) e.Value; break;
				case "LineStyle": this.LineStyle = (DashStyle) e.Value; break;
				case "LineEnd": this.LineEnd = (ConnectionEnds) e.Value; break;
				case "LinePath": this.LinePath = (string) e.Value; break;
				//case "test": test = (string) e.Value; break;
			}
		}

		
		
		
		#endregion
	}
	
}

