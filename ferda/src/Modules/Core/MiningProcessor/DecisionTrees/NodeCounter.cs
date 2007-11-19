// NodeCounter.cs - unique node counter
//
// Authors: Martin Ralbovský <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2007 Martin Ralbovský 
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

namespace Ferda.Guha.MiningProcessor.DecisionTrees
{
    /// <summary>
    /// Counter of nodes. Each node gets its unique number.
    /// </summary>
    public class NodeCounter
    {
        #region Private fields

        /// <summary>
        /// The unique node number
        /// </summary>
        private static double number = 0;

        /// <summary>
        /// Object used for thread-safe access to the bit string cache
        /// </summary>
        private static readonly object padlock = new object();

        #endregion

        #region Constructor

        /// <summary>
        /// Explicit static constructor to tell C# compiler
        /// not to mark type as beforefieldinit
        /// </summary>
        static NodeCounter()
        {
        }

        #endregion

        /// <summary>
        /// Gets a unique node number
        /// </summary>
        /// <returns>Unique node number</returns>
        public static double GetNodeNumber()
        {
            lock (padlock)
            {
                number++;
                return number;
            }
        }
    }
}
