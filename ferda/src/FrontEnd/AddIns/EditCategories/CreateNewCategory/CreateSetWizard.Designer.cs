// CreateSetWizard.cs - GUI part
//
// Author: Alexander Kuzmin <alexander.kuzmin@gmail.com>
//
// Copyright (c) 2005 Alexander Kuzmin
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


namespace Ferda.FrontEnd.AddIns.EditCategories.CreateNewCategory
{
    partial class CreateSetWizard
    {
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.LabelNewName = new System.Windows.Forms.Label();
            this.TextBoxNewName = new System.Windows.Forms.TextBox();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.ButtonSubmit = new System.Windows.Forms.Button();
            this.LabelExistingValues = new System.Windows.Forms.Label();
            this.LabelAvailableValues = new System.Windows.Forms.Label();
            this.ListBoxExistingValues = new System.Windows.Forms.ListBox();
            this.ButtonAdd = new System.Windows.Forms.Button();
            this.ButtonRemove = new System.Windows.Forms.Button();
            this.ListBoxAvailableValues = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // LabelNewName
            // 
            this.LabelNewName.AutoSize = true;
            this.LabelNewName.Location = new System.Drawing.Point(3, 12);
            this.LabelNewName.Name = "LabelNewName";
            this.LabelNewName.Size = new System.Drawing.Size(54, 13);
            this.LabelNewName.TabIndex = 53;
            this.LabelNewName.Text = "New name";
            // 
            // TextBoxNewName
            // 
            this.TextBoxNewName.Location = new System.Drawing.Point(3, 28);
            this.TextBoxNewName.Name = "TextBoxNewName";
            this.TextBoxNewName.Size = new System.Drawing.Size(219, 20);
            this.TextBoxNewName.TabIndex = 1;
            this.TextBoxNewName.Text = "New category";
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.Location = new System.Drawing.Point(126, 307);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(58, 23);
            this.ButtonCancel.TabIndex = 7;
            this.ButtonCancel.Text = "Cancel";
            this.ButtonCancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // ButtonSubmit
            // 
            this.ButtonSubmit.Location = new System.Drawing.Point(43, 307);
            this.ButtonSubmit.Name = "ButtonSubmit";
            this.ButtonSubmit.Size = new System.Drawing.Size(58, 23);
            this.ButtonSubmit.TabIndex = 6;
            this.ButtonSubmit.Text = "Submit";
            this.ButtonSubmit.Click += new System.EventHandler(this.Submit_Click);
            // 
            // LabelExistingValues
            // 
            this.LabelExistingValues.AutoSize = true;
            this.LabelExistingValues.Location = new System.Drawing.Point(125, 67);
            this.LabelExistingValues.Name = "LabelExistingValues";
            this.LabelExistingValues.Size = new System.Drawing.Size(73, 13);
            this.LabelExistingValues.TabIndex = 49;
            this.LabelExistingValues.Text = "Existing values";
            // 
            // LabelAvailableValues
            // 
            this.LabelAvailableValues.AutoSize = true;
            this.LabelAvailableValues.Location = new System.Drawing.Point(3, 67);
            this.LabelAvailableValues.Name = "LabelAvailableValues";
            this.LabelAvailableValues.Size = new System.Drawing.Size(80, 13);
            this.LabelAvailableValues.TabIndex = 48;
            this.LabelAvailableValues.Text = "Available values";
            // 
            // ListBoxExistingValues
            // 
            this.ListBoxExistingValues.FormattingEnabled = true;
            this.ListBoxExistingValues.Location = new System.Drawing.Point(126, 83);
            this.ListBoxExistingValues.Name = "ListBoxExistingValues";
            this.ListBoxExistingValues.Size = new System.Drawing.Size(96, 212);
            this.ListBoxExistingValues.TabIndex = 3;
            // 
            // ButtonAdd
            // 
            this.ButtonAdd.Location = new System.Drawing.Point(105, 97);
            this.ButtonAdd.Name = "ButtonAdd";
            this.ButtonAdd.Size = new System.Drawing.Size(15, 50);
            this.ButtonAdd.TabIndex = 4;
            this.ButtonAdd.Text = ">";
            this.ButtonAdd.Click += new System.EventHandler(this.Add_Click);
            // 
            // ButtonRemove
            // 
            this.ButtonRemove.Location = new System.Drawing.Point(105, 153);
            this.ButtonRemove.Name = "ButtonRemove";
            this.ButtonRemove.Size = new System.Drawing.Size(15, 50);
            this.ButtonRemove.TabIndex = 5;
            this.ButtonRemove.Text = "<";
            this.ButtonRemove.Click += new System.EventHandler(this.Remove_Click);
            // 
            // ListBoxAvailableValues
            // 
            this.ListBoxAvailableValues.FormattingEnabled = true;
            this.ListBoxAvailableValues.Location = new System.Drawing.Point(3, 83);
            this.ListBoxAvailableValues.Name = "ListBoxAvailableValues";
            this.ListBoxAvailableValues.Size = new System.Drawing.Size(96, 212);
            this.ListBoxAvailableValues.TabIndex = 2;
            // 
            // CreateSetWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.LabelNewName);
            this.Controls.Add(this.TextBoxNewName);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.ButtonSubmit);
            this.Controls.Add(this.LabelExistingValues);
            this.Controls.Add(this.LabelAvailableValues);
            this.Controls.Add(this.ListBoxExistingValues);
            this.Controls.Add(this.ButtonAdd);
            this.Controls.Add(this.ButtonRemove);
            this.Controls.Add(this.ListBoxAvailableValues);
            this.Name = "CreateSetWizard";
            this.Size = new System.Drawing.Size(228, 370);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label LabelNewName;
        private System.Windows.Forms.Label LabelExistingValues;
        private System.Windows.Forms.Label LabelAvailableValues;
        internal System.Windows.Forms.Button ButtonCancel;
        internal System.Windows.Forms.Button ButtonSubmit;
        internal System.Windows.Forms.ListBox ListBoxExistingValues;
        internal System.Windows.Forms.Button ButtonAdd;
        internal System.Windows.Forms.Button ButtonRemove;
        internal System.Windows.Forms.ListBox ListBoxAvailableValues;
        internal System.Windows.Forms.TextBox TextBoxNewName;
    }
}