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
        private ListView lvArchive;

        /// <summary>
        /// The prefernces manager - informs about preferences of the environment,
        /// mainly localization
        /// </summary>
        protected Menu.IPreferencesManager preferencesManager;

        /// <summary>
        /// The network archive providing functionality for the
        /// component
        /// </summary>
        protected ProjectManager.NetworkArchive networkArchive;

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
        public FerdaNetworkArchive(IPreferencesManager prefManager, 
            ProjectManager.ProjectManager projManager)
        {
            preferencesManager = prefManager;
            resManager = preferencesManager.ResManager;
            networkArchive = projManager.NetworkArchive;

            InitializeComponent();

            this.AllowDrop = true;
            this.lvArchive.AllowDrop = true;
            //lvArchive.DragDrop += new DragEventHandler(lvArchive_DragDrop);
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
            this.lvArchive = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // lvArchive
            // 
            this.lvArchive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvArchive.Location = new System.Drawing.Point(0, 0);
            this.lvArchive.Name = "lvArchive";
            this.lvArchive.Size = new System.Drawing.Size(170, 500);
            this.lvArchive.TabIndex = 0;
            this.lvArchive.UseCompatibleStateImageBehavior = false;
            this.lvArchive.View = System.Windows.Forms.View.List;
            // 
            // FerdaNetworkArchive
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lvArchive);
            this.Name = "FerdaNetworkArchive";
            this.Size = new System.Drawing.Size(170, 500);
            this.ResumeLayout(false);

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
                MessageBox.Show("the archive already contains this box");
            }
        }

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// Handles dropping event from the desktop and archive
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        //void lvArchive_DragDrop(object sender, DragEventArgs e)
        //{
        //    IBoxModule box = null;
        //    Object o = e.Data.GetData(typeof(FerdaTreeNode));

        //    //it is dropped from archive
        //    if (o is FerdaTreeNode)
        //    {
        //        box = ((FerdaTreeNode)o).Box;

        //        AskLabelAddBox(box);
        //    }
        //}

        #endregion
    }
}
