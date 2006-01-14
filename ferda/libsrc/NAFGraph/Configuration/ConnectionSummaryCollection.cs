using System;
using System.Collections;
namespace Netron.GraphLib.Configuration
{
	/// <summary>
	/// Implements a strongly typed collection of connection summaries
	/// </summary>
	public class ConnectionSummaryCollection : CollectionBase
	{
		public ConnectionSummaryCollection()
		{
			
		}
		public int Add(ConnectionSummary summary)
		{
			return this.InnerList.Add(summary);
		}

		public ConnectionSummary this[int index]
		{
			get{return this.InnerList[index] as ConnectionSummary;}
		}
	}
}
