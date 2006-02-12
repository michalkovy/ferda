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
using Ferda.Modules;
using System.Collections.Specialized;namespace Ferda.ModulesManager
{
	public class GroupBoxFactoryCreator : IBoxModuleFactoryCreator
	{
				
		/// <summary>
		/// Method GetBoxModuleFunctionsIceIds
		/// </summary>
		/// <returns>A System.Collections.Specialized.StringCollection</returns>
		public StringCollection GetBoxModuleFunctionsIceIds()
		{
			return new StringCollection();
		}
		
		/// <summary>
		/// Method IsWithIceId
		/// </summary>
		/// <returns>A bool</returns>
		/// <param name="iceId">A string</param>
		public bool IsWithIceId(String iceId)
		{
			return true;
		}
		
		/// <summary>
		/// Method HasBoxType
		/// </summary>
		/// <returns>A bool</returns>
		/// <param name="boxType">A  BoxType</param>
		public bool HasBoxType(BoxType boxType)
		{
			return true;
		}
		
		/// <summary>
		/// Method CreateBoxModule
		/// </summary>
		/// <returns>A Ferda.ModulesManager.BoxModule</returns>
		public IBoxModule CreateBoxModule()
		{
			return new GroupBox(this);
		}
		
		/// <summary>
		/// Method GetHelpFilePath
		/// </summary>
		/// <returns>A String</returns>
		/// <param name="identifier">A string</param>
		public String GetHelpFilePath(String identifier)
		{
			return null;
		}
		
		public ActionInfo[] Actions
		{
			get {
				return new ActionInfo[0];
			}
		}
		
		public byte[] Icon
		{
			get {
				//TODO
				return new byte[0];
			}
		}
		
		public SocketInfo[] Sockets
		{
			get {
				//TODO doplnit lepe newModule o socketu - lokalizace, design
				SocketInfo socket = new SocketInfo();
				socket.design = "identifier";
				socket.hint = "Give there any boxes";
				socket.label = "items";
				socket.moreThanOne = true;
				socket.name = "items";
				socket.settingProperties = new String[0];
				BoxType boxType = new BoxType();
				boxType.functionIceId = "::Ice::Object";
				boxType.neededSockets = new NeededSocket[0];
				socket.socketType = new BoxType[]{ boxType };
				return new SocketInfo[]{ socket };
		}
		}
		
		
		/// <summary>
		/// Gets information about socket in this boxes created by this creator.
		/// </summary>
		/// <value>A <see cref="T:Ferda.Modules.SocketInfo"/>
		/// representing information about socket</value>
		public SocketInfo GetSocket(String socketName)
		{
			if(socketName == "items")
			{
				return this.Sockets[0];
			}
			else
			{
				throw new KeyNotFoundException();
			}
		}
		
		public String Design
		{
			get {
				// TODO
				return "";
			}
		}
		
		public String[] BoxCategories
		{
			get {
				// TODO lokalizace
				return new String[1]{ "other" };
			}
		}
		
		public Dictionary<String, HelpFileInfo> HelpFileInfoSeq
		{
			get {
				return new Dictionary<String, HelpFileInfo>();
			}
		}
		
		//public String[] PropertyDrivingLabel
		//{
		//    get {
		//        return new String[0];
		//    }
		//}
		
		public String Identifier
		{
			get {
				return "group";
			}
		}
		
		public PropertyInfo[] Properties
		{
			get {
				return new PropertyInfo[0];
			}
		}
		
		/// <summary>
		/// Method GetProperty
		/// </summary>
		/// <returns>A Ferda.Modules.PropertyInfo</returns>
		/// <param name="name">A string</param>
		public PropertyInfo GetProperty(String name)
		{
			throw new NameNotExistError();
		}
		
		public String Label
		{
			get {
				//TODO lokalizace
				return "Group";
			}
		}
		
		public String Hint
		{
			get {
				//TODO lokalizace
				return "Group of box modules";
			}
		}
	}
}
