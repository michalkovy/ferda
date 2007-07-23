// GarbageThread.cs - Deleting of old database connections handling
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
using System.Threading;

namespace Ferda.Guha.Data
{
    /// <summary>
    /// Class handling deleting of old database connections
    /// </summary>
    internal class GarbageThread
    {
        private bool _terminated;
        private const int _timeout = 360000; //miliseconds
        
        /// <summary>
        /// Running the garbage thread. This thread cleans the database
        /// connections, that have last access to the database older
        /// then defined time.
        /// </summary>
        public void Run()
        {
            Thread.CurrentThread.Priority = ThreadPriority.Lowest;
            while (!_terminated)
            {
                lock (GenericDatabaseCache._connections)
                {
                    if (!_terminated)
                    {
                        ResetLoop:
                        foreach (
                            KeyValuePair<DatabaseConnectionSettingHelper, GenericDatabase> p in
                                GenericDatabaseCache._connections)
                        {
                            TimeSpan ts = DateTime.Now - p.Value.LastAccess;
                            if (ts.Milliseconds > _timeout)
                            {
                                GenericDatabaseCache.RemoveGenericDatabase(p.Key);
                                goto ResetLoop;
                            }
                        }
                    }
                }
                Thread.Sleep(_timeout);
            }
        }

        /// <summary>
        /// Terminates the garbage thread
        /// </summary>
        public void Terminate()
        {
            lock (this)
            {
                _terminated = true;
                Monitor.Pulse(this);
            }
        }
    }
}