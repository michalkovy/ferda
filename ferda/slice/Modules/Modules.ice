/*
GENERAL KEYWORDS
Nonmutating
	 Operations that use the Slice Nonmutating keyword must not modify object
	 state.
Idempotent
	 Operations that use the Slice Idempotent keyword can modify object state,
	 but invoking an operation twice in a row must result in the same object
	 state as invoking it once.
*/
#ifndef FERDA_MODULES_MODULES
#define FERDA_MODULES_MODULES

#include <Modules/BuiltinSequences.ice>
#include <Modules/BoxType.ice>
#include <Modules/Common.ice>
#include <ModulesManager/ManagersEngine.ice>
#include <Modules/BasicPropertyTypes.ice>
#include <Modules/Exceptions.ice>

module Ferda {
	module ModulesManager {
		interface ManagersEngine;
	};

	module Modules {
		/*  FORWARD DECLARATIONS  */
		interface BoxModule;
		interface BoxModuleFactory;
		interface BoxModuleFactoryCreator;

		/*  CONSTANTS  */
		const int factoryRefreshTime = 300000;
		const int factoryRefreshedTestTime = 1000000;

		/*  USER TYPES  */

		/**
		 *
		 * Each box module has list of connections to other
		 * box modules connected to his sockets. The
		 * connection to other box module is represented by
		 * this structure.
		 *
		 **/
		struct ModulesConnection {

			/**
			 *
			 * Name (identifier) of the box module`s socket, to which is other box module conected.
			 *
			 **/
			string socketName;

			/**
			 *
			 * Other box module, which is connected to the socket.
			 *
			 **/
			BoxModule* boxModuleParam;
		};
		sequence<ModulesConnection> ModulesConnectionSeq;

		/**
		 *
		 * Used in ModuleAskingForCreation for setting some
		 * values in some properties during creating new box module.
		 *
		 **/
		 ["clr:class"]
		struct PropertySetting {
			/**
			 *
			 * Name of property in new box module which has
			 * to be set.
			 *
			 **/
			string propertyName;
			/**
			 *
			 * Value which will be saved in the property.
			 *
			 **/
			PropertyValue value;
		};
		sequence<PropertySetting> PropertySettingSeq;
		sequence<PropertySettingSeq> PropertySettingSeqSeq;

		/**
		 *
		 * This structure contains information about help files.
		 * Each item in DynamicHelpItemSeq contains identifier
		 * which point to identifier in HelpFileInfo structure.
		 * It is possible and expected that each language version
		 * (localization) will have its own HelpFileInfo structures.
		 *
		 * @see DynamicHelpItem
		 *
		 **/
		struct HelpFileInfo { /* all fields will be localized */
			/**
			 *
			 * Identifier of help file.
			 * Matches to "DynamicHelpItem.identifier".
			 * DynamicHelpItem points to HelpFileInfo which
			 * is representing help file, where DynamicHelpItem
			 * is pointing. Can be different in different
			 * localizations.
			 *
			 * @see DynamicHelpItem
			 *
			 **/
			string identifier;
			/**
			 *
			 * Version of help file. Can be different
			 * in different localizations.
			 *
			 * @see DynamicHelpItem::identifier
			 *
			 **/
			int version;
			/**
			 *
			 * Localized label of help file.
			 *
			 **/
			string label;
		};
		sequence<HelpFileInfo> HelpFileInfoSeq;

		/**
		 *
		 * Used in ModuleAskingForCreation, BoxModule, ModuleForInteraction
		 * for labeled references to help files.
		 *
		 * @see HelpFileInfo
		 *
		 **/
		struct DynamicHelpItem {
			/**
			 *
			 * Identifier of help file.
			 *
			 * @see HelpFileInfo::identifier
			 *
			 **/
			string identifier;
			/**
			 *
			 * Localized dynamic help item label.
			 *
			 **/
			string label;

			/**
			 *
			 * Location in a help file.
			 *
			 * @see HelpFileInfo
			 *
			 **/
			string url;
		};
		sequence<DynamicHelpItem> DynamicHelpItemSeq;

		/**
		 *
		 * Structure for one item of cobobox.
		 *
		 **/
		struct SelectString {
			/**
			 *
			 * Combobox item`s identifier i. e. cobobox item`s value.
			 *
			 **/
			string name;

			/**
			 *
			 * Localized name of combobox item i. e. combobox item`s label.
			 *
			 **/
			string label;

			/**
			 *
			 * Names of properties which will not be editable at that
			 * time is this item selected.
			 *
			 **/
			StringSeq disableProperties;
		};

		/**
		 *
		 * Sequence of cobobox items (name, label, disable properties).
		 *
		 **/
		sequence<SelectString> SelectStringSeq;

		/**
		 *
		 * Structure for description of box module`s socket.
		 * Please keep in mind that each property is also a socket
		 * among others because of fundamental idea that each
		 * property can be setted by other box module (connected
		 * in corresponding socket).
		 *
		 * Please note that not all sockets can be used as properties.
		 *
		 **/
		struct SocketInfo {

			/**
			 *
			 * Socket`s unique name (identifier).
			 *
			 **/
			string name;

			/**
			 *
			 * Localized socket`s name i. e. socket`s label.
			 *
			 **/
			string label;

			/**
			 *
			 * Localized hint (short help) of the socket.
			 *
			 **/
			string hint;

			/**
			 *
			 * Socket`s design (SVG file).
			 *
			 **/
			string design;

			/**
			 *
			 * Types of boxes that can be connected to the socket.
			 *
			 **/
			BoxTypeSeq socketType;

			/**
			 *
			 * Names of properties that are set by the socket automatically.
			 * This is useful iff some property value depends on value of the
			 * socket.
			 *
			 **/
			StringSeq settingProperties;

			/**
			 *
			 * If there can be more than one box module
			 * connected in this socket ("infinite").
			 *
			 **/
			bool moreThanOne;
		};
		sequence<SocketInfo> SocketInfoSeq;

		/**
		 *
		 * Struct providing information about box module`s actions.
		 * Action is a function, which can be started directly by user
		 * (e. g. some generation, ...).
		 *
		 **/
		struct ActionInfo {

			/**
			 *
			 * Action`s identifier.
			 *
			 **/
			string name;

			/**
			 *
			 * Localized action`s name i. e. action`s label.
			 *
			 **/
			string label;

			/**
			 *
			 * Localized hint (short help) for the action.
			 *
			 **/
			string hint;

			/**
			 *
			 * Action`s icon design (ICO file).
			 *
			 **/
			ByteSeq icon;

			/**
			 *
			 * Sequence of sequences of socket names i. e.
			 * sequence of neccessary conditions for the action`s running.
			 * More precisely at least on of these conditions
			 * (on of these sequences of socket names) has to be solved i. e.
			 * in all the sockets in the sequence has to be connected some box
			 * module.
			 *
			 **/
			StringSeqSeq neededConnectedSockets;
		};
		sequence<ActionInfo> ActionInfoSeq;

		/**
		 *
		 * Struct providing information about restrictions on values of
		 * property of the box module.
		 *
		 **/
		struct Restriction {
			/**
			 *
			 * Long value used for short/int/long restriction values.
			 *
			 **/
			LongOpt integral;
			/**
			 *
			 * Double value used for float/double restriction values.
			 *
			 **/
			DoubleOpt floating;
			/**
			 *
			 * Iff true than value (integral or floating) will be
			 * used as minimum otherwise as maximum.
			 *
			 **/
			bool min;
			/**
			 *
			 * Iff true tha value (integral or floating) will be included
			 * in valid values interval (... or equal) otherwise the value
			 * is excluded from valid values interval.
			 *
			 **/
			bool including;
		};
		sequence<Restriction> RestrictionSeq;


		/**
		 *
		 * Struct providing additional information about properties.
		 * (Properties are also mentioned in sockets i. e. SocketInfo)
		 *
		 **/
		struct PropertyInfo {
			/**
			 *
			 * Property`s identifier. Label, hint, ... are in SocketInfo with
			 * same value of name (identifier).
			 *
			 **/
			string name;

			/**
			 *
			 * Name of category, whre the property belongs to.
			 *
			 **/
			string categoryName;

			/**
			 *
			 * Property`s data type.
			 * Example for float property i.e. "::Ferda::Modules::FloatT"
			 *
			 **/
			string typeClassIceId;


			/**
			 *
			 * Options of property iff if it`s dataType is SelectT
			 * (But no if property dataType is SelectOptT - it is
			 * dynamicaly generated).
			 *
			 **/
			SelectStringSeq selectBoxParams;

			/**
			 *
			 * Whether user can see this property. If false than property is
			 * usually used for permanent saving some computed data by serializing
			 * the project.
			 *
			 **/
			bool visible;

			/**
			 *
			 * If this property is only for reading, not for writing i. e.
			 * user can not change value of this property. If property is not visible
			 * than property should be not readOnly - opposite doesn`t make any sense.
			 *
			 **/
			bool readOnly;

			/**
			 *
			 * Sequence of restrictions of numerical values.
			 * Each restriciton must be satisfied.
			 *
			 **/
			RestrictionSeq numericalRestrictions;

			/**
			 *
			 * Property`s value has to satisfy this
			 * regular expression condition.
			 *
			 **/
			string regexp;

			/**
			 *
			 * Identifier of SettingModule which have to be used
			 * for setting this property. When it is set to "",
			 * non Setting Module will be used.
			 *
			 **/
			string settingModuleIdentifier;
		};
		sequence<PropertyInfo> PropertyInfoSeq;

		/**
		 *
		 * Struct providing information about one single
		 * box module asking for creation.
		 *
		 * ModulesAskingForCreation can create more than one
		 * new box module therefore it contanins array of
		 * ModuleAskingForCreation.
		 *
		 **/
		struct ModuleAskingForCreation {

			/**
			 *
			 * Identifier of new box module. It is used for getting
			 * corresponding new box module`s factory creator.
			 *
			 **/
			string newBoxModuleIdentifier;

			/**
			 *
			 * This string will be used as user label of the new box module.
			 *
			 **/
			StringOpt newBoxModuleUserLabel;

			/**
			 *
			 * This is a sequence of connections, which will be created to
			 * newly created box module.
			 *
			 **/
			ModulesConnectionSeq modulesConnection;

			/**
			 *
			 * This is a sequence of names of properties and their values,
			 * which will be set in newly created box module.
			 *
			 **/
			PropertySettingSeq propertySetting;
		};
		sequence<ModuleAskingForCreation> ModuleAskingForCreationSeq;

		/**
		 *
		 * Struct providing information box module(s) which are
		 * asking for creation.
		 *
		 **/
		struct ModulesAskingForCreation {

			/**
			 *
			 * Localized label of the modules asking for creation.
			 *
			 **/
			string label;

			/**
			 *
			 * Localized hint (short help) for the .
			 *
			 **/
			string hint;

			/**
			 *
			 * Localized sequence of dynamic help items.
			 *
			 * @see DynamicHelpItem
			 *
			 **/
			DynamicHelpItemSeq help;


			/**
			 *
			 * Number of single box modules asking for creation
			 * (ModuleAskingForCreation) within one option
			 * i. e. ModulesAskingForCreation
			 *
			 * @see ModuleAskingForCreation
			 *
			 **/
			ModuleAskingForCreationSeq newModules;
		};
		sequence<ModulesAskingForCreation> ModulesAskingForCreationSeq;

		sequence<BoxModule*> BoxModulePrxSeq;



		/**
		 *
		 * Box module`s interface.
		 *
		 **/
		interface BoxModule {

			/**
			 *
			 * Gets default user label e. g. if some box
			 * module`s property drives user label.
			 *
			 **/
			nonmutating StringOpt getDefaultUserLabel();

			/**
			 *
			 * Dynamically returns box module`s sequence of help items.
			 *
			 **/
			nonmutating DynamicHelpItemSeq getDynamicHelpItems();

			/**
			 *
			 * Executes action of specified name.
			 * Invocation of this method can be asynchronous.
			 *
			 **/
			["ami"] void runAction(string actionName)
				throws NeedConnectedSocketError, BoxRuntimeError,
					NameNotExistError, BadParamsError;

			/**
			 *
			 * //TODO
			 * For lambda-like boxes
			 *
			 **/
			nonmutating SocketInfoSeq getAdditionalSockets();

			/**
			 *
			 * Creates connection with one BoxModule to socket.
			 *
			 * @param socketName Name of the socket, where should be connected
			 * other otherModule.
			 *
			 * @param otherModule The proxy of other module, which should be
			 * connected to socket socketName.
			 *
			 * @throws BadTypeError This exception means bad type of functions.
			 *
			 * @throws NameNotExistError Is thrown if socket with socketName does
			 * not exist in this BoxModule.
			 *
			 * @throws ConnectionExistsError Is thrown if this connection alredy exists.
			 *
			 **/
			idempotent void setConnection(string socketName, BoxModule* otherModule)
				throws NameNotExistError, BadTypeError, ConnectionExistsError;

			/**
			 *
			 * Gets all connection in the socket.
			 *
			 * @param socketName Name of the socket.
			 *
			 * @throws NameNotExistError Is thrown if socket with socketName does
			 * not exist in this BoxModule.
			 *
			 **/
			nonmutating BoxModulePrxSeq getConnections(string socketName)
				throws NameNotExistError;

			/**
			 *
			 * Removes connection with one BoxModule from socket.
			 *
			 * @param socketName Name of socket from which to remove connection.
			 *
			 * @param boxModuleIceIdentity Ice identity of BoxModule which is
			 * connected to socket socketName and this connection is to be
			 * removed.
			 *
			 * @throws NameNotExistError Is thrown if socket with socketName does
			 * not exist in this BoxModule.
			 *
			 * @throws ConnectionNotExistError Is thrown if BoxModule
			 * boxModuleIceIdentity is not connected to socket socketName.
			 *
			 **/
			void removeConnection(string socketName, string boxModuleIceIdentity)
				throws NameNotExistError, ConnectionNotExistError;

			/**
			 *
			 * Creates connection with one BoxModule to socket.
			 *
			 * @param propertyName Name of the property.
			 *
			 * @param value New value of the property.
			 *
			 * @throws BadTypeError This exception means bad data type of value.
			 *
			 * @throws NameNotExistError Is thrown if property with propertyName does
			 * not exist in this BoxModule.
			 *
			 * @throws ReadOnlyError Is thrown if this property is only for reading.
			 *
			 * @throws BadValueError This exception means bad value e. g. it is out
			 * of restrictions or regular expression conditions.
			 *
			 **/
			idempotent void setProperty(string propertyName, PropertyValue value)
				throws NameNotExistError, BadTypeError, BadValueError, ReadOnlyError;

			/**
			 *
			 * Gets value of the property. Invocation of this method can be asynchronous.
			 *
			 * @throws NameNotExistError Is thrown if property with propertyName does
			 * not exist in this BoxModule.
			 *
			 **/
			["ami"] nonmutating PropertyValue getProperty(string propertyName)
				throws NameNotExistError;

			/**
			 *
			 * Used only for property of SelectOptT data type.
			 *
			 * @throws NameNotExistError Is thrown if property with propertyName does
			 * not exist in this BoxModule.
			 *
			 **/
			nonmutating SelectStringSeq getPropertyOptions(string propertyName)
				throws NameNotExistError;

			/**
			 *
			 * Useful for property of OtherT type and it`s neededProperty
			 * checking
			 *
			 * @throws NameNotExistError Is thrown if property with propertyName does
			 * not exist in this BoxModule.
			 *
			 * @return True if this property was set by SettingModule
			 *
			 **/
			nonmutating bool isPropertySet(string propertyName)
				throws NameNotExistError;

			/**
			 *
			 * Gets sequence of possible modules asking for creation.
			 *
			 **/
			nonmutating ModulesAskingForCreationSeq getModulesAskingForCreation();

			/**
			 *
			 * Returns ice_id of proxy which will be returned by getFunctions(),
			 * theoreticaly this can return something other than what returns
			 * BoxModuleFactoryCreator in function getBoxModuleFunctionsIceIds -
			 * specially lambda box module
			 *
			 * @return ice_id of proxy which will be returned by getFunctions
			 *
			 * @see BoxModuleFactoryCreator::getBoxModuleFunctionsIceIds
			 *
			 **/
			nonmutating StringSeq getFunctionsIceIds();

			/**
			 *
			 * Throught lambda abstraction is BoxModule interface separated
			 * from functions over this BoxModule. Functions is module (object)
			 * implementing some interface defined in it`s slice design.
			 * Functions can works with properties and sockets of this BoxModule
			 *
			 * @return Functions object proxy.
			 *
			 **/
			nonmutating Object* getFunctions();

			/**
			 *
			 * Gets proxy of factory of this box module.
			 *
			 **/
			nonmutating BoxModuleFactory* getMyFactory();

			/**
			 *
			 * Validates box module (e.g. its settings).
			 *
			 * @throws BoxRuntimeError Is thrown if some runtime error ocured.
			 *
			 * @throws BadValueError Is thrown if function/method gets bad values.
			 *
			 * @throws BadParamsError Is thrown if some property/socket has bad value.
			 *
			 * @throws NoConnectionInSocketError Is thrown if some connection is required.
			 *
			 **/
			nonmutating void validate()
				throws
					BoxRuntimeError,
					BadValueError,
					BadParamsError,
					NoConnectionInSocketError;
		};

		interface Entity {
			nonmutating string getLabel(StringSeq localePrefs);
		};

		interface ModuleForInteraction extends Entity {

			/**
			 *
			 * Gets conditions on box module`s socket, which has to be
			 * satisfied before the module for interaction can run.
			 *
			 **/
			nonmutating StringSeq getNeededConnectedSockets();

			/**
			 *
			 * Gets array of box types, which can use this module for interaction.
			 *
			 **/
			nonmutating BoxTypeSeq getAcceptedBoxTypes();

			/**
			 *
			 * Gets localized hint (short help) for this module for interaction.
			 *
			 **/
			nonmutating string getHint(StringSeq localePrefs);

			/**
			 *
			 * Gets help file as aray of Bytes for this module for interaction.
			 *
			 **/
			nonmutating ByteSeq getHelpFile(string identifier);

			/**
			 *
			 * Gets information about help files for this module for interaction.
			 *
			 **/
			nonmutating HelpFileInfoSeq getHelpFileInfoSeq(StringSeq localePrefs);

			/**
			 *
			 * Dynamically gets help item for this module for interaction.
			 *
			 **/
			nonmutating DynamicHelpItemSeq getDynamicHelpItems(StringSeq localePrefs);

			/**
			 *
			 * Runs the module for interaction.
			 *
			 * @param boxModuleParam Proxy of the box module, over which this
			 * module for interaction runs.
			 *
			 * @param localePrefs Localization preferences.
			 *
			 * @param manager Proxy of modules manager.
			 *
			 * @throws BoxRuntimeError If there is any hard ModuleForInteraction problem
			 * of running it, this exception will be raised. Raise this exception
			 * only if it is really needed, it is still error of application this
			 * exception!
			 *
			 **/
			idempotent void run(
				BoxModule* boxModuleParam,
				StringSeq localePrefs,
				Ferda::ModulesManager::ManagersEngine* manager)
					throws BoxRuntimeError;

			/**
			 *
			 * Gets icon (*.ico) as array of Bytes for this module for intraction.
			 *
		 	 **/
			nonmutating ByteSeq getIcon();
		};

		interface SettingModule extends Entity {

			nonmutating string getIdentifier();

			/**
			 *
			 * Run this Module for setting property.
			 *
			 * @param valueBefore Value of property (which this module set) before
			 * this action.
			 *
			 * @param boxModuleParam Over this Box Module SettingModule works.
			 *
			 * @param about String value which should be shown in property bar.
			 *
			 * @return New value of property to be set.
			 *
			 * @throws CouldNotRunError If there is any hard SettingModule problem
			 * of running it, this exception will be raised. Raise this exception
			 * only if it is really needed, it is still error of application this
			 * exception!
			 *
			 * @see PropertyInfo
			 *
			 **/
			PropertyValue run(
				PropertyValue valueBefore,
				BoxModule* boxModuleParam,
				StringSeq localePrefs,
				Ferda::ModulesManager::ManagersEngine* manager,
				out string about)
					throws BoxRuntimeError;

			/**
			 *
			 * Gets "about" string from the PropertyValue.
			 *
			 * @param value PropertyValue for generating the "about" string.
			 *
			 * @return String, which describes specified
			 * PropertyValue as well as possible.
			 *
			 **/
			nonmutating string getPropertyAbout(PropertyValue value);
		};

		/**
		 *
		 * This type of module is used if about string in property grid can be
		 * converted to "involved" type (set by SettingModule) and vice versa.
		 *
		 **/
		interface SettingModuleWithStringAbility extends SettingModule {

			/**
			 *
			 * Gets PropertyValue form the "about" string.
			 *
			 * @param about The about (input) string for conversion to PropertyValue.
			 *
			 * @param localePrefs Localization prefrences.
			 *
			 **/
			PropertyValue convertFromStringAbout(string about, StringSeq localePrefs)
				throws IsNotConvertibleError;
		};

		interface BoxModuleFactory {

			/**
			 *
			 * Creates new box module in this factory.
			 * Proxy of newly created box module is returned.
			 *
			 **/
			BoxModule* createBoxModule();

			/**
			 *
			 * If client will not call this in 30 minutes, destroy will be called
			 * automaticly
			 *
			 */
			idempotent void refresh();

			/**
			 *
			 * If the factory is empty it will be destroyed and true will be returned;
			 * othervise, false is returned.
			 *
			 **/
			bool destroyIfEmpty();

			/**
			 *
			 * Destroys the box module witch specified boxIdentity.
			 *
			 * @param boxIdentity Identity of box module, which should be destroyed.
			 *
			 **/
			void destroyBoxModule(string boxIdentity);

			/**
			 *
			 * Destroys this box module factory and all it`s box modules.
			 *
			 **/
			void destroy();

			/**
			 *
			 * Gets information about the sockets of the box module.
			 * IMPORTANT: each property has to have equivalent socket.
			 *
			 **/
			nonmutating SocketInfoSeq getSockets();

			/**
			 *
			 * Gets information about the actions of the box module.
			 *
			 **/
			nonmutating ActionInfoSeq getActions();

			/**
			 *
			 * Gets information about the properties of the box module.
			 * IMPORTANT: each property has to have equivalent socket.
			 *
			 **/
			nonmutating PropertyInfoSeq getProperties();

			/**
			 *
			 * Gets information about the help files of the box module.
			 *
			 **/
			nonmutating HelpFileInfoSeq getHelpFileInfoSeq();

			/**
			 *
			 * Gets proxy of factory creator of this box module.
			 *
			 **/
			nonmutating BoxModuleFactoryCreator* getMyFactoryCreator();
		};

		interface BoxModuleFactoryCreator extends Entity {

			/**
			 *
			 * Creates new factory for box modules.
			 *
			 * @param localePrefs Localization preferences.
			 *
			 * @param manager Proxy of modules manager, to which this
			 * newly created factory belongs to.
			 *
			 **/
			BoxModuleFactory* createBoxModuleFactory(
				StringSeq localePrefs,
				Ferda::ModulesManager::ManagersEngine* manager);


			/**
			 *
			 * Gets identifier of BoxModule witch can be created by the
			 * BoxModuleFactory created by this BoxModuleFactoryCreator.
			 *
			 **/
			nonmutating string getIdentifier();

			/**
			 *
			 * Gets ice_ids (array of strings) of functions provided
			 * by box module..
			 *
		 	 **/
			nonmutating StringSeq getBoxModuleFunctionsIceIds();

			/**
			 *
			 * Gets names (not localized) of categories, whre the box
			 * module belongs to.
			 *
		 	 **/
			nonmutating StringSeq getBoxCategories();

			/**
			 *
			 * Gets localized name of given categoryName.
			 *
			 * @param localePrefs Localization preferences.
			 *
			 * @param categoryName Name of the category.
			 *
		 	 **/
			nonmutating StringOpt getBoxCategoryLocalizedName(string locale, string categoryName);

			/**
			 *
			 * Gets localized box module`s hint i. e. short help.
			 *
			 * @param localePrefs Localization preferences.
			 *
		 	 **/
			nonmutating string getHint(StringSeq localePrefs);

			/**
			 *
			 * Gets box module`s help file as array of Bytes.
			 *
			 * @param identifier Identifier of the help file.
			 *
		 	 **/
			nonmutating ByteSeq getHelpFile(string identifier);

			/**
			 *
			 * Gets box module`s icon (*.ico) as array of Bytes.
			 *
		 	 **/
			nonmutating ByteSeq getIcon();

			/**
			 *
			 * Gets box module`s design from SVG file as string.
			 *
		 	 **/
			nonmutating string getDesign();
		};


		/**
		 *
		 * PropertyBoxModule is simple BoxModule that is used for setting
		 * property value by BoxModule. This BoxModule has to have property with
		 * name "value".
		 *
		 **/
		interface PropertyBoxModuleFactoryCreator extends BoxModuleFactoryCreator {
			nonmutating string getPropertyClassIceId();
		};

	};
};

#endif;
