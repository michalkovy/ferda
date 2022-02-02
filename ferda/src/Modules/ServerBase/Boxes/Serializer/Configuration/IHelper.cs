﻿// IHelper.cs - Helper with XML configuration files serialization
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

namespace Ferda.Modules.Boxes.Serializer.Configuration
{
    /// <summary>
    /// This interface provides some functions and properties which
    /// helps <see cref="T:Ferda.Modules.Boxes.BoxInfo"/> to implement
    /// it functionality.
    /// </summary>
    /// <remarks>
    /// By implementation this interface you can avoid
    /// creating of <see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Box">
    /// configuration file</see>.
    /// </remarks>
    public interface IHelper
    {
        /// <summary>
        /// Gets the action`s needed connected sockets.
        /// </summary>
        /// <value>The action`s needed connected sockets.
        /// <para><c>Key</c> is the action`s name.</para>
        /// <para><c>Value</c> is the array of conditions on needed connected 
        /// sockets. Box has to satisfy at least one of the conditions before
        /// the action can be executed. The condition is array of socket`s names
        /// in which other box(es) has to be connected.</para>
        /// </value>
        Dictionary<string, string[][]> ActionNeededConnectedSockets { get; }

        /// <summary>
        /// Gets the actions.
        /// </summary>
        /// <value>The actions.
        /// <para><c>Key</c> is the action`s name.</para>
        /// <para><c>Value</c> is the <see cref="Ferda.Modules.Boxes.Serializer.Configuration.Action"/>.</para>
        /// </value>
        Dictionary<string, Action> Actions { get; }

        /// <summary>
        /// Gets the categories i.e. names of categories where the box module belongs to.
        /// </summary>
        /// <value>The categories.</value>
        string[] Categories { get; }

        /// <summary>
        /// Gets the path to the <see href="http://www.w3.org/tr/2000/cr-svg-20001102/index.html">
        /// Scalable Vector Graphics (SVG)</see> design file of the box.
        /// </summary>
        /// <remarks>
        /// For further information about relative pathes please see remars in 
        /// <see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Box"/>.
        /// </remarks>
        /// <value>The path to the SVG design file.</value>
        string DesignPath { get; }

        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns><see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Property"/> if 
        /// exists an element of specified <c>propertyName</c>; otherwise, throws 
        /// <see cref="T:Ferda.Modules.NameNotExistError"/>.
        /// </returns>
        /// <exception cref="T:Ferda.Modules.NameNotExistError">
        /// There is no property with specified name (<c>propertyName</c>) in the box.
        /// </exception>
        Property GetProperty(string propertyName);

        /// <summary>
        /// Gets the restrictions on possible values of the property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        /// List of <see cref="T:Ferda.Modules.Restriction">Restrictions</see>.
        /// </returns>
        /// <exception cref="T:Ferda.Modules.NameNotExistError">
        /// There is no property with specified name (<c>propertyName</c>) in the box.
        /// </exception>
        List<Modules.Restriction> GetPropertyRestrictions(string propertyName);

        /// <summary>
        /// Gets the socket.
        /// </summary>
        /// <param name="socketName">Name of the socket.</param>
        /// <returns><see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Socket"/> if 
        /// exists an element of specified <c>socketName</c>; otherwise, throws 
        /// <see cref="T:Ferda.Modules.NameNotExistError"/>.
        /// </returns>
        /// <exception cref="T:Ferda.Modules.NameNotExistError">There is no socket with 
        /// the specified name (<c>socketName</c>) in the box.</exception>
        Socket GetSocket(string socketName);

        /// <summary>
        /// Gets the socket`s types.
        /// </summary>
        /// <param name="socketName">Name of the socket.</param>
        /// <returns>Array of <see cref="T:Ferda.Modules.BoxType"/>.</returns>
        /// <exception cref="T:Ferda.Modules.NameNotExistError">There is no socket with 
        /// specified <c>socketName</c> in the box.</exception>
        Modules.BoxType[] GetSocketTypes(string socketName);

        /// <summary>
        /// Gets the path to box`s icon design i.e. the "ico" file.
        /// </summary>
        /// <remarks>
        /// For further information about relative pathes please see remars in 
        /// <see cref="T:Ferda.Modules.Boxes.Serializer.Configuration.Box"/>.
        /// </remarks>
        /// <value>The path to the icon.</value>
        string IconPath { get; }

        /// <summary>
        /// Gets the box`s identifier.
        /// </summary>
        /// <value>The box`s identifier.</value>
        string Identifier { get; }

        /// <summary>
        /// Gets names the modules asking for creation.
        /// </summary>
        /// <value>Names of the modules asking for creation.</value>
        /// <remarks>
        /// <see cref="T:Ferda.Modules.ModulesAskingForCreation"/> structures are empty because all the
        /// memebers in this structure depends on localization or dynamic (or runtime) factors.
        /// </remarks>
        List<string> ModulesAskingForCreation { get; }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <value>The properties.
        /// <para><c>Key</c> is name of the property.</para>
        /// <para><c>Value</c> is the <see cref="Ferda.Modules.Boxes.Serializer.Configuration.Property"/>.</para>
        /// </value>
        SortedList<string, Property> Properties { get; }

        /// <summary>
        /// Gets the sockets.
        /// </summary>
        /// <value>The sockets.
        /// <para><c>Key</c> is the socket`s name.</para>
        /// <para><c>Value</c> is the <see cref="Ferda.Modules.Boxes.Serializer.Configuration.Socket"/>.</para>
        /// </value>
        Dictionary<string, Socket> Sockets { get; }
    }
}