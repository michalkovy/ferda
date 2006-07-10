using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Ferda.Guha.Data;

namespace Ferda.Guha.Attribute
{
    /// <summary>
    /// Serializable form of generic interval.
    /// </summary>
    /// <typeparam name="T">See typeparam in <see cref="T:Ferda.Guha.Attribute.Attribute{T}"/></typeparam>
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
        /// Initializes a new instance of the <see cref="T:Ferda.Guha.Attribute.IntervalSerializable&lt;T&gt;"/> class.
        /// </summary>
        public IntervalSerializable()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Ferda.Guha.Attribute.IntervalSerializable&lt;T&gt;"/> class.
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
    /// <typeparam name="T">See typeparam in <see cref="T:Ferda.Guha.Attribute.Attribute{T}"/></typeparam>
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
        /// Initializes a new instance of the <see cref="T:Ferda.Guha.Attribute.CategorySerializable&lt;T&gt;"/> class.
        /// </summary>
        public CategorySerializable()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Ferda.Guha.Attribute.CategorySerializable&lt;T&gt;"/> class.
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
    /// <typeparam name="T">See typeparam in <see cref="T:Ferda.Guha.Attribute.Attribute{T}"/></typeparam>
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
        /// Initializes a new instance of the <see cref="T:Ferda.Guha.Attribute.AttributeSerializable&lt;T&gt;"/> class.
        /// </summary>
        public AttributeSerializable()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Ferda.Guha.Attribute.AttributeSerializable&lt;T&gt;"/> class.
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
    /// form of generic attribute. See <see cref="T:Ferda.Guha.Attribute.AttributeSerializable{T}"/>.
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
            XmlSerializer serializer = new XmlSerializer(typeof(AttributeSerializable<T>));
            StringBuilder sb = new StringBuilder();
            using (
            StringWriter writer = new StringWriter(sb)
            )
            {
                serializer.Serialize(writer, attributeSerializable);
                return sb.ToString();
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
