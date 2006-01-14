using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Reflection;
//using System.Text.RegularExpressions;

namespace ContextHelpPreview
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private AxSHDocVw.AxWebBrowser axWebBrowser1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.ToolBar toolBar1;
		private System.Windows.Forms.Button button4;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			String cesta;
			cesta = OnStart(); 
			 

			System.Object nullObject = 0;
			string str = "";
			System.Object nullObjStr = str;
			Cursor.Current = Cursors.WaitCursor;
			

			cesta = System.IO.Path.GetDirectoryName(Application.ExecutablePath);

			cesta = cesta + "\\sample_context_help.htm";
			axWebBrowser1.Navigate(cesta, ref nullObject, ref nullObjStr, ref nullObjStr, ref nullObjStr);
			Cursor.Current = Cursors.Default;
			//this.button2.Enabled=false;
			//this.button3.Enabled=false;

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		/// 

		private static String OnStart()
		{
			String cesta;
			cesta = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
			cesta = cesta + "\\sample_context_help.htm";
			return cesta;
		}

		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Form1));
			this.axWebBrowser1 = new AxSHDocVw.AxWebBrowser();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.toolBar1 = new System.Windows.Forms.ToolBar();
			this.button4 = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.axWebBrowser1)).BeginInit();
			this.SuspendLayout();
			// 
			// axWebBrowser1
			// 
			this.axWebBrowser1.Enabled = true;
			this.axWebBrowser1.Location = new System.Drawing.Point(0, 40);
			this.axWebBrowser1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWebBrowser1.OcxState")));
			this.axWebBrowser1.Size = new System.Drawing.Size(792, 264);
			this.axWebBrowser1.TabIndex = 0;
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(0, 0);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(48, 40);
			this.button2.TabIndex = 3;
			this.button2.Text = "<<<";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(48, 0);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(48, 40);
			this.button3.TabIndex = 4;
			this.button3.Text = ">>>";
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// toolBar1
			// 
			this.toolBar1.DropDownArrows = true;
			this.toolBar1.Location = new System.Drawing.Point(0, 0);
			this.toolBar1.Name = "toolBar1";
			this.toolBar1.ShowToolTips = true;
			this.toolBar1.Size = new System.Drawing.Size(792, 42);
			this.toolBar1.TabIndex = 5;
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(96, 0);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(56, 40);
			this.button4.TabIndex = 6;
			this.button4.Text = "< >";
			this.button4.Click += new System.EventHandler(this.button4_Click_1);
			// 
			// Form1
			// 
			this.AutoScale = false;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(792, 303);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.axWebBrowser1);
			this.Controls.Add(this.toolBar1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(800, 330);
			this.MinimumSize = new System.Drawing.Size(800, 330);
			this.Name = "Form1";
			this.Text = "Ferda Context Help";
			((System.ComponentModel.ISupportInitialize)(this.axWebBrowser1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion


		

		

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		

		

		private void button2_Click(object sender, System.EventArgs e)
		{
			//bool i = false;

			try
			{
				axWebBrowser1.GoBack();
			}
			catch(System.Runtime.InteropServices.COMException)
			{
                            
			}
			finally
			{
				

			}
			
		}

		private void button3_Click(object sender, System.EventArgs e)
		{
			//bool i = false;

			try
			{
				axWebBrowser1.GoForward();
			}
			catch(System.Runtime.InteropServices.COMException)
			{
			}
			finally
			{

			}		
		}

		/*private void button4_Click(object sender, System.EventArgs e)
		{
			 axWebBrowser1.Refresh();
		}*/

		private void button4_Click_1(object sender, System.EventArgs e)
		{
            axWebBrowser1.Refresh();		
		}

		
	}
}
