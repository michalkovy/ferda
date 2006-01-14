namespace Ferda
{
    namespace FrontEnd.AddIns.DatabaseInfo
    {
        partial class DataBaseInfo
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
                this.DataBaseInfoListView = new System.Windows.Forms.ListView();
                this.TableName = new System.Windows.Forms.ColumnHeader();
                this.TableRemarks = new System.Windows.Forms.ColumnHeader();
                this.TableRowCount = new System.Windows.Forms.ColumnHeader();
                this.TableType = new System.Windows.Forms.ColumnHeader();
                this.SuspendLayout();
                // 
                // DataBaseInfoListView
                // 
                this.DataBaseInfoListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.TableName,
            this.TableRemarks,
            this.TableRowCount,
            this.TableType});
                this.DataBaseInfoListView.Dock = System.Windows.Forms.DockStyle.Fill;
                this.DataBaseInfoListView.FullRowSelect = true;
                this.DataBaseInfoListView.GridLines = true;
                this.DataBaseInfoListView.Location = new System.Drawing.Point(0, 0);
                this.DataBaseInfoListView.Name = "DataBaseInfoListView";
                this.DataBaseInfoListView.Size = new System.Drawing.Size(467, 262);
                this.DataBaseInfoListView.TabIndex = 0;
                this.DataBaseInfoListView.View = System.Windows.Forms.View.Details;
                // 
                // TableName
                // 
                this.TableName.Width = 106;
                // 
                // TableRemarks
                // 
                this.TableRemarks.Width = 109;
                // 
                // TableRowCount
                // 
                this.TableRowCount.Width = 93;
                // 
                // TableType
                // 
                this.TableType.Width = 87;
                // 
                // DataBaseInfo
                // 
                this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.DataBaseInfoListView);
                this.Name = "DataBaseInfo";
                this.Size = new System.Drawing.Size(467, 262);
                this.ResumeLayout(false);

            }

            #endregion

            private System.Windows.Forms.ListView DataBaseInfoListView;
            private System.Windows.Forms.ColumnHeader TableName;
            private System.Windows.Forms.ColumnHeader TableRemarks;
            private System.Windows.Forms.ColumnHeader TableRowCount;
            private System.Windows.Forms.ColumnHeader TableType;

        }
    }
}