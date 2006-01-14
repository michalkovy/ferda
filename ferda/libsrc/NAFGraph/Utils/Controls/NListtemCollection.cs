using System;
using System.Collections;
namespace Netron.GraphLib.Utils
{
	/// <summary>
	/// Summary description for ComboItemCollection.
	/// </summary>
	public class NListItemCollection : CollectionBase
	{

		public delegate void NListChange(NListItem item, int index);
		public event NListChange OnItemAdded;
		public event NListChange OnItemRemoved;
		public NListItemCollection()
		{}
		public int Add(NListItem item)
		{
			return this.InnerList.Add(item);
		}
		protected override void OnInsert(int index, object value)
		{
			base.OnInsert (index, value);
			if(OnItemAdded!=null) OnItemAdded( value as NListItem ,index);
		}

		protected override void OnRemove(int index, object value)
		{
			base.OnRemove (index, value);
			if(OnItemRemoved!=null) OnItemRemoved(value as NListItem,index);

		}



		public NListItem this[int index]
		{get{return this.InnerList[index] as NListItem;}}
	}
}
