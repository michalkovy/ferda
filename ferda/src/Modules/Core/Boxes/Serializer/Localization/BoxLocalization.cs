using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Ferda.Modules.Boxes.Serializer.Localization
{
    /// <summary>
    /// <para>
    /// Class for <see cref="T:Ferda.Modules.Boxes.Serializer.Reader">deserealization</see>
    /// of box`s XML localization file.
    /// </para>
    /// <para>
    /// Mainly provides localization for corresponding classes in 
    /// <see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Box"/>.
    /// Lot of memebers has field named "Name" it is uniquie identifier
    /// which among other serve to joining corresponding records in 
    /// <see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Box">
    /// XML localization file</see>.
    /// </para>
    /// <para>
    /// For further information about Ferda`s way of work localization files please see
    /// <see cref="T:Ferda.Modules.Boxes.BoxInfo"/> and <see cref="T:Ferda.Modules.Boxes.IBoxInfo"/>.
    /// </para>
    /// <para>
    /// For further information please see other documentation e. g. documentation of 
    /// XML config files, XSD schema or slice design of Ferda.Modules.
    /// </para>
    /// </summary>
    /// <seealso cref="T:Ferda.Modules.Boxes.Serializer.Reader"/>
    /// <seealso cref="T:Ferda.Modules.Boxes.BoxInfo"/>
    /// <seealso cref="T:Ferda.Modules.Boxes.Serializer.Localization.Helper"/>
    [Serializable]
    [XmlRootAttribute("BoxLocalization", Namespace = "http://ferda.is-a-geek.net", IsNullable = false)]
	public class BoxLocalization
	{
        /// <summary>
        /// Box`s identifier.
        /// </summary>
        /// <remarks>
        /// Corresponds to <see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Box.Identifier"/>.
        /// </remarks>
		public string Identifier;

        /// <summary>
        /// Label i.e. localized name.
        /// </summary>
        public string Label;

        /// <summary>
        /// Hint i.e. short tip.
        /// </summary>
        public string Hint;

        /// <summary>
        /// Localization of <see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Box.Categories"/>.
        /// </summary>
		[XmlArray]
		public Category[] Categories;

        /// <summary>
        /// Dynamic help for the box. Each localization has its own
        /// dynamic help items because there can be different 
        /// <see cref="F:Ferda.Modules.Boxes.Serializer.Localization.BoxLocalization.HelpFiles">
        /// help (if you like documentation) files</see> in each 
        /// language i.e. localization.
        /// </summary>
		[XmlArray]
		public DynamicHelpItem[] DynamicHelpItems;

        /// <summary>
        /// Localization of <see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Box.Actions"/>.
        /// </summary>
		[XmlArray]
		public Action[] Actions;

        /// <summary>
        /// Localization of <see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Box.Sockets"/>
        /// (i.e. also localization of <see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Box.Properties"/>).
        /// </summary>
		[XmlArray]
		public Socket[] Sockets;

        /// <summary>
        /// Localization of <see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Property.CategoryName">
        /// categories, where <see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Box">box`s</see>
        /// <see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Property">properties</see> belongs to</see>.
        /// </summary>
		[XmlArray]
		public PropertyCategory[] PropertyCategories;

        /// <summary>
        /// Localization of <see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Box.ModulesAskingForCreationSeq"/>.
        /// </summary>
		[XmlArray]
		public ModulesAskingForCreation[] ModulesAskingForCreationSeq;

        /// <summary>
        /// Help files.
        /// </summary>
        /// <remarks>
        /// Help files depends on localization because there can be 
        /// different sets of help (if you like documentation) files 
        /// for each language.
        /// </remarks>
		[XmlArray]
		public HelpFile[] HelpFiles;

        /// <summary>
        /// Represents dictionary entries i.e. identifier of the phrase and 
        /// localized text of the phrase.
        /// </summary>
        /// <remarks>
        /// Usefull for localization of some prhases used e.g. in actions,
        /// error reprotings, outputs, ...
        /// </remarks>
        [XmlArray]
        public Phrase[] Phrases;
	}

    /// <summary>
    /// Represents dictionary entry i.e. identifier of the phrase and 
    /// localized text of the phrase.
    /// </summary>
    /// <remarks>
    /// Usefull for localization of some prhases used e.g. in actions,
    /// error reprotings, outputs, ...
    /// </remarks>
    [Serializable]
    public class Phrase
    { 
        /// <summary>
        /// Identifier of the phrase.
        /// </summary>
        public string PhraseIdentifier;

        /// <summary>
        /// Localized text of the phrase.
        /// </summary>
        public string PhraseText;
    }

    /// <summary>
    /// Localization of <see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Property.CategoryName">
    /// categories, where <see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Box">box`s</see>
    /// <see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Property">properties</see> belongs to</see>.
    /// </summary>
	[Serializable]
	public class PropertyCategory
	{
        /// <summary>
        /// Name of the category.
        /// </summary>
        /// <remarks>
        /// Corresponds to <see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Property.CategoryName"/>.
        /// </remarks>
		public string Name;

        /// <summary>
        /// Label i.e. localized name.
        /// </summary>
		public string Label;
	}

	/// <summary>
	/// Help file.
	/// </summary>
    /// <remarks>
    /// Help files depends on localization because there can be 
    /// different sets of help (if you like documentation) files 
    /// for each language.
    /// </remarks>
    [Serializable]
	public class HelpFile
	{
        /// <summary>
        /// Identifier of the help file. 
        /// </summary>
        /// <remarks>
        /// Corresponds to <see cref="F:Ferda.Modules.Boxes.Serializer.Localization.DynamicHelpItem.Identifier"/>.
        /// </remarks>
		public string Identifier;

        /// <summary>
        /// Label of the help file.
        /// </summary>
		public string Label;

        /// <summary>
        /// Version of the help file.
        /// </summary>
		public int Version;
		
        /// <summary>
        /// Path to the help file.
        /// </summary>
        public string Path;
	}

    /// <summary>
    /// Localization of <see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.ModulesAskingForCreation"/>.
    /// </summary>
	[Serializable]
	public class ModulesAskingForCreation
	{
        /// <summary>
        /// Identifier.
        /// </summary>
        /// <remarks>
        /// Corresponds to <see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.ModulesAskingForCreation.Name"/>.
        /// </remarks>
        public string Name;

        /// <summary>
        /// Label i.e. localized name.
        /// </summary>
        public string Label;

        /// <summary>
        /// Hint i.e. short tip.
        /// </summary>
		public string Hint;

        /// <summary>
        /// Array of dynamic help items.
        /// </summary>
		[XmlArray]
		public DynamicHelpItem[] DynamicHelpItems;
	}

    /// <summary>
    /// Localization of item of <see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Box.Categories"/>.
    /// </summary>
    [Serializable]
	public class Category
	{
        /// <summary>
        /// Name (identifier).
        /// </summary>
        /// <remarks>
        /// Corresponds to item of <see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Box.Categories"/>.
        /// </remarks>
        public string Name;

        /// <summary>
        /// Label i.e. localized name.
        /// </summary>
        public string Label;
	}

    /// <summary>
    /// Dynamic help for the box. Each localization has its own
    /// dynamic help items because there can be different 
    /// <see cref="F:Ferda.Modules.Boxes.Serializer.Localization.BoxLocalization.HelpFiles">
    /// help (if you like documentation) files</see> in each 
    /// language i.e. localization.
    /// </summary>
    [Serializable]
	public class DynamicHelpItem
	{
        /// <summary>
        /// Identifier of the <see cref="F:Ferda.Modules.Boxes.Serializer.Localization.HelpFile.Identifier">help file</see>.
        /// </summary>
		public string Identifier;

        /// <summary>
        /// Label i.e. localized name.
        /// </summary>
        public string Label;

        /// <summary>
        /// Url path into the <see cref="T:Ferda.Modules.Boxes.Serializer.Localization.HelpFile"/>.
        /// </summary>
		public string Url;
	}

    /// <summary>
    /// Localization of <see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Socket"/>
    /// (i.e. also localization of <see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Property"/>).
    /// </summary>
	[Serializable]
	public class Socket
	{
        /// <summary>
        /// Name (identifier).
        /// </summary>
        /// <remarks>
        /// Corresponds to item of <see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Socket.Name"/>.
        /// </remarks>
        public string Name;

        /// <summary>
        /// Label i.e. localized name.
        /// </summary>
        public string Label;

        /// <summary>
        /// Hint i.e. short tip
        /// </summary>
		public string Hint;

        /// <summary>
        /// Localization of <see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Property.SelectOptions"/>.
        /// </summary>
		[XmlArray]
		public SelectOption[] SelectOptions;
	}

    /// <summary>
    /// Localization of <see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.SelectOption"/>.
    /// </summary>
	[Serializable]
	public class SelectOption
	{
        /// <summary>
        /// Name (identifier).
        /// </summary>
        /// <remarks>
        /// Corresponds to item of <see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.SelectOption.Name"/>.
        /// </remarks>
        public string Name;

        /// <summary>
        /// Label i.e. localized name.
        /// </summary>
        public string Label;

        /// <summary>
        /// Short label i.e. abbreviation of label.
        /// </summary>
		public string ShortLabel;
	}

    /// <summary>
    /// Localization of <see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Action"/>.
    /// </summary>
	[Serializable]
	public class Action
	{
        /// <summary>
        /// Name (identifier).
        /// </summary>
        /// <remarks>
        /// Corresponds to item of <see cref="F:Ferda.Modules.Boxes.Serializer.Configuration.Action.Name"/>.
        /// </remarks>
        public string Name;

        /// <summary>
        /// Label i.e. localized name.
        /// </summary>
        public string Label;

        /// <summary>
        /// Hint i.e. short tip.
        /// </summary>
		public string Hint;
	}
}
