// Cache.cs - Caching functions
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

namespace Ferda.Modules.Helpers.Caching
{
    /// <summary>
    /// Provides basic functionality for caching. 
    /// See member methods for futher information.
    /// </summary>
    /// <example>
    /// This is exemplary usage of this abstract class.
    /// <code>
    /// public class SomeCache : Ferda.Modules.Helpers.Caching.Cache
    /// {
    ///     private object cachedValue;
    ///  
    ///     public object GetCachedValue(IComparable valueA, IComparable valueB)
    ///     {
    ///         Dictionary&lt;string, IComparable&gt; cacheSetting = new Dictionary&lt;string, IComparable&gt;();
    ///         cacheSetting.Add("ValueA", valueA);
    ///         cacheSetting.Add("ValueB", valueB);
    ///         if (IsObsolete(cacheSetting))
    ///         {
    ///             // refresh the cachedValue i.e. compute the cachedValue again
    ///             cachedValue = /*...*/;
    ///         }
    ///         return cachedValue;
    ///     }
    /// }
    /// </code>
    /// </example>
    public abstract class Cache
    {
        /// <summary>
        /// Storage for last setting. Last setting is used for test of equality with
        /// current setting i.e. if last setting is not equal to current setting than
        /// the cache is obsolete.
        /// </summary>
        private Dictionary<string, IComparable> lastSettingForEqualTest = new Dictionary<string, IComparable>();

        /// <summary>
        /// The time of last call of <c>IsObsolete</c> method.
        /// </summary>
        private DateTime lastUpdate = DateTime.MinValue;

        /// <summary>
        /// Determines whether the cache is obsolete i.e. determines whether
        /// <see cref="F:Ferda.Modules.Helpers.Caching.Cache.lastSettingForEqualTest">last setting</see>
        /// is not equal to specified current setting (<c>currentSettingForEqualTest</c>)
        /// or <c>explicitReload</c> is greater than 
        /// <see cref="F:Ferda.Modules.Helpers.Caching.Cache.lastUpdate"/>. If the cache is obsolete
        /// <see cref="F:Ferda.Modules.Helpers.Caching.Cache.lastSettingForEqualTest">last setting</see> is 
        /// updated to <c>currentSettingForEqualTest</c> and 
        /// <see cref="F:Ferda.Modules.Helpers.Caching.Cache.lastUpdate"/> is udated to 
        /// <see cref="P:System.DateTime.Now"/> and <c>true</c> is returend; otherwise, 
        /// <c>false</c> is returned.
        /// </summary>
        /// <param name="explicitReload">
        /// The explicit reload. If time of <c>explicitReload</c> is greater than
        /// <see cref="F:Ferda.Modules.Helpers.Caching.Cache.lastUpdate"/> the cache is obsolete.
        /// </param>
        /// <param name="currentSettingForEqualTest">
        /// The current setting for equal test. If 
        /// <see cref="F:Ferda.Modules.Helpers.Caching.Cache.lastSettingForEqualTest"/>
        /// is not equal to <c>currentSettingForEqualTest</c> the cache is obsolete.
        /// </param>
        /// <returns>
        /// <c>true</c> if the cache is obsolete; otherwise, <c>false</c>.
        /// </returns>
        public bool IsObsolete(DateTimeTI explicitReload, Dictionary<string, IComparable> currentSettingForEqualTest)
        {
            lock (this)
            {
                bool result = false;

                // tests explicit and last reload time
                DateTime tmpExplicitReload;
                if (((DateTimeTI)explicitReload).TryGetDateTime(out tmpExplicitReload) && tmpExplicitReload.CompareTo(lastUpdate) > 0)
                    result = true;

                // tests last and current setting equality
                if (!settingIsEqual(currentSettingForEqualTest))
                    result = true;

                // if the cache is obsolete 
                if (result)
                {
                    // last update time is updated
                    lastUpdate = DateTime.Now;
                    // last setting is updated
                    lastSettingForEqualTest = currentSettingForEqualTest;
                }
                return result;
            }
        }

        /// <summary>
        /// Determines whether the cache is obsolete i.e. determines whether
        /// <see cref="F:Ferda.Modules.Helpers.Caching.Cache.lastSettingForEqualTest">last setting</see>
        /// is not equal to specified current setting (<c>currentSettingForEqualTest</c>).
        /// If the cache is obsolete
        /// <see cref="F:Ferda.Modules.Helpers.Caching.Cache.lastSettingForEqualTest">last setting</see> is 
        /// updated to <c>currentSettingForEqualTest</c> and <c>true</c> is returend; otherwise, 
        /// <c>false</c> is returned.
        /// </summary>
        /// <param name="currentSettingForEqualTest">
        /// The current setting for equal test. If 
        /// <see cref="F:Ferda.Modules.Helpers.Caching.Cache.lastSettingForEqualTest"/>
        /// is not equal to <c>currentSettingForEqualTest</c> the cache is obsolete.
        /// </param>
        /// <returns>
        /// <c>true</c> if the cache is obsolete; otherwise, <c>false</c>.
        /// </returns>
        public bool IsObsolete(Dictionary<string, IComparable> currentSettingForEqualTest)
        {
            lock (this)
            {
                bool result = !settingIsEqual(currentSettingForEqualTest);

                // if the cache is obsolete 
                if (result)
                {
                    // last update time is updated
                    lastUpdate = DateTime.Now;
                    // last setting is updated
                    lastSettingForEqualTest = currentSettingForEqualTest;
                }

                return result;
            }
        }

        /// <summary>
        /// Determines whether <see cref="F:Ferda.Modules.Helpers.Caching.Cache.lastSettingForEqualTest">last setting</see>
        /// is equal to specified current setting (<c>currentSettingForEqualTest</c>).
        /// </summary>
        /// <param name="currentSettingForEqualTest">The current setting for equal test.</param>
        /// <returns>
        /// <c>true</c> iff all items from <c>currentSettingForEqualTest</c> are alredy
        /// stored in <c>lastSettingForEqualTest</c> and both settings are equal by their items;
        /// otherwise, <c>false</c>.
        /// </returns>
        private bool settingIsEqual(Dictionary<string, IComparable> currentSettingForEqualTest)
        {
            IComparable value;

            // for each item of current setting
            foreach (KeyValuePair<string, IComparable> item in currentSettingForEqualTest)
            {
                if (lastSettingForEqualTest.TryGetValue(item.Key, out value))
                {
                    // tests equality of last and current result
                    if (value == item.Value)
                        continue;
                    else if ((value != null && item.Value != null)
                        && (value.CompareTo(item.Value) == 0))
                        continue;
                }
                // if values are not equal (false will be returned)
                return false;
            }
            return true;
        }
    }
}