using System;
using System.Drawing;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Diagnostics;
using Netron.GraphLib.UI;
using Netron.GraphLib.Interfaces;
using Netron.Maths;
namespace Netron.GraphLib
{
	/// <summary>
	/// Base class for the implementation of a layout algorithm
	/// </summary>
	public abstract class GraphLayout: IGraphLayout
	{

		[NonSerialized]  protected IGraphSite mSite;
		protected int nnodes = 0;
		protected int nedges=0;
		protected Size CanvasSize ;
		protected ShapeCollection nodes=null;
		protected ArrayList edges=null;
		protected virtual NetronVector GraphCenter()
		{
			return new NetronVector(0,0,0);
		}
		/// <summary>
		/// Gets or sets the mSite to which the layout belongs
		/// </summary>
	public IGraphSite Site
		{
			get{return mSite;}
			set
			{
				if(mSite==null) return;
				mSite = value;
				nnodes = mSite.Nodes.Count;
				nedges=mSite.Edges.Count;
				CanvasSize = mSite.Size;
				nodes=mSite.Nodes;
				edges=mSite.Edges;
			}
		}

		public GraphLayout(IGraphSite mSite)
		{
			if(mSite==null) throw new Exception("No mSite specified in graph layout constructor.");;
			this.mSite = mSite;
			nnodes = mSite.Nodes.Count;
			nedges=mSite.Edges.Count;
			CanvasSize = mSite.Size;
			nodes=mSite.Nodes;
			edges=mSite.Edges;
		}



		public virtual void StartLayout()
		{
			
		}

		public virtual void StopLayout()
		{}


	}
}