// ReapThread.cs - testing of actuality of box modules factories
//
// Authors: 
//   Michal Kováč <michal.kovac.develop@centrum.cz>
//   Tomáš Kuchař <tomas.kuchar@gmail.com>
//
// Copyright (c) 2005 Michal Kováč, Tomáš Kuchař 
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
using System.Collections;
using System.Threading;
using Ice;

namespace Ferda.Modules
{
    /// <summary>
    /// Periodically tests if the <see cref="T:Ferda.Modules.BoxModuleFactoryI">
    /// box module factories</see> are 
    /// <see cref="M:Ferda.Modules.BoxModuleFactoryI.refresh(Ice.Current)">refreshed</see>
    /// if some factory isn`t then it is destroyed.
    /// </summary>
    public class ReapThread
    {
        /// <summary>
        /// Pair of the <b>box module factory</b> and its proxy.
        /// </summary>
        private class BoxModuleFactoryProxyPair
        {
            /// <summary>
            /// Initializes a new instance of the 
            /// <see cref="T:Ferda.Modules.ReapThread.BoxModuleFactoryProxyPair"/> class.
            /// </summary>
            /// <param name="factoryProxy">The proxy of the <b>box module factory</b>.</param>
            /// <param name="factory">The <b>box module factory</b>.</param>
            public BoxModuleFactoryProxyPair(BoxModuleFactoryPrx factoryProxy, BoxModuleFactoryI factory)
            {
                this.factoryProxy = factoryProxy;
                this.factory = factory;
            }

            /// <summary>
            /// The proxy of the <b>box module factory</b>.
            /// </summary>
            private BoxModuleFactoryPrx factoryProxy;

            /// <summary>
            /// Gets the proxy of the <b>box module factory</b>.
            /// </summary>
            /// <value>The proxy of the <b>box module factory</b>.</value>
            public BoxModuleFactoryPrx FactoryProxy
            {
                get { return factoryProxy; }
            }

            /// <summary>
            /// The <b>box module factory</b>.
            /// </summary>
            private BoxModuleFactoryI factory;

            /// <summary>
            /// Gets the <b>box module factory</b>.
            /// </summary>
            /// <value>The factory.</value>
            public BoxModuleFactoryI Factory
            {
                get { return factory; }
            }
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="T:Ferda.Modules.ReapThread"/> class.
        /// </summary>
        /// <remarks>
        /// Sets the timeout to 
        /// <see cref="F:Ferda.Modules.factoryRefreshedTestTime.value"/>.
        /// </remarks>
        public ReapThread()
        {
            _timeout = factoryRefreshedTestTime.value;
            _factories = new ArrayList();
        }

        /// <summary>
        /// Runs this instance. Periodically (by timeouts) tests if
        /// <b>box module factories</b> are refreshed if not, they 
        /// are destroyed.
        /// </summary>
        public void Run()
        {
            Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;
            lock (this)
            {
                while (!_terminated)
                {
                    Monitor.Wait(this, _timeout);
                    if (!_terminated)
                    {
                        ArrayList tmp = new ArrayList();
                        foreach (BoxModuleFactoryProxyPair p in _factories)
                        {
                            try
                            {
                                // Factory destruction may take time in a
                                // real-world example. Therefore the current time
                                // is computed for each iteration.
                                TimeSpan ts = DateTime.Now - p.Factory.LastRefresh;
                                if (ts.Milliseconds > _timeout)
                                {
                                    p.FactoryProxy.destroy();
                                }
                                else
                                {
                                    tmp.Add(p);
                                }
                            }
                            catch (ObjectNotExistException)
                            {
                                // Ignore.
                            }
                        }
                        _factories = tmp;
                    }
                }
            }
        }

        /// <summary>
        /// Terminates this instance.
        /// </summary>
        public void Terminate()
        {
            lock (this)
            {
                _terminated = true;
                Monitor.Pulse(this);

                //TODO Michal ma tohle byt skutecne zakomentovane?
/*
                foreach (BoxModuleFactoryProxyPair p in _factories)
                {
                    try
                    {
                        p.Proxy.destroy();
                    }
                    catch (Ice.Exception)
                    {
                        // Ignore.
                    }
                }*/

                _factories.Clear();
            }
        }

        /// <summary>
        /// Adds the specified <b>box module factory</b>.
        /// </summary>
        /// <param name="factoryProxy">The proxy of the <b>box module factory</b>.</param>
        /// <param name="factory">The <b>box module factory</b>.</param>
        public void Add(BoxModuleFactoryPrx factoryProxy, BoxModuleFactoryI factory)
        {
            lock (this)
            {
                _factories.Add(new BoxModuleFactoryProxyPair(factoryProxy, factory));
            }
        }

        private bool _terminated;
        private int _timeout;
        private ArrayList _factories;
    }
}