using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Modules.Helpers.Common
{
    public static class Print
    {
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
