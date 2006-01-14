using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
namespace Netron.GraphLib
{
	/// <summary>
	/// Base class for a (graph) shape embedded control 
	/// </summary>
	public abstract class NetronGraphControl
	{

		#region Fields

		protected Font mFont;

		protected Shape parent;
		protected Point mLocation;
		protected int mWidth = 100;
		protected int mHeight = 15;

		#endregion

		#region Properties

		public Rectangle Rectangle
		{
			get{return new Rectangle(mLocation.X,mLocation.Y,mWidth,mHeight);}
			
		}
		public Point Location
		{
			get{return mLocation;}
			set{mLocation = value;}
		}
		public int Height
		{
			get{return mHeight;}
			set{mHeight = value;}
		}
		public int Width
		{
			get{return mWidth;}
			set{mWidth = Math.Max(value,mHeight);}
		}
		#endregion

		#region Constructor
		public NetronGraphControl(Shape parent)
		{	
			this.parent = parent;
			Init();
		}
		#endregion

		#region Methods

		private void Init()
		{
			mFont = parent.Font;
		}

		public abstract void Paint(Graphics g);

		public abstract void OnMouseDown(MouseEventArgs e);
		public abstract void OnMouseMove(MouseEventArgs e);

		public abstract bool Hit(Point p);

		public virtual void OnKeyDown(KeyEventArgs e){}
		public virtual void OnKeyPress(KeyPressEventArgs e){}
		
		#endregion

	}

	
}
