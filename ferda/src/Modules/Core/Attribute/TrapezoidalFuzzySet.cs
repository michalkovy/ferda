// Enumeration.cs - fuzzy set of the trapezoidal shape
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
using System.Xml.Serialization;
using System.IO;

namespace Ferda.Guha.Attribute
{
    /// <summary>
    /// A simplified representation of a trapezoidal fuzzy set. Only 4 points
    /// and a name of the fuzzy set is stored. 
    /// </summary>
    [Serializable()]
    public class TrapezoidalFuzzySet
    {
        /// <summary>
        /// Name of the fuzzy set
        /// </summary>
        public string Name;

        /// <summary>
        /// A (start of the trapezoid)
        /// </summary>
        public double A;

        /// <summary>
        /// B (end of the trapezoid)
        /// </summary>
        public double B;

        /// <summary>
        /// C (descending peak)
        /// </summary>
        public double C;

        /// <summary>
        /// D (ascending peak)
        /// </summary>
        public double D;

        /// <summary>
        /// Serializes the specified input.
        /// </summary>
        /// <param name="input">The trapezoidal fuzzy set to be serialized</param>
        /// <returns>Serialized version of the trapezoidal fuzzy set</returns>
        public static string Serialize(TrapezoidalFuzzySet input)
        {
            if (input == null)
            {
                return null;
            }
            XmlSerializer serializer = new XmlSerializer(typeof(TrapezoidalFuzzySet));
            StringBuilder sb = new StringBuilder();
            using (StringWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, input);
                return sb.ToString();
            }
        }

        /// <summary>
        /// Deserializes the specified input (string representing a
        /// <see cref="T:TrapezoidalFuzzySet"/>)
        /// </summary>
        /// <param name="input">The input string</param>
        /// <returns>Trapezoidal fuzzy set object</returns>
        public static TrapezoidalFuzzySet Deserialize(string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return null;
            }
            using (
                StringReader reader = new StringReader(input))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(TrapezoidalFuzzySet));
                object deserialized = deserializer.Deserialize(reader);
                return (TrapezoidalFuzzySet)deserialized;
            }
        }
    }

    /// <summary>
    /// Class representing array of trapezoidal fuzzy sets
    /// (for serialization purposes only).
    /// </summary>
    [Serializable()]
    public class TrapezoidalFuzzySets
    {
        /// <summary>
        /// The array of fuzzy sets
        /// </summary>
        public TrapezoidalFuzzySet[] fuzzySets;

        /// <summary>
        /// Serializes the specified input.
        /// </summary>
        /// <param name="input">The trapezoidal fuzzy sets to be serialized</param>
        /// <returns>Serialized version of the trapezoidal fuzzy sets</returns>
        public static string Serialize(TrapezoidalFuzzySets input)
        {
            if (input == null)
            {
                return null;
            }
            XmlSerializer serializer = new XmlSerializer(typeof(TrapezoidalFuzzySets));
            StringBuilder sb = new StringBuilder();
            using (StringWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, input);
                return sb.ToString();
            }
        }

        /// <summary>
        /// Deserializes the specified input (string representing a
        /// <see cref="T:TrapezoidalFuzzySets"/>)
        /// </summary>
        /// <param name="input">The input string</param>
        /// <returns>Trapezoidal fuzzy sets object</returns>
        public static TrapezoidalFuzzySets Deserialize(string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return null;
            }
            using (
                StringReader reader = new StringReader(input))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(TrapezoidalFuzzySets));
                object deserialized = deserializer.Deserialize(reader);
                return (TrapezoidalFuzzySets)deserialized;
            }
        }
    }
}
