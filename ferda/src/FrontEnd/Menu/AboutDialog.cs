// AboutDialog.cs - a dialog that displays information about the program
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
using System.Resources;
using System.Reflection;

namespace Ferda.FrontEnd.Menu
{
    /// <summary>
    /// Dialog that presents information about the Ferda DataMiner application
    /// </summary>
    public class AboutDialog : Form
    {
        #region Fields

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        /// <summary>
        /// Panel to hold the Ferda image
        /// </summary>
        protected Panel PImage;
        /// <summary>
        /// OK button
        /// </summary>
        protected Button BOk;
        /// <summary>
        /// Ferda label
        /// </summary>
        protected Label LFerda;
        /// <summary>
        /// Description of the project
        /// </summary>
        protected RichTextBox RTBDescription;
        /// <summary>
        /// Link label with sourceforge address
        /// </summary>
        protected LinkLabel LLAddress;
        /// <summary>
        /// Version of the application
        /// </summary>
        protected Label LVersion;

        #endregion

        #region Constructor

        /// <summary>
        /// Fills the form with the right values
        /// </summary>
        /// <param name="resManager">The localized information</param>
        public AboutDialog(ResourceManager resManager)
        {
            InitializeComponent();

            this.BOk.Text = resManager.GetString("OKButton");
            this.Text = resManager.GetString("AboutDialogText");
            StringBuilder str =
                new StringBuilder(resManager.GetString("AboutDialogDescription"));
            
            //writing the developers
            str.Append("\n\n"); 
            str.Append(resManager.GetString("AboutDialogDevelopers"));
            str.Append("\nAlexander Kuzmin:\tAddIns, Modules for Interaction");
            str.Append("\nMichal Kováč:\tProject Manager, Modules Manager");
            str.Append("\nTomáš Kuchař:\tImplementation of boxes");
            str.Append("\nMartin Ralbovský:\tFrontEnd");

            this.RTBDescription.Text = str.ToString();
            
            //getting the version of Ferda
            Assembly assembly = Assembly.GetExecutingAssembly();
            string version = GetVersionFromAssembly(assembly);
            LVersion.Text = resManager.GetString("AboutDialogVersion") + version;
        }

        #endregion 

        #region Methods

        /// <summary>
        /// Retrieves the information about the version of the 
        /// current assembly
        /// </summary>
        /// <param name="assembly">Assembly</param>
        /// <returns>string that contains the version of the assebly</returns>
        protected string GetVersionFromAssembly(Assembly assembly)
        {
            string tmp = assembly.FullName;
            int startOfVersion = tmp.IndexOf("Version=");
            tmp = tmp.Substring(startOfVersion + 8);
            int comma = tmp.IndexOf(',');
            tmp = tmp.Substring(0, comma);

            return tmp;
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.PImage = new System.Windows.Forms.Panel();
            this.BOk = new System.Windows.Forms.Button();
            this.LFerda = new System.Windows.Forms.Label();
            this.RTBDescription = new System.Windows.Forms.RichTextBox();
            this.LLAddress = new System.Windows.Forms.LinkLabel();
            this.LVersion = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // PImage
            // 
            this.PImage.BackgroundImage = global::Ferda.FrontEnd.Localization_en_US.ferda64;
            this.PImage.Location = new System.Drawing.Point(12, 12);
            this.PImage.Name = "PImage";
            this.PImage.Size = new System.Drawing.Size(64, 64);
            this.PImage.TabIndex = 0;
            // 
            // BOk
            // 
            this.BOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.BOk.Location = new System.Drawing.Point(205, 238);
            this.BOk.Name = "BOk";
            this.BOk.Size = new System.Drawing.Size(75, 23);
            this.BOk.TabIndex = 1;
            this.BOk.Text = "button1";
            this.BOk.UseVisualStyleBackColor = true;
            // 
            // LFerda
            // 
            this.LFerda.AutoSize = true;
            this.LFerda.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.LFerda.Location = new System.Drawing.Point(83, 12);
            this.LFerda.Name = "LFerda";
            this.LFerda.Size = new System.Drawing.Size(173, 25);
            this.LFerda.TabIndex = 2;
            this.LFerda.Text = "Ferda DataMiner";
            // 
            // RTBDescription
            // 
            this.RTBDescription.Location = new System.Drawing.Point(12, 88);
            this.RTBDescription.Name = "RTBDescription";
            this.RTBDescription.ReadOnly = true;
            this.RTBDescription.Size = new System.Drawing.Size(268, 144);
            this.RTBDescription.TabIndex = 3;
            this.RTBDescription.Text = "";
            // 
            // LLAddress
            // 
            this.LLAddress.AutoSize = true;
            this.LLAddress.Location = new System.Drawing.Point(88, 41);
            this.LLAddress.Name = "LLAddress";
            this.LLAddress.Size = new System.Drawing.Size(144, 13);
            this.LLAddress.TabIndex = 4;
            this.LLAddress.TabStop = true;
            this.LLAddress.Text = "http://ferda.sourceforge.net/";
            this.LLAddress.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LLAddress_LinkClicked);
            // 
            // LVersion
            // 
            this.LVersion.AutoSize = true;
            this.LVersion.Location = new System.Drawing.Point(88, 62);
            this.LVersion.Name = "LVersion";
            this.LVersion.Size = new System.Drawing.Size(41, 13);
            this.LVersion.TabIndex = 5;
            this.LVersion.Text = "version";
            // 
            // AboutDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.LVersion);
            this.Controls.Add(this.LLAddress);
            this.Controls.Add(this.RTBDescription);
            this.Controls.Add(this.LFerda);
            this.Controls.Add(this.BOk);
            this.Controls.Add(this.PImage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AboutDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// Should redirect to the homepage of the Ferda Project
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        private void LLAddress_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://ferda.sourceforge.net/");
        }

        #endregion
    }
}