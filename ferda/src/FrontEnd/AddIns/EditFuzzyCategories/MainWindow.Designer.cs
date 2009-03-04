namespace Ferda.FrontEnd.AddIns.EditFuzzyCategories
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.CancelButton = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.GraphPanel = new System.Windows.Forms.Panel();
            this.NewFCLabel = new System.Windows.Forms.Label();
            this.NewFCNameLabel = new System.Windows.Forms.Label();
            this.NameTB = new System.Windows.Forms.TextBox();
            this.ValueALabel = new System.Windows.Forms.Label();
            this.ATB = new System.Windows.Forms.TextBox();
            this.ValueDLabel = new System.Windows.Forms.Label();
            this.DTB = new System.Windows.Forms.TextBox();
            this.CTB = new System.Windows.Forms.TextBox();
            this.BTB = new System.Windows.Forms.TextBox();
            this.ValueCLabel = new System.Windows.Forms.Label();
            this.ValueBLabel = new System.Windows.Forms.Label();
            this.TrapezoidPB = new System.Windows.Forms.PictureBox();
            this.AddFCButton = new System.Windows.Forms.Button();
            this.ExistingFCLabel = new System.Windows.Forms.Label();
            this.ExistingFCLB = new System.Windows.Forms.ListBox();
            this.RemoveFCButton = new System.Windows.Forms.Button();
            this.EditFCButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.TrapezoidPB)).BeginInit();
            this.SuspendLayout();
            // 
            // CancelButton
            // 
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.CancelButton.Location = new System.Drawing.Point(730, 331);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(104, 62);
            this.CancelButton.TabIndex = 0;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            // 
            // OKButton
            // 
            this.OKButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.OKButton.Location = new System.Drawing.Point(610, 331);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(114, 63);
            this.OKButton.TabIndex = 1;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            // 
            // GraphPanel
            // 
            this.GraphPanel.Location = new System.Drawing.Point(12, 12);
            this.GraphPanel.Name = "GraphPanel";
            this.GraphPanel.Size = new System.Drawing.Size(822, 236);
            this.GraphPanel.TabIndex = 2;
            // 
            // NewFCLabel
            // 
            this.NewFCLabel.AutoSize = true;
            this.NewFCLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.NewFCLabel.Location = new System.Drawing.Point(12, 251);
            this.NewFCLabel.Name = "NewFCLabel";
            this.NewFCLabel.Size = new System.Drawing.Size(141, 16);
            this.NewFCLabel.TabIndex = 3;
            this.NewFCLabel.Text = "New fuzzy category";
            // 
            // NewFCNameLabel
            // 
            this.NewFCNameLabel.AutoSize = true;
            this.NewFCNameLabel.Location = new System.Drawing.Point(12, 276);
            this.NewFCNameLabel.Name = "NewFCNameLabel";
            this.NewFCNameLabel.Size = new System.Drawing.Size(38, 13);
            this.NewFCNameLabel.TabIndex = 4;
            this.NewFCNameLabel.Text = "Name:";
            // 
            // NameTB
            // 
            this.NameTB.Location = new System.Drawing.Point(142, 273);
            this.NameTB.Name = "NameTB";
            this.NameTB.Size = new System.Drawing.Size(100, 20);
            this.NameTB.TabIndex = 5;
            // 
            // ValueALabel
            // 
            this.ValueALabel.AutoSize = true;
            this.ValueALabel.Location = new System.Drawing.Point(12, 302);
            this.ValueALabel.Name = "ValueALabel";
            this.ValueALabel.Size = new System.Drawing.Size(122, 13);
            this.ValueALabel.TabIndex = 6;
            this.ValueALabel.Text = "A (start of the trapezoid):";
            // 
            // ATB
            // 
            this.ATB.Location = new System.Drawing.Point(142, 299);
            this.ATB.Name = "ATB";
            this.ATB.Size = new System.Drawing.Size(100, 20);
            this.ATB.TabIndex = 7;
            // 
            // ValueDLabel
            // 
            this.ValueDLabel.AutoSize = true;
            this.ValueDLabel.Location = new System.Drawing.Point(12, 328);
            this.ValueDLabel.Name = "ValueDLabel";
            this.ValueDLabel.Size = new System.Drawing.Size(103, 13);
            this.ValueDLabel.TabIndex = 8;
            this.ValueDLabel.Text = "D (ascending peak):";
            // 
            // DTB
            // 
            this.DTB.Location = new System.Drawing.Point(142, 325);
            this.DTB.Name = "DTB";
            this.DTB.Size = new System.Drawing.Size(100, 20);
            this.DTB.TabIndex = 9;
            // 
            // CTB
            // 
            this.CTB.Location = new System.Drawing.Point(142, 351);
            this.CTB.Name = "CTB";
            this.CTB.Size = new System.Drawing.Size(100, 20);
            this.CTB.TabIndex = 10;
            // 
            // BTB
            // 
            this.BTB.Location = new System.Drawing.Point(142, 377);
            this.BTB.Name = "BTB";
            this.BTB.Size = new System.Drawing.Size(100, 20);
            this.BTB.TabIndex = 11;
            // 
            // ValueCLabel
            // 
            this.ValueCLabel.AutoSize = true;
            this.ValueCLabel.Location = new System.Drawing.Point(12, 354);
            this.ValueCLabel.Name = "ValueCLabel";
            this.ValueCLabel.Size = new System.Drawing.Size(108, 13);
            this.ValueCLabel.TabIndex = 12;
            this.ValueCLabel.Text = "C (descending peak):";
            // 
            // ValueBLabel
            // 
            this.ValueBLabel.AutoSize = true;
            this.ValueBLabel.Location = new System.Drawing.Point(12, 380);
            this.ValueBLabel.Name = "ValueBLabel";
            this.ValueBLabel.Size = new System.Drawing.Size(120, 13);
            this.ValueBLabel.TabIndex = 13;
            this.ValueBLabel.Text = "B (end of the trapezoid):";
            // 
            // TrapezoidPB
            // 
            this.TrapezoidPB.Image = ((System.Drawing.Image)(resources.GetObject("TrapezoidPB.Image")));
            this.TrapezoidPB.Location = new System.Drawing.Point(248, 273);
            this.TrapezoidPB.Name = "TrapezoidPB";
            this.TrapezoidPB.Size = new System.Drawing.Size(138, 75);
            this.TrapezoidPB.TabIndex = 14;
            this.TrapezoidPB.TabStop = false;
            // 
            // AddFCButton
            // 
            this.AddFCButton.Location = new System.Drawing.Point(248, 354);
            this.AddFCButton.Name = "AddFCButton";
            this.AddFCButton.Size = new System.Drawing.Size(138, 43);
            this.AddFCButton.TabIndex = 15;
            this.AddFCButton.Text = "Add fuzzy category";
            this.AddFCButton.UseVisualStyleBackColor = true;
            // 
            // ExistingFCLabel
            // 
            this.ExistingFCLabel.AutoSize = true;
            this.ExistingFCLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ExistingFCLabel.Location = new System.Drawing.Point(389, 255);
            this.ExistingFCLabel.Name = "ExistingFCLabel";
            this.ExistingFCLabel.Size = new System.Drawing.Size(178, 16);
            this.ExistingFCLabel.TabIndex = 16;
            this.ExistingFCLabel.Text = "Existing fuzzy categories";
            // 
            // ExistingFCLB
            // 
            this.ExistingFCLB.FormattingEnabled = true;
            this.ExistingFCLB.Location = new System.Drawing.Point(392, 273);
            this.ExistingFCLB.Name = "ExistingFCLB";
            this.ExistingFCLB.Size = new System.Drawing.Size(212, 121);
            this.ExistingFCLB.TabIndex = 17;
            // 
            // RemoveFCButton
            // 
            this.RemoveFCButton.Location = new System.Drawing.Point(610, 302);
            this.RemoveFCButton.Name = "RemoveFCButton";
            this.RemoveFCButton.Size = new System.Drawing.Size(224, 23);
            this.RemoveFCButton.TabIndex = 18;
            this.RemoveFCButton.Text = "Remove fuzzy category";
            this.RemoveFCButton.UseVisualStyleBackColor = true;
            // 
            // EditFCButton
            // 
            this.EditFCButton.Location = new System.Drawing.Point(610, 273);
            this.EditFCButton.Name = "EditFCButton";
            this.EditFCButton.Size = new System.Drawing.Size(224, 23);
            this.EditFCButton.TabIndex = 19;
            this.EditFCButton.Text = "Edit fuzzy category";
            this.EditFCButton.UseVisualStyleBackColor = true;
            // 
            // MainWindow
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(846, 402);
            this.Controls.Add(this.EditFCButton);
            this.Controls.Add(this.RemoveFCButton);
            this.Controls.Add(this.ExistingFCLB);
            this.Controls.Add(this.ExistingFCLabel);
            this.Controls.Add(this.AddFCButton);
            this.Controls.Add(this.TrapezoidPB);
            this.Controls.Add(this.ValueBLabel);
            this.Controls.Add(this.ValueCLabel);
            this.Controls.Add(this.BTB);
            this.Controls.Add(this.CTB);
            this.Controls.Add(this.DTB);
            this.Controls.Add(this.ValueDLabel);
            this.Controls.Add(this.ATB);
            this.Controls.Add(this.ValueALabel);
            this.Controls.Add(this.NameTB);
            this.Controls.Add(this.NewFCNameLabel);
            this.Controls.Add(this.NewFCLabel);
            this.Controls.Add(this.GraphPanel);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.CancelButton);
            this.Name = "MainWindow";
            this.Text = "MainWindow";
            ((System.ComponentModel.ISupportInitialize)(this.TrapezoidPB)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Panel GraphPanel;
        private System.Windows.Forms.Label NewFCLabel;
        private System.Windows.Forms.Label NewFCNameLabel;
        private System.Windows.Forms.TextBox NameTB;
        private System.Windows.Forms.Label ValueALabel;
        private System.Windows.Forms.TextBox ATB;
        private System.Windows.Forms.Label ValueDLabel;
        private System.Windows.Forms.TextBox DTB;
        private System.Windows.Forms.TextBox CTB;
        private System.Windows.Forms.TextBox BTB;
        private System.Windows.Forms.Label ValueCLabel;
        private System.Windows.Forms.Label ValueBLabel;
        private System.Windows.Forms.PictureBox TrapezoidPB;
        private System.Windows.Forms.Button AddFCButton;
        private System.Windows.Forms.Label ExistingFCLabel;
        private System.Windows.Forms.ListBox ExistingFCLB;
        private System.Windows.Forms.Button RemoveFCButton;
        private System.Windows.Forms.Button EditFCButton;
    }
}