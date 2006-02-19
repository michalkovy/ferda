using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

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
        /// <returns>Output array of <see cref="T:Ferda.Modules.SelectString"/>.</returns>
        public static SelectString[] StringArrayToSelectStringArray(string[] input)
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
        /// Parses array of <see cref="T:System.String">strings</see>
        /// from specified CSV string (<c>csvString</c>).
        /// </summary>
        /// <param name="csvString">The CSV string.</param>
        /// <returns>
        /// Splited CSV string in array of <see cref="T:System.String">strings</see>.</returns>
        public static string[] Csv2Strings(string csvString)
        {
            Regex r = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))", RegexOptions.Compiled);
            if (!String.IsNullOrEmpty(csvString))
            {
                return r.Split(csvString);
            }
            else
                return new string[0];
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
                        throw ex;
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
                        throw ex;
                }
            }
            return String.Empty;
        }
        #endregion
    }
}
