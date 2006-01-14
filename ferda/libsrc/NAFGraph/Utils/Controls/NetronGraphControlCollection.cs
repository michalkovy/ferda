using System;
using System.Collections;
namespace Netron.GraphLib.Utils
{
	[Serializable] public class NetronGraphControlCollection : CollectionBase
	{
		public int Add(NetronGraphControl control)
		{
			return this.InnerList.Add(control);
		}

		public NetronGraphControl this[int index]
		{
			get{return this.InnerList[index] as NetronGraphControl;}
		}

	}
}
