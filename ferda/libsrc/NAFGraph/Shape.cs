using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
//using System.Runtime.Serialization.Formatters.Soap;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Netron.GraphLib.UI;
using Netron.GraphLib.Interfaces;
using Netron.GraphLib.Attributes;
using Netron.GraphLib.Utils;
namespace Netron.GraphLib
{
	/// <summary>
	/// Template class definition to be inherited by all shapes you want to insert and use in your plex
	/// </summary>
	[Serializable]public class Shape : Entity, IAutomataCell, ILayoutNode
	{
		
		#region Fields

#if NAFEnabled
		/// <summary>
		/// Only necessary if you need NAF integration
		/// </summary>
		protected NAF.Core.Plugin.NAFRootMediator root;
#endif


		/// <summary>
		/// whether you can move the shape
		/// </summary>
		protected bool mMobile = true;
		/// <summary>
		/// volatile all-purpose mTag
		/// </summary>
		protected object mTag;
		/// <summary>
		/// the array list of .Net mControls the shape contains
		/// </summary>
		protected NetronGraphControlCollection mControls;
		/// <summary>
		/// the infinitesimal x-shift used by layout
		/// </summary>
		protected double mdx =0 ;
		/// <summary>
		/// the infinitesimal y-shift used by layout
		/// </summary>
		protected double mdy = 0 ;
		/// <summary>
		/// fixed node boolean
		/// </summary>
		protected bool isfixed=false;
		/// <summary>
		/// the default node color
		/// </summary>
		protected Color mNodeColor = Color.LightGray;
		/// <summary>
		/// The Abstarct object to which this shape belongs.
		/// </summary>
		/// <remarks>
		/// A shape cannot exist on its own and will not be drawn outside an abstract structure
		/// </remarks>
		internal   GraphAbstract Parent = null;
		/// <summary>
		/// tells wether or not the user can resize the mRectangle
		/// </summary>
		protected Boolean mResizable = true;
		/// <summary>
		/// The internal collection of connectors attached to this shape object
		/// </summary>
		/// <remarks>
		/// Note that connectors are sub-ordinated to the shapes and thus do not have to be deleted or taken care off independently
		/// </remarks>
		private ConnectorCollection connectors = new ConnectorCollection();
		/// <summary>
		/// This is the floating-point mRectangle associated to the shape
		/// It determines the shape's size or boundaries
		/// </summary>
		protected internal RectangleF mRectangle = new RectangleF(0,0,100,100);
		/// <summary>
		/// The internal tracker object, representing the mRectangle and grips with which one can resize the shape.
		/// </summary>
		private ShapeTracker mShapeTracker = null;
		#endregion

		#region Constructor
		public Shape()
		{
			mControls = new NetronGraphControlCollection();
		}

		public Shape(IGraphSite site) : base(site)
		{
			mControls = new NetronGraphControlCollection();
		}

		#endregion

		#region Properties
#if NAFEnabled
		public Netron.NAF.Core.Plugin.NAFRootMediator Root
		{
			get{return root;}
			set{root = value;}
		}
#endif


		public bool Resizable
		{
			get{return mResizable;}
			set{mResizable = value;}
		}

		public bool Mobile
		{
			get{return mMobile;}
			set{mMobile = value;}
		}
		/// <summary>
		/// Gets or sets a general purpose mTag object
		/// </summary>
		public object Tag
		{
			get{return mTag;}
			set{mTag = value;}
		}
		/// <summary>
		/// This method represents the transmission of data over a connection. Once the data is transmitted to the connectors the senders' value is reset
		/// </summary>
		/// <remarks>This method is not strictly part of the plex structure but belongs to the possible applications.</remarks>
		public virtual void Transmit()
		{				
			foreach (Connector c in Connectors)
			{
				foreach (Connection n in c.Connections)
					n.Transmit();
				c.Sends.Clear();
			}
		}
		public virtual void BeforeUpdate()
		{
		
		}
		/// <summary>
		/// The method allows to update the dynamical state of the plex, to compute something on the basis of the received values and to set the new send values.
		/// 
		/// </summary>
		public virtual void Update()
		{
				
			//here you can calculate the state and set the new send values before resetting the receive.
			//for example:
			//				clear the sends values
			//				foreach (Connector c in Connectors) c.Sends.Clear();
			//				calculate things with the Receives and set internal state of the automata
			//			    set new values of Sends
			//			    finally, clean the receives before the nex transmit
			//				foreach (Connector c in Connectors) c.Receives.Clear();
		}
		public virtual void AfterUpdate()
		{
		
		}
		internal void Zoom(float amount)
		{
			mShapeTracker=null;
			this.IsSelected=false;
			
			mRectangle.Width*=amount;mRectangle.Height*=amount;
			mRectangle.X*=amount;mRectangle.Y*=amount;
				
		}

		/// <summary>
		/// Returns the associated mRectangle for this shape
		/// </summary>
		/// <remarks>
		/// The need for this becomes more clear if start to use non-rectangular shapes.
		/// </remarks>
		public virtual RectangleF Rectangle
		{
			set 
			{
				//Invalidate();
				mRectangle = value;
				if (mShapeTracker != null) mShapeTracker.Rectangle = mRectangle;
				//Invalidate();
			}
			get
			{
				return (mShapeTracker != null) && (mShapeTracker.Track) ? mShapeTracker.Rectangle : mRectangle;
			}
		}
		/// <summary>
		/// Returns the collection of connectors for this shape object
		/// </summary>
		public ConnectorCollection Connectors
		{
			get { return connectors; }
		}
		/// <summary>
		/// Is the shape selected?
		/// </summary>
		protected internal override bool IsSelected
		{
			set
			{
				//the base keeps the value
				base.IsSelected = value;

				if (value)
				{
					mShapeTracker = new ShapeTracker(this.mSite,Rectangle, Resizable);
					Invalidate();
				}
				else
				{
					Invalidate();
					mShapeTracker = null;
				}
			}
			get
			{
				return base.IsSelected;
			}
		}

		/// <summary>
		/// The list of Controls the shape contains
		/// </summary>
		public NetronGraphControlCollection Controls
		{
			get{return mControls;}
			set{mControls = value;}
		}

		/// <summary>
		/// Returns the tracker, which represents the grips and mRectangle with which one can resize the shape.
		/// </summary>
		protected internal ShapeTracker ShapeTracker
		{
			get {return mShapeTracker;}
		}

		/// <summary>
		/// Gets or sets the x-coordinate of the shape
		/// </summary>
		[GraphMLData]public float X
		{
			get
			{
					
				return this.mRectangle.X;
			}
			set
			{
				this.mRectangle.X=value;
			}
		}

		/// <summary>
		/// Gets or sets the y-coordinate
		/// </summary>
		[GraphMLData]public float Y
		{
			get
			{
					
				return this.mRectangle.Y;
			}
			set
			{
				this.mRectangle.Y=value;
			}
		}
		/// <summary>
		/// Increment in x for the graph layout
		/// </summary>
		public double dx
		{
			get
			{
					
				return mdx;
			}
			set
			{
				mdx=value;
			}
		}

		/// <summary>
		/// Increment in y for the graph layout
		/// </summary>
		public double dy
		{
			get
			{
				return mdy;
			}
			set
			{
				mdy=value;
			}
		}

		/// <summary>
		/// Gets or sets whether the shape is fixed/unmovable
		/// </summary>
		[GraphMLData]public bool Fixed
		{
			get
			{
				return isfixed;
			}
			set
			{
				isfixed=value;
			}
		}

		/// <summary>
		/// Gets or sets the node color
		/// </summary>
		[GraphMLData]public virtual Color NodeColor
		{
			get
			{
				// TODO:  Add Shape.NodeColor getter implementation
				return mNodeColor;
			}
			set
			{
				mNodeColor=value;
			}
		}
		/// <summary>
		/// Gets the background brush
		/// </summary>
		[GraphMLData]protected internal virtual Brush BackgroundBrush
		{
			get{ return IsSelected ? new SolidBrush(Color.LightSteelBlue) : new SolidBrush(mNodeColor);}	
		}

		/// <summary>
		/// Gets the text brush
		/// </summary>
		[GraphMLData]protected internal virtual Brush TextBrush
		{
			get{return new SolidBrush(mTextColor);}
		}

			
		/// <summary>
		/// Width of a shape.
		/// </summary>
		[Browsable(false),GraphMLData]
		public virtual float Width
		{
			get { return this.Rectangle.Width; }
			set 
			{ 
				RectangleF r = this.Rectangle;
				this.Rectangle = new RectangleF(r.X,r.Y,value,r.Height);
			}
		}

		/// <summary>
		/// Height of a shape.
		/// </summary>
		[Browsable(false),GraphMLData]
		public virtual float Height
		{
			get { return this.Rectangle.Height; }
			set 
			{ 
				RectangleF r = this.Rectangle;
				this.Rectangle = new RectangleF(r.X,r.Y,r.Width,value);
			}
		}
		
		#endregion
		
		#region Methods

		public bool IsConnectedTo(Shape shape)
		{
			foreach(Connector c in this.connectors)
			{
				foreach(Connection con in c.Connections)
				{
					if(con.To.BelongsTo.Equals(shape) || con.From.BelongsTo.Equals(shape)) return true;
					
				}
			}
			return false;
		}
		/// <summary>
		/// Required interface implementation
		/// </summary>
		public virtual void InitAutomata()
		{
			
		}
		/// <summary>
		/// Adds the shape to an GraphAbstract collection
		/// </summary>
		/// <param name="p"></param>
		internal   void Insert(GraphAbstract p)
		{
			Parent = p;
			Parent.Shapes.Add(this);
			Invalidate();
		}
		/// <summary>
		/// Removes itself from an GraphAbstract. The connectors are deleted as part of this deletion process.
		/// </summary>
		 internal protected override void Delete()
		{
			Invalidate();
    	
			// throw the connections away
			 ArrayList toDelete = new ArrayList();

			 // throw the connections away. 
			 // Action is done in two steps because calling Delete() of a connection manipulates the connector
			 // collections. To Prevent an exception the connections are iterated and moved to a deletion list first.   
			 foreach (Connector c in Connectors)
			 {
				 foreach (Connection cn in c.Connections)
				 {
					 toDelete.Add(cn);
				 }
			 }

			 foreach( Connection cn in toDelete )
			 {
				 cn.Delete();
			 }
			if (Parent.Shapes.Contains(this))
				Parent.Shapes.Remove(this);

			Parent = null;
			
			Invalidate();
		}
		/// <summary>
		/// Returns true if the given mRectangle contains the shape (this)
		/// </summary>
		/// <param name="r">A floating-point mRectangle object</param>
		/// <returns>True if contained</returns>
		public override Boolean Hit(RectangleF r)
		{
			//if mRectangle is point like
			if ((r.Width == 0) && (r.Height == 0))
			{   //hit the mRectangle of the shape
				if (Rectangle.Contains(r.Location)) return true;
				//hit the tracker
				if (mShapeTracker != null)
				{
					Point h = mShapeTracker.Hit(r.Location);
					if ((h.X >= -1) && (h.X <= +1) && (h.Y >= -1) && (h.Y <= +1)) return true;
				}
				//hit a connector
				foreach (Connector c in Connectors)
					if (c.Hit(r)) return true;

				return false;
			}
			//if not point-like then use the normal method
			return r.Contains(Rectangle);
		}
		/// <summary>
		/// Returns the coordinates of a given connector attached to this shape
		/// </summary>
		/// <param name="c">A connector object</param>
		/// <returns>A floating-point point (pointF)</returns>
		public virtual PointF ConnectionPoint(Connector c)
		{
			return PointF.Empty;
		}

		/// <summary>
		/// Returns the thumbnail of the shape (for the shape viewer)
		/// </summary>
		/// <returns></returns>
		public virtual Bitmap GetThumbnail()
		{
			Stream stream=Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.GraphLib.Resources.UnknownShape.gif");
					
			Bitmap bmp= Bitmap.FromStream(stream) as Bitmap;
			stream.Close();
			stream=null;
			return bmp;
				 
		}
		/// <summary>
		/// Overrides the paint method
		/// </summary>
		/// <remarks>
		/// Do not forget to call this via base.Paint to paint the tracker.
		/// </remarks>
		/// <param name="g">The graphics canvas onto which to paint</param>
		protected internal override void Paint(Graphics g)
		{
			if (IsSelected)
				mShapeTracker.Paint(g);
			//this.site.PaintArrow(g,new PointF(0,0),new PointF(this.Rectangle.X,Rectangle.Y),Color.Red,true, true);
		}
		/// <summary>
		/// Recalculates the size of the mRectangle to fit the size of the text
		/// </summary>
		public void FitSize()
		{
			SizeF newSize = this.mSite.Graphics.MeasureString(this.mText,mFont);
			newSize.Width=Math.Max(newSize.Height,newSize.Width) + 4;
			newSize.Height=newSize.Width;
			Rectangle = new RectangleF(new PointF(Rectangle.X,Rectangle.Y), newSize);	
			
			
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="square">if true the shape will be square with the maximum otherwise only the width will be resized to fit the content</param>
		public void FitSize(bool square)
		{
			SizeF newSize = this.mSite.Graphics.MeasureString(this.mText,mFont);
			newSize.Width=Math.Max(newSize.Height,newSize.Width) + 4;
			if(square) newSize.Height=newSize.Width;
			newSize.Height+=7;
			Rectangle = new RectangleF(new PointF(Rectangle.X,Rectangle.Y),newSize);	


		}

		/// <summary>
		/// Refreshes the shape
		/// </summary>
		public override void Invalidate()
		{
			if (Site == null) return;
			if (Connectors == null) return;

			// Invalidate the shape mRectangle, pay attention on scroll position and zoom
			RectangleF r = this.Rectangle;
			r.Inflate(+3, +3); // padding for selection frame.

			System.Drawing.Rectangle r2 = System.Drawing.Rectangle.Round(r);
			r2.Offset( Site.AutoScrollPosition.X, Site.AutoScrollPosition.Y );

			r2 = Site.ZoomRectangle(r2);
			r2.Inflate(2,2);

			Site.InvalidateRectangle(r2);

			// Invalidate each connector
			foreach (Connector c in Connectors)
			{
				c.Invalidate();

				if (ShapeTracker != null)
					foreach (Connection n in c.Connections)
						n.Invalidate();
			}

			// Invalidate tracker
			if (ShapeTracker != null)
			{
				RectangleF a = ShapeTracker.Grip(new Point(-1, -1));
				RectangleF b = ShapeTracker.Grip(new Point(+1, +1));
				r2 = System.Drawing.Rectangle.Round(RectangleF.Union(a, b));
				r2.Offset( Site.AutoScrollPosition.X, Site.AutoScrollPosition.Y );
				r2 = Site.ZoomRectangle(r2);
				r2.Inflate(2,2);

				Site.InvalidateRectangle(r2);
			}
		}
		/// <summary>
		/// Returns the cursor for this shape
		/// </summary>
		/// <param name="p">A floaint-point point</param>
		/// <returns>A cursor object</returns>
		public override Cursor GetCursor(PointF p)
		{
			if (mShapeTracker != null)
			{
				Cursor c = mShapeTracker.Cursor(p);
				if (c != Cursors.No) return c;
			}

			if (Control.ModifierKeys == Keys.Shift)
				return MouseCursors.Add;

			return Cursors.Arrow;//MouseCursors.Select;
		}


		/// <summary>
		/// Moves the shape controls (if any) when the shape has been moved
		/// </summary>
		public virtual void MoveControls(){}

		#region Properties related
		public  override void AddProperties()
		{
			base.AddProperties ();
			//the size of the shape
			bag.Properties.Add(new PropertySpec("Size",typeof(SizeF),"Layout","The size of the shape",SizeF.Empty,typeof(System.Drawing.Design.UITypeEditor),typeof(Netron.GraphLib.SizeFTypeConverter)));
			//the location of the shape
			bag.Properties.Add(new PropertySpec("Location",typeof(PointF),"Layout","The location of the shape",PointF.Empty,typeof(System.Drawing.Design.UITypeEditor),typeof(Netron.GraphLib.PointFTypeConverter)));
			//graph specs		
			//bag.Properties.Add(new PropertySpec("Connectors",typeof(ConnectorCollection),"Graph","The connector collection of the shape",SizeF.Empty));
			//the node's color
			bag.Properties.Add(new PropertySpec("NodeColor",typeof(Color),"Appearance","The background color of the shape",Color.Gray));
		}
		protected override void GetPropertyBagValue(object sender, PropertySpecEventArgs e)
		{
			base.GetPropertyBagValue (sender, e);
			switch(e.Property.Name)
			{
				case "Size": e.Value=this.mRectangle.Size;break;
				case "Location": e.Value=this.mRectangle.Location;break;
				//case "Connectors": e.Value=this.connectors;break;
				case "NodeColor": e.Value=this.mNodeColor;break;
				
			}
		}

		protected override void SetPropertyBagValue(object sender, PropertySpecEventArgs e)
		{
			base.SetPropertyBagValue (sender, e);

			switch(e.Property.Name)
			{
				case "Size": 
					if(this.Resizable) this.mRectangle.Size=(SizeF) e.Value;
					else
						MessageBox.Show("The shape is defined as non-resizable.","Not allowed", MessageBoxButtons.OK,MessageBoxIcon.Information);
					if(this.ShapeTracker!=null) 
					{
						this.ShapeTracker.Rectangle=this.mRectangle;
					}
					recalculateSize = false;					
					this.Invalidate();
					break;
				case "Location": 
					this.mRectangle.Location=(PointF) e.Value;
					if(this.ShapeTracker!=null)
					{
						ShapeTracker.ChangeLocation((PointF) e.Value);						
					}
					recalculateSize = false;					
					this.Invalidate();
					break;
				case "NodeColor":
					this.mNodeColor=(Color) e.Value;					
					recalculateSize = false;					
					this.Invalidate();
					break;
			}
			if(e.Property.Name=="Text" && this.Resizable) recalculateSize = true;

		}

		#endregion

		public virtual MenuItem[] ShapeMenu()
		{
			return null;
		}

		public virtual void OnKeyDown(KeyEventArgs e)
		{return;}
		public virtual void OnKeyPress(KeyPressEventArgs e)
		{return;}

		#endregion		
	}
}
