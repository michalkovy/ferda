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
using Ice;
using System.Diagnostics;

namespace Ferda {
	namespace ModulesManager {

		/// <summary>
		/// Summary description for the class.
		/// </summary>
		public class Helper : IAppPrefs, IIceHelper, IIcePrefs
		{
			public Helper(Ice.ObjectAdapter adapter, string[] localePrefs, ModulesManager manager)
			{
				this.adapter = adapter;
				this.localePrefs = localePrefs;
				Debug.WriteLine("Creating BoxModuleIceFactories...");
				this.boxModuleIceFactories = new BoxModuleIceFactories( this );
				Debug.WriteLine("Creating ManagersEngine...");
				this.managersEngineI = new ManagersEngineI(adapter,this, manager);
				this.managersEnginePrx = ManagersEnginePrxHelper.uncheckedCast(
					adapter.addWithUUID(managersEngineI));
			}
			
			public ObjectAdapter ObjectAdapter
			{
				get {
					return adapter;
				}
			}
			
			public BoxModuleIceFactories BoxModuleIceFactories
			{
				get {
					return this.boxModuleIceFactories;
				}
			}
			
			public ManagersEngineI ManagersEngineI
			{
				get {
					return this.managersEngineI;
				}
			}
			
			public ManagersEnginePrx ManagersEnginePrx
			{
				get {
					return this.managersEnginePrx;
				}
			}
			
			public String[] LocalePrefs
			{
				get {
					return this.localePrefs;
				}
			}
			
			private String[] localePrefs;
			private Ice.ObjectAdapter adapter;
			private BoxModuleIceFactories boxModuleIceFactories;
			private ManagersEngineI managersEngineI;
			private ManagersEnginePrx managersEnginePrx;
		}
	}
}
