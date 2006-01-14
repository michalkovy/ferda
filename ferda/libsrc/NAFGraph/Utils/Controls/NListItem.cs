using System;
using System.ComponentModel;
namespace Netron.GraphLib.Utils
{
	/// <summary>
	/// Summary description for NListItem.
	/// </summary>
	[DefaultProperty("Text")] public class NListItem
	{
		#region Fields
		protected string mText = string.Empty;
		#endregion

		#region Properties
		public string Text
		{
			get{return mText;}
			set{mText = value;}
		}
		#endregion

		#region Constructor
		public NListItem(string text)
		{
			this.mText = text;
		}
		public NListItem(){}

		#endregion

		#region Methods

		public override string ToString()
		{
			return mText;
		}
		#endregion

	}
}
