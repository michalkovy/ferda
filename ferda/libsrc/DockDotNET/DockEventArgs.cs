#region CVS info
// $Id: DockEventArgs.cs,v 1.1 2005/12/31 10:04:20 kovacm Exp $
// $Log: DockEventArgs.cs,v $
// Revision 1.1  2005/12/31 10:04:20  kovacm
// Prenos ze stare subversion repository - dalsi cast
//
// Revision 1.1  2005/05/25 23:08:23  mr_circuit
// Beta release.
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
// Revision 1.1  2005/02/15 22:43:37  mr_circuit
// First check in.
//
#endregion

using System;
using System.Drawing;

namespace DockDotNET
{
	/// <summary>
	/// DockEventArgs is used with DockEventHandler to report docking requests of a moving DockWindow to DockManagers.
	/// </summary>
	public class DockEventArgs : EventArgs
	{
		#region Variables
		private DockContainer target = null;
		private DockContainerType dockType;
		
		private Point point;
		
		private bool release;
		#endregion

		#region Properties
		/// <summary>
		/// Gets the point where to dock the window.
		/// </summary>
		public Point Point
		{
			get { return point; }
		}

		/// <summary>
		/// Gets the needed target container type.
		/// </summary>
		public DockContainerType DockType
		{
			get { return dockType; }
		}

		/// <summary>
		/// Gets the state of the dock process. True, if dock is finally performed.
		/// </summary>
		public bool Release
		{
			get { return release; }
		}

		/// <summary>
		/// Gets or sets the target retrieved by one of the containers.
		/// </summary>
		public DockContainer Target
		{
			get { return target; }
			set { target = value; }
		}
		#endregion

		#region Construct
		/// <summary>
		/// Creates a new DockEventArgs object.
		/// </summary>
		/// <param name="point">The point where to dock the window.</param>
		/// <param name="dockType">The needed target container type.</param>
		/// <param name="release">The state of the dock process. True, if dock is finally performed.</param>
		public DockEventArgs(Point point, DockContainerType dockType, bool release)
		{
			this.point = point;
			this.dockType = dockType;
			this.release = release;
			this.target = null;
		}
		#endregion
	}

	/// <summary>
	/// The event handler for dock events.
	/// </summary>
	public delegate void DockEventHandler(object sender, DockEventArgs e);
}
