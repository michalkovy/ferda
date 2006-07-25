// BoxProgressBar.cs - user control represents a progress bar of a action
// of a particular box
//
// Author: Martin Ralbovsk� <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2006 Martin Ralbovsk�
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

namespace Ferda.FrontEnd.ProgressBar
{
    /// <summary>
    /// Represents a progress bar control for a action of a box
    /// </summary>
    public class BoxProgressBar : UserControl
    {
        #region Fields

        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label LName;
        private System.Windows.Forms.Label LHint;
        private System.Windows.Forms.LinkLabel LLStop;
        private System.Windows.Forms.LinkLabel LLHide;
        private ProgressBarI myProgressBarI;

        delegate void SetHintDelegate(string hint);
        delegate void SetValueDelegate(float value);

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #endregion

        #region Properties

        /// <summary>
        /// Property sets the boxes name
        /// </summary>
        public string BoxName
        {
            set
            {
                LName.Text = value;
            }
        }

        /// <summary>
        /// Sets the style of the progress bar - in order to display the continuos
        /// scrolling without any values
        /// </summary>
        public ProgressBarStyle ProgressStyle
        {
            set
            {
                progressBar.Style = value;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor for the class
        /// </summary>
        /// <param name="progressBarI">Interface to handle progress
        /// bars in Ferda</param>
        public BoxProgressBar(ProgressBarI progressBarI)
        {
            InitializeComponent();

            this.myProgressBarI = progressBarI;
            this.progressBar.Minimum = 0;
            this.progressBar.Maximum = 100;
            this.progressBar.Style = ProgressBarStyle.Continuous;
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.LName = new System.Windows.Forms.Label();
            this.LHint = new System.Windows.Forms.Label();
            this.LLStop = new System.Windows.Forms.LinkLabel();
            this.LLHide = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(0, 50);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(152, 23);
            this.progressBar.TabIndex = 0;
            // 
            // LName
            // 
            this.LName.AutoSize = true;
            this.LName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.LName.Location = new System.Drawing.Point(4, 4);
            this.LName.Name = "LName";
            this.LName.Size = new System.Drawing.Size(41, 13);
            this.LName.TabIndex = 1;
            this.LName.Text = "label1";
            // 
            // LHint
            // 
            this.LHint.AutoSize = true;
            this.LHint.Location = new System.Drawing.Point(7, 21);
            this.LHint.Name = "LHint";
            this.LHint.Size = new System.Drawing.Size(35, 13);
            this.LHint.TabIndex = 2;
            this.LHint.Text = "label1";
            // 
            // LLStop
            // 
            this.LLStop.AutoSize = true;
            this.LLStop.Location = new System.Drawing.Point(118, 4);
            this.LLStop.Name = "LLStop";
            this.LLStop.Size = new System.Drawing.Size(29, 13);
            this.LLStop.TabIndex = 3;
            this.LLStop.TabStop = true;
            this.LLStop.Text = "Stop";
            this.LLStop.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LLStop_LinkClicked);
            // 
            // LLHide
            // 
            this.LLHide.AutoSize = true;
            this.LLHide.Location = new System.Drawing.Point(118, 21);
            this.LLHide.Name = "LLHide";
            this.LLHide.Size = new System.Drawing.Size(29, 13);
            this.LLHide.TabIndex = 4;
            this.LLHide.TabStop = true;
            this.LLHide.Text = "Hide";
            this.LLHide.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LLHide_LinkClicked);
            // 
            // BoxProgressBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.LLHide);
            this.Controls.Add(this.LLStop);
            this.Controls.Add(this.LHint);
            this.Controls.Add(this.LName);
            this.Controls.Add(this.progressBar);
            this.Name = "BoxProgressBar";
            this.Size = new System.Drawing.Size(150, 76);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        /// <summary>
        /// Sets the hint of the box progress bar (also from another
        /// thread)
        /// </summary>
        /// <param name="hint">Name of the hint to be displayed</param>
        public void SetHint(string hint)
        {
            if (InvokeRequired)
            {
                SetHintDelegate d = new SetHintDelegate(SetHint);
                Invoke(d, new object[] { hint });
            }
            else
            {
                LHint.Text = hint;
            }
        }

        /// <summary>
        /// Sets the value of the progress bar (also from another thread)
        /// </summary>
        /// <param name="value">New value of the progress bar</param>
        public void SetValue(float value)
        {
            if (InvokeRequired)
            {
                SetValueDelegate d = new SetValueDelegate(SetValue);
                Invoke(d, new object[] { value });
            }
            else
            {
                if (value > 1 || value < 0)
                {
                    throw new ApplicationException("Invalid value for a progress bar");
                }
                progressBar.Value = System.Convert.ToInt32(value * 100);
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Hides the actual progress bar
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        private void LLHide_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProgressBarsManager man = Parent as ProgressBarsManager;
            if (man != null)
            {
                man.RemoveBoxProgressBar(this);
            }
        }

        /// <summary>
        /// Sends a stop signal to the box
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        private void LLStop_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            myProgressBarI.stop();
        }

        #endregion
    }
}