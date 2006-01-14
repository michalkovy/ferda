using System;
using System.Collections.Generic;
using System.Text;
using Ferda.Modules.Boxes;
using System.Diagnostics;

namespace Ferda.Modules
{
    //TODO Michal
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
            try
            {
                lock (connections)
                {
                    BoxModulePrx[] result = new BoxModulePrx[connections[socketName].Values.Count];
                    connections[socketName].Values.CopyTo(result, 0);
                    return result;
                }
            }
            catch (KeyNotFoundException ex)
            {
                string message = "BMI03: " + ex.Message;
                Debug.WriteLine(message);
                throw new ArgumentOutOfRangeException("socketName", socketName, message);
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
        public override BoxModulePrx[] getConnections(string socketName, Ice.Current __current)
        {
            if (!this.boxInfo.TestSocketNameExistence(socketName))
            {
                throw Ferda.Modules.Exceptions.NameNotExistError(null, null, "BMI05", socketName);
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
        public override void removeConnection(string socketName, string boxModuleIceIdentity, Ice.Current __current)
        {
            if (!this.boxInfo.TestSocketNameExistence(socketName))
            {
                throw Ferda.Modules.Exceptions.NameNotExistError(null, null, "BMI06", socketName);
            }
            lock (this)
            {
                try
                {
                    this.connections[socketName].Remove(boxModuleIceIdentity);
                    this.functions[socketName].Remove(boxModuleIceIdentity);
                }
                catch
                {
                    Debug.WriteLine("BMI04");
                    throw new Ferda.Modules.ConnectionNotExistError();
                }
                if (boxInfo.TestPropertyNameExistence(socketName))
                {
                    switch (boxInfo.GetPropertyDataType(socketName))
                    {
                        case "::Ferda::Modules::BoolT":
                            lock (propertiesBool)
                            {
                                propertiesBool.Remove(socketName);
                            }
                            break;
                        case "::Ferda::Modules::ShortT":
                            lock (propertiesShort)
                            {
                                propertiesShort.Remove(socketName);
                            }
                            break;
                        case "::Ferda::Modules::IntT":
                            lock (propertiesInt)
                            {
                                propertiesInt.Remove(socketName);
                            }
                            break;
                        case "::Ferda::Modules::LongT":
                            lock (propertiesLong)
                            {
                                propertiesLong.Remove(socketName);
                            }
                            break;
                        case "::Ferda::Modules::FloatT":
                            lock (propertiesFloat)
                            {
                                propertiesFloat.Remove(socketName);
                            }
                            break;
                        case "::Ferda::Modules::DoubleT":
                            lock (propertiesDouble)
                            {
                                propertiesDouble.Remove(socketName);
                            }
                            break;
                        case "::Ferda::Modules::StringT":
                            lock (propertiesString)
                            {
                                propertiesString.Remove(socketName);
                            }
                            break;
                        case "::Ferda::Modules::DateTimeT":
                            lock (propertiesDateTime)
                            {
                                propertiesDateTime.Remove(socketName);
                            }
                            break;
                        case "::Ferda::Modules::DateT":
                            lock (propertiesDate)
                            {
                                propertiesDate.Remove(socketName);
                            }
                            break;
                        case "::Ferda::Modules::TimeT":
                            lock (propertiesTime)
                            {
                                propertiesTime.Remove(socketName);
                            }
                            break;
                        default:
                            lock (propertiesOther)
                            {
                                propertiesOther.Remove(socketName);
                            }
                            break;
                    }
                    this.setProperty(socketName, boxInfo.GetPropertyDefaultValue(socketName));
                }
            }
        }
        #endregion

        #region Functions i.e. functions in sockets
        /// <summary>
        /// <para><c>Key</c> is the socket`s name.</para>
        /// <para><c>Value</c> is next Dictionary.</para>
        /// <para>
        /// Inner <c>Key</c> is <see cref="M:Ice.Util.identityToString(Ice.Identity)">string</see>
        /// representation of the box module`s ice identifier.
        /// </para>
        /// <para>
        /// Inner <c>Value</c> is the proxy of the connected box module`s functions object.
        /// </para>
        /// </summary>
        private Dictionary<string, Dictionary<string, Ice.ObjectPrx>> functions;
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
        public Ice.ObjectPrx[] GetFunctions(string socketName)
        {
            try
            {
                lock (functions)
                {
                    Ice.ObjectPrx[] result = new Ice.ObjectPrx[functions[socketName].Values.Count];
                    functions[socketName].Values.CopyTo(result, 0);
                    return result;
                }
            }
            catch (KeyNotFoundException ex)
            {
                string message = "BMI07: " + ex.Message;
                Debug.WriteLine(message);
                throw new ArgumentOutOfRangeException("socketName", socketName, message);
            }
        }
        #endregion

        /// <summary>
        /// Properties which are set by <see cref="T:Ferda.Modules.PropertyValue"/>
        /// (Tthere are not properties, which are set by PropertyBoxes 
        /// which implements interfacesof property values.)
        /// </summary>
        private Dictionary<string, PropertyValue> properties;

        #region Properties providers by their type (short, bool, int, long, float, double, string, datetime, date, time and other)

        /// <summary>
        /// The short integer properties.
        /// </summary>
        private Dictionary<string, ShortTInterfacePrx> propertiesShort =
            new Dictionary<string, ShortTInterfacePrx>();
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
            try
            {
                lock (propertiesShort)
                {
                    return propertiesShort[propertyName].getShortValue();
                }
            }
            catch (KeyNotFoundException ex)
            {
                Debug.WriteLine("BMI08");
                throw new ArgumentOutOfRangeException("propertyName", propertyName, ex.Message);
            }
        }

        /// <summary>
        /// The boolean properties.
        /// </summary>
        private Dictionary<string, BoolTInterfacePrx> propertiesBool =
            new Dictionary<string, BoolTInterfacePrx>();
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
            try
            {
                lock (propertiesBool)
                {
                    return propertiesBool[propertyName].getBoolValue();
                }
            }
            catch (KeyNotFoundException ex)
            {
                Debug.WriteLine("BMI09");
                throw new ArgumentOutOfRangeException("propertyName", propertyName, ex.Message);
            }
        }

        /// <summary>
        /// The integer properties.
        /// </summary>   
        private Dictionary<string, IntTInterfacePrx> propertiesInt =
            new Dictionary<string, IntTInterfacePrx>();
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
            try
            {
                lock (propertiesInt)
                {
                    return propertiesInt[propertyName].getIntValue();
                }
            }
            catch (KeyNotFoundException ex)
            {
                Debug.WriteLine("BMI10");
                throw new ArgumentOutOfRangeException("propertyName", propertyName, ex.Message);
            }
        }

        /// <summary>
        /// The long integer properties.
        /// </summary>
        private Dictionary<string, LongTInterfacePrx> propertiesLong =
            new Dictionary<string, LongTInterfacePrx>();
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
            try
            {
                lock (propertiesLong)
                {
                    return propertiesLong[propertyName].getLongValue();
                }
            }
            catch (KeyNotFoundException ex)
            {
                Debug.WriteLine("BMI11");
                throw new ArgumentOutOfRangeException("propertyName", propertyName, ex.Message);
            }
        }

        /// <summary>
        /// The float properties.
        /// </summary>
        private Dictionary<string, FloatTInterfacePrx> propertiesFloat =
            new Dictionary<string, FloatTInterfacePrx>();
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
            try
            {
                lock (propertiesFloat)
                {
                    return propertiesFloat[propertyName].getFloatValue();
                }
            }
            catch (KeyNotFoundException ex)
            {
                Debug.WriteLine("BMI12");
                throw new ArgumentOutOfRangeException("propertyName", propertyName, ex.Message);
            }
        }

        /// <summary>
        /// The double properties.
        /// </summary>
        private Dictionary<string, DoubleTInterfacePrx> propertiesDouble =
            new Dictionary<string, DoubleTInterfacePrx>();
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
            try
            {
                lock (propertiesDouble)
                {
                    return propertiesDouble[propertyName].getDoubleValue();
                }
            }
            catch (KeyNotFoundException ex)
            {
                Debug.WriteLine("BMI13");
                throw new ArgumentOutOfRangeException("propertyName", propertyName, ex.Message);
            }
        }

        /// <summary>
        /// The string properties.
        /// </summary>
        private Dictionary<string, StringTInterfacePrx> propertiesString =
            new Dictionary<string, StringTInterfacePrx>();
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
            try
            {
                lock (propertiesString)
                {
                    return propertiesString[propertyName].getStringValue();
                }
            }
            catch (KeyNotFoundException ex)
            {
                Debug.WriteLine("BMI14");
                throw new ArgumentOutOfRangeException("propertyName", propertyName, ex.Message);
            }
        }

        /// <summary>
        /// The datetime properties.
        /// </summary>
        private Dictionary<string, DateTimeTInterfacePrx> propertiesDateTime =
            new Dictionary<string, DateTimeTInterfacePrx>();
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
            try
            {
                int year;
                short month, day, hour, minute, second;
                lock (propertiesDateTime)
                {
                    propertiesDateTime[propertyName].getDateTimeValue(out year, out month, out day, out hour, out minute, out second);
                }
                return new DateTime(year, month, day, hour, minute, second);
            }
            catch (KeyNotFoundException ex)
            {
                Debug.WriteLine("BMI15");
                throw new ArgumentOutOfRangeException("propertyName", propertyName, ex.Message);
            }
        }

        /// <summary>
        /// The date properties.
        /// </summary>
        private Dictionary<string, DateTInterfacePrx> propertiesDate =
            new Dictionary<string, DateTInterfacePrx>();
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
            try
            {
                int year;
                short month, day;
                lock (propertiesDate)
                {
                    propertiesDate[propertyName].getDateValue(out year, out month, out day);
                }
                return new DateTime(year, month, day);
            }
            catch (KeyNotFoundException ex)
            {
                Debug.WriteLine("BMI16");
                throw new ArgumentOutOfRangeException("propertyName", propertyName, ex.Message);
            }
        }

        /// <summary>
        /// The time properties.
        /// </summary>
        private Dictionary<string, TimeTInterfacePrx> propertiesTime =
            new Dictionary<string, TimeTInterfacePrx>();
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
            try
            {
                short hour, minute, second;
                lock (propertiesTime)
                {
                    propertiesTime[propertyName].getTimeValue(out hour, out minute, out second);
                }
                return new TimeSpan(hour, minute, second);
            }
            catch (KeyNotFoundException ex)
            {
                Debug.WriteLine("BMI17");
                throw new ArgumentOutOfRangeException("propertyName", propertyName, ex.Message);
            }
        }

        /// <summary>
        /// The other properties.
        /// </summary>
        private Dictionary<string, Ice.ObjectPrx> propertiesOther =
            new Dictionary<string, Ice.ObjectPrx>();
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
            try
            {
                lock (propertiesOther)
                {
                    return boxInfo.GetPropertyObjectFromInterface(propertyName, propertiesOther[propertyName]);
                }
            }
            catch (KeyNotFoundException ex)
            {
                Debug.WriteLine("BMI18");
                throw new ArgumentOutOfRangeException("propertyName", propertyName, ex.Message);
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
        /// The <see cref="T:Ferda.Modules.Boxex.IBoxInfo"/> provides 
        /// some fundamental functionality so if you are developing 
        /// new box module you don`t have to bother about implementing the 
        /// <b>Factory Creator</b> moreover if you are using e.g. 
        /// <see cref="T:Ferda.Modules.Boxex.BoxInfo"/> implementatiion of
        /// the <see cref="T:Ferda.Modules.Boxex.IBoxInfo"/> interface you
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
        private Ice.Identity iceIdentity;

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
        public Ice.Identity IceIdentity
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
            get
            {
                return Manager.getProjectInformation().getProjectIdentifier(StringIceIdentity);
            }
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
        public ModulesManager.OutputPrx Output
        {
            get { return this.manager.getOutputInterface(); }
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
            get { return this.myProxy; }
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
        public override BoxModuleFactoryPrx getMyFactory(Ice.Current __current)
        {
            return this.myFactoryProxy;
        }

        private Ferda.ModulesManager.ManagersEnginePrx manager;
        /// <summary>
        /// Gets the manager.
        /// </summary>
        /// <value>The manager.</value>
        public Ferda.ModulesManager.ManagersEnginePrx Manager
        {
            get { return manager; }
        }

        private Ice.ObjectAdapter adapter;

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
        private Ice.Object functionsIceObj;

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
        private Ice.ObjectPrx functionsObjPrx;

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
        public override Ice.ObjectPrx getFunctions(Ice.Current __current)
        {
            return this.functionsObjPrx;
        }

        /// <summary>
        /// Gets the box module`s functions object`s ice ids.
        /// </summary>
        /// <param name="__current">The Ice.Current.</param>
        /// <returns>
        /// Array of <see cref="T:System.String"/> as ids 
        /// of all provided functions (i.e. implemented intefaces).
        /// </returns>
        public override string[] getFunctionsIceIds(Ice.Current __current)
        {
            return this.functionsObjPrx.ice_ids();
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
            Ice.Identity myIdentity,
            BoxModuleFactoryPrx myFactoryProxy,
            Ferda.ModulesManager.ManagersEnginePrx manager,
            Ice.ObjectAdapter adapter,
            string[] localePrefs)
        {
            System.Diagnostics.Debug.WriteLine("BoxModuleI Constructor (entering): " + boxInfo.Identifier);

            // initializes inner fields by specified parameters
            this.boxInfo = boxInfo;
            this.iceIdentity = myIdentity;
            this.stringIceIdentity = Ice.Util.identityToString(IceIdentity);
            this.myFactoryProxy = myFactoryProxy;
            this.manager = manager;
            this.adapter = adapter;
            this.localePrefs = localePrefs;

            // add the new box module to the specified adapter
            adapter.add(this, this.iceIdentity);
            // get my proxy
            this.myProxy = BoxModulePrxHelper.uncheckedCast(adapter.createProxy(myIdentity));

            // initializes box module`s functions object
            this.boxInfo.CreateFunctions(this, out this.functionsIceObj, out this.functionsIObj);
            this.functionsIObj.setBoxModuleInfo(this, this.boxInfo);
            this.functionsObjPrx = Ice.ObjectPrxHelper.uncheckedCast(adapter.addWithUUID(this.functionsIceObj));

            // initializes properties
            this.properties = new Dictionary<string, PropertyValue>();
            foreach (string propertyName in boxInfo.GetPropertiesNames())
            {
                if (!boxInfo.IsPropertyReadOnly(propertyName))
                {
                    this.setProperty(propertyName, boxInfo.GetPropertyDefaultValue(propertyName));
                }
            }

            // initializes sockets (connections and functions)
            this.connections = new Dictionary<string, Dictionary<string, BoxModulePrx>>();
            this.functions = new Dictionary<string, Dictionary<string, Ice.ObjectPrx>>();
            foreach (string socketName in boxInfo.GetSocketNames())
            {
                connections[socketName] = new Dictionary<string, BoxModulePrx>();
                functions[socketName] = new Dictionary<string, Ice.ObjectPrx>();
            }

            System.Diagnostics.Debug.WriteLine("BoxModuleI Constructor (leaving): " + this.boxInfo.Identifier);
        }

        //TODO DOC Michal
        /// <summary>
        /// Gets the additional sockets.
        /// </summary>
        /// <param name="__current">The Ice.Current.</param>
        /// <returns>
        /// Array of <see cref="T:Ferda.Modules.SocketInfo"/> as
        /// possible additional sockets.
        /// </returns>
        /// <remarks>For lamda-like boxes.</remarks>
        public override SocketInfo[] getAdditionalSockets(Ice.Current __current)
        {
            return new SocketInfo[0];
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
        public override ModulesAskingForCreation[] getModulesAskingForCreation(Ice.Current __current)
        {
            return this.boxInfo.GetModulesAskingForCreation(this.localePrefs, this);
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
        public override SelectString[] getPropertyOptions(string propertyName, Ice.Current __current)
        {
            // tests if there is the specified property exists
            if (!this.boxInfo.TestPropertyNameExistence(propertyName))
                throw Ferda.Modules.Exceptions.NameNotExistError(null, null, "BMI19", propertyName);
            // gets and returns the options
            return this.boxInfo.GetPropertyOptions(propertyName, this);
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
        public override bool isPropertySet(string propertyName, Ice.Current __current)
        {
            // tests property existence
            if (!this.boxInfo.TestPropertyNameExistence(propertyName))
            {
                throw Ferda.Modules.Exceptions.NameNotExistError(null, null, "BMI20", propertyName);
            }
            // tests property value if it is set
            return boxInfo.IsPropertySet(propertyName, this.getProperty(propertyName));
        }

        /// <summary>
        /// Determines whether the specified <c>sockets</c> satisfy 
        /// the condition on required sockets (<c>neededSockets</c>) 
        /// i.e. if there are sockets of the same name and the same type 
        /// (<see cref="F:Ferda.Modules.Serializer.BoxSerializer.NeededSocket.FunctionIceId"/>)
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
        private static bool hasBoxType(BoxType boxType, Ice.ObjectPrx functionsPrx, SocketInfo[] sockets)
        {
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
        public override void setConnection(string socketName, BoxModulePrx otherModule, Ice.Current __current)
        {
            // tests specified socketName existence
            if (!this.boxInfo.TestSocketNameExistence(socketName))
            {
                throw Ferda.Modules.Exceptions.NameNotExistError(null, null, "BMI21", socketName);
            }

            // tests Ferda.Modules.BoxType of otherModule
            bool badTypeError = true;
            Ice.ObjectPrx objPrx = otherModule.getFunctions();
            SocketInfo[] otherModuleSocketInfos = otherModule.getMyFactory().getSockets();
            foreach (BoxType socketBoxType in
                this.boxInfo.GetSocketTypes(socketName))
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
                throw new Ferda.Modules.BadTypeError();
            }

            string identity = Ice.Util.identityToString(otherModule.ice_getIdentity());

            lock (this)
            {
                // the socket is actually property (i.e. value of the property is set by its socket)
                if (boxInfo.TestPropertyNameExistence(socketName))
                {
                    switch (boxInfo.GetPropertyDataType(socketName))
                    {
                        case "::Ferda::Modules::BoolT":
                            propertiesBool[socketName] = BoolTInterfacePrxHelper.checkedCast(objPrx);
                            break;
                        case "::Ferda::Modules::ShortT":
                            propertiesShort[socketName] = ShortTInterfacePrxHelper.checkedCast(objPrx);
                            break;
                        case "::Ferda::Modules::IntT":
                            propertiesInt[socketName] = IntTInterfacePrxHelper.checkedCast(objPrx);
                            break;
                        case "::Ferda::Modules::LongT":
                            propertiesLong[socketName] = LongTInterfacePrxHelper.checkedCast(objPrx);
                            break;
                        case "::Ferda::Modules::FloatT":
                            propertiesFloat[socketName] = FloatTInterfacePrxHelper.checkedCast(objPrx);
                            break;
                        case "::Ferda::Modules::DoubleT":
                            propertiesDouble[socketName] = DoubleTInterfacePrxHelper.checkedCast(objPrx);
                            break;
                        case "::Ferda::Modules::StringT":
                            propertiesString[socketName] = StringTInterfacePrxHelper.checkedCast(objPrx);
                            break;
                        case "::Ferda::Modules::DateTimeT":
                            propertiesDateTime[socketName] = DateTimeTInterfacePrxHelper.checkedCast(objPrx);
                            break;
                        case "::Ferda::Modules::DateT":
                            propertiesDate[socketName] = DateTInterfacePrxHelper.checkedCast(objPrx);
                            break;
                        case "::Ferda::Modules::TimeT":
                            propertiesTime[socketName] = TimeTInterfacePrxHelper.checkedCast(objPrx);
                            break;
                        default:
                            propertiesOther[socketName] = objPrx;
                            break;
                    }
                }
                else
                {
                    // tests if socket (accepting only one connection) is already full
                    if ((!this.boxInfo.IsSocketMoreThanOne(socketName)) &&
                        this.connections[socketName].Count != 0)
                    {
                        // the socket is already used -> exception is thrown
                        Debug.WriteLine("BMI23");
                        throw new ConnectionExistsError();
                    }
                }
                this.connections[socketName][identity] = otherModule;
                this.functions[socketName][identity] = objPrx;
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
        public override void runAction(string actionName, Ice.Current __current)
        {
            lock (connections)
            {
                bool neededSocketsConnected = true;
                foreach (string[] neededSockets in this.boxInfo.GetActionInfoNeededConnectedSockets(actionName))
                {
                    neededSocketsConnected = true;
                    foreach (string neededSocket in neededSockets)
                    {
                        if (!this.connections.ContainsKey(neededSocket)
                            || !(this.connections[neededSocket].Count > 0))
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
                    throw new Ferda.Modules.NeedConnectedSocketError();
                }

                // lock the box module
                this.manager.getBoxModuleLocker().lockBoxModule(StringIceIdentity);

                try
                {
                    this.boxInfo.RunAction(actionName, this);
                    //throws BoxRuntimeError, NameNotExistError
                }
                finally
                {
                    // unlock the box module
                    this.manager.getBoxModuleLocker().unlockBoxModule(StringIceIdentity);
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
        /// specified property data type. See <see cref="T:Ferda.Modules.Serialier.BoxSerializer.Property.TypeClassIceId"/>.
        /// </exception>
        /// <exception cref="T:Ferda.Modules.ReadOnlyError">
        /// Thrown iff specified property is read only.
        /// </exception>
        public override void setProperty(string propertyName, PropertyValue propertyValue, Ice.Current __current)
        {
            if (!this.boxInfo.TestPropertyNameExistence(propertyName))
            {
                // there is no property of the specified propertyName
                throw Ferda.Modules.Exceptions.NameNotExistError(null, null, "BMI25", propertyName);
            }
            if (propertyValue != null && !propertyValue.ice_isA(this.boxInfo.GetPropertyDataType(propertyName)))
            {
                // bad type of the specified propertyValue
                Debug.WriteLine("BMI26");
                throw new Ferda.Modules.BadTypeError();
            }
            if (this.boxInfo.IsPropertyReadOnly(propertyName))
            {
                // the specified property is readonly
                Debug.WriteLine("BMI27");
                throw new Ferda.Modules.ReadOnlyError();
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
                    case "::Ferda::Modules::BoolT":
                        BoolTInterfacePrx oldPrxBool = null;
                        if (propertiesBool.TryGetValue(propertyName, out oldPrxBool))
                            destroyPrxIfSetAsProperty(propertyName, oldPrxBool, this.adapter);
                        propertiesBool[propertyName] = BoolTInterfacePrxHelper.checkedCast(this.adapter.addWithUUID(propertyValue));
                        break;
                    case "::Ferda::Modules::ShortT":
                        PropertyValueRestrictionsHelper.TryIsIntegralPropertyCorrect(
                            this.boxInfo,
                            propertyName,
                            ((ShortT)propertyValue).getShortValue());
                        ShortTInterfacePrx oldPrxShort = null;
                        if (propertiesShort.TryGetValue(propertyName, out oldPrxShort))
                            destroyPrxIfSetAsProperty(propertyName, oldPrxShort, this.adapter);
                        propertiesShort[propertyName] = ShortTInterfacePrxHelper.checkedCast(this.adapter.addWithUUID(propertyValue));
                        break;
                    case "::Ferda::Modules::IntT":
                        PropertyValueRestrictionsHelper.TryIsIntegralPropertyCorrect(
                            this.boxInfo,
                            propertyName,
                            ((IntT)propertyValue).getIntValue());
                        IntTInterfacePrx oldPrxInt = null;
                        if (propertiesInt.TryGetValue(propertyName, out oldPrxInt))
                            destroyPrxIfSetAsProperty(propertyName, oldPrxInt, this.adapter);
                        propertiesInt[propertyName] = IntTInterfacePrxHelper.checkedCast(this.adapter.addWithUUID(propertyValue));
                        break;
                    case "::Ferda::Modules::LongT":
                        PropertyValueRestrictionsHelper.TryIsIntegralPropertyCorrect(
                            this.boxInfo,
                            propertyName,
                            ((LongT)propertyValue).getLongValue());
                        LongTInterfacePrx oldPrxLong = null;
                        if (propertiesLong.TryGetValue(propertyName, out oldPrxLong))
                            destroyPrxIfSetAsProperty(propertyName, oldPrxLong, this.adapter);
                        propertiesLong[propertyName] = LongTInterfacePrxHelper.checkedCast(this.adapter.addWithUUID(propertyValue));
                        break;
                    case "::Ferda::Modules::FloatT":
                        PropertyValueRestrictionsHelper.TryIsFloatingPropertyCorrect(
                            this.boxInfo,
                            propertyName,
                            ((FloatT)propertyValue).getFloatValue());
                        FloatTInterfacePrx oldPrxFloat = null;
                        if (propertiesFloat.TryGetValue(propertyName, out oldPrxFloat))
                            destroyPrxIfSetAsProperty(propertyName, oldPrxFloat, this.adapter);
                        propertiesFloat[propertyName] = FloatTInterfacePrxHelper.checkedCast(this.adapter.addWithUUID(propertyValue));
                        break;
                    case "::Ferda::Modules::DoubleT":
                        PropertyValueRestrictionsHelper.TryIsFloatingPropertyCorrect(
                            this.boxInfo,
                            propertyName,
                            ((DoubleT)propertyValue).getDoubleValue());
                        DoubleTInterfacePrx oldPrxDouble = null;
                        if (propertiesDouble.TryGetValue(propertyName, out oldPrxDouble))
                            destroyPrxIfSetAsProperty(propertyName, oldPrxDouble, this.adapter);
                        propertiesDouble[propertyName] = DoubleTInterfacePrxHelper.checkedCast(this.adapter.addWithUUID(propertyValue));
                        break;
                    case "::Ferda::Modules::StringT":
                        PropertyValueRestrictionsHelper.TryIsStringPropertyCorrect(
                            this.boxInfo,
                            propertyName,
                            ((StringT)propertyValue).getStringValue());
                        StringTInterfacePrx oldPrxString = null;
                        if (propertiesString.TryGetValue(propertyName, out oldPrxString))
                            destroyPrxIfSetAsProperty(propertyName, oldPrxString, this.adapter);
                        propertiesString[propertyName] = StringTInterfacePrxHelper.checkedCast(this.adapter.addWithUUID(propertyValue));
                        break;
                    case "::Ferda::Modules::DateTimeT":
                        PropertyValueRestrictionsHelper.TryIsDateTimePropertyCorrect(
                            this.boxInfo, propertyName, (DateTimeT)propertyValue);
                        DateTimeTInterfacePrx oldPrxDateTime = null;
                        if (propertiesDateTime.TryGetValue(propertyName, out oldPrxDateTime))
                            destroyPrxIfSetAsProperty(propertyName, oldPrxDateTime, this.adapter);
                        propertiesDateTime[propertyName] = DateTimeTInterfacePrxHelper.checkedCast(this.adapter.addWithUUID(propertyValue));
                        break;
                    case "::Ferda::Modules::DateT":
                        DateT date = new DateTI();
                        ((DateT)propertyValue).getDateValue(out date.year, out date.month, out date.day);
                        PropertyValueRestrictionsHelper.TryIsDatePropertyCorrect(
                            this.boxInfo, propertyName, date);
                        DateTInterfacePrx oldPrxDate = null;
                        if (propertiesDate.TryGetValue(propertyName, out oldPrxDate))
                            destroyPrxIfSetAsProperty(propertyName, oldPrxDate, this.adapter);
                        propertiesDate[propertyName] = DateTInterfacePrxHelper.checkedCast(this.adapter.addWithUUID(propertyValue));
                        break;
                    case "::Ferda::Modules::TimeT":
                        TimeT time = new TimeTI();
                        ((TimeT)propertyValue).getTimeValue(out time.hour, out time.minute, out time.second);
                        PropertyValueRestrictionsHelper.TryIsTimePropertyCorrect(
                            this.boxInfo, propertyName, time);
                        TimeTInterfacePrx oldPrxTime = null;
                        if (propertiesTime.TryGetValue(propertyName, out oldPrxTime))
                            destroyPrxIfSetAsProperty(propertyName, oldPrxTime, this.adapter);
                        propertiesTime[propertyName] = TimeTInterfacePrxHelper.checkedCast(this.adapter.addWithUUID(propertyValue));
                        break;
                    default:
                        Ice.ObjectPrx oldPrxOtherType = null;
                        if (propertiesOther.TryGetValue(propertyName, out oldPrxOtherType))
                            destroyPrxIfSetAsProperty(propertyName, oldPrxOtherType, this.adapter);
                        propertiesOther[propertyName] = this.adapter.addWithUUID(propertyValue);
                        break;
                }
                // proxy of new propertyValue is already saved
                // save the propertyValue
                this.properties[propertyName] = propertyValue;
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
            if (!this.boxInfo.TestPropertyNameExistence(propertyName))
                throw Ferda.Modules.Exceptions.NameNotExistError(null, null, "BMI28", propertyName);
            if (!this.boxInfo.IsPropertyReadOnly(propertyName))
            {
                string message = "BMI01: Property " + propertyName + " is not readonly as expected";
                Debug.WriteLine(message);
                throw new Exception(message);
            }
            return this.boxInfo.GetReadOnlyPropertyValue(propertyName, this);
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
        public override PropertyValue getProperty(string propertyName, Ice.Current __current)
        {
            // tests if property of specified name exists
            if (!this.boxInfo.TestPropertyNameExistence(propertyName))
            {
                throw Ferda.Modules.Exceptions.NameNotExistError(null, null, "DBI29", propertyName);
            }

            // readonly properties
            if (this.boxInfo.IsPropertyReadOnly(propertyName))
            {
                PropertyValue result = this.getReadOnlyPropertyValue(propertyName);
                if (result != null)
                    return result;
                else // returns "empty" PropertyValue
                {
                    switch (boxInfo.GetPropertyDataType(propertyName))
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
                            string message = "BMI02";
                            Debug.WriteLine(message);
                            throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(boxInfo.GetPropertyDataType(propertyName), message);
                    }
                }
            }
            if (properties.ContainsKey(propertyName))
            {
                lock (properties)
                {
                    return properties[propertyName];
                }
            }
            switch (boxInfo.GetPropertyDataType(propertyName))
            {
                case "::Ferda::Modules::BoolT":
                    lock (propertiesBool)
                    {
                        return new BoolTI(propertiesBool[propertyName]);
                    }
                case "::Ferda::Modules::ShortT":
                    lock (propertiesShort)
                    {
                        return new ShortTI(propertiesShort[propertyName]);
                    }
                case "::Ferda::Modules::IntT":
                    lock (propertiesInt)
                    {
                        return new IntTI(propertiesInt[propertyName]);
                    }
                case "::Ferda::Modules::LongT":
                    lock (propertiesLong)
                    {
                        return new LongTI(propertiesLong[propertyName]);
                    }
                case "::Ferda::Modules::FloatT":
                    lock (propertiesFloat)
                    {
                        return new FloatTI(propertiesFloat[propertyName]);
                    }
                case "::Ferda::Modules::DoubleT":
                    lock (propertiesDouble)
                    {
                        return new DoubleTI(propertiesDouble[propertyName]);
                    }
                case "::Ferda::Modules::StringT":
                    lock (propertiesString)
                    {
                        return new StringTI(propertiesString[propertyName]);
                    }
                case "::Ferda::Modules::DateTimeT":
                    lock (propertiesDateTime)
                    {
                        return new DateTimeTI(propertiesDateTime[propertyName]);
                    }
                case "::Ferda::Modules::DateT":
                    lock (propertiesDate)
                    {
                        return new DateTI(propertiesDate[propertyName]);
                    }
                case "::Ferda::Modules::TimeT":
                    lock (propertiesTime)
                    {
                        return new TimeTI(propertiesTime[propertyName]);
                    }
                default:
                    return this.GetPropertyOther(propertyName);
            }
        }

        /// <summary>
        /// Gets items of the dynamic help.
        /// </summary>
        /// <param name="__current">The Ice.Current.</param>
        /// <returns>
        /// Array of <seealso cref="T:Ferda.Modules.DynamicHelpItem">DynamicHelpItems</seealso>.
        /// </returns>
        public override DynamicHelpItem[] getDynamicHelpItems(Ice.Current __current)
        {
            return this.boxInfo.GetDynamicHelpItems(this.localePrefs, this);
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
        public override string[] getDefaultUserLabel(Ice.Current __current)
        {
            string result = this.boxInfo.GetDefaultUserLabel(this);
            if (String.IsNullOrEmpty(result))
                return new string[0];
            else
                return new string[1] { result };
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
            isLocalized = this.boxInfo.TryGetPhrase(phraseIdentifier, out result, this.localePrefs);
            return result;
        }

        /// <summary>
        /// Outputs the message.
        /// </summary>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="messageTitle">The message title (phrase identifier).</param>
        /// <param name="messageText">The message text (phrase identifier).</param>
        public void OutputMessage(Ferda.ModulesManager.MsgType messageType, string messageTitle, string messageText)
        {
            bool dummy;
            this.Output.writeMsg(
                messageType,
                GetPhrase(messageTitle, out dummy),
                GetPhrase(messageText, out dummy)
                );
        }

        #region Destroying

        /// <summary>
        /// Removes the specified propertyValue object`s proxy (<c>prx</c>) of 
        /// specified property (<c>propertyName</c>) from the specified <c>adapter</c>.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="prx">The object`s proxy.</param>
        /// <param name="adapter">The adapter.</param>
        private void destroyPrxIfSetAsProperty(string propertyName, Ice.ObjectPrx prx, Ice.ObjectAdapter adapter)
        {
            if (properties.ContainsKey(propertyName))
                adapter.remove(prx.ice_getIdentity());
        }

        /// <summary>
        /// Destroys all Ice objects inherent to the box module 
        /// (e.g. <see cref="T:Ferda.Modules.PropertyValue">property values</see>)
        /// i.e. remove their proxies from the specified <c>adapter</c>.
        /// </summary>
        /// <param name="adapter">The adapter.</param>
        public void destroy(Ice.ObjectAdapter adapter)
        {
            foreach (KeyValuePair<string, BoolTInterfacePrx> pair in propertiesBool)
                destroyPrxIfSetAsProperty(pair.Key, pair.Value, adapter);
            foreach (KeyValuePair<string, ShortTInterfacePrx> pair in propertiesShort)
                destroyPrxIfSetAsProperty(pair.Key, pair.Value, adapter);
            foreach (KeyValuePair<string, IntTInterfacePrx> pair in propertiesInt)
                destroyPrxIfSetAsProperty(pair.Key, pair.Value, adapter);
            foreach (KeyValuePair<string, LongTInterfacePrx> pair in propertiesLong)
                destroyPrxIfSetAsProperty(pair.Key, pair.Value, adapter);
            foreach (KeyValuePair<string, FloatTInterfacePrx> pair in propertiesFloat)
                destroyPrxIfSetAsProperty(pair.Key, pair.Value, adapter);
            foreach (KeyValuePair<string, DoubleTInterfacePrx> pair in propertiesDouble)
                destroyPrxIfSetAsProperty(pair.Key, pair.Value, adapter);
            foreach (KeyValuePair<string, StringTInterfacePrx> pair in propertiesString)
                destroyPrxIfSetAsProperty(pair.Key, pair.Value, adapter);
            foreach (KeyValuePair<string, DateTimeTInterfacePrx> pair in propertiesDateTime)
                destroyPrxIfSetAsProperty(pair.Key, pair.Value, adapter);
            foreach (KeyValuePair<string, DateTInterfacePrx> pair in propertiesDate)
                destroyPrxIfSetAsProperty(pair.Key, pair.Value, adapter);
            foreach (KeyValuePair<string, TimeTInterfacePrx> pair in propertiesTime)
                destroyPrxIfSetAsProperty(pair.Key, pair.Value, adapter);
            foreach (KeyValuePair<string, Ice.ObjectPrx> pair in propertiesOther)
                destroyPrxIfSetAsProperty(pair.Key, pair.Value, adapter);
        }

        #endregion
    }
}
