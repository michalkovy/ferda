//Editor for editing reagular expressions. 
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
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace Ferda.FrontEnd.AddIns.RegEditor
{
    /// <summary>
    /// Form for inserting regular expressions
    /// and connectiong him with successors.
    /// </summary>        
    public partial class RegularExpEditor : Form
    {
        /// <summary>
        /// Global numericUpDown.
        /// </summary>        
        NumericUpDown numeric;

        /// <summary>
        /// Main RichTextBox
        /// </summary>        
        RichTextBox edit;

        /// <summary>
        /// Dialog form setting successor
        /// </summary>        
        private Form new_form;

        /// <summary>
        /// Language code
        /// </summary>
        private string language;

        /// <summary>
        /// List of variant shortcuts.
        /// </summary>        
        private List<int> numerics;

        /// <summary>
        /// List of successor form names.
        /// </summary>        
        private List<string> successor;

        /// <summary>
        /// Assotiative array of regular expressions.
        /// </summary>        
        private System.Collections.Hashtable regulars;


        /// <summary>
        /// index of active dialog.
        /// </summary>
        private int akt_index;

        /// <summary>
        /// EN hint for inserting rerular expressions
        /// </summary>
        private string regular_help_EN = @"
        Here insert set of regular expressions. 
        E.g.:

         (form10_123){5}
         (form10_[1-9]{1,4}){6}
         (form1(0|5)_[1-9]){8}

        Each expression must be in different line.
        To discribe the succesor after matching
        regular expression - select it.   
        ";

        /// <summary>
        /// CZ hint for inserting rerular expressions
        /// </summary>
        private string regular_help_CZ = @"
        Vložte množinu regulárních výrazů. 
        Př.:

         (form10_123){5}
         (form10_[1-9]{1,4}){6}
         (form1(0|5)_[1-9]){8}

        Každý regulární výraz musí být na zvláštním řádku.
        Pro specifikaci následnického formuláře, jež se zobrazí
        pokud je některý výraz splněn - vyberte příslušný výraz. 
        ";

        /// <summary>
        /// EN hint for successors
        /// </summary>
        private String successorBox_help_EN = @"
        Here insert user name of next box in scenario
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


        private string returnString;

        /// <summary>
        /// All content in XML format
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
        /// Initializing global properties and
        /// loading content to main RichTextBox.
        /// </summary>        
        public RegularExpEditor(string ContentToLoad, string localization)
        {
            regulars = new System.Collections.Hashtable();

            numerics = new List<int>();

            this.language = localization;

            successor = new List<string>();

            akt_index = 0;

            InitializeComponent();

            if (ContentToLoad != "")
            LoadFormFromXMLString(ContentToLoad);
        }

        /// <summary>
        /// Standard from_load method.
        /// Setting titles etc.
        /// </summary>        
        private void RegualExpEditor_Load(object sender, EventArgs e)
        {
           // this.ShowInTaskbar = false;
            if (language == "en-US") this.Text = "Regular expressions editor";
            else this.Text = "Editor regulárních výrazů";

            if (language == "en-US")
                 main_toolTip.SetToolTip(TextBox, regular_help_EN);
            else
                 main_toolTip.SetToolTip(TextBox, regular_help_CZ);
        }

        /// <summary>
        /// Standard button_click method.
        /// When user click on OK button of modal successor dialog,
        /// it is necessary to save successor name and variant.
        /// </summary>
        private void OK1_Click(object sender, EventArgs e)
        {
            if (numerics.Count > akt_index)
            {
                numerics.RemoveAt(akt_index);
                successor.RemoveAt(akt_index);
            }

            numerics.Insert(akt_index, (int)numeric.Value);
            successor.Insert(akt_index, (string)edit.Text);
            
            new_form.Close();
        }

        /// <summary>
        /// Standard button_click method.
        /// When user click on Cancel button of modal successor dialog,
        /// must be closed.
        /// </summary>
        private void Cancel1_Click(object sender, EventArgs e)
        {
            new_form.Close();
        }

        /// <summary>
        /// Function creates modal dialog for setting box successor.
        /// </summary>
        /// <param name="regularExp"> Regular expression.</param>
        /// <param name="is_new"> If the dialog with its variable was yet created</param>
        private void CreateForm(String regularExp, bool is_new)
        {
          akt_index = (int)regulars[regularExp];

              new_form = new Form();

              new_form.FormBorderStyle = FormBorderStyle.FixedToolWindow;
              new_form.StartPosition = FormStartPosition.CenterParent;
              new_form.Width = 200;
              new_form.Height = 180;
              if (language == "en-US")
                  new_form.Text = "Setting successors";
              else
                  new_form.Text = "Nastavení pro následníky";
              new_form.MinimizeBox = false;
              new_form.MaximizeBox = false;
              this.AddOwnedForm(new_form);

              ToolTip dialog_toolTip = new ToolTip();
#if MONO
#else
              dialog_toolTip.IsBalloon = true;
#endif

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

              edit = new RichTextBox();

              edit.Left = 35;
              edit.Top = new_form.Size.Height - 150;
              edit.Width = 120;
              edit.Height = 20;
              if (language == "en-US")
                  dialog_toolTip.SetToolTip(edit, successorBox_help_EN);
              else
                  dialog_toolTip.SetToolTip(edit, successorBox_help_CZ);


              if (!is_new)
                  edit.Text = successor[akt_index];

              new_form.Controls.Add(edit);
              edit.Show();

            Label label2 = new Label();
            label2.Left = 35;
            label2.Top = new_form.Size.Height - 115;
            label2.Text = "Next Form box variant:";
            label2.AutoSize = true;
            new_form.Controls.Add(label2);
            label2.Show();

            numeric = new NumericUpDown();

            numeric.Left = 35;
            numeric.Top = new_form.Size.Height - 100;
            numeric.Minimum = -100000;
            numeric.Maximum = 100000;
            if (language == "en-US")
                dialog_toolTip.SetToolTip(numeric, nextVariant_help_EN);
            else
                dialog_toolTip.SetToolTip(numeric, nextVariant_help_CZ);

            if (!is_new)
                  numeric.Value = numerics[akt_index];

            new_form.Controls.Add(numeric);
            numeric.Show();

                       
            Button ok_button = new Button();

            ok_button.Left = new_form.Size.Width / 2 - 80;
            ok_button.Top = new_form.Size.Height - 60;
            ok_button.Text = "OK";
            ok_button.Click += new System.EventHandler(this.OK1_Click);
            new_form.Controls.Add(ok_button);
            ok_button.Show();

            Button cancel_button = new Button();

            cancel_button.Left = new_form.Size.Width / 2 ;
            cancel_button.Top = new_form.Size.Height - 60;
            cancel_button.Text = "Cancel";
            cancel_button.Click += new System.EventHandler(this.Cancel1_Click);
            new_form.Controls.Add(cancel_button);
            cancel_button.Show();

            DialogResult result = new_form.ShowDialog();
        }

        /// <summary>
        /// Standard RichTextBox SelectionChanged function.
        /// When one line is selected CreateForm function is called. 
        /// </summary>
        private void SelectionChanged(object sender, EventArgs e)
        {
            bool stop = false;
            bool is_new = false;
            int index;
            String selected_text = TextBox.SelectedText;

            selected_text.Replace("\n", "");

            for (index = 0; index < TextBox.Lines.Length; index++)
                if ( (selected_text == TextBox.Lines[index]) &&
                      (TextBox.Lines[index] != ""))
                    stop = true;

            if (stop)
            {
                if (!regulars.ContainsKey(selected_text))
                {
                    regulars[selected_text] = regulars.Count;
                    is_new = true;
                }
                CreateForm(selected_text, is_new);
            }
        }

        /// <summary>
        /// Validation of the form content.
        /// Method produces MessageBoxes and not
        /// allow to leave FormEditor when Error.
        /// </summary>
        /// <returns>true if the validation passed good otherwise false</returns>
        private bool Form_validate()
        {
            int i;

            if (TextBox.Lines.Length > regulars.Count)
            {
                MessageBox.Show("Some regular expression has not set successor", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            for (i = 0; i < regulars.Count; i++)
                if ((string)successor[i] == "")
                 {
                    MessageBox.Show("Some form name or form variant are not defined", "Error",
                                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                     return false;
                 }


            return true;
        }

        /// <summary>
        /// If validation passed good,
        /// method save content and close regular expression dialog.
        /// </summary>
        private void OK_Click(object sender, EventArgs e)
        {
            bool validation = Form_validate();

            if (!validation) return;

            this.returnString = SaveFormToXMLString("1");

            this.DialogResult = DialogResult.OK;

            //LoadFormFromXMLString(XMLString);
            this.Dispose();
        }

        /// <summary>
        /// User click on Cancel button the form must be 
        /// closed with Cancel result.
        /// </summary>
        private void Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;

            this.Dispose();
        }

        /// <summary>
        /// Method save content of all forms to one string with
        /// XML format.
        /// </summary>
        /// <param name="FormID"> Globally unique ID of WizardGenereted box - not important</param>
        /// <returns>XML content in string</returns>
        public string SaveFormToXMLString(string FormID)
        {
            int index;
            string XMLStringOutput = "";

            StringWriter XMLstring = new StringWriter();

            XmlTextWriter XMLwriter = new XmlTextWriter(XMLstring);

            XmlDocument form_document = new XmlDocument();

            XmlElement parent_form = form_document.CreateElement("parent"); 

            XmlElement new_form = form_document.CreateElement("Expressions");
            XmlAttribute form_attribute = form_document.CreateAttribute("ID");

            form_attribute.Value = FormID;
            new_form.SetAttributeNode(form_attribute);
            parent_form.AppendChild(new_form);

            for (index = 0; index < TextBox.Lines.Length; index++)
            {
                XmlElement regular_expr = form_document.CreateElement("RegularExpr");
                XmlAttribute regular_expr_attribute = form_document.CreateAttribute("text");

                regular_expr_attribute.Value = TextBox.Lines[index];
                regular_expr.SetAttributeNode(regular_expr_attribute);

                XmlElement successor_name = form_document.CreateElement("successor");
                successor_name.InnerText = (string)successor[index];
                regular_expr.AppendChild(successor_name);

                XmlElement variant_name = form_document.CreateElement("variant");
                variant_name.InnerText = (string)numerics[index].ToString();
                regular_expr.AppendChild(variant_name);


                //regular_expr.InnerText = numerics[index].ToString();
                new_form.AppendChild(regular_expr);
            }

            parent_form.WriteContentTo(XMLwriter);

            XMLStringOutput = XMLstring.ToString();

            XMLwriter.Close();

           return XMLStringOutput;
        }

        /// <summary>
        /// Method load content to regular expressions
        /// editor.
        /// </summary>
        /// <param name="XMLString"> String with XML content</param>
        public void LoadFormFromXMLString(string XMLString)
        {
            int index = 0;
            XmlDocument form_document = new XmlDocument();

            form_document.LoadXml(XMLString);

            XmlNode FormsContent = form_document.DocumentElement;

            string text;

            foreach (XmlNode fils in FormsContent.ChildNodes)
                switch (fils.Name)
                {
                    case "RegularExpr":
                        {
                            text = fils.Attributes.Item(0).Value;
                            regulars[text] = index;
                            if (index != 0) TextBox.Text += "\n";
                            TextBox.Text += text;
                            //numerics.Add(int.Parse(fils.InnerText));
                            index++;

                            foreach (XmlNode prafils in fils.ChildNodes)
                                switch (prafils.Name)
                                {
                                    case "successor":
                                        {
                                            successor.Add(prafils.InnerText);
                                            break;
                                        }
                                    case "variant":
                                        {
                                            numerics.Add(int.Parse(prafils.InnerText));
                                            break;
                                        }
                                }

                            break;
                        }
                }
        }
//END
        /// <summary>
        /// Validation of the form -> each regular expression must have
        /// its own number.
        /// </summary>
        private void TextBox_Validating(object sender, CancelEventArgs e)
        {
        }

    }
}
