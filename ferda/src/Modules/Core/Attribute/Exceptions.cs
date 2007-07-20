// Exceptions.cs - exceptions for Ferda attributes
//
// Author: Tomáš Kuchaø <tomas.kuchar@gmail.com>
//
// Copyright (c) 2006 Tomáš Kuchaø
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
using Ferda.Modules;

namespace Ferda.Guha.Attribute
{
    /// <summary>
    /// Provides basic method for instanciate exceptions
    /// joint with attribute.
    /// </summary>
    public class Exceptions
    {
        /// <summary>
        /// Gets the categories in attribute disjunctivity error.
        /// </summary>
        /// <param name="innnerException">The innner exception.</param>
        /// <param name="boxIdentity">The box identity.</param>
        /// <returns></returns>
        public static BadParamsError AttributeCategoriesDisjunctivityError(Exception innnerException, string boxIdentity)
        {
            return Modules.Exceptions.BadParamsError(
                innnerException,
                boxIdentity,
                "Categories in the attribute are not disjunctive.",
                restrictionTypeEnum.AttributeCategoriesDisjunctivityError
                );
        }

        private static string sequenceToString(IEnumerable<string> items, string separator)
        {
            if (items == null)
                return String.Empty;
            string result = null;
            foreach (string s in items)
            {
                if (result != null)
                    result += separator + s;
                else
                    result = s;
            }
            if (result == null)
                return String.Empty;
            else
                return result;
        }

        /// <summary>
        /// Gets the categories in attribute disjunctivity error.
        /// </summary>
        /// <param name="innnerException">The innner exception.</param>
        /// <param name="boxIdentity">The box identity.</param>
        /// <param name="categoriesInCollision">The categories in collision.</param>
        /// <returns></returns>
        public static BadParamsError AttributeCategoriesDisjunctivityError(Exception innnerException, string boxIdentity, IEnumerable<string> categoriesInCollision)
        {
            string message = sequenceToString(categoriesInCollision, ", ");

            return Modules.Exceptions.BadParamsError(
                innnerException,
                boxIdentity,
                "Categories in collision are: " + message,
                restrictionTypeEnum.AttributeCategoriesDisjunctivityError
                );
        }
    }

    /// <summary>
    /// This exception extends <see cref="T:System.InvalidOperationException"/>.
    /// </summary>
    public class IntervalsNotAllowedException : InvalidOperationException
    {
    }

    ///// <summary>
    ///// Disjunctivity is broken i.e. some objects are in collision.
    ///// </summary>
    //public class DisjunctivityCollisionException : ArgumentException
    //{
    //    private string[] _objectsInCollision;

    //    /// <summary>
    //    /// Initializes a new instance of the <see cref="T:Ferda.Guha.Attribute.DisjunctivityCollisionException"/> class.
    //    /// </summary>
    //    /// <param name="objectsInCollision">The objects in collision.</param>
    //    public DisjunctivityCollisionException(string[] objectsInCollision)
    //    {
    //        _objectsInCollision = objectsInCollision;
    //    }

    //    /// <summary>
    //    /// Initializes a new instance of the <see cref="T:Ferda.Guha.Attribute.DisjunctivityCollisionException"/> class.
    //    /// </summary>
    //    /// <param name="innerException">The inner exception.</param>
    //    public DisjunctivityCollisionException(Exception innerException)
    //        : base("", innerException)
    //    {
    //    }

    //    /// <summary>
    //    /// Initializes a new instance of the <see cref="T:Ferda.Guha.Attribute.DisjunctivityCollisionException"/> class.
    //    /// </summary>
    //    public DisjunctivityCollisionException()
    //    {
    //    }
    //}

    /// <summary>
    /// Order can not be determined i.e. neither x &lt; y nor x &gt; y.
    /// </summary>
    public class NotComparableCollisionException : ArgumentException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Ferda.Guha.Attribute.NotComparableCollisionException"/> class.
        /// </summary>
        public NotComparableCollisionException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Ferda.Guha.Attribute.NotComparableCollisionException"/> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        public NotComparableCollisionException(Exception innerException)
            : base("", innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Ferda.Guha.Attribute.NotComparableCollisionException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public NotComparableCollisionException(string message)
            : base(message)
        {
        }
    }
}