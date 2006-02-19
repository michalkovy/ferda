// 
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
using Ferda.Modules;
using System.Diagnostics;
using System.Collections.Generic;

namespace Ferda {
    namespace ModulesManager {

		/// <summary>
		/// Summary description for the class.
		/// </summary>
		public class BoxModuleFactoryCreator : IBoxModuleFactoryCreator
		{

			public BoxModuleFactoryCreator(Helper lnkHelper, Ferda.Modules.BoxModuleFactoryCreatorPrx creatorPrx, ModulesManager modulesManager, HelpFiles helpFiles)
			{
				Debug.WriteLine("Entering boxModuleFactoryCreator constructor");
				this.creatorPrx = creatorPrx;
				this.lnkHelper = lnkHelper;
				this.modulesManager = modulesManager;
				this.helpFiles = helpFiles;
				_functionsIceIds = new System.Collections.Specialized.StringCollection();
				Debug.WriteLine("Getting functions ice ids...");
				_functionsIceIds.AddRange(
					this.creatorPrx.getBoxModuleFunctionsIceIds());
				Debug.WriteLine("Creating factory...");
				CreateBoxModuleFactory();
				Debug.WriteLine("Getting sockets...");
				sockets = this.factoryPrx.getSockets();
				foreach(SocketInfo socketNfo in sockets)
				{
					this.socketsByName[socketNfo.name] = socketNfo;
				}
				Debug.WriteLine("Getting properties...");
				PropertyInfo[] propNfos = factoryPrx.getProperties();
				foreach(PropertyInfo propNfo in propNfos)
				{
					this.properties[propNfo.name] = propNfo;
				}
				Debug.WriteLine("Getting actions...");
				actions = factoryPrx.getActions();
				Debug.WriteLine("Getting help file infos...");
				foreach(Ferda.Modules.HelpFileInfo nfo in factoryPrx.getHelpFileInfoSeq())
				{
					helpFileInfos.Add(nfo.identifier,nfo);
				}
				//propertyDrivingLabel = factoryPrx.getPropertyDrivingLabel();
				Debug.WriteLine("Destroying factory...");
				DestroyFactoryIfEmpty();

				label = this.creatorPrx.getLabel(this.lnkHelper.LocalePrefs);
				identifier = this.creatorPrx.getIdentifier();
				icon = this.creatorPrx.getIcon();
                design = this.creatorPrx.getDesign();
                hint = this.creatorPrx.getHint(this.lnkHelper.LocalePrefs);

                //box categories
                string[] categories = this.creatorPrx.getBoxCategories();
                localizedCategories = new string[categories.Length];
                int i = 0;
                foreach (string category in categories)
                {
                    localizedCategories[i] =
                        modulesManager.GetBoxModuleCategoryLocalization(category, identifier);
                    i++;
                }
                
				Debug.WriteLine("Leaving boxModuleFactoryCreator constructor");
			}
			
			protected internal Ferda.Modules.BoxModuleFactoryCreatorPrx CreatorPrx
			{
				get {
					return creatorPrx;
				}
			}

			#region Creator functions
			
            protected internal void CreateBoxModuleFactory()
			{
				factoryPrx = creatorPrx.createBoxModuleFactory(
					this.lnkHelper.LocalePrefs,
					this.lnkHelper.ManagersEnginePrx);
				this.lnkHelper.BoxModuleIceFactories.AddFactory(factoryPrx);
            }
			
			private System.Collections.Specialized.StringCollection  _functionsIceIds;
			
			public System.Collections.Specialized.StringCollection GetBoxModuleFunctionsIceIds()
			{
				return _functionsIceIds;
            }
						
            public bool IsWithIceId(string iceId)
			{
				return _functionsIceIds.Contains(iceId);
            }
			
			private bool hasSockets(NeededSocket[] neededSockets)
			{
				Dictionary<string,SocketInfo> socketsDict = new Dictionary<string,SocketInfo>();
				foreach(SocketInfo socket in sockets)
				{
					socketsDict[socket.name]=socket;
				}
				foreach(NeededSocket socketNeeded in neededSockets)
				{
					SocketInfo sockNfo;
					try
					{
						sockNfo = socketsDict[socketNeeded.socketName];
					}
					catch(KeyNotFoundException)
					{
						return false;
					}
					BoxType[] boxTypeSeq = sockNfo.socketType;
					bool finded = false;
					foreach(BoxType boxType in boxTypeSeq)
					{
						if(boxType.functionIceId == socketNeeded.functionIceId)
						{
							finded = true;
							break;
						}
					}
					if(!finded) return false;
				}
				return true;
			}
			
			public bool HasBoxType(BoxType boxType)
			{
				return this.IsWithIceId(boxType.functionIceId) &&
				this.hasSockets(boxType.neededSockets);
			}
			
			public string[] BoxCategories
			{
				get{	
					return localizedCategories;
				}
            }

            private string[] localizedCategories;
			
			public byte[] Icon
			{
                get {
                    return icon;
				}
            }

            private byte[] icon;

            public string Design
			{
                get {
                    return design;
				}
            }

            private string design;

            public String Hint
			{
                get {
                    return hint;
                }
            }

            private string hint;

            public String Identifier
			{
                get {
                    return identifier;
				}
            }

            private string identifier;
			
			public String Label
			{
                get {
                    return label;
				}
            }

            private string label;
			#endregion
			
			
            #region factory methods
			protected internal bool CreateFactoryIfNull()
			{
				if(factoryPrx==null)
				{
					this.CreateBoxModuleFactory();
					return true;
				}
				return false;
			}
			
			protected internal void DestroyFactoryIfEmpty()
			{
				if(factoryPrx.destroyIfEmpty())
				{
					this.lnkHelper.BoxModuleIceFactories.RemoveFactory(factoryPrx);
					factoryPrx=null;
				}
			}
			
			protected internal void DestroyBoxModule(Ferda.Modules.BoxModulePrx box)
			{
				//TODO: osetrit null?
				modulesManager.removeBoxModuleWithProxy(box);
				this.factoryPrx.destroyBoxModule(Ice.Util.identityToString(box.ice_getIdentity()));
				DestroyFactoryIfEmpty();
			}
			
			public IBoxModule CreateBoxModule()
			{
				CreateFactoryIfNull();
				BoxModule result = new BoxModule(factoryPrx.createBoxModule(), this, this.lnkHelper, modulesManager);
				modulesManager.addBoxModuleWithProxy(result,result.IceBoxModulePrx);
				return result;
            }
			
			protected internal void DestroyFactoryIfIs()
			{
				if(factoryPrx!=null) DestroyFactory();
			}
			
			protected internal void DestroyFactory()
			{
				this.lnkHelper.BoxModuleIceFactories.RemoveFactory(factoryPrx);
				factoryPrx.destroy();
				factoryPrx = null;
			}
			
			public SocketInfo[] Sockets
			{
                get {
                    return sockets;
				}
            }

            private SocketInfo[] sockets;
			
			/// <summary>
			/// Gets information about socket in this boxes created by this creator.
			/// </summary>
			/// <value>A <see cref="T:Ferda.Modules.SocketInfo"/>
			/// representing information about socket</value>
			public SocketInfo GetSocket(String socketName)
			{
				return this.socketsByName[socketName];
			}
			
			public Ferda.Modules.ActionInfo[] Actions
			{
                get {
                    return actions;
				}
            }

            private Ferda.Modules.ActionInfo[] actions;
			
			public Ferda.Modules.PropertyInfo[] Properties
			{
                get {
					PropertyInfo[] result = new PropertyInfo[properties.Count];
					properties.Values.CopyTo(result,0);
                    return result;
				}
            }
			
			public Ferda.Modules.PropertyInfo GetProperty(string name)
			{
				return properties[name];
			}
			
			private System.Collections.Generic.Dictionary<string,Ferda.Modules.PropertyInfo> properties =
			new System.Collections.Generic.Dictionary<string,Ferda.Modules.PropertyInfo>();
			
			//public String[] PropertyDrivingLabel
			//{
			//    get {
			//        return propertyDrivingLabel;
			//    }
			//}

            //private string[] propertyDrivingLabel;

			public Dictionary<string,HelpFileInfo> HelpFileInfoSeq
			{
                get {
					return helpFileInfos;
				}
            }
			
			public String GetHelpFilePath(string identifier)
			{
				int? actualVersion = helpFiles.GetHelpFileVersion(identifier);
				if(actualVersion == null ||
				   actualVersion.Value < helpFileInfos[identifier].version)
				{
					helpFiles.SaveHelpFile(identifier,
										   helpFileInfos[identifier].version,
										   creatorPrx.getHelpFile(identifier));
				}
				return helpFiles.GetHelpFilePath(identifier);
			}
			#endregion
			
			
///
///<directed></directed>
            private Helper lnkHelper;
			
			private Ferda.Modules.BoxModuleFactoryCreatorPrx creatorPrx;
			
			private Ferda.Modules.BoxModuleFactoryPrx factoryPrx;	
			
			private System.Collections.Generic.Dictionary<string,Ferda.Modules.SocketInfo> socketsByName =
			new System.Collections.Generic.Dictionary<string,Ferda.Modules.SocketInfo>();
				
			private ModulesManager modulesManager;
			
			private HelpFiles helpFiles;
			
			private Dictionary<string,HelpFileInfo> helpFileInfos = new Dictionary<string,HelpFileInfo>();
		}
    }
}
