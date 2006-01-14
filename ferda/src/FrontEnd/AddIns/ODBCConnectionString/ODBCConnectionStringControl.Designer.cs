namespace Ferda.FrontEnd.AddIns.ODBCConnectionString
{
    partial class ODBCConnectionStringControl
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
            this.ToolBar = new System.Windows.Forms.ToolStrip();
            this.ToolStripButtonOk = new System.Windows.Forms.ToolStripButton();
            this.ToolStripButtonCancel = new System.Windows.Forms.ToolStripButton();
            this.ToolStripButtonTest = new System.Windows.Forms.ToolStripButton();
            this.ToolStripButtonOdbc = new System.Windows.Forms.ToolStripButton();
            this.CurrentDSNName = new System.Windows.Forms.StatusStrip();
            this.StatusBarCurrent = new System.Windows.Forms.ToolStripStatusLabel();
            this.CheckBoxAllowPasswordSave = new System.Windows.Forms.CheckBox();
            this.CheckBoxEmptyPassword = new System.Windows.Forms.CheckBox();
            this.LabelPassword = new System.Windows.Forms.Label();
            this.TextBoxPassword = new System.Windows.Forms.TextBox();
            this.LabelInputUsername = new System.Windows.Forms.Label();
            this.TextBoxUsername = new System.Windows.Forms.TextBox();
            this.LabelInputServerAuth = new System.Windows.Forms.Label();
            this.RadioUseCustomString = new System.Windows.Forms.RadioButton();
            this.RadioUseDSN = new System.Windows.Forms.RadioButton();
            this.LabelInputDataSource = new System.Windows.Forms.Label();
            this.LabelWhatToInput = new System.Windows.Forms.Label();
            this.ComboboxExistingSources = new System.Windows.Forms.ComboBox();
            this.TextBoxCustomConnectionString = new System.Windows.Forms.TextBox();
            this.TextBoxNewConnectionString = new System.Windows.Forms.TextBox();
            this.LabelResultingString = new System.Windows.Forms.Label();
            this.ButtonRefresh = new System.Windows.Forms.Button();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolBar.SuspendLayout();
            this.CurrentDSNName.SuspendLayout();
            this.SuspendLayout();
            // 
            // ToolBar
            // 
            this.ToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripButtonOk,
            this.toolStripSeparator1,
            this.ToolStripButtonCancel,
            this.toolStripSeparator2,
            this.ToolStripButtonTest,
            this.toolStripSeparator3,
            this.ToolStripButtonOdbc});
            this.ToolBar.Location = new System.Drawing.Point(0, 0);
            this.ToolBar.Name = "ToolBar";
            this.ToolBar.Size = new System.Drawing.Size(358, 25);
            this.ToolBar.TabIndex = 0;
            this.ToolBar.TabStop = true;
            this.ToolBar.Text = "toolStrip1";
            // 
            // ToolStripButtonOk
            // 
            this.ToolStripButtonOk.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripButtonOk.Name = "ToolStripButtonOk";
            this.ToolStripButtonOk.Size = new System.Drawing.Size(25, 22);
            this.ToolStripButtonOk.Text = "OK";
            this.ToolStripButtonOk.Click += new System.EventHandler(this.ToolStripButtonOk_Click);
            // 
            // ToolStripButtonCancel
            // 
            this.ToolStripButtonCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripButtonCancel.Name = "ToolStripButtonCancel";
            this.ToolStripButtonCancel.Size = new System.Drawing.Size(43, 22);
            this.ToolStripButtonCancel.Text = "Cancel";
            this.ToolStripButtonCancel.Click += new System.EventHandler(this.ToolStripButtonCancel_Click);
            // 
            // ToolStripButtonTest
            // 
            this.ToolStripButtonTest.Enabled = false;
            this.ToolStripButtonTest.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripButtonTest.Name = "ToolStripButtonTest";
            this.ToolStripButtonTest.Size = new System.Drawing.Size(32, 22);
            this.ToolStripButtonTest.Text = "Test";
            this.ToolStripButtonTest.Click += new System.EventHandler(this.ToolStripButtonTest_Click);
            // 
            // ToolStripButtonOdbc
            // 
            this.ToolStripButtonOdbc.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripButtonOdbc.Name = "ToolStripButtonOdbc";
            this.ToolStripButtonOdbc.Size = new System.Drawing.Size(108, 22);
            this.ToolStripButtonOdbc.Text = "ODBC Connection...";
            this.ToolStripButtonOdbc.Click += new System.EventHandler(this.ToolStripButtonOdbc_Click);
            // 
            // CurrentDSNName
            // 
            this.CurrentDSNName.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusBarCurrent});
            this.CurrentDSNName.Location = new System.Drawing.Point(0, 402);
            this.CurrentDSNName.Name = "CurrentDSNName";
            this.CurrentDSNName.Size = new System.Drawing.Size(358, 22);
            this.CurrentDSNName.SizingGrip = false;
            this.CurrentDSNName.TabIndex = 6;
            this.CurrentDSNName.Text = "statusStrip1";
            // 
            // StatusBarCurrent
            // 
            this.StatusBarCurrent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.StatusBarCurrent.Name = "StatusBarCurrent";
            this.StatusBarCurrent.Size = new System.Drawing.Size(109, 17);
            this.StatusBarCurrent.Text = "toolStripStatusLabel1";
            // 
            // CheckBoxAllowPasswordSave
            // 
            this.CheckBoxAllowPasswordSave.AutoSize = true;
            this.CheckBoxAllowPasswordSave.Location = new System.Drawing.Point(151, 309);
            this.CheckBoxAllowPasswordSave.Name = "CheckBoxAllowPasswordSave";
            this.CheckBoxAllowPasswordSave.Size = new System.Drawing.Size(137, 17);
            this.CheckBoxAllowPasswordSave.TabIndex = 9;
            this.CheckBoxAllowPasswordSave.Text = "Allow to save password";
            this.CheckBoxAllowPasswordSave.UseVisualStyleBackColor = true;
            this.CheckBoxAllowPasswordSave.CheckedChanged += new System.EventHandler(this.CheckBoxAllowPasswordSave_CheckedChanged);
            // 
            // CheckBoxEmptyPassword
            // 
            this.CheckBoxEmptyPassword.AutoSize = true;
            this.CheckBoxEmptyPassword.Location = new System.Drawing.Point(17, 309);
            this.CheckBoxEmptyPassword.Name = "CheckBoxEmptyPassword";
            this.CheckBoxEmptyPassword.Size = new System.Drawing.Size(103, 17);
            this.CheckBoxEmptyPassword.TabIndex = 8;
            this.CheckBoxEmptyPassword.Text = "Empty password";
            this.CheckBoxEmptyPassword.UseVisualStyleBackColor = true;
            this.CheckBoxEmptyPassword.CheckedChanged += new System.EventHandler(this.CheckBoxEmptyPassword_CheckedChanged);
            // 
            // LabelPassword
            // 
            this.LabelPassword.AutoSize = true;
            this.LabelPassword.Location = new System.Drawing.Point(14, 274);
            this.LabelPassword.Name = "LabelPassword";
            this.LabelPassword.Size = new System.Drawing.Size(53, 13);
            this.LabelPassword.TabIndex = 26;
            this.LabelPassword.Text = "Password";
            // 
            // TextBoxPassword
            // 
            this.TextBoxPassword.Location = new System.Drawing.Point(151, 271);
            this.TextBoxPassword.Name = "TextBoxPassword";
            this.TextBoxPassword.PasswordChar = '*';
            this.TextBoxPassword.Size = new System.Drawing.Size(195, 20);
            this.TextBoxPassword.TabIndex = 7;
            // 
            // LabelInputUsername
            // 
            this.LabelInputUsername.AutoSize = true;
            this.LabelInputUsername.Location = new System.Drawing.Point(14, 248);
            this.LabelInputUsername.Name = "LabelInputUsername";
            this.LabelInputUsername.Size = new System.Drawing.Size(55, 13);
            this.LabelInputUsername.TabIndex = 24;
            this.LabelInputUsername.Text = "Username";
            // 
            // TextBoxUsername
            // 
            this.TextBoxUsername.Location = new System.Drawing.Point(151, 241);
            this.TextBoxUsername.Name = "TextBoxUsername";
            this.TextBoxUsername.Size = new System.Drawing.Size(195, 20);
            this.TextBoxUsername.TabIndex = 6;
            // 
            // LabelInputServerAuth
            // 
            this.LabelInputServerAuth.AutoSize = true;
            this.LabelInputServerAuth.Location = new System.Drawing.Point(6, 214);
            this.LabelInputServerAuth.Name = "LabelInputServerAuth";
            this.LabelInputServerAuth.Size = new System.Drawing.Size(192, 13);
            this.LabelInputServerAuth.TabIndex = 22;
            this.LabelInputServerAuth.Text = "Input server authentification information";
            // 
            // RadioUseCustomString
            // 
            this.RadioUseCustomString.AutoSize = true;
            this.RadioUseCustomString.Location = new System.Drawing.Point(58, 150);
            this.RadioUseCustomString.Name = "RadioUseCustomString";
            this.RadioUseCustomString.Size = new System.Drawing.Size(165, 17);
            this.RadioUseCustomString.TabIndex = 4;
            this.RadioUseCustomString.TabStop = true;
            this.RadioUseCustomString.Text = "Use custom connection string";
            this.RadioUseCustomString.UseVisualStyleBackColor = true;
            this.RadioUseCustomString.CheckedChanged += new System.EventHandler(this.RadioUseCustomString_CheckedChanged);
            // 
            // RadioUseDSN
            // 
            this.RadioUseDSN.AutoSize = true;
            this.RadioUseDSN.Checked = true;
            this.RadioUseDSN.Location = new System.Drawing.Point(60, 87);
            this.RadioUseDSN.Name = "RadioUseDSN";
            this.RadioUseDSN.Size = new System.Drawing.Size(132, 17);
            this.RadioUseDSN.TabIndex = 1;
            this.RadioUseDSN.TabStop = true;
            this.RadioUseDSN.Text = "Use data source name";
            this.RadioUseDSN.UseVisualStyleBackColor = true;
            this.RadioUseDSN.CheckedChanged += new System.EventHandler(this.RadioUseDSN_CheckedChanged);
            // 
            // LabelInputDataSource
            // 
            this.LabelInputDataSource.AutoSize = true;
            this.LabelInputDataSource.Location = new System.Drawing.Point(17, 62);
            this.LabelInputDataSource.Name = "LabelInputDataSource";
            this.LabelInputDataSource.Size = new System.Drawing.Size(90, 13);
            this.LabelInputDataSource.TabIndex = 19;
            this.LabelInputDataSource.Text = "Input data source";
            // 
            // LabelWhatToInput
            // 
            this.LabelWhatToInput.AutoSize = true;
            this.LabelWhatToInput.Location = new System.Drawing.Point(6, 38);
            this.LabelWhatToInput.Name = "LabelWhatToInput";
            this.LabelWhatToInput.Size = new System.Drawing.Size(117, 13);
            this.LabelWhatToInput.TabIndex = 18;
            this.LabelWhatToInput.Text = "Input the following data";
            // 
            // ComboboxExistingSources
            // 
            this.ComboboxExistingSources.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboboxExistingSources.FormattingEnabled = true;
            this.ComboboxExistingSources.Location = new System.Drawing.Point(60, 110);
            this.ComboboxExistingSources.Name = "ComboboxExistingSources";
            this.ComboboxExistingSources.Size = new System.Drawing.Size(163, 21);
            this.ComboboxExistingSources.TabIndex = 2;
            // 
            // TextBoxCustomConnectionString
            // 
            this.TextBoxCustomConnectionString.Enabled = false;
            this.TextBoxCustomConnectionString.Location = new System.Drawing.Point(60, 173);
            this.TextBoxCustomConnectionString.Name = "TextBoxCustomConnectionString";
            this.TextBoxCustomConnectionString.Size = new System.Drawing.Size(165, 20);
            this.TextBoxCustomConnectionString.TabIndex = 5;
            // 
            // TextBoxNewConnectionString
            // 
            this.TextBoxNewConnectionString.Enabled = false;
            this.TextBoxNewConnectionString.Location = new System.Drawing.Point(17, 367);
            this.TextBoxNewConnectionString.Name = "TextBoxNewConnectionString";
            this.TextBoxNewConnectionString.Size = new System.Drawing.Size(329, 20);
            this.TextBoxNewConnectionString.TabIndex = 10;
            this.TextBoxNewConnectionString.TabStop = false;
            // 
            // LabelResultingString
            // 
            this.LabelResultingString.AutoSize = true;
            this.LabelResultingString.Location = new System.Drawing.Point(6, 342);
            this.LabelResultingString.Name = "LabelResultingString";
            this.LabelResultingString.Size = new System.Drawing.Size(79, 13);
            this.LabelResultingString.TabIndex = 30;
            this.LabelResultingString.Text = "Resulting string";
            // 
            // ButtonRefresh
            // 
            this.ButtonRefresh.Location = new System.Drawing.Point(245, 108);
            this.ButtonRefresh.Name = "ButtonRefresh";
            this.ButtonRefresh.Size = new System.Drawing.Size(75, 23);
            this.ButtonRefresh.TabIndex = 3;
            this.ButtonRefresh.Text = "Refresh";
            this.ButtonRefresh.UseVisualStyleBackColor = true;
            this.ButtonRefresh.Click += new System.EventHandler(this.ButtonRefresh_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // ODBCConnectionStringControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(358, 424);
            this.Controls.Add(this.ButtonRefresh);
            this.Controls.Add(this.LabelResultingString);
            this.Controls.Add(this.TextBoxNewConnectionString);
            this.Controls.Add(this.CheckBoxAllowPasswordSave);
            this.Controls.Add(this.CheckBoxEmptyPassword);
            this.Controls.Add(this.LabelPassword);
            this.Controls.Add(this.TextBoxPassword);
            this.Controls.Add(this.LabelInputUsername);
            this.Controls.Add(this.TextBoxUsername);
            this.Controls.Add(this.LabelInputServerAuth);
            this.Controls.Add(this.RadioUseCustomString);
            this.Controls.Add(this.RadioUseDSN);
            this.Controls.Add(this.LabelInputDataSource);
            this.Controls.Add(this.LabelWhatToInput);
            this.Controls.Add(this.ComboboxExistingSources);
            this.Controls.Add(this.TextBoxCustomConnectionString);
            this.Controls.Add(this.CurrentDSNName);
            this.Controls.Add(this.ToolBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ODBCConnectionStringControl";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ToolBar.ResumeLayout(false);
            this.ToolBar.PerformLayout();
            this.CurrentDSNName.ResumeLayout(false);
            this.CurrentDSNName.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip ToolBar;
        private System.Windows.Forms.ToolStripButton ToolStripButtonOk;
        private System.Windows.Forms.ToolStripButton ToolStripButtonCancel;
        private System.Windows.Forms.ToolStripButton ToolStripButtonTest;
        private System.Windows.Forms.StatusStrip CurrentDSNName;
        private System.Windows.Forms.ToolStripStatusLabel StatusBarCurrent;
        private System.Windows.Forms.CheckBox CheckBoxAllowPasswordSave;
        private System.Windows.Forms.CheckBox CheckBoxEmptyPassword;
        private System.Windows.Forms.Label LabelPassword;
        private System.Windows.Forms.TextBox TextBoxPassword;
        private System.Windows.Forms.Label LabelInputUsername;
        private System.Windows.Forms.TextBox TextBoxUsername;
        private System.Windows.Forms.Label LabelInputServerAuth;
        private System.Windows.Forms.RadioButton RadioUseCustomString;
        private System.Windows.Forms.RadioButton RadioUseDSN;
        private System.Windows.Forms.Label LabelInputDataSource;
        private System.Windows.Forms.Label LabelWhatToInput;
        private System.Windows.Forms.ComboBox ComboboxExistingSources;
        private System.Windows.Forms.TextBox TextBoxCustomConnectionString;
        private System.Windows.Forms.TextBox TextBoxNewConnectionString;
        private System.Windows.Forms.Label LabelResultingString;
        private System.Windows.Forms.ToolStripButton ToolStripButtonOdbc;
        private System.Windows.Forms.Button ButtonRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;

    }
}
