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
            if (k == 1)
            {
                return (double)l * v;
            }

            double result = k;

            for (int i = 1; i <= l; i++)
            {
                //result *= k ^ (v ^ i);
                result *= System.Math.Pow(k, System.Math.Pow(v, i));
            }

            return result;
        }

        /// <summary>
        /// Counts the chi-squared statictics for given parameters. The formula for
        /// chi-squared statiscics is
        /// <c>SUM(i=0..r.Length-1)SUM(j=0..s.Length-1)[a_{i,j}-(r_{i}*s_{j}/n)^2]/r_{i}*s_{j}/n</c>
        /// </summary>
        /// <param name="r">The <c>r_{i}</c> array corresponding to numbers of items of
        /// individual categories of the attribute.</param>
        /// <param name="s">The <c>s_{i}</c> array corresponding to numbers of items
        /// of classification categories.</param>
        /// <param name="a">The <c>a_{i,j}</c> array. Item on indes <c>(i,j)</c> is the
        /// number of items that are present in given node (determined by the
        /// base bit string) for classification category <c>j</c> and attribute
        /// category <c>i</c></param>
        /// <returns>Chi squared value for given parametersu</returns>
        public static double ChiSquared(int[] r, int[] s, int[,] a)
        {
            double result = 0;

            //counting n
            int n=0;
            for (int i = 0; i < r.Length; i++)
            {
                n+= r[i];
            }

            for (int i = 0; i < r.Length; i++)
            {
                if (r[i] == 0)
                {
                    continue;
                }
                for (int j = 0; j < s.Length; j++)
                {
                    if (s[j] == 0)
                    {
                        continue;
                    }

                    double step = System.Math.Pow((a[i, j] - (double)r[i] * s[j] / n), 2) 
                        / ((double)r[i] * s[j] / n);
                    result += step;
                }
            }

            return result;
        }
    }
}
