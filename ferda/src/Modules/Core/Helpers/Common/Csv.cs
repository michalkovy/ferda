using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using LumenWorks.Framework.IO.Csv;

namespace Ferda.Modules.Helpers.Common
{
    /// <summary>
    /// Provides some static functions for working with CSV files/strings.
    /// </summary>
    public static class Csv
    {
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
    }
}
