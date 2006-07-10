using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Modules.Helpers.Common
{
    /// <summary>
    /// This class provides some methods that helps to converts
    /// some objects to strings.
    /// </summary>
    public static class Print
    {
        /// <summary>
        /// Convert sequence of objects to string.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="separator">The separator.</param>
        /// <returns></returns>
        public static string SequenceToString<T>(IEnumerable<T> items, string separator)
        {
            if (items == null)
                return String.Empty;
            
            string result = null;
            foreach (T s in items)
            {
                if (result != null)
                    result += separator + s.ToString();
                else
                    result = s.ToString();
            }
            if (result == null)
                return String.Empty;
            else
                return result;
        }
    }
}
