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
                return null;
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
        /// <para><c>Key</c> is the file`s path.</para>
        /// <para><c>Value</c> is the content of the file as array of bytes.</para>
        /// </summary>
        private static Dictionary<string, byte[]> cachedFiles = new Dictionary<string, byte[]>();

        /// <summary>
        /// Tries to get the file from by the path specified by <c>filePath</c>.
        /// </summary>
        /// <param name="directoryPath">Path to the file.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="fallOnError">True if an exception can be thrown on error.</param>
        /// <param name="useCache">Iff true the readed file is cached to memory i.e. 
        /// reading the file is executed only once.</param>
        /// <returns>Array of <see cref="T:System.Byte">Bytes</see>.</returns>
        /// <exception cref="T:System.Exception">
        /// If <c>fallOnError</c> is <c>true</c> and 
        /// any error occured while getting the file.
        /// </exception>
        public static byte[] TryGetFile(string directoryPath, string fileName, bool fallOnError, bool useCache)
        {
            if (!String.IsNullOrEmpty(fileName))
            {
                byte[] result;
                if (String.IsNullOrEmpty(directoryPath))
                    directoryPath = String.Empty;

                string filePath = Path.Combine(directoryPath, fileName);
                //is file cached (alredy readed in memory)
                if (useCache && cachedFiles.TryGetValue(filePath, out result))
                {
                    return result;
                }
                else
                {
                    try
                    {
                        if (File.Exists(filePath) || fallOnError)
                        {
                            //gets stream
                            FileStream fileStream = new FileStream(filePath, FileMode.Open);

                            //reads stream and create result
                            int fileLength = (int)(fileStream.Length);
                            result = new byte[fileLength];
                            fileStream.Read(result, 0, fileLength);

                            //close stream
                            fileStream.Close();

                            //save readed file to cache
                            if (useCache)
                                cachedFiles.Add(filePath, result);

                            return result;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("FileHelper01: TryGetFile(" + filePath + "):" + ex.Message);
                        if (fallOnError)
                            throw ex;
                    }
                }
            }
            return new byte[0];
        }

        #endregion
    }
}
