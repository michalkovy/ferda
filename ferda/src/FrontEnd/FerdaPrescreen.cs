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