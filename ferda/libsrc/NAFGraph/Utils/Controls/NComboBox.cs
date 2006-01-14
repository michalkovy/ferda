using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Text;
using Netron.GraphLib.Utils;
namespace Netron.GraphLib
{
	/// <summary>
	/// Summary description for ComboBox.
	/// </summary>
	public class NComboBox : NetronGraphControl
	{
		#region Events
		public delegate void ResizeInfo(SizeF newSize);

		public event ResizeInfo OnResize;

		#endregion		

		#region Fields
		protected int mSelectedIndex = -1;	
		protected string mText = "            ";
		/// <summary>
		/// for the text property
		/// </summary>
		protected bool resize = true;
		protected bool mExpanded = false;
		protected NListItemCollection mListItems;
		protected int expandedWidth;
		protected int expandedHeight;
		protected readonly int maxHeight = 100;
		protected Point hoverPoint;
		protected bool windowed = false;
		protected int startIndex = 0;
		protected int stopIndex;
		/// <summary>
		/// for the expanded combo size
		/// </summary>
		protected bool recalc = true;
		#endregion

		#region Properties
		public bool Expanded
		{
			get{return mExpanded;}
			set{mExpanded = value;}
		}
		public NListItemCollection ListItems
		{
			get{return mListItems;}
			set{mListItems = value;}
		}
	
		public string Text
		{
			get{return mText;}
			set{mText = value;resize = true;}
		}
		public int SelectedIndex
		{
			get{return mSelectedIndex;}
			set{
				if(mListItems.Count>=1 &&value<mListItems.Count && value >=0)
				{
					mText = mListItems[value].Text;
					mSelectedIndex = value;
				}
				
			}
		}
		#endregion

		#region Constructor
		public NComboBox(Shape shape) : base(shape)
		{
			mListItems = new NListItemCollection();
			mListItems.OnItemAdded+=new Netron.GraphLib.Utils.NListItemCollection.NListChange(mListItems_OnItemAdded);
			mListItems.OnItemRemoved+=new Netron.GraphLib.Utils.NListItemCollection.NListChange(mListItems_OnItemRemoved);

		}
		
		public NComboBox(Shape shape, int width, int height):base(shape)
		{			
			this.mWidth = width;
			this.mHeight = height;
			mListItems = new NListItemCollection();
			mListItems.OnItemAdded+=new Netron.GraphLib.Utils.NListItemCollection.NListChange(mListItems_OnItemAdded);
			mListItems.OnItemRemoved+=new Netron.GraphLib.Utils.NListItemCollection.NListChange(mListItems_OnItemRemoved);

		}

		#endregion

		#region Methods

		public override void Paint(Graphics g)
		{
			
			if(resize)
			{
				SizeF s = new SizeF(g.MeasureString(mText,mFont));
				s.Width += mHeight +20;
				s.Height += 2;
				if(OnResize!=null) OnResize(s);
				mWidth =(int) s.Width;
				mHeight =(int) s.Height;
				resize = false;
			}
			ControlPaint.DrawComboButton(g,mLocation.X,mLocation.Y,mHeight,mHeight,ButtonState.Flat);
			if(mExpanded)
			{
				if(recalc) RecalcSize(g);
				
				StringBuilder sb = new StringBuilder();
				
				for(int k = startIndex; k<Math.Min(mListItems.Count,startIndex+5); k++)
				{
					sb.Append(mListItems[k].ToString());
					sb.Append(Environment.NewLine);
				}			
				
				float step = ((float)expandedHeight) / (5F);
				if(((mListItems.Count>5) && (startIndex+5)<mListItems.Count) || startIndex>0)
				{
					g.FillRectangle(Brushes.WhiteSmoke,new Rectangle(mLocation.X+mHeight,mLocation.Y,expandedWidth+10,expandedHeight));
					//this is the scroller	
					g.FillRectangle(Brushes.LightGray,mLocation.X+mHeight+expandedWidth,mLocation.Y,10,expandedHeight);	
					//down scroller
					if((mListItems.Count>5) && (startIndex+5)<mListItems.Count)
						ControlPaint.DrawScrollButton(g,mLocation.X+mHeight+expandedWidth,mLocation.Y+expandedHeight-10,10,10,ScrollButton.Down,ButtonState.Flat);
						
					//ControlPaint.DrawScrollButton(g,mLocation.X+mHeight+expandedSize.Width,mLocation.Y+expandedSize.Height,10,10);						
					if(startIndex>0 )
					{
						//up scroller						
						ControlPaint.DrawScrollButton(g,mLocation.X+mHeight+expandedWidth,mLocation.Y,10,10,ScrollButton.Up,ButtonState.Flat);
					}
				}
				else
					g.FillRectangle(Brushes.WhiteSmoke,new Rectangle(mLocation.X+mHeight,mLocation.Y,expandedWidth+10,expandedHeight+10));
				
				g.DrawString(sb.ToString(),mFont,Brushes.Black,new Point(mLocation.X+mHeight,mLocation.Y));
				ControlPaint.DrawBorder(g,new Rectangle(mLocation.X+mHeight,mLocation.Y,expandedWidth+10,expandedHeight),Color.DarkGray,ButtonBorderStyle.Solid);
			}
			else
			{
				ControlPaint.DrawBorder(g,new Rectangle(mLocation.X+mHeight,mLocation.Y,mWidth-mHeight,mHeight),Color.DarkGray, ButtonBorderStyle.Solid);
				g.DrawString(mText,mFont,Brushes.Black,mLocation.X+mHeight+2,mLocation.Y+1);
			}
		
			
		}

		public override void OnMouseDown(MouseEventArgs e)
		{
			Point p =new Point(e.X,e.Y);
			string s;

			
			
			//is it a selection?

			if(HitComboButton(p))
				mExpanded = !mExpanded;
			else if(mExpanded)
			{
				 if((s= HitListItem(p))!=null) Text = s; 
				//is it scrolling?
				if(((mListItems.Count>5) && (startIndex+5)<mListItems.Count) || startIndex>0)
				{
					if(new Rectangle(mLocation.X+mHeight+expandedWidth,mLocation.Y+expandedHeight-10,10,10).Contains(p))
					{					
						if(startIndex+5<mListItems.Count) startIndex++;	
						//disable tracking otherwise you move the shape when scrolling
						this.parent.Site.DoTrack = false;
						return;
					}
					if(new Rectangle(mLocation.X+mHeight+expandedWidth,mLocation.Y,10,10).Contains(p))
					{
						startIndex--;
						if(startIndex<0) startIndex=0; 
						//disable tracking otherwise you move the shape when scrolling
						this.parent.Site.DoTrack = false;
						return;
					}
				}
				
			}
			
		}

		public override void OnMouseMove(MouseEventArgs e)
		{
			hoverPoint=HoverListItem(new Point(e.X,e.Y));
			
		}

	


		public override bool Hit(Point p)
		{
			if(mExpanded)
			{
				Rectangle r = new Rectangle(mLocation.X+mHeight,mLocation.Y,expandedWidth+10,expandedHeight);
				return r.Contains(p);
			}
			else
				return Rectangle.Contains(p);			
		}

		private bool HitComboButton(Point p)
		{
			return new Rectangle(mLocation, new Size(mHeight, mHeight)).Contains(p); 
		}
		private string HitListItem(Point p)
		{
			float step = ((float)expandedHeight) / Math.Min((float) mListItems.Count,5F);

			for(int k = 0; k<Math.Min(mListItems.Count,startIndex+5); k++)
			{
				if(((mLocation.Y +k*step<p.Y) && (p.Y<mLocation.Y + (k+1)*step)) && ( p.X>mLocation.X+mHeight && p.X<mLocation.X+mHeight+expandedWidth))
				{
					mExpanded = false;
					mSelectedIndex = k + startIndex;
					return mListItems[k + startIndex].ToString();
				}
			}
			return null;
		}

		private Point HoverListItem(Point p)
		{
			float step = ((float)expandedHeight) /  Math.Min((float) mListItems.Count,5F);

			for(int k =0;k<mListItems.Count; k++)
			{
				if((mLocation.Y +k*step<p.Y) && (p.Y<mLocation.Y + (k+1)*step))
				{
					
					return new Point(mLocation.X+mHeight,(int) (mLocation.Y + k*step));					
				}
			}
			return Point.Empty;
		}


		/// <summary>
		/// Recalculates the size of the expanded combo
		/// </summary>
		/// <param name="g"></param>
		private void RecalcSize(Graphics g)
		{
			StringBuilder sb = new StringBuilder();
			for(int k =0; k<mListItems.Count; k++)
			{
				sb.Append(mListItems[k]);
				sb.Append(Environment.NewLine);
			}
			//we take the max width to refrain from resizing the combo on scrolling
			expandedWidth =  (int) g.MeasureString(sb.ToString(),mFont).Width;			

			sb = new StringBuilder();
			for(int k = startIndex; k<Math.Min(mListItems.Count,startIndex+5); k++)
			{
				sb.Append(mListItems[k].ToString());
				sb.Append(Environment.NewLine);
			}		
			expandedHeight = (int)  g.MeasureString(sb.ToString(),mFont).Height;		
	
			recalc = false;
		}

		private void mListItems_OnItemAdded(NListItem item, int index)
		{
			recalc=true;
		}

		private void mListItems_OnItemRemoved(NListItem item, int index)
		{
			recalc = true;
		}
		#endregion

	
	}
}
