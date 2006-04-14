using System;
using Ice;
using System.Collections.Generic;

namespace Ferda.Modules
{
	public class PropertyBoxModuleFactoryI : Ferda.Modules.BoxModuleFactoryDisp_
	{
		private string propertyClassIceId;
		private string[] propertyFunctionsIceIds;
		private BoxModuleFactoryCreatorPrx myFactoryCreatorProxy;
        private string settingModuleIdentifier;

		private Dictionary<string, PropertyBoxModuleI> boxModules =
		new Dictionary<string, PropertyBoxModuleI>();

		//private string[] localePrefs;

		private PropertyValue defaultValue;

		private BoxModuleFactoryPrx myProxy;

		private PropertyBoxModuleFactoryCreatorI.ValueFromPrx valueFromPrx;

		public PropertyBoxModuleFactoryI(string propertyClassIceId,
										 string[] propertyFunctionsIceIds,
										 BoxModuleFactoryCreatorPrx myFactoryCreatorProxy,
										 string[] localePrefs,
										 PropertyValue defaultValue,
										 PropertyBoxModuleFactoryCreatorI.ValueFromPrx valueFromPrx,
										 Ice.ObjectAdapter adapter,
                                         string settingModuleIdentifier)
		{
			this.propertyClassIceId = propertyClassIceId;
			this.propertyFunctionsIceIds = propertyFunctionsIceIds;
			//this.localePrefs = localePrefs;
			this.myFactoryCreatorProxy = myFactoryCreatorProxy;
			this.defaultValue = defaultValue;
			this.valueFromPrx = valueFromPrx;
			this.myProxy = BoxModuleFactoryPrxHelper.uncheckedCast(adapter.addWithUUID(this));
            this.settingModuleIdentifier = settingModuleIdentifier;
		}

		public BoxModuleFactoryPrx MyProxy
		{
			get
			{
				return myProxy;
			}
		}

		/// <summary>
		/// Method createBoxModule
		/// </summary>
		/// <returns>A Ferda.Modules.BoxModulePrx</returns>
		/// <param name="__current">An Ice.Current</param>
		public override BoxModulePrx createBoxModule(Current __current)
		{
			if (_destroy)
			{
				throw new Ice.ObjectNotExistException();
			}
			Ice.Identity boxModuleIdentity = Ice.Util.stringToIdentity(Ice.Util.generateUUID());
			PropertyBoxModuleI boxModule = new PropertyBoxModuleI(myProxy, this.propertyClassIceId, this.propertyFunctionsIceIds, __current.adapter, boxModuleIdentity, valueFromPrx, defaultValue);
			BoxModulePrx boxModulePrx = boxModule.MyProxy;
			string boxIdentity = Ice.Util.identityToString(boxModulePrx.ice_getIdentity());
			this.boxModules[boxIdentity] = boxModule;
			return boxModulePrx;
		}

		private System.DateTime timestamp;
		/// <summary>
		/// The last time the session was refreshed.
		/// </summary>
		public System.DateTime Timestamp
		{
			get
			{
				if (_destroy)
				{
					throw new Ice.ObjectNotExistException();
				}
				return timestamp;
			}
		}

		private bool _destroy;

		/// <summary>
		/// Method refresh
		/// </summary>
		/// <param name="__current">An Ice.Current</param>
		public override void refresh(Current __current)
		{
			if (_destroy)
			{
				throw new Ice.ObjectNotExistException();
			}
			timestamp = System.DateTime.Now;
		}

		/// <summary>
		/// Method destroyIfEmpty
		/// </summary>
		/// <returns>A bool</returns>
		/// <param name="__current">An Ice.Current</param>
		public override bool destroyIfEmpty(Current __current)
		{
			if (this.boxModules.Count == 0)
			{
				this.destroy(__current);
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Method destroyBoxModule
		/// </summary>
		/// <param name="boxIdentity">A  string</param>
		/// <param name="__current">An Ice.Current</param>
		public override void destroyBoxModule(String boxIdentity, Current __current)
		{
			this.boxModules[boxIdentity].destroy(__current.adapter);
			this.boxModules.Remove(boxIdentity);
			__current.adapter.remove(Ice.Util.stringToIdentity(boxIdentity));
		}

		/// <summary>
		/// Method destroy
		/// </summary>
		/// <param name="__current">An Ice.Current</param>
		public override void destroy(Current __current)
		{
			if (_destroy)
			{
				throw new Ice.ObjectNotExistException();
			}

			_destroy = true;

			try
			{
				__current.adapter.remove(__current.id);
				foreach (string boxIdentity in boxModules.Keys)
				{
					__current.adapter.remove(Ice.Util.stringToIdentity(boxIdentity));
				}
			}
			catch (Ice.ObjectAdapterDeactivatedException)
			{
				// This method is called on shutdown of the server, in which
				// case this exception is expected.
			}
		}

		/// <summary>
		/// Method getSockets
		/// </summary>
		/// <returns>A Ferda.Modules.SocketInfo[]</returns>
		/// <param name="__current">An Ice.Current</param>
		public override SocketInfo[] getSockets(Current __current)
		{
			SocketInfo result = new SocketInfo();
			result.hint = "";
			result.label = "value";
			result.moreThanOne = false;
			result.name = "value";
			result.settingProperties = null;
			BoxType boxType = new BoxType();
			boxType.functionIceId = this.propertyFunctionsIceIds[0];
			boxType.neededSockets = null;
			result.socketType = new BoxType[] { boxType };
			return new SocketInfo[] { result };
		}

		/// <summary>
		/// Method getActions
		/// </summary>
		/// <returns>A Ferda.Modules.ActionInfo[]</returns>
		/// <param name="__current">An Ice.Current</param>
		public override ActionInfo[] getActions(Current __current)
		{
			return null;
		}

		/// <summary>
		/// Method getProperties
		/// </summary>
		/// <returns>A Ferda.Modules.PropertyInfo[]</returns>
		/// <param name="__current">An Ice.Current</param>
		public override PropertyInfo[] getProperties(Current __current)
		{
			PropertyInfo result = new PropertyInfo();
			result.categoryName = "";
			result.name = "value";
			result.numericalRestrictions = null;
			result.readOnly = false;
			result.regexp = null;
			result.selectBoxParams = null;
			result.typeClassIceId = this.propertyClassIceId;
			result.visible = true;
            result.settingModuleIdentifier = this.settingModuleIdentifier;
			return new PropertyInfo[] { result };
		}

		/// <summary>
		/// Method getHelpFileInfoSeq
		/// </summary>
		/// <returns>A Ferda.Modules.HelpFileInfo[]</returns>
		/// <param name="__current">An Ice.Current</param>
		public override HelpFileInfo[] getHelpFileInfoSeq(Current __current)
		{
			return null;
		}

		/// <summary>
		/// Method getMyFactoryCreator
		/// </summary>
		/// <returns>A Ferda.Modules.BoxModuleFactoryCreatorPrx</returns>
		/// <param name="__current">An Ice.Current</param>
		public override BoxModuleFactoryCreatorPrx getMyFactoryCreator(Current __current)
		{
			return myFactoryCreatorProxy;
		}
	}
}
