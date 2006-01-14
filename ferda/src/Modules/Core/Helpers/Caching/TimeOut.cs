using System;
using System.Collections.Generic;
using System.Text;

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