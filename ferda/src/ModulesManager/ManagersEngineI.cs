// ManagersEngineI.cs - Managers engine for modules
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
        /// Managers engine for modules
        /// </summary>
        public class ManagersEngineI : ManagersEngineDisp_
		{
			
			///<summary>
			/// Constructor
			/// </summary>
			public ManagersEngineI(Ice.ObjectAdapter adapter, Helper helper, ModulesManager manager)
			{
				Debug.WriteLine("Creating Output...");
				lnkOutputI = new OutputI();
				outputPrx = OutputPrxHelper.uncheckedCast(
					adapter.addWithUUID(this.lnkOutputI));
				Debug.WriteLine("Creating BoxModuleLocker...");
				lnkBoxModuleLockerI = new BoxModuleLockerI(manager);
				boxModuleLockerPrx = BoxModuleLockerPrxHelper.uncheckedCast(
					adapter.addWithUUID(this.lnkBoxModuleLockerI));
                Debug.WriteLine("Creating BoxModuleValidator...");
                lnkBoxModuleValidatorI = new BoxModuleValidatorI(manager);
                boxModuleValidatorPrx = BoxModuleValidatorPrxHelper.uncheckedCast(
                    adapter.addWithUUID(this.lnkBoxModuleValidatorI));
				Debug.WriteLine("Creating BoxModuleProjectInformation...");
				lnkBoxModuleProjectInformationI = new BoxModuleProjectInformationI(manager);
				boxModuleProjectInformationPrx =
					BoxModuleProjectInformationPrxHelper.uncheckedCast(
					adapter.addWithUUID(this.lnkBoxModuleProjectInformationI));
				Debug.WriteLine("Creating ManagersLocator...");
				lnkManagersLocatorI = new ManagersLocatorI(adapter, helper);
				managersLocatorPrx = ManagersLocatorPrxHelper.uncheckedCast(
					adapter.addWithUUID(this.lnkManagersLocatorI));
			}
			
			public OutputPrx OutputPrx
			{
				set {
					outputPrx = value;
				}
				
				get {
					return outputPrx;
				}
			}
			
			public ManagersLocatorI ManagersLocatorI
			{
				get {
					return lnkManagersLocatorI;
				}
			}
			
			/// <summary>
			/// Method getBoxModuleLocker
			/// </summary>
			/// <returns>A Ferda.ModulesManager.BoxModuleLockerPrx</returns>
			/// <param name="__current">An Ice.Current</param>
			public override BoxModuleLockerPrx getBoxModuleLocker(Current __current)
			{
				return this.boxModuleLockerPrx;
			}

            /// <summary>
            /// Method getBoxModuleValidator
            /// </summary>
            /// <param name="__current">An Ice.Current</param>
            /// <returns>A Ferda.ModulesManager.BoxModuleValidatorPrx</returns>
            public override BoxModuleValidatorPrx getBoxModuleValidator(Current __current)
            {
                return this.boxModuleValidatorPrx;
            }
			
			
			/// <summary>
			/// Method getOutputInterface
			/// </summary>
			/// <returns>A Ferda.ModulesManager.OutputPrx</returns>
			/// <param name="__current">An Ice.Current</param>
			public override OutputPrx getOutputInterface(Current __current)
			{
				return this.outputPrx;
			}
			
			/// <summary>
			/// Method getManagersLocator
			/// </summary>
			/// <returns>A Ferda.ModulesManager.ManagersLocatorPrx</returns>
			/// <param name="__current">An Ice.Current</param>
			public override ManagersLocatorPrx getManagersLocator(Current __current)
			{
				return this.managersLocatorPrx;
			}

			/// <summary>
			/// Method getProjectInformation
			/// </summary>
			/// <returns>A Ferda.ModulesManager.BoxModuleProjectInformationPrx</returns>
			/// <param name="__current">An Ice.Current</param>
			public override BoxModuleProjectInformationPrx getProjectInformation(Current __current)
			{
				return this.boxModuleProjectInformationPrx;
			}

            private BoxModuleLockerI lnkBoxModuleLockerI;
			
			private BoxModuleLockerPrx boxModuleLockerPrx;

            private BoxModuleProjectInformationI lnkBoxModuleProjectInformationI;
			
			private BoxModuleProjectInformationPrx boxModuleProjectInformationPrx;

            private ManagersLocatorI lnkManagersLocatorI;
			
			private ManagersLocatorPrx managersLocatorPrx;

            private BoxModuleValidatorI lnkBoxModuleValidatorI; 

            private BoxModuleValidatorPrx boxModuleValidatorPrx; 

            private OutputI lnkOutputI = new OutputI();
			
			private OutputPrx outputPrx;
        }
    }
}
