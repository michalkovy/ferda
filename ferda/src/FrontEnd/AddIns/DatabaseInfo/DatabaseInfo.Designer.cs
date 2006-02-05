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
                this.components = new System.ComponentModel.Container();
                this.DataBaseInfoListView = new System.Windows.Forms.ListView();
                this.TableName = new System.Windows.Forms.ColumnHeader();
                this.TableRemarks = new System.Windows.Forms.ColumnHeader();
                this.TableRowCount = new System.Windows.Forms.ColumnHeader();
                this.TableType = new System.Windows.Forms.ColumnHeader();
                this.ContextMenuData = new System.Windows.Forms.ContextMenuStrip(this.components);
                this.ToolStripMenuItemCopyAll = new System.Windows.Forms.ToolStripMenuItem();
                this.ToolStripMenuItemCopySelected = new System.Windows.Forms.ToolStripMenuItem();
                this.ContextMenuData.SuspendLayout();
                this.SuspendLayout();
                // 
                // DataBaseInfoListView
                // 
                this.DataBaseInfoListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.TableName,
            this.TableRemarks,
            this.TableRowCount,
            this.TableType});
                this.DataBaseInfoListView.ContextMenuStrip = this.ContextMenuData;
                this.DataBaseInfoListView.Dock = System.Windows.Forms.DockStyle.Fill;
                this.DataBaseInfoListView.FullRowSelect = true;
                this.DataBaseInfoListView.GridLines = true;
                this.DataBaseInfoListView.Location = new System.Drawing.Point(0, 0);
                this.DataBaseInfoListView.Name = "DataBaseInfoListView";
                this.DataBaseInfoListView.Size = new System.Drawing.Size(467, 262);
                this.DataBaseInfoListView.TabIndex = 0;
                this.DataBaseInfoListView.UseCompatibleStateImageBehavior = false;
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
                // ContextMenuData
                // 
                this.ContextMenuData.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemCopyAll,
            this.ToolStripMenuItemCopySelected});
                this.ContextMenuData.Name = "ContextMenuData";
                this.ContextMenuData.Size = new System.Drawing.Size(180, 48);
                // 
                // ToolStripMenuItemCopyAll
                // 
                this.ToolStripMenuItemCopyAll.Name = "ToolStripMenuItemCopyAll";
                this.ToolStripMenuItemCopyAll.Size = new System.Drawing.Size(179, 22);
                this.ToolStripMenuItemCopyAll.Text = "toolStripMenuItem1";
                // 
                // ToolStripMenuItemCopySelected
                // 
                this.ToolStripMenuItemCopySelected.Name = "ToolStripMenuItemCopySelected";
                this.ToolStripMenuItemCopySelected.Size = new System.Drawing.Size(179, 22);
                this.ToolStripMenuItemCopySelected.Text = "toolStripMenuItem1";
                // 
                // DataBaseInfo
                // 
                this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.DataBaseInfoListView);
                this.Name = "DataBaseInfo";
                this.Size = new System.Drawing.Size(467, 262);
                this.ContextMenuData.ResumeLayout(false);
                this.ResumeLayout(false);

            }

            #endregion

            private System.Windows.Forms.ListView DataBaseInfoListView;
            private System.Windows.Forms.ColumnHeader TableName;
            private System.Windows.Forms.ColumnHeader TableRemarks;
            private System.Windows.Forms.ColumnHeader TableRowCount;
            private System.Windows.Forms.ColumnHeader TableType;
            private System.Windows.Forms.ContextMenuStrip ContextMenuData;
            private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemCopyAll;
            private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemCopySelected;

        }
    }
}