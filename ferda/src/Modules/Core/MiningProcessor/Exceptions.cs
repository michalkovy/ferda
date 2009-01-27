// Exceptions.cs - Defines exceptions for the MiningProcessor
//
// Authors: Tomáš Kuchaø, Martin Ralbovský <tomas.kuchar@gmail.com> 
//          <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2006 Tomáš Kuchaø, Martin Ralbovský
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
using System.Text;
using Ferda.Modules;

namespace Ferda.Guha.MiningProcessor
{
    /// <summary>
    /// Defines exceptions for the MiningProcessor
    /// </summary>
    public class Exceptions
    {
        /// <summary>
        /// Returns an error concerning bit string lenght
        /// </summary>
        /// <returns>error concerning bit string lenght</returns>
        public static BoxRuntimeError BitStringLengthError()
        {
            BoxRuntimeError result = new BoxRuntimeError();
            result.userMessage = "The length of a BitString must be a positive integer.";

            return result;
        }

        /// <summary>
        /// Returns an error concerning bit string lenght
        /// </summary>
        /// <returns>error concerning bit string lenght</returns>
        public static BoxRuntimeError BitStringLengthError2()
        {
            BoxRuntimeError result = new BoxRuntimeError();
            result.userMessage = "The number of 1 bits in the bit string is higher that its length.";

            return result;
        }

        /// <summary>
        /// Returns an error concerning different size of bit strings
        /// </summary>
        /// <returns>error concerning different size of bit strings</returns>
        public static BoxRuntimeError BitStringsLengtsAreNotEqualError()
        {
            BoxRuntimeError result = new BoxRuntimeError();
            result.userMessage = "BitString sizes do not match.";
            return result;
        }

        /// <summary>
        /// Returns a error saying that minimal length is greater then maximal
        /// length
        /// </summary>
        /// <returns>error saying that minimal length is greater then maximal
        /// length</returns>
        public static BoxRuntimeError MaxLengthIsLessThanMinLengthError()
        {
            BoxRuntimeError result = new BoxRuntimeError();
            result.userMessage = "MinLength is greather than MaxLength";
            return result;
        }

        /// <summary>
        /// Returns error saying that a cedent type speicified by the parameter
        /// <paramref name="cedentType"/> is not allowed.
        /// </summary>
        /// <param name="cedentType">Type of the cedent</param>
        /// <returns>error saying that a cedent type speicified by the parameter</returns>
        public static BoxRuntimeError EmptyCedentIsNotAllowedError(MarkEnum cedentType)
        {
            BoxRuntimeError result = new BoxRuntimeError();
            result.userMessage = "Empty cedent (or with MinLength equal to zero) is not allowed for cedent type: " + cedentType.ToString();
            return result;
        }

        /// <summary>
        /// Returns an error saying that given algorithm parameter should be greater than zero.
        /// The exception is used for ETree mining
        /// </summary>
        /// <param name="algorithmParameter">Algorithm parameter</param>
        /// <returns>Exception</returns>
        public static BoxRuntimeError NotMoreThanZeroException(string algorithmParameter)
        {
            BoxRuntimeError result = new BoxRuntimeError();
            result.userMessage = "Parameter " + algorithmParameter + "should be always greater than zero.";
            return result;
        }

        /// <summary>
        /// Returns an error saying that the ETree node is treated differently that it should
        /// be. This can occur when i.e. code asks for the subNodes, but the node is a leaf
        /// (or vice versa). More details in <see cref="Ferda.Guha.MiningProcessor.DecisionTrees.Node"/>
        /// </summary>
        /// <returns>Exception</returns>
        public static BoxRuntimeError WrongNodeTypeException()
        {
            BoxRuntimeError result = new BoxRuntimeError();
            result.userMessage = "The GUHA tree node is not treated according to its position in the tree (leaf vs. inner node).";
            return result;
        }

        /// <summary>
        /// Returns an error saying that it is expected to create a non fuzzy bit string,
        /// however the numbers are float i.e. fuzzy. 
        /// </summary>
        /// <returns></returns>
        public static BoxRuntimeError NotFuzzyBitStringException()
        {
            BoxRuntimeError result = new BoxRuntimeError();
            result.userMessage = "It is expected to create a non fuzzy bit string, however the data are fuzzy.";
            return result;
        }
    }
}
