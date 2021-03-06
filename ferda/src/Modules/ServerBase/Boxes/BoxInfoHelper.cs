// BoxInfoHelper.cs - Provides some static functions for easier
// boxes handling
//
// Author: Tom?? Kucha? <tomas.kuchar@gmail.com>
//
// Copyright (c) 2006 Tom?? Kucha?
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
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using Ferda.Guha.Data;
using LumenWorks.Framework.IO.Csv;

namespace Ferda.Modules.Boxes
{
    /// <summary>
    /// Provides some static functions for easier implementing some functions
    /// of <see cref="T:Ferda.Modules.Boxes.IBoxInfo"/> or 
    /// <see cref="T:Ferda.Modules.Boxes.BoxInfo"/>.
    /// </summary>
    public static class BoxInfoHelper
    {
        /// <summary>
        /// Creates an array of <see cref="T:Ferda.Modules.SelectString"/> from
        /// specified array of <see cref="T:System.String">Strings</see> (<c>input</c>).
        /// </summary>
        /// <param name="input">Input array of <see cref="T:System.String">Strings</see>.</param>
        /// <returns>
        /// Output array of <see cref="T:Ferda.Modules.SelectString"/>.
        /// </returns>
        public static SelectString[] GetSelectStringArray(string[] input)
        {
            if ((input == null) || (input.Length == 0))
                return new SelectString[0];
            List<SelectString> result = new List<SelectString>();
            foreach (string item in input)
            {
                SelectString selectString = new SelectString();
                selectString.name = item;
                selectString.label = item;
                result.Add(selectString);
            }
            return result.ToArray();
        }

        /// <summary>
        /// Creates an array of <see cref="T:Ferda.Modules.SelectString"/> from
        /// specified list of <see cref="T:System.String">Strings</see> (<c>input</c>).
        /// </summary>
        /// <param name="input">Input list of <see cref="T:System.String">Strings</see>.</param>
        /// <returns>
        /// Output array of <see cref="T:Ferda.Modules.SelectString"/>.
        /// </returns>
        public static SelectString[] GetSelectStringArray(List<string> input)
        {
            if ((input == null) || (input.Count == 0))
                return new SelectString[0];
            List<SelectString> result = new List<SelectString>();
            foreach (string item in input)
            {
                SelectString selectString = new SelectString();
                selectString.name = item;
                selectString.label = item;
                result.Add(selectString);
            }
            return result.ToArray();
        }

        /// <summary>
        /// Creates an array of <c>ValueFrequencyPair</c> from
        /// specified <c>Dictionary&lt;string, int&gt;</c> (<c>input</c>).
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        /// Output array of <see cref="T:Ferda.Modules.SelectString"/>.
        /// </returns>
        public static ValueFrequencyPair[] GetValueFrequencyPairArray(Dictionary<string, int> input)
        {
            if ((input == null) || (input.Count == 0))
                return new ValueFrequencyPair[0];
            List<ValueFrequencyPair> result = new List<ValueFrequencyPair>();
            foreach (KeyValuePair<string, int> pair in input)
            {
                result.Add(new ValueFrequencyPair(pair.Key, pair.Value));
            }
            return result.ToArray();
        }

        /// <summary>
        /// Parses array of <see cref="T:System.String">strings</see>
        /// from specified CSV string (<c>csvString</c>).
        /// </summary>
        /// <param name="csvString">The CSV string.</param>
        /// <returns>
        /// Splited CSV string in array of <see cref="T:System.String">strings</see>.</returns>
        public static string[] Csv2Strings(string csvString)
        {
            if (String.IsNullOrEmpty(csvString))
                return new string[0];

            using (CsvReader csv =
                   new CsvReader(new StringReader(csvString), false))
            {
                int fieldCount = csv.FieldCount;
                List<string> result = new List<string>();
                for (int i = 0; i < fieldCount; i++)
                    result.Add(csv[i]);
                return result.ToArray();
            }

            //Regex r = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))", RegexOptions.Compiled);
        }

        #region Files

        /// <summary>
        /// Tries to get specified binary file from by the path 
        /// specified by <c>filePath</c>.
        /// </summary>
        /// <param name="directoryPath">Path to the file.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="fallOnError">True if an exception can be thrown on error.</param>
        /// <returns>Array of <see cref="T:System.Byte">Bytes</see>.</returns>
        /// <exception cref="T:System.Exception">
        /// If <c>fallOnError</c> is <c>true</c> and 
        /// any error occured while getting the file.
        /// </exception>
        public static byte[] TryGetBinaryFile(string directoryPath, string fileName, bool fallOnError)
        {
            if (!String.IsNullOrEmpty(fileName))
            {
                if (String.IsNullOrEmpty(directoryPath))
                    directoryPath = String.Empty;

                // build whole path
                string filePath = Path.Combine(directoryPath, fileName);
                try
                {
                    if (File.Exists(filePath) || fallOnError)
                    {
                        return File.ReadAllBytes(filePath);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("FileHelper01: TryGetBinaryFile(" + filePath + "):" + ex.Message);
                    if (fallOnError)
                        throw;
                }
            }
            return new byte[0];
        }

        /// <summary>
        /// Tries to get specified string file from by the path 
        /// specified by <c>filePath</c>.
        /// </summary>
        /// <param name="directoryPath">Path to the file.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="fallOnError">True if an exception can be thrown on error.</param>
        /// <returns>Array of <see cref="T:System.Byte">Bytes</see>.</returns>
        /// <exception cref="T:System.Exception">
        /// If <c>fallOnError</c> is <c>true</c> and 
        /// any error occured while getting the file.
        /// </exception>
        public static string TryGetStringFile(string directoryPath, string fileName, bool fallOnError)
        {
            if (!String.IsNullOrEmpty(fileName))
            {
                if (String.IsNullOrEmpty(directoryPath))
                    directoryPath = String.Empty;

                // build whole path
                string filePath = Path.Combine(directoryPath, fileName);
                try
                {
                    if (File.Exists(filePath) || fallOnError)
                    {
                        return File.ReadAllText(filePath);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("FileHelper02: TryGetStringFile(" + filePath + "):" + ex.Message);
                    if (fallOnError)
                        throw;
                }
            }
            return String.Empty;
        }

        #endregion

        /// <summary>
        /// Gets the GUID struct from specified property. If the property value is null or
        /// empty string, the method creates a new GUID and writes it to the property.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="boxModule">The box module.</param>
        /// <returns>A new GUID struct</returns>
        /// <remarks>The method can be used only when the property is string type.</remarks>
        public static GuidStruct GetGuidStructFromProperty(string propertyName, BoxModuleI boxModule)
        {
            string guid = boxModule.GetPropertyString(propertyName);
            if (String.IsNullOrEmpty(guid))
            {
                Guid value = Guid.NewGuid();
                guid = value.ToString();
                boxModule.setProperty(
                    propertyName, 
                    new StringTI(guid),
                    null
                    );
            }

            return new GuidStruct(guid);
        }
    }
}