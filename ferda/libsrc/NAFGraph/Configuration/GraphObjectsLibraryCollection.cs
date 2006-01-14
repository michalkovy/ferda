using System;
using System.Collections;
namespace Netron.GraphLib.Configuration
{
	/// <library>
	/// Strongly typed collection of shape libraries
	/// </library>
	public class GraphObjectsLibraryCollection : CollectionBase
	{
		public GraphObjectsLibraryCollection()
		{			
		}

		public int Add(GraphObjectsLibrary library)
		{
			return this.InnerList.Add(library);
		}

		public GraphObjectsLibrary this[int index]
		{
			get{return this.InnerList[index] as GraphObjectsLibrary;}
		}

		public ShapeSummary GetShapeSummary(string shapeKey)
		{
			for(int k=0; k<this.InnerList.Count; k++)
			{
				for(int m=0; m<this[k].ShapeSummaries.Count; m++)
					if(this[k].ShapeSummaries[m].ShapeKey==shapeKey)
						return this[k].ShapeSummaries[m];
			}
			return null;
		}

		public ConnectionSummary GetConnectionSummary(string connectionName)
		{
			for(int k=0; k<this.InnerList.Count; k++)
			{
				for(int m=0; m<this[k].ConnectionSummaries.Count; m++)
					if(this[k].ConnectionSummaries[m].ConnectionName==connectionName)
						return this[k].ConnectionSummaries[m];
			}
			return null;
		}


	}
}
