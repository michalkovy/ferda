using System;
using System.Windows.Forms;
using Netron.GraphLib.Configuration;
namespace Netron.GraphLib.UI
{
	/// <mSummary>
	/// 
	/// </mSummary>
	public class GraphMenuItem : ToolStripMenuItem
	{
		protected ShapeSummary mSummary;

		public ShapeSummary Summary
		{
			get{return mSummary;}
			set{mSummary = value;}
		}
		public GraphMenuItem()
		{
			
		}
		public GraphMenuItem(ShapeSummary mSummary):base()
		{
			this.mSummary = mSummary;
		}

		public GraphMenuItem(ShapeSummary mSummary, EventHandler handler) : base(mSummary.ShapeName, null, handler)
		{
			this.mSummary = mSummary;
		}
	}
}
