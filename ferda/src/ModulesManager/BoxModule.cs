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
using System.Collections.Generic;
using System.Collections.Specialized;
using Ferda.Modules;
using System.Diagnostics;

namespace Ferda
{
    namespace ModulesManager
    {

		/// <summary>
		/// Class for normal boxes.
		/// </summary>
		public class BoxModule : BoxModuleForManager
        {
			public BoxModule(Ferda.Modules.BoxModulePrx iceBoxModulePrx, BoxModuleFactoryCreator madeInCreator, Helper helper, ModulesManager modulesManager)
			{
				this.iceBoxModulePrx = iceBoxModulePrx;
				this.madeInCreator = madeInCreator;
				this.iceIdentity = Ice.Util.identityToString(
					iceBoxModulePrx.ice_getIdentity());
				this.helper = helper;
				this.modulesManager = modulesManager;
				this.managersLocatorI = this.helper.ManagersEngineI.ManagersLocatorI;
				foreach(PropertyInfo property in madeInCreator.Properties)
				{
					this.propertyNames.Add(property.name);
				}
				
				//string[] driver = this.MadeInCreator.PropertyDrivingLabel;
				//if (driver != null && driver.Length != 0)
				//{
				//    propertyDrivingLabel = driver[0];
				//}
			}
			
			/// <summary>
			/// Method destory
			/// </summary>
			public override void destroy()
			{
				this.madeInCreator.DestroyBoxModule(iceBoxModulePrx);
			}
			
			#region User newModule properties
			public override string UserName
            {
                get {
					if (this.userNameSet)
					{
						return this.userName;
					}
					else
					{
						string[] defaultUserLabel = this.iceBoxModulePrx.getDefaultUserLabel();
						if (defaultUserLabel.Length > 0 && !String.IsNullOrEmpty(defaultUserLabel[0]))
							return defaultUserLabel[0];
						else
						{
							return this.MadeInCreator.Label;
						}
						//else
						//{
						//    string result = this.GetPropertyString(propertyDrivingLabel);
						//    if (String.IsNullOrEmpty(result))
						//    {
						//        return this.MadeInCreator.Label;
						//    }
						//    else
						//    {
						//        return result;
						//    }
						//}
					}
				}
                set {
					this.userName = value;
					if(String.IsNullOrEmpty(value))
					{
						this.userNameSet = false;
					}
					else
					{
						this.userNameSet = true;
					}
				}
            }
			
			
			public override bool UserNameSet
			{
				get {
					return userNameSet;
				}
			}
			
			private string userName;
			private bool userNameSet = false;
			//private string propertyDrivingLabel;
			#endregion
			
			#region MadeInCreator
///
			///<supplierCardinality>1</supplierCardinality>
			///<clientCardinality>0..*</clientCardinality>
			///<undirected></undirected>
			public override IBoxModuleFactoryCreator MadeInCreator
            {
                get {
					return this.madeInCreator;
				}
            }
			
			private BoxModuleFactoryCreator madeInCreator;
			#endregion
			
			#region Connections
			public override void SetConnection(string socketName, Ferda.ModulesManager.IBoxModule otherModule)
			{
				this.SetUnvisibleConnection(socketName, otherModule);
				List<Ferda.ModulesManager.IBoxModule> modulesInSocket;
				if(!connectedModules.TryGetValue(socketName,out modulesInSocket))
				{
					modulesInSocket = new List<Ferda.ModulesManager.IBoxModule>();
					connectedModules[socketName] = modulesInSocket;
				}
				if(!modulesInSocket.Contains(otherModule))
				{
					modulesInSocket.Add(otherModule);
					BoxModuleForManager otherModuleForManager = otherModule
						as BoxModuleForManager;
					otherModuleForManager.AddConnectionTo(this);
				}
            }
	
			public override Ferda.ModulesManager.IBoxModule[] GetConnections(string socketName)
            {
				List<Ferda.ModulesManager.IBoxModule> modulesInSocket;
				if(connectedModules.TryGetValue(socketName, out modulesInSocket))
				{
					return modulesInSocket.ToArray();
				}
				else
					return new IBoxModule[0];
            }
	
			public override void RemoveConnection(string socketName, Ferda.ModulesManager.IBoxModule otherModule)
            {
				this.RemoveUnvisibleConnection(socketName, otherModule);
				List<Ferda.ModulesManager.IBoxModule> modulesInSocket;
				if(connectedModules.TryGetValue(socketName, out modulesInSocket))
				{
					modulesInSocket.Remove(otherModule);
				}
				BoxModuleForManager otherModuleForManager = otherModule
					as BoxModuleForManager;
				otherModuleForManager.RemoveConnectionTo(this);
            }
					
			private Dictionary<string,List<Ferda.ModulesManager.IBoxModule>> connectedModules =
				new Dictionary<string,List<Ferda.ModulesManager.IBoxModule>>();
			
			
			/// <summary>
			/// Method GetUnvisibleConnections
			/// </summary>
			/// <returns>An IBoxModuleForManager[]</returns>
			/// <param name="socketName">A  string</param>
			public override List<IBoxModule> GetUnvisibleConnections(String socketName)
			{
				List<IBoxModule> result = new List<IBoxModule>();
				foreach(BoxModulePrx prx in
					this.iceBoxModulePrx.getConnections(socketName))
				{
					BoxModule box = modulesManager.getBoxModuleByProxy(prx);
					if(box!=null && !connectedModules[socketName].Contains(box))
					{
						result.Add(box);
					}
				}
				return result;
			}
			
			/// <summary>
			/// Method SetUnvisibleConnection
			/// </summary>
			/// <param name="socketName">A  string</param>
			/// <param name="otherModule">An IBoxModuleForManager</param>
			public override void SetUnvisibleConnection(String socketName, IBoxModule otherModule)
			{
				BoxModule module = otherModule as BoxModule;
				if(module!=null)
				{
					this.iceBoxModulePrx.setConnection(socketName, module.IceBoxModulePrx);
				}
				else
				{
					GroupBox groupBox = otherModule as GroupBox;
					if(groupBox!=null)
					{
						foreach(IBoxModule box in groupBox.ConnectionsFrom())
						{
							this.SetUnvisibleConnection(socketName, box);
						}
					}
				}
			}
			
			/// <summary>
			/// Method RemoveUnvisibleConnection
			/// </summary>
			/// <param name="socketName">A  string</param>
			/// <param name="otherModule">An IBoxModuleForManager</param>
			public override void RemoveUnvisibleConnection(String socketName, IBoxModule otherModule)
			{
				BoxModule module = otherModule as BoxModule;
				if(module!=null)
				{
					this.iceBoxModulePrx.removeConnection(socketName, module.IceIdentity);
				}
				else
				{
					GroupBox groupBox = otherModule as GroupBox;
					if(groupBox!=null)
					{
						foreach(IBoxModule box in groupBox.ConnectionsFrom())
						{
							this.RemoveUnvisibleConnection(socketName, box);
						}
					}
				}
			}
			#endregion

            #region Properties
			public override void SetPropertyBool(string name, bool value)
            {
				this.iceBoxModulePrx.setProperty(name, new Ferda.Modules.BoolTI(value));
            }
			
            public override bool GetPropertyBool(string name)
            {
				return ((Ferda.Modules.BoolT)
					this.iceBoxModulePrx.getProperty(name)).boolValue;
            }
			
            public override void SetPropertyShort(string name, short value)
            {
				this.iceBoxModulePrx.setProperty(name, new Ferda.Modules.ShortTI(value));
            }
			
            public override short GetPropertyShort(string name)
            {
				return ((Ferda.Modules.ShortT)
					this.iceBoxModulePrx.getProperty(name)).shortValue;
            }
			
            public override void SetPropertyInt(string name, int value)
            {
				this.iceBoxModulePrx.setProperty(name, new Ferda.Modules.IntTI(value));
            }
			
            public override int GetPropertyInt(string name)
            {
				return ((Ferda.Modules.IntT)
					this.iceBoxModulePrx.getProperty(name)).intValue;
            }
			
            public override void SetPropertyLong(string name, long value)
            {
				this.iceBoxModulePrx.setProperty(name, new Ferda.Modules.LongTI(value));
            }
			
            public override long GetPropertyLong(string name)
            {
				return ((Ferda.Modules.LongT)
					this.iceBoxModulePrx.getProperty(name)).longValue;
            }
			
            public override void SetPropertyFloat(string name, float value)
            {
				this.iceBoxModulePrx.setProperty(name, new Ferda.Modules.FloatTI(value));
            }
			
            public override float GetPropertyFloat(string name)
            {
				return ((Ferda.Modules.FloatT)
					this.iceBoxModulePrx.getProperty(name)).floatValue;
            }
			
            public override void SetPropertyDouble(string name, double value)
            {
				this.iceBoxModulePrx.setProperty(name, new Ferda.Modules.DoubleTI(value));
            }
			
            public override double GetPropertyDouble(string name)
            {
				return ((Ferda.Modules.DoubleT)
					this.iceBoxModulePrx.getProperty(name)).doubleValue;
            }
			
            public override void SetPropertyString(string name, string value)
            {
				this.iceBoxModulePrx.setProperty(name, new Ferda.Modules.StringTI(value));
            }
			
            public override string GetPropertyString(string name)
            {
				return ((Ferda.Modules.StringT)
					this.iceBoxModulePrx.getProperty(name)).stringValue;
            }
			
			/// <summary>
			/// Sets date property <paramref name="name"/> to value <paramref name="value"/>.
			/// </summary>
			/// <param name="name">A string representing name of property which is of type date</param>
			/// <param name="value">A string value which has to be set to property <paramref name="name"/></param>
			/// <exception cref="T:Ferda.Modules.NameNotExistError">property with name
			/// <paramref name="name"/> was not found</exception>
			/// <exception cref="T:Ferda.Modules.BadTypeError">property is not of type date</exception>
			/// <exception cref="T:Ferda.Modules.ReadOnlyError">property is for read only</exception>
			public override void SetPropertyDate(String name, DateTime value)
			{
				this.iceBoxModulePrx.setProperty(name,
												 new Ferda.Modules.DateTI(value.Year,(short)(value.Month),(short)(value.Day)));
			}
			
			/// <summary>
			/// Gets value of date property with name <paramref name="name"/>
			/// </summary>
			/// <returns>A <see cref="T:System.DateTime"/> representing value of property</returns>
			/// <param name="name">A string representing name of property</param>
			/// <exception cref="T:Ferda.Modules.NameNotExistError">property with name
			/// <paramref name="name"/> and type date was not found</exception>
			public override DateTime GetPropertyDate(String name)
			{
				DateT value = ((Ferda.Modules.DateT)
					this.iceBoxModulePrx.getProperty(name));
				//if ((value.year == 0) && (value.month == 0) && (value.day == 0))
				try
				{
					return new DateTime(value.year, value.month, value.day);
				}
				catch(ArgumentOutOfRangeException)
				{
					return new DateTime();
				}
				
			}
			
			/// <summary>
			/// Sets DateTime property <paramref name="name"/> to value <paramref name="value"/>.
			/// </summary>
			/// <param name="name">A string representing name of property which is of type DateTime</param>
			/// <param name="value">A string value which has to be set to property <paramref name="name"/></param>
			/// <exception cref="T:Ferda.Modules.NameNotExistError">property with name
			/// <paramref name="name"/> was not found</exception>
			/// <exception cref="T:Ferda.Modules.BadTypeError">property is not of type DateTime</exception>
			/// <exception cref="T:Ferda.Modules.ReadOnlyError">property is for read only</exception>
			public override void SetPropertyDateTime(String name, DateTime value)
			{
				this.iceBoxModulePrx.setProperty(name,
												 new Ferda.Modules.DateTimeTI(value.Year,(short)(value.Month),(short)(value.Day), (short)(value.Hour), (short)(value.Minute), (short)(value.Second)));
			}
			
			/// <summary>
			/// Gets value of DateTime property with name <paramref name="name"/>
			/// </summary>
			/// <returns>A <see cref="T:System.DateTime"/> representing value of property</returns>
			/// <param name="name">A string representing name of property</param>
			/// <exception cref="T:Ferda.Modules.NameNotExistError">property with name
			/// <paramref name="name"/> and type date was not found</exception>
			public override DateTime GetPropertyDateTime(String name)
			{
				DateTimeT value = ((Ferda.Modules.DateTimeT)
					this.iceBoxModulePrx.getProperty(name));
				try
				{
					return new DateTime(value.year, value.month, value.day, value.hour, value.minute, value.second);
				}
				catch(ArgumentOutOfRangeException)
				{
					return new DateTime();
				}
			}
			
			/// <summary>
			/// Sets Time property <paramref name="name"/> to value <paramref name="value"/>.
			/// </summary>
			/// <param name="name">A string representing name of property which is of type Time</param>
			/// <param name="value">A string value which has to be set to property <paramref name="name"/></param>
			/// <exception cref="T:Ferda.Modules.NameNotExistError">property with name
			/// <paramref name="name"/> was not found</exception>
			/// <exception cref="T:Ferda.Modules.BadTypeError">property is not of type Time</exception>
			/// <exception cref="T:Ferda.Modules.ReadOnlyError">property is for read only</exception>
			public override void SetPropertyTime(String name, TimeSpan value)
			{
				this.iceBoxModulePrx.setProperty(name,
												 new Ferda.Modules.TimeTI((short)(value.Hours), (short)(value.Minutes), (short)(value.Seconds)));
			}
			
			/// <summary>
			/// Gets value of Time property with name <paramref name="name"/>
			/// </summary>
			/// <returns>A <see cref="T:System.TimeSpan"/> representing value of property</returns>
			/// <param name="name">A string representing name of property</param>
			/// <exception cref="T:Ferda.Modules.NameNotExistError">property with name
			/// <paramref name="name"/> and type date was not found</exception>
			public override TimeSpan GetPropertyTime(String name)
			{
				TimeT value = ((Ferda.Modules.TimeT)
					this.iceBoxModulePrx.getProperty(name));
				return new TimeSpan(value.hour, value.minute, value.second);
			}
			
            public override void SetPropertyOther(string name, Ferda.Modules.PropertyValue value)
            {
				this.iceBoxModulePrx.setProperty(name,value);
            }
			
            public override Ferda.Modules.PropertyValue GetPropertyOther(string name)
            {
				return this.iceBoxModulePrx.getProperty(name);
            }

            public override void GetProperty_async(AMI_BoxModule_getProperty callBack, string name)
            {
                this.iceBoxModulePrx.getProperty_async(callBack, name);
            }
			
			public override string GetPropertyOtherAbout(string name)
            {
				SettingModulePrx prx = this.managersLocatorI.findSettingModule(
                    this.madeInCreator.GetProperty(name).settingModuleIdentifier);
					if(prx != null)
					return prx.getPropertyAbout(this.GetPropertyOther(name));
				else
					return "";
            }

            public override string GetPropertyOtherAboutFromValue(string name, Ferda.Modules.PropertyValue value)
            {
                SettingModulePrx prx = this.managersLocatorI.findSettingModule(
                    this.madeInCreator.GetProperty(name).settingModuleIdentifier);
                if (prx != null)
                    return prx.getPropertyAbout(value);
                else
                    return "";
            }
			
			public override bool IsPossibleToSetWithAbout(string name)
            {
				SettingModulePrx prx = this.managersLocatorI.findSettingModule(
                    this.madeInCreator.GetProperty(name).settingModuleIdentifier);
				return prx != null &&
					prx.ice_isA("::Ferda::Modules::SettingModuleWithStringAbility");
            }
			
			public override void SetPropertyOtherAbout(string name, string value)
            {
				SettingModulePrx prx = this.managersLocatorI.findSettingModule(
                    this.madeInCreator.GetProperty(name).settingModuleIdentifier);
				SettingModuleWithStringAbilityPrx prxs = SettingModuleWithStringAbilityPrxHelper.checkedCast(prx);
				SetPropertyOther(name, prxs.convertFromStringAbout(value,helper.LocalePrefs));
            }
			
			/// <summary>
			/// Method GetPropertyOptions
			/// </summary>
			/// <returns>A Ferda.Modules.SelectString[]</returns>
			/// <param name="name">A  string</param>
			public override SelectString[] GetPropertyOptions(string name)
			{
				try
				{
					SelectString[] selectStrings = this.madeInCreator.GetProperty(name).selectBoxParams;
					if ((selectStrings!=null) && (selectStrings.Length > 0))
					{
						return selectStrings;
					}
				}
				catch(KeyNotFoundException)
				{
					throw new Ferda.Modules.NameNotExistError();
				}
				return this.iceBoxModulePrx.getPropertyOptions(name);
			}
			
			public override bool ArePropertyOptionsObligatory(string name)
			{
				try
				{
					SelectString[] selectStrings = this.madeInCreator.GetProperty(name).selectBoxParams;
					return (selectStrings!=null) && (selectStrings.Length > 0);
				}
				catch(KeyNotFoundException)
				{
					throw new Ferda.Modules.NameNotExistError();
				}
			}
			
            public override bool IsPropertySet(string name)
            {
				try
				{
					bool result = this.iceBoxModulePrx.isPropertySet(name);
					return result;
				}
				catch
				{
					return false;
				}
            }
			
			private bool? findPropertyDisabler(string propertyValue, string propertyName, SelectString[] findWhere)
			{
				foreach(SelectString selectString in findWhere)
				{
					if(selectString.name == propertyValue)
					{
						foreach (string disableProperty in selectString.disableProperties)
						{
							if(disableProperty == propertyName) return true;
						}
						return false;
					}
				}
				return null;
			}
			
			/// <summary>
			/// Method IsPropertyReadOnly
			/// </summary>
			/// <returns>A bool</returns>
			/// <param name="name">A  string</param>
			public override bool IsPropertyReadOnly(String name)
			{
				if(!this.propertyNames.Contains(name))
					throw new Ferda.Modules.NameNotExistError();
				bool result = this.MadeInCreator.GetProperty(name).readOnly;
				if(result == false)
				{
					foreach(PropertyInfo propertyInfo in this.MadeInCreator.Properties)
					{
						if(propertyInfo.typeClassIceId == "::Ferda::Modules::StringT")
						{
							string propertyValue = this.GetPropertyString(propertyInfo.name);
							bool? retValue = findPropertyDisabler(propertyValue, name,
												 propertyInfo.selectBoxParams);
							if(retValue == null)
							{
								retValue = findPropertyDisabler(propertyValue, name,
																this.GetPropertyOptions(name));
							}
							if(retValue != null)
							{
								return (retValue == true);
							}
						}
					}
				}
				return result;
			}

            public override bool IsPropertySetWithSettingModule(string name)
            {
                return !(String.IsNullOrEmpty(
                    this.madeInCreator.GetProperty(name).settingModuleIdentifier));
            }
			
            public override string RunSetPropertyOther(string name)
            {
				SettingModulePrx prx = this.managersLocatorI.findSettingModule(
                    this.madeInCreator.GetProperty(name).settingModuleIdentifier);
				string about;
				PropertyValue value = prx.run(this.GetPropertyOther(name),
						this.iceBoxModulePrx,
						this.helper.LocalePrefs,
						this.helper.ManagersEnginePrx,
						out about);
				SetPropertyOther(name, value);
				return about;
            }
			
			public override void SetPropertySocking(string propertyName, bool socked)
			{
				if(!this.propertyNames.Contains(propertyName)) return;
				if(socked)
				{
					if(!this.sockedProperties.Contains(propertyName))
						this.sockedProperties.Add(propertyName);
				}
				else
				{
					if(this.sockedProperties.Contains(propertyName))
						this.sockedProperties.Remove(propertyName);
				}
			}
			
			public override bool GetPropertySocking(string propertyName)
			{
				return this.sockedProperties.Contains(propertyName);
			}
			#endregion
			
			#region BoxType and sockets
			public override System.Collections.Specialized.StringCollection GetFunctionsIceIds()
			{
				System.Collections.Specialized.StringCollection _functionsIceIds =
				new System.Collections.Specialized.StringCollection();
				_functionsIceIds.AddRange(
					this.iceBoxModulePrx.getFunctionsIceIds());
				return _functionsIceIds;
            }
			
			public override Ferda.Modules.SocketInfo[] Sockets
			{
                get {
					SocketInfo[] si1 = this.MadeInCreator.Sockets;
					List<SocketInfo> result = new List<SocketInfo>();
					foreach(SocketInfo socket in si1)
					{
						string socketName = socket.name;
						if((!this.propertyNames.Contains(socketName)) ||
						   (this.sockedProperties.Contains(socketName)))
						{
							result.Add(socket);
						}
					}
					SocketInfo[] si2 = iceBoxModulePrx.getAdditionalSockets();
					foreach(SocketInfo socket in si2)
					{
						result.Add(socket);
					}
					return result.ToArray();
				}
            }
			
			protected bool hasSockets(NeededSocket[] neededSockets)
			{
				Dictionary<string,SocketInfo> socketsDict = new Dictionary<string,SocketInfo>();
				foreach (SocketInfo socket in Sockets)
				{
					socketsDict[socket.name] = socket;
				}
				foreach (NeededSocket socketNeeded in neededSockets)
				{
					SocketInfo sockNfo;
					if(!socketsDict.TryGetValue(socketNeeded.socketName,out sockNfo))
					{
						return false;
					}
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
			
			protected bool isSocketConnected(string socket)
			{
				return (this.GetConnections(socket).Length != 0 || this.IsPropertySet(socket));
			}
			
			protected bool hasConnectedSockets(string[] sockets)
			{
				foreach (string socket in sockets)
				{
					if (!isSocketConnected(socket)) return false;
				}
				return true;
			}
			
			/// <summary>
			/// Method HasBoxType
			/// </summary>
			/// <returns>A bool</returns>
			/// <param name="boxType">A  Ferda.Modules.BoxType</param>
			public override bool HasBoxType(BoxType boxType)
			{
				return this.IsWithIceId(boxType.functionIceId) &&
					this.hasSockets(boxType.neededSockets);
			}
			#endregion
			
            #region Action and interaction
			public override void RunAction(string actionName)
            {
				iceBoxModulePrx.runAction(actionName);
            }
			
			public override void RunAction_async(AMI_BoxModule_runAction callBack, string actionName)
            {
				iceBoxModulePrx.runAction_async(callBack, actionName);
            }
			
			public override bool IsPossibleToRunAction(string actionName)
			{
				foreach(ActionInfo actionInfo in this.madeInCreator.Actions)
				{
					if(actionInfo.name == actionName)
					{
						if(actionInfo.neededConnectedSockets.Length == 0) return true;
						foreach(string[] sockets in actionInfo.neededConnectedSockets)
						{
							if(this.hasConnectedSockets(sockets)) return true;
						}
						return false;
					}
				}
				throw new NameNotExistError();
			}
			
			public override bool IsPossibleToRunModuleForInteraction(String moduleIceIdentity)
			{
				ModuleForInteractionPrx[] prxs =
				this.managersLocatorI.findAllModulesForInteraction(
					this.madeInCreator.CreatorPrx);
				foreach (ModuleForInteractionPrx prx in prxs)
				{
					if (Ice.Util.identityToString(prx.ice_getIdentity()) == moduleIceIdentity)
					{
						if (!hasConnectedSockets(prx.getNeededConnectedSockets()))
						{
							return false;
						}
					}
				}
				return true;
			}
			
            public override void RunModuleForInteraction(string moduleIceIdentity)
            {
				ModuleForInteractionPrx[] prxs =
				this.managersLocatorI.findAllModulesForInteraction(
					this.madeInCreator.CreatorPrx);
				foreach (ModuleForInteractionPrx prx in prxs)
				{
					if (Ice.Util.identityToString(prx.ice_getIdentity()) == moduleIceIdentity)
					{
						if (!hasConnectedSockets(prx.getNeededConnectedSockets())) break;
						prx.run(this.iceBoxModulePrx,
								this.helper.LocalePrefs,
								this.helper.ManagersEnginePrx);
						break;
					}
				}
            }
			
            public override ModuleForInteractionInfo[] ModuleForInteractionInfos
            {
				
                get {
					List<ModuleForInteractionInfo> modulesForInteraction =
						new List<ModuleForInteractionInfo>();
					foreach(ModuleForInteractionPrx prx in this.managersLocatorI.findAllModulesForInteraction(
						this.madeInCreator.CreatorPrx))
					{
						modulesForInteraction.Add(new ModuleForInteractionInfo(prx,helper));
					}
					return modulesForInteraction.ToArray();
                }
            }
			#endregion
			
			public override Ferda.Modules.ModulesAskingForCreation[] ModulesAskingForCreation
            {
                get {
					return this.iceBoxModulePrx.getModulesAskingForCreation();
				}
            }
			
			public override Ferda.Modules.DynamicHelpItem[] DynamicHelpItems
			{
                get
				{
                    return iceBoxModulePrx.getDynamicHelpItems();
                }
            }
			
			#region internal newModule - BoxModulePrx and its identity
			protected internal  Ferda.Modules.BoxModulePrx IceBoxModulePrx
            {
                get {
					return this.iceBoxModulePrx;
				}
            }
			
            private Ferda.Modules.BoxModulePrx iceBoxModulePrx;
			
			protected internal string IceIdentity
			{
				set {
					iceIdentity = value;
				}
				
				get {
					return iceIdentity;
				}
			}
			
			private void lockUnlockRecursive(IBoxModule box, BoxModule lockBox, Stack<IBoxModule> stack, bool locking)
			{
				BoxModule boxModule = box as BoxModule;
				if(boxModule!=null)
				{
                    if(locking)
                        boxModule.LockByBox(lockBox);
                    else
                        boxModule.UnlockByBox(lockBox);
				}
				else
				{
					foreach(IBoxModule newBox in box.ConnectionsFrom())
					{
						if(!stack.Contains(newBox))
						{
							stack.Push(newBox);
                            lockUnlockRecursive(newBox, lockBox, stack, locking);
							stack.Pop();
						}
					}
				}
			}
			
			protected internal void LockByBox(BoxModule box)
			{
				lock(locks)
				{
					if(!locks.Contains(box))
					{
						locks.Add(box);
                        //Debug.WriteLine("Vstupuji " + this.UserName);
						Stack<IBoxModule> stack = new Stack<IBoxModule>();
						foreach(IBoxModule otherBox in this.ConnectionsFrom())
						{
							stack.Push(otherBox);
                            lockUnlockRecursive(otherBox, box, stack, true);
							stack.Pop();
						}
					}
				}
			}
			
			protected internal void Lock()
			{
				lock(lockObject)
				{
					lockNumber++;
					if(lockNumber == 1)
					{
						LockByBox(this);
					}
				}
			}
			
			protected internal void UnlockByBox(BoxModule box)
			{
				lock(locks)
				{
					if(locks.Contains(box))
					{
						locks.Remove(box);
                        //Debug.WriteLine("Opoustim " + this.UserName);
						Stack<IBoxModule> stack = new Stack<IBoxModule>();
						foreach(IBoxModule otherBox in this.ConnectionsFrom())
						{
							stack.Push(otherBox);
                            lockUnlockRecursive(otherBox, box, stack, false);
							stack.Pop();
						}
					}
				}
			}
			
			protected internal void Unlock()
			{
				lock(lockObject)
				{
					if(lockNumber>0) lockNumber--;
					if(lockNumber == 0)
					{
						UnlockByBox(this);
					}
				}
			}

            protected internal void UnlockAll()
            {
                lock (lockObject)
                {
                    lockNumber = 0;
                    lock(locks)
				    {
                        //Debug.WriteLine("Opoustim " + this.UserName);
                        locks.Clear();
                    }
                }
            }
			
			/// <summary>
			/// Method TryWriteEnter
			/// </summary>
			/// <returns>A bool</returns>
			public override bool TryWriteEnter()
			{
                System.Threading.Monitor.Enter(locks);
                bool vystup = (locks.Count == 0);
                
                //Debug.WriteLine("Vstupuji by try " + this.UserName + " " + vystup.ToString());

                if (vystup)
                    return true;
                else
                {
                    System.Threading.Monitor.Exit(locks);
                    return false;
                }
			}
			
			/// <summary>
			/// Method WriteExit
			/// </summary>
			/// <returns>A bool</returns>
			public override void WriteExit()
			{
                //Debug.WriteLine("Opoustim po try " + this.UserName);
                System.Threading.Monitor.Exit(locks);
			}
			
			private int lockNumber = 0;
			private Object lockObject = new Object();
			private List<BoxModule> locks = new List<BoxModule>();
			
			private string iceIdentity;
			#endregion
			
			private Helper helper;
			private ModulesManager modulesManager;
			private ManagersLocatorI managersLocatorI;
			private StringCollection sockedProperties = new StringCollection();
			private StringCollection propertyNames = new StringCollection();
        }
    }
}
