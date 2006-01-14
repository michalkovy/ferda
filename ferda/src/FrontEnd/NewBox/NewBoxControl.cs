using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Ferda.FrontEnd.NewBox
{
    public class NewBoxControl : UserControl
    {
        #region Fields

        private NewBoxTreeView treeView;
        //text box, where the hint is displayed
        private RichTextBox textBox;

        //constant for determining the height of the textbox
        private int heightConstant = 68;

        #endregion

        /// <summary>
        /// Constant for determining the height of the textbox
        /// </summary>
        public int HeightConstant
        {
            get { return heightConstant; }
            set { heightConstant = value; }
        }

        public NewBoxControl(Menu.ILocalizationManager locManager,
            Menu.IMenuDisplayer menuDisp, ModulesManager.ModulesManager modManager,
            IIconProvider iconProvider, Menu.IMenuDisplayer toolBar)
        {
            treeView = new NewBoxTreeView(locManager, menuDisp, modManager, iconProvider, toolBar);
            treeView.WidthConstant = this.heightConstant;
            treeView.BorderStyle = BorderStyle.FixedSingle;
            textBox = new RichTextBox();
            treeView.TextBox = textBox;
            textBox.BorderStyle = BorderStyle.Fixed3D;
            textBox.ReadOnly = true;
            this.Controls.Add(textBox);
            this.Controls.Add(treeView);

            //InitializeComponent things
            this.Name = "FerdaArchive";
            this.Size = new System.Drawing.Size(170, 500);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        public void ChangeSize()
        {
            if (Parent != null)
            {
                this.Size = new System.Drawing.Size(
                    Parent.Size.Width - 5, Parent.Size.Height - 20);
            }
            treeView.ChangeSize();

            //changing the size & location of the textBox
            textBox.Location = new Point(2, this.Height - HeightConstant + 4);
            textBox.Size = new Size(this.Width, HeightConstant);
        }
    }
}
