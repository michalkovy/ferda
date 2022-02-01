// TrueBitString.cs - Singleton true bit string
//
// Author:  Tomáš Kuchaø <tomas.kuchar@gmail.com>,
//          Martin Ralbovský <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2007 Tomáš Kuchaø, Martin Ralbovský
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
using Ferda.Guha.MiningProcessor.Formulas;

namespace Ferda.Guha.MiningProcessor.BitStrings
{
    /// <summary>
    /// Singleton class that represents a true bit string - bit string that
    /// is 1 for any length. For performance
    /// reasons, we chose to have only one true bit string.
    /// </summary>
    public class TrueBitString : IEmptyBitString
    {
        #region Singleton

        /// <summary>
        /// The singleton (one) object that can be created
        /// </summary>
        private static readonly TrueBitString _instance = new TrueBitString();

        /// <summary>
        /// Object used for thread-safe access to the bit string cache
        /// </summary>
        private static readonly object padlock = new object();

        /// <summary>
        /// Explicit static constructor to tell C# compiler
        /// not to mark type as beforefieldinit
        /// </summary>
        static TrueBitString()
        {
        }

        /// <summary>
        /// Gets the (singleton) instance of the true bit string. 
        /// </summary>
        /// <returns>Instance of the true bit string</returns>
        public static TrueBitString GetInstance()
        {
            lock (padlock)
            {
                return _instance;
            }
        }

        #endregion

        /// <summary>
        /// The true bit string identifier
        /// </summary>
        public static readonly BitStringIdentifier TrueBitStringIdentifier =
            new BitStringIdentifier(null, "True BitString");

        /// <summary>
        /// Boolean attribute formula for the false bit string
        /// </summary>
        public static readonly BooleanAttributeFormula TrueBitStringId =
            new AtomFormula(TrueBitStringIdentifier);

        #region IBitString Members

        /// <summary>
        /// Identifier of the bit string (each bit string should be
        /// identified by a boolean attribute formula representing the
        /// bit string. 
        /// </summary>
        public BooleanAttributeFormula Identifier
        {
            get
            {
                return TrueBitStringId;
            }
        }

        /// <summary>
        /// Performs the bitwise AND operation on current BitString against the specified BitString.
        /// </summary>
        /// <param name="source">The second BitString operand.</param>
        public IBitString And(IBitString source)
        {
            return source;
        }

        /// <summary>
        /// Performs a NOT operation on the true bit string. This should not
        /// happen.
        /// </summary>
        /// <returns>Result of NOT operation</returns>
        public IBitString Not()
        {
            throw new NotSupportedException("Negation of true bit string is not supported.");
        }

        /// <summary>
        /// Performs the bitwise OR operation on current BitString against the specified BitString.
        /// </summary>
        /// <param name="source">The second BitString operand.</param>
        public IBitString Or(IBitString source)
        {
            return this;
        }

        /// <summary>
        /// Performs the bitwise SUM operation on current BitString.
        /// </summary>
        /// <returns>The number of bits set to 1 in current BitString.</returns>
        public float Sum
        {
            get { return 0f; }
            set { }
        }

        /// <summary>
        /// Number of bits in the current bit string, that are not equal to zero.
        /// This property came with introduction of fuzzy bit strings. In boolean
        /// bit strings, the Sum operation determines both the number of non-zero
        /// bits and the sum of the bit string. In the fuzzy case these two numbers
        /// are different. The function is needed for determining frequencies in ETrees
        /// and number of all items belonging to a condition in a 4FT.
        /// </summary>
        public long NonZeroBitsCount
        {
            get
            {
                return 0;
            }
        }

        #endregion

        #region IBitStringBase Members

        /// <summary>
        /// Length of a bit string
        /// </summary>
        public int Length
        {
            get { return 1; }
        }

        #endregion
    }
}
