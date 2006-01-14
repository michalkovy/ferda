namespace Ferda
{
    namespace FrontEnd.AddIns.ExplainTable
    {
        partial class ExplainTable
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
                this.ExplainTableListView = new System.Windows.Forms.ListView();
                this.ColumnName = new System.Windows.Forms.ColumnHeader();
                this.ColumnAllowDBNull = new System.Windows.Forms.ColumnHeader();
                this.ColumnOrdinal = new System.Windows.Forms.ColumnHeader();
                this.ColumnSize = new System.Windows.Forms.ColumnHeader();
                this.ColumnDataType = new System.Windows.Forms.ColumnHeader();
                this.ColumnAutoIncrement = new System.Windows.Forms.ColumnHeader();
                this.ColumnIsKey = new System.Windows.Forms.ColumnHeader();
                this.ColumnIsLong = new System.Windows.Forms.ColumnHeader();
                this.ColumnIsReadOnly = new System.Windows.Forms.ColumnHeader();
                this.ColumnIsRowVersion = new System.Windows.Forms.ColumnHeader();
                this.ColumnIsUnique = new System.Windows.Forms.ColumnHeader();
                this.ColumnNumericalPrecision = new System.Windows.Forms.ColumnHeader();
                this.ColumnNumericalScale = new System.Windows.Forms.ColumnHeader();
                this.ColumnProviderType = new System.Windows.Forms.ColumnHeader();
                this.SuspendLayout();
                // 
                // ExplainTableListView
                // 
                this.ExplainTableListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnName,
            this.ColumnAllowDBNull,
            this.ColumnOrdinal,
            this.ColumnSize,
            this.ColumnDataType,
            this.ColumnAutoIncrement,
            this.ColumnIsKey,
            this.ColumnIsLong,
            this.ColumnIsReadOnly,
            this.ColumnIsRowVersion,
            this.ColumnIsUnique,
            this.ColumnNumericalPrecision,
            this.ColumnNumericalScale,
            this.ColumnProviderType});
                this.ExplainTableListView.Dock = System.Windows.Forms.DockStyle.Fill;
                this.ExplainTableListView.FullRowSelect = true;
                this.ExplainTableListView.GridLines = true;
                this.ExplainTableListView.Location = new System.Drawing.Point(0, 0);
                this.ExplainTableListView.Name = "ExplainTableListView";
                this.ExplainTableListView.ShowItemToolTips = true;
                this.ExplainTableListView.Size = new System.Drawing.Size(730, 262);
                this.ExplainTableListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
                this.ExplainTableListView.TabIndex = 0;
                this.ExplainTableListView.View = System.Windows.Forms.View.Details;
                // 
                // ColumnName
                // 
                this.ColumnName.Width = 144;
                // 
                // ExplainTable
                // 
                this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.ExplainTableListView);
                this.Name = "ExplainTable";
                this.Size = new System.Drawing.Size(730, 262);
                this.ResumeLayout(false);

            }

            #endregion

            private System.Windows.Forms.ListView ExplainTableListView;
            private System.Windows.Forms.ColumnHeader ColumnName;
            private System.Windows.Forms.ColumnHeader ColumnOrdinal;
            private System.Windows.Forms.ColumnHeader ColumnSize;
            private System.Windows.Forms.ColumnHeader ColumnDataType;
            private System.Windows.Forms.ColumnHeader ColumnAutoIncrement;
            private System.Windows.Forms.ColumnHeader ColumnIsKey;
            private System.Windows.Forms.ColumnHeader ColumnIsLong;
            private System.Windows.Forms.ColumnHeader ColumnIsReadOnly;
            private System.Windows.Forms.ColumnHeader ColumnIsRowVersion;
            private System.Windows.Forms.ColumnHeader ColumnIsUnique;
            private System.Windows.Forms.ColumnHeader ColumnNumericalPrecision;
            private System.Windows.Forms.ColumnHeader ColumnProviderType;
            private System.Windows.Forms.ColumnHeader ColumnAllowDBNull;
            private System.Windows.Forms.ColumnHeader ColumnNumericalScale;

        }
    }
}