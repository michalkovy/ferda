using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Ferda.Modules.Boxes.Serializer.Configuration
{
    /// <summary>
    /// <para>
    /// Class for <see cref="T:Ferda.Modules.Boxes.Serializer.Reader">deserealization</see>
    /// of box`s XML configuration file.
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// For further information please see other documentation e. g. documentation for 
    /// XML config files, XSD schema or slice design of Ferda.Modules.
    /// </para>
    /// <para>
    /// Lot of memebers has field named "Name" it is uniquie identifier
    /// which among other serve to joining corresponding records in 
    /// <see cref="T:Ferda.Modules.Boxes.Serializer.Localization.BoxLocalization">
    /// XML localization file</see>.
    /// </para>
    /// <b>Box Module`s default directory:</b>
    /// <para>
    /// All relative pathes specified in the XML config file are 
    /// related to the box module`s default directory. Please see 
    /// <see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Box.Identifier"/>
    /// </para>
    /// </remarks>
    /// <seealso cref="T:Ferda.Modules.Boxes.Serializer.Reader"/>
    /// <seealso cref="T:Ferda.Modules.Boxes.BoxInfo"/>
    /// <seealso cref="T:Ferda.Modules.Boxes.IBoxInfo"/>
    /// <seealso cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Helper"/>
    [Serializable]
    [XmlRootAttribute("Box", Namespace = "http://ferda.is-a-geek.net", IsNullable = false)]
    public class Box
    {
        /// <summary>
        /// Box`s identifier it has to be uniquie i.e. each box module
        /// has to have unique identifier.
        /// </summary>
        /// <remarks>
        /// <b>Box Module`s default directory</b>
        /// <para>
        /// If you are using <see cref="T:Ferda.Modules.Boxes.BoxInfo"/>
        /// (recommended) please note that there is another meaning of the 
        /// identifier. Please see 
        /// <see cref="P:Ferda.Modules.Boxes.BoxInfo.configFilesDirectoryPath"/>.
        /// </para>
        /// </remarks>
        public string Identifier;

        /// <summary>
        /// Path to box`s icon design i.e. the "ico" file.
        /// <see cref="M:Ferda.Modules.BoxModuleFactoryCreatorI.getIcon(Ice.Current)">
        /// Box finally use Byte[]</see>.
        /// </summary>
        /// <remarks>
        /// For further information about relative pathes please see remars in 
        /// <see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Box"/>.
        /// </remarks>
        public string IconPath;

        /// <summary>
        /// Path to the <see href="http://www.w3.org/tr/2000/cr-svg-20001102/index.html">
        /// Scalable Vector Graphics (SVG)</see> design file.
        /// <see cref="M:Ferda.Modules.BoxModuleFactoryCreatorI.getDesign(Ice.Current)">
        /// Box finally use svg file as string</see>.
        /// </summary>
        /// <remarks>
        /// For further information about relative pathes please see remars in 
        /// <see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Box"/>.
        /// </remarks>
        public string DesignPath;

        /// <summary>
        /// Names of categories where the box belongs to.
        /// </summary>
        /// <remarks>
        /// Each box should belong to at least one category.
        /// </remarks>
        [XmlArray]
        public string[] Categories;

        /// <summary>
        /// Actions of box.
        /// </summary>
        [XmlArray]
        public Action[] Actions;

        /// <summary>
        /// Box`s sockets.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Properties are included i.e. each property has to have
        /// corresponding socket 
        /// (<see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Box.Socket">Socket</see>.<see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Box.Socket.Name">Name</see> == <see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Box.Property">Socket</see>.<see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Box.Property.Name">Name</see>) 
        /// because each property should be adjustable by its socket.
        /// </para>
        /// <para>
        /// There can be sockets which are not properties i.e.
        /// some values in the box module can not be adjustble by
        /// properties but only by sockets.
        /// </para>
        /// <para>
        /// Sockets which should be defautly used as properties has 
        /// to have corresponding property definition i.e.
        /// (<see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Box.Socket">Socket</see>.<see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Box.Socket.Name">Name</see> == <see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Box.Property">Socket</see>.<see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Box.Property.Name">Name</see>)
        /// </para>
        /// </remarks>
        [XmlArray]
        public Socket[] Sockets;

        /// <summary>
        /// Box`s properties.
        /// </summary>
        /// <remarks>
        /// <para>
        /// All defined properties has to have corresponding socket 
        /// (<see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Box.Socket">Socket</see>.<see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Box.Socket.Name">Name</see> == <see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Box.Property">Socket</see>.<see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Box.Property.Name">Name</see>).
        /// </para>
        /// <para>
        /// All defined properties (i.e. sockets) are used as properties
        /// by default.
        /// </para>
        /// </remarks>
        [XmlArray]
        public Property[] Properties;

        /// <summary>
        /// Box`s modules asking for creation.
        /// </summary>
        /// <remarks>
        /// Each box module can advise creation of some new box modules.
        /// </remarks>
        [XmlArray]
        public ModulesAskingForCreation[] ModulesAskingForCreationSeq;
    }

    /// <summary>
    /// Modules asking for creation.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Each box module can advise creation of some new box modules.
    /// </para>
    /// <para>
    /// Please note that Lot of fields of 
    /// <see cref="T:Ferda.Modules.ModulesAskingForCreation"/>
    /// depends on localization and runtime data.
    /// </para>
    /// </remarks>
    [Serializable]
    public class ModulesAskingForCreation
    {
        /// <summary>
        /// Identifier. Used only for joining with 
        /// corresponding record in localization config file.
        /// </summary>
        public string Name;
    }

    /// <summary>
    /// <para>
    /// Box`s sockets.
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// Properties are included i.e. each property has to have corresponding socket 
    /// (<see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Box.Socket">Socket</see>.<see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Box.Socket.Name">Name</see> == <see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Box.Property">Socket</see>.<see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Box.Property.Name">Name</see>)
    /// because each property should be adjustable by its socket.
    /// </para>
    /// <para>
    /// There can be sockets which are not properties i.e.
    /// some values in the box module can not be adjustble by
    /// properties but only by sockets.
    /// </para>
    /// <para>
    /// Sockets which should be defautly used as properties has 
    /// to have corresponding property definition i.e.
    /// (<see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Box.Socket">Socket</see>.<see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Box.Socket.Name">Name</see> == <see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Box.Property">Socket</see>.<see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Box.Property.Name">Name</see>).
    /// </para>
    /// </remarks>
    /// <seealso cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Property"/>.
    [Serializable]
    public class Socket
    {
        /// <summary>
        /// Name of the socket.
        /// </summary>
        /// <seealso cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Property.Name"/>
        public string Name;

        /// <summary>
        /// Path to the <see href="http://www.w3.org/tr/2000/cr-svg-20001102/index.html">
        /// Scalable Vector Graphics (SVG)</see> design file.
        /// <see cref="T:Ferda.Modules.SocketInfo"/> finally use svg 
        /// file as <see cref="T:System.String"/>
        /// </summary>
        /// <remarks>
        /// For further information about relative pathes please see remars in 
        /// <see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Box"/>.
        /// </remarks>
        public string DesignPath;

        /// <summary>
        /// Defines acceptable types of box(es), which can be connected to this socket.
        /// </summary>
        [XmlArray]
        public BoxType[] SocketTypes;

        /// <summary>
        /// <see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Property.Name">
        /// Names of properties</see> that are set by the socket automatically.
        /// This is useful iff some property`s value depends on value of the
        /// socket.
        /// </summary>
        [XmlArray]
        public string[] SettingProperties;

        /// <summary>
        /// <c>true</c> if more than one box can be connected in this socket;
        /// otherwise, <c>false</c>.
        /// </summary>
        public bool MoreThanOne;
    }

    /// <summary>
    /// Box`s properties.
    /// </summary>
    /// <remarks>
    /// <para>
    /// All defined properties has to have corresponding socket 
    /// (<see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Box.Socket">Socket</see>.<see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Box.Socket.Name">Name</see> == <see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Box.Property">Socket</see>.<see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Box.Property.Name">Name</see>).
    /// </para>
    /// <para>
    /// All defined properties (i.e. sockets) are used as properties
    /// by default.
    /// </para>
    /// </remarks>
    /// <seealso cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Socket"/>.
    [Serializable]
    public class Property
    {
        /// <summary>
        /// Name of the property.
        /// </summary>
        /// <remarks>
        /// The name of the property has to be equal to corresponding socket`s name.
        /// </remarks>
        /// <seealso cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Socket.Name"/>
        public string Name;

        /// <summary>
        /// Ice id of type of the <see cref="T:Ferda.Modules.PropertyValue"/> class.
        /// </summary>
        /// <remarks>
        /// Possible values:
        /// <list type="bullet">
        /// <item>
        /// <term>::Ferda::Modules::BoolT</term>
        /// <description>For boolean properties</description>
        /// </item>
        /// <item>
        /// <term>::Ferda::Modules::ShortT</term>
        /// <description>For short integer properties</description>
        /// </item>
        /// <item>
        /// <term>::Ferda::Modules::IntT</term>
        /// <description>For integer properties</description>
        /// </item>
        /// <item>
        /// <term>::Ferda::Modules::LongT</term>
        /// <description>For long integer properties</description>
        /// </item>
        /// <item>
        /// <term>::Ferda::Modules::FloatT</term>
        /// <description>For single precision (float) properties</description>
        /// </item>
        /// <item>
        /// <term>::Ferda::Modules::DoubleT</term>
        /// <description>For double precision properties</description>
        /// </item>
        /// <item>
        /// <term>::Ferda::Modules::StringT</term>
        /// <description>For string properties</description>
        /// </item>
        /// <item>
        /// <term>::Ferda::Modules::DateT</term>
        /// <description>For date properties</description>
        /// </item>
        /// <item>
        /// <term>::Ferda::Modules::DateTimeT</term>
        /// <description>For date time properties</description>
        /// </item>
        /// <item>
        /// <term>::Ferda::Modules::TimeT</term>
        /// <description>For time properties</description>
        /// </item>
        /// <item>
        /// <term>::Ferda::Modules::StringSeqT</term>
        /// <description>For string array properties</description>
        /// </item>
        /// <item>
        /// <term>::Ferda::Modules::CategoriesT</term>
        /// <description>Fort attribute i.e. set of categories properties</description>
        /// </item>        
        /// <item>
        /// <term>...</term>
        /// <description>See BasicPropertyTypes.ice or OtherPropertyTypes.ice.</description>
        /// </item>        
        /// </list>
        /// </remarks>
        public string TypeClassIceId;

        /// <summary>
        /// Name of the category, where the property belongs to.
        /// </summary>
        /// <remarks>
        /// Each property belongs to 0 or 1 category. This is an
        /// identifier (unique name) of category of properties.
        /// </remarks>
        public string CategoryName;

        /// <summary>
        /// Array of selectbox options.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Please use this options if the property accepts values
        /// only from (fixed) set of options.
        /// </para>
        /// <para>
        /// This is usefull if possible values of the property are from 
        /// some (user defined) <see cref="T:System.Enum"/>. In this case
        /// the data type of this property value is string 
        /// (<c>::Ferda::Modules::StringT</c>) and the property has 
        /// defined options.
        /// </para>
        /// </remarks>
        [XmlArray]
        public SelectOption[] SelectOptions;

        /// <summary>
        /// <c>true</c> iff the property is visible for user 
        /// (in front-end); otherwise, <c>false</c>.
        /// </summary>
        public bool Visible;

        /// <summary>
        /// <c>true</c> iff user may not edit value of this property 
        /// (e. g. value is dynamically generated); otherwise, <c>false</c>.
        /// </summary>
        public bool ReadOnly;

        /// <summary>
        /// Specifies default value of the property.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property is not in <see cref="T:Ferda.Modules.PropertyInfo"/>.
        /// During box module`s initialization is this string value converted to
        /// <see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Property.TypeClassIceId">
        /// specified data type</see> and stored in the 
        /// <see cref="T:Ferda.Modules.PropertyValue"/>.
        /// </para>
        /// <para>
        /// If property data type is <c>::Ferda::Modules::StringSeqT</c>
        /// the default value is parsed as CSV (semicolon as separator) string 
        /// into array of strings.
        /// </para>
        /// </remarks>
        public string Default;

        /// <summary>
        /// Array of numerical restrictions.
        /// </summary>
        [XmlArray]
        public Restriction[] NumericalRestrictions;

        /// <summary>
        /// Regular expression restiction for value of the property.
        /// </summary>
        public string Regexp;

        /// <summary>
        /// Identifier of SettingModule which can (or has to) be used 
        /// for setting this property. When it is set to "",
        /// no Setting Module will be used.
        /// </summary>
        /// <seealso cref="T:Ferda.Modules.SettingModule"/>
        /// <seealso cref="M:Ferda.Modules.SettingModule.getIdentifier(Ice.Current)"/>
        public string SettingModuleIdentifier;
    }

    /// <summary>
    /// Restriction for value of the 
    /// <see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Property"/>.
    /// </summary>
    /// <see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Restriction.Floating"/>
    /// <remarks>
    /// Please note that only one restriction is used i.e. in one 
    /// <c>Restriction</c> element you can use only either 
    /// <see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Restriction.Integral"/> or 
    /// <see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Restriction.Floating"/> 
    /// restriction. Precisely if 
    /// <see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Restriction.Floating"/> 
    /// value is equal to "0" (or empty) than the 
    /// <see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Restriction.Integral"/> 
    /// value is used for the restriction (if it is also empty than "0" is used); otherwise 
    /// <see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Restriction.Floating"/> 
    /// value is used.
    /// </remarks>
    [Serializable]
    public class Restriction
    {
        /// <summary>
        /// Restriction value for integral numbers.
        /// </summary>
        /// <remarks>
        /// Please see remarks for 
        /// <see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Restriction"/>.
        /// </remarks>
        /// <seealso cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Restriction"/>
        public long Integral;

        /// <summary>
        /// Restriction value for floating numbers.
        /// </summary>
        /// <remarks>
        /// Please see remarks for 
        /// <see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Restriction"/>.
        /// </remarks>
        /// <seealso cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Restriction"/>
        public double Floating;

        /// <summary>
        /// <c>true</c> iff restriction value 
        /// (<see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Restriction.Integral"/>
        /// or <see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Restriction.Floating"/>)
        /// is used as minimum; othervise, the restriction value is used as maximum.
        /// </summary>
        public bool Min;

        /// <summary>
        /// <c>true</c> iff restriction value 
        /// is not included in possible values.
        /// </summary>
        public bool Including;
    }

    /// <summary>
    /// Option of <c>selectbox</c> (if you like <c>combobox</c>).
    /// </summary>
    /// <remarks>
    /// <para>
    /// Please use this options if the property accepts values
    /// only from (fixed) set of options.
    /// </para>
    /// <para>
    /// This is usefull if possible values of the property are from 
    /// some (user defined) <see cref="T:System.Enum"/>. In this case
    /// the data type of this property value is string 
    /// (<c>::Ferda::Modules::StringT</c>) and the property has 
    /// defined options.
    /// </para>
    /// </remarks>
    /// <seealso cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Property.SelectOptions"/>
    [Serializable]
    public class SelectOption
    {
        /// <summary>
        /// Name of the option.
        /// </summary>
        public string Name;

        /// <summary>
        /// Array of <see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Property.Name">
        /// names of properties</see> which will be disabled by selecting this option.
        /// </summary>
        [XmlArray]
        public string[] DisableProperties;
    }

    /// <summary>
    /// Defines acceptable types of box(es), which can be connected to the 
    /// <see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Socket"/>.
    /// </summary>
    [Serializable]
    public class BoxType
    {
        /// <summary>
        /// Ice id of the functions required on box, which can be connected to the 
        /// <see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Socket"/>.
        /// </summary>
        /// <remarks>
        /// There is shown possible values. At first there are shown ice ids of 
        /// functions of PropertyBoxes i.e. boxes which can be used for 
        /// seting properties (see that each property always the socket 
        /// at the same time). Next there is an example of ice id of 
        /// specified functions object.
        /// <list type="bullet">
        /// <item>
        /// <term>::Ferda::Modules::BoolTInterface</term>
        /// <description>For boolean properties</description>
        /// </item>
        /// <item>
        /// <term>::Ferda::Modules::ShortTInterface</term>
        /// <description>For short integer properties</description>
        /// </item>
        /// <item>
        /// <term>::Ferda::Modules::IntTInterface</term>
        /// <description>For integer properties</description>
        /// </item>
        /// <item>
        /// <term>::Ferda::Modules::LongTInterface</term>
        /// <description>For long integer properties</description>
        /// </item>
        /// <item>
        /// <term>::Ferda::Modules::FloatTInterface</term>
        /// <description>For single precision (float) properties</description>
        /// </item>
        /// <item>
        /// <term>::Ferda::Modules::DoubleTInterface</term>
        /// <description>For double precision properties</description>
        /// </item>
        /// <item>
        /// <term>::Ferda::Modules::StringTInterface</term>
        /// <description>For string properties</description>
        /// </item>
        /// <item>
        /// <term>::Ferda::Modules::DateTInterface</term>
        /// <description>For date properties</description>
        /// </item>
        /// <item>
        /// <term>::Ferda::Modules::DateTimeTInterface</term>
        /// <description>For date time properties</description>
        /// </item>
        /// <item>
        /// <term>::Ferda::Modules::TimeTInterface</term>
        /// <description>For time properties</description>
        /// </item>
        /// <item>
        /// <term>::Ferda::Modules::StringSeqTInterface</term>
        /// <description>For string array properties</description>
        /// </item>
        /// <item>
        /// <term>::Ferda::Modules::CategoriesTInterface</term>
        /// <description>Fort attribute i.e. set of categories properties</description>
        /// </item>        
        /// <item>
        /// <term>...</term>
        /// <description>Ice ids of concrete required functions. Please see an example</description>
        /// </item>        
        /// </list>
        /// </remarks>
        /// <example>
        /// <para>This is an example of slice design. As you can see <c>MyBoxModule</c> 
        /// has one interface for implementation named <c>MyBoxModuleFunctions</c>.</para>
        /// <code>
        /// module MyBoxModule
        /// {
        /// 	interface MyBoxModuleFunctions
        /// 	{
        ///         nonmutating string HelloWorld();
        /// 		/* ... */
        /// 	};
        /// };
        /// </code>
        /// <para>
        /// This is the ice id for functions object implementing
        /// the interface in the slice design above.
        /// </para>
        /// <code>
        /// ::MyBoxModule::MyBoxModuleFunctions
        /// </code>
        /// </example>
        public string FunctionIceId;

        /// <summary>
        /// Array of <see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.NeededSocket">sockets</see>
        /// which are required on the box, which can be connected to the 
        /// <see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Socket"/>.
        /// </summary>
        [XmlArray]
        public NeededSocket[] NeededSockets;
    }

    /// <summary>
    /// Action of the box.
    /// </summary>
    /// <remarks>
    /// Action is function of the box module, which is not (typically)
    /// in its slice design i.e. it is not used by another box modules.
    /// This functions are called directly by user from front-end.
    /// </remarks>
    /// <example>
    /// Action function can be e.g. "Run task" or "Generate results", ...
    /// </example>
    [Serializable]
    public class Action
    {
        /// <summary>
        /// Name of the action.
        /// </summary>
        public string Name;

        /// <summary>
        /// Path to box`s icon design i.e. the "ico" file.
        /// <see cref="T:Ferda.Modules.ActionInfo"/> finally use Byte[].
        /// </summary>
        /// <remarks>
        /// For further information about relative pathes please see remars in 
        /// <see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Box"/>.
        /// </remarks>
        public string IconPath;

        /// <summary>
        /// Array of conditions on the box, where at least one has to be satisfied
        /// before action is executed. Condition is array of 
        /// <see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Box.Socket.Name">
        /// names of sockets</see> which has to be occupated.
        /// </summary>
        [XmlArray]
        public NeededConnectedSocketsOption[] NeededConnectedSocketsOptions;
    }

    /// <summary>
    /// Defines the condition on occupation of the 
    /// <see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Box.Socket">sockets</see> as array of 
    /// <see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Box.Socket.Name">socket`s names</see>
    /// which has to be occupated.
    /// </summary>
    [Serializable]
    public class NeededConnectedSocketsOption
    {
        /// <summary>
        /// <see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Box.Socket.Name">Names</see>
        /// of the <see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Box.Socket">sockets</see>
        /// which has to be occupated.
        /// </summary>
        [XmlArray]
        public string[] NeededConnectedSockets;
    }

    /// <summary>
    /// Defines requirements on socket type i.e. socket of specified name
    /// has to satisfy specified condition on required functions i.e. implemeted
    /// interfaces (if you like slice design inteface).
    /// </summary>
    [Serializable]
    public class NeededSocket
    {
        /// <summary>
        /// <see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Box.Socket.Name">Name</see>
        /// of the socket.
        /// </summary>
        public string SocketName;

        /// <summary>
        /// Ice id of required functions of box(es), which can be connected 
        /// in the socket. Please see 
        /// <see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.BoxType.FunctionIceId"/>
        /// for futher information about ice ids of functions.
        /// </summary>
        public string FunctionIceId;
    }
}