#region CVS info
// $Id: DockManager.cs,v 1.1 2005/12/31 10:04:20 kovacm Exp $
// $Log: DockManager.cs,v $
// Revision 1.1  2005/12/31 10:04:20  kovacm
// Prenos ze stare subversion repository - dalsi cast
//
// Revision 1.1  2005/05/25 23:08:23  mr_circuit
// Beta release.
//
// Revision 1.8  2005/05/21 10:26:49  mr_circuit
// DockManager removes closed windows on its own.
//
// Revision 1.7  2005/05/20 09:03:05  mr_circuit
// One more tab select function added
//
// Revision 1.6  2005/03/11 23:43:55  mr_circuit
// DockManager now contains a list for each panel and doc and tool window.
//
// Revision 1.5  2005/03/07 22:36:53  mr_circuit
// Added comments and changed some functions to override.
//
// Revision 1.4  2005/03/01 00:41:14  mr_circuit
// Finished forced docking. Close, hide and show of dockable windows now stable. Introduced "WasDocked" property for user handling of show events.
//
// Revision 1.3  2005/02/24 01:55:18  mr_circuit
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
// Revision 1.1  2005/02/15 22:43:38  mr_circuit
// First check in.
//
#endregion

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;

namespace DockDotNET
{
	/// <summary>
	/// This class is derived from the DockDotNET.DockContainer class to extend its features.
	/// Use this class as top-level container in the destination windows of your application.
	/// </summary>
	public class DockManager : DockContainer
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#region Construct and dispose
		public DockManager(System.ComponentModel.IContainer container)
		{
			///
			/// Required for Windows.Forms Class Composition Designer support
			///
			container.Add(this);
			InitializeComponent();

			Init();
		}

		public DockManager()
		{
			///
			/// Required for Windows.Forms Class Composition Designer support
			///
			InitializeComponent();

			Init();
		}

		/// <summary>
		/// Initializes the manager (paint styles and startup containter type).
		/// </summary>
		void Init()
		{
			// Enable double buffering.
			SetStyle(ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);

			// Set container type to document and clear removeable flag to prevent manager to destroy itself.
			DockType = DockContainerType.Document;
			removeable = false;

			// Create event handler.
			dragWindowHandler = new DockEventHandler(this.DragWindow);
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
			// DockManager
			// 

		}
		#endregion

		#region Variables
		private ArrayList listPanel = new ArrayList();
		private ArrayList listDocument = new ArrayList();
		private ArrayList listTool = new ArrayList();

		private DockEventHandler dragWindowHandler;

		DockPanel activeDoc = null;
		#endregion

		#region Properties
		public ArrayList ListPanel
		{
			get { return listPanel; }
			set { listPanel = value; }
		}
		
		public ArrayList ListDocument
		{
			get { return listDocument; }
			set { listDocument = value; }
		}
		
		public ArrayList ListTool
		{
			get { return listTool; }
			set { listTool = value; }
		}
		#endregion

		#region Control management
		/// <summary>
		/// Overrides the base class function OnControlAdded.
		/// It blocks any attempt of adding a control that is not a DockContainer, DockPanel or FlatButton.
		/// </summary>
		/// <param name="e">A ControlEventArgs that contains the event data.</param>
		protected override void OnControlAdded(ControlEventArgs e)
		{
			if (!(e.Control is DockContainer) && !(e.Control is DockPanel) && !(e.Control is FlatButton))
			{
				if (Parent != null)
					Parent.Controls.Add(e.Control);
				else
					this.Controls.Remove(e.Control);

				Invalidate();
			}
			else
			{
				base.OnControlAdded(e);
			}
		}
		#endregion

		#region Parent form
		/// <summary>
		/// Overrides the function OnParentChanged.
		/// Tries to convert the parent into a form and then signs into some functions of the parent window.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected override void OnParentChanged(EventArgs e)
		{
			if (Parent is Form)
			{
				Form frm = Parent as Form;
				frm.KeyDown += new KeyEventHandler(InvokeKeyDown);
				frm.KeyUp += new KeyEventHandler(InvokeKeyUp);
				frm.Deactivate += new EventHandler(DeactivateParent);
				frm.Activated += new EventHandler(ActivateParent);
			}

			base.OnParentChanged(e);
		}

		/// <summary>
		/// A message handler for the Deactivate event of the parent window.
		/// Needed to refresh the child controls.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
		private void DeactivateParent(object sender, EventArgs e)
		{
			try
			{
				foreach (DockPanel p in listDocument)
				{
					DockContainer c = p.Form.HostContainer;
					if (c == null)
						continue;

					if ((c.ActivePanel == p) && (c.ContainsFocus))
					{
						activeDoc = p;
						p.SetFocus(false);
						break;
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("DockManager.DeactivateParent: "+ex.Message);
			}
			finally
			{
				Invalidate(true);
			}
		}
		
		/// <summary>
		/// A message handler for the Activate event of the parent window.
		/// Needed to refresh the child controls.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
		private void ActivateParent(object sender, EventArgs e)
		{
			try
			{
				if (activeDoc != null)
					activeDoc.SetFocus(true);
			}
			catch (Exception ex)
			{
				Console.WriteLine("DockManager.ActivateParent: "+ex.Message);
			}
			finally
			{
				Invalidate(true);
			}
		}
		#endregion

		#region Form handling
		public void AddForm(DockWindow wnd)
		{
			try
			{
				// Event handler.
				wnd.DragWindow += dragWindowHandler;
				wnd.Closed += new EventHandler(FormClosed);

				// Update lists.
				listPanel.Add(wnd.ControlContainer);
				if (wnd.DockType == DockContainerType.Document)
					listDocument.Add(wnd.ControlContainer);
				else if (wnd.DockType == DockContainerType.ToolWindow)
					listTool.Add(wnd.ControlContainer);
			}
			catch (Exception ex)
			{
				Console.WriteLine("DockManager.AddForm: "+ex.Message);
			}
		}

		public void RemoveForm(DockWindow wnd)
		{
			if (!listPanel.Contains(wnd.ControlContainer))
				return;

			try
			{
				// Event handler.
				wnd.DragWindow -= dragWindowHandler;

				// Update lists.
				listPanel.Remove(wnd.ControlContainer);
				if (wnd.DockType == DockContainerType.Document)
					listDocument.Remove(wnd.ControlContainer);
				else if (wnd.DockType == DockContainerType.ToolWindow)
					listTool.Remove(wnd.ControlContainer);
			}
			catch (Exception ex)
			{
				Console.WriteLine("DockManager.RemoveForm: "+ex.Message);
			}
		}

		private void FormClosed(object sender, EventArgs e)
		{
			try
			{
				RemoveForm(sender as DockWindow);
			}
			catch (Exception ex)
			{
				Console.WriteLine("DockManager.FormClosed: "+ex.Message);
			}
		}
		#endregion
	}
}
