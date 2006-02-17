// FerdaPrescreen.cs - dialog that is shown on initialization
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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ferda.FrontEnd
{
    /// <summary>
    /// This form is viewed when the Ferda program starts. It has the ability to 
    /// show the user what is beeing initialized at the moment.
    /// /// </summary>
    public class FerdaPrescreen : Form
    {
        #region Class fields

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        /// <summary>
        /// Label for displaying the messages
        /// </summary>
        protected Label LMessages;

        #endregion

        #region Constructor

        /// <summary>
        /// Initial constructor
        /// </summary>
        public FerdaPrescreen()
        {
            InitializeComponent();
            LMessages.Visible = true;
        }

        #endregion

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FerdaPrescreen));
            this.LMessages = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LMessages
            // 
            this.LMessages.AutoSize = true;
            this.LMessages.BackColor = System.Drawing.Color.Transparent;
            this.LMessages.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.LMessages.ForeColor = System.Drawing.SystemColors.Window;
            this.LMessages.Location = new System.Drawing.Point(12, 157);
            this.LMessages.Name = "LMessages";
            this.LMessages.Size = new System.Drawing.Size(0, 13);
            this.LMessages.TabIndex = 1;
            // 
            // FerdaPrescreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SkyBlue;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(400, 200);
            this.Controls.Add(this.LMessages);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FerdaPrescreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FerdaPrescreen";
            this.ResumeLayout(false);
            this.PerformLayout();

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
        /// Displays text messages from the from into the label
        /// </summary>
        /// <param name="text">Text to be displayed</param>
        public void DisplayText(string text)
        {
            LMessages.Text = text;
            Refresh();
        }

        #endregion

        #region Events

        #endregion
    }
}