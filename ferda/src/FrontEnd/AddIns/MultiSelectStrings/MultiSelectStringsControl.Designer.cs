namespace Ferda.FrontEnd.AddIns.MultiSelectStrings
{
    partial class MultiSelectStringsControl
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
            this.Button1Cancel = new System.Windows.Forms.Button();
            this.Button2Submit = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.CheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Button1Cancel
            // 
            this.Button1Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Button1Cancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.Button1Cancel.Location = new System.Drawing.Point(454, 0);
            this.Button1Cancel.Name = "Button1Cancel";
            this.Button1Cancel.Size = new System.Drawing.Size(75, 26);
            this.Button1Cancel.TabIndex = 3;
            this.Button1Cancel.Text = "Cancel";
            this.Button1Cancel.UseVisualStyleBackColor = true;
            // 
            // Button2Submit
            // 
            this.Button2Submit.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Button2Submit.Dock = System.Windows.Forms.DockStyle.Right;
            this.Button2Submit.Location = new System.Drawing.Point(379, 0);
            this.Button2Submit.Name = "Button2Submit";
            this.Button2Submit.Size = new System.Drawing.Size(75, 26);
            this.Button2Submit.TabIndex = 2;
            this.Button2Submit.Text = "Submit";
            this.Button2Submit.UseVisualStyleBackColor = true;
            this.Button2Submit.Click += new System.EventHandler(this.Button2Submit_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Button2Submit);
            this.panel1.Controls.Add(this.Button1Cancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 261);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(529, 26);
            this.panel1.TabIndex = 3;
            // 
            // CheckedListBox
            // 
            this.CheckedListBox.CheckOnClick = true;
            this.CheckedListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CheckedListBox.FormattingEnabled = true;
            this.CheckedListBox.Location = new System.Drawing.Point(0, 0);
            this.CheckedListBox.Name = "CheckedListBox";
            this.CheckedListBox.Size = new System.Drawing.Size(529, 259);
            this.CheckedListBox.TabIndex = 1;
            // 
            // MultiSelectStringsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(529, 287);
            this.Controls.Add(this.CheckedListBox);
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(200, 200);
            this.Name = "MultiSelectStringsControl";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Button1Cancel;
        private System.Windows.Forms.Button Button2Submit;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckedListBox CheckedListBox;
    }
}
