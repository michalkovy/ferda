using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

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
            Regex r = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))", RegexOptions.Compiled);
            if (!String.IsNullOrEmpty(csvString))
            {
                return r.Split(csvString);
            }
            else
                return new string[0];
        }
    }
}
