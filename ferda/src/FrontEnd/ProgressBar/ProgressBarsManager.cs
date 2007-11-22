// ProgressBarsManager.cs - a component of the working environment that can display
// progress bars of controls' actions
//
// Author: Martin Ralbovský <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2006 Martin Ralbovský
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
using System.Resources;
using Ferda.FrontEnd.Menu;

namespace Ferda.FrontEnd.ProgressBar
{
    /// <summary>
    /// A component of the working environmnet that can display, hide or stop the
    /// progress bar of actions of individual boxes.
    /// </summary>
    public class ProgressBarsManager : UserControl
    {
        #region Class fields

        delegate void MyDelegate(BoxProgressBar progressBar);

        //Resource manager from the FerdaForm
        private ResourceManager resManager = null;

        /// <summary>
        /// Preferences of the application
        /// </summary>
        public IPreferencesManager preferences;

        #endregion

        #region Constructor

        /// <summary>
        /// Default consturctor for the class
        /// </summary>
        /// <param name="rm">Resource manager of the application</param>
        /// <param name="preferences">Preferences of the application</param>
        public ProgressBarsManager(ResourceManager rm, IPreferencesManager preferences)
        {
            InitializeComponent();

            //Initialize components things
            components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.resManager = rm;
            this.preferences = preferences;
        }

        #endregion

        #region Methods

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
        /// Resizes the control according to the size of the parent content
        /// </summary>
        public void ChangeSize()
        {
            if (Parent != null)
            {
                this.Size = Parent.Size;

                foreach (BoxProgressBar bar in Controls)
                {
                    bar.ChangeWidht(this.Size.Width);
                }
            }
        }

        /// <summary>
        /// Initializing the component
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ProgressBarsManager
            // 
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Name = "ProgressBarControl";
            this.ResumeLayout(false);
        }

        /// <summary>
        /// Adds a progress bar control of a specific box
        /// </summary>
        /// <param name="progressBar">control to be added</param>
        public void AddBoxProgressBar(BoxProgressBar progressBar)
        {
            if (InvokeRequired)
            {
                MyDelegate d = new MyDelegate(AddBoxProgressBar);
                this.BeginInvoke(d, new object[] { progressBar });
            }
            else
            {
                if (Controls.Count != 0)
                {
                    int controlHeight = Controls[0].Height;
                    progressBar.Location = 
                        new Point(0, Controls.Count * controlHeight);
                }
                progressBar.Localize(resManager);
                this.Controls.Add(progressBar);

                ChangeSize();
            }
        }

        /// <summary>
        /// Removes a progress bar control of a specific box
        /// </summary>
        /// <param name="progressBar">control to be removed</param>
        public void RemoveBoxProgressBar(BoxProgressBar progressBar)
        {
            if (InvokeRequired)
            {
                MyDelegate d = new MyDelegate(RemoveBoxProgressBar);
                this.BeginInvoke(d, new object[] { progressBar });
            }
            else
            {
                this.Controls.Remove(progressBar);

                //vsechny ostatni controly, ktere byly "za" zrusenym progressbarem
                //se posunou na jeho misto
                for (int i = 0; i < Controls.Count; i++)
                {
                    Controls[i].Location = new Point(0, i * Controls[0].Height);
                }
            }
        }

        /// <summary>
        /// Hides a progress bar control of a specific box
        /// </summary>
        /// <param name="progressBar">control to be hidden</param>
        public void HideProgressBar(BoxProgressBar progressBar)
        {
            progressBar.Visible = false;

            //a potom posunu vsechny ostatni
        }

        #endregion
    }
}
