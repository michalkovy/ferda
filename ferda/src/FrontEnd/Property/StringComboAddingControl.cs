// StringComboAddingControl.cs - control where user selects a string and can add its own
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
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using Ferda.Modules;

namespace Ferda.FrontEnd.Properties
{
    /// <summary>
    /// User control that displays a combobox, textbox and a button to 
    /// add new value to the combobox. It is used to edit and select 
    /// a option of the string
    /// </summary>
    internal class StringComboAddingControl : UserControl
    {
        #region Class fields

        /// <summary>
        /// Listbox with the options
        /// </summary>
        /// <remarks>
        /// this has to be public, because we need to react on the double click
        /// from the editor
        /// </remarks>
        public ListBox LBOptions;

        /// <summary>
        /// BUtton to add an option
        /// </summary>
        /// <remarks>
        /// this has to be public, because we need to react on the double click
        /// from the editor
        /// </remarks>
        public Button BAddOption;
        private TextBox TBNewOption;
        private Label LText;
        private StringSequence sequence;

        #endregion

        #region Constructors

        /// <summary>
        /// Constuctor that should be used
        /// </summary>
        /// <param name="seq">The control will be filled with this data</param>
        public StringComboAddingControl(StringSequence seq) : base()
        {
            SizeChanged += new EventHandler(StringComboAddingControl_SizeChanged);

            InitializeComponent();
            sequence = seq;

            LText.Text = sequence.ResManager.GetString("StringComboAddingControlText");
            BAddOption.Text = seq.ResManager.GetString("StringComboAddingControlButtonText");

            //filling the listbox
            foreach (SelectString ss in sequence.GetArray())
            {
                LBOptions.Items.Add(ss.label);
            }
        }

        /// <summary>
        /// Changes the size of controls in this control
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event parameters</param>
        void StringComboAddingControl_SizeChanged(object sender, EventArgs e)
        {
            Constants con = new Constants();

            if (Size != con.StringComboAddingControlInitialSize)
            {
                LBOptions.Width +=
                    (Size.Width - con.StringComboAddingControlInitialSize.Width);
                BAddOption.Width +=
                    (Size.Width - con.StringComboAddingControlInitialSize.Width);
                TBNewOption.Width +=
                    (Size.Width - con.StringComboAddingControlInitialSize.Width);
            }
        }

        #endregion

        /// <summary>
        /// Writes the new (other) option to the property
        /// </summary>
        public void SetOtherOption()
        {
            if (TBNewOption.Text != "")
            {
                sequence.selectedName = TBNewOption.Text;
                sequence.selectedLabel = TBNewOption.Text;
                sequence.SetSelectedOption();
            }
        }

        /// <summary>
        /// Writes the existing option from the list to the property
        /// </summary>
        public void SetListOption()
        {
            if (LBOptions.SelectedIndex != -1)
            {
                //setting the property into the ProjectManager
                string name = string.Empty;
                foreach (SelectString ss in sequence.GetArray())
                {
                    if (ss.label == LBOptions.SelectedItem.ToString())
                    {
                        name = ss.name;
                        break;
                    }
                }

                //checking some name was found
                if (name == string.Empty)
                {
                    throw new ApplicationException("Inconsistent label and name in SelectString");
                }

                sequence.selectedName = name;
                sequence.selectedLabel = LBOptions.SelectedItem.ToString();
                sequence.SetSelectedOption();
            }
        }

        #region Component Designer generated code

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
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Constants con = new Constants();

            this.LBOptions = new System.Windows.Forms.ListBox();
            this.BAddOption = new System.Windows.Forms.Button();
            this.TBNewOption = new System.Windows.Forms.TextBox();
            this.LText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LBOptions
            // 
            this.LBOptions.FormattingEnabled = true;
            this.LBOptions.Location = new System.Drawing.Point(3, 3);
            this.LBOptions.Name = "LBOptions";
            this.LBOptions.Size = new System.Drawing.Size(120, 95);
            this.LBOptions.TabIndex = 0;
            // 
            // BAddOption
            // 
            this.BAddOption.BackColor = System.Drawing.SystemColors.ControlDark;
            this.BAddOption.Location = new System.Drawing.Point(3, 157);
            this.BAddOption.Name = "BAddOption";
            this.BAddOption.Size = new System.Drawing.Size(120, 23);
            this.BAddOption.TabIndex = 1;
            this.BAddOption.Text = "button1";
            this.BAddOption.UseVisualStyleBackColor = false;
            // 
            // TBNewOption
            // 
            this.TBNewOption.Location = new System.Drawing.Point(3, 131);
            this.TBNewOption.Name = "TBNewOption";
            this.TBNewOption.Size = new System.Drawing.Size(120, 20);
            this.TBNewOption.TabIndex = 2;
            // 
            // LText
            // 
            this.LText.AutoSize = true;
            this.LText.Location = new System.Drawing.Point(3, 101);
            this.LText.Name = "LText";
            this.LText.Size = new System.Drawing.Size(31, 13);
            this.LText.TabIndex = 3;
            this.LText.Text = "label1";
            // 
            // StringComboAddingControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.LText);
            this.Controls.Add(this.TBNewOption);
            this.Controls.Add(this.BAddOption);
            this.Controls.Add(this.LBOptions);
            this.Name = "StringComboAddingControl";
            this.Size = con.StringComboAddingControlInitialSize;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}
