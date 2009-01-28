// IBitString.cs - Basic interface for the bit strings in Ferda
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

using Ferda.Guha.MiningProcessor.Formulas;

namespace Ferda.Guha.MiningProcessor.BitStrings
{
    /// <summary>
    /// Defines an empty bit string
    /// </summary>
    public interface IEmptyBitString : IBitString
    {
    }

    /// <summary>
    /// Basic interface for bit strings. Defines operations that can 
    /// be done with the bit strings.
    /// </summary>
    public interface IBitString : IBitStringBase
    {
        /// <summary>
        /// Identifier of the bit string (each bit string should be
        /// identified by a boolean attribute formula representing the
        /// bit string. 
        /// </summary>
        BooleanAttributeFormula Identifier { get; }

        /// <summary>
        /// Performs the bitwise AND operation on current BitString against the specified BitString.
        /// </summary>
        /// <param name="source">The second BitString operand.</param>
        /// <returns>Result of the AND operation</returns>
        IBitString And(IBitString source);

        ///// <summary>
        ///// Performs the bitwise AND operation on
        ///// copy of current BitString against the specified BitString.
        ///// </summary>
        ///// <param name="source">The second BitString operand.</param>
        //IBitString AndCloned(IBitString source);

        /// <summary>
        /// Performs the bitwise OR operation on current BitString against the specified BitString.
        /// </summary>
        /// <param name="source">The second BitString operand.</param>
        /// <returns>Result of the OR operation</returns>
        IBitString Or(IBitString source);

        ///// <summary>
        ///// Performs the bitwise OR operation on
        ///// copy of current BitString against the specified BitString.
        ///// </summary>
        ///// <param name="source">The second BitString operand.</param>
        //IBitString OrCloned(IBitString source);

        /// <summary>
        /// Performs the bitwise NOT on current BitString.
        /// </summary>
        IBitString Not();

        ///// <summary>
        ///// Performs the bitwise NOT on copy of current BitString.
        ///// </summary>
        //IBitString NotCloned();

        /// <summary>
        /// Number of bits in the current bit string, that are not equal to zero.
        /// This property came with introduction of fuzzy bit strings. In boolean
        /// bit strings, the Sum operation determines both the number of non-zero
        /// bits and the sum of the bit string. In the fuzzy case these two numbers
        /// are different. The function is needed for determining frequencies in ETrees
        /// and number of all items belonging to a condition in a 4FT.
        /// </summary>
        long NonZeroBitsCount { get; }

        /// <summary>
        /// Performs the bitwise SUM operation on current BitString.
        /// </summary>
        float Sum { get; set; }
    }

    /// <summary>
    /// Defines a bit string capable of some advanced functionality
    /// </summary>
    public interface IBitStringCreate : IBitStringBase
    {
        /// <summary>
        /// Fills the whole BitString with the specified value. In case of 
        /// crisp bit strings, the value is 1 or 0, in case of fuzzy bit strings,
        /// the value is a float [0,1].
        /// </summary>
        /// <param name="value">Value to be filled into every "bit" of the BitString.</param>
        /// <remarks>
        /// <para>BitString are filled with zeroes when created, so there is no need to call Fill(false) after create() method.</para>
        /// </remarks>
        void Fill(float value);

        /// <summary>
        /// Gets a value of the specified bit from the BitString.
        /// </summary>
        /// <param name="index">Index of the bit to be retrieved.</param>
        /// <returns>Value of the specified bit from the BitString.</returns>
        float GetBit(int index);

        /// <summary>
        /// Sets a specified bit in the BitString.
        /// </summary>
        /// <param name="index">Index of the bit to be set.</param>
        /// <param name="value">New value of the bit.</param>
        void SetBit(int index, float value);
    }

    /// <summary>
    /// Defines a bit string base.
    /// </summary>
    public interface IBitStringBase
    {
        /// <summary>
        /// Length of a bit string
        /// </summary>
        int Length { get; }
        /// <summary>
        /// String (human readable) form of the bit string
        /// </summary>
        /// <returns>String representation of the bit string</returns>
        string ToString();
    }
}
