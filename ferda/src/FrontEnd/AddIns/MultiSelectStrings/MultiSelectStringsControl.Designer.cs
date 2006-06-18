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
            this.ButtonSubmit = new System.Windows.Forms.Button();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.CheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonSubmit
            // 
            this.ButtonSubmit.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ButtonSubmit.Dock = System.Windows.Forms.DockStyle.Left;
            this.ButtonSubmit.Location = new System.Drawing.Point(0, 0);
            this.ButtonSubmit.Name = "ButtonSubmit";
            this.ButtonSubmit.Size = new System.Drawing.Size(75, 26);
            this.ButtonSubmit.TabIndex = 1;
            this.ButtonSubmit.Text = "Submit";
            this.ButtonSubmit.UseVisualStyleBackColor = true;
            this.ButtonSubmit.Click += new System.EventHandler(this.ButtonSubmit_Click);
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.ButtonCancel.Location = new System.Drawing.Point(217, 0);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(75, 26);
            this.ButtonCancel.TabIndex = 2;
            this.ButtonCancel.Text = "Cancel";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ButtonCancel);
            this.panel1.Controls.Add(this.ButtonSubmit);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 247);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(292, 26);
            this.panel1.TabIndex = 3;
            // 
            // CheckedListBox
            // 
            this.CheckedListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CheckedListBox.FormattingEnabled = true;
            this.CheckedListBox.Location = new System.Drawing.Point(0, 0);
            this.CheckedListBox.Name = "CheckedListBox";
            this.CheckedListBox.Size = new System.Drawing.Size(292, 244);
            this.CheckedListBox.TabIndex = 4;
            // 
            // SelectTablesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.CheckedListBox);
            this.Controls.Add(this.panel1);
            this.Name = "SelectTablesControl";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ButtonSubmit;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckedListBox CheckedListBox;
    }
}
