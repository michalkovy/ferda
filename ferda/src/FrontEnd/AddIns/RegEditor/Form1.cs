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
    public partial class RegularExpEditor : Form
    {
        /// <summary>
        /// Global numericUpDown.
        /// </summary>        
        NumericUpDown numeric;

        /// <summary>
        /// Global new form variable.
        /// </summary>        
        private Form new_form;

        /// <summary>
        /// List of variant shortcuts.
        /// </summary>        
       // private int[] numerics = new int[20];
        private List<int> numerics;

        /// <summary>
        /// Assotiative array of regular expressions.
        /// </summary>        
        private System.Collections.Hashtable regulars;

        /// <summary>
        /// index of active dialog.
        /// </summary>
        private int akt_index;

        /// <summary>
        /// toolTip text for main RichTextBox.
        /// </summary>
        private string regular_help = @"Here insert set of regular expressions. For example :
        Each expression must be in different line.
        ";

        /// <summary>
        /// Class constructor.
        /// </summary>        
        public RegularExpEditor(string ContentToLoad)
        {
            regulars = new System.Collections.Hashtable();

            numerics = new List<int>();

            akt_index = 0;

            InitializeComponent();

            //if (ContentToLoad != "")
            //LoadFormFromXMLString(ContentToLoad);
        }

        /// <summary>
        /// Standard from load function.
        /// </summary>        
        private void RegualExpEditor_Load(object sender, EventArgs e)
        {
            main_toolTip.SetToolTip(TextBox, regular_help);
        }

        /// <summary>
        /// OK button click in modal dialog.
        /// </summary>
        private void OK1_Click(object sender, EventArgs e)
        {
            if (numerics.Count > akt_index)
                        numerics.RemoveAt(akt_index);
            
            numerics.Insert(akt_index, (int)numeric.Value);
            
            new_form.Close();
        }
        /// <summary>
        /// Function creates modal dialog with NumericUpDown.
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
              new_form.Height = 110;
              new_form.MinimizeBox = false;
              new_form.MaximizeBox = false;
              this.AddOwnedForm(new_form);

            numeric = new NumericUpDown();

            numeric.Left = new_form.Size.Height / 2 - 20;
            numeric.Top = new_form.Size.Height - 90;
            numeric.Minimum = 0;
            numeric.Maximum = 100000;

            if (!is_new)
                  numeric.Value = numerics[akt_index];

            new_form.Controls.Add(numeric);
            numeric.Show();

            Button ok_button = new Button();

            ok_button.Left = new_form.Size.Height / 2;
            ok_button.Top = new_form.Size.Height - 60;
            ok_button.Text = "OK";
            ok_button.Click += new System.EventHandler(this.OK1_Click);
            new_form.Controls.Add(ok_button);
            ok_button.Show();

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
        /// Function save content when user clicks on OK button.
        /// </summary>
        private void OK_Click(object sender, EventArgs e)
        {
            string XMLString = SaveFormToXMLString("1");

           // MessageBox.Show(XMLString);

             this.Close();
        }

        /// <summary>
        /// User click on Cancel button -> only close form.
        /// </summary>
        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Function save content of all forms to one string with
        /// XML format.
        /// </summary>
        /// <param name="FormID"> Globally unique ID of WizardGenereted box.</param>
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
                regular_expr.InnerText = numerics[index].ToString();
                new_form.AppendChild(regular_expr);
            }

            parent_form.WriteContentTo(XMLwriter);

            XMLStringOutput = XMLstring.ToString();

            XMLwriter.Close();

           return XMLStringOutput;
        }

        /// <summary>
        /// Function load informations to generated wizard from string
        /// with XML format.
        /// </summary>
        /// <param name="XMLString"> String with XML content.</param>
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
                            numerics[index] = int.Parse(fils.InnerText);
                            index++;

                            break;
                        }
                }
        }

        /// <summary>
        /// Validation of the form -> each regular expression must have
        /// its own number.
        /// </summary>
        private void TextBox_Validating(object sender, CancelEventArgs e)
        {
             if (TextBox.Lines.Length > regulars.Count)
              {
                 e.Cancel = true;
                 MessageBox.Show("Too little parametress are set !!!", "Error");
              }
        }

    }
}


/*       private void WriteChanges()
       {
           int index;
           String ID = "3";

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
           XmlElement new_form = form_document.CreateElement("Expressions");
           XmlAttribute form_attribute = form_document.CreateAttribute("ID");

           form_attribute.Value = ID;
           new_form.SetAttributeNode(form_attribute);

           for (index = 0; index < TextBox.Lines.Length; index++)
           {
               XmlElement regular_expr = form_document.CreateElement("RegularExpr");
               XmlAttribute regular_expr_attribute = form_document.CreateAttribute("text");

               regular_expr_attribute.Value = TextBox.Lines[index];
               regular_expr.SetAttributeNode(regular_expr_attribute);
               regular_expr.InnerText = numerics[index].ToString();
               new_form.AppendChild(regular_expr);
           }

           form_document.DocumentElement.InsertAfter(new_form,
                                  form_document.DocumentElement.LastChild);

           form_document.WriteContentTo(writer);

           writer.Close();
 * 
 * 
 *         private void LoadDocument(String File, String form_identifier)
        {
          int index = 0;
          XmlDocument form_document = new XmlDocument();

            form_document.Load(File);

            XmlNode FormsContent = form_document.DocumentElement;

            foreach (XmlNode forms in FormsContent.ChildNodes)
                if (forms.Attributes.Item(0).Value == form_identifier)
                {
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
                                    numerics[index] =int.Parse(fils.InnerText);
                                    index++;

                                    break;
                                }
                        }
               //}
        }

       }*/

