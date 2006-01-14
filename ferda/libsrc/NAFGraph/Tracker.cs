using System;
using System.Collections;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using Netron.GraphLib.UI;
using Netron.GraphLib.Interfaces;
namespace Netron.GraphLib
{
	/// <summary>
	/// Abstract base class for tracker objects.
	/// </summary>
	[Serializable]
	public abstract class Tracker 
	{
		#region Fields
		/// <summary>
		/// the mSite to which the tracker belongs
		/// </summary>
		[NonSerialized] protected IGraphSite mSite =null;
		/// <summary>
		/// Resize state (Default: true).
		/// </summary>
		protected Boolean    mResizable = true;

		/// <summary>
		/// Tracking state.
		/// </summary>
		protected Boolean    mTrack = false;

		/// <summary>
		/// Current mCurrentHandle.
		/// </summary>
		protected Point      mCurrentHandle = Point.Empty;

		/// <summary>
		/// Current mouse location,
		/// </summary>
		protected PointF     mCurrentPoint = PointF.Empty;

		/// <summary>
		/// Size of handles (Default: 7x7).
		/// </summary>
		protected Size       handleSize = new Size(7,7);

		#endregion

		#region Constructor
		/// <summary>
		/// Constructor taking a resizable argument.
		/// </summary>
		/// <param name="resize">Resizable state</param>
		public Tracker(Boolean resize)
		{
			mResizable = resize;
		}
		/// <summary>
		/// Default constructor.
		/// </summary>
		public Tracker() {}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets resizable mode.
		/// </summary>
		public Boolean Resizable
		{
			get { return mResizable;  }
			set { mResizable = value; }
		}
		public IGraphSite Site
		{
			get{return mSite;}
			set{mSite = value;}
		}

		/// <summary>
		/// Gets the width of a mCurrentHandle
		/// </summary>
		public int HandleWidth
		{
			get { return handleSize.Width; }
		}

		/// <summary>
		/// Gets the height of a mCurrentHandle
		/// </summary>
		public int HandleHeight
		{
			get { return handleSize.Height; }
		}

		/// <summary>
		/// Gets the tracking mode
		/// </summary>
		public Boolean Track
		{
			get { return mTrack;  }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Start tracking
		/// </summary>
		/// <param name="p"></param>
		/// <param name="h"></param>
		public virtual void Start(PointF p, Point h)
		{
			mCurrentHandle = h;
			mCurrentPoint = p;
			mTrack = true;
		}

		/// <summary>
		/// Ends tracking
		/// </summary>
		public virtual void End()
		{
			mTrack = false;
			mCurrentHandle = Point.Empty;
		}

		/// <summary>
		/// Hit check. To be overridden.
		/// </summary>
		/// <param name="p">Current point</param>
		/// <returns>Handle</returns>
		public abstract Point Hit(PointF p);

		/// <summary>
		/// Returns the cursor at a given location.
		/// </summary>
		/// <param name="p">Current location</param>
		/// <returns>Cursor</returns>
		public abstract Cursor Cursor(PointF p);

		/// <summary>
		/// Moves a tracker to the given location.
		/// </summary>
		/// <param name="p">New Location</param>
		public abstract void Move(PointF p,Size maxsize, bool snap, int snapsize);		
		
		/// <summary>
		/// Paints the tracker
		/// </summary>
		/// <param name="g">Graphics context</param>
		public abstract void Paint(Graphics g);

		/// <summary>
		/// Returns the grip at given point.
		/// </summary>
		/// <param name="p">Point</param>
		/// <returns>Rectangle of grip</returns>
		public abstract RectangleF Grip(Point p);
		#endregion
	}
}
