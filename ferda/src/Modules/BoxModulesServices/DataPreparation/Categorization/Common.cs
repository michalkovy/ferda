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
    }
}
