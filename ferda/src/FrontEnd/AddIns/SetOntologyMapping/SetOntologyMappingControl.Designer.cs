namespace Ferda.FrontEnd.AddIns.SetOntologyMapping
{
    partial class SetOntologyMappingControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetOntologyMappingControl));
            this.okButton = new System.Windows.Forms.Button();
            this.MappingListBox = new System.Windows.Forms.ListBox();
            this.mapButton = new System.Windows.Forms.Button();
            this.unmapButton = new System.Windows.Forms.Button();
            this.loadButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.helpButton = new System.Windows.Forms.Button();
            this.infoBox = new System.Windows.Forms.TextBox();
            this.DataTableListBoxLabel = new System.Windows.Forms.Label();
            this.OntologyListBoxLabel = new System.Windows.Forms.Label();
            this.InfoBoxLabel = new System.Windows.Forms.Label();
            this.ColumnsRemoveButton = new System.Windows.Forms.Button();
            this.OntologyRemoveButton = new System.Windows.Forms.Button();
            this.ontologyTreeView = new System.Windows.Forms.TreeView();
            this.dataTablesTreeView = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(546, 580);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "Ok";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // MappingListBox
            // 
            this.MappingListBox.FormattingEnabled = true;
            this.MappingListBox.Location = new System.Drawing.Point(12, 375);
            this.MappingListBox.Name = "MappingListBox";
            this.MappingListBox.Size = new System.Drawing.Size(672, 199);
            this.MappingListBox.TabIndex = 4;
            // 
            // mapButton
            // 
            this.mapButton.Location = new System.Drawing.Point(309, 346);
            this.mapButton.Name = "mapButton";
            this.mapButton.Size = new System.Drawing.Size(75, 23);
            this.mapButton.TabIndex = 5;
            this.mapButton.Text = "Map";
            this.mapButton.UseVisualStyleBackColor = true;
            this.mapButton.Click += new System.EventHandler(this.mapButton_Click);
            // 
            // unmapButton
            // 
            this.unmapButton.Location = new System.Drawing.Point(384, 346);
            this.unmapButton.Name = "unmapButton";
            this.unmapButton.Size = new System.Drawing.Size(75, 23);
            this.unmapButton.TabIndex = 6;
            this.unmapButton.Text = "Unmap";
            this.unmapButton.UseVisualStyleBackColor = true;
            this.unmapButton.Click += new System.EventHandler(this.unmapButton_Click);
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(459, 346);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(118, 23);
            this.loadButton.TabIndex = 7;
            this.loadButton.Text = "Load Mapping...";
            this.loadButton.UseVisualStyleBackColor = true;
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(577, 346);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(118, 23);
            this.saveButton.TabIndex = 8;
            this.saveButton.Text = "Save Mapping...";
            this.saveButton.UseVisualStyleBackColor = true;
            // 
            // helpButton
            // 
            this.helpButton.Location = new System.Drawing.Point(621, 580);
            this.helpButton.Name = "helpButton";
            this.helpButton.Size = new System.Drawing.Size(75, 23);
            this.helpButton.TabIndex = 9;
            this.helpButton.Text = "Help";
            this.helpButton.UseVisualStyleBackColor = true;
            // 
            // infoBox
            // 
            this.infoBox.Enabled = false;
            this.infoBox.ForeColor = System.Drawing.Color.Red;
            this.infoBox.Location = new System.Drawing.Point(46, 320);
            this.infoBox.Name = "infoBox";
            this.infoBox.Size = new System.Drawing.Size(638, 20);
            this.infoBox.TabIndex = 10;
            // 
            // DataTableListBoxLabel
            // 
            this.DataTableListBoxLabel.AutoSize = true;
            this.DataTableListBoxLabel.Location = new System.Drawing.Point(13, 6);
            this.DataTableListBoxLabel.Name = "DataTableListBoxLabel";
            this.DataTableListBoxLabel.Size = new System.Drawing.Size(167, 13);
            this.DataTableListBoxLabel.TabIndex = 11;
            this.DataTableListBoxLabel.Text = "Names of the datatables columns:";
            // 
            // OntologyListBoxLabel
            // 
            this.OntologyListBoxLabel.AutoSize = true;
            this.OntologyListBoxLabel.Location = new System.Drawing.Point(350, 6);
            this.OntologyListBoxLabel.Name = "OntologyListBoxLabel";
            this.OntologyListBoxLabel.Size = new System.Drawing.Size(199, 13);
            this.OntologyListBoxLabel.TabIndex = 12;
            this.OntologyListBoxLabel.Text = "Classes and instances from the ontology:";
            // 
            // InfoBoxLabel
            // 
            this.InfoBoxLabel.AutoSize = true;
            this.InfoBoxLabel.Location = new System.Drawing.Point(13, 323);
            this.InfoBoxLabel.Name = "InfoBoxLabel";
            this.InfoBoxLabel.Size = new System.Drawing.Size(27, 13);
            this.InfoBoxLabel.TabIndex = 13;
            this.InfoBoxLabel.Text = "info:";
            // 
            // ColumnsRemoveButton
            // 
            this.ColumnsRemoveButton.Location = new System.Drawing.Point(140, 291);
            this.ColumnsRemoveButton.Name = "ColumnsRemoveButton";
            this.ColumnsRemoveButton.Size = new System.Drawing.Size(75, 23);
            this.ColumnsRemoveButton.TabIndex = 14;
            this.ColumnsRemoveButton.Text = "Remove";
            this.ColumnsRemoveButton.UseVisualStyleBackColor = true;
            this.ColumnsRemoveButton.Click += new System.EventHandler(this.ColumnsRemoveButton_Click);
            // 
            // OntologyRemoveButton
            // 
            this.OntologyRemoveButton.Location = new System.Drawing.Point(481, 291);
            this.OntologyRemoveButton.Name = "OntologyRemoveButton";
            this.OntologyRemoveButton.Size = new System.Drawing.Size(75, 23);
            this.OntologyRemoveButton.TabIndex = 15;
            this.OntologyRemoveButton.Text = "Remove";
            this.OntologyRemoveButton.UseVisualStyleBackColor = true;
            this.OntologyRemoveButton.Click += new System.EventHandler(this.OntologyRemoveButton_Click);
            // 
            // ontologyTreeView
            // 
            this.ontologyTreeView.Location = new System.Drawing.Point(354, 25);
            this.ontologyTreeView.Name = "ontologyTreeView";
            this.ontologyTreeView.Size = new System.Drawing.Size(330, 264);
            this.ontologyTreeView.TabIndex = 16;
            this.ontologyTreeView.Enter += new System.EventHandler(this.ontologyTreeView_Enter);
            this.ontologyTreeView.Leave += new System.EventHandler(this.ontologyTreeView_Leave);
            // 
            // dataTablesTreeView
            // 
            this.dataTablesTreeView.Location = new System.Drawing.Point(12, 25);
            this.dataTablesTreeView.Name = "dataTablesTreeView";
            this.dataTablesTreeView.Size = new System.Drawing.Size(330, 264);
            this.dataTablesTreeView.TabIndex = 17;
            this.dataTablesTreeView.Enter += new System.EventHandler(this.dataTablesTreeView_Enter);
            this.dataTablesTreeView.Leave += new System.EventHandler(this.dataTablesTreeView_Leave);
            // 
            // SetOntologyMappingControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(696, 603);
            this.Controls.Add(this.dataTablesTreeView);
            this.Controls.Add(this.ontologyTreeView);
            this.Controls.Add(this.OntologyRemoveButton);
            this.Controls.Add(this.ColumnsRemoveButton);
            this.Controls.Add(this.InfoBoxLabel);
            this.Controls.Add(this.OntologyListBoxLabel);
            this.Controls.Add(this.DataTableListBoxLabel);
            this.Controls.Add(this.infoBox);
            this.Controls.Add(this.helpButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.loadButton);
            this.Controls.Add(this.unmapButton);
            this.Controls.Add(this.mapButton);
            this.Controls.Add(this.MappingListBox);
            this.Controls.Add(this.okButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SetOntologyMappingControl";
            this.Text = "DataTable-Ontology Mapping";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.ListBox MappingListBox;
        private System.Windows.Forms.Button mapButton;
        private System.Windows.Forms.Button unmapButton;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button helpButton;
        private System.Windows.Forms.TextBox infoBox;
        private System.Windows.Forms.Label DataTableListBoxLabel;
        private System.Windows.Forms.Label OntologyListBoxLabel;
        private System.Windows.Forms.Label InfoBoxLabel;
        private System.Windows.Forms.Button ColumnsRemoveButton;
        private System.Windows.Forms.Button OntologyRemoveButton;
        private System.Windows.Forms.TreeView ontologyTreeView;
        private System.Windows.Forms.TreeView dataTablesTreeView;
    }
}