#region CVS info
// $Id: DockContainer.cs,v 1.1 2005/12/31 10:04:20 kovacm Exp $
// $Log: DockContainer.cs,v $
// Revision 1.1  2005/12/31 10:04:20  kovacm
// Prenos ze stare subversion repository - dalsi cast
//
// Revision 1.1  2005/05/25 23:08:22  mr_circuit
// Beta release.
//
// Revision 1.12  2005/05/23 17:32:55  mr_circuit
// Icons sizes are fixed to 16x16. Positions updated also.
//
// Revision 1.11  2005/05/20 09:03:05  mr_circuit
// One more tab select function added
//
// Revision 1.10  2005/04/25 06:53:50  mr_circuit
// Window activation enhanced.
//
// Revision 1.9  2005/03/10 23:49:32  mr_circuit
//  - Resize event of a DockPanel will be sent to its associated DockWindow, too.
//  - If a tool tip is shown, a context menu for close and undock can be used.
//
// Revision 1.8  2005/03/09 21:39:50  mr_circuit
// Added the HideOnClose flag to the DockWindow class.
//
// Revision 1.7  2005/03/07 22:36:53  mr_circuit
// Added comments and changed some functions to override.
//
// Revision 1.6  2005/03/04 15:08:32  mr_circuit
// Some bugs fixed.
//
// Revision 1.5  2005/03/03 01:17:23  mr_circuit
// Fixed some bugs concerning the border adjustment. Panels can now be re-docked during removement of their host container.
//
// Revision 1.4  2005/03/01 00:41:14  mr_circuit
// Finished forced docking. Close, hide and show of dockable windows now stable. Introduced "WasDocked" property for user handling of show events.
//
// Revision 1.3  2005/02/24 01:55:17  mr_circuit
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
// Revision 1.2  2005/02/16 00:50:11  mr_circuit
// Only the active panel is closed in document type containers, if the close button is pressed.
//
// Revision 1.1  2005/02/15 22:43:37  mr_circuit
// First check in.
//
#endregion

using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DockDotNET
{
	/// <summary>
	/// The DockContainer class is derived from the standard framework class System.Windows.Forms.Panel.
	/// It provides the basic functions to dock windows including the whole panel management.
	/// </summary>
	public class DockContainer : System.Windows.Forms.Panel
	{
		private System.ComponentModel.IContainer components;

		#region Construct and dispose
		public DockContainer(System.ComponentModel.IContainer container)
		{
			///
			/// Required for Windows.Forms Class Composition Designer support
			///
			container.Add(this);
			InitializeComponent();

			Init();
		}

		public DockContainer()
		{
			///
			/// Required for Windows.Forms Class Composition Designer support
			///
			InitializeComponent();

			Init();
		}

		/// <summary>
		/// The central initialization method for all constructors.
		/// </summary>
		private void Init()
		{
			// Enable double buffering.
			SetStyle(ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);

			// Create close button.
			btnClose.Hide();
			btnClose.ShadowColor = Color.Black;
			btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			btnClose.PostPaint += new PaintEventHandler(btnClose_PostPaint);
			btnClose.Click += new EventHandler(btnClose_Click);
			this.Controls.Add(btnClose);

			// Create tab left button.
			btnTabL.Hide();
			btnTabL.ShadowColor = Color.Black;
			btnTabL.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			btnTabL.Enabled = false;
			btnTabL.PostPaint += new PaintEventHandler(btnTabL_PostPaint);
			btnTabL.Click += new EventHandler(btnTabL_Click);
			this.Controls.Add(btnTabL);

			// Create tab right button.
			btnTabR.Hide();
			btnTabR.ShadowColor = Color.Black;
			btnTabR.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			btnTabR.Enabled = false;
			btnTabR.PostPaint += new PaintEventHandler(btnTabR_PostPaint);
			btnTabR.Click += new EventHandler(btnTabR_Click);
			this.Controls.Add(btnTabR);
		
			// Drag Panel.
			dragPanel.Hide();
			dragPanel.MouseDown += new MouseEventHandler(dragPanel_MouseDown);
			dragPanel.MouseMove += new MouseEventHandler(dragPanel_MouseMove);
			dragPanel.MouseUp += new MouseEventHandler(dragPanel_MouseUp);
			this.Controls.Add(dragPanel);

			// Context menu.
            //THIS HAS BEEN CHANGED BY FERDA
            ToolStripMenuItem rename = new ToolStripMenuItem("Rename");
            rename.Click += new EventHandler(RenameClick);
            ToolStripMenuItem undock = new ToolStripMenuItem("Undock");
            undock.Click += new EventHandler(UndockClick);
            ToolStripSeparator sep = new ToolStripSeparator();
            ToolStripMenuItem close = new ToolStripMenuItem("Close");
            close.Click += new EventHandler(CloseClick);

            contextMenu.Items.AddRange(new ToolStripItem[] 
                { rename, undock, sep, close });
            
            //trying to get the middle wheel button to work
            this.MouseClick += new MouseEventHandler(DockContainer_MouseClick);

            //END OF CHANGE
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
			this.components = new System.ComponentModel.Container();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            //THIS HAS BEEN CHANGED BY FERDA
			this.contextMenu = new System.Windows.Forms.ContextMenuStrip();
            //END OF CHANGE
			// 
			// toolTip
			// 
			this.toolTip.Active = false;
			// 
			// DockContainer
			// 
			this.BackColor = System.Drawing.Color.Transparent;

		}
		#endregion

		#region Variables
		private Point ptStart = Point.Empty;
		private Size oldSize = Size.Empty;
		
		private DockContainerType dockType = DockContainerType.None;
		private DockPanel activePanel = null;
		private DockPanel dragPanel = new DockPanel();
		private DockWindow dragWindow = null;
		private DockContainer dragDummy = null;
		
		protected bool isActive = false;
		protected bool isDragContainer = false;
		protected bool disableOnControlRemove = false;
		protected bool removeable = true;
		private bool panelOverflow = false;
		private bool showIcons = true;
		private bool resizing  = false;
		private bool blockFocusEvents = false;
		
		private int panelOffset = 0;
		private int splitterWidth = 4;
		private int dockBorder = 20;
		
		protected ArrayList containerList = new ArrayList();
		protected ArrayList panelList = new ArrayList();
		
		private FlatButton btnClose = new FlatButton();
		private FlatButton btnTabL = new FlatButton();
		private FlatButton btnTabR = new FlatButton();
		
		private const int bottomDock = 24;
		private System.Windows.Forms.ToolTip toolTip;

        //THIS HAS BEEN CHAHGED BY FERDA
		private System.Windows.Forms.ContextMenuStrip contextMenu;
        //END OF CHANGE

		private const int topDock = 22;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the width of the splitter.
		/// </summary>
		public int SplitterWidth
		{
			get { return splitterWidth; }
			set { splitterWidth = value; }
		}
		
		/// <summary>
		/// Determines, if icons are shown in the panel tabs.
		/// </summary>
		public bool ShowIcons
		{
			get { return showIcons; }
			set { showIcons = value; }
		}
		
		/// <summary>
		/// Gets or sets the type of the container.
		/// </summary>
		virtual public DockContainerType DockType
		{
			get { return dockType; }
			set
			{
				dockType = value;

				try
				{
					if (dockType == DockContainerType.Document)
					{
						btnTabR.Size = btnTabL.Size = btnClose.Size = new Size(14, 15);
						btnTabR.ForeColor = btnTabL.ForeColor = btnClose.ForeColor = Color.FromArgb(85, 85, 85);
						btnTabR.BackColor = btnTabL.BackColor = btnClose.BackColor = Color.FromArgb(247, 243, 233);
						btnTabR.LightColor = btnTabL.LightColor = btnClose.LightColor = SystemColors.Control;
						
						btnClose.Location = new Point(Width-18, DockPadding.Top-bottomDock+5);
						btnTabR.Location = new Point(Width-18-14, DockPadding.Top-bottomDock+5);
						btnTabL.Location = new Point(Width-18-2*14, DockPadding.Top-bottomDock+5);
					}
					else
					{
						btnClose.Size = new Size(17, 13);
						btnClose.Location = new Point(Width-DockPadding.Right-18, DockPadding.Top-topDock+5);
						btnClose.ForeColor = SystemColors.ControlText;
						btnClose.BackColor = SystemColors.Control;
						btnClose.LightColor = Color.White;
					}
				}
				catch
				{
				}
			}
		}

		/// <summary>
		/// Gets or sets the border of the active docking region.
		/// </summary>
		public int DockBorder
		{
			get { return dockBorder; }
			set { dockBorder = value; }
		}
		
		/// <summary>
		/// Gets or sets the selected panel of the container.
		/// </summary>
		[Browsable(false)]
		internal DockPanel ActivePanel
		{
			get { return activePanel; }
			set
			{
				if (activePanel == value)
					return;

				if (activePanel != null)
				{
					activePanel.Hide();
					activePanel.SetFocus(false);
				}

				activePanel = value;

				if (activePanel != null)
				{
					activePanel.Show();
					activePanel.SetFocus(true);
				}

				Invalidate();
			}
		}

		/// <summary>
		/// Gets the top container of the current hierarchy.
		/// </summary>
		public DockContainer TopLevelContainer
		{
			get
			{
				DockContainer c = this;

				while (c.Parent is DockContainer)
					c = c.Parent as DockContainer;

				return c;
			}
		}
		#endregion

		#region Paint
		/// <summary>
		/// Overrides the base class function.
		/// Draws borders and tabs.
		/// </summary>
		/// <param name="e">A PaintEventArgs that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			Graphics graphics = e.Graphics;

			HatchBrush dimBrush = new HatchBrush(HatchStyle.Percent50, Color.Black, Color.Transparent);
			Pen penBlack = new Pen(Color.Black);
			Pen penWhite = new Pen(Color.White);
			Brush textBrush;
			Font textFont;
			float n = 0;
			int y = 0;
			int h = 16;
			int y0 = 0;
					
			StringFormat sf = StringFormat.GenericDefault;
			sf.Trimming = StringTrimming.EllipsisCharacter;
			
			if (isDragContainer)
			{
				#region Drag container
				graphics.FillRectangle(dimBrush, this.ClientRectangle);
				#endregion
			}
			else if (panelList.Count <= 0)
			{
				#region Empty container
				if (containerList.Count <= 0)
					graphics.FillRectangle(new SolidBrush(SystemColors.ControlDark), this.ClientRectangle);
				#endregion
			}
			else if (dockType == DockContainerType.ToolWindow)
			{
				#region Toolbox drawing
				int w = Width-DockPadding.Left-DockPadding.Right+2;
				float hTitle = 0;
				y0 = DockPadding.Top-topDock;
				
				this.BackColor = SystemColors.Control;

				// Draw top bar.
				if (this.ContainsFocus)
				{
					// active
					graphics.FillRectangle(SystemBrushes.ActiveCaption, DockPadding.Left-1, y0+3, DockPadding.Left-1+w, y0+h);
					textBrush = SystemBrushes.ActiveCaptionText;

					btnClose.BackColor = SystemColors.ActiveCaption;
					btnClose.ForeColor = SystemColors.ActiveCaptionText;
					btnClose.Invalidate();
				}
				else
				{
					// inactive
					graphics.FillRectangle(SystemBrushes.Control, DockPadding.Left-1, y0+3, DockPadding.Left-1+w, y0+h);
					graphics.DrawLine(SystemPens.ControlDark, DockPadding.Left, y0+3, DockPadding.Left-1+w-2, y0+3);
					graphics.DrawLine(SystemPens.ControlDark, DockPadding.Left-1, y0+4, DockPadding.Left-1, y0+h+1);
					graphics.DrawLine(SystemPens.ControlDark, DockPadding.Left, y0+h+2, DockPadding.Left-1+w-2, y0+h+2);
					graphics.DrawLine(SystemPens.ControlDark, DockPadding.Left-1+w-1, y0+4, DockPadding.Left-1+w-1, y0+h+1);
					textBrush = SystemBrushes.ControlText;

					btnClose.BackColor = SystemColors.Control;
					btnClose.ForeColor = SystemColors.ControlText;
					btnClose.Invalidate();
				}

				// Draw top bar string.
				if (activePanel != null)
				{
					hTitle = graphics.MeasureString(activePanel.Form.Text, Font).Height;
					graphics.DrawString(activePanel.Form.Text, Font, textBrush, DockPadding.Left-1+2, y0+(topDock-hTitle)/2+1, sf);
				}

				// Draw panel border.
				graphics.DrawRectangle(SystemPens.ControlDark, DockPadding.Left-1, y0+21, w-1, Height-DockPadding.Top-DockPadding.Bottom+1);
				
				// Draw bottom bar, if needed.
				if (panelList.Count > 1)
				{
					// Background rectangle.
					graphics.FillRectangle(new SolidBrush(Color.FromArgb(247, 243, 233)), DockPadding.Left-1+1, Height-DockPadding.Bottom+3, w-2, bottomDock-3);
					graphics.DrawLine(penBlack, DockPadding.Left-1+1, Height-DockPadding.Bottom+3, w-2, Height-DockPadding.Bottom+3);

					n = DockPadding.Left-1+4;
					y = Height-DockPadding.Bottom-2+bottomDock;
					h = bottomDock-5;
				
					foreach (DockPanel panel in panelList)
					{
						SizeF panelSize = MeasurePanel(panel, graphics, true, Font);
						panel.TabRect = new RectangleF(n, (float)(Height-DockPadding.Bottom+2), panelSize.Width, (float)(bottomDock-4));
							
						if (panel == activePanel)
						{
							// Active panel.
							graphics.FillRectangle(SystemBrushes.Control, panel.TabRect);
							// ...left line
							graphics.DrawLine(penBlack, n+panelSize.Width, y, n+panelSize.Width, y-h);
							// ...long line
							graphics.DrawLine(penBlack, n+1, y, n+panelSize.Width, y);
							// ...right line
							graphics.DrawLine(penWhite, n, y, n, y-h);
							// Set text brush.
							textBrush = new SolidBrush(Color.Black);
						}
						else
						{
							// Inactive panel.
							int i = panelList.IndexOf(panel)+1;
							if (i != panelList.IndexOf(activePanel))
								graphics.DrawLine(new Pen(Color.FromArgb(128, 128, 128)), n+panelSize.Width-1, y-2, n+panelSize.Width-1, y-h+3);
							textBrush = new SolidBrush(Color.FromArgb(85, 85, 85));
						}

						// Panel icon.
						RectangleF rc = new RectangleF(n+3, y+3+(-bottomDock+2-panelSize.Height)/2, panelSize.Width-6, panelSize.Height);
						
						if (showIcons && (panel.Form.Icon != null))
						{
							graphics.DrawIcon(panel.Form.Icon, new Rectangle((int)rc.X+1, (int)(y+2+(-bottomDock+2-16)/2), 16, 16));
							rc.Offset(panelSize.Height+3, 0);
							rc.Width -= panelSize.Height+3;
						}

						// Panel text.
						graphics.DrawString(panel.Form.Text, Font, textBrush, rc, sf);

						n += (int)panelSize.Width;
					}
				}
				#endregion
			}
			else if (dockType == DockContainerType.Document)
			{
				#region Document drawing
				y0 = DockPadding.Top-bottomDock;
				this.BackColor = SystemColors.Control;

				// Draw bounding rectangle.
				graphics.DrawRectangle(SystemPens.ControlDark, 0, y0, Width-1, Height-1-y0);

				// Draw background rectangle.
				graphics.FillRectangle(new SolidBrush(Color.FromArgb(247, 243, 233)), 1, y0+1, Width-2, bottomDock-3);
				graphics.DrawLine(penWhite, 1, y0+bottomDock-3, Width-2, y0+bottomDock-3);

				// Draw each header tab.
				n = DockPadding.Left-1+4;
				y = 3;
				h = -bottomDock+6;
				int xMax = Width-18-2*14;
				panelOverflow = false;
				
				foreach (DockPanel panel in panelList)
				{
					if (panelList.IndexOf(panel) < panelOffset)
					{
						panel.TabRect = RectangleF.Empty;
						continue;
					}

					if ((panel == activePanel) && (this.ContainsFocus))
						textFont = new Font(Font, FontStyle.Bold);
					else
						textFont = Font;
					
					SizeF panelSize = MeasurePanel(panel, graphics, false, textFont);
					panel.TabRect = new RectangleF(n, y0+3, panelSize.Width, (float)(bottomDock-4));

					if (n+(int)panelSize.Width >= xMax-2)
					{
						panelOverflow = true;
						panelSize.Width = xMax-n-2;
					}
							
					if (panel == activePanel)
					{
						// Active panel.
						graphics.FillRectangle(SystemBrushes.Control, panel.TabRect);
						// ...left line
						graphics.DrawLine(penBlack, n+panelSize.Width, y0+y, n+panelSize.Width, y0+y-h);
						// ...long line
						graphics.DrawLine(penWhite, n+1, y0+y, n+panelSize.Width, y0+y);
						// ...right line
						graphics.DrawLine(penWhite, n, y0+y, n, y0+y-h);
						// Set text brush and font.
						textBrush = new SolidBrush(Color.Black);
						
					}
					else
					{
						// Inactive panel.
						int i = panelList.IndexOf(panel)+1;
						if (i != panelList.IndexOf(activePanel))
							graphics.DrawLine(new Pen(Color.FromArgb(128, 128, 128)), n+panelSize.Width-1, y0+y+2, n+panelSize.Width-1, y0+y-h-2);
						// Set text brush and font.
						textBrush = new SolidBrush(Color.FromArgb(85, 85, 85));
					}

					// Panel icon.
					RectangleF rc = new RectangleF(n+3, y0+y-1+(bottomDock-2-panelSize.Height)/2, panelSize.Width-6, panelSize.Height);
						
					if (showIcons && (panel.Form.Icon != null))
					{
						graphics.DrawIcon(panel.Form.Icon, new Rectangle((int)rc.X, y0-1+(int)(y+(bottomDock-2-16)/2), 16, 16));
						rc.Offset(panelSize.Height+3, 0);
						rc.Width -= panelSize.Height+3;
					}

					// Panel text.
					graphics.DrawString(panel.Form.Text, textFont, textBrush, rc, sf);
					
					n += (int)panelSize.Width;

					if (panelOverflow)
						break;
				}

				if (panelOverflow)
				{
					// Redraw background.
					graphics.FillRectangle(new SolidBrush(Color.FromArgb(247, 243, 233)), xMax-2, y0+1, Width-2-xMax, bottomDock-3);

					// Activate buttons.
					if (panelOffset < panelList.Count-1)
						btnTabR.Enabled = true;
					else
						btnTabR.Enabled = false;
				}
				else
				{
					btnTabR.Enabled = false;
				}
				#endregion
			}
		}

		/// <summary>
		/// Measures the size of a DockPanel tab depending on a given font.
		/// </summary>
		/// <param name="panel">The DockPanel object.</param>
		/// <param name="graphics">The Graphics interface.</param>
		/// <param name="cut">Enables the reduction of the panel size according to the available space.</param>
		/// <param name="font">The target font.</param>
		/// <returns>The size of the panel tab.</returns>
		private SizeF MeasurePanel(DockPanel panel, Graphics graphics, bool cut, Font font)
		{
			SizeF ret = graphics.MeasureString(panel.Form.Text, font);

			if (font.Bold)
				ret.Width += 12;
			else
				ret.Width += 6;

			if (showIcons && (panel.Form.Icon != null))
				ret.Width += ret.Height+3;

			if ((ret.Width > (Width-DockPadding.Left-DockPadding.Right+2-8)/panelList.Count) && cut)
				ret.Width = (Width-DockPadding.Left-DockPadding.Right+2-8)/panelList.Count;

			return ret;
		}

		/// <summary>
		/// Overrides the base class function.
		/// Invokes an Invalidate after calling the base class function.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);
			if ((activePanel != null) && !blockFocusEvents)
				activePanel.SetFocus(true);
			Invalidate();
		}

		/// <summary>
		/// Overrides the base class function.
		/// Invokes an Invalidate after calling the base class function.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLeave(EventArgs e)
		{
			base.OnLeave(e);
			if ((activePanel != null) && !blockFocusEvents)
				activePanel.SetFocus(false);
			Invalidate();
		}

		/// <summary>
		/// Overrides the base class function.
		/// Invokes an Invalidate after calling the base class function.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			Invalidate();
		}
		#endregion

		#region Panel management
		/// <summary>
		/// Determines if a point is contained in the client region.
		/// </summary>
		/// <param name="pt">The point to test.</param>
		/// <returns>True, if the point is in the client region.</returns>
		protected bool HitTest(Point pt)
		{
			return RectangleToScreen(this.ClientRectangle).Contains(pt);
		}

		/// <summary>
		/// Retrieves a container that matches a specific mouse location, when a window is dragged.
		/// </summary>
		/// <param name="type">The target container type.</param>
		/// <param name="pt">The target position.</param>
		/// <returns>A valid container or null, if no suitable container was found.</returns>
		protected DockContainer GetTarget(Size srcSize, DockContainerType type, Point pt)
		{
			// Prepare rectangles.
			Rectangle rcClient = RectangleToScreen(this.ClientRectangle);
			Rectangle rcDock = new Rectangle(rcClient.Left+this.DockPadding.Left,
				rcClient.Top+this.DockPadding.Top,
				rcClient.Width-this.DockPadding.Left-this.DockPadding.Right,
				rcClient.Height-this.DockPadding.Top-this.DockPadding.Bottom);
			
			// Test on split.
			if (rcDock.Contains(pt))
			{
				DockContainer cont = null;

				if (pt.X-rcDock.Left <= dockBorder)
				{
					// Split left.
					cont = new DockContainer();
					cont.DockType = type;
					cont.Dock = DockStyle.Left;
					if (srcSize.Width > this.Width/2)
						cont.Width = this.Width/2;
					else
						cont.Width = srcSize.Width;
					cont.Height = this.Height;
					cont.Location = new Point(rcClient.X, rcClient.Y);
				}
				else if (rcDock.Right-pt.X <= dockBorder)
				{
					// Split right.
					cont = new DockContainer();
					cont.DockType = type;
					cont.Dock = DockStyle.Right;
					if (srcSize.Width > this.Width/2)
						cont.Width = this.Width/2;
					else
						cont.Width = srcSize.Width;
					cont.Height = this.Height;
					cont.Location = new Point(rcClient.X+this.Width-cont.Width, rcClient.Y);
				}
				else if (pt.Y-rcDock.Top <= dockBorder)
				{
					// Split top.
					cont = new DockContainer();
					cont.DockType = type;
					cont.Dock = DockStyle.Top;
					if (srcSize.Height > this.Height/2)
						cont.Height = this.Height/2;
					else
						cont.Height = srcSize.Height;
					cont.Width = this.Width;
					cont.Location = new Point(rcClient.X, rcClient.Y);
				}
				else if (rcDock.Bottom-pt.Y <= dockBorder)
				{
					// Split bottom.
					cont = new DockContainer();
					cont.DockType = type;
					cont.Dock = DockStyle.Bottom;
					if (srcSize.Height > this.Height/2)
						cont.Height = this.Height/2;
					else
						cont.Height = srcSize.Height;
					cont.Width = this.Width;
					cont.Location = new Point(rcClient.X, rcClient.Y+this.Height-cont.Height);
				}

				return cont;
			}
			
			// Test on add to own panel list.
			if (rcClient.Contains(pt))
			{
				if ((pt.Y <= rcClient.Top+this.DockPadding.Top) && (dockType == type))
					return this;
				else if ((dockType == type) && (dockType == DockContainerType.ToolWindow) && (panelList.Count > 1) && (pt.Y > rcClient.Top+this.Height-this.DockPadding.Bottom))
					return this;
			}

			return null;
		}
		
		/// <summary>
		/// Adds a panel to the container.
		/// </summary>
		/// <param name="src">The source window of the panel.</param>
		/// <param name="pt">The target position.</param>
		public void AddPanel(DockWindow src, Point pt)
		{
			try
			{
				if (!src.IsLoaded)
					src.CreateContainer();
				
				blockFocusEvents = true;
				DockPanel panel = src.ControlContainer;
				this.Controls.Add(panel);
				ActivePanel = panel;
				blockFocusEvents = false;
				
				if (!src.IsLoaded)
					src.Show();

				src.Hide();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		/// <summary>
		/// Selects a specific tab beginning with 0 from left to the right.
		/// </summary>
		/// <param name="i">The index of the tab.</param>
		public void SelectTab(int i)
		{
			try
			{
				if (panelList[i] != null)
					ActivePanel = panelList[i] as DockPanel;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		/// <summary>
		/// Selects a specific tab based on its reference pointer.
		/// </summary>
		/// <param name="p">The tab reference.</param>
		public void SelectTab(DockPanel p)
		{
			try
			{
				if ((panelList != null) && (p != null))
				{
					if (!panelList.Contains(p))
						return;

					ActivePanel = p;
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
		#endregion

		#region Control management
		/// <summary>
		/// Overrides the base class function.
		/// Before adding the control, the internal lists are updated depending on the type.
		/// </summary>
		/// <param name="e">A ControlEventArgs that contains the event data.</param>
		protected override void OnControlAdded(ControlEventArgs e)
		{
			if ((e.Control is DockPanel) && (e.Control != dragPanel))
			{
				// Add the panel to the list object and show buttons.
				panelList.Add(e.Control);
				ShowButtons();
			}
			else if (e.Control is FlatButton)
			{
				// Show or hide buttons.
				if (panelList.Count > 0)
					ShowButtons();
				else
					HideButtons();
			}

			// Adjust the borders of the container.
			AdjustBorders();
			
			// Raise event and redraw client area.
			base.OnControlAdded(e);
			Invalidate();
		}

		/// <summary>
		/// Overrides the base class function.
		/// Before removing the control, the internal lists are updated depending on the type.
		/// </summary>
		/// <param name="e">A ControlEventArgs that contains the event data.</param>
		protected override void OnControlRemoved(ControlEventArgs e)
		{
			if (disableOnControlRemove)
			{
				if (e.Control is DockContainer)
					containerList.Remove(e.Control);
				else if (e.Control is DockPanel)
					panelList.Remove(e.Control);

				return;
			}

			if ((e.Control is DockPanel) && (e.Control != dragPanel))
			{
				// Remove panel from list object.
				panelList.Remove(e.Control);

				// Show or hide buttons.
				if (panelList.Count == 0)
					HideButtons();
			}
			else if ((e.Control is DockContainer) && (containerList.Count == 2))
			{
				// Check, if the container is a drag dummy.
				if ((e.Control as DockContainer).isDragContainer)
					return;

                // Remove container from list object.
				containerList.Remove(e.Control);

				// Remove the last container and copy all panels.
				DockContainer cont = containerList[0] as DockContainer;
				cont.disableOnControlRemove = true;
				
				containerList.Remove(cont);
				this.Controls.Remove(cont);
				this.DockType = cont.DockType;
				removeable = cont.removeable;
				
				while (cont.containerList.Count > 0)
				{
					containerList.Add(cont.containerList[0] as DockContainer);
					this.Controls.Add(cont.containerList[0] as DockContainer);
				}

				DockPanel temp = cont.ActivePanel;
				while (cont.panelList.Count > 0)
					this.Controls.Add(cont.panelList[0] as DockPanel);
				ActivePanel = temp;

				cont.Dispose();
				cont = null;
			}
			
			// Adjust the borders of the container.
			AdjustBorders();
			
			// Retrieve the active panel.
			activePanel = null;
			if (panelList.Count > 0)
				ActivePanel = (DockPanel)panelList[panelList.Count-1];
			
			// Raise event and redraw client area.
			base.OnControlRemoved(e);
			Invalidate();
		}

		/// <summary>
		/// Adjusts the DockPadding property and drag panel according to the state of the container.
		/// </summary>
		protected void AdjustBorders()
		{
			int topOff = 0;
			int bottomOff = 0;
			int sideOff = 0;

			if ((panelList.Count != 0) || !removeable)
			{
				if (dockType == DockContainerType.ToolWindow)
				{
					topOff = topDock;
					if (panelList.Count > 1)
						bottomOff = bottomDock;
					else
						bottomOff = 1;
					sideOff = 1;
				}
				else if (dockType == DockContainerType.Document)
				{
					topOff = bottomDock;
					bottomOff = 2;
					sideOff = 2;
				}
			}

			this.DockPadding.Top = topOff;
			this.DockPadding.Bottom = bottomOff;
			this.DockPadding.Right = sideOff;
			this.DockPadding.Left = sideOff;
			
			switch (this.Dock)
			{
				case DockStyle.Left:
					this.DockPadding.Right += splitterWidth;
					dragPanel.Height = this.Height;
					dragPanel.Width = splitterWidth;
					dragPanel.Location = new Point(this.Width-splitterWidth, 0);
					dragPanel.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
					dragPanel.Cursor = Cursors.VSplit;
					dragPanel.Show();
					break;
				case DockStyle.Right:
					this.DockPadding.Left += splitterWidth;
					dragPanel.Height = this.Height;
					dragPanel.Width = splitterWidth;
					dragPanel.Location = new Point(0, 0);
					dragPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom;
					dragPanel.Cursor = Cursors.VSplit;
					dragPanel.Show();
					break;
				case DockStyle.Top:
					this.DockPadding.Bottom += splitterWidth;
					dragPanel.Height = splitterWidth;
					dragPanel.Width = this.Width;
					dragPanel.Location = new Point(0, this.Height-splitterWidth);
					dragPanel.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
					dragPanel.Cursor = Cursors.HSplit;
					dragPanel.Show();
					break;
				case DockStyle.Bottom:
					this.DockPadding.Top += splitterWidth;
					dragPanel.Height = splitterWidth;
					dragPanel.Width = this.Width;
					dragPanel.Location = new Point(0, 0);
					dragPanel.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
					dragPanel.Cursor = Cursors.HSplit;
					dragPanel.Show();
					break;
				case DockStyle.Fill:
					dragPanel.Hide();
					break;
				default:
					dragPanel.Hide();
					break;
			}

			Invalidate();
		}

		/// <summary>
		/// Removes a child container from the internal list and disposes it.
		/// The other container will be transferred back to the host container at the call of the OnControlRemoved method.
		/// </summary>
		/// <param name="cont">The DockContainer that is to be removed.</param>
		protected void RemoveContainer(DockContainer cont)
		{
			if (this.Controls.Contains(cont))
			{
				this.Controls.Remove(cont);
				cont.Dispose();
				cont = null;
			}
		}

		public DockContainer GetNextChild(DockContainerType type, DockContainer last)
		{
			DockContainer ret = null;

			foreach (DockContainer cont in containerList)
			{
				ret = cont.GetNextChild(type, last);
				if (ret != null)
					return ret;
			}

			if ((containerList.Count == 0) && (dockType == type) && (this != last))
				ret = last;

			return ret;
		}
		#endregion

		#region Buttons
		/// <summary>
		/// Shows all buttons depending on the type of the container.
		/// </summary>
		private void ShowButtons()
		{
			btnClose.Show();

			if (dockType == DockContainerType.Document)
			{
				btnTabL.Show();
				btnTabR.Show();
			}
		}

		/// <summary>
		/// Hides all buttons.
		/// </summary>
		private void HideButtons()
		{
			btnClose.Hide();
			btnTabL.Hide();
			btnTabR.Hide();
		}	
		
		/// <summary>
		/// The Click event handler for the close button.
		/// Closes the active panel and destoys the container, if needed.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
		private void btnClose_Click(object sender, EventArgs e)
		{
			DockPanel panel = null;

			if ((panelList.Count > 1) && (activePanel != null))
			{
				panel = activePanel;
				panel.Form.Close();
			}
			else
			{
				while (panelList.Count > 0)
				{
					panel = (DockPanel)panelList[0];
					panel.Form.Close();
				}

				panelList.Clear();
			}

			if ((panelList.Count == 0) && (containerList.Count == 0) && (this.Parent is DockContainer) && (removeable))
			{
				(this.Parent as DockContainer).RemoveContainer(this);
			}
		}

		/// <summary>
		/// The Click event handler for the tab left button.
		/// Switches to the next tab to the left.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
		private void btnTabL_Click(object sender, EventArgs e)
		{
			panelOffset--;

			if (panelOffset == 0)
				btnTabL.Enabled = false;

			Invalidate();
		}

		/// <summary>
		/// The Click event handler for the tab right button.
		/// Switches to the next tab to the right.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
		private void btnTabR_Click(object sender, EventArgs e)
		{
			panelOffset++;

			if (panelOffset > 0)
				btnTabL.Enabled = true;

			if (panelOffset == panelList.Count-1)
				btnTabR.Enabled = false;

			Invalidate();
		}
		
		/// <summary>
		/// The PostPaint event handler of the close button.
		/// Used for custom drawing.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An PaintEventArgs that contains the drawing data.</param>
		private void btnClose_PostPaint(object sender, PaintEventArgs e)
		{
			Graphics graphics = e.Graphics;

			Pen pen = new Pen(btnClose.ForeColor);
			int x0 = 0;
			int y0 = 0;

			if (btnClose.Pressed)
			{
				x0 = 1;
				y0 = 1;
			}

			if (dockType == DockContainerType.Document)
			{
				graphics.DrawLine(pen, x0+3, y0+3, x0+9, y0+9);
				graphics.DrawLine(pen, x0+4, y0+3, x0+10, y0+9);
				graphics.DrawLine(pen, x0+3, y0+9, x0+9, y0+3);
				graphics.DrawLine(pen, x0+4, y0+9, x0+10, y0+3);
			}
			else
			{
				graphics.DrawLine(pen, x0+6, y0+3, x0+12, y0+9);
				graphics.DrawLine(pen, x0+12, y0+3, x0+6, y0+9);
			}
		}

		/// <summary>
		/// The PostPaint event handler of the tab left button.
		/// Used for custom drawing.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An PaintEventArgs that contains the drawing data.</param>
		private void btnTabL_PostPaint(object sender, PaintEventArgs e)
		{
			Graphics graphics = e.Graphics;

			Pen pen = new Pen(btnTabL.ForeColor);
			int x0 = 0;
			int y0 = 0;

			if (btnTabL.Pressed)
			{
				x0 = 1;
				y0 = 1;
			}

			GraphicsPath path = new GraphicsPath();
			path.AddLine(x0+4, y0+6, x0+8, y0+2);
			path.AddLine(x0+8, y0+2, x0+8, y0+10);
			path.AddLine(x0+8, y0+10, x0+4, y0+6);

			graphics.DrawPath(pen, path);
			
			if (btnTabL.Enabled)
				graphics.FillPath(new SolidBrush(pen.Color), path);
		}

		/// <summary>
		/// The PostPaint event handler of the tab right button.
		/// Used for custom drawing.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An PaintEventArgs that contains the drawing data.</param>
		private void btnTabR_PostPaint(object sender, PaintEventArgs e)
		{
			Graphics graphics = e.Graphics;

			Pen pen = new Pen(btnTabR.ForeColor);
			int x0 = 0;
			int y0 = 0;

			if (btnTabR.Pressed)
			{
				x0 = 1;
				y0 = 1;
			}

			GraphicsPath path = new GraphicsPath();
			path.AddLine(x0+4, y0+2, x0+4, y0+10);
			path.AddLine(x0+4, y0+10, x0+8, y0+6);
			path.AddLine(x0+8, y0+6, x0+4, y0+2);

			graphics.DrawPath(pen, path);
			
			if (btnTabR.Enabled)
				graphics.FillPath(new SolidBrush(pen.Color), path);
		}
		#endregion

		#region Drag event and docking
		/// <summary>
		/// The DragWindow event handler for dockable windows.
		/// Used to enable a DockWindow to send its position changes to this container.
		/// Calls recursively all DragWindow event handlers of the child containers.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An DockEventArgs that contains the docking data.</param>
		public void DragWindow(object sender, DockEventArgs e)
		{
			try
			{
				// First, check if the given point is in the current tree.
				if (!HitTest(e.Point))
					return;

				// Then, check if the container is dockable at all.
				if (e.DockType == DockContainerType.None)
					return;

				// Then test all containers.
				foreach (DockContainer c in containerList)
				{
					c.DragWindow(sender, e);
					if (e.Target != null)
						return;
				}

				if (containerList.Count != 0)
					return;

				// Then test itself.
				DockWindow wnd = sender as DockWindow;
				e.Target = GetTarget(wnd.Size, e.DockType, e.Point);
				if ((e.Target != null) && (e.Release))
				{
					if (panelList.Contains(wnd.ControlContainer) && (panelList.Count == 1))
					{
						e.Target = this;
						return;
					}

					// Dock the container, if needed.
					if (!containerList.Contains(e.Target) && (e.Target != this))
					{
						// Create new container and fill it with own panels or containers.
						DockContainer cont = new DockContainer();
						cont.removeable = removeable;
						removeable = true;
						cont.DockBorder = dockBorder;
						cont.DockType = dockType;
						
						disableOnControlRemove = true;
						while (containerList.Count > 0)
							cont.Controls.Add(containerList[0] as DockContainer);
						disableOnControlRemove = false;

						containerList.Add(cont);
						
						DockPanel temp = ActivePanel;
						while (panelList.Count > 0)
							cont.Controls.Add(panelList[0] as DockPanel);
						cont.ActivePanel = temp;

						this.Controls.Add(cont);
						cont.Dock = DockStyle.Fill;

						// Add the container to the list object.
						containerList.Add(e.Target);
						this.Controls.Add(e.Target);
					}

					// Add the panel.
					e.Target.AddPanel(wnd, e.Point);

					// Set focus.
					this.TopLevelControl.BringToFront();
					this.TopLevelControl.Invalidate(true);
					e.Target.ActivePanel.Focus();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		/// <summary>
		/// The direct version of the DragWindow event handler to dock a window into the container.
		/// Can be used for every container.
		/// </summary>
		/// <param name="wnd">The DockWindow that is to be docked.</param>
		/// <param name="style">The preferred dock style. Use Fill to dock directly into the container.</param>
		/// <returns>The success of the operation.</returns>
		public bool DockWindow(DockWindow wnd, DockStyle style)
		{
			Rectangle rc = RectangleToScreen(this.ClientRectangle);
			Point pt = new Point(0, 0);

			switch (style)
			{
				case DockStyle.Left:
					pt = new Point(rc.Left+this.DockPadding.Left+dockBorder/2, rc.Top+rc.Height/2);
					break;
				case DockStyle.Right:
					pt = new Point(rc.Left+rc.Width-this.DockPadding.Right-dockBorder/2, rc.Top+rc.Height/2);
					break;
				case DockStyle.Top:
					pt = new Point(rc.Left+rc.Width/2, rc.Top+this.DockPadding.Top+dockBorder/2);
					break;
				case DockStyle.Bottom:
					pt = new Point(rc.Left+rc.Width/2, rc.Top+rc.Height-this.DockPadding.Bottom-dockBorder/2);
					break;
				case DockStyle.Fill:
					pt = new Point(rc.Left+rc.Width/2, rc.Top+this.DockPadding.Top/2);
					break;
				default:
					return false;
			}

			if ((wnd.IsDocked) && (wnd.HostContainer.Dock == style))
				return true;
            
			wnd.Release();
			DragWindow(wnd, new DockEventArgs(pt, wnd.DockType, true));
			return wnd.IsDocked;
		}

		/// <summary>
		/// Releases a DockWindow from the container.
		/// The container may destroy itself at the end of this procedure if it is empty and removeable.
		/// </summary>
		/// <param name="form">The DockWindow that is to be released.</param>
		/// <returns>The success of the operation.</returns>
		internal bool ReleaseWindow(DockWindow form)
		{
			bool ret = false;

			if (this.Controls.Contains(form.ControlContainer))
			{
				form.Controls.Add(form.ControlContainer);
				form.Show();
				ret = true;
			}
			
			if ((panelList.Count == 0) && (containerList.Count == 0) && (this.Parent is DockContainer) && (removeable))
			{
				(this.Parent as DockContainer).RemoveContainer(this);
			}

			return ret;
		}
		#endregion

		#region Misc overrides
		/// <summary>
		/// Extends the key processing features of the base class (Panel).
		/// This method is called recursively beginning from the top-level container.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">A KeyEventArgs that contains the key data.</param>
		public void InvokeKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				if (activePanel != null)
					activePanel.Form.InvokeKeyDown(e);
				else
				{
					foreach (DockContainer c in containerList)
					{
						if (c.ContainsFocus)
						{
							c.InvokeKeyDown(sender, e);
							break;
						}
					}
				}
			}
			catch
			{
			}
		}

		/// <summary>
		/// Extends the key processing features of the base class (Panel).
		/// This method is called recursively beginning from the top-level container.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">A KeyEventArgs that contains the key data.</param>
		public void InvokeKeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				if (activePanel != null)
					activePanel.Form.InvokeKeyUp(e);
				else
				{
					foreach (DockContainer c in containerList)
					{
						if (c.ContainsFocus)
						{
							c.InvokeKeyUp(sender, e);
							break;
						}
					}
				}
			}
			catch
			{
			}
		}

		/// <summary>
		/// Overrides the base class function to pass "true" at any time.
		/// </summary>
		/// <param name="keyData">The key data.</param>
		/// <returns>True at any time.</returns>
		protected override bool IsInputKey(Keys keyData)
		{
			return true;
		}

		/// <summary>
		/// Overrides the base class function to adjust the sizes of the child containers.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected override void OnResize(EventArgs e)
		{
			foreach (DockContainer cont in containerList)
			{
				if (cont.Dock == DockStyle.Fill)
					continue;

				if ((this.Dock == DockStyle.Left) || (this.Dock == DockStyle.Right))
					cont.Height += (this.Height-oldSize.Height)/2;
				else if ((this.Dock == DockStyle.Top) || (this.Dock == DockStyle.Bottom))
					cont.Width += (this.Width-oldSize.Width)/2;
			}

			oldSize = this.Size;

			base.OnResize(e);
		}

		/// <summary>
		/// Overrides the base class function to adjust the borders.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected override void OnParentChanged(EventArgs e)
		{
			AdjustBorders();

			base.OnParentChanged(e);
		}
		#endregion

		#region Mouse
		#region Mouse down
		/// <summary>
		/// Overrides the base class function to handle drag of panels and resizing.
		/// </summary>
		/// <param name="e">A MouseEventArgs that contains the mouse data.</param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			// Destroy drag window.
			if (dragWindow != null)
			{
				dragWindow.Close();
				dragWindow.Dispose();
				dragWindow = null;
			}

			// Test header.
			Focus();

			ptStart = new Point(e.X, e.Y);
			
			// Test footer.
			if (panelList.Count > 1)
			{
				foreach (DockPanel panel in panelList)
				{
					if (panel.TabRect.Contains(e.X, e.Y))
					{
						ActivePanel = panel;
						return;
					}
				}
			}

			base.OnMouseDown(e);
		}
		#endregion

		#region Mouse move
		/// <summary>
		/// Overrides the base class function to handle drag of panels and resizing.
		/// </summary>
		/// <param name="e">A MouseEventArgs that contains the mouse data.</param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			bool inPanelBar = ((panelList.Count > 1) && (e.Y > this.Height-this.DockPadding.Bottom) && (this.DockType != DockContainerType.Document)) || ((e.Y < bottomDock)  && (this.DockType == DockContainerType.Document));

			if (inPanelBar && (this.ClientRectangle.Contains(e.X, e.Y)))
			{
				// Display tool-tip.
				UpdateToolTip(new Point(e.X, e.Y));
			}
			else
			{
				// Check panel movement.
				if ((e.Button == MouseButtons.Left) && (activePanel != null) && !ptStart.Equals(new Point(e.X, e.Y)))
				{	
					Rectangle rc = RectangleToScreen(ClientRectangle);
					DockWindow form = activePanel.Form;
					
					if (dragWindow == null)
					{
						dragWindow = new DockWindow();
						dragWindow.IsDragWindow = true;
						dragWindow.Size = form.Size;
						dragWindow.DockType = this.DockType;
						DockContainer c = TopLevelContainer;
						if (c != null)
							dragWindow.DragWindow += new DockEventHandler(c.DragWindow);
						dragWindow.Show();
					}

					if (dragWindow.dragTarget == null)
						dragWindow.Location = new Point(e.X+rc.X-10, e.Y+rc.Y-10);
					else
						dragWindow.MoveWindow();

					if (dragWindow.dragTarget == null)
						dragWindow.Size = form.Size;
				}
			}
		
			base.OnMouseMove(e);
		}
		#endregion

		#region Mouse up
		/// <summary>
		/// Overrides the base class function to handle drag of panels and resizing.
		/// </summary>
		/// <param name="e">A MouseEventArgs that contains the mouse data.</param>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (dragWindow != null)
			{
				DockWindow form = activePanel.Form;
				
				Rectangle rc = RectangleToScreen(this.ClientRectangle);
				dragWindow.Close();
				dragWindow.Dispose();
				dragWindow = null;
				
				if (form.ConfirmDock() != this)
				{
					form.Location = new Point(e.X+rc.X-10, e.Y+rc.Y-10);
					ReleaseWindow(form);
				}
			}
		
			base.OnMouseUp(e);
		}
		#endregion

		#region Mouse leave
		protected override void OnMouseLeave(EventArgs e)
		{
            //THIS HAS BEEN CHAHGED BY FERDA
			this.ContextMenuStrip = null;
            //END OF CHANGE
			base.OnMouseLeave(e);
		}
		#endregion

        #region Ferda Added middle mouse button handler

        /// <summary>
        /// The mouse click event, that reacts on the middle mouse
        /// button to close the window
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A MouseEventArgs that contains the mouse data.</param>
        void DockContainer_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                CloseClick(sender, e);
            }
        }

        #endregion

        #region Drag panel
        /// <summary>
		/// The MouseDown event handler of the drag panel.
		/// Used to handle resizing of the container.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">A MouseEventArgs that contains the mouse data.</param>
		private void dragPanel_MouseDown(object sender, MouseEventArgs e)
		{
			// Copy last point.
			ptStart = new Point(e.X+dragPanel.Left, e.Y+dragPanel.Top);

			// Create dummy container.
			dragDummy = new DockContainer();
			dragDummy.isDragContainer = true;

			// Set size and location.
			if ((this.Dock == DockStyle.Left) || (this.Dock == DockStyle.Right))
			{
				dragDummy.Location = new Point(this.Location.X+ptStart.X-2, this.Location.Y);
				dragDummy.Size = new Size(4, this.Height);
			}
			else
			{
				dragDummy.Location = new Point(this.Location.X, this.Location.Y+ptStart.Y-2);
				dragDummy.Size = new Size(this.Width, 4);
			}

			// Add container to parent and resizing flag.
			Parent.Controls.Add(dragDummy);
			dragDummy.BringToFront();
			resizing = true;
		}

		/// <summary>
		/// The MouseMove event handler of the drag panel.
		/// Used to handle resizing of the container.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">A MouseEventArgs that contains the mouse data.</param>
		private void dragPanel_MouseMove(object sender, MouseEventArgs e)
		{
			// If resizing, drag dummy to new position.
			if (resizing)
			{
				ptStart = new Point(e.X+dragPanel.Left, e.Y+dragPanel.Top);

				if (this.Dock == DockStyle.Left)
				{
					if (ptStart.X < dockBorder*3)
						ptStart.X = dockBorder*3;
					else if (ptStart.X > this.Parent.Width-dockBorder*3)
						ptStart.X = this.Parent.Width-dockBorder*3;

					dragDummy.Location = new Point(ptStart.X-2, 0);
				}
				else if (this.Dock == DockStyle.Right)
				{
					if (ptStart.X > this.Width-dockBorder*3)
						ptStart.X = this.Width-dockBorder*3;
					else if (ptStart.X < this.Width-this.Parent.Width+dockBorder*3)
						ptStart.X = this.Width-this.Parent.Width+dockBorder*3;

					dragDummy.Location = new Point(this.Parent.Width-this.Width+ptStart.X-2, 0);
				}
				else if (this.Dock == DockStyle.Top)
				{
					if (ptStart.Y < dockBorder*3)
						ptStart.Y = dockBorder*3;
					else if (ptStart.Y > this.Parent.Height-dockBorder*3)
						ptStart.Y = this.Parent.Height-dockBorder*3;

					dragDummy.Location = new Point(0, ptStart.Y-2);
				}
				else if (this.Dock == DockStyle.Bottom)
				{
					if (ptStart.Y > this.Height-dockBorder*3)
						ptStart.Y = this.Height-dockBorder*3;
					else if (ptStart.Y < this.Height-this.Parent.Height+dockBorder*3)
						ptStart.Y = this.Height-this.Parent.Height+dockBorder*3;

					dragDummy.Location = new Point(0, this.Parent.Height-this.Height+ptStart.Y-2);
				}

				dragDummy.BringToFront();
				Invalidate();
			}
		}

		/// <summary>
		/// The MouseUp event handler of the drag panel.
		/// Used to handle resizing of the container.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">A MouseEventArgs that contains the mouse data.</param>
		private void dragPanel_MouseUp(object sender, MouseEventArgs e)
		{
			// If resizing, release dummy and set new size.
			if (resizing)
			{
				resizing = false;
				
				// Release dummy.
				Parent.Controls.Remove(dragDummy);
				Parent.Invalidate();
				dragDummy.Dispose();
				dragDummy = null;

				// Set new size according to the actual state.
				ptStart = new Point(e.X+dragPanel.Left, e.Y+dragPanel.Top);

				if (this.Dock == DockStyle.Left)
				{
					if (ptStart.X < dockBorder*3)
						ptStart.X = dockBorder*3;
					else if (ptStart.X > this.Parent.Width-dockBorder*3)
						ptStart.X = this.Parent.Width-dockBorder*3;
				}
				else if (this.Dock == DockStyle.Right)
				{
					if (ptStart.X > this.Width-dockBorder*3)
						ptStart.X = this.Width-dockBorder*3;
					else if (ptStart.X < this.Width-this.Parent.Width+dockBorder*3)
						ptStart.X = this.Width-this.Parent.Width+dockBorder*3;
				}
				else if (this.Dock == DockStyle.Top)
				{
					if (ptStart.Y < dockBorder*3)
						ptStart.Y = dockBorder*3;
					else if (ptStart.Y > this.Parent.Height-dockBorder*3)
						ptStart.Y = this.Parent.Height-dockBorder*3;
				}
				else if (this.Dock == DockStyle.Bottom)
				{
					if (ptStart.Y > this.Height-dockBorder*3)
						ptStart.Y = this.Height-dockBorder*3;
					else if (ptStart.Y < this.Height-this.Parent.Height+dockBorder*3)
						ptStart.Y = this.Height-this.Parent.Height+dockBorder*3;
				}

				switch (this.Dock)
				{
					case DockStyle.Left:
						if (ptStart.X < 40)
							this.Width = 40;
						else
							this.Width = ptStart.X;
						break;
					case DockStyle.Right:
						int x = 0;

						if (this.Width - ptStart.X < 40)
							x = this.Width - 40;
						else
							x = ptStart.X;
						
						this.Location = new Point(this.Location.X+x, this.Location.Y);
						this.Width -= x;
						break;
					case DockStyle.Top:
						if (ptStart.Y < 60)
							this.Height = 60;
						else
							this.Height = ptStart.Y;
						break;
					case DockStyle.Bottom:
						int y = 0;

						if (this.Height - ptStart.Y < 60)
							y = this.Height - 60;
						else
							y = ptStart.Y;

						this.Location = new Point(this.Location.X, this.Location.Y+y);
						this.Height -= y;
						break;
				}
			}
		}
		#endregion

		#region Tool tip
		/// <summary>
		/// Updates the tool tip and reorders the panels, if needed (mouse movement + LMB).
		/// </summary>
		/// <param name="pt">The pointer position.</param>
		private void UpdateToolTip(Point pt)
		{
			RectangleF rc;

			foreach (DockPanel panel in panelList)
			{
				rc = panel.TabRect;
				if (!rc.Contains(pt))
					continue;
				
				if ((MouseButtons == MouseButtons.Left) && (panel != activePanel))
				{
					if (panelList.IndexOf(panel) > panelList.IndexOf(activePanel))
					{
						if (pt.X-activePanel.TabRect.Right < rc.Width-activePanel.TabRect.Width)
							return;
						panelList.Remove(activePanel);
						panelList.Insert(panelList.IndexOf(panel)+1, activePanel);
					}
					else
					{
						if (activePanel.TabRect.Left-pt.X < rc.Width-activePanel.TabRect.Width)
							return;
						panelList.Remove(activePanel);
						panelList.Insert(panelList.IndexOf(panel), activePanel);
					}
					Invalidate();
					return;
				}
				else
				{
					toolTip.SetToolTip(this, panel.Form.Text);
					toolTip.Active = true;

                    //THIS HAS BEEN CHAHGED BY FERDA
                    if (panel.Form.DockType == DockContainerType.Document)
                    {
                        contextMenu.Items[0].Visible = true;
                    }
                    else
                    {
                        contextMenu.Items[0].Visible = false;
                    }
					this.ContextMenuStrip = contextMenu;

                    //END OF CHANGE
					return;
				}
			}

			toolTip.Active = false;
            //THIS HAS BEEN CHAHGED BY FERDA
			this.ContextMenuStrip = null;
            //ENDOFCHANGE
		}
		#endregion

		#region Context menu
		public void UndockClick(object sender, EventArgs e)
		{
			DockWindow form = activePanel.Form;
				
			form.Location = new Point(MousePosition.X-10, MousePosition.Y-10);
			ReleaseWindow(form);
		}

		public void CloseClick(object sender, EventArgs e)
		{
			btnClose_Click(sender, e);
		}

        //THIS HAS BEEN CHENGED BY FEDRA

        public void RenameClick(object sender, EventArgs e)
        {
            if (activePanel == null)
                return;

            //setting the old name
            activePanel.Form.OldText = activePanel.Form.Text;

            //invoking a rename dialog
            RenameDialog dialog = new RenameDialog(activePanel.Form.Text);

            dialog.ShowDialog();
            if (dialog.DialogResult == DialogResult.OK)
            {
                activePanel.Form.Text = dialog.NewName;
            }
        }

        //END OF CHANGE

		#endregion

		#endregion
	}
}