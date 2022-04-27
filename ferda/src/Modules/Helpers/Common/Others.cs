// Others.cs - Other useful functions
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
using System.Text;

namespace Ferda.Modules.Helpers.Common
{
    /// <summary>
    /// Provides some other minor functionality.
    /// </summary>
    public static class Others
    {
        /// <summary>
        /// Converts array of strings to one single string.
        /// (the input array is sorted first).
        /// </summary>
        /// <param name="sourceArray">The source array.</param>
        /// <returns><see cref="System.String"/></returns>
        public static string StringArray2String(string[] sourceArray)
        {
            List<string> sourceList = new List<string>();
            sourceList.AddRange(sourceArray);
            sourceList.Sort();
            string result = String.Empty;
            foreach (string item in sourceList)
            {
                result += item;
            }
            return result;
        }
    }
}
