namespace Ferda
{
    namespace ShowTable.GUIClasses
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
                this.components = new System.ComponentModel.Container();
                this.dataGridView1 = new System.Windows.Forms.DataGrid();
                this.MakeDataTableBindingSource = new System.Windows.Forms.BindingSource(this.components);
                this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
                this.MakeDataTableBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
                ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
                ((System.ComponentModel.ISupportInitialize)(this.MakeDataTableBindingSource)).BeginInit();
                ((System.ComponentModel.ISupportInitialize)(this.MakeDataTableBindingSource1)).BeginInit();
                this.SuspendLayout();
                // 
                // dataGridView1
                // 
                this.dataGridView1.DataMember = "";
                this.dataGridView1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
                this.dataGridView1.Location = new System.Drawing.Point(3, 3);
                this.dataGridView1.Name = "dataGridView1";
                this.dataGridView1.Size = new System.Drawing.Size(763, 430);
                this.dataGridView1.TabIndex = 0;
                // 
                // dataGridViewTextBoxColumn1
                // 
                this.dataGridViewTextBoxColumn1.HeaderText = "Column1";
                this.dataGridViewTextBoxColumn1.Name = "Column1";
                // 
                // MakeDataTableBindingSource1
                // 
                this.MakeDataTableBindingSource1.DataSource = typeof(Ferda.ShowTable.NonGUIClasses.MakeDataTable);
                // 
                // MainWindow
                // 
                this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.dataGridView1);
                this.Name = "MainWindow";
                this.Size = new System.Drawing.Size(769, 433);
                ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
                ((System.ComponentModel.ISupportInitialize)(this.MakeDataTableBindingSource)).EndInit();
                ((System.ComponentModel.ISupportInitialize)(this.MakeDataTableBindingSource1)).EndInit();
                this.ResumeLayout(false);

            }

            #endregion

            private System.Windows.Forms.DataGrid dataGridView1;
            private System.Windows.Forms.BindingSource MakeDataTableBindingSource;
            private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
            private System.Windows.Forms.BindingSource MakeDataTableBindingSource1;
        }
    }
}
