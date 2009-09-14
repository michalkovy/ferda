using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
        static string[] hosts = new string[3] { "http://sewebar-dev.vse.cz/xmlrpc/", 
            "http://sewebar.vse.cz/tinnitus/xmlrpc/", 
            "http://sewebar.vse.cz/cardio/xmlrpc/" };

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

            this.ResumeLayout(false);
            this.PerformLayout();
        }


    }
}
