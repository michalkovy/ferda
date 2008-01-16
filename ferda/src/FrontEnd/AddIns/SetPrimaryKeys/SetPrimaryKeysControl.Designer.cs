namespace Ferda.FrontEnd.AddIns.SetPrimaryKeys
{
    partial class SetPrimaryKeysControl
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.Button2Submit = new System.Windows.Forms.Button();
            this.Button1Cancel = new System.Windows.Forms.Button();
            this.Button3Help = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Button2Submit);
            this.panel1.Controls.Add(this.Button1Cancel);
            this.panel1.Controls.Add(this.Button3Help);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 524);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(600, 26);
            this.panel1.TabIndex = 0;
            // 
            // Button2Submit
            // 
            this.Button2Submit.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Button2Submit.Dock = System.Windows.Forms.DockStyle.Right;
            this.Button2Submit.Location = new System.Drawing.Point(375, 0);
            this.Button2Submit.Name = "Button2Submit";
            this.Button2Submit.Size = new System.Drawing.Size(75, 26);
            this.Button2Submit.TabIndex = 1;
            this.Button2Submit.Text = "Submit";
            this.Button2Submit.UseVisualStyleBackColor = true;
            this.Button2Submit.Click += new System.EventHandler(this.Button2Submit_Click);
            // 
            // Button1Cancel
            // 
            this.Button1Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Button1Cancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.Button1Cancel.Location = new System.Drawing.Point(450, 0);
            this.Button1Cancel.Name = "Button1Cancel";
            this.Button1Cancel.Size = new System.Drawing.Size(75, 26);
            this.Button1Cancel.TabIndex = 2;
            this.Button1Cancel.Text = "Cancel";
            this.Button1Cancel.UseVisualStyleBackColor = true;
            // 
            // Button3Help
            // 
            this.Button3Help.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Button3Help.Dock = System.Windows.Forms.DockStyle.Right;
            this.Button3Help.Location = new System.Drawing.Point(525, 0);
            this.Button3Help.Name = "Button3Help";
            this.Button3Help.Size = new System.Drawing.Size(75, 26);
            this.Button3Help.TabIndex = 3;
            this.Button3Help.Text = "Help";
            this.Button3Help.UseVisualStyleBackColor = true;
            this.Button3Help.Click += new System.EventHandler(this.Button3Help_Click);
            // 
            // SetPrimaryKeysControl
            // 
            this.ClientSize = new System.Drawing.Size(600, 550);
            this.Controls.Add(this.panel1);
            this.Name = "SetPrimaryKeysControl";
            this.Resize += new System.EventHandler(this.SetPrimaryKeysControl_Resize);
            this.ResizeEnd += new System.EventHandler(this.SetPrimaryKeysControl_ResizeEnd);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button Button2Submit;
        private System.Windows.Forms.Button Button1Cancel;
        private System.Windows.Forms.Button Button3Help;
    }
}
