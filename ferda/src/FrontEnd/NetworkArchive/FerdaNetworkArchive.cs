// FerdaNetworkArchive.cs - control class for Ferda Network Archive
//
// Author: Martin Ralbovský <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2007 Martin Ralbovský
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
using System.ComponentModel;
using System.Windows.Forms;
using System.Resources;
using System.IO;
using System.Drawing;

using Ferda.FrontEnd.Menu;
using Ferda.FrontEnd.Archive;
using Ferda.ModulesManager;
using Ferda.Modules;
using Ferda.NetworkArchive;

namespace Ferda.FrontEnd.NetworkArchive
{
    /// <summary>
    /// Control class for the Ferda Network Archive
    /// </summary>
    public class FerdaNetworkArchive : UserControl, INetworkArchiveDisplayer
    {
        #region Fields

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        ///<summary>
        ///Resource manager from the FerdaForm
        ///</summary>
        private ResourceManager resManager;

        /// <summary>
        /// The main list view
        /// </summary>
        private TreeView tvArchive;

        /// <summary>
        /// The prefernces manager - informs about preferences of the environment,
        /// mainly localization
        /// </summary>
        protected IPreferencesManager preferencesManager;

        /// <summary>
        /// The network archive providing functionality for the
        /// component
        /// </summary>
        protected ProjectManager.NetworkArchive networkArchive;

        /// <summary>
        /// Menu of the application
        /// </summary>
        protected IMenuDisplayer menu;

        /// <summary>
        /// Toolbar of the application
        /// </summary>
        protected IMenuDisplayer toolbar;

        /// <summary>
        /// List of icons of all boxes in the archive
        /// </summary>
        protected ImageList boxIcons;

        /// <summary>
        /// Dictionary that converts the type of the box to a number
        /// for the .NET framework
        /// </summary>
        protected Dictionary<string, int> iconDictionary;

        /// <summary>
        /// Box that was selected in the network archive
        /// </summary>
        protected string selectedBox;

        /// <summary>
        /// Provider of icons (of FrontEnd, not boxes)
        /// </summary>
        protected IIconProvider iconProvider;

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
                        "Archive.ResManager cannot be null");
                }
                return resManager;
            }
        }

        /// <summary>
        /// Context menu for the edit part of the main menu
        /// </summary>
        public ContextMenuStrip EditMenu
        {
            get
            {
                //A menu containing "Remove box from network archive"
                //should be returned, except the case that there is no
                //box in the network archive (or no box is selected)
                if (selectedBox != null)
                {
                    return CreateContextMenu();
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor for the class
        /// </summary>
        /// <param name="prefManager">
        /// Preferences manager for the whole FrontEnd
        /// </param>
        /// <param name="projManager">
        /// The project manager
        /// </param>
        /// <param name="menuDisp">Main menu of the application</param>
        /// <param name="toolBar">The toolbar of the application</param>
        /// <param name="iconprovider">Provider of the icons of the
        /// application</param>
        public FerdaNetworkArchive(IPreferencesManager prefManager, 
            ProjectManager.ProjectManager projManager, 
            IMenuDisplayer menuDisp, IMenuDisplayer toolBar,
            IIconProvider iconprovider)
        {
            preferencesManager = prefManager;
            resManager = preferencesManager.ResManager;
            networkArchive = projManager.NetworkArchive;
            menu = menuDisp;
            toolbar = toolBar;
            iconProvider = iconprovider;

            iconDictionary = new Dictionary<string, int>();
            boxIcons = new ImageList();

            //adding the "no image" icon to the dictionary and the list
            boxIcons.Images.Add(iconProvider.GetIcon("NAIcon"));
            iconDictionary.Add("NAIcon", 0);

            InitializeComponent();
            tvArchive.ImageList = boxIcons;

            Adapt();
        }

        #endregion

        #region Methods

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tvArchive = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // lvArchive
            // 
            this.tvArchive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvArchive.Location = new System.Drawing.Point(0, 0);
            this.tvArchive.Name = "tvArchive";
            this.tvArchive.Size = new System.Drawing.Size(170, 500);
            this.tvArchive.TabIndex = 0;
            // 
            // FerdaNetworkArchive
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tvArchive);
            this.Name = "FerdaNetworkArchive";
            this.Size = new System.Drawing.Size(170, 500);
            this.ResumeLayout(false);

            //event handlers
            GotFocus += new EventHandler(FerdaNetworkArchive_GotFocus);
            tvArchive.GotFocus += new EventHandler(FerdaNetworkArchive_GotFocus);
            tvArchive.AfterSelect += new TreeViewEventHandler(tvArchive_AfterSelect);
            tvArchive.MouseDown += new MouseEventHandler(tvArchive_MouseDown);
            tvArchive.MouseMove += new MouseEventHandler(tvArchive_MouseMove);
        }

        /// <summary>
        /// Changes the size of the child controls. Network archive has to do it
        /// itself, because DockDotNET doesnt support these kinds of events
        /// </summary>
        public void ChangeSize()
        {
            if (Parent != null)
            {
                this.Size = Parent.Size;
            }
        }

        /// <summary>
        /// Creates a context menu for indivual tree nodes items of the
        /// tree view control.
        /// </summary>
        /// <returns>The controls context menu</returns>
        private ContextMenuStrip CreateContextMenu()
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem remove = new ToolStripMenuItem(ResManager.GetString("NetworkArchiveRemove"));
            remove.Click += new EventHandler(remove_Click);
            remove.Image = iconProvider.GetIcon("DeleteFromArchive").ToBitmap();
            menu.Items.Add(remove);

            return menu;
        }

        /// <summary>
        /// The core functionality that removes box from network
        /// archive
        /// </summary>
        protected void RemoveCore()
        {
            networkArchive.RemoveBox(selectedBox);
            selectedBox = null;
            Adapt();

            menu.Adapt();
            toolbar.Adapt();
        }

        #region INetworkArchiveDisplayer implementation

        /// <summary>
        /// Adds Box to a network archive
        /// </summary>
        /// <param name="box">Box module to be added</param>
        /// <param name="label">Label of the box in the network archive</param>
        public void AddBox(IBoxModule box, string label)
        {
            try
            {
                networkArchive.AddBox(box, label);
            }
            catch (NameExistsError)
            {
                MessageBox.Show(ResManager.GetString("NetworkArchiveAlreadyContains"));
            }
        }

        ///<summary>
        ///Forces the control to refresh its state. The method redraws the network
        /// archive + refreshes the image lists iff a new type of icon appeared. 
        ///</summary>
        public void Adapt()
        {
            if (!IsDisposed)
            {
                tvArchive.Nodes.Clear();

                foreach (string label in networkArchive.Labels)
                {
                    NetworkArchiveNode newItem = new NetworkArchiveNode();
                    newItem.Text = label;

                    IBoxModuleFactoryCreator creator =
                        networkArchive.GetBoxModuleFactoryCreatorOfBox(label);
                    newItem.Identifier = creator.Identifier;
                    newItem.ContextMenuStrip = CreateContextMenu();

                    if (!iconDictionary.ContainsKey(newItem.Identifier) &&
                        creator.Icon.Length != 0)
                    {
                    
                        //adding the icon to the dictionary and list view
                        //to the last place in the dictionary
                        MemoryStream stream = new MemoryStream(creator.Icon);
                        Icon icon = new Icon(stream);

                        boxIcons.Images.Add(icon);
                        int iconsCount = iconDictionary.Count;
                        iconDictionary.Add(newItem.Identifier, iconsCount);

                        tvArchive.ImageList = boxIcons;
                        newItem.ImageIndex = iconsCount;
                        newItem.SelectedImageIndex = iconsCount;
                    }
                    else
                    {
                        if (iconDictionary.ContainsKey(newItem.Identifier))
                        {
                            newItem.ImageIndex = iconDictionary[newItem.Identifier];
                            newItem.SelectedImageIndex = iconDictionary[newItem.Identifier];
                        }
                        else
                        {
                            newItem.ImageIndex = iconDictionary["NAIcon"];
                            newItem.SelectedImageIndex = iconDictionary["NAIcon"];
                        }
                    }

                    tvArchive.Nodes.Add(newItem);
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
                throw new ApplicationException("Unexpected caller");
            }

            //we cannot use switch, because there is not a constant expression
            if (bn.ToolTipText == ResManager.GetString("NetworkArchiveRemove"))
            {
                RemoveCore();
            }
        }

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// Event when the archive recieves focus. It forces the menu and toolbar
        /// to adapt
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void FerdaNetworkArchive_GotFocus(object sender, EventArgs e)
        {
            menu.ControlHasFocus = this;
            menu.Adapt();

            toolbar.ControlHasFocus = this;
            toolbar.Adapt();
        }

        /// <summary>
        /// The removal of box from network archive
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void remove_Click(object sender, EventArgs e)
        {
            RemoveCore();
        }

        /// <summary>
        /// Writes that a box has been selected
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void tvArchive_AfterSelect(object sender, TreeViewEventArgs e)
        {
            NetworkArchiveNode node = e.Node as NetworkArchiveNode;
            selectedBox = node.Text;

            menu.Adapt();
            toolbar.Adapt();
        }

        /// <summary>
        /// Reacts on a mouse down-click. This method is used when user
        /// clicks the right button (and so he should select a node). 
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void tvArchive_MouseDown(object sender, MouseEventArgs e)
        {
            TreeNode node = tvArchive.GetNodeAt(e.X, e.Y) as TreeNode;
            if (node != null)
            {
                selectedBox = node.Text;
            }
        }

        /// <summary>
        /// Reacts to a mouse move - prepares the drag&amp;drop operation
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void tvArchive_MouseMove(object sender, MouseEventArgs e)
        {
            //if it was a left mouse button
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                //there is something selected
                if (selectedBox != null)
                {
                    DoDragDrop(selectedBox, DragDropEffects.Copy);
                }
            }
        }

        #endregion
    }
}
