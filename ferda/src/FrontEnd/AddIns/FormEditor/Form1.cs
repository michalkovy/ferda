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
using System.Diagnostics;
using System.Xml.XPath;
using System.Xml.Serialization;
using System.Windows.Forms;
//using Ferda.FrontEnd.AddIns.FormGenerator;
//using Interpreter;


/// <summary>
/// Class include implementation of Form editor
/// </summary>
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
        ///List of RadioButtons in form
        /// </summary>
        private List<RadioButton> buttons;

        /// <summary>
        /// List of buttons successor
        /// </summary>
        private List<Button> successor;


        /// <summary>
        /// List of RichTextBoxes in form
        /// </summary>
        private List<RichTextBox> boxes;

        /// <summary>
        ///Modal dialog for setting variable, example or path
        /// </summary>
        private Form new_form;

        private RichTextBox dialog_box;
        private RichTextBox edit1;
        private RichTextBox edit2;
        private RichTextBox edit3;

        /// <summary>
        ///List of content of all RichTextBoxes.
        /// </summary>
        private List<string> areas_text;

        /// <summary>
        /// Index of last active RichTextBox
        /// </summary>
        private int akt_index;

        /// <summary>
        /// Language code
        /// </summary>
        private string language;

        /// <summary>
        /// Keep track of when fake "drag and drop" mode is enabled.
        /// </summary>
        private bool isDragging = false;
        private CheckBox check;

        /// <summary>
        /// XML string that is the output of the Form editor
        /// </summary>
        private string returnString;


        /// <summary>
        /// EN hint for main area(main RichTextBox)
        /// </summary>
        private String area_help_EN = @"
           Here insert text, that displays to common user. 

        -  When you want to insert variable identifier , write $variable_name. 
           Variable identifier is displayed with red color. Later the variable 
           value will be evaluated and in substituted in generated form. 
        -  When you want to insert example identifier, write @example_name. 
           Example identifier is displayed with green color. In generated
           form will be figured hypertext with link to text content of the 
           example.
        
           To fill the body of identifier, mark it.
           
           ATTENTION: Identifiers must be without spaces. 
           ";

        /// <summary>
        /// CZ hint for main area(main RichTextBox)
        /// </summary>
        private String area_help_CZ = @"
           Sem můžete vložit text, jež se zobrazí běžnému uživatli.

        -  Pokud chcete vložit identifikátor proměnné, napište $jmeno_promenne. 
           Identifikátor se zobrazí červenou barvou. Ve vygenerovaném formuláři
           se  obsah proměnné vyhodnotí a expanduje.  
        -  Pokud chcete vložit identifikátor příkladu, napište @jmeno_prikladu. 
           Identifikátor příkladu je zobrazen v zelené barvě. Ve vygenerovaném
           formuláři bude zobrazen hypertext s odkazem na znění příkladu. 
        
           Pro vyplnění těla identifikátoru, tento označte. 
           
           POZOR: Identifikátory musí být bez mezer. 
           ";

        /// <summary>
        /// EN hint for edits(choices)
        /// </summary>
        private String edit_help_EN = @"
        Here insert version of one choice. Priorities are ordered from top to down.
        You can use variables, examples as in the main area.
 
        And more you can use PATH identifiers , if you'll write ^path_name.
        This identifier serves for definition controlled path property.
        To fill the path body - mark its identifier. 
        ";

        /// <summary>
        /// CZ hint for edits(choices)
        /// </summary>
        private String edit_help_CZ = @"
        Sem vložte znění k jedné volbě. Volby jsou řazeny dle priorit směrem od 
        shora dolů. Můžete využívat identifikátory proměnných a příkadů jako v 
        hlavní oblasti.

        A navíc můžete zadat identifikátor CESTY, pokud napišete ^jmeno_cesty.
        Tento identifikátor slouží k definici kontrolované vlastnosti jisté krabičky.
        Pro vyplnění těla cesty je nutné označit jeho identifikátor. 
        ";

        /// <summary>
        /// EN hint for variable definition window.
        /// </summary>
        private String variableUsing_help_EN = @"
        Here you can write source code of wizard language such as:

          $a = 1;
          $b = $a * 4;
          $c = $array[$b] - $a;   
          $d = if ($a > 4) {$b} else {$c};
          $f = if (($a - 2 >= 6) && ($b < 8)) {$b+1} else {$a * 4};
        ";

        /// <summary>
        /// CZ hint for variable definition window.
        /// </summary>
        private String variableUsing_help_CZ = @"
        Sem můžete vkládat zdrojový kod jazyka WizardLanguage napr.:

          $a = 1;
          $b = $a * 4;
          $c = $array[$b] - $a;   
          $d = if ($a > 4) {$b} else {$c};
          $f = if (($a - 2 >= 6) && ($b < 8)) {$b+1} else {$a * 4};
        ";

        /// <summary>
        /// EN hint for example RichTextBox
        /// </summary>
        private String exampleUsing_help_EN = @"
        Here insert text version of practical example
        ";

        /// <summary>
        /// CZ hint for example RichTextBox
        /// </summary>
        private String exampleUsing_help_CZ = @"
        Vložte textové znění příkladu
        ";

        /// <summary>
        /// EN hint for successors
        /// </summary>
        private String successorBox_help_EN = @"
        Here insert user name of next box in scenario.
        ";

        /// <summary>
        /// CZ hint for successors
        /// </summary>
        private String successorBox_help_CZ = @"
        Vložte uživatelské pojmenování následnické krabičky
        ve scénáři.
        ";

        /// <summary>
        /// EN hint for setting form variant
        /// </summary>
        private String nextVariant_help_EN = @"
        Here insert variant of first box of type Form.

        E.g.: We have form with four choices - basic variant of this form is 1234 
              (all choices are present). Variant 124 discribe, that are present 
              first two choices and last choice (sorted by priority).
        ";

        /// <summary>
        /// CZ hint for setting form variant
        /// </summary>
        private String nextVariant_help_CZ = @"
        Vložte variantu následnické krabičky typu Formulář.

        Př.: Máme formulář se čtyřmi volbami - základní varianta tohoto formuláře je 1234 
             (všechny volby přítomny. Varianta 124 popisuje, že jsou přítomné první dvě
             volby a volba poslední (seřazany dle priorit).   
        ";

        /// <summary>
        /// EN hint box user name
        /// </summary>
        private String controlledBox_help_EN = @"
        Here insert user name of box, whose property will be checked.   
        ";

        /// <summary>
        /// CZ hint box user name
        /// </summary>
        private String controlledBox_help_CZ = @"
        Vložte uživatelské jméno krabičky, jejíž hodnota bude kontrolována.
        ";

        /// <summary>
        /// EN hint property
        /// </summary>
        private String controlledProperty_help_EN = @"
        Here insert name of property, whose will be checked.   
        ";

        /// <summary>
        /// CZ hint property
        /// </summary>
        private String controlledProperty_help_CZ = @"
        Vložte jméno vlastosti, jejíž hodnota bude kontrolována.
        ";

        /// <summary>
        /// EN hint for value
        /// </summary>
        private String offeredValue_help_EN = @"
        Here insert recommended value.
        ";

        /// <summary>
        /// CZ hint for value
        /// </summary>
        private String offeredValue_help_CZ = @"
        Vložte doporučovanou hodnotu.
        ";

        /// <summary>
        /// hash table between variable and value
        /// </summary>
        private System.Collections.Hashtable variables;

        /// <summary>
        /// hash table between index and value
        /// </summary>
        private System.Collections.Hashtable indexes;

        /// <summary>
        /// hash table between successor and value
        /// </summary>
        private System.Collections.Hashtable successors;

        /// <summary>
        /// hash table between variant and value
        /// </summary>
        private System.Collections.Hashtable numerics;

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
        /// Class constructor.
        /// Lading content from XML string.
        /// </summary>
        /// <param name="ContentToLoad">Content of form in XML format</param>
        /// <param name="localization">Localization EN/CZ</param>
        public WizardFormEditor(string ContentToLoad, string localization)
        {
            InitializeComponent();

           // number_buttons = 0;
           // number_areas = 0;

            areas_text = new List<string>();
            boxes = new List<RichTextBox>();
            buttons = new List<RadioButton>();
            successor = new List<Button>();

            dialog_box = new RichTextBox();
            edit1 = new RichTextBox();
            edit2 = new RichTextBox();
            edit3 = new RichTextBox();
            check = new CheckBox();

            variables = new System.Collections.Hashtable();
            successors = new System.Collections.Hashtable();
            numerics = new System.Collections.Hashtable();
            indexes = new System.Collections.Hashtable();

            RButtonState = ButtonState.unpressed;
            AButtonState = ButtonState.unpressed;

            this.language = localization;

            this.returnString = ContentToLoad;

            Initialize();

            if (ContentToLoad != "")
              {
                  LoadFormFromXMLString(ContentToLoad);

                  for (int i = 0; i < 5; i++)
                      FormHihglighter(boxes[i]);
              }
            

        }
        /// <summary>
        /// Standard form_load method here is set title
        /// according localization.
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            if (language == "en-US") this.Text = "Wizard form editor";
            else this.Text = "Editor formulářů ";
          //  this.ShowInTaskbar = false;
        }

        /// <summary>
        /// Function initialize common controls,
        /// set toolTips, etc.
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
            
            if (language == "en-US")
              this.main_toolTip.SetToolTip(text_box, area_help_EN);
            else
              this.main_toolTip.SetToolTip(text_box, area_help_CZ);

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
                if (language == "en-US")
                   this.main_toolTip.SetToolTip(edit, edit_help_EN);
                else
                   this.main_toolTip.SetToolTip(edit, edit_help_CZ);

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
        /// Standard button_click method.
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
        /// Standard button_click method.
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
        /// Standard button_click method.
        /// When user click on Successor button, it is
        /// necessary to save the content of Successor dialog.
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
        /// Standard button_click method.
        /// When user click on OK button in 
        /// WizardLanguage or Example modal dialog.
        /// It is necessary to save changes.
        /// </summary>
        private void OK1_Click(object sender, EventArgs e)
        {
            if (areas_text.Count > akt_index)
                        areas_text.RemoveAt(akt_index);

            areas_text.Insert(akt_index, dialog_box.Text);
            new_form.Close();
        }

        /// <summary>
        /// Standard button_click method.
        /// When user click on Cancel button of modal dialog,
        /// this dialog must be closed.
        /// </summary>
        private void Cancel1_Click(object sender, EventArgs e)
        {
            new_form.Close();
        }

        /// <summary>
        /// Standard button_click method.
        /// When user click on OK button of modal successor dialog,
        /// it is necessary to save successor name and variant.
        /// </summary>
        private void OK2_Click(object sender, EventArgs e)
        {
            successors[akt_index] = dialog_box.Text;
            numerics[akt_index] = numeric.Value;
            new_form.Close();
        }

        /// <summary>
        /// Standard button_click method.
        /// When user click on Cancel button of modal successor dialog,
        /// dialog must be closed.
        /// </summary>
        private void Cancel2_Click(object sender, EventArgs e)
        {
            new_form.Close();
        }

        /// <summary>
        /// Standard button_click method.
        /// When user click on OK button of modal path definition dialog,
        /// it is necessary to save box name, property name and value.
        /// </summary>
        private void OK3_Click(object sender, EventArgs e)
        {
            if (areas_text.Count > akt_index)
                areas_text.RemoveAt(akt_index);

            string tendency = check.Checked ? "Y" : "N";
            string path = edit1.Name + "->" +
                          edit1.Text + "->" + edit2.Text + "->" + 
                          edit3.Text + "->" + tendency;

            areas_text.Insert(akt_index, path);

            new_form.Close();
        }

        /// <summary>
        /// Standard button_click method.
        /// When user click on Cancel button of modal path definition dialog,
        /// dialog is closed.
        /// </summary>
        private void Cancel3_Click(object sender, EventArgs e)
        {
            new_form.Close();
        }

        /// <summary>
        /// Method finds specific text(condition types) in input string
        /// Method is used by highlighter.
        /// </summary>
        /// <param name="text">Input text.</param>
        /// <param name="start">Start finding position.</param>
        /// <param name="begin">Begin of founded text.</param>
        /// <param name="length">Number chars of founded text</param>
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
        /// Function find specific text(variables, examples or paths) identifiers 
        /// in input string(used by highliter).
        /// </summary>
        /// <param name="text">Input text.</param>
        /// <param name="start">Start finding position.</param>
        /// <param name="begin">Begin of founded text.</param>
        /// <param name="length">Number chars of founded text</param>
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
        /// Method highlight variables, examples and paths 
        /// in incoming RichTextBox. Highlighting is made
        /// by changing color of selecting text.
        /// </summary>
        /// <param name="box">Highlighted RichTextBox</param>
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
        /// Highlighting is made by changing color of selecting text.
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
        /// Standard TextChanged function of RichTextBox.
        /// Function only calls Highlighter function.
        /// </summary>
        private void SubTextChanged(object sender, EventArgs e)
         {
            ExpressionHighlighter(dialog_box);
         }

         /// <summary>
         /// Function creates modal dialog of several type.
         /// </summary>
         /// <param name="form_type">Types of form - possibilities are Variable, Example and Path</param>
         /// <param name="variable"> Variable name</param>
         /// <param name="is_new"> If the dialog with its variable was yet created</param>
         private void CreateForm(String form_type, String variable, bool is_new, bool show)
         {
            new_form = new Form();
            new_form.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            new_form.StartPosition = FormStartPosition.CenterParent;
            new_form.MinimizeBox = false;
            new_form.MaximizeBox = false;
            this.AddOwnedForm(new_form);

            ToolTip dialog_toolTip = new ToolTip();
            dialog_toolTip.IsBalloon = true;

            if (form_type == "Successor")
            {
                akt_index = int.Parse(variable);

                new_form.Width = 200;
                new_form.Height = 180;

                if (language == "en-US")
                  new_form.Text = "Setting successors";
                else
                  new_form.Text = "Nastavení pro následníky";

                Label label1 = new Label();
                label1.Left = 35;
                label1.Top = new_form.Size.Height - 165;
                if (language == "en-US")
                    label1.Text = "Successor box user name:";
                else
                    label1.Text = "Jméno následníka:"; 
                label1.AutoSize = true;
                new_form.Controls.Add(label1);
                label1.Show();

                RichTextBox edit = new RichTextBox();

                edit.Left = 35;
                edit.Top = new_form.Size.Height - 150;
                edit.Width = 120;
                edit.Height = 20;
                if (language == "en-US")
                   dialog_toolTip.SetToolTip(edit, successorBox_help_EN);
                else
                   dialog_toolTip.SetToolTip(edit, successorBox_help_CZ);

                if (!is_new)
                    edit.Text = (string)successors[akt_index];

                new_form.Controls.Add(edit);
                edit.Show();

                dialog_box = edit;

                Label label2 = new Label();
                label2.Left = 35;
                label2.Top = new_form.Size.Height - 115;
                if (language == "en-US")
                    label2.Text = "Next Form box variant:";
                else
                    label2.Text = "Varianta následníka:";
                label2.AutoSize = true;
                new_form.Controls.Add(label2);
                label2.Show();

                numeric = new NumericUpDown();

                numeric.Left = 35;
                numeric.Top = new_form.Size.Height - 100;
                numeric.Minimum = 0;
                numeric.Maximum = 100000;
                if (language == "en-US")
                   dialog_toolTip.SetToolTip(numeric, nextVariant_help_EN);
                else
                   dialog_toolTip.SetToolTip(numeric, nextVariant_help_CZ);

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
            else if (form_type == "Path")
            {
                akt_index = (int)variables[variable];

                string[] path_array = {};

                if (!is_new)
                {
                  path_array = areas_text[akt_index].Split(new string[] { "->" },  
                                                     StringSplitOptions.None );
                }

                new_form.Width = 420;
                new_form.Height = 150;
                if (language == "en-US")
                  new_form.Text = "Path to property";
                else
                  new_form.Text = "Cesta k vlastnosti";

                Label label1 = new Label();
                label1.Left = 20;
                label1.Top = 20;
                if (language == "en-US")
                   label1.Text = "Box user name:";
                else
                   label1.Text = "Jméno krabičky:";
                label1.AutoSize = true;
                new_form.Controls.Add(label1);
                label1.Show();

                edit1 = new RichTextBox();

                edit1.Left = 20;
                edit1.Top = 35;
                edit1.Width = 120;
                edit1.Height = 20;
                edit1.Name = variable + " : ";
                if (language == "en-US")
                  dialog_toolTip.SetToolTip(edit1, controlledBox_help_EN);
                else
                  dialog_toolTip.SetToolTip(edit1, controlledBox_help_CZ);
                if (!is_new)
                    edit1.Text = path_array[1];

                new_form.Controls.Add(edit1);
                edit1.Show();

                Label label2 = new Label();
                label2.Left = 150;
                label2.Top = 20;
                if (language == "en-US")
                   label2.Text = "Property name:";
                else
                   label2.Text = "Jméno vlastnosti:";

                label2.AutoSize = true;
                new_form.Controls.Add(label2);
                label2.Show();

                edit2 = new RichTextBox();

                edit2.Left = 150;
                edit2.Top = 35;
                edit2.Width = 120;
                edit2.Height = 20;
                if (language == "en-US")
                  dialog_toolTip.SetToolTip(edit2, controlledProperty_help_EN);
                else
                  dialog_toolTip.SetToolTip(edit2, controlledProperty_help_CZ);
                if (!is_new)
                    edit2.Text = path_array[2];

                new_form.Controls.Add(edit2);
                edit2.Show();

                Label label3 = new Label();
                label3.Left = 280;
                label3.Top = 20;
                if (language == "en-US")
                    label3.Text = "Property value:";
                else
                    label3.Text = "Hodnota vlastnosti:";
                label3.AutoSize = true;
                new_form.Controls.Add(label3);
                label3.Show();

                edit3 = new RichTextBox();

                edit3.Left = 280;
                edit3.Top = 35;
                edit3.Width = 120;
                edit3.Height = 20;
                if (language == "en-US")
                    dialog_toolTip.SetToolTip(edit3, offeredValue_help_EN);
                else
                    dialog_toolTip.SetToolTip(edit3, offeredValue_help_CZ);

                if (!is_new)
                    edit3.Text = path_array[3];

                new_form.Controls.Add(edit3);
                edit3.Show();

                check = new CheckBox();

                check.Left = 20;
                check.Top = 60;
                if (language == "en-US")
                   check.Text = "Follow tendency:";
                else
                   check.Text = "Sleduj tendence:";
                check.AutoSize = true;

                if (!is_new)
                {
                    if (path_array[4] == "Y")
                                    check.Checked = true;
                }

                new_form.Controls.Add(check);
                check.Show();

                Button ok_button = new Button();

                ok_button.Left = new_form.Size.Width  - 180;
                ok_button.Top = new_form.Size.Height - 60;
                ok_button.Text = "OK";
                ok_button.Click += new System.EventHandler(this.OK3_Click);
                new_form.Controls.Add(ok_button);
                ok_button.Show();

                Button cancel_button = new Button();

                cancel_button.Left = new_form.Size.Width - 100;
                cancel_button.Top = new_form.Size.Height - 60;
                cancel_button.Text = "Cancel";
                cancel_button.Click += new System.EventHandler(this.Cancel3_Click);
                new_form.Controls.Add(cancel_button);
                cancel_button.Show();

            }
            else
            {
                akt_index = (int)variables[variable];

                if (form_type == "Variable")
                    new_form.FormBorderStyle = FormBorderStyle.SizableToolWindow;

                new_form.Width = 300;
                new_form.Height = 200;


                if (form_type == "Variable")
                {
                    if (language == "en-US")
                         new_form.Text = "WizardLanguage code";
                    else
                         new_form.Text = "Kód jazyka WizardLanguage";
                }

                else if (form_type == "Example")
                {
                    if (language == "en-US")
                       new_form.Text = "Example of practical situation";
                    else
                       new_form.Text = "Příklad praktické situace";
                }

                //else new_form.Text = "Path to box property";

                RichTextBox richtext_box = new RichTextBox();

                richtext_box.Left = 20;
                richtext_box.Top = 20;
                richtext_box.Width = 250;

                richtext_box.Height = 100;
                richtext_box.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
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

                    if (language == "en-US")
                       dialog_toolTip.SetToolTip(richtext_box, variableUsing_help_EN);
                    else
                       dialog_toolTip.SetToolTip(richtext_box, variableUsing_help_CZ);
                }
                else if ( form_type == "Example" )
                {
                    int length = variable.Length;

                    richtext_box.Text = variable + " : ";
                    richtext_box.Select(length + 3, 0);
                    if (language == "en-US")
                       dialog_toolTip.SetToolTip(richtext_box, exampleUsing_help_EN);
                    else
                       dialog_toolTip.SetToolTip(richtext_box, exampleUsing_help_CZ);
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
                ok_button.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
                ok_button.Top = new_form.Size.Height - 70;
                ok_button.Text = "OK";

                ok_button.Click += new System.EventHandler(this.OK1_Click);

                new_form.Controls.Add(ok_button);
                ok_button.Show();

                Button cancel_button = new Button();

                cancel_button.Left = new_form.Size.Height;
                cancel_button.Top = new_form.Size.Height - 70;
                cancel_button.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
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
         /// Standard RichTextBox TextChanged method.
         /// Here it is necessary to call highlighter function.
         /// </summary>
        private new void TextChanged(object sender, EventArgs e)
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
        /// Standard RichTextBox SelectionChanged method.
        /// Method reacts when user select path, example
        /// or variable identifier.
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
                         int bound = variables.Count;

                         variables[selected_text] = bound;
                         indexes[bound] = selected_text;

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
        /// Standard mouse MouseDown method. 
        /// Methd is used form placing edits(RichtextBoxes)
        /// and choices (RadioButtons with RichtextBoxes and Buttons).
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
                if (language == "en-US")
                   this.main_toolTip.SetToolTip(edit, edit_help_EN);
                else
                   this.main_toolTip.SetToolTip(edit, edit_help_CZ);

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
        /// Standard mouse MouseMove function.
        /// Here is used for drawing RichTextBox.
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
        /// Standard mouse MouseUp function.
        /// Here is used forchanging state of toolStrip buttons. 
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
        /// Function load informations to FormEditor form from string
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
                                  
                                  break;
                              }
                          case "followchoice":
                              {
                                  priority = int.Parse(fils.Attributes.Item(0).Value);

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
                                                         //int.Parse(
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
                                  indexes[index] = pointer_text;
                                  areas_text.Add(fils.InnerText);

                                  break;
                              }
                      }
                  }
        }

        /// <summary>
        /// Function save content of all RichtextBoxes
        /// to one string with and dialogs to XML format.
        /// </summary>
        /// <param name="FormID"> Globally unique ID of WizardForm box - now not important.</param>
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
                   XmlElement pointer_element = form_document.CreateElement("pointer");
                   XmlAttribute pointer_attribute = form_document.CreateAttribute("Name");

                   pointer_attribute.Value = (string)indexes[index];// pointer;
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
        /// Method produces MessageBoxes and not
        /// allow to leave FormEditor when Error.
        /// </summary>
        private bool Form_validate()
        {
            int i;

            for (i = 1; i < boxes.Count; i++)
            {
                if (boxes[i].Text.IndexOf('^') != boxes[i].Text.LastIndexOf('^'))
                {
                  MessageBox.Show("Some choice has more paths", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                  return false;
                }

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
                else if ((boxes[i].Text != "") && ((string)successors[i - 1] == ""))
                {
                  if (language == "en-US")
                    MessageBox.Show("Some form name is not defined", "Error",
                                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                  else
                    MessageBox.Show("Jméno některého formuláře není definováno", "Chyba",
                                      MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return false;
                }
                else if ((boxes[i].Text == "") &&
                     (((string)successors[i - 1] != "")))
                {
                  if (language == "en-US")
                    MessageBox.Show("Some choice has no text", "Error",
                                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                  else
                    MessageBox.Show("Některá volba nemá text", "Chyba",
                                      MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return false;
                }
            }

            for (i = 0; i < variables.Count; i++)
                if (areas_text[i][0] == '^')
                {
                  int index;
    
                    if ((index=areas_text[i].IndexOf("->->")) >= 0)      
                    {
                      if (language == "en-US")
                        MessageBox.Show("Some part of path to property is not filled", "Error",
                                          MessageBoxButtons.OK, MessageBoxIcon.Error);
                      else
                        MessageBox.Show("Některá část cesty k vlastnosti není vyplněna", "Chyba",
                                          MessageBoxButtons.OK, MessageBoxIcon.Error);

                        return false;
                    }
                }

            return true;
        }

        /// <summary>
        /// When Validation is OK, 
        /// method save content when user clicks on OK button.
        /// </summary>
        private void OK_Click(object sender, EventArgs e)
        {
            bool validation = Form_validate();

            if (!validation) return;
              
            this.returnString = this.SaveFormToXMLString("1");

          // VariableServices service = new VariableServices();
            /*
            WizardFormGenerator gen = 
                new WizardFormGenerator(this.returnString, 200, "Next_STOP", 0, service);

            System.Windows.Forms.DialogResult result = gen.GenerateForm();

            //MessageBox.Show(returnString);*/

            this.DialogResult = DialogResult.OK;
           
            this.Dispose();
        }

        /// <summary>
        /// User click on Cancel button the form must be 
        /// closed with Cancel result.
        /// </summary>
        private void Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            
 //           System.DiagnosticsConfigurationHandler.

            //this.Close();
            this.Dispose();

            
        }


        //END
        private void WizardFormEditor_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
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

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            string path = Application.ExecutablePath;
            int index = path.LastIndexOf("\\");
            path = path.Remove(index);
            Process.Start(path + "\\AddIns\\Help\\FormEditor.pdf");
        }

    }
}
