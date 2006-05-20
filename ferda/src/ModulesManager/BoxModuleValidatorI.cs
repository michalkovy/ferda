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
using Ice;

namespace Ferda {
    namespace ModulesManager {

        /// <summary>
        /// Summary description for the class.
        /// </summary>
        public class BoxModuleValidatorI : BoxModuleValidatorDisp_
		{
			
			///<summary>
			/// Constructor
			/// </summary>
			/// <param name="modulesManager">A  ModulesManager</param>
            public BoxModuleValidatorI(ModulesManager modulesManager)
			{
				this.modulesManager = modulesManager;
			}

            public override void validate(string boxModuleIceIdentity, Current __current)
            {
                BoxModule box = modulesManager.getBoxModuleByIdentity(boxModuleIceIdentity);
                if (box != null)
                {
                    box.ValidateRecursive();
                }
                else throw new BoxModuleNotExistError();
            }
			
			private ModulesManager modulesManager;
        }
    }
}
