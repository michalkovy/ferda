// TimeOut.cs - Timing specific procedure calls
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
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USAusing System;

using System.Collections.Generic;
using System.Text;
using System;

namespace Ferda.Modules.Helpers.Caching
{
    /// <summary>
    /// Clocks time between subsequent calls of 
    /// <see cref="M:Ferda.Modules.Helpers.Caching.TimeOut.IsObsolete">IsObsolete method</see>
    /// which returns false iff time between two last calls was shorter
    /// than <c>timeout</c>.
    /// </summary>
	public class TimeOut
	{
        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="T:Ferda.Modules.Helpers.Caching.TimeOut"/> class.
        /// </summary>
        /// <param name="timeout">
        /// <para>The timeout</para>
        /// <para>The meaning of <c>timeout</c> is that if two subsequent calls of 
        /// <see cref="M:Ferda.Modules.Helpers.Caching.TimeOut.IsObsolete">IsObsolete method</see>
        /// are proceeded during time interval shorter than the <c>timeout</c> 
        /// the second call returns false (i.e. it isn`t obsolete).
        /// </para>
        /// </param>
		public TimeOut(TimeSpan timeout)
		{
			this.timeout = timeout;
			this.lastTime = DateTime.MinValue;
		}

		/// <summary>
        /// <para>The timeout</para>
        /// <para>
        /// The meaning of <c>timeout</c> is that if two subsequent calls of 
        /// <see cref="M:Ferda.Modules.Helpers.Caching.TimeOut.IsObsolete">IsObsolete method</see>
        /// are proceeded during time interval shorter than the <c>timeout</c> 
        /// the second call returns false (i.e. it isn`t obsolete).
        /// </para>
		/// </summary>
        private TimeSpan timeout;

        /// <summary>
        /// The time of last call of 
        /// <see cref="M:Ferda.Modules.Helpers.Caching.TimeOut.IsObsolete">IsObsolete method</see>.
        /// </summary>
		private DateTime lastTime;

        /// <summary>
        /// Determines whether previous call of this method is older than 
        /// <see cref="F:Ferda.Modules.Helpers.Caching.TimeOut.timeout">time interval</see>.
        /// </summary>
        /// <returns>
        /// <c>true</c> if last call of this method is older than 
        /// <see cref="F:Ferda.Modules.Helpers.Caching.TimeOut.timeout"/>; 
        /// otherwise, <c>false</c>.
        /// </returns>
        public bool IsObsolete()
		{
            lock (this)
            {
                if (lastTime + timeout < DateTime.Now)
                {
                    lastTime = DateTime.Now;
                    return true;
                }
                return false;
            }
		}
	}
}