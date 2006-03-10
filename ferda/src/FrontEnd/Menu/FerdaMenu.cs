// FerdaMenu.cs - menu of the Ferda application
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
using System.Globalization;
using System.Windows.Forms;
using System.Resources;
using System.Collections.Generic;

using Ferda.FrontEnd;
using Ferda.FrontEnd.Desktop;
using Ferda.FrontEnd.Archive;
using Ferda.ProjectManager;
using Ferda.ModulesManager;
using Ferda.Modules;

namespace Ferda.FrontEnd.Menu
{
	/// <summary>
	/// The class represents a menu for the application. It is a MenuStrip
    /// class and some added functionality for the menu handling
	/// </summary>
    /// <remarks>
    /// Dynamic creating of menu items and dynamic handling of menu events
    /// must be supported.
    /// </remarks>
    ///<stereotype>control</stereotype>
	public class FerdaMenu : MenuStrip, IMenuDisplayer
    {
        #region Private fields

        //Interfaces needed for control to have all the functionality
        private ProjectManager.ProjectManager projectManager;
        private IDockingManager dockingManager;
        private ILocalizationManager localizationManager;
        private IControlsManager controlsManager;
        private IArchiveDisplayer archiveDisplayer;
        private IIconProvider iconProvider;

		/// Main menus - 1.st level
        private ToolStripMenuItem file;
        private ToolStripMenuItem edit;
        private ToolStripMenuItem view;
        private ToolStripMenuItem desktop;
        private ToolStripMenuItem tools;
        private ToolStripMenuItem help;

		/// Group File
        private ToolStripMenuItem newProject;
        private ToolStripMenuItem openProject;
        private ToolStripMenuItem saveProject;
        private ToolStripMenuItem saveProjectAs;
        private ToolStripMenuItem exit;

        /// Group View
        private ToolStripMenuItem archive;
        private ToolStripMenuItem newBoxTV;
        private ToolStripMenuItem contextHelp;
        private ToolStripMenuItem propertyGrid;
        private ToolStripMenuItem userNote;

		/// Group Desktop
		private ToolStripMenuItem newDesktop;
        private ToolStripMenuItem openDesktop;
		private ToolStripMenuItem removeDesktop;
        /// additional Desktop functions may be implemented 
        /// Group Tools
		private ToolStripMenuItem preferences;
        // using FerdaPreferencesDialog
		// maybe something more

        /// Group Help
        private ToolStripMenuItem applicationHelp;
        private ToolStripMenuItem theoreticalHelp;
        private ToolStripMenuItem tutorial;
        private ToolStripMenuItem about;

        //Resource manager from the FerdaForm
        private ResourceManager resManager;

        /// <summary>
        /// Determines in the FerdaMenu, which control is now
        /// focused. The Other option stays for ModulesForInteraction and/or
        /// other controls.
        /// </summary>
        private Control controlHasFocus;

        #endregion

        #region Properties

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
                        "Menu.ResManager cannot be null");
                }
                return resManager;
            }
        }

        /// <summary>
        /// Determines in the FerdaMenu, which control is now
        /// focused. The Other option stays for ModulesForInteraction and/or
        /// other controls.
        /// </summary>
        public Control ControlHasFocus
        {
            set
            {
                controlHasFocus = value;
            }
            get
            {
                return controlHasFocus;
            }
        }

        /// <summary>
        /// Archive of the application
        /// </summary>
        public IArchiveDisplayer ArchiveDisplayer
        {
            set
            {
                archiveDisplayer = value;
            }
            get
            {
                return archiveDisplayer;
            }
        }

        #endregion

        #region Constructor

        ///<summary>
        /// Default constructor for FerdaArchive class. Initializes all menu controls
        /// and adds them to the menu.
        ///</summary>
        public FerdaMenu(
            IDockingManager dockManager, ILocalizationManager lockManager,
            ProjectManager.ProjectManager pm, IControlsManager contMan, 
            IIconProvider provider)
            : base()
        {
            //setting the iconProvider
            iconProvider = provider;

            //filling the private fields
            dockingManager = dockManager;
            localizationManager = lockManager;
            ResManager = localizationManager.ResManager;
            projectManager = pm;
            controlsManager = contMan;

            //adding the main group of the menu
            SetupMainMenuGroup();

            //adding the file group of the menu
            SetupFile();

            //adding the edit group of the menu
            SetupEdit();

            //adding the view group of the menu
            SetupView();

            //SetupDesktop();

            //adding the actions group of the menu, will be done dynamically
            //probably calling some function

            SetupTools();
            SetupHelp();
        }

        #endregion

        #region Methods

        #region Initial settings

        /// <summary>
        /// This method initializes the main group of the menu (level0 menu)
        /// </summary>
        private void SetupMainMenuGroup()
        {
            file = new ToolStripMenuItem(ResManager.GetString("MenuFile"));
            edit = new ToolStripMenuItem(ResManager.GetString("MenuEdit"));
            view = new ToolStripMenuItem(ResManager.GetString("MenuView"));
            desktop = new ToolStripMenuItem(ResManager.GetString("MenuDesktop"));
            tools = new ToolStripMenuItem(ResManager.GetString("MenuTools"));
            help = new ToolStripMenuItem(ResManager.GetString("MenuHelp"));
            this.Items.AddRange(new ToolStripItem[]
                {
                    file,
                    edit,
                    view,
                    desktop,
                    tools,
                    help
                });
        }

        /// <summary>
        /// This method initializes the file group of the menu
        /// </summary>
        private void SetupFile()
        {
            newProject = new ToolStripMenuItem(ResManager.GetString("MenuFileNewProject"));
            openProject = new ToolStripMenuItem(ResManager.GetString("MenuFileOpenProject"));
            saveProject = new ToolStripMenuItem(ResManager.GetString("MenuFileSaveProject"));
            saveProjectAs = new ToolStripMenuItem(ResManager.GetString("MenuFileSaveProjectAs"));
            exit = new ToolStripMenuItem(ResManager.GetString("MenuFileExit"));

            newProject.ShortcutKeys = (Keys)Shortcut.CtrlN;
            openProject.ShortcutKeys = (Keys)Shortcut.CtrlO;
            saveProject.ShortcutKeys = (Keys)Shortcut.CtrlS;
            saveProjectAs.ShortcutKeys = (Keys)Shortcut.CtrlShiftS;
            exit.ShortcutKeys = (Keys)Shortcut.AltF4;

            newProject.Image = iconProvider.GetIcon("NewProject").ToBitmap();
            openProject.Image = iconProvider.GetIcon("OpenProject").ToBitmap();
            saveProject.Image = iconProvider.GetIcon("SaveProject").ToBitmap();
            exit.Image = iconProvider.GetIcon("Exit").ToBitmap();

            this.file.DropDownItems.AddRange(new ToolStripItem[]
                {
                    newProject,
                    openProject,
                    saveProject,
                    saveProjectAs,
                    exit
                });

            newProject.Click += new EventHandler(newProject_Click);
            openProject.Click += new EventHandler(openProject_Click);
            saveProject.Click += new EventHandler(saveProject_Click);
            saveProjectAs.Click += new EventHandler(saveProjectAs_Click);
            exit.Click += new EventHandler(exit_Click);
        }

        /// <summary>
        /// This method initializes the edit group of the menu
        /// </summary>
        private void SetupEdit()
        {
            /*
            ToolStripSeparator sep, sep2;

            newBox = new ToolStripMenuItem(ResManager.GetString("MenuEditNewBox"));
            copy = new ToolStripMenuItem(ResManager.GetString("MenuEditCopy"));
            clone = new ToolStripMenuItem(ResManager.GetString("MenuEditClone"));
            paste = new ToolStripMenuItem(ResManager.GetString("MenuEditPaste"));
            sep = new ToolStripSeparator();
            deleteFromDesktop = new ToolStripMenuItem(ResManager.GetString("MenuEditDeleteFromDesktop"));
            deleteFromArchive = new ToolStripMenuItem(ResManager.GetString("MenuEditDeleteFromArchive"));
            sep2 = new ToolStripSeparator();
            localizeInArchive = new ToolStripMenuItem(ResManager.GetString("MenuEditLocalizeInArchive"));
            hide = new ToolStripMenuItem(ResManager.GetString("MenuEditHide"));
            this.edit.DropDownItems.AddRange(new ToolStripItem[]
                {
                    newBox,
                    copy,
                    clone,
                    paste,
                    sep,
                    deleteFromDesktop,
                    deleteFromArchive,
                    sep2,
                    hide,
                    localizeInArchive
               });
             */
        }

        /// <summary>
        /// This method inicalizes the tools group of the menu
        /// </summary>
        private void SetupTools()
        {
            //adding the tools group of the menu
            preferences = new ToolStripMenuItem(ResManager.GetString("MenuToolsPreferences"));
            preferences.ShortcutKeys = (Keys)Shortcut.CtrlShiftP;
            preferences.Image = iconProvider.GetIcon("Properties").ToBitmap();
            this.tools.DropDownItems.Add(preferences);
            preferences.Click += new EventHandler(preferences_Click);

        }

        /// <summary>
        /// This method initializes the view group of the menu
        /// </summary>
        private void SetupView()
        {
            archive = new ToolStripMenuItem(ResManager.GetString("MenuViewArchive"));
            archive.Click += new EventHandler(archive_Click);
            archive.ShortcutKeys = (Keys)Shortcut.Ctrl0;

            contextHelp = new ToolStripMenuItem(ResManager.GetString("MenuViewContextHelp"));
            contextHelp.Click += new EventHandler(contextHelp_Click);
            contextHelp.ShortcutKeys = (Keys)Shortcut.Ctrl1;

            propertyGrid = new ToolStripMenuItem(ResManager.GetString("MenuViewPropertyGrid"));
            propertyGrid.Click += new EventHandler(propertyGrid_Click);
            propertyGrid.ShortcutKeys = (Keys)Shortcut.Ctrl2;

            newBoxTV = new ToolStripMenuItem(ResManager.GetString("MenuViewNewBox"));
            newBoxTV.Click += new EventHandler(newBoxTV_Click);
            newBoxTV.ShortcutKeys = (Keys)Shortcut.Ctrl3;

            userNote = new ToolStripMenuItem(ResManager.GetString("UserNote"));
            userNote.Click += new EventHandler(userNote_Click);
            userNote.ShortcutKeys = (Keys)Shortcut.Ctrl4;

            this.view.DropDownItems.AddRange(new ToolStripItem[]
                {
                    archive,
                    contextHelp,
                    newBoxTV,
                    propertyGrid,
                    userNote
                });
        }

        /// <summary>
        /// This method initializes the Desktop group of the menu. It should dynamically
        /// create the submenus and their corresponding actions
        /// </summary>
        /// <remarks>
        /// The method is public because when the program is setting up the menu, all the views(desktops)
        /// are closed. Thus it would put all the views into the open group. After
        /// the inicialization of the desktops, all the dekstops are open - and
        /// the behaviour is correct. It has to be called from the FerdaForm.
        /// </remarks>
        public void SetupDesktop()
        {
            ToolStripMenuItem item;
            this.desktop.DropDownItems.Clear();

            //adding the desktop group of the menu
            newDesktop = new ToolStripMenuItem(ResManager.GetString("MenuDesktopNewDesktop"));
            newDesktop.ShortcutKeys = (Keys)Shortcut.CtrlT;
            newDesktop.Image = iconProvider.GetIcon("NewDesktop").ToBitmap();
            openDesktop = new ToolStripMenuItem(ResManager.GetString("MenuDesktopOpenDesktop"));
            removeDesktop = new ToolStripMenuItem(ResManager.GetString("MenuDesktopRemoveDesktop"));
            this.desktop.DropDownItems.AddRange(new ToolStripItem[]
                {
                    newDesktop,
                    openDesktop,
                    removeDesktop
                });
            newDesktop.Click += new EventHandler(newDesktop_Click);

            //
            //creating the dynamic part of the menus
            //
            ProjectManager.View [] projectViews = projectManager.Views;

            //if there are no views, disable the remove part
            if (projectManager.Views.Length == 0)
            {
                removeDesktop.Enabled = false;
            }

            //getting all the views from the project manager - remove part
            foreach (ProjectManager.View view in projectManager.Views)
            {
                item = new ToolStripMenuItem(view.Name);
                item.Click += new EventHandler(removeDesktop_Click);
                removeDesktop.DropDownItems.Add(item);
            }

            //getting all the views that are not opened - open part
            List<string> viewNames = new List<string>();

            //getting all the views from the project manager
            foreach (ProjectManager.View view in projectViews)
            {
                viewNames.Add(view.Name);
            }

            //getting all the views that are displayed
            List<string> openedViewNames = controlsManager.OpenedViews();
            //if there are no views to open, disable the control
            if (openedViewNames.Count == projectViews.Length)
            {
                openDesktop.Enabled = false;
                return;
            }

            foreach (string s in viewNames)
            {
                if (!openedViewNames.Contains(s))
                {
                    item = new ToolStripMenuItem(s);
                    item.Click += new EventHandler(openDesktop_Click);
                    openDesktop.DropDownItems.Add(item);
                }
            }
        }

        /// <summary>
        /// This method initializes the help group of the menu
        /// </summary>
        private void SetupHelp()
        {
            //adding the help group of the menu
            tutorial = new ToolStripMenuItem(ResManager.GetString("MenuHelpTutorial"));
            applicationHelp = new ToolStripMenuItem(ResManager.GetString("MenuHelpApplicationHelp"));
            theoreticalHelp = new ToolStripMenuItem(ResManager.GetString("MenuHelpTheoreticalHelp"));
            about = new ToolStripMenuItem(ResManager.GetString("MenuHelpAbout"));
            this.help.DropDownItems.AddRange(new ToolStripItem[]
            {
                tutorial,
                applicationHelp,
                theoreticalHelp,
                about
            });

            tutorial.Click += new EventHandler(tutorial_Click);
            about.Click += new EventHandler(about_Click);
            applicationHelp.Click += new EventHandler(applicationHelp_Click);
        }

        #endregion

        #region IMenuDisplayer implementation

        ///<summary>
        ///This function is called when the localization
        ///of the application is changed - the whole menu needs to be redrawn
        ///</summary>
        public void ChangeLocalization()
        {
            //updating the resource manager
            ResManager = localizationManager.ResManager;

            //renaming all static menu items
            file.Text = ResManager.GetString("MenuFile");
            edit.Text = ResManager.GetString("MenuEdit");
            view.Text = ResManager.GetString("MenuView");
            desktop.Text = ResManager.GetString("MenuDesktop");
            tools.Text = ResManager.GetString("MenuTools");
            help.Text = ResManager.GetString("MenuHelp");

            newProject.Text = ResManager.GetString("MenuFileNewProject");
            openProject.Text = ResManager.GetString("MenuFileOpenProject");
            saveProject.Text = ResManager.GetString("MenuFileSaveProject");
            exit.Text = ResManager.GetString("MenuFileExit");

            archive.Text = ResManager.GetString("MenuViewArchive");
            contextHelp.Text = ResManager.GetString("MenuViewContextHelp");
            propertyGrid.Text = ResManager.GetString("MenuViewPropertyGrid");
            newBoxTV.Text = ResManager.GetString("MenuViewNewBox");

            newDesktop.Text = ResManager.GetString("MenuDesktopNewDesktop");
            openDesktop.Text = ResManager.GetString("MenuDesktopOpenDesktop");
            removeDesktop.Text = ResManager.GetString("MenuDesktopRemoveDesktop");

            preferences.Text = ResManager.GetString("MenuToolsPreferences");

            applicationHelp.Text = ResManager.GetString("MenuHelpApplicationHelp");
            theoreticalHelp.Text = ResManager.GetString("MenuHelpTheoreticalHelp");
            about.Text = ResManager.GetString("MenuHelpAbout");

            //tady se neco bude muset dit ohledne dynamickych veci, problem bude
            //i v refresh
        }

        ///<summary>
        ///Forces the whole menu to redefine itself accroding to the new
        ///state in the ProjectManager (enabled/disabled items, actions of
        ///the selected box, etc.)
        ///</summary>
		public void Adapt()
        {
            if (ControlHasFocus is Archive.IArchiveDisplayer)
            {
                SetForArchive();
                return;
            }

            if (ControlHasFocus is ContextHelp.FerdaContextHelp)
            {
                //disable edit and desktop
                edit.Enabled = false;
                //desktop.Enabled = false;
                return;
            }

            if (ControlHasFocus is Properties.FerdaPropertyGrid)
            {
                //disable edit and desktop
                edit.Enabled = false;
                //desktop.Enabled = false;
                return;
            }

            if (ControlHasFocus is NewBox.NewBoxTreeView)
            {
                //disable edit and desktop
                edit.Enabled = false;
                //desktop.Enabled = false;
                return;
            }

            if (ControlHasFocus is IViewDisplayer)
            {
                SetForDesktop();
                return;
            }

            //disable edit and desktop if there is something else selected
            edit.Enabled = false;
            //desktop.Enabled = false;
        }

        #endregion

        /// <summary>
        /// Sets the menu to be used when the archive is focused
        /// </summary>
        protected void SetForArchive()
        {
            //enable edit and desktop
            desktop.Enabled = true;
            edit.Enabled = true;

            IArchiveDisplayer ad = ControlHasFocus as IArchiveDisplayer;

            ContextMenuStrip cMenu = ad.EditMenu;
            edit.DropDownItems.Clear();

            ToolStripItem[] newArray = new ToolStripItem[cMenu.Items.Count];
            cMenu.Items.CopyTo(newArray, 0);

            //there are wrong Click events we have to correct them
            //CorrectDynamicClick(newArray);

            edit.DropDownItems.AddRange(newArray);
        }

        /// <summary>
        /// Sets the menu to be used whed desktop is focused
        /// </summary>
        protected void SetForDesktop()
        {
            //enable edit and desktop
            desktop.Enabled = true;
            edit.Enabled = true;

            IViewDisplayer fd = ControlHasFocus as IViewDisplayer;

            ContextMenuStrip cMenu = fd.EditMenu;
            edit.DropDownItems.Clear();

            ToolStripItem[] newArray = new ToolStripItem[cMenu.Items.Count];
            cMenu.Items.CopyTo(newArray, 0);

            //there are wrong Click events we have to correct them
            CorrectDynamicClick(newArray);

            edit.DropDownItems.AddRange(newArray);
        }

        /// <summary>
        /// The menu passed from the desktop (or archive) contains click events
        /// from that control. Because they wouldnt work, the click events have to be
        /// corrected by this method
        /// </summary>
        /// <param name="items">Array containg the menu items</param>
        protected void CorrectDynamicClick(ToolStripItem[] items)
        {
            foreach (ToolStripItem i in items)
            {
                //we are not doing it for the separators
                if (i is ToolStripMenuItem)
                {
                    if (i.Text == ResManager.GetString("MenuActions"))
                    {
                        ToolStripMenuItem item = i as ToolStripMenuItem;

                        foreach (ToolStripMenuItem item2 in item.DropDownItems)
                        {
                            item2.Click += new EventHandler(Actions_Click);
                        }
                    }

                    if (i.Text == ResManager.GetString("MenuModulesForInteraction"))
                    {
                        ToolStripMenuItem item = i as ToolStripMenuItem;

                        foreach (ToolStripMenuItem item2 in item.DropDownItems)
                        {
                            item2.Click += new EventHandler(Interaction_Click);
                        }
                    }

                    if (i.Text == ResManager.GetString("MenuModulesAskingCreation"))
                    {
                        ToolStripMenuItem item = i as ToolStripMenuItem;

                        foreach (ToolStripMenuItem item2 in item.DropDownItems)
                        {
                            item2.Click += new EventHandler(Creation_Click);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Provides core functionality for saving a Ferda project to a specified
        /// location
        /// </summary>
        /// <param name="fileName">Name of the file where the project should be
        /// saved</param>
        protected void SaveProjectCore(string fileName)
        {
            //saving the project to the location
            System.IO.FileStream fs =
                new System.IO.FileStream(fileName, System.IO.FileMode.Create);
            try
            {
                Cursor previosCursor = Parent.Cursor;
                Parent.Cursor = Cursors.WaitCursor;
                Parent.Refresh();
                projectManager.SaveProject(fs);
                Parent.Cursor = previosCursor;
            }
            finally
            {
                fs.Close();
            }
        }

        /// <summary>
        /// Saves the project as
        /// </summary>
        /// <returns>True if uer clicked Ok in the SaveFile dialog, false otherwise</returns>
        protected bool SaveProjectAsCore()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = ResManager.GetString("MenuFileSaveProject");
            dialog.Filter = ResManager.GetString("MenuFileFerdaProjectFiles")
                + "|*.xfp|" + ResManager.GetString("MenuFileAllFiles")
                + "|*.*";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (dialog.FileName != "")
                {
                    SaveProjectCore(dialog.FileName);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determines if the project contains any boxes.
        /// </summary>
        /// <returns>If there are boxes in the project</returns>
        protected bool ProjectContainsBoxes()
        {
            return (projectManager.Archive.Boxes.Length != 0);
        }

        #endregion

        #region Event handlers

        #region File group

        /// <summary>
        /// Event handles the File->NewProject click
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        /// <remarks>
        /// This method is public, because the toolbar uses it also
        /// </remarks>
        public void newProject_Click(object sender, EventArgs e)
        {
            string caption = ResManager.GetString("MenuFileNewProject");
            string message = ResManager.GetString("MenuFileSaveChanges");

            MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;
            DialogResult result;

            if (ProjectContainsBoxes())
            {
                // Displays the MessageBox.
                result = MessageBox.Show(this, message, caption, buttons,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

                if (result == DialogResult.Cancel) //does nothing
                {
                    return;
                }

                if (result == DialogResult.Yes) //saving the current project
                {
                    if (!SaveProjectAsCore())
                    {
                        return;
                    }
                }
            }

            //creating a new project
            projectManager.NewProject();
            controlsManager.ClearDocking();
            controlsManager.FillPM();
            controlsManager.GlobalAdapt();
        }

        /// <summary>
        /// Event handles the File->OpenProject click. Shows the savefile dialog
        /// and opens the project with the selected file
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        /// <remarks>
        /// This method is public, because the toolbar uses it also
        /// </remarks>
        public void openProject_Click(object sender, EventArgs e)
        {
            //asks if the user wants to save the current project
            string caption = ResManager.GetString("MenuFileOpenProject");
            string message = ResManager.GetString("MenuFileSaveChanges");

            MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;
            DialogResult result;

            if (ProjectContainsBoxes())
            {
                // Displays the MessageBox.
                result = MessageBox.Show(this, message, caption, buttons,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

                if (result == DialogResult.Cancel) //does nothing
                {
                    return;
                }

                if (result == DialogResult.Yes) //saving the current project
                {
                    if (!SaveProjectAsCore())
                    {
                        return;
                    }
                }
            }

            //continuing with opening a project
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = ResManager.GetString("MenuFileOpenProject");
            dialog.Filter = ResManager.GetString("MenuFileFerdaProjectFiles")
                + "|*.xfp|" + ResManager.GetString("MenuFileAllFiles")
                + "|*.*";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = dialog.FileName;

                controlsManager.ClearDocking();
                FrontEndCommon.LoadProject(fileName, this, resManager, 
                    ref projectManager, controlsManager);
            }
        }

        /// <summary>
        /// Event handles the File->SaveProject click
        /// Saves the project
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        /// <remarks>
        /// This method is public, because the toolbar uses it also
        /// </remarks>
        public void saveProject_Click(object sender, EventArgs e)
        {
            if (controlsManager.ProjectName != string.Empty)
            {
                SaveProjectCore(controlsManager.ProjectName);
            }
            else
            {
                SaveProjectAsCore();
            }
        }

        /// <summary>
        /// Event handles the File->Save Project As click
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void saveProjectAs_Click(object sender, EventArgs e)
        {
            SaveProjectAsCore();           
        }

        /// <summary>
        /// Event handles the File->Exit click and closes the
        /// whole application
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        /// <remarks>
        /// This method is public, because the toolbar uses it also
        /// </remarks>
        public void exit_Click(object sender, EventArgs e)
        {
            string caption = ResManager.GetString("MenuFileExit");
            string message = ResManager.GetString("MenuFileSaveChanges");

            MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;
            DialogResult result;

            if (ProjectContainsBoxes())
            {
                // Displays the MessageBox.
                result = MessageBox.Show(this, message, caption, buttons,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

                if (result == DialogResult.Cancel) //does nothing
                {
                    return;
                }

                if (result == DialogResult.Yes) //saving the current project
                {
                    if (!SaveProjectAsCore())
                    {
                        return;
                    }
                }
            }

            Application.Exit();
        }

        #endregion

        #region Edit group

        /// <summary>
        /// Reacts to a click for an action, triggers the action
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void Actions_Click(object sender, EventArgs e)
        {
            IBoxModule box;
            IBoxSelector selector;

            if (ControlHasFocus is IViewDisplayer)
            {
                box = ((IViewDisplayer)ControlHasFocus).SelectedBoxes[0];
                selector = ((FerdaDesktop)ControlHasFocus);
            }
            else //it is an archive
            {
                box = ((IArchiveDisplayer)ControlHasFocus).SelectedBox;
                selector = ((FerdaArchive)ControlHasFocus);
            }

            //run the action
            foreach (ActionInfo info in box.MadeInCreator.Actions)
            {
                if (info.label == sender.ToString())
                {
                    ActionExceptionCatcher catcher =
                        new ActionExceptionCatcher(projectManager, ResManager, selector);
					box.RunAction_async(catcher, info.name);
                    break;
                }
            }
        }

        /// <summary>
        /// Reacts to a click for a module asking for creation and adds the module
        /// to the view
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void Creation_Click(object sender, EventArgs e)
        {
            IBoxModule box;

            if (ControlHasFocus is IViewDisplayer)
            {
                box = ((IViewDisplayer)ControlHasFocus).SelectedBoxes[0];
            }
            else //it is an archive
            {
                box = null;
                return;
            }

            foreach (ModulesAskingForCreation info in box.ModulesAskingForCreation)
            {
                if (info.label == sender.ToString())
                {
                    ((IViewDisplayer)ControlHasFocus).View.CreateBoxesAskingForCreation(info);
                    break;
                }
            }

            archiveDisplayer.Adapt();
            //((IViewDisplayer)ControlHasFocus).Adapt();
        }

        /// <summary>
        /// Reacts to a click for a module for interaction, triggers the module
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void Interaction_Click(object sender, EventArgs e)
        {
            IBoxModule box;

            if (ControlHasFocus is IViewDisplayer)
            {
                box = ((IViewDisplayer)ControlHasFocus).SelectedBoxes[0];
            }
            else //it is an archive
            {
                box = ((IArchiveDisplayer)ControlHasFocus).SelectedBox;
            }

            foreach (ModuleForInteractionInfo info in box.ModuleForInteractionInfos)
            {
                if (info.Label == sender.ToString())
                {
                    box.RunModuleForInteraction(info.IceIdentity);
                    break;
                }
            }

            //Adapt();
        }

        #endregion

        #region View group

        /// <summary>
        /// Event handles the View->Archive click
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void archive_Click(object sender, EventArgs e)
        {
            dockingManager.ShowArchive();
        }

        /// <summary>
        /// Event handles the View->ContextHelp click
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void contextHelp_Click(object sender, EventArgs e)
        {
            dockingManager.ShowContextHelp();
        }

        /// <summary>
        /// Event handles the View->PropertyGrid click
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void propertyGrid_Click(object sender, EventArgs e)
        {
            dockingManager.ShowPropertyGrid();
        }

        /// <summary>
        /// Event handles the View->NewBoxTreeView click
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void newBoxTV_Click(object sender, EventArgs e)
        {
            dockingManager.ShowNewBox();
        }

        /// <summary>
        /// Event handles the View->UserNote click, displays (and docks the UserNote
        /// control)
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void userNote_Click(object sender, EventArgs e)
        {
            dockingManager.ShowUserNote();
        }

        #endregion

        #region Desktop group

        /// <summary>
        /// Event handles the Desktop->New Desktop click. Opens a new desktop
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        /// <remarks>
        /// This method is public, because the toolbar uses it also
        /// </remarks>        
        public void newDesktop_Click(object sender, EventArgs e)
        {
            controlsManager.NewDesktop();

            SetupDesktop();
        }

        /// <summary>
        /// Event handles the Desktop->RemoveDesktopClick. It forces the desktop
        /// to close (if it is opened) and then it is removed from the project
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void removeDesktop_Click(object sender, EventArgs e)
        {
            string name = sender.ToString();

            if (controlsManager.OpenedViews().Contains(name))
            {
                controlsManager.CloseView(name);
            }
            projectManager.RemoveView(name);

            SetupDesktop();
        }

        /// <summary>
        /// Event handles the Desktop->Open Desktop click. It opens a dialog where
        /// the user chooses which view from the project manager to open
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void openDesktop_Click(object sender, EventArgs e)
        {
            string name = sender.ToString();
            controlsManager.OpenView(name);

            SetupDesktop();
        }

        #endregion

        /// <summary>
        /// Event handles the Tools->Preferences click and evokes the
        /// FerdaPreferencesDialog
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        /// <remarks>
        /// This method is public, because the toolbar uses it also
        /// </remarks>
        public void preferences_Click(object sender, EventArgs e)
        {
            FerdaPreferencesDialog prefDialog = new FerdaPreferencesDialog(ResManager, this.localizationManager);
            prefDialog.ShowDialog();
            if (prefDialog.DialogResult == DialogResult.OK)
            {
                localizationManager.LocalePrefs = prefDialog.LocalePrefs;

                MessageBox.Show(ResManager.GetString("LocalizationLabel2"),
                    ResManager.GetString("LocalizationTab"), MessageBoxButtons.OK, 
                    MessageBoxIcon.Exclamation);
            }
        }

        #region Help group

        /// <summary>
        /// Event handles the Help->About click and evokes the
        /// AboutDialog
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void about_Click(object sender, EventArgs e)
        {
            AboutDialog aboutDialog = new AboutDialog(ResManager);
            aboutDialog.ShowDialog();
        }

        /// <summary>
        /// Event handles the Help->ApplicationHelp click and
        /// displayes the help for the application
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void applicationHelp_Click(object sender, EventArgs e)
        {
            string path = FrontEndCommon.GetBinPath();
            path += "\\Help\\" + ResManager.GetString("FerdaUserEnvironment");

            FrontEndCommon.OpenPdf(path, ResManager);
        }

        /// <summary>
        /// Event handles the Help->Turorial click
        /// and displays the tutorial
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void tutorial_Click(object sender, EventArgs e)
        {
            string path = FrontEndCommon.GetBinPath();
            path += "\\Help\\" + ResManager.GetString("FerdaTutorial");

            FrontEndCommon.OpenPdf(path, ResManager);
        }

        #endregion

        #endregion
    }
}
