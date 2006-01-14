using Ice;

namespace Ferda.FrontEnd.AddIns
{
	public interface IAddInMain
	{
		IOwnerOfAddIn OwnerOfAddIn
		{
			set;
		}
		
		Ice.ObjectAdapter ObjectAdapter
		{
			set;
		}
		
		string[] ObjectProxiesToAdd
		{
			get;
		}
	}
}
