using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.ComponentModel.Design;
namespace Netron.GraphLib.UI
{
	/// <summary>
	/// 
	/// </summary>
	internal class GraphControlDesigner : ControlDesigner 
	{
		public GraphControlDesigner()
		{
			
		}
		
		public override void Initialize(System.ComponentModel.IComponent component)
		{
			base.Initialize (component);
			(component as Control).AllowDrop = false;
			(component as Control).BackColor = Color.Gray;
		}
		
		protected override void OnPaintAdornments(PaintEventArgs pe)
		{
			base.OnPaintAdornments (pe);
			System.Reflection.Assembly ass = System.Reflection.Assembly.GetExecutingAssembly();
			pe.Graphics.DrawString("Netron Graph Control [" + ass.GetName().Version.ToString() + "]",new Font("Verdana",12), Brushes.Red,new PointF(10,10));
			
			
		}


		public override System.ComponentModel.Design.DesignerVerbCollection Verbs
		{
			get
			{
//				return base.Verbs;
				DesignerVerbCollection col=new DesignerVerbCollection();
				col.Add(new DesignerVerb("test",new EventHandler(test)));
				return col;
			}
		}

		private void test(object sender, EventArgs e)
		{
			MessageBox.Show("yep");
		}
	
	}
}
