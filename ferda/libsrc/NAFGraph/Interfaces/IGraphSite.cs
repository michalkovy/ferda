using System;
using System.Drawing;
using System.Collections;
using Netron.GraphLib.Configuration;
namespace Netron.GraphLib.Interfaces
{
	/// <summary>
	/// Interface of a graph site (control) 
	/// </summary>
	public interface IGraphSite
	{
		/// <summary>
		/// 		
		/// </summary>
		/// <param name="r">A System.Drawing.Rectangle object that represents the region to invalidate. </param>
		/// <param name="b">invalidateChildren: true to invalidate the control's child controls; otherwise, false.</param>
		void Invalidate(Rectangle r, bool b);

		void Invalidate(Rectangle r);

		void Invalidate();

		ShapeCollection Nodes {get;}

		ArrayList Edges {get;}

		Size Size {get;}

		float Zoom {get; set;}

		bool RestrictToCanvas {get; set;}

		Point AutoScrollPosition {get; set;}

		bool DoTrack {get; set;}

		/// <summary>
		/// Let the site invalidate the rectangle
		/// </summary>
		/// <param name="rect">invalid rectangle</param>
		void InvalidateRectangle( Rectangle rect );

		/// <summary>
		/// Zooms a point
		/// </summary>
		Point ZoomPoint(Point originalPt);

		/// <summary>
		/// Unzooms a point.
		/// </summary>
		Point UnzoomPoint(Point originalPt);

		/// <summary>
		/// Zooms a rectangle.
		/// </summary>
		Rectangle ZoomRectangle(Rectangle originalRect);

		/// <summary>
		/// Unzooms a rectangle.
		/// </summary>
		Rectangle UnzoomRectangle(Rectangle originalRect);

		/// <summary>
		/// Zooms a point
		/// </summary>
		PointF ZoomPoint(PointF originalPt);

		/// <summary>
		/// Unzooms a point.
		/// </summary>
		PointF UnzoomPoint(PointF originalPt);

		/// <summary>
		/// Zooms a rectangle.
		/// </summary>
		RectangleF ZoomRectangle(RectangleF originalRect);

		/// <summary>
		/// Unzooms a rectangle.
		/// </summary>
		RectangleF UnzoomRectangle(RectangleF originalRect);

		int Width {get;}
		int Height {get;}
		Graphics Graphics {get;}

		void PaintArrow(Graphics g, PointF startPoint, PointF endPoint,Color lineColor, bool filled, bool showLabel);

		Entity GetEntity(string UID);

		GraphAbstract Abstract {get;}

		GraphObjectsLibraryCollection Libraries{get;}

		void RaiseShowProps(PropertyBag props);

		
	}
}
