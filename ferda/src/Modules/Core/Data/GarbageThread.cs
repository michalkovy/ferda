using System;
using System.Collections.Generic;
using System.Threading;

namespace Ferda.Guha.Data
{
    internal class GarbageThread
    {
        private bool _terminated;
        private const int _timeout = 360000; //miliseconds

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