// LNFactorialTable.cs - The table of natural logarithms of factorials of N numbers
//
// Author: Martin Ralbovský <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2009 Martin Ralbovský
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

namespace Ferda.Guha.Math
{
    /// <summary>
    /// Table of natural logarithms of factorials of N numbers
    /// </summary>
    public class LNFactorialTable
    {
        /// <summary>
        /// The table itself
        /// </summary>
        protected double[] table = null;

        /// <summary>
        /// Size of the table
        /// </summary>
        protected int n;

        /// <summary>
        /// The default constructor of the class
        /// </summary>
        /// <param name="n">Size of the table</param>
        public LNFactorialTable(int n)
        {
            this.n = n;
            table = new double[n];
            table[0] = 0;
            for (int i = 1; i < n; i++)
            {
                table[i] = table[i - 1] + System.Math.Log(i + 1);
            }
        }

        /// <summary>
        /// Gets natural logarithm of factorial of specified number less than 
        /// N.
        /// </summary>
        /// <param name="i">The number</param>
        /// <returns>Natural logarithm of factorial of <paramref name="i"/></returns>
        public double GetLNFact(double i)
        {
            int index = (int)i;
            if (i == 0)
                return 0; //factorial of 0 is 1, ln of 1 is zero
            if (i > n || i < 0)
            {
                return Double.NaN; 
            }
            return table[index-1];
        }
    }
}
