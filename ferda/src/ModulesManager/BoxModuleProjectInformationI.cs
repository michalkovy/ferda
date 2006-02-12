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

namespace Ferda {
    namespace ModulesManager {

        /// <summary>
        /// Summary description for the class.
        /// </summary>

        public class BoxModuleProjectInformationI : BoxModuleProjectInformationDisp_
		{
			
			///<summary>
			/// Constructor
			/// </summary>
			/// <param name="modulesManager">A  ModulesManager</param>
			public BoxModuleProjectInformationI(ModulesManager modulesManager)
			{
				this.modulesManager = modulesManager;
			}

            /// <summary>
            /// Method getUserLabel
            /// </summary>
            /// <returns>A string</returns>
            /// <param name="boxModuleIceIdentity">A  string</param>
            /// <param name="__current">An Ice.Current</param>
            public override string getUserLabel(string boxModuleIceIdentity, Current __current) {
                BoxModule box = modulesManager.getBoxModuleByIdentity(boxModuleIceIdentity);
				if(box!=null)
				{
					return box.UserName;
				}
				throw new BoxModuleNotExistError();
            }

            /// <summary>
            /// Method getUserHint
            /// </summary>
            /// <returns>A string</returns>
            /// <param name="boxModuleIceIdentity">A  string</param>
            /// <param name="__current">An Ice.Current</param>
            public override string getUserHint(string boxModuleIceIdentity, Current __current) {
                BoxModule box = modulesManager.getBoxModuleByIdentity(boxModuleIceIdentity);
				if(box!=null)
				{
					return box.UserHint;
				}
				throw new BoxModuleNotExistError();
            }

            /// <summary>
            /// Method getProjectIdentifier
            /// </summary>
            /// <returns>A string</returns>
            /// <param name="boxModuleIceIdentity">A  string</param>
            /// <param name="__current">An Ice.Current</param>
            public override int getProjectIdentifier(string boxModuleIceIdentity, Current __current) {
                BoxModule box = modulesManager.getBoxModuleByIdentity(boxModuleIceIdentity);
				if(box!=null)
				{
					return box.ProjectIdentifier;
				}
				throw new BoxModuleNotExistError();
            }
			
			private ModulesManager modulesManager;
        }

    }
}
