using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ferda.FrontEnd.AddIns.FormGenerator
{
    public partial class UserControl : Form
    {
        public event EventHandler Next;

        public event EventHandler Stop;



        public UserControl(string whatToChange, string type)
        {
            InitializeComponent();

            int position = whatToChange.LastIndexOf('-');
            whatToChange = whatToChange.Remove(position, 2);
            whatToChange = whatToChange.Insert(position, "  set to value : ");


            int plannedWidth = 40 + whatToChange.Length * this.Font.Height / 2;

            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Width = Math.Max(300, plannedWidth);
            this.Height = 130;
            this.MinimizeBox = true;
            this.MaximizeBox = false;
            this.AutoSize = true;
            this.Text = "Control";
            

            Label main_label = new Label();

            main_label.Top = 20;
            main_label.Left = 10;
            main_label.AutoSize = true;
            main_label.ForeColor = Color.Red;
            main_label.Text = " Now, change this property according to recommended value:";
            main_label.TextAlign = ContentAlignment.BottomLeft;
            main_label.Visible = true;
            this.Controls.Add(main_label);
            main_label.Show();

            Label property_label = new Label();

            property_label.Top = 50;
            property_label.Left = 20;
            property_label.AutoSize = true;
            property_label.ForeColor = Color.Black;
            property_label.Text = whatToChange;
            property_label.TextAlign = ContentAlignment.BottomLeft;
            property_label.Visible = true;
            this.Controls.Add(property_label);
            property_label.Show();

            if (type == "OK")
            {
                Button next_button = new Button();

                next_button.Left = this.Size.Width - 85;
                next_button.Top = this.Size.Height - 50;
                next_button.Text = "OK";
                next_button.BackColor = Color.Green;
                next_button.Anchor = AnchorStyles.Right;
                next_button.Click += new System.EventHandler(this.STOP_Click);
                this.Controls.Add(next_button);
                next_button.Show();
            }
            else
            {
                Button next_button = new Button();

                next_button.Left = this.Size.Width - 165;
                next_button.Top = this.Size.Height - 50;
                next_button.Text = "Next >>";
                next_button.BackColor = Color.Green;
                next_button.Anchor = AnchorStyles.Right;
                next_button.Click += new System.EventHandler(this.Next_Click);
                this.Controls.Add(next_button);
                next_button.Show();

                Button STOP_button = new Button();

                STOP_button.Left = this.Size.Width - 85;
                STOP_button.Top = this.Size.Height - 50;
                STOP_button.Text = "STOP";
                STOP_button.BackColor = Color.Red;
                STOP_button.Anchor = AnchorStyles.Right;
                STOP_button.Click += new System.EventHandler(this.STOP_Click);
                this.Controls.Add(STOP_button);
                STOP_button.Show();
            }
        }


        /// <summary>
        /// Function jump to the next form in scenario. 
        /// </summary>
        private void Next_Click(object sender, EventArgs e)
        {
            this.Hide();

            if (Next != null) Next(this, EventArgs.Empty);

            //this.Dispose();
        }

        /// <summary>
        /// Stops the current scenarion.
        /// </summary>
        private void STOP_Click(object sender, EventArgs e)
        {
            this.Hide();

            if (Stop != null) Stop(this, EventArgs.Empty);

           // this.Dispose();
        }

    }
}