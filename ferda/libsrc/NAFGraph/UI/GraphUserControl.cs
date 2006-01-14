#if NAFEnabled
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Netron.GraphLib.UI;
using Netron.NAF.Core.Plugin;
using Netron.NAF.Core.Interfaces;
namespace Netron.GraphLib.UI
{
	/// <summary>
	/// Summary description for GraphUserControl.
	/// </summary>

	public class GraphUserControl : NAFPanel, INAFGraphService 
	{
		[NonSerialized] protected Netron.GraphLib.UI.GraphControl graphControl1;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		protected PointF Center
		{
			get{ return new PointF(this.graphControl1.Width/2,this.graphControl1.Height/2);}
		}

		public GraphUserControl(Netron.NAF.Core.Plugin.NAFRootMediator root)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			
			graphControl1.LoadLibraries();
			this.dockState = NAFDockState.Fill;
			this.panelText = "Grapher";
			this.graphControl1.Root = root;
			
			
			

		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
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
			this.graphControl1 = new Netron.GraphLib.UI.GraphControl();
			this.SuspendLayout();
			// 
			// graphControl1
			// 
			this.graphControl1.AllowDrop = true;
			this.graphControl1.AutomataPulse = 10;
			this.graphControl1.BackgroundColor = System.Drawing.Color.WhiteSmoke;
			this.graphControl1.BackgroundImagePath = null;
			this.graphControl1.BackgroundType = Netron.GraphLib.CanvasBackgroundTypes.FlatColor;
			this.graphControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.graphControl1.EnableContextMenu = true;
			this.graphControl1.EnableLayout = false;
			this.graphControl1.FileName = null;
			this.graphControl1.GradientBottom = System.Drawing.Color.White;
			this.graphControl1.GradientTop = System.Drawing.Color.LightSteelBlue;
			this.graphControl1.GraphLayoutAlgorithm = Netron.GraphLib.GraphLayoutAlgorithms.SpringEmbedder;
			this.graphControl1.Location = new System.Drawing.Point(0, 0);
			this.graphControl1.Name = "graphControl1";
			this.graphControl1.RestrictToCanvas = true;
			this.graphControl1.Size = new System.Drawing.Size(400, 424);
			this.graphControl1.TabIndex = 0;
			this.graphControl1.Text = "graphControl1";
			this.graphControl1.Zoom = 100;
			this.graphControl1.ShowNodeProperties += new Netron.GraphLib.ShowPropsDelegate(this.graphControl_ShowNodeProperties);
			// 
			// GraphUserControl
			// 
			this.Controls.Add(this.graphControl1);
			this.Name = "GraphUserControl";
			this.Size = new System.Drawing.Size(400, 424);
			this.ResumeLayout(false);

		}
		#endregion

		private void graphControl_ShowNodeProperties(PropertyBag props)
		{
			this.mediator.Root.RaiseLoadService(BaseServices.PropertyGridService,"");
			this.mediator.Root.PropertyGrid.SelectedObject = props;
		}
		

		public void StopLayout()
		{
		 this.graphControl1.StopLayout();
		}

		public bool EnableContextMenu
		{
			get
			{
				return this.graphControl1.EnableContextMenu;
				
			}
			set
			{
				this.graphControl1.EnableContextMenu = value;
			}
		}

		public string GraphLayoutAlgorithm
		{
			get
			{				
				return this.graphControl1.GraphLayoutAlgorithm.ToString();
			}
			set
			{
				switch(value)
				{
					case "SpringEmbedder":
						this.graphControl1.GraphLayoutAlgorithm = GraphLayoutAlgorithms.SpringEmbedder;
						break;
					case "Rectangular":
						this.graphControl1.GraphLayoutAlgorithm = GraphLayoutAlgorithms.Rectangular;
						break;

				}
			}
		}

		public void StartLayout()
		{
			this.graphControl1.StopLayout();
		}

		public bool EnableLayout
		{
			get
			{
				
				return this.graphControl1.EnableLayout;
			}
			set
			{
				this.EnableLayout = value;
				
			}
		}

		public new bool Enabled
		{
			get
			{
				
				return this.graphControl1.Enabled ;
			}
			set
			{
				this.graphControl1.Enabled = value;
			}
		}

		public int AutomataPulse
		{
			get
			{
				
				return this.graphControl1.AutomataPulse;
			}
			set
			{
				this.graphControl1.AutomataPulse = value;
			}
		}

		public object AddNode(string shapeType, string label)
		{

			return this.graphControl1.AddNode(ConvertToKnownType(shapeType), label, Center);			
		}
		public object AddNode(string shapeType, string label, PointF position)
		{
			return this.graphControl1.AddNode(ConvertToKnownType(shapeType), label,position);			
		}
		public object AddNode()
		{
			return this.graphControl1.AddNode(BasicShapeTypes.SimpelNode,"[Not set]", Center);						
		}
		public object AddNode(string label)
		{
			return this.graphControl1.AddNode(BasicShapeTypes.SimpelNode,label, Center);						
		}
		public object AddNode(string label ,PointF position)
		{
			return this.graphControl1.AddNode(BasicShapeTypes.SimpelNode, label,position);			
		}
		public object AddNode(PointF position)
		{
			return this.graphControl1.AddNode(BasicShapeTypes.SimpelNode,"[Not set]",position);			
		}

		private BasicShapeTypes ConvertToKnownType(string type)
		{
			switch(type.ToLower())
			{
				case "basicnode": return BasicShapeTypes.BasicNode;
				case "simpelnode": return BasicShapeTypes.SimpelNode;
				case "textlabel": return BasicShapeTypes.TextLabel;
				default:
					throw new Exception("Unknown basic shapes, has to be one of the three basic shapes 'BasicNode', 'SimpelNode' or 'TextLabel'.");
			}
		}

	}
}
#endif