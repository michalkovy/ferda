using System;
using System.Drawing;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Diagnostics;
using Netron.GraphLib.UI;
using Netron.GraphLib.Interfaces;
namespace Netron.GraphLib
{
	/// <summary>
	/// Factory of graph layouts
	/// </summary>
	public class LayoutFactory
	{

		#region Fields
		public  GraphLayoutAlgorithms GraphLayoutAlgorithm=GraphLayoutAlgorithms.SpringEmbedder;
		[NonSerialized]  protected IGraphSite mSite;
		#endregion

		public LayoutFactory(IGraphSite mSite)
		{
		
			this.mSite = mSite;
		}

		
		
		public IGraphSite Site
		{
			get{return mSite;}
			set{mSite = value;}
		}
		public Func<Task> GetRunable(CancellationToken cancellation)
		{
			switch (GraphLayoutAlgorithm)
			{
				case GraphLayoutAlgorithms.SpringEmbedder:
					SpringEmbedder emb=new SpringEmbedder(mSite);
					return () => emb.StartLayout(cancellation);							
				default:
					throw new Exception("Invalid or unknown layout algorithm");					
			}
		}

	}
}