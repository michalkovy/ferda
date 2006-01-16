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
        private Panel PImage;
        private Button BOk;
        private Label LFerda;
        private RichTextBox RTBDescription;
        private LinkLabel LLAddress;
        private Label LVersion;
        #region Fields

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            str.Append("\nMichal Kováè:\tProject Manager, Modules Manager");
            str.Append("\nTomáš Kuchaø:\tImplementation of boxes");
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
        private string GetVersionFromAssembly(Assembly assembly)
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