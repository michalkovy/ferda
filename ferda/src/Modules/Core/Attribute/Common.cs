// Common.cs - common functionality for attributes
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
using Ferda.Guha.Data;
using Ferda.Modules;

namespace Ferda.Guha.Attribute
{
    /// <summary>
    /// Left/right side enumeration.
    /// </summary>
    public enum Side
    {
        /// <summary>
        /// Left side
        /// </summary>
        Left,

        /// <summary>
        /// Right side
        /// </summary>
        Right
    }


    /// <summary>
    /// Provides basic/hepfull methods for working with the attribute
    /// including some ToString() necessary string constants.
    /// </summary>
    public static class Common
    {
        #region ToString() helpers

        /// <summary>
        /// Label of the Null value.
        /// </summary>
        public const string NullValue = nullValueConstant.value;

        /// <summary>
        /// Mark of interval`s left open side.
        /// </summary>
        private const string leftIntervalOpened = "(";

        /// <summary>
        /// Mark of interval`s left closed side.
        /// </summary>
        private const string leftIntervalClosed = "<";

        /// <summary>
        /// Mark of interval`s right open side.
        /// </summary>
        private const string rightIntervalOpened = ")";

        /// <summary>
        /// Mark of interval`s right closed side.
        /// </summary>
        private const string rightIntervalClosed = ">";

        /// <summary>
        /// Positive infinity string representation.
        /// </summary>
        private static string positiveInfinity = float.PositiveInfinity.ToString();

        /// <summary>
        /// Negative infinity string representation.
        /// </summary>
        private static string negativeInfinity = float.NegativeInfinity.ToString();

        /// <summary>
        /// String separating interval values e.g. &lt;0[separator]2)
        /// </summary>
        public const string IntervalValuesSeparator = ";";

        /// <summary>
        /// String separating members of category e.g. (0;1)[separator](2;3)
        /// </summary>
        public const string CategoryMembersSeparator = ";";

        /// <summary>
        /// String separating names of categories e.g. firstCategory[separator]secondCategory
        /// </summary>
        public const string CategoriesNamesSeparator = ";";


        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the boundary.
        /// </summary>
        /// <param name="side">The side.</param>
        /// <param name="boundaryType">Type of the boundary.</param>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the boundary.
        /// </returns>
        public static string BoundaryToString(Side side, BoundaryEnum boundaryType)
        {
            if (side == Side.Left)
            {
                switch (boundaryType)
                {
                    case BoundaryEnum.Open:
                        return leftIntervalOpened;
                    case BoundaryEnum.Closed:
                        return leftIntervalClosed;
                    case BoundaryEnum.Infinity:
                        return leftIntervalOpened + negativeInfinity;
                    default:
                        throw new NotImplementedException();
                }
            }
            else if (side == Side.Right)
            {
                switch (boundaryType)
                {
                    case BoundaryEnum.Open:
                        return rightIntervalOpened;
                    case BoundaryEnum.Closed:
                        return rightIntervalClosed;
                    case BoundaryEnum.Infinity:
                        return positiveInfinity + rightIntervalOpened;
                    default:
                        throw new NotImplementedException();
                }
            }
            throw new NotImplementedException();
        }

        #endregion

        /// <summary>
        /// Gets the T-type from specified property value.
        /// </summary>
        /// <param name="propertyValue">The property value.</param>
        public static T GetTFromPropertyValue<T>(PropertyValue propertyValue)
        {
            return (T)BasicPropertyValueTypes.GetPropertyValue(propertyValue);
        }

        /// <summary>
        /// Gets the attribute object for specified datatype.
        /// </summary>
        /// <param name="dbDataType">Type of the db data.</param>
        /// <param name="intervalsAllowed">if set to <c>true</c> intervals are allowed.</param>
        /// <returns></returns>
        public static object GetAttributeObject(DbSimpleDataTypeEnum dbDataType, bool intervalsAllowed)
        {
            return new Attribute<IComparable>(dbDataType, intervalsAllowed);
            //switch (dbDataType)
            //{
            //    case DbSimpleDataTypeEnum.BooleanSimpleType:
            //        return new Attribute<bool>(intervalsAllowed);
            //    case DbSimpleDataTypeEnum.DateTimeSimpleType:
            //        return new Attribute<DateTime>(intervalsAllowed);
            //    case DbSimpleDataTypeEnum.DoubleSimpleType:
            //        return new Attribute<double>(intervalsAllowed);
            //    case DbSimpleDataTypeEnum.FloatSimpleType:
            //        return new Attribute<float>(intervalsAllowed);
            //    case DbSimpleDataTypeEnum.IntegerSimpleType:
            //        return new Attribute<int>(intervalsAllowed);
            //    case DbSimpleDataTypeEnum.LongSimpleType:
            //        return new Attribute<long>(intervalsAllowed);
            //    case DbSimpleDataTypeEnum.ShortSimpleType:
            //        return new Attribute<short>(intervalsAllowed);
            //    case DbSimpleDataTypeEnum.StringSimpleType:
            //        return new Attribute<string>(intervalsAllowed);
            //    case DbSimpleDataTypeEnum.TimeSimpleType:
            //        return new Attribute<TimeSpan>(intervalsAllowed);
            //    case DbSimpleDataTypeEnum.UnknownSimpleType:
            //        return new Attribute<IComparable>(intervalsAllowed);
            //    default:
            //        throw new NotImplementedException();
            //}
        }
    }
}