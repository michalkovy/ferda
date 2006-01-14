using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Resources;

namespace Ferda.FrontEnd
{
    public class UserNoteDialog : Form
    {
        #region Fields

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button BOk;
        private System.Windows.Forms.Button BCancel;
        private System.Windows.Forms.RichTextBox RTBUserNote;

        #endregion

        #region Properties

        /// <summary>
        /// User note from the dialog
        /// </summary>
        public string UserNote
        {
            get
            {
                return RTBUserNote.Text;
            }
        }

        #endregion

        /// <summary>
        /// Default constructor of the class
        /// </summary>
        /// <param name="resManager">Resource manager for lacalization</param>
        /// <param name="boxName">Name of the which user note is beeing edited</param>
        /// <param name="userNote">Note (IBoxModule.UserHint) present in da box</param>
        public UserNoteDialog(ResourceManager resManager, string boxName, string userNote)
        {
            InitializeComponent();

            BOk.Text = resManager.GetString("OKButton");
            BCancel.Text = resManager.GetString("CancelButton");
            Text = resManager.GetString("UserNoteCaption") + boxName;
            RTBUserNote.Text = userNote;
        }

        #region Constructor

        #endregion

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
            this.BOk = new System.Windows.Forms.Button();
            this.BCancel = new System.Windows.Forms.Button();
            this.RTBUserNote = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // BOk
            // 
            this.BOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.BOk.Location = new System.Drawing.Point(124, 142);
            this.BOk.Name = "BOk";
            this.BOk.Size = new System.Drawing.Size(75, 23);
            this.BOk.TabIndex = 0;
            this.BOk.Text = "Ok";
            this.BOk.UseVisualStyleBackColor = true;
            // 
            // BCancel
            // 
            this.BCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BCancel.Location = new System.Drawing.Point(205, 142);
            this.BCancel.Name = "BCancel";
            this.BCancel.Size = new System.Drawing.Size(75, 23);
            this.BCancel.TabIndex = 1;
            this.BCancel.Text = "Cancel";
            this.BCancel.UseVisualStyleBackColor = true;
            // 
            // RTBUserNote
            // 
            this.RTBUserNote.Location = new System.Drawing.Point(12, 12);
            this.RTBUserNote.Name = "RTBUserNote";
            this.RTBUserNote.Size = new System.Drawing.Size(268, 124);
            this.RTBUserNote.TabIndex = 2;
            this.RTBUserNote.Text = "";
            // 
            // UserNoteDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BCancel;
            this.ClientSize = new System.Drawing.Size(292, 177);
            this.Controls.Add(this.RTBUserNote);
            this.Controls.Add(this.BCancel);
            this.Controls.Add(this.BOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserNoteDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "UserNoteDialog";
            this.ResumeLayout(false);

        }

        #endregion

    }
}