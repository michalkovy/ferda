// FerdaForm.cs - main form of the application
//
// Author: Martin Ralbovský <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2005 Martin Ralbovský
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
using System;
using System.Threading;
using DockDotNET;
using System.Windows.Forms;
using Ferda.FrontEnd;
using Ferda.FrontEnd.AddIns;
using Ferda.ProjectManager;
using System.Resources;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml.Serialization;

using Ferda.FrontEnd.Desktop;
using Ferda.ModulesManager;

 namespace Ferda.FrontEnd
{
	/// <summary>
	/// Main form for the Ferda application
	/// </summary>
	public class FerdaForm : System.Windows.Forms.Form, Menu.IDockingManager,
        Menu.ILocalizationManager, IFerdaClipboard, IControlsManager, IOwnerOfAddIn,
        IIconProvider
    {
        #region Class fields

        /// <summary>
        /// Archive control
        /// </summary>
		protected Archive.FerdaArchive archive;
        /// <summary>
        /// PropertyGrid control
        /// </summary>
        protected Properties.FerdaPropertyGrid propertyGrid;
        /// <summary>
        /// Menu of the application
        /// </summary>
        protected Menu.FerdaMenu menu;
        /// <summary>
        /// Toolbar of the application
        /// </summary>
        protected FerdaToolBar toolBar;
        /// <summary>
        /// ContextHelp control
        /// </summary>
        private ContextHelp.FerdaContextHelp contextHelp;
        /// <summary>
        /// New box control
        /// </summary>
        private NewBox.NewBoxControl newBox;
        /// <summary>
        /// User note control
        /// </summary>
        private UserNote.FerdaUserNote userNote;

        /// <summary>
        /// All view controls
        /// </summary>
        protected List<FerdaDesktop> views = new List<FerdaDesktop>();

        private List<DockWindow> viewContents = new List<DockWindow>();
		//docking fields
		private DockDotNET.DockManager dockingManager;
        private DockDotNET.DockWindow archiveContent;
        private DockDotNET.DockWindow propertyGridContent;
        private DockDotNET.DockWindow contextHelpContent;
        private DockDotNET.DockWindow newBoxContent;
        private DockDotNET.DockWindow userNoteContent;

        /// <summary>
        /// Project manager
        /// </summary>
        protected ProjectManager.ProjectManager projectManager;

		private static FrontEndConfig iceConfig;

        private static List<IAddInMain> addIns = new List<IAddInMain>();

        private static string recentProjectsPath = "recent.xml";

        /// <summary>
        /// Dictionary that contains all the icons for the application, ]
        /// that are keyed by string values. See 
        /// <see cref="F:Ferda.FrontEnd.FerdaForm.LoadIcons"/> for their names
        /// </summary>
        protected Dictionary<string, Icon> iconProvider;

        /// <summary>
        /// Prescreen 
        /// </summary>
        protected static FerdaPrescreen prescreen;

        /// <summary>
        /// Contains list of projects recently opened
        /// </summary>
        protected List<string> recentProjects;

        //the name of the project
        private string projectName = string.Empty;
        //One SVGManager for the whole application
        private SVGManager svgManager;
        //resource manager and localization string of the application
        private ResourceManager resManager;
        //IFerdaClipboard implementation
        private List<ModulesManager.IBoxModule> nodes = new List<ModulesManager.IBoxModule>();

        #endregion

        #region Properties

        /// <summary>
        /// Resource manager of the application, it is filled according to the
        /// current localization
        /// </summary>
        public ResourceManager ResManager
        {
            get
            {
                return resManager;
            }
        }

        /// <summary>
        /// Manager of the SVG graphics (for the desktop)
        /// </summary>
        public Desktop.SVGManager SvgManager
        {
            get
            {
                if (svgManager == null)
                    throw new NullReferenceException("Global SVGManager");
                else
                    return svgManager;
            }
        }

        #region IFerdaClipboart implementation

        /// <summary>
        /// Implementation of IFerdaClipboard, Nodes
        /// </summary>
        public List<ModulesManager.IBoxModule> Nodes
        {
            set
            {
                nodes = value;
            }
            get
            {
                return nodes;
            }
        }

        /// <summary>
        /// Implementation of IFerdaClipboard, IsEmpty property
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return (nodes.Count == 0);
            }
        }

        #endregion

        #region IControlsManager implementation

        /// <summary>
        /// If there is an opened project, this property shows the name of the opened
        /// project, otherwise it is a string.Empty
        /// </summary>
        public string ProjectName
        {
            set
            {
                projectName = value;
                if (projectName != string.Empty)
                {
                    //setting the Text of the form - we know what project
                    //we are working on
                    string tmp =
                        projectName.Substring(projectName.LastIndexOf('\\') + 1);
                    tmp = tmp.Substring(0, tmp.LastIndexOf('.'));
                    Text = tmp + " - Ferda DataMiner";
                }
                else
                {
                    Text = "Ferda DataMiner";
                }
            }
            get
            {
                return projectName;
            }
        }

        #endregion

        /// <summary>
        /// Localization strings (application is able to load these localizations)
        /// The application uses always the first string
        /// </summary>
        public string[] LocalePrefs
        {
            get
            {
                return iceConfig.ProjectManagerOptions.LocalePrefs;
            }
            set
            {
                iceConfig.ProjectManagerOptions.LocalePrefs = value;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor for the FerdaForm class. Does nothing because it
        /// has to wait for the construction of the ProjectManager. The real
        /// constructor job is carried out by RightAfterConstructor method
        /// </summary>
        private FerdaForm() : base()
        {
        }

        /// <summary>
        /// This method should be always called after the constructor, because
        /// without this method FerdaForm wont work at all. It contains all the
        /// necessary setup for the form.
        /// </summary>
        /// <param name="pm">Project Manager of the application</param>
        /// <param name="prescreen">Prescreen where text about application
        /// loading is displayed</param>
        public void RightAfterConstructor(ProjectManager.ProjectManager pm, 
            FerdaPrescreen prescreen)
        {
            projectManager = pm;

            //fills the projectManager with some stuff
            FillPM();

            SizeChanged += new EventHandler(FormSizeChanged);
            Closing += new CancelEventHandler(FerdaForm_Closing);

            //loading the recent projects
            LoadRecentProjects();

            prescreen.DisplayText(ResManager.GetString("LoadingIcons"));
            LoadIcons();
            this.Icon = iconProvider["FerdaIcon"];

            prescreen.DisplayText(ResManager.GetString("LoadingSVG"));
            SetupSVG();
            prescreen.DisplayText(ResManager.GetString("LoadingMenus"));
            SetupStripes();

            prescreen.DisplayText(ResManager.GetString("LoadingContextHelp"));
            SetupContextHelp();
            prescreen.DisplayText(ResManager.GetString("LoadingProperties"));
            SetupProperties();

            prescreen.DisplayText(ResManager.GetString("LoadingArchive"));
            SetupArchive();
            prescreen.DisplayText(ResManager.GetString("LoadingNewBox"));
            SetupNewBox();
            SetupUserNote();
            prescreen.DisplayText(ResManager.GetString("LoadingDesktop"));
            SetupDesktop();

            //Because when the program is setting up the menu, all the views(desktops)
            //are closed. Thus it would put all the views into the open group. After
            //the inicialization of the desktops, all the dekstops are open - and
            //the behaviour is correct.
            menu.SetupDesktop();

            prescreen.DisplayText(ResManager.GetString("LoadingDocking"));
            SetupDocking();

            //Name and title of the application
            Name = "FerdaForm";
            Text = "Ferda DataMiner";

            //this command should be the last one, otherwise the window is not
            //maximized correctly
            WindowState = FormWindowState.Maximized;
        }

        #endregion

        #region Methods

        #region Setup methods

        /// <summary>
        /// Sets the ResourceManager of the form. This method is not included in the
        /// <see cref="F:Ferda.FrontEnd.FerdaForm.RightAfterConstructor"/> function,
        /// because the program needs resource manager to localize 
        /// </summary>
        /// <param name="config">File with application configuration
        /// </param>
        protected void SetupResources(FrontEndConfig config)
        {
            //setting the ResManager resource manager and localization string
            string locString = "Ferda.FrontEnd.Localization_" + config.ProjectManagerOptions.LocalePrefs[0];
            resManager = new ResourceManager(locString, Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// Loads the icons for the application
        /// </summary>
        /// <remarks>
        /// Sometimes, the program path can change and at this time, no icons
        /// are present
        /// </remarks>
        protected void LoadIcons()
        {
            Icon i;
            iconProvider = new Dictionary<string, Icon>();

            //loading the program icon
            i = new Icon("FerdaFrontEnd.ico");
            iconProvider.Add("FerdaIcon", i);

            //loading the newbox and archive icons
            i = new Icon("Icons\\NA.ico");
            iconProvider.Add("NAIcon", i);
            i = new Icon("Icons\\Category.ico");
            iconProvider.Add("FolderIcon", i);

            //loading the menu->File group icons
            i = new Icon("Icons\\Open project.ico");
            iconProvider.Add("OpenProject", i);
            i = new Icon("Icons\\New project.ico");
            iconProvider.Add("NewProject", i);
            i = new Icon("Icons\\Save project.ico");
            iconProvider.Add("SaveProject", i);
            i = new Icon("Icons\\Exit.ico");
            iconProvider.Add("Exit", i);

            //loading the menu->Desktop part
            i = new Icon("Icons\\New Desktop.ico");
            iconProvider.Add("NewDesktop", i);

            //loading the menu->Preferences part
            i = new Icon("Icons\\Properties.ico");
            iconProvider.Add("Properties", i);

            //loading the icons connected to task
            i = new Icon("Icons\\Rename Icon.ico");
            iconProvider.Add("Rename", i);
            i = new Icon("Icons\\Clone.ico");
            iconProvider.Add("Clone", i);
            i = new Icon("Icons\\Copy.ico");
            iconProvider.Add("Copy", i);
            i = new Icon("Icons\\Paste.ico");
            iconProvider.Add("Paste", i);
            i = new Icon("Icons\\Delete from Archive.ico");
            iconProvider.Add("DeleteFromArchive", i);
            i = new Icon("Icons\\UserNote.ico");
            iconProvider.Add("UserNote", i);


            //loading other icons needed for the desktop
            i = new Icon("Icons\\Layout.ico");
            iconProvider.Add("Layout", i);
            i = new Icon("Icons\\Delete from Desktop.ico");
            iconProvider.Add("DeleteFromDesktop", i);
            i = new Icon("Icons\\Unpack socket one layer.ico");
            iconProvider.Add("UnpackSocketOneLayer", i);
            i = new Icon("Icons\\Unpack socket all layers.ico");
            iconProvider.Add("UnpackSocketAllLayers", i);
            i = new Icon("Icons\\Pack sockets.ico");
            iconProvider.Add("PackSocket", i);
            i = new Icon("Icons\\Pack all sockets.ico");
            iconProvider.Add("PackAllSockets", i);
            i = new Icon("Icons\\Unpack all sockets all layers.ico");
            iconProvider.Add("UnpackAllLayersAllSockets", i);
            i = new Icon("Icons\\Unpack all sockets one layer.ico");
            iconProvider.Add("UnpackAllLayersOneSocketAllSockets", i);
            i = new Icon("Icons\\Localize in archive.ico");
            iconProvider.Add("LocalizeInArchive", i);
            i = new Icon("Icons\\MakeGroup.ico");
            iconProvider.Add("MakeGroup", i);
        }

        /// <summary>
        /// Initializes the bars that use the stripe classes in the form:
        /// <code>FerdaMenu</code>, <code>FerdaToolBar</code> and
        /// <code>FerdaStatusBar</code>
        /// </summary>
        protected void SetupStripes()
        {
            //menu
            menu = new Menu.FerdaMenu(this, this, projectManager, this, this);

            //toolBar
            toolBar = new FerdaToolBar(this, this, menu);

            //adding controls to the form (this order should remain)
            Controls.Add(toolBar);
            Controls.Add(menu);
        }

        /// <summary>
        /// Initializes the archive of the application
        /// </summary>
        protected void SetupArchive()
        {
            //creating the archive and its content
            archive = new Ferda.FrontEnd.Archive.FerdaArchive(this, menu, this, 
                projectManager.Archive, this, toolBar, projectManager);
            archiveContent = new DockWindow();
            archiveContent.Resize += new EventHandler(archiveContent_Resize);

            //synchronizing the sizes of content and archive
            archiveContent.ClientSize = archive.Size;

            //Settings required by the DockDotNET library to dock anything
            archiveContent.DockType = DockContainerType.ToolWindow;
            archiveContent.Text = ResManager.GetString("ArchiveContentText");
            archive.ResManager = this.ResManager;
            archive.PropertiesDisplayer = propertyGrid;
            archive.ContextHelpDisplayer = contextHelp;

            //setting the archive displayer
            propertyGrid.ArchiveDisplayer = archive;
            menu.ArchiveDisplayer = archive;

            archiveContent.Controls.Add(archive);
        }

        /// <summary>
        /// Initializes the NewBoxTreeView of the application
        /// </summary>
        protected void SetupNewBox()
        {
            //creating the newBox and its content
            newBox = new FrontEnd.NewBox.NewBoxControl(this, menu, 
                projectManager.ModulesManager, this, toolBar);
            newBoxContent = new DockWindow();
            newBoxContent.Resize += new EventHandler(newBoxContent_Resize);

            //synchronizin the sizes of content and newbox
            newBoxContent.ClientSize = newBox.Size;

            //Settings required by the DockDotNET library to dock anything
            newBoxContent.DockType = DockContainerType.ToolWindow;
            newBoxContent.Text = ResManager.GetString("NewBoxContentText");
            //newBox.ResManager = this.ResManager;

            newBoxContent.Controls.Add(newBox);
        }

        /// <summary>
        /// Initializes the desktop (view) of the application
        /// </summary>
        protected void SetupDesktop()
        {
            foreach (ProjectManager.View view in projectManager.Views)
            {
                //creating a view and its content
                FerdaDesktop desktop =
                    new FerdaDesktop(this, SvgManager, menu, view, 
                    projectManager, archive, this, toolBar);
                DockWindow desktopContent = new DockWindow();

                views.Add(desktop);
                viewContents.Add(desktopContent);

                //other desktop initializations
                desktop.Dock = DockStyle.Fill;
                desktopContent.DockType = DockContainerType.Document;
                desktopContent.Text = view.Name;
                desktop.ResManager = this.ResManager;
                desktop.Clipboard = this;
                desktop.PropertiesDisplayer = propertyGrid;
                desktop.ContextHelpDisplayer = contextHelp;
                desktop.UserNote = userNote;

                //event for changing names
                desktopContent.TextChanged += new EventHandler(desktopContent_TextChanged);
                //the closing event
                desktopContent.Closed += new EventHandler(desktopContent_Closed);

                desktopContent.Controls.Add(desktop);
                desktopContent.ResumeLayout(false);

                desktopContent.Activated += new EventHandler(desktopContent_Activated);

                //setting the IViewDisplayers of the property grid
                propertyGrid.ViewDisplayers.Add(desktop);
            }

            //setting the views property to the archive and all other views
            archive.Views = views;
            foreach (FerdaDesktop desktop in views)
            {
                desktop.Views = views;
            }
        }

        /// <summary>
        /// Initializes the property grid of the application
        /// </summary>
        protected void SetupProperties()
        {
            //creating the property grid and its content
            propertyGrid = new Ferda.FrontEnd.Properties.FerdaPropertyGrid(this, menu, toolBar);
            propertyGridContent = new DockWindow();
            propertyGridContent.Resize += new EventHandler(propertyGridContent_Resize);
            propertyGrid.Name = "PropertyGrid";

            //synchronizing the sizes of content and propertiesDisplayer
            propertyGridContent.ClientSize = propertyGrid.Size;

            //Settings required by the DockDotNET library to dock anything
            propertyGridContent.DockType = DockContainerType.ToolWindow;
            propertyGridContent.Text = ResManager.GetString("PropertiesContentText");
            propertyGrid.ResManager = this.ResManager;

            propertyGridContent.Controls.Add(propertyGrid);
            propertyGridContent.ResumeLayout(false);

            //more initializations
        }

        /// <summary>
        /// Initializes the dynamic help of the application
        /// </summary>
        protected void SetupContextHelp()
        {
            //creating the dynamic help and its content
            contextHelp = new Ferda.FrontEnd.ContextHelp.FerdaContextHelp(this, menu, toolBar);
            contextHelpContent = new DockWindow();

            //synchronizing the sizes of content and contextHelp
            contextHelpContent.ClientSize = contextHelp.Size;
            contextHelpContent.Resize += new EventHandler(contextHelpContent_Resize);

            //Settings required by the DockDotNET library to dock anything
            contextHelpContent.DockType = DockContainerType.ToolWindow;
            contextHelpContent.Text = ResManager.GetString("ContextHelpContentText");
            contextHelpContent.ResumeLayout(false);
            contextHelp.ResManager = this.ResManager;

            contextHelpContent.Controls.Add(contextHelp);
        }

        /// <summary>
        /// Initializes the user note control of the application
        /// </summary>
        protected void SetupUserNote()
        {
            //creating the user note control and its content
            userNote = new UserNote.FerdaUserNote();
            userNoteContent = new DockWindow();
            //I dont really know how this works, but putting there a smaller number
            //makes the user note a little bit smaller
            userNoteContent.Size = new Size(150, 50);

            //synchronizing the sizes of the content and userNote
            userNoteContent.ClientSize = userNoteContent.Size;
            userNoteContent.Resize += new EventHandler(userNoteContent_Resize);

            //Settings required by teh DockDotNet library to dock anything
            userNoteContent.DockType = DockContainerType.ToolWindow;
            userNoteContent.Text = ResManager.GetString("UserNoteCaption");
            userNoteContent.ResumeLayout(false);

            userNoteContent.Controls.Add(userNote);

            archive.UserNote = userNote;
            foreach (FerdaDesktop desktop in views)
            {
                desktop.UserNote = userNote;
            }
            userNote.Reset();
        }

        /// <summary>
        /// Initialization of all the SVG stuff in the application
        /// </summary>
        protected void SetupSVG()
        {
            Control c = new Control();
            c.Visible = false;
            c.Size = new Size(32, 32);
            this.Controls.Add(c);

            svgManager = new Ferda.FrontEnd.Desktop.SVGManager(c);
        }

        #endregion

        #region AddIns methods

        /// <summary>
        /// Load the addins to the FrontEnd
        /// </summary>
        /// <param name="ownerOfAddIn">Who will own this addin</param>
        /// <param name="objectAdapter">Some ice stuff</param>
        /// <param name="modulesManager">Modules Manager</param>
        /// <param name="displayer">Displayer of the properties (if an Add-in has a property)</param>
        private static void loadAddIns(IOwnerOfAddIn ownerOfAddIn,
            Ice.ObjectAdapter objectAdapter,
            Ferda.ModulesManager.ModulesManager modulesManager, Properties.IOtherObjectDisplayer displayer)
		{
			System.Collections.Specialized.StringCollection proxies
				= new System.Collections.Specialized.StringCollection();

			foreach(string file in System.IO.Directory.GetFiles("AddIns"))
			{
				if(System.IO.Path.GetExtension(file) == ".dll")
				{
                    string path = "Ferda.FrontEnd.AddIns." +
                        System.IO.Path.GetFileNameWithoutExtension(file) +
                        ".Main";

                    //tohle se nezvladne
                    Assembly asembly =
                        System.Reflection.Assembly.LoadFile(System.IO.Path.GetFullPath(file));

                    IAddInMain addInMain = (IAddInMain)asembly.CreateInstance(path);

                    //adding the properties displayer if it is a addin capable of
                    //displaying properties
                    if (addInMain is Properties.IPropertyProvider)
                    {
                        Properties.IPropertyProvider prov = addInMain as
                            Properties.IPropertyProvider;

                        prov.Displayer = displayer;
                    }

					addInMain.OwnerOfAddIn = ownerOfAddIn;
					addInMain.ObjectAdapter = objectAdapter;
					proxies.AddRange(addInMain.ObjectProxiesToAdd);
					addIns.Add(addInMain);
				}
			}
			int count = proxies.Count;
			string[] newServices = new string[count];
			if(count > 0)
			{
				proxies.CopyTo(newServices, 0);
			}
			modulesManager.AddModuleServices(newServices);
        }

        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string [] args)
        {
        	prescreen = new FerdaPrescreen();

            prescreen.Show();
            prescreen.Refresh();

            //tries to load the config
            try
            {
                iceConfig = FrontEndConfig.Load();
            }
            catch
            {
                prescreen.Hide();
                MessageBox.Show("Could not locate the FrontEndConfig.xml configuration file", 
                    "Invalid config file",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            //loading the form
            FerdaForm form = new FerdaForm();
            form.SetupResources(iceConfig);
            
            //switching to the "bin" directory
            string previousDir = FrontEndCommon.SwitchToBinDirectory();

            prescreen.DisplayText(form.ResManager.GetString("LoadingProjectManager"));
            //loading the project manager
            ProjectManager.ProjectManager pm =
                new ProjectManager.ProjectManager(
				args,
				iceConfig.ProjectManagerOptions,
				new Ferda.FrontEnd.OutputI());

            //setting the form for the project manager
		    form.RightAfterConstructor(pm, prescreen);

            prescreen.DisplayText(form.ResManager.GetString("LoadingAddIns"));
            //loading the add ins
			loadAddIns(form, pm.ModulesManager.Helper.ObjectAdapter, pm.ModulesManager, form.propertyGrid);
			pm.ModulesManager.AddModuleServices(iceConfig.FrontEndIceObjects);

            //switching to the directory from where it was executed
            FrontEndCommon.SwitchToPreviousDirectory(previousDir);

            //loading the associated file (if there is one)
            if (args.Length > 0)
            {
                FrontEndCommon.LoadProject(args[0], form, form.ResManager, ref pm,
                    form);
                form.WindowState = FormWindowState.Maximized;
            }

			prescreen.Hide();
            try
            {
                //running the application
                Application.Run(form);
            }
            finally
            {
                //clearing the add in and project manager resources
				addIns.Clear();
                pm.DestroyProjectManager();
            }

            form.SaveRecentProjects();

            //tries to save the config
            try
            {
                FrontEndConfig.Save(iceConfig);
            }
            catch
            {
                MessageBox.Show("Could not save the FrontEndConfig.xml configuration file",
                    "Invalid config file",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }

        #region Docking related functions

        ///<summary>
        /// Does all the necessary work to set up docking in the application.
        /// Docks all the main controls.
        ///</summary>
        ///<remarks>
        /// Ferda is using DockDotNET as its docking library.
        ///</remarks>
        protected void SetupDocking()
        {
            Ferda.FrontEnd.Constants constants;
            constants = new Constants();

            //initializing the dockManager class
            dockingManager = new DockManager();

            dockingManager.DockBorder = 20;
            dockingManager.DockPadding.Bottom = 2;
            dockingManager.DockPadding.Left = 2;
            dockingManager.DockPadding.Right = 2;
            dockingManager.DockPadding.Top = 24;
            dockingManager.DockType = DockDotNET.DockContainerType.Document;
            dockingManager.ShowIcons = true;
            dockingManager.SplitterWidth = 4;
            dockingManager.TabIndex = 0;
            dockingManager.Size = new System.Drawing.Size(500, 300);

            //adjusting the location of the dockingManager according to
            //the preset Contants
            dockingManager.Location =
                new System.Drawing.Point(0, 2 * constants.StripWidth);

            Controls.Add(dockingManager);

            //docking the archive
            dockingManager.AddForm(archiveContent);
            AddOwnedForm(archiveContent); //required process by the DockDotNET
            dockingManager.DockWindow(archiveContent, DockStyle.Left);

            //docking the view
            foreach (DockWindow viewContent in viewContents)
            {
                dockingManager.AddForm(viewContent);
                AddOwnedForm(viewContent); //required process by the DockDotNET
                dockingManager.DockWindow(viewContent, DockStyle.Fill);
            }

            //docking the property grid
            dockingManager.AddForm(propertyGridContent);
            AddOwnedForm(propertyGridContent);
            dockingManager.DockWindow(propertyGridContent, DockStyle.Right);

            //docking the user note
            dockingManager.AddForm(userNoteContent);
            AddOwnedForm(userNoteContent);
            DockContainer cont = propertyGridContent.HostContainer;
            cont.DockWindow(userNoteContent, DockStyle.Bottom);

            //docking the context help
            dockingManager.AddForm(contextHelpContent);
            AddOwnedForm(contextHelpContent);
            cont = userNoteContent.HostContainer;
            cont.DockWindow(contextHelpContent, DockStyle.Fill);

            //docking the newBox
            dockingManager.AddForm(newBoxContent);
            AddOwnedForm(newBoxContent);
            cont = archiveContent.HostContainer;
            cont.DockWindow(newBoxContent, DockStyle.Bottom);
        }

        ///<summary>
        ///Shows the application's archive
        ///</summary>
        public void ShowArchive()
        {
            if (archiveContent.IsVisible)
            {
                //nedelam nic, uz tam je
            }
            else
            {
                SetupArchive();
                archive.Views = views;
                archive.UserNote = userNote;
                foreach (FerdaDesktop desktop in views)
                {
                    desktop.ArchiveDisplayer = archive;
                }

                //docking the archive
                dockingManager.AddForm(archiveContent);
                AddOwnedForm(archiveContent); //required process by the DockDotNET
                if (newBoxContent.IsVisible)
                {
                    DockContainer cont = newBoxContent.HostContainer;
                    cont.DockWindow(archiveContent, DockStyle.Top);
                }
                else
                {
                    dockingManager.DockWindow(archiveContent, DockStyle.Left);
                }
            }
        }

        ///<summary>
        ///Shows the application's context help
        ///</summary>
        public void ShowContextHelp()
        {
            if (contextHelpContent.IsVisible)
            {
            }
            else
            {
                SetupContextHelp();
                foreach (FerdaDesktop desktop in views)
                {
                    desktop.ContextHelpDisplayer = contextHelp;
                }
                archive.ContextHelpDisplayer = contextHelp;

                //docking the control
                dockingManager.AddForm(contextHelpContent);
                AddOwnedForm(contextHelpContent);
                if (userNoteContent.IsVisible)
                {
                    DockContainer cont = userNoteContent.HostContainer;
                    cont.DockWindow(contextHelpContent, DockStyle.Fill);
                }
                else
                {
                if (propertyGridContent.IsVisible)
                {
                    //docking according to the propertiesDisplayer
                    DockContainer cont = propertyGridContent.HostContainer;
                    cont.DockWindow(contextHelpContent, DockStyle.Bottom);
                }
                    else
                    {
                        //docking to the right side as a first control
                        dockingManager.DockWindow(contextHelpContent, DockStyle.Right);
                    }
                }
            }
        }

        ///<summary>
        ///Shows the application's property grid
        ///</summary>
        public void ShowPropertyGrid()
        {
            if (propertyGridContent.IsVisible)
            {
            }
            else
            {
                SetupProperties();
                foreach (FerdaDesktop desktop in views)
                {
                    //setting the IViewDisplayers of the property grid
                    propertyGrid.ViewDisplayers.Add(desktop);
                    desktop.PropertiesDisplayer = propertyGrid;
                }
                archive.PropertiesDisplayer = propertyGrid;
                menu.PropertiesDisplayer = propertyGrid;

                //docking
                dockingManager.AddForm(propertyGridContent);
                AddOwnedForm(propertyGridContent);
                if (contextHelpContent.IsVisible)
                {
                    //docking above the ContextHelp
                    DockContainer cont = contextHelpContent.HostContainer;
                    cont.DockWindow(propertyGridContent, DockStyle.Top);
                    return;
                }

                if (userNoteContent.IsVisible)
                {
                    //docking it above the user note
                    DockContainer cont = userNoteContent.HostContainer;
                    cont.DockWindow(propertyGridContent, DockStyle.Top);
                    return;
                }
                
                //docking to the right side as a first control
                dockingManager.DockWindow(propertyGridContent, DockStyle.Right);
            }
        }

        /// <summary>
        /// Shows the newBox treeview
        /// </summary>
        public void ShowNewBox()
        {
            if (newBoxContent.IsVisible)
            {
            }
            else
            {
                //the necessary stuff
                SetupNewBox();
                dockingManager.AddForm(newBoxContent);
                AddOwnedForm(newBoxContent);

                if (archiveContent.IsVisible)
                {
                    //docking according to the archive
                    DockContainer cont = archiveContent.HostContainer;
                    cont.DockWindow(newBoxContent, DockStyle.Bottom);
                }
                else
                {
                    //docking to the left side as a first control
                    dockingManager.DockWindow(newBoxContent, DockStyle.Left);
                }
            }
        }

        /// <summary>
        /// Shows the user note control
        /// </summary>
        public void ShowUserNote()
        {
            if (!userNoteContent.IsVisible)
            {
                //letting the views know about the change
                foreach (FerdaDesktop desktop in views)
                {
                    desktop.UserNote = userNote;
                }
                archive.UserNote = userNote;

                //the nececcary docking initialization
                SetupUserNote();
                dockingManager.AddForm(userNoteContent);
                AddOwnedForm(userNoteContent);

                //docking to the context help - nearest control
                if (contextHelpContent.IsVisible)
                {
                    DockContainer cont = contextHelpContent.HostContainer;
                    cont.DockWindow(userNoteContent, DockStyle.Fill);
                }
                else
                {
                    //docking to the propertry grid
                    if (propertyGridContent.IsVisible)
                    {
                        DockContainer cont = propertyGridContent.HostContainer;
                        cont.DockWindow(userNoteContent, DockStyle.Bottom);
                    }
                    else //docking to the main control
                    {
                        //docking to the right side as a first control
                        dockingManager.DockWindow(userNoteContent, DockStyle.Right);
                    }
                }
            }
        }

        #endregion //Docking related functions

        #region Appereance related functions


        #endregion //Appereance related functions

        #region Localization related functions

        /// <summary>
        /// Part of the ILocalizationManager interface, forces the whole application
        /// to change its localization according to the new resource Manager
        /// </summary>
        /// <remarks>
        /// This method is not used, because the project manager cannot change
        /// the localization of boxes at runtime - it can only be changed
        /// during restart
        /// </remarks>
        /// <param name="locstring">Target new localization (en for english,
        /// cz for czech
        /// </param>
        public void ChangeGlobalLocalization(string locstring)
        {
            if (locstring != "cz" || locstring != "en")
            {
                throw new ArgumentException("Invalid localization string");
            }

            //localizationString = locstring;

            if (locstring == "cz") //czech localization
            {
                resManager = new ResourceManager("Ferda.FrontEnd.LocalizationCZ",
                Assembly.GetExecutingAssembly());
            }
            else //english localization
            {
                resManager = new ResourceManager("Ferda.FrontEnd.LocalizationEN",
                Assembly.GetExecutingAssembly());
            }

            //changing the localization of the stripes
            menu.ChangeLocalization();
            toolBar.ChangeLocalization();

            //changing the localization of the archive
            archive.ChangeLocalization();
            archiveContent.Text = ResManager.GetString("ArchiveContentText");

            //changing the localization of the newbox
            newBoxContent.Text = ResManager.GetString("NewBoxContentText");

            //changing the localization of the desktop
            foreach (Desktop.FerdaDesktop desktop in views)
            {
                desktop.ChangeLocalization();
            }

            //changing the localization of the propertygrid
            propertyGrid.ChangeLocalization();
            propertyGridContent.Text = ResManager.GetString("PropertiesContentText");

            //changing the localization of the context help
            contextHelp.ChangeLocalization();
            contextHelpContent.Text = ResManager.GetString("ContextHelpContentText");
        }

        #endregion

        #region IControlsManager implementation + related

        /// <summary>
        /// Forces all the relevant controls to adapt
        /// </summary>
        public void GlobalAdapt()
        {
            //restoring the archive
            archive.Archive = projectManager.Archive;
            archive.ArchiveSetupAfterLoadProject();
            //deleting the views
            /* It does not have to be here, because the ClearDocking() method
             * does the business
            foreach (DockWindow wnd in viewContents)
            {
                dockingManager.RemoveForm(wnd);
                wnd.IsVisible = false;
            }
            */

            //creating the new views
            views = new List<FerdaDesktop>();
            viewContents = new List<DockWindow>();
            propertyGrid.ViewDisplayers = new List<IViewDisplayer>();

            //recreating the views
            SetupDesktop();

            //docking the views
            foreach (DockWindow viewContent in viewContents)
            {
                dockingManager.AddForm(viewContent);
                AddOwnedForm(viewContent); //required process by the DockDotNET
                dockingManager.DockWindow(viewContent, DockStyle.Fill);
            }

            //resets the propertygrid
            propertyGrid.Reset();
        }

        /// <summary>
        /// Adds a new desktop to the FrontEnd (new view to the project)
        /// </summary>
        public void NewDesktop()
        {
            string tmp = ResManager.GetString("MenuNewViewName");

            //determining the name of the new desktop
            int count = 1;
            foreach (ProjectManager.View view in projectManager.Views)
            {
                if (view.Name.Contains(tmp))
                {
                    //getting the number
                    int number = view.Name.IndexOfAny(new char[] {'0', '1',
                        '2', '3', '4', '5', '6', '7', '8', '9'});
                    if (number != -1)
                    {
                        string numberString = view.Name.Substring(number);
                        number = System.Convert.ToInt32(numberString);

                        if (number > count)
                        {
                            count = number;
                        }
                    }
                }
            }
            string name = tmp + System.Convert.ToString(count + 1);

            //creating new view and its content
            ProjectManager.View newView =
                projectManager.NewView(name);
            FerdaDesktop newDesktop =
                new FerdaDesktop(this, SvgManager, menu, newView, projectManager, 
                archive, this, toolBar);
            DockWindow desktopContent = new DockWindow();

            views.Add(newDesktop);
            viewContents.Add(desktopContent);

            //other desktop initializations
            newDesktop.Dock = DockStyle.Fill;
            desktopContent.DockType = DockContainerType.Document;
            desktopContent.Text = newView.Name;
            desktopContent.TextChanged += new EventHandler(desktopContent_TextChanged);
            desktopContent.Closed += new EventHandler(desktopContent_Closed);
            desktopContent.Activated += new EventHandler(desktopContent_Activated);
            newDesktop.ResManager = this.ResManager;
            newDesktop.Clipboard = this;
            newDesktop.PropertiesDisplayer = propertyGrid;
            newDesktop.ContextHelpDisplayer = contextHelp;
            newDesktop.UserNote = userNote;

            //adding the views
            newDesktop.Views = views;

            desktopContent.Controls.Add(newDesktop);
            desktopContent.ResumeLayout(false);

            //setting the IViewDisplayers of the property grid
            propertyGrid.ViewDisplayers.Add(newDesktop);

            //docking the desktop
            dockingManager.AddForm(desktopContent);
            AddOwnedForm(desktopContent);
            dockingManager.DockWindow(desktopContent, DockStyle.Fill);
        }

        /// <summary>
        /// Function that returns the names of the views that are opened in the
        /// FrontEnd
        /// </summary>
        /// <returns>Names of opened views in the FrontEnd</returns>
        public List<string> OpenedViews()
        {
            List<string> result = new List<string>();

            foreach (DockWindow dw in viewContents)
            {
                result.Add(dw.Text);
            }

            return result;
        }

        /// <summary>
        /// Opens a view that is present in the project but was closed by the user
        /// </summary>
        /// <param name="name">name of the view</param>
        public void OpenView(string name)
        {
            //creating the view and its content
            ProjectManager.View newView = projectManager.GetView(name);
            FerdaDesktop newDesktop = new FerdaDesktop(this, SvgManager, menu, newView,
                projectManager, archive, this, toolBar);
            DockWindow desktopContent = new DockWindow();

            views.Add(newDesktop);
            viewContents.Add(desktopContent);

            //other desktop initializations
            newDesktop.Dock = DockStyle.Fill;
            desktopContent.DockType = DockContainerType.Document;
            desktopContent.Text = newView.Name;
            desktopContent.TextChanged += new EventHandler(desktopContent_TextChanged);
            desktopContent.Closed += new EventHandler(desktopContent_Closed);
            desktopContent.Activated += new EventHandler(desktopContent_Activated);
            newDesktop.ResManager = this.ResManager;
            newDesktop.Clipboard = this;
            newDesktop.PropertiesDisplayer = propertyGrid;
            newDesktop.ContextHelpDisplayer = contextHelp;

            //adding the views
            newDesktop.Views = views;

            desktopContent.Controls.Add(newDesktop);
            desktopContent.ResumeLayout(false);

            //setting the IViewDisplayers of the property grid
            propertyGrid.ViewDisplayers.Add(newDesktop);

            //docking the desktop
            dockingManager.AddForm(desktopContent);
            AddOwnedForm(desktopContent);
            dockingManager.DockWindow(desktopContent, DockStyle.Fill);
        }
        
        /// <summary>
        /// Closes a view that is opened in the FerdaForm
        /// </summary>
        /// <param name="name">name of the view</param>
        public void CloseView(string name)
        {
            foreach (DockWindow w in viewContents)
            {
                if (w.Text == name)
                {
                    w.Close();
                    break;
                }
            }
        }

        /// <summary>
        /// Method that fills the project manager with a new view. It is used
        /// when a new project is created
        /// </summary>
        public void FillPM()
        {
            ProjectManager.View v1 =
                projectManager.NewView(ResManager.GetString("MenuDesktopNewDesktop"));
        }

        /// <summary>
        /// It is used when a new project is created. Any modules for interaction that
        /// are docked in the center of the screen should be removed.
        /// </summary>
        public void ClearDocking()
        {
            while (dockingManager.ListDocument.Count > 0)
            {
                DockPanel dockPanel = (DockPanel)dockingManager.ListDocument[0];
                DockWindow dockWindow = dockPanel.Form;
                dockingManager.RemoveForm(dockWindow);
                dockWindow.IsVisible = false;
            }
        }

        /// <summary>
        /// Loads the information about the recent projects from a file and
        /// loads it to its temporary structures
        /// </summary>
        /// <returns>A list containing recent projects</returns>
        public IList<string> GetRecentProjects()
        {
            return recentProjects;
        }

        /// <summary>
        /// Adds the new project path to the recent project and
        /// returns the modified project paths
        /// </summary>
        /// <param name="newProject">New path of the opened project</param>
        /// <returns>Modified list containg the recent project paths</returns>
        public void AddToRecentProjects(string newProject)
        {
            if (!recentProjects.Contains(newProject))
            {
                recentProjects.Add(newProject);
            }
        }

        /// <summary>
        /// Saves recent projects to a file
        /// </summary>
        protected void SaveRecentProjects()
        {
            //getting the right path
            string path = FrontEndCommon.GetBinPath();
            path += "\\" + recentProjectsPath;

            FileStream fs = null;
            try
            {
                fs = new FileStream(path, FileMode.Create);
                XmlSerializer s = new XmlSerializer(typeof(List<string>));
                TextWriter w = new StreamWriter(fs);
                s.Serialize(w, recentProjects);
                w.Close();
            }
            finally
            {
                fs.Close();
            }
        }

        /// <summary>
        /// Loads recent projects from a file
        /// </summary>
        protected void LoadRecentProjects()
        {
            //getting the right path
            string path = FrontEndCommon.GetBinPath();
            path += "\\" + recentProjectsPath;

            recentProjects = new List<string>();

            FileStream fs;
            try
            {
                fs = new FileStream(path, FileMode.Open);
            }
            catch (Exception)
            {
                //for some reason, the file could not be opened
                //we take it as there is no file there
                return;
            }

            try
            {
                XmlSerializer s = new XmlSerializer(typeof(List<string>));
                TextReader r = new StreamReader(fs);
                recentProjects = (List<string>)s.Deserialize(r);
                r.Close();
            }
            finally
            {
                fs.Close();
            }
        }

        #endregion

        #region OwnerOfAddIn implementation
        /// <summary>
        /// Method ShowForm
        /// </summary>
        /// <param name="form">A  System.Windows.Forms.Form</param>
        public void ShowForm(Form form)
        {
            form.Show();
        }

        /// <summary>
        /// Method ShowDialog
        /// </summary>
        /// <param name="form">A  System.Windows.Forms.Form</param>
        public System.Windows.Forms.DialogResult ShowDialog(Form form)
        {
            return form.ShowDialog();
        }

        /// <summary>
        /// Method ShowDockableControl
        /// </summary>
        /// <param name="userControl">A  System.Windows.Forms.UserControl</param>
        /// <param name="name">Text of the control</param>
        public void ShowDockableControl(UserControl userControl, string name)
        {
            DockWindow dockWindow = new DockWindow();
            userControl.Dock = DockStyle.Fill;
            dockWindow.DockType = DockContainerType.Document;
            dockWindow.Text = name;
            dockWindow.Controls.Add(userControl);
            dockWindow.ResumeLayout(false);

            dockingManager.AddForm(dockWindow);
            AddOwnedForm(dockWindow);
            dockingManager.DockWindow(dockWindow, DockStyle.Fill);
        }

        /// <summary>
        /// Forces to adapt the property grid from another thread
        /// </summary>
        public void AsyncAdapt()
        {
            propertyGrid.AsyncAdapt();
        }

        /// <summary>
        /// Shows the exception with the box to the user
        /// </summary>
        /// <param name="boxUserName">Name of the box that has thrown the exception</param>
        /// <param name="userMessage">User message to be displayed</param>
        public void ShowBoxException(string boxUserName, string userMessage)
        {
            BoxExceptionDialog dialog = new BoxExceptionDialog(ResManager,
                boxUserName, userMessage);
            dialog.TopMost = true;
            dialog.ShowDialog();
        }

        /// <summary>
        /// Project manager of the Ferda system
        /// </summary>
        public ProjectManager.ProjectManager ProjectManager
        {
            get
            {
                return projectManager;
            }
        }

        #endregion

        #region IIconProvider implementation

        /// <summary>
        /// Gets the icon specified by icons string identifier
        /// </summary>
        /// <param name="IconName">Name of the icon</param>
        /// <returns>Icon that is connected to this name</returns>
        public Icon GetIcon(string IconName)
        {
            return iconProvider[IconName];
        }

        #endregion 

        #region OutputPrx implementation

        /// <summary>
        /// Shows a message box
        /// </summary>
        /// <param name="type">Type of the message</param>
        /// <param name="name">Title of the message</param>
        /// <param name="message">text of the message</param>
        public void writeMsg(MsgType type, string name, string message)
        {
            switch (type)
            {
                case MsgType.Debug :
                    MessageBox.Show(message, name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;

                case MsgType.Error:
                    break;

                case MsgType.Info:
                    break;

                case MsgType.Warning:
                    break;
            }
        }

        /// <summary>
        /// Shows a message box
        /// </summary>
        /// <param name="type">Type of the message</param>
        /// <param name="name">Title of the message</param>
        /// <param name="message">text of the message</param>
        /// <param name="context">ICE context of the message</param>
        public void writeMsg(MsgType type, string name, string message, Ice.Context context)
        {
            writeMsg(type, name, message);
        }

        #endregion

        #endregion //Methods

        #region Events

        /// <summary>
        /// Handles size changes of the main form - the docking manager must be redrawn
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Arguments of the event</param>
        /// <remarks>Maybe the sizes of menu, toolbar and statusbar also need to
        /// be changed.
        /// </remarks>
        void FormSizeChanged(object sender, System.EventArgs e)
        {
            Ferda.FrontEnd.Constants constants;
            constants = new Constants();

            //changing the dockingManager size
            dockingManager.Size = new System.Drawing.Size(
                Size.Width - constants.WidthFormOffset,
                Size.Height - constants.HeightFormOffset
                );
        }

        /// <summary>
        /// Forces the archive to resize, DockDotNET cannot do it
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void archiveContent_Resize(object sender, EventArgs e)
        {
            archive.ChangeSize();
        }

        /// <summary>
        /// Forces the NewBoxTreeView control to resize
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void newBoxContent_Resize(object sender, EventArgs e)
        {
            newBox.ChangeSize();
        }

        /// <summary>
        /// Forces the ContextHelp to resize
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void contextHelpContent_Resize(object sender, EventArgs e)
        {
            contextHelp.ChangeSize();
        }

        /// <summary>
        /// Forces the userNote to resize
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void userNoteContent_Resize(object sender, EventArgs e)
        {
            userNote.ChangeSize();    
        }

        /// <summary>
        /// Forces the property grid to resize
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void propertyGridContent_Resize(object sender, EventArgs e)
        {
            propertyGrid.ChangeSize();
        }

        /// <summary>
        /// Asks the user if he should save the changes to the project
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void FerdaForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string caption = ResManager.GetString("MenuFileExit");
            string message = ResManager.GetString("MenuFileSaveChanges");

            MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;
            DialogResult result;

            // Displays the MessageBox.
            result = MessageBox.Show(this, message, caption, buttons,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

            if (result == DialogResult.Cancel) //does nothing
            {
                e.Cancel = true;
            }

            if (result == DialogResult.Yes) //saving the current project
            {
                if (ProjectName != string.Empty) //we have already a project
                //name
                {
                    menu.SaveProjectCore(ProjectName);
                }
                else //we are using the save dialog
                {
                    menu.SaveProjectAsCore();
                }
            }
        }

        /// <summary>
        /// Reacts on a change of the name of one of the desktops (views). Changes
        /// the name of the corresponding view in ProjectManager
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void desktopContent_TextChanged(object sender, EventArgs e)
        {
            DockWindow send = sender as DockWindow;

            string newName = send.Text;

            ProjectManager.View v = projectManager.GetView(send.OldText);
            v.Name = newName;

            //recreating the desktop part of the menu (a name of the desktop was changed)
            menu.SetupDesktop();
        }

        /// <summary>
        /// Reacts to a closing desktop - must delete the item from ViewContents
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void desktopContent_Closed(object sender, EventArgs e)
        {
            DockWindow send = sender as DockWindow;

            //because it would not work when closing the whole application
            if (send.Controls.Count > 0)
            {
                FerdaDesktop desktop = send.Controls[0].Controls[0] as FerdaDesktop;

                //removing the FerdaDesktop view from the views
                views.Remove(desktop);
            }

            //removing the viewContents
            viewContents.Remove(send);

            //recreating the desktop part of the menu (a desktop was closed)
            menu.SetupDesktop();

            //reseting all the stuff
            propertyGrid.Reset();
            userNote.Reset();
            contextHelp.Reset();
        }

        /// <summary>
        /// Reaction when a desktop recieves focus - the dynamic part of the main menu
        /// and the toolbar should be redrawn
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void desktopContent_Activated(object sender, EventArgs e)
        {
            DockWindow send = sender as DockWindow;

            menu.ControlHasFocus = send;
            menu.Adapt();
            toolBar.ControlHasFocus = send;
            toolBar.Adapt();
        }

        #endregion
    }
}
