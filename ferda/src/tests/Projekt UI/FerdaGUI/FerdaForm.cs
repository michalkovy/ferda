using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Crownwood.Magic.Menus;
using Crownwood.Magic.Docking;
using Crownwood.Magic.Common;
using Netron.GraphLib;
using Netron.GraphLib.UI;

namespace proj_pokus2
{
	/// <remarks>
	/// <c>
	///		POSTUP PRI TVORENI MENU Z MAGIC LIBRARY:
	/// </c>
	/// <para>
	///		1. zainteresovane tridy
	/// </para>
	/// <list>
	/// <c>Crownwood.Magic.Menus.MenuControl</c> - globalni trida pro vytvoreni hlavniho menu
	///		ma dost zvlastni eventy na aktivaci a deaktivaci a zere to MenuCommand
	/// </list>
	/// <list>
	/// <c>Crownwood.Magic.Menus.MenuCommand</c> - jednotlive polozky v menu, muzou byt normalni,
	///		nebo jenom submenu
	///		eventu je dost a jsou v dokumentaci ke knihovne hlavne Click a Update
	/// </list>
	/// <para>
	/// <c>Crownwood.Magic.Menus.PopupMenu</c> - hezka realizace popup menu
	///		funguje stejne jako MenuCommand, akurat ze ma zmaknute vecicky
	///		kolem kontextoveho menu
	///	</para>
	///	<para>
	///	2. vytvareni menu
	///	</para>
	///	<para>
	///		hlavni form musi mit jednu instanci tridy MenuControl. Ta zprostredkovava vsechny
	///		menu. Na ni se navesi jednotlive instance MenuCommand - to muze byt popup menu,
	///		nebo menu item. Naveseni se provadi pomoci prikazu MenuCommnad::AddRange(...)
	///	</para>
	///	<para>
	///	3. vytvareni kontextoveho menu
	///		nejdriv se prida handler na this.MouseUp. Zbytek je pochopitelny z procedury
	///		OnMojeFormMouseUp.
	///	</para>	
	///	<para>
	///	4. priklady slozitejsich aplikaci jsou v knihovne
	///	</para>
	///	<c>
	///	DOCKING PROPERTIES Z MAGIC LIBRARY
	///	</c>
	///	<para>
	///	1. zainteresovane tridy
	///	</para>
	///	<list>
	///	<c>Crownwood.Magic.Docking.DockingManager</c> - celkove nejakym zpusobem ridi 
	///		veskere dokovani
	///	</list>
	///	<list>	
	///	<c>Crownwood.Magic.Docking.Content</c> - jednotliva dokovaci okna se naplni na 
	///		zacatku
	///	</list>
	///	<para>
	///	Otazka je ktere fields budou zaroven i properties - ty, 
	///	ktere se budou pouzivat i jinde (jeste nevim jestli vsechny)
	///	</para>
	/// </remarks>
	///<summary>
	/// Trial class for UI work 14.11.2004
	/// <para>
	/// Funcioning menu, context menu, dummy docking - simple 
	/// docking into groups
	/// </para>
	///</summary>
	public class FerdaForm : System.Windows.Forms.Form
	{
		#region Private class fields

		/// <summary>
		/// Required designer variable
		/// </summary>
		private Container components = null;

		/// <summary>
		/// Property grid of objects (usually docked in the right)
		/// </summary>
		private PropertyGrid propertyGrid = null;

		private Netron.GraphLib.UI.GraphControl mainControl;
		private Netron.GraphLib.UI.GraphShapesView testGraphView;

		#endregion

		#region Protected class fields

		/// <summary>
		/// Variable for menu inicialization
		/// </summary>
		protected MenuControl topMenu = null;
		
		/// <summary>
		/// Variable for docking inicialization
		/// </summary>
		protected DockingManager dockingManager = null;

		/// <summary>
		/// one of the toolbars (maybe the only 1)
		/// </summary>
		protected ToolBar toolBar = null;

		/// <summary>
		/// The main status bar for the application
		/// </summary>
		protected StatusBar statusBar = null;

		/// <summary>
		/// MagicLibrary.Content for Property Grid docking
		/// </summary>
		protected Content propertyGridContent = null;

		/// <summary>
		/// MagicLibrary.Content for Context Help docking
		/// </summary>
		protected Content contextHelpContent = null;

		/// <summary>
		/// MagicLibrary.Content for Data Matrix Grid docking
		/// </summary>
		protected Content dataGridContent = null;

		/// <summary>
		/// MagicLibrary.Content for Procedure List window
		/// docking
		/// </summary>
		protected Content procedureListContent = null;

		/// <summary>
		/// MagicLibrary.Content for Attribute List window
		/// docking
		/// </summary>
		protected Content attributeListContent = null;

		/// <summary>
		/// Context help Control for the User
		/// </summary>
		protected UserControl contextHelpControl = null;

		/// <summary>
		/// Control for displaying the ODBC sources (hopefully)
		/// </summary>
		protected DataGrid dataMatrixGrid = null;

		/// <summary>
		/// List of DM procedures
		/// </summary>
		protected ListView procedureList = null;

		/// <summary>
		/// List fo LISp-Miner attributes
		/// </summary>
		protected ListView attributeList = null;

		#endregion

		#region Public properties

		/// <summary>
		/// Property grid of objects (usually docked in the right)
		/// </summary>
		public PropertyGrid PropertyGrid
		{
			set 
			{
				propertyGrid = value;
			}
			get
			{
				return propertyGrid;
			}
		}

		#endregion

		#region Constructors
		/// <summary>
		/// Constructor of MojeForm
		/// </summary>
		public FerdaForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			SetupMainControl();
			SetupToolBar();
			SetupMenus();
			SetupStatusBar();
			SetupDocking();
		}


		#endregion

		#region Methods

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}


		/// <summary>
		/// Inicialization of the main drawing graph control
		/// </summary>
		/// <remarks>
		/// It has to load the graphical component libraries either by
		/// the GraphControl.LoadLibraries() or by GraphControl.AddLibrary(path)
		/// </remarks>
		protected void SetupMainControl()
		{
			//Adding a library
			string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
			path += '\\';
			path += "BasicShapes.dll";

			if ( System.IO.File.Exists(path) )
			{
				mainControl.AddLibrary(path);
			}
			else
			{
				throw new ApplicationException("FerdaForm.SetupMainControl - nenalezena knihovna komponent");
			}

			//Adding property grid events handling
			mainControl.ShowNodeProperties += new ShowPropsDelegate(OnShowProperties);
		}


		/// <summary>
		/// Inicialization of menu
		/// </summary>
		/// <remarks>
		/// Creating icons to menus can be added 
		/// to the Magic Library
		/// </remarks>
		protected void SetupMenus()
		{
			//Create single top level menu item
			MenuCommand top = new MenuCommand("TestTop");
			//Create a View top level menu item
			MenuCommand view = new MenuCommand("View");

			//Associate appropriate status bar descriiption for item
			top.Description = "Description of the submenu";

			//Creating submenu with the same class, MenuCommands
			MenuCommand polozka1 = new MenuCommand("Add node");
			MenuCommand polozka2 = new MenuCommand("Polozka 2");
			MenuCommand polozkaExit = new MenuCommand("Exit");
		
			// The view menu items definitions and descriptions
			MenuCommand propertiesMenuItem = new MenuCommand(
				"Property Grid");
			MenuCommand procListMenuItem = new MenuCommand(
				"Procedure List");
			MenuCommand attrListMenuItem = new MenuCommand(
				"Attribute List");
			MenuCommand contextHelpMenuItem = new MenuCommand(
				"Context help");
			MenuCommand dataMatrixMenuItem = new MenuCommand(
				"Data Matrix");

			//the most important part - adding it to the top MenuCommand
			top.MenuCommands.AddRange(
				new MenuCommand[] {polozka1, polozka2, polozkaExit});
			view.MenuCommands.AddRange(
				new MenuCommand[] {propertiesMenuItem, procListMenuItem,
									  attrListMenuItem, contextHelpMenuItem,
									  dataMatrixMenuItem});


			//adding an event handler - most important thing
			polozka1.Click += new EventHandler(Polozka1Click);
			polozka2.Click += new EventHandler(Polozka2Click);
			polozkaExit.Click += new EventHandler(PolozkaExitClick);

			propertiesMenuItem.Click += new EventHandler(PropertiesView);
			procListMenuItem.Click += new EventHandler(ProcListView);
			attrListMenuItem.Click += new EventHandler(AttrListView);
			contextHelpMenuItem.Click += new EventHandler(ContextHelpView);
			dataMatrixMenuItem.Click += new EventHandler(DataMatrixView);
			
			//Adding the menu to the form by a
			//MenuControl class
			topMenu = new MenuControl();

			topMenu.MenuCommands.Add(top);
			topMenu.MenuCommands.Add(view);
			topMenu.Dock = DockStyle.Top;

			this.Controls.Add(topMenu);
		}


		/// <summary>
		/// Inicialization of docking
		/// </summary>
		/// <remarks>
		/// <para>
		/// Creating icons to docking can be added 
		/// to the Magic Library
		/// </para>
		/// If there is in the future too much flickering
		/// it can be reduced by having a special layer between
		/// the main form and the control with some on paint
		/// flags - more in Magic Library SampleDocking example
		/// </remarks>
		protected void SetupDocking()
		{
			//Inicialization of dockingManager
			dockingManager = new DockingManager(this,
				VisualStyle.IDE);

			// Setting the inner/outer controls
			dockingManager.InnerControl = mainControl;
			dockingManager.OuterControl = toolBar;

			// Setting up the right content - Property Grid
			propertyGrid = new PropertyGrid();
			propertyGridContent = dockingManager.Contents.Add( 
				propertyGrid, "Properties");

			dockingManager.AddContentWithState(propertyGridContent,
				State.DockRight);

			// Setting up the lower content - Context Help
			contextHelpControl = new UserControl();
			contextHelpContent = dockingManager.Contents.Add(
				contextHelpControl, "Context Help");

			dockingManager.AddContentWithState(contextHelpContent,
				State.DockBottom);

			// setting up the left contents
			dataMatrixGrid = new DataGrid();
			dataGridContent = dockingManager.Contents.Add(
				dataMatrixGrid, "Data matrix");
			
			procedureList = new ListView();
			procedureListContent = dockingManager.Contents.Add(
				procedureList, "Procedure List");

			// Adding a testing graphical view
			testGraphView = new Netron.GraphLib.UI.GraphShapesView();
			//Adding a library
			string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
			path += '\\';
			path += "BasicShapes.dll";

			if ( System.IO.File.Exists(path) )
			{
				testGraphView.AddLibrary(path);
			}
			else
			{
				throw new ApplicationException("FerdaForm.SetupMainControl - nenalezena knihovna komponent");
			}


			attributeListContent = dockingManager.Contents.Add(testGraphView,
				"Graphical components");				

			//attributeList = new ListView();
			//attributeListContent = dockingManager.Contents.Add(
			//	attributeList, "Attribute List");

			// creating a window content (group of contents)
			WindowContent wc = dockingManager.AddContentWithState(
				dataGridContent, State.DockLeft) as WindowContent;
			dockingManager.AddContentToWindowContent(procedureListContent,
				wc);
			dockingManager.AddContentToWindowContent(attributeListContent,
				wc);
		}


		/// <summary>
		/// Setting the Status Bar - creates 2 panels on the status Bar
		/// </summary>
		protected void SetupStatusBar()
		{
			statusBar = new StatusBar();

			// Creating 2 StatusBarPanel objects to display in the StatusBar
			StatusBarPanel panel1 = new StatusBarPanel();
			StatusBarPanel panel2 = new StatusBarPanel();
			// Changing a border style of a StatusBarPanels
			panel1.BorderStyle = StatusBarPanelBorderStyle.Sunken;
			panel2.BorderStyle = StatusBarPanelBorderStyle.Sunken;

			// Displaying text on the StatusBar
			panel1.Text = "Frndíme!!!!!!!";
			panel2.Text = System.DateTime.Today.ToLongDateString();

			// Changing size of StatusBarPanels
			panel1.AutoSize = StatusBarPanelAutoSize.Spring;

			// Display panels in the Status Bar control
			statusBar.ShowPanels = true;

			// Adding StatusBarPanel to StatusBar
			statusBar.Panels.Add(panel1);
			statusBar.Panels.Add(panel2);

			// Adding the StatusBar to the form
			this.Controls.Add(statusBar);
		}


		/// <summary>
		/// Setting the ToolBar
		/// </summary>
		protected void SetupToolBar()
		{
			// Create and initialize the ToolBar 
			//and ToolBarButton controls.
			toolBar = new ToolBar();
			ToolBarButton toolBarButton1 = new ToolBarButton();
			ToolBarButton toolBarButton2 = new ToolBarButton();
			ToolBarButton toolBarButton3 = new ToolBarButton();

			// Playing with the appearence			
			toolBar.Divider = true;
			toolBar.ButtonSize = new Size(24,24);
			toolBar.AutoSize = true;
			toolBar.BorderStyle = BorderStyle.None;
			toolBar.Divider = false;
			toolBar.BackColor = SystemColors.Window;
			toolBar.Appearance = ToolBarAppearance.Flat;

			// Add the ToolBarButton controls to the ToolBar.
			toolBar.Buttons.Add(toolBarButton1);
			toolBar.Buttons.Add(toolBarButton2);
			toolBar.Buttons.Add(toolBarButton3);

			toolBar.Dock = DockStyle.Top;
    
			// Add the ToolBar to the Form.
			Controls.Add(toolBar);
		}


		/// <summary>
		/// Menu->View->Properties reaction. Displays a 
		/// dockable properties window
		/// </summary>
		/// <param name="sender">Sender of event</param>
		/// <param name="e">Event Arguments</param>
		protected void PropertiesView(object sender, System.EventArgs e)
		{
			dockingManager.ShowContent(propertyGridContent);
		}


		/// <summary>
		/// Menu ->View->Procedure List reaction. Displays 
		/// a dockable Procedure List window
		/// </summary>
		/// <remarks>
		/// If there are in the future more dockable windows
		/// in the source group, the contents can be put in
		/// a field and use a for cycle instead of two ifs.
		/// </remarks>
		/// <param name="sender">Sender of event</param>
		/// <param name="e">Event Arguments</param>
		protected void ProcListView(object sender, System.EventArgs e)
		{
			// If visible, no extra work
			if ( procedureListContent.Visible)
			{
				return;
			}

			//Docking to DataGridContent
			if ( dataGridContent.Visible )
			{
				dockingManager.AddContentToWindowContent(procedureListContent,
					dataGridContent.ParentWindowContent);
			}
			else
			{
				// Docking to AttributeListContent
				if ( attributeListContent.Visible )
				{
					dockingManager.AddContentToWindowContent(procedureListContent,
						attributeListContent.ParentWindowContent);
				}
				else
				{
					dockingManager.ShowContent(procedureListContent);
				}
			}
		}


		/// <summary>
		/// Menu->View->Attribute List reaction. Displays a
		/// dockable Attribute List window
		/// </summary>
		/// <remarks>
		/// If there are in the future more dockable windows
		/// in the source group, the contents can be put in
		/// a field and use a for cycle instead of two ifs.
		/// </remarks>
		/// <param name="sender">Sender of event</param>
		/// <param name="e">Event Arguments</param>
		protected void AttrListView(object sender, System.EventArgs e)
		{
			if ( attributeListContent.Visible )
			{
				return;
			}

			//Docking to DataGridContent
			if ( dataGridContent.Visible )
			{
				dockingManager.AddContentToWindowContent(attributeListContent,
					dataGridContent.ParentWindowContent);
			}
			else
			{
				//Docking to ProcedureListContent
				if ( procedureListContent.Visible )
				{
					dockingManager.AddContentToWindowContent(attributeListContent,
						procedureListContent.ParentWindowContent);
				}
				else
				{
					dockingManager.ShowContent(attributeListContent);
				}
			}
		}


		/// <summary>
		/// Menu->View->Context Help reaction. Displays a
		/// dockable Context Help window in the down docking 
		/// group
		/// </summary>
		/// <param name="sender">Sender of event</param>
		/// <param name="e">Event Arguments</param>
		protected void ContextHelpView(object sender, System.EventArgs e)
		{
			dockingManager.ShowContent(contextHelpContent);
		}


		/// <summary>
		/// Menu->View->Data Matrix reaction. Displays a
		/// dockable Data Matrix window
		/// </summary>
		/// <remarks>
		/// If there are in the future more dockable windows
		/// in the source group, the contents can be put in
		/// a field and use a for cycle instead of two ifs.
		/// </remarks>
		/// <param name="sender">Sender of event</param>
		/// <param name="e">Event Arguments</param>
		protected void DataMatrixView(object sender, System.EventArgs e)
		{
			if ( dataGridContent.Visible )
			{
				return;
			}

			//Docking to Attribute List
			if ( attributeListContent.Visible )
			{
				dockingManager.AddContentToWindowContent(dataGridContent,
					attributeListContent.ParentWindowContent);
			}
			else
			{
				//Docking to ProcedureList
				if ( procedureListContent.Visible )
				{
					dockingManager.AddContentToWindowContent(dataGridContent,
						procedureListContent.ParentWindowContent);
				}
				else
				{
					dockingManager.ShowContent(dataGridContent);
				}
			}
		}


		/// <summary>
		/// Polozka1 menu reaction
		/// </summary>
		/// <param name="sender">Sender of event</param>
		/// <param name="e">Event Arguments</param>
		protected void Polozka1Click(object sender, System.EventArgs e)
		{
			System.Diagnostics.Debug.WriteLine(mainControl.NodesCount);
			Shape shape1 = mainControl.AddNode("Item 1",new PointF(50,50));
			System.Diagnostics.Debug.WriteLine(mainControl.NodesCount);
			return;
		}


		/// <summary>
		/// Polozka2 menu reaction
		/// </summary>
		/// <param name="sender">Sender of event</param>
		/// <param name="e">Event arguments</param>
		protected void Polozka2Click(object sender, System.EventArgs e)
		{
			MessageBoxButtons buttons = MessageBoxButtons.OK;
			MessageBox.Show(this, "polozka 2","Info",
				buttons, MessageBoxIcon.Information);
			return;
		}


		/// <summary>
		/// Exits the program
		/// </summary>
		/// <remarks>
		/// Here we can add a whole closing procedure (or do it by
		/// adding to event handlers
		/// </remarks>
		/// <param name="sender">Sender of event</param>
		/// <param name="e">Event arguments</param>
		protected void PolozkaExitClick(object sender, System.EventArgs e)
		{
			this.Close();
		}


		/// <summary>
		/// Event for GraphControl property grid coordination
		/// </summary>
		/// <param name="sender">Sender of event</param>
		/// <param name="props">PropertyBag structure (Netron way of
		/// handling property grids
		/// </param>
		private void OnShowProperties(object sender, Netron.GraphLib.PropertyBag props)
		{
			this.PropertyGrid.SelectedObject=props;
		}


		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.mainControl = new Netron.GraphLib.UI.GraphControl();
			this.SuspendLayout();
			// 
			// mainControl
			// 
			this.mainControl.AllowAddConnection = true;
			this.mainControl.AllowAddShape = true;
			this.mainControl.AllowDeleteShape = true;
			this.mainControl.AllowDrop = true;
			this.mainControl.AllowMoveShape = true;
			this.mainControl.AutomataPulse = 10;
			this.mainControl.AutoScroll = true;
			this.mainControl.BackgroundColor = System.Drawing.Color.Transparent;
			this.mainControl.BackgroundImagePath = null;
			this.mainControl.BackgroundType = Netron.GraphLib.CanvasBackgroundTypes.Gradient;
			this.mainControl.DefaultConnectionPath = "Default";
			this.mainControl.DefaultLineEnd = Netron.GraphLib.ConnectionEnds.NoEnds;
			this.mainControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainControl.EnableContextMenu = true;
			this.mainControl.EnableLayout = false;
			this.mainControl.FileName = null;
			this.mainControl.GradientBottom = System.Drawing.Color.White;
			this.mainControl.GradientTop = System.Drawing.Color.SkyBlue;
			this.mainControl.GraphLayoutAlgorithm = Netron.GraphLib.GraphLayoutAlgorithms.SpringEmbedder;
			this.mainControl.GridSize = 20;
			this.mainControl.ImeMode = System.Windows.Forms.ImeMode.On;
			this.mainControl.Location = new System.Drawing.Point(0, 0);
			this.mainControl.Name = "mainControl";
			this.mainControl.RestrictToCanvas = true;
			this.mainControl.ShowGrid = true;
			this.mainControl.Size = new System.Drawing.Size(576, 405);
			this.mainControl.Snap = true;
			this.mainControl.TabIndex = 0;
			this.mainControl.Text = "graphControl1";
			this.mainControl.Zoom = 1;
			// 
			// FerdaForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(576, 405);
			this.Controls.Add(this.mainControl);
			this.Name = "FerdaForm";
			this.Text = "Pokusne UI pro projekt";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new FerdaForm());
		}


		#endregion Methods
	}
}
