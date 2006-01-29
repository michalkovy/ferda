using System.Collections.Generic;
using Ferda.Modules;
using System.Collections.Specialized;
using System;

namespace Ferda.ModulesManager
{
	/// <summary>
	/// An ancestor class of boxes for Modules Manager. There are
	/// implemented some methods from <see cref="IBoxModule"/>
	/// and added other methods on top of box which need Modules Manager
	/// </summary>
	public abstract class BoxModuleForManager : IBoxModule
	{
		
		/// <summary>
		/// Method CompareTo
		/// </summary>
		/// <returns>An int</returns>
		/// <param name="other">A  T</param>
		public int CompareTo(IBoxModule other)
		{
            BoxModuleForManager otherForManager = other as BoxModuleForManager;
            return CompareString.CompareTo(otherForManager.CompareString);
		}

        public void RefreshOrder()
        {
            CompareString = this.UserName;
        }

        protected internal string CompareString;
		
		public int ProjectIdentifier
		{
			set {
				projectIdentifier = value;
			}
			
			get {
				return projectIdentifier;
			}
		}
		
		private int projectIdentifier;

		public abstract void destroy();
		
		public IBoxModule Clone()
		{
			IBoxModule result = this.MadeInCreator.CreateBoxModule();
			result.UserName = this.UserName;
			result.UserHint = this.UserHint;
			
			foreach(PropertyInfo propertyInfo in this.MadeInCreator.Properties)
			{
				string property = propertyInfo.name;
				if(!this.GetPropertySocking(property))
				{
					if(!propertyInfo.readOnly)
					{
						result.SetPropertyOther(property,
											this.GetPropertyOther(property));
					}
				}
				else
				{
					result.SetPropertySocking(property,true);
				}
			}
			foreach(SocketInfo socket in this.Sockets)
			{
				foreach(IBoxModule otherBox in this.GetConnections(socket.name))
				{
					result.SetConnection(socket.name, otherBox);
				}
			}
			return result;
		}
		
		#region User newModule properties
		public abstract string UserName
		{
			get;
			set;
		}
		
		public abstract bool UserNameSet
		{
			get;
		}
	
		public string UserHint
		{
			set {
				userHint = value;
			}
			
			get {
				return userHint;
			}
		}
		private string userHint;
		#endregion
		
		#region MadeInCreator
		public abstract IBoxModuleFactoryCreator MadeInCreator
		{
			get;
		}
		#endregion
		
		#region Connections
		public abstract void SetConnection(string socketName, Ferda.ModulesManager.IBoxModule otherModule);
		
		public abstract Ferda.ModulesManager.IBoxModule[] GetConnections(string socketName);
		
		public abstract void RemoveConnection(string socketName, Ferda.ModulesManager.IBoxModule otherModule);
		
		/// <summary>
		/// Method ConnectedTo
		/// </summary>
		/// <returns>An IBoxModule[]</returns>
		public IBoxModule[] ConnectedTo()
		{
			return connectionsTo.ToArray();
		}
		
		/// <summary>
		/// Adds box <paramref name="box"/> to collection of boxes to which is this box connected.
		/// </summary>
		/// <param name="box">An IBoxModule representing box to which is this box connected.</param>
		/// <seealso cref="BoxModuleForManager.RemoveConnectionTo">Remove connection to</seealso>
		/// <seealso cref="IBoxModule.ConnectedTo">Get connections to</seealso>
		public void AddConnectionTo(IBoxModule box)
		{
			connectionsTo.Add(box);
		}
		
		/// <summary>
		/// Removes box <paramref name="box"/> from collection of boxes to which is this box connected.
		/// </summary>
		/// <param name="box">An IBoxModule representing box to which was this box connected.</param>
		/// <seealso cref="BoxModuleForManager.AddConnectionTo">Add connection to</seealso>
		/// <seealso cref="IBoxModule.ConnectedTo">Get connections to</seealso>
		public void RemoveConnectionTo(IBoxModule box)
		{
			connectionsTo.Remove(box);
		}
		private List<IBoxModule> connectionsTo = new List<IBoxModule>();

		public List<IBoxModule> ConnectionsFrom()
		{
			List<IBoxModule> result = new List<IBoxModule>();
			foreach (SocketInfo socket in this.Sockets)
			{
				foreach (IBoxModule otherBox in this.GetConnections(socket.name))
				{
					result.Add(otherBox);
				}
			}
			return result;
		}
		#endregion
		
		#region Properties
		public abstract void SetPropertyBool(string name, bool value);
		
		public abstract bool GetPropertyBool(string name);

		public abstract void SetPropertyShort(string name, short value);

		public abstract short GetPropertyShort(string name);

		public abstract void SetPropertyInt(string name, int value);

		public abstract int GetPropertyInt(string name);

		public abstract void SetPropertyLong(string name, long value);

		public abstract long GetPropertyLong(string name);

		public abstract void SetPropertyFloat(string name, float value);

		public abstract float GetPropertyFloat(string name);

		public abstract void SetPropertyDouble(string name, double value);

		public abstract double GetPropertyDouble(string name);

		public abstract void SetPropertyString(string name, string value);

		public abstract string GetPropertyString(string name);
		
		/// <summary>
		/// Sets date property <paramref name="name"/> to value <paramref name="value"/>.
		/// </summary>
		/// <param name="name">A string representing name of property which is of type date</param>
		/// <param name="value">A string value which has to be set to property <paramref name="name"/></param>
		/// <exception cref="T:Ferda.Modules.NameNotExistError">property with name
		/// <paramref name="name"/> was not found</exception>
		/// <exception cref="T:Ferda.Modules.BadTypeError">property is not of type date</exception>
		/// <exception cref="T:Ferda.Modules.ReadOnlyError">property is for read only</exception>
		public abstract void SetPropertyDate(string name, System.DateTime value);
		
		/// <summary>
		/// Gets value of date property with name <paramref name="name"/>
		/// </summary>
		/// <returns>A <see cref="T:System.DateTime"/> representing value of property</returns>
		/// <param name="name">A string representing name of property</param>
		/// <exception cref="T:Ferda.Modules.NameNotExistError">property with name
		/// <paramref name="name"/> and type date was not found</exception>
		public abstract System.DateTime GetPropertyDate(string name);
		
		/// <summary>
		/// Sets DateTime property <paramref name="name"/> to value <paramref name="value"/>.
		/// </summary>
		/// <param name="name">A string representing name of property which is of type DateTime</param>
		/// <param name="value">A string value which has to be set to property <paramref name="name"/></param>
		/// <exception cref="T:Ferda.Modules.NameNotExistError">property with name
		/// <paramref name="name"/> was not found</exception>
		/// <exception cref="T:Ferda.Modules.BadTypeError">property is not of type DateTime</exception>
		/// <exception cref="T:Ferda.Modules.ReadOnlyError">property is for read only</exception>
		public abstract void SetPropertyDateTime(string name, System.DateTime value);
		
		/// <summary>
		/// Gets value of DateTime property with name <paramref name="name"/>
		/// </summary>
		/// <returns>A <see cref="T:System.DateTime"/> representing value of property</returns>
		/// <param name="name">A string representing name of property</param>
		/// <exception cref="T:Ferda.Modules.NameNotExistError">property with name
		/// <paramref name="name"/> and type date was not found</exception>
		public abstract System.DateTime GetPropertyDateTime(string name);
		
		/// <summary>
		/// Sets Time property <paramref name="name"/> to value <paramref name="value"/>.
		/// </summary>
		/// <param name="name">A string representing name of property which is of type Time</param>
		/// <param name="value">A string value which has to be set to property <paramref name="name"/></param>
		/// <exception cref="T:Ferda.Modules.NameNotExistError">property with name
		/// <paramref name="name"/> was not found</exception>
		/// <exception cref="T:Ferda.Modules.BadTypeError">property is not of type Time</exception>
		/// <exception cref="T:Ferda.Modules.ReadOnlyError">property is for read only</exception>
		public abstract void SetPropertyTime(string name, System.TimeSpan value);
		
		/// <summary>
		/// Gets value of Time property with name <paramref name="name"/>
		/// </summary>
		/// <returns>A <see cref="T:System.TimeSpan"/> representing value of property</returns>
		/// <param name="name">A string representing name of property</param>
		/// <exception cref="T:Ferda.Modules.NameNotExistError">property with name
		/// <paramref name="name"/> and type date was not found</exception>
		public abstract System.TimeSpan GetPropertyTime(string name);

		public abstract void SetPropertyOther(string name, Ferda.Modules.PropertyValue value);

		public abstract Ferda.Modules.PropertyValue GetPropertyOther(string name);

        public abstract void GetProperty_async(Ferda.Modules.AMI_BoxModule_getProperty callBack, string name);

		public abstract string GetPropertyOtherAbout(string name);

		public abstract bool IsPossibleToSetWithAbout(string name);

		public abstract void SetPropertyOtherAbout(string name, string value);

		public abstract SelectString[] GetPropertyOptions(string name);
		
		public abstract bool ArePropertyOptionsObligatory(string name);
		
		public abstract bool IsPropertySet(string name);
		
		public abstract bool IsPropertyReadOnly(string name);

        public abstract bool IsPropertySetWithSettingModule(string name);

		public abstract string RunSetPropertyOther(string name);

		public abstract void SetPropertySocking(string propertyName, bool socked);
				
		public abstract bool GetPropertySocking(string propertyName);
		#endregion
		
		#region BoxType and sockets
		public abstract System.Collections.Specialized.StringCollection GetFunctionsIceIds();
		
		public bool IsWithIceId(string iceId)
		{
			return this.GetFunctionsIceIds().Contains(iceId);
		}
		
		public abstract bool HasBoxType(BoxType boxType);

		
		public abstract Ferda.Modules.SocketInfo[] Sockets
		{
			get;
		}
		#endregion
		
		#region Action and interaction
		public abstract void RunAction(string actionName);
		
		public abstract void RunAction_async(AMI_BoxModule_runAction callBack, string actionName);
		
		public abstract bool IsPossibleToRunAction(string actionName);
		
		public abstract bool IsPossibleToRunModuleForInteraction(String moduleIceIdentity);

		public abstract void RunModuleForInteraction(string moduleIceIdentity);
		
		public abstract ModuleForInteractionInfo[] ModuleForInteractionInfos
		{
			get;
		}
		#endregion
		
		public abstract Ferda.Modules.ModulesAskingForCreation[] ModulesAskingForCreation
		{
			get;
		}
		
		public abstract Ferda.Modules.DynamicHelpItem[] DynamicHelpItems
		{
			get;
		}
		
		public abstract bool TryWriteEnter();
		
		public abstract void WriteExit();
		
		public abstract List<IBoxModule> GetUnvisibleConnections(string socketName);
		
		public abstract void SetUnvisibleConnection(string socketName, IBoxModule otherModule);
		
		public abstract void RemoveUnvisibleConnection(string socketName, IBoxModule otherModule);
    }
}
