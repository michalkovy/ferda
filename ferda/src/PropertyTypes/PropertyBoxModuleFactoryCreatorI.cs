

using Ice;
using System;
using Ferda.ModulesManager;

namespace Ferda.Modules
{
	public class PropertyBoxModuleFactoryCreatorI : Ferda.Modules.PropertyBoxModuleFactoryCreatorDisp_
	{
		private string propertyClassIceId;
		private string[] propertyFunctionsIceIds;
		private string identifier;
		private PropertyReapThread reaper;
		private PropertyValue defaultValue;
		private BoxModuleFactoryCreatorPrx myProxy;
		
		public delegate PropertyValue ValueFromPrx(Ice.ObjectPrx value);
		
		private ValueFromPrx valueFromPrx;
		
		///<summary>
		/// Constructor
		/// </summary>
		/// <param name="propertyClassIceId">A  string</param>
		/// <param name="propertyFunctionsIceIds">A  string[]</param>
		/// <param name="identifier">A  string</param>
		public PropertyBoxModuleFactoryCreatorI(string propertyClassIceId,
												string[] propertyFunctionsIceIds,
												string identifier,
												PropertyValue defaultValue,
												ValueFromPrx valueFromPrx,
												PropertyReapThread reaper,
												Ice.ObjectAdapter adapter)
		{
			this.propertyClassIceId = propertyClassIceId;
			this.propertyFunctionsIceIds = propertyFunctionsIceIds;
			this.identifier = identifier;
			this.valueFromPrx = valueFromPrx;
			this.reaper = reaper;
			this.defaultValue = defaultValue;
			this.myProxy =
				PropertyBoxModuleFactoryCreatorPrxHelper.uncheckedCast(adapter.add(this,Ice.Util.stringToIdentity(identifier)));
		}
		
		public BoxModuleFactoryCreatorPrx MyProxy
		{
			get {
				return myProxy;
			}
		}
		
		/// <summary>
		/// Method getPropertyClassIceId
		/// </summary>
		/// <returns>A string</returns>
		/// <param name="__current">An Ice.Current</param>
		public override String getPropertyClassIceId(Current __current)
		{
			return this.propertyClassIceId;
		}
		
		/// <summary>
		/// Method createBoxModuleFactory
		/// </summary>
		/// <returns>A Ferda.Modules.BoxModuleFactoryPrx</returns>
		/// <param name="localePrefs">A  string[]</param>
		/// <param name="manager">A  Ferda.ModulesManager.ManagersEnginePrx</param>
		/// <param name="__current">An Ice.Current</param>
		public override BoxModuleFactoryPrx createBoxModuleFactory(String[] localePrefs, ManagersEnginePrx manager, Current __current)
		{
			
			PropertyBoxModuleFactoryI boxModuleFactory = new PropertyBoxModuleFactoryI(propertyClassIceId,
																					   propertyFunctionsIceIds,
																					   myProxy,
																					   localePrefs,
																					   defaultValue,
																					   valueFromPrx,
																					   __current.adapter);
			this.reaper.Add(boxModuleFactory.MyProxy, boxModuleFactory);
			return boxModuleFactory.MyProxy;
		}
		
		/// <summary>
		/// Method getBoxCategories
		/// </summary>
		/// <returns>A string[]</returns>
		/// <param name="__current">An Ice.Current</param>
		public override String[] getBoxCategories(Current __current)
		{
			return new string[]{"other"};
	}
		
		/// <summary>
		/// Method getBoxCategoryLocalizedName
		/// </summary>
		/// <returns>A string[]</returns>
		/// <param name="locale">A  string</param>
		/// <param name="categoryName">A  string</param>
		/// <param name="__current">An Ice.Current</param>
		public override String[] getBoxCategoryLocalizedName(String locale, String categoryName, Current __current)
		{
			return new string[0];
	}
		
		/// <summary>
		/// Method getBoxModuleFunctionsIceIds
		/// </summary>
		/// <returns>A string[]</returns>
		/// <param name="__current">An Ice.Current</param>
		public override String[] getBoxModuleFunctionsIceIds(Current __current)
		{
			return this.propertyFunctionsIceIds;
		}
		
		/// <summary>
		/// Method getDesign
		/// </summary>
		/// <returns>A string</returns>
		/// <param name="__current">An Ice.Current</param>
		public override String getDesign(Current __current)
		{
			return null;
		}
		
		/// <summary>
		/// Method getHelpFile
		/// </summary>
		/// <returns>A byte[]</returns>
		/// <param name="identifier">A  string</param>
		/// <param name="__current">An Ice.Current</param>
		public override byte[] getHelpFile(String identifier, Current __current)
		{
			// TODO
			return null;
		}
		
		/// <summary>
		/// Method getHint
		/// </summary>
		/// <returns>A string</returns>
		/// <param name="localePrefs">A  string[]</param>
		/// <param name="__current">An Ice.Current</param>
		public override String getHint(String[] localePrefs, Current __current)
		{
			// TODO
			return null;
		}
		
		/// <summary>
		/// Method getIcon
		/// </summary>
		/// <returns>A byte[]</returns>
		/// <param name="__current">An Ice.Current</param>
		public override byte[] getIcon(Current __current)
		{
			// TODO
			return null;
		}
		
		/// <summary>
		/// Method getIdentifier
		/// </summary>
		/// <returns>A string</returns>
		/// <param name="__current">An Ice.Current</param>
		public override String getIdentifier(Current __current)
		{
			return this.identifier;
		}
		
		/// <summary>
		/// Method getLabel
		/// </summary>
		/// <returns>A string</returns>
		/// <param name="localePrefs">A  string[]</param>
		/// <param name="__current">An Ice.Current</param>
		public override String getLabel(String[] localePrefs, Current __current)
		{
			//TODO
			return this.identifier;
		}
		
	}
}
