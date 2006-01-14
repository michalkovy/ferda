using System;
using System.Drawing;
namespace Netron.GraphLib
{
	/// <summary>
	/// The rectangular connection painter
	/// </summary>
	public class RectangularPainter : ConnectionPainter
	{

		protected PointF s,e;
		

		#region Constructor
		public RectangularPainter(Connection connection) : base(connection){}
		#endregion

		#region Methods

		public override void Paint(System.Drawing.Graphics g)
		{
			PointF[] points = new PointF[5];			
			switch(mConnection.From.ConnectorLocation)
			{
				case ConnectorLocations.North:case ConnectorLocations.South:
					points[0] = mPoints[0];

					if(mConnection.From.ConnectorLocation == ConnectorLocations.Unknown)
						points[1] = mPoints[0];
					else
						points[1] = mPoints[1];
								
					if(mConnection.To==null || mConnection.To.ConnectorLocation == ConnectorLocations.Unknown)
						points[3] = mPoints[mPoints.Length-1];
					else
						points[3] = mPoints[mPoints.Length-2];
								
					points[2] = new PointF(points[1].X,points[3].Y);
					points[4] = mPoints[mPoints.Length-1];
								
					g.DrawLines(pen,points);
					break;
				case ConnectorLocations.East: case ConnectorLocations.West: case ConnectorLocations.Unknown:
					points[0] = mPoints[0];

					if(mConnection.From.ConnectorLocation == ConnectorLocations.Unknown)
						points[1] = mPoints[0];
					else
						points[1] = mPoints[1];

					if(mConnection.To==null || mConnection.To.ConnectorLocation == ConnectorLocations.Unknown)
						points[3] = mPoints[mPoints.Length-1];
					else
						points[3] = mPoints[mPoints.Length-2];
								
					points[2] = new PointF(points[3].X,points[1].Y);
					points[4] = mPoints[mPoints.Length-1];
								
					g.DrawLines(pen,points);
					break;

							
			}
		}

		public override bool Hit(System.Drawing.PointF p)
		{
			
			
			if (mConnection.From == null) return false;
			RectangleF r1=RectangleF.Empty, r2=RectangleF.Empty, r3=RectangleF.Empty;
			PointF[] points;		
			if ((mPoints[0].X != mPoints[mPoints.Length-1].X) || (mPoints[0].Y !=mPoints[mPoints.Length-1].Y)) 
			{
				points = new PointF[5];
				switch(mConnection.From.ConnectorLocation)
				{
					case ConnectorLocations.North:case ConnectorLocations.South:
						points[0] = mPoints[0];

						if(mConnection.From.ConnectorLocation == ConnectorLocations.Unknown)
							points[1] = mPoints[0];
						else
							points[1] = mPoints[1];								
								
								
						if(mConnection.To==null || mConnection.To.ConnectorLocation == ConnectorLocations.Unknown)
							points[3] = mPoints[mPoints.Length-1];
						else
							points[3] = mPoints[mPoints.Length-2];
								
						points[2] = new PointF(points[1].X,points[3].Y);
						points[4] = mPoints[mPoints.Length-1];
						r1 = new RectangleF(points[1].X-2,points[1].Y,10,10);
						r2 = new RectangleF(points[2].X-2,points[2].Y-2,10,10);
						r3 = new RectangleF(points[3].X-2,points[3].Y-2,10,10);
								
						break;
					case ConnectorLocations.East: case ConnectorLocations.West:
						points[0] = mPoints[0];

						if(mConnection.From.ConnectorLocation == ConnectorLocations.Unknown)
							points[1] = mPoints[0];
						else
							points[1] = mPoints[1];
								
								
								
						if(mConnection.To==null || mConnection.To.ConnectorLocation == ConnectorLocations.Unknown)
							points[3] = mPoints[mPoints.Length-1];
						else
							points[3] = mPoints[mPoints.Length-2];
								
						points[2] = new PointF(points[3].X,points[1].Y);
						points[4] = mPoints[mPoints.Length-1];
								
						r1 = new RectangleF(points[1].X,points[1].Y-2,10,10);
						r2 = new RectangleF(points[2].X-2,points[2].Y-2,10,10);
						r3 = new RectangleF(points[3].X-2,points[3].Y-2,10,10);
								
						break;

				}
							

				return RectangleF.Union(r1,r2).Contains(p) || RectangleF.Union(r2,r3).Contains(p);

			}
			return false;
		}


		#endregion
	}
}
