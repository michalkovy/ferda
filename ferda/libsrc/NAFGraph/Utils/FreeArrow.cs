using System;
using System.Drawing;
namespace Netron.GraphLib.Utils
{
	/// <summary>
	/// 
	/// </summary>
	public class FreeArrow
	{

		#region Fields
		protected PointF mStartPoint = new PointF(0,0);
		protected PointF mEndPoint;
		protected Color mArrowColor = Color.Red;
		protected bool mFilled = true;
		protected bool mShowLabel = false;
		protected string mText = string.Empty;
		protected string mName = string.Empty;

		#endregion

		#region Properties
		
		public PointF StartPoint
		{
			get{return mStartPoint;}
			set{mStartPoint = value;}
		}

		public PointF EndPoint
		{
			get{return mEndPoint;}
			set{mEndPoint = value;}
		}
		public Color ArrowColor
		{
			get{return mArrowColor;}
			set{mArrowColor = value;}
		}
		public bool Filled
		{
			get{return mFilled;}
			set{mFilled = value;}
		}

		public bool ShowLabel
		{
			get{return mShowLabel;}
			set{mShowLabel = value;}
		}

		public string Text
		{
			get{return mText;}
			set{mText = value;}
		}

		public string Name
		{
			get{return mName;}
			set{mName = value;}
		}

		#endregion


		public FreeArrow()
		{
			
		}

		public FreeArrow(	PointF startPoint, PointF endPoint, Color color,  bool filled, bool showLabel, string text)
		{
			this.mStartPoint = startPoint;
			this.mEndPoint = endPoint;
			this.mArrowColor = color;
			this.mText = text;
			this.mShowLabel = showLabel;
			this.mFilled = filled;

		}
		public FreeArrow(	PointF startPoint, PointF endPoint)
		{
			this.mStartPoint = startPoint;
			this.mEndPoint = endPoint;						
		}
		public FreeArrow(	PointF startPoint, PointF endPoint, string text)
		{
			this.mStartPoint = startPoint;
			this.mEndPoint = endPoint;						
			this.mText = text;
			this.mShowLabel = true;
		}

		public  void PaintArrow(Graphics g)
		{
			try
			{
				g.DrawLine(new Pen(mArrowColor,1F),mStartPoint,mEndPoint);

				SolidBrush brush=new SolidBrush(mArrowColor);
				double angle = Math.Atan2(mEndPoint.Y - mStartPoint.Y,mEndPoint.X-mStartPoint.X);
				double length = Math.Sqrt((mEndPoint.X - mStartPoint.X)*(mEndPoint.X - mStartPoint.X)+(mEndPoint.Y - mStartPoint.Y)*(mEndPoint.Y - mStartPoint.Y))-10;
				double delta = Math.Atan2(7,length);
				PointF left = new PointF(Convert.ToSingle(mStartPoint.X + length*Math.Cos(angle-delta)),Convert.ToSingle(mStartPoint.Y+length*Math.Sin(angle-delta)));
				PointF right = new PointF(Convert.ToSingle(mStartPoint.X+length*Math.Cos(angle+delta)),Convert.ToSingle(mStartPoint.Y+length*Math.Sin(angle+delta)));

				PointF[] points={left, mEndPoint, right};
				if (mFilled)
					g.FillPolygon(brush,points);
				else
				{
					Pen p=new Pen(brush,1F);
					g.DrawLines(p,points);
				}
				if(mShowLabel)
				{
					if(mText==string.Empty)
						g.DrawString("(" + mEndPoint.X + "," + mEndPoint.Y +")",new Font("Verdana",10),brush,new PointF(mEndPoint.X-20,mEndPoint.Y-20));
					else
						g.DrawString(mText,new Font("Verdana",10),brush,new PointF(mEndPoint.X-20,mEndPoint.Y-20));

				}
			}
			catch(Exception )
			{
				//Trace.WriteLine(exc.Message);
			}
				
		}


	}
}
