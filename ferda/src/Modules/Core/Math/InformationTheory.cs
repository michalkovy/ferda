// InformationTheory.cs - Information theory equasions
//
// Author: Tomáš Kuchař <tomas.kuchar@gmail.com>
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


namespace Ferda.Guha.Math
{
    /// <summary>
    /// Specified direction of tested information dependence.
    /// </summary>
    public enum DependenceDirection
    {
        /// <summary>
        /// Wonder in depencence of Rows on Columns.
        /// </summary>
        RowsOnColumns,

        /// <summary>
        /// Wonder in depencence of Columns on Rows.
        /// </summary>
        ColumnsOnRows
    }

    /// <summary>
    /// Provides basic methods for study Information depencedence.
    /// </summary>
    /// <remarks>
    /// See [055 Náměty pro další vývoj KL-Miner.doc] 
    /// or [080 Náměty pro další vývoj KL-Miner II.doc].
    /// </remarks>
    /// <seealso href="http://en.wikipedia.org/wiki/Information_theory"/>
    public static class InformationTheory
    {
        /// <summary>
        /// H(C) = - Sum(c)( (p *,c) * Log(p *,c) )
        /// (because H = - Sum(i)( p_i Log(p_i))).
        /// </summary>
        /// <param name="table">The contingency table.</param>
        /// <seealso href="http://en.wikipedia.org/wiki/Information_entropy"/>
        public static double MarginalCEntropy(ContingencyTable table)
        {
            double denominator = table.Denominator;
            double result = 0;

            foreach (double freq in table.ColumnSums)
            {
                double item = freq/denominator;
                result += item*System.Math.Log(item, 2);
            }
            return -result;
        }

        /// <summary>
        /// H(R) = - Sum(r)( (p r,*) * Log(p r,*) )
        /// (because H = - Sum(i)( p_i Log(p_i))).
        /// </summary>
        /// <param name="table">The contingency table.</param>
        /// <seealso href="http://en.wikipedia.org/wiki/Information_entropy"/>
        public static double MarginalREntropy(ContingencyTable table)
        {
            double denominator = table.Denominator;
            double result = 0;

            foreach (double freq in table.RowSums)
            {
                double item = freq/denominator;
                result += item*System.Math.Log(item, 2);
            }
            return -result;
        }

        /// <summary>
        /// H(X,Y) = H(Y,X) = - Sum(i,j)( (p i,j) * Log(p i,j) )
        /// </summary>
        /// <param name="table">The contingency table.</param>
        /// <seealso href="http://en.wikipedia.org/wiki/Joint_entropy"/>
        public static double JointEntropy(ContingencyTable table)
        {
            double denominator = table.Denominator;
            double result = 0;

            table.ForEach(delegate(double v)
                              {
                                  double item = v/denominator;
                                  result += item*System.Math.Log(item, 2);
                              }
                );
            return -result;
        }

        /// <summary>
        /// H(C|R) = H(C,R) - H(R)
        /// </summary>
        /// <param name="table">The table.</param>
        /// <seealso href="http://en.wikipedia.org/wiki/Conditional_entropy"/>
        public static double ConditionalCREntropyValue(ContingencyTable table)
        {
            return JointEntropy(table) - MarginalREntropy(table);
        }

        /// <summary>
        /// H(R|C) = H(C,R) - H(C)
        /// </summary>
        /// <param name="table">The table.</param>
        /// <seealso href="http://en.wikipedia.org/wiki/Conditional_entropy"/>
        public static double ConditionalRCEntropyValue(ContingencyTable table)
        {
            return JointEntropy(table) - MarginalCEntropy(table);
        }

        /// <summary>
        /// Mutual information.
        /// I(X,Y) = H(X) - H(X|Y) = H(Y) - H(Y|X) = H(X) + H(Y) - H(X,Y)
        /// </summary>
        /// <param name="table">The table.</param>
        /// <remarks>
        /// In some notations is I(X,Y) marked as MI(X,Y).
        /// </remarks>
        /// <seealso href="http://en.wikipedia.org/wiki/Mutual_information"/>
        public static double MutualInformation(ContingencyTable table)
        {
            return MarginalCEntropy(table) + MarginalREntropy(table) - JointEntropy(table);
        }

        /// <summary>
        /// Mutual information normalized. Value of Mutual information I(X,Y)
        /// is normalized into interval [0,1].
        /// MI*(X,Y) = I(X,Y) / Min{ H(X), H(Y) } = 
        /// ( H(X) + H(Y) - H(X,Y) ) / Min{ H(X), H(Y) }.
        /// </summary>
        /// <param name="table">The table.</param>
        public static double MutualInformationNormalized(ContingencyTable table)
        {
            double hX = MarginalCEntropy(table);
            double hY = MarginalREntropy(table);
            return (hX + hY - JointEntropy(table))/System.Math.Min(hX, hY);
        }

        /// <summary>
        /// Information dependence of C on R
        /// ID(C|R) = 1 - ( H(C|R) / H(C) ) = 
        /// 1 - ( (H(C,R) - H(R)) / H(C) ) = 
        /// (H(C) - H(C,R) + H(R)) / H(C) = 
        /// I(C,R) / H(C)
        /// </summary>
        public static double InformationDependenceCR(ContingencyTable table)
        {
            double hC = MarginalCEntropy(table);
            double hR = MarginalREntropy(table);
            return (hC + hR - JointEntropy(table))/hC;
        }

        /// <summary>
        /// Information dependence of R on C
        /// ID(R|C) = 1 - ( H(R|C) / H(R) ) = 
        /// 1 - ( (H(C,R) - H(C)) / H(R) ) = 
        /// (H(R) - H(C,R) + H(C)) / H(R) = 
        /// I(C,R) / H(R)
        /// </summary>
        public static double InformationDependenceRC(ContingencyTable table)
        {
            double hC = MarginalCEntropy(table);
            double hR = MarginalREntropy(table);
            return (hC + hR - JointEntropy(table))/hR;
        }
    }
}