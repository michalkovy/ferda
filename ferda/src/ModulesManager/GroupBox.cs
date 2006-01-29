using Ferda.Modules;
using System.Collections.Specialized;
using System.Collections.Generic;
using System;

namespace Ferda.ModulesManager
{
	/// <summary>
	/// Specialized box - group box.
	/// </summary>
	public class GroupBox : BoxModuleForManager
	{
		
		///<summary>
		/// Constructor
		/// </summary>
		/// <param name="creator">A  GroupBoxFactoryCreator</param>
		public GroupBox(GroupBoxFactoryCreator creator)
		{
			this.creator = creator;
		}
		
		/// <summary>
		/// Method GetFunctionsIceIds
		/// </summary>
		/// <returns>A System.Collections.Specialized.StringCollection</returns>
		public override StringCollection GetFunctionsIceIds()
		{
			StringCollection result = new StringCollection();
			List<IBoxModule> connectionsFrom = new List<IBoxModule>(this.ConnectionsFrom());
			if(connectionsFrom.Count > 0)
			{
				List<StringCollection> connectedIceIds = new List<StringCollection>();
				StringCollection testStrings = connectionsFrom[0].GetFunctionsIceIds();
				connectionsFrom.RemoveAt(0);
				foreach(IBoxModule box in connectionsFrom)
				{
					connectedIceIds.Add(box.GetFunctionsIceIds());
				}
				foreach(string testString in testStrings)
				{
					bool add = true;
					foreach(StringCollection strings in connectedIceIds)
					{
						if(!strings.Contains(testString))
						{
							add = false;
							break;
						}
					}
					if(add) result.Add(testString);
				}
			}
			return result;
		}

		
		/// <summary>
		/// Method HasBoxType
		/// </summary>
		/// <returns>A bool</returns>
		/// <param name="boxType">A  BoxType</param>
		public override bool HasBoxType(BoxType boxType)
		{
			foreach(IBoxModule box in this.ConnectionsFrom())
			{
				if(!box.HasBoxType(boxType)) return false;
			}
			return true;
		}
		
		private struct addedConnection
		{
			
			private BoxModuleForManager to;
			private BoxModuleForManager box;
			private string addedToSocket;

			///<summary>
			/// Constructor
			/// </summary>
			/// <param name="to">An IBoxModuleForManager</param>
			/// <param name="box">An IBoxModuleForManager</param>
			/// <param name="addedToSocket">A string</param>
			public addedConnection(BoxModuleForManager to, BoxModuleForManager box, string addedToSocket)
			{
				this.to = to;
				this.box = box;
				this.addedToSocket = addedToSocket;
			}
			
			public BoxModuleForManager To
			{
				set {
					to = value;
				}
				
				get {
					return to;
				}
			}
			
			public BoxModuleForManager Box
			{
				set {
					box = value;
				}
				
				get {
					return box;
				}
			}
			
			public string AddedToSocket
			{
				set {
					addedToSocket = value;
				}
				
				get {
					return addedToSocket;
				}
			}
			
		}
		
		public bool TestCycleRecursive(IBoxModule otherModule)
		{
			foreach(IBoxModule box in this.ConnectionsFrom())
			{
				if(testCycle(otherModule,box)) return true;
			}
			return false;
		}
		
		private bool testCycle(IBoxModule firstModule, IBoxModule otherModule)
		{
			if(firstModule==otherModule) return true;
			GroupBox otherGroup = otherModule as GroupBox;
			if(otherGroup==null) return false;
			return otherGroup.TestCycleRecursive(firstModule);
		}
		
		private List<IBoxModule> modulesInSocket = new List<IBoxModule>();
	
		/// <summary>
		/// Method SetConnection
		/// </summary>
		/// <param name="socketName">A string</param>
		/// <param name="otherModule">A  Ferda.ModulesManager.IBoxModule</param>
		public override void SetConnection(String socketName, IBoxModule otherModule)
		{
			if (testCycle(this, otherModule)) throw new BadTypeError();
			this.SetUnvisibleConnection(socketName, otherModule);
			if(!modulesInSocket.Contains(otherModule))
			{
				modulesInSocket.Add(otherModule);
				BoxModuleForManager otherModuleForManager = otherModule
					as BoxModuleForManager;
				otherModuleForManager.AddConnectionTo(this);
			}
		}
		
		/// <summary>
		/// Method GetConnections
		/// </summary>
		/// <returns>A Ferda.ModulesManager.IBoxModule[]</returns>
		/// <param name="socketName">A string</param>
		public override IBoxModule[] GetConnections(String socketName)
		{
			return modulesInSocket.ToArray();
		}
		
		/// <summary>
		/// Method RemoveConnection
		/// </summary>
		/// <param name="socketName">A string</param>
		/// <param name="otherModule">A  Ferda.ModulesManager.IBoxModule</param>
		public override void RemoveConnection(String socketName, IBoxModule otherModule)
		{
			this.RemoveUnvisibleConnection(socketName, otherModule);
			modulesInSocket.Remove(otherModule);
			BoxModuleForManager otherModuleForManager = otherModule
				as BoxModuleForManager;
			otherModuleForManager.RemoveConnectionTo(this);
		}
		
		
		/// <summary>
		/// Method GetUnvisibleConnections
		/// </summary>
		/// <returns>An IBoxModuleForManager[]</returns>
		/// <param name="socketName">A string</param>
		public override List<IBoxModule> GetUnvisibleConnections(String socketName)
		{
			return null;
		}
		
		/// <summary>
		/// Method SetUnvisibleConnection
		/// </summary>
		/// <param name="socketName">A string</param>
		/// <param name="otherModule">An IBoxModuleForManager</param>
		public override void SetUnvisibleConnection(String socketName, IBoxModule otherModule)
		{
			BoxModuleForManager oModule = otherModule as BoxModuleForManager;
			List<addedConnection> added = new List<addedConnection>();
			try
			{
				foreach(IBoxModule box in this.ConnectedTo())
				{
					BoxModuleForManager boxM = box as BoxModuleForManager;
					foreach(SocketInfo socket in box.Sockets)
					{
						foreach(IBoxModule boxO in
						boxM.GetConnections(socket.name))
						{
							if(boxO == this)
							{
								boxM.SetUnvisibleConnection(socket.name,oModule);
								added.Add(new addedConnection(boxM,oModule,socket.name));
								break;
							}
						}
					}
				}
			}
			catch(BadTypeError)
			{
				foreach(addedConnection con in added)
				{
					con.To.RemoveUnvisibleConnection(con.AddedToSocket,con.Box);
				}
				throw new BadTypeError();
			}
		}
		
		/// <summary>
		/// Method RemoveUnvisibleConnection
		/// </summary>
		/// <param name="socketName">A string</param>
		/// <param name="otherModule">An IBoxModuleForManager</param>
		public override void RemoveUnvisibleConnection(String socketName, IBoxModule otherModule)
		{
			BoxModuleForManager oModule = otherModule as BoxModuleForManager;
			foreach(IBoxModule box in this.ConnectedTo())
			{
				BoxModuleForManager boxM = box as BoxModuleForManager;
				foreach(SocketInfo socket in box.Sockets)
				{
					foreach(IBoxModule boxO in
					boxM.GetConnections(socket.name))
					{
						if(boxO == this)
						{
							try
							{
								boxM.RemoveUnvisibleConnection(socket.name,oModule);
							}
							catch(NameNotExistError){}
							catch(ConnectionNotExistError){}
							break;
						}
					}
				}
			}
		}
		
		/// <summary>
		/// Method SetPropertyBool
		/// </summary>
		/// <param name="name">A string</param>
		/// <param name="value">A  bool</param>
		public override void SetPropertyBool(String name, bool value)
		{
			throw new Ferda.Modules.NameNotExistError();
		}
		
		/// <summary>
		/// Method GetPropertyBool
		/// </summary>
		/// <returns>A bool</returns>
		/// <param name="name">A string</param>
		public override bool GetPropertyBool(String name)
		{
			throw new Ferda.Modules.NameNotExistError();
		}
		
		/// <summary>
		/// Method SetPropertyShort
		/// </summary>
		/// <param name="name">A string</param>
		/// <param name="value">A  short</param>
		public override void SetPropertyShort(String name, short value)
		{
			throw new Ferda.Modules.NameNotExistError();
		}
		
		/// <summary>
		/// Method GetPropertyShort
		/// </summary>
		/// <returns>A short</returns>
		/// <param name="name">A string</param>
		public override short GetPropertyShort(String name)
		{
			throw new Ferda.Modules.NameNotExistError();
		}
		
		/// <summary>
		/// Method SetPropertyInt
		/// </summary>
		/// <param name="name">A string</param>
		/// <param name="value">An int</param>
		public override void SetPropertyInt(String name, int value)
		{
			throw new Ferda.Modules.NameNotExistError();
		}
		
		/// <summary>
		/// Method GetPropertyInt
		/// </summary>
		/// <returns>An int</returns>
		/// <param name="name">A string</param>
		public override int GetPropertyInt(String name)
		{
			throw new Ferda.Modules.NameNotExistError();
		}
		
		/// <summary>
		/// Method SetPropertyLong
		/// </summary>
		/// <param name="name">A string</param>
		/// <param name="value">A  long</param>
		public override void SetPropertyLong(String name, long value)
		{
			throw new Ferda.Modules.NameNotExistError();
		}
		
		/// <summary>
		/// Method GetPropertyLong
		/// </summary>
		/// <returns>A long</returns>
		/// <param name="name">A string</param>
		public override long GetPropertyLong(String name)
		{
			throw new Ferda.Modules.NameNotExistError();
		}
		
		/// <summary>
		/// Method SetPropertyFloat
		/// </summary>
		/// <param name="name">A string</param>
		/// <param name="value">A  float</param>
		public override void SetPropertyFloat(String name, float value)
		{
			throw new Ferda.Modules.NameNotExistError();
		}
		
		/// <summary>
		/// Method GetPropertyFloat
		/// </summary>
		/// <returns>A float</returns>
		/// <param name="name">A string</param>
		public override float GetPropertyFloat(String name)
		{
			throw new Ferda.Modules.NameNotExistError();
		}
		
		/// <summary>
		/// Method SetPropertyDouble
		/// </summary>
		/// <param name="name">A string</param>
		/// <param name="value">A  double</param>
		public override void SetPropertyDouble(String name, double value)
		{
			throw new Ferda.Modules.NameNotExistError();
		}
		
		/// <summary>
		/// Method GetPropertyDouble
		/// </summary>
		/// <returns>A double</returns>
		/// <param name="name">A string</param>
		public override double GetPropertyDouble(String name)
		{
			throw new Ferda.Modules.NameNotExistError();
		}
		
		/// <summary>
		/// Method SetPropertyString
		/// </summary>
		/// <param name="name">A string</param>
		/// <param name="value">A string</param>
		public override void SetPropertyString(String name, String value)
		{
			throw new Ferda.Modules.NameNotExistError();
		}
		
		/// <summary>
		/// Method GetPropertyString
		/// </summary>
		/// <returns>A string</returns>
		/// <param name="name">A string</param>
		public override String GetPropertyString(String name)
		{
			throw new Ferda.Modules.NameNotExistError();
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
			throw new Ferda.Modules.NameNotExistError();
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
			throw new Ferda.Modules.NameNotExistError();
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
			throw new Ferda.Modules.NameNotExistError();
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
			throw new Ferda.Modules.NameNotExistError();
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
			throw new Ferda.Modules.NameNotExistError();
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
			throw new Ferda.Modules.NameNotExistError();
		}
		
		/// <summary>
		/// Method SetPropertyOther
		/// </summary>
		/// <param name="name">A string</param>
		/// <param name="value">A  byte[]</param>
		public override void SetPropertyOther(String name, PropertyValue value)
		{
			throw new Ferda.Modules.NameNotExistError();
		}
		
		/// <summary>
		/// Method GetPropertyOther
		/// </summary>
		/// <returns>A byte[]</returns>
		/// <param name="name">A string</param>
		public override PropertyValue GetPropertyOther(String name)
		{
			throw new Ferda.Modules.NameNotExistError();
		}

        public override void GetProperty_async(AMI_BoxModule_getProperty callBack, string name)
        {
            callBack.ice_exception(new Ferda.Modules.NameNotExistError());
        }
		
		/// <summary>
		/// Method GetPropertyOtherAbout
		/// </summary>
		/// <returns>A string</returns>
		/// <param name="name">A string</param>
		public override String GetPropertyOtherAbout(String name)
		{
			throw new Ferda.Modules.NameNotExistError();
		}
		
		/// <summary>
		/// Method IsPossibleToSetWithAbout
		/// </summary>
		/// <returns>A bool</returns>
		/// <param name="name">A string</param>
		public override bool IsPossibleToSetWithAbout(String name)
		{
			return false;
		}
		
		/// <summary>
		/// Method SetPropertyOtherAbout
		/// </summary>
		/// <param name="name">A string</param>
		/// <param name="value">A string</param>
		public override void SetPropertyOtherAbout(String name, String value)
		{
			throw new Ferda.Modules.NameNotExistError();
		}
		
		/// <summary>
		/// Method IsPropertySet
		/// </summary>
		/// <returns>A bool</returns>
		/// <param name="name">A string</param>
		public override bool IsPropertySet(String name)
		{
			throw new Ferda.Modules.NameNotExistError();
		}
		
		
		/// <summary>
		/// Method IsPropertyReadOnly
		/// </summary>
		/// <returns>A bool</returns>
		/// <param name="name">A string</param>
		public override bool IsPropertyReadOnly(String name)
		{
			throw new Ferda.Modules.NameNotExistError();
		}

        public override bool IsPropertySetWithSettingModule(string name)
        {
            throw new Ferda.Modules.NameNotExistError();
        }
		
		public override string RunSetPropertyOther(string name)
		{
			throw new Ferda.Modules.NameNotExistError();
		}
		
		/// <summary>
		/// Method GetPropertyOptions
		/// </summary>
		/// <returns>A Ferda.Modules.SelectString[]</returns>
		/// <param name="name">A string</param>
		public override SelectString[] GetPropertyOptions(String name)
		{
			throw new Ferda.Modules.NameNotExistError();
		}
		
		public override bool ArePropertyOptionsObligatory(string name)
		{
			throw new Ferda.Modules.NameNotExistError();
		}
		
		/// <summary>
		/// Method SetPropertySocking
		/// </summary>
		/// <param name="propertyName">A string</param>
		/// <param name="socked">A  bool</param>
		public override void SetPropertySocking(String propertyName, bool socked)
		{
			throw new Ferda.Modules.NameNotExistError();
		}
		
		/// <summary>
		/// Method GetPropertySocking
		/// </summary>
		/// <returns>A bool</returns>
		/// <param name="propertyName">A string</param>
		public override bool GetPropertySocking(String propertyName)
		{
			throw new Ferda.Modules.NameNotExistError();
		}
		
		/// <summary>
		/// Method RunAction
		/// </summary>
		/// <param name="actionName">A string</param>
		public override void RunAction(String actionName)
		{
			throw new Ferda.Modules.NameNotExistError();
		}
		
		public override void RunAction_async(AMI_BoxModule_runAction callBack, string actionName)
		{
			callBack.ice_exception(new Ferda.Modules.NameNotExistError());
		}
		
		public override bool IsPossibleToRunAction(string actionName)
		{
			throw new Ferda.Modules.NameNotExistError();
		}
		
		/// <summary>
		/// Method IsPossibleToRunModuleForInteraction
		/// </summary>
		/// <returns>A bool</returns>
		/// <param name="moduleIceId">A string</param>
		public override bool IsPossibleToRunModuleForInteraction(String moduleIceId)
		{
			return false;
		}
		
		public override void RunModuleForInteraction(String moduleIceIdentity)
		{
			throw new Ferda.Modules.NameNotExistError();
		}
		
		public override SocketInfo[] Sockets
		{
			get {
				return this.MadeInCreator.Sockets;
			}
		}
		
		public override ModulesAskingForCreation[] ModulesAskingForCreation
		{
			get {
				return new ModulesAskingForCreation[0];
			}
		}
		
		///<supplierCardinality>1</supplierCardinality>
		///<clientCardinality>0..*</clientCardinality>
		///<undirected></undirected>
		public override IBoxModuleFactoryCreator MadeInCreator
		{
			get {
				return creator;
			}
		}
		
		public override DynamicHelpItem[] DynamicHelpItems
		{
			get {
				return new DynamicHelpItem[0];
			}
		}
		
		public override String UserName
		{
			get {
				return userName;
			}
			set {
				userName = value;
			}
		}
		
		public override bool UserNameSet
		{
			get {
				return userName != "Group";
			}
		}
		
		public override ModuleForInteractionInfo[] ModuleForInteractionInfos
		{
			get {
				return new ModuleForInteractionInfo[0];
			}
		}
		
		/// <summary>
		/// Method destroy
		/// </summary>
		public override void destroy()
		{
			
		}
		
		/// <summary>
		/// Method TryWriteEnter
		/// </summary>
		/// <returns>A bool</returns>
		public override bool TryWriteEnter()
		{
			return true;
		}
		
		/// <summary>
		/// Method WriteExit
		/// </summary>
		public override void WriteExit()
		{
		}
		
		private GroupBoxFactoryCreator creator;
		private string userName = "Group";
    }
}
