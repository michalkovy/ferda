using System;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace Netron.GraphLib
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable] public abstract class ConnectionPainter 
	{
		#region Fields
		protected Connection mConnection = null;
		[NonSerialized] protected Pen pen;
		protected bool mSelected = false;
		protected bool mHover = false;
		protected PointF[] mPoints;
		#endregion

		#region Constructor

		/// <summary>
		/// Creates a connection painter based on the given connection
		/// </summary>
		/// <param name="connection"></param>
		public ConnectionPainter(Connection connection)
		{
			mConnection = connection;
			pen = connection.pen;
			mPoints = connection.GetConnectionPoints();
		}
		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the points upon which the painting of this connection painter is based
		/// </summary>
		public virtual PointF[] Points
		{
			get{return mPoints;}
			set{mPoints = value;}
		}
		/// <summary>
		/// Gets or sets whether the mouse is hovering over this object
		/// </summary>
		internal virtual bool Hover
		{
			get{return mHover;}
			set
			{
				mHover = value;
				if(value) pen.Width=2F; else pen.Width=1F;				
			}
		}

		/// <summary>
		/// Gets or sets whether the connection is selected
		/// </summary>
		public virtual bool Selected
		{
			get{return mSelected;}
			set
			{
				mSelected = value;
				pen = mConnection.pen;
				//showManips = value;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Handles the addition of a new (intermediate) connection point
		/// </summary>
		/// <param name="p"></param>
		internal virtual void AddConnectionPoint(PointF p){}
		/// <summary>
		/// Handles the removal of an (intermediate) connection point
		/// </summary>
		/// <param name="p"></param>
		internal virtual void RemoveConnectionPoint(PointF p){}

		/// <summary>
		/// Paints the connection on the canvas
		/// </summary>
		/// <param name="g"></param>
		public virtual void Paint(Graphics g){}

		/// <summary>
		/// Returns true if the given point hit the connection
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public virtual bool Hit(PointF p){return false;}
		#endregion
	}
}
