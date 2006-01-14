using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Ferda.Modules
{
    public class PropertyReapThread
    {
        public class BoxModuleFactoryProxyPair
        {
            public BoxModuleFactoryProxyPair(BoxModuleFactoryPrx p, PropertyBoxModuleFactoryI s)
            {
                proxy = p;
                factory = s;
            }

            public BoxModuleFactoryPrx proxy;
            public PropertyBoxModuleFactoryI factory;
        }

        public PropertyReapThread()
        {
            _timeout = Ferda.Modules.factoryRefreshedTestTime.value;
            _factories = new ArrayList();
        }
        public void Run()
        {
            lock (this)
            {
                while (!_terminated)
                {
                    System.Threading.Monitor.Wait(this, _timeout);
                    if (!_terminated)
                    {
                        ArrayList tmp = new ArrayList();
                        foreach (BoxModuleFactoryProxyPair p in _factories)
                        {
                            try
                            {
                                //
                                // Factory destruction may take time in a
                                // real-world example. Therefore the current time
                                // is computed for each iteration.
                                //
                                System.TimeSpan ts = System.DateTime.Now - p.factory.Timestamp;
                                if (ts.Milliseconds > _timeout)
                                {
                                    p.proxy.destroy();
                                }
                                else
                                {
                                    tmp.Add(p);
                                }
                            }
                            catch (Ice.ObjectNotExistException)
                            {
                                // Ignore.
                            }
                        }
                        _factories = tmp;
                    }
                }
            }
        }

        public void Terminate()
        {
            lock (this)
            {
                _terminated = true;
                System.Threading.Monitor.Pulse(this);
/*
                foreach (BoxModuleFactoryProxyPair p in _factories)
                {
                    try
                    {
                        p.proxy.destroy();
                    }
                    catch (Ice.Exception)
                    {
                        // Ignore.
                    }
                }
                */
                _factories.Clear();
            }
        }

        public void Add(BoxModuleFactoryPrx proxy, PropertyBoxModuleFactoryI factory)
        {
            lock (this)
            {
                _factories.Add(new BoxModuleFactoryProxyPair(proxy, factory));
            }
        }

        private bool _terminated;
        private int _timeout;
        private ArrayList _factories;
    }
}
