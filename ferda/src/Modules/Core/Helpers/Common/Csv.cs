// Csv.cs - Working with CSV files/strings
//
// Author: Tomáš Kuchaø <tomas.kuchar@gmail.com>
//
// Copyright (c) 2006 Tomáš Kuchaø
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
