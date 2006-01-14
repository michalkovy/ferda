#region CVS info
// $Id: FlatButton.cs,v 1.1 2005/12/31 10:04:20 kovacm Exp $
// $Log: FlatButton.cs,v $
// Revision 1.1  2005/12/31 10:04:20  kovacm
// Prenos ze stare subversion repository - dalsi cast
//
// Revision 1.1  2005/05/25 23:08:23  mr_circuit
// Beta release.
//
// Revision 1.2  2005/04/25 06:53:50  mr_circuit
// Window activation enhanced.
//
// Revision 1.1  2005/02/15 22:43:39  mr_circuit
// First check in.
//
#endregion

using System;
using System.Drawing;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;

namespace DockDotNET
{
	/// <summary>
	/// Summary description for FlatButton.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(FlatButton))]
	public class FlatButton : System.Windows.Forms.Button
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FlatButton(System.ComponentModel.IContainer container)
		{
			///
			/// Required for Windows.Forms Class Composition Designer support
			///
			container.Add(this);
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		public FlatButton()
		{
			///
			/// Required for Windows.Forms Class Composition Designer support
			///
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		#endregion

		#region Variables
		private bool pressed = false;

		private Color lightColor = Color.White;
		private Color shadowColor = Color.Black;
		#endregion
		
		#region Properties
		public Color LightColor
		{
			get { return lightColor; }
			set { lightColor = value; }
		}

		public Color ShadowColor
		{
			get { return shadowColor; }
			set { shadowColor = value; }
		}

		public bool Pressed
		{
			get { return pressed; }
		}
		#endregion

		#region Paint
		public event PaintEventHandler PostPaint;

		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics graphics = e.Graphics;

			graphics.FillRectangle(new SolidBrush(this.BackColor), this.ClientRectangle);

			Point pt = MousePosition;
			Rectangle rc = RectangleToScreen(ClientRectangle);

			if (rc.Contains(pt) && Enabled)
			{
				Pen pen1, pen2;

				if (MouseButtons == MouseButtons.Left)
				{
					pen1 = new Pen(shadowColor);
					pen2 = new Pen(lightColor);

					pressed = true;
				}
				else
				{
					pen1 = new Pen(lightColor);
					pen2 = new Pen(shadowColor);

					pressed = false;
				}

				graphics.DrawLine(pen1, 0, 0, Width-1, 0);
				graphics.DrawLine(pen1, 0, 1, 0, Height-2);
				graphics.DrawLine(pen2, 0, Height-1, Width-1, Height-1);
				graphics.DrawLine(pen2, Width-1, 1, Width-1, Height-2);
			}

			if (PostPaint != null)
				PostPaint(this, e);
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
			Invalidate();
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			pressed = false;
			Invalidate();
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			Invalidate();
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			Invalidate();
		}
		#endregion
	}
}
