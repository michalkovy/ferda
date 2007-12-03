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
        /// Method that returns the minimal and maximal value for attribute creation
        /// (it specifies global boundaries of the domain)
        /// </summary>
        /// <param name="dt">Datatable to count values frequency from</param>
        /// <param name="converter">Delegate for conversion to the correct type</param>
        /// <param name="count">Requested count of intervals</param>
        /// <returns></returns>
        public static T returnMinValue(string ontologyMin, string from, bool subdomain, string dataMin, 
            string domainDividingValueMin, string distinctValueMin, ToTypeDelegate converter)
        {
            T __min;

            
            //ontology derived property has the biggest priority
            //if the minimum is set, then it is the minimal allowed value
            if (ontologyMin != "")
            {
                __min = converter(ontologyMin);
            }
            //if ontology min value is not set, then the domain is bounded by from property
            else if (subdomain && from != "")
            {
                __min = converter(from);
            }
            /// neither from nor minimum property is set
            /// mimimal value is the minimal value from the dataColumn
            else
            {
                __min = converter(dataMin);
            }
            
            /*
            int i = new int();
            int j = new int();
            int k = new int();

            if (ontologyMin != "")
            {
                T _ontologyMin = converter(ontologyMin);
                /// if true: properties from ontology are in conflict
                /// domain dividing values can't be lower than specified minimal value
                if (domainDividingValueMin != "" && 
                    (i = Comparer<T>.Default.Compare(converter(domainDividingValueMin), _ontologyMin)) < 0)
                {
                    System.Windows.Forms.MessageBox.Show("i-" + i.ToString() + "j-" + j.ToString() + "k-" + k.ToString() + "\nERROR DDV je mensi nez ontomin!!! \n\nontomin: " + ontologyMin + "\n from" + from + "\n subdomain" + subdomain.ToString()
                            + "\ndataMin: " + dataMin + "\n domainDivValMin" + domainDividingValueMin + "\n distvalMin" + distinctValueMin);
                    throw new ArgumentException("Ontology derived properties are in conflict. Some of the domain dividing values is lower than allowed minimum. The problem may be in ontology, but maybe you have changed the values manually.");
                }
                /// if true: properties from ontology are in conflict
                /// domain dividing values can't be lower than specified minimal value
                else if (distinctValueMin != "" &&
                    (i = Comparer<T>.Default.Compare(converter(distinctValueMin), _ontologyMin)) < 0)
                {
                    System.Windows.Forms.MessageBox.Show("i-" + i.ToString() + "j-" + j.ToString() + "k-" + k.ToString() + "\nERROR DistVal je mensi nez ontomin!!! \n\nontomin: " + ontologyMin + "\n from" + from + "\n subdomain" + subdomain.ToString()
                            + "\ndataMin: " + dataMin + "\n domainDivValMin" + domainDividingValueMin + "\n distvalMin" + distinctValueMin);
                    throw new ArgumentException("Ontology derived properties are in conflict. Some of the domain dividing values is lower than allowed minimum. The problem may be in ontology, but maybe you have changed the values manually.");
                }
                /// if true: column contains unallowed values,
                /// values must be greater than specified minimal value
                else if ((j = Comparer<T>.Default.Compare(converter(dataMin), _ontologyMin)) < 0)
                {
                    System.Windows.Forms.MessageBox.Show("i-" + i.ToString() + "j-" + j.ToString() + "k-" + k.ToString() + "\nERROR datamin je mensi nez ontomin!!! \n\nontomin: " + ontologyMin + "\n from" + from + "\n subdomain" + subdomain.ToString()
                            + "\ndataMin: " + dataMin + "\n domainDivValMin" + domainDividingValueMin + "\n distvalMin" + distinctValueMin);
                    throw new ArgumentException("In the database there are unallowed values in the column.");
                }
                else if (subdomain && from != "")
                {
                    /// if true: user defined range of attribute is smaller
                    /// than it is allowed by ontology minimum property
                    /// the value is set to ontologyMin (in the column there are none values lower than ontologyMin - 
                    /// due one of the previous conditions)
                    if ((k = Comparer<T>.Default.Compare(converter(from), _ontologyMin)) < 0)
                    {
                        __min = _ontologyMin;
                        System.Windows.Forms.MessageBox.Show("i-" + i.ToString() + "j-" + j.ToString() + "k-" + k.ToString() + "\nmin je ONTOMIN: " + __min.ToString() + "\n\nontomin: " + ontologyMin + "\n from" + from + "\n subdomain" + subdomain.ToString()
                            + "\ndataMin: " + dataMin + "\n domainDivValMin" + domainDividingValueMin + "\n distvalMin" + distinctValueMin);
                    }
                    /// From property is set by user and is greater or equal to ontologyMin
                    else
                    {
                        __min = converter(from);
                        System.Windows.Forms.MessageBox.Show("i-" + i.ToString() + "j-" + j.ToString() + "k-" + k.ToString() + "\nmin je FROM: " + __min.ToString() + "\n\nontomin: " + ontologyMin + "\n from" + from + "\n subdomain" + subdomain.ToString()
                            + "\ndataMin: " + dataMin + "\n domainDivValMin" + domainDividingValueMin + "\n distvalMin" + distinctValueMin);
                    }
                }
                /// ontology property minimum is set and property From is not set
                else
                {
                    __min = _ontologyMin;
                    System.Windows.Forms.MessageBox.Show("i-" + i.ToString() + "j-" + j.ToString() + "k-" + k.ToString() + "\nmin je ONTOMIN: " + __min.ToString() + "\n\nontomin: " + ontologyMin + "\n from" + from + "\n subdomain" + subdomain.ToString()
                            + "\ndataMin: " + dataMin + "\n domainDivValMin" + domainDividingValueMin + "\n distvalMin" + distinctValueMin);
                }
            }
            else
            /// neither From nor Minimum property is set
            /// mimimal value is the minimal value from the dataColumn
            {
                __min = converter(dataMin);
                System.Windows.Forms.MessageBox.Show("i-" + i.ToString() + "j-" + j.ToString() + "k-" + k.ToString() + "\nmin je DATAMIN: " + __min.ToString() + "\n\nontomin: " + ontologyMin + "\n from" + from + "\n subdomain" + subdomain.ToString()
                            + "\ndataMin: " + dataMin + "\n domainDivValMin" + domainDividingValueMin + "\n distvalMin" + distinctValueMin);
            }*/

            return __min;
        }

        /// <summary>
        /// Method that returns the maximal value for attribute creation
        /// (it specifies right global boundary of the domain)
        /// </summary>
        /// <param name="dt">Datatable to count values frequency from</param>
        /// <param name="converter">Delegate for conversion to the correct type</param>
        /// <param name="count">Requested count of intervals</param>
        /// <returns></returns>
        public static T returnMaxValue(string ontologyMax, string to, bool subdomain, string dataMax,
            T domainDividingValueMax, T distinctValueMax, ToTypeDelegate converter)
        {
            T __max = converter("9000");
            return __max;
        }
    }
}
