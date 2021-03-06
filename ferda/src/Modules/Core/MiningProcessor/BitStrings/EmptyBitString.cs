// EmptyBitString.cs - Singleton empty bit string
//
// Author: Tomáš Kuchař <tomas.kuchar@gmail.com>
// Commented by: Martin Ralbovský <martin.ralbovsky@gmail.com>
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

using System;
using Ferda.Guha.MiningProcessor.Formulas;

namespace Ferda.Guha.MiningProcessor.BitStrings
{
    /// <summary>
    /// Singleton class that represents an empty bit string. For performance
    /// reasons, we chose to have only one empty bit string.
    /// </summary>
    public class EmptyBitString : IEmptyBitString
    {
        #region Singleton
        
        /// <summary>
        /// The singleton (one) object that can be created
        /// </summary>
        private static readonly EmptyBitString _instance = new EmptyBitString();

        /// <summary>
        /// Explicit static constructor to tell C# compiler
        /// not to mark type as beforefieldinit
        /// </summary>
        static EmptyBitString()
        {
        }

        /// <summary>
        /// Gets the (singleton) instance of the empty bit string. 
        /// </summary>
        /// <returns>Instance of the empty bit string</returns>
        public static EmptyBitString GetInstance()
        {
            return _instance;
        }

        #endregion

        /// <summary>
        /// The empty bit string identifier
        /// </summary>
        public static readonly BitStringIdentifier EmptyBitStringIdentifier =
            new BitStringIdentifier(null, "Empty BitString Category");

        /// <summary>
        /// Boolean attribute formula for the empty bit string
        /// </summary>
        public static readonly BooleanAttributeFormula EmptyBitStringId = new AtomFormula(EmptyBitStringIdentifier);

        /// <summary>
        /// Inicializes the empty bit string
        /// </summary>
        private EmptyBitString()
        {
        }

        #region IBitString Members

        /// <summary>
        /// Identifier of the bit string (each bit string should be
        /// identified by a boolean attribute formula representing the
        /// bit string. 
        /// </summary>
        public BooleanAttributeFormula Identifier
        {
            get { return EmptyBitStringId; }
        }

        #region AND
        
        /// <summary>
        /// Special AND operation for handling empty and false bit strings
        /// </summary>
        /// <param name="source">Source bit string</param>
        /// <returns>Result of special AND operation</returns>
        public IBitString andSpecial(IBitString source)
        {
            if (source is EmptyBitString)
                return this;
            else if (source is FalseBitString)
                return source;
            else
                throw new NotImplementedException();
        }

        /// <summary>
        /// Performs the bitwise AND operation on current BitString against the specified BitString.
        /// </summary>
        /// <param name="source">The second BitString operand.</param>
        public IBitString And(IBitString source, bool precomputeSum = false)
        {
            BitString bs = source as BitString;
            if (bs != null)
                return bs.And(this);
            FuzzyBitString fbs = source as FuzzyBitString;
            if (fbs != null)
                return fbs.And(this);
            
            return andSpecial(source);
        }

        public IBitString AndInPlace(IBitString source)
        {
            return And(source);
        }

        #endregion

        #region NOT

        /// <summary>
        /// Performs a NOT operation on the empty bit string (result is
        /// again an empty bit string.
        /// </summary>
        /// <returns>Result of NOT operation</returns>
        public IBitString Not()
        {
            return this;
        }

        /// <summary>
        /// Performs a NOT operation on the empty bit string (result is
        /// again an empty bit string.
        /// </summary>
        /// <returns>Result of NOT operation</returns>
        public IBitString NotCloned()
        {
            return this;
        }
        
        #endregion

        #region OR

        /// <summary>
        /// Special OR operation for handling empty and false bit strings
        /// </summary>
        /// <param name="source">Source bit string</param>
        /// <returns>Result of special OR operation</returns>
        public IBitString orSpecial(IBitString source)
        {
            if (source is EmptyBitString)
                return this;
            else if (source is FalseBitString)
                return this;
            else
                throw new NotImplementedException();
        }

        /// <summary>
        /// Performs the bitwise OR operation on current BitString against the specified BitString.
        /// </summary>
        /// <param name="source">The second BitString operand.</param>
        public IBitString Or(IBitString source, bool precomputeSum = false)
        {
            BitString bs = source as BitString;
            if (bs != null)
                return bs.Or(this);
            FuzzyBitString fbs = source as FuzzyBitString;
            if (fbs != null)
                return fbs.Or(this);
            
                return orSpecial(source);
        }

        public IBitString OrInPlace(IBitString source)
        {
            return And(source);
        }

        #endregion

        /// <summary>
        /// Performs the bitwise SUM operation on current BitString.
        /// </summary>
        /// <returns>The number of bits set to 1 in current BitString.</returns>
        public float Sum
        {
            get { return Single.MaxValue; }
			set {}
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
                return long.MaxValue;
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
