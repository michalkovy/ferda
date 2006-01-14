using Ferda;
namespace Ferda
{
    namespace ShowTable
    {
        partial class MainWindowForm
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
                this.mainWindow1 = new ShowTable.GUIClasses.MainWindow();
                this.SuspendLayout();
                // 
                // mainWindow1
                // 
                this.mainWindow1.Location = new System.Drawing.Point(2, 3);
                this.mainWindow1.Name = "mainWindow1";
                this.mainWindow1.Size = new System.Drawing.Size(769, 433);
                this.mainWindow1.TabIndex = 0;
                // 
                // MainWindowForm
                // 
                this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.ClientSize = new System.Drawing.Size(773, 440);
                this.Controls.Add(this.mainWindow1);
                this.Name = "MainWindowForm";
                this.Text = "ShowTable - Sample Form";
                this.ResumeLayout(false);

            }

            #endregion

            private global::Ferda.ShowTable.GUIClasses.MainWindow mainWindow1;
        }
    }
}

