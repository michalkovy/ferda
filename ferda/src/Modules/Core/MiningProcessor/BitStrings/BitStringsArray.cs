// BitStringArray.cs - Implements functionality for array of bit strings
//
// Author: Tomáš Kuchaø <tomas.kuchar@gmail.com>
// Commented by: Martin Ralbovský <martin.ralbovsky@gmail.com>
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

namespace Ferda.Guha.MiningProcessor.BitStrings
{
    /// <summary>
    /// Implementation of AND operation on arrays of bit string
    /// </summary>
    public static class BitStringsArrayAnd
    {
        /// <summary>
        /// Performs AND operation on two bitstrings in parameters
        /// </summary>
        /// <param name="operand1">First bit string</param>
        /// <param name="operand2">Second bit string</param>
        /// <returns>Result of AND operation</returns>
        private static IBitString operation(IBitString operand1, IBitString operand2)
        {
            return operand1.And(operand2);
        }

        /// <summary>
        /// Performs AND operation on two arrays of bit strings. Operation is performed
        /// <c>r*c</c> times, where <c>r</c> is length of <paramref name="rowOperand"/>
        /// bitstring and <c>r</c> is length of <paramref name="columnOperand"/>
        /// bit string.
        /// </summary>
        /// <param name="rowOperand">Row bit string array</param>
        /// <param name="columnOperand">Column bit string array</param>
        /// <returns>Result of AND operation</returns>
        public static IBitString[][] Operation(IBitString[] rowOperand, IBitString[] columnOperand)
        {
            if (rowOperand == null)
                throw new ArgumentNullException("rowOperand");
            if (columnOperand == null)
                throw new ArgumentNullException("columnOperand");

            int rNum = rowOperand.Length;
            int cNum = columnOperand.Length;

            IBitString[][] result = new IBitString[rNum][];
            for (int r = 0; r < rNum; r++)
            {
                result[r] = new IBitString[cNum];
                for (int c = 0; c < cNum; c++)
                {
                    result[r][c] = operation(rowOperand[r], columnOperand[c]);
                }
            }
            return result;
        }

        /// <summary>
        /// Performs AND operation of a two dimensional array of bit strings with one
        /// bit string. Operation is performed <c>r*c</c> times, where <c>r</c> and
        /// <c>c</c> are dimensions of the <paramref name="twoDimTable"/>
        /// bit string. 
        /// </summary>
        /// <param name="twoDimTable">Two dimensional bit string table</param>
        /// <param name="operand">Individual bit string</param>
        /// <returns>Result of AND operation</returns>
        public static IBitString[][] Operation(IBitString[][] twoDimTable, IBitString operand)
        {
            if (twoDimTable == null)
                throw new ArgumentNullException("twoDimTable");
            if (operand == null)
                throw new ArgumentNullException("operand");

            int rNum = twoDimTable.Length;
            int cNum = twoDimTable[0].Length;

            IBitString[][] result = new IBitString[rNum][];
            for (int r = 0; r < rNum; r++)
            {
                result[r] = new IBitString[cNum];
                for (int c = 0; c < cNum; c++)
                {
                    result[r][c] = operation(twoDimTable[r][c], operand);
                }
            }
            return result;
        }

        /// <summary>
        /// Performs AND operation between a one dimensional array of bit strings with
        /// one bit string. Operation is performed <c>r</c> times where <c>r</c> is
        /// the lenght of the <paramref name="rowOperand"/> bitstring.
        /// </summary>
        /// <param name="rowOperand">One dimensional bit string array</param>
        /// <param name="operand">Individual bit string</param>
        /// <returns>Result of AND operation</returns>
        public static IBitString[] Operation(IBitString[] rowOperand, IBitString operand)
        {
            if (rowOperand == null)
                throw new ArgumentNullException("rowOperand");
            if (operand == null)
                throw new ArgumentNullException("operand");

            int rNum = rowOperand.Length;

            IBitString[] result = new IBitString[rNum];
            for (int r = 0; r < rNum; r++)
            {
                result[r] = operation(rowOperand[r], operand);
            }
            return result;
        }
    }

    /// <summary>
    /// Implementation of OR operation on arrays of bit string
    /// </summary>
    public static class BitStringsArrayOr
    {
        /// <summary>
        /// Performs OR operation on two bitstrings in parameters
        /// </summary>
        /// <param name="operand1">First bit string</param>
        /// <param name="operand2">Second bit string</param>
        /// <returns>Result of OR operation</returns>
        private static IBitString operation(IBitString operand1, IBitString operand2)
        {
            return operand1.Or(operand2);
        }

        /// <summary>
        /// Performs OR operation on two arrays of bit strings. Operation is performed
        /// <c>r*c</c> times, where <c>r</c> is length of <paramref name="rowOperand"/>
        /// bitstring and <c>r</c> is length of <paramref name="columnOperand"/>
        /// bit string.
        /// </summary>
        /// <param name="rowOperand">Row bit string array</param>
        /// <param name="columnOperand">Column bit string array</param>
        /// <returns>Result of OR operation</returns>
        public static IBitString[][] Operation(IBitString[] rowOperand, IBitString[] columnOperand)
        {
            if (rowOperand == null)
                throw new ArgumentNullException("rowOperand");
            if (columnOperand == null)
                throw new ArgumentNullException("columnOperand");

            int rNum = rowOperand.Length;
            int cNum = columnOperand.Length;

            IBitString[][] result = new IBitString[rNum][];
            for (int r = 0; r < rNum; r++)
            {
                result[r] = new IBitString[cNum];
                for (int c = 0; c < cNum; c++)
                {
                    result[r][c] = operation(rowOperand[r], columnOperand[c]);
                }
            }
            return result;
        }

        /// <summary>
        /// Performs OR operation of a two dimensional array of bit strings with one
        /// bit string. Operation is performed <c>r*c</c> times, where <c>r</c> and
        /// <c>c</c> are dimensions of the <paramref name="twoDimTable"/>
        /// bit string. 
        /// </summary>
        /// <param name="twoDimTable">Two dimensional bit string table</param>
        /// <param name="operand">Individual bit string</param>
        /// <returns>Result of OR operation</returns>
        public static IBitString[][] Operation(IBitString[][] twoDimTable, IBitString operand)
        {
            if (twoDimTable == null)
                throw new ArgumentNullException("twoDimTable");
            if (operand == null)
                throw new ArgumentNullException("operand");

            int rNum = twoDimTable.Length;
            int cNum = twoDimTable[0].Length;

            IBitString[][] result = new IBitString[rNum][];
            for (int r = 0; r < rNum; r++)
            {
                result[r] = new IBitString[cNum];
                for (int c = 0; c < cNum; c++)
                {
                    result[r][c] = operation(twoDimTable[r][c], operand);
                }
            }
            return result;
        }

        /// <summary>
        /// Performs OR operation between a one dimensional array of bit strings with
        /// one bit string. Operation is performed <c>r</c> times where <c>r</c> is
        /// the lenght of the <paramref name="rowOperand"/> bitstring.
        /// </summary>
        /// <param name="rowOperand">One dimensional bit string array</param>
        /// <param name="operand">Individual bit string</param>
        /// <returns>Result of OR operation</returns>
        public static IBitString[] Operation(IBitString[] rowOperand, IBitString operand)
        {
            if (rowOperand == null)
                throw new ArgumentNullException("rowOperand");
            if (operand == null)
                throw new ArgumentNullException("operand");

            int rNum = rowOperand.Length;

            IBitString[] result = new IBitString[rNum];
            for (int r = 0; r < rNum; r++)
            {
                result[r] = operation(rowOperand[r], operand);
            }
            return result;
        }
    }

    /// <summary>
    /// Implementation of SUM operation on arrays of bit string
    /// </summary>
    public static class BitStringsArraySums
    {
        /// <summary>
        /// Performs SUM operation on every bit string in the
        /// two dimensional <paramref name="bitStringsArray"/>
        /// array. 
        /// </summary>
        /// <param name="bitStringsArray">The bit string array</param>
        /// <returns>Two dimensional array of SUMS of bit string
        /// in the array of <paramref name="bitStringsArray"/></returns>
        public static double[][] Sum(IBitString[][] bitStringsArray)
        {
            if (bitStringsArray == null)
                throw new ArgumentNullException("bitStringsArray");
            
            int rNum = bitStringsArray.Length;
            int cNum = bitStringsArray[0].Length;
            double[][] result = new double[rNum][];
            for (int r = 0; r < rNum; r++)
            {
                result[r] = new double[cNum];
                for (int c = 0; c < cNum; c++)
                {
                    result[r][c] = bitStringsArray[r][c].Sum;
                }
            }
            return result;            
        }

        /// <summary>
        /// Performs SUM operation on every bit string in the
        /// one dimensional <paramref name="bitStringsArray"/>
        /// array. 
        /// </summary>
        /// <param name="bitStringsArray">The bit string array</param>
        /// <returns>One dimensional array of SUMS of bit string
        /// in the array of <paramref name="bitStringsArray"/></returns>
        public static double[] Sum(IBitString[] bitStringsArray)
        {
            if (bitStringsArray == null)
                throw new ArgumentNullException("bitStringsArray");

            int rNum = bitStringsArray.Length;
            double[] result = new double[rNum];
            for (int r = 0; r < rNum; r++)
            {
                result[r] = bitStringsArray[r].Sum;
            }
            return result;
        }
    }
}