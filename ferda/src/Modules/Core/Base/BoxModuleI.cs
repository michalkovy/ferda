// BoxModuleI.cs - box on Ferda modules side
//
// Authors:
//   Michal Kováč <michal.kovac.develop@centrum.cz>
//   Tomáš Kuchař <tomas.kuchar@gmail.com>
//
// Copyright (c) 2005 Michal Kováč, Tomáš Kuchař
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
using System.Diagnostics;
using Ferda.Modules.Boxes;
using Ferda.ModulesManager;
using Ice;
using Exception=System.Exception;
using Object=Ice.Object;

namespace Ferda.Modules
{
    /// <summary>
    /// Representation of box on Ferda modules side
    /// </summary>
    public class BoxModuleI : BoxModuleDisp_
    {
        #region Connections i.e. boxes in sockets

        /// <summary>
        /// <para><c>Key</c> is the socket`s name.</para>
        /// <para><c>Value</c> is next Dictionary.</para>
        /// <para>
        /// Inner <c>Key</c> is <see cref="M:Ice.Util.identityToString(Ice.Identity)">string</see>
        /// representation of the box module`s ice identifier.
        /// </para>
        /// <para>
        /// Inner <c>Value</c> is the proxy of the box module connected in the socket.
        /// </para>
        /// </summary>
        private Dictionary<string, Dictionary<string, BoxModulePrx>> connections;
        
        private Dictionary<string, Dictionary<string, BoxModulePrx>> connectionsOfAdditionalSockets = new Dictionary<string, Dictionary<string, BoxModulePrx>>();

        /// <summary>
        /// <para>
        /// Gets the boxes (proxies of the box modules) connected
        /// to the socket of the specified name.
        /// </para>
        /// <para>
        /// Using of <see cref="T:Ferda.Modules.Boxes.SocketConnections">
        /// helper</see> is recommended.
        /// </para>
        /// </summary>
        /// <param name="socketName">Name of the socket.</param>
        /// <returns>
        /// Array of <see cref="T:Ferda.Modules.BoxModulePrx">
        /// proxies of box modules</see> connected in the specified socket.
        /// </returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// Thrown iff specified <c>socketName</c> doesn`t match any
        /// socket in the box module.
        /// </exception>
        /// <seealso cref="T:Ferda.Modules.Boxes.SocketConnections"/>
        public BoxModulePrx[] GetConnections(string socketName)
        {
           lock (this)
           {
               BoxModulePrx[] result = new BoxModulePrx[getConnection(socketName).Values.Count];
               getConnection(socketName).Values.CopyTo(result, 0);
               return result;
           }
        }

        /// <summary>
        /// Gets the boxes (proxies of the box modules) connected
        /// to the socket of the specified name.
        /// </summary>
        /// <param name="socketName">Name of the socket.</param>
        /// <param name="__current">The Ice.Current.</param>
        /// <returns>
        /// Array of <see cref="T:Ferda.Modules.BoxModulePrx">
        /// proxies of box modules</see> connected in the specified socket.
        /// </returns>
        /// <exception cref="T:Ferda.Modules.NameNotExistError">
        /// Thrown iff specified <c>socketName</c> doesn`t match any
        /// socket in the box module.
        /// </exception>
        public override BoxModulePrx[] getConnections(string socketName, Current __current)
        {
            if (!boxInfo.TestSocketNameExistence(socketName, this))
            {
                Debug.Assert(false);
                throw Exceptions.NameNotExistError(null, socketName);
            }
            return GetConnections(socketName);
        }

        /// <summary>
        /// Removes connection to specified box module (<c>boxModuleIceIdentity</c>)
        /// from specified socket (<c>socketName</c>).
        /// </summary>
        /// <param name="socketName">Name of the socket.</param>
        /// <param name="boxModuleIceIdentity">The box module ice identity.</param>
        /// <param name="__current">The Ice.Current.</param>
        /// <exception cref="T:Ferda.Modules.NameNotExistError">
        /// Thrown if specified <c>socketName</c> doesn`t match any
        /// socket in the box module.
        /// </exception>
        /// <exception cref="Ferda.Modules.ConnectionNotExistError">
        /// Thrown if specified box module is not connected to specified socket.
        /// </exception>
        public override void removeConnection(string socketName, string boxModuleIceIdentity, Current __current)
        {
            if (!boxInfo.TestSocketNameExistence(socketName, this))
            {
                Debug.Assert(false);
                throw Exceptions.NameNotExistError(null, socketName);
            }
            lock (this)
            {
                try
                {
                    getConnection(socketName).Remove(boxModuleIceIdentity);
                }
                catch
                {
                    Debug.WriteLine("BMI04");
                    throw new ConnectionNotExistError();
                }
                if (boxInfo.TestPropertyNameExistence(socketName))
                {
                    setProperty(socketName, boxInfo.GetPropertyDefaultValue(socketName));
                }
            }
        }

        #endregion

        #region Functions i.e. functions in sockets

        /// <summary>
        /// <para>
        /// Gets the functions objects (more precisely its proxies)
        /// connected to the socket of the specified name.
        /// </para>
        /// <para>
        /// Using of <see cref="T:Ferda.Modules.Boxes.SocketConnections">
        /// helper</see> is recommended.
        /// </para>
        /// </summary>
        /// <param name="socketName">Name of the socket.</param>
        /// <returns>
        /// Array of <see cref="T:Ice.ObjectPrx">proxies of
        /// functions objects</see> connected in the specified socket.
        /// </returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// Thrown iff specified <c>socketName</c> doesn`t match any
        /// socket in the box module.
        /// </exception>
        /// <seealso cref="T:Ferda.Modules.Boxes.SocketConnections"/>
        public ObjectPrx[] GetFunctions(string socketName)
        {
           List<ObjectPrx> result = new List<ObjectPrx>();

           foreach (BoxModulePrx boxModule in getConnection(socketName).Values)
           {
               ObjectPrx functions = boxModule.getFunctions();
               if (functions != null)
                   result.Add(functions);
           }
           return result.ToArray();
        }

        #endregion

        /// <summary>
        /// Properties which are set by <see cref="T:Ferda.Modules.PropertyValue"/>
        /// (There are not properties, which are set by PropertyBoxes
        /// which implements interfaces of property values.)
        /// </summary>
        private Dictionary<string, PropertyValue> properties;

        #region Properties providers by their type (short, bool, int, long, float, double, string, datetime, date, time and other)

        /// <summary>
        /// Gets the short integer property of the specified name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>The <see cref="System.Int16">Short</see> value of the specified property.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// Thrown iff specified <c>propertyName</c> doesn`t match any
        /// short integer property.
        /// </exception>
        public short GetPropertyShort(string propertyName)
        {
            lock (this)
            {
                PropertyValue value;
                if (properties.TryGetValue(propertyName, out value))
                    return ((ShortT) value).getShortValue();
            }

            ObjectPrx[] functions = GetFunctions(propertyName);
            if (functions.Length > 0)
            {
                return ShortTInterfacePrxHelper.checkedCast(functions[0]).getShortValue();
            }
            else
            {
                Debug.WriteLine("BMI08");
                throw new ArgumentOutOfRangeException("propertyName", propertyName, "");
            }
        }

        /// <summary>
        /// Gets the boolean property of the specified name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>The <see cref="System.Boolean">Boolean</see> value of the specified property.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// Thrown iff specified <c>propertyName</c> doesn`t match any
        /// boolean property.
        /// </exception>
        public bool GetPropertyBool(string propertyName)
        {
            lock (this)
            {
                PropertyValue value;
                if (properties.TryGetValue(propertyName, out value))
                    return ((BoolT) value).getBoolValue();
            }

            ObjectPrx[] functions = GetFunctions(propertyName);
            if (functions.Length > 0)
            {
                return BoolTInterfacePrxHelper.checkedCast(functions[0]).getBoolValue();
            }
            else
            {
                Debug.WriteLine("BMI09");
                throw new ArgumentOutOfRangeException("propertyName", propertyName, "");
            }
        }

        /// <summary>
        /// Gets the integer property of the specified name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>The <see cref="System.Int32">Integer</see> value of the specified property.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// Thrown iff specified <c>propertyName</c> doesn`t match any
        /// integer property.
        /// </exception>
        public int GetPropertyInt(string propertyName)
        {
            lock (this)
            {
                PropertyValue value;
                if (properties.TryGetValue(propertyName, out value))
                    return ((IntT) value).getIntValue();
            }

            ObjectPrx[] functions = GetFunctions(propertyName);
            if (functions.Length > 0)
            {
                return IntTInterfacePrxHelper.checkedCast(functions[0]).getIntValue();
            }
            else
            {
                Debug.WriteLine("BMI10");
                throw new ArgumentOutOfRangeException("propertyName", propertyName, "");
            }
        }

        /// <summary>
        /// Gets the long integer property of the specified name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>The <see cref="System.Int64">Long</see> value of the specified property.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// Thrown iff specified <c>propertyName</c> doesn`t match any
        /// long integer property.
        /// </exception>
        public long GetPropertyLong(string propertyName)
        {
            lock (this)
            {
                PropertyValue value;
                if (properties.TryGetValue(propertyName, out value))
                    return ((LongT) value).getLongValue();
            }

            ObjectPrx[] functions = GetFunctions(propertyName);
            if (functions.Length > 0)
            {
                return LongTInterfacePrxHelper.checkedCast(functions[0]).getLongValue();
            }
            else
            {
                Debug.WriteLine("BMI11");
                throw new ArgumentOutOfRangeException("propertyName", propertyName, "");
            }
        }

        /// <summary>
        /// Gets the float property of the specified name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>The <see cref="System.Single">Float</see> value of the specified property.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// Thrown iff specified <c>propertyName</c> doesn`t match any
        /// float property.
        /// </exception>
        public float GetPropertyFloat(string propertyName)
        {
            lock (this)
            {
                PropertyValue value;
                if (properties.TryGetValue(propertyName, out value))
                    return ((FloatT) value).getFloatValue();
            }

            ObjectPrx[] functions = GetFunctions(propertyName);
            if (functions.Length > 0)
            {
                return FloatTInterfacePrxHelper.checkedCast(functions[0]).getFloatValue();
            }
            else
            {
                Debug.WriteLine("BMI12");
                throw new ArgumentOutOfRangeException("propertyName", propertyName, "");
            }
        }

        /// <summary>
        /// Gets the double property of the specified name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>The <see cref="System.Double">Double</see> value of the specified property.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// Thrown iff specified <c>propertyName</c> doesn`t match any
        /// double property.
        /// </exception>
        public double GetPropertyDouble(string propertyName)
        {
            lock (this)
            {
                PropertyValue value;
                if (properties.TryGetValue(propertyName, out value))
                    return ((DoubleT) value).getDoubleValue();
            }

            ObjectPrx[] functions = GetFunctions(propertyName);
            if (functions.Length > 0)
            {
                return DoubleTInterfacePrxHelper.checkedCast(functions[0]).getDoubleValue();
            }
            else
            {
                Debug.WriteLine("BMI13");
                throw new ArgumentOutOfRangeException("propertyName", propertyName, "");
            }
        }

        /// <summary>
        /// Gets the string property of the specified name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>The <see cref="System.String">String</see> value of the specified property.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// Thrown iff specified <c>propertyName</c> doesn`t match any
        /// string property.
        /// </exception>
        public string GetPropertyString(string propertyName)
        {
            lock (this)
            {
                PropertyValue value;
                if (properties.TryGetValue(propertyName, out value))
                    return ((StringT) value).getStringValue();
            }

            ObjectPrx[] functions = GetFunctions(propertyName);
            if (functions.Length > 0)
            {
                return StringTInterfacePrxHelper.checkedCast(functions[0]).getStringValue();
            }
            else
            {
                Debug.WriteLine("BMI14");
                throw new ArgumentOutOfRangeException("propertyName", propertyName, "");
            }
        }

        /// <summary>
        /// Gets the string[] property of the specified name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>The <see cref="System.String">String</see>[] value of the specified property.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// Thrown iff specified <c>propertyName</c> doesn`t match any
        /// StringSeq property.
        /// </exception>
        public string[] GetPropertyStringSeq(string propertyName)
        {
            lock (this)
            {
                PropertyValue value;
                if (properties.TryGetValue(propertyName, out value))
                    return ((StringSeqT) value).getStringSeq();
            }

            ObjectPrx[] functions = GetFunctions(propertyName);
            if (functions.Length > 0)
            {
                return StringSeqTInterfacePrxHelper.checkedCast(functions[0]).getStringSeq();
            }
            else
            {
                Debug.WriteLine("BMI144");
                throw new ArgumentOutOfRangeException("propertyName", propertyName, "");
            }
        }

        /// <summary>
        /// Gets the datetime property of the specified name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>The <see cref="System.DateTime">DateTime</see> value of the specified property.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// Thrown iff specified <c>propertyName</c> doesn`t match any
        /// datetime property.
        /// </exception>
        public DateTime GetPropertyDateTime(string propertyName)
        {
            PropertyValue value;
            DateTime returnValue;
            lock (this)
            {
                if (properties.TryGetValue(propertyName, out value))
                {
                    ((DateTimeTI) value).TryGetDateTime(out returnValue);
                    return returnValue;
                }
            }

            ObjectPrx[] functions = GetFunctions(propertyName);
            if (functions.Length > 0)
            {
                (new DateTimeTI(DateTimeTInterfacePrxHelper.checkedCast(functions[0]))).TryGetDateTime(
                    out returnValue);
                return returnValue;
            }
            else
            {
                Debug.WriteLine("BMI15");
                throw new ArgumentOutOfRangeException("propertyName", propertyName, "");
            }
        }

        /// <summary>
        /// Gets the date property of the specified name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>The <see cref="System.Int16">DateTime</see> value of the specified property.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// Thrown iff specified <c>propertyName</c> doesn`t match any
        /// date property.
        /// </exception>
        public DateTime GetPropertyDate(string propertyName)
        {
            PropertyValue value;
            DateTime returnValue;
            lock (this)
            {
                if (properties.TryGetValue(propertyName, out value))
                {
                    ((DateTI) value).TryGetDateTime(out returnValue);
                    return returnValue;
                }
            }

            ObjectPrx[] functions = GetFunctions(propertyName);
            if (functions.Length > 0)
            {
                (new DateTI(DateTInterfacePrxHelper.checkedCast(functions[0]))).TryGetDateTime(out returnValue);
                return returnValue;
            }
            else
            {
                Debug.WriteLine("BMI16");
                throw new ArgumentOutOfRangeException("propertyName", propertyName, "");
            }
        }

        /// <summary>
        /// Gets the time property of the specified name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>The <see cref="System.TimeSpan">Time</see> value of the specified property.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// Thrown iff specified <c>propertyName</c> doesn`t match any
        /// time property.
        /// </exception>
        public TimeSpan GetPropertyTime(string propertyName)
        {
            PropertyValue value;
            TimeSpan returnValue;
            lock (this)
            {
                if (properties.TryGetValue(propertyName, out value))
                {
                    ((TimeTI) value).TryGetTimeSpan(out returnValue);
                    return returnValue;
                }
            }

            ObjectPrx[] functions = GetFunctions(propertyName);
            if (functions.Length > 0)
            {
                (new TimeTI(TimeTInterfacePrxHelper.checkedCast(functions[0]))).TryGetTimeSpan(out returnValue);
                return returnValue;
            }
            else
            {
                Debug.WriteLine("BMI17");
                throw new ArgumentOutOfRangeException("propertyName", propertyName, "");
            }
        }

        /// <summary>
        /// Gets the "other type" property of the specified name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>The "other type" value of the specified property.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// Thrown iff specified <c>propertyName</c> doesn`t match any
        /// "other type" property.
        /// </exception>
        public PropertyValue GetPropertyOther(string propertyName)
        {
            lock (this)
            {
                PropertyValue value;
                if (properties.TryGetValue(propertyName, out value))
                    return value;
            }

            ObjectPrx[] functions = GetFunctions(propertyName);
            if (functions.Length > 0)
            {
                return boxInfo.GetPropertyObjectFromInterface(propertyName, functions[0]);
            }
            else
            {
                Debug.WriteLine("BMI18");
                throw new ArgumentOutOfRangeException("propertyName", propertyName, "");
            }
        }

        #endregion

        #region BoxInfo, Identity, Proxies, Manager, Adapter, LocalePrefs of the Box module

        /// <summary>
        /// The box info.
        /// </summary>
        private IBoxInfo boxInfo;

        /// <summary>
        /// Gets the box info.
        /// </summary>
        /// <remarks>
        /// The <see cref="T:Ferda.Modules.Boxes.IBoxInfo"/> provides
        /// some fundamental functionality so if you are developing
        /// new box module you don`t have to bother about implementing the
        /// <b>Factory Creator</b> moreover if you are using e.g.
        /// <see cref="T:Ferda.Modules.Boxes.BoxInfo"/> implementatiion of
        /// the <see cref="T:Ferda.Modules.Boxes.IBoxInfo"/> interface you
        /// don`t need to understand the theory about <b>Factory Creators</b>
        /// and <b>Factories</b> in practice.
        /// </remarks>
        /// <value>The box info.</value>
        public IBoxInfo BoxInfo
        {
            get { return boxInfo; }
        }

        /// <summary>
        /// Ice identity of the box module.
        /// </summary>
        private Identity iceIdentity;

        /// <summary>
        /// Gets Ice identity i.e. identity of the Ice object (box module).
        /// </summary>
        /// <value>Ice identity of current object (box module).</value>
        /// <remarks>
        /// Please note that this identity of box module is not
        /// presistent if you wants to get persistent box module`s
        /// identity (i.e. persistent identifier of current box module)
        /// please use <see cref="P:Ferda.Modules.BoxModuleI.PersistentIdentity"/>.
        /// </remarks>
        public Identity IceIdentity
        {
            get { return iceIdentity; }
        }

        /// <summary>
        /// String representation of the
        /// <see cref="P:Ferda.Modules.BoxModuleI.IceIdentity">ice identity</see>
        /// of current box module..
        /// </summary>
        private string stringIceIdentity;

        /// <summary>
        /// Gets a string representation of the
        /// <see cref="P:Ferda.Modules.BoxModuleI.IceIdentity">ice identity</see>
        /// of current box module..
        /// </summary>
        /// <value>String representation of Ice identity of
        /// current object (box module).</value>
        /// <remarks>
        /// Please note that this identity of box module is not
        /// presistent if you wants to get persistent box module`s
        /// identity (i.e. persistent identifier of current box module)
        /// please use <see cref="P:Ferda.Modules.BoxModuleI.PersistentIdentity"/>.
        /// </remarks>
        public string StringIceIdentity
        {
            get { return stringIceIdentity; }
        }

        /// <summary>
        /// Gets the persistent identity of the box module.
        /// </summary>
        /// <value>The persistent identity of the box module.</value>
        public int PersistentIdentity
        {
            get { return Manager.getProjectInformation().getProjectIdentifier(StringIceIdentity); }
        }

        /// <summary>
        /// Gets the output.
        /// </summary>
        /// <value>The output.</value>
        /// <remarks>
        /// Output provides some functions for writing
        /// messages to the front-end but it is not overly
        /// recomended to use this output. Please use it only
        /// if you are certain of your are going to do.
        /// </remarks>
        public OutputPrx Output
        {
            get { return manager.getOutputInterface(); }
        }

        /// <summary>
        /// The box module`s proxy
        /// </summary>
        private BoxModulePrx myProxy;

        /// <summary>
        /// Gets the proxy fo the box module.
        /// </summary>
        /// <value>Box module`s proxy.</value>
        public BoxModulePrx MyProxy
        {
            get { return myProxy; }
        }

        /// <summary>
        /// The proxy of the box module`s factory.
        /// </summary>
        private BoxModuleFactoryPrx myFactoryProxy;

        /// <summary>
        /// Gets the proxy of the box module`s factory.
        /// </summary>
        /// <param name="__current">The Ice.Current.</param>
        /// <returns>
        /// The <see cref="Ferda.Modules.BoxModuleFactoryPrx"/>.
        /// </returns>
        public override BoxModuleFactoryPrx getMyFactory(Current __current)
        {
            return myFactoryProxy;
        }

        private ManagersEnginePrx manager;

        /// <summary>
        /// Gets the manager.
        /// </summary>
        /// <value>The manager.</value>
        public ManagersEnginePrx Manager
        {
            get { return manager; }
        }

        private ObjectAdapter adapter;
        
        public ObjectAdapter Adapter
        {
        	get
        	{
        		return this.adapter;
        	}
        }

        /// <summary>
        /// The localization preferences.
        /// </summary>
        private string[] localePrefs;

        /// <summary>
        /// Gets the localization preferences.
        /// </summary>
        /// <value>The localization preferences.</value>
        public string[] LocalePrefs
        {
            get { return localePrefs; }
        }

        #endregion

        #region Box modules functions object

        /// <summary>
        /// The box module`s functions <see cref="T:Ice.Object"/>.
        /// </summary>
        private Object functionsIceObj;

        /// <summary>
        /// The box module`s functions object.
        /// </summary>
        private IFunctions functionsIObj;

        /// <summary>
        /// Gets the box module`s functions object.
        /// </summary>
        /// <value>The box module`s functions object.</value>
        public IFunctions FunctionsIObj
        {
            get { return functionsIObj; }
        }

        /// <summary>
        /// The proxy of box module`s functions object.
        /// </summary>
        private ObjectPrx functionsObjPrx;
        
        public ObjectPrx FunctionsObjPrx
        {
            get { return functionsObjPrx; }
        }

        /// <summary>
        /// <para>Gets the functions object proxy.</para>
        /// <para>
        /// Throught lambda abstraction is BoxModule interface separated from
        /// functions over this BoxModule. Functions object is module containing
        /// functions over properties and sockets of this BoxModule specified in
        /// slice design.
        /// </para>
        /// </summary>
        /// <param name="__current">The Ice.Current.</param>
        /// <returns>
        /// The <see cref="Ice.ObjectPrx">proxy </see> of the box module`s
        /// functions object.
        /// </returns>
        public override ObjectPrx getFunctions(Current __current)
        {
            return boxInfo.GetFunctionsObjPrx(this);
        }

        /// <summary>
        /// Gets the box module`s functions object`s ice ids.
        /// </summary>
        /// <param name="__current">The Ice.Current.</param>
        /// <returns>
        /// Array of <see cref="T:System.String"/> as ids
        /// of all provided functions (i.e. implemented intefaces).
        /// </returns>
        public override string[] getFunctionsIceIds(Current __current)
        {
        	ObjectPrx functionsObjProxy = getFunctions(__current);
            return (functionsObjProxy == null) ? new string[0] : functionsObjProxy.ice_ids();
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Ferda.Modules.BoxModuleI"/> class.
        /// </summary>
        /// <param name="boxInfo">The box info.</param>
        /// <param name="myIdentity">My identity.</param>
        /// <param name="myFactoryProxy">My factory proxy.</param>
        /// <param name="manager">The manager.</param>
        /// <param name="adapter">The adapter.</param>
        /// <param name="localePrefs">The localization preferences.</param>
        public BoxModuleI(IBoxInfo boxInfo,
                          Identity myIdentity,
                          BoxModuleFactoryPrx myFactoryProxy,
                          ManagersEnginePrx manager,
                          ObjectAdapter adapter,
                          string[] localePrefs)
        {
            Debug.WriteLine("BoxModuleI Constructor (entering): " + boxInfo.Identifier);

            // initializes inner fields by specified parameters
            this.boxInfo = boxInfo;
            iceIdentity = myIdentity;
            stringIceIdentity = Util.identityToString(IceIdentity);
            this.myFactoryProxy = myFactoryProxy;
            this.manager = manager;
            this.adapter = adapter;
            this.localePrefs = localePrefs;

            // add the new box module to the specified adapter
            adapter.add(this, iceIdentity);
            // get my proxy
            myProxy = BoxModulePrxHelper.uncheckedCast(adapter.createProxy(myIdentity));

            // initializes box module`s functions object
            this.boxInfo.CreateFunctions(this, out functionsIceObj, out functionsIObj);
			
			if(functionsIObj != null)
				functionsIObj.setBoxModuleInfo(this, this.boxInfo);
            if(functionsIceObj != null)
				functionsObjPrx = ObjectPrxHelper.uncheckedCast(adapter.addWithUUID(functionsIceObj));

            // initializes properties
            properties = new Dictionary<string, PropertyValue>();
            foreach (string propertyName in boxInfo.GetPropertiesNames())
            {
                if (!boxInfo.IsPropertyReadOnly(propertyName))
                {
                    setProperty(propertyName, boxInfo.GetPropertyDefaultValue(propertyName));
                }
            }

            // initializes sockets (connections and functions)
            connections = new Dictionary<string, Dictionary<string, BoxModulePrx>>();
            foreach (string socketName in boxInfo.GetSocketNames())
            {
                connections[socketName] = new Dictionary<string, BoxModulePrx>();
            }

            Debug.WriteLine("BoxModuleI Constructor (leaving): " + this.boxInfo.Identifier);
        }

        /// <summary>
        /// Gets the additional sockets. Some boxes can create sockets dynamicaly from user input, for example "lambda box".
        /// </summary>
        /// <param name="__current">The Ice.Current.</param>
        /// <returns>
        /// Array of <see cref="T:Ferda.Modules.SocketInfo"/> as
        /// possible additional sockets.
        /// </returns>
        /// <remarks>For lamda-like boxes.</remarks>
        public override SocketInfo[] getAdditionalSockets(Current __current)
        {
            return boxInfo.GetAdditionalSockets(localePrefs, this);
        }
		
		/// <summary>
        /// Gets the additional properties. Some boxes can create properties dynamicaly from user input, for example "lambda box".
        /// </summary>
        /// <param name="__current">The Ice.Current.</param>
        /// <returns>
        /// Array of <see cref="T:Ferda.Modules.PropertyInfo"/> as
        /// possible additional properties.
        /// </returns>
        /// <remarks>For lamda-like boxes.</remarks>
		public override PropertyInfo[] getAdditionalProperties(Current __current)
		{
			return boxInfo.GetAdditionalProperties(localePrefs, this);
		}

        /// <summary>
        /// Gets the box modules asking for creation.
        /// </summary>
        /// <param name="__current">The Ice.Current.</param>
        /// <returns>
        /// Array of <see cref="T:Ferda.Modules.ModuleAskingForCreation">
        /// Modules Asking For Creation</see>.
        /// </returns>
        /// <remarks>
        /// Modules asking for creation dynamically depends on actual
        /// inner state of the box module.
        /// </remarks>
        public override ModulesAskingForCreation[] getModulesAskingForCreation(Current __current)
        {
            return boxInfo.GetModulesAskingForCreation(localePrefs, this);
        }

        /// <summary>
        /// Gets array of <see cref="T:Ferda.Modules.SelectString"/> as
        /// options for the specified property, whose options are dynamically variable.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="__current">The Ice.Current.</param>
        /// <returns>
        /// An array of <see cref="T:Ferda.Modules.SelectString"/>
        /// as list of options for property named <c>propertyName</c>.
        /// </returns>
        /// <exception cref="T:Ferda.Modules.NameNotExistError">
        /// Thrown if property named <c>propertyName</c> doesn`t exist in the box module.
        /// </exception>
        public override SelectString[] getPropertyOptions(string propertyName, Current __current)
        {
            // tests if there is the specified property exists
            if (!boxInfo.TestPropertyNameExistence(propertyName))
            {
                Debug.Assert(false);
                throw Exceptions.NameNotExistError(null, propertyName);
            }
            // gets and returns the options
            return boxInfo.GetPropertyOptions(propertyName, this);
        }

        /// <summary>
        /// Determines whether specified property (<c>propertyName</c>)
        /// is set. It is usefull for properties of other than basic types.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="__current">The Ice.Current.</param>
        /// <returns>
        /// <c>true</c> if the specified property is set; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:Ferda.Modules.NameNotExistError">
        /// Thrown if property named <c>propertyName</c> doesn`t exist in the box module.
        /// </exception>
        public override bool isPropertySet(string propertyName, Current __current)
        {
            // tests property existence
            if (!boxInfo.TestPropertyNameExistence(propertyName))
            {
                Debug.Assert(false);
                throw Exceptions.NameNotExistError(null, propertyName);
            }
            // tests property value if it is set
            return boxInfo.IsPropertySet(propertyName, getProperty(propertyName));
        }

        /// <summary>
        /// Determines whether the specified <c>sockets</c> satisfy
        /// the condition on required sockets (<c>neededSockets</c>)
        /// i.e. if there are sockets of the same name and the same type
        /// (<see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.NeededSocket.FunctionIceId"/>)
        /// as required.
        /// </summary>
        /// <param name="neededSockets">The needed sockets.</param>
        /// <param name="sockets">The sockets.</param>
        /// <returns>
        /// <c>true</c> if the specified <c>sockets</c>satisfy
        /// <c>neededSockets</c> restrictions; otherwise, <c>false</c>.
        /// </returns>
        private static bool hasSockets(NeededSocket[] neededSockets, SocketInfo[] sockets)
        {
            // creates dictionary of sockets
            Dictionary<string, SocketInfo> socketsDict = new Dictionary<string, SocketInfo>();
            foreach (SocketInfo socket in sockets)
            {
                socketsDict[socket.name] = socket;
            }
            // tests if each needed socket is present and implements required interface
            foreach (NeededSocket socketNeeded in neededSockets)
            {
                // tests if needed socket is present
                SocketInfo socketInfo;
                if (!socketsDict.TryGetValue(socketNeeded.socketName, out socketInfo))
                {
                    // needed socket is not present
                    return false;
                }
                // tests if the socket has needed boxtype
                BoxType[] boxTypeSeq = socketInfo.socketType;
                bool finded = false;
                foreach (BoxType boxType in boxTypeSeq)
                {
                    if (boxType.functionIceId == socketNeeded.functionIceId)
                    {
                        // the socket has needed boxtype
                        finded = true;
                        break;
                    }
                }
                // the socket has not needed boxtype
                if (!finded)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Determines whether specified functions object (<c>functionsPrx</c>)
        /// is satisfy specified box type (<c>boxType</c>) i.e. whether
        /// functions object implements required functions and its box module
        /// has required sockets.
        /// </summary>
        /// <param name="boxType">Required type of the box.</param>
        /// <param name="functionsPrx">The functions object`s proxy.</param>
        /// <param name="sockets">The sockets of box module to which the functions object belongs to.</param>
        /// <returns>
        /// <c>true</c> if the specified functions object
        /// <see cref="M:Ice.ObjectPrx.ice_isA(System.String)">
        /// implements required functions</see> and and its box module
        /// <see cref="M:Ferda.Modules.BoxModuleI.hasSockets(Ferda.Modules.NeededSocket[],Ferda.Modules.SocketInfo[])">
        /// has required sockets</see>.
        /// </returns>
        private static bool hasBoxType(BoxType boxType, ObjectPrx functionsPrx, SocketInfo[] sockets)
        {
        	if(boxType == null || functionsPrx == null) return true;
            return functionsPrx.ice_isA(boxType.functionIceId) &&
                   hasSockets(boxType.neededSockets, sockets);
        }

        /// <summary>
        /// Sets the connection to specified box module (<c>otherModule</c>)
        /// in specified socket (<c>socketName</c>).
        /// </summary>
        /// <param name="socketName">Name of the socket.</param>
        /// <param name="otherModule">The other module.</param>
        /// <param name="__current">The Ice.Current.</param>
        /// <exception cref="T:Ferda.Modules.NameNotExistError">
        /// Thrown if socket named <c>socketName</c> doesn`t exist in the box module.
        /// </exception>
        /// <exception cref="T:Ferda.Modules.BadTypeError">
        /// Thrown if the type of functions provided by
        /// the <c>otherModule</c> is not accepted by the socket.
        /// </exception>
        /// <exception cref="T:Ferda.Modules.ConnectionExistsError">
        /// Thrown if the socket accept only one connection and it is already used.
        /// </exception>
        public override void setConnection(string socketName, BoxModulePrx otherModule, Current __current)
        {
            // tests specified socketName existence
            if (!boxInfo.TestSocketNameExistence(socketName, this))
            {
                Debug.Assert(false);
                throw Exceptions.NameNotExistError(null, socketName);
            }

            // tests Ferda.Modules.BoxType of otherModule
            bool badTypeError = true;
            ObjectPrx objPrx = otherModule.getFunctions();
            SocketInfo[] otherModuleSocketInfos = otherModule.getMyFactory().getSockets();
            foreach (BoxType socketBoxType in
                boxInfo.GetSocketTypes(socketName, this))
            {
                // tests otherModule`s functions type (functionsPrx.ice_isA)
                // tests if otherModule has needed sockets
                if (hasBoxType(socketBoxType, objPrx, otherModuleSocketInfos))
                {
                    badTypeError = false;
                    break;
                }
            }
            if (badTypeError)
            {
                // type of otherModule`s functions is bad
                Debug.WriteLine("BMI22");
                throw new BadTypeError();
            }

            string identity = Util.identityToString(otherModule.ice_getIdentity());

            lock (this)
            {
                // the socket is actually property (i.e. value of the property is set by its socket)
                if (boxInfo.TestPropertyNameExistence(socketName))
                {
                    properties.Remove(socketName);
                }
                else
                {
                    // tests if socket (accepting only one connection) is already full
                    if ((!boxInfo.IsSocketMoreThanOne(socketName, this)) &&
                        getConnection(socketName).Count != 0)
                    {
                        // the socket is already used -> exception is thrown
                        Debug.WriteLine("BMI23");
                        throw new ConnectionExistsError();
                    }
                }
                getConnection(socketName)[identity] = otherModule;
            }
        }

        /// <summary>
        /// Executes specified action (<c>actionName</c>)
        /// </summary>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="__current">The Ice.Current.</param>
        /// <remarks>
        /// For the duration of execution of the specified action
        /// is the box module (recursively) locked.
        /// </remarks>
        /// <exception cref="T:Ferda.Modules.NeedConnectedSocketError">
        /// Thrown if condition on
        /// <see cref="M:Ferda.Modules.Boxes.IBoxInfo.GetActionInfoNeededConnectedSockets(System.String)">needed connected sockets</see>
        /// is not satisfied.
        /// </exception>
        /// <exception cref="T:Ferda.Modules.NameNotExistError">
        /// Thrown if action named <c>actionName</c> doesn`t exist.
        /// </exception>
        /// <exception cref="T:Ferda.Modules.BoxRuntimeError">
        /// Thrown if any runtime error or exception occured during execution of the action.
        /// </exception>
        public override void runAction(string actionName, Current __current)
        {
            lock (this)
            {
                bool neededSocketsConnected = true;
                foreach (string[] neededSockets in boxInfo.GetActionInfoNeededConnectedSockets(actionName))
                {
                    neededSocketsConnected = true;
                    foreach (string neededSocket in neededSockets)
                    {
                        if (!(getConnection(neededSocket).Count > 0))
                        {
                            neededSocketsConnected = false;
                            break;
                        }
                    }
                    if (neededSocketsConnected)
                        break;
                }
                if (!neededSocketsConnected)
                {
                    Debug.WriteLine("BMI24");
                    throw new NeedConnectedSocketError();
                }

                // lock the box module
                manager.getBoxModuleLocker().lockBoxModule(StringIceIdentity);

                try
                {
                    boxInfo.RunAction(actionName, this);
                    //throws BoxRuntimeError, NameNotExistError
                }
                catch (BoxRuntimeError e)
                {
                    // na vyber jestli poslad vyjimku nebo implicitni konstrukci vysledku
                    if (String.IsNullOrEmpty(e.boxIdentity))
                        e.boxIdentity = StringIceIdentity;
                    Debug.Assert(!String.IsNullOrEmpty(e.boxIdentity));
                    Debug.Assert(!String.IsNullOrEmpty(e.userMessage));
                    throw;
                }
                catch (Ice.Exception e)
                {
                    Debug.Assert(false);
                    throw Exceptions.BoxRuntimeError(e, StringIceIdentity, "Unexpected Ice exception." + e.Message);
                }
                catch (Exception e)
                {
                    Debug.Assert(false);
                    throw Exceptions.BoxRuntimeError(e, StringIceIdentity, "Unexpected exception." + e.Message);
                }
                finally
                {
                    // unlock the box module
                    manager.getBoxModuleLocker().unlockBoxModule(StringIceIdentity);
                }
            }
        }

        /// <summary>
        /// Sets the specified property (<c>propertyName</c>) by
        /// specified <c>propertyValue</c>.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="propertyValue">The property value.</param>
        /// <param name="__current">The Ice.Current.</param>
        /// <exception cref="T:Ferda.Modules.NameNotExistError">
        /// Thrown iff there is no property of specified <c>propertyName</c>.
        /// </exception>
        /// <exception cref="T:Ferda.Modules.BadTypeError">
        /// Thrown iff specified <c>propertyValue</c> is not of the
        /// specified property data type. See <code>Ferda.Modules.Boxes.Serializer.Configuration.Property.TypeClassIceId</code>.
        /// </exception>
        /// <exception cref="T:Ferda.Modules.ReadOnlyError">
        /// Thrown iff specified property is read only.
        /// </exception>
        public override void setProperty(string propertyName, PropertyValue propertyValue, Current __current)
        {
            if (!boxInfo.TestPropertyNameExistence(propertyName))
            {
                // there is no property of the specified propertyName
                Debug.Assert(false);
                throw Exceptions.NameNotExistError(null, propertyName);
            }
            if (propertyValue != null && !propertyValue.ice_isA(boxInfo.GetPropertyDataType(propertyName)))
            {
                // bad type of the specified propertyValue
                Debug.WriteLine("BMI26");
                throw new BadTypeError();
            }
            if (boxInfo.IsPropertyReadOnly(propertyName))
            {
                // the specified property is readonly
                Debug.WriteLine("BMI27");
                throw new ReadOnlyError();
            }
            // switch data type of the property
            // check if new propertyValue satisfy restrictions of the property
            // destroy old propetyValue object`s proxy
            // add new propertyValue object to the adapter
            // and save the proxy of added object
            lock (this)
            {
                switch (boxInfo.GetPropertyDataType(propertyName))
                {
                    case "::Ferda::Modules::ShortT":
                        PropertyValueRestrictionsHelper.TryIsIntegralPropertyCorrect(
                            boxInfo,
                            propertyName,
                            ((ShortT) propertyValue).getShortValue());
                        break;
                    case "::Ferda::Modules::IntT":
                        PropertyValueRestrictionsHelper.TryIsIntegralPropertyCorrect(
                            boxInfo,
                            propertyName,
                            ((IntT) propertyValue).getIntValue());
                        break;
                    case "::Ferda::Modules::LongT":
                        PropertyValueRestrictionsHelper.TryIsIntegralPropertyCorrect(
                            boxInfo,
                            propertyName,
                            ((LongT) propertyValue).getLongValue());
                        break;
                    case "::Ferda::Modules::FloatT":
                        PropertyValueRestrictionsHelper.TryIsFloatingPropertyCorrect(
                            boxInfo,
                            propertyName,
                            ((FloatT) propertyValue).getFloatValue());
                        break;
                    case "::Ferda::Modules::DoubleT":
                        PropertyValueRestrictionsHelper.TryIsFloatingPropertyCorrect(
                            boxInfo,
                            propertyName,
                            ((DoubleT) propertyValue).getDoubleValue());
                        break;
                    case "::Ferda::Modules::StringT":
                        PropertyValueRestrictionsHelper.TryIsStringPropertyCorrect(
                            boxInfo,
                            propertyName,
                            ((StringT) propertyValue).getStringValue());
                        break;
                    case "::Ferda::Modules::DateTimeT":
                        PropertyValueRestrictionsHelper.TryIsDateTimePropertyCorrect(
                            boxInfo, propertyName, (DateTimeT) propertyValue);
                        break;
                    case "::Ferda::Modules::DateT":
                        DateT date = new DateTI();
                        ((DateT) propertyValue).getDateValue(out date.year, out date.month, out date.day);
                        PropertyValueRestrictionsHelper.TryIsDatePropertyCorrect(
                            boxInfo, propertyName, date);
                        break;
                    case "::Ferda::Modules::TimeT":
                        TimeT time = new TimeTI();
                        ((TimeT) propertyValue).getTimeValue(out time.hour, out time.minute, out time.second);
                        PropertyValueRestrictionsHelper.TryIsTimePropertyCorrect(
                            boxInfo, propertyName, time);
                        break;
                }
                // proxy of new propertyValue is already saved
                // save the propertyValue
                properties[propertyName] = propertyValue;
            }
        }

        /// <summary>
        /// Gets the <see cref="T:Ferda.Modules.PropertySetting"/>
        /// for the specified property (<c>propertyName</c>).
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        /// The <see cref="T:Ferda.Modules.PropertySetting"/> i.e.
        /// value and name of the specified property.
        /// </returns>
        public PropertySetting GetPropertySetting(string propertyName)
        {
            PropertySetting result = new PropertySetting();
            result.propertyName = propertyName;
            result.value = getProperty(propertyName);
            return result;
        }

        /// <summary>
        /// Gets the value of the specified read only property (<c>propertyName</c>).
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        /// The <see cref="T:Ferda.Modules.PropertyValue"/>.
        /// </returns>
        private PropertyValue getReadOnlyPropertyValue(string propertyName)
        {
            if (!boxInfo.TestPropertyNameExistence(propertyName))
            {
                Debug.Assert(false);
                throw Exceptions.NameNotExistError(null, propertyName);
            }
            if (!boxInfo.IsPropertyReadOnly(propertyName))
            {
                string message = "BMI01: Property " + propertyName + " is not readonly as expected";
                Debug.WriteLine(message);
                throw new Exception(message);
            }
            return boxInfo.GetReadOnlyPropertyValue(propertyName, this);
        }

        /// <summary>
        /// Gets the value of the specified property (<c>propertyName</c>).
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="__current">The Ice.Current.</param>
        /// <returns>
        /// The <see cref="T:Ferda.Modules.PropertyValue"/>.
        /// </returns>
        /// <exception cref="T:Ferda.Modules.NameNotExistError">
        /// Thrown iff there is no property of specified <c>propertyName</c>.
        /// </exception>
        public override PropertyValue getProperty(string propertyName, Current __current)
        {
            // tests if property of specified name exists
            if (!boxInfo.TestPropertyNameExistence(propertyName))
            {
                Debug.Assert(false);
                throw Exceptions.NameNotExistError(null, propertyName);
            }

            // readonly properties
            if (boxInfo.IsPropertyReadOnly(propertyName))
            {
                PropertyValue result = getReadOnlyPropertyValue(propertyName);
                if (result != null)
                    return result;
                else // returns "empty" PropertyValue
                {
                    string propertyDataType = boxInfo.GetPropertyDataType(propertyName);
                    switch (propertyDataType)
                    {
                        case "::Ferda::Modules::BoolT":
                            return new BoolTI(false);
                        case "::Ferda::Modules::ShortT":
                            return new ShortTI(0);
                        case "::Ferda::Modules::IntT":
                            return new IntTI();
                        case "::Ferda::Modules::LongT":
                            return new LongTI(0);
                        case "::Ferda::Modules::FloatT":
                            return new FloatTI(0);
                        case "::Ferda::Modules::DoubleT":
                            return new DoubleTI(0);
                        case "::Ferda::Modules::StringT":
                            return new StringTI("");
                        case "::Ferda::Modules::DateT":
                            return new DateTI(0, 0, 0);
                        case "::Ferda::Modules::DateTimeT":
                            return new DateTimeTI(0, 0, 0, 0, 0, 0);
                        case "::Ferda::Modules::TimeT":
                            return new TimeTI(0, 0, 0);
                        default:
                            throw new NotImplementedException();
                    }
                }
            }

            lock (this)
            {
                if (properties.ContainsKey(propertyName))
                {
                    return properties[propertyName];
                }
            }
            switch (boxInfo.GetPropertyDataType(propertyName))
            {
                case "::Ferda::Modules::BoolT":
                    return new BoolTI(GetPropertyBool(propertyName));
                case "::Ferda::Modules::ShortT":
                    return new ShortTI(GetPropertyShort(propertyName));
                case "::Ferda::Modules::IntT":
                    return new IntTI(GetPropertyInt(propertyName));
                case "::Ferda::Modules::LongT":
                    return new LongTI(GetPropertyLong(propertyName));
                case "::Ferda::Modules::FloatT":
                    return new FloatTI(GetPropertyFloat(propertyName));
                case "::Ferda::Modules::DoubleT":
                    return new DoubleTI(GetPropertyDouble(propertyName));
                case "::Ferda::Modules::StringT":
                    return new StringTI(GetPropertyString(propertyName));
                case "::Ferda::Modules::DateTimeT":
                    return new DateTimeTI(GetPropertyDateTime(propertyName));
                case "::Ferda::Modules::DateT":
                    return new DateTI(GetPropertyDate(propertyName));
                case "::Ferda::Modules::TimeT":
                    return new TimeTI(GetPropertyTime(propertyName));
                default:
                    return GetPropertyOther(propertyName);
            }
        }

        /// <summary>
        /// Gets items of the dynamic help.
        /// </summary>
        /// <param name="__current">The Ice.Current.</param>
        /// <returns>
        /// Array of <seealso cref="T:Ferda.Modules.DynamicHelpItem">DynamicHelpItems</seealso>.
        /// </returns>
        public override DynamicHelpItem[] getDynamicHelpItems(Current __current)
        {
            return boxInfo.GetDynamicHelpItems(localePrefs, this);
        }

        /// <summary>
        /// Gets default value for box module`s user label. This label
        /// should predicate box module`s settings (values of its properties)
        /// for easier working with many boxes on user desktop (in FrontEnd).
        /// </summary>
        /// <param name="__current">The Ice.Current.</param>
        /// <returns>
        /// <para>
        /// Result is String representing default user label of the box module.
        /// This label can change in time ... it can reflect inner
        /// box module`s state e.g. up-to-date values of its properties.
        /// </para>
        /// <para>
        /// (StringOpt) If result string is null or empty returns empty string
        /// array; otherwise, string array with one member.
        /// </para>
        /// </returns>
        public override string[] getDefaultUserLabel(Current __current)
        {
            string result = boxInfo.GetDefaultUserLabel(this);
            if (String.IsNullOrEmpty(result))
                return new string[0];
            else
                return new string[1] {result};
        }

        /// <summary>
        /// Gets the specified phrase (<c>phraseIdentifier</c>).
        /// </summary>
        /// <param name="phraseIdentifier">The phrase`s identifier.</param>
        /// <param name="isLocalized">
        /// if set to <c>true</c> localization exists; otherwise,
        /// <c>phraseIdentifier</c> is returned.
        /// </param>
        /// <returns>
        /// The phrase`s localized text (if it exists; otherwise,
        /// <c>phraseIdentifier</c> is returned).
        /// </returns>
        public string GetPhrase(string phraseIdentifier, out bool isLocalized)
        {
            string result;
            isLocalized = boxInfo.TryGetPhrase(phraseIdentifier, out result, localePrefs);
            return result;
        }

        /// <summary>
        /// Outputs the message.
        /// </summary>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="messageTitle">The message title (phrase identifier).</param>
        /// <param name="messageText">The message text (phrase identifier).</param>
        public void OutputMessage(MsgType messageType, string messageTitle, string messageText)
        {
            bool titleLocalized;
            bool textLocalized;
            Output.writeMsg(
                messageType,
                GetPhrase(messageTitle, out titleLocalized),
                GetPhrase(messageText, out textLocalized)
                );
            Debug.WriteLineIf(!titleLocalized,
                              "BMI28: box module `" + boxInfo.Identifier + "` has not localized phrase `" + messageTitle +
                              "`");
            Debug.WriteLineIf(!textLocalized,
                              "BMI29: box module `" + boxInfo.Identifier + "` has not localized phrase `" + messageText +
                              "`");
        }

        #region Destroying

        /// <summary>
        /// Destroys all Ice objects inherent to the box module
        /// (e.g. <see cref="T:Ferda.Modules.PropertyValue">property values</see>)
        /// i.e. remove their proxies from the specified <c>adapter</c>.
        /// </summary>
        /// <param name="adapter">The adapter.</param>
        public void destroy(ObjectAdapter adapter)
        {
        }

        #endregion

        /// <summary>
        /// Validates setting of this box module.
        /// </summary>
        /// <param name="__current">The Ice.Current.</param>
        /// <remarks>
        /// <para>
        /// Some settings may cause to exceptions or some error states.
        /// </para>
        /// <para>
        /// Validates the specified box module. (e.g. setting of some properties
        /// is right (satisfies its restrictions) but box module can not work with
        /// this setting e.g. property "OdbcConnectionString" is valid ODBC connection
        /// string but the box module can not connect with given value to the
        /// specified data source.)
        /// </para>
        /// <para>
        /// E. g. if (current) box module provides OdbcConnectionString
        /// and its value is bad (not valid ODBC connection string) then
        /// if another box module wants to use the (bad) value of the
        /// connection string, probably exception will be thrown but
        /// the error occured because of bad param of current box and
        /// its property OdbcConnectionString. So, the other box, where
        /// the error occured, should call (job of ModulesManager) function
        /// Validate on current box and current box should test validity
        /// and usability of the OdbcConnectionString.
        /// </para>
        /// <para>
        /// If setting of current box is bad (may leads to some errors
        /// of exceptions) than some exception is thrown.
        /// </para>
        /// </remarks>
        public override void validate(Current __current)
        {
            try
            {
                boxInfo.Validate(this);
            }
            catch (BoxRuntimeError)
            {
                throw;
            }
            catch(Ice.Exception e)
            {
                throw new BoxRuntimeError(e);
            }
            catch (Exception e)
            {
                throw new BoxRuntimeError(e);
            }
        }
        
        private Dictionary<string, BoxModulePrx> getConnection(string socketName)
        {
			lock (this)
			{
				Dictionary<string, BoxModulePrx> result = null;
				//now test if there is socket socketName

				if(!connections.TryGetValue(socketName, out result))
				{
					StringCollection socketNames = this.boxInfo.GetAdditionalSocketsNames(this);
					if(socketNames.Contains(socketName))
					{
						if(!connectionsOfAdditionalSockets.TryGetValue(socketName, out result))
						{
							result = new Dictionary<string, BoxModulePrx>();
							connectionsOfAdditionalSockets[socketName] = result;
						}
					}
					else
					{
						string message = "BMI03";
						Debug.WriteLine(message);
						throw new ArgumentOutOfRangeException("socketName", socketName, message);
					}
				}
				return result;
			}
        }
    }
}
