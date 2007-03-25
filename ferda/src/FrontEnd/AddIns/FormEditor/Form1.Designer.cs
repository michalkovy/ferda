namespace Ferda.FrontEnd.AddIns.FormEditor
{
    partial class WizardFormEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
       /* protected override void Dispose(bool disposing)
        {
 
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }*/

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WizardFormEditor));
            this.ToolStrip = new System.Windows.Forms.ToolStrip();
            this.RadioButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.AreaButton = new System.Windows.Forms.ToolStripButton();
            this.OK = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.main_toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.ToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // ToolStrip
            // 
            this.ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RadioButton,
            this.toolStripSeparator1,
            this.AreaButton});
            this.ToolStrip.Location = new System.Drawing.Point(0, 0);
            this.ToolStrip.Name = "ToolStrip";
            this.ToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.ToolStrip.Size = new System.Drawing.Size(502, 25);
            this.ToolStrip.TabIndex = 0;
            this.ToolStrip.Text = "ToolStrip";
            this.ToolStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ToolStrip_ItemClicked);
            // 
            // RadioButton
            // 
            this.RadioButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RadioButton.Image = ((System.Drawing.Image)(resources.GetObject("RadioButton.Image")));
            this.RadioButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RadioButton.Name = "RadioButton";
            this.RadioButton.Size = new System.Drawing.Size(23, 22);
            this.RadioButton.Text = "Insert radio button";
            this.RadioButton.ToolTipText = "Insert choice";
            this.RadioButton.Click += new System.EventHandler(this.RButton_click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // AreaButton
            // 
            this.AreaButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AreaButton.Image = ((System.Drawing.Image)(resources.GetObject("AreaButton.Image")));
            this.AreaButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AreaButton.Name = "AreaButton";
            this.AreaButton.Size = new System.Drawing.Size(23, 22);
            this.AreaButton.Text = "Insert text area";
            this.AreaButton.Click += new System.EventHandler(this.AButton_Click);
            // 
            // OK
            // 
            this.OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OK.Location = new System.Drawing.Point(322, 338);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 23);
            this.OK.TabIndex = 1;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(403, 338);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 2;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // main_toolTip
            // 
            this.main_toolTip.AutomaticDelay = 1500;
            this.main_toolTip.IsBalloon = true;
            // 
            // WizardFormEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(502, 382);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.ToolStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WizardFormEditor";
            this.Text = "Wizard form editor";
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Mouse_Up);
            this.Validating += new System.ComponentModel.CancelEventHandler(this.Form_Validating);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Mouse_move);
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.WizardFormEditor_HelpRequested);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Mouse_Down);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ToolStrip.ResumeLayout(false);
            this.ToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip ToolStrip;
        private System.Windows.Forms.ToolStripButton RadioButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton AreaButton;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.ToolTip main_toolTip;
    }
}

