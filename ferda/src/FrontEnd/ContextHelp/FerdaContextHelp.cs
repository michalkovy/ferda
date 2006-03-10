// FerdaContextHelp.cs - control that displays context help in Ferda
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
using System.Drawing;
using System.Windows.Forms;
using System.Resources;
using Ferda.FrontEnd.Menu;
using Ferda.ModulesManager;
using Ferda.Modules;

namespace Ferda.FrontEnd.ContextHelp
{
    /// <summary>
	/// The context help control is displays information about each box
    /// and displays them as hyperlinks to a pdf-file. The contextual information
    /// can be edited in the boxes configuration.
	/// </summary>
	///<stereotype>control</stereotype>
    public class FerdaContextHelp : System.Windows.Forms.UserControl, 
        IContextHelpDisplayer
    {
        #region Class fields

        //Resource manager from the FerdaForm
        private ResourceManager resManager;
        /// <summary>
        /// Localization manager of the application
        /// </summary>
        private ILocalizationManager localizationManager;

        /// <summary>
        /// The menu of the application
        /// </summary>
        protected IMenuDisplayer menuDisplayer;
        /// <summary>
        /// The toolbar of the application
        /// </summary>
        protected IMenuDisplayer toolBar;

        //control that have all the boxes in common
        /// <summary>
        /// Label that shows type of the box
        /// </summary>
        protected Label LBoxType;
        /// <summary>
        /// TextBox that shows the hint about the box type
        /// </summary>
        protected RichTextBox RTBBoxTypeHint;
        /// <summary>
        /// Label that shows name of the box
        /// </summary>
        protected Label LBoxName;

        //the box we are working with
        private IBoxModule selectedBox;
        /// <summary>
        /// A link label that shows the tutorial for Ferda
        /// </summary>
        protected LinkLabel LLTutorial;

        /// <summary>
        /// the distance between the first
        /// </summary>
        protected int itemsOffset = 20;

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
                        "ContextHelp.ResManager cannot be null");
                }
                return resManager;
            }
        }

        #endregion

        #region Constructor

        ///<summary>
		/// Default constructor for FerdaContextHelp class.
		///</summary>
        ///<param name="locManager">Manager of the localization of the application</param>
        ///<param name="menuDisp">The displayer of the menu</param>
        ///<param name="toolBar">The toolbar of the application</param>
        public FerdaContextHelp(ILocalizationManager locManager, IMenuDisplayer menuDisp, 
            IMenuDisplayer toolBar)
            : base()
		{
            //setting the localization
            localizationManager = locManager;
            ResManager = localizationManager.ResManager;

            //setting the menu displayer
            menuDisplayer = menuDisp;
            this.toolBar = toolBar;

            InitializeComponent();

            //setting the focus
            GotFocus += new EventHandler(FerdaContextHelp_GotFocus);

            //reseting the component
            Reset();

            //setting the text of the tutorial link label
            LLTutorial.Text = ResManager.GetString("ContextHelpStart");
        }

        #endregion

        #region IContextHelpDisplayer implementation

        ///<summary>
        ///The box currently selected in the archive
        ///</summary>
		public IBoxModule SelectedBox
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

        ///<summary>
        ///Forces the control to refresh its state
        ///</summary>
        public void Adapt()
        {
            if (IsDisposed)
            {
                return;
            }

            //setting the static stuff
            LBoxName.Visible = true;
            LBoxName.Text = SelectedBox.UserName;
            LBoxType.Visible = true;
            LBoxType.Text = SelectedBox.MadeInCreator.Label;
            RTBBoxTypeHint.Visible = true;
            RTBBoxTypeHint.Text = SelectedBox.MadeInCreator.Hint;

            //determining where to start the dynamic part of the help
            int y = RTBBoxTypeHint.Bottom + itemsOffset;

            foreach (DynamicHelpItem item in SelectedBox.DynamicHelpItems)
            {
                LinkLabel label = new LinkLabel();
                label.AutoSize = true;
                label.Size = new Size(62, 16);
                label.Name = item.identifier;
                label.Text = item.label;
                label.Location = new Point(0, y);
                label.Visible = true;
                this.Controls.Add(label);
                y += 16;
            }
        }

        /// <summary>
        /// Resets the context help to be without any information
        /// </summary>
        public void Reset()
        {
            if (IsDisposed)
            {
                return;
            }

            LBoxName.Visible = false;
            LBoxType.Visible = false;
            RTBBoxTypeHint.Visible = false;
        }

		///<summary>
		///This function is called when the localization
		///of the application is changed - the whole menu needs to be redrawn
		///</summary>
		public void ChangeLocalization()
		{
			//postup stejny jako pri konstrukci, znova se vytvori cele menu
        }

        #endregion

        #region Methods

        private void InitializeComponent()
        {
            this.LBoxName = new System.Windows.Forms.Label();
            this.LBoxType = new System.Windows.Forms.Label();
            this.RTBBoxTypeHint = new System.Windows.Forms.RichTextBox();
            this.LLTutorial = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // LBoxName
            // 
            this.LBoxName.AutoSize = true;
            this.LBoxName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.LBoxName.Location = new System.Drawing.Point(3, 22);
            this.LBoxName.Name = "LBoxName";
            this.LBoxName.Size = new System.Drawing.Size(32, 16);
            this.LBoxName.TabIndex = 0;
            this.LBoxName.Text = "text";
            // 
            // LBoxType
            // 
            this.LBoxType.AutoSize = true;
            this.LBoxType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.LBoxType.Location = new System.Drawing.Point(3, 38);
            this.LBoxType.Name = "LBoxType";
            this.LBoxType.Size = new System.Drawing.Size(62, 16);
            this.LBoxType.TabIndex = 1;
            this.LBoxType.Text = "boxType";
            // 
            // RTBBoxTypeHint
            // 
            this.RTBBoxTypeHint.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.RTBBoxTypeHint.Location = new System.Drawing.Point(3, 57);
            this.RTBBoxTypeHint.Name = "RTBBoxTypeHint";
            this.RTBBoxTypeHint.Size = new System.Drawing.Size(137, 96);
            this.RTBBoxTypeHint.TabIndex = 2;
            this.RTBBoxTypeHint.Text = "box type hint";
            // 
            // LLTutorial
            // 
            this.LLTutorial.AutoSize = true;
            this.LLTutorial.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.LLTutorial.Location = new System.Drawing.Point(4, 4);
            this.LLTutorial.Name = "LLTutorial";
            this.LLTutorial.Size = new System.Drawing.Size(69, 16);
            this.LLTutorial.TabIndex = 3;
            this.LLTutorial.TabStop = true;
            this.LLTutorial.Text = "linkLabel1";
            this.LLTutorial.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LLTutorial_LinkClicked);
            // 
            // FerdaContextHelp
            // 
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Controls.Add(this.LLTutorial);
            this.Controls.Add(this.RTBBoxTypeHint);
            this.Controls.Add(this.LBoxType);
            this.Controls.Add(this.LBoxName);
            this.Name = "FerdaContextHelp";
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
            if (Parent != null)
            {
                this.Size = Parent.Size;

                //changing the size of the RTB
                //there is a gap for better readability (5 pixels)
                RTBBoxTypeHint.Width = this.Width - 5;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Event when the context help receives focus. It forces the menu and toolbar
        /// to adapt
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void FerdaContextHelp_GotFocus(object sender, EventArgs e)
        {
            menuDisplayer.ControlHasFocus = this;
            menuDisplayer.Adapt();
            toolBar.ControlHasFocus = this;
            toolBar.Adapt();
        }

        /// <summary>
        /// This event should show the pdf help with Ferda tutorial
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void LLTutorial_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string path = FrontEndCommon.GetBinPath();
            path += "\\Help\\" + ResManager.GetString("FerdaTutorial");

            FrontEndCommon.OpenPdf(path, ResManager);
        }

        #endregion
    }
}
