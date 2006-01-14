using System;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Runtime.CompilerServices;

using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Resources;
using Netron.GraphLib;
using Netron.GraphLib.Configuration;
using Netron.GraphLib.Attributes;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Contexts;
using System.Windows.Forms.Design;
using Netron.GraphLib.Interfaces;
using System.Runtime.InteropServices;
using Netron.GraphLib.Utils;
using Netron.GraphLib.IO;
using System.Xml;
namespace Netron.GraphLib.UI
{
	/// <summary>
	/// The graph control is the container of shape objects and is an owner-drawn control
	/// </summary>
	/// <remarks>
	/// <br>the control is listening for hits, on hitting an object the HitEntity returns the entity hit and passes it to the Hover object in the HitHover handler.</br>
	/// <br>the biggest part of the code is taken by the handlers for the mouse events</br>
	/// </remarks>
	/// 
	
	[Description("This UI controls allows to display shapes and diagrams.")]
	[Designer(typeof(Netron.GraphLib.UI.GraphControlDesigner))]
	[ToolboxItem(true)]
	public class GraphControl : ScrollableControl, IGraphSite, IGraphLayout
	{

		#region Constants
		private const int WM_VSCROLL = 0x0115;
		private const int WM_HSCROLL = 0x0114;

		#endregion

		#region Delegates and events
		/// <summary>
		/// raised when a properties request is issued
		/// </summary>
		[Category("Graph"), Description("Occurs on double-clicking the entity or via the context menu."), Browsable(true)]
		public event ShowPropsDelegate ShowNodeProperties;
		/// <summary>
		/// The OnInfo allows to output general purpose info from the canvas to the form. 
		/// Added in the context of the automata applications where scripting of nodes is available and where it is useful
		/// to give the user feedback about the pulsating automata.
		/// </summary>
		[Category("Graph"), Description("General purpose info output from the control."), Browsable(true)]
		public event InfoDelegate OnInfo;

		/// <summary>
		/// raised when a new connection is added
		/// </summary>
		[Category("Graph"), Description("Occurs when a new connection is added."), Browsable(true)]
		public event NewConnection OnNewConnection;

		/// <summary>
		/// occurse when a new shape is added to the control
		/// </summary>
		[Category("Graph"), Description("Occurs when a new shape is added."), Browsable(true)]
		public event NewShape OnShapeAdded;
		
		
		[Category("Graph"), Description("Occurs when a shape is removed."), Browsable(true)]
		public event NewShape OnShapeRemoved;


		[Category("Graph"), Description("Occurs when the canvas is cleared."), Browsable(true)]
		public event EventHandler OnClear;

		[Category("Graph"), Description("Occurs when the context menu is called."), Browsable(true)]
		public event MouseEventHandler OnContextMenu;

		[Category("Graph"), Description("Occurs when the automata calculation is started."), Browsable(true)]
		public event EventHandler OnStartAutomata;

		[Category("Graph"), Description("Occurs when the automata calculation is stopped."), Browsable(true)]
		public event EventHandler OnStopAutomata;

		[Category("Graph"), Description("Occurs when the automata transmits data bewteen nodes and updates the cell state."), Browsable(true)]
		public event EventHandler OnDataTransmission;

		[Category("Graph"), Description("Occurs when the control has opened a file."), Browsable(true)]
		public event InfoDelegate OnOpen;

        [Category("Graph"), Description("Handler specifically for FerdaDesktop to notify that a MouseUp event was handled by the parent control."), BrowsableAttribute(true)]
        public event MouseEventHandler OnFerdaMouseUp;

        [Category("Graph"), Description("Handler specifically for FerdaDesktop to notify that a Connection is gonna be deleted")]
        public event FerdaConnection OnFerdaDeleteConnection;

		#endregion

		#region Fields

#if NAFEnabled
		protected NAF.Core.Plugin.NAFRootMediator root;
#endif


		/// <summary>
		/// allow delete boolean
		/// </summary>
		protected bool mAllowDeleteShape = true;

		/// <summary>
		/// allow move shape boolean
		/// </summary>
		protected bool mAllowMoveShape = true;

		/// <summary>
		/// allow add shape boolean
		/// </summary>
		protected bool mAllowAddShape = true;

		/// <summary>
		/// allow add connection boolean
		/// </summary>
		protected bool mAllowAddConnection = true;

		/// <summary>
		/// free, non graph dependen arrow, useful for debugging purposes
		/// </summary>
		protected FreeArrowCollection freeArrows = new FreeArrowCollection();



		protected PointF insertionPoint = PointF.Empty;

		/// <summary>
		/// custom format (see bug problem in the copy/paste code
		/// </summary>
		static DataFormats.Format format = DataFormats.GetFormat("swa");
		

		/// <summary>
		/// the property bag of the canvas
		/// </summary>
		[NonSerialized] protected PropertyBag bag;

		/// <summary>
		/// the mContextMenu of the canvas
		/// </summary>
		protected ContextMenu mContextMenu = new ContextMenu();

		/// <summary>
		/// shortcut
		/// </summary>
		protected bool CtrlShift = false;

		protected Size workingSize = new Size(200, 400);
		/// <summary>
		/// the default path style of the new connections
		/// </summary>
		protected string connectionPath = "Default";

		/// <summary>
		/// the default connection end
		/// </summary>
		protected ConnectionEnds connectionEnd = ConnectionEnds.NoEnds;
		/// <summary>
		/// the grid size
		/// </summary>
		protected int mGridSize = 20;
		/// <summary>
		/// show grid?
		/// </summary>
		protected bool mShowGrid = false;
		/// <summary>
		/// whether to mSnap to the grid
		/// </summary>
		protected bool mSnap = false;
		/// <summary>
		/// keeps the last shape key for easy addition via ALT+click
		/// </summary>
		protected string lastAddedShapeKey = null;
		/// <summary>
		/// whether to enable the context menu
		/// </summary>
		protected bool mEnableContextMenu = false;
		/// <summary>
		/// whether to restrict the shapes to the canvas size
		/// </summary>
		protected bool mRestrictToCanvas = true;
		/// <summary>
		/// the shape mLibraries
		/// </summary>
		protected GraphObjectsLibraryCollection mLibraries = null;
		/// This constant is used when checking for the CTRL key during drop operations.
		/// This makes the code more readable for future maintenance.
		/// </summary>
		protected const int ctrlKey = 8;
		/// <summary>
		/// a reference to the layout factory which organizes the layout algo's
		/// </summary>
		protected LayoutFactory layoutFactory = null;
		/// <summary>
		/// is the layout active?
		/// </summary>
		protected bool mEnableLayout = false;
		/// <summary>
		/// The thread pointer which will run on the layout thread.
		/// </summary>
		protected ThreadStart ts=null;
		/// <summary>
		/// The thread for laying out the graph.
		/// </summary>
		protected Thread thLayout = null;
		/// <summary>
		/// The timer interval in milliseconds that updates the state of the automata in the plex.
		/// </summary>
		protected int mAutomataPulse = 10; 
		/// <summary>
		/// the kinda background the canvas has
		/// </summary>
		protected CanvasBackgroundTypes mBackgroundType = CanvasBackgroundTypes.FlatColor;
		/// <summary>
		/// the color of the lower gradient part
		/// </summary>
		protected Color mGradientBottom = Color.White;
		/// <summary>
		/// the color of the upper gradient part
		/// </summary>
		protected Color mGradientTop = Color.LightSteelBlue;
		/// <summary>
		/// the image path for the background
		/// </summary>
		protected string mBackgroundImagePath=null;
		/// <summary>
		/// the background color
		/// </summary>
		protected Color mBackgroundColor = Color.WhiteSmoke;		
		/// <summary>
		/// the currentZoomFactor factor
		/// </summary>
		protected  float currentZoomFactor=1F;		
		/// <summary>
		/// The current filename of the plex
		/// </summary>
		protected string mFileName = null;      
		/// <summary>
		/// the extract contains the data of the plex and its structure
		/// </summary>
		internal  GraphAbstract extract = null;
		/// <summary>
		/// volatile object not connected to extract structure, used for the current added or selected shape
		/// </summary>
		protected Shape shapeObject = null;               
		/// <summary>
		/// volatile connection, used to manipulate the current connection
		/// </summary>
		protected Connection connection = null;         
		/// <summary>
		/// Entity with current mouse focus. Is automatically set by the HitHover and HitEntity handlers
		/// </summary>
		protected Entity Hover = null;        
		/// <summary>
		/// for tracking selection state, represents the dashed selection rectangle.
		/// </summary>
		protected Selector selector = null;
		/// <summary>
		/// indicates track mode, i.e. moving a shape around
		/// </summary>
		protected Boolean mDoTrack = false;      
		/// <summary>
		/// the timer controlling the refresh rate of the graph
		/// </summary>
		protected System.Windows.Forms.Timer transmissionTimer = new System.Windows.Forms.Timer();

		
	
		
		#endregion

		#region Properties

		#region Browsable properties

		/// <summary>
		/// Gets or sets whether shapes can be moved in the graph
		/// </summary>
		[Category("Graph"), Browsable(true), Description("Gets or sets whether shapes can be moved in the graph.")]
		public bool AllowMoveShape
		{
			get{return this.mAllowMoveShape;}
			set{this.mAllowMoveShape = value;}
		}

		/// <summary>
		/// Gets or sets whether shapes can be deleted from the graph
		/// </summary>
		[Category("Graph"), Browsable(true), Description("Gets or sets whether shapes can be deleted from the graph.")]
		public bool AllowDeleteShape
		{
			get{return this.mAllowDeleteShape;}
			set{this.mAllowDeleteShape = value;}
		}

		/// <summary>
		/// Gets or sets whether shapes can be added to the graph
		/// </summary>
		[Category("Graph"), Browsable(true), Description("Gets or sets whether shapes can be added to the graph.")]
		public bool AllowAddShape
		{
			get{return this.mAllowAddShape;}
			set{this.mAllowAddShape = value;}
		}

		/// <summary>
		/// Gets or sets whether connections can be added to the graph
		/// </summary>
		[Category("Graph"), Browsable(true), Description("Gets or sets whether connections can be added to the graph.")]
		public bool AllowAddConnection
		{
			get{return this.mAllowAddConnection;}
			set{this.mAllowAddConnection = value;}
		}


		/// <summary>
		/// Gets or sets the default path style or shape of the newly created connections
		/// </summary>
		[Category("Graph"), Browsable(true), Description("Gets or sets the default line end of the newly created connections.")]
		public ConnectionEnds DefaultLineEnd
		{
			get{return this.connectionEnd;}
			set{this.connectionEnd = value;}
		}
		
		/// <summary>
		/// Gets or sets the default path style or shape of the newly created connections
		/// </summary>
		[Category("Graph"), Browsable(true), Description("Gets or sets the default path style or shape of the newly created connections.")]
		public string DefaultConnectionPath
		{
			get{return this.connectionPath;}
			set{this.connectionPath = value;this.Invalidate();}
		}


		/// <summary>
		/// Gets or sets whether the grid is visible.
		/// </summary>
		[Category("Graph"), Browsable(true), Description("Gets or sets whether the grid is visible.")]
		public bool ShowGrid
		{
			get{return mShowGrid;}
			set{mShowGrid = value;this.Invalidate();}
		}

		/// <summary>
		/// Gets or sets the grid size.
		/// </summary>
		[Category("Graph"), Browsable(true), Description("Gets or sets the grid size.")]
		public int GridSize
		{
			get{return mGridSize;}
			set{mGridSize = value;this.Invalidate();}
		}
		/// <summary>
		/// Gets or sets whether the graph elements mSnap to the grid
		/// </summary>
		[Category("Graph"), Browsable(true), Description("Gets or sets whether the graph elements mSnap to the grid.")]
		public bool Snap
		{
			get{return mSnap;}
			set{mSnap = value;}
		}

		/// <summary>
		/// Gets or sets whether the context menu is visible
		/// </summary>
		[Category("Appearance"), Browsable(true), Description("Gets or sets whether the context menu is visible.")]
		public bool EnableContextMenu
		{
			get{return mEnableContextMenu;}
			set{
				mEnableContextMenu = value;
				if(value)
					ContextMenu=mContextMenu;
				else
					ContextMenu = null;
			}
		}
		/// <summary>
		/// Gets or sets the background color of the canvas.
		/// </summary>
		[Category("Graph"), Browsable(true), Description("Gets or sets the background color of the canvas.")]
		public Color BackgroundColor 
		{
			get{return mBackgroundColor;}
			set
			{
				mBackgroundColor = value;
				Invalidate();
			}
		}
		

		/// <summary>
		/// Gets or sets the upper color of the gradient.
		/// </summary>
		[Category("Graph"), Browsable(true), Description("Gets or sets the upper color of the gradient.")]
		public Color GradientTop		
		{
			get{return mGradientTop;}
			set
			{
				mGradientTop = value;
				this.Invalidate();
			}
		}
		/// <summary>
		/// Gets or sets the lower color of the gradient.
		/// </summary>
		[Category("Graph"), Browsable(true), Description("Gets or sets the lower color of the gradient.")]
		public Color GradientBottom
		{
			get{return mGradientBottom;}
			set
			{
				mGradientBottom = value;
				this.Invalidate();
			}
		}
		/// <summary>
		/// Gets or sets time interval of the automata update pulse.
		/// </summary>
		[Category("Automata"), Browsable(true), Description("Gets or sets time interval of the automata update pulse.")]
		public int AutomataPulse
		{
			get{return mAutomataPulse;}
			set{mAutomataPulse = value;
			this.transmissionTimer.Interval = value;
			}

		}
		/// <summary>
		/// Gets or sets wether the graph shapes should be kept inside the canvas frame or allowed to move/resize outside it.
		/// </summary>
		[Category("Graph"), Browsable(true), Description("Gets or sets wether the graph shapes should be kept inside the canvas frame or allowed to move/resize outside it.")]
		public  bool RestrictToCanvas
		{
			get
			{
				return mRestrictToCanvas;
			}
			set
			{
				mRestrictToCanvas = value;
			}
		}
		/// <summary>
		/// Gets or sets the kind of background the canvas has.
		/// </summary>
		[Category("Graph"), Browsable(true), Description("Gets or sets the kind of background the canvas has.")]
		public CanvasBackgroundTypes BackgroundType
		{
			get{return this.mBackgroundType;}
			set
			{
				this.mBackgroundType = value;
				this.Invalidate();
			}
		}
		/// <summary>
		/// Gets or sets whether layout algorithms can be applied
		/// </summary>
		[Category("Graph"), Browsable(true), Description("Gets or sets whether layout algorithms are active.")]
		public bool EnableLayout
		{
			get{return mEnableLayout;}
			set{mEnableLayout = value;
				if(mEnableLayout)				
				{
					StartLayout();
				}
				else
				{
					StopLayout();
				}
			}
		}

		/// <summary>
		/// Gets or sets the graph layout algorithm to be used.
		/// </summary>
		[ReadOnly(false), Browsable(true), Description("The layout algorithm to be used."), Category("Graph")]
		public GraphLayoutAlgorithms GraphLayoutAlgorithm
		{
			get{return layoutFactory.GraphLayoutAlgorithm;}
			set{layoutFactory.GraphLayoutAlgorithm=value; }
		}

		/// <summary>
		/// Gets or sets the background image path
		/// </summary>
		[Category("Graph"), Browsable(true), Description("Gets or sets the path to the background image (only visible or used if the background type is 'Image').")]
		public string BackgroundImagePath
		{
			get{ return mBackgroundImagePath;}
			set{mBackgroundImagePath = value;}
		}
		#endregion

		#region Non-browsable properties
#if NAFEnabled
		public Netron.NAF.Core.Plugin.NAFRootMediator Root
		{
			get{return root;}
			set{root = value;}
		}
#endif

		/// <summary>
		/// Gets or sets whether tracking is on
		/// </summary>
		public bool DoTrack
		{
			get{return mDoTrack;}
			set{mDoTrack = value;}
		}

		public GraphObjectsLibraryCollection Libraries
		{
			get{
				return this.mLibraries;
			}
		}


		public ShapeCollection SelectedShapes
		{
			get
			{
				ShapeCollection shapes = new ShapeCollection();
				foreach(Shape shape in this.Abstract.Shapes)
					if(shape.IsSelected)
						shapes.Add(shape);
				return shapes;
			}
		}

		public GraphAbstract Abstract
		{
			get{return extract;}
		}

		public virtual PropertyBag Properties
		{
			get{return bag;}

		}

		public Graphics Graphics
		{
			get{return this.CreateGraphics();}
		}

		/// <summary>
		/// Gets the center position of the control
		/// </summary>
		[Browsable(false)]
		protected PointF Center
		{
			get{return new PointF(this.Width/2,this.Height/2);}
		}

		[Browsable(false)]
		public override Image BackgroundImage
		{
			get
			{
				return null;
			}
			set
			{
				return ;
			}
		}


		[Browsable(false)]
		public Color NodeColor
		{
			set
			{
				foreach(Shape so in extract.Shapes)
					if(so.IsSelected) so.NodeColor=value;
			}
		}
		[Browsable(false)]
		public float LabelFontSize
		{
			set
			{
				foreach(Shape so in extract.Shapes)
					if(so.IsSelected) so.FontSize=value;
			}
		}
		[Browsable(false)]
		public Color TextColor
		{
			set
			
			{
				foreach(Shape so in extract.Shapes)
					if(so.IsSelected) so.TextColor=value;
			}
		}
		[Browsable(false)]
		public bool ShowLabel
		{
			set
			{
				foreach(Shape so in extract.Shapes)
					if(so.IsSelected) so.mShowLabel=value;
			}
		}
		[Browsable(false)]
		public DashStyle LineStyle
		{
			set
			{

				foreach(Shape so in extract.Shapes)
					foreach(Connector co in so.Connectors)
						foreach (Connection con in co.Connections)
							if(con.IsSelected) con.LineStyle=value;
			}
		}

		[Browsable(false)]
		public ConnectionWeight LineWeight
		{
			set
			{
				foreach(Shape so in extract.Shapes)
					foreach(Connector co in so.Connectors)
						foreach (Connection con in co.Connections)
							if(con.IsSelected) con.LineWeight=value;
			}
		}
		[Browsable(false)]
		public ConnectionEnds LineEnd
		{
			set
			{
				foreach(Shape so in extract.Shapes)
					foreach(Connector co in so.Connectors)
						foreach (Connection con in co.Connections)
							if(con.IsSelected) con.LineEnd=value;
			}
		}
		[Browsable(false)]
		public Color LineColor
		{
			set
			{
				foreach(Shape so in extract.Shapes)
					foreach(Connector co in so.Connectors)
						foreach (Connection con in co.Connections)
							if(con.IsSelected) con.LineColor=value;
			}
		}

		/// <summary>
		/// Gets or sets the currentZoomFactor factor
		/// </summary>
		[Browsable(false)]
		public  float Zoom
		{
			get
			{
				return currentZoomFactor;
			}
			set
			{
				if(value==currentZoomFactor) return;
				/*if(value>10 && value<500)
				{
					float amount;
					amount=((float) value/currentZoomFactor);
					if(this.extract != null)
					{					
						foreach (Shape so in this.extract.Shapes)
							so.Zoom(amount);
					}
					currentZoomFactor=value;
				}*/
					currentZoomFactor=value;
			
				//this.AutoScrollMargin = new Size
				//this.AutoScrollPosition = Point.Round(new PointF(Width*(currentZoomFactor-1)*2,Height*(currentZoomFactor-1)*2));
				
				this.Invalidate();
			}
		}
		/// <summary>
		/// Gets or sets the backcolor (redirected to the background color)
		/// </summary>
		[Browsable(false)]
		public override Color BackColor
		{
			get
			{
				return mBackgroundColor;
			}
			set
			{
				mBackgroundColor = value;
			}
		}
		/// <summary>
		/// Gets or sets the file name of the current graph
		/// </summary>
		[Browsable(false)]
		public string FileName
		{
			get{return mFileName;}
			set{mFileName = value;}
		}
		/// <summary>
		/// Gets the number of edges or connections in the graph.
		/// </summary>
		[ReadOnly(true),Browsable(false)]
		public int EdgeCount
		{
			get
			{
				return Edges.Count;
			}
		}
		/// <summary>
		/// Gets the number of nodes on the canvas
		/// </summary>
		[ReadOnly(true), Browsable(false)]
		public int NodesCount
		{
			get
			{
				return extract.Shapes.Count;				
			}
		}
		/// <summary>
		/// Gets the arraylist of nodes in the graph.
		/// </summary>
		/// 
		[ReadOnly(true), Browsable(false)]
		public ShapeCollection Nodes
		{
			get
			{
				return extract.Shapes;
			}
		}
		/// <summary>
		/// Gets the arraylist of edges in the graph. A more elegant way to implement this would be to keep the collection up to date
		/// while adding or deleting connections but it would mean a doubling since the connections are already in seperate arraylists all the time, hence this method.
		/// </summary>
		/// 
		[ReadOnly(true), Browsable(false)]
		public ArrayList Edges
		{
			get
			{
				ArrayList cons=new ArrayList();
				foreach(Shape so in extract.Shapes)
					foreach(Connector co in so.Connectors)
						foreach(Connection con in co.Connections)
							if (!cons.Contains(con)) cons.Add(con);

				return cons;
			}
		}
		
		#endregion
		#endregion
		
		#region Constructor & finalize
		

		/// <summary>
		/// Class constructor
		/// </summary>
		/// <remarks>
		/// Notice the double buffering directives at the end
		/// </remarks>
		public GraphControl()
		{		
			//Make sure the control repaints as it is resized
			SetStyle(ControlStyles.ResizeRedraw, true);
			//allow dragdrop
			this.AllowDrop=true;
			//instantiate the layout factory
			layoutFactory=new LayoutFactory(this);			

			//instanciating an attached extract to the canvas
			extract = new GraphAbstract();
			extract.Site=this;

			//init the transmission timer
			transmissionTimer.Interval = AutomataPulse; 
			transmissionTimer.Tick += new EventHandler(OnTransmissionTimer); //attach of the handler
			
			//enable double buffering of graphics
			SetStyle(ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			
			//instantiate the library collection
			mLibraries = new GraphObjectsLibraryCollection();

			
			this.KeyDown+=new KeyEventHandler(OnKeyDown);
			this.KeyPress+=new KeyPressEventHandler(OnKeyPress);
			
			AddBaseMenu();
			bag = new PropertyBag();
			AddProperties();


			this.AutoScroll=true;			
			this.HScroll=true;
			this.VScroll=true;

	
		

			try
			{
				Assembly ass= Assembly.GetExecutingAssembly();
				MouseCursors.Add = new Cursor(  ass.GetManifestResourceStream("Netron.GraphLib.Resources.Add.cur"));
				MouseCursors.Cross = new Cursor(    ass.GetManifestResourceStream("Netron.GraphLib.Resources.Cross.cur"));
				MouseCursors.Grip =  new Cursor(   ass.GetManifestResourceStream("Netron.GraphLib.Resources.Grip.cur"));
				MouseCursors.Move =  new Cursor(  ass.GetManifestResourceStream("Netron.GraphLib.Resources.Move.cur"));
				MouseCursors.Select =  new Cursor(  ass.GetManifestResourceStream("Netron.GraphLib.Resources.Select.cur"));
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.Message);
			}
			
		}


		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				//TODO: disposing things, what?
			}
			base.Dispose( disposing );
		}
		#endregion

		#region Methods
	
	
		#region Control logic

		#region Zoomers

		
		/// <summary>
		/// Zooms given point.
		/// </summary>
		/// <param name="originalPt">Point to currentZoomFactor</param>
		/// <returns>zoomed point.</returns>
		public Point ZoomPoint(Point originalPt)
		{
			Point newPt = new Point((int)(this.currentZoomFactor*originalPt.X), (int)( this.currentZoomFactor*originalPt.Y ));
			return newPt; 
		}
		/// <summary>
		/// Unzooms given point.
		/// </summary>
		/// <param name="originalPt">Point to unzoom</param>
		/// <returns>Unzoomed point.</returns>
		public Point UnzoomPoint(Point originalPt)
		{
			Point newPt = new Point((int)(originalPt.X/this.currentZoomFactor), (int)(originalPt.Y /this.currentZoomFactor));
			return newPt;
		}

		/// <summary>
		/// Zooms a given point.
		/// </summary>
		/// <param name="originalPt">Point to currentZoomFactor</param>
		/// <returns>Zoomed point.</returns>
		public PointF ZoomPoint(PointF originalPt)
		{
			PointF newPt = new PointF( this.currentZoomFactor*originalPt.X ,  this.currentZoomFactor*originalPt.Y );
			return newPt; 
		}

		/// <summary>
		/// Unzooms a given point.
		/// </summary>
		/// <param name="originalPt">Point to unzoom.</param>
		/// <returns>Unzoomed point.</returns>
		public PointF UnzoomPoint(PointF originalPt)
		{
			PointF newPt = new PointF(originalPt.X/this.currentZoomFactor, originalPt.Y/this.currentZoomFactor);
			return newPt;
		}
		/// <summary>
		/// Zooms a given rectangle.
		/// </summary>
		/// <param name="originalRect">Rectangle to currentZoomFactor.</param>
		/// <returns>Zoomed rectangle.</returns>
		public RectangleF ZoomRectangle(RectangleF originalRect)
		{
			RectangleF newRect = new RectangleF( this.currentZoomFactor*originalRect.X ,  this.currentZoomFactor*originalRect.Y , 
				originalRect.Width * this.currentZoomFactor, originalRect.Height * this.currentZoomFactor);
			return newRect; 
		}
		/// <summary>
		/// Unzooms given rectangle.
		/// </summary>
		/// <param name="originalRect">Rectangle to unzoom.</param>
		/// <returns>Unzoomed rectangle.</returns>
		public RectangleF UnzoomRectangle(RectangleF originalRect)
		{
			RectangleF newRect = new RectangleF(originalRect.X /this.currentZoomFactor, originalRect.Y /this.currentZoomFactor, 
				originalRect.Width / this.currentZoomFactor, originalRect.Height / this.currentZoomFactor);
			return newRect;
		}


		/// <summary>
		/// Zooms given rectangle.
		/// </summary>
		/// <param name="originalRect">Rectangle to currentZoomFactor</param>
		/// <returns>Zoomed rectangle</returns>
		public Rectangle ZoomRectangle(Rectangle originalRect)
		{
			Rectangle newRect = new Rectangle((int)(this.currentZoomFactor*originalRect.X ), (int)( this.currentZoomFactor*originalRect.Y ), 
				(int)(originalRect.Width * this.currentZoomFactor), (int)(originalRect.Height * this.currentZoomFactor));
			return newRect; 
		}
		/// <summary>
		/// Unzooms given rectangle. 
		/// </summary>
		/// <param name="originalRect">Rectangle to unzoom</param>
		/// <returns>Unzoomed rectangle.</returns>
		public Rectangle UnzoomRectangle(Rectangle originalRect)
		{
			Rectangle newRect = new Rectangle((int)(originalRect.X /this.currentZoomFactor), (int)(originalRect.Y /this.currentZoomFactor), 
				(int)(originalRect.Width / this.currentZoomFactor), (int)(originalRect.Height / this.currentZoomFactor));
			return newRect;
		}

		
		
		
	
		#endregion


		/// <summary>
		/// Let the site invalidate the rectangle
		/// </summary>
		/// <param name="rect">invalid rectangle</param>
		public void InvalidateRectangle( Rectangle rect )
		{
			Invalidate( rect );
		}

		/// <summary>
		/// This is the event handler for the mousemove and returns an entity when it hits anything from the plex.
		/// The Mouse coordinates are converted to a RectangleF of zero size and passed to all the entities in the plex.
		/// If one of the plex finds that the cursor is contained in itself then it recturns itself.
		/// This place can be used to give feedback through statusbar messages or any other info coming from the entities.
		/// 
		/// </summary>
		/// <param name="p">Coordinates, normally this should be the mouse coordinates.</param>
		/// <returns>An entity; shape, connector, connection.</returns>
		protected Entity HitEntity(PointF p)
		{
			if (extract ==null) return null;
			RectangleF r = new RectangleF(p.X, p.Y, 0, 0);
			//------------------ test connector hits
			foreach (Shape o in extract.Shapes)
				foreach (Connector c in o.Connectors)
					if (c.Hit(r))
					{
						//String s = "\"" + c.Description + "\" connector of \"" + c.Shape.GetType().FullName + "\" object.";
						//Trace.WriteLine("The mouse hit the connector " +c.Description);
						return c;
					}
			//------------------ test shape hits
			foreach (Shape o in extract.Shapes)
				if (o.Hit(r))
				{
					return o;
				}
			//---------------- test connection hits
			foreach (Shape o in extract.Shapes)
				foreach (Connector c in o.Connectors)
					foreach (Connection n in c.Connections)
						if (n.Hit(r))
						{
							
							return n;
						}
			//--------------- nothing was hit at all...
			return null;
		}


		/// <summary>
		/// Pretty much the same as the HitEntity handler, it simply checks if the HitEntity returns something different than the previous hit.
		/// If it is something different than the previous hit it sets the internal 'Hover' entity to the new one.
		/// </summary>
		/// <param name="p">In normal circumstances this should be the mouse coordinates.</param>
		protected void HitHover(PointF p)
		{
			Entity Hit = HitEntity(p);
			
			if (Hit != Hover)
			{
				if (Hover != null) Hover.Hover = false;
				Hover = Hit;
				//				if(Hover==null)
				//					{Trace.WriteLine(DateTime.Now.ToString() + ": changed Hover to null");}
				//				else
				//					{Trace.WriteLine(DateTime.Now.ToString() + ": changed Hover to " + Hover.ToString());};
				if (Hover != null)
				{
					Hover.Hover = true;
				}
			}
		}
		/// <summary>
		/// Collects all modules of the astract and sets the IsSelected to False
		/// </summary>
		/// <param name="s">The collection of selected item</param>
		public void Deselect()
		{
			foreach (Shape o in extract.Shapes)
			{				
				o.IsSelected=false;
				foreach (Connector c in o.Connectors)
					foreach (Connection n in c.Connections)
						n.IsSelected=false;
						
			}
		}

		/// <summary>
		/// For the given point returns a cursor
		/// </summary>
		/// <param name="p">Floating-point point, usually linked to the cursor</param>
		protected void SetCursor(PointF p)
		{
			if (connection != null)
			{
				if ((Hover != null) && (typeof(Connector).IsInstanceOfType(Hover)))
					Cursor = Hover.GetCursor(p);
				else
					Cursor = MouseCursors.Cross;
			}
			else
			{
				if (Hover != null)
					Cursor = Hover.GetCursor(p);
				else
					Cursor = Cursors.Arrow;
			}
		}

		/// <summary>
		/// Handles the mouse down event
		/// </summary>
		/// <param name="e">Events arguments</param>

		protected override void OnMouseDown(MouseEventArgs e)
		{

			//make sure the canvas has the focus			
			this.Focus();
			// Get a point adjusted by the current scroll position and zoom factor
			PointF p = new PointF(e.X - this.AutoScrollPosition.X, e.Y - this.AutoScrollPosition.Y);
			p = UnzoomPoint(Point.Round(p));


			//pass the event to the hit entity
			HitHover(p);
			

			#region Ctrl+Shift left
			if((e.Button==MouseButtons.Left) && (e.Clicks==1) && (CtrlShift))
			{
				this.Zoom +=0.1F;
				CtrlShift = false;
				return;
			}
			#endregion
			#region Ctrl+Shift right
			if((e.Button==MouseButtons.Right) && (e.Clicks==1) && (CtrlShift))
			{
				this.Zoom -=0.1F;
				CtrlShift = false;
				this.ContextMenu = null;
				return;
			}
			#endregion
			#region Double click left
			//shows the properties of the underlying object
			if ((e.Button == MouseButtons.Left) && (e.Clicks == 2))
			{	
				//do we hit something under the cursor?
				HitHover(p);

				if ((Hover != null) && (typeof(Entity).IsInstanceOfType(Hover)))
				{					
					this.RaiseShowProps(((Entity) Hover).Properties);
					Update();
					return;
				}
			}
			#endregion
			#region SINGLE click left

			if ((e.Button == MouseButtons.Left) && (e.Clicks == 1))
			{

				

				// Alt+Click allows fast creation of elements
				if ((shapeObject == null) && (ModifierKeys == Keys.Alt))
				{
					shapeObject = this.GetShapeInstance(lastAddedShapeKey);

				}
				if (shapeObject != null)
				{
					shapeObject.Invalidate();
					RectangleF r = shapeObject.Rectangle;
					shapeObject.Rectangle = new RectangleF(p.X, p.Y, r.Width, r.Height);
					shapeObject.Invalidate();
					if(OnShapeAdded!=null) OnShapeAdded(this,shapeObject); //raise the event
					extract.Insert(shapeObject);
					shapeObject = null;
					return;
				}
				//reset the selector marquee
				selector = null;
				//see if something under the cursor
				HitHover(p); 
					
				if (Hover != null)//the click resulted in an object; shape or connection
				{
					if (typeof(Connector).IsInstanceOfType(Hover))  
					{
						if(!((Connector) Hover).AllowMultipleConnections && ((Connector) Hover).Connections.Count>0) return;
						if(!mAllowAddConnection) return;
						connection = new Connection(this);
						connection.Site=this;
						connection.From = (Connector) Hover;
						connection.ToPoint = p; //we use the mouse as To as long as we haven't a real connector, which will occur in the OnMouseUp
						Hover.Invalidate();
						Capture = true;
						Update();
						return;
					}

					// select object or add to the list of selected objects
					if (!Hover.IsSelected)
					{
						if (ModifierKeys != Keys.Shift) Deselect(); //s is empty only if shift is pushed
						//shift-click adds only ONE item while a normal click can change a set of shapes to
						//another state.
						Hover.IsSelected=true;
						Update();
					}
					//fix the node if the layout is manipulating the position
					if (typeof(Shape).IsInstanceOfType(Hover))
					{
						foreach(Shape sho in extract.Shapes) sho.Fixed=false;
						((Shape) Hover).Fixed=true;
					}

					// search tracking handle
					Point h = new Point(0, 0);
					if (extract.Shapes.Contains(Hover))
					{
						Shape o = (Shape) Hover;
						h = o.ShapeTracker.Hit(p);
					}

					foreach (Shape j in extract.Shapes)
					{
						if (j.ShapeTracker != null) //will only be one tracker per hit
						{
							j.ShapeTracker.Start(p, h);
							foreach(Connector c in j.Connectors)
							{
								foreach(Connection cn in c.Connections)
									if( cn.Tracker != null )
										cn.Tracker.Start(p,Point.Empty);
							}
									
						}

					}

					
					// Search tracker handle of connection and start tracking
					if( typeof(Connection).IsInstanceOfType(Hover) )
					{
						connection = Hover as Connection;
						h = connection.Tracker.Hit(p);
						connection.Tracker.Start(p,h);
					}

					mDoTrack = true;
					Capture = true;

					if ((Hover != null) && (typeof(Entity).IsInstanceOfType(Hover)))
					{
						(Hover as Entity).RaiseMouseDown(e);
					}

					SetCursor(p);
					return;
				}

				p=new PointF(e.X,e.Y);
				selector = new Selector(p, this);
			
				
				
				
			}
			#endregion
			
			#region SINGLE click right
			if (e.Button == MouseButtons.Right)
			{
				if (Hover != null)
				{
					if (!Hover.IsSelected)
					{
						//Select s = new Select();
						Deselect();
						//s.Add(Hover, true);
						//extract.History.Do(s);
						Hover.IsSelected=true;
						Update();
					}
				}

				if(this.mEnableContextMenu )
				{
					if(OnContextMenu !=null) OnContextMenu(this, e);
                    //This region must be dropped out. It is a little against the
                    //concept of object programming to let the successor of this class
                    //create some context menu and then erase it with this context menu
                    /*
					this.ContextMenu=new ContextMenu();

					ResetToBaseMenu();
					
					if(typeof(Shape).IsInstanceOfType(Hover))
					{
						//MenuItem[] tmp = new MenuItem[mContextMenu.MenuItems.Count];
						//mContextMenu.MenuItems.CopyTo(tmp,0);	
						
						MenuItem[] additionals = (Hover as Shape).ShapeMenu();
						if(additionals != null)	
						{
							this.ContextMenu.MenuItems.Add("-");
							this.ContextMenu.MenuItems.AddRange(additionals);
						}
						
					}
					else if(typeof(Connection).IsInstanceOfType(Hover))
					{
						if((Hover as Connection).LinePath== "Rectangular") return;
						this.insertionPoint = p;
						this.ContextMenu.MenuItems.Add("-");
						MenuItem[] subconnection = new MenuItem[2]{
								new MenuItem("Add point",new EventHandler(AddConnectionPoint)),
								new MenuItem("Delete point",new EventHandler(RemoveConnectionPoint))
																  };
						this.ContextMenu.MenuItems.Add(new MenuItem("Connection",subconnection));					
					}
                    */
				}
				
			}
			#endregion

			
		}

		private void AddConnectionPoint(object sender, EventArgs e)
		{

			//freeArrows.Add(new FreeArrow(new PointF(0,0),insertionPoint, "(" + insertionPoint.X + "," + insertionPoint.Y + ")"));			

		(Hover as Connection).AddConnectionPoint(insertionPoint);
		}

		private void RemoveConnectionPoint(object sender, EventArgs e)
		{
			(Hover as Connection).RemoveConnectionPoint(insertionPoint);
		}
		
		/// <summary>
		/// Handles the mouse up event
		/// </summary>
		/// <param name="e">Event arguments</param>
		protected override void OnMouseUp(MouseEventArgs e)
		{	
			base.OnMouseUp(e);
			// Get current mouse point adjusted by the current scroll position and zoom factor
			PointF p = new PointF(e.X - this.AutoScrollPosition.X, e.Y - this.AutoScrollPosition.Y);
			p = UnzoomPoint(Point.Round(p));
			
			//pass the event to the entity
			HitHover(p);
			if ((Hover != null) && (typeof(Entity).IsInstanceOfType(Hover)))
			{
				(Hover as Entity).RaiseMouseUp(e);
			}
			//paint the selector if there is one
			if (selector != null) selector.Paint(this);

			#region Left mouse button
			if (e.Button == MouseButtons.Left)
			{
				//are we dragging a new connection?
				if (connection != null)
				{
					HitHover(p);

					connection.Invalidate();
					//if the cursor is over a connector then attach new connection
					if ((Hover != null) && (typeof(Connector).IsInstanceOfType(Hover)))
						if (!Hover.Equals(connection.From))
						{//check if the connector can have an extra connection					
							if(((Connector) Hover).AllowMultipleConnections || ((Connector) Hover).Connections.Count==0) 
							{
								
								
									connection.Insert(connection.From,(Connector) Hover);
									connection.LinePath = this.connectionPath; //set the default path/shape style
									connection.LineEnd = this.connectionEnd; //the default end
								if(OnNewConnection != null)
								{
									if(!OnNewConnection(this,new ConnectionEventArgs(connection,true)))
									{
										connection.Delete(); //if the (external) handler tells it's not OK we delete the connection again
									}
								}
								
							}
						}

					connection = null;
					Capture = false;
				}
				//are we dragging a marquee to select shapes?
				if (selector != null)
				{					
					RectangleF r = selector.Rectangle;
					r = this.UnzoomRectangle(r);
					//if ((Hover == null) || (Hover.IsSelected == false))
					if (ModifierKeys != Keys.Shift) Deselect();

					if ((r.Width != 0) || (r.Height != 0))
					{  
						foreach (Shape o in extract.Shapes)
						{
							if (o.Hit(r)) 
							{
								o.IsSelected=true; 							
							}

							foreach (Connector c in o.Connectors)
								foreach (Connection n in c.Connections)
								{
									if (n.Hit(r)) 
									{										
										n.IsSelected=true;
										c.Invalidate();
									}
								}
						}
					}
					selector = null;
					Capture = false;       
				}
				//are we draggging or resizing shapes?
				if (mDoTrack)
				{	
					foreach (Shape o in extract.Shapes)
						if (o.ShapeTracker != null)
						{
							o.ShapeTracker.End();
							o.Invalidate();				
							o.Rectangle=o.ShapeTracker.Rectangle;
						}
					mDoTrack = false;
					Capture = false;
					HitHover(p);
				}
                OnFerdaMouseUp(this, e);
			}
			#endregion
			
			Update();
			SetCursor(p);
		}
		/// <summary>
		/// overrides the Mouse move event handler
		/// <br>if the mousemove is a dragging action on a tracker grip it will enlarge the tracker</br>
		/// 
		/// </summary>
		/// <param name="e">
		/// Mouse event arguments</param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			
			try
			{
				PointF p = new PointF(e.X - this.AutoScrollPosition.X, e.Y - this.AutoScrollPosition.Y);
				p = UnzoomPoint(Point.Round(p));	
				//marching ants - - - - 
				if (selector != null) selector.Paint(this);
				//the volatile temporary shape
				if (shapeObject != null)
				{
					shapeObject.Invalidate();            // invalidate previous rendering.
					RectangleF r = shapeObject.Rectangle;
					shapeObject.Rectangle = new RectangleF(p.X, p.Y, r.Width, r.Height);
					shapeObject.Invalidate();            // invalidate next rendering.
				}
				//are we moving a shape around, resizing?			
				if (mDoTrack)
				{	
					this.StopLayout();
					foreach (Shape o in extract.Shapes)
					{
						//ChangedStatus("(" + o.Rectangle.X.ToString() + "," + o.Rectangle.Y.ToString() + ")");
					
						if (o.ShapeTracker != null && o.Mobile && this.mAllowMoveShape)
						{

							o.Invalidate();							
							//for the subcontrols
							if(o.Controls != null && o.Controls.Count>0)
							{
								for(int k = 0 ; k<o.Controls.Count;k++)
								{
//									Control c=(o.Controls[k] as Control);
//									c.Width=Convert.ToInt32(o.Rectangle.Width)-6;
//									c.Height=Convert.ToInt32(o.Rectangle.Height)-6;
//									c.Left=Convert.ToInt32(o.Rectangle.Location.X)+3;
//									c.Top=Convert.ToInt32(o.Rectangle.Location.Y)+3;

									//o.Controls[k].Location = Point.Round(p);
								}
							}
							o.ShapeTracker.Move(p,this.Size,mSnap,mGridSize); //passing the Size of the canvas allows to keep the shapes inside the canvas!
							o.Invalidate();
							
							//connection tracker
							foreach(Connector c in o.Connectors)
							{
								foreach(Connection cn in c.Connections)
								{
									cn.Invalidate();
									if( cn.Tracker != null )
										cn.Tracker.MoveAll(p);
									cn.Invalidate();
								}
							}
						}
					}
					if( connection!=null && connection.Tracker != null )
					{
						connection.Invalidate();
						connection.Tracker.Move(p,this.Size,this.mSnap,this.mGridSize);						
						connection.Invalidate();
					}
					if(this.mEnableLayout) this.StartLayout();

				}
 
				if (connection != null)
				{
					connection.Invalidate();
					connection.ToPoint = p;
					connection.Invalidate();
				}

				HitHover(p); // set the internal Hover entity to the one hit, if any.

				if ((Hover != null) && (typeof(Entity).IsInstanceOfType(Hover)))
				{
					(Hover as Entity).RaiseMouseMove(e);
				}
			

				
				if (selector != null)
				{
					selector.Update(new PointF(e.X,e.Y));
					selector.Paint(this);
				}

		

				this.Invalidate(true);
			
				SetCursor(p);
				
				Update();

			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.Message);
			}

		}
		/// <summary>
		/// Paints an arrow
		/// </summary>
		/// <param name="g"></param>
		/// <param name="tip"></param>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <param name="filled"></param>
		public  void PaintArrow(Graphics g, PointF startPoint, PointF endPoint,Color lineColor, bool filled, bool showLabel)
		{
			try
			{
				g.DrawLine(new Pen(lineColor,1F),startPoint,endPoint);

				SolidBrush brush=new SolidBrush(lineColor);
				double angle = Math.Atan2(endPoint.Y - startPoint.Y,endPoint.X-startPoint.X);
				double length = Math.Sqrt((endPoint.X - startPoint.X)*(endPoint.X - startPoint.X)+(endPoint.Y - startPoint.Y)*(endPoint.Y - startPoint.Y))-10;
				double delta = Math.Atan2(7,length);
				PointF left = new PointF(Convert.ToSingle(startPoint.X + length*Math.Cos(angle-delta)),Convert.ToSingle(startPoint.Y+length*Math.Sin(angle-delta)));
				PointF right = new PointF(Convert.ToSingle(startPoint.X+length*Math.Cos(angle+delta)),Convert.ToSingle(startPoint.Y+length*Math.Sin(angle+delta)));

				PointF[] points={left, endPoint, right};
				if (filled)
					g.FillPolygon(brush,points);
				else
				{
					Pen p=new Pen(brush,1F);
					g.DrawLines(p,points);
				}
				if(showLabel)
				{
					g.DrawString("(" + endPoint.X + "," + endPoint.Y +")",new Font("Arial",10),brush,new PointF(endPoint.X-20,endPoint.Y-20));
				}
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.Message);
			}
				
		}
		/// <summary>
		/// Overrides the paint method and calls the paint method of the extract structure which will loop 
		/// through all the shapes, connections and connectors.
		/// Two other objects need attention here. One is an eventual shape just created and hanging at the cursor.
		/// The other is a connection fixed on a shape on one end but hanging at the cursor on the other (not attached yet).
		/// These two are painted separately.
		/// </summary>
		/// <param name="e">Paint event arguments.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			
			Rectangle b = ZoomRectangle(Rectangle.Round(extract.Rectangle));
			AutoScrollMinSize = b.Size;

			Graphics g =e.Graphics;
			//prepend transformations for zooming
			//transformations for zooming
			g.TranslateTransform(this.AutoScrollPosition.X, this.AutoScrollPosition.Y,MatrixOrder.Append);
			g.ScaleTransform(this.currentZoomFactor,this.currentZoomFactor,MatrixOrder.Prepend);			
			
			g.SmoothingMode=SmoothingMode.HighQuality;
		
#if DEBUG
			freeArrows.Paint(g);
#endif

			extract.Paint(g);
			if (shapeObject != null) shapeObject.Paint(g);
			if (connection != null) connection.PaintTrack(g);
		}
	
		
		/// <summary>
		/// Paints the background of the canvas, could have been done in the paint handler as well.
		/// This is not clear in the .Net doc.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			Graphics g=e.Graphics;	
			
			Rectangle r;
			switch (this.mBackgroundType)
			{
				case CanvasBackgroundTypes.Image:
					if (mBackgroundImagePath==null) return;
					if(mBackgroundImagePath.Trim()==string.Empty) return;
					try
					{
						Image im=Image.FromFile(BackgroundImagePath);
						g.DrawImage(im,0,0,Size.Width,Size.Height);
						if(mShowGrid)
						{
							r = this.ClientRectangle;
							ControlPaint.DrawGrid(g,r,new Size(mGridSize,mGridSize),Color.Wheat);
						}
					}
					catch (System.IO.FileNotFoundException exc)
					{
						this.mBackgroundType = CanvasBackgroundTypes.FlatColor;
						MessageBox.Show("The path to the background image was not found.","Error",MessageBoxButtons.OK,MessageBoxIcon.Warning);
						Trace.WriteLine(exc.Message);
					}							
					break;
				case CanvasBackgroundTypes.FlatColor:
					
					g.SmoothingMode=SmoothingMode.AntiAlias;
					r = this.ClientRectangle;
					g.FillRectangle(new SolidBrush(mBackgroundColor), r);
					if(mShowGrid)
						ControlPaint.DrawGrid(g,r,new Size(mGridSize,mGridSize),Color.Wheat);
					break;
				case CanvasBackgroundTypes.Gradient:
					
					g.SmoothingMode=SmoothingMode.AntiAlias;
					r = this.ClientRectangle;
					//g.FillRectangle(new SolidBrush(Color.White), r);
		
					LinearGradientMode lgm =LinearGradientMode.Vertical;
					LinearGradientBrush lgb = new LinearGradientBrush(r,this.mGradientTop,this.mGradientBottom,lgm);
					g.FillRectangle(lgb,r);
					if(mShowGrid)
						ControlPaint.DrawGrid(g,r,new Size(mGridSize,mGridSize),Color.Wheat);
					//some text on the canvas
					//g.DrawString("Netron", new Font("Verdana", 100,FontStyle.Bold),new SolidBrush(Color.White),55,55);
					
					break;

			}
			//the ControlPaint class has interesting stuff, have a look at it!
			//ControlPaint.DrawBorder3D(g,0,0,workingSize.Width,workingSize.Height);

			
			

		}
		
		#endregion		
		
		#region dragdrop
		protected override void OnDragDrop(DragEventArgs drgevent)
		{
			if(!mAllowAddShape) return;
			Shape sob = null;
			ShapeSummary summary = null;
			base.OnDragDrop (drgevent);
			// Store the data as a string so that it can be accessed from the
			// mnuCopy and mnuMove click events.
			//string sourceData = drgevent.Data.GetData(DataFormats.Text, true).ToString();
			//Shape sob=(Shape)  drgevent.Data.GetData( drgevent.Data.GetFormats()[0]);
			try
			{
				summary = drgevent.Data.GetData(typeof(ShapeSummary).FullName,false) as ShapeSummary;
			}
			catch
			{
				MessageBox.Show("No valid shape data found to create.","No data error",MessageBoxButtons.OK,MessageBoxIcon.Warning);
			}
			try
			{
				sob = GetShapeInstance(summary.ShapeKey);
				if(sob==null) throw new Exception();
			}
			catch
			{
				MessageBox.Show("Shape data not found. \n (Did you add the necessary shape mLibraries?)","No data error",MessageBoxButtons.OK,MessageBoxIcon.Warning);
			}
			
			Point currentPt = this.PointToClient( new Point(drgevent.X,drgevent.Y) );

			// Get the mouse drop location 
			PointF p = new PointF(currentPt.X - this.AutoScrollPosition.X, 
				currentPt.Y - this.AutoScrollPosition.Y);
			p = UnzoomPoint(Point.Round(p));
			if(sob != null)	this.AddShape(sob,p);
		}

		protected override void OnDragEnter(DragEventArgs drgevent)
		{
			base.OnDragEnter (drgevent);
			if ((drgevent.AllowedEffect & DragDropEffects.Move) == DragDropEffects.Move && (drgevent.KeyState & ctrlKey) != ctrlKey)
			{
				// Show the standard Move icon.
				drgevent.Effect = DragDropEffects.Move;
			}
			else
			{
				// Show the standard Copy icon.
				drgevent.Effect = DragDropEffects.Copy;
			}
		}

		#endregion
	
		#region Menu related

		protected void OnNewGraph(object sender, EventArgs e)
		{
			this.New();
		}
		protected void OnSelectAll(object sender, EventArgs e)
		{
			this.SelectAll(true);
		}

		protected void OnCut(object sender,EventArgs e)
		{
			return;
		}
		protected void OnCopy(object sender,EventArgs e)
		{
			this.Copy();
		}
		protected void OnPaste(object sender,EventArgs e)
		{
			this.Paste();;
		}
		protected void OnProperties(object sender,EventArgs e)
		{
			if(Hover!=null)
				this.RaiseShowProps(Hover.Properties);
			else
				this.RaiseShowProps(this.Properties);
		}
		/// <summary>
		/// Common gate for a delete action on the plex. Deletes the selected shapes and goes via the history (of course).
		/// </summary>		
		protected void OnDelete(object sender,EventArgs e)
		{
			try
			{
				if(!mAllowDeleteShape) return;

				ArrayList col = this.extract.mShapes.Clone() as ArrayList;
				
				foreach(Shape sh in this.extract.mShapes.Clone() as ArrayList)
				{
					
					if (sh.IsSelected) 
					{
						if(OnShapeRemoved!=null)
							OnShapeRemoved(this,sh);
						//sh.Delete();
					}
					else
						foreach(Connector c in sh.Connectors)
						{
							ArrayList ar = (ArrayList) c.Connections.Clone();
                            foreach (Connection n in ar)
                            {
                                if (n.IsSelected)
                                {
                                    //throw an event
                                    OnFerdaDeleteConnection(this, n);
                                    n.Delete();
                                }
                            }
						}
					//extract.Delete();
				}
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.Message);
			}
			Update();	
		}
		/// <summary>
		/// Adds the given shape summary to the context menu
		/// </summary>
		/// <param name="summary"></param>
		protected void AddToContextMenu(ShapeSummary summary)
		{
			MenuItem currentItem =null;
			foreach(MenuItem item in this.mContextMenu.MenuItems)
			{
				if(item.Text == summary.ShapeCategory){ currentItem = item; break;}
			}
			if(currentItem == null)
			{
				this.mContextMenu.MenuItems.Add("-");
				currentItem =new MenuItem(summary.ShapeCategory);
				this.mContextMenu.MenuItems.Add(currentItem);
			}
			GraphMenuItem i =new GraphMenuItem(summary,new EventHandler(OnContextMenuItem));			
			currentItem.MenuItems.Add(i);


		}		

		/// <summary>
		/// Fires when a contextmenu item is selected
		/// </summary>
		/// <param name="sender">a MenuItem</param>
		/// <param name="e">an EventArgs</param>
		protected void OnContextMenuItem(object sender, EventArgs e)
		{
			if(sender is GraphMenuItem)
			{
				ShapeSummary summary = (sender as GraphMenuItem).Summary;
				Shape shape = this.GetShapeInstance(summary.ShapeKey);
				if(shape != null & AllowAddShape)
					this.AddShape(shape);
			}
		}
	

		protected void AddBaseMenu()
		{
			//mContextMenu.MenuItems.Add("-");//add a separation
			MenuItem graphItem= new MenuItem("Graph");
			mContextMenu.MenuItems.Add(graphItem);
			graphItem.MenuItems.Add("New",new EventHandler(OnNewGraph));
			graphItem.MenuItems.Add("SelectAll",new EventHandler(OnSelectAll));
				
			mContextMenu.MenuItems.Add("-");//add a separation
			//Cut, copy, paste doesn't work cause of a bug in Net, hopefully fixed in Net v2
			/*
			mContextMenu.MenuItems.Add(new MenuItem("C&ut", new EventHandler(OnCut)));
			mContextMenu.MenuItems.Add(new MenuItem("&Copy", new EventHandler(OnCopy)));
			mContextMenu.MenuItems.Add(new MenuItem("&Paste", new EventHandler(OnPaste)));
			*/
			mContextMenu.MenuItems.Add(new MenuItem("&Delete", new EventHandler(OnDelete)));
			mContextMenu.MenuItems.Add("-");//add a separation
			mContextMenu.MenuItems.Add(new MenuItem("P&roperties", new EventHandler(OnProperties)));
		}

		/// <summary>
		/// Resets the menu to its base state.
		/// <see cref="Right click mouse-down handler"/>
		/// </summary>
		protected void ResetToBaseMenu()
		{
			int howmany = mContextMenu.MenuItems.Count;
			for(int k=0; k<howmany; k++)
				this.ContextMenu.MenuItems.Add(mContextMenu.MenuItems[k].CloneMenu());
		}
		#endregion
		
		#region Printing
	
		public void PrintCanvas(object Sender, PrintPageEventArgs e)
		{
			Graphics g = e.Graphics;
			float w = extract.Rectangle.Width;
			float h = extract.Rectangle.Height;
			float zoom = ( w >= h )? e.MarginBounds.Width / w : e.MarginBounds.Height / h;
			g.ScaleTransform( zoom , zoom  );
			this.extract.Paint(g);
		}
		
		#endregion

		#region Automata related
		/// <summary>
		/// This starts the heart beat of the automata data flow, it simply calls the start method of the timer.
		/// </summary>
		public void StartAutomata()
		{
			if(OnStartAutomata != null) OnStartAutomata(this, EventArgs.Empty);
			transmissionTimer.Start();
		}
		/// <summary>
		/// This stops the heart beat of the automata data flow.
		/// </summary>
		public void StopAutomata()
		{
			if(OnStopAutomata != null) OnStopAutomata(this, EventArgs.Empty);
			transmissionTimer.Stop();
		}

		public void ResetAutomata()
		{
			foreach(Shape so in this.extract.Shapes)
			{
				so.InitAutomata();
			}
		}

		/// <summary>
		/// This is the event handler when the transmission timer fires. Handles the transmissions of data over the connections.
		/// 
		/// </summary>
		/// <param name="Sender"></param>
		/// <param name="e"></param>
		public void OnTransmissionTimer(object Sender, EventArgs e)
		{
			if(OnDataTransmission !=null) OnDataTransmission(this, EventArgs.Empty);
			extract.Transmit();
			extract.Update();
			Invalidate();		
			Application.DoEvents();
		}
		
		#endregion

		#region Layout related
		/// <summary>
		/// Starts the graph layout thread using a previously defined algorithm
		/// </summary>
		public void StartLayout()
		{
			
			if(thLayout !=null) thLayout.Abort();
			ts=null;
			try
			{
				
				ts=new ThreadStart(layoutFactory.GetRunable());
				thLayout = new Thread(ts);
				thLayout.Start();
			}
			catch
			{
				MessageBox.Show("Unable to launch the graph layout process","Graph layout exception",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
			}

		}
		/// <summary>
		/// Stops the graph layout thread 
		/// </summary>
		public void StopLayout()
		{
			if (thLayout !=null)
				if (thLayout.IsAlive) this.thLayout.Abort();
		}
		
		#endregion

		#region Input/output

		public void SaveGraphMLAs(string filePath)
		{
			XmlTextWriter tw = new XmlTextWriter(filePath,System.Text.Encoding.Unicode);
			GraphMLSerializer g = new GraphMLSerializer();
			g.Serialize(tw,extract);
			tw.Close();
		}

        /*
		/// <summary>
		/// GDI32 imported function not available in the framework,
		/// used here to save a picture of the turtle world.
		/// Can also be used, in general, to take a snapshot of a (actually) any) control.
		/// </summary>
		/// <param name="hdcDest"></param>
		/// <param name="nXDest"></param>
		/// <param name="nYDest"></param>
		/// <param name="nWidth"></param>
		/// <param name="nHeight"></param>
		/// <param name="hdcSrc"></param>
		/// <param name="nXSrc"></param>
		/// <param name="nYSrc"></param>
		/// <param name="dwRop"></param>
		/// <returns></returns>
		[DllImport("gdi32.dll")]
		private static extern bool BitBlt(

			IntPtr hdcDest, // handle to destination DC

			int nXDest, // x-coord of destination upper-left corner

			int nYDest, // y-coord of destination upper-left corner

			int nWidth, // width of destination rectangle

			int nHeight, // height of destination rectangle

			IntPtr hdcSrc, // handle to source DC

			int nXSrc, // x-coordinate of source upper-left corner

			int nYSrc, // y-coordinate of source upper-left corner

			System.Int32 dwRop // raster operation code

			);
		/// <summary>
		/// Saves an image of the canvas in JPG format.
		/// </summary>
		/// <param name="path"></param>
		public void SaveImage(string path)
		{
			//not a simple thing to do, seemingly you need interop to do it....
			Graphics g1 = this.CreateGraphics();
			Bitmap bm=new Bitmap(this.Width,this.Height,g1);
			
			Bitmap backBM=new Bitmap(this.Width,this.Height);
			Graphics g2 = Graphics.FromImage(backBM);

			System.IntPtr dc1=g1.GetHdc();

			System.IntPtr dc2=g2.GetHdc();

			BitBlt(dc2,0,0,this.Size.Width,this.Size.Height,dc1,0,0,0x00CC0020);

			g1.ReleaseHdc(dc1);

			g2.ReleaseHdc(dc2);

			g1.Dispose();

			g2.Dispose();
			backBM.Save(path, ImageFormat.Jpeg);

		}
        */

		public void LoadLibraries()
		{
			ArrayList graphLibs = ConfigurationSettings.GetConfig("GraphLibs") as ArrayList;
			if(graphLibs==null) return ;
			if(graphLibs.Count>0)
			{
				for(int k=0; k<graphLibs.Count;k++)
				{
					this.ImportShapes(graphLibs[k] as string);
				}
			}
			

			

			
		}

		/// <summary>
		/// Adds a shape library to the collection
		/// </summary>
		/// <param name="path"></param>
		public void AddLibrary(string path)
		{
			this.ImportShapes(path);
		}

		protected void ImportShapes(string path)
		{
			GraphObjectsLibrary library = new GraphObjectsLibrary();
			ShapeSummary shapeSum;
			ConnectionSummary conSum;
			library.Path = path;
			mLibraries.Add(library);
			
			try
			{			
				Assembly ass=Assembly.LoadFrom(path);
				if (ass==null) return;
				Type[] tps=ass.GetTypes();
			
				if (tps==null) return ;
				
				object[] objs;
				for(int k=0; k<tps.Length;k++) //loop over modules in assembly
				{
					
					if(!tps[k].IsClass) continue;		
					try
					{
						objs = tps[k].GetCustomAttributes(typeof(Netron.GraphLib.Attributes.NetronGraphShapeAttribute),false);
						if(objs.Length>=1)
						{
							
							//now, we are sure to have a shape object					
					
							NetronGraphShapeAttribute shapeAtts = objs[0] as NetronGraphShapeAttribute;
							shapeSum = new ShapeSummary(path, shapeAtts.ShapeKey,shapeAtts.ShapeName, shapeAtts.ShapeCategory,shapeAtts.ReflectionName);
							library.ShapeSummaries.Add(shapeSum);
							if(this.mEnableContextMenu)
								AddToContextMenu(shapeSum);
							
							continue;//cannot be a connection and a shape at the same time
						}
						
						//try a custom connection

						objs = tps[k].GetCustomAttributes(typeof(Netron.GraphLib.Attributes.NetronGraphConnectionAttribute),false);
						if(objs.Length>=1) 						
						{
							//now, we are sure to have a connection object					
					
							NetronGraphConnectionAttribute conAtts = objs[0] as NetronGraphConnectionAttribute;
							conSum = new ConnectionSummary(path, conAtts.ConnectionName, conAtts.ReflectionName);
							library.ConnectionSummaries.Add(conSum);	
							continue;					
						}
					}
					catch(Exception exc)
					{
						Trace.WriteLine(exc.Message);
						continue;
					}
							
				}
				
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.Message);
			}
		}


		/// <summary>
		/// Saves the plex to the selected path using the wonderful .Net serialization.
		/// </summary>
		/// <remarks><br>Would it be possible to save all this to a database?</br>
		/// <br>You can also use the XML or SOAP serialization but note that only public methods and properties will be serialized, not sure the plex will be allright on deserialization.</br></remarks>
		/// <param name="mFileName">The path to which to save the data.</param>
		public bool SaveAs(string mFileName)
		{
			
			this.mFileName = mFileName;
			return BinarySerializer.SaveAs(mFileName,this);
		}
		/// <summary>
		/// Opens the specified file and deserializes it. In essence the extract is being filled with the saved data and the next OnPaint event simply paints its contents.
		/// </summary>
		/// <param name="mFileName"></param>
		public void Open(String mFileName)

		{
			BinarySerializer.Open(mFileName,this);

			this.mFileName = mFileName;
			//notify the outside world
			if(OnOpen!=null)
				OnOpen(mFileName);
		}

		protected internal void OutputInfo(object obj)
		{
			if(OnInfo !=null)
				OnInfo(obj);
		}
		public void RaiseShowProps(PropertyBag props)
		{
			if(ShowNodeProperties !=null) 				
				ShowNodeProperties(this,props);
			
		}
		/// <summary>
		/// Handles the key press
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnKeyPress(object sender, KeyPressEventArgs e)
		{
			//pass the event down to the shapes
			for(int k=0; k<this.extract.mShapes.Count; k++)
			{
				extract.mShapes[k].OnKeyPress(e);
			}
		}
		/// <summary>
		/// This is the general event handler for key events
		/// </summary>
		/// <param name="e">A KeyEventArgs object</param>
		private void OnKeyDown(object sender, KeyEventArgs e)
		{

			//pass the event down to the shapes
			for(int k=0; k<this.extract.mShapes.Count; k++)
			{
				extract.mShapes[k].OnKeyDown(e);
			}

			if (Keys.Escape==e.KeyData)
			{
				mDoTrack = false;
				foreach (Shape o in this.extract.Shapes)
				{
					o.IsSelected=false;
				}				
			};

			if (e.KeyCode==Keys.A && e.Control)	this.SelectAll(true);

			if (e.KeyCode==Keys.S && e.Control) 
				if (this.mFileName!=null) this.SaveAs(this.mFileName);
			PointF p =this.PointToClient(MousePosition);

			//insert a basic node
			if(e.KeyCode==Keys.Insert && e.Control) 
			{
				AddBasicNode(BasicShapeTypes.BasicNode,"New node",p);
			}
			//insert a simple node
			if(e.KeyCode==Keys.N && e.Control) 
			{
				AddBasicNode(BasicShapeTypes.SimpelNode,"New node",p);
			}
			//insert a text label
			if(e.KeyCode==Keys.T && e.Control) 
			{
				AddBasicNode(BasicShapeTypes.TextLabel,"New node",p);
			}

			if(e.KeyCode==Keys.Left && e.Control) 
			{
				foreach (Shape o in extract.Shapes)
				{
					if(!o.IsSelected) continue;
					o.Invalidate();					
					if(mSnap)
					{
						o.X-=mGridSize;
						o.X=o.X-o.X%mGridSize;						
					}
					else
					{
						o.X-=1;
					}
					if(this.mRestrictToCanvas)	o.X = Math.Max(o.X,2);
					o.ShapeTracker.Rectangle=o.Rectangle;
					o.Invalidate();
				}
				this.Invalidate(true);
			}

			if(e.KeyCode==Keys.Right && e.Control) 
			{
				foreach (Shape o in extract.Shapes)
				{
					if(!o.IsSelected) continue;
					o.Invalidate();
					if(mSnap)
					{
						o.X+= mGridSize;						
						o.X=o.X-o.X%mGridSize;
					}
					else
					{
						o.X+=1;					
					}
					if(mRestrictToCanvas) o.X = Math.Min(o.X, this.Width-o.Width-2);
					o.ShapeTracker.Rectangle=o.Rectangle;
					o.Invalidate();
				}
				this.Invalidate(true);
			}

			if(e.KeyCode==Keys.Up && e.Control) 
			{
				foreach (Shape o in extract.Shapes)
				{
					if(!o.IsSelected) continue;
					o.Invalidate();
					if(mSnap)
					{
						o.Y-=mGridSize;						
						o.Y=o.Y-o.Y%mGridSize;
					}
					else
					{						
							o.Y-=1;						
					}
					if(mRestrictToCanvas) o.Y = Math.Max(o.Y, 2);
					o.ShapeTracker.Rectangle=o.Rectangle;
					o.Invalidate();
				}
				this.Invalidate(true);
			}

			if(e.KeyCode==Keys.Down && e.Control) 
			{
				foreach (Shape o in extract.Shapes)
				{
					if(!o.IsSelected) continue;
					o.Invalidate();
					if(mSnap)
					{
						o.Y+=mGridSize;
						o.Y=o.Y-o.Y%mGridSize;
					}
					else
					{
						o.Y+=1;
					}
					if(mRestrictToCanvas) o.Y = Math.Min(o.Y, this.Height-o.Height-2);
					o.ShapeTracker.Rectangle=o.Rectangle;
					o.Invalidate();
				}
				this.Invalidate(true);
			}

			if(e.KeyCode==Keys.ShiftKey && e.Control) 
			{
				CtrlShift = true;
			}

			if(e.KeyCode==Keys.Delete)
			{
				this.OnDelete(this, EventArgs.Empty);
			}
			
			SetCursor(p);
		}

		/// <summary>
		/// Moves the scrollbar up-down.
		/// You can override this and let it zoom instead
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel (e);

			this.AutoScrollPosition.Offset(5*e.Delta,5*e.Delta);
			this.Invalidate();

		}

		#endregion

		#region Graph operations

		#region various overloads of AddShape
		/// <summary>
		/// Adds a given shape to the canvas at the specified position
		/// </summary>
		/// <param name="sob">a shape object</param>
		/// <param name="position">the position at twhich the shape has to be placed</param>
		/// <returns>the added shape object or null if unsuccessful</returns>
		protected Shape AddShape(Shape sob, PointF position)
		{
			try
			{
				sob.Site=this;
				//sob.Invalidate();
				RectangleF r = sob.Rectangle;
				
				sob.Rectangle = new RectangleF(position.X, position.Y, r.Width, r.Height);

				sob.Invalidate();
#if NAFEnabled
				//only necessary if you want NAF integration
				sob.Root = this.root;

#endif
				extract.Insert(sob);
				if(OnShapeAdded !=null)
				{
					OnShapeAdded(this,sob);
				}
			
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.Message);
				sob = null;
			}
			return sob ;
		}


		public Shape AddShape(string  key, PointF position)
		{
			
			Shape sob = GetShapeInstance(key);
			return AddShape(sob,position);
		}

		public void RemoveShape(Shape shape)
		{
			if(OnShapeRemoved!=null)
				OnShapeRemoved(this,shape);
		}

		/// <summary>
		/// Adds a given shape to the canvas at the mouse position
		/// </summary>
		/// <param name="sob"></param>
		/// <returns></returns>
		protected Shape AddShape(Shape sob)
		{
			return this.AddShape(sob, Center);
		}
		#endregion

		#region Add node methods
		
		public Shape AddNode(BasicShapeTypes shapeType, string label)
		{
			return AddBasicNode(shapeType, label, Center);			
		}
		public Shape AddNode(BasicShapeTypes shapeType, string label, PointF position)
		{
			return AddBasicNode(shapeType, label, position);			
		}
		public Shape AddNode()
		{
			return AddBasicNode(BasicShapeTypes.SimpelNode,"[Not set]", Center);						
		}
		public Shape AddNode(string label)
		{
			return AddBasicNode(BasicShapeTypes.SimpelNode,label, Center);						
		}
		public Shape AddNode(string label ,PointF position)
		{
			return AddBasicNode(BasicShapeTypes.SimpelNode, label,position);			
		}
		public Shape AddNode(PointF position)
		{
			return AddBasicNode(BasicShapeTypes.SimpelNode,"[Not set]",position);			
		}
		private Shape AddBasicNode(BasicShapeTypes shapeType, string nodeLabel, PointF position)
		{
			
			string key =string.Empty;
			Shape sob = null;
			switch(shapeType)
			{
				case BasicShapeTypes.BasicNode: key = "8ED1469D-90B2-43ab-B000-4FF5C682F530"; break;
				case BasicShapeTypes.SimpelNode: key = "57AF94BA-4129-45dc-B8FD-F82CA3B4433E"; break;
				case BasicShapeTypes.TextLabel: key = "4F878611-3196-4d12-BA36-705F502C8A6B"; break;
			}			
			
			sob= GetShapeInstance(key);		
			if(sob != null)	
			{
				this.AddShape(sob,position);
				sob.Text=nodeLabel;		
#if NAFEnabled
				sob.Root=this.root;
#endif
			if(OnShapeAdded !=null)
			{
				OnShapeAdded(this,sob);
			}
			}
			
			return sob;
			
			//			shapeObject = new BasicNode(this);
			//
			//			if (shapeObject != null)
			//			{
			//				shapeObject.Invalidate();
			//				RectangleF r = shapeObject.Rectangle;
			//				
			//					shapeObject.Rectangle = new RectangleF(position.X, position.Y, r.Width, r.Height);
			//
			//				shapeObject.Invalidate();
			//
			//				extract.Insert(shapeObject);
			//				((BasicNode) shapeObject).ShowProps +=new ShowPropsDelegate(CanvasControl_ShowProps);
			//				if (NodeLabel==null)
			//				{
			//					((BasicNode) shapeObject).Label="Node " + BasicNodeCounter.ToString();
			//					BasicNodeCounter++; //just a way to label newly created nodes	
			//				}
			//				else
			//					((BasicNode) shapeObject).Label=NodeLabel;
			//				
			//				shapeObject = null;
			//					
			//			}
		}


		
		

		#endregion

		/// <summary>
		/// Returns whether for the given two connectors there is a connection
		/// </summary>
		/// <param name="b"></param>
		/// <param name="e"></param>
		/// <returns></returns>
		protected Boolean ConnectionExists(Connector b,Connector e)
		{

			foreach(Shape so in extract.Shapes)
				foreach(Connector c in so.Connectors)
					foreach(Connection n in c.Connections)
						if ((n.From.Equals(b) && n.To.Equals(e) || (n.From.Equals(e) && n.To.Equals(b) ))) return true;
		
			return false;
		}

		/// <summary>
		/// Returns the entity with the specified UID
		/// </summary>
		/// <param name="UID">The unique identitfier specifying the entity</param>
		/// <returns></returns>
		public Entity GetEntity(string UID)
		{
			foreach(Shape en in this.extract.Shapes)
			{
				if(en.UID.ToString()==UID) return en;
				foreach(Connector c in  en.Connectors)
				{
					if(c.UID.ToString()==UID) return c;	
					foreach(Connection n in c.Connections)
						if (n.UID.ToString()==UID) return n;
				}
			}
			return null;
		}
		/// <summary>
		/// Implements the copy function to the clipboard
		/// </summary>
		/// <remarks>
		/// This doesn't work, bug in Clipboard object, hopefully fixed in Net v2 
		///</remarks>
		public void Copy()
		{
			
			ShapeCollection ar = new ShapeCollection();
			foreach(Shape so in extract.Shapes)
			{	
				if(so.IsSelected)
				{
					ar.Add(so);					
				}
			}
			if(ar.Count>0)
			{
					
				DataObject blurb = new DataObject(format.Name,extract);			
				Clipboard.SetDataObject(blurb);
			}

			
		}

		/// <summary>
		/// The clipboard paste action.
		/// 
		/// </summary>
		/// <remarks>
		/// This doesn't work, bug in Clipboard object, hopefully fixed in Net v2 
		///</remarks>
		public void Paste()
		{
			
			IDataObject data = Clipboard.GetDataObject();
			
			
			if (data.GetDataPresent(format.Name))
			{				
				//unselect all
				foreach(Shape sh in extract.Shapes) sh.IsSelected=false;

				foreach(Shape so in data.GetData(format.Name) as ArrayList)
				{
					
					so.IsSelected=true;
					so.GenerateNewUID();
					so.ShapeTracker.Move(new PointF(10,10),Size,mSnap,mGridSize);
					so.Invalidate();
					RectangleF r = so.Rectangle;
					so.Rectangle = new RectangleF(so.Rectangle.X+10, so.Rectangle.Y+10, r.Width, r.Height);
					so.Invalidate();
											
					so.Insert(this.extract);


		
					foreach(Connector c in so.Connectors)
					{
						c.GenerateNewUID();
						//						ArrayList connar = c.Connections.Clone();
						foreach(Connection n in c.Connections)
						{
							
							Connector s = null;
							Connector e = null;
							if (n.From==null || n.To==null) continue;
							if (n.From.mIsReset && n.To.mIsReset) 
							{
								s=n.From;e=n.To; 
							}
							else if(n.From.IsReset) //find the existing (before paste, that is) connector UID
							{s=n.From;e=(Connector) GetEntity(n.To.UID.ToString());}								
							else if(n.To.IsReset==true)
							{s=(Connector) GetEntity(n.From.UID.ToString());e=n.To;}
							//only add it if it does not exist
							if (!ConnectionExists(s,e))
							{
								n.GenerateNewUID();
								//m.AddInsertConnection(n,s,e);
								n.Insert(s,e);
								
							}
						}
						c.Connections.Clear();
					}

				}

				//extract.History.Do(m);				
			}

			

		}


		/// <summary>
		/// There is only one way to specify a connection: by specifying the two connectors
		/// Unless you have a single connector per shape which allows you to use the shape GUID
		/// Going via node label is possible as well if you filter out doubles on (automatic) creation.
		/// </summary>
		/// <param name="From"></param>
		/// <param name="To"></param>
		public Connection AddEdge(Connector From, Connector To)
		{
			if (From !=null && To != null)
			{
				Connection con =new Connection(this);
				con.From=From;
				con.To=To;
				From.Connections.Add(con);
				To.Connections.Add(con);
				con.LineEnd=this.DefaultLineEnd;
				con.Site = this;
				Update();
				Invalidate();
				if(OnNewConnection != null)
				{
					OnNewConnection(this,new ConnectionEventArgs(con,false));
    			}
				return con;

			}
			return null;
	
		}
		
	
		
		
		/// <summary>
		/// Selects all shapes and things of the plex.
		/// </summary>
		public void SelectAll(bool includeAll)
		{
			if(includeAll)
			{
				foreach (Shape o in extract.Shapes)
				{
					o.IsSelected= true;

					foreach (Connector c in o.Connectors)
						foreach (Connection n in c.Connections)
							n.IsSelected=true;
				}

			
				Update();
			}
			else
			{
				foreach (Shape so in extract.Shapes)
				{
					so.IsSelected=true;
				}
			}
		}



		protected Shape GetShapeInstance(string shapeKey)
		{
			Shape shape = null;
			ObjectHandle handle;									
			
			for(int k=0; k<mLibraries.Count;k++)
			{
				for(int m=0; m<mLibraries[k].ShapeSummaries.Count; m++)
				{
					if(mLibraries[k].ShapeSummaries[m].ShapeKey == shapeKey)
					{
						//TODO: lightweight pattern here?
						//Assembly ass = Assembly.LoadFrom(mLibraries[k].Path);
						try
						{
								
							//activationAttributes = new object[]{new SynchronizationAttribute()};
							//Note this one! The OpenFileDialog changes the CurrentDirectory, this is the only way to make sure .Net will look in the bin directory.
							Directory.SetCurrentDirectory(Path.GetDirectoryName(Application.ExecutablePath));
							handle = Activator.CreateInstanceFrom(mLibraries[k].Path,mLibraries[k].ShapeSummaries[m].ReflectionName);
							shape = handle.Unwrap() as Shape;
							shape.Site = this;
							lastAddedShapeKey = shapeKey; //keep for ALT+click addition
							return shape;
						}
						catch(Exception exc)
						{
							Trace.WriteLine(exc.Message);
						}
					}
				}
			}
			return shape;
		}

		

		/// <summary>
		/// Clears the canvas of nodes and edges, goes via the clearing of the attached GraphAbstract class
		/// </summary>
		public void Clear()
		{

			this.extract.Shapes.Clear();
			this.Invalidate();


			if(OnClear != null)
				OnClear(this, EventArgs.Empty);			

		}
		public Shape GetNodeByLabel(string label)
		{
			foreach(Shape so in extract.Shapes)
				if(so.Text.ToLower()==label.ToLower())	return so;
			return null;
		}
		/// <summary>
		/// Creates a blank new canvas
		/// </summary>
		public void New()
		{
			//save before new?			


			this.extract=new GraphAbstract();
			this.mFileName=null;
		}

		protected override void WndProc(ref Message m)
		{
			if(m.Msg==WM_VSCROLL || m.Msg==WM_HSCROLL)
			{
				this.Invalidate();
			}
			base.WndProc (ref m);
		}


		#endregion

		#region Property bag

		/// <summary>
		/// Determines which properties are accessible via the property grid
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected virtual void GetPropertyBagValue(object sender, PropertySpecEventArgs e)
		{
			switch(e.Property.Name)
			{
				
				case "AutomataPulse": e.Value=this.mAutomataPulse;break;
				case "BackgroundColor": e.Value=this.mBackgroundColor;break;
				case "BackgroundImagePath": e.Value=this.mBackgroundImagePath;break;
				case "DefaultConnectionPath": e.Value=this.connectionPath;break;
				case "DefaultLineEnd": e.Value=this.connectionEnd;break;
				case "EnableContextMenu": e.Value=this.mEnableContextMenu;break;
				case "EnableLayout": e.Value=this.mEnableLayout;break;
				case "GradientBottom": e.Value=this.mGradientBottom;break;
				case "GradientTop": e.Value=this.mGradientTop;break;
				case "GraphLayoutAlgorithm": e.Value=this.layoutFactory.GraphLayoutAlgorithm;break;
				case "GridSize": e.Value=this.mGridSize;break;
				case "RestrictToCanvas": e.Value=this.mRestrictToCanvas;break;
				case "ShowGrid": e.Value=this.mShowGrid;break;
				case "Snap": e.Value=this.mSnap;break;
				case "BackgroundType": e.Value=this.mBackgroundType;break;
				case "AllowAddShape": e.Value=this.mAllowAddShape; break;
				case "AllowAddConnection": e.Value=this.mAllowAddConnection; break;
				case "AllowDeleteShape": e.Value = this.mAllowDeleteShape; break;
				case "AllowMoveShape": e.Value = this.mAllowMoveShape; break;
				
			}
		}


		/// <summary>
		/// Sets the values passed by the property grid
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected virtual void SetPropertyBagValue(object sender, PropertySpecEventArgs e)
		{

			switch(e.Property.Name)
			{
				case "AutomataPulse": 
					this.AutomataPulse = (int) e.Value;										
					break;
				case "BackgroundColor": 
					this.mBackgroundColor = (Color) e.Value;					
					this.Invalidate();
					break;
				case "BackgroundImagePath": 
					this.mBackgroundImagePath = (string) e.Value;					
					this.Invalidate();
					break;
				case "DefaultConnectionPath": 
					this.connectionPath = (string) e.Value;										
					break;
				case "DefaultLineEnd": 
					this.connectionEnd = (ConnectionEnds) e.Value;										
					break;
				case "EnableContextMenu": 
					this.EnableContextMenu = (bool) e.Value;					
					this.Invalidate();
					break;
				case "EnableLayout": 
					this.EnableLayout = (bool) e.Value;
					break;
				case "GradientBottom": 
					this.mGradientBottom = (Color) e.Value;					
					this.Invalidate();
					break;
				case "GradientTop": 
					this.mGradientTop = (Color) e.Value;					
					this.Invalidate();
					break;
				case "GraphLayoutAlgorithm": 
					this.layoutFactory.GraphLayoutAlgorithm = (GraphLayoutAlgorithms) e.Value;										
					break;
				case "GridSize": 
					this.mGridSize = (int) e.Value;					
					this.Invalidate();
					break;				
				case "RestrictToCanvas": 
					this.mRestrictToCanvas = (bool) e.Value;										
					break;
				case "ShowGrid": 
					this.mShowGrid = (bool) e.Value;					
					this.Invalidate();
					break;
				case "Snap": 
					this.mSnap = (bool) e.Value;					
					this.Invalidate();
					break;
				case "BackgroundType": 
					this.mBackgroundType= (CanvasBackgroundTypes) e.Value;					
					this.Invalidate();
					break;
				case "AllowAddShape":
					this.mAllowAddShape = (bool) e.Value;
					break;
				case "AllowAddConnection":
					this.mAllowAddConnection = (bool) e.Value;
					break;
				case "AllowDeleteShape":
					this.mAllowDeleteShape = (bool) e.Value;
					break;
				case "AllowMoveShape":
					this.mAllowMoveShape = (bool) e.Value;
					break;




			}
		}

		

		protected void AddProperties()
		{
			try
			{					
				bag.GetValue+=new PropertySpecEventHandler(GetPropertyBagValue);
				bag.SetValue+=new PropertySpecEventHandler(SetPropertyBagValue);
				bag.Properties.Add(new PropertySpec("AutomataPulse",typeof(int),"Automata","Gets or sets time interval of the automata update pulse.",10));
				bag.Properties.Add(new PropertySpec("BackgroundColor",typeof(Color),"Appearance","Gets or sets the background color of the canvas.",Color.SteelBlue));
				bag.Properties.Add(new PropertySpec("BackgroundType",typeof(CanvasBackgroundTypes),"Appearance","Gets or sets the kind of background the canvas has.",CanvasBackgroundTypes.FlatColor));
				bag.Properties.Add(new PropertySpec("BackgroundImagePath",typeof(string),"Appearance","Gets or sets the path to the background image (only visible or used if the background type is 'Image').",""));
				bag.Properties.Add(new PropertySpec("DefaultConnectionPath",typeof(string),"Graph","Gets or sets the default path style or shape of the newly created connections.","Default"));
				bag.Properties.Add(new PropertySpec("DefaultLineEnd",typeof(ConnectionEnds),"Graph","Gets or sets the default line end of the newly created connections.",ConnectionEnds.NoEnds));
				bag.Properties.Add(new PropertySpec("EnableContextMenu",typeof(bool),"Appearance","Gets or sets whether the context menu is visible.",true));
				bag.Properties.Add(new PropertySpec("EnableLayout",typeof(bool),"Graph","Gets or sets whether layout algorithms are active.",false));
				bag.Properties.Add(new PropertySpec("GradientBottom",typeof(Color),"Appearance","Gets or sets the lower color of the gradient.",Color.SteelBlue));
				bag.Properties.Add(new PropertySpec("GradientTop",typeof(Color),"Appearance","Gets or sets the upper color of the gradient.",Color.White));
				bag.Properties.Add(new PropertySpec("GraphLayoutAlgorithm",typeof(GraphLayoutAlgorithms),"Graph","The layout algorithm to be used.",GraphLayoutAlgorithms.SpringEmbedder));
				bag.Properties.Add(new PropertySpec("GridSize",typeof(int),"Appearance","Gets or sets the grid size.",20));				
				bag.Properties.Add(new PropertySpec("RestrictToCanvas",typeof(bool),"Appearance","Gets or sets wether the graph shapes should be kept inside the canvas frame or allowed to move/resize outside it.",true));
				bag.Properties.Add(new PropertySpec("ShowGrid",typeof(bool),"Appearance","Gets or sets whether the grid is visible.",false));
				bag.Properties.Add(new PropertySpec("Snap",typeof(bool),"Appearance","Gets or sets whether the graph elements mSnap to the grid.",false));
				bag.Properties.Add(new PropertySpec("AllowAddShape",typeof(bool),"Graph","Gets or sets whether shapes can be added to the graph.",true));
				bag.Properties.Add(new PropertySpec("AllowAddConnection",typeof(bool),"Graph","Gets or sets whether connections can be added to the graph.",true));
				bag.Properties.Add(new PropertySpec("AllowDeleteShape",typeof(bool),"Graph","Gets or sets whether shapes can be deleted from the graph.",true));
				bag.Properties.Add(new PropertySpec("AllowMoveShape",typeof(bool),"Graph","Gets or sets whether shapes can be moved.",true));
				//					PropertySpec spec=new PropertySpec("MDI children",typeof(string[]));
				//					spec.Attributes=new Attribute[]{new System.ComponentModel.ReadOnlyAttribute(true)};
				//					bag.Properties.Add(spec);
							
					
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.Message);
			}

		}
		
		#endregion


		#endregion

		
	}
}

