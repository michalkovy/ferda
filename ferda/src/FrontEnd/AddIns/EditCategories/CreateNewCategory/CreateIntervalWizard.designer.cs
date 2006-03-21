// CreateIntervalWizard.cs - GUI part
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
    partial class CreateIntervalWizard
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
            this.ButtonAddInterval = new System.Windows.Forms.Button();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.ButtonSubmit = new System.Windows.Forms.Button();
            this.LabelLeftBoundType = new System.Windows.Forms.Label();
            this.LabelLeftBoundValue = new System.Windows.Forms.Label();
            this.RadioLeftBoundSharp = new System.Windows.Forms.RadioButton();
            this.RadioLeftBoundRound = new System.Windows.Forms.RadioButton();
            this.TextBoxLeftBound = new System.Windows.Forms.TextBox();
            this.LabelRightBoundType = new System.Windows.Forms.Label();
            this.LabelRightBoundValue = new System.Windows.Forms.Label();
            this.RadioRightBoundSharp = new System.Windows.Forms.RadioButton();
            this.RadioRightBoundRound = new System.Windows.Forms.RadioButton();
            this.TextBoxRightBound = new System.Windows.Forms.TextBox();
            this.TextBoxCategoryName = new System.Windows.Forms.TextBox();
            this.LabelNewName = new System.Windows.Forms.Label();
            this.RadioMinusInfinity = new System.Windows.Forms.RadioButton();
            this.RadioPlusInfinity = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ListBoxIntervals = new System.Windows.Forms.ListBox();
            this.LabelIntervals = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.RadioFloat = new System.Windows.Forms.RadioButton();
            this.RadioLong = new System.Windows.Forms.RadioButton();
            this.LabelIntervalType = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonAddInterval
            // 
            this.ButtonAddInterval.Location = new System.Drawing.Point(51, 313);
            this.ButtonAddInterval.Name = "ButtonAddInterval";
            this.ButtonAddInterval.Size = new System.Drawing.Size(102, 23);
            this.ButtonAddInterval.TabIndex = 12;
            this.ButtonAddInterval.Text = "Add to category";
            this.ButtonAddInterval.Click += new System.EventHandler(this.AddToCategoryButton_Click);
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.Location = new System.Drawing.Point(133, 470);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(63, 23);
            this.ButtonCancel.TabIndex = 15;
            this.ButtonCancel.Text = "Cancel";
            this.ButtonCancel.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // ButtonSubmit
            // 
            this.ButtonSubmit.Location = new System.Drawing.Point(16, 470);
            this.ButtonSubmit.Name = "ButtonSubmit";
            this.ButtonSubmit.Size = new System.Drawing.Size(63, 23);
            this.ButtonSubmit.TabIndex = 14;
            this.ButtonSubmit.Text = "Submit";
            this.ButtonSubmit.Click += new System.EventHandler(this.SubmitButton_Click);
            // 
            // LabelLeftBoundType
            // 
            this.LabelLeftBoundType.AutoSize = true;
            this.LabelLeftBoundType.Location = new System.Drawing.Point(15, 175);
            this.LabelLeftBoundType.Name = "LabelLeftBoundType";
            this.LabelLeftBoundType.Size = new System.Drawing.Size(81, 13);
            this.LabelLeftBoundType.TabIndex = 32;
            this.LabelLeftBoundType.Text = "Left bound type";
            // 
            // LabelLeftBoundValue
            // 
            this.LabelLeftBoundValue.AutoSize = true;
            this.LabelLeftBoundValue.Location = new System.Drawing.Point(3, 66);
            this.LabelLeftBoundValue.Name = "LabelLeftBoundValue";
            this.LabelLeftBoundValue.Size = new System.Drawing.Size(90, 13);
            this.LabelLeftBoundValue.TabIndex = 30;
            this.LabelLeftBoundValue.Text = "Left bound value ";
            // 
            // RadioLeftBoundSharp
            // 
            this.RadioLeftBoundSharp.AutoSize = true;
            this.RadioLeftBoundSharp.Location = new System.Drawing.Point(3, 77);
            this.RadioLeftBoundSharp.Name = "RadioLeftBoundSharp";
            this.RadioLeftBoundSharp.Size = new System.Drawing.Size(31, 17);
            this.RadioLeftBoundSharp.TabIndex = 8;
            this.RadioLeftBoundSharp.Text = "<";
            // 
            // RadioLeftBoundRound
            // 
            this.RadioLeftBoundRound.AutoSize = true;
            this.RadioLeftBoundRound.Checked = true;
            this.RadioLeftBoundRound.Location = new System.Drawing.Point(3, 38);
            this.RadioLeftBoundRound.Name = "RadioLeftBoundRound";
            this.RadioLeftBoundRound.Size = new System.Drawing.Size(28, 17);
            this.RadioLeftBoundRound.TabIndex = 7;
            this.RadioLeftBoundRound.TabStop = true;
            this.RadioLeftBoundRound.Text = "(";
            // 
            // TextBoxLeftBound
            // 
            this.TextBoxLeftBound.Location = new System.Drawing.Point(3, 83);
            this.TextBoxLeftBound.Name = "TextBoxLeftBound";
            this.TextBoxLeftBound.Size = new System.Drawing.Size(96, 20);
            this.TextBoxLeftBound.TabIndex = 2;
            // 
            // LabelRightBoundType
            // 
            this.LabelRightBoundType.AutoSize = true;
            this.LabelRightBoundType.Location = new System.Drawing.Point(131, 175);
            this.LabelRightBoundType.Name = "LabelRightBoundType";
            this.LabelRightBoundType.Size = new System.Drawing.Size(88, 13);
            this.LabelRightBoundType.TabIndex = 26;
            this.LabelRightBoundType.Text = "Right bound type";
            // 
            // LabelRightBoundValue
            // 
            this.LabelRightBoundValue.AutoSize = true;
            this.LabelRightBoundValue.Location = new System.Drawing.Point(125, 66);
            this.LabelRightBoundValue.Name = "LabelRightBoundValue";
            this.LabelRightBoundValue.Size = new System.Drawing.Size(97, 13);
            this.LabelRightBoundValue.TabIndex = 23;
            this.LabelRightBoundValue.Text = "Right bound value ";
            // 
            // RadioRightBoundSharp
            // 
            this.RadioRightBoundSharp.AutoSize = true;
            this.RadioRightBoundSharp.Location = new System.Drawing.Point(3, 77);
            this.RadioRightBoundSharp.Name = "RadioRightBoundSharp";
            this.RadioRightBoundSharp.Size = new System.Drawing.Size(31, 17);
            this.RadioRightBoundSharp.TabIndex = 11;
            this.RadioRightBoundSharp.Text = ">";
            // 
            // RadioRightBoundRound
            // 
            this.RadioRightBoundRound.AutoSize = true;
            this.RadioRightBoundRound.Checked = true;
            this.RadioRightBoundRound.Location = new System.Drawing.Point(3, 38);
            this.RadioRightBoundRound.Name = "RadioRightBoundRound";
            this.RadioRightBoundRound.Size = new System.Drawing.Size(28, 17);
            this.RadioRightBoundRound.TabIndex = 10;
            this.RadioRightBoundRound.TabStop = true;
            this.RadioRightBoundRound.Text = ")";
            // 
            // TextBoxRightBound
            // 
            this.TextBoxRightBound.Location = new System.Drawing.Point(126, 83);
            this.TextBoxRightBound.Name = "TextBoxRightBound";
            this.TextBoxRightBound.Size = new System.Drawing.Size(96, 20);
            this.TextBoxRightBound.TabIndex = 3;
            // 
            // TextBoxCategoryName
            // 
            this.TextBoxCategoryName.Location = new System.Drawing.Point(3, 28);
            this.TextBoxCategoryName.Name = "TextBoxCategoryName";
            this.TextBoxCategoryName.Size = new System.Drawing.Size(219, 20);
            this.TextBoxCategoryName.TabIndex = 1;
            this.TextBoxCategoryName.Text = "New category";
            // 
            // LabelNewName
            // 
            this.LabelNewName.AutoSize = true;
            this.LabelNewName.Location = new System.Drawing.Point(3, 11);
            this.LabelNewName.Name = "LabelNewName";
            this.LabelNewName.Size = new System.Drawing.Size(102, 13);
            this.LabelNewName.TabIndex = 34;
            this.LabelNewName.Text = "New category name";
            // 
            // RadioMinusInfinity
            // 
            this.RadioMinusInfinity.AutoSize = true;
            this.RadioMinusInfinity.Location = new System.Drawing.Point(3, 3);
            this.RadioMinusInfinity.Name = "RadioMinusInfinity";
            this.RadioMinusInfinity.Size = new System.Drawing.Size(60, 17);
            this.RadioMinusInfinity.TabIndex = 6;
            this.RadioMinusInfinity.Text = "- infinity";
            this.RadioMinusInfinity.CheckedChanged += new System.EventHandler(this.MinusInfinity_CheckedChanged);
            // 
            // RadioPlusInfinity
            // 
            this.RadioPlusInfinity.AutoSize = true;
            this.RadioPlusInfinity.Location = new System.Drawing.Point(3, 3);
            this.RadioPlusInfinity.Name = "RadioPlusInfinity";
            this.RadioPlusInfinity.Size = new System.Drawing.Size(63, 17);
            this.RadioPlusInfinity.TabIndex = 9;
            this.RadioPlusInfinity.Text = "+ infinity";
            this.RadioPlusInfinity.CheckedChanged += new System.EventHandler(this.PlusInfinity_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.RadioLeftBoundRound);
            this.panel1.Controls.Add(this.RadioMinusInfinity);
            this.panel1.Controls.Add(this.RadioLeftBoundSharp);
            this.panel1.Location = new System.Drawing.Point(16, 199);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(64, 97);
            this.panel1.TabIndex = 35;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.RadioPlusInfinity);
            this.panel2.Controls.Add(this.RadioRightBoundRound);
            this.panel2.Controls.Add(this.RadioRightBoundSharp);
            this.panel2.Location = new System.Drawing.Point(132, 199);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(64, 97);
            this.panel2.TabIndex = 36;
            // 
            // ListBoxIntervals
            // 
            this.ListBoxIntervals.FormattingEnabled = true;
            this.ListBoxIntervals.Location = new System.Drawing.Point(16, 374);
            this.ListBoxIntervals.Name = "ListBoxIntervals";
            this.ListBoxIntervals.Size = new System.Drawing.Size(180, 82);
            this.ListBoxIntervals.TabIndex = 13;
            this.ListBoxIntervals.DoubleClick += new System.EventHandler(this.ListBoxIntervals_DoubleClick);
            this.ListBoxIntervals.SelectedIndexChanged += new System.EventHandler(this.ListBoxIntervals_SelectedIndexChanged);
            // 
            // LabelIntervals
            // 
            this.LabelIntervals.AutoSize = true;
            this.LabelIntervals.Location = new System.Drawing.Point(16, 357);
            this.LabelIntervals.Name = "LabelIntervals";
            this.LabelIntervals.Size = new System.Drawing.Size(120, 13);
            this.LabelIntervals.TabIndex = 38;
            this.LabelIntervals.Text = "Intervals in the category";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.RadioFloat);
            this.panel3.Controls.Add(this.RadioLong);
            this.panel3.Location = new System.Drawing.Point(19, 138);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(177, 22);
            this.panel3.TabIndex = 36;
            // 
            // RadioFloat
            // 
            this.RadioFloat.AutoSize = true;
            this.RadioFloat.Location = new System.Drawing.Point(125, 3);
            this.RadioFloat.Name = "RadioFloat";
            this.RadioFloat.Size = new System.Drawing.Size(48, 17);
            this.RadioFloat.TabIndex = 5;
            this.RadioFloat.Text = "Float";
            this.RadioFloat.UseVisualStyleBackColor = true;
            // 
            // RadioLong
            // 
            this.RadioLong.AutoSize = true;
            this.RadioLong.Checked = true;
            this.RadioLong.Location = new System.Drawing.Point(3, 3);
            this.RadioLong.Name = "RadioLong";
            this.RadioLong.Size = new System.Drawing.Size(49, 17);
            this.RadioLong.TabIndex = 4;
            this.RadioLong.TabStop = true;
            this.RadioLong.Text = "Long";
            this.RadioLong.UseVisualStyleBackColor = true;
            // 
            // LabelIntervalType
            // 
            this.LabelIntervalType.AutoSize = true;
            this.LabelIntervalType.Location = new System.Drawing.Point(19, 122);
            this.LabelIntervalType.Name = "LabelIntervalType";
            this.LabelIntervalType.Size = new System.Drawing.Size(65, 13);
            this.LabelIntervalType.TabIndex = 39;
            this.LabelIntervalType.Text = "Interval type";
            // 
            // CreateIntervalWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.LabelIntervalType);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.LabelIntervals);
            this.Controls.Add(this.ListBoxIntervals);
            this.Controls.Add(this.LabelNewName);
            this.Controls.Add(this.TextBoxCategoryName);
            this.Controls.Add(this.ButtonAddInterval);
            this.Controls.Add(this.LabelLeftBoundType);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.LabelLeftBoundValue);
            this.Controls.Add(this.ButtonSubmit);
            this.Controls.Add(this.TextBoxLeftBound);
            this.Controls.Add(this.LabelRightBoundType);
            this.Controls.Add(this.LabelRightBoundValue);
            this.Controls.Add(this.TextBoxRightBound);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "CreateIntervalWizard";
            this.Size = new System.Drawing.Size(228, 514);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion


        internal System.Windows.Forms.Button ButtonCancel;
        internal System.Windows.Forms.Button ButtonSubmit;
        internal System.Windows.Forms.Button ButtonAddInterval;
        internal System.Windows.Forms.Label LabelLeftBoundType;
        internal System.Windows.Forms.Label LabelLeftBoundValue;
        internal System.Windows.Forms.RadioButton RadioLeftBoundSharp;
        internal System.Windows.Forms.RadioButton RadioLeftBoundRound;
        internal System.Windows.Forms.TextBox TextBoxLeftBound;
        internal System.Windows.Forms.Label LabelRightBoundType;
        internal System.Windows.Forms.Label LabelRightBoundValue;
        internal System.Windows.Forms.RadioButton RadioRightBoundSharp;
        internal System.Windows.Forms.RadioButton RadioRightBoundRound;
        internal System.Windows.Forms.TextBox TextBoxRightBound;
        internal System.Windows.Forms.TextBox TextBoxCategoryName;
        private System.Windows.Forms.Label LabelNewName;
        internal System.Windows.Forms.RadioButton RadioPlusInfinity;
        internal System.Windows.Forms.RadioButton RadioMinusInfinity;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        protected System.Windows.Forms.ListBox ListBoxIntervals;
        protected System.Windows.Forms.Label LabelIntervals;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label LabelIntervalType;
        protected System.Windows.Forms.RadioButton RadioFloat;
        protected System.Windows.Forms.RadioButton RadioLong;
    }
}