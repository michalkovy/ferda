using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Reflection;
using System.Configuration;
using System.IO;
using System.Diagnostics;
using Netron.GraphLib.Attributes;
using Netron.GraphLib.Configuration;

namespace Netron.GraphLib.UI
{
	/// <summary>
	/// Summary description for GraphShapesView.
	/// </summary>
	#if NAFEnabled
	[ToolboxItem(true)]
	public class GraphShapesView : Netron.NAF.Core.Plugin.NAFPanel, ISupportInitialize
	#else
	[ToolboxItem(true)]
	public class GraphShapesView : System.Windows.Forms.UserControl, ISupportInitialize
	#endif
	{		
		/// <summary>
		/// the shape libraries
		/// </summary>
		protected GraphObjectsLibraryCollection libraries;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label DescriptionLabel;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Button ShowListView;
		private System.Windows.Forms.Button ShowTree;
		private System.Windows.Forms.Panel MainPanel;
		private System.Windows.Forms.TreeView treeView;
		private System.Windows.Forms.TabControl tabControl;
		protected ImageList imageList;
		/// <summary>
		/// current mView of the list
		/// </summary>
		protected View mView = View.LargeIcon;
		

		public View View
		{
			get{return mView;}
			set{
				mView = value;
				for(int k = 0 ; k<this.tabControl.TabPages.Count; k++)
				{
					(this.tabControl.TabPages[k] as ShapesTab).View = mView;					
				}
			}
		}
		

		/// <summary>
		/// Adds a shape library to the collection
		/// </summary>
		/// <param name="path"></param>
		public void AddLibrary(string path)
		{
			this.ImportShapes(path);
		}
		#region Constructor
		public GraphShapesView()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			imageList=new ImageList();
			
			this.libraries=new GraphObjectsLibraryCollection();
			
			imageList.Images.Add(this.GetDefaultThumbnail());
			
			#region NAF specific
			//these lines can safely be deleted if you want to use this control outside NAF
			#if NAFEnabled
			this.dockState = Netron.NAF.Core.Plugin.NAFDockState.Right;
			this.panelText = "Shape viewer";
			#endif
			#endregion

		}
		#endregion
		/// <summary>
		/// Loads the shapes from the assembly at the given path
		/// </summary>
		/// <param name="path"></param>
		protected void ImportShapes(string path)
		{
			GraphObjectsLibrary library = new GraphObjectsLibrary();
			ShapeSummary summary;
			ShapesTab tab;
			TreeNode categoryNode;
			library.Path = path;
			libraries.Add(library);
			int currentImageId;
			try
			{
			Assembly ass=Assembly.LoadFrom(path);
			if (ass==null) return;
			Type[] tps=ass.GetTypes();
			
				if (tps==null) return ;
				Shape shapeInstance=null;
				object[] objs;
				for(int k=0; k<tps.Length;k++) //loop over modules in assembly
				{
					
					if(!tps[k].IsClass) continue;		
					objs = tps[k].GetCustomAttributes(typeof(Netron.GraphLib.Attributes.NetronGraphShapeAttribute),false);
					if(objs.Length<1) continue;							
					//now, we are sure to have a shape object					
					
					try
					{
						//normally you'd need the constructor passing the Site but this instance will not actually live on the canvas and hence cause no problem
						//but you do need a ctor with no parameters!
						shapeInstance=(Shape) ass.CreateInstance(tps[k].FullName);
						NetronGraphShapeAttribute shapeAtts = objs[0] as NetronGraphShapeAttribute;
						summary = new ShapeSummary(path, shapeAtts.ShapeKey,shapeAtts.ShapeName, shapeAtts.ShapeCategory, shapeAtts.ReflectionName, shapeAtts.Description);
						library.ShapeSummaries.Add(summary);

						#region For the listview
						tab=this.GetTab(summary.ShapeCategory);

						//if not override the Shape gives a default bitmap
						Bitmap bmp = shapeInstance.GetThumbnail();
						imageList.ImageSize=new Size(48,48);
						imageList.ColorDepth=ColorDepth.Depth8Bit;
						if(bmp !=null)
						{
							
							//imageList.TransparentColor=Color.White;								
							currentImageId=imageList.Images.Add(bmp,Color.Empty);	
						}
						else
						{
							currentImageId = 0;
						}
						//this.pictureBox1.Image=bmp;
						//bmp.Save("c:\\temp\\test.gif",System.Drawing.Imaging.ImageFormat.Gif);
						tab.LargeImageList=imageList;
						tab.AddItem(summary,currentImageId);
						
						#endregion

						#region For the treeview
						categoryNode = this.GetTreeNode(summary.ShapeCategory);
						categoryNode.Nodes.Add(new GraphTreeNode(summary));

						#endregion
					}
					catch(Exception exc)
					{
						Trace.WriteLine(exc.Message);
						continue;
					}						
					
							
				}
				
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.Message);
			}
		}

		public virtual Bitmap GetDefaultThumbnail()
		{
			Stream stream=Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.GraphLib.Resources.UnknownShape.gif");
					
			Bitmap bmp= Bitmap.FromStream(stream) as Bitmap;
			stream.Close();
			stream=null;
			return bmp;
				 
		}
		/// <summary>
		/// Gets a the tabpage with the given name or a new one if it does not yet exist
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		private ShapesTab GetTab(string text)
		{
			ShapesTab tab = null;
			for(int k = 0 ; k<this.tabControl.TabPages.Count; k++)
			{
				tab = this.tabControl.TabPages[k] as ShapesTab;
				if(tab.Text == text) return tab;
			}
			//didn't find it, let's make one
			tab=new ShapesTab(text);
			tab.View = mView;
			this.tabControl.TabPages.Add(tab);
			tab.ShowDescription+=new ItemDescription(ShowDescription);
			return tab;

		}
		private TreeNode GetTreeNode(string text)
		{
			TreeNode node = null;
			for(int k = 0; k<treeView.Nodes.Count; k++)
			{
				if(treeView.Nodes[k].Text == text) return treeView.Nodes[k];
			}
			node = new TreeNode(text);
			this.treeView.Nodes.Add(node);
			return node;

		}
		private ShapesTab AddDummyShapes(ShapesTab tab)
		{
			
			try
			{
				tab.LargeImageList=this.imageList;
				for(int k=0; k<5;k++)
				{
					imageList.ImageSize=new Size(48,48);
					imageList.ColorDepth=ColorDepth.Depth8Bit;
					tab.AddItem("Item" + k);
				}
			}
			catch(Exception exc)
			{
				System.Diagnostics.Trace.WriteLine(exc.Message);
			}
			return tab;
		}


		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.panel1 = new System.Windows.Forms.Panel();
			this.DescriptionLabel = new System.Windows.Forms.Label();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.panel2 = new System.Windows.Forms.Panel();
			this.ShowTree = new System.Windows.Forms.Button();
			this.ShowListView = new System.Windows.Forms.Button();
			this.MainPanel = new System.Windows.Forms.Panel();
			this.treeView = new System.Windows.Forms.TreeView();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.MainPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.DescriptionLabel);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 396);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(304, 100);
			this.panel1.TabIndex = 1;
			// 
			// DescriptionLabel
			// 
			this.DescriptionLabel.BackColor = System.Drawing.SystemColors.Control;
			this.DescriptionLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.DescriptionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DescriptionLabel.Location = new System.Drawing.Point(0, 0);
			this.DescriptionLabel.Name = "DescriptionLabel";
			this.DescriptionLabel.Size = new System.Drawing.Size(304, 100);
			this.DescriptionLabel.TabIndex = 2;
			// 
			// splitter1
			// 
			this.splitter1.Cursor = System.Windows.Forms.Cursors.HSplit;
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.splitter1.Location = new System.Drawing.Point(0, 393);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(304, 3);
			this.splitter1.TabIndex = 2;
			this.splitter1.TabStop = false;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.ShowTree);
			this.panel2.Controls.Add(this.ShowListView);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(304, 32);
			this.panel2.TabIndex = 3;
			// 
			// ShowTree
			// 
			this.ShowTree.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.ShowTree.Location = new System.Drawing.Point(88, 4);
			this.ShowTree.Name = "ShowTree";
			this.ShowTree.TabIndex = 1;
			this.ShowTree.Text = "Tree View";
			this.ShowTree.Click += new System.EventHandler(this.ShowTree_Click);
			// 
			// ShowListView
			// 
			this.ShowListView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.ShowListView.Location = new System.Drawing.Point(8, 4);
			this.ShowListView.Name = "ShowListView";
			this.ShowListView.TabIndex = 0;
			this.ShowListView.Text = "List View";
			this.ShowListView.Click += new System.EventHandler(this.ShowListView_Click);
			// 
			// MainPanel
			// 
			this.MainPanel.Controls.Add(this.treeView);
			this.MainPanel.Controls.Add(this.tabControl);
			this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MainPanel.Location = new System.Drawing.Point(0, 32);
			this.MainPanel.Name = "MainPanel";
			this.MainPanel.Size = new System.Drawing.Size(304, 361);
			this.MainPanel.TabIndex = 4;
			// 
			// treeView
			// 
			this.treeView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeView.HotTracking = true;
			this.treeView.ImageIndex = -1;
			this.treeView.Location = new System.Drawing.Point(0, 0);
			this.treeView.Name = "treeView";
			this.treeView.SelectedImageIndex = -1;
			this.treeView.Size = new System.Drawing.Size(304, 361);
			this.treeView.TabIndex = 7;
			this.treeView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TreeMouseDown);
			this.treeView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TreeMouseMove);
			// 
			// tabControl
			// 
			this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl.Location = new System.Drawing.Point(0, 0);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(304, 361);
			this.tabControl.TabIndex = 6;
			this.tabControl.Visible = false;
			// 
			// GraphShapesView
			// 
			this.Controls.Add(this.MainPanel);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.panel1);
			this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Name = "GraphShapesView";
			this.Size = new System.Drawing.Size(304, 496);
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.MainPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region ISupportInitialize Members

		public void BeginInit()
		{
			// TODO:  Add GraphShapesView.BeginInit implementation
		}

		public void EndInit()
		{
			
		}

		#endregion

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
			if(this.DesignMode)
			{
				this.tabControl.TabPages.Add(AddDummyShapes(new ShapesTab("Dummy")));
			}
			
			
			//this.Invalidate();
		}

		/// <summary>
		/// Loads the shapes from the libraries specified in the application configuration file.
		/// </summary>
		public void LoadLibraries()
		{
			ArrayList graphLibs = ConfigurationSettings.GetConfig("GraphLibs") as ArrayList;
			if(graphLibs.Count>0)
			{
				for(int k=0; k<graphLibs.Count;k++)
				{
					this.ImportShapes(graphLibs[k] as string);
				}
			}
		}

		/// <summary>
		/// Show the shape description in the lower part of the control
		/// </summary>
		/// <param name="message"></param>
		private void ShowDescription(string message)
		{
			this.DescriptionLabel.Text = message;
		}

		private void ShowListView_Click(object sender, System.EventArgs e)
		{
			ShowAs(ShapesView.Icons);
		}

		private void ShowTree_Click(object sender, System.EventArgs e)
		{
			ShowAs(ShapesView.Tree);
		}

		public void ShowAs(ShapesView mView)
		{
			switch(mView)
			{
			
				case ShapesView.Icons:
					this.treeView.Visible=false;
					this.tabControl.Visible = true;
					break;
				case ShapesView.Tree:
					this.treeView.Visible=true;
					this.tabControl.Visible = false;
					break;
			}
		}

		private void TreeMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			TreeNode testitem = this.treeView.GetNodeAt(e.X,e.Y);
			if(testitem != null)			
			{				
				try
				{
					GraphTreeNode item = testitem as GraphTreeNode;
					if(item != null)
					{
						this.DoDragDrop(item.Summary, DragDropEffects.Copy);
					}
				}
				catch(Exception exc)
				{
					Trace.WriteLine(exc.Message);
				}
			}
			//this.Invalidate();
		}

		private void TreeMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			TreeNode testitem = this.treeView.GetNodeAt(e.X,e.Y);
			if(testitem != null)			
			{				
				try
				{
					GraphTreeNode item = testitem as GraphTreeNode;
					if(item != null)
					{
						this.ShowDescription(item.Summary.Description);
					}
				}
				catch(Exception exc)
				{
					Trace.WriteLine(exc.Message);
				}
			}
		}
	}


	
}
