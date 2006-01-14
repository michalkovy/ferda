using System.Diagnostics;
using Ice;

namespace Ferda.FrontEnd.AddIns
{
	public abstract class AbstractMain : IAddInMain
	{
		public abstract string NameOfObject
		{
			get;
		}
		
		public abstract Ice.Object IceObject
		{
			get;
		}
		
		public IOwnerOfAddIn OwnerOfAddIn
		{
			get {
				return ownerOfAddIn;
			}
			set {
				ownerOfAddIn = value;
			}
		}
		
		public ObjectAdapter ObjectAdapter
		{
			get {
				return objectAdapter;
			}
			set {
				objectAdapter = value;
				Ice.ObjectPrx prx = objectAdapter.add(
					IceObject,
					Ice.Util.stringToIdentity(NameOfObject));
				objectProxiesToAdd = new string[]{ objectAdapter.getCommunicator().proxyToString(prx) };
				Debug.WriteLine("created " + objectProxiesToAdd[0]);
			}
		}
		
		public string[] ObjectProxiesToAdd
		{
			get
			{
				return objectProxiesToAdd;
			}
		}
		
		private ObjectAdapter objectAdapter;
		private IOwnerOfAddIn ownerOfAddIn;
		private string[] objectProxiesToAdd;
	}
}
