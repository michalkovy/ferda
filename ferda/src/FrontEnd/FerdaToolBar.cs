// FerdaToolBar.cs - toolbar of the application
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
using System.Resources;
using System.Windows.Forms;
using Ferda.FrontEnd.Menu;

namespace Ferda.FrontEnd
{
    /// <summary>
	/// Toolbar for Ferda application, icons of the most important actions
    /// in the project. Consists of static part and dynamic part, that changes 
	/// </summary>
	///<stereotype>control</stereotype>
    public class FerdaToolBar : ToolStrip, IMenuDisplayer
    {
        #region Private fields
        //Resource manager from the FerdaForm
        private ResourceManager resManager;
        /// <summary>
        /// Determines in the FerdaToolBar, which control is now
        /// focused. The Other option stays for ModulesForInteraction and/or
        /// other controls.
        /// </summary>
        private Control controlHasFocus;

        /// <summary>
        /// Localization manager
        /// </summary>
        protected ILocalizationManager localizationManager;
        /// <summary>
        /// Provider of the icons for the toolbar
        /// </summary>
        protected IIconProvider iconProvider;

        //menu that contains all the action handlers
        private Menu.FerdaMenu menu;

        /// Group File
        private ToolStripButton newProject;
        private ToolStripButton openProject;
        private ToolStripButton saveProject;
        private ToolStripButton exit;

        //Other icons
        private ToolStripButton preferences;
        private ToolStripButton newDesktop;

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
                        "Desktop.ResManager cannot be null");
                }
                return resManager;
            }
        }

        /// <summary>
        /// Determines in the FerdaToolBar, which control is now
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

        #endregion

        #region Constructor

        ///<summary>
        /// Default constructor for FerdaToolBar class.
        ///</summary>
        public FerdaToolBar(ILocalizationManager lockManager, IIconProvider provider, 
            Menu.FerdaMenu menu)
            : base()
        {
            this.iconProvider = provider;
            this.menu = menu;

            localizationManager = lockManager;
            ResManager = localizationManager.ResManager;

            SetupFile();
            SetupOther();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Changes the localization of the control
        /// </summary>
        public void ChangeLocalization()
        {
            //updating the resource manager
            ResManager = localizationManager.ResManager;

            //Tooltips
            newProject.ToolTipText = ResManager.GetString("MenuFileNewProject");
            openProject.ToolTipText = ResManager.GetString("MenuFileOpenProject");
            saveProject.ToolTipText = ResManager.GetString("MenuFileSaveProject");
            exit.ToolTipText = ResManager.GetString("MenuFileExit");
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

            if (ControlHasFocus is Desktop.IViewDisplayer)
            {
                SetForDesktop();
                return;
            }

            RemoveDynamicPart();
        }

        /// <summary>
        /// Removes the dynamic part of the items (everything to the left from
        /// preferences)
        /// </summary>
        protected void RemoveDynamicPart()
        {
            //+ 1 means the separator behind the preferences as last piece
            int i = Items.IndexOf(preferences) + 2;
            //removing the rest of the toolbar
            //we have to retrieve the number of items before, because Items.Count
            //counts it dynamically
            int count = Items.Count;
            int j = i; //removing always one index
            for (; i < count; i++)
            {
                Items.RemoveAt(j);
            }
        }

        /// <summary>
        /// Sets the toolbar for archive (archive has focus)
        /// </summary>
        protected void SetForArchive()
        {
            ToolStripButton button;
            //value determining if a menu item contains submenu (thus should not
            //be displayed
            bool hasDropdown = false;

            RemoveDynamicPart();

            //getting the context menu of the archive
            Archive.IArchiveDisplayer ad = ControlHasFocus as 
                Archive.IArchiveDisplayer;
            ContextMenuStrip cMenu = ad.EditMenu;

            foreach (ToolStripItem item in cMenu.Items)
            {
                if (item is ToolStripSeparator)
                {
                    if (!hasDropdown)
                    {
                        Items.Add(new ToolStripSeparator());
                    }
                    else
                    {
                        hasDropdown = false;
                    }
                }
                else
                {
                    //getting the menu item and seeing if it has a submenu
                    ToolStripMenuItem mi = item as ToolStripMenuItem;
                    if (mi.DropDownItems.Count != 0)
                    {
                        //dont display
                        hasDropdown = true;
                    }
                    else
                    {
                        //display
                        hasDropdown = false;
                        button = new ToolStripButton(item.Image);
                        button.Click += new EventHandler(Archive_Click);
                        button.ToolTipText = item.Text;
                        Items.Add(button);
                    }
                }
            }
        }

        /// <summary>
        /// Sets the toolbar for desktop (the desktop has focus)
        /// </summary>
        protected void SetForDesktop()
        {
            ToolStripButton button;
            //value determining if a menu item contains submenu (thus should not
            //be displayed
            bool hasDropdown = false;

            RemoveDynamicPart();

            //getting the context menu of the desktop
            Desktop.IViewDisplayer vd = ControlHasFocus as
                Desktop.IViewDisplayer;
            ContextMenuStrip cMenu = vd.EditMenu;

            foreach (ToolStripItem item in cMenu.Items)
            {
                if (item is ToolStripSeparator)
                {
                    if (!hasDropdown)
                    {
                        Items.Add(new ToolStripSeparator());
                    }
                    else
                    {
                        hasDropdown = false;
                    }
                }
                else
                {
                    //getting the menu item and seeing if it has a submenu
                    ToolStripMenuItem mi = item as ToolStripMenuItem;
                    if (mi.DropDownItems.Count != 0)
                    {
                        //dont display
                        hasDropdown = true;
                    }
                    else
                    {
                        //display
                        hasDropdown = false;
                        button = new ToolStripButton(item.Image);
                        button.Click += new EventHandler(Desktop_Click);
                        button.ToolTipText = item.Text;
                        Items.Add(button);
                    }
                }
            }
        }

        /// <summary>
        /// Sets the file part of the toolbar
        /// </summary>
        protected void SetupFile()
        {
            newProject = new ToolStripButton(iconProvider.GetIcon("NewProject").ToBitmap());
            openProject = new ToolStripButton(iconProvider.GetIcon("OpenProject").ToBitmap());
            saveProject = new ToolStripButton(iconProvider.GetIcon("SaveProject").ToBitmap());
            exit = new ToolStripButton(iconProvider.GetIcon("Exit").ToBitmap());
            ToolStripSeparator sep = new ToolStripSeparator();

            //Tooltips
            newProject.ToolTipText = ResManager.GetString("MenuFileNewProject");
            openProject.ToolTipText = ResManager.GetString("MenuFileOpenProject");
            saveProject.ToolTipText = ResManager.GetString("MenuFileSaveProject");
            exit.ToolTipText = ResManager.GetString("MenuFileExit");

            //Events
            newProject.Click += new EventHandler(menu.newProject_Click);
            openProject.Click += new EventHandler(menu.openProject_Click);
            saveProject.Click += new EventHandler(menu.saveProject_Click);
            exit.Click += new EventHandler(menu.exit_Click);

            Items.AddRange(new ToolStripItem[]
            {
                newProject,
                openProject,
                saveProject,
                exit,
                sep
            });
        }

        /// <summary>
        /// Sets the other icons that are in the menu (preferences and new desktop)
        /// </summary>
        protected void SetupOther()
        {
            preferences = new ToolStripButton(iconProvider.GetIcon("Properties").ToBitmap());
            newDesktop = new ToolStripButton(iconProvider.GetIcon("NewDesktop").ToBitmap());
            ToolStripSeparator sep = new ToolStripSeparator();

            //Tooltips
            preferences.ToolTipText = ResManager.GetString("MenuToolsPreferences");
            newDesktop.ToolTipText = ResManager.GetString("MenuDesktopNewDesktop");

            //Events
            preferences.Click += new EventHandler(menu.preferences_Click);
            newDesktop.Click += new EventHandler(menu.newDesktop_Click);

            Items.AddRange(new ToolStripItem[]
            {
                newDesktop,
                preferences,
                sep
            });
        }

        #endregion

        #region Events

        /// <summary>
        /// If user clicks on the toolbar on some archive action,
        /// this method takes care that proper event is raised
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        private void Archive_Click(object sender, EventArgs e)
        {
            Archive.IArchiveDisplayer arch = ControlHasFocus as
                Archive.IArchiveDisplayer;

            if (arch == null)
            {
                throw new
                    ApplicationException("An archive should be selected - incorrect use of Desktop_Click function");
            }

            arch.RaiseToolBarAction(sender);
        }

        /// <summary>
        /// If user clicks on the toolbar on some desktop action,
        /// this method takes care that proper event is raised
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        private void Desktop_Click(object sender, EventArgs e)
        {
            Desktop.IViewDisplayer view = ControlHasFocus as
                Desktop.IViewDisplayer;

            if (view == null)
            {
                throw new
                    ApplicationException("A view should be selected - incorrect use of Desktop_Click function");
            }

            view.RaiseToolBarAction(sender);
        }

        #endregion
    }
}
