// BoxExceptionDialog.cs - dialog that displays exceptions thrown by boxes
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
using Ferda.ModulesManager;

namespace Ferda.FrontEnd
{
    /// <summary>
    /// A dialog that informs a user when exception is thrown by a box
    /// </summary>
    public class BoxExceptionDialog : Form
    {
        #region Fields

        /// <summary>
        /// Panel that holds the picture of the box
        /// </summary>
        protected Panel panel1;
        /// <summary>
        /// Label saying box
        /// </summary>
        protected Label LBox;
        /// <summary>
        /// Label with the name of the box
        /// </summary>
        protected Label LBoxName;
        /// <summary>
        /// Label with some exception stuff
        /// </summary>
        protected Label LExceptionKecy;
        /// <summary>
        /// Exception details label
        /// </summary>
        protected Label LExceptionDetails;
        /// <summary>
        /// Text box with details of the exception
        /// </summary>
        protected RichTextBox RTBExceptionDetails;
        private System.Windows.Forms.Button BOk;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor that fills all the controls of the class with
        /// localized and right values
        /// </summary>
        /// <param name="resManager">Resource Manager</param>
        /// <param name="boxName">name of the box that has thrown the exception
        /// </param>
        /// <param name="exceptionDetails">Details of the exception</param>
        public BoxExceptionDialog(ResourceManager resManager, string boxName,
            string exceptionDetails)
        {
            InitializeComponent();

            //Filling the dialog with the right values
            this.Text = resManager.GetString("BoxExceptionDialogCaption");
            this.LBox.Text = resManager.GetString("BoxExceptionDailogBox");
            this.LBoxName.Text = boxName;
            this.LExceptionKecy.Text = resManager.GetString("BoxExceptionDialogKecy");
            this.LExceptionDetails.Text = resManager.GetString("BoxExceptionDialogDetails");
            this.RTBExceptionDetails.Text = exceptionDetails;
            this.BOk.Text = resManager.GetString("OKButton");
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

        #endregion

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.BOk = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.LBox = new System.Windows.Forms.Label();
            this.LBoxName = new System.Windows.Forms.Label();
            this.LExceptionKecy = new System.Windows.Forms.Label();
            this.LExceptionDetails = new System.Windows.Forms.Label();
            this.RTBExceptionDetails = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // BOk
            // 
            this.BOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.BOk.Location = new System.Drawing.Point(382, 235);
            this.BOk.Name = "BOk";
            this.BOk.Size = new System.Drawing.Size(75, 23);
            this.BOk.TabIndex = 0;
            this.BOk.Text = "Ok";
            this.BOk.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::Ferda.FrontEnd.Localization_en_US.Box;
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(192, 242);
            this.panel1.TabIndex = 1;
            // 
            // LBox
            // 
            this.LBox.AutoSize = true;
            this.LBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.LBox.Location = new System.Drawing.Point(214, 15);
            this.LBox.Name = "LBox";
            this.LBox.Size = new System.Drawing.Size(31, 16);
            this.LBox.TabIndex = 2;
            this.LBox.Text = "Box";
            // 
            // LBoxName
            // 
            this.LBoxName.AutoSize = true;
            this.LBoxName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.LBoxName.Location = new System.Drawing.Point(214, 33);
            this.LBoxName.Name = "LBoxName";
            this.LBoxName.Size = new System.Drawing.Size(70, 15);
            this.LBoxName.TabIndex = 3;
            this.LBoxName.Text = "box name";
            // 
            // LExceptionKecy
            // 
            this.LExceptionKecy.AutoSize = true;
            this.LExceptionKecy.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.LExceptionKecy.Location = new System.Drawing.Point(214, 52);
            this.LExceptionKecy.Name = "LExceptionKecy";
            this.LExceptionKecy.Size = new System.Drawing.Size(150, 16);
            this.LExceptionKecy.TabIndex = 4;
            this.LExceptionKecy.Text = "has thrown an exception";
            // 
            // LExceptionDetails
            // 
            this.LExceptionDetails.AutoSize = true;
            this.LExceptionDetails.Location = new System.Drawing.Point(214, 99);
            this.LExceptionDetails.Name = "LExceptionDetails";
            this.LExceptionDetails.Size = new System.Drawing.Size(87, 13);
            this.LExceptionDetails.TabIndex = 5;
            this.LExceptionDetails.Text = "Exception details";
            // 
            // RTBExceptionDetails
            // 
            this.RTBExceptionDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.RTBExceptionDetails.Location = new System.Drawing.Point(214, 116);
            this.RTBExceptionDetails.Name = "RTBExceptionDetails";
            this.RTBExceptionDetails.ReadOnly = true;
            this.RTBExceptionDetails.Size = new System.Drawing.Size(243, 113);
            this.RTBExceptionDetails.TabIndex = 6;
            this.RTBExceptionDetails.Text = "";
            // 
            // BoxExceptionDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(467, 265);
            this.Controls.Add(this.RTBExceptionDetails);
            this.Controls.Add(this.LExceptionDetails);
            this.Controls.Add(this.LExceptionKecy);
            this.Controls.Add(this.LBoxName);
            this.Controls.Add(this.LBox);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.BOk);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(475, 294);
            this.Name = "BoxExceptionDialog";
            this.Text = "BoxExceptionDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}