namespace Ferda.FrontEnd.AddIns.ShowTable
{
    partial class ShowTableControl
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
            this.ListViewShowTable = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // ListViewShowTable
            // 
            this.ListViewShowTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListViewShowTable.GridLines = true;
            this.ListViewShowTable.Location = new System.Drawing.Point(0, 0);
            this.ListViewShowTable.MultiSelect = false;
            this.ListViewShowTable.Name = "ListViewShowTable";
            this.ListViewShowTable.ShowGroups = false;
            this.ListViewShowTable.Size = new System.Drawing.Size(558, 324);
            this.ListViewShowTable.TabIndex = 0;
            this.ListViewShowTable.UseCompatibleStateImageBehavior = false;
            this.ListViewShowTable.View = System.Windows.Forms.View.Details;
            // 
            // ShowTableControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ListViewShowTable);
            this.Name = "ShowTableControl";
            this.Size = new System.Drawing.Size(558, 324);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView ListViewShowTable;
    }
}
