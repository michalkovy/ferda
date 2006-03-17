// ModulesManager.cs - Main file of Modules Manager
//
// Author: Michal Kováč <michal.kovac.develop@centrum.cz>
//
// Copyright (c) 2005 Michal Kováč 
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;
using Ferda.Modules;
//using Ferda.Modules.Boxes;

namespace Ferda {
    namespace ModulesManager {

        /// <summary>
        /// Main class of Modules Manager
        /// </summary>
        public class ModulesManager  {
			/// <summary>
			/// Inits Ice object adapter and helper class
			/// </summary>
			/// <param name="args">A string[] representing arguments from command line</param>
			/// <param name="localePrefs">A string[] representing locales with descending importance</param>
			protected void InitAdapterAndHelper(string[] args, string[] localePrefs)
			{
				Debug.WriteLine("Loading config...");
				Ice.Properties properties = Ice.Util.createProperties();
				properties.load("config");
				Debug.WriteLine("Creating communicator...");
				communicator = Ice.Util.initializeWithProperties(ref args, properties);
				Debug.WriteLine("Creating factory for basic property types...");
				Ferda.Modules.ObjectFactoryForPropertyTypes factory =
				new Ferda.Modules.ObjectFactoryForPropertyTypes();
				Debug.WriteLine("Adding that factory to communicator...");
				Ferda.Modules.ObjectFactoryForPropertyTypes.addFactoryToCommunicator(
					communicator, factory);
				//Debug.WriteLine("Creating and adding factory for other property types...");
				//ClassesFactory classesFactory = new ClassesFactory();
				//ClassesFactory.addFactoryToCommunicator(communicator, classesFactory);
				Debug.WriteLine("Creating adapter...");
				Ice.ObjectAdapter adapter =
                communicator.createObjectAdapter("ModulesManager");
				Debug.WriteLine("Activating adapter...");
				adapter.activate();
				Debug.WriteLine("Creating helper of modules manager...");
				this.lnkHelper = new Helper(adapter, localePrefs, this);
				Debug.WriteLine("Making factory refresh thread...");
				refreshThread = new Thread(
					new ThreadStart(
						this.lnkHelper.BoxModuleIceFactories.StartFactoriesRefreshing
					));
				refreshThread.Start();
			}

			/// <summary>
			/// Ends initialization of modules manager -
			/// mainly adds creators
			/// </summary>
			protected void InitEnd()
			{
				Debug.WriteLine("Adding Creators...");
                creatorPrxs = this.lnkHelper.ManagersEngineI.ManagersLocatorI.BoxModuleFactoryCreatorsByIdentifier;
                foreach (KeyValuePair<string, Ferda.Modules.BoxModuleFactoryCreatorPrx> val in creatorPrxs)
				{
					Debug.WriteLine("Adding " + val.Key);
					boxModuleFactoryCreators[val.Key] = new BoxModuleFactoryCreator(this.lnkHelper,val.Value,this,helpFiles);
				}
				Debug.WriteLine("Adding " + groupCreator.Identifier);
				boxModuleFactoryCreators[groupCreator.Identifier] = groupCreator;
			}

			/// <summary>
			/// Constructs Modules manager with default
			/// <see cref="T:Ferda.ModulesManager.OutputPrx"/>
			/// implementation
			/// </summary>
			/// <param name="args">A string[] representing arguments from command line</param>
			/// <param name="localePrefs">A string[] representing locales with descending importance</param>
            public ModulesManager(string[] args, string[] localePrefs) {
				InitAdapterAndHelper(args, localePrefs);
				/*IEnumerator<KeyValuePair<string,BoxModuleFactoryCreatorPrx>> iter =
				.GetEnumerator();*/
				InitEnd();
            }

			/// <summary>
			/// Constructs Modules manager with specified
			/// <see cref="T:Ferda.ModulesManager.OutputPrx"/>
			/// implementation
			/// </summary>
			/// <param name="args">A string[] representing arguments from command line</param>
			/// <param name="localePrefs">A string[] representing locales with descending importance</param>
			/// <param name="output">A <see cref="T:Ferda.ModulesManager.Output"/> class
			/// representing inerface where are send output from user</param>
			public ModulesManager(string[] args, string[] localePrefs, Ferda.ModulesManager.Output output)
			{
				InitAdapterAndHelper(args, localePrefs);
				Ice.ObjectPrx outputPrx =
					this.Helper.ObjectAdapter.addWithUUID(output);
				this.Helper.ManagersEngineI.OutputPrx =
					OutputPrxHelper.uncheckedCast(outputPrx);
				InitEnd();
			}

			/// <summary>
			/// Refreshes Modules manager informations
			/// </summary>
			/// <remarks>
			/// Is mainly nead to execute when in Ice locator
			/// (IceGrid) are new instances of
			/// <see cref="T:Ferda.Modules.BoxModuleFactoryCreatorPrx"/>
			/// </remarks>
			public void Refresh()
			{
				this.Helper.ManagersEngineI.ManagersLocatorI.Refresh();
				System.Collections.Specialized.StringCollection usedKeys
					= new System.Collections.Specialized.StringCollection();
				Debug.WriteLine("Refreshing Creators...");
				//TODO udelat lepe - co kdyz se zmeni server na kterem bezi moduly?!
                creatorPrxs = this.lnkHelper.ManagersEngineI.ManagersLocatorI.BoxModuleFactoryCreatorsByIdentifier;
				foreach(KeyValuePair<string,Ferda.Modules.BoxModuleFactoryCreatorPrx> val in creatorPrxs)
				{
					if(!boxModuleFactoryCreators.ContainsKey(val.Key))
						boxModuleFactoryCreators[val.Key] = new BoxModuleFactoryCreator(this.lnkHelper,val.Value, this, helpFiles);
					usedKeys.Add(val.Key);
				}
				foreach(string key in creatorPrxs.Keys)
				{
					if(!usedKeys.Contains(key)) boxModuleFactoryCreators.Remove(key);
				}
				boxModuleFactoryCreators[groupCreator.Identifier] = groupCreator;
			}

			/// <summary>
			/// Gets creator of boxes by their identifier
			/// </summary>
			/// <returns>A <see cref="T:Ferda.ModulesManager.IBoxModuleFactoryCreator"/>
			/// representing creator of boxes. If there is not
			/// creator with identifier <paramref name="name"/>
			/// returns null</returns>
			/// <param name="name">A string representing identifier of
			/// creators</param>
			public Ferda.ModulesManager.IBoxModuleFactoryCreator GetBoxModuleFactoryCreator(string name)
			{
				Ferda.ModulesManager.IBoxModuleFactoryCreator creator=null;
				if(boxModuleFactoryCreators.TryGetValue(name, out creator))
				{
					return creator;
				}
				else
				{
					return null;
				}
			}

			/// <summary>
			/// Creators of boxes
			/// </summary>
			/// <value>
			/// An array of
			/// <see cref="T:Ferda.ModulesManager.IBoxModuleFactoryCreator"/>
			/// which represents creators of boxes
			/// </value>
			public Ferda.ModulesManager.IBoxModuleFactoryCreator[] BoxModuleFactoryCreators
			{
				get {
					if(boxModuleFactoryCreators.Count>0)
					{
						IBoxModuleFactoryCreator[] result = new IBoxModuleFactoryCreator[boxModuleFactoryCreators.Count];
						boxModuleFactoryCreators.Values.CopyTo(result,0);
						return result;
					}
					else
					{
						return new IBoxModuleFactoryCreator[0];
					}
				}
			}

			/// <summary>
			/// Creates boxes from structure
			/// <see cref="T:Ferda.Modules.ModulesAskingForCreation"/>
			/// </summary>
			/// <remarks>
			/// You can get <see cref="T:Ferda.Modules.ModulesAskingForCreation"/>
			/// structure from property
			/// <see cref="P:Ferda.ModulesManager.IBoxModule.ModulesAskingForCreation"/>
			/// of <see cref="T:Ferda.ModulesManager.IBoxModule"/>.
			/// </remarks>
			/// <param name="newModule">A  ModulesAskingForCreation</param>
			/// <seealso cref="P:Ferda.ModulesManager.IBoxModule.ModulesAskingForCreation"/>
			public IBoxModule[] CreateBoxesAskingForCreation(ModulesAskingForCreation info)
			{
				List<IBoxModule> result = new List<IBoxModule>();
				foreach (ModuleAskingForCreation newModule in info.newModules)
				{
					Ferda.ModulesManager.BoxModuleFactoryCreator creator = (BoxModuleFactoryCreator)this.GetBoxModuleFactoryCreator(
						newModule.newBoxModuleIdentifier);
					BoxModule box = creator.CreateBoxModule() as BoxModule;
					if (box != null)
					{
						foreach (ModulesConnection connection in newModule.modulesConnection)
						{
							box.SetConnection(connection.socketName, this.getBoxModuleByProxy(connection.boxModuleParam));
						}
						foreach (PropertySetting propertySetting in newModule.propertySetting)
						{
							box.IceBoxModulePrx.setProperty(propertySetting.propertyName,
															propertySetting.value);
						}
						result.Add(box);
					}
				}
				return result.ToArray();
			}

			/// <summary>
			/// Gets <see cref="T:Ferda.ModulesManager.BoxModule"/>
			/// by <see cref="T:Ferda.Modules.BoxModulePrx"/>.
			/// </summary>
			/// <returns>A <see cref="T:Ferda.ModulesManager.BoxModule"/></returns>
			/// <param name="proxy">A <see cref="T:Ferda.Modules.BoxModulePrx"/></param>
			protected internal Ferda.ModulesManager.BoxModule getBoxModuleByProxy(BoxModulePrx proxy)
			{
				return boxModuleByProxyIdentity[proxy.ice_getIdentity()];
			}

			/// <summary>
			/// Gets <see cref="T:Ferda.ModulesManager.BoxModule"/>
			/// by string representation of <see cref="T:Ice.Identity"/>.
			/// </summary>
			/// <returns>A <see cref="T:Ferda.ModulesManager.BoxModule"/></returns>
			/// <param name="identity">A string representing <see cref="T:Ice.Identity"/>
			/// of <see cref="T:Ferda.Modules.BoxModulePrx"/></param>
			protected internal Ferda.ModulesManager.BoxModule getBoxModuleByIdentity(string identity)
			{
				Ferda.ModulesManager.BoxModule boxModule = null;
				boxModuleByProxyIdentity.TryGetValue(Ice.Util.stringToIdentity(identity),out boxModule);
				return boxModule;
			}

			/// <summary>
			/// Gets <see cref="T:Ferda.ModulesManager.IBoxModule"/>
			/// by string representation of <see cref="T:Ice.Identity"/>.
			/// </summary>
			/// <returns>A <see cref="T:Ferda.ModulesManager.IBoxModule"/></returns>
			/// <param name="identity">A string representing <see cref="T:Ice.Identity"/>
			/// of <see cref="T:Ferda.Modules.BoxModulePrx"/></param>
			public Ferda.ModulesManager.IBoxModule GetIBoxModuleByIdentity(string identity)
			{
				return this.getBoxModuleByIdentity(identity);
			}

			/// <summary>
			/// Adds <see cref="T:Ferda.ModulesManager.BoxModule"/> to internal collection of boxes
			/// </summary>
			/// <param name="box">A <see cref="T:Ferda.ModulesManager.BoxModule"/></param>
			/// <param name="proxy">A <see cref="T:Ferda.Modules.BoxModulePrx"/></param>
			protected internal void addBoxModuleWithProxy(Ferda.ModulesManager.BoxModule box, BoxModulePrx proxy)
			{
				boxModuleByProxyIdentity[proxy.ice_getIdentity()]=box;
			}

			/// <summary>
			/// Removes <see cref="T:Ferda.ModulesManager.BoxModule"/> from
			/// internal collection of boxes.
			/// </summary>
			/// <param name="proxy">A <see cref="T:Ferda.Modules.BoxModulePrx"/></param>
			protected internal void removeBoxModuleWithProxy(BoxModulePrx proxy)
			{
				boxModuleByProxyIdentity.Remove(proxy.ice_getIdentity());
			}

			/// <summary>
			/// Gets localized category name
			/// </summary>
			/// <returns>A string representing category
			/// localized name</returns>
			/// <param name="category">A string representing category
			/// identifier</param>
            /// <param name="creatorIdentifier">A string representing identifier of creator which to ask first</param>
			public string GetBoxModuleCategoryLocalization(string category, string creatorIdentifier)
			{
				foreach(string locale in lnkHelper.LocalePrefs)
				{
                    BoxModuleFactoryCreatorPrx creatorPrxFirst = creatorPrxs[creatorIdentifier];
                    if (creatorPrxFirst != null)
                    {
                        string[] result = creatorPrxFirst.getBoxCategoryLocalizedName(locale, category);
                        if (result.Length > 0)
                            return result[0];
                    }
                    foreach (BoxModuleFactoryCreatorPrx creatorPrx in creatorPrxs.Values)
					{
                        if (creatorPrx != null)
						{
                            string[] result = creatorPrx.getBoxCategoryLocalizedName(locale, category);
							if(result.Length > 0)
								return result[0];
						}
					}
				}
				return category;
			}

			/// <summary>
			/// Modules manager helper
			/// </summary>
			/// <value>
			/// A
			/// <see cref="T:Ferda.ModulesManager.Helper"/>
			/// class
			/// </value>
			public Helper Helper
			{
				set {
					lnkHelper = value;
				}

				get {
					return lnkHelper;
				}
			}

            /// <summary>
            /// Destroys objects which use Modules manager and stops refreshing thread.
            /// </summary>
            /// <seealso cref="M:Ferda.ModulesManager.ModulesManager.DestroyModulesManager()"/>
            /// <seealso cref="M:Ferda.ModulesManager.ModulesManager.DestroyModulesManagersCommunicator()"/>
            public void DestroyModulesManagersObjects()
            {
                Debug.WriteLine("Starting destroing...");
                foreach (Ferda.ModulesManager.IBoxModuleFactoryCreator icreator in boxModuleFactoryCreators.Values)
                {
                    Ferda.ModulesManager.BoxModuleFactoryCreator creator = icreator as Ferda.ModulesManager.BoxModuleFactoryCreator;
                    if (creator != null)
                    {
                        Debug.WriteLine("Destroing " + creator.Identifier);
                        creator.DestroyFactoryIfIs();
                    }
                }
                Debug.WriteLine("End refreshing...");
                this.lnkHelper.BoxModuleIceFactories.EndFactoriesRefreshing();
                Debug.WriteLine("Saving help files config...");
                helpFiles.SaveHelpFilesConfig();
                Debug.WriteLine("Waiting for refreshing thread...");
                refreshThread.Join();
            }

            /// <summary>
            /// Destroys ice communicator which Modules Manager use.
            /// </summary>
            /// <seealso cref="M:Ferda.ModulesManager.ModulesManager.DestroyModulesManager()"/>
            /// <seealso cref="M:Ferda.ModulesManager.ModulesManager.DestroyModulesManagersObjects()"/>
            public void DestroyModulesManagersCommunicator()
            {
                Debug.WriteLine("Destroing communicator...");
                if (communicator != null)
                {
                    try
                    {
                        communicator.shutdown();
                        communicator.destroy();
                    }
                    catch (Ice.LocalException ex)
                    {
                        Console.Error.WriteLine(ex);
                    }
                }
            }

			/// <summary>
			/// Destroys Modules manager
			/// </summary>
			/// <remarks>
			/// Don't forgot to call this method after end of
			/// using Modules manager or before exit of
            /// application. Or use
            /// <see cref="M:Ferda.ModulesManager.ModulesManager.DestroyModulesManagersObjects()"/>
            /// with
            /// <see cref="M:Ferda.ModulesManager.ModulesManager.DestroyModulesManagersCommunicator()"/>.
			/// </remarks>
            /// <seealso cref="M:Ferda.ModulesManager.ModulesManager.DestroyModulesManagersObjects()"/>
            /// <seealso cref="M:Ferda.ModulesManager.ModulesManager.DestroyModulesManagersCommunicator()"/>
			public void DestroyModulesManager()
			{
                this.DestroyModulesManagersObjects();
                this.DestroyModulesManagersCommunicator();
			}

			/// <summary>
			/// Adds <see cref="T:Ice.ObjectPrx"/> to Modules manager
			/// </summary>
			/// <param name="objectProxies">A <see cref="T:System.String"/>
			/// representing some Ice interface with which
			/// Modules manager will work
			/// </param>
            public void AddModuleServices(string[] objectProxies)
            {
				List<Ice.ObjectPrx> proxies = new List<Ice.ObjectPrx>();
				foreach(string objectProxy in objectProxies)
				{
					Ice.ObjectPrx objectPrx = communicator.stringToProxy(objectProxy);
					proxies.Add(objectPrx);
				}
				this.lnkHelper.ManagersEngineI.ManagersLocatorI.AddIceObjectProxies(proxies);
            }

            public void UnlockAllBoxes()
            {
                foreach (Ferda.ModulesManager.BoxModule box in boxModuleByProxyIdentity.Values)
                {
                    box.UnlockAll();
                }
            }

            private Dictionary<string, BoxModuleFactoryCreatorPrx> creatorPrxs;
			private System.Collections.Generic.Dictionary<string,IBoxModuleFactoryCreator> boxModuleFactoryCreators =
				new System.Collections.Generic.Dictionary<string,IBoxModuleFactoryCreator>();

			private System.Collections.Generic.Dictionary<Ice.Identity,Ferda.ModulesManager.BoxModule> boxModuleByProxyIdentity =
			new System.Collections.Generic.Dictionary<Ice.Identity,Ferda.ModulesManager.BoxModule>();

			private Helper lnkHelper;
			private Thread refreshThread;
			private Ice.Communicator communicator;
			//TODO serializovat a deserializovat
			private HelpFiles helpFiles = new HelpFiles();
			private GroupBoxFactoryCreator groupCreator = new GroupBoxFactoryCreator();
        }
    }
}
