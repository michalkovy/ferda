using System;
using Netron.GraphLib.UI;
using Netron.GraphLib.Configuration;
using System.Windows.Forms;
namespace Netron.GraphLib.UI
{
	/// <mSummary>
	/// 
	/// </mSummary>
	public class ShapesTabItem : ListViewItem
	{
		public ShapesTabItem()
		{
			this.ImageIndex = 0; //there is a default image in slot 0
		}

		public ShapesTabItem(ShapeSummary mSummary, int imageIndex)
		{
			this.mSummary=mSummary;
			this.ImageIndex=imageIndex;
			this.Text = mSummary.ShapeName;
			
		}

		protected ShapeSummary mSummary = null;
	
		public ShapeSummary Summary
		{
			get{return mSummary;}
			set{mSummary = value;}
		}
			
	}
}

