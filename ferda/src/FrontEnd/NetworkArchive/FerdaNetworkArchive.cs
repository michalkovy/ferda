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

namespace Ferda.FrontEnd.NetworkArchive
{
    /// <summary>
    /// Control class for the Ferda Network Archive
    /// </summary>
    public class FerdaNetworkArchive : UserControl
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
        private Label label1;

        /// <summary>
        /// The prefernces manager - informs about preferences of the environment,
        /// mainly localization
        /// </summary>
        protected Menu.IPreferencesManager preferencesManager;

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
        public FerdaNetworkArchive(IPreferencesManager prefManager)
        {
            preferencesManager = prefManager;
            resManager = preferencesManager.ResManager;

            InitializeComponent();
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
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Network Archive";
            // 
            // FerdaNetworkArchive
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Name = "FerdaNetworkArchive";
            this.Size = new System.Drawing.Size(170, 500);
            this.ResumeLayout(false);
            this.PerformLayout();

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

        #endregion
    }
}
