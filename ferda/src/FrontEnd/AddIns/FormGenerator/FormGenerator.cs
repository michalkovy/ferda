//Generator of wizard forms
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
using System.Text;
using System.Data;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Windows.Forms;
//using Ferda.FrontEnd.AddIns.FormEditor;
using Interpreter;


namespace Generator
{
    class WizardFormGenerator : Form
    {
        /// <summary>
        /// Generated form
        /// </summary>
        Form new_form;

        /// <summary>
        /// Example form
        /// </summary>
        Form example_form;

        /// <summary>
        /// XML element that contains content of generated form.
        /// </summary>
        private string XMLContent;

        /// <summary>
        /// Variant of the form Next variant -> Next, STO buttons are present.
        /// OK variant -> OK button is present.
        /// </summary>
        private string form_variant;


        /// <summary>
        /// Maximal number of chars in one line.
        /// </summary>
        private int MaximumLineLength;

        /// <summary>
        /// Instance of class that service variables.
        /// </summary>
        private VariableServices variableService;

        /// <summary>
        /// Struct with pointers and their content.
        /// </summary>
        public struct Directory
        {
            public string pointer;
            public string content;

        }
        /// <summary>
        /// Dictionary of double strings.
        /// </summary>
        Dictionary<int, Directory> pointer_dictionary = new Dictionary<int, Directory>();

        /// <summary>
        /// Class constructor with content XML string.
        /// </summary>
        public WizardFormGenerator(string XMLContent, int MaximumLineLength, string form_variant)
        {
            this.XMLContent = XMLContent;

            this.MaximumLineLength = MaximumLineLength;

            this.form_variant = form_variant;

            this.variableService = new VariableServices();

            this.ParseXMLContent();
        }

        /// <summary>
        /// Function get length of longest line in multiline text.
        /// </summary>
        /// <param name="multiline_text">Input text</param>
        private int FindMaximumLineLength(string multiline_text)
        {
            int maximum_length = 0;

            string[] sub_strings = multiline_text.Split('\n');

            for (int i = 0; i < sub_strings.Length; i++)
                if (sub_strings[i].Length > maximum_length) maximum_length = sub_strings[i].Length;

            return maximum_length;
        }

        /// <summary>
        /// Function get number lines of input multiline text
        /// </summary>
        /// <param name="multiline_text">Input text</param>
        private int GetNumberLines(string multiline_text)
        {
            int number_lines = 0;

            for (int i = 0; i < multiline_text.Length; i++)
                if (multiline_text[i] == '\n') number_lines++;

            return number_lines + 1;
        }

        /// <summary>
        /// Function parse string and set some global parameters.
        /// </summary>
        private void ParseXMLContent()
        {
            XmlDocument form_document = new XmlDocument();
            List<string> XMLString_list = new List<string>();
            List<int> StringNumber_lines = new List<int>();

            int maximumLineLength = 0;
            int index = 0;

            form_document.LoadXml(this.XMLContent);

            XmlNode FormsContent = form_document.DocumentElement;

            int priority;

            foreach (XmlNode fils in FormsContent.ChildNodes)
            {
                int almostMax = 0;
                switch (fils.Name)
                {
                    case "mainarea":
                        {
                            almostMax = FindMaximumLineLength(fils.InnerText);
                            if (almostMax > maximumLineLength)
                                maximumLineLength = almostMax;

                            XMLString_list.Insert(0, fils.InnerText);
                            StringNumber_lines.Insert(0, GetNumberLines(fils.InnerText));

                            break;
                        }
                    case "followchoice":
                        {
                            priority = int.Parse(fils.Attributes.Item(0).Value);
                            almostMax = FindMaximumLineLength(fils.InnerText);
                            if (almostMax > maximumLineLength)
                                maximumLineLength = almostMax;

                            XMLString_list.Insert(priority, fils.InnerText);
                            StringNumber_lines.Insert(priority, GetNumberLines(fils.InnerText));

                            break;
                        }
                    case "pointer":
                        {
                            Directory directory;
                            directory.pointer = fils.Attributes.Item(0).Value;
                            directory.content = fils.InnerText;

                            pointer_dictionary.Add(index, directory);

                            index++;

                            break;
                        }
                }
            }


            for (index = 0; index < pointer_dictionary.Count; index++)
            {
                string identifier = pointer_dictionary[index].pointer;

                if (identifier[0] == '@') continue;

                else if (identifier[0] == '$')
                {

                    /*if (identifier[0] == '^') 
                    {
                        PathInterpreter path = new PathInterpreter(pointer_dictionary[index].content);
                        string[] sss = path.splitPath();
                    }*/

                    WizardLanguageInterpreter interpreter = new WizardLanguageInterpreter(
                    pointer_dictionary[index].content, variableService);

                    Object variable_value = variableService.getValue(identifier);

                    for (int index2 = 0; index2 < XMLString_list.Count; index2++)
                    {
                        XMLString_list[index2] =
                        XMLString_list[index2].Replace(identifier, variable_value.ToString());
                    }
                }
                else if (identifier[0] == '^')
                {
                    for (int index2 = 0; index2 < XMLString_list.Count; index2++)
                    {
                        XMLString_list[index2] =
                        XMLString_list[index2].Replace(identifier, "");
                    }
                }
            }

            for (int index2 = 0; index2 < XMLString_list.Count; index2++)
            {
                XMLString_list[index2] =
                XMLString_list[index2].Replace("@", "");
            }


            //   Repare_texts(ref XMLString_list, ref pointer_dictionary);

            GenerateFormFormXML(XMLString_list, StringNumber_lines, pointer_dictionary, maximumLineLength);
        }

        /// <summary>
        /// Function generate new modal dialog with advices and choices.
        /// </summary>
        /// <param name="XMLString_list">List of strings of all choices</param>
        /// <param name="pointer dictionary">Dictionary with variable identifiers and their body.</param>
        /*private void Repare_texts(List<string> XMLString_list, Dictionary<int, Directory> pointer_dictionary)
                {
                    Object variable_value;

                  //  foreach (Dictionary<int, Directory> dictionary in pointer_dictionary)
                    {

                    }
                }*/

        /// <summary>
        /// Standard LinkClicked function, here we generate
        /// new form with examples.
        /// </summary>
        private void LINK_Click(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Control activeLabel = null;

            foreach (Control LinkLabel in new_form.Controls)
                if (LinkLabel.Equals(sender))
                { activeLabel = LinkLabel; break; }


            String example_content = "";

            for (int i = 0; i < pointer_dictionary.Count; i++)
                if ((pointer_dictionary[i].pointer[0] == '@') &&
                     (pointer_dictionary[i].pointer.Substring(1) == activeLabel.Text))
                {
                    example_content = pointer_dictionary[i].content;
                    break;
                }

            example_form = new Form();

            example_form.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            example_form.StartPosition = FormStartPosition.CenterParent;
            example_form.Width = 210;
            example_form.Height = 300;
            example_form.MinimizeBox = false;
            example_form.MaximizeBox = false;
            example_form.AutoSize = true;
            new_form.AddOwnedForm(example_form);

            RichTextBox richtext_box = new RichTextBox();

            richtext_box.Left = 20;
            richtext_box.Top = 20;
            richtext_box.Width = 170;
            richtext_box.Height = 200;
            richtext_box.Text = example_content.Substring(1);
            richtext_box.ReadOnly = true;
            richtext_box.BackColor = Color.LightYellow;
            richtext_box.IsAccessible = false;
            example_form.Controls.Add(richtext_box);
            richtext_box.Show();

            Button OK_button = new Button();

            OK_button.Left = example_form.Size.Width / 2 - OK_button.Size.Width / 2;
            OK_button.Top = example_form.Size.Height - 60;
            OK_button.Text = "OK";
            OK_button.Click += new System.EventHandler(this.OK_Click);
            example_form.Controls.Add(OK_button);
            OK_button.Show();

            example_form.ShowDialog();
        }

        /// <summary>
        /// Function close dialog with example.
        /// </summary>
        private void OK_Click(object sender, EventArgs e)
        {
            example_form.Close();
        }

        /// <summary>
        /// Function STOP current scenario.
        /// </summary>
        private void STOP_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;

            this.Dispose();

            //this.Close();
        }

        /// <summary>
        /// Function jump to the next form in scenario. 
        /// </summary>
        private void Next_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;

            this.Dispose();

            //new_form.Close();
        }

        /// <summary>
        /// Function generate new modal dialog with advices and choices.
        /// </summary>
        /// <param name="XMLString_list">List of strings of all choices</param>
        /// <param name="StringNumber_lines">Number lines of each string.</param>
        /// <param name="maximumLineLength">Maximum line length.</param>
        public void GenerateFormFormXML(List<string> XMLString_list, List<int> StringNumber_lines,
                                        Dictionary<int, Directory> pointer_dictionary, int maximumLineLength)
        {
            int StandardFontWidth;
            int plannedWidth;
            int plannedHeigth;
            int choices_heigth = 0;

            new_form = new Form();

            plannedWidth = 20 + maximumLineLength * new_form.Font.Height / 2;
            plannedHeigth = StringNumber_lines[0] * new_form.Font.Height + 10;

            for (int i = 1; i < XMLString_list.Count; i++)
                if (i == 1) choices_heigth = new_form.Font.Height * StringNumber_lines[i];
                else choices_heigth += 20 + new_form.Font.Height * StringNumber_lines[i];

            plannedHeigth += choices_heigth + 10 + 50;

            new_form.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            new_form.StartPosition = FormStartPosition.CenterParent;
            new_form.Width = Math.Max(250, plannedWidth);
            new_form.Height = Math.Max(200, plannedHeigth);
            new_form.MinimizeBox = false;
            new_form.MaximizeBox = false;
            new_form.AutoSize = true;
            StandardFontWidth = new_form.Font.Height;
            this.AddOwnedForm(new_form);

            Label main_label = new Label();

            main_label.Top = 20;
            main_label.Left = 10;
            main_label.AutoSize = true;
            main_label.Text = XMLString_list[0];
            main_label.TextAlign = ContentAlignment.BottomLeft;
            main_label.Visible = true;
            new_form.Controls.Add(main_label);
            main_label.Show();


            for (int i = 1; i < XMLString_list.Count; i++)
            {
                Label choice_label = new Label();

                choice_label.Top = main_label.Top + main_label.Size.Height + 10 + (i - 1) * 20;
                choice_label.Left = 30;
                // choice_label.BackColor = Color.Red;
                choice_label.AutoSize = true;
                choice_label.Text = XMLString_list[i];
                choice_label.TextAlign = ContentAlignment.BottomLeft;
                choice_label.Visible = true;
                new_form.Controls.Add(choice_label);
                choice_label.Show();

                RadioButton choice = new RadioButton();

                choice.Top = main_label.Top + main_label.Size.Height + 10 + (i - 1) * 20;
                choice.Left = 10;
                choice.Text = "";
                choice.Appearance = Appearance.Normal;
                if (i == 1) choice.Checked = true;
                choice.Width = 15;
                choice.Height = 15;
                choice.AutoSize = false;
                new_form.Controls.Add(choice);
            }

            int counter = 0;

            LinkLabel example_copy = new LinkLabel();

            for (int i = 0; i < pointer_dictionary.Count; i++)
                if (pointer_dictionary[i].pointer[0] == '@')
                {
                    LinkLabel example = new LinkLabel();

                    example.Top = new_form.Size.Height - 75;
                    if (counter == 0) example.Left = 20;
                    else example.Left = example_copy.Left + example_copy.Width + 5;
                    example.Text = pointer_dictionary[i].pointer.Substring(1);
                    example.AutoSize = true;
                    example.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LINK_Click);
                    new_form.Controls.Add(example);
                    example.Show();

                    example_copy = example;
                    counter++;
                }

            if (this.form_variant == "OK")
            {
                Button OK_button = new Button();

                OK_button.Left = new_form.Size.Height - 45;
                OK_button.Top = new_form.Size.Height - 60;
                OK_button.Text = "OK";
                OK_button.BackColor = Color.Green;
                OK_button.Click += new System.EventHandler(this.STOP_Click);
                new_form.Controls.Add(OK_button);
                OK_button.Show();

            }
            else
            {
                Button next_button = new Button();

                next_button.Left = new_form.Size.Height - 85;
                next_button.Top = new_form.Size.Height - 60;
                next_button.Text = "Next >>";
                next_button.BackColor = Color.Green;
                next_button.Click += new System.EventHandler(this.Next_Click);
                new_form.Controls.Add(next_button);
                next_button.Show();

                Button STOP_button = new Button();

                STOP_button.Left = new_form.Size.Height - 5;
                STOP_button.Top = new_form.Size.Height - 60;
                STOP_button.Text = "STOP";
                STOP_button.BackColor = Color.Red;
                STOP_button.Click += new System.EventHandler(this.STOP_Click);
                new_form.Controls.Add(STOP_button);
                STOP_button.Show();
            }

            new_form.ShowDialog();

        }

    }
}
