using System;

namespace Netron.GraphLib.IO.GraphML
{
	/// <summary>
	/// Generic Collectioon of GraphML related data
	/// </summary>
	public class DataCollection : System.Collections.CollectionBase 
	{
            
		
		
		public DataCollection() 
		{
		}
            
		
		
		public virtual object this[int index] 
		{
			get 
			{
				return this.List[index];
			}
			set 
			{
				this.List[index] = value;
			}
		}
            
		
		
		public virtual void Add(object o) 
		{
			this.List.Add(o);
		}
            
		
            
		
		
		public virtual bool Contains(object o) 
		{
			return this.List.Contains(o);
		}
            
		
		
		public virtual void Remove(object o) 
		{
			this.List.Remove(o);
		}
		
		
		public override string ToString() 
		{
			System.IO.StringWriter sw = new System.IO.StringWriter();
			// <foreach>
			// This loop mimics a foreach call. See C# programming language, pg 247
			// Here, the enumerator is seale and does not implement IDisposable
			System.Collections.IEnumerator enumerator = this.List.GetEnumerator();
			for (
				; enumerator.MoveNext(); 
				) 
			{
				string s = ((string)(enumerator.Current));
				// foreach body
				sw.Write(s);
			}
			// </foreach>
			return sw.ToString();
		}

	}
}
