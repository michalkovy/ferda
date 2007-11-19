// FerdaTreeNode.cs - This class is a classical TreeNode with a info about
// the IBoxModule that this tree node represents.
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
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Resources;

using Ferda.ProjectManager;

namespace Ferda.FrontEnd.Archive
{
    /// <summary>
    /// The class is a classical TreeNode with a info about the IBoxModule that
    /// this tree node represents
    /// </summary>
    public class FerdaTreeNode : TreeNode
    {
        #region Class fields

        private ModulesManager.IBoxModule box;
        private bool alongDirection;
        private ProjectManager.Archive archive;
        /// <summary>
        /// Project manager of the application
        /// </summary>
        protected ProjectManager.ProjectManager projectManager;
        private FerdaArchive parentTreeView;
        /// <summary>
        /// Provider of icons
        /// </summary>
        protected IIconProvider provider;

        #endregion

        #region Properties

        /// <summary>
        /// The box node of the treenode
        /// </summary>
        public ModulesManager.IBoxModule Box
        {
            set
            {
                box = value;
            }
            get
            {
                return box;
            }
        }

        /// <summary>
        /// The direction in which the archive will be expanded
        /// </summary>
        public bool AlongDirection
        {
            set
            {
                alongDirection = value;
            }
            get
            {
                return alongDirection;
            }
        }

        /// <summary>
        /// The box belongs to this archive
        /// </summary>
        public ProjectManager.Archive Archive
        {
            set
            {
                archive = value;
            }
            get
            {
                return archive;
            }
        }

        /// <summary>
        /// The archive in which this this node is contained
        /// </summary>
        public FerdaArchive ParentTreeView
        {
            set
            {
                parentTreeView = value;
            }
            get
            {
                return parentTreeView;
            }
        }

        /// <summary>
        /// Function returns the context menu of the node
        /// </summary>
        public ContextMenuStrip EditMenu
        {
            get
            {
                SetContextMenu();
                return ContextMenuStrip;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor that sets the Box property
        /// </summary>
        /// <param name="box">box that will be represented by this node</param>
        /// <param name="alongDirection">the direction of expanding</param>
        /// <param name="arch">Archive where this box belongs</param>
        /// <param name="parent">Parent component of the archive</param>
        /// <param name="iconDictionary">Icon dictionary containing all the icons
        /// for the box visualization</param>
        /// <param name="list">Some image list</param>
        /// <param name="provider">Control providing access to all the icons</param>
        /// <param name="projManager">Project manager of the application</param>
        public FerdaTreeNode(ModulesManager.IBoxModule box, bool alongDirection, 
            ProjectManager.Archive arch, FerdaArchive parent, Dictionary<string, int> iconDictionary,
            ImageList list, IIconProvider provider, ProjectManager.ProjectManager projManager)
            : this(box, alongDirection,
            arch, parent, iconDictionary, list, provider, projManager, new List<ModulesManager.IBoxModule>())
        {
        }
            

        /// <summary>
        /// Constructor that sets the Box property
        /// </summary>
        /// <param name="box">box that will be represented by this node</param>
        /// <param name="alongDirection">the direction of expanding</param>
        /// <param name="arch">Archive where this box belongs</param>
        /// <param name="parent">Parent component of the archive</param>
        /// <param name="iconDictionary">Icon dictionary containing all the icons
        /// for the box visualization</param>
        /// <param name="list">Some image list</param>
        /// <param name="provider">Control providing access to all the icons</param>
        /// <param name="projManager">Project manager of the application</param>
        /// <param name="usedBoxModules">Used box modules in recursion - for stopping recursion when in cycle</param>
        public FerdaTreeNode(ModulesManager.IBoxModule box, bool alongDirection, 
            ProjectManager.Archive arch, FerdaArchive parent, Dictionary<string, int> iconDictionary,
            ImageList list, IIconProvider provider, ProjectManager.ProjectManager projManager, List<ModulesManager.IBoxModule> usedBoxModules)
            : base()
        {
            Box = box;
            Text = box.UserName;
            AlongDirection = alongDirection;
            Archive = arch;
            ParentTreeView = parent;

            //icon provider for the menu
            this.provider = provider;

            //setting the projectManager
            projectManager = projManager;

            //add box module for stopping recursion
            usedBoxModules.Add(box);

            //setting the icon
            if (box.MadeInCreator.Icon.Length == 0)
            {
                this.ImageIndex = iconDictionary["naIcon"];
                this.SelectedImageIndex = iconDictionary["naIcon"];
            }
            else
            {
                string label = box.MadeInCreator.Label;

                if (iconDictionary.ContainsKey(label))
                {
                    this.ImageIndex = iconDictionary[label];
                    this.SelectedImageIndex = iconDictionary[label];
                }
                else
                {
                    MemoryStream stream = new MemoryStream(box.MadeInCreator.Icon);
                    Icon icon = new Icon(stream);
                    int count = list.Images.Count;
                    list.Images.Add(icon);
                    iconDictionary.Add(label, count);

                    this.ImageIndex = count;
                    this.SelectedImageIndex = count;
                }
            }

            //sets the child nodes for this node
            if (AlongDirection)
            {
                //sets the boxes as ConnectionTo
                foreach(ModulesManager.IBoxModule b in Archive.ConnectedTo(Box))
                {
                    if (!usedBoxModules.Contains(b))
                    {
                        Nodes.Add(new FerdaTreeNode(b, AlongDirection, Archive,
                            ParentTreeView, iconDictionary, list, provider, projectManager, usedBoxModules));
                    }
                }
            }
            else
            {
                //sets the boxes as ConnectionFrom
                foreach(ModulesManager.IBoxModule b in Archive.ConnectionsFrom(Box))
                {
                    if (!usedBoxModules.Contains(b))
                    {
                        Nodes.Add(new FerdaTreeNode(b, AlongDirection, Archive,
                            ParentTreeView, iconDictionary, list, provider, projectManager, usedBoxModules));
                    }
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the contextMenu for the box
        /// </summary>
        public void SetContextMenu()
        {
            this.ContextMenu = null;

            ContextMenuStrip cMenu = new ContextMenuStrip();

            SetDynamicContextMenu(cMenu);
            SetStaticContextMenu(cMenu, provider);

            this.ContextMenuStrip = cMenu;
        }

        /// <summary>
        /// Fills the dynamic fields of context menu for the node
        /// </summary>
        /// <param name="cMenu">Context menu of the TreeNode</param>
        protected void SetDynamicContextMenu(ContextMenuStrip cMenu)
        {
            bool containsDynamic = false;

            //creating dynamic parts of the menu
            if (box.MadeInCreator.Actions.Length != 0)
            {
                containsDynamic = true;

                ToolStripMenuItem actions =
                    new ToolStripMenuItem(ParentTreeView.ResManager.GetString("MenuActions"));
                CreateActionsMenu(actions);
                cMenu.Items.Add(actions);
            }

            if (box.ModuleForInteractionInfos.Length != 0)
            {
                containsDynamic = true;

                ToolStripMenuItem modulesForInteraction =
                    new ToolStripMenuItem(ParentTreeView.ResManager.GetString("MenuModulesForInteraction"));
                CreateModulesForInteractionMenu(modulesForInteraction);
                cMenu.Items.Add(modulesForInteraction);
            }

            if (containsDynamic)
            {
                ToolStripSeparator sep0 = new ToolStripSeparator();
                cMenu.Items.Add(sep0);
            }
        }

        /// <summary>
        /// Fills the static fields of context menu for the node
        /// </summary>
        /// <param name="cMenu">Context menu of the TreeNode</param>
        /// <param name="provider">Interface that provides icons for the boxes</param>
        protected void SetStaticContextMenu(ContextMenuStrip cMenu, IIconProvider provider)
        {
            //handling normal items
            ToolStripMenuItem rename =
                new ToolStripMenuItem(ParentTreeView.ResManager.GetString("MenuEditRename"));
            ToolStripMenuItem copy =
                new ToolStripMenuItem(ParentTreeView.ResManager.GetString("MenuEditCopy"));
            ToolStripMenuItem clone =
                new ToolStripMenuItem(ParentTreeView.ResManager.GetString("MenuEditClone"));
            ToolStripMenuItem validate =
                new ToolStripMenuItem(ParentTreeView.ResManager.GetString("MenuEditValidate"));

            copy.Click += new EventHandler(copy_Click);
            rename.Click += new EventHandler(rename_Click);
            clone.Click += new EventHandler(clone_Click);
            validate.Click += new EventHandler(validate_Click);

            //shortcuts
            rename.ShortcutKeys = Keys.F2;
            copy.ShortcutKeys = (Keys)Shortcut.CtrlC;
            clone.ShortcutKeys = (Keys)Shortcut.CtrlE;
            validate.ShortcutKeys = (Keys)Shortcut.CtrlQ;

            //icons
            rename.Image = provider.GetIcon("Rename").ToBitmap();
            copy.Image = provider.GetIcon("Copy").ToBitmap();
            clone.Image = provider.GetIcon("Clone").ToBitmap();
            validate.Image = provider.GetIcon("Validate").ToBitmap();

            cMenu.Items.AddRange(new ToolStripItem[] {rename, copy, clone, validate });

            if (!ParentTreeView.Clipboard.IsEmpty)
            {
                ToolStripMenuItem paste =
                    new ToolStripMenuItem(ParentTreeView.ResManager.GetString("MenuEditPaste"));
                paste.Click += new EventHandler(paste_Click);
                paste.ShortcutKeys = (Keys)Shortcut.CtrlV;
                paste.Image = provider.GetIcon("Paste").ToBitmap();
                cMenu.Items.Add(paste);
            }

            ToolStripSeparator sep = new ToolStripSeparator();
            ToolStripMenuItem delete =
                new ToolStripMenuItem(ParentTreeView.ResManager.GetString("MenuArchiveDelete"));
            delete.Click += new EventHandler(delete_Click);
            delete.ShortcutKeys = (Keys)Shortcut.ShiftDel;
            delete.Image = provider.GetIcon("DeleteFromArchive").ToBitmap();
            cMenu.Items.AddRange(new ToolStripItem[] { sep, delete });
        }

        /// <summary>
        /// Creates actions submenu for a selected box
        /// </summary>
        protected void CreateActionsMenu(ToolStripMenuItem act)
        {
            ToolStripMenuItem item;

            List<ToolStripMenuItem> actions = new List<ToolStripMenuItem>();

            foreach (Modules.ActionInfo info in box.MadeInCreator.Actions)
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
        protected void CreateModulesForInteractionMenu(ToolStripMenuItem mod)
        {
            ToolStripMenuItem item;

            List<ToolStripMenuItem> modules = new List<ToolStripMenuItem>();

            foreach (ModulesManager.ModuleForInteractionInfo info in box.ModuleForInteractionInfos)
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

        /*
        /// <summary>
        /// Creates ModulesForCreationSubmenu for a selected box
        /// </summary>
        protected void CreateModulesForCreationMenu(ToolStripMenuItem menuItem)
        {
            ToolStripMenuItem it;

            List<ToolStripMenuItem> modules = new List<ToolStripMenuItem>();

            foreach (Modules.ModuleAskingForCreation info in box.ModulesAskingForCreation)
            {
                it = new ToolStripMenuItem(info.label);
                it.Click += new EventHandler(Creation_Click);

                modules.Add(it);
            }

            foreach (ToolStripMenuItem item in modules)
            {
                menuItem.DropDownItems.Add(item);
            }
        }*/

        /// <summary>
        /// Shows a messagebox saying that user cannot write to the box
        /// </summary>
        public void CannotWriteToBox(ModulesManager.IBoxModule box)
        {
            MessageBox.Show(
                ParentTreeView.ResManager.GetString("PropertiesCannotWriteText"),
                box.UserName + ": " + ParentTreeView.ResManager.GetString("PropertiesCannotWriteCaption"));
        }

        /// <summary>
        /// Refreshes the name (text) of the TreeNode
        /// </summary>
        public void RefreshName()
        {
            Text = Box.UserName;
            foreach (FerdaTreeNode tn in Nodes)
            {
                tn.RefreshName();
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Renames a box by calling the rename dialog
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void rename_Click(object sender, EventArgs e)
        {
            ParentTreeView.Rename(this);
        }

        /// <summary>
        /// Copies the box to the clipboard
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void copy_Click(object sender, EventArgs e)
        {
            ParentTreeView.Copy(Box);
        }

        /// <summary>
        /// Clones the box to the archive
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void clone_Click(object sender, EventArgs e)
        {
            ParentTreeView.Clone(Box);
        }

        /// <summary>
        /// Deletes the box from the archive
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void delete_Click(object sender, EventArgs e)
        {
            ParentTreeView.Delete(Box);
        }

        /// <summary>
        /// Pastes the box to the archive (actually creates a clone of the box(es)
        /// in the clipboard
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void paste_Click(object sender, EventArgs e)
        {
            ParentTreeView.Paste();
        }

        /// <summary>
        /// Validates the selected box
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void validate_Click(object sender, EventArgs e)
        {
            ParentTreeView.Validate(Box);
        }

        /// <summary>
        /// Reacts to a click for an action, triggers the action
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void Actions_Click(object sender, EventArgs e)
        {
            ModulesManager.IBoxModule box = ParentTreeView.SelectedBox;

            //run the action
            foreach (Modules.ActionInfo info in box.MadeInCreator.Actions)
            {
                if (info.label == sender.ToString())
                {
                    ActionExceptionCatcher catcher =
                        new ActionExceptionCatcher(projectManager, ParentTreeView.ResManager, 
                        parentTreeView, parentTreeView.PropertiesDisplayer);
					box.RunAction_async(catcher, info.name);
                    break;
                }
            }

            //archiveDisplayer.Adapt(); i think there is no need for this in here
        }

        /*
        /// <summary>
        /// Reacts to a click for a module asking for creation and adds the module
        /// to the view
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void Creation_Click(object sender, EventArgs e)
        {
            foreach (Modules.ModuleAskingForCreation info in box.ModulesAskingForCreation)
            {
                if (info.label == sender.ToString())
                {
                    view.CreateBoxAskingForCreation(info);
                    break;
                }
            }

            ParentTreeView.Adapt();
        }
        */

        /// <summary>
        /// Reacts to a click for a module for interaction, triggers the module
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void Interaction_Click(object sender, EventArgs e)
        {
            ModulesManager.IBoxModule box = ParentTreeView.SelectedBox;

            foreach (ModulesManager.ModuleForInteractionInfo info in box.ModuleForInteractionInfos)
            {
                if (info.Label == sender.ToString())
                {
                    box.RunModuleForInteraction(info.IceIdentity);
                    break;
                }
            }
        }

        #endregion
    }
}
