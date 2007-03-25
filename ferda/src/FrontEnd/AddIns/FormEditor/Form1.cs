//From editor for editing Wizard forms. 
//
// Author: Daniel Kupka<kupkd9am@post.cz>
//
// Copyright (c) 2007 Daniel Kupka
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA


using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Resources;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Serialization;
using System.Windows.Forms;
//using Generator;


namespace Ferda.FrontEnd.AddIns.FormEditor
{

    public partial class WizardFormEditor : Form
    {
        /// <summary>
        /// State of toolStrip buttons
        /// </summary>
        public enum ButtonState : byte
        {
            unpressed = 0,
            pressed = 1
        }
        /// <summary>
        /// which button in toolStrip was pressed
        /// </summary>
        private ButtonState RButtonState, AButtonState;

        /// <summary>
        /// Global numericUpDown.
        /// </summary>        
        NumericUpDown numeric;

        /// <summary>
        ///array of radiobuttons
        /// </summary>
        //public RadioButton[] buttons = new RadioButton[20];
        private List<RadioButton> buttons;

        /// <summary>
        ///array of successor buttons
        /// </summary>
        private List<Button> successor;


        /// <summary>
        ///array of all richtexboxes in the form
        /// </summary>
        private List<RichTextBox> boxes;

        /// <summary>
        ///modal dialog for setting variable, example or path
        /// </summary>
        private Form new_form;

        private RichTextBox dialog_box = new RichTextBox();

        /// <summary>
        ///list of content of all RichTextBoxes.
        /// </summary>
        private List<string> areas_text;

        /// <summary>
        /// index of last active RichTextBox
        /// </summary>
        private int akt_index;

        /// <summary>
        /// Keep track of when fake "drag and drop" mode is enabled.
        /// </summary>
        private bool isDragging = false;

        /// <summary>
        /// Resulting XML string
        /// </summary>
        private string returnString;

        //System.Collections.ArrayList buttons;
        private String selectedtext = "";

        /// <summary>
        /// Hint for main area(main RichTextBox)
        /// </summary>
        private String area_help = @"Here insert text, that displays to user. 
        -  When you want to insert variable, write $variable_name. Variable name is 
           displayed with red color. Variable id substituted in generated form. 
        -  When you want to insert example, write @example_name. Example name is 
           displayed with green color. 
        -  When you want to insert identifier path, write ^path_name. Path name is 
           displayed with green color. 

           If you wat to fill the body of variable, example or path, select 
           '$variable_name', '@example_name' or '^path_name'
           Variables, examples and paths names must be without spaces. 
           ";

        /// <summary>
        /// Hint for edits(choices)
        /// </summary>
        private String edit_help = @"Here insert text version of one choice.
        Priority is from top to down.
        You can use variables, examples and paths as in the main area.
        ";

        /// <summary>
        /// Hint for RichTextBox in variable definition window.
        /// </summary>
        private String variableUsing_help = @"Here insert source code of wizard language such as :
          $a = 1;
          $b = $a * 4;
          $c = $array[$b] - $a;   
          $d = if ($a > 4) {$b} else {$c};
          $f = if (($a - 2 >= 6) && ($b < 8)) {$b+1} else {$a * 4};
        ";

        /// <summary>
        /// Hint for example RichTextBox
        /// </summary>
        private String exampleUsing_help = @"Here insert text version of practical example
        ";

        /// <summary>
        /// Hint for path RichTextBox
        /// </summary>
        private String pathUsing_help = @"Here path of identifiers saparated by '->' such as :
         founded_implication -> p
        ";

        public System.Collections.Hashtable variables;

        public System.Collections.Hashtable successors;

        public System.Collections.Hashtable numerics;

        /// <summary>
        /// Resulting XML string
        /// </summary>
        public string ReturnString
        {
            get
            {
                return returnString;
            }
        }


        /// <summary>
        /// Class constructor
        /// </summary>
        public WizardFormEditor(string ContentToLoad)
        {
            InitializeComponent();

           // number_buttons = 0;
           // number_areas = 0;

            areas_text = new List<string>();
            boxes = new List<RichTextBox>();
            buttons = new List<RadioButton>();
            successor = new List<Button>();

            variables = new System.Collections.Hashtable();
            successors = new System.Collections.Hashtable();
            numerics = new System.Collections.Hashtable();

            RButtonState = ButtonState.unpressed;
            AButtonState = ButtonState.unpressed;

            this.returnString = ContentToLoad;

            Initialize();

            if (ContentToLoad != "")
              {
                  LoadFormFromXMLString(ContentToLoad);

                  for (int i = 0; i < 5; i++)
                      FormHihglighter(boxes[i]);
              }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Function initialize common controls
        /// </summary>
        private void Initialize()
        {
          int i;

            RichTextBox text_box = new RichTextBox();
            text_box.Left = 50;
            text_box.Top = 50;
            text_box.Width = 350;
            text_box.Height = 130;
            text_box.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            //text_box.ScrollBars = RichTextBoxScrollBars.ForcedBoth;
            this.main_toolTip.SetToolTip(text_box, area_help);

            text_box.SelectionChanged += new System.EventHandler(this.SelectionChanged);
            text_box.TextChanged += new System.EventHandler(this.TextChanged);
            this.Controls.Add(text_box);

            boxes.Insert(0, text_box);
            

            for (i = 0; i < 4; i++)
            {
                RadioButton radio_button = new RadioButton();
                radio_button.Left = 50;
                radio_button.Top = 200 + i * 30;
                radio_button.Text = "";
                radio_button.Appearance = Appearance.Normal;
                radio_button.Width = 15;
                radio_button.Height = 15;
                radio_button.AutoSize = false;
                if (i == 0) radio_button.Checked = true;
                this.Controls.Add(radio_button);
                buttons.Add(radio_button);

                RichTextBox edit = new RichTextBox();
                edit.Left = 80;
                edit.Top = 200 + i * 30;
                edit.Width = 320;
                edit.Height = 20;
                edit.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                this.main_toolTip.SetToolTip(edit, edit_help);

                edit.SelectionChanged += new System.EventHandler(this.SelectionChanged);
                edit.TextChanged += new System.EventHandler(this.TextChanged);
                this.Controls.Add(edit);
                boxes.Insert(i + 1, edit);

                Button successor_button = new Button();
                successor_button.Left = this.Size.Width - 100;
                successor_button.Top = 200 + i * 30;
                successor_button.Anchor = AnchorStyles.Right | AnchorStyles.Top;
                successor_button.Text = "Successor";

                successor_button.Click += new System.EventHandler(this.Successor_Click);
                this.Controls.Add(successor_button);
                successor.Insert (i, successor_button);
            }
        }

        /// <summary>
        /// When user click on Radio Button, neccessary to change state. 
        /// </summary>
        private void RButton_click(object sender, EventArgs e)
        {
            if (RButtonState == ButtonState.unpressed)
            {
                RButtonState = ButtonState.pressed;
                
            }
            AButtonState = ButtonState.unpressed;
        }

        /// <summary>
        /// When user click on RichTextBox Button, necessary to change state.
        /// </summary>
        private void AButton_Click(object sender, EventArgs e)
        {

            if (AButtonState == ButtonState.unpressed)
            {
                AButtonState = ButtonState.pressed;
                this.Cursor = Cursors.Cross;
            }
            else if (AButtonState == ButtonState.pressed)
                AButtonState = ButtonState.unpressed;

           RButtonState = ButtonState.unpressed;
        }

        /// <summary>
        /// When user click on Successor button.
        /// </summary>
        private void Successor_Click(object sender, EventArgs e)
        {
           int i;
           bool is_new = true;
           
            for (i = 0; i < successor.Count; i++)
               if (successor[i].Equals(sender)) break;

           if (successors.Contains(i)) is_new = false;
           else
           {
               successors[i] = "";
               numerics[i] = 0;
           }

            CreateForm("Successor", i.ToString(), is_new, true);
        }

        /// <summary>
        /// When user click on OK button of modal dialog.
        /// </summary>
        private void OK1_Click(object sender, EventArgs e)
        {
            if (areas_text.Count > akt_index)
                        areas_text.RemoveAt(akt_index);

            areas_text.Insert(akt_index, dialog_box.Text);
            new_form.Close();
        }

        /// <summary>
        /// When user click on CAncel button of modal dialog.
        /// </summary>
        private void Cancel1_Click(object sender, EventArgs e)
        {
            new_form.Close();
        }

        /// <summary>
        /// When user click on OK button of modal successor dialog.
        /// </summary>
        private void OK2_Click(object sender, EventArgs e)
        {
          //  if (successors.Count > akt_index)
            //    successors.RemoveAt(akt_index);

            successors[akt_index] = dialog_box.Text;
            numerics[akt_index] = numeric.Value;
            //successors.Insert(akt_index, dialog_box.Text);
            new_form.Close();
        }

        /// <summary>
        /// When user click on CAncel button of modal successor dialog.
        /// </summary>
        private void Cancel2_Click(object sender, EventArgs e)
        {
            new_form.Close();
        }

        /// <summary>
        /// Function find specific text(condition types) in input string(used by highliter)
        /// </summary>
        /// <param name="text">Input text.</param>
        /// <param name="start">Start finding position.</param>
        /// <param name="begin">Begin of founded text.</param>
        /// <param name="length">NUmber chars of founded text</param>
        private void Find_condition(String text, int start, ref int begin, ref int length)
        {
            String if_word = "if";
            String elseif_word = "elseif";
            String else_word = "else";
            String ends = "([{;. &|!<=>\n";
            int else_position, if_position;

            else_position = text.IndexOf(else_word, start);
            if_position = text.IndexOf(if_word, start);

            if (Math.Max(else_position, if_position) == -1) 
              { begin = -1; return; }

             if ( ((else_position < if_position) && (else_position >= 0) ||
                    (else_position > if_position) && (if_position < 0)) )
              { 
                 begin = else_position; 
                 if (text.IndexOf(elseif_word, start) >= 0) length = 6;
                 else length = 4;
              }
              else if ( ((else_position > if_position) && (if_position >= 0)) ||
                        ((else_position < if_position) && (else_position < 0)))
              {  begin = if_position; length = 2;   }

              if ((begin > 0) && (begin + length < text.Length))
              {
                  if ((ends.IndexOf(text[begin - 1]) < 0) ||
                      (ends.IndexOf(text[begin + length]) < 0))
                      length = 0;
              }
        }
        /// <summary>
        /// Function find specific text(variables, examples or paths) in input string(used by highliter)
        /// </summary>
        /// <param name="text">Input text.</param>
        /// <param name="start">Start finding position.</param>
        /// <param name="begin">Begin of founded text.</param>
        /// <param name="length">NUmber chars of founded text</param>
        private void Find_variable(String text, ref int chr, int start, ref int begin, ref int length)
        {
            char[] chars = new char[3] {'$', '@', '^'};
            String ends = "([{;. &|!<=>\n";
            string found_char;

            length = 1;
            if (start < 0) begin = text.LastIndexOfAny(chars);
            else begin = text.IndexOfAny(chars, start);
      
            if (begin < 0) return;

            found_char = text.Substring(begin, 1);
            chr = found_char[0];

            while ((begin + length < text.Length) &&
                    (ends.IndexOf(text[begin + length]) < 0))
                length++;
        }

        /// <summary>
        /// Function highlight variables, examples and paths in incoming RichTextBox
        /// </summary>
        /// <param name="box">Highlighted RichTextBox.</param>
        private void FormHihglighter(RichTextBox box)
        {
            int begin = 0, start = 0, index = 0;
            int cursor_position, length = 0;
            int chr = 0;
            String text;

            cursor_position = box.SelectionStart;
            text = box.Text;
            box.ResetText();
            box.Text = text;
            box.Select(cursor_position, 0);

            while (true)
            {
                Find_variable(box.Text, ref chr, start, ref begin, ref length);
                if (begin >= 0)
                {
                    start = begin + 1;

                    for (index = 0; index < length; index++)
                    {
                        box.Select(begin + index, 1);

                        if (chr == '$')
                            box.SelectionColor = Color.Red;
                        else if ((chr == '@') || (chr == '^'))
                            box.SelectionColor = Color.Green;

                        box.Select(cursor_position, 0);
                        box.SelectionColor = Color.Black;
                    }
                }
                else break;
            }
        }

        /// <summary>
        /// Function highlight expressions in incoming RichTextBox.
        /// </summary>
        /// <param name="box">Highlighted RichTextBox.</param>
        private void ExpressionHighlighter(RichTextBox box)
        {
          int length = 0, begin = 0;
          int chr = 0, start = 0;
          int cursor_position;
          String text;

            cursor_position = box.SelectionStart;
            text = box.Text;
            box.ResetText();
            box.Text = text;
            box.Select(cursor_position, 0);

            while (true)
            {
                Find_variable(box.Text, ref chr, start, ref begin, ref length);
                if (begin >= 0)
                {
                    start = begin + 1;
                    box.Select(begin, length);
                    box.SelectionColor = Color.Red;
                }
                else break;
            }

            start = 0;

            while (true)
            {
                Find_condition(box.Text, start, ref begin, ref length);

                if (begin >= 0)
                {
                    start = begin + 1;
                    box.Select(begin, length);
                    box.SelectionColor = Color.Blue;
                }
                else break;
            }

            box.Select(cursor_position, 0);
            box.SelectionColor = Color.Black;
        }

        /// <summary>
        /// Standart TextChanged function of RichTextBox.
        /// Function only calls Highlighter function.
        /// </summary>
        private void SubTextChanged(object sender, EventArgs e)
         {
            ExpressionHighlighter(dialog_box);
         }

         /// <summary>
         /// Function creates modal dialog.
         /// </summary>
         /// <param name="form_type">Types of form - possibilities are Variable, Example and Path.</param>
         /// <param name="variable"> Variable name.</param>
         /// <param name="is_new"> If the dialog with its variable was yet created</param>
         private void CreateForm(String form_type, String variable, bool is_new, bool show)
         {
           //if (form_type == "Successor")
             //akt_index = int.Parse(variable);
           //else
             //akt_index = (int)variables[variable];

            new_form = new Form();
            new_form.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            new_form.StartPosition = FormStartPosition.CenterParent;
            new_form.MinimizeBox = false;
            new_form.MaximizeBox = false;
            this.AddOwnedForm(new_form);

            if (form_type == "Successor")
            {
                akt_index = int.Parse(variable);

                new_form.Width = 200;
                new_form.Height = 180;
                new_form.Text = "Successor name and variant";

                Label label1 = new Label();
                label1.Left = 35;
                label1.Top = new_form.Size.Height - 165;
                label1.Text = "Successor form name:";
                label1.AutoSize = true;
                new_form.Controls.Add(label1);
                label1.Show();

                RichTextBox edit = new RichTextBox();

                edit.Left = 35;
                edit.Top = new_form.Size.Height - 150;
                edit.Width = 120;
                edit.Height = 20;

                if (!is_new)
                    edit.Text = (string)successors[akt_index];

                new_form.Controls.Add(edit);
                edit.Show();

                dialog_box = edit;

                Label label2 = new Label();
                label2.Left = 35;
                label2.Top = new_form.Size.Height - 115;
                label2.Text = "Successor form variant:";
                label2.AutoSize = true;
                new_form.Controls.Add(label2);
                label2.Show();

                numeric = new NumericUpDown();

                numeric.Left = 35;
                numeric.Top = new_form.Size.Height - 100;
                numeric.Minimum = 0;
                numeric.Maximum = 100000;

                if (!is_new)
                    numeric.Value = (decimal)numerics[akt_index];
              
                new_form.Controls.Add(numeric);
                numeric.Show();



                Button ok_button = new Button();

                ok_button.Left = new_form.Size.Width / 2 - 80;
                ok_button.Top = new_form.Size.Height - 60;
                ok_button.Text = "OK";
                ok_button.Click += new System.EventHandler(this.OK2_Click);
                new_form.Controls.Add(ok_button);
                ok_button.Show();

                Button cancel_button = new Button();

                cancel_button.Left = new_form.Size.Width / 2;
                cancel_button.Top = new_form.Size.Height - 60;
                cancel_button.Text = "Cancel";
                cancel_button.Click += new System.EventHandler(this.Cancel2_Click);
                new_form.Controls.Add(cancel_button);
                cancel_button.Show();

            }
            else
            {
                akt_index = (int)variables[variable];

                new_form.Width = 300;
                new_form.Height = 200;

                if (form_type == "Variable")
                    new_form.Text = "WizardLanguage code";

                else if (form_type == "Example")
                    new_form.Text = "Example of practical situation";

                else new_form.Text = "Path to box property";

                RichTextBox richtext_box = new RichTextBox();

                richtext_box.Left = 20;
                richtext_box.Top = 20;
                richtext_box.Width = 250;

                richtext_box.Height = 100;

                new_form.Controls.Add(richtext_box);
                richtext_box.Show();

                if (form_type == "Variable")
                {
                    int length = variable.Length;

                    richtext_box.Text = variable + " = ";
                    richtext_box.Select(0, length);
                    richtext_box.SelectionColor = Color.Red;
                    richtext_box.Select(length + 3, 0);
                    richtext_box.TextChanged += new System.EventHandler(this.SubTextChanged);
                    this.main_toolTip.SetToolTip(richtext_box, variableUsing_help);
                }
                else if ((form_type == "Path") || (form_type == "Example"))
                {
                    int length = variable.Length;

                    richtext_box.Text = variable + " : ";
                    richtext_box.Select(length + 3, 0);

                    //if (form_type == "Path")
                    //  this.main_toolTip.SetToolTip(richtext_box, pathUsing_help); 
                    //else if (form_type == "Example")
                    //  this.main_toolTip.SetToolTip(richtext_box, exampleUsing_help); 
                }


                dialog_box = richtext_box;

                if (!is_new)
                {
                        dialog_box.Text = areas_text[akt_index];
                        if (form_type == "Variable")
                            ExpressionHighlighter(dialog_box);

                        richtext_box.Select(dialog_box.Text.Length + 1, 0);
                }

                Button ok_button = new Button();

                ok_button.Left = new_form.Size.Height - 80;
                ok_button.Top = new_form.Size.Height - 70;
                ok_button.Text = "OK";

                    ok_button.Click += new System.EventHandler(this.OK1_Click);

                new_form.Controls.Add(ok_button);
                ok_button.Show();

                Button cancel_button = new Button();

                cancel_button.Left = new_form.Size.Height;
                cancel_button.Top = new_form.Size.Height - 70;
                cancel_button.Text = "Cancel";

                    cancel_button.Click += new System.EventHandler(this.Cancel1_Click);

                new_form.Controls.Add(cancel_button);
                cancel_button.Show();

            }
             if (show)
             {
                 DialogResult result = new_form.ShowDialog();
             }
         }

         /// <summary>
         /// Standard RichTextBox TextChanged function, from here it is 
         /// necessary to call highlighter function
         /// </summary>
        private void TextChanged(object sender, EventArgs e)
        {
          int active_area;
            
            //for (active_area = 0; active_area < number_areas; active_area++)
          for (active_area = 0; active_area < boxes.Count; active_area++)
                if (boxes[active_area].Focused == true) break;

           // if (active_area == number_areas) return;
            if (active_area == boxes.Count) return;

            FormHihglighter(boxes[active_area]);
            
        }

        /// <summary>
        /// Standard RichTextBox SelectionChanged function, from here it is 
        /// necessary to call CreateForm function
        /// </summary>
        private void SelectionChanged(object sender, EventArgs e)
        {
            bool is_new = false;
            int area_index;
            int start_select, select_length;
            String selected_text;
            String ends = "([{;. &|!<=>\n";

           // for (area_index = 0; area_index < number_areas; area_index++)
            for (area_index = 0; area_index < boxes.Count; area_index++)
                if ((boxes[area_index].Focused == true) &&
                    (boxes[area_index].SelectionLength >= 2) ) break;
            
            //if (area_index != number_areas)
            if (area_index != boxes.Count)    
            {
                start_select = boxes[area_index].SelectionStart;
                select_length = boxes[area_index].SelectionLength;

             selected_text = boxes[area_index].Text.Substring(start_select, select_length);

             if (((selected_text.IndexOf('$') >= 0) || (selected_text.IndexOf('@') >= 0)||
                  (selected_text.IndexOf('^') >= 0))
                   && (selected_text.IndexOf(' ') < 0) ) 
              {
                  if (((start_select == 0) ||
                       (ends.IndexOf(boxes[area_index].Text[start_select - 1]) >= 0)) &&
                       //(boxes[area_index].Text[start_select - 1] == ' ')) &&
                       ((start_select + select_length == boxes[area_index].Text.Length) ||
                       (ends.IndexOf(boxes[area_index].Text[start_select + select_length]) >= 0)
                     //  (boxes[area_index].Text[start_select + select_length] == ' ')
                       ))
                  {
                      if (!variables.ContainsKey(selected_text))
                      {
                         variables[selected_text] = variables.Count;
                         is_new = true;
                      }

                      if (selected_text.IndexOf('$') >= 0)
                          CreateForm("Variable", selected_text, is_new, true);
                      else if (selected_text.IndexOf('^') >= 0)
                          CreateForm("Path", selected_text, is_new, true);
                      else CreateForm("Example", selected_text, is_new, true);
                  }
             } }
        }

        /// <summary>
        /// Standard mouse MouseDown function. Here the function 
        /// is used form drawing RichTextBox and Radio button with choice.
        /// </summary>
        public void Mouse_Down(object sender, MouseEventArgs e)
        {
            if (RButtonState == ButtonState.pressed)
            {
                RButtonState = ButtonState.unpressed;
                
                RadioButton radio_button = new RadioButton();
                RichTextBox edit = new RichTextBox();
                Button successor_button = new Button();

                if (buttons.Count == 0)
                {
                    radio_button.Left = e.X - Cursor.Size.Width / 4;
                    edit.Left = e.X - Cursor.Size.Width / 4 + 20;
                }
                else
                {
                    radio_button.Left = buttons[0].Left;
                    edit.Left = boxes[1].Left;
                }

                radio_button.Top = e.Y - Cursor.Size.Height / 4; 
                radio_button.Text = "";
                radio_button.Appearance = Appearance.Normal;
                radio_button.Width = 15;
                radio_button.Height = 15;
                radio_button.AutoSize = false;
                this.Controls.Add(radio_button);

                edit.Top = e.Y - Cursor.Size.Height / 4;
                edit.Width = this.Width - 190; 
                edit.Height = 20;
                edit.ReadOnly = true;
                edit.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                this.main_toolTip.SetToolTip(edit, edit_help);

                edit.SelectionChanged += new System.EventHandler(this.SelectionChanged);
                edit.TextChanged += new System.EventHandler(this.TextChanged);
                this.Controls.Add(edit);

                successor_button.Left = this.Size.Width - 100;
                successor_button.Top = e.Y - Cursor.Size.Height / 4; ;
                successor_button.Anchor = AnchorStyles.Right | AnchorStyles.Top;
                successor_button.Text = "Successor";

                successor_button.Click += new System.EventHandler(this.Successor_Click);
                this.Controls.Add(successor_button);

                successor.Add(successor_button);
                buttons.Add(radio_button);
                boxes.Add(edit);
            }
            else if (AButtonState == ButtonState.pressed)
            {
                RichTextBox text_box = new RichTextBox();
                text_box.Left = e.X;
                text_box.Top = e.Y;
                text_box.Width = 0;
                text_box.Height = 0;
                text_box.ReadOnly = true;
                
                text_box.SelectionChanged += new System.EventHandler(this.SelectionChanged);
                text_box.TextChanged += new System.EventHandler(this.TextChanged);
                this.Controls.Add(text_box);

                boxes.Add(text_box);
                isDragging = true;
           }
        }

        /// <summary>
        /// Standard mouse MouseMove function.Here is used for
        /// drawing RichTextBox.
        /// </summary>
        private void Mouse_move(object sender, MouseEventArgs e)
        {
            if ((isDragging) && (AButtonState == ButtonState.pressed))
            {
                boxes[boxes.Count-1].Height = e.Y - boxes[boxes.Count-1].Top;
                boxes[boxes.Count-1].Width = e.X - boxes[boxes.Count-1].Left;
            }
        }

        /// <summary>
        /// Standard mouse MouseUp function.Here is used for
        /// changing state of toolStrip buttons. 
        /// </summary>
        private void Mouse_Up(object sender, MouseEventArgs e)
        {
            if ((isDragging) && (AButtonState == ButtonState.pressed))
            {
                isDragging = false;
                AButtonState = ButtonState.unpressed;
                this.Cursor = Cursors.Default;
            }
            if (RButtonState == ButtonState.pressed)
            {
                RButtonState = ButtonState.unpressed;
            }
        }

        /// <summary>
        /// Function load informations to wizard form from string
        /// with XML format.
        /// </summary>
        /// <param name="XMLString"> String with XML content.</param>
        public void LoadFormFromXMLString(string XMLString)
        {
            XmlDocument form_document = new XmlDocument();

            form_document.LoadXml(XMLString);

            XmlNode FormsContent = form_document.DocumentElement;

                  int priority;

                  foreach (XmlNode fils in FormsContent.ChildNodes)
                  {
                     RichTextBox new_richtextbox = new RichTextBox();   
                      switch (fils.Name)
                      {
                          case "mainarea":
                              {
                                  boxes[0].Text = fils.InnerText;
                                  //new_richtextbox.Text = fils.InnerText;
                                  //boxes.RemoveAt(0);
                                  //boxes.Insert(0,new_richtextbox);
                                  
                                  break;
                              }
                          case "followchoice":
                              {
                                  priority = int.Parse(fils.Attributes.Item(0).Value);
                                  //new_richtextbox.Text = fils.InnerText;
                                  //boxes.RemoveAt(priority);
                                  //boxes.Insert(priority, new_richtextbox);
                                  foreach (XmlNode prafils in fils.ChildNodes)
                                    switch (prafils.Name)
                                     {
                                       case "text":
                                                    {
                                                      boxes[priority].Text = prafils.InnerText;    
                                                      break;
                                                    }
                                       case "successor":
                                                    {
                                                      successors[priority-1] = prafils.InnerText;    
                                                      break;
                                                    }
                                       case "variant":
                                                    {
                                                     numerics[priority - 1] =  decimal.Parse(prafils.InnerText);
                                                     break;
                                                    }

                                    }
                                  //XmlNodeList children = fils.ChildNodes;
                                  //children.Item(0).Name == "text"
                                  //boxes[priority].Text = fils.InnerText;

                                  break;
                              }
                          case "pointer":
                              {
                                  String pointer_text = fils.Attributes.Item(0).Value;
                                  int index = variables.Count;

                                  variables[pointer_text] = index;
                                  areas_text.Add(fils.InnerText);

                                  break;
                              }
                      }
                  }
        }

        /// <summary>
        /// Function save content of all forms to one string with
        /// XML format
        /// </summary>
        /// <param name="FormID"> Globally unique ID of WizardForm box.</param>
        public string SaveFormToXMLString(string FormID)
        {
            int index;
            string XMLStringOutput = "";
 
            StringWriter XMLstring = new StringWriter();

            XmlTextWriter XMLwriter = new XmlTextWriter(XMLstring);

            XmlDocument form_document = new XmlDocument();

            XmlElement parent_form = form_document.CreateElement("parent");

             XmlElement new_form = form_document.CreateElement("form");
             XmlAttribute form_attribute = form_document.CreateAttribute("ID");

               form_attribute.Value = FormID;
               new_form.SetAttributeNode(form_attribute);
               parent_form.AppendChild(new_form);

               XmlElement mainarea = form_document.CreateElement("mainarea");
               mainarea.InnerText = boxes[0].Text;
               new_form.AppendChild(mainarea);

               for (index = 1; index <= buttons.Count; index++)      
               {
                   if (boxes[index].Text == "") break;

                   XmlElement choice = form_document.CreateElement("followchoice");
                   XmlAttribute choice_attribute = form_document.CreateAttribute("Priority");

                   choice_attribute.Value = index.ToString();
                   choice.SetAttributeNode(choice_attribute);

                   XmlElement choice_text = form_document.CreateElement("text");
                   choice_text.InnerText = boxes[index].Text;
                   choice.AppendChild(choice_text);

                   XmlElement successor_name = form_document.CreateElement("successor");
                   successor_name.InnerText = (string) successors[index-1];
                   choice.AppendChild(successor_name);

                   XmlElement variant_name = form_document.CreateElement("variant");
                   variant_name.InnerText = (string)numerics[index - 1].ToString();
                   choice.AppendChild(variant_name);

                   //choice.InnerText = boxes[index].Text;
                   new_form.AppendChild(choice);
               }

               for (index = 0; index < variables.Count; index++)
               {
                   int char_index = 0;
                   string pointer = "";

                   while ((areas_text[index][char_index] != ':') &&
                          (areas_text[index][char_index] != '=') &&
                          (areas_text[index][char_index] != ' '))
                   {
                       pointer = pointer + areas_text[index][char_index].ToString();
                       char_index++;
                   }
                   XmlElement pointer_element = form_document.CreateElement("pointer");
                   XmlAttribute pointer_attribute = form_document.CreateAttribute("Name");

                   pointer_attribute.Value = pointer;
                   pointer_element.SetAttributeNode(pointer_attribute);
                   pointer_element.InnerText = areas_text[index];
                   new_form.AppendChild(pointer_element);
               }

               parent_form.WriteContentTo(XMLwriter);

               XMLStringOutput = XMLstring.ToString();

               XMLwriter.Close();

           return XMLStringOutput;
        }

        /// <summary>
        /// Validation of the form content.
        /// </summary>
        private bool Form_validate()
        {
            int i;

            for (i = 1; i < boxes.Count; i++)
                if ((!successors.ContainsKey(i - 1)) && (!successors.ContainsKey(i - 1))
                      && (boxes[i].Text == "")) continue;
                else if ((boxes[i].Text == "") &&
                         (((string)successors[i - 1] == "") || ((decimal)numerics[i - 1] == 0)))
                            continue;
                else if ((boxes[i].Text != "") && (!successors.ContainsKey(i - 1)))
                {
                    MessageBox.Show("Some form name and form variant are not defined", "Error",
                                      MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return false;
                }
                else if ((boxes[i].Text != "") && ((string)successors[i - 1] == "") )
                {
                    MessageBox.Show("Some form name is not defined", "Error",
                                      MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return false;
                }
                else if ((boxes[i].Text == "") &&
                     (((string)successors[i - 1] != "")))
                {
                    MessageBox.Show("Some choice has no text", "Error",
                                      MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return false;
                }

            return true;
        }

        /// <summary>
        /// Function save content when user clicks on OK button.
        /// </summary>
        private void OK_Click(object sender, EventArgs e)
        {
            bool validation = Form_validate();

            if (!validation) return;
              
            this.returnString = this.SaveFormToXMLString("1");

          //  WizardFormGenerator gen = 
            //    new WizardFormGenerator(this.returnString, 200, "Next", 0);

            //MessageBox.Show(returnString);

            this.DialogResult = DialogResult.OK;
           
            this.Dispose();
        }

        /// <summary>
        /// User click on Cancel button -> only close form.
        /// </summary>
        private void Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
 
            //this.Close();
            this.Dispose();
        }

        private void WizardFormEditor_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
     /*       Control controlNeedingHelp = null;
   
             Point pt = this.PointToClient(hlpevent.MousePos);

               foreach(Control control in this.Controls)
               {
                   if (control == null) return;
                   Rectangle myBounds = control.DisplayRectangle;

                   if ( myBounds.Contains(pt)) 
                   {
                       controlNeedingHelp = control;
                       break;
                   }
               }
               string help = main_toolTip.GetToolTip(controlNeedingHelp);
            if (help.Length > 0)  MessageBox.Show(help, "Help");*/
        }

        private void IDEdit_Click(object sender, EventArgs e)
        {
                    }

        private void ToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void Form_Validating(object sender, CancelEventArgs e)
        {
                 
        }


    }
}






































/*   XmlTextWriter writer = new XmlTextWriter("forms.xml", null);
            
    writer.Formatting = Formatting.Indented;
    writer.Indentation = 2;

      writer.WriteStartDocument();
      writer.WriteStartElement("FormsContent");
            
       writer.WriteStartElement("form");
       writer.WriteAttributeString("ID", IDEdit.Text);
       writer.WriteWhitespace("\n");
       writer.WriteRaw(boxes[0].Text);
       writer.WriteWhitespace("\n");

       for (index = 1; index < number_buttons; index++)
        {
         if (boxes[index].Text == "") break;

           writer.WriteStartElement("followchoice");
           writer.WriteAttributeString("Priority", index.ToString());
           writer.WriteWhitespace("\n");
           writer.WriteRaw(boxes[index].Text);
           writer.WriteWhitespace("\n");
           writer.WriteEndElement();
           writer.WriteWhitespace("\n");
        }
                
        for (index = 0; index < variables.Count; index++)
        {
            int char_index=0;
            String pointer = "";

            while ((areas[index].Text[char_index] != ':') &&
                     (areas[index].Text[char_index] != '='))
               {
                   pointer = pointer + areas[index].Text[char_index].ToString();
                   char_index++;
               }
             pointer.Trim();
            writer.WriteStartElement("pointer");
            writer.WriteAttributeString("Name", pointer);
            writer.WriteWhitespace("\n");
            writer.WriteRaw(areas[index].Text);
            writer.WriteWhitespace("\n");
            writer.WriteEndElement();
            writer.WriteWhitespace("\n");
        }

    writer.WriteWhitespace("\n");
    writer.WriteEndElement();
    writer.Close();  
 
 *           /*      if ((r_begin = text.IndexOf(else_word, start)) >= 0)
                { length = 4; begin = r_begin; }
                else if ((r_begin = text.IndexOf(if_word, start)) >= 0)
                { length = 2; begin = r_begin; }
                else if ((r_begin = text.IndexOf(elseif_word, start)) >= 0)
                { length = 6; begin = r_begin; }
                else begin = -1;
            */
            //if (begin != -1)


 
  
              /*
            Find_variable(areas[active_area].Text, ref chr, -1, ref begin1, ref length1);
            if (begin1 < 0) return;

            Find_condition(areas[active_area].Text, -1, ref begin2, ref length2);
            if (begin2 < 0) return;

            areas[active_area].Select(begin1, length1);
            areas[active_area].SelectionColor = Color.Red;

            areas[active_area].Select(begin2, length2);
            areas[active_area].SelectionColor = Color.Blue;

            areas[active_area].Select(areas[active_area].Text.Length, 0);
            areas[active_area].SelectionColor = Color.Black;
             * */




//            Cursor.Position = position;
            /*length = boxes[active_area].Text.Length;

            if (boxes[active_area].Text[length - 1] == '$')
            {
                boxes[active_area].Select(length - 1, 1);
                boxes[active_area].SelectionColor = Color.Red;
                boxes[active_area].Select(length, 0);
            }
            if ( (boxes[active_area].Text[length - 1] == '@') ||
               (boxes[active_area].Text[length - 1] == '^') )
            {
                boxes[active_area].Select(length - 1, 1);
                boxes[active_area].SelectionColor = Color.Green;
                boxes[active_area].Select(length, 0);
            }

            if (ends.IndexOf(boxes[active_area].Text[length - 1]) >= 0)
                boxes[active_area].SelectionColor = Color.Black;
              
              */


//int active_area;

//for (active_area = 0; active_area < variables.Count; active_area++)
//   if (areas[active_area].Focused == true) break;

// if (active_area == variables.Count) return;









/*        private void LoadDocument(String File, String form_identifier)
        {
            XmlDocument form_document = new XmlDocument();

            form_document.Load(File);

            XmlNode FormsContent = form_document.DocumentElement;

            foreach (XmlNode forms in FormsContent.ChildNodes)
                if (forms.Attributes.Item(0).Value == form_identifier)
                {
                   int priority;   

                    //IDEdit.Text = form_identifier;

                    foreach (XmlNode fils in forms.ChildNodes)
                       switch (fils.Name)
                       {
                         case "mainarea": boxes[0].Text = fils.InnerText; break;
                         case "followchoice":
                                            {
                                              priority = int.Parse(fils.Attributes.Item(0).Value);
                                              boxes[priority].Text = fils.InnerText;

                                              break;   
                                            }
                         case "pointer":
                                            {
                                             String pointer_text = fils.Attributes.Item(0).Value;
                                             int index = variables.Count;
                                             variables[pointer_text] = index;
                                             
                                             areas_text[index] = fils.InnerText;
                                             break;
                                            }
         }  }   
        } */

/* private void WriteChanges()
 {
    int index;
    String ID = "1";// IDEdit.Text;

    XmlDocument form_document = new XmlDocument();

     form_document.Load("forms.xml");
     XmlNode document_child = form_document.DocumentElement;

     foreach (XmlNode node in document_child.ChildNodes)
         if (node.Attributes.Item(0).Value == ID)
         {
             document_child.RemoveChild(node);
             break;
         }
       document_child = form_document.DocumentElement;


      XmlTextWriter writer = new XmlTextWriter("forms.xml", null);
      XmlElement new_form = form_document.CreateElement("form");
      XmlAttribute form_attribute = form_document.CreateAttribute("ID");

        form_attribute.Value = ID;
        new_form.SetAttributeNode(form_attribute);

        XmlElement mainarea = form_document.CreateElement("mainarea");
        mainarea.InnerText = boxes[0].Text;
        new_form.AppendChild(mainarea);
               

                   for (index = 1; index < number_buttons; index++)
                   {
                       if (boxes[index].Text == "") break;
                       XmlElement choice = form_document.CreateElement("followchoice");
                       XmlAttribute choice_attribute = form_document.CreateAttribute("Priority");

                         choice_attribute.Value = index.ToString();
                         choice.SetAttributeNode(choice_attribute);
                         choice.InnerText = boxes[index].Text;
                         new_form.AppendChild(choice);
                   }

                   for (index = 0; index < variables.Count; index++)
                   {
                       int char_index = 0;
                       string pointer = "";

                       while ((areas_text[index][char_index] != ':') &&
                              (areas_text[index][char_index] != '=') &&
                              (areas_text[index][char_index] != ' ') )
                       {
                           pointer = pointer + areas_text[index][char_index].ToString();
                           char_index++;
                       }
                       XmlElement pointer_element = form_document.CreateElement("pointer");
                       XmlAttribute pointer_attribute = form_document.CreateAttribute("Name");

                         pointer_attribute.Value = pointer;
                         pointer_element.SetAttributeNode(pointer_attribute);
                         pointer_element.InnerText = areas_text[index];
                         new_form.AppendChild(pointer_element);
                   }
      form_document.DocumentElement.InsertAfter(new_form,
                             form_document.DocumentElement.LastChild);

     form_document.WriteContentTo(writer);

    writer.Close();
 }*/
