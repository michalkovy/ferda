namespace Ferda
{
    namespace DgFrequencies
    {
        partial class MainWindow
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
                this.groupBox1 = new System.Windows.Forms.GroupBox();
                this.listView1 = new System.Windows.Forms.ListView();
                this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
                this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
                this.groupBox1.SuspendLayout();
                this.SuspendLayout();
                // 
                // groupBox1
                // 
                this.groupBox1.Controls.Add(this.listView1);
                this.groupBox1.Location = new System.Drawing.Point(0, 3);
                this.groupBox1.Name = "groupBox1";
                this.groupBox1.Size = new System.Drawing.Size(515, 303);
                this.groupBox1.TabIndex = 0;
                this.groupBox1.TabStop = false;
                this.groupBox1.Text = "Frequencies browser";
               
                // 
                // listView1
                // 
                this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
                this.listView1.Location = new System.Drawing.Point(6, 19);
                this.listView1.Name = "listView1";
                this.listView1.Size = new System.Drawing.Size(503, 278);
                this.listView1.TabIndex = 0;
                this.listView1.View = System.Windows.Forms.View.Details;
                // 
                // columnHeader1
                // 
                this.columnHeader1.Text = "Attribute name";
                this.columnHeader1.Width = 336;
                // 
                // columnHeader2
                // 
                this.columnHeader2.Text = "Attribute frequency";
                this.columnHeader2.Width = 121;
                // 
                // MainWindow
                // 
                this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.groupBox1);
                this.Name = "MainWindow";
                this.Size = new System.Drawing.Size(518, 306);
                this.groupBox1.ResumeLayout(false);
                this.ResumeLayout(false);

            }

            #endregion

            private System.Windows.Forms.GroupBox groupBox1;
            private System.Windows.Forms.ListView listView1;
            private System.Windows.Forms.ColumnHeader columnHeader1;
            private System.Windows.Forms.ColumnHeader columnHeader2;
        }
    }
}