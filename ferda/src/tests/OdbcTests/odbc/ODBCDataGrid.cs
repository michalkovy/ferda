using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.Odbc;
using System.Text.RegularExpressions;

namespace odbc
{
	/// <summary>
	/// Summary description for ODBCDataGrid.
	/// </summary>
	public class ODBCDataGrid : System.Windows.Forms.Form
	{
		private System.Windows.Forms.DataGrid dataGrid;
		private System.Windows.Forms.Button buttonOK;
		private ComboBox comboBox1;
		private SimpleDatabaseLayer MyODBC;
		private Label label1;
		private TextBox Min;
		private Label label7;
		private TextBox Rows;
		private Label label3;
		private TextBox Max;
		private Label label8;
		private TextBox Median;
		private Label label5;
		private TextBox Avg;
		private Label label9;
		private TextBox Variability;
		private DataGrid dataGridFreq;
		private Label label2;
		private TextBox DataType;
		private Label label4;
		private TextBox Distincts;
		private Label label6;
		private TextBox StandardDeviation;
		private RadioButton SysTables;
		private RadioButton SysColumns;
		private RadioButton Star;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ODBCDataGrid()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.dataGrid = new System.Windows.Forms.DataGrid();
			this.buttonOK = new System.Windows.Forms.Button();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.Min = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.Rows = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.Max = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.Median = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.Avg = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.Variability = new System.Windows.Forms.TextBox();
			this.dataGridFreq = new System.Windows.Forms.DataGrid();
			this.label2 = new System.Windows.Forms.Label();
			this.DataType = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.Distincts = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.StandardDeviation = new System.Windows.Forms.TextBox();
			this.SysTables = new System.Windows.Forms.RadioButton();
			this.SysColumns = new System.Windows.Forms.RadioButton();
			this.Star = new System.Windows.Forms.RadioButton();
			((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dataGridFreq)).BeginInit();
			this.SuspendLayout();
			// 
			// dataGrid
			// 
			this.dataGrid.DataMember = "";
			this.dataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGrid.Location = new System.Drawing.Point(0, 0);
			this.dataGrid.Name = "dataGrid";
			this.dataGrid.Size = new System.Drawing.Size(405, 200);
			this.dataGrid.TabIndex = 0;
			// 
			// buttonOK
			// 
			this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonOK.Location = new System.Drawing.Point(519, 282);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(75, 23);
			this.buttonOK.TabIndex = 1;
			this.buttonOK.Text = "OK";
			this.buttonOK.Click += new System.EventHandler(this.button1_Click);
			// 
			// comboBox1
			// 
			this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Items.AddRange(new object[] {
            "DSN=LM LM Barbora.mdb Metabase;",
            "DSN=LM Barbora.mdb;"});
			this.comboBox1.Location = new System.Drawing.Point(12, 205);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(582, 21);
			this.comboBox1.TabIndex = 2;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 236);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(20, 13);
			this.label1.TabIndex = 6;
			this.label1.Text = "Min";
			// 
			// Min
			// 
			this.Min.Location = new System.Drawing.Point(78, 233);
			this.Min.Name = "Min";
			this.Min.ReadOnly = true;
			this.Min.Size = new System.Drawing.Size(100, 20);
			this.Min.TabIndex = 5;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(12, 262);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(30, 13);
			this.label7.TabIndex = 8;
			this.label7.Text = "Rows";
			// 
			// Rows
			// 
			this.Rows.Location = new System.Drawing.Point(78, 259);
			this.Rows.Name = "Rows";
			this.Rows.ReadOnly = true;
			this.Rows.Size = new System.Drawing.Size(100, 20);
			this.Rows.TabIndex = 7;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(187, 236);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(23, 13);
			this.label3.TabIndex = 10;
			this.label3.Text = "Max";
			// 
			// Max
			// 
			this.Max.Location = new System.Drawing.Point(236, 233);
			this.Max.Name = "Max";
			this.Max.ReadOnly = true;
			this.Max.Size = new System.Drawing.Size(100, 20);
			this.Max.TabIndex = 9;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(187, 262);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(31, 13);
			this.label8.TabIndex = 12;
			this.label8.Text = "label8";
			// 
			// Median
			// 
			this.Median.Location = new System.Drawing.Point(236, 259);
			this.Median.Name = "Median";
			this.Median.ReadOnly = true;
			this.Median.Size = new System.Drawing.Size(100, 20);
			this.Median.TabIndex = 11;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(347, 236);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(22, 13);
			this.label5.TabIndex = 14;
			this.label5.Text = "Avg";
			// 
			// Avg
			// 
			this.Avg.Location = new System.Drawing.Point(411, 233);
			this.Avg.Name = "Avg";
			this.Avg.ReadOnly = true;
			this.Avg.Size = new System.Drawing.Size(100, 20);
			this.Avg.TabIndex = 13;
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(347, 262);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(47, 13);
			this.label9.TabIndex = 16;
			this.label9.Text = "Variability";
			// 
			// Variability
			// 
			this.Variability.Location = new System.Drawing.Point(411, 259);
			this.Variability.Name = "Variability";
			this.Variability.ReadOnly = true;
			this.Variability.Size = new System.Drawing.Size(100, 20);
			this.Variability.TabIndex = 15;
			// 
			// dataGridFreq
			// 
			this.dataGridFreq.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.dataGridFreq.DataMember = "";
			this.dataGridFreq.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGridFreq.Location = new System.Drawing.Point(411, 0);
			this.dataGridFreq.Name = "dataGridFreq";
			this.dataGridFreq.Size = new System.Drawing.Size(265, 200);
			this.dataGridFreq.TabIndex = 18;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.label2.Location = new System.Drawing.Point(347, 288);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(58, 13);
			this.label2.TabIndex = 24;
			this.label2.Text = "DataType";
			// 
			// DataType
			// 
			this.DataType.Location = new System.Drawing.Point(411, 285);
			this.DataType.Name = "DataType";
			this.DataType.ReadOnly = true;
			this.DataType.Size = new System.Drawing.Size(100, 20);
			this.DataType.TabIndex = 23;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(187, 288);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(43, 13);
			this.label4.TabIndex = 22;
			this.label4.Text = "Distincts";
			// 
			// Distincts
			// 
			this.Distincts.Location = new System.Drawing.Point(236, 285);
			this.Distincts.Name = "Distincts";
			this.Distincts.ReadOnly = true;
			this.Distincts.Size = new System.Drawing.Size(100, 20);
			this.Distincts.TabIndex = 21;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(12, 288);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(60, 13);
			this.label6.TabIndex = 20;
			this.label6.Text = "Stand. Dev.";
			// 
			// StandardDeviation
			// 
			this.StandardDeviation.Location = new System.Drawing.Point(78, 285);
			this.StandardDeviation.Name = "StandardDeviation";
			this.StandardDeviation.ReadOnly = true;
			this.StandardDeviation.Size = new System.Drawing.Size(100, 20);
			this.StandardDeviation.TabIndex = 19;
			// 
			// SysTables
			// 
			this.SysTables.AutoSize = true;
			this.SysTables.Checked = true;
			this.SysTables.Location = new System.Drawing.Point(13, 311);
			this.SysTables.Name = "SysTables";
			this.SysTables.Size = new System.Drawing.Size(70, 17);
			this.SysTables.TabIndex = 25;
			this.SysTables.TabStop = false;
			this.SysTables.Text = "SysTables";
			// 
			// SysColumns
			// 
			this.SysColumns.AutoSize = true;
			this.SysColumns.Location = new System.Drawing.Point(89, 311);
			this.SysColumns.Name = "SysColumns";
			this.SysColumns.Size = new System.Drawing.Size(78, 17);
			this.SysColumns.TabIndex = 26;
			this.SysColumns.Text = "SysColumns";
			// 
			// Star
			// 
			this.Star.AutoSize = true;
			this.Star.Location = new System.Drawing.Point(173, 311);
			this.Star.Name = "Star";
			this.Star.Size = new System.Drawing.Size(40, 17);
			this.Star.TabIndex = 28;
			this.Star.Text = "Star";
			// 
			// ODBCDataGrid
			// 
			this.ClientSize = new System.Drawing.Size(688, 345);
			this.Controls.Add(this.Star);
			this.Controls.Add(this.SysColumns);
			this.Controls.Add(this.SysTables);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.DataType);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.Distincts);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.StandardDeviation);
			this.Controls.Add(this.dataGridFreq);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.Variability);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.Avg);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.Median);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.Max);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.Rows);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.Min);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.dataGrid);
			this.Name = "ODBCDataGrid";
			this.Text = "ODBCDataGrid";
			((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dataGridFreq)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void button1_Click(object sender, System.EventArgs e)
		{
			int selectedIndex = this.comboBox1.SelectedIndex;
			Object selectedItem = this.comboBox1.SelectedItem;
			string connectionString = null;
			if (selectedItem != null)
				connectionString = selectedItem.ToString();

			int p = 0;
			p++;

			//"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=E:\Saves\Projekt\svn\bin\MetabaseLayer\DB\9dae03fe-64b2-4ddd-bc28-df4574a7ee16LISpMinerMetabaseEmpty.mdb;User Id=admin;Password=;"
			//"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=E:\\Saves\\Projekt\\svn\\bin\\MetabaseLayer\\DB\\9dae03fe-64b2-4ddd-bc28-df4574a7ee16LISpMinerMetabaseEmpty.mdb;User Id=admin;Password=;"

			/*
			 * Access
			 * * ODBC
			 * * * Standard Security:
			 * * * * "Driver={Microsoft Access Driver (*.mdb)};Dbq=C:\mydatabase.mdb;Uid=Admin;Pwd=;"
			 * * * Workgroup:
			 * * * * "Driver={Microsoft Access Driver (*.mdb)};Dbq=C:\mydatabase.mdb;SystemDB=C:\mydatabase.mdw;"
			 * * * Exclusive:
			 * * * * "Driver={Microsoft Access Driver (*.mdb)};Dbq=C:\mydatabase.mdb;Exclusive=1;Uid=admin;Pwd="
			 * *

			string cs1 = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=E:\\Saves\\Projekt\\svn\\bin\\MetabaseLayer\\DB\\9dae03fe-64b2-4ddd-bc28-df4574a7ee16LISpMinerMetabaseEmpty.mdb;User Id=admin;Password=;";
			string path = "E:\\Saves\\Projekt\\svn\\bin\\MetabaseLayer\\DB\\LISpMinerMetabaseEmpty.mdb";
			string cs2 = "Driver={Microsoft Access Driver (*.mdb)};Dbq=" + path + ";Exclusive=1;Uid=admin;Pwd=";
			System.Data.Odbc.OdbcConnection c = 
				new System.Data.Odbc.OdbcConnection(cs2);
			c.Open();
			 */
			/**
			string mySelectQuery = "SELECT loans_id FROM Loans GROUP BY loans_id HAVING COUNT(*) > 1";
			System.Data.Odbc.OdbcConnection conn = new System.Data.Odbc.OdbcConnection("DSN=LM Barbora.mdb");
			conn.Open();
			OdbcDataAdapter myDataAdapter = new OdbcDataAdapter(mySelectQuery, conn);
			System.Data.DataSet myDataSet = new System.Data.DataSet();
			bool result = false;
			try
			{
				myDataAdapter.Fill(myDataSet);
				result = !(myDataSet.Tables[0].Rows.Count > 1);
			}
			catch (OdbcException) { }
			System.Windows.Forms.MessageBox.Show(result.ToString());
			return;
			/**/

			/**
			int[][] myArray = new int[2][];
			myArray[0] = new int[] { 1, 2, 3 };
			myArray[1] = new int[] { 4, 5, 6 };

			string mess = "myArray:";
			mess += "\n GetLength(0)" + myArray.GetLength(0).ToString();
			//mess += "\n GetLength(1)" + myArray.GetLength(1).ToString();
			mess += "\n Length" + myArray.Length;
			mess += "myArray[0]:";
			mess += "\n GetLength(0)" + myArray[0].GetLength(0).ToString();
			mess += "\n Length" + myArray[0].Length;
			System.Windows.Forms.MessageBox.Show(mess);
			return;
			/**/

			/**
			int[,] myArray = new int[3,2];
			myArray[0, 0] = 0;
			myArray[0, 1] = 1;
			myArray[1, 0] = 2;
			myArray[1, 1] = 3;
			myArray[2, 0] = 4;
			myArray[2, 1] = 5;
			string mess = "myArray:";
			mess += "\n GetLength(0)" + myArray.GetLength(0).ToString();
			mess += "\n GetLength(1)" + myArray.GetLength(1).ToString();
			mess += "\n Length" + myArray.Length;
			System.Windows.Forms.MessageBox.Show(mess);
			return;
			/**/

			/**/
			Regex r = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
			string s = "\"a\",b,\"c, d, e\",\\,f";
			string[] sAry = r.Split(s);
			for (int i = 0; i < sAry.Length; i++)
			{
				Console.WriteLine(sAry[i]);
			}
/*
			Regex r = new Regex(",(?=(?:( \")*\"(\")*\")*(?!(\")*\"))");
			//r = new Regex(",(?=(?:[^\]*\[^\]*\)*(?![^\]*\))");
			string s = "\\a\\,b,\\c, d, e\\,,f";
			string[] sAry = r.Split(s);
			foreach (string val in sAry)
			{
				Console.WriteLine(val);
			}
			/**/

			try
			{
				string tableName = "Loans";
				string table = tableName;
				this.MyODBC = null;
				this.MyODBC = new SimpleDatabaseLayer(connectionString);
				if (connectionString != null)
				{
					if (this.SysColumns.Checked)
					{
						this.dataGrid.CaptionText = "SysColumns";
						this.dataGrid.DataSource = this.MyODBC.QuerySyscolumns(tableName);
					}
					else if (this.SysTables.Checked)
					{
						this.dataGrid.CaptionText = "SysTables";
						this.dataGrid.DataSource = this.MyODBC.QuerySystables();
					}
					else if (this.Star.Checked)
					{
						this.dataGrid.CaptionText = "SELECT * FROM " + tableName;
						this.dataGrid.DataSource = this.MyODBC.SelectAllModuleForInteraction(tableName);
					}
					string column;
					//column = "District";
					column = "Salary";
					/**/
					this.MyODBC.TestPrimaryKeysUnique(table, new string[] { "loan_id" });
					this.MyODBC.TestPrimaryKeysUnique(table, new string[] { "District" });
					this.MyODBC.TestPrimaryKeysUnique(table, new string[0]);
					this.MyODBC.TestPrimaryKeysUnique(table, new string[] { "District", "birth_number", "amount" });
					this.MyODBC.TestPrimaryKeysUnique(table, new string[] { "payments", "duration", "status" });
					this.MyODBC.TestPrimaryKeysUnique(table, new string[] { "District", "birth_number", "amount", "payments", "duration", "status" });
					this.MyODBC.TestPrimaryKeysUnique(table, new string[] { "loan_id", "birth_number" });
					/**/
					this.MyODBC.TablesModuleForInteraction();
					this.MyODBC.ColumnsModuleForInteraction("Loans");
					this.DataType.Text = this.MyODBC.GetColumnSubType(table, column);
					this.Min.Text = this.MyODBC.QuerySelectMin(table, column).ToString();
					this.Max.Text = this.MyODBC.QuerySelectMax(table, column).ToString();
					this.Avg.Text = this.MyODBC.QuerySelectAvg(table, column).ToString();
					this.Rows.Text = this.MyODBC.QueryRowCount(table).ToString();
					this.Distincts.Text = this.MyODBC.QuerySelectDistincts(table, column).ToString();
					this.Variability.Text = this.MyODBC.QuerySelectVariability(table, column).ToString("F");
					this.StandardDeviation.Text = this.MyODBC.QuerySelectStandardDeviation(table, column).ToString("F");
					this.dataGridFreq.CaptionText = column + " frequencies";
					this.dataGridFreq.DataSource = this.MyODBC.ColumnFrequenciesModuleForInteraction(table, column);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}
	}
}
