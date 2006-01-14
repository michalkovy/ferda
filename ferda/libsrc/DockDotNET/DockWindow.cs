#region CVS info
// $Id: DockWindow.cs,v 1.1 2005/12/31 10:04:20 kovacm Exp $
// $Log: DockWindow.cs,v $
// Revision 1.1  2005/12/31 10:04:20  kovacm
// Prenos ze stare subversion repository - dalsi cast
//
// Revision 1.1  2005/05/25 23:08:23  mr_circuit
// Beta release.
//
// Revision 1.13  2005/05/21 11:08:34  mr_circuit
// IsVisible property modified. -> Real show and hide behavior now.
//
// Revision 1.12  2005/05/20 19:45:05  mr_circuit
// Small changes.
//
// Revision 1.11  2005/04/25 06:53:50  mr_circuit
// Window activation enhanced.
//
// Revision 1.10  2005/03/24 12:13:36  mr_circuit
// Removed the window layout bug when using controls with non-standard anchors.
//
// Revision 1.9  2005/03/11 23:43:55  mr_circuit
// DockManager now contains a list for each panel and doc and tool window.
//
// Revision 1.8  2005/03/10 23:49:32  mr_circuit
//  - Resize event of a DockPanel will be sent to its associated DockWindow, too.
//  - If a tool tip is shown, a context menu for close and undock can be used.
//
// Revision 1.7  2005/03/10 22:03:02  mr_circuit
// IsVisible is now a set property, two. (Toogles Hide() or Close())
//
// Revision 1.6  2005/03/09 21:39:51  mr_circuit
// Added the HideOnClose flag to the DockWindow class.
//
// Revision 1.5  2005/03/04 15:08:44  mr_circuit
// Some bugs fixed.
//
// Revision 1.4  2005/03/03 01:17:23  mr_circuit
// Fixed some bugs concerning the border adjustment. Panels can now be re-docked during removement of their host container.
//
// Revision 1.3  2005/03/01 00:41:14  mr_circuit
// Finished forced docking. Close, hide and show of dockable windows now stable. Introduced "WasDocked" property for user handling of show events.
//
// Revision 1.2  2005/02/24 01:55:18  mr_circuit
// Completely new basis for the core components. Docking now works (nearly) perfect.
//
// TODO:
//  - Automation interface for forced docking.
//  - XML container tree dump and import.
//  - Drag complete containers including all panels into an own window.
//  - Dock panels in DockWindow (complete docking mayhem... :-))
//
// IMPORTANT:
//  - Sample program.
//  - First release.
//
// Revision 1.1  2005/02/15 22:43:38  mr_circuit
// First check in.
//
#endregion

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;

namespace DockDotNET
{
	/// <summary>
	/// Enumerates the standard types for dockable windows.
	/// </summary>
	public enum DockContainerType
	{
		None,
		Document,
		ToolWindow
	}

	/// <summary>
	/// The DockWindow class is derived from the standard framework class System.Windows.Forms.Form.
	/// It prepares the window for docking with the help of an own container of the DockPanel type.
	/// </summary>
	[Designer(typeof(System.Windows.Forms.Design.ControlDesigner))]
	public class DockWindow : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#region Construction and dispose
		public DockWindow()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			ShowInTaskbar = false;
			
			controlContainer.Form = this;
			controlContainer.Resize += new EventHandler(controlContainer_Resize);
			controlContainer.Activated += new EventHandler(controlContainer_Activated);
			controlContainer.Deactivate += new EventHandler(controlContainer_Deactivate);

			//this.ControlAdded += new ControlEventHandler(DockWindow_ControlAdded);
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// DockWindow
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(104, 48);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "DockWindow";
			this.Text = "DockWindow";
		}
		#endregion

		#region Variables
		protected bool moving = false;
		protected bool isDragWindow = false;
		protected bool isLoaded = false;
		protected bool wasDocked = false;
		protected bool hideOnClose = false;

		protected Point ptStart;

		protected DockWindow dragWindow = null;
		protected DockContainerType dockType = DockContainerType.None;
		protected DockPanel controlContainer = new DockPanel();
		protected DockContainer lastHost = null;
		internal DockContainer dragTarget = null;

        //THIS HAS BEEN CHENGED BY FEDRA

        protected string oldText;

        //END OF CHANGE

		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the dock element type.
		/// </summary>
		public DockContainerType DockType
		{
			get { return dockType; }
			set { dockType = value; /*tbd*/}
		}

		/// <summary>
		/// Gets the move state of the window.
		/// </summary>
		public bool Moving
		{
			get { return moving; }
		}

		/// <summary>
		/// Gets or sets a flag that prevents the window from closing.
		/// </summary>
		public bool HideOnClose
		{
			get { return hideOnClose; }
			set { hideOnClose = value; }
		}

		/// <summary>
		/// Gets the panel that is connected to this window.
		/// </summary>
		public DockPanel ControlContainer
		{
			get { return controlContainer; }
		}

		/// <summary>
		/// Gets or sets the drag window flag.
		/// </summary>
		internal bool IsDragWindow
		{
			get { return isDragWindow; }
			set
			{
				isDragWindow = value;

				if (isDragWindow)
				{
					this.FormBorderStyle = FormBorderStyle.None;
					this.TransparencyKey = this.BackColor;
				}
			}
		}

		/// <summary>
		/// Gets the retrieved target of a drag operation.
		/// </summary>
		internal DockContainer DragTarget
		{
			get { return dragTarget; }
		}

		/// <summary>
		/// Gets the container of the associated panel (null if not docked).
		/// </summary>
		public DockContainer HostContainer
		{
			get
			{
				if (IsDocked)
					return controlContainer.Parent as DockContainer;
				else
					return null;
			}
		}

		/// <summary>
		/// Gets the docking state of the window.
		/// </summary>
		public bool IsDocked
		{
			get { return ((controlContainer.Parent != this) && (controlContainer.Parent is DockContainer)); }
		}

		/// <summary>
		/// Gets the docking state after last close.
		/// </summary>
		public bool WasDocked
		{
			get { return wasDocked; }
		}

		/// <summary>
		/// Gets the loaded flag.
		/// </summary>
		public bool IsLoaded
		{
			get { return isLoaded; }
		}

		/// <summary>
		/// Gets the real visibility state of the window contents.
		/// Use this property instead of Visible, since it can not be overridden.
		/// </summary>
		public bool IsVisible
		{
			get { return this.Visible || IsDocked; }
			set
			{
				if (value)
				{
					if (lastHost != null)
					{
						if (!lastHost.IsDisposed)
						{
							lastHost.DockWindow(this, DockStyle.Fill);
							return;
						}

						lastHost = null;
					}
					
					Show();
				}
				else
				{
					wasDocked = IsDocked;

					if (wasDocked)
					{
						lastHost = controlContainer.Parent as DockContainer;
						this.Controls.Add(controlContainer);
					}
					else
						lastHost = null;
					
					Hide();
				}
			}
		}

        //THIS HAS BEEN CHENGED BY FEDRA

        /// <summary>
        /// Old text of the control that is used to locate the
        /// view with that name in the project manager
        /// </summary>
        public string OldText
        {
            set
            {
                oldText = value;
            }
            get
            {
                return oldText;
            }
        }

        // END OF CHANGE

		#endregion

		#region Window movement
		/// <summary>
		/// Calls the base classs routine and invokes drag handling.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected override void OnMove(EventArgs e)
		{
			base.OnMove(e);

			MoveWindow();
		}

		/// <summary>
		/// Invokes the drag event and adjusts the size and location if a valid docking position was received.
		/// This method is only used by explicit drag windows (see flag).
		/// </summary>
		internal void MoveWindow()
		{
			if ((this.DragWindow != null) && isDragWindow)
			{
				dragTarget = null;

				DockEventArgs e = new DockEventArgs(new Point(MousePosition.X, MousePosition.Y), this.dockType, false);
				this.DragWindow(this, e);

				dragTarget = e.Target;

				if (dragTarget != null)
				{
					this.Size = dragTarget.Size;

					if (dragTarget.Parent != null)
                        this.Location = dragTarget.RectangleToScreen(dragTarget.ClientRectangle).Location;
					else
						this.Location = dragTarget.Location;
				}
			}
		}

		/// <summary>
		/// Closes the attached drag window.
		/// </summary>
		private void CloseDragWindow()
		{
			if (dragWindow != null)
			{
				dragWindow.Close();
				dragWindow.Dispose();
				dragWindow = null;
			}
		}

		/// <summary>
		/// Creates an attached drag window.
		/// </summary>
		private void ShowDragWindow()
		{
			if (!isDragWindow)
			{
				Rectangle rc = RectangleToScreen(this.Bounds);
				dragWindow = new DockWindow();
				dragWindow.IsDragWindow = true;
				dragWindow.Size = this.Size;
				dragWindow.DockType = this.DockType;
				dragWindow.DragWindow += DragWindow;
				dragWindow.Show();
				dragWindow.Location = this.Location;
			}
		}
		
		/// <summary>
		/// Since there are no non-client area notifications in .NET, this message loop hook is needed.
		/// Captures a window move event and starts own movement procedure with an attached drag window.
		/// </summary>
		/// <param name="m">The Windows Message to process.</param>
		protected override void WndProc(ref Message m)
		{
			if ((m.Msg == (int)Win32.Msgs.WM_NCLBUTTONDOWN) && (m.WParam == (IntPtr)Win32.HitTest.HTCAPTION))
			{	
				CloseDragWindow();
				ShowDragWindow();

				moving = true;
				ptStart = new Point(MousePosition.X, MousePosition.Y);

				this.Capture = true;
				return;
			}
			else if (moving)
			{
				if (m.Msg == (int)Win32.Msgs.WM_MOUSEMOVE)
				{
					if (MouseButtons == MouseButtons.None)
					{
						CloseDragWindow();
						moving = false;
					}
					else
					{
						if (dragWindow.dragTarget == null)
							dragWindow.Location = new Point(this.Location.X+MousePosition.X-ptStart.X, this.Location.Y+MousePosition.Y-ptStart.Y); 
						else
							dragWindow.MoveWindow();
						
						if (dragWindow.dragTarget == null)
							dragWindow.Size = this.Size;
					
						this.Capture = true;
					}
					return;
				}
				else if ((m.Msg == (int)Win32.Msgs.WM_LBUTTONUP) || ((m.Msg == (int)Win32.Msgs.WM_NCMOUSEMOVE) && (MouseButtons == MouseButtons.None)))
				{
					Point pt = dragWindow.Location;
					CloseDragWindow();
					BringToFront();
					this.Capture = false;
					moving = false;

					ConfirmDock();
					this.Location = pt;
				}
			}

			base.WndProc(ref m);
		}

		/// <summary>
		/// Invokes a drag event that accepts a valid position for docking.
		/// </summary>
		/// <returns>Returns the target docking container.</returns>
		internal DockContainer ConfirmDock()
		{
			DockEventArgs e = new DockEventArgs(new Point(MousePosition.X, MousePosition.Y), this.dockType, true);
			if (DragWindow != null)
				DragWindow(this, e);

			return e.Target;
		}

		/// <summary>
		/// Occurs, when the window is moved over the screen.
		/// </summary>
		public event DockEventHandler DragWindow;
		#endregion

		#region Paint
		/// <summary>
		/// Raises the Paint event or draws a transparent window with a hatched border.
		/// </summary>
		/// <param name="e">A PaintEventArgs that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			if (isDragWindow)
			{
				Graphics graphics = e.Graphics;

				HatchBrush brush = new HatchBrush(HatchStyle.Percent50, Color.Black, this.TransparencyKey);
				Rectangle rc = this.ClientRectangle;
				graphics.DrawRectangle(new Pen(brush, 6), rc);
			}
			else
				base.OnPaint(e);
		}
		#endregion

		#region Load and container management
		/// <summary>
		/// Calls the CreateContainer function, if not in design mode.
		/// Raises the Load event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected override void OnLoad(EventArgs e)
		{
			if (!isDragWindow && !IsDocked)
				CreateContainer();

			base.OnLoad(e);
		}

		/// <summary>
		/// Creates the control container and fills it with the controls contained by the window.
		/// The controls will behave like at design time.
		/// </summary>
		public void CreateContainer()
		{
			if (this.DesignMode)
				return;

			controlContainer.Dock = DockStyle.Fill;

			int max = this.Controls.Count;
			int off = 0;
			Control c;
			
			if (!this.Controls.Contains(controlContainer))
			{
				controlContainer.Dock = DockStyle.None;
				this.Controls.Add(controlContainer);
				controlContainer.Location = Point.Empty;
				controlContainer.Size = ClientRectangle.Size;
			}

			while (this.Controls.Count > off)
			{
				if (this.Controls[0] != controlContainer)
				{
					c = this.Controls[off];
					this.Controls.Remove(c);
					
					if (c != null)
						controlContainer.Controls.Add(c);
				}
				else
					off = 1;
			}

			controlContainer.Dock = DockStyle.Fill;
		}

		/// <summary>
		/// Releases the window from its host container.
		/// </summary>
		internal void Release()
		{
			if (IsDocked)
				HostContainer.ReleaseWindow(this);
		}
		
		private void controlContainer_Resize(object sender, EventArgs e)
		{
			if (IsDocked)
				this.OnResize(e);
		}
		
		private void controlContainer_Activated(object sender, EventArgs e)
		{
			this.OnActivated(e);
		}

		private void controlContainer_Deactivate(object sender, EventArgs e)
		{
			this.OnDeactivate(e);
		}
		#endregion

		#region Key mapping
		/// <summary>
		/// Invokes the KeyDown event.
		/// Is used as interface to the key routines of the DockManager.
		/// </summary>
		/// <param name="e">A KeyEventArgs that contains the event data.</param>
		public void InvokeKeyDown(System.Windows.Forms.KeyEventArgs e)
		{
			OnKeyDown(e);
		}

		/// <summary>
		/// Invokes the KeyUp event.
		/// Is used as interface to the key routines of the DockManager.
		/// </summary>
		/// <param name="e">A KeyEventArgs that contains the event data.</param>
		public void InvokeKeyUp(System.Windows.Forms.KeyEventArgs e)
		{
			OnKeyUp(e);
		}
		#endregion

		#region Show and close
		/// <summary>
		/// Implements the VisibleChanged event of the base class.
		/// Releases the window if showed.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);

			if (this.Visible)
			{
				if (isLoaded)
					Release();
				else
					isLoaded = true;
			}
		}

		/// <summary>
		/// Implements the Closing event of the base class.
		/// Adds the container back to the own control list, if docked and sets the WasDocked flag.
		/// </summary>
		/// <param name="e">A CancelEventArgs that contains the event data.</param>
		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);
		
			this.IsVisible = false;

			if (hideOnClose)
				e.Cancel = true;	
		}
		#endregion
	}
}
