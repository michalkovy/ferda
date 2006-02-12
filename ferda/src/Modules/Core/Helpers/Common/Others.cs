using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Modules.Helpers.Common
{
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
