// 
//
// Author: Michal Kováč <michal.kovac.develop@centrum.cz>
//
// Copyright (c) 2005 Michal Kováč 
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

namespace Ferda {
    namespace ModulesManager {

        /// <summary>
        /// Summary description for the class.
        /// </summary>
		public class BoxModuleIceFactories {
			private bool makeRefresh = true;
			protected Dictionary<Ferda.Modules.BoxModuleFactoryPrx,Ferda.Modules.BoxModuleFactoryPrx>
			_factories = new Dictionary<Ferda.Modules.BoxModuleFactoryPrx,Ferda.Modules.BoxModuleFactoryPrx>();
			private Helper helper;
			
			public BoxModuleIceFactories(Helper helper)
			{
				this.helper = helper;
			}

			///<summary>Calls periodicaly refresh method of BoxModuleFactory
			///classes. Will be run by one special thread.</summary>
            public void StartFactoriesRefreshing() {
				lock(this)
				{
					while (makeRefresh) {
						System.Threading.Monitor.Wait(this,Ferda.Modules.factoryRefreshedTestTime.value);
						if (makeRefresh)
						{
							foreach(Ferda.Modules.BoxModuleFactoryPrx factory in _factories.Values)
							{
								try
								{
									factory.refresh();
								}
								catch(Ice.Exception ex)
								{
									//TODO: osetrit lepe, asi danou factory odebrat
									Console.Error.WriteLine("refreshing BoxModuleFactory was unsuccesfull: " + ex);
								}
							}
							helper.ObjectAdapter.getCommunicator().flushBatchRequests();
						}
					}
				}
            }

			///<summary>ends thread which calls refreshes on box module
			///factories</summary>
            public void EndFactoriesRefreshing() {
				lock(this)
				{
                	makeRefresh = false;
					System.Threading.Monitor.Pulse(this);
				}
            }

            public void AddFactory(Ferda.Modules.BoxModuleFactoryPrx factoryPrx) {
				lock(this)
				{
					_factories[factoryPrx]=
						Ferda.Modules.BoxModuleFactoryPrxHelper.uncheckedCast(
							factoryPrx.ice_batchOneway()
						);
				}
            }

            public bool RemoveFactory(Ferda.Modules.BoxModuleFactoryPrx factoryPrx) {
				lock(this)
				{
					return _factories.Remove(factoryPrx);
				}
            }
        }
    }
}
