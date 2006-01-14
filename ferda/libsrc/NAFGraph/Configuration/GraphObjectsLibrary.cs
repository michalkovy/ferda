using System;
using Netron.GraphLib.Configuration;
namespace Netron.GraphLib.Configuration
{
	/// <summary>
	/// Collects shape and lib info of an assembly containing shapes
	/// </summary>
	public class GraphObjectsLibrary
	{
		#region Fields
		protected string mPath;
		protected ShapeSummaryCollection shapeSummmaries;
		protected ConnectionSummaryCollection conSummaries;
		#endregion

		#region Constructor
		public GraphObjectsLibrary()
		{
			shapeSummmaries = new ShapeSummaryCollection();
			conSummaries = new ConnectionSummaryCollection();
		}
		#endregion	
 
		#region Properties
		/// <summary>
		/// Gets or sets the mPath of the library
		/// </summary>
		public string Path
		{
			get{return mPath;}
			set{mPath = value;}
		}

		/// <summary>
		/// Gets or sets the shape summaries
		/// </summary>
		public ShapeSummaryCollection ShapeSummaries
		{
			get{return shapeSummmaries;}
			set{shapeSummmaries =value;}
		}
		/// <summary>
		/// Gets or sets the connection summaries
		/// </summary>
		public ConnectionSummaryCollection ConnectionSummaries
		{
			get{return conSummaries;}
			set{conSummaries = value;}
		}
		#endregion

	}
}
