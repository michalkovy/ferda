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

namespace Netron.GraphLib
{
	/// <summary>
	/// The abstract contains the abstract structure of the graph
	/// Pretty much just an enumeration of the elements with standard collection methods.
	/// Derived from the Shape class, can draw the whole plex as if it was a single shape
	/// </summary>
	[Serializable] public class GraphAbstract : Shape
	{
		#region Fields	

		/// <summary>
		/// the collection of mShapes
		/// </summary>
		internal protected ShapeCollection mShapes = new ShapeCollection();
		/// <summary>
		/// the collection of mConnections
		/// </summary>
		internal protected ConnectionCollection mConnections = new ConnectionCollection();

		#endregion

		#region Properties
		/// <summary>
		/// Returns the bounding mRectangle
		/// </summary>
		[Browsable(false)]
		public override RectangleF Rectangle
		{
			set 
			{
				RectangleF r = mRectangle;
				Single dX = value.X - r.X;
				Single dY = value.Y - r.Y;
				Single dWidth = value.Width - r.Width;
				Single dHeight = value.Height - r.Height;
				foreach (Shape shape in mShapes)
				{
					shape.X += dX; shape.Y += dY;
					shape.Width += dWidth;
					shape.Height += dHeight;
				}
				mRectangle = value;
			}
			get
			{
				SumRectangles(); 
				return mRectangle;
			}
		}

		/// <summary>
		/// Updates the bounding mRectangle
		/// </summary>
		private void SumRectangles()
		{
			mRectangle = RectangleF.Empty;

			// for each shape in mShapes of the abstract
			foreach (Shape shape in mShapes)
			{
				mRectangle = RectangleF.Union(mRectangle,shape.Rectangle);
				if( shape.ShapeTracker != null )
				{
					RectangleF a = shape.ShapeTracker.Grip(new Point(-1, -1));
					RectangleF b = shape.ShapeTracker.Grip(new Point(+1, +1));
					mRectangle = RectangleF.Union(mRectangle,RectangleF.Union(a, b));
				}
			}
		}
		/// <summary>
		/// Gets the shape collection of the graph
		/// </summary>
		internal protected ShapeCollection Shapes
		{
			get
			{
				return mShapes;
			}
		}	

		internal protected ConnectionCollection Connections
		{
			get{
				return mConnections;
			}
		}

		#endregion

		#region Methods
		/// <summary>
		/// Paint overrides the base method and paints all elements of the array,
		/// i.e. the boxes and connectors. The paint method of the elements is called to draw themselves.
		/// </summary>
		/// <param name="g"> Graphics class</param>
		protected internal override void Paint(Graphics g)
		{	
			if(this.Shapes.Count<1) return;
			// paint the connections
			foreach (Shape o in Shapes)
				foreach (Connector c in o.Connectors)
					foreach (Connection n in c.Connections)
						n.Paint(g);				
			// paint the mShapes
			foreach (Shape o in Shapes)
				o.Paint(g);
			//paint the connector
			foreach (Shape o in Shapes)
				foreach (Connector c in o.Connectors)
					//if ((o.Hover) || (c.Hover))
						c.Paint(g);
			
		}
		/// <summary>
		/// Inserts a new object into the plex. 
		/// </summary>
		/// <param name="so">the object to insert</param>
		/// <remarks>Note that you can add only one shape at a time.
		/// </remarks>
		internal protected void Insert(Shape so)
		{			
			so.Insert(this);
		}

		
		/// <summary>
		/// Deletes an element of the plex, goes via the History class, also deletes the mConnections.
		/// </summary>
		/// <remarks>Note that multiple mShapes can be delete in one go if they have the Selected flag set to true.</remarks>
		internal protected override void Delete()
		{
			foreach (Shape so in mShapes)
			{
				if (so.IsSelected) so.Delete();
      
				foreach (Connector conor in so.Connectors)
					foreach (Connection conn in conor.Connections)
						if ((conn.IsSelected) || (so.IsSelected)) conn.Delete();
			}
		}
		/// <summary>
		/// This method initiates the tramsmission of data over the mConnections. It calls the tramsit method on all sub-level objects.
		/// </summary>
		public override void Transmit()
		{
			//this is the old way
			//foreach (Shape so in Shapes) so.Transmit();

			//this one is much more performant and each connection transmit will occur only once:
			foreach(Connection con in mConnections) con.Transmit();
			//base.Transmit(); //the abstract has no connectors and hence no data transfer on its own
		}
		/// <summary>
		/// Starts to update all the nodes of the plex; can be a calculation on the basis of the sent values or any other.
		/// Usually the process before all the received values are reset.
		/// </summary>
		/// <remarks>
		/// In normal circumstances this method goes hand-in-hand with the transmit method.
		/// Well, maybe some new physics can be invented if you hack here.
		/// </remarks>
		public override void Update()
		{
			foreach (Shape o in Shapes) 
			{
				o.BeforeUpdate();
				o.Update();
				o.AfterUpdate();
			}
			base.Update();
		}



	
		/// <summary>
		/// Updates the bounding mRectangle
		/// </summary>
		private void UpdateRectangle()
		{
			mRectangle = RectangleF.Empty;

			// for each shape in mShapes of the abstract
			foreach (Shape shape in mShapes)
			{
				mRectangle = RectangleF.Union(mRectangle,shape.Rectangle);
				if( shape.ShapeTracker != null )
				{
					RectangleF a = shape.ShapeTracker.Grip(new Point(-1, -1));
					RectangleF b = shape.ShapeTracker.Grip(new Point(+1, +1));
					mRectangle = RectangleF.Union(mRectangle,RectangleF.Union(a, b));
				}
			}
		}

		


	

		#endregion

		
	}
}


