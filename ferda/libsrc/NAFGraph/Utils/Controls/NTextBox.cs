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
	public class NTextBox : NetronGraphControl
	{
		#region Events
		public delegate void ResizeInfo(SizeF newSize);

		public event ResizeInfo OnResize;

		#endregion		

		#region Fields
		protected int mSelectedIndex = -1;	
		protected string mText = "";
		protected bool resize = true;
		protected bool mExpanded = false;
		protected NListItemCollection mListItems;
		protected Size expandedSize;
		protected Timer timer;
		protected bool yn;
		protected bool mEditing = false;
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
		public bool Editing
		{
			get{return mEditing;}
			set{mEditing = value;
			if(value)
				timer.Start();
			else
				timer.Stop();
			}
		}
		#endregion

		#region Constructor
		public NTextBox(Shape shape) : base(shape)
		{
			mListItems = new NListItemCollection();
			timer = new Timer();
			timer.Interval = 600;
			timer.Tick+=new EventHandler(timer_Tick);
			
		}
		
		public NTextBox(Shape shape, int width, int height):base(shape)
		{			
			this.mWidth = width;
			this.mHeight = height;
			mListItems = new NListItemCollection();
			timer = new Timer();
			timer.Interval = 600;
			timer.Tick+=new EventHandler(timer_Tick);
			
		}

		#endregion

		#region Methods

		public override void Paint(Graphics g)
		{
			
			if(resize)
			{
				SizeF s = new SizeF(g.MeasureString(mText,mFont));
				s.Width =Math.Max(100,s.Width);
				s.Height =Math.Max(s.Height, 15);
				if(OnResize!=null) OnResize(s);
				mWidth =(int) s.Width;
				mHeight =(int) s.Height;
				resize = false;
			}
			if(mEditing)
				g.FillRectangle(Brushes.WhiteSmoke,new Rectangle(mLocation.X,mLocation.Y,mWidth,mHeight));
			g.DrawString(mText,mFont,Brushes.Black,mLocation.X+2,mLocation.Y+1);
			ControlPaint.DrawBorder(g,new Rectangle(mLocation.X,mLocation.Y,mWidth,mHeight),Color.DarkGray, ButtonBorderStyle.Solid);

			
			
			//ControlPaint.DrawContainerGrabHandle(g,new Rectangle(mLocation.X-50,mLocation.Y-5,100,100));
			//ControlPaint.DrawFocusRectangle(g,new Rectangle(mLocation.X-50,mLocation.Y-5,100,100));
			//ControlPaint.DrawGrabHandle(g,new Rectangle(mLocation.X-50,mLocation.Y-5,100,100),true,true);
			//ControlPaint.DrawLockedFrame(g,new Rectangle(mLocation.X-50,mLocation.Y-5,100,100),true);
			if(yn)
			{
				SizeF s=g.MeasureString(mText,this.mFont);
				g.FillRectangle(Brushes.Black, mLocation.X+s.Width+2,mLocation.Y+2,2,10);
			}
		}

		public override void OnMouseDown(MouseEventArgs e)
		{
			//Point p =new Point(e.X,e.Y);
			mEditing = true;
			timer.Start();
			
		}
		public override void OnKeyDown(KeyEventArgs e)
		{
//			if(mEditing)
//			{
//				if(e.KeyData==Keys.Back)
//				{
//					if(mText.Length>0)
//						mText = mText.Substring(0,mText.Length-1);
//					parent.Invalidate();
//					e.Handled=true;
//				}
//				else if(e.KeyData==Keys.Escape)
//				{Editing = false;e.Handled=true;}
//				else 
//				{
//					
//				}
//			}
		}


		public override void OnKeyPress(KeyPressEventArgs e)
		{
			if(mEditing)
			{
				if(e.KeyChar==(char)Keys.Back)
				{
					if(mText.Length>0)
						mText = mText.Substring(0,mText.Length-1);
					parent.Invalidate();
					return;	
				}
				else if(e.KeyChar==(char)Keys.Escape)
				{
					Editing = false;e.Handled=true;
				}
				else 
				{
					mText+= e.KeyChar;					
				}
				this.parent.Invalidate();	
			}
		}



		public override bool Hit(Point p)
		{
			if(mExpanded)
			{
				return new Rectangle(mLocation.X+mHeight,mLocation.Y,expandedSize.Width,expandedSize.Height).Contains(p);			
				//return false;
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
			float step = ((float)expandedSize.Height) / ((float) mListItems.Count);

			for(int k =0;k<mListItems.Count; k++)
			{
				if((mLocation.Y +k*step<p.Y) && (p.Y<mLocation.Y + (k+1)*step))
				{
					mExpanded = false;
					mSelectedIndex = k;
					return mListItems[k].ToString();
				}
			}
			return null;
		}

		#endregion

		private void timer_Tick(object sender, EventArgs e)
		{
			yn = !yn;			
			parent.Invalidate();
			
		}
		public override void OnMouseMove(MouseEventArgs e)
		{

		}

	}
}
