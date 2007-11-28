
using System;
using Ice;namespace Ferda.Modules
{
	public class PropertyBoxModuleI : Ferda.Modules.BoxModuleDisp_
	{

		private BoxModuleFactoryPrx factory;
		private string propertyClassIceId;
		private string[] propertyFunctionsIceIds;
		private string mainFunctionsIceId;
		private BoxModulePrx myProxy;
		private Ice.ObjectPrx propertyValuePrx;
        private PropertyValue propertyValue;
		private Ferda.Modules.BoxModulePrx connectedBox;
		private bool propertySetByValue = false;
		private Ice.ObjectAdapter adapter;
		private PropertyValue defaultValue;

		private PropertyBoxModuleFactoryCreatorI.ValueFromPrx valueFromPrx;

		public BoxModulePrx MyProxy
		{
			get
			{
				return this.myProxy;
			}
		}

		///<summary>
		/// Constructor
		/// </summary>
		/// <param name="factory">A  BoxModuleFactoryPrx</param>
		/// <param name="propertyClassIceId">A  string</param>
		/// <param name="propertyFunctionsIceIds">A  string[]</param>
		/// <param name="identifier">A  string</param>
		public PropertyBoxModuleI(BoxModuleFactoryPrx factory, string propertyClassIceId, string[] propertyFunctionsIceIds, string mainFunctionsIceId, Ice.ObjectAdapter adapter, Ice.Identity myIdentity, Modules.PropertyBoxModuleFactoryCreatorI.ValueFromPrx valueFromPrx, PropertyValue defaultValue)
		{
			this.factory = factory;
			this.propertyClassIceId = propertyClassIceId;
			this.propertyFunctionsIceIds = propertyFunctionsIceIds;
			this.mainFunctionsIceId = mainFunctionsIceId;
			this.myProxy = BoxModulePrxHelper.uncheckedCast(adapter.add(this,myIdentity));
			this.adapter = adapter;
			this.valueFromPrx = valueFromPrx;
			this.defaultValue = defaultValue;
			this.setProperty("value",defaultValue);
		}

		/// <summary>
		/// Method destroy
		/// </summary>
		/// <param name="adapter">An ObjectAdapter</param>
		public void destroy(ObjectAdapter adapter)
		{
			if(propertyValuePrx != null && propertySetByValue)
				adapter.remove(propertyValuePrx.ice_getIdentity());
		}

		/// <summary>
		/// Method getDynamicHelpItems
		/// </summary>
		/// <returns>A Ferda.Modules.DynamicHelpItem[]</returns>
		/// <param name="__current">An Ice.Current</param>
		public override DynamicHelpItem[] getDynamicHelpItems(Current __current)
		{
			return new DynamicHelpItem[0];
		}

		/// <summary>
		/// Method runAction
		/// </summary>
		/// <param name="actionName">A  string</param>
		/// <param name="__current">An Ice.Current</param>
		public override void runAction(String actionName, Current __current)
		{
			throw new NameNotExistError();
		}

		/// <summary>
		/// Method getAdditionalSockets
		/// </summary>
		/// <returns>A Ferda.Modules.SocketInfo[]</returns>
		/// <param name="__current">An Ice.Current</param>
		public override SocketInfo[] getAdditionalSockets(Current __current)
		{
			return new SocketInfo[0];
		}
		
		/// <summary>
		/// Method getAdditionalProperties
		/// </summary>
		/// <returns>A Ferda.Modules.PropertyInfo[]</returns>
		/// <param name="__current">An Ice.Current</param>
		public override PropertyInfo[] getAdditionalProperties(Current __current)
		{
			return new PropertyInfo[0];
		}

		/// <summary>
		/// Method setConnection
		/// </summary>
		/// <param name="socketName">A  string</param>
		/// <param name="otherModule">A  Ferda.Modules.BoxModulePrx</param>
		/// <param name="__current">An Ice.Current</param>
		public override void setConnection(String socketName, BoxModulePrx otherModule, Current __current)
		{
			if (! (socketName == "value"))
            {
                throw new Ferda.Modules.NameNotExistError();
            }
			if(propertyValuePrx!= null && !propertySetByValue)
			{
				throw new Modules.ConnectionExistsError();
			}
			if(otherModule == null)
			{
				throw new System.Exception("Can not set null box module");
			}
            string[] iceIds = otherModule.getFunctionsIceIds();
            System.Collections.Specialized.StringCollection collectionIceIds = new System.Collections.Specialized.StringCollection();
            collectionIceIds.AddRange(iceIds);
            if (iceIds.Length != 0 && !collectionIceIds.Contains(mainFunctionsIceId))
				throw new Ferda.Modules.BadTypeError();
			if(propertyValuePrx != null && propertySetByValue)
				adapter.remove(propertyValuePrx.ice_getIdentity());
			this.propertyValuePrx = null;
			connectedBox = otherModule;
			this.propertySetByValue = false;
		}

		/// <summary>
		/// Method getConnections
		/// </summary>
		/// <returns>A Ferda.Modules.BoxModulePrx[]</returns>
		/// <param name="socketName">A  string</param>
		/// <param name="__current">An Ice.Current</param>
		public override BoxModulePrx[] getConnections(String socketName, Current __current)
		{
			if(socketName == "value" && (!this.propertySetByValue))
			{
				return new BoxModulePrx[]{connectedBox};
			}
			return null;
		}

		/// <summary>
		/// Method removeConnection
		/// </summary>
		/// <param name="socketName">A  string</param>
		/// <param name="boxModuleIceIdentity">A  string</param>
		/// <param name="__current">An Ice.Current</param>
		public override void removeConnection(String socketName, String boxModuleIceIdentity, Current __current)
		{
			if (! (socketName == "value"))
            {
                throw new Ferda.Modules.NameNotExistError();
            }
            if (connectedBox == null || propertySetByValue || Ice.Util.identityToString(connectedBox.ice_getIdentity()) != boxModuleIceIdentity)
			{
				throw new Modules.ConnectionNotExistError();
			}
            connectedBox = null;
			this.setProperty("value",defaultValue);
		}

		/// <summary>
		/// Method setProperty
		/// </summary>
		/// <param name="propertyName">A  string</param>
		/// <param name="value">A  Ferda.Modules.PropertyValue</param>
		/// <param name="__current">An Ice.Current</param>
		public override void setProperty(String propertyName, PropertyValue value, Current __current)
		{
			if (! (propertyName == "value"))
            {
                throw new Ferda.Modules.NameNotExistError();
            }
            if (value!=null && !value.ice_isA(this.propertyClassIceId))
            {
                throw new Ferda.Modules.BadTypeError();
            }

			if(propertyValuePrx != null && propertySetByValue)
				adapter.remove(propertyValuePrx.ice_getIdentity());
			this.propertySetByValue = true;
            propertyValue = value;
			propertyValuePrx = this.adapter.addWithUUID(value);
		}

		/// <summary>
		/// Method getProperty
		/// </summary>
		/// <returns>A Ferda.Modules.PropertyValue</returns>
		/// <param name="propertyName">A  string</param>
		/// <param name="__current">An Ice.Current</param>
		public override PropertyValue getProperty(String propertyName, Current __current)
		{
			if (! (propertyName == "value"))
            {
                throw new Ferda.Modules.NameNotExistError();
            }
            if (propertySetByValue)
            {
                return propertyValue;
            }
            else
            {
                return valueFromPrx(connectedBox.getFunctions());
            }
		}

		/// <summary>
		/// Method getPropertyOptions
		/// </summary>
		/// <returns>A Ferda.Modules.SelectString[]</returns>
		/// <param name="propertyName">A  string</param>
		/// <param name="__current">An Ice.Current</param>
		public override SelectString[] getPropertyOptions(String propertyName, Current __current)
		{
			return new SelectString[0];
		}

		/// <summary>
		/// Method isPropertySet
		/// </summary>
		/// <returns>A bool</returns>
		/// <param name="propertyName">A  string</param>
		/// <param name="__current">An Ice.Current</param>
		public override bool isPropertySet(String propertyName, Current __current)
		{
			return true;
		}

		/// <summary>
		/// Method getModulesAskingForCreation
		/// </summary>
		/// <returns>A Ferda.Modules.ModuleAskingForCreation[]</returns>
		/// <param name="__current">An Ice.Current</param>
		public override ModulesAskingForCreation[] getModulesAskingForCreation(Current __current)
		{
			return new ModulesAskingForCreation[0];
		}

		/// <summary>
		/// Method getFunctionsIceIds
		/// </summary>
		/// <returns>A string[]</returns>
		/// <param name="__current">An Ice.Current</param>
		public override String[] getFunctionsIceIds(Current __current)
		{
			return this.propertyFunctionsIceIds;
		}

		/// <summary>
		/// Method getFunctions
		/// </summary>
		/// <returns>An Ice.ObjectPrx</returns>
		/// <param name="__current">An Ice.Current</param>
		public override ObjectPrx getFunctions(Current __current)
		{
            if (this.propertySetByValue)
            {
                return this.propertyValuePrx;
            }
            else
            {
                return connectedBox.getFunctions();
            }
		}

		/// <summary>
		/// Method getMyFactory
		/// </summary>
		/// <returns>A Ferda.Modules.BoxModuleFactoryPrx</returns>
		/// <param name="__current">An Ice.Current</param>
		public override BoxModuleFactoryPrx getMyFactory(Current __current)
		{
			return this.factory;
		}

		/// <summary>
		/// Method getDefaultUserLabel
		/// </summary>
		/// <returns>A string[]</returns>
		/// <param name="__current">An Ice.Current</param>
		public override string[] getDefaultUserLabel(Current __current)
		{
			return new string[0];
		}

		/// <summary>
		/// Method validate
		/// </summary>
		/// <param name="__current">An Ice.Current</param>
		public override void validate(Ice.Current __current)
		{
		}
	}
}
