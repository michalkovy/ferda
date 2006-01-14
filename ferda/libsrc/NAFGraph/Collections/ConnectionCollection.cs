using System;
using System.Collections;
namespace Netron.GraphLib
{
	/// <summary>
	/// Collection of connections
	/// </summary>
	[Serializable] public class ConnectionCollection : CollectionBase
	{
		#region Constructor
		public ConnectionCollection()
		{
			
		}
		#endregion

		#region Properties
		public Connection this[int index]
		{
			get
			{
				return this.InnerList[index] as Connection;
			}
		}

		#endregion

		#region Methods

		public int Add(Connection connection)
		{
			return this.InnerList.Add(connection);
		}

		public bool Contains(object connection)
		{
			if(connection is Connection)
				return this.InnerList.Contains(connection);
			else
				return false;
		}		

		public void Remove(Connection connection)
		{
			this.InnerList.Remove(connection);
		}

		#endregion		
		
	}
}
