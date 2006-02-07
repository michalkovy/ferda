using System;
using Ice;
using Ferda.Modules;
using System.Collections.Generic;
using System.Diagnostics;

namespace Ferda {
    namespace ModulesManager {

        /// <summary>
        /// Summary description for the class.
        /// </summary>

        public class ManagersLocatorI : ManagersLocatorDisp_
		{
			public class LocatorNotFoundException : System.Exception
			{
				
			}
			
			public ManagersLocatorI(Ice.ObjectAdapter adapter, Helper helper)
			{
				this.adapter = adapter;
				this.helper = helper;
				this.Refresh();
			}
			
			public void Refresh()
			{
				Debug.WriteLine("Starting Refresh in ManagersLocatorI...");
				Debug.WriteLine("Getting communicator...");
				Ice.Communicator communicator =
					this.adapter.getCommunicator();
				Debug.WriteLine("Getting QueryPrx...");
				ObjectPrx objectQuery = null;
				int size = 0;
				int i = 0;
				ObjectPrx[] creators = null;
				while(size == 0 && i<200)
				{
					objectQuery = communicator.stringToProxy("IceGrid/Query");
					if(objectQuery==null)
					{
						System.Threading.Thread.Sleep(50);
						i++;
						continue;
					}
					query = IceGrid.QueryPrxHelper.checkedCast(objectQuery);
					Debug.WriteLine("Getting BoxModuleFactoryCreators...");
					creators = this.findAllObjectsWithType("::Ferda::Modules::BoxModuleFactoryCreator");
					size = creators.Length;
					if(size == 0)
					{
						System.Threading.Thread.Sleep(50);
						i++;
					}
				}
				if(objectQuery==null)
					throw new LocatorNotFoundException();
				List<ObjectPrx> boxCreators = new List<ObjectPrx>();
				foreach(ObjectPrx creator in creators) boxCreators.Add(creator);
				creators = this.findAllObjectsWithType("::Ferda::Modules::PropertyBoxModuleFactoryCreator");
				foreach(ObjectPrx creator in creators)
				{
					if(!boxCreators.Contains(creator))
						boxCreators.Add(creator);
				}
				boxModuleFactoryCreatorsByIdentifier =
					new Dictionary<string,BoxModuleFactoryCreatorPrx>();
				Debug.WriteLine("Adding BoxModuleFactoryCreators...");
				foreach(ObjectPrx creatorUntyped in boxCreators)
				{
					Debug.WriteLine("Checked cast...");
					Debug.WriteIf(creatorUntyped==null,"CreatorUntyped  is null");
					bool bad = true;
					i = 0;
					BoxModuleFactoryCreatorPrx creator = null;
					while(bad)
					{
						try
						{
							creator =
								BoxModuleFactoryCreatorPrxHelper.checkedCast(creatorUntyped);
							if(creator!=null)
								bad = false;
						}
						catch
						{
							
						}
						if(bad)
						{
							if(i>50)
								throw new LocatorNotFoundException();
							System.Threading.Thread.Sleep(500);
							i++;
						}
					}
					Debug.WriteLine("Checked cast was OK");
					if(creator!=null)
					{
						Debug.Write("Adding that creator ");
						string id = creator.getIdentifier();
						Debug.WriteLine(id);
						if (boxModuleFactoryCreatorsByIdentifier.ContainsKey(id))
							throw new System.Exception("There are more boxes with identifier " + id + " !");
						boxModuleFactoryCreatorsByIdentifier.Add(
						creator.getIdentifier(), creator);
					}
				}
                refreshModules();
				Debug.WriteLine("Refresh in ManagersLocatorI ended");
			}

            private void refreshModules()
            {
                Debug.WriteLine("Adding SettingModules...");
                List<ObjectPrx> settingModules = new List<ObjectPrx>();
                ObjectPrx[] settingModulesArray = this.findAllObjectsWithType("::Ferda::Modules::SettingModule");
                foreach (ObjectPrx settingModule in settingModulesArray) settingModules.Add(settingModule);
                settingModulesArray = this.findAllObjectsWithType("::Ferda::Modules::SettingModuleWithStringAbility");
                foreach (ObjectPrx settingModule in settingModulesArray)
                {
                    if (!settingModules.Contains(settingModule))
                        settingModules.Add(settingModule);
                }

                settingModuleByIdentifier.Clear();
                foreach (ObjectPrx settingModuleUntyped in settingModules)
                {
                    SettingModulePrx settingModule =
                    SettingModulePrxHelper.checkedCast(settingModuleUntyped);

                    settingModuleByIdentifier[settingModule.getIdentifier()] = settingModule;
                }

                Debug.WriteLine("Adding ModulesForInteraction...");
                modulesForInteractionByBoxType.Clear();
                ObjectPrx[] modulesForInteraction = this.findAllObjectsWithType("::Ferda::Modules::ModuleForInteraction");
                foreach (ObjectPrx moduleForInteractionUntyped in modulesForInteraction)
                {
                    ModuleForInteractionPrx moduleForInteraction =
                    ModuleForInteractionPrxHelper.checkedCast(moduleForInteractionUntyped);
                    BoxType[] acceptedBoxTypes = moduleForInteraction.getAcceptedBoxTypes();
                    foreach (BoxType acceptedBoxType in acceptedBoxTypes)
                    {
                        List<ModuleForInteractionPrx> modulesInThisType;
                        if (!modulesForInteractionByBoxType.TryGetValue(acceptedBoxType, out modulesInThisType))
                        {
                            modulesInThisType = new List<ModuleForInteractionPrx>();
                            modulesForInteractionByBoxType[acceptedBoxType] = modulesInThisType;
                        }
                        modulesInThisType.Add(moduleForInteraction);
                    }
                }
            }
			
			public bool IsWithIceId(string iceId, BoxModuleFactoryCreatorPrx creator)
			{
				System.Collections.Specialized.StringCollection _functionsIceIds =
				new System.Collections.Specialized.StringCollection();
				_functionsIceIds.AddRange(
					creator.getBoxModuleFunctionsIceIds());
				return _functionsIceIds.Contains(iceId);
            }
			
			private bool hasSockets(NeededSocket[] neededSockets, BoxModuleFactoryCreatorPrx creator)
			{
				Dictionary<string,SocketInfo> socketsDict = new Dictionary<string,SocketInfo>();
				BoxModuleFactoryPrx factory = creator.createBoxModuleFactory(
					helper.LocalePrefs,
					helper.ManagersEnginePrx);
				SocketInfo[] sockets = factory.getSockets();
				factory.destroy();
				foreach (SocketInfo socket in sockets)
				{
					socketsDict[socket.name] = socket;
				}
				foreach (NeededSocket socketNeeded in neededSockets)
				{
					SocketInfo sockNfo;
                    if (!socketsDict.TryGetValue(socketNeeded.socketName, out sockNfo))
                        return false;
					BoxType[] boxTypeSeq = sockNfo.socketType;
					bool finded = false;
					foreach (BoxType boxType in boxTypeSeq)
					{
						if (boxType.functionIceId == socketNeeded.functionIceId)
						{
							finded = true;
							break;
						}
					}
					if (!finded) return false;
				}
				return true;
			}
			
			public bool HasBoxType(BoxType boxType, BoxModuleFactoryCreatorPrx creator)
			{
				return this.IsWithIceId(boxType.functionIceId, creator) &&
				this.hasSockets(boxType.neededSockets, creator);
			}
			
			public Dictionary<string, BoxModuleFactoryCreatorPrx> BoxModuleFactoryCreatorsByIdentifier
			{
				get {
					return boxModuleFactoryCreatorsByIdentifier;
				}
			}

            /// <summary>
            /// Method findBoxModuleCreatorByIdentifier
            /// </summary>
            /// <returns>A Ferda.Modules.BoxModuleFactoryCreatorPrx</returns>
            /// <param name="identifier">A string</param>
            /// <param name="__current">An Ice.Current</param>
            public override BoxModuleFactoryCreatorPrx findBoxModuleCreatorByIdentifier(String identifier, Current __current) {
				BoxModuleFactoryCreatorPrx result;
                return boxModuleFactoryCreatorsByIdentifier.TryGetValue(identifier, out result) ? result : null;
            }

            /// <summary>
            /// Method findBoxModuleCreatorByBoxType
            /// </summary>
            /// <returns>A Ferda.Modules.BoxModuleFactoryCreatorPrx</returns>
            /// <param name="moduleType">A  Ferda.Modules.BoxType</param>
            /// <param name="__current">An Ice.Current</param>
            public override BoxModuleFactoryCreatorPrx findBoxModuleCreatorByBoxType(BoxType moduleType, Current __current)
			{
                foreach(BoxModuleFactoryCreatorPrx creator in boxModuleFactoryCreatorsByIdentifier.Values)
				{
					if(this.HasBoxType(moduleType,creator))
					{
						return creator;
					}
				}
                return null;
            }

            /// <summary>
            /// Method findAllBoxModuleCreatorsWithBoxType
            /// </summary>
            /// <returns>A Ferda.Modules.BoxModuleFactoryCreatorPrx[]</returns>
            /// <param name="moduleType">A  Ferda.Modules.BoxType</param>
            /// <param name="__current">An Ice.Current</param>
            public override BoxModuleFactoryCreatorPrx[]
               findAllBoxModuleCreatorsWithBoxType(BoxType moduleType, Current __current)
			{
				List<BoxModuleFactoryCreatorPrx> result = new List<BoxModuleFactoryCreatorPrx>();
				foreach(BoxModuleFactoryCreatorPrx creator in boxModuleFactoryCreatorsByIdentifier.Values)
				{
					if(this.HasBoxType(moduleType,creator))
					{
						result.Add(creator);
					}
				}
				return result.ToArray();
            }

            /// <summary>
            /// Method findSettingModule
            /// </summary>
            /// <returns>A Ferda.Modules.SettingModulePrx</returns>
            /// <param name="settingModuleIdentifier">A string</param>
            /// <param name="__current">An Ice.Current</param>
            public override SettingModulePrx findSettingModule(String settingModuleIdentifier, Current __current) {
                SettingModulePrx result;
                if(settingModuleByIdentifier.TryGetValue(settingModuleIdentifier, out result))
                    return result;
                return null;
            }

            public override ModuleForInteractionPrx findModuleForInteraction(BoxModuleFactoryCreatorPrx creator, Current __current) {
                List<ModuleForInteractionPrx> result = new List<ModuleForInteractionPrx>();
                foreach (BoxType acceptedBoxType in modulesForInteractionByBoxType.Keys)
                {
                    if (HasBoxType(acceptedBoxType, creator))
                        return modulesForInteractionByBoxType[acceptedBoxType][0];
                }
                return null;
            }

            public override ModuleForInteractionPrx[] findAllModulesForInteraction(BoxModuleFactoryCreatorPrx creator, Current __current) {
                List<ModuleForInteractionPrx> result = new List<ModuleForInteractionPrx>();
                foreach (BoxType acceptedBoxType in modulesForInteractionByBoxType.Keys)
                {
                    if (HasBoxType(acceptedBoxType, creator))
                        result.AddRange(modulesForInteractionByBoxType[acceptedBoxType]);
                }
                return result.ToArray();
            }

            /// <summary>
            /// Method findAllObjectsWithType
            /// </summary>
            /// <returns>An Ice.ObjectPrx[]</returns>
            /// <param name="type">A string</param>
            /// <param name="__current">An Ice.Current</param>
            public override ObjectPrx[] findAllObjectsWithType(String type, Current __current) {
				ObjectPrx[] resultOne;
				try
				{
					resultOne = query.findAllObjectsByType(type);
				}
				catch(IceGrid.ObjectNotRegisteredException)
				{
					resultOne = new ObjectPrx[0];
				}
                List<ObjectPrx> result;
                if (additionalProxiesIceIds.TryGetValue(type, out result))
                {
                    ObjectPrx[] resc = new ObjectPrx[result.Count+resultOne.Length];
					result.CopyTo(resc);
					resultOne.CopyTo(resc,result.Count);
					return resc;   
                }
                else
                {
                    return resultOne;
                }
            }

            /// <summary>
            /// Method findObjectByType
            /// </summary>
            /// <returns>An Ice.ObjectPrx</returns>
            /// <param name="type">A string</param>
            /// <param name="__current">An Ice.Current</param>
            public override ObjectPrx findObjectByType(String type, Current __current) {
				ObjectPrx[] objectPrxArray = this.findAllObjectsWithType(type);
				if(objectPrxArray.Length==0)
				{
					return null;
				}
				else
				{
					return objectPrxArray[0];
				}
            }
			
			public void AddIceObjectProxies(List<ObjectPrx> objectPrxies)
			{
				foreach(ObjectPrx objectPrx in objectPrxies)
				{
					additionalProxies.Add(objectPrx);
					foreach(string iceId in objectPrx.ice_ids())
					{
						List<ObjectPrx> objects;
						if(additionalProxiesIceIds.TryGetValue(
							iceId,
							   out objects))
						{
							objects.Add(objectPrx);
						}
						else
						{
							objects = new List<ObjectPrx>();
							objects.Add(objectPrx);
							additionalProxiesIceIds[iceId]=objects;
						}
					}
				}
                if (additionalProxiesIceIds.ContainsKey("::Ferda::Modules::BoxModuleFactoryCreator"))
                    this.Refresh();
                else
                    if (additionalProxiesIceIds.ContainsKey("::Ferda::Modules::ModuleForInteraction")
                        || additionalProxiesIceIds.ContainsKey("::Ferda::Modules::SettingModule"))
                        refreshModules();
			}
			
			private IceGrid.QueryPrx query;
			private Ice.ObjectAdapter adapter;
			private List<ObjectPrx> additionalProxies =
				new List<ObjectPrx>();
            private Dictionary<string, SettingModulePrx> settingModuleByIdentifier =
                new Dictionary<string, SettingModulePrx>();
			private Dictionary<string,List<ObjectPrx>> additionalProxiesIceIds =
				new Dictionary<string,List<ObjectPrx>>();
			private Dictionary<string,BoxModuleFactoryCreatorPrx>
				boxModuleFactoryCreatorsByIdentifier;
			private Helper helper;
            private Dictionary<BoxType, List<ModuleForInteractionPrx>> modulesForInteractionByBoxType =
                new Dictionary<BoxType, List<ModuleForInteractionPrx>>();
        }
    }
}
