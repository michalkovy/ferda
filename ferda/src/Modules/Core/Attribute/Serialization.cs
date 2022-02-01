// Serialization.cs - serialization and deserialization of attributes in Ferda
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
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Ferda.Guha.Data;
using System.Collections.Generic;

namespace Ferda.Guha.Attribute
{
    /// <summary>
    /// Class with methods for retyping AttributeSerializable IComparable to the explicit type
    /// </summary>
    /// <typeparam name="T">See typeparam in <see cref="T:Ferda.Guha.Attribute.Attribute`1"/></typeparam>
    /// <typeparam name="U">See typeparam in <see cref="T:Ferda.Guha.Attribute.Attribute`1"/></typeparam>
    public class Retyper<T, U>
        where T : IComparable
        where U : IComparable
    {
        /// <summary>
        /// Method for retyping attribute to IComparable
        /// </summary>
        /// <param name="attribute">Attribute to retype</param>
        /// <returns>Retyped attribute</returns>
        public static Attribute<IComparable> ToIComparable(AttributeSerializable<U> attribute)
        {
            AttributeSerializable<IComparable> result =
                new AttributeSerializable<IComparable>();
            result.NullContainingCategoryName
                        = attribute.NullContainingCategoryName;
            result.IntervalsAllowed = attribute.IntervalsAllowed;
            result.DbDataType = attribute.DbDataType;
            List<CategorySerializable<IComparable>> categories
                = new List<CategorySerializable<IComparable>>();
            foreach (CategorySerializable<U> cat in attribute.Categories)
            {
                CategorySerializable<IComparable> _cat =
                    new CategorySerializable<IComparable>();

                _cat.Enumeration = new IComparable[cat.Enumeration.Length];
                _cat.Intervals = new IntervalSerializable<IComparable>[cat.Intervals.Length];

                int i = 0;
                foreach (U value in cat.Enumeration)
                {
                    _cat.Enumeration[i] = (IComparable)value;
                    i++;
                }
                i = 0;
                foreach (IntervalSerializable<U> _interval in cat.Intervals)
                {
                    _cat.Intervals[i] = new IntervalSerializable<IComparable>();
                    _cat.Intervals[i].LeftBoundary = _interval.LeftBoundary;
                    _cat.Intervals[i].RightBoundary = _interval.RightBoundary;
                    _cat.Intervals[i].LeftValue = (IComparable)_interval.LeftValue;
                    _cat.Intervals[i].RightValue = (IComparable)_interval.RightValue;
                    i++;
                }

                _cat.Name = cat.Name;
                _cat.OrdNumber = cat.OrdNumber;
                categories.Add(_cat);
            }
            result.Categories = categories.ToArray();

            Attribute<IComparable> returnAttribute =
                new Attribute<IComparable>(attribute.DbDataType, result, true);

            return returnAttribute;
        }


        /// <summary>
        /// Retypes to boolean
        /// </summary>
        /// <param name="input">Attribute to retype</param>
        /// <returns>Retyped attribute</returns>
        public static AttributeSerializable<Boolean> RetypeToBool(AttributeSerializable<T> input)
        {
            AttributeSerializable<Boolean> result =
                new AttributeSerializable<Boolean>();

            result.NullContainingCategoryName
                        = input.NullContainingCategoryName;
            result.IntervalsAllowed = input.IntervalsAllowed;
            result.DbDataType = input.DbDataType;

            List<CategorySerializable<Boolean>> categories
                = new List<CategorySerializable<Boolean>>();
            foreach (CategorySerializable<T> cat in input.Categories)
            {
                CategorySerializable<Boolean> _cat =
                    new CategorySerializable<Boolean>();

                _cat.Enumeration = new Boolean[cat.Enumeration.Length];
                _cat.Intervals = new IntervalSerializable<Boolean>[cat.Intervals.Length];

                int i = 0;
                foreach (T value in cat.Enumeration)
                {
                    _cat.Enumeration[i] = Convert.ToBoolean(value);
                    i++;
                }
                i = 0;
                foreach (IntervalSerializable<T> _interval in cat.Intervals)
                {
                    _cat.Intervals[i] = new IntervalSerializable<bool>();
                    _cat.Intervals[i].LeftBoundary = _interval.LeftBoundary;
                    _cat.Intervals[i].RightBoundary = _interval.RightBoundary;
                    _cat.Intervals[i].LeftValue = Convert.ToBoolean(_interval.LeftValue);
                    _cat.Intervals[i].RightValue = Convert.ToBoolean(_interval.RightValue);
                    i++;
                }

                _cat.Name = cat.Name;
                _cat.OrdNumber = cat.OrdNumber;
                categories.Add(_cat);
            }
            result.Categories = categories.ToArray();
            return result;
        }

        /// <summary>
        /// Retypes to short
        /// </summary>
        /// <param name="input">Attribute to retype</param>
        /// <returns>Retyped attribute</returns>
        public static AttributeSerializable<Int16> RetypeToShort(AttributeSerializable<T> input)
        {
            AttributeSerializable<Int16> result =
                new AttributeSerializable<Int16>();

            result.NullContainingCategoryName
                        = input.NullContainingCategoryName;
            result.IntervalsAllowed = input.IntervalsAllowed;
            result.DbDataType = input.DbDataType;

            List<CategorySerializable<Int16>> categories
                = new List<CategorySerializable<Int16>>();
            foreach (CategorySerializable<T> cat in input.Categories)
            {
                CategorySerializable<Int16> _cat =
                    new CategorySerializable<Int16>();

                _cat.Enumeration = new Int16[cat.Enumeration.Length];
                _cat.Intervals = new IntervalSerializable<Int16>[cat.Intervals.Length];

                int i = 0;
                foreach (T value in cat.Enumeration)
                {
                    _cat.Enumeration[i] = Convert.ToInt16(value);
                    i++;
                }

                i = 0;
                foreach (IntervalSerializable<T> _interval in cat.Intervals)
                {
                    _cat.Intervals[i] = new IntervalSerializable<short>();
                    _cat.Intervals[i].LeftBoundary = _interval.LeftBoundary;
                    _cat.Intervals[i].RightBoundary = _interval.RightBoundary;
                    _cat.Intervals[i].LeftValue = Convert.ToInt16(_interval.LeftValue);
                    _cat.Intervals[i].RightValue = Convert.ToInt16(_interval.RightValue);
                    i++;
                }

                _cat.Name = cat.Name;
                _cat.OrdNumber = cat.OrdNumber;
                categories.Add(_cat);
            }

            result.Categories = categories.ToArray();

            return result;
        }

        /// <summary>
        /// Retypes to int
        /// </summary>
        /// <param name="input">Attribute to retype</param>
        /// <returns>Retyped attribute</returns>
        public static AttributeSerializable<Int32> RetypeToInt(AttributeSerializable<T> input)
        {
            AttributeSerializable<Int32> result =
                new AttributeSerializable<Int32>();

            result.NullContainingCategoryName
                        = input.NullContainingCategoryName;
            result.IntervalsAllowed = input.IntervalsAllowed;
            result.DbDataType = input.DbDataType;

            List<CategorySerializable<Int32>> categories
                = new List<CategorySerializable<Int32>>();
            foreach (CategorySerializable<T> cat in input.Categories)
            {
                CategorySerializable<Int32> _cat =
                    new CategorySerializable<Int32>();

                _cat.Enumeration = new Int32[cat.Enumeration.Length];
                _cat.Intervals = new IntervalSerializable<Int32>[cat.Intervals.Length];

                int i = 0;
                foreach (T value in cat.Enumeration)
                {
                    _cat.Enumeration[i] = Convert.ToInt32(value);
                    i++;
                }

                i = 0;
                foreach (IntervalSerializable<T> _interval in cat.Intervals)
                {
                    _cat.Intervals[i] = new IntervalSerializable<int>();
                    _cat.Intervals[i].LeftBoundary = _interval.LeftBoundary;
                    _cat.Intervals[i].RightBoundary = _interval.RightBoundary;
                    _cat.Intervals[i].LeftValue = Convert.ToInt32(_interval.LeftValue);
                    _cat.Intervals[i].RightValue = Convert.ToInt32(_interval.RightValue);
                    i++;
                }

                _cat.Name = cat.Name;
                _cat.OrdNumber = cat.OrdNumber;
                categories.Add(_cat);
            }

            result.Categories = categories.ToArray();

            return result;
        }

        /// <summary>
        /// Retypes to long
        /// </summary>
        /// <param name="input">Attribute to retype</param>
        /// <returns>Retyped attribute</returns>
        public static AttributeSerializable<Int64> RetypeToLong(AttributeSerializable<T> input)
        {
            AttributeSerializable<Int64> result =
                new AttributeSerializable<Int64>();

            result.NullContainingCategoryName
                        = input.NullContainingCategoryName;
            result.IntervalsAllowed = input.IntervalsAllowed;
            result.DbDataType = input.DbDataType;

            List<CategorySerializable<Int64>> categories
                = new List<CategorySerializable<Int64>>();
            foreach (CategorySerializable<T> cat in input.Categories)
            {
                CategorySerializable<Int64> _cat =
                    new CategorySerializable<Int64>();

                _cat.Enumeration = new Int64[cat.Enumeration.Length];
                _cat.Intervals = new IntervalSerializable<Int64>[cat.Intervals.Length];

                int i = 0;
                foreach (T value in cat.Enumeration)
                {
                    _cat.Enumeration[i] = Convert.ToInt64(value);
                    i++;
                }

                i = 0;
                foreach (IntervalSerializable<T> _interval in cat.Intervals)
                {
                    _cat.Intervals[i] = new IntervalSerializable<long>();
                    _cat.Intervals[i].LeftBoundary = _interval.LeftBoundary;
                    _cat.Intervals[i].RightBoundary = _interval.RightBoundary;
                    _cat.Intervals[i].LeftValue = Convert.ToInt64(_interval.LeftValue);
                    _cat.Intervals[i].RightValue = Convert.ToInt64(_interval.RightValue);
                    i++;
                }

                _cat.Name = cat.Name;
                _cat.OrdNumber = cat.OrdNumber;
                categories.Add(_cat);
            }

            result.Categories = categories.ToArray();

            return result;
        }

        /// <summary>
        /// Retypes to float
        /// </summary>
        /// <param name="input">Attribute to retype</param>
        /// <returns>Retyped attribute</returns>
        public static AttributeSerializable<Single> RetypeToSingle(AttributeSerializable<T> input)
        {
            AttributeSerializable<Single> result =
                new AttributeSerializable<Single>();

            result.NullContainingCategoryName
                        = input.NullContainingCategoryName;
            result.IntervalsAllowed = input.IntervalsAllowed;
            result.DbDataType = input.DbDataType;

            List<CategorySerializable<Single>> categories
                = new List<CategorySerializable<Single>>();
            foreach (CategorySerializable<T> cat in input.Categories)
            {
                CategorySerializable<Single> _cat =
                    new CategorySerializable<Single>();

                _cat.Enumeration = new Single[cat.Enumeration.Length];
                _cat.Intervals = new IntervalSerializable<Single>[cat.Intervals.Length];

                int i = 0;
                foreach (T value in cat.Enumeration)
                {
                    _cat.Enumeration[i] = Convert.ToSingle(value);
                    i++;
                }

                i = 0;
                foreach (IntervalSerializable<T> _interval in cat.Intervals)
                {
                    _cat.Intervals[i] = new IntervalSerializable<float>();
                    _cat.Intervals[i].LeftBoundary = _interval.LeftBoundary;
                    _cat.Intervals[i].RightBoundary = _interval.RightBoundary;
                    _cat.Intervals[i].LeftValue = Convert.ToSingle(_interval.LeftValue);
                    _cat.Intervals[i].RightValue = Convert.ToSingle(_interval.RightValue);
                    i++;
                }

                _cat.Name = cat.Name;
                _cat.OrdNumber = cat.OrdNumber;
                categories.Add(_cat);
            }

            result.Categories = categories.ToArray();

            return result;
        }

        /// <summary>
        /// Retypes to double
        /// </summary>
        /// <param name="input">Attribute to retype</param>
        /// <returns>Retyped attribute</returns>
        public static AttributeSerializable<Double> RetypeToDouble(AttributeSerializable<T> input)
        {
            AttributeSerializable<Double> result =
                new AttributeSerializable<Double>();

            result.NullContainingCategoryName
                        = input.NullContainingCategoryName;
            result.IntervalsAllowed = input.IntervalsAllowed;
            result.DbDataType = input.DbDataType;

            List<CategorySerializable<Double>> categories
                = new List<CategorySerializable<Double>>();
            foreach (CategorySerializable<T> cat in input.Categories)
            {
                CategorySerializable<Double> _cat =
                    new CategorySerializable<Double>();

                _cat.Enumeration = new Double[cat.Enumeration.Length];
                _cat.Intervals = new IntervalSerializable<Double>[cat.Intervals.Length];

                int i = 0;
                foreach (T value in cat.Enumeration)
                {
                    _cat.Enumeration[i] = Convert.ToDouble(value);
                    i++;
                }

                i = 0;
                foreach (IntervalSerializable<T> _interval in cat.Intervals)
                {
                    _cat.Intervals[i] = new IntervalSerializable<double>();
                    _cat.Intervals[i].LeftBoundary = _interval.LeftBoundary;
                    _cat.Intervals[i].RightBoundary = _interval.RightBoundary;
                    _cat.Intervals[i].LeftValue = Convert.ToDouble(_interval.LeftValue);
                    _cat.Intervals[i].RightValue = Convert.ToDouble(_interval.RightValue);
                    i++;
                }

                _cat.Name = cat.Name;
                _cat.OrdNumber = cat.OrdNumber;
                categories.Add(_cat);
            }

            result.Categories = categories.ToArray();

            return result;
        }

        /// <summary>
        /// Retypes to string
        /// </summary>
        /// <param name="input">Attribute to retype</param>
        /// <returns>Retyped attribute</returns>
        public static AttributeSerializable<String> RetypeToString(AttributeSerializable<T> input)
        {
            AttributeSerializable<String> result =
                new AttributeSerializable<String>();

            result.NullContainingCategoryName
                        = input.NullContainingCategoryName;
            result.IntervalsAllowed = input.IntervalsAllowed;
            result.DbDataType = input.DbDataType;

            List<CategorySerializable<String>> categories
                = new List<CategorySerializable<String>>();
            foreach (CategorySerializable<T> cat in input.Categories)
            {
                CategorySerializable<String> _cat =
                    new CategorySerializable<String>();

                _cat.Enumeration = new String[cat.Enumeration.Length];
                _cat.Intervals = new IntervalSerializable<String>[cat.Intervals.Length];

                int i = 0;
                foreach (T value in cat.Enumeration)
                {
                    _cat.Enumeration[i] = value.ToString();
                    i++;
                }

                i = 0;
                foreach (IntervalSerializable<T> _interval in cat.Intervals)
                {
                    _cat.Intervals[i] = new IntervalSerializable<string>();
                    _cat.Intervals[i].LeftBoundary = _interval.LeftBoundary;
                    _cat.Intervals[i].RightBoundary = _interval.RightBoundary;
                    _cat.Intervals[i].LeftValue = _interval.LeftValue.ToString();
                    _cat.Intervals[i].RightValue = _interval.RightValue.ToString();
                    i++;
                }

                _cat.Name = cat.Name;
                _cat.OrdNumber = cat.OrdNumber;
                categories.Add(_cat);
            }

            result.Categories = categories.ToArray();

            return result;
        }

        /// <summary>
        /// Retypes to DateTime
        /// </summary>
        /// <param name="input">Attribute to retype</param>
        /// <returns>Retyped attribute</returns>
        public static AttributeSerializable<DateTime> RetypeToDateTime(AttributeSerializable<T> input)
        {
            AttributeSerializable<DateTime> result =
                new AttributeSerializable<DateTime>();

            result.NullContainingCategoryName
                        = input.NullContainingCategoryName;
            result.IntervalsAllowed = input.IntervalsAllowed;
            result.DbDataType = input.DbDataType;

            List<CategorySerializable<DateTime>> categories
                = new List<CategorySerializable<DateTime>>();
            foreach (CategorySerializable<T> cat in input.Categories)
            {
                CategorySerializable<DateTime> _cat =
                    new CategorySerializable<DateTime>();

                _cat.Enumeration = new DateTime[cat.Enumeration.Length];
                _cat.Intervals = new IntervalSerializable<DateTime>[cat.Intervals.Length];

                int i = 0;
                foreach (T value in cat.Enumeration)
                {
                    _cat.Enumeration[i] = Convert.ToDateTime(value);
                    i++;
                }

                i = 0;
                foreach (IntervalSerializable<T> _interval in cat.Intervals)
                {
                    _cat.Intervals[i] = new IntervalSerializable<DateTime>();
                    _cat.Intervals[i].LeftBoundary = _interval.LeftBoundary;
                    _cat.Intervals[i].RightBoundary = _interval.RightBoundary;
                    _cat.Intervals[i].LeftValue = Convert.ToDateTime(_interval.LeftValue);
                    _cat.Intervals[i].RightValue = Convert.ToDateTime(_interval.RightValue);
                    i++;
                }

                _cat.Name = cat.Name;
                _cat.OrdNumber = cat.OrdNumber;
                categories.Add(_cat);
            }

            result.Categories = categories.ToArray();

            return result;
        }
    }


    /// <summary>
    /// Serializable form of generic interval.
    /// </summary>
    /// <typeparam name="T">See typeparam in <see cref="T:Ferda.Guha.Attribute.Attribute`1"/></typeparam>
    [Serializable()]
    [XmlInclude(typeof(Int16)),
        XmlInclude(typeof(UInt16)),
        XmlInclude(typeof(Int32)),
        XmlInclude(typeof(UInt32)),
        XmlInclude(typeof(Int64)),
        XmlInclude(typeof(UInt64)),
        XmlInclude(typeof(Single)),
        XmlInclude(typeof(Double)),
        XmlInclude(typeof(Decimal)),
        XmlInclude(typeof(Boolean)),
        XmlInclude(typeof(String)),
        XmlInclude(typeof(DateTime)),
        XmlInclude(typeof(TimeSpan))]
    public class IntervalSerializable<T>
        where T : IComparable
    {
        /// <summary>
        /// Left value.
        /// </summary>
        public T LeftValue;

        /// <summary>
        /// Right value.
        /// </summary>
        public T RightValue;

        /// <summary>
        /// Left boundary.
        /// </summary>
        public BoundaryEnum LeftBoundary;

        /// <summary>
        /// Right boundary.
        /// </summary>
        public BoundaryEnum RightBoundary;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Ferda.Guha.Attribute.IntervalSerializable`1"/> class.
        /// </summary>
        public IntervalSerializable()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Ferda.Guha.Attribute.IntervalSerializable`1"/> class.
        /// </summary>
        /// <param name="leftValue">The left value.</param>
        /// <param name="rightValue">The right value.</param>
        /// <param name="leftBoundary">The left boundary.</param>
        /// <param name="rightBoundary">The right boundary.</param>
        public IntervalSerializable(T leftValue, T rightValue, BoundaryEnum leftBoundary, BoundaryEnum rightBoundary)
        {
            LeftValue = leftValue;
            RightValue = rightValue;
            LeftBoundary = leftBoundary;
            RightBoundary = rightBoundary;
        }
    }

    /// <summary>
    /// Serializabled form of generic category.
    /// </summary>
    /// <typeparam name="T">See typeparam in <see cref="T:Ferda.Guha.Attribute.Attribute`1"/></typeparam>
    [Serializable()]
    [XmlInclude(typeof(Int16)),
        XmlInclude(typeof(UInt16)),
        XmlInclude(typeof(Int32)),
        XmlInclude(typeof(UInt32)),
        XmlInclude(typeof(Int64)),
        XmlInclude(typeof(UInt64)),
        XmlInclude(typeof(Single)),
        XmlInclude(typeof(Double)),
        XmlInclude(typeof(Decimal)),
        XmlInclude(typeof(Boolean)),
        XmlInclude(typeof(String)),
        XmlInclude(typeof(DateTime)),
        XmlInclude(typeof(TimeSpan))]
    public class CategorySerializable<T>
        where T : IComparable
    {
        /// <summary>
        /// Category name.
        /// </summary>
        public string Name;

        /// <summary>
        /// Category ord number.
        /// </summary>
        public int OrdNumber;

        /// <summary>
        /// Enumeration of values (for discrete values).
        /// </summary>
        public T[] Enumeration;

        /// <summary>
        /// Array of intervals.
        /// </summary>
        public IntervalSerializable<T>[] Intervals;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Ferda.Guha.Attribute.CategorySerializable`1"/> class.
        /// </summary>
        public CategorySerializable()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Ferda.Guha.Attribute.CategorySerializable`1"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="ordNumber">The ord number.</param>
        /// <param name="enumeration">The enumeration.</param>
        /// <param name="intervals">The intervals.</param>
        public CategorySerializable(string name, int ordNumber, T[] enumeration, IntervalSerializable<T>[] intervals)
        {
            Name = name;
            OrdNumber = ordNumber;
            Enumeration = enumeration;
            Intervals = intervals;
        }
    }

    /// <summary>
    /// Serializabled form of generic attribute.
    /// </summary>
    /// <typeparam name="T">See typeparam in <see cref="T:Ferda.Guha.Attribute.Attribute`1"/></typeparam>
    [Serializable()]
    [XmlInclude(typeof(Int16)),
        XmlInclude(typeof(UInt16)),
        XmlInclude(typeof(Int32)),
        XmlInclude(typeof(UInt32)),
        XmlInclude(typeof(Int64)),
        XmlInclude(typeof(UInt64)),
        XmlInclude(typeof(Single)),
        XmlInclude(typeof(Double)),
        XmlInclude(typeof(Decimal)),
        XmlInclude(typeof(Boolean)),
        XmlInclude(typeof(String)),
        XmlInclude(typeof(DateTime)),
        XmlInclude(typeof(TimeSpan))]

    public class AttributeSerializable<T>
        where T : IComparable
    {
        /// <summary>
        /// Array of categories.
        /// </summary>
        public CategorySerializable<T>[] Categories;

        /// <summary>
        /// Name of category containing null.
        /// </summary>
        public string NullContainingCategoryName;

        /// <summary>
        /// Intervals allowed.
        /// </summary>
        public bool IntervalsAllowed;

        /// <summary>
        /// Data type of values i.e. of T.
        /// </summary>
        public DbSimpleDataTypeEnum DbDataType;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Ferda.Guha.Attribute.AttributeSerializable`1"/> class.
        /// </summary>
        public AttributeSerializable()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Ferda.Guha.Attribute.AttributeSerializable`1"/> class.
        /// </summary>
        /// <param name="categories">The categories.</param>
        /// <param name="nullContainingCategoryName">Name of the null containing category.</param>
        /// <param name="intervalsAllowed">if set to <c>true</c> intervals are allowed.</param>
        /// <param name="dbDataType">Type of the db data.</param>
        public AttributeSerializable(CategorySerializable<T>[] categories, string nullContainingCategoryName, bool intervalsAllowed, DbSimpleDataTypeEnum dbDataType)
        {
            Categories = categories;
            NullContainingCategoryName = nullContainingCategoryName;
            IntervalsAllowed = intervalsAllowed;
            DbDataType = dbDataType;
        }

    }


    /// <summary>
    /// Provides serialize and deserialize method for serializable
    /// form of generic attribute. See <see cref="T:Ferda.Guha.Attribute.AttributeSerializable`1"/>.
    /// </summary>
    public static class Serializer
    {
        /// <summary>
        /// Serializes the specified attribute.
        /// </summary>
        /// <param name="attributeSerializable">The serializable attribute.</param>
        /// <returns></returns>
        public static string Serialize<T>(AttributeSerializable<T> attributeSerializable)
            where T : IComparable
        {
            XmlSerializer serializer;
            StringBuilder sb = new StringBuilder();

            switch (attributeSerializable.DbDataType)
            {
                case DbSimpleDataTypeEnum.BooleanSimpleType:
                    serializer = new XmlSerializer(typeof(AttributeSerializable<Boolean>));
                    using (
                    StringWriter writer = new StringWriter(sb)
                    )
                    {
                        serializer.Serialize(writer,
                            Retyper<T, T>.RetypeToBool(attributeSerializable));
                        return sb.ToString();
                    }

                case DbSimpleDataTypeEnum.ShortSimpleType:
                    serializer = new XmlSerializer(typeof(AttributeSerializable<Int16>));
                    using (
                    StringWriter writer = new StringWriter(sb)
                    )
                    {
                        serializer.Serialize(writer,
                            Retyper<T, T>.RetypeToShort(attributeSerializable));
                        return sb.ToString();
                    }

                case DbSimpleDataTypeEnum.IntegerSimpleType:
                    serializer = new XmlSerializer(typeof(AttributeSerializable<Int32>));
                    using (
                    StringWriter writer = new StringWriter(sb)
                    )
                    {
                        serializer.Serialize(writer,
                            Retyper<T, T>.RetypeToInt(attributeSerializable));
                        return sb.ToString();
                    }

                case DbSimpleDataTypeEnum.DoubleSimpleType:
                    serializer = new XmlSerializer(typeof(AttributeSerializable<Double>));
                    using (
                    StringWriter writer = new StringWriter(sb)
                    )
                    {
                        serializer.Serialize(writer,
                            Retyper<T, T>.RetypeToDouble(attributeSerializable));
                        return sb.ToString();
                    }

                case DbSimpleDataTypeEnum.FloatSimpleType:
                    serializer = new XmlSerializer(typeof(AttributeSerializable<Single>));
                    using (
                    StringWriter writer = new StringWriter(sb)
                    )
                    {
                        serializer.Serialize(writer,
                            Retyper<T, T>.RetypeToSingle(attributeSerializable));
                        return sb.ToString();
                    }

                case DbSimpleDataTypeEnum.LongSimpleType:
                    serializer = new XmlSerializer(typeof(AttributeSerializable<Int64>));
                    using (
                    StringWriter writer = new StringWriter(sb)
                    )
                    {
                        serializer.Serialize(writer,
                            Retyper<T, T>.RetypeToLong(attributeSerializable));
                        return sb.ToString();
                    }

                case DbSimpleDataTypeEnum.StringSimpleType:
                    serializer = new XmlSerializer(typeof(AttributeSerializable<String>));
                    using (
                    StringWriter writer = new StringWriter(sb)
                    )
                    {
                        serializer.Serialize(writer,
                            Retyper<T, T>.RetypeToString(attributeSerializable));
                        return sb.ToString();
                    }

                case DbSimpleDataTypeEnum.DateTimeSimpleType:
                    serializer = new XmlSerializer(typeof(AttributeSerializable<DateTime>));
                    using (
                    StringWriter writer = new StringWriter(sb)
                    )
                    {
                        serializer.Serialize(writer,
                            Retyper<T, T>.RetypeToDateTime(attributeSerializable));
                        return sb.ToString();
                    }

                case DbSimpleDataTypeEnum.TimeSimpleType:
                    serializer = new XmlSerializer(typeof(AttributeSerializable<TimeSpan>));
                    return null;

                default:
                    throw new ArgumentException();
            }
        }

        /// <summary>
        /// Deserializes the specified serialized attribute.
        /// </summary>
        /// <param name="serializedAttributeSerializable">The serialized attribute.</param>
        /// <returns></returns>
        public static AttributeSerializable<T> Deserialize<T>(string serializedAttributeSerializable)
            where T : IComparable
        {
            using (
            StringReader reader = new StringReader(serializedAttributeSerializable)
            )
            {
                XmlSerializer deserealizer = new XmlSerializer(typeof(AttributeSerializable<T>));
                object deserealized = deserealizer.Deserialize(reader);
                return (AttributeSerializable<T>)deserealized;
            }
        }

        //public static string Serialize<T>(AttributeSerializable<T> attributeSerializable, DbSimpleDataTypeEnum dataType)
        //    where T : IComparable
        //{
        //    string serializedAttribute = Serialize(attributeSerializable);
        //    AttributeSerialization result = new AttributeSerialization();
        //    result.DataType = dataType;
        //    result.SerializedAttribute = serializedAttribute;

        //    XmlSerializer serializer = new XmlSerializer(typeof(AttributeSerialization));
        //    StringBuilder sb = new StringBuilder();
        //    StringWriter writer = new StringWriter(sb);
        //    serializer.Serialize(writer, result);
        //    writer.Dispose(); 
        //    return sb.ToString();
        //}

        /// <summary>
        /// Deserializes a serialized attribute and retypes it into 
        /// <see cref="System.IComparable"/> form according to a 
        /// column type
        /// </summary>
        /// <param name="serializedAttribute">Serialized attribute</param>
        /// <param name="columnType">Type of column to which the attribute
        /// should be serialized</param>
        /// <returns>Deserialized attribute</returns>
        public static Attribute<IComparable> RetypeAttributeSerializable(
            string serializedAttribute, DbDataTypeEnum columnType)
        {
            Attribute<IComparable> tmp;
            switch (columnType)
            {
                case DbDataTypeEnum.BooleanType:
                    tmp =
                        Retyper<IComparable, Boolean>.ToIComparable(
                        Deserialize<Boolean>(serializedAttribute));
                    break;

                case DbDataTypeEnum.DateTimeType:
                    tmp =
                        Retyper<IComparable, DateTime>.ToIComparable(
                        Deserialize<DateTime>(serializedAttribute));
                    break;

                case DbDataTypeEnum.DoubleType:
                    tmp =
                        Retyper<IComparable, Double>.ToIComparable(
                        Deserialize<Double>(serializedAttribute));
                    break;

                case DbDataTypeEnum.FloatType:
                case DbDataTypeEnum.DecimalType:
                    tmp =
                        Retyper<IComparable, Single>.ToIComparable(
                        Deserialize<Single>(serializedAttribute));
                    break;


                case DbDataTypeEnum.IntegerType:
                case DbDataTypeEnum.UnsignedIntegerType:
                    tmp =
                       Retyper<IComparable, Int32>.ToIComparable(
                        Deserialize<Int32>(serializedAttribute));
                    break;

                case DbDataTypeEnum.ShortIntegerType:
                case DbDataTypeEnum.UnsignedShortIntegerType:
                    tmp =
                       Retyper<IComparable, Int16>.ToIComparable(
                        Deserialize<Int16>(serializedAttribute));
                    break;

                case DbDataTypeEnum.LongIntegerType:
                case DbDataTypeEnum.UnsignedLongIntegerType:
                    tmp =
                       Retyper<IComparable, Int64>.ToIComparable(
                        Deserialize<Int64>(serializedAttribute));
                    break;

                case DbDataTypeEnum.StringType:
                    tmp =
                       Retyper<IComparable, String>.ToIComparable(
                        Deserialize<String>(serializedAttribute));
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            return tmp;
        }
    }
}
