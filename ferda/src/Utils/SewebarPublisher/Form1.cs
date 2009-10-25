using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sewebar;

namespace SewebarPublisher
{
    /// <summary>
    /// The main form class of the SEWEBAR Publisher component. 
    /// It contains only the non-generated code. 
    /// </summary>
    public partial class SEWEBARForm : Form
    {
        /// <summary>
        /// A static array of strings containing the possible values of XML-RPC
        /// hosts including the URL of the XMLRPC server side service,
        /// full path to the service (including the directories)
        /// </summary>
        static string[] hosts = new string[3] { "http://sewebar.vse.cz/adamek/xmlrpc/",
            "http://sewebar-dev.vse.cz/xmlrpc/", 
            "http://sewebar.vse.cz/tinnitus/xmlrpc/"  };

        /// <summary>
        /// The constructor
        /// </summary>
        public SEWEBARForm()
        {
            InitializeComponent();

            //adding the hosts and selecting the index
            foreach (string s in hosts)
            {
                CBXMLRPCHost.Items.Add(s);
            }
            CBXMLRPCHost.SelectedIndex = 0;

            //adding the name and password of the trial student (can be removed)
            //TBUserName.Text = "admin";
            //TBPassword.Text = "studentFIS";

            LVArticles.Columns.Add("Article ID", -2, HorizontalAlignment.Left);
            LVArticles.Columns.Add("Article title", -2, HorizontalAlignment.Left);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        /// <summary>
        /// The event lists files of a particular user and fills them into 
        /// the list view.
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        private void BListFiles_Click(object sender, EventArgs e)
        {
            LVArticles.Items.Clear();

            try
            {
                IDictionary<int, string> files =
                    Sewebar.Sewebar.ListFiles(
                        CBXMLRPCHost.SelectedItem.ToString(),
                        TBUserName.Text, TBPassword.Text);

                foreach (int key in files.Keys)
                {
                    ListViewItem item = new ListViewItem(key.ToString());
                    item.SubItems.Add(files[key]);
                    LVArticles.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }

        /// <summary>
        /// The event closes the form
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        private void BCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// The event publishes the content of the clipboard to SEWEBAR
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        private void BPublish_Click(object sender, EventArgs e)
        {
            if (!Clipboard.ContainsText())
            {
                MessageBox.Show("The system clipboard does not contain text","Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
                return;
            }

            string pmml = Clipboard.GetText();

            if (LVArticles.Items.Count == 0 && TBNewArticle.Text == string.Empty)
            {
                MessageBox.Show("No article is selected","Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
                return;
            }

            if (LVArticles.SelectedItems.Count == 0 && TBNewArticle.Text == string.Empty)
            {
                MessageBox.Show("No article is selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //retrieving the ID of the article to publish
            int articleID = -1;
            if (TBNewArticle.Text == string.Empty)
            {
                articleID = Convert.ToInt32(LVArticles.SelectedItems[0].Text);
            }

            //retrieving the title of the article to publish
            string articleTitle = TBNewArticle.Text;
            if (articleTitle == string.Empty)
            {
                articleTitle = LVArticles.SelectedItems[0].SubItems[1].Text;
            }

            string response = null;
            try
            {
                response = Sewebar.Sewebar.PublishToSewebar(
                    CBXMLRPCHost.SelectedItem.ToString(),
                    pmml,
                    TBUserName.Text,
                    TBPassword.Text,
                    articleTitle,
                    articleID);
            }
            catch (Exception ex)
            {
                MessageBox.Show("File upload unsuccessfull\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            MessageBox.Show(response, "Response");
            this.Close();
        }


    }
}
