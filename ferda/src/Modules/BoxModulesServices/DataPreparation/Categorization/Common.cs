// Common.cs - common functionality for categorization boxes
//
// Author: Alexander Kuzmin <alexander.kuzmin@gmail.com>
//
// Copyright (c) 2007 Alexander Kuzmin
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
using Ferda.Guha.Data;

namespace Ferda.Modules.Boxes.DataPreparation.Categorization
{
    /// <summary>
    /// Class for retyping arrays
    /// </summary>
    /// <typeparam name="T">Type of element in the array</typeparam>
    internal static class Retyper<T>
    {
        public delegate T ToTypeDelegate(string input);

        /// <summary>
        /// Retypes array to array of objects
        /// </summary>
        /// <param name="input">Input array of defined type</param>
        /// <returns>Array of object</returns>
        public static object[] Retype(T[] input)
        {
            object[] result = new object[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                result[i] = (object)input[i];
            }
            return result;
        }

        /// <summary>
        /// Retypes array of longs to array of ints
        /// </summary>
        /// <param name="input">Input array of longs</param>
        /// <returns>Array of ints</returns>
        public static int[] RetypeLongToInt(long[] input)
        {
            int[] result = new int[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                result[i] = (int)input[i];
            }
            return result;
        }

        /// <summary>
        /// Retypes array of longs to array of shorts
        /// </summary>
        /// <param name="input">Input array of longs</param>
        /// <returns>Array of shorts</returns>
        public static short[] RetypeLongToShort(long[] input)
        {
            short[] result = new short[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                result[i] = (short)input[i];
            }
            return result;
        }

        /// <summary>
        /// Retypes array of doubles to array of floats
        /// </summary>
        /// <param name="input">Input array of doubles</param>
        /// <returns>Array of floats</returns>
        public static float[] RetypeDoubleToFloat(double[] input)
        {
            float[] result = new float[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                result[i] = (float)input[i];
            }
            return result;
        }

        /// <summary>
        /// Method that retrieves the devision points for equifrequency intervals
        /// and retypes them to the correct datatype
        /// </summary>
        /// <param name="dt">Datatable to count values frequency from</param>
        /// <param name="converter">Delegate for conversion to the correct type</param>
        /// <param name="count">Requested count of intervals</param>
        /// <returns></returns>
        public static T[] PrepareForEquifrequency(System.Data.DataTable dt, ToTypeDelegate converter, int count)
        {
            List<
            Guha.Attribute.DynamicAlgorithm.ValueFrequencyPair<T>> enumeration =
                new List<Guha.Attribute.DynamicAlgorithm.ValueFrequencyPair<T>>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                object v = dt.Rows[i][0];
                if (v == null || v is DBNull)
                    continue;
                Guha.Attribute.DynamicAlgorithm.ValueFrequencyPair<T> tmpItem =
                    new Guha.Attribute.DynamicAlgorithm.ValueFrequencyPair<T>(
                    converter(dt.Rows[i][0].ToString()),
                    Convert.ToInt32(dt.Rows[i][1]));
                enumeration.Add(tmpItem);
            }

            T[] divisionPoints;

                divisionPoints =
                    Guha.Attribute.DynamicAlgorithm.EquifrequencyIntervals.GenerateIntervals(
                    count, enumeration.ToArray());

            return divisionPoints;

        }

        /// <summary>
        /// Method that converts the string with values separated by separator into a sorted array of T values.
        /// </summary>
        /// <param name="inputString">String with values, which are separated by separator</param>
        /// <param name="separator">Separator</param>
        /// <param name="converter">Delegate for conversion to the correct type</param>
        /// <returns></returns>
        public static T[] ConvertStringToCustomTypedSortedArray(string inputString, string separator, ToTypeDelegate converter)
        {
            /// split the string of domain dividing values separated by separator
            string[] tmpStringArray = inputString.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            T[] outputArray = new T[tmpStringArray.Length];

            int i = 0;
            foreach (string tmpString in tmpStringArray)
            {
                try
                {
                    /// converting the value into target data type
                    outputArray[i] = converter(tmpString);
                }
                catch
                {
                    throw new ArgumentException("Unable to convert value " + tmpString + " into target datatype");
                }
                i++;
            }

            /// sorting the array
            Array.Sort<T>(outputArray);

            return outputArray;
        }

        
        /// <summary>
        /// Method that returns the minimal value for attribute creation
        /// (it specifies boundaries of the domain)
        /// </summary>
        /// <param name="ontologyMin">Minimal value set by ontology</param>
        /// <param name="dataMin">Minimal value in database</param>
        /// <param name="domainDividingValueMin">Minimal domain dividing value</param>
        /// <param name="distinctValueMin">Minimal distinct value</param>
        /// <param name="converter">Delegate for conversion to the correct type</param>
        /// <returns></returns>
        public static T returnMinValue(string ontologyMin, string dataMin, 
            string domainDividingValueMin, string distinctValueMin, ToTypeDelegate converter)
        {
            T __min;

            if (ontologyMin != "")
            {
                T _ontologyMin = converter(ontologyMin);
                /// if true: properties from ontology are in conflict
                /// domain dividing values can't be lower than specified minimal value
                if (domainDividingValueMin != "" &&
                    (Comparer<T>.Default.Compare(converter(domainDividingValueMin), _ontologyMin) < 0))
                {
                    throw new ArgumentException("Ontology derived properties are in conflict. Some of the domain dividing values is lower than allowed minimum. The problem may be in ontology, but maybe you have changed the values manually.");
                }
                /// if true: properties from ontology are in conflict
                /// domain dividing values can't be lower than specified minimal value
                else if (distinctValueMin != "" &&
                    (Comparer<T>.Default.Compare(converter(distinctValueMin), _ontologyMin) < 0))
                {
                    throw new ArgumentException("Ontology derived properties are in conflict. Some of the domain dividing values is lower than allowed minimum. The problem may be in ontology, but maybe you have changed the values manually.");
                }
                /// if true: column contains unallowed values,
                /// values must be greater than specified minimal value
                
                /// TODO what is desired behaviour for user?
                /// 1) To throw error when in database is some bad value (next IF not in comment)
                /// 2) To ignore bad values (next IF in comment)
                /*else if (Comparer<T>.Default.Compare(converter(dataMin), _ontologyMin) < 0)
                {
                    throw new ArgumentException("There are unallowed values in the database for this column.");
                }*/

                /// ontology property minimum is set and no other properties are in conflict
                else
                {
                    __min = _ontologyMin;
                }
            }
            else
            /// Minimum property is not set
            /// mimimal value is the minimal value of {minimal value from the dataColumn, domainDividingValueMin, distinctValueMin}
            {
                T tmpMin = converter(dataMin);

                if (distinctValueMin != "" &&
                    (Comparer<T>.Default.Compare(converter(distinctValueMin), tmpMin) < 0))
                {
                    tmpMin = converter(distinctValueMin);
                }

                if (domainDividingValueMin != "" &&
                    (Comparer<T>.Default.Compare(converter(domainDividingValueMin), tmpMin) < 0))
                {
                    tmpMin = converter(domainDividingValueMin);
                }

                __min = tmpMin;
            }
            return __min;
        }

        /// <summary>
        /// Method that returns the maximal value for attribute creation
        /// (it specifies boundaries of the domain)
        /// </summary>
        /// <param name="ontologyMax">Maximal value set by ontology</param>
        /// <param name="dataMax">Maximal value in database</param>
        /// <param name="domainDividingValueMax">Maximal domain dividing value</param>
        /// <param name="distinctValueMax">Maximal distinct value</param>
        /// <param name="converter">Delegate for conversion to the correct type</param>
        /// <returns></returns>
        public static T returnMaxValue(string ontologyMax, string dataMax,
            string domainDividingValueMax, string distinctValueMax, ToTypeDelegate converter)
        {
            T __max;

            if (ontologyMax != "")
            {
                T _ontologyMax = converter(ontologyMax);
                /// if true: properties from ontology are in conflict
                /// domain dividing values can't be greater than specified maximal value
                if (domainDividingValueMax != "" &&
                    (Comparer<T>.Default.Compare(converter(domainDividingValueMax), _ontologyMax) > 0))
                {
                    throw new ArgumentException("Ontology derived properties are in conflict. Some of the domain dividing values is lower than allowed maximum. The problem may be in ontology, but maybe you have changed the values manually.");
                }
                /// if true: properties from ontology are in conflict
                /// domain dividing values can't be greater than specified maximal value
                else if (distinctValueMax != "" &&
                    (Comparer<T>.Default.Compare(converter(distinctValueMax), _ontologyMax) > 0))
                {
                    throw new ArgumentException("Ontology derived properties are in conflict. Some of the domain dividing values is lower than allowed maximum. The problem may be in ontology, but maybe you have changed the values manually.");
                }
                /// if true: column contains unallowed values,
                /// values must be lower than specified maximal value
                                
                /// TODO what is desired behaviour for user?
                /// 1) To throw error when in database is some bad value (next IF not in comment)
                /// 2) To ignore bad values (next IF in comment)
                /*
                else if (Comparer<T>.Default.Compare(converter(dataMax), _ontologyMax) > 0)
                {
                    throw new ArgumentException("There are unallowed values in the database for this column.");
                }*/

                /// ontology property maximum is set and no other properties are in conflict
                else
                {
                    __max = _ontologyMax;
                }
            }
            else
            /// Maximum property is not set
            /// mimimal value is the Maximal value of {maximal value from the dataColumn, domainDividingValueMax, distinctValueMax}
            {
                T tmpMax = converter(dataMax);

                if (distinctValueMax != "" &&
                    (Comparer<T>.Default.Compare(converter(distinctValueMax), tmpMax) > 0))
                {
                    tmpMax = converter(distinctValueMax);
                }

                if (domainDividingValueMax != "" &&
                    (Comparer<T>.Default.Compare(converter(domainDividingValueMax), tmpMax) > 0))
                {
                    tmpMax = converter(domainDividingValueMax);
                }

                __max = tmpMax;
            }
            return __max;
        }
    }
}
