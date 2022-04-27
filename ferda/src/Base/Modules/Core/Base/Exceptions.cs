// Exceptions.cs - basic exceptions for Ferda boxes
//
// Authors:
//   Michal Kováè <michal.kovac.develop@centrum.cz>
//   Tomáš Kuchaø <tomas.kuchar@gmail.com>
//
// Copyright (c) 2005 Michal Kováè, Tomáš Kuchaø
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
using System.Diagnostics;

namespace Ferda.Modules
{
    /// <summary>
    /// Provides some functionality which helps you to work with (not only)
    /// Ferda.Modules exceptions.
    /// </summary>
    public static class Exceptions
    {
        /// <summary>
        /// Prepares the exception for further handling.
        /// Writes the <paramref name="boxIdentity"/> and
        /// <paramref name="userMessage"/> to an 
        /// <paramref name="ex"/> exception.
        /// </summary>
        /// <param name="ex">The exception to be prepared</param>
        /// <param name="boxIdentity">Identity of the box</param>
        /// <param name="userMessage">Exception message to the user</param>
        private static void prepare(BoxRuntimeError ex, string boxIdentity, string userMessage)
        {
            Debug.Assert(!String.IsNullOrEmpty(userMessage));
            ex.boxIdentity = boxIdentity;
            ex.userMessage = userMessage;
        }

        /// <summary>
        /// Prepares the exception for further handling.
        /// Writes the <paramref name="boxIdentity"/>,
        /// <paramref name="restrictionType"/> and
        /// <paramref name="userMessage"/> to an 
        /// <paramref name="ex"/> exception.
        /// </summary>
        /// <param name="ex">The exception to be prepared</param>
        /// <param name="boxIdentity">Identity of the box</param>
        /// <param name="userMessage">Exception message to the user</param>
        /// <param name="restrictionType">Type of restriction</param>
        private static void prepare(BadParamsError ex, string boxIdentity, string userMessage,
                                    restrictionTypeEnum restrictionType)
        {
            prepare(ex, boxIdentity, userMessage);
            ex.restrictionType = restrictionType;
        }

        /// <summary>
        /// Gets BadValueError exception.
        /// </summary>
        /// <param name="e">The inner exception.</param>
        /// <param name="boxIdentity">The box identity.</param>
        /// <param name="userMessage">The user message.</param>
        /// <param name="socketsNames">The names of the sockets where are bad values.</param>
        /// <param name="restrictionType">Type of the restriction.</param>
        /// <returns>
        /// The <see cref="Ferda.Modules.BadValueError"/> exception.
        /// </returns>
        public static BadValueError BadValueError(Exception e, string boxIdentity, string userMessage,
                                                  string[] socketsNames, restrictionTypeEnum restrictionType)
        {
            BadValueError ex = new BadValueError(e);
            prepare(ex, boxIdentity, userMessage, restrictionType);
            ex.socketsNames = socketsNames;
            return ex;
        }

        /// <summary>
        /// Gets BadParamsError exception.
        /// </summary>
        /// <param name="e">The inner exception.</param>
        /// <param name="boxIdentity">The box identity.</param>
        /// <param name="userMessage">The user message.</param>
        /// <param name="restrictionType">Type of the restriction.</param>
        /// <returns>
        /// The <see cref="T:Ferda.Modules.BadParamsError"/> exception.
        /// </returns>
        public static BadParamsError BadParamsError(Exception e, string boxIdentity, string userMessage,
                                                    restrictionTypeEnum restrictionType)
        {
            BadParamsError ex = new BadParamsError(e);
            prepare(ex, boxIdentity, userMessage, restrictionType);
            return ex;
        }

        /// <summary>
        /// Gets NoConnectionInSocketError exception.
        /// </summary>
        /// <param name="e">The inner exception.</param>
        /// <param name="boxIdentity">The box identity.</param>
        /// <param name="userMessage">The user message.</param>
        /// <param name="socketsNames">Names of the empty sockets.</param>
        /// <returns>
        /// The <see cref="Ferda.Modules.NoConnectionInSocketError"/> exception.
        /// </returns>
        public static NoConnectionInSocketError NoConnectionInSocketError(Exception e, string boxIdentity, string[] socketsNames)
        {
            Debug.Assert(socketsNames == null || socketsNames.Length == 0);

            NoConnectionInSocketError ex = new NoConnectionInSocketError(e);
            prepare(ex, boxIdentity, "There is no connection in the socket!");
            ex.socketsNames = socketsNames;
            return ex;
        }

        /// <summary>
        /// Gets BoxRuntimeError exception.
        /// </summary>
        /// <param name="e">The inner exception.</param>
        /// <param name="boxIdentity">The box identity.</param>
        /// <param name="userMessage">The user message.</param>
        /// <returns>
        /// The <see cref="Ferda.Modules.BoxRuntimeError"/> exception.
        /// </returns>
        public static BoxRuntimeError BoxRuntimeError(Exception e, string boxIdentity, string userMessage)
        {
            BoxRuntimeError ex = new BoxRuntimeError(e);
            prepare(ex, boxIdentity, userMessage);
            return ex;
        }

        /// <summary>
        /// Gets NameNotExistError exception.
        /// </summary>
        /// <param name="e">The inner exception.</param>
        /// <param name="notExistingName">The not existing name.</param>
        /// <returns>
        /// The <see cref="Ferda.Modules.NameNotExistError"/> exception.
        /// </returns>
        public static NameNotExistError NameNotExistError(Exception e, string notExistingName)
        {
            return new NameNotExistError("NotExistingName: " + notExistingName, e);
        }
    }
}