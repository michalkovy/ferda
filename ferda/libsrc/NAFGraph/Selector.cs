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
//using System.Runtime.Serialization.Formatters.Soap;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Netron.GraphLib.UI;

namespace Netron.GraphLib
{
	/// <summary>
	/// When you drag with the mouse this class produces the dashed line rectangle to visualize the elements you select.
	/// </summary>

	public class Selector
	{
		private PointF Start;
		private PointF Current;
		private GraphControl site;
		/// <summary>
		/// Constructor of the class
		/// </summary>
		/// <param name="p"></param>
		public Selector(PointF p, GraphControl site)
		{
			Start = p;
			Current = p;
			this.site = site;
		}
		/// <summary>
		/// Update the selector to reflect the current position of the mouse
		/// </summary>
		/// <param name="p"></param>
		public void Update(PointF p)
		{
			Current = p;
		}
		/// <summary>
		/// Paint the selector on the canvas
		/// </summary>
		/// <param name="c"></param>
		public void Paint(Control c)
		{
			
			

			Rectangle r = c.RectangleToScreen(System.Drawing.Rectangle.Round(Rectangle));			
			ControlPaint.DrawReversibleFrame(r, Color.SteelBlue, FrameStyle.Dashed);			
		}
		/// <summary>
		/// The rectangle corresponding to the selector marquee
		/// </summary>
		public RectangleF Rectangle
		{
			get
			{
				RectangleF r = new RectangleF();
				r.X = (Start.X <= Current.X) ? Start.X : Current.X;
				r.Y = (Start.Y <= Current.Y) ? Start.Y : Current.Y;
				r.Width = Current.X - Start.X;
				if (r.Width < 0) r.Width *= -1.0f;
				r.Height = Current.Y - Start.Y;
				if (r.Height < 0) r.Height *= -1.0f;
				r = RectangleF.Intersect(r,site.ClientRectangle);
				return r;
			}
		}
	}
}

	
