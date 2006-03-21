// IBoxModule.cs - interface of boxes for upper layers
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

namespace Ferda.ModulesManager
{
	/// <summary>
	/// Base interface for box modules. Implemented by classes that represents box.
	/// </summary>
	public interface IBoxModule : IComparable<IBoxModule> {
		/// <summary>
		/// Gets ice identifiers of functions interface of this box.
		/// </summary>
		/// <remarks>These ice identifiers are used in
		/// <see cref="T:Ferda.Modules.BoxType">BoxType</see>.</remarks>
		/// <returns>A System.Collections.Specialized.StringCollection of ice identifiers of functions interface</returns>
		System.Collections.Specialized.StringCollection
		GetFunctionsIceIds();
		
		/// <summary>
		/// Returns bool indicating whether functions interface has the specified ice identifier
		/// </summary>
		/// <returns>True if this functions interface is of type <paramref name="iceId"/>, otherwise false</returns>
		/// <param name="iceId">A string representation of ice identifier of functions interface</param>
		bool IsWithIceId(string iceId);
		
		/// <summary>
		/// Sets connection between box <paramref name="otherModule"/> and this box.
		/// </summary>
		/// <param name="socketName">A string representing name of socket of this box</param>
		/// <param name="otherModule">A <see cref="T:Ferda.ModulesManager.IBoxModule"/> representing box which
		/// have to be connected to this box in socket <paramref name="socketName"/></param>
		/// <exception cref="T:Ferda.Modules.NameNotExistError">socket with name
		/// <paramref name="socketName"/> was not found</exception>
		/// <exception cref="T:Ferda.Modules.BadTypeError">box <paramref name="otherModule"/>
		/// has bad <see cref="T:Ferda.Modules.BoxType">BoxType</see></exception>
		/// <exception cref="T:Ferda.Modules.ConnectionExistsError">Socket <paramref name="socketName"/>
		/// is not of type <see cref="F:Ferda.Modules.SocketInfo.moreThanOne">moreThanOne</see> and there is allready connection to this socket</exception>
		void SetConnection(string socketName, Ferda.ModulesManager.IBoxModule otherModule);

		/// <summary>
		/// Gets boxes which are connected to this box to socket <paramref name="socketName"/>.
		/// </summary>
		/// <returns>An array of <see cref="T:Ferda.ModulesManager.IBoxModule"/> representing boxes in socket <paramref name="socketName"/></returns>
		/// <param name="socketName">A string representing name of socket of this box</param>
		/// <exception cref="T:Ferda.Modules.NameNotExistError">socket with name
		/// <paramref name="socketName"/> was not found</exception>
		Ferda.ModulesManager.IBoxModule[] GetConnections(string socketName);

		/// <summary>
		/// Removes connection from socket <paramref name="socketName"/>
		/// </summary>
		/// <param name="socketName">A string representing name of socket of this box</param>
		/// <param name="otherModule">A <see cref="T:Ferda.ModulesManager.IBoxModule"/> representing box which
		/// have to be removed from connection of this box in socket <paramref name="socketName"/></param>
		/// <exception cref="T:Ferda.Modules.NameNotExistError">socket with name
		/// <paramref name="socketName"/> was not found</exception>
		/// <exception cref="T:Ferda.Modules.ConnectionNotExistError">Socket <paramref name="socketName"/>
		/// does not contain box <paramref name="otherModule"/></exception>
		void RemoveConnection(string socketName, Ferda.ModulesManager.IBoxModule otherModule);
		
		/// <summary>
		/// Gets boxes connected to some socket of this box.
		/// </summary>
		/// <returns>An IBoxModule array of boxes connected to this box.</returns>
		IBoxModule[] ConnectedTo();
		
		/// <summary>
		/// Gets boxes connected to which is this box connected.
		/// </summary>
		/// <remarks>returns more same boxes if there
		/// are more connections like from that</remarks>
		/// <returns>A collection of boxes to which
		/// exists visible connection from this box</returns>
		List<IBoxModule> ConnectionsFrom();

		/// <summary>
		/// Sets bool property <paramref name="name"/> to value <paramref name="value"/>.
		/// </summary>
		/// <param name="name">A string representing name of property which is of type bool</param>
		/// <param name="value">A bool value which has to be set to property <paramref name="name"/></param>
		/// <exception cref="T:Ferda.Modules.NameNotExistError">property with name
		/// <paramref name="name"/> was not found</exception>
		/// <exception cref="T:Ferda.Modules.BadTypeError">property is not of type bool</exception>
		/// <exception cref="T:Ferda.Modules.ReadOnlyError">property is for read only</exception>
		void SetPropertyBool(string name, bool value);

		/// <summary>
		/// Gets value of bool property with name <paramref name="name"/>
		/// </summary>
		/// <returns>A bool representing value of property</returns>
		/// <param name="name">A string representing name of property</param>
		/// <exception cref="T:Ferda.Modules.NameNotExistError">property with name
		/// <paramref name="name"/> and type bool was not found</exception>
		bool GetPropertyBool(string name);

		/// <summary>
		/// Sets short property <paramref name="name"/> to value <paramref name="value"/>.
		/// </summary>
		/// <param name="name">A string representing name of property which is of type short</param>
		/// <param name="value">A short value which has to be set to property <paramref name="name"/></param>
		/// <exception cref="T:Ferda.Modules.NameNotExistError">property with name
		/// <paramref name="name"/> was not found</exception>
		/// <exception cref="T:Ferda.Modules.BadTypeError">property is not of type short</exception>
		/// <exception cref="T:Ferda.Modules.BadValueError"><paramref name="value"/>
		/// does not satisfy restrictions on property <paramref name="name"/>
		/// </exception>
		/// <exception cref="T:Ferda.Modules.ReadOnlyError">property is for read only</exception>
		void SetPropertyShort(string name, short value);

		/// <summary>
		/// Gets value of short property with name <paramref name="name"/>
		/// </summary>
		/// <returns>A short representing value of property</returns>
		/// <param name="name">A string representing name of property</param>
		/// <exception cref="T:Ferda.Modules.NameNotExistError">property with name
		/// <paramref name="name"/> and type short was not found</exception>
		short GetPropertyShort(string name);

		/// <summary>
		/// Sets int property <paramref name="name"/> to value <paramref name="value"/>.
		/// </summary>
		/// <param name="name">A string representing name of property which is of type int</param>
		/// <param name="value">A int value which has to be set to property <paramref name="name"/></param>
		/// <exception cref="T:Ferda.Modules.NameNotExistError">property with name
		/// <paramref name="name"/> was not found</exception>
		/// <exception cref="T:Ferda.Modules.BadTypeError">property is not of type int</exception>
		/// <exception cref="T:Ferda.Modules.BadValueError"><paramref name="value"/>
		/// does not satisfy restrictions on property <paramref name="name"/>
		/// </exception>
		/// <exception cref="T:Ferda.Modules.ReadOnlyError">property is for read only</exception>
		void SetPropertyInt(string name, int value);

		/// <summary>
		/// Gets value of int property with name <paramref name="name"/>
		/// </summary>
		/// <returns>A int representing value of property</returns>
		/// <param name="name">A string representing name of property</param>
		/// <exception cref="T:Ferda.Modules.NameNotExistError">property with name
		/// <paramref name="name"/> and type int was not found</exception>
		int GetPropertyInt(string name);

		/// <summary>
		/// Sets long property <paramref name="name"/> to value <paramref name="value"/>.
		/// </summary>
		/// <param name="name">A string representing name of property which is of type long</param>
		/// <param name="value">A long value which has to be set to property <paramref name="name"/></param>
		/// <exception cref="T:Ferda.Modules.NameNotExistError">property with name
		/// <paramref name="name"/> was not found</exception>
		/// <exception cref="T:Ferda.Modules.BadTypeError">property is not of type long</exception>
		/// <exception cref="T:Ferda.Modules.BadValueError"><paramref name="value"/>
		/// does not satisfy restrictions on property <paramref name="name"/>
		/// </exception>
		/// <exception cref="T:Ferda.Modules.ReadOnlyError">property is for read only</exception>
		void SetPropertyLong(string name, long value);

		/// <summary>
		/// Gets value of long property with name <paramref name="name"/>
		/// </summary>
		/// <returns>A long representing value of property</returns>
		/// <param name="name">A string representing name of property</param>
		/// <exception cref="T:Ferda.Modules.NameNotExistError">property with name
		/// <paramref name="name"/> and type long was not found</exception>
		long GetPropertyLong(string name);

		/// <summary>
		/// Sets float property <paramref name="name"/> to value <paramref name="value"/>.
		/// </summary>
		/// <param name="name">A string representing name of property which is of type float</param>
		/// <param name="value">A float value which has to be set to property <paramref name="name"/></param>
		/// <exception cref="T:Ferda.Modules.NameNotExistError">property with name
		/// <paramref name="name"/> was not found</exception>
		/// <exception cref="T:Ferda.Modules.BadTypeError">property is not of type float</exception>
		/// <exception cref="T:Ferda.Modules.BadValueError"><paramref name="value"/>
		/// does not satisfy restrictions on property <paramref name="name"/>
		/// </exception>
		/// <exception cref="T:Ferda.Modules.ReadOnlyError">property is for read only</exception>
		void SetPropertyFloat(string name, float value);

		/// <summary>
		/// Gets value of float property with name <paramref name="name"/>
		/// </summary>
		/// <returns>A float representing value of property</returns>
		/// <param name="name">A string representing name of property</param>
		/// <exception cref="T:Ferda.Modules.NameNotExistError">property with name
		/// <paramref name="name"/> and type float was not found</exception>
		float GetPropertyFloat(string name);

		/// <summary>
		/// Sets double property <paramref name="name"/> to value <paramref name="value"/>.
		/// </summary>
		/// <param name="name">A string representing name of property which is of type double</param>
		/// <param name="value">A double value which has to be set to property <paramref name="name"/></param>
		/// <exception cref="T:Ferda.Modules.NameNotExistError">property with name
		/// <paramref name="name"/> was not found</exception>
		/// <exception cref="T:Ferda.Modules.BadTypeError">property is not of type double</exception>
		/// <exception cref="T:Ferda.Modules.BadValueError"><paramref name="value"/>
		/// does not satisfy restrictions on property <paramref name="name"/>
		/// </exception>
		/// <exception cref="T:Ferda.Modules.ReadOnlyError">property is for read only</exception>
		void SetPropertyDouble(string name, double value);

		/// <summary>
		/// Gets value of double property with name <paramref name="name"/>
		/// </summary>
		/// <returns>A double representing value of property</returns>
		/// <param name="name">A string representing name of property</param>
		/// <exception cref="T:Ferda.Modules.NameNotExistError">property with name
		/// <paramref name="name"/> and type double was not found</exception>
		double GetPropertyDouble(string name);

		/// <summary>
		/// Sets string property <paramref name="name"/> to value <paramref name="value"/>.
		/// </summary>
		/// <param name="name">A string representing name of property which is of type string</param>
		/// <param name="value">A string value which has to be set to property <paramref name="name"/></param>
		/// <exception cref="T:Ferda.Modules.NameNotExistError">property with name
		/// <paramref name="name"/> was not found</exception>
		/// <exception cref="T:Ferda.Modules.BadTypeError">property is not of type string</exception>
		/// <exception cref="T:Ferda.Modules.BadValueError"><paramref name="value"/>
		/// does not satisfy restrictions on property <paramref name="name"/>
		/// </exception>
		/// <exception cref="T:Ferda.Modules.ReadOnlyError">property is for read only</exception>
		void SetPropertyString(string name, string value);

		/// <summary>
		/// Gets value of string property with name <paramref name="name"/>
		/// </summary>
		/// <returns>A string representing value of property</returns>
		/// <param name="name">A string representing name of property</param>
		/// <exception cref="T:Ferda.Modules.NameNotExistError">property with name
		/// <paramref name="name"/> and type string was not found</exception>
		string GetPropertyString(string name);
		
		/// <summary>
		/// Sets date property <paramref name="name"/> to value <paramref name="value"/>.
		/// </summary>
		/// <param name="name">A string representing name of property which is of type date</param>
		/// <param name="value">A string value which has to be set to property <paramref name="name"/></param>
		/// <exception cref="T:Ferda.Modules.NameNotExistError">property with name
		/// <paramref name="name"/> was not found</exception>
		/// <exception cref="T:Ferda.Modules.BadTypeError">property is not of type date</exception>
		/// <exception cref="T:Ferda.Modules.ReadOnlyError">property is for read only</exception>
		void SetPropertyDate(string name, System.DateTime value);
		
		/// <summary>
		/// Gets value of date property with name <paramref name="name"/>
		/// </summary>
		/// <returns>A <see cref="T:System.DateTime"/> representing value of property</returns>
		/// <param name="name">A string representing name of property</param>
		/// <exception cref="T:Ferda.Modules.NameNotExistError">property with name
		/// <paramref name="name"/> and type date was not found</exception>
		System.DateTime GetPropertyDate(string name);
		
		/// <summary>
		/// Sets DateTime property <paramref name="name"/> to value <paramref name="value"/>.
		/// </summary>
		/// <param name="name">A string representing name of property which is of type DateTime</param>
		/// <param name="value">A string value which has to be set to property <paramref name="name"/></param>
		/// <exception cref="T:Ferda.Modules.NameNotExistError">property with name
		/// <paramref name="name"/> was not found</exception>
		/// <exception cref="T:Ferda.Modules.BadTypeError">property is not of type DateTime</exception>
		/// <exception cref="T:Ferda.Modules.ReadOnlyError">property is for read only</exception>
		void SetPropertyDateTime(string name, System.DateTime value);
		
		/// <summary>
		/// Gets value of DateTime property with name <paramref name="name"/>
		/// </summary>
		/// <returns>A <see cref="T:System.DateTime"/> representing value of property</returns>
		/// <param name="name">A string representing name of property</param>
		/// <exception cref="T:Ferda.Modules.NameNotExistError">property with name
		/// <paramref name="name"/> and type date was not found</exception>
		System.DateTime GetPropertyDateTime(string name);
		
		/// <summary>
		/// Sets Time property <paramref name="name"/> to value <paramref name="value"/>.
		/// </summary>
		/// <param name="name">A string representing name of property which is of type Time</param>
		/// <param name="value">A string value which has to be set to property <paramref name="name"/></param>
		/// <exception cref="T:Ferda.Modules.NameNotExistError">property with name
		/// <paramref name="name"/> was not found</exception>
		/// <exception cref="T:Ferda.Modules.BadTypeError">property is not of type Time</exception>
		/// <exception cref="T:Ferda.Modules.ReadOnlyError">property is for read only</exception>
		void SetPropertyTime(string name, System.TimeSpan value);
		
		/// <summary>
		/// Gets value of Time property with name <paramref name="name"/>
		/// </summary>
		/// <returns>A <see cref="T:System.TimeSpan"/> representing value of property</returns>
		/// <param name="name">A string representing name of property</param>
		/// <exception cref="T:Ferda.Modules.NameNotExistError">property with name
		/// <paramref name="name"/> and type date was not found</exception>
		System.TimeSpan GetPropertyTime(string name);

		/// <summary>
		/// Sets property <paramref name="name"/> to value <paramref name="value"/>.
		/// </summary>
		/// <param name="name">A string representing name of property</param>
		/// <param name="value">A <see cref="T:Ferda.Modules.PropertyValue"/> value which has to be set to property <paramref name="name"/></param>
		/// <exception cref="T:Ferda.Modules.NameNotExistError">property with name
		/// <paramref name="name"/> was not found</exception>
		/// <exception cref="T:Ferda.Modules.BadTypeError">property is not of type of <paramref name="value"/></exception>
		/// <exception cref="T:Ferda.Modules.BadValueError"><paramref name="value"/>
		/// does not satisfy restrictions on property <paramref name="name"/>
		/// </exception>
		/// <exception cref="T:Ferda.Modules.ReadOnlyError">property is for read only</exception>
		void SetPropertyOther(string name, Ferda.Modules.PropertyValue value);

		/// <summary>
        /// Gets <see cref="T:Ferda.Modules.PropertyValue"/> representation of value of property with name <paramref name="name"/>
		/// and not standard type
		/// </summary>
		/// <returns>A <see cref="T:Ferda.Modules.PropertyValue"/> representing value of property</returns>
		/// <param name="name">A string representing name of property</param>
		/// <exception cref="T:Ferda.Modules.NameNotExistError">property with name
		/// <paramref name="name"/> and not of standard type was not found</exception>
		Ferda.Modules.PropertyValue GetPropertyOther(string name);

        /// <summary>
        /// Gets <see cref="T:Ferda.Modules.PropertyValue"/> representation of value of property with name <paramref name="name"/> asynchronously
        /// </summary>
        /// <param name="callBack">A <see cref="T:Ferda.Modules.AMI_BoxModule_runAction"/> call back interface will be used for catching exceptions</param>
        /// <param name="name">A string representing name of property</param>
        /// <seealso cref="M:Ferda.ModulesManager.IBoxModule.GetPropertyOther(System.String)"/>
        void GetProperty_async(Ferda.Modules.AMI_BoxModule_getProperty callBack, string name);

		/// <summary>
		/// Gets string representation of value of property with name <paramref name="name"/>
		/// and not standard type
		/// </summary>
		/// <returns>A string representing value of property</returns>
		/// <param name="name">A string representing name of property</param>
		/// <exception cref="T:Ferda.Modules.NameNotExistError">property with name
		/// <paramref name="name"/> and not of standard type was not found</exception>
		string GetPropertyOtherAbout(string name);

        /// <summary>
        /// Gets string representation of value of property with name <paramref name="name"/>
        /// and not standard type if value is <paramref name="value"/>
        /// </summary>
        /// <returns>A string representing value of property</returns>
        /// <param name="name">A string representing name of property</param>
        /// <param name="value">A <see cref="T:Ferda.Modules.PropertyValue"/> representing value of property</param>
        /// <exception cref="T:Ferda.Modules.NameNotExistError">property with name
        /// <paramref name="name"/> and not of standard type was not found</exception>
        string GetPropertyOtherAboutFromValue(string name, Ferda.Modules.PropertyValue value);

		/// <summary>
		/// Looks if it is possible to set by string representation of other property its value
		/// </summary>
		/// <returns>true if it is possible to set by string representation of other property its value, otherwise false</returns>
		/// <param name="name">A string representing name of property</param>
		bool IsPossibleToSetWithAbout(string name);

		/// <summary>
		/// Sets property <paramref name="name"/> to value <paramref name="value"/>.
		/// </summary>
		/// <param name="name">A string representing name of property</param>
		/// <param name="value">A string representation of value which has to be set to property <paramref name="name"/></param>
		/// <exception cref="T:Ferda.Modules.NameNotExistError">property with name
		/// <paramref name="name"/> was not found</exception>
		/// <exception cref="T:Ferda.Modules.BadTypeError">property is not of type of <paramref name="value"/></exception>
		/// <exception cref="T:Ferda.Modules.BadValueError"><paramref name="value"/>
		/// does not satisfy restrictions on property <paramref name="name"/>
		/// </exception>
		/// <exception cref="T:Ferda.Modules.ReadOnlyError">property is for read only</exception>
		void SetPropertyOtherAbout(string name, string value);

		/// <summary>
		/// Looks if is property <paramref name="name"/> set.
		/// </summary>
		/// <returns>true if property is set, otherwise false</returns>
		/// <param name="name">A string representing name of property</param>
		bool IsPropertySet(string name);
		
		/// <summary>
		/// Looks if is it is not possible to set value of property <paramref name="name"/>.
		/// </summary>
		/// <returns>true if property is read only, otherwise false</returns>
		/// <param name="name">A string representing name of property</param>
		bool IsPropertyReadOnly(string name);

        /// <summary>
        /// Looks if you have to run setting module for property <paramref name="name"/>.
        /// </summary>
        /// <returns>true if property have to be set by setting module</returns>
        /// <param name="name">A string representing name of property</param>
        bool IsPropertySetWithSettingModule(string name);
		
		/// <summary>
		/// Executes module for data setting for property <paramref name="name"/>
		/// and sets return value to property <paramref name="name"/>
		/// </summary>
		/// <param name="name">A string representing name of property</param>
		/// <returns>A string representing value of property</returns>
		/// <exception cref="T:Ferda.Modules.BoxRuntimeError">
		/// it is not possible to run action exception in some box. You can read it
		/// in <see cref="F:Ferda.Modules.BoxRuntimeError.userMessage"/>.
		/// In <see cref="F:Ferda.Modules.BoxRuntimeError.boxIdentity"/>
		/// you can read string representation of idenity of box.
		/// Use <see cref="M:Ferda.ModulesManager.ModulesManager.GetIBoxModuleByIdentity(System.String)"/>
		/// for getting box by its identity.
		/// </exception>
		/// <exception cref="T:Ferda.Modules.NameNotExistError">property with name
		/// <paramref name="name"/> was not found</exception>
		string RunSetPropertyOther(string name);
		
		/// <summary>
		/// Gets select options for string property <paramref name="name"/>
		/// </summary>
		/// <remarks>
		/// If property is of type string, there are three posibilities how user sets
		/// this string value.
		/// <list type="number">
		/// <item><description>
		/// It is normal string. It is if and only if this method returns zero length
		/// array.
		/// </description></item>
		/// <item><description>
		/// User can select values from select box and only these values can be set.
		/// It is if and only if this method returns nonzero length
		/// array and method
		/// <see cref="M:Ferda.ModulesManager.IBoxModule.ArePropertyOptionsObligatory(System.String)"/>
		/// returns true.
		/// </description></item>
		/// <item><description>
		/// User can select values from select box but also can use other string value.
		/// It is if and only if this method returns nonzero length
		/// array and method
		/// <see cref="M:Ferda.ModulesManager.IBoxModule.ArePropertyOptionsObligatory(System.String)"/>
		/// returns false.
		/// </description></item>
		/// </list>
		/// </remarks>
		/// <returns>A Ferda.Modules.SelectString[] containing labels and values for select box</returns>
		/// <param name="name">A string representing name of property</param>
		/// <seealso cref="M:Ferda.ModulesManager.IBoxModule.ArePropertyOptionsObligatory(System.String)"/>
		Ferda.Modules.SelectString[] GetPropertyOptions(string name);
			
		/// <summary>
		/// Returns if user have to choose some value from select options for
		/// string property <paramref name="name"/> and can not set other value.
		/// </summary>
		/// <returns>true if select options are obligatory</returns>
		/// <param name="name">A string representing name of property</param>
		/// <seealso cref="M:Ferda.ModulesManager.IBoxModule.GetPropertyOptions(System.String)"/>
		bool ArePropertyOptionsObligatory(string name);
		
		/// <summary>
		/// Makes property socked/unsocked
		/// </summary>
		/// <param name="propertyName">A string representing name of property</param>
		/// <param name="socked">A bool saying if make it socked or if make it unsocked.
		/// If true, it will make property socked, otherwise unsocked.</param>
		void SetPropertySocking(string propertyName, bool socked);
		
		/// <summary>
		/// Gets value of socking of property <paramref name="propertyName"/>
		/// </summary>
		/// <returns>A bool saying if property is socked or if is unsocked.
		/// If true, property is socked, otherwise unsocked.</returns>
		/// <param name="propertyName">A string representing name of property</param>
		bool GetPropertySocking(string propertyName);
		
		/// <summary>
		/// Returns a Boolean value indicating whether this box has type <paramref name="boxType"/>
		/// </summary>
		/// <remarks>
		/// <see cref="T:Ferda.Modules.BoxType"/> is used in
		/// <see cref="P:Ferda.ModulesManager.IBoxModule.Sockets"/>
		/// for indicating whether some box can be connected into that socket.
		/// </remarks>
		/// <returns>true if this box is of type <paramref name="boxType"/></returns>
		/// <param name="boxType">A <see cref="T:Ferda.Modules.BoxType"/> type of
		/// box</param>
		bool HasBoxType(Ferda.Modules.BoxType boxType);

		/// <summary>
		/// Executes action <paramref name="actionName"/>
		/// </summary>
		/// <param name="actionName">A string representing name of action for execution</param>
		/// <exception cref="T:Ferda.Modules.NameNotExistError">action with name
		/// <paramref name="actionName"/> was not found</exception>
		/// <exception cref="T:Ferda.Modules.NeedConnectedSocketError">
		/// Before executing action it is needed to connect some socket.
		/// Use <see cref="M:Ferda.ModulesManager.IBoxModule.IsPossibleToRunAction(System.String)"/>
		/// to check if it is possible to run action. But between time you
		/// check it and you try it, it can change state.
		/// </exception>
		/// <exception cref="T:Ferda.Modules.BoxRuntimeError">
		/// it is not possible to run action due to other reason. You can read it
		/// in <see cref="F:Ferda.Modules.BoxRuntimeError.userMessage"/>.
		/// In <see cref="F:Ferda.Modules.BoxRuntimeError.boxIdentity"/>
		/// you can read string representation of idenity of box.
		/// Use <see cref="M:Ferda.ModulesManager.ModulesManager.GetIBoxModuleByIdentity(System.String)"/>
		/// for getting box by its identity.
		/// </exception>
		/// <seealso cref="M:Ferda.ModulesManager.IBoxModule.RunAction_async(Ferda.Modules.AMI_BoxModule_runAction,System.String)"/>
		/// <seealso cref="M:Ferda.ModulesManager.IBoxModule.IsPossibleToRunAction(System.String)"/>
		void RunAction(string actionName);
		
		/// <summary>
		/// Executes action <paramref name="actionName"/> asynchronously
		/// </summary>
        /// <param name="callBack">A <see cref="T:Ferda.Modules.AMI_BoxModule_runAction"/> call back interface will be used for catching exceptions</param>
        /// <param name="actionName">A string representing name of action for execution</param>
		/// <seealso cref="M:Ferda.ModulesManager.IBoxModule.RunAction(System.String)"/>
		/// <seealso cref="M:Ferda.ModulesManager.IBoxModule.IsPossibleToRunAction(System.String)"/>
		void RunAction_async(Ferda.Modules.AMI_BoxModule_runAction callBack, string actionName);
		
		/// <summary>
		/// Returns a Boolean value indicating whether it is possible to run action
		/// with name <paramref name="actionName"/>
		/// </summary>
		/// <returns>true if it is possible to run, otherwise false</returns>
		/// <param name="actionName">A string representing name of action</param>
		/// <exception cref="T:Ferda.Modules.NameNotExistError">action with name
		/// <paramref name="actionName"/> was not found</exception>
		/// <seealso cref="M:Ferda.ModulesManager.IBoxModule.RunAction(System.String)"/>
		bool IsPossibleToRunAction(string actionName);
		
		/// <summary>
		/// Returns a Boolean value indicating whether it is possible to run module
		/// for interaction with ice identity <paramref name="moduleIceIdentity"/>
		/// </summary>
		/// <returns>A bool</returns>
		/// <param name="moduleIceIdentity">A string representing ice identity of
		/// module for interaction</param>
		/// <exception cref="T:Ferda.Modules.NameNotExistError">module for interaction
		/// with ide identity
		/// <paramref name="moduleIceIdentity"/> was not found</exception>
		bool IsPossibleToRunModuleForInteraction(string moduleIceIdentity);

		/// <summary>
		/// Executes module for interaction with ice identity <paramref name="moduleIceIdentity"/>
		/// </summary>
		/// <param name="moduleIceIdentity">A string representing ice identity of
		/// module for interaction</param>
		/// <exception cref="T:Ferda.Modules.NameNotExistError">module for interaction
		/// with ide identity
		/// <paramref name="moduleIceIdentity"/> was not found</exception>
        /// <exception cref="T:Ferda.Modules.BoxRuntimeError">
        /// it is not possible to run module for interaction due to problem in some box. You can read it
        /// in <see cref="F:Ferda.Modules.BoxRuntimeError.userMessage"/>.
        /// In <see cref="F:Ferda.Modules.BoxRuntimeError.boxIdentity"/>
        /// you can read string representation of idenity of box.
        /// Use <see cref="M:Ferda.ModulesManager.ModulesManager.GetIBoxModuleByIdentity(System.String)"/>
        /// for getting box by its identity.
        /// </exception>
		void RunModuleForInteraction(string moduleIceIdentity);
		
		/// <summary>
		/// Information about sockets in this box.
		/// </summary>
		/// <value>An array of <see cref="T:Ferda.Modules.SocketInfo"/>
		/// where every <see cref="T:Ferda.Modules.SocketInfo"/> is for
		/// one socket</value>
		Modules.SocketInfo[] Sockets
		{
			get;
		}
		
		/// <summary>
		/// Modules asking for creation for this box.
		/// </summary>
		/// <value>An array of <see cref="T:Ferda.Modules.ModuleAskingForCreation"/>.
		/// Every <see cref="T:Ferda.Modules.ModuleAskingForCreation"/> represents
		/// one box which is asking for creation.
		/// </value>
		Ferda.Modules.ModulesAskingForCreation[] ModulesAskingForCreation
		{
			get;
		}
		
		/// <summary>
		/// Creator in which was this box created.
		/// </summary>
		/// <value>A <see cref="T:Ferda.ModulesManager.IBoxModuleFactoryCreator"/>
		/// </value>
		IBoxModuleFactoryCreator MadeInCreator
		{
			get;
		}
		
		/// <summary>
		/// Unique identifier in project
		/// </summary>
		/// <value>An int representing unique identifier in project</value>
		int ProjectIdentifier
		{
			set;
			
			get;
		}
		
		/// <summary>
		/// User note about box
		/// </summary>
		/// <value>A string representing user note</value>
		string UserHint
		{
			set;
			
			get;
		}
		
		/// <summary>
		/// Dynamic help
		/// </summary>
		/// <value>An array of <see cref="T:Ferda.Modules.DynamicHelpItem"/>
		/// representing items in dynamic help</value>
		Ferda.Modules.DynamicHelpItem[] DynamicHelpItems
		{
			get;
		}
		
		/// <summary>
		/// User name of box
		/// </summary>
		/// <value>
		/// A string representing user name of box
		/// </value>
		string UserName
		{
			get;
			set;
		}
		
		/// <summary>
		/// Is user name set by user?
		/// </summary>
		/// <value>
		/// True if UserName is set by user, otherwise false
		/// </value>
		bool UserNameSet
		{
			get;
		}

		/// <summary>
		/// Information about modules for interaction which can be run
		/// on top of this box
		/// </summary>
		/// <value>
		/// An array of <see cref="T:Ferda.ModulesManager.ModuleForInteractionInfo"/>
		/// representing information about modules for interaction
		/// which can be run on top of this box
		/// </value>
		ModuleForInteractionInfo[] ModuleForInteractionInfos
		{
			get;
		}

		/// <summary>
		/// Destroys this box. It is needed to destroy box if is not needed.
		/// If not you waste with resources.
		/// </summary>
		void destroy();
		
		/// <summary>
		/// Creates clone of this box
		/// </summary>
		/// <returns>An <see cref="T:Ferda.ModulesManager.IBoxModule"/>
		/// representing new box module which have the same properties,
		/// connected sockets, user hint,...</returns>
		IBoxModule Clone();
		
		/// <summary>
		/// Tries to get write access to box. If returns true, you have access
		/// and can write. Return access by executing
		/// <see cref="M:Ferda.ModulesManager.IBoxModule.WriteExit()"/>.
		/// </summary>
		/// <remarks>
		/// Box can be locked for writing by user with some working box
		/// or module. Sometimes it is not good when user sets box properties,
		/// connections, user names or user hints.
		/// </remarks>
		/// <returns>true if you got access</returns>
		/// <example><code lang="C#">
		/// public void sample(IBoxModule box)
		/// {
		/// 	if(box.TryWriteEnter())
		/// 	{
		/// 		//set property, connection or what you want
		/// 		...
		/// 		box.WriteExit();
		/// 	}
		/// 	else
		/// 	{
		/// 		//say to user that box is locked by some working box
		/// 	}
		/// }
		/// </code></example>
		/// <seealso cref="M:Ferda.ModulesManager.IBoxModule.WriteExit()"/>
		bool TryWriteEnter();
		
		/// <summary>
		/// Returns write access to this box.
		/// </summary>
		/// <seealso cref="M:Ferda.ModulesManager.IBoxModule.TryWriteEnter()"/>
		void WriteExit();

        /// <summary>
        /// Use before comparing this box with other
        /// </summary>
        void RefreshOrder();
	}
}
