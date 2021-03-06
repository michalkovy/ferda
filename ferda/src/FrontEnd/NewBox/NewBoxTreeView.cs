// NewBoxTreeView.cs - a treeview that displays new Ferda boxes
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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;

using Ferda.ModulesManager;

namespace Ferda.FrontEnd.NewBox
{
    /// <summary>
    /// A NewBoxTreeView is tree view part of the <see cref="T:Ferda.FrontEnd.NewBox.NewBoxControl"/>
    /// class that is used for creating new boxes on the desktop. Class has functionality
    /// to load complicated ProjectManager and ModulesManager structures and to display
    /// the box creators in a tree structure.
    /// </summary>
    public class NewBoxTreeView : TreeView
    {
        #region Class fields

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// The prefernces manager - informs about preferences of the environment,
        /// mainly localization
        /// </summary>
        protected Menu.IPreferencesManager preferencesManager;
        /// <summary>
        /// Menu of the application
        /// </summary>
        protected Menu.IMenuDisplayer menuDisplayer;
        /// <summary>
        /// Toolbar of the application
        /// </summary>
        protected Menu.IMenuDisplayer toolBar;

        /// <summary>
        /// Modules manager of the application
        /// </summary>
        protected ModulesManager.ModulesManager modulesManager;

        //node for drag&drop operations
        private NewBoxNode mySelectedNode;

        /// <summary>
        /// Dictionary that converts the name of the box to a number
        /// for the .NET framework
        /// </summary>
        protected Dictionary<string, int> iconDictionary;

        /// <summary>
        /// Not available icon
        /// </summary>
        protected Icon naIcon;
        /// <summary>
        /// Folder icon
        /// </summary>
        protected Icon folderIcon;

        //constant for determining the height of the textbox
        private int widthConstant;
        //textBox to display the hints
        private RichTextBox textBox;

        #endregion

        #region Properties

        /// <summary>
        /// Constant for determining the height of the textbox
        /// </summary>
        public int WidthConstant
        {
            get { return widthConstant; }
            set { widthConstant = value; }
        }

        /// <summary>
        /// textBox to display the hints
        /// </summary>
        public RichTextBox TextBox
        {
            get { return textBox; }
            set { textBox = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="prefManager">
        /// The prefernces manager - informs about preferences of the environment,
        /// mainly localization
        ///</param>
        /// <param name="menuDisp">Menu of the application</param>
        /// <param name="modManager">Modules manager</param>
        /// <param name="iconProvider">Provider of the icons for the component</param>
        /// <param name="toolBar">ToolBar of the application</param>
        public NewBoxTreeView(Menu.IPreferencesManager prefManager, 
            Menu.IMenuDisplayer menuDisp, ModulesManager.ModulesManager modManager, 
            IIconProvider iconProvider, Menu.IMenuDisplayer toolBar)
            : base()
        {
            //setting the icons
            naIcon = iconProvider.GetIcon("NAIcon");
            folderIcon = iconProvider.GetIcon("FolderIcon");

            preferencesManager = prefManager;
            menuDisplayer = menuDisp;
            this.toolBar = toolBar;
            modulesManager = modManager;

            InitializeComponent();

            IBoxModuleFactoryCreator[] creators = modulesManager.BoxModuleFactoryCreators;

            FillImageList(creators);
            FillTreeView(creators);

            AfterSelect += new TreeViewEventHandler(NewBoxTreeView_AfterSelect);
            GotFocus += new EventHandler(NewBox_GotFocus);
            MouseMove += new MouseEventHandler(NewBoxTreeView_MouseMove);
            MouseDown += new MouseEventHandler(NewBoxTreeView_MouseDown);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fills the image list of the tree view and the dictionary for
        /// converting category names into icons.
        /// </summary>
        /// <param name="creators">creators of boxes in the project</param>
        protected void FillImageList(IBoxModuleFactoryCreator[] creators)
        {
            int j = 0;

            MemoryStream stream;
            Icon icon;

            ImageList list = new ImageList();
            iconDictionary = new Dictionary<string, int>();

            for (int i = 0; i < creators.Length; i++)
            {
                if (creators[i].Icon.Length != 0)
                {
                    stream = new MemoryStream(creators[i].Icon);
                    icon = new Icon(stream);
                    list.Images.Add(icon);
                    iconDictionary.Add(creators[i].Identifier, j);
                    j++;
                }
            }

            //adding the folder and NA icon
            list.Images.Add(naIcon);
            iconDictionary.Add("noicon", list.Images.Count - 1);
            list.Images.Add(folderIcon);
            iconDictionary.Add("foldericon", list.Images.Count - 1);

            //adding the list to the control
            this.ImageList = list;
        }

        /// <summary>
        /// Changes the size of the child controls. Archive has to do it 
        /// itself, because DockDotNET doesnt support these kinds of events
        /// </summary>
        public void ChangeSize()
        {
            if (Parent != null)
            {
                this.Location = new Point(2, 2);
                this.Size = new System.Drawing.Size(
                    Parent.Size.Width, Parent.Size.Height - WidthConstant);
            }
        }

        /// <summary>
        /// Fills the treeview with box types
        /// </summary>
        /// <param name="creators">creators of boxes in the project</param>
        protected void FillTreeView(IBoxModuleFactoryCreator[] creators)
        {
            List<string> allCategories = new List<string>();

            //finding all the level 1 categories
            foreach (IBoxModuleFactoryCreator creator in creators)
            {
                foreach (string category in creator.BoxCategories)
                {
                    if (!allCategories.Contains(category))
                    {
                        allCategories.Add(category);
                    }
                }
            }
            allCategories.Sort();

            foreach (string cat in allCategories)
            {
                if (cat.Contains("."))
                {
                    CreateDottedTree(cat, creators);
                }
                else
                {
                    FillCategory(cat, creators, null);
                }
            }
        }

        /// <summary>
        /// Creates a tree from a label that contains dots
        /// </summary>
        /// <param name="cat">Label of the category</param>
        /// <param name="creators">Creators of the boxes</param>
        protected void CreateDottedTree(string cat, IBoxModuleFactoryCreator[] creators)
        {
            NewBoxNode tn = null;
            NewBoxNode nextNode;
            string beginning;
            bool contains = false;

            //the first occurence is dealt separately, because we have to add directly
            //to the control
            beginning = cat.Substring(0, cat.IndexOf('.'));
            cat = cat.Substring(cat.IndexOf('.') + 1);

            //checking if the treeNode is there already
            foreach (NewBoxNode n in Nodes)
            {
                if (n.Text == beginning)
                {
                    contains = true;
                    tn = n;
                    break;
                }
            }
            if (!contains)
            {
                nextNode = new NewBoxNode(beginning, ENodeType.Category, String.Empty);
                nextNode.ImageIndex = iconDictionary["foldericon"];
                nextNode.SelectedImageIndex = iconDictionary["foldericon"];
                Nodes.Add(nextNode);
                tn = nextNode;
            }

            //all other dots
            while (cat.Contains("."))
            {
                contains = false;
                beginning = cat.Substring(0, cat.IndexOf('.'));
                cat = cat.Substring(cat.IndexOf('.') + 1);

                //checking if the treeNode is there already
                foreach (NewBoxNode n in tn.Nodes)
                {
                    if (n.Text == beginning)
                    {
                        contains = true;
                        tn = n;
                        break;
                    }
                }
                if (!contains)
                {
                    nextNode = new NewBoxNode(beginning, ENodeType.Category, String.Empty);
                    nextNode.ImageIndex = iconDictionary["foldericon"];
                    nextNode.SelectedImageIndex = iconDictionary["foldericon"];
                    tn.Nodes.Add(nextNode);
                    tn = nextNode;
                }
            }

            //adding the last element that's left (shouldn't be there already,
            //because FillTreeView chooses only distinct category names
            //tn.Nodes.Add(cat);
            FillCategory(cat, creators, tn);
        }

        /// <summary>
        /// Fills a particular category and all the boxes that belong
        /// to that category
        /// </summary>
        /// <param name="catName">name of the category</param>
        /// <param name="creators">Creators of the boxes</param>
        /// <param name="baseNode">The node where a new node should be created
        /// It is a null, when it should be created to the control itself</param>
        protected void FillCategory(string catName, IBoxModuleFactoryCreator[] creators, NewBoxNode baseNode)
        {
            bool contains;
            List<IBoxModuleFactoryCreator> newNodes = new List<IBoxModuleFactoryCreator>();

            NewBoxNode tn = new NewBoxNode(catName, ENodeType.Category, String.Empty);
            tn.ImageIndex = iconDictionary["foldericon"];
            tn.SelectedImageIndex = iconDictionary["foldericon"];
            if (baseNode == null)
            {
                Nodes.Add(tn);
            }
            else
            {
                baseNode.Nodes.Add(tn);
            }
            string catFullName = GetFullCategoryName(catName, tn.Parent);

            //iterating through all the creators we have
            foreach (IBoxModuleFactoryCreator creator in creators)
            {
                contains = false;

                foreach (string category in creator.BoxCategories)
                {
                    if (category == catFullName)
                    {
                        contains = true;
                        break;
                    }
                }

                if (contains)
                {
                    newNodes.Add(creator);
                }
            }

            IBoxModuleFactoryCreatorComparer comp = new IBoxModuleFactoryCreatorComparer();
            newNodes.Sort(comp);

            foreach (IBoxModuleFactoryCreator c in newNodes)
            {
                NewBoxNode node = new NewBoxNode(c.Label, ENodeType.Box, c.Identifier);
                if (iconDictionary.ContainsKey(c.Identifier))
                {
                    node.ImageIndex = iconDictionary[c.Identifier];
                    node.SelectedImageIndex = iconDictionary[c.Identifier];
                }
                else
                {
                    node.ImageIndex = iconDictionary["noicon"];
                    node.SelectedImageIndex = iconDictionary["noicon"];
                }
                tn.Nodes.Add(node);
            }
        }

        /// <summary>
        /// Retrieves a full name of the category (the full path including all the
        /// parents names)
        /// </summary>
        /// <param name="catName">name of the category ending</param>
        /// <param name="baseNode">the parent node, null if the parent is the control
        /// </param>
        /// <returns>full name of the category (the full path including all the
        /// parents names)</returns>
        protected string GetFullCategoryName(string catName, TreeNode baseNode)
        {
            string result = catName;

            if (baseNode == null)
            {
                return catName;
            }

            while (baseNode.Parent != null)
            {
                result = baseNode.Text + "." + result;
                baseNode = baseNode.Parent;
            }

            result = baseNode.Text + "." + result;
            return result;
        }

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

        #endregion

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.Size = new System.Drawing.Size(170, 500);
        }

        #endregion

        #region Events

        /// <summary>
        /// Event when the newboxTreeView receives focus. It forces the menu
        /// to adapt
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        protected void NewBox_GotFocus(object sender, EventArgs e)
        {
            menuDisplayer.ControlHasFocus = this;
            menuDisplayer.Adapt();
            toolBar.ControlHasFocus = this;
            toolBar.Adapt();
        }


        /// <summary>
        /// Reacts to a mouse move - prepares the drag&amp;drop operation
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void NewBoxTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            NewBoxNode node = e.Node as NewBoxNode;

            if (node != null)
            {
                if (node.NodeType == ENodeType.Box)
                {
                    IBoxModuleFactoryCreator[] creators = 
                        modulesManager.BoxModuleFactoryCreators;

                    foreach (IBoxModuleFactoryCreator creator in creators)
                    {
                        if (creator.Identifier == node.Identifier)
                        {
                            TextBox.Text = creator.Hint;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Reacts to a mouse move - prepares the drag&amp;drop operation
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void NewBoxTreeView_MouseMove(object sender, MouseEventArgs e)
        {
            //if it was a left mouse button
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                //there is something selected
                if (mySelectedNode != null)
                {
                    //the selected node is a second level node (not a category)
                    if (mySelectedNode.NodeType == ENodeType.Box)
                    {
                        DoDragDrop(mySelectedNode, DragDropEffects.Copy);
                    }
                }
            }
        }

        /// <summary>
        /// Reaction to a mouse down - the mySelectedNode gets initialized
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void NewBoxTreeView_MouseDown(object sender, MouseEventArgs e)
        {
            //if it was a left mouse button
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                mySelectedNode = (NewBoxNode)GetNodeAt(e.X, e.Y);
            }
        }

        #endregion
    }
}
