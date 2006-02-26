using System;
namespace Ferda.Modules.Boxes.Serializer.Localization
{
    /// <summary>
    /// This interface provides some functions and properties which
    /// helps <see cref="T:Ferda.Modules.Boxes.BoxInfo"/> to implement
    /// it functionality.
    /// </summary>
    /// <remarks>
    /// By implementation this interface you can avoid
    /// creating of <see cref="T:Ferda.Modules.Boxes.Serializer.Localization.BoxLocalization">
    /// localization file</see>.
    /// </remarks>
    public interface IHelper
    {
        /// <summary>
        /// Gets the actions of the box.
        /// </summary>
        /// <value>The actions of the box.</value>
        System.Collections.Generic.Dictionary<string, Ferda.Modules.Boxes.Serializer.Localization.Action> Actions { get; }

        /// <summary>
        /// Gets (localized) names of the categories of the box.
        /// </summary>
        /// <value>The box`s categories.
        /// <para><c>Key</c> is name the category where the box belongs to.</para>
        /// <para><c>Value</c> is localized name of the category.</para>
        /// </value>
        System.Collections.Generic.Dictionary<string, string> Categories { get; }

        /// <summary>
        /// Gets the localizec dynamic help items of the box.
        /// </summary>
        /// <value>The dynamic help items of the box.</value>
        Ferda.Modules.DynamicHelpItem[] DynamicHelpItems { get; }

        /// <summary>
        /// Gets the the selectbox`s option of specified name of the property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="optionName">Name of the option.</param>
        /// <param name="fallOnError">If set to <c>true</c> and there is no 
        /// localization of specified option in the property <see cref="T:System.Exception"/> is thrown.</param>
        /// <returns>Label and short label of the specified option of the property.</returns>
        Ferda.Modules.Boxes.Serializer.Localization.SelectOption GetSelectBoxOption(string propertyName, string optionName, bool fallOnError);

        /// <summary>
        /// Gets the localization of the socket of the specified name.
        /// </summary>
        /// <param name="socketName">Name of the socket.</param>
        /// <returns><see cref="T:Ferda.Modules.Boxes.Serializer.Localization.Socket"/> if 
        /// exists an element of specified <c>socketName</c>; otherwise, throws
        /// <see cref="T:Ferda.Modules.NameNotExistError"/>.
        /// </returns>
        /// <exception cref="T:Ferda.Modules.NameNotExistError">There is no socket with 
        /// specified <c>socketName</c> in the box.</exception>
        Ferda.Modules.Boxes.Serializer.Localization.Socket GetSocket(string socketName);

        /// <summary>
        /// Gets the help files.
        /// </summary>
        /// <value>The help files.</value>
        Ferda.Modules.HelpFileInfo[] HelpFiles { get; }

        /// <summary>
        /// Gets the help files paths.
        /// </summary>
        /// <remarks>Path is relative to the directory, where the config file is stored.</remarks>
        /// <value>The help files paths.
        /// <para><c>Key</c> is localeId + helpFileIdentifier.</para>
        /// <para><c>Value</c> is path to the help file.</para>
        /// </value>
        System.Collections.Generic.Dictionary<string, string> HelpFilesPaths { get; }

        /// <summary>
        /// Gets the box`s hint i.e. short tip.
        /// </summary>
        /// <value>The box`s hint.</value>
        string Hint { get; }

        /// <summary>
        /// Gets the box`s identifier.
        /// </summary>
        /// <value>The box`s identifier.</value>
        string Identifier { get; }

        /// <summary>
        /// Gets the box`s label.
        /// </summary>
        /// <value>The box`s label.</value>
        string Label { get; }

        /// <summary>
        /// Gets the locale id.
        /// </summary>
        /// <value>
        /// The culture names follow the RFC 1766 standard in the format 
        /// "&lt;languagecode2&gt;-&lt;country/regioncode2&gt;", where &lt;languagecode2&gt;
        /// is a lowercase two-letter code derived from ISO 639-1 and &lt;country/regioncode2&gt;
        /// is an uppercase two-letter code derived from ISO 3166. For example, U.S. English 
        /// is "en-US". In cases where a two-letter language code is not available, 
        /// the three-letter code derived from ISO 639-2 is used; for example, 
        /// the three-letter code "div" is used for cultures that use the Dhivehi language. 
        /// Some culture names have suffixes that specify the script; for example, 
        /// "-Cyrl" specifies the Cyrillic script, "-Latn" specifies the Latin script.
        /// </value>
        string LocaleId { get; }

        /// <summary>
        /// Gets the modules asking for creation.
        /// </summary>
        /// <value>The modules asking for creation.
        /// <para><c>Key</c> is name of modules asking for creation.</para>
        /// <para><c>Value</c> is the array of <see cref="T:Ferda.Modules.DynamicHelpItem"/> i.e. dynamic help items. (label, hint and dynamic help items are specified)</para>
        /// </value>
        System.Collections.Generic.Dictionary<string, Ferda.Modules.ModulesAskingForCreation> ModulesAskingForCreation { get; }

        /// <summary>
        /// Gets (localized) names of the categories of the property.
        /// </summary>
        /// <value>The property categories.
        /// <para><c>Key</c> is name the category where the property belongs to.</para>
        /// <para><c>Value</c> is localized name of the category.</para>
        /// </value>
        System.Collections.Generic.Dictionary<string, string> PropertyCategories { get; }

        /// <summary>
        /// Gets the localization of the sockets.
        /// </summary>
        /// <value>
        /// The sockets.
        /// <para><c>Key</c> is socket`s name.</para>
        /// 	<para><c>Value</c> is socket`s localization.</para>
        /// </value>
        /// <remarks>Localization of properties is included.</remarks>
        System.Collections.Generic.Dictionary<string, Ferda.Modules.Boxes.Serializer.Localization.Socket> Sockets { get; }

        /// <summary>
        /// Tries to the get specified phrase (<c>phraseIdentifier</c>).
        /// </summary>
        /// <param name="phraseIdentifier">The phrase`s identifier.</param>
        /// <param name="phraseLocalizedText">The phrase`s localized text.</param>
        /// <returns>
        /// <c>true</c> if localization of specified phrase (<c>phraseIdentifier</c>)
        /// exists; otherwise, <c>false</c>.
        /// </returns>
        bool TryGetPhrase(string phraseIdentifier, out string phraseLocalizedText);
    }
}
