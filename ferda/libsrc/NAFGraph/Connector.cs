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
using Netron.GraphLib.Interfaces;
using Netron.GraphLib.Configuration;

namespace Netron.GraphLib
{
	/// <summary>
	/// A Connector aka 'connection point' is the spot on a shape where a line (connection) will attach itself.
	/// Lits up when cursor is nearby and can contain in/outflow data that can propagate through the connections.
	/// </summary>
	/// <remarks>
	/// Things you can do:
	/// <br>- making the connector blink or flash when hit</br>
	/// <br>- show an extensive information box when hovered</br>
	/// <br>- attach a status message when hovered</br>
	/// <br>- differentiate different connector on their propagation type or their parnet/child relation</br>
	///</remarks>
	
		
	[Serializable] public class Connector : Entity
	{

		#region Fields

		/// <summary>
		/// gives a little displacement between the connection and the connector
		/// </summary>
		protected float connectionShift = 15F;

		/// <summary>
		/// determines the place of the connection shift
		/// </summary>
		protected ConnectorLocations mConnectorLocation = ConnectorLocations.Unknown;

		/// <summary>
		/// the shift point
		/// </summary>
		protected PointF mAdjacentPoint = PointF.Empty;
		/// <summary>
		/// only 1 connection allowed if false
		/// </summary>
		protected Boolean mAllowMultipleConnections;  		
		/// <summary>
		/// object this connector belongs to.
		/// </summary>
		private Shape _ShapeObject; 
		/// <summary>
		/// connections attached to this connector
		/// </summary>
		private ArrayList _Connections; 
		/// <summary>
		/// collection of objects that the connector propagates
		/// </summary>
		private ArrayList _SendList;  
		/// <summary>
		/// collection of values/objects that the connector receives from other connectors
		/// </summary>
		private ArrayList _ReceiveList; 

		#endregion

		#region Properties

		public PointF Location
		{
			get{return _ShapeObject.ConnectionPoint(this);}
		}

		/// <summary>
		/// Gets or sets whether the connector can have multiple connection attached
		/// </summary>	
		public bool AllowMultipleConnections
		{
			get{return mAllowMultipleConnections;}
			[Browsable(false)] set{mAllowMultipleConnections = value;}
		}

		/// <summary>
		/// Gets the connections of a connector
		/// </summary>
		public ArrayList Connections
		{
			get
			{
				return _Connections;
			}
		}

		/// <summary>
		/// The values/objects that the connector propagates
		/// </summary>
		public ArrayList Sends
		{
			get
			{
				return _SendList;
			}
			[Browsable(false)] set
			{
				_SendList= value;
			}
		}

		/// <summary>
		/// The values/objects that the connectors receives from other connectors
		/// </summary>
		public ArrayList Receives
		{
			get
			{
				return _ReceiveList;
			}
			[Browsable(false)]set
			{
				_ReceiveList= value;
			}
		}

		/// <summary>
		/// The get/Set the ShapeObjects this connector is attached to
		/// </summary>
		public Shape BelongsTo
		{
			get
			{
				return _ShapeObject;
			}
			[Browsable(false)] set
			{
				_ShapeObject = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the adjacent point which allows to have a little distance between shapes and connections
		/// </summary>
		public PointF AdjacentPoint
		{
			get
			{
				
				PointF p = _ShapeObject.ConnectionPoint(this);
				switch(this.mConnectorLocation)
				{
					case ConnectorLocations.North: this.mAdjacentPoint= new PointF(p.X,p.Y - connectionShift); break;
					case ConnectorLocations.East: this.mAdjacentPoint= new PointF(p.X+connectionShift,p.Y); break;
					case ConnectorLocations.South: this.mAdjacentPoint= new PointF(p.X,p.Y + connectionShift); break;
					case ConnectorLocations.West: this.mAdjacentPoint= new PointF(p.X-connectionShift,p.Y); break;					
					case ConnectorLocations.Omni: case ConnectorLocations.Unknown: this.mAdjacentPoint= p; break;
				}

				
				return mAdjacentPoint;
			}
		}


		/// <summary>
		/// Gets or sets the location of the connector which will determine where the adjacent point will be
		/// </summary>
		public ConnectorLocations ConnectorLocation
		{
			get{return mConnectorLocation;}
			[Browsable(false)] set{mConnectorLocation = value;}
		}

		#endregion
		
		
		#region Constructor
		/// <summary>
		/// Constructor of the connector clss
		/// </summary>
		/// <param name="o">the underlying shape to which the connector belongs</param>
		/// <param name="d">the name of the connector</param>
		/// <param name="f">wether the connector allows more than one connection</param>
		public Connector(Shape o, string ConnectorName, Boolean MultipleConnections) : base()
		{
			_ShapeObject = o;
			mText = ConnectorName;
			_Connections = new ArrayList();
			_SendList = new ArrayList();
			_ReceiveList = new ArrayList();
			mAllowMultipleConnections = MultipleConnections;				
		}

		#endregion

		#region Methods
		
		/// <summary>
		/// Says wether the given RectangleF is contained inside this connector
		/// </summary>
		/// <param name="r">the RectangleF as a candidate, usually the mouse coordinates converted to a zero sized rectangle.</param>
		/// <returns>True/false</returns>
		public override Boolean Hit(RectangleF r)
		{
			if ((r.Width == 0) && (r.Height == 0))
				return ConnectionGrip().Contains(r.Location);

			return r.Contains(ConnectionGrip());
		}
		/// <summary>
		/// Overrides the Paint of the control and paint a little connection point or a highlighted connecting widget to 
		/// show the user that a connection is possible.
		/// </summary>
		/// <remarks>
		/// The parent's Hover boolean can be used to check if the mouse is currently hovering over this object. This enables a status message or a different shape.
		/// </remarks>
		/// <param name="g">The Graphics or canvas onto which to paint.</param>
		protected internal override void Paint(Graphics g)
		{
			Rectangle r = Rectangle.Round(ConnectionGrip());
				
			//Barva linky, ktera se kresli okolo konektoru
			Color Line = Color.Silver;
			//Barva vnitrni vyplne
			Color Fill = Color.FromArgb(49, 69, 107); // dark blue

			if (Hover)
			{
				//stanovi se linka vyplne
				Line = Color.Red;
						
				//tady se testuje, jestli uz sem dosahl maximalniho poctu pripojeni
				//v pripade, ze budu moct zapojovat pouze jedno, potom to bude uz 
				//cervene a nebude se do toho moct nic zapojovat
				if ((mAllowMultipleConnections) || (_Connections.Count < 1))
					Fill = Color.FromArgb(0, 192, 0); // medium green
				else
					Fill = Color.FromArgb(255, 0, 0); // red
			}

			//Barvime ctverecek, presne tak jak bude vypadat
			g.FillRectangle(new SolidBrush(Fill), r);
			g.DrawRectangle(new Pen(Line, 1), r);


			//Pise ctverecek s kecami okolo
			//da se tam neco napsat s poctem dalsich prvku zapojenych do tohoto konektoru
            //kvuli velikosti krabicky se musi psat na pravou stranu
			if (Hover)
			{
				Font f = new Font("Tahoma", 8.25f);
				//Size s = g.MeasureString(Description + " " + this.Connections.Count , f).ToSize();
				Size s = g.MeasureString(mText, f).ToSize();
				Rectangle a = 
				//new Rectangle(r.X - (s.Width), r.Y, s.Width, s.Height + 1);
                new Rectangle(r.X + 7, r.Y, s.Width, s.Height + 1);
				Rectangle b = a;
				a.Inflate(+2, +1);
			      
				g.FillRectangle(new SolidBrush(Color.FromArgb(255, 255, 231)), a);
				g.DrawRectangle(new Pen(Color.Black, 1), a);
				//g.DrawString(Description + " " + this.Connections.Count, f, new SolidBrush(Color.Black), b.Location);
				g.DrawString(mText, f, new SolidBrush(Color.Black), b.Location);
			}
		}

		/// <summary>
		/// Necessary implementation of the abstract delete method defined in Entity
		/// </summary>
		protected internal override void Delete()
		{

		}


		/// <summary>
		/// Update/refresh the connector's appearance
		/// </summary>
		public override void Invalidate()
		{
			if (_ShapeObject == null) return;
			IGraphSite c = _ShapeObject.Site;//should return the underlying canvasobject
			if (c == null) return;
			RectangleF r = ConnectionGrip();//get the neighborhood of the connector
			if (Hover) r.Inflate(+100, +100); // make sure a sufficient piece of the neighborhood will be refreshed.
			c.Invalidate(Rectangle.Round(r)); //...and refresh it by calling the control's invalidate method.
		}
		/// <summary>
		/// Returns the cursor for the current connector
		/// </summary>
		/// <param name="p">The cursor location</param>
		/// <returns>A grip cursor, looks like a focus/target</returns>
		public override Cursor GetCursor(PointF p)
		{
				
			return MouseCursors.Grip; 
		}
		/// <summary>
		/// Represents the spot around a connector that lits up and where the connections is attaching itself
		/// The color is determined by various things, can be red, grey or green. See the Hover conditions in the paint handler for this.
		/// </summary>
		/// <returns>A little rectangleF (3x3)</returns>
		public RectangleF ConnectionGrip()
		{
			FerdaConstants constants = new FerdaConstants();

			PointF p = _ShapeObject.ConnectionPoint(this);
			RectangleF r = new RectangleF(p.X, p.Y, 0, 0);

			//tady se das urcit, jak veliky ctverec to bude
			r.Inflate(constants.ConnectorSize/2, constants.ConnectorSize/2);
			return r;
		}
		
		#endregion
	}
}

