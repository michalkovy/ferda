using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ferda.DgFrequencies.NonGUIClasses;
using Ferda.DgFrequencies.Dummy;

namespace Ferda
{
    namespace DgFrequencies
    {
        public partial class MainWindow : UserControl
        {
            FrequenciesBrowser browser;
            
            public MainWindow()
            {
                InitializeComponent();
                InitializeBrowserWithDummy();
                BrowserToList();
            }

            public void InitializeBrowserWithDummy()
            {
                SampleFrequencies sample = new SampleFrequencies();
                browser = new FrequenciesBrowser(sample.GetFrequencies());
                listView1.FullRowSelect = true;
            }

            private void BrowserToList()
            {
                foreach (Frequency frequency in browser)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = frequency.AttributeName;
                    item.SubItems.Add(frequency.Freq.ToString());
                    listView1.Items.Add(item);
                }
            }

        }
    }
}
