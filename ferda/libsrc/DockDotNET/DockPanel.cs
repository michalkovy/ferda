#region CVS info
// $Id: DockPanel.cs,v 1.1 2005/12/31 10:04:20 kovacm Exp $
// $Log: DockPanel.cs,v $
// Revision 1.1  2005/12/31 10:04:20  kovacm
// Prenos ze stare subversion repository - dalsi cast
//
// Revision 1.1  2005/05/25 23:08:23  mr_circuit
// Beta release.
//
// Revision 1.5  2005/05/21 11:55:53  mr_circuit
// DockPanel overrides ToString to display its associated forms caption.
//
// Revision 1.4  2005/04/25 06:53:50  mr_circuit
// Window activation enhanced.
//
// Revision 1.3  2005/03/07 22:36:53  mr_circuit
// Added comments and changed some functions to override.
//
// Revision 1.2  2005/02/16 00:50:11  mr_circuit
// Only the active panel is closed in document type containers, if the close button is pressed.
//
// Revision 1.1  2005/02/15 22:43:38  mr_circuit
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
	/// This class is derived from the standard framework class System.Windows.Forms.Panel.
	/// It is used as final container of the window's controls and is transferred between DockContainer objects and the window.
	/// </summary>
	public class DockPanel : System.Windows.Forms.Panel
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#region Construction and dispose
		public DockPanel(System.ComponentModel.IContainer container)
		{
			///
			/// Required for Windows.Forms Class Composition Designer support
			///
			container.Add(this);
			InitializeComponent();

			Init();
		}

		public DockPanel()
		{
			///
			/// Required for Windows.Forms Class Composition Designer support
			///
			InitializeComponent();

			Init();
		}

		private void Init()
		{
			SetStyle(ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
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
		#endregion

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// DockPanel
			// 
			this.AutoScroll = true;

		}
		#endregion

		#region Variables
		RectangleF tabRect = Rectangle.Empty;

		DockWindow form;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the rectangle of the tab panel.
		/// </summary>
		[Browsable(false)]
		public RectangleF TabRect
		{
			get { return tabRect; }
			set { tabRect = value; }
		}

		/// <summary>
		/// Gets or sets the host window.
		/// </summary>
		[Browsable(false)]
		public DockWindow Form
		{
			get { return form; }
			set
			{
				form = value;

				if (form != null)
					form.TextChanged += new EventHandler(form_TextChanged);
			}
		}
		#endregion

		#region Overrides
		/// <summary>
		/// Overrides the base class function OnMouseDown.
		/// Used to set the focus to the parent control.
		/// </summary>
		/// <param name="e">A MouseEventArgs that contains the mouse event data.</param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (Parent != null)
				Parent.Focus();

			base.OnMouseDown (e);
		}
		
		/// <summary>
		/// Overrides the base class function IsInputKey.
		/// </summary>
		/// <param name="keyData">The key that is to be evaluated.</param>
		/// <returns>Always set to true.</returns>
		protected override bool IsInputKey(Keys keyData)
		{
			return true;
		}

		/// <summary>
		/// Overrides the base class function OnPaint.
		/// Fires the PostPaint event after drawing the contents.
		/// </summary>
		/// <param name="e">A PaintEventArgs that contains the paint data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint (e);

			if (PostPaint != null)
				PostPaint(this, e);
		}
		
		/// <summary>
		/// Overrides the base class function ToString.
		/// </summary>
		/// <returns>The caption string of the attached form.</returns>
		public override string ToString()
		{
			return form.Text;
		}
		#endregion

		#region Own events
		/// <summary>
		/// Occurs after the base drawing is finished.
		/// </summary>
		public event PaintEventHandler PostPaint;

		/// <summary>
		/// Occurs when the panel gets activated.
		/// </summary>
		public event EventHandler Activated;

		/// <summary>
		/// Occurs when the panel gets deactivated.
		/// </summary>
		public event EventHandler Deactivate;

		/// <summary>
		/// Sets the focus to the panel.
		/// </summary>
		/// <param name="activate">True, if the panel is activated.</param>
		public void SetFocus(bool activate)
		{
			if (activate && (Activated != null))
				Activated(this, new EventArgs());
			else if (!activate && (Deactivate != null))
				Deactivate(this, new EventArgs());
		}
		#endregion

		/// <summary>
		/// A message handler for the host form event TextChanged.
		/// Refreshes the parent control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
		private void form_TextChanged(object sender, EventArgs e)
		{
			if (Parent != null)
				Parent.Invalidate();
		}
	}
}
