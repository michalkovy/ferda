/*
 * 
 * Created on 5/10/2004 at 12:02 PM
 *
 * REVISION HISTORY
 *
 * Author		Date		changes
 * GBaseke		5/10/2004 	Initial Revision
 */

using System;
using System.Windows.Forms;
using Guy.Utilities.Reg;

namespace SA
{
	/// <summary>
	/// Description of dsn.	
	/// </summary>
	public class DSN : System.Windows.Forms.Form
	{
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Button btOK;
		private System.Windows.Forms.ImageList imageList;
		private System.Windows.Forms.ColumnHeader hdrName;
		private System.Windows.Forms.Button btCancel;
		private System.Windows.Forms.Label lblServer;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label lblDatabase;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label;
		private System.Windows.Forms.ColumnHeader hdrDriver;
		private System.Windows.Forms.Label lblDSN;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ListView dsnList;
		private System.Windows.Forms.Button btODBC;
		public DSN()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		#region Windows Forms Designer generated code
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DSN));
			this.btODBC = new System.Windows.Forms.Button();
			this.dsnList = new System.Windows.Forms.ListView();
			this.label3 = new System.Windows.Forms.Label();
			this.lblDSN = new System.Windows.Forms.Label();
			this.hdrDriver = new System.Windows.Forms.ColumnHeader();
			this.label = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.lblDatabase = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.lblServer = new System.Windows.Forms.Label();
			this.btCancel = new System.Windows.Forms.Button();
			this.hdrName = new System.Windows.Forms.ColumnHeader();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.btOK = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btODBC
			// 
			this.btODBC.Location = new System.Drawing.Point(368, 320);
			this.btODBC.Name = "btODBC";
			this.btODBC.TabIndex = 10;
			this.btODBC.Text = "ODBC";
			this.btODBC.Click += new System.EventHandler(this.BtODBCClick);
			// 
			// dsnList
			// 
			this.dsnList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
						this.hdrName,
						this.hdrDriver});
			this.dsnList.FullRowSelect = true;
			this.dsnList.GridLines = true;
			this.dsnList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.dsnList.LargeImageList = this.imageList;
			this.dsnList.Location = new System.Drawing.Point(8, 32);
			this.dsnList.MultiSelect = false;
			this.dsnList.Name = "dsnList";
			this.dsnList.Size = new System.Drawing.Size(432, 200);
			this.dsnList.SmallImageList = this.imageList;
			this.dsnList.TabIndex = 1;
			this.dsnList.View = System.Windows.Forms.View.Details;
			this.dsnList.Click += new System.EventHandler(this.OnListItemClicked);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 264);
			this.label3.Name = "label3";
			this.label3.TabIndex = 3;
			this.label3.Text = "Database";
			// 
			// lblDSN
			// 
			this.lblDSN.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblDSN.Location = new System.Drawing.Point(120, 240);
			this.lblDSN.Name = "lblDSN";
			this.lblDSN.Size = new System.Drawing.Size(320, 23);
			this.lblDSN.TabIndex = 5;
			this.lblDSN.Text = "lblDSN";
			// 
			// hdrDriver
			// 
			this.hdrDriver.Text = "Driver";
			this.hdrDriver.Width = 250;
			// 
			// label
			// 
			this.label.Location = new System.Drawing.Point(8, 8);
			this.label.Name = "label";
			this.label.Size = new System.Drawing.Size(112, 23);
			this.label.TabIndex = 0;
			this.label.Text = "System Data Source:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 240);
			this.label2.Name = "label2";
			this.label2.TabIndex = 2;
			this.label2.Text = "DSN";
			// 
			// lblDatabase
			// 
			this.lblDatabase.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblDatabase.Location = new System.Drawing.Point(120, 264);
			this.lblDatabase.Name = "lblDatabase";
			this.lblDatabase.Size = new System.Drawing.Size(320, 23);
			this.lblDatabase.TabIndex = 6;
			this.lblDatabase.Text = "lblDatabse";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 288);
			this.label4.Name = "label4";
			this.label4.TabIndex = 4;
			this.label4.Text = "Server";
			// 
			// lblServer
			// 
			this.lblServer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblServer.Location = new System.Drawing.Point(120, 288);
			this.lblServer.Name = "lblServer";
			this.lblServer.Size = new System.Drawing.Size(320, 23);
			this.lblServer.TabIndex = 7;
			this.lblServer.Text = "lblServer";
			// 
			// btCancel
			// 
			this.btCancel.Location = new System.Drawing.Point(288, 320);
			this.btCancel.Name = "btCancel";
			this.btCancel.TabIndex = 9;
			this.btCancel.Text = "Cancel";
			this.btCancel.Click += new System.EventHandler(this.BtCancelClick);
			// 
			// hdrName
			// 
			this.hdrName.Text = "Name";
			this.hdrName.Width = 175;
			// 
			// imageList
			// 
			this.imageList.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// btOK
			// 
			this.btOK.Location = new System.Drawing.Point(208, 320);
			this.btOK.Name = "btOK";
			this.btOK.TabIndex = 8;
			this.btOK.Text = "OK";
			this.btOK.Click += new System.EventHandler(this.BtOKClick);
			// 
			// DSN
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(448, 349);
			this.Controls.Add(this.btODBC);
			this.Controls.Add(this.btCancel);
			this.Controls.Add(this.btOK);
			this.Controls.Add(this.lblServer);
			this.Controls.Add(this.lblDatabase);
			this.Controls.Add(this.lblDSN);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.dsnList);
			this.Controls.Add(this.label);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DSN";
			this.Text = "Data Source";
			this.Load += new System.EventHandler(this.OnLoad);
			this.ResumeLayout(false);
		}
		#endregion
		void OnLoad(object sender, System.EventArgs e)
		{
			ClearDsnInfo();
			fillList();
		}
		
		private void ClearDsnInfo()
		{
			lblDSN.Text = "";
			lblDatabase.Text = "";
			lblServer.Text = "";
		}
		
		private void fillList()
		{
			string []dsns = ODBCDSN.DsnList(HKEY.LocalMachine);
			string driver;
			string currentDsn = Reg.ReadDsn();
			foreach(string str in dsns)
			{
				ListViewItem item = new ListViewItem();
				item.Text = str;
				driver = ODBCDSN.provider(str, HKEY.LocalMachine);
				
				if ( driver == "SQL Server")
					item.ImageIndex = 1;
				else
					item.ImageIndex = 0;
				item.SubItems.Add(driver);
				
				if ( currentDsn == str )
					item.Selected = true;
				
				dsnList.Items.Add(item);
			}
			OnListItemClicked(null, null);
		}
		
		void OnListItemClicked(object sender, System.EventArgs e)
		{
			ListViewItem item = new ListViewItem();
			if ( dsnList.SelectedItems.Count > 0)
			{
				item = dsnList.SelectedItems[0];
				lblDSN.Text = item.Text;
				lblServer.Text = ODBCDSN.Server( item.Text, HKEY.LocalMachine);
				lblDatabase.Text = ODBCDSN.Database( item.Text, HKEY.LocalMachine);
			}
		}
		
		void BtCancelClick(object sender, System.EventArgs e)
		{
			Close();	
		}
		
		void BtOKClick(object sender, System.EventArgs e)
		{
			string dsn = lblDSN.Text;
			Reg.WriteDsn(dsn);
			Close();	
		}
		
		void BtODBCClick(object sender, System.EventArgs e)
		{
			ODBCCP32 obj = new ODBCCP32();
			if ( !obj.ManageDatasources(this.Handle))
				MessageBox.Show("Error calling ODBCCP32.DLL");
		}
		
	}
}
