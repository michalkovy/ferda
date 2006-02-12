using System;
using System.Windows.Forms;
using System.Resources;
using System.Collections.Generic;
using System.Collections;
using System.Drawing;

using Ferda.FrontEnd.Menu;
using Ferda.ModulesManager;
using Ferda.ProjectManager;
using Ferda.Modules;
using Ferda.FrontEnd.Archive;
using Ferda.FrontEnd.NewBox;

using Netron.GraphLib.UI;
using Netron.GraphLib;

namespace Ferda.FrontEnd.Desktop
{
    /// <summary>
	/// One of the Graph controls for Ferda project, each control represents
	/// one view of the archive
	/// </summary>
	/// <remarks>
	/// Nevim presne, jak se budou nektere veci implementovat, protoze v
	/// tuto dobu jeste nevidim do Netronu do takove hloubky, abych presne
	/// vedel jak : <br/>
	/// musi se naimplementovat funkcionalita ohledne hybani krabicky a vice
	/// krabicek, dale by se mel udelat nejaky handler na to, jestli byla
	/// krabicka selektnuta... a korespondovat s Project Managerem
	/// </remarks>
	///<stereotype>control</stereotype>
    public class FerdaDesktop : GraphControl, IViewDisplayer, IBoxSelector
    {
        #region Private fields

        /// <summary>
        /// Manager of localization in the application
        /// </summary>
        protected ILocalizationManager localizationManager;
		private ProjectManager.View view;
        /// <summary>
        /// ProjectManager of the application
        /// </summary>
        protected ProjectManager.ProjectManager projectManager;
		/// <summary>
		/// Archive of the application
		/// </summary>
        protected Archive.IArchiveDisplayer archiveDisplayer;
        private Properties.IPropertiesDisplayer propertiesDisplayer;
        private ContextHelp.IContextHelpDisplayer contextHelpDisplayer;
        private SVGManager svgManager;
        /// <summary>
        /// Menu of the application
        /// </summary>
        protected IMenuDisplayer menuDisplayer;
        /// <summary>
        /// Toolbar of the application
        /// </summary>
        protected IMenuDisplayer toolBar;
        private IFerdaClipboard clipboard;
        private UserNote.IUserNoteDisplayer userNote;

        //All the views of the project
        private List<FerdaDesktop> views;

        //Resource manager from the FerdaForm
        private ResourceManager resManager;

        //selected boxes
        private List<ModulesManager.IBoxModule> selectedBoxes = new List<ModulesManager.IBoxModule>();

        ///<summary>
        ///provider of the icons in the context menu
        ///</summary>
        protected IIconProvider provider;

        #endregion

        #region Properties

        /// <summary>
        /// SVGManager of the Desktop, it should be the global SVGManager
        /// of the whole application
        /// </summary>
        public SVGManager SvgManager
        {
            get
            {
                return svgManager;
            }
        }

        /// <summary>
        /// Resource manager of the application, it is filled according to the
        /// current localization
        /// </summary>
        public ResourceManager ResManager
        {
            set
            {
                resManager = value;
            }
            get
            {
                if (resManager == null)
                {
                    throw new ApplicationException(
                        "Desktop.ResManager cannot be null");
                }
                return resManager;
            }
        }

        /// <summary>
        /// Clipboard to store nodes
        /// </summary>
        public IFerdaClipboard Clipboard
        {
            get
            {
                return clipboard;
            }
            set
            {
                clipboard = value;
            }
        }

        /// <summary>
        /// Properties displayer of the application
        /// </summary>
        public Properties.IPropertiesDisplayer PropertiesDisplayer
        {
            set
            {
                propertiesDisplayer = value;
            }
            get
            {
                return propertiesDisplayer;
            }
        }

        /// <summary>
        /// Context help displayer of the application
        /// </summary>
        public ContextHelp.IContextHelpDisplayer ContextHelpDisplayer
        {
            set
            {
                contextHelpDisplayer = value;
            }
            get
            {
                return contextHelpDisplayer;
            }
        }

        /// <summary>
        /// All the views of the project
        /// </summary>
        public List<FerdaDesktop> Views
        {
            get
            {
                return views;
            }
            set
            {
                views = value;
            }
        }

        /// <summary>
        /// The desktop can adapt this interface to display a user note of a box
        /// </summary>
        public UserNote.IUserNoteDisplayer UserNote
        {
            set
            {
                userNote = value;
            }
            get
            {
                return userNote;
            }
        }

        #region IViewDisplayer implementation

        ///<summary>
		///Determines which boxes are selected in the control
		///</summary>
		public List <IBoxModule> SelectedBoxes
		{
            get
            {
                return selectedBoxes;
            }
		}

		///<summary>
		///Determines, wheather only one box is selected in the control
		///(and thus all the context menu of box module actions ... can
		///be displayed)
		///</summary>
		public bool IsBoxSelected
		{
            get
            {
                if (selectedBoxes.Count == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Determines, if there is more than one box selected.
        /// It is used to determine, if a contextMenu for box
        /// or for blank should be used
        /// </summary>
        public bool AreMoreBoxesSelected
        {
            get
            {
                if (selectedBoxes.Count > 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Context menu for the edit part of the main menu
        /// </summary>
        public ContextMenuStrip EditMenu
        {
            get
            {
                //a new context menu
                ContextMenuStrip cMenu;

                if (IsBoxSelected)
                {
                    if (AreMoreBoxesSelected)
                    {
                        cMenu = CreateBlankContextMenu();
                    }
                    else
                    {
                        cMenu = CreateBoxContextMenu(SelectedBoxes[0]);
                    }
                }
                else
                {
                    if (typeof(Netron.GraphLib.Connection).IsInstanceOfType(Hover))
                    {
                        cMenu = CreateConnectionContextMenu();
                    }
                    else //Creating context menu for the blank view underneath
                    {
                        cMenu = CreateBlankContextMenu();
                    }
                }

                return cMenu;
            }
        }

        /// <summary>
        /// View of the desktop (to run modules asking for creation)
        /// </summary>
        public ProjectManager.View View
        {
            get
            {
                return view;
            }
        }

        #endregion

        #endregion

        #region Constructor

        ///<summary>
		/// Default constructor for FerdaView class.
		///</summary>
        ///<param name="locManager">
        /// Interface that takes care of the 
        ///localizacion in the application
        /// </param>
        ///<param name="svgMan">
        /// Interface for providing SVG graphics
        ///</param>
        ///<param name="menuDisp">Menu displayer</param>
        ///<param name="view">The view thath should be connected to this desktop</param>
        ///<param name="pm">The project manager of the application</param>
        ///<param name="arch">Control that displays the archive</param>
        ///<param name="provider">Provider of the icons</param>
        ///<param name="toolBar">Toolbar control</param>
		public FerdaDesktop(Menu.ILocalizationManager locManager,
            SVGManager svgMan, IMenuDisplayer menuDisp, ProjectManager.View view,
            ProjectManager.ProjectManager pm, Archive.IArchiveDisplayer arch, 
            IIconProvider provider, IMenuDisplayer toolBar)
            : base()
		{
            //setting the icon
            this.provider = provider;

            //adding the localization
            localizationManager = locManager;
            ResManager = localizationManager.ResManager;

            //adding the svgManager
            svgManager = svgMan;

            //setting the menu displayer
            menuDisplayer = menuDisp;
            archiveDisplayer = arch;
            this.toolBar = toolBar;

            //Current view
            this.view = view;
            projectManager = pm;

            //name of the desktop
            Name = view.Name;

            //properties od the GraphControl to work propertly
            AllowMoveShape = true;
            AllowAddConnection = true;
            AllowAddShape = true;
            AllowDeleteShape = true;
            AllowDrop = true;
            AutoScroll = true;
            EnableContextMenu = true;
            EnableLayout = false;
            ShowGrid = false;
            Zoom = 1;
            RestrictToCanvas = false;
            this.ImeMode = System.Windows.Forms.ImeMode.On;

            //for now trial stuff
            BackgroundType = Netron.GraphLib.CanvasBackgroundTypes.Gradient;
            GradientTop = System.Drawing.Color.SkyBlue;

            //EventHandlers
            GotFocus += new EventHandler(FerdaDesktop_GotFocus);
            OnContextMenu += new MouseEventHandler(FerdaDesktop_ContextMenu);
            OnNewConnection += new NewConnection(FerdaDesktop_OnNewConnection);
            KeyPress += new KeyPressEventHandler(FerdaDesktop_KeyPress);
            OnFerdaMouseUp += new MouseEventHandler(FerdaDesktop_OnFerdaMouseUp);
            OnFerdaDeleteConnection += new FerdaConnection(FerdaDesktop_OnFerdaDeleteConnection);
            OnShapeRemoved += new NewShape(FerdaDesktop_OnShapeRemoved);

            //Setting the arrow for connections
            DefaultLineEnd = ConnectionEnds.RightFilledArrow;

            //Creation of the boxes and connections
            Adapt();
        }

        #endregion

        #region IViewDisplayer method implementation

        ///<summary>
        ///Redraws all shapes and connections according to a new state in
        ///the view object
        ///</summary>
        ///<remarks>
        ///Dont know if the control will remember previous shapes and connections
        ///and ask for the changed or write the entire structure over again...
        ///</remarks>
        public void Adapt()
        {
            Nodes.Clear();
            selectedBoxes.Clear();

            //We have to remove the handler because it would create a connection
            //that is already there
            OnNewConnection -= new NewConnection(FerdaDesktop_OnNewConnection);

            //adding the boxes on the desktop
            foreach (IBoxModule box in view.Boxes)
            {
                BoxNode boxNode = new BoxNode(this, box, SvgManager, view, ResManager);
                boxNode.X = view.GetPosition(box).X;
                boxNode.Y = view.GetPosition(box).Y;

                Nodes.Add(boxNode);
            }

            //adding the connections on the desktop
            foreach (ProjectManager.Connection con in view.Connections)
            {
                //finding the from and to connectors
                Connector from = FromConnector(con.FromBox);
                Connector to = ToConnector(con.ToBox, con.ToSocket);

                //adding the connector
                AddEdge(from, to);
            }

            OnNewConnection += new NewConnection(FerdaDesktop_OnNewConnection);
        }

        ///<summary>
        ///The method retrieves locations of all the boxes from the view
        ///and updates their locations in the View
        ///</summary>
        public void SetLocations()
        {
            foreach (BoxNode node in Nodes)
            {
                view.SetPosition(node.Box, new PointF(node.X, node.Y));
            }
        }

        ///<summary>
        ///This function is called when the localization
        ///of the application is changed - the whole menu needs to be redrawn
        ///</summary>
        public void ChangeLocalization()
        {
            //updating the resource manager
            ResManager = localizationManager.ResManager;

            Adapt();
        }

        /// <summary>
        /// When a box is selected in the archive, it should also be selected on the 
        /// view. This function selects the box in the desktop
        /// </summary>
        /// <param name="box">Box to be selected</param>
        public void SelectBox(IBoxModule box)
        {
            selectedBoxes.Clear();
            SelectedShapes.Clear();

            foreach (BoxNode bn in Nodes)
            {
                if (bn.Box == box)
                {
                    bn.Select(true);
                    selectedBoxes.Add(box);
                }
                else
                {
                    bn.Select(false);
                }
            }

            Refresh();
        }

        /// <summary>
        /// This function is called by the property grid when a property is changed -
        /// that can only mean the change of the user name of the box.
        /// </summary>
        public void RefreshBoxNames()
        {
            foreach (BoxNode bn in Nodes)
            {
                if (!View.ContainsBox(bn.Box))
                {
                    throw new ApplicationException("Box on the desktop is not contained in the view");
                }
                else
                {
                    bn.RefreshText();
                }
            }
        }

        /// <summary>
        /// Because there are problems with sharing the clicking actions on the menu
        /// with other controls (ToolBox), this method raises the action that was
        /// clicked on the toolbar
        /// </summary>
        /// <param name="sender">sender of the method</param>
        public void RaiseToolBarAction(object sender)
        {
            ToolStripButton bn = sender as ToolStripButton;
            if (bn == null)
            {
                throw new ApplicationException("Unexpected toolbar caller");
            }

            //we cannot use switch, because there is not a constant expression
            if (bn.ToolTipText == ResManager.GetString("DesktopLayout"))
            {
                LayoutCore();
                return;
            }

            if (bn.ToolTipText == ResManager.GetString("MenuEditRename"))
            {
                RenameCore();
                return;
            }

            if (bn.ToolTipText == ResManager.GetString("MenuEditCopy"))
            {
                CopyCore();
                return;
            }

            if (bn.ToolTipText == ResManager.GetString("MakeGroup"))
            {
                MakeGroupCore();
                return;
            }

            if (bn.ToolTipText == ResManager.GetString("MenuEditClone"))
            {
                CloneCore();
                return;
            }

            if (bn.ToolTipText == ResManager.GetString("MenuEditDeleteFromDesktop"))
            {
                DeleteFromDesktopCore();
                return;
            }

            if (bn.ToolTipText == ResManager.GetString("MenuEditDeleteFromArchive"))
            {
                DeleteFromArchiveCore();
                return;
            }

            if (bn.ToolTipText == ResManager.GetString("MenuEditLocalizeInArchive"))
            {
                LocalizeInArchiveCore();
                return;
            }

            if (bn.ToolTipText == ResManager.GetString("MenuEditPaste"))
            {
                PasteCore();
                return;
            }

            if (bn.ToolTipText == ResManager.GetString("MenuEditPackAllSockets"))
            {
                PackAllSocketsCore();
                return;
            }

            if (bn.ToolTipText == ResManager.GetString("MenuEditUnpackOneLayerAllSockets"))
            {
                UnpackOneLayerAllSocketsCore();
                return;
            }

            if (bn.ToolTipText == ResManager.GetString("MenuEditUnpackAllLayersAllSockets"))
            {
                UnpackAllLayersAllSocketsCore();
                return;
            }
        }

        /// <summary>
        /// Removes a box from the desktop
        /// </summary>
        /// <param name="box">IBoxmodule to be removed</param>
        public void RemoveBox(IBoxModule box)
        {
            List<BoxNode> toRemove = new List<BoxNode>();

            foreach (BoxNode node in Nodes)
            {
                if (node.Box == box)
                {
                    toRemove.Add(node);
                }
            }

            foreach (BoxNode node in toRemove)
            {
                RemoveShape(node);
            }
            SelectedBoxes.Clear();
        }

        /// <summary>
        /// Adds a box to the desktop
        /// </summary>
        /// <param name="box">Box to be added</param>
        public void AddBox(IBoxModule box)
        {
            //Adapt();
            BoxNode boxNode = new BoxNode(this, box, SvgManager, view, ResManager);
            boxNode.X = view.GetPosition(box).X;
            boxNode.Y = view.GetPosition(box).Y;

            //adding the box
            Nodes.Add(boxNode);
        }

        #endregion

        #region Event cores

        /// <summary>
        /// Provides core for the layout event
        /// </summary>
        protected void LayoutCore()
        {
            //this.StartLayout();
            view.Relayout();
            Adapt();
        }

        /// <summary>
        /// Provides core for the copy event
        /// </summary>
        protected void CopyCore()
        {
            Clipboard.Nodes.Clear();

            foreach (BoxNode bn in SelectedShapes)
            {
                Clipboard.Nodes.Add(bn.Box);
            }

            archiveDisplayer.Adapt();
        }

        /// <summary>
        /// Provides core for the clone event
        /// </summary>
        protected void CloneCore()
        {
            foreach (BoxNode bn in SelectedShapes)
            {
                IBoxModule box = bn.Box;
                IBoxModule newBox = projectManager.Archive.Clone(box);

                view.Add(newBox);
            }

            Adapt();
            archiveDisplayer.Adapt();
            propertiesDisplayer.Reset();
        }

        /// <summary>
        /// Provides core for the delete from desktop event
        /// </summary>
        protected void DeleteFromDesktopCore()
        {
            foreach (BoxNode bn in SelectedShapes)
            {
                IBoxModule box = bn.Box;
                view.Remove(box);
            }

            Adapt();
            archiveDisplayer.Adapt();
            propertiesDisplayer.Reset();
        }

        /// <summary>
        /// Provides core for the delete from archive event
        /// </summary>
        protected void DeleteFromArchiveCore()
        {
            if (!this.Focused)
            {
                return;
            }

            string caption = ResManager.GetString("DeleteFromArchiveCaption");
            string message = ResManager.GetString("DeleteFromArchiveMessage");

            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            // Displays the MessageBox.
            result = MessageBox.Show(this, message, caption, buttons,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

            if (result == DialogResult.Yes)
            {
                //deleting the nodes from the archive
                foreach (BoxNode bn in SelectedShapes)
                {
                    IBoxModule box = bn.Box;
                    projectManager.Archive.Remove(box);
                }
                //adapts all the views - we dont need to redraw them
                foreach (IViewDisplayer desktop in Views)
                {
                    if (desktop != this)
                    {
                        foreach (BoxNode bn in SelectedShapes)
                        {
                            desktop.RemoveBox(bn.Box);
                        }
                    }
                }

                RemoveBoxes(SelectedShapes);

                //adapts the archive displayer
                archiveDisplayer.Adapt();
                propertiesDisplayer.Reset();
            }
        }

        /// <summary>
        /// Provides core for the localize in archive event
        /// </summary>
        protected void LocalizeInArchiveCore()
        {
            IBoxModule box;
            if (Hover == null)
            {
                box = SelectedBoxes[0];
            }
            else
            {
                BoxNode bn = Hover as BoxNode;
                box = bn.Box;
            }

            archiveDisplayer.LocalizeInArchive(box);
        }

        /// <summary>
        /// Provides core for the paste event
        /// </summary>
        protected void PasteCore()
        {
            foreach (IBoxModule box in Clipboard.Nodes)
            {
                if (view.ContainsBox(box))
                {
                    IBoxModule newBox = projectManager.Archive.Clone(box);
                    view.Add(newBox);
                }
                else
                {
                    view.Add(box);
                }
            }

            Adapt();
            archiveDisplayer.Adapt();
            propertiesDisplayer.Reset();
        }

        /// <summary>
        /// Provides core for the Pack all sockets event
        /// </summary>
        protected void PackAllSocketsCore()
        {
            IBoxModule box;
            if (Hover == null)
            {
                box = SelectedBoxes[0];
            }
            else
            {
                BoxNode bn = Hover as BoxNode;
                box = bn.Box;
            }

            view.PackAllSockets(box);
            Adapt();
        }

        /// <summary>
        /// Provides core for the unpack one layer all sockets event
        /// </summary>
        protected void UnpackOneLayerAllSocketsCore()
        {
            IBoxModule box;
            if (Hover == null)
            {
                box = SelectedBoxes[0];
            }
            else
            {
                BoxNode bn = Hover as BoxNode;
                box = bn.Box;
            }


            view.UnpackOneLayerAllSockets(box);
            Adapt();
        }

        /// <summary>
        /// Provides core for the Unpack all layers all sockets event
        /// </summary>
        protected void UnpackAllLayersAllSocketsCore()
        {
            IBoxModule box;
            if (Hover == null)
            {
                box = SelectedBoxes[0];
            }
            else
            {
                BoxNode bn = Hover as BoxNode;
                box = bn.Box;
            }

            view.UnpackAllLayersAllSockets(box);
            Adapt();
        }

        /// <summary>
        /// Provides core for the rename event
        /// </summary>
        protected void RenameCore()
        {
            IBoxModule box;
            if (Hover == null)
            {
                box = SelectedBoxes[0];
            }
            else
            {
                BoxNode bn = Hover as BoxNode;
                box= bn.Box;
            }

            RenameDialog renameDialog = new RenameDialog(box.UserName, ResManager);

            renameDialog.ShowDialog();
            if (renameDialog.DialogResult == DialogResult.OK)
            {
                if (box.TryWriteEnter())
                {
                    Text = renameDialog.NewName;
                    box.UserName = renameDialog.NewName;
                    box.WriteExit();

                    //refresh all the views
                    foreach (Desktop.IViewDisplayer view in Views)
                    {
                        view.Adapt();
                    }
                    archiveDisplayer.RefreshBoxNames();

                    //forcing the propertygrid to adapt
                    PropertiesDisplayer.SelectedBox = box;
                    PropertiesDisplayer.IsOneBoxSelected = true;
                    PropertiesDisplayer.Adapt();

                    //here we dont have to adapt the user note, the box 
                    //remains the same
                }
                else
                {
                    //set a message that it cannot happen
                    FrontEndCommon.CannotWriteToBox(box, ResManager);
                }
            }
        }

        /// <summary>
        /// Makes a group box out of selected boxes
        /// </summary>
        protected void MakeGroupCore()
        {
            //getting the creator for the new group box (identifier "group")
            IBoxModuleFactoryCreator creator =
                projectManager.ModulesManager.GetBoxModuleFactoryCreator("group");

            if (creator == null)
            {
                throw new ApplicationException("Wrong group creator identifier");
            }

            //getting the IBoxModule
            IBoxModule box = creator.CreateBoxModule();

            //placing the IBoxModule on the current view
            projectManager.Archive.Add(box);
            view.Add(box, new PointF(SelectedShapes[0].X + SelectedShapes[0].Width * 2, SelectedShapes[0].Y));

            //connecting the selected boxes into the new group box
            foreach (IBoxModule fromBox in SelectedBoxes)
            {
                if (box.TryWriteEnter())
                {
                    try
                    {
                        //the group box has only 1 socket
                        box.SetConnection(box.Sockets[0].name, fromBox);
                    }
                    catch (BadTypeError)
                    {
                        //showing the messageBox
                        MessageBox.Show(ResManager.GetString("DesktopBadConnectionInvalidType"),
                            ResManager.GetString("DesktopBadConnectionCaption"));
                        box.WriteExit();
                        return;
                    }
                    catch (ConnectionExistsError)
                    {
                        //showing the messageBox
                        MessageBox.Show(ResManager.GetString("DesktopMoreThanOneErrorText"),
                            ResManager.GetString("DesktopBadConnectionCaption"));
                        box.WriteExit();
                        return;
                    }
                    box.WriteExit();
                }
                else
                {
                    FrontEndCommon.CannotWriteToBox(box, ResManager);
                }
            }

            //if there is a box that all the selected boxes are connected
            //into, we should connect also the new group box into that
            //box


            //after all is connected, we can hide the boxes from the view
            foreach (IBoxModule fromBox in SelectedBoxes)
            {
                view.Remove(fromBox);
            }

            //setting the user name of the group box to be the name of the first
            //box selected and then three dots
            if (box.TryWriteEnter())
            {
                box.UserName = selectedBoxes[0].UserName + "...";
                box.WriteExit();
            }
            else
            {
                FrontEndCommon.CannotWriteToBox(box, ResManager);
            }

            //TODO - pri pridavani krabicky je to tady strasne neefektivni,
            //lepsi by bylo, kdyby se vytvoril rovnou aj node, ktery by se pridal
            Adapt();
            archiveDisplayer.Adapt();
        }

        #endregion

        #region Other methods

        /// <summary>
        /// Function that returns the output connector(graphical) that is
        /// assinged to the box in parameter.
        /// </summary>
        /// <param name="box">Box whose output connector we want to know</param>
        /// <returns>output connector of the box</returns>
        protected Connector FromConnector(IBoxModule box)
        {
            int i;
            BoxNode b;
            bool found = false;

            //iterating through the nodes to find the one that has the box in
            //parameter
            for (i = 0; i < Nodes.Count; i++)
            {
                BoxNode bn = Nodes[i] as BoxNode;
                if (bn.Box == box)
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                throw new ApplicationException("Connector not found");
            }

            //return the output connector
            b = Nodes[i] as BoxNode;
            return b.OutputConnector;
        }

        /// <summary>
        /// Function that returns the input connector that is determined
        /// by a box and a name of the socket.
        /// </summary>
        /// <param name="box">Box whose input connector we want to know</param>
        /// <param name="socketName">Name of the socket</param>
        /// <returns>Input connector of the specified box and name</returns>
        protected Connector ToConnector(IBoxModule box, string socketName)
        {
            BoxNode b;
            int i;
            bool found = false;

            //interating through nodes to find the node with box from the
            //parameter
            for (i = 0; i < Nodes.Count; i++)
            {
                BoxNode bn = Nodes[i] as BoxNode;
                if (bn.Box == box)
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                throw new ApplicationException("Connector not found");
            }
            found = false;

            b = Nodes[i] as BoxNode;

            //finding the input connector specified with text socketname
            for (i = 0; i < b.InputConnectors.Count; i++)
            {
                FerdaConnector fc = b.InputConnectors[i] as FerdaConnector;
                if (fc.Socket.name == socketName)
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                throw new ApplicationException("Connector not found");
            }

            return b.InputConnectors[i];
        }

        /// <summary>
        /// Creates a context menu over blank view (no socket or box is selected)
        /// </summary>
        protected ContextMenuStrip CreateBlankContextMenu()
        {
            ContextMenuStrip cMenu = new ContextMenuStrip();

            ToolStripMenuItem layout = new ToolStripMenuItem(ResManager.GetString("DesktopLayout"));
            layout.Click += new EventHandler(layout_Click);
            layout.Image = provider.GetIcon("Layout").ToBitmap();
            cMenu.Items.Add(layout);

            //if there are boxes selected, we can add copy, clone, delete from archive and
            //delete from desktop  and hide to the menu
            if (IsBoxSelected)
            {
                ToolStripMenuItem copy = new ToolStripMenuItem(ResManager.GetString("MenuEditCopy"));
                copy.ShortcutKeys = (Keys)Shortcut.CtrlC;
                copy.Click += new EventHandler(copy_Click);
                copy.Image = provider.GetIcon("Copy").ToBitmap();
                ToolStripMenuItem clone = new ToolStripMenuItem(ResManager.GetString("MenuEditClone"));
                clone.ShortcutKeys = (Keys)Shortcut.CtrlE;
                clone.Click += new EventHandler(clone_Click);
                clone.Image = provider.GetIcon("Clone").ToBitmap();

                ToolStripMenuItem makeGroup = new ToolStripMenuItem(ResManager.GetString("MakeGroup"));
                makeGroup.ShortcutKeys = (Keys)Shortcut.CtrlG;
                makeGroup.Image = provider.GetIcon("MakeGroup").ToBitmap();
                makeGroup.Click += new EventHandler(makeGroup_Click);

                ToolStripSeparator sep = new ToolStripSeparator();
                cMenu.Items.Add(copy);
                cMenu.Items.Add(clone);
                cMenu.Items.Add(sep);
                cMenu.Items.Add(makeGroup);

                ToolStripMenuItem deleteFromDesktop =
                    new ToolStripMenuItem(ResManager.GetString("MenuEditDeleteFromDesktop"));
                deleteFromDesktop.ShortcutKeys = (Keys)Shortcut.CtrlD;
                deleteFromDesktop.Image = provider.GetIcon("DeleteFromDesktop").ToBitmap();
                deleteFromDesktop.Click += new EventHandler(deleteFromDesktop_Click);

                ToolStripMenuItem deleteFromArchive =
                    new ToolStripMenuItem(ResManager.GetString("MenuEditDeleteFromArchive"));
                deleteFromArchive.ShortcutKeys = (Keys)Shortcut.ShiftDel;
                deleteFromArchive.Image = provider.GetIcon("DeleteFromArchive").ToBitmap();
                deleteFromArchive.Click += new EventHandler(deleteFromArchive_Click);

                //ToolStripSeparator sep2 = new ToolStripSeparator();
                ToolStripMenuItem localizeInArchive =
                    new ToolStripMenuItem(ResManager.GetString("MenuEditLocalizeInArchive"));
                localizeInArchive.Image = provider.GetIcon("LocalizeInArchive").ToBitmap();
                localizeInArchive.ShortcutKeys = (Keys)Shortcut.CtrlL;

                cMenu.Items.AddRange(new ToolStripItem[] { deleteFromDesktop, deleteFromArchive });
            }


            if (!Clipboard.IsEmpty)
            {
                ToolStripMenuItem paste = new ToolStripMenuItem(ResManager.GetString("MenuEditPaste"));
                paste.ShortcutKeys = (Keys)Shortcut.CtrlV;
                paste.Click += new EventHandler(paste_Click);
                paste.Image = provider.GetIcon("Paste").ToBitmap();
                cMenu.Items.Add(paste);
            }

            return cMenu;
        }

        /// <summary>
        /// Creates a context menu when some socket is selected
        /// </summary>
        protected ContextMenuStrip CreateSocketContextMenu(Netron.GraphLib.Entity connector)
        {
            ContextMenuStrip cMenu = new ContextMenuStrip();

            if (connector is FerdaConnector)
            {
                FerdaConnector fc = connector as FerdaConnector;

                if (fc.HasPacked)
                {
                    ToolStripMenuItem unpackOneL = new ToolStripMenuItem(ResManager.GetString("MenuEditUnpackOneLayer"));
                    unpackOneL.Click += new EventHandler(unpackOneL_Click);
                    unpackOneL.Image = provider.GetIcon("UnpackSocketOneLayer").ToBitmap();
                    cMenu.Items.Add(unpackOneL);
                    ToolStripMenuItem unpackAllL = new ToolStripMenuItem(ResManager.GetString("MenuEditUnpackAllLayers"));
                    unpackAllL.Click += new EventHandler(unpackAllL_Click);
                    unpackAllL.Image = provider.GetIcon("UnpackSocketAllLayers").ToBitmap();
                    cMenu.Items.Add(unpackAllL);
                }
                else
                {
                    ToolStripMenuItem packSocket = new ToolStripMenuItem(ResManager.GetString("MenuEditPackSocket"));
                    packSocket.Click += new EventHandler(packSocket_Click);
                    packSocket.Image = provider.GetIcon("PackSocket").ToBitmap();
                    cMenu.Items.Add(packSocket);
                }
            }

            return cMenu;
        }

        /// <summary>
        /// Creates actions submenu for a selected box
        /// </summary>
        protected void CreateActionsMenu(ToolStripMenuItem act, IBoxModule box)
        {
            ToolStripMenuItem item;

            List<ToolStripMenuItem> actions = new List<ToolStripMenuItem>();

            foreach (ActionInfo info in box.MadeInCreator.Actions)
            {
                item = new ToolStripMenuItem(info.label);
                item.Click += new EventHandler(Actions_Click);
                item.Enabled = box.IsPossibleToRunAction(info.name);
                actions.Add(item);
            }

            foreach (ToolStripMenuItem i in actions)
            {
                act.DropDownItems.Add(i);
            }
        }

        /// <summary>
        /// Creates ModulesForInteraction submenu for a selected box
        /// </summary>
        protected void CreateModulesForInteractionMenu(ToolStripMenuItem mod, IBoxModule box)
        {
            ToolStripMenuItem item;

            List<ToolStripMenuItem> modules = new List<ToolStripMenuItem>();

            foreach (ModuleForInteractionInfo info in box.ModuleForInteractionInfos)
            {
                item = new ToolStripMenuItem(info.Label);
                item.Click += new EventHandler(Interaction_Click);
                item.Enabled = box.IsPossibleToRunModuleForInteraction(info.IceIdentity);
                modules.Add(item);
            }

            foreach (ToolStripMenuItem it in modules)
            {
                mod.DropDownItems.Add(it);
            }
        }

        /// <summary>
        /// Creates ModulesForCreationSubmenu for a selected box
        /// </summary>
        protected void CreateModulesForCreationMenu(ToolStripMenuItem mod, IBoxModule box)
        {
            ToolStripMenuItem it;

            List<ToolStripMenuItem> modules = new List<ToolStripMenuItem>();

            foreach (ModulesAskingForCreation info in box.ModulesAskingForCreation)
            {
                it = new ToolStripMenuItem(info.label);
                it.Click += new EventHandler(Creation_Click);

                modules.Add(it);
            }

            foreach (ToolStripMenuItem item in modules)
            {
                mod.DropDownItems.Add(item);
            }
        }

        /// <summary>
        /// Creates a context menu when a box is selected
        /// </summary>
        protected ContextMenuStrip CreateBoxContextMenu(IBoxModule box)
        {
            bool canPack = false;
            bool canUnpack = false;
            bool containsDynamic = false;

            ContextMenuStrip cMenu = new ContextMenuStrip();

            //creating dynamic parts of the menu
            if (box.MadeInCreator.Actions.Length != 0)
            {
                containsDynamic = true;

                ToolStripMenuItem actions =
                    new ToolStripMenuItem(ResManager.GetString("MenuActions"));
                CreateActionsMenu(actions, box);
                cMenu.Items.Add(actions);
            }

            if (box.ModuleForInteractionInfos.Length != 0)
            {
                containsDynamic = true;

                ToolStripMenuItem modulesForInteraction =
                    new ToolStripMenuItem(ResManager.GetString("MenuModulesForInteraction"));
                CreateModulesForInteractionMenu(modulesForInteraction, box);
                cMenu.Items.Add(modulesForInteraction);
            }

            if (box.ModulesAskingForCreation.Length != 0)
            {
                containsDynamic = true;
                ToolStripMenuItem modulesForCreation =
                    new ToolStripMenuItem(ResManager.GetString("MenuModulesAskingCreation"));
                CreateModulesForCreationMenu(modulesForCreation, box);
                cMenu.Items.Add(modulesForCreation);
            }

            if (containsDynamic)
            {
                ToolStripSeparator sep0 = new ToolStripSeparator();
                cMenu.Items.Add(sep0);
            }

            //normal part of the menu
            ToolStripMenuItem rename = new ToolStripMenuItem(ResManager.GetString("MenuEditRename"));
            ToolStripMenuItem copy = new ToolStripMenuItem(ResManager.GetString("MenuEditCopy"));
            ToolStripMenuItem clone = new ToolStripMenuItem(ResManager.GetString("MenuEditClone"));
            ToolStripSeparator sep = new ToolStripSeparator();
            ToolStripMenuItem deleteFromDesktop =
                new ToolStripMenuItem(ResManager.GetString("MenuEditDeleteFromDesktop"));
            ToolStripMenuItem deleteFromArchive =
                new ToolStripMenuItem(ResManager.GetString("MenuEditDeleteFromArchive"));
            ToolStripSeparator sep2 = new ToolStripSeparator();
            ToolStripMenuItem localizeInArchive =
                new ToolStripMenuItem(ResManager.GetString("MenuEditLocalizeInArchive"));
            ToolStripMenuItem packAllSockets =
                new ToolStripMenuItem(ResManager.GetString("MenuEditPackAllSockets"));
            ToolStripMenuItem unpackOneLayerAllSockets =
                new ToolStripMenuItem(ResManager.GetString("MenuEditUnpackOneLayerAllSockets"));
            ToolStripMenuItem unpackAllLayersAllSockets =
                new ToolStripMenuItem(ResManager.GetString("MenuEditUnpackAllLayersAllSockets"));
            
            //determines if there is anything to pack unpack
            BoxNode node = null;
            foreach (BoxNode bc in Nodes)
            {
                if (bc.Box == box)
                {
                    node = bc;
                    break;
                }
            }

            if (node == null)
            {
                throw new ApplicationException("Node with selected box not found");
            }

            foreach (FerdaConnector fc in node.InputConnectors)
            {
                if (fc.HasPacked)
                {
                    canUnpack = true;
                }
                else
                {
                    canPack = true;
                }
            }

            //click handlers
            rename.Click += new EventHandler(rename_Click);
            copy.Click += new EventHandler(copy_Click);
            clone.Click += new EventHandler(clone_Click);
            deleteFromDesktop.Click += new EventHandler(deleteFromDesktop_Click);
            deleteFromArchive.Click += new EventHandler(deleteFromArchive_Click);
            packAllSockets.Click += new EventHandler(packAllSockets_Click);
            unpackOneLayerAllSockets.Click += new EventHandler(unpackOneLayerAllSockets_Click);
            unpackAllLayersAllSockets.Click += new EventHandler(unpackAllLayersAllSockets_Click);
            localizeInArchive.Click += new EventHandler(localizeInArchive_Click);

            //shortcuts
            rename.ShortcutKeys = Keys.F2;
            copy.ShortcutKeys = (Keys)Shortcut.CtrlC;
            clone.ShortcutKeys = (Keys)Shortcut.CtrlE;
            deleteFromDesktop.ShortcutKeys = (Keys)Shortcut.CtrlD;
            deleteFromArchive.ShortcutKeys = (Keys)Shortcut.ShiftDel;
            packAllSockets.ShortcutKeys = (Keys)Shortcut.CtrlP;
            unpackAllLayersAllSockets.ShortcutKeys = (Keys)Shortcut.CtrlU;
            unpackOneLayerAllSockets.ShortcutKeys = (Keys)Shortcut.CtrlS;
            localizeInArchive.ShortcutKeys = (Keys)Shortcut.CtrlL;

            //icons
            rename.Image = provider.GetIcon("Rename").ToBitmap();
            copy.Image = provider.GetIcon("Copy").ToBitmap();
            clone.Image = provider.GetIcon("Clone").ToBitmap();
            deleteFromDesktop.Image = provider.GetIcon("DeleteFromDesktop").ToBitmap();
            deleteFromArchive.Image = provider.GetIcon("DeleteFromArchive").ToBitmap();
            packAllSockets.Image = provider.GetIcon("PackAllSockets").ToBitmap();
            unpackAllLayersAllSockets.Image = provider.GetIcon("UnpackAllLayersAllSockets").ToBitmap();
            unpackOneLayerAllSockets.Image = provider.GetIcon("UnpackAllLayersOneSocketAllSockets").ToBitmap();
            localizeInArchive.Image = provider.GetIcon("LocalizeInArchive").ToBitmap();

            //addding the makegroup item (if there are more boxes selected)
            if (SelectedBoxes.Count > 1)
            {
                ToolStripMenuItem makeGroup = new ToolStripMenuItem(ResManager.GetString("MakeGroup"));
                makeGroup.Image = provider.GetIcon("MakeGroup").ToBitmap();
                makeGroup.ShortcutKeys = Keys.F3;
                makeGroup.Click += new EventHandler(makeGroup_Click);

                //adding
                cMenu.Items.AddRange(new ToolStripItem[] {
                rename, copy, clone, sep, makeGroup, deleteFromDesktop,
                deleteFromArchive, sep2, localizeInArchive });
            }
            else
            {
                cMenu.Items.AddRange(new ToolStripItem[] {
                rename, copy, clone, sep, deleteFromDesktop,
                deleteFromArchive, sep2, localizeInArchive });
            }

            if (canUnpack)
            {
                cMenu.Items.Add(unpackOneLayerAllSockets);
                cMenu.Items.Add(unpackAllLayersAllSockets);
            }
            if (canPack)
            {
                cMenu.Items.Add(packAllSockets);
            }

            return cMenu;
        }

        /// <summary>
        /// Creates a context menu when a connection is selected
        /// </summary>
        protected ContextMenuStrip CreateConnectionContextMenu()
        {
            ContextMenuStrip cMenu = new ContextMenuStrip();

            ToolStripMenuItem delete =
                new ToolStripMenuItem(ResManager.GetString("DesktopDeleteConnection"));
            delete.Click += new EventHandler(deleteConnection_Click);

            cMenu.Items.Add(delete);

            return cMenu;
        }

        /// <summary>
        /// Updates the interface structures. This allows
        /// other controls to know what has been selected. Then notifies
        /// other controls
        /// </summary>
        protected void UpdateInterface()
        {
            //changing the cursor to the wait cursor
            Cursor previousCursor = this.Cursor;

            BoxNode bn;
            selectedBoxes.Clear();

            foreach (Shape sh in SelectedShapes)
            {
                bn = sh as BoxNode;
                selectedBoxes.Add(bn.Box);
            }

            if (SelectedBoxes.Count > 0)
            {
                Cursor = Cursors.WaitCursor;
            }

            menuDisplayer.Adapt();
            toolBar.Adapt();

            //sending info to the propertygrid and context help
            if ( SelectedBoxes.Count > 1 )
            {
                propertiesDisplayer.SelectedBoxes = SelectedBoxes;
                propertiesDisplayer.IsOneBoxSelected = false;
                propertiesDisplayer.Adapt();

                contextHelpDisplayer.Reset();
                userNote.Reset();
            }

            if (SelectedBoxes.Count == 1)
            {
                propertiesDisplayer.SelectedBox = SelectedBoxes[0];
                propertiesDisplayer.IsOneBoxSelected = true;
                propertiesDisplayer.Adapt();

                contextHelpDisplayer.SelectedBox = SelectedBoxes[0];
                contextHelpDisplayer.Adapt();

                userNote.SelectedBox = SelectedBoxes[0];
                userNote.Adapt();
            }

            if (SelectedBoxes.Count == 0)
            {
                propertiesDisplayer.Reset();
                contextHelpDisplayer.Reset();
                userNote.Reset();
            }

            Cursor = previousCursor;
        }

        /// <summary>
        /// Removes the box in the parameter from the desktop
        /// (used when deleting from archive)
        /// </summary>
        /// <param name="boxes">Boxes to be removed</param>
        /// <remarks>There no need to have a
        /// IBoxModule list in the argument, because
        /// this command can be only accessible from 
        /// the desktop (not from the archive)</remarks>
        protected void RemoveBoxes(ShapeCollection boxes)
        {
            foreach (BoxNode node in boxes)
            {
                RemoveShape(node);
            }
            SelectedBoxes.Clear();
        }

        #endregion

        #region Events

        /// <summary>
        /// Handles a keypress, updates selected boxes for the inteface. This allows
        /// other controls to know what has been selected.
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void FerdaDesktop_KeyPress(object sender, KeyPressEventArgs e)
        {
            UpdateInterface();
            SetLocations();
            archiveDisplayer.Adapt();
        }

        /// <summary>
        /// This is a special Ferda delegate event which was not originally in Netron.
        /// It was developed in order to notify the FerdaDesktop about the changes of
        /// selected boxes on the GraphControl and to adjust da menu
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void FerdaDesktop_OnFerdaMouseUp(object sender, MouseEventArgs e)
        {
            UpdateInterface();
            SetLocations();
        }

        /// <summary>
        /// Handles the creation of a new connection. It does not allow a connection
        /// from a intput connector to a output connector (it must be other way around)
        /// </summary>
        /// <param name="Sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        /// <returns>
        /// If true, the connection is added by the Netron library,
        /// otherwise no connection is added
        /// </returns>
        /// <remarks>
        /// Little trick is used with checking if the connection has the right direction
        /// (from output to input). Because the output connector in Ferda is always
        /// Connection type and the input connector is always FerdaConnector type, we can
        /// check it by is
        /// </remarks>
        protected bool FerdaDesktop_OnNewConnection(object Sender, ConnectionEventArgs e)
		{
            //checking if the connection has the right direction
            if (e.From is FerdaConnector)
            {
                //a message is throw to the user
                MessageBox.Show(ResManager.GetString("DesktopBadConnectionDescription"),
                    ResManager.GetString("DesktopBadConnectionCaption"));
                return false;
            }
            if (!(e.To is FerdaConnector))
            {
                //a message is throw to the user
                MessageBox.Show(ResManager.GetString("DesktopBadConnectionDescription"),
                    ResManager.GetString("DesktopBadConnectionCaption"));
                return false;
            }

            //I have the right connection and now I am checking the type consistency
            IBoxModule fromBox = ((BoxNode)e.From.BelongsTo).Box;
            IBoxModule toBox = ((BoxNode)e.To.BelongsTo).Box;
            Modules.SocketInfo toSocket = ((FerdaConnector)e.To).Socket;

            //checking the good socket Types
            Modules.BoxType [] goodBoxTypes = toSocket.socketType;

            bool contains = false;
            foreach (Modules.BoxType boxType in goodBoxTypes)
            {
                if (fromBox.HasBoxType(boxType))
                {
                    contains = true;
                    break;
                }
            }

            if (contains)
            {
                //adding the connection to the ProjectManager
                if (toBox.TryWriteEnter())
                {
                    try
                    {
                        toBox.SetConnection(toSocket.name, fromBox);
                    }
                    catch (BadTypeError)
                    {
                        //showing the messageBox
                        MessageBox.Show(ResManager.GetString("DesktopBadConnectionInvalidType"),
                            ResManager.GetString("DesktopBadConnectionCaption"));
                        toBox.WriteExit();
                        return false;
                    }
                    catch (ConnectionExistsError)
                    {
                        //showing the messageBox
                        MessageBox.Show(ResManager.GetString("DesktopMoreThanOneErrorText"),
                            ResManager.GetString("DesktopBadConnectionCaption"));
                        toBox.WriteExit();
                        return false;
                    }
                    toBox.WriteExit();
                }
                else
                {
                    FrontEndCommon.CannotWriteToBox(toBox, ResManager);
                }
            }
            else
            {
                //showing the messageBox
                MessageBox.Show(ResManager.GetString("DesktopBadConnectionInvalidType"),
                    ResManager.GetString("DesktopBadConnectionCaption"));
                return false;
            }

            archiveDisplayer.Adapt();
            return true;
		}

        /// <summary>
        /// Draws the context menu on the canvas. A .NET 2.0 ContextMenuStrip is used
        /// instead of the old ContextMenu
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        protected void FerdaDesktop_ContextMenu(object sender, MouseEventArgs e)
        {
            //must null the old context menu in order to display the menustrip.
            ContextMenu = null;

            //a new context menu
            ContextMenuStrip cMenu;

            if (typeof(Shape).IsInstanceOfType(Hover))
            {
                //Creating context menu for the box underneath
                BoxNode bn = Hover as BoxNode;
                //IBoxModule box = bn.Box;

                cMenu = CreateBoxContextMenu(bn.Box);
            }
            else
            {
                if (typeof(Connector).IsInstanceOfType(Hover))
                {
                    //Creating context menu for the socket underneath
                    cMenu = CreateSocketContextMenu(Hover);
                }
                else
                {
                    if (typeof(Netron.GraphLib.Connection).IsInstanceOfType(Hover))
                    {
                        cMenu = CreateConnectionContextMenu();
                    }
                    else //Creating context menu for the blank view underneath
                    {
                        cMenu = CreateBlankContextMenu();
                    }
                }
            }

            ContextMenuStrip = cMenu;
        }

        /// <summary>
        /// Event when the desktop receives focus. It forces the menu and toolbar
        /// to adapt
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        protected void FerdaDesktop_GotFocus(object sender, EventArgs e)
        {
            menuDisplayer.ControlHasFocus = this;
            menuDisplayer.Adapt();
            toolBar.ControlHasFocus = this;
            toolBar.Adapt();
        }

		///<summary>
		///Handles dropping event from the control
		///</summary>
		protected override void OnDragDrop(DragEventArgs drgevent)
		{
            IBoxModule box = null;
            IBoxModuleFactoryCreator creator = null;
            string text = ResManager.GetString("DesktopBoxAlreadyExistsText");
            string caption = ResManager.GetString("DesktopBoxAlreadyExistsCaption");

            //we are getting data from the NewBox control
            object o = drgevent.Data.GetData(typeof(NewBoxNode));

            //adjusting the coordinates
            PointF clientPoint =
                this.PointToClient(new Point(drgevent.X, drgevent.Y));

            if (o is NewBoxNode)
            {
                string boxName = String.Empty;

                IBoxModuleFactoryCreator[] creators =
                    projectManager.ModulesManager.BoxModuleFactoryCreators;

                //setting the name of the box
                foreach (IBoxModuleFactoryCreator cr in creators)
                {
                    if (((NewBoxNode)o).Identifier == cr.Identifier)
                    {
                        boxName = cr.Label;
                        creator = cr;
                        break;
                    }
                }

                //testing if a creator was found
                if (creator == null)
                {
                    throw new ApplicationException(
                        "Creator of a box was not found according to the identifier.");
                }
                box = creator.CreateBoxModule();

                projectManager.Archive.Add(box);
                view.Add(box, clientPoint);
                AddBox(box);
            }

            o = drgevent.Data.GetData(typeof(FerdaTreeNode));

            //we are getting the data from the archive
            if (o is FerdaTreeNode)
            {
                box = ((FerdaTreeNode)o).Box;

                if (!view.ContainsBox(box))
                {
                    view.Add(box, clientPoint);
                }
                else
                {
                    MessageBox.Show(text, caption);
                }
                Adapt();
            }

            archiveDisplayer.Adapt();
        }

        /// <summary>
        /// This is a special Ferda delegate event which was not originally in Netron.
        /// It was developed in order to notify the FerdaDesktop about that a connection
        /// is going to be deleted. This is the easiest way to do it, because
        /// GraphControl does not provide direct acces to the connections.
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="con">Information about the connection that has been deleted</param>
        void FerdaDesktop_OnFerdaDeleteConnection(object sender, Netron.GraphLib.Connection con)
        {
            try
            {
                BoxNode fromNode = con.From.BelongsTo as BoxNode;
                IBoxModule fromBox = fromNode.Box;

                IBoxModule toBox = ((BoxNode)con.To.BelongsTo).Box;
                SocketInfo toSocket = ((FerdaConnector)con.To).Socket;

                toBox.RemoveConnection(toSocket.name, fromBox);
                archiveDisplayer.Adapt();
            }
            catch (NameNotExistError exc)
            {
                //zatim
                MessageBox.Show(exc.Message);
            }
            catch (ConnectionNotExistError exc)
            {
                //zatim
                MessageBox.Show(exc.Message);
            }
        }

        /// <summary>
        /// This should delete all the connectors of the shape
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="shape">Shape that is being removed</param>
        void FerdaDesktop_OnShapeRemoved(object sender, Shape shape)
        {
            BoxNode bn = shape as BoxNode;
            if (bn != null)
            {
                //removing all the edges
                foreach (Connector connector in shape.Connectors)
                {
                    foreach (Netron.GraphLib.Connection connection in Edges)
                    {
                        if (connection.From == connector || connection.To == connector)
                        {
                            DeleteConnection(connection);
                        }
                    }
                }

                IBoxModule box = bn.Box;
                view.Remove(box);

                //removing the node
                Nodes.Remove(shape);
            }
        }

        #region Context menu events

        /// <summary>
        /// This is here to test the layout algoritm
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void layout_Click(object sender, EventArgs e)
        {
            LayoutCore();
        }

        /// <summary>
        /// Reaction on context item copy
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void copy_Click(object sender, EventArgs e)
        {
            CopyCore();
        }

        /// <summary>
        /// Deletes a connection. There should be only one connection selected
        /// in one while
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void deleteConnection_Click(object sender, EventArgs e)
        {
            OnDelete(sender, e);

            //adapts all the views
            foreach (FerdaDesktop desktop in Views)
            {
                desktop.Adapt();
            }
            archiveDisplayer.Adapt();
        }

        /// <summary>
        /// Packs a particular socket
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void packSocket_Click(object sender, EventArgs e)
        {
            if (typeof(FerdaConnector).IsInstanceOfType(Hover))
            {
                FerdaConnector fc = Hover as FerdaConnector;
                BoxNode bn = fc.BelongsTo as BoxNode;
                view.PackSocket(bn.Box, fc.Socket.name);

                Adapt();
            }
        }

        /// <summary>
        /// Unpacks all layers in a particular socket
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void unpackAllL_Click(object sender, EventArgs e)
        {
            if (typeof(FerdaConnector).IsInstanceOfType(Hover))
            {
                FerdaConnector fc = Hover as FerdaConnector;
                BoxNode bn = fc.BelongsTo as BoxNode;
                view.UnpackAllLayers(bn.Box, fc.Socket.name);

                Adapt();
            }
        }

        /// <summary>
        /// Unpacks one layer in a particular socket
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void unpackOneL_Click(object sender, EventArgs e)
        {
            if (typeof(FerdaConnector).IsInstanceOfType(Hover))
            {
                FerdaConnector fc = Hover as FerdaConnector;
                BoxNode bn = fc.BelongsTo as BoxNode;
                view.UnpackOneLayer(bn.Box, fc.Socket.name);

                Adapt();
            }
        }

        /// <summary>
        /// Reaction to clone event of the context menu
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void clone_Click(object sender, EventArgs e)
        {
            CloneCore();
        }

        /// <summary>
        /// Deletes the box from desktop
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void deleteFromDesktop_Click(object sender, EventArgs e)
        {
            DeleteFromDesktopCore();
        }

        /// <summary>
        /// Deletes the box from archive
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void deleteFromArchive_Click(object sender, EventArgs e)
        {
            DeleteFromArchiveCore();
        }

        /// <summary>
        /// Packs all sockets of a box
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void packAllSockets_Click(object sender, EventArgs e)
        {
            PackAllSocketsCore();
        }

        /// <summary>
        /// Reacts to a click for an action, triggers the action
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void Actions_Click(object sender, EventArgs e)
        {
            //this prevents executing anything when the event is not called from the
            //desktop (but from the main menu)
            if (Hover == null)
            {
                return;
            }

            BoxNode bn = Hover as BoxNode;
            IBoxModule box = bn.Box;

            //run the action
            foreach (ActionInfo info in box.MadeInCreator.Actions)
            {
                if (info.label == sender.ToString())
                {
                    ActionExceptionCatcher catcher = 
                        new ActionExceptionCatcher(projectManager, ResManager, this);
					box.RunAction_async(catcher, info.name);
                    break;
                }
            }

            //archiveDisplayer.Adapt(); myslim ze to tady nemusi byt
            Adapt();
        }

        /// <summary>
        /// Reacts to a click for a module asking for creation and adds the module
        /// to the view
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void Creation_Click(object sender, EventArgs e)
        {
            //this prevents executing anything when the event is not called from the
            //desktop (but from the main menu)
            if (Hover == null)
            {
                return;
            }

            BoxNode bn = Hover as BoxNode;
            IBoxModule box = bn.Box;

            foreach (ModulesAskingForCreation info in box.ModulesAskingForCreation)
            {
                if (info.label == sender.ToString())
                {
                    view.CreateBoxesAskingForCreation(info);
                    break;
                }
            }

            Adapt();
            archiveDisplayer.Adapt();
        }

        /// <summary>
        /// Reacts to a click for a module for interaction, triggers the module
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void Interaction_Click(object sender, EventArgs e)
        {
            //this prevents executing anything when the event is not called from the
            //desktop (but from the main menu)
            if (Hover == null)
            {
                return;
            }

            BoxNode bn = Hover as BoxNode;
            IBoxModule box = bn.Box;

            foreach (ModuleForInteractionInfo info in box.ModuleForInteractionInfos)
            {
                if (info.Label == sender.ToString())
                {
                    box.RunModuleForInteraction(info.IceIdentity);
                    break;
                }
            }

            //there is no need to adapt anything
            //Adapt();
            //menuDisplayer.Adapt();
        }

        /// <summary>
        /// Unpacks one layer of all sockets in the box
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void unpackOneLayerAllSockets_Click(object sender, EventArgs e)
        {
            UnpackOneLayerAllSocketsCore();
        }

        /// <summary>
        /// Unpacks all layers of all sockets in the box
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void unpackAllLayersAllSockets_Click(object sender, EventArgs e)
        {
            UnpackAllLayersAllSocketsCore();
        }

        /// <summary>
        /// Pastes the boxes in the clipboard into the view. If the box exists
        /// in the view, then it clones the box otherwise it adds the box to the view
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void paste_Click(object sender, EventArgs e)
        {
            PasteCore();
        }

        /// <summary>
        /// Displays the rename dialog and
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void rename_Click(object sender, EventArgs e)
        {
            RenameCore();
        }

        /// <summary>
        /// Localizes the box in the archive
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void localizeInArchive_Click(object sender, EventArgs e)
        {
            LocalizeInArchiveCore();
        }

        /// <summary>
        /// Makes a group box out of selected  boxes
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void makeGroup_Click(object sender, EventArgs e)
        {
            MakeGroupCore();
        }

        #endregion

        #endregion
    }
}
