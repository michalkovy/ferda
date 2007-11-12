namespace Ferda.FrontEnd.AddIns.OntologyMapping
{
    partial class OntologyMappingControl
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.DataTableListBox = new System.Windows.Forms.ListBox();
            this.OntologyListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(311, 256);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Ok";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // DataTableListBox
            // 
            this.DataTableListBox.FormattingEnabled = true;
            this.DataTableListBox.Location = new System.Drawing.Point(12, 12);
            this.DataTableListBox.Name = "DataTableListBox";
            this.DataTableListBox.Size = new System.Drawing.Size(316, 225);
            this.DataTableListBox.TabIndex = 2;
            // 
            // OntologyListBox
            // 
            this.OntologyListBox.FormattingEnabled = true;
            this.OntologyListBox.Location = new System.Drawing.Point(368, 12);
            this.OntologyListBox.Name = "OntologyListBox";
            this.OntologyListBox.Size = new System.Drawing.Size(316, 225);
            this.OntologyListBox.TabIndex = 3;
            // 
            // OntologyMappingControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(696, 573);
            this.Controls.Add(this.OntologyListBox);
            this.Controls.Add(this.DataTableListBox);
            this.Controls.Add(this.button1);
            this.Name = "OntologyMappingControl";
            this.Text = "OntologyMappingControl";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox DataTableListBox;
        private System.Windows.Forms.ListBox OntologyListBox;
    }
}