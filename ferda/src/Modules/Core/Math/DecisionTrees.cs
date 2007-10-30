// DecisionTrees.cs - mathematical support for computing GUHA decision trees
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

namespace Ferda.Guha.Math
{
    /// <summary>
    /// Provides mathematical functionality for the GUHA decision trees by
    /// implementing some static functions. 
    /// </summary>
    public class DecisionTrees
    {
        /// <summary>
        /// <para>
        /// Counts number of relevant questions for this task setting. The formula
        /// can be upper approximated with variables <c>k</c>, <c>l</c> and <c>v</c> as
        /// <c>k*PRODUCT(i=1 to l)(k^v)</c>.
        /// </para>
        /// <para>
        /// where <c>k</c> stands for maximal number attributes for branching,
        /// <c>l</c> stands for maximal tree depth and <c>v</c> for maximal 
        /// number of categories from branching attributes.
        /// </para>
        /// </summary>
        /// <param name="k">maximal number attributes for branchin</param>
        /// <param name="l">maximal tree depth</param>
        /// <param name="v">maximal number of categories from branching attributes</param>
        /// <returns>Number of relevant questions</returns>
        public static double CountRelevantQuestions(int k, int l, int v)
        {
            double result = k;

            for (int i = 1; i <= l; i++)
            {
                //result *= k ^ (v ^ i);
                result *= System.Math.Pow(k, System.Math.Pow(v, i));
            }

            return result;
        }
    }
}
