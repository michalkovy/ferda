using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ferda.ShowTable.NonGUIClasses;
using Ferda.ShowTable.Dummy;

namespace Ferda
{
    namespace ShowTable.GUIClasses
    {
        public partial class MainWindow : UserControl
        {
            public MainWindow()
            {
                InitializeComponent();
                GenerateStrings dummy = new GenerateStrings(20,20);
                MakeDataTable newTable = new MakeDataTable(dummy.GetStrings());
                DataTable myTable = newTable.GetDataTable();
                DataView myView = new DataView(myTable);
                this.dataGridView1.DataSource = myView;
                DataGrid myGrid = new DataGrid();
                myGrid.DataSource = myView;
                this.dataGridView1.DataSource = myView;
             //   this.dataGridView1.SetDataBinding();
                this.dataGridView1.Refresh();

            }
        }
    }
}
