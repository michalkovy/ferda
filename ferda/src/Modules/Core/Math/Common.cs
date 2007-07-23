// Common.cs - Common mathematical functionality
//
// Author: Tomáš Kuchaø <tomas.kuchar@gmail.com>
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
using Ferda.Modules;

namespace Ferda.Guha.Math
{
    /// <summary>
    /// Class providing common functionality for the mathematical functions
    /// </summary>
    public static class Common
    {
        const int defaultPrecisionTestForEquality = 10;

        /// <summary>
        /// Determines if two numbers are close to each other.
        /// The <paramref name="precision"/> determines how close the
        /// numbers should be
        /// </summary>
        /// <param name="precision">Precision of the closeness</param>
        /// <param name="x">First number</param>
        /// <param name="y">Second number</param>
        /// <returns>True if numbers are close enough</returns>
        public static bool CloseEnough(int precision, double x, double y)
        {
            if (precision < 1)
                throw new ArgumentOutOfRangeException("precision", precision, "The precision must be at least 1.");
            if ((x == 0.0) && (y == 0.0))
                return true;

            double level = (System.Math.Abs(x) > System.Math.Abs(y))
                               ? System.Math.Log10(System.Math.Abs(x))
                               : System.Math.Log10(System.Math.Abs(y));

            return (System.Math.Abs(x - y) < System.Math.Pow(10, level - precision));
        }

        /// <summary>
        /// Returns if the two operands belong to a specified relation
        /// </summary>
        /// <param name="relation">Rekation</param>
        /// <param name="firstOperand">First operand</param>
        /// <param name="secondOperand">Second operand</param>
        /// <returns>True iff two operands belong to a specified relation</returns>
        public static bool Compare(RelationEnum relation, double firstOperand, double secondOperand)
        {
            if (firstOperand == Double.NaN)
                return false;
            if (secondOperand == Double.NaN)
                return false;
            switch (relation)
            {
                case RelationEnum.Equal:
                    return CloseEnough(defaultPrecisionTestForEquality, firstOperand, secondOperand);
                case RelationEnum.Greater:
                    return (firstOperand > secondOperand);
                case RelationEnum.GreaterOrEqual:
                    return (firstOperand >= secondOperand);
                case RelationEnum.Less:
                    return (firstOperand < secondOperand);
                case RelationEnum.LessOrEqual:
                    return (firstOperand <= secondOperand);
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Get orientation of specified relation. I.e.
        /// for orientation to growth (&gt; or &gt;=) returns 1;
        /// for request for equality returns 0;
        /// otherwise returns -1.
        /// </summary>
        /// <param name="relation">The relation.</param>
        /// <returns></returns>
        public static int GetRelationOrientation(RelationEnum relation)
        {
            switch (relation)
            {
                case RelationEnum.Equal:
                    return 0;
                case RelationEnum.Greater:
                case RelationEnum.GreaterOrEqual:
                    return 1;
                case RelationEnum.Less:
                case RelationEnum.LessOrEqual:
                    return -1;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the orientation start value for specified orientation. 
        /// I.e. NaN for Equality, -INF for growth, +INF lowering.
        /// </summary>
        /// <param name="relation">The relation.</param>
        /// <returns></returns>
        public static double GetOrientationStartValue(RelationEnum relation)
        {
            switch (relation)
            {
                case RelationEnum.Equal:
                    return Double.NaN;
                case RelationEnum.Greater:
                case RelationEnum.GreaterOrEqual:
                    return Double.NegativeInfinity;
                case RelationEnum.Less:
                case RelationEnum.LessOrEqual:
                    return Double.PositiveInfinity;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the orientation better value for specified relation.
        /// I.e. greater value for growth, lower value for lowering.
        /// Exception is thrown if relation is request for equality.
        /// </summary>
        /// <param name="relation">The relation.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public static double GetOrientationBetterValue(RelationEnum relation, double x, double y)
        {
            switch (relation)
            {
                case RelationEnum.Equal:
                    throw new InvalidOperationException();
                case RelationEnum.Greater:
                case RelationEnum.GreaterOrEqual:
                    if (x == Double.NaN)
                        return y;
                    if (y == Double.NaN)
                        return x;
                    return System.Math.Max(x, y);
                case RelationEnum.Less:
                case RelationEnum.LessOrEqual:
                    if (x == Double.NaN)
                        return y;
                    if (y == Double.NaN)
                        return x;
                    return System.Math.Min(x, y);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}