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
    /// <typeparam name="T">See typeparam in <see cref="T:Ferda.Guha.Attribute.Attribute`1"/></typeparam></typeparam>
    public class Retyper<T>
        where T : IComparable
    {
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
                            Retyper<T>.RetypeToBool(attributeSerializable));
                        return sb.ToString();
                    }

                case DbSimpleDataTypeEnum.ShortSimpleType:
                    serializer = new XmlSerializer(typeof(AttributeSerializable<Int16>));
                    using (
                    StringWriter writer = new StringWriter(sb)
                    )
                    {
                        serializer.Serialize(writer,
                            Retyper<T>.RetypeToShort(attributeSerializable));
                        return sb.ToString();
                    }

                case DbSimpleDataTypeEnum.IntegerSimpleType:
                    serializer = new XmlSerializer(typeof(AttributeSerializable<Int32>));
                    using (
                    StringWriter writer = new StringWriter(sb)
                    )
                    {
                        serializer.Serialize(writer,
                            Retyper<T>.RetypeToInt(attributeSerializable));
                        return sb.ToString();
                    }

                case DbSimpleDataTypeEnum.DoubleSimpleType:
                    serializer = new XmlSerializer(typeof(AttributeSerializable<Double>));
                    using (
                    StringWriter writer = new StringWriter(sb)
                    )
                    {
                        serializer.Serialize(writer,
                            Retyper<T>.RetypeToDouble(attributeSerializable));
                        return sb.ToString();
                    }

                case DbSimpleDataTypeEnum.FloatSimpleType:
                    serializer = new XmlSerializer(typeof(AttributeSerializable<Single>));
                    using (
                    StringWriter writer = new StringWriter(sb)
                    )
                    {
                        serializer.Serialize(writer,
                            Retyper<T>.RetypeToSingle(attributeSerializable));
                        return sb.ToString();
                    }

                case DbSimpleDataTypeEnum.LongSimpleType:
                    serializer = new XmlSerializer(typeof(AttributeSerializable<Int64>));
                    using (
                    StringWriter writer = new StringWriter(sb)
                    )
                    {
                        serializer.Serialize(writer,
                            Retyper<T>.RetypeToLong(attributeSerializable));
                        return sb.ToString();
                    }

                case DbSimpleDataTypeEnum.StringSimpleType:
                    serializer = new XmlSerializer(typeof(AttributeSerializable<String>));
                    using (
                    StringWriter writer = new StringWriter(sb)
                    )
                    {
                        serializer.Serialize(writer,
                            Retyper<T>.RetypeToString(attributeSerializable));
                        return sb.ToString();
                    }

                case DbSimpleDataTypeEnum.DateTimeSimpleType:
                    serializer = new XmlSerializer(typeof(AttributeSerializable<DateTime>));
                    using (
                    StringWriter writer = new StringWriter(sb)
                    )
                    {
                        serializer.Serialize(writer,
                            Retyper<T>.RetypeToDateTime(attributeSerializable));
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
    }

    //[Serializable()]
    //public class AttributeSerialization
    //{
    //    public DbSimpleDataTypeEnum DataType;
    //    public string SerializedAttribute;
    //}
}
