using System;
using System.Collections;
namespace Netron.GraphLib.Configuration
{
	/// <summary>
	/// Implements a strongly typed collection of shape summaries
	/// </summary>
	public class ShapeSummaryCollection : CollectionBase
	{
		public ShapeSummaryCollection()
		{
			
		}
		public int Add(ShapeSummary summary)
		{
			return this.InnerList.Add(summary);
		}

		public ShapeSummary this[int index]
		{
			get{return this.InnerList[index] as ShapeSummary;}
		}
	}
}
