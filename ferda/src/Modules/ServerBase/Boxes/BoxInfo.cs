// BoxInfo.cs - Abstract class implementing the IBoxInfo interface
//
// Author: Tomáš Kuchař <tomas.kuchar@gmail.com>
//
// Copyright (c) 2006 Tomáš Kuchař
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
using System.Globalization;
using System.IO;
using System.Text;
using Ferda.Modules.Boxes.Serializer;
using Ferda.Modules.Boxes.Serializer.Configuration;
using Ferda.Modules.Boxes.Serializer.Localization;
using Ice;
using Action=Ferda.Modules.Boxes.Serializer.Configuration.Action;
using Exception=System.Exception;
using Helper=Ferda.Modules.Boxes.Serializer.Configuration.Helper;
using IHelper=Ferda.Modules.Boxes.Serializer.Configuration.IHelper;
using Object=Ice.Object;
using SelectOption=Ferda.Modules.Boxes.Serializer.Configuration.SelectOption;
using Socket=Ferda.Modules.Boxes.Serializer.Configuration.Socket;

namespace Ferda.Modules.Boxes
{
    /// <summary>
    /// <para>
    /// This is the basic implementation of
    /// <see cref="T:Ferda.Modules.Boxes.IBoxInfo"/>.
    /// </para>
    /// <para>
    /// Lot of provided features are stored in configuration XML
    /// files. There are two kinds of configuration files. The
    /// <see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Box">first one</see>
    /// contains settings independent on localization. The
    /// <see cref="T:Ferda.Modules.Boxes.Serializer.Localization.BoxLocalization">second one</see>
    /// contains localization of Box module
    /// </para>
    /// <para>
    /// Thanks to the configuration and localization files, most of
    /// <see cref="T:Ferda.Modules.Boxes.IBoxInfo">IBoxInfo`s</see>
    /// functions are implemeted directly but some functions are still
    /// abstract and some are virtual.
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Box">
    /// first type of configuration XML file</see> is
    /// <see cref="T:Ferda.Modules.Boxes.Serializer.Reader">deserealized</see>
    /// to <see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Helper">
    /// more effective representation</see> and cached to
    /// <see cref="F:Ferda.Modules.Boxes.BoxInfo.box">member field</see>
    /// alredy in the constructor.
    /// </para>
    /// <para>
    /// The <see cref="T:Ferda.Modules.Boxes.Serializer.Localization.BoxLocalization">
    /// second type of configuration XML file(s) (i.e. localization)</see> is(are)
    /// <see cref="T:Ferda.Modules.Boxes.Serializer.Reader">deserealized</see>
    /// to <see cref="T:Ferda.Modules.Boxes.Serializer.Localization.Helper">
    /// more effective representation</see> and cached to
    /// <see cref="F:Ferda.Modules.Boxes.BoxInfo.boxLocalizations">member field</see>
    /// when the specified <c>localePrefs</c> is required.
    /// </para>
    /// </remarks>
    public abstract class BoxInfo : IBoxInfo
    {
        #region Config files directory

        /// <summary>
        /// Name of application subdirectory, were are
        /// configuration files stored (in next subdirectories).
        /// </summary>
        /// <seealso cref="P:Ferda.Modules.Boxes.BoxInfo._configFilesDirectoryPath"/>
        private const string configFilesFolderName = "BoxModulesServices";

        private string _configFilesDirectoryPath = String.Empty;

        /// <summary>
        /// Default directory for config files is in ApplicationDomainDirectory +
        /// <see cref="P:Ferda.Modules.Boxes.BoxInfo.configFilesFolderName"/> +
        /// name of directory of current box i.e.
        /// <see cref="P:Ferda.Modules.Boxes.BoxInfo.identifier"/> when you replace
        /// pluses "+" by <see cref="F:System.IO.Path.DirectorySeparatorChar"/>.
        /// </summary>
        public string ConfigFilesDirectoryPath
        {
            get
            {
                if (String.IsNullOrEmpty(_configFilesDirectoryPath))
                {
                    return Path.Combine(
                        configFilesFolderName,
                        identifier.Replace(
                            '.',
                            Path.DirectorySeparatorChar)
                        );
                }
                else
                {
                    return _configFilesDirectoryPath;
                }
            }
        }

        #endregion

        #region Config files

        /// <summary>
        /// Name of box configuration xml file. (This file is independent on localization.)
        /// </summary>
        protected const string boxCofigFileName = "BoxConfig.xml";

        #region Localization config files

        /// <summary>
        /// <see cref="M:getLocalizationFileName(System.String)"/>
        /// </summary>
        private const string localizationFileNamePrefix = "Localization";

        /// <summary>
        /// <see cref="M:getLocalizationFileName(System.String)"/>
        /// </summary>
        private const string localizationFileNamePostfix = "xml";

        /// <summary>
        /// Allows to obtain name of localization config file.
        /// E.g. Localization.en-US.xml where "Localization" invariable is prefix,
        /// "xml" is invariable posfix and "en-US" is varying localizeId.
        /// </summary>
        /// <param name="localeId">
        /// Identification of localization i.e. something like "en-US" or
        /// "cs-CZ" ... or empty string for default localization ("") or
        /// <see cref="F:Ferda.Modules.Boxes.BoxInfo.defaultLanguageId"/>.
        /// </param>
        /// <returns>String representing name of localization config file.</returns>
        /// <seealso cref="T:Ferda.Modules.Boxes.IBoxInfo">
        /// See some chapter about localization preferences i.e. culture names.
        /// </seealso>
        protected string getLocalizationFileName(string localeId)
        {
            if (String.IsNullOrEmpty(localeId))
                localeId = defaultLanguageId;
            return localizationFileNamePrefix + "." + localeId + "." + localizationFileNamePostfix;
        }

        #endregion

        #endregion

        #region Config files cache

        /// <summary>
        /// Cache for <see cref="T:Ferda.Modules.Boxes.Serializer.Reader">deserealized</see>
        /// <see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Box">config file</see>
        /// (independent on localization).
        /// </summary>
        protected IHelper box;

        /// <summary>
        /// Identifier of default localization.
        /// </summary>
        /// <remarks>Used as key in <see cref="F:Ferda.Modules.Boxes.BoxInfo.boxLocalizations"/>.</remarks>
        protected const string defaultLanguageId = "en-US";

        /// <summary>
        /// Cache for <see cref="T:Ferda.Modules.Boxes.Serializer.Reader">deserealized</see>
        /// <see cref="T:Ferda.Modules.Boxes.Serializer.Localization.BoxLocalization">localization config file</see>.
        /// </summary>
        /// <remarks>
        /// <para>Key: localeId (something like: en-US, cs-CZ, ..., or <see cref="F:Ferda.Modules.Boxes.BoxInfo.defaultLanguageId"/>)</para>
        /// <para>Value: <see cref="T:Ferda.Modules.Boxes.Serializer.Localization.Helper">deserealized localization config file</see>.</para>
        /// </remarks>
        protected Dictionary<string, Serializer.Localization.IHelper> boxLocalizations =
            new Dictionary<string, Serializer.Localization.IHelper>();

        #endregion

        #region Constructors

        /// <summary>
        /// <para>Default constructor.</para>
        /// <para>Loads box config file (independent on localization)
        /// i.e. stores box config file deserealization to
        /// <see cref="F:Ferda.Modules.Boxes.BoxInfo.box">cache</see>.
        /// </para>
        /// <para>Config files are loaded from <see cref="F:Ferda.Modules.Boxes.BoxInfo._configFilesDirectoryPath"/>.</para>
        /// </summary>
        /// <remarks>
        /// Localization config files are loaded (deserealized and stored in
        /// cache) as needed (lazy loading).
        /// </remarks>
        /// <seealso cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Helper"/>
        /// <exception cref="T:System.Exception">Thrown iff getting box config faild.</exception>
        protected BoxInfo()
        {
            //Deserealize and store box config file
            box = new Helper(
                Reader.ReadBox(
                    Path.Combine(ConfigFilesDirectoryPath, boxCofigFileName)
                    )
                );

            //Deserealization has to be sucessful
            if (box == null)
            {
                string message = "BoxInf01: Unable to get config for " + identifier;
                Debug.WriteLine(message);
                throw new Exception(message);
            }
        }

        /// <summary>
        /// 	<para>Default constructor.</para>
        /// 	<para>Loads box config file (independent on localization)
        /// i.e. stores box config file deserealization to
        /// <see cref="F:Ferda.Modules.Boxes.BoxInfo.box">cache</see>.
        /// </para>
        /// 	<para>Config files are loaded from specified <c>pathToConfigFiles</c>.</para>
        /// </summary>
        /// <param name="pathToConfigFiles">The path to config files.</param>
        /// <remarks>
        /// Localization config files are loaded (deserealized and stored in
        /// cache) as needed (lazy loading).
        /// </remarks>
        /// <seealso cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Helper"/>
        /// <exception cref="T:System.Exception">Thrown iff getting box config faild.</exception>
        public BoxInfo(string pathToConfigFiles)
        {
            _configFilesDirectoryPath = pathToConfigFiles;
            //Deserealize and store box config file
            box = new Helper(
                Reader.ReadBox(
                    Path.Combine(ConfigFilesDirectoryPath, boxCofigFileName)
                    )
                );

            //Deserealization has to be sucessful
            if (box == null)
            {
                string message = "BoxInf01: Unable to get config for " + identifier;
                Debug.WriteLine(message);
                throw new Exception(message);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoxInfo"/> class.
        /// </summary>
        /// <param name="configurationHelper">The configuration helper.</param>
        /// <remarks>
        /// If this constructor is used no configuration file is loaded
        /// and localization config files are loaded (deserealized and stored in
        /// cache) as needed (lazy loading).
        /// </remarks>
        public BoxInfo(IHelper configurationHelper)
        {
            if (configurationHelper == null)
            {
                string message = "BoxInf17: Unable to get config for " + identifier;
                Debug.WriteLine(message);
                throw new ArgumentNullException(message);
            }
            box = configurationHelper;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoxInfo"/> class.
        /// </summary>
        /// <param name="configurationHelper">The configuration helper.</param>
        /// <param name="defaultLocalization">The default localization.</param>
        /// <param name="localizations">The localizations.</param>
        /// <remarks>
        /// If this constructor is used no configuration file is loaded
        /// and localization config files are loaded (deserealized and stored in
        /// cache) as needed (lazy loading) but only if specified
        /// <c>localizations</c> does not satisfy some localePrefs.
        /// </remarks>
        public BoxInfo(
            IHelper configurationHelper,
            Serializer.Localization.IHelper defaultLocalization,
            Serializer.Localization.IHelper[] localizations
            )
        {
            if (configurationHelper == null)
            {
                string message = "BoxInf18: Unable to get config for " + identifier;
                Debug.WriteLine(message);
                throw new ArgumentNullException(message);
            }
            box = configurationHelper;

            if (defaultLocalization == null)
            {
                string message = "BoxInf19: Unable to get default localization for " + identifier;
                Debug.WriteLine(message);
                throw new ArgumentNullException(message);
            }
            boxLocalizations.Add(defaultLanguageId, defaultLocalization);

            if (localizations.Length > 0)
            {
                foreach (Serializer.Localization.IHelper localization in localizations)
                {
                    boxLocalizations.Add(localization.LocaleId, localization);
                }
            }
        }

        #endregion

        #region Localization files

        /// <summary>
        /// Gets <see cref="T:Ferda.Modules.Boxes.Serializer.Localization.IHelper">localization</see>.
        /// according to specified <c>localePrefs</c>.
        /// </summary>
        /// <param name="localePrefs">The localization preferences.</param>
        /// <returns>Localization</returns>
        /// <exception cref="T:System.Exception">Thrown iff no localization
        /// file corresponding to <c>localePrefs</c> was found.</exception>
        /// <remarks>
        /// Specified <c>localePrefs</c> are processed by following scenario:
        /// if first localePref is already loaded then it is returned,
        /// else if it is not loaded, than the function tries to load localization file;
        /// otherwise, next localePref is processed.
        /// If no localePref can be used (no ones is loaded no one can be loaded) than
        /// default localePref is used.
        /// </remarks>
        private Serializer.Localization.IHelper getLocalization(string[] localePrefs)
        {
            //I need to try use most forward localeId from localePrefs,
            //If no localeId can be used I have to use default locale (defaultLanguageId)

            //Prepare sequence of locale Ids (add defaultLanguageId at last position)
            List<string> localePrefsWithDefault = new List<string>();
            if (localePrefs != null)
            {
                foreach (string locale in localePrefs)
                    localePrefsWithDefault.Add(locale);
            }
            localePrefsWithDefault.Add(defaultLanguageId);

            //Go over sequence of locale Ids until any localization is useful
            foreach (string localeId in localePrefsWithDefault)
            {
                //Localization exists and is useful
                if (boxLocalizations.ContainsKey(localeId) && boxLocalizations[localeId] != null)
                    return boxLocalizations[localeId];

                    //Localization does not exist (try another)
                else if (boxLocalizations.ContainsKey(localeId))
                {
                    continue;
                }

                    //I don`t know if Localization exists (try to load file)
                else
                {
                    BoxLocalization boxLoc =
                        Reader.ReadBoxLocalization(
                            Path.Combine(
                                ConfigFilesDirectoryPath,
                                getLocalizationFileName(localeId)
                                )
                            );
                    if (boxLoc == null)
                    {
                        //Localization does not exist
                        boxLocalizations.Add(localeId, null);
                        continue;
                    }
                    else
                    {
                        //Localization exists
                        boxLocalizations.Add(
                            localeId,
                            new Serializer.Localization.Helper(boxLoc, localeId)
                            );
                        return boxLocalizations[localeId];
                    }
                }
            }
            // localizatino file not found -> throw an exception
            StringBuilder message = new StringBuilder();
            foreach (string localePref in localePrefs)
                message.Append(localePref + ",");
            throw new Exception("BoxInf02: Localization file not found: " + Identifier + "{" + message.ToString() + "}");
        }

        #endregion

        #region Abstract and Virtual members

        /// <summary>
        /// Unique identifier of type of Box module
        /// </summary>
        /// <remarks>
        /// This string identifier is parsed i.e. dots (".") are
        /// replaced by <see cref="P:System.IO.Path.DirectorySeparatorChar"/>.
        /// Returned path is combined with application directory`s
        /// <see cref="F:Ferda.Modules.Boxes.BoxInfo.configFilesFolderName">subdirectory</see>
        /// and final path is used for getting stored configuration [localization] XML files.
        /// </remarks>
        protected abstract string identifier { get; }

        /// <summary>
        /// Creates <see cref="T:Ice.Object"/> implementing Ice interface of
        /// the box module i.e. box`s functions declared in slice design.
        /// </summary>
        /// <param name="boxModule">Box module, to which created functions
        /// will belong to.</param>
        /// <param name="iceObject">An out parameter returning <see cref="T:Ice.Object"/>
        /// implementing box`s "ice" functions. This value is same as value
        /// of <c>functions</c>.</param>
        /// <param name="functions">An out parameter returning <see cref="T:Ice.Object"/>
        /// implementing box`s "ice" functions. This value is same as value
        /// of <c>iceObject</c>.</param>
        /// <example>
        /// Please see an example for <see cref="T:Ferda.Modules.Boxes.IBoxInfo">IBoxInfo`s</see> method <code>CreateFunctions(...)</code>.
        /// </example>
        /// <remarks>
        /// Each instance of the box module has its own functions object but
        /// class implementing <see cref="T:Ferda.Modules.Boxes.IBoxInfo">
        /// this interface</see> is shared by all instances of the box modules
        /// of the same type <see cref="P:Ferda.Modules.Boxes.IBoxInfo.Identifier"/>
        /// </remarks>
        public abstract void CreateFunctions(BoxModuleI boxModule, out Object iceObject, out IFunctions functions);

        /// <summary>
        /// Gets function`s Ice identifiers of the box module.
        /// </summary>
        /// <returns>
        /// An array of strings representing Ice identifiers
        /// of the box module`s functions.
        /// </returns>
        /// <example>
        /// Please see an example for <see cref="T:Ferda.Modules.Boxes.IBoxInfo">IBoxInfo`s</see>
        /// <see cref="M:Ferda.Modules.Boxes.IBoxInfo.GetBoxModuleFunctionsIceIds()"/>.
        /// </example>
        public abstract string[] GetBoxModuleFunctionsIceIds();
        
        /// <summary>
        /// Gets function`s Ice identifiers of the box module.
        /// </summary>
        /// <param name="boxModule">The box module.</param>
        /// <returns>
        /// An array of strings representing Ice identifiers
        /// of the box module`s functions.
        /// </returns>
        /// <see cref="M:Ferda.Modules.Boxes.IBoxInfo.GetBoxModuleFunctionsIceIds()"/>
        public string[] GetBoxModuleFunctionsIceIds(BoxModule boxModule)
        {
        	return GetBoxModuleFunctionsIceIds();
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
        /// <param name="boxModule">The box module.</param>
        /// <returns>
        /// The <see cref="Ice.ObjectPrx">proxy </see> of the box module`s
        /// functions object.
        /// </returns>
        public virtual ObjectPrx GetFunctionsObjPrx(BoxModuleI boxModule)
        {
        	return boxModule.FunctionsObjPrx;
        }

        /// <summary>
        /// <para>Gets ice ids of functions object.</para>
        /// <para>
        /// Throught lambda abstraction is BoxModule interface separated from
        /// functions over this BoxModule. Functions object is module containing
        /// functions over properties and sockets of this BoxModule specified in
        /// slice design. This method gets its function ice ids
        /// </para>
        /// </summary>
        /// <param name="boxModule">The box module.</param>
        /// <returns>
        /// The array of the ice ids of the box module`s
        /// functions object.
        /// </returns>
        public virtual string[] GetFunctionsIceIds(BoxModuleI boxModule)
        {
            ObjectPrx functionsObjProxy = GetFunctionsObjPrx(boxModule);
            return (functionsObjProxy == null) ? new string[0] : functionsObjProxy.ice_ids();
        }

        /// <summary>
        /// Gets array of <see cref="T:Ferda.Modules.SelectString"/> as
        /// options for property, whose options are dynamically variable.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="boxModule">The box module.</param>
        /// <returns>
        /// An array of <see cref="T:Ferda.Modules.SelectString"/>
        /// as list of options for property named <c>propertyName</c>.
        /// </returns>
        /// <example>
        /// Please see an example for <see cref="T:Ferda.Modules.Boxes.IBoxInfo">IBoxInfo`s</see>
        /// <see cref="M:Ferda.Modules.Boxes.IBoxInfo.GetPropertyOptions(System.String,Ferda.Modules.BoxModuleI)"/>.
        /// </example>
        /// <remarks>
        /// This function doesn`t make any test iff the property of
        /// specified name <c>propertyName</c> exists.
        /// </remarks>
        public abstract SelectString[] GetPropertyOptions(string propertyName, BoxModuleI boxModule);

        /// <summary>
        /// <para>
        /// Gets <see cref="T:Ferda.Modules.PropertyValue"/> from
        /// <see cref="T:Ice.ObjectPrx">objectPrx</see> parameter.
        /// </para>
        /// <para>
        /// Useful for "OtherT" types of property. This will return
        /// instance of class implementing appropriate interface of
        /// <see cref="T:Ferda.Modules.PropertyValue"/> or throws
        /// <see cref="T:Ferda.Modules.NameNotExistError"/> iff there
        /// is no property named as <c>propertyName</c>.
        /// </para>
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="objectPrx"><see cref="T:Ice.ObjectPrx"/> covering
        /// <see cref="T:Ferda.Modules.PropertyValue"/> of the property.</param>
        /// <returns>
        /// The <see cref="T:Ferda.Modules.PropertyValue"/>.
        /// </returns>
        /// <example>
        /// Please see an example for <see cref="T:Ferda.Modules.Boxes.IBoxInfo">IBoxInfo`s</see>
        /// <see cref="M:Ferda.Modules.Boxes.IBoxInfo.GetPropertyObjectFromInterface(System.String,Ice.ObjectPrx)"/>.
        /// </example>
        /// <exception cref="T:Ferda.Modules.NameNotExistError">Iff
        /// there is no property named <c>propertyName</c></exception>
        public virtual PropertyValue GetPropertyObjectFromInterface(string propertyName, ObjectPrx objectPrx)
        {
            if (TestPropertyNameExistence(propertyName))
                return null;
            Debug.Assert(false);
            throw Exceptions.NameNotExistError(null, propertyName);
        }

        /// <summary>
        /// Gets the box modules asking for creation.
        /// </summary>
        /// <param name="localePrefs">The localization preferences.</param>
        /// <param name="boxModule">The box module.</param>
        /// <returns>
        /// Array of <see cref="T:Ferda.Modules.ModuleAskingForCreation">
        /// Modules Asking For Creation</see>.
        /// </returns>
        /// <remarks>
        /// Modules asking for creation dynamically depends on actual
        /// inner state of the box module.
        /// </remarks>
        public abstract ModulesAskingForCreation[] GetModulesAskingForCreation(string[] localePrefs,
                                                                               BoxModuleI boxModule);

        /// <summary>
        /// Gets value of readonly property. Readonly properties may not be
        /// modified by user. Basically values of readonly properties depends on
        /// actual inner state of Box module
        /// </summary>
        /// <param name="propertyName">Name of readonly property.</param>
        /// <param name="boxModule">Box module.</param>
        /// <returns>
        /// A <see cref="T:Ferda.Modules.PropertyValue"/> of
        /// readonly property named <c>propertyName</c>.
        /// </returns>
        /// <remarks>
        /// This function doesn`t make any test if specified property
        /// (by <c>propertyName</c>) really exists and is really readonly.
        /// </remarks>
        public virtual PropertyValue GetReadOnlyPropertyValue(string propertyName, BoxModuleI boxModule)
        {
            string message = "BoxInf04: GetReadOnlyPropertyValue(...) is not implemented in box module: " + identifier +
                             " for property named " + propertyName + ".";
            Debug.WriteLine(message);
            throw new Exception(message);
        }

        /// <summary>
        /// Useful for property of "OtherT" type of the property.
        /// Returns true iff the property specified by <c>propertyName</c> is set.
        /// This information is useful for neededProperty test.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="propertyValue">Value of the property.</param>
        /// <returns>
        /// True iff property value is purposeful/entered.
        /// </returns>
        public virtual bool IsPropertySet(string propertyName, PropertyValue propertyValue)
        {
            return true;
        }

        /// <summary>
        /// Gets items of the dynamic help.
        /// </summary>
        /// <param name="localePrefs">The localization preferences.</param>
        /// <param name="boxModule">The box module.</param>
        /// <returns>
        /// Array of <seealso cref="T:Ferda.Modules.DynamicHelpItem">DynamicHelpItems</seealso>.
        /// </returns>
        public virtual DynamicHelpItem[] GetDynamicHelpItems(string[] localePrefs, BoxModuleI boxModule)
        {
            Serializer.Localization.IHelper boxLocalization = getLocalization(localePrefs);
            return boxLocalization.DynamicHelpItems;
        }

        /// <summary>
        /// Executes (runs) action specified by <c>actionName</c>.
        /// </summary>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="boxModule">The Box module.</param>
        /// <exception cref="T:Ferda.Modules.NameNotExistError">Thrown if action named <c>actionName</c> doesn`t exist.</exception>
        /// <exception cref="T:Ferda.Modules.BoxRuntimeError">Thrown if any runtime error occured while executing the action.</exception>
        public virtual void RunAction(string actionName, BoxModuleI boxModule)
        {
            if (!box.Actions.ContainsKey(actionName))
                throw Exceptions.NameNotExistError(null, actionName);
            Debug.Assert(false);
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets default value for box module`s user label. This label
        /// should predicate box module`s settings (values of its properties)
        /// for easier working with many boxes on user desktop (in FrontEnd).
        /// </summary>
        /// <param name="boxModule">Box Module.</param>
        /// <returns>
        /// String representing default user label of the box module.
        /// This label can change in time ... it can reflect inner
        /// box module`s state e.g. up-to-date values of its properties.
        /// </returns>
        /// <remarks>
        /// Please don`t foreget that localization preferences are specified by
        /// <see cref="P:Ferda.Modules.BoxModuleI.LocalePrefs"/>.
        /// </remarks>
        public abstract string GetDefaultUserLabel(BoxModuleI boxModule);
		
		public virtual PropertyInfo[] GetAdditionalProperties(string[] localePrefs, BoxModuleI boxModule)
		{
			return new PropertyInfo[0];
		}
		
		public virtual SocketInfo[] GetAdditionalSockets(string[] localePrefs, BoxModuleI boxModule)
		{
			return new SocketInfo[0];
		}
		
		public virtual StringCollection GetAdditionalSocketsNames(BoxModuleI boxModule)
		{
			return new StringCollection();
		}

        #endregion

        #region HelpFiles

        /// <summary>
        /// Gets information about the help files.
        /// </summary>
        /// <param name="localePrefs">The localization preferences.</param>
        /// <returns>
        /// Array of <seealso cref="T:Ferda.Modules.HelpFileInfo">HelpFileInfos</seealso>.
        /// </returns>
        public HelpFileInfo[] GetHelpFileInfoSeq(string[] localePrefs)
        {
            Serializer.Localization.IHelper boxLocalization = getLocalization(localePrefs);
            return boxLocalization.HelpFiles;
        }

        /// <summary>
        /// Gets help file as aray of <see cref="T:System.Byte">Bytes</see>.
        /// </summary>
        /// <param name="identifier">The identifier of the help file.</param>
        /// <returns>
        /// Content of the help file as array of <see cref="T:System.Byte">Bytes</see>.
        /// </returns>
        public byte[] GetHelpFile(string identifier)
        {
            string helpFilePath;
            foreach (Serializer.Localization.IHelper boxLocalization in boxLocalizations.Values)
            {
                if (boxLocalization.HelpFilesPaths.TryGetValue(identifier, out helpFilePath))
                    return BoxInfoHelper.TryGetBinaryFile(ConfigFilesDirectoryPath, helpFilePath, false);
            }
            return null;
        }

        #endregion

        #region Sockets

        /// <summary>
        /// Gets the sockets of the box module.
        /// </summary>
        /// <param name="localePrefs">The localization preferences.</param>
        /// <returns>
        /// Array of <seealso cref="T:Ferda.Modules.SocketInfo">SocketInfos</seealso>.
        /// </returns>
        public SocketInfo[] GetSockets(string[] localePrefs)
        {
            Serializer.Localization.IHelper boxLocalization = getLocalization(localePrefs);
            List<SocketInfo> result = new List<SocketInfo>();
            foreach (Socket socket in box.Sockets.Values)
            {
                //gets localized part of result
                Serializer.Localization.Socket localizedSocket = null;
                if (!boxLocalization.Sockets.TryGetValue(socket.Name, out localizedSocket))
                {
                    //socket isn`t localized so if corresponding property (if exists) is visible throw exception!
                    Property property;
                    if (box.Properties.TryGetValue(socket.Name, out property) && property.Visible)
                        //this will throw Ferda.Modules.NameNotExistError exception
                        boxLocalization.GetSocket(socket.Name);
                }

                SocketInfo resultItem = new SocketInfo(
                    socket.Name,
                    (localizedSocket == null) ? "" : localizedSocket.Label,
                    (localizedSocket == null) ? "" : localizedSocket.Hint,
                    BoxInfoHelper.TryGetStringFile(ConfigFilesDirectoryPath, socket.DesignPath, false),
                    box.GetSocketTypes(socket.Name),
                    socket.SettingProperties,
                    socket.MoreThanOne
                    );
                result.Add(resultItem);
            }
            return result.ToArray();
        }

        /// <summary>
        /// Gets array of <see cref="T:Ferda.Modules.BoxType"/> as list
        /// of types of boxes, which can be connected to <c>socketName</c>.
        /// </summary>
        /// <param name="socketName">Unique name (identifer) of socket.</param>
        /// <param name="boxModule">Box module.</param>
        /// <returns>
        /// Returns types of boxes, which can be connected to <c>socketName</c>.
        /// </returns>
        /// <remarks>
        /// 	<b>Box type</b> is given by the functions, which the box is providing, and array of its
        /// <see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.NeededSocket">needed sockets</see>.
        /// Needed socket is given by its name and the functions accepted by this socket.
        /// </remarks>
        public BoxType[] GetSocketTypes(string socketName, BoxModuleI boxModule)
        {
        	if (box.Sockets.ContainsKey(socketName))
        	{
            	return box.GetSocketTypes(socketName);
            }
            else
            {
            	SocketInfo[] socketInfos = this.GetAdditionalSockets(null, boxModule);
            	foreach(SocketInfo socketInfo in socketInfos)
            	{
            		if (String.Equals(socketInfo.name, socketName))
            		{
            			return socketInfo.socketType;
            		}
            	}
            	return null;
            }
        }

        /// <summary>
        /// <para>
        /// Tests existence of the socket named <c>socketName</c>.
        /// </para>
        /// <para>
        /// Returns boolean value that indicates wheather there is some
        /// socket named <c>socketName</c> in the box module.</para>
        /// </summary>
        /// <param name="socketName">Name of the socket.</param>
        /// <param name="boxModule">Box module.</param>
        /// <returns>
        /// Returns true iff the box module has socket
        /// named <c>socketName</c>.
        /// </returns>
        public bool TestSocketNameExistence(string socketName, BoxModuleI boxModule)
        {
            return box.Sockets.ContainsKey(socketName) ||
            	this.GetAdditionalSocketsNames(boxModule).Contains(socketName);
        }

        /// <summary>
        /// Returns boolean value that indicates wheter more than
        /// one box can be connected to the socket specified by <c>socketName</c>.
        /// </summary>
        /// <param name="socketName">Name of the socket.</param>
        /// <param name="boxModule">Box module.</param>
        /// <returns>
        /// Returns true iff more than one box can be connected in
        /// the socket named <c>socketName</c> otherwise returns false.
        /// </returns>
        /// <exception cref="T:Ferda.Modules.Exceptions.NameNotExistError">There
        /// is no <c>socketName</c> in Box module</exception>
        public bool IsSocketMoreThanOne(string socketName, BoxModuleI boxModule)
        {
        	if (box.Sockets.ContainsKey(socketName))
        	{
            	return box.GetSocket(socketName).MoreThanOne;
            }
            else
            {
            	SocketInfo[] socketInfos = this.GetAdditionalSockets(null, boxModule);
            	foreach(SocketInfo socketInfo in socketInfos)
            	{
            		if (String.Equals(socketInfo.name, socketName))
            		{
            			return socketInfo.moreThanOne;
            		}
            	}
            	Debug.Assert(false);
                throw Exceptions.NameNotExistError(null, socketName);
            }
        }

        /// <summary>
        /// Gets names of the sockets.
        /// </summary>
        /// <returns>
        /// Array of <seealso cref="T:System.String">Strings</seealso> as
        /// names of the sockets in the box module.
        /// </returns>
        /// <remarks>
        /// The name of the socket is its unique identifier.
        /// </remarks>
        public string[] GetSocketNames()
        {
            int count = box.Sockets.Count;
            if (count > 0)
            {
                string[] result = new string[count];
                box.Sockets.Keys.CopyTo(result, 0);
                return result;
            }
            return new string[0];
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns boolean value that indicates wheter the property
        /// specified by <c>propertyName</c> is readonly.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        /// Returns true iff the property is readonly.
        /// </returns>
        /// <exception cref="T:Ferda.Modules.Exceptions.NameNotExistError">There
        /// is no property named <c>propertyName</c> in the box module</exception>
        public bool IsPropertyReadOnly(string propertyName)
        {
            return box.GetProperty(propertyName).ReadOnly;
        }

        /// <summary>
        /// Gets regular expression restricting possible values of
        /// the property specified by <c>propertyName</c>.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        /// Returns regular expression restriction for
        /// possible values of the property named <c>propertyName</c>.
        /// </returns>
        /// <exception cref="T:Ferda.Modules.Exceptions.NameNotExistError">There
        /// is no property named <c>propertyName</c> in the box module</exception>
        public string GetPropertyRegexp(string propertyName)
        {
            return box.GetProperty(propertyName).Regexp;
        }

        /// <summary>
        /// Gets list of numeric restrictions of possible values of
        /// the property specified by <c>propertyName</c>.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        /// Lists of numeric restrictions fo possible values of the
        /// property named <c>propertyName</c>.
        /// </returns>
        /// <exception cref="T:Ferda.Modules.Exceptions.NameNotExistError">There
        /// is no property named <c>propertyName</c> in the box module</exception>
        public List<Restriction> GetPropertyRestrictions(string propertyName)
        {
            return box.GetPropertyRestrictions(propertyName);
        }

        /// <summary>
        /// <para>
        /// Tests existence of the property named <c>propertyName</c>.
        /// </para>
        /// <para>
        /// Returns boolean value that indicates wheather there is some
        /// property named <c>propertyName</c> in the box module.</para>
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        /// Returns true iff the box module has property
        /// named <c>propertyName</c>.
        /// </returns>
        public bool TestPropertyNameExistence(string propertyName)
        {
            return box.Properties.ContainsKey(propertyName);
        }

        /// <summary>
        /// Gets names of the properties.
        /// </summary>
        /// <returns>
        /// Array of <seealso cref="T:System.String">Strings</seealso> as
        /// names of the properties in the box module.
        /// </returns>
        /// <remarks>
        /// The name of the property is its unique identifier.
        /// </remarks>
        public string[] GetPropertiesNames()
        {
            int count = box.Properties.Count;
            if (count > 0)
            {
                string[] result = new string[count];
                box.Properties.Keys.CopyTo(result, 0);
                return result;
            }
            return new string[0];
        }

        /// <summary>
        /// Gets array of <see cref="T:Ferda.Modules.SelectString"/> as list
        /// of options for <c>propertyName</c>.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        /// An array of <see cref="T:Ferda.Modules.SelectString"/>
        /// as list of options for property named <c>propertyName</c>.
        /// </returns>
        /// <remarks>
        /// Only for static selectboxes i.e. options doesn`t depend
        /// on box module`s inner state.
        /// </remarks>
        public SelectString[] GetPropertyFixedOptions(string propertyName)
        {
            List<SelectString> result = new List<SelectString>();
            SelectOption[] selectBoxItems = box.GetProperty(propertyName).SelectOptions;
            if (selectBoxItems != null)
                foreach (SelectOption selectBoxItem in selectBoxItems)
                {
                    SelectString resultItem = new SelectString();
                    resultItem.name = selectBoxItem.Name;
                    result.Add(resultItem);
                }
            return result.ToArray();
        }

        /// <summary>
        /// Gets the properties of the box module.
        /// </summary>
        /// <param name="localePrefs">The localization preferences.</param>
        /// <returns>
        /// Array of <seealso cref="T:Ferda.Modules.PropertyInfo">PropertyInfos</seealso>.
        /// </returns>
        public PropertyInfo[] GetProperties(string[] localePrefs)
        {
            List<PropertyInfo> result = new List<PropertyInfo>();
            List<SelectString> selectStrings = new List<SelectString>();
            Serializer.Localization.IHelper boxLocalization = getLocalization(localePrefs);
            foreach (Property property in box.Properties.Values)
            {
                //gets name of category where the proprety belongs to
                string categoryName = String.Empty;
                if (!String.IsNullOrEmpty(property.CategoryName))
                    categoryName = boxLocalization.PropertyCategories[property.CategoryName];

                //gets selectbox options (only for static selectboxes)
                SelectString[] selectBoxParams = null;
                if (property.SelectOptions != null && property.SelectOptions.Length > 0)
                {
                    foreach (SelectOption selectBoxItem in property.SelectOptions)
                    {
                        SelectString selectString = new SelectString();
                        selectString.name = selectBoxItem.Name;
                        selectString.disableProperties = selectBoxItem.DisableProperties;
                        selectString.label =
                            boxLocalization.GetSelectBoxOption(property.Name, selectBoxItem.Name, true).Label;
                        selectStrings.Add(selectString);
                    }
                    selectBoxParams = selectStrings.ToArray();
                    selectStrings.Clear();
                }
                if (selectBoxParams == null)
                    selectBoxParams = new SelectString[0];

                PropertyInfo resultItem = new PropertyInfo(
                    property.Name,
                    categoryName,
                    property.TypeClassIceId,
                    selectBoxParams,
                    property.Visible,
                    property.ReadOnly,
                    GetPropertyRestrictions(property.Name).ToArray(),
                    GetPropertyRegexp(property.Name),
                    property.SettingModuleIdentifier);
                result.Add(resultItem);
            }
            return result.ToArray();
        }

        /// <summary>
        /// Gets TypeClassIceId of the data type of the property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        /// Returns Ice id of type of property value class i.e.
        /// <see cref="F:Ferda.Modules.PropertyInfo.typeClassIceId">TypeClassIceId</see>.
        /// </returns>
        public string GetPropertyDataType(string propertyName)
        {
            return box.GetProperty(propertyName).TypeClassIceId;
        }

        /// <summary>
        /// Gets localized short label for option specified by <c>optionName</c>
        /// of the property specified by <c>propertyName</c>.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="optionName">Name of the option from <see cref="T:Ferda.Modules.SelectString"/> array.</param>
        /// <param name="localePrefs">The localization preferences.</param>
        /// <returns>
        /// Returns <c>short label</c> or <c>label</c> or <c>optionName</c> of the option.
        /// </returns>
        public string GetPropertyOptionShortLocalizedLabel(string propertyName, string optionName, string[] localePrefs)
        {
            //get localization
            Serializer.Localization.IHelper boxLocalization = getLocalization(localePrefs);

            //get SelectBoxParam according to propertyName and optionName
            Serializer.Localization.SelectOption selectBoxParam =
                boxLocalization.GetSelectBoxOption(propertyName, optionName, false);

            //return short label (if its not empty)
            if (!String.IsNullOrEmpty(selectBoxParam.ShortLabel))
                return selectBoxParam.ShortLabel;

            //return label (if its not empty)
            if (!String.IsNullOrEmpty(selectBoxParam.Label))
                return selectBoxParam.Label;

            //return optionName otherwise
            return optionName;
        }

        /// <summary>
        /// Gets default value of the property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        /// Returns default <see cref="T:Ferda.Modules.PropertyValue">value</see>
        /// of the property named <c>propertyName</c>.
        /// </returns>
        public PropertyValue GetPropertyDefaultValue(string propertyName)
        {
            Property boxProperty = box.GetProperty(propertyName);
            if (boxProperty != null)
            {
                switch (boxProperty.TypeClassIceId)
                {
                    case "::Ferda::Modules::BoolT":
                        return
                            new BoolTI((boxProperty.Default == "True") || (boxProperty.Default == "true") ||
                                       (boxProperty.Default == "1"));
                    case "::Ferda::Modules::ShortT":
                        return new ShortTI(Convert.ToInt16(boxProperty.Default));
                    case "::Ferda::Modules::IntT":
                        return new IntTI(Convert.ToInt32(boxProperty.Default));
                    case "::Ferda::Modules::LongT":
                        return new LongTI(Convert.ToInt64(boxProperty.Default));
                    case "::Ferda::Modules::FloatT":
                        if (!String.IsNullOrEmpty(boxProperty.Default))
                        {
                            return new FloatTI(Convert.ToSingle(boxProperty.Default, CultureInfo.InvariantCulture));
                        }
                        else
                            return new FloatTI(0);
                    case "::Ferda::Modules::DoubleT":
                        if (!String.IsNullOrEmpty(boxProperty.Default))
                        {
                            return new DoubleTI(Convert.ToDouble(boxProperty.Default, CultureInfo.InvariantCulture));
                        }
                        else
                            return new DoubleTI(0);
                    case "::Ferda::Modules::StringT":
                        return new StringTI(boxProperty.Default);
                    case "::Ferda::Modules::StringSeqT":
                        return new StringSeqTI(BoxInfoHelper.Csv2Strings(boxProperty.Default));
                    case "::Ferda::Modules::DateTimeT":
                        if (!String.IsNullOrEmpty(boxProperty.Default))
                            return new DateTimeTI(Convert.ToDateTime(boxProperty.Default));
                        else
                            return new DateTimeTI(0, 0, 0, 0, 0, 0);
                    case "::Ferda::Modules::DateT":
                        if (!String.IsNullOrEmpty(boxProperty.Default))
                            return new DateTI(Convert.ToDateTime(boxProperty.Default));
                        else
                            return new DateTI(0, 0, 0);
                    case "::Ferda::Modules::TimeT":
                        if (!String.IsNullOrEmpty(boxProperty.Default))
                            return new TimeTI(Convert.ToDateTime(boxProperty.Default));
                        else
                            return new TimeTI(0, 0, 0);
                    //case "::Ferda::Modules::CategoriesT":
                    //    return new CategoriesTI();
                    //case "::Ferda::Modules::GenerationInfoT":
                    //    return new GenerationInfoTI();
                    //case "::Ferda::Modules::HypothesesT":
                    //    return new HypothesesTI();
                    default:
                        return null;
                }
            }
            return null;
        }

        #endregion

        #region Box main config

        /// <summary>
        /// Gets localized label of the box module.
        /// </summary>
        /// <param name="localePrefs">The localization preferences.</param>
        /// <returns>Localized label of the box module.</returns>
        public string GetLabel(string[] localePrefs)
        {
            Serializer.Localization.IHelper boxLocalization = getLocalization(localePrefs);
            if (String.IsNullOrEmpty(boxLocalization.Label))
            {
#if DEBUG
                string message = "BoxInf07: Localized label is empty :" + identifier + " (" + boxLocalization.LocaleId +
                                 ")";
                Debug.WriteLine(message);
                throw new Exception(message);
#endif
#if !DEBUG
                return this.Identifier;
#endif
            }
            return boxLocalization.Label;
        }

        /// <summary>
        /// The identifier of the box module`s type. It has to be unique!
        /// </summary>
        /// <value>
        /// The identifier of the box module`s type. Please remember that the
        /// identifier is used for identification of the box module type/kind
        /// so that if new instance of some box module`s type wants be created
        /// the <see cref="T:Ferda.Modules.BoxModuleFactoryCreatorI"/>
        /// of <see cref="T:Ferda.Modules.BoxModuleFactoryI"/> with the specified
        /// <see cref="M:Ferda.Modules.BoxModuleFactoryCreatorI.getIdentifier(Ice.Current)">identifier</see>
        /// i.e. type is used.
        /// </value>
        public string Identifier
        {
            get
            {
#if DEBUG
                if (box.Identifier != identifier)
                {
                    string message = "BoxInf08: Box identifier inconsistence. " + identifier + " vs. " + box.Identifier;
                    Debug.WriteLine(message);
                    throw new Exception(message);
                }
                if (String.IsNullOrEmpty(identifier))
                {
                    string message = "BoxInf09: Box Identifer is null or empty";
                    Debug.WriteLine(message);
                    throw new Exception(message);
                }
#endif
                return identifier;
            }
        }

        /// <summary>
        /// Gets the box module`s icon.
        /// </summary>
        /// <value>
        /// The box module`s icon i.e. content of the "*.ico" file
        /// as array of <see cref="T:System.Byte">Bytes</see>.
        /// </value>
        public byte[] Icon
        {
            get { return BoxInfoHelper.TryGetBinaryFile(ConfigFilesDirectoryPath, box.IconPath, false); }
        }

        /// <summary>
        /// Gets localized hint (short suggestion) of the box module.
        /// </summary>
        /// <param name="localePrefs">The localization preferences.</param>
        /// <returns>
        /// Localized hint (short suggestion) of the box module.
        /// </returns>
        public string GetHint(string[] localePrefs)
        {
            Serializer.Localization.IHelper boxLocalization = getLocalization(localePrefs);
            if (!String.IsNullOrEmpty(boxLocalization.Hint))
                return boxLocalization.Hint;
            else
                return GetLabel(localePrefs);
        }

        /// <summary>
        /// Gets the <see href="http://www.w3.org/tr/2000/cr-svg-20001102/index.html">
        /// Scalable Vector Graphics (SVG)</see> design.
        /// </summary>
        /// <value>The string representation of SVG design file.</value>
        public string Design
        {
            get
            {
                return Convert.ToString(
                    BoxInfoHelper.TryGetStringFile(ConfigFilesDirectoryPath, box.DesignPath, false)
                    );
            }
        }

        /// <summary>
        /// Array of <see cref="T:System.String">Strings</see>
        /// as list of names of categories, in which this
        /// box module belongs to.
        /// </summary>
        /// <value>Names of categories, in which the box module belongs to.</value>
        /// <remarks>
        /// <para>
        /// Box module can be in any number of categories.
        /// </para>
        /// <para>
        /// These names are not localized  i.e. the name of
        /// the category is an identifier of the category.
        /// </para>
        /// <para>
        /// For localization of this identifiers use
        /// <see cref="M:Ferda.Modules.Boxes.IBoxInfo.GetBoxCategoryLocalizedName(System.String,System.String)"/>.
        /// </para>
        /// </remarks>
        /// <seealso cref="M:Ferda.Modules.Boxes.IBoxInfo.GetBoxCategoryLocalizedName(System.String,System.String)"/>
        public string[] Categories
        {
            get
            {
                if (box.Categories == null)
                    return new string[0];
                return box.Categories;
            }
        }

        /// <summary>
        /// Gets the localized name of the box`s category.
        /// </summary>
        /// <param name="cultureName">Name of the culture i.e. (one) localization prefrence.</param>
        /// <param name="categoryName">Name of the category.</param>
        /// <returns>
        /// Localized name of the category named <c>categoryName</c>.
        /// </returns>
        public string GetBoxCategoryLocalizedName(string cultureName, string categoryName)
        {
            if (String.IsNullOrEmpty(categoryName))
                return String.Empty;
            string[] localePrefs = (String.IsNullOrEmpty(cultureName)) ? new string[0] : new string[] {cultureName};
            Serializer.Localization.IHelper boxLocalization = getLocalization(localePrefs);
            string result;
            if (boxLocalization.Categories.TryGetValue(categoryName, out result))
            {
                return result;
            }
            else
            {
                Debug.Assert(categoryName == "other",
                             "Category is not localized (" + identifier + " - " + cultureName + " - " + categoryName +
                             ".");
                return String.Empty;
            }
        }

        #endregion

        #region Actions

        /// <summary>
        /// Gets actions of the box module.
        /// </summary>
        /// <param name="localePrefs">The localization preferences.</param>
        /// <returns>
        /// Array of <see cref="T:Ferda.Modules.ActionInfo"/>.
        /// </returns>
        public ActionInfo[] GetActions(string[] localePrefs)
        {
            List<ActionInfo> result = new List<ActionInfo>();
            Serializer.Localization.IHelper boxLocalization = getLocalization(localePrefs);
            foreach (Action action in box.Actions.Values)
            {
                ActionInfo resultItem = new ActionInfo(
                    action.Name,
                    boxLocalization.Actions[action.Name].Label,
                    boxLocalization.Actions[action.Name].Hint,
                    BoxInfoHelper.TryGetBinaryFile(ConfigFilesDirectoryPath, action.IconPath, false),
                    GetActionInfoNeededConnectedSockets(action.Name)
                    );
                result.Add(resultItem);
            }
            return result.ToArray();
        }

        /// <summary>
        /// Gets needed connected sockets of the action specified by <c>actionName</c>.
        /// </summary>
        /// <param name="actionName">Name of the action.</param>
        /// <returns>
        /// Array of conditions on needed conected sockets.
        /// </returns>
        /// <remarks>Returned value is array of conditions on connections in
        /// sockets. At least one of returned conditions has to be realized before
        /// execution of the action is allowed.</remarks>
        /// <exception cref="T:Ferda.Modules.NameNotExistError">Iff <c>actionName</c> is bad.</exception>
        public string[][] GetActionInfoNeededConnectedSockets(string actionName)
        {
            try
            {
                return box.ActionNeededConnectedSockets[actionName];
            }
            catch (KeyNotFoundException ex)
            {
                throw Exceptions.NameNotExistError(ex, actionName);
            }
        }

        #endregion

        #region Other helping functions

        /// <summary>
        /// Gets localized data prepared for processing and completing result for
        /// <see cref="M:Ferda.Modules.BoxModuleI.getModulesAskingForCreation()"/>.
        /// </summary>
        /// <param name="localePrefs">The localization preferences.</param>
        /// <returns>Prepared data for next processing by
        /// <see cref="M:Ferda.Modules.Boxes.BoxInfo.GetModulesAskingForCreation(System.String[], Ferda.Modules.BoxModuleI)"/>.
        /// </returns>
        public Dictionary<string, ModulesAskingForCreation> getModulesAskingForCreationNonDynamic(string[] localePrefs)
        {
            List<string> input = box.ModulesAskingForCreation;
            Serializer.Localization.IHelper boxLocalization = getLocalization(localePrefs);
            Dictionary<string, ModulesAskingForCreation> result = new Dictionary<string, ModulesAskingForCreation>();
            foreach (string moduleAFCName in input)
            {
                //localized item contains label, hint, dynamic help items
                ModulesAskingForCreation localizedItem = boxLocalization.ModulesAskingForCreation[moduleAFCName];
                result.Add(moduleAFCName, localizedItem);
            }
            return result;
        }

        #endregion

        /// <summary>
        /// Tries to the get specified phrase (<c>phraseIdentifier</c>).
        /// </summary>
        /// <param name="phraseIdentifier">The phrase`s identifier.</param>
        /// <param name="phraseLocalizedText">The phrase`s localized text.</param>
        /// <param name="localePrefs">Localization preferences</param>
        /// <returns>
        /// <c>true</c> if localization of specified phrase (<c>phraseIdentifier</c>)
        /// exists; otherwise, <c>false</c>.
        /// </returns>
        public bool TryGetPhrase(string phraseIdentifier, out string phraseLocalizedText, string[] localePrefs)
        {
            Serializer.Localization.IHelper boxLocalization = getLocalization(localePrefs);
            return boxLocalization.TryGetPhrase(phraseIdentifier, out phraseLocalizedText);
        }

        #region IBoxInfo Members

        /// <summary>
        /// Validates setting of this box module.
        /// </summary>
        /// <param name="boxModule">The box module.</param>
        /// <remarks>
        /// 	<para>
        /// Some settings may cause to exceptions or some error states.
        /// </para>
        /// 	<para>
        /// Validates the specified box module. (e.g. setting of some properties
        /// is right (satisfies its restrictions) but box module can not work with
        /// this setting e.g. property "OdbcConnectionString" is valid ODBC connection
        /// string but the box module can not connect with given value to the
        /// specified data source.)
        /// </para>
        /// 	<para>
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
        /// 	<para>
        /// If setting of current box is bad (may leads to some errors
        /// of exceptions) than some exception is thrown.
        /// </para>
        /// </remarks>
        public virtual void Validate(BoxModuleI boxModule)
        {
        }

        #endregion
    }
}
