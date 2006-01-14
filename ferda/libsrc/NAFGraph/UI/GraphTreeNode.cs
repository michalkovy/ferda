using System;
using System.Windows.Forms;
using Netron.GraphLib.Configuration;
namespace Netron.GraphLib.UI
{
	/// <mSummary>
	/// Summary description for GraphTreeNode.
	/// </mSummary>
	public class GraphTreeNode : TreeNode
	{
		protected ShapeSummary mSummary;
		public GraphTreeNode()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public GraphTreeNode(ShapeSummary mSummary)
		{
			this.mSummary = mSummary;
			this.Text = mSummary.ShapeName;
		}
		public ShapeSummary Summary
		{
			get{return this.mSummary;}
			set{mSummary = value;}
		}
	}
}
