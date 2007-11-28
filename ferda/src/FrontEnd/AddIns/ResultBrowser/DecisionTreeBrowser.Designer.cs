// DecisionTreeBrowser.cs - GUI part
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

namespace Ferda.FrontEnd.AddIns.ResultBrowser
{
    partial class DecisionTreeBrowser
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
            this.treesListView = new System.Windows.Forms.ListView();
            this.treesRTB = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // treesListView
            // 
            this.treesListView.Dock = System.Windows.Forms.DockStyle.Left;
            this.treesListView.GridLines = true;
            this.treesListView.Location = new System.Drawing.Point(0, 0);
            this.treesListView.MultiSelect = false;
            this.treesListView.Name = "treesListView";
            this.treesListView.ShowGroups = false;
            this.treesListView.Size = new System.Drawing.Size(255, 324);
            this.treesListView.TabIndex = 0;
            this.treesListView.UseCompatibleStateImageBehavior = false;
            this.treesListView.View = System.Windows.Forms.View.Details;
            // 
            // treesRTB
            // 
            this.treesRTB.AutoWordSelection = true;
            this.treesRTB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treesRTB.Location = new System.Drawing.Point(255, 0);
            this.treesRTB.Name = "treesRTB";
            this.treesRTB.ReadOnly = true;
            this.treesRTB.Size = new System.Drawing.Size(303, 324);
            this.treesRTB.TabIndex = 1;
            this.treesRTB.Text = "";
            // 
            // DecisionTreeBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treesRTB);
            this.Controls.Add(this.treesListView);
            this.Name = "DecisionTreeBrowser";
            this.Size = new System.Drawing.Size(558, 324);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
