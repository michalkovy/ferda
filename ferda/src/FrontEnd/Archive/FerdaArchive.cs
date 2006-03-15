// FerdaArchive.cs - control class for Ferda Archive
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
using System.Drawing;
using System.Collections.Generic;
using System.Collections.Specialized;

using Ferda.FrontEnd.Menu;
using Ferda.FrontEnd;
using Ferda.FrontEnd.Desktop;
using Ferda.ProjectManager;
using Ferda.ModulesManager;

namespace Ferda.FrontEnd.Archive
{
    /// <summary>
    /// Control class for Ferda archive
    /// </summary>
    ///<stereotype>control</stereotype>
    public class FerdaArchive : System.Windows.Forms.UserControl, IArchiveDisplayer,
        IBoxSelector
    {
        #region Fields

        /// <summary>
        /// The localization manager - takes care of localizing
        /// </summary>
        protected Menu.ILocalizationManager localizationManager;
        /// <summary>
        /// Menu of the application
        /// </summary>
        protected Menu.IMenuDisplayer menuDisplayer;
        /// <summary>
        /// Toolbar of the application
        /// </summary>
        protected Menu.IMenuDisplayer toolBar;

        private Properties.IPropertiesDisplayer propertiesDisplayer;
        private ContextHelp.IContextHelpDisplayer contextHelpDisplayer;
        /// <summary>
        /// Archive part of the control
        /// </summary>
        protected TreeView TVArchive;
        /// <summary>
        /// Radio button for moving along the connections
        /// </summary>
        protected RadioButton RBAlong;
        /// <summary>
        /// Radio button for moving against the connections
        /// </summary>
        protected RadioButton RBAgainst;

        //Archive that will set the class
        private ProjectManager.Archive archive;

        /// <summary>
        /// Combo box to display the box categories
        /// </summary>
        protected ComboBox CBCategories;

        //ProjectManger to display the messages about the box to the user
        private ProjectManager.ProjectManager projectManager;

        //the selected box in the archive
        private ModulesManager.IBoxModule selectedBox;

        //Resource manager from the FerdaForm
        private ResourceManager resManager;

        //Clipboard
        private IFerdaClipboard clipboard;

        //Selected node for the drag&drop operations
        private FerdaTreeNode mySelectedNode;

        //All the views of the project
        private List<FerdaDesktop> views;
        
        /// <summary>
        /// Icon for the boxes that don't have their icon
        /// </summary>
        protected Icon naIcon;

        //userNote control
        private UserNote.IUserNoteDisplayer userNote;

        /// <summary>
        /// Icon provider for the application
        /// </summary>
        protected IIconProvider iconProvider;

        /// <summary>
        /// Combo box that displays the types of boxes
        /// </summary>
        protected ComboBox CBTypes;

        /// <summary>
        /// Dictionary that converts the name of the box to a number
        /// for the .NET framework
        /// </summary>
        protected Dictionary<string, int> iconDictionary;

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

        ///<summary>
        ///The box currently selected in the archive
        ///</summary>
        public ModulesManager.IBoxModule SelectedBox
        {
            set
            {
                selectedBox = value;
            }
            get
            {
                return selectedBox;
            }
        }

        /// <summary>
        /// Archive of the control
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
        /// Context Help displayer of the application
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
        /// Context menu for the edit part of the main menu
        /// </summary>
        public ContextMenuStrip EditMenu
        {
            get
            {
                if (mySelectedNode != null)
                {
                    return mySelectedNode.EditMenu;
                }
                else
                {
                    if (TVArchive.SelectedNode != null)
                    {
                        FerdaTreeNode tn = TVArchive.SelectedNode as FerdaTreeNode;
                        return tn.EditMenu;
                    }
                    else
                    {
                        return new ContextMenuStrip();
                    }
                }
            }
        }

        #endregion

        #region Constructor

        ///<summary>
        /// Default constructor for FerdaArchive class.
        ///</summary>
        public FerdaArchive(Menu.ILocalizationManager locManager,
            Menu.IMenuDisplayer menuDisp, IFerdaClipboard clipboard,
            ProjectManager.Archive arch, IIconProvider iconProvider,
            Menu.IMenuDisplayer toolBar,
            ProjectManager.ProjectManager projManager)
            : base()
        {
            //setting the icon
            naIcon = iconProvider.GetIcon("NAIcon");
            this.iconProvider = iconProvider;

            //setting the localization
            localizationManager = locManager;
            ResManager = localizationManager.ResManager;

            //setting the menu displayer
            menuDisplayer = menuDisp;
            this.toolBar = toolBar;

            //setting the archive
            Archive = arch;

            //setting the projectManager
            projectManager = projManager;

            InitializeComponent();

            //Setting the texts of the radiobuttons
            RBAlong.Text = ResManager.GetString("ArchiveAlongText");
            RBAgainst.Text = ResManager.GetString("ArchiveAgainstText");
            RBAlong.Checked = true;

            //Setting the clipboard
            this.Clipboard = clipboard;

            //events
            GotFocus += new EventHandler(FerdaArchive_GotFocus);
            TVArchive.GotFocus += new EventHandler(FerdaArchive_GotFocus);
            TVArchive.MouseDown += new MouseEventHandler(TVArchive_MouseDown);
            TVArchive.MouseMove += new MouseEventHandler(TVArchive_MouseMove);

            //the labelEdit property
            TVArchive.LabelEdit = true;
            TVArchive.AfterLabelEdit += new NodeLabelEditEventHandler(TVArchive_AfterLabelEdit);

            CBCategories.SelectedIndexChanged += new EventHandler(CBArchive_SelectedIndexChanged);
            CBTypes.SelectedIndexChanged += new EventHandler(CBTypes_SelectedIndexChanged);
            RBAlong.CheckedChanged += new EventHandler(RBAlong_CheckedChanged);
            TVArchive.AfterSelect += new TreeViewEventHandler(TVArchive_AfterSelect);

            ArchiveSetupAfterLoadProject();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the archive when the archive object is changed
        /// (when the project is loaded)
        /// </summary>
        public void ArchiveSetupAfterLoadProject()
        {
            //fill the box types
            FillBoxTypes();

            //resets the archive to an "initial position"
            //ResetArchive(ResManager.GetString("ArchiveAllText"), true);
            if (CBCategories.Items.Count > 1)
            {
                ResetArchive(CBCategories.Items[1].ToString(),
                    ResManager.GetString("ArchiveAllText"), true);
                CBCategories.SelectedIndex = 1;
            }
            else
            {
                ResetArchive(CBCategories.Items[0].ToString(),
                    ResManager.GetString("ArchiveAllText"), true);
                CBCategories.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Reffils the context menus of all the nodes
        /// </summary>
        public void RefillContextMenus()
        {
            ContextMenuStrip cMenu = new ContextMenuStrip();
            //setting the context menu of all the nodes
            foreach (TreeNode n in TVArchive.Nodes)
            {
                FerdaTreeNode ftn = n as FerdaTreeNode;
                ftn.SetContextMenu();
            }

            this.ContextMenuStrip = cMenu;
        }

        private void InitializeComponent()
        {
            this.TVArchive = new System.Windows.Forms.TreeView();
            this.RBAlong = new System.Windows.Forms.RadioButton();
            this.RBAgainst = new System.Windows.Forms.RadioButton();
            this.CBCategories = new System.Windows.Forms.ComboBox();
            this.CBTypes = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // TVArchive
            // 
            this.TVArchive.FullRowSelect = true;
            this.TVArchive.HotTracking = true;
            this.TVArchive.Location = new System.Drawing.Point(0, 44);
            this.TVArchive.Name = "TVArchive";
            this.TVArchive.Size = new System.Drawing.Size(140, 380);
            this.TVArchive.TabIndex = 1;
            // 
            // RBAlong
            // 
            this.RBAlong.AutoSize = true;
            this.RBAlong.Location = new System.Drawing.Point(1, 425);
            this.RBAlong.Name = "RBAlong";
            this.RBAlong.Size = new System.Drawing.Size(85, 17);
            this.RBAlong.TabIndex = 2;
            this.RBAlong.Text = "radioButton1";
            // 
            // RBAgainst
            // 
            this.RBAgainst.AutoSize = true;
            this.RBAgainst.Location = new System.Drawing.Point(1, 442);
            this.RBAgainst.Name = "RBAgainst";
            this.RBAgainst.Size = new System.Drawing.Size(85, 17);
            this.RBAgainst.TabIndex = 3;
            this.RBAgainst.Text = "radioButton2";
            // 
            // CBCategories
            // 
            this.CBCategories.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBCategories.FormattingEnabled = true;
            this.CBCategories.Location = new System.Drawing.Point(0, 0);
            this.CBCategories.Name = "CBCategories";
            this.CBCategories.Size = new System.Drawing.Size(140, 22);
            this.CBCategories.TabIndex = 0;
            // 
            // CBTypes
            // 
            this.CBTypes.FormattingEnabled = true;
            this.CBTypes.Location = new System.Drawing.Point(0, 22);
            this.CBTypes.Name = "CBTypes";
            this.CBTypes.Size = new System.Drawing.Size(140, 22);
            this.CBTypes.TabIndex = 4;
            // 
            // FerdaArchive
            // 
            this.Controls.Add(this.CBTypes);
            this.Controls.Add(this.CBCategories);
            this.Controls.Add(this.RBAgainst);
            this.Controls.Add(this.RBAlong);
            this.Controls.Add(this.TVArchive);
            this.Name = "FerdaArchive";
            this.Size = new System.Drawing.Size(170, 500);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        /// <summary>
        /// Changes the size of the child controls. Archive has to do it
        /// itself, because DockDotNET doesnt support these kinds of events
        /// </summary>
        public void ChangeSize()
        {
            Constants c = new Constants();

            //it is docked somewhere
            if (Parent != null)
            {
                this.Size = Parent.Size;

                //adjusting the size of the control and all its child controls
                //height of the treeview
                int h = Parent.Size.Height - c.ArchiveButtonsHeight - (2 * c.ArchiveComboHeight);
                //width of the treeview
                int w = Parent.Size.Width - c.ArchiveBlankSpace;
                TVArchive.Size = new Size(w, h);
                CBCategories.Size = new Size(w, c.ArchiveComboHeight);
                CBTypes.Size = new Size(w, c.ArchiveComboHeight);
                RBAlong.Location = new Point(c.ArchiveLeftBlank,
                    (2 * c.ArchiveComboHeight) + h + 1);
                RBAgainst.Location =
                    new Point(c.ArchiveLeftBlank,
                    (2 * c.ArchiveComboHeight) + h + 1 + c.ArchiveButtonsHeight / 2);
            }
        }

        /// <summary>
        /// Fills the 1. combobox with the box categories
        /// </summary>
        public void FillBoxTypes()
        {
            CBCategories.Items.Clear();

            string[] types = archive.ArchiveBoxTypes;

            CBCategories.Items.Add(ResManager.GetString("ArchiveAllText"));

            foreach (string type in types)
            {
                CBCategories.Items.Add(type);
            }
        }

        /// <summary>
        /// Fills the 2nd comboBox (CBTypes) with labels from the archive
        /// according to the category name in the parameter
        /// </summary>
        /// <param name="boxCategory">Box category according to
        /// which the types of the boxes should be changed</param>
        public void FillBoxLabels(string boxCategory)
        {
            string[] types;
            CBTypes.Items.Clear();

            CBTypes.Items.Add(ResManager.GetString("ArchiveAllText"));
            if (boxCategory == ResManager.GetString("ArchiveAllText"))
            {
                types = archive.ListBoxLabelsInCategory(null);
            }
            else
            {
                types = archive.ListBoxLabelsInCategory(boxCategory);
            }

            foreach (string type in types)
            {
                CBTypes.Items.Add(type);
            }
        }

        /// <summary>
        /// Resets the archive to the main position according to the
        /// selected box type and direction
        /// </summary>
        /// <param name="BoxCategory">Category of the box</param>
        /// <param name="BoxType">Boxes of which label (box type) should be
        /// taken</param>
        /// <param name="AlongDirection">
        /// True if the direction is along, false if against direction
        /// </param>
        public void ResetArchive(string BoxCategory, string BoxType, bool AlongDirection)
        {
            FerdaTreeNode treeNode;

            //clears all the nodes in the treeview
            TVArchive.Nodes.Clear();

            //clearing the images
            TVArchive.ImageList = null;
            ImageList list = new ImageList();
            iconDictionary = new Dictionary<string, int>();

            //getting the first icon to the list
            list.Images.Add(naIcon);
            iconDictionary.Add("naIcon", 0);

            archive.RefreshOrder();
            //getting all the boxes in the archive
            if (BoxCategory == ResManager.GetString("ArchiveAllText"))
            {
                foreach (ModulesManager.IBoxModule b in archive.SortedBoxes)
                {
                    treeNode = new FerdaTreeNode(b, AlongDirection,
                        Archive, this, iconDictionary, list, iconProvider, projectManager);
                    TVArchive.Nodes.Add(treeNode);
                }
            }
            //getting only boxes from one category
            else
            {
                IBoxModule[] boxesList;
                //all boxes in that category
                if (BoxType == ResManager.GetString("ArchiveAllText"))
                {
                    boxesList = archive.ListBoxesWithType(BoxCategory, null);
                }
                //a specified box type
                else
                {
                    boxesList = archive.ListBoxesWithType(BoxCategory, BoxType);
                }

                foreach (ModulesManager.IBoxModule b in boxesList)
                {
                    treeNode = new FerdaTreeNode(b, AlongDirection,
                        Archive, this, iconDictionary, list, iconProvider, projectManager);
                    TVArchive.Nodes.Add(treeNode);
                }
            }

            TVArchive.ImageList = list;
        }

        #region IArchiveDisplayer implementation

        /// <summary>
        /// Because there are problems with sharing the clicking actions on the menu
        /// with other controls (ToolBox), this method raises the action that was
        /// clicked on the toolbar
        /// </summary>
        /// <param name="sender">sender of the method</param>
        public void RaiseToolBarAction(object sender)
        {
            FerdaTreeNode activeNode;
            if (mySelectedNode != null)
            {
                activeNode = mySelectedNode;
            }
            else
            {
                activeNode = TVArchive.SelectedNode as FerdaTreeNode;
            }
            if (activeNode == null)
            {
                throw new ApplicationException("Doprdele");
            }

            ToolStripButton bn = sender as ToolStripButton;
            if (bn == null)
            {
                throw new ApplicationException("Unexpected caller");
            }

            //we cannot use switch, because there is not a constant expression
            if (bn.ToolTipText == ResManager.GetString("MenuEditRename"))
            {
                Rename(activeNode);
                return;
            }

            if (bn.ToolTipText == ResManager.GetString("MenuEditCopy"))
            {
                Copy(activeNode.Box);
                return;
            }

            if (bn.ToolTipText == ResManager.GetString("MenuEditClone"))
            {
                Clone(activeNode.Box);
                return;
            }

            if (bn.ToolTipText == ResManager.GetString("MenuArchiveDelete"))
            {
                Delete(activeNode.Box);
                return;
            }

            if (bn.ToolTipText == ResManager.GetString("MenuEditPaste"))
            {
                Paste();
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

            //Setting the texts of the radiobuttons
            RBAlong.Text = ResManager.GetString("ArchiveAlongText");
            RBAgainst.Text = ResManager.GetString("ArchiveAgainstText");

            RefillContextMenus();
        }

        ///<summary>
        ///Localizes view in parameter in the archive
        ///</summary>
        ///<param name="box">
        /// Box to be localized
        ///</param>
        public void LocalizeInArchive(ModulesManager.IBoxModule box)
        {
            int i;
            string boxtype = string.Empty;
            List<string> types = new List<string>(box.MadeInCreator.BoxCategories);

            mySelectedNode = null;

            //indexing from one, because first group is all and every box belongs
            //to all group. We want to know a specific group
            for (i = 1; i < CBCategories.Items.Count; i++)
            {
                if (types.Contains(CBCategories.Items[i].ToString()))
                {
                    boxtype = CBCategories.Items[i].ToString();
                    break;
                }
            }

            //selecting the boxtype
            for (i = 0; i < CBCategories.Items.Count; i++)
            {
                string item = (string)CBCategories.Items[i];
                if (item == boxtype)
                {
                    break;
                }
            }
            CBCategories.SelectedIndex = i;

            //filling the labels in the second combo-box
            FillBoxLabels(boxtype);
            for (i = 0; i < CBTypes.Items.Count; i++)
            {
                string item = (string)CBTypes.Items[i];
                if (item == box.MadeInCreator.Label)
                {
                    break;
                }
            }
            CBTypes.SelectedIndex = i;

            //resetting the archive
            ResetArchive(boxtype, box.MadeInCreator.Label, true);

            //now we have to select the box in the list...
            foreach (FerdaTreeNode tn in TVArchive.Nodes)
            {
                if (tn.Box == box)
                {
                    TVArchive.SelectedNode = tn;
                    mySelectedNode = tn;
                    break;
                }
            }

            //setting the focus to the treeview
            TVArchive.Focus();
        }

        ///<summary>
        ///Forces the control to refresh its state
        ///</summary>
        ///<remarks>
        /// It also adapts the menu and toolbar
        ///</remarks>
        public void Adapt()
        {
            if (!IsDisposed)
            {
                mySelectedNode = null;

                int i;
                string oldCategory = CBCategories.SelectedItem.ToString();
                string oldType = CBTypes.SelectedItem.ToString();

                FillBoxTypes();

                //selecting the box category
                for (i = 0; i < CBCategories.Items.Count; i++)
                {
                    string item = CBCategories.Items[i].ToString();
                    if (item == oldCategory)
                    {
                        break;
                    }
                }

                if (i == CBCategories.Items.Count)
                {
                    CBCategories.SelectedIndex = 0;
                    //resets the archive to an "initial position"
                    ResetArchive(ResManager.GetString("ArchiveAllText"),
                        ResManager.GetString("ArchiveAllText"), true);
                }
                else
                {
                    CBCategories.SelectedIndex = i;
                    //resets the archive to an "initial position"
                    ResetArchive(oldCategory, oldType, true);
                }

                //selecting the box type
                for (int j = 0; j < CBTypes.Items.Count; j++)
                {
                    string item = CBTypes.Items[j].ToString();
                    if (item == oldType)
                    {
                        CBTypes.SelectedIndex = j;
                        break;
                    }
                }
            }

            menuDisplayer.Adapt();
            toolBar.Adapt();
        }

        /// <summary>
        /// This function is called by the property grid when a property is changed -
        /// that can only mean the change of the user name of the box.
        /// </summary>
        public void RefreshBoxNames()
        {
            foreach (FerdaTreeNode tn in TVArchive.Nodes)
            {
                tn.RefreshName();
            }
        }

        #endregion

        #region IBoxSelector implementation

        /// <summary>
        /// When a box is selected in the archive, it should also be selected on the 
        /// view. This function selects the box in the desktop
        /// </summary>
        /// <param name="box">Box to be selected</param>
        public void SelectBox(IBoxModule box)
        {
        }

        #endregion

        #region Context menu functions

        /// <summary>
        /// Renames a FerdaTreeNode in the parameter
        /// </summary>
        /// <param name="tn">node to be renamed</param>
        public void Rename(FerdaTreeNode tn)
        {
            tn.BeginEdit();
        }

        /// <summary>
        /// Copies the box int the parameter into the clipboard
        /// </summary>
        /// <param name="box">box for the clipboard</param>
        public void Copy(ModulesManager.IBoxModule box)
        {
            Clipboard.Nodes.Clear();
            Clipboard.Nodes.Add(box);
        }

        /// <summary>
        /// Clones the box in the project
        /// </summary>
        /// <param name="box">box to be cloned</param>
        public void Clone(ModulesManager.IBoxModule box)
        {
            Archive.Clone(box);

            Adapt();
            PropertiesDisplayer.Reset();
        }

        /// <summary>
        /// Deletes the box in the parameter from the project manager
        /// </summary>
        /// <param name="box">box to be deleted</param>
        public void Delete(ModulesManager.IBoxModule box)
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
            result = MessageBox.Show(message, caption, buttons,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

            if (result == DialogResult.Yes)
            {
                Archive.Remove(box);

                foreach (Desktop.FerdaDesktop desktop in Views)
                {
                    desktop.Adapt();
                }

                Adapt();
                PropertiesDisplayer.Reset();
            }
        }

        /// <summary>
        /// Pastes the content of the clipboard into the project
        /// </summary>
        public void Paste()
        {
            foreach (ModulesManager.IBoxModule b in Clipboard.Nodes)
            {
                Archive.Clone(b);
            }

            Adapt();
            PropertiesDisplayer.Reset();
        }

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// Event when the archive receives focus. It forces the menu
        /// to adapt
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void FerdaArchive_GotFocus(object sender, EventArgs e)
        {
            if (mySelectedNode != null)
            {
                //setting the SelectedBox to be the right box that has been selected by the previous action
                FerdaTreeNode ft = mySelectedNode as FerdaTreeNode;
                SelectedBox = ft.Box;
            }

            menuDisplayer.ControlHasFocus = this;
            menuDisplayer.Adapt();
            toolBar.ControlHasFocus = this;
            toolBar.Adapt();
        }

        /// <summary>
        /// Redraws the treeview according to the new settings of this ComboBox
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void CBArchive_SelectedIndexChanged(object sender, EventArgs e)
        {
            //getting the index
            int index = CBCategories.SelectedIndex;
            //filling the labels in the second combo-box
            FillBoxLabels(CBCategories.Items[index].ToString());
            //selecting the first item in the combo-box
            CBTypes.SelectedIndex = 0;
            //resetting the archive
            ResetArchive(CBCategories.Items[index].ToString(),
                ResManager.GetString("ArchiveAllText"), RBAlong.Checked);
        }

        /// <summary>
        /// Redraws the treeview accordint to the new settings of this ComboBox
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void CBTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            //getting the indexes
            int categoryIndex = CBCategories.SelectedIndex;
            int typeIndex = CBTypes.SelectedIndex;

            //if in the first category, there is the "all" option
            //selected, we must first determine, to which category
            //the type of the second combobox belongs
            if ((CBCategories.Items[categoryIndex].ToString() ==
                ResManager.GetString("ArchiveAllText")) &&
                (CBTypes.Items[typeIndex].ToString() !=
                ResManager.GetString("ArchiveAllText")))
            {
                string[] categories = archive.ArchiveBoxTypes;
                foreach (string category in categories)
                {
                    StringCollection types = new StringCollection();
                    types.AddRange(archive.ListBoxLabelsInCategory(category));
                    if (types.Contains(CBTypes.Items[typeIndex].ToString()))
                    {
                        ResetArchive(category, CBTypes.Items[typeIndex].ToString(),
                            RBAlong.Checked);
                        break;
                    }
                }
            }
            else
            {
                ResetArchive(CBCategories.Items[categoryIndex].ToString(),
                    CBTypes.Items[typeIndex].ToString(), RBAlong.Checked);
            }
        }

        /// <summary>
        /// Redraws the archive according to the new settings of the RadioButtons
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void RBAlong_CheckedChanged(object sender, EventArgs e)
        {
            int index = CBCategories.SelectedIndex;
            if (index == -1)
            {
                ResetArchive(CBCategories.Items[0].ToString(),
                    ResManager.GetString("ArchiveAllText"), RBAlong.Checked);
            }
            else
            {
                ResetArchive(CBCategories.Items[index].ToString(),
                    ResManager.GetString("ArchiveAllText"), RBAlong.Checked);
            }
        }

        /// <summary>
        /// Notifies the other controls that a node has been selected
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void TVArchive_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //gets the box that was selected
            FerdaTreeNode f = e.Node as FerdaTreeNode;
            SelectedBox = f.Box;

            //forcing the menu to adapt
            menuDisplayer.ControlHasFocus = this;
            menuDisplayer.Adapt();
            toolBar.ControlHasFocus = this;
            toolBar.Adapt();

            //selecting the box on the active desktop
            foreach (IViewDisplayer view in views)
            {
                view.SelectBox(f.Box);
            }

            //forcing the propertygrid to adapt
            propertiesDisplayer.SelectedBox = this.SelectedBox;
            propertiesDisplayer.IsOneBoxSelected = true;
            propertiesDisplayer.Adapt();

            //forcing the context help to adapt
            contextHelpDisplayer.SelectedBox = this.SelectedBox;
            contextHelpDisplayer.Adapt();

            //forcing the user note to adapt
            userNote.SelectedBox = this.SelectedBox;
            userNote.Adapt();
        }

        /// <summary>
        /// Reacts to a mouse move - prepares the drag&amp;drop operation
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void TVArchive_MouseMove(object sender, MouseEventArgs e)
        {
            //if it was a left mouse button
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                //there is something selected
                if (mySelectedNode != null)
                {
                    DoDragDrop(mySelectedNode, DragDropEffects.Copy);
                }
            }
        }

        /// <summary>
        /// Reaction to a mouse down - the mySelectedNode gets initialized
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void TVArchive_MouseDown(object sender, MouseEventArgs e)
        {
            mySelectedNode = TVArchive.GetNodeAt(e.X, e.Y) as FerdaTreeNode;
            if (mySelectedNode != null)
            {
                mySelectedNode.SetContextMenu();
            }
        }

        /// <summary>
        /// Tries to write a new name to the box after editing the label
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void TVArchive_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            FerdaTreeNode node = e.Node as FerdaTreeNode;

            string name;
            if (e.Label == null)
            {
                name = node.Text;
            }
            else
            {
                name = e.Label;
            }

            //tries to write to the box
            if (node.Box.TryWriteEnter())
            {
                node.Box.UserName = name;

                node.Box.WriteExit();
                node.EndEdit(false);

                //notifies the propertyDisplayer and all the views
                PropertiesDisplayer.Adapt();
                foreach (Desktop.IViewDisplayer view in Views)
                {
                    view.Adapt();
                }
                //here it is not necessaty to adapt also the user note
                //control, because the change of the label of the box does not
                //affect the user note of the box
            }
            else
            {
                node.CannotWriteToBox(node.Box);
                e.CancelEdit = true;
            }
        }

        #endregion
    }
}
