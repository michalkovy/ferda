// IHelper.cs - Helper with XML localization files
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

using System.Collections.Generic;

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
        Dictionary<string, Action> Actions { get; }

        /// <summary>
        /// Gets (localized) names of the categories of the box.
        /// </summary>
        /// <value>The box`s categories.
        /// <para><c>Key</c> is name the category where the box belongs to.</para>
        /// <para><c>Value</c> is localized name of the category.</para>
        /// </value>
        Dictionary<string, string> Categories { get; }

        /// <summary>
        /// Gets the localizec dynamic help items of the box.
        /// </summary>
        /// <value>The dynamic help items of the box.</value>
        Modules.DynamicHelpItem[] DynamicHelpItems { get; }

        /// <summary>
        /// Gets the the selectbox`s option of specified name of the property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="optionName">Name of the option.</param>
        /// <param name="fallOnError">If set to <c>true</c> and there is no 
        /// localization of specified option in the property <see cref="T:System.Exception"/> is thrown.</param>
        /// <returns>Label and short label of the specified option of the property.</returns>
        SelectOption GetSelectBoxOption(string propertyName, string optionName, bool fallOnError);

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
        Socket GetSocket(string socketName);

        /// <summary>
        /// Gets the help files.
        /// </summary>
        /// <value>The help files.</value>
        HelpFileInfo[] HelpFiles { get; }

        /// <summary>
        /// Gets the help files paths.
        /// </summary>
        /// <remarks>Path is relative to the directory, where the config file is stored.</remarks>
        /// <value>The help files paths.
        /// <para><c>Key</c> is localeId + helpFileIdentifier.</para>
        /// <para><c>Value</c> is path to the help file.</para>
        /// </value>
        Dictionary<string, string> HelpFilesPaths { get; }

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
        Dictionary<string, Modules.ModulesAskingForCreation> ModulesAskingForCreation { get; }

        /// <summary>
        /// Gets (localized) names of the categories of the property.
        /// </summary>
        /// <value>The property categories.
        /// <para><c>Key</c> is name the category where the property belongs to.</para>
        /// <para><c>Value</c> is localized name of the category.</para>
        /// </value>
        Dictionary<string, string> PropertyCategories { get; }

        /// <summary>
        /// Gets the localization of the sockets.
        /// </summary>
        /// <value>
        /// The sockets.
        /// <para><c>Key</c> is socket`s name.</para>
        /// 	<para><c>Value</c> is socket`s localization.</para>
        /// </value>
        /// <remarks>Localization of properties is included.</remarks>
        Dictionary<string, Socket> Sockets { get; }

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