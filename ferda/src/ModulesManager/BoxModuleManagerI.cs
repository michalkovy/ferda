// 
//
// Author: Michal Kováč <michal.kovac.develop@centrum.cz>
//
// Copyright (c) 2007 Michal Kováč 
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
        public class BoxModuleManagerI : BoxModuleManagerDisp_
		{
			
			///<summary>
			/// Constructor
			/// </summary>
			/// <param name="modulesManager">A  ModulesManager</param>
            public BoxModuleManagerI(ModulesManager modulesManager)
			{
				this.modulesManager = modulesManager;
			}

            public override Ferda.Modules.BoxModulePrx CloneBoxModuleWithChilds (Ferda.Modules.BoxModulePrx boxModule, bool addToProject, Ferda.Modules.BoxModulePrx[][] variables, Ferda.Modules.BoxModulePrx[] variableValues, Current current__)
            {
            	if(boxModule == null) return null;
            	if (this.modulesManager.ProjectHelper == null) throw new System.Exception("Error 10002 - ProjectHelper should be set at startup of ModulesManager");
            	
            	BoxModule box = this.modulesManager.getBoxModuleByProxy(boxModule);
            	
            	BoxModule[][] variablesN = null;
            	BoxModule[] variableValuesN = null;
            	if (variables != null && variableValues != null)
            	{
	            	variablesN = new BoxModule[variables.Length][];
	            	for(int i = 0; i < variables.Length; i++)
	            	{
	            		variablesN[i] = new BoxModule[variables[i].Length];
	            		for(int j = 0; j < variables[i].Length; j++)
	            		{
	            			if(variables[i][j] != null)
	            			{
	            				variablesN[i][j] = this.modulesManager.getBoxModuleByProxy(variables[i][j]);
	            			}
	            			else
	            			{
	            				variablesN[i][j] = null;
	            			}
	            		}
	            	}
	            	variableValuesN = new BoxModule[variableValues.Length];
		       		for(int j = 0; j < variableValues.Length; j++)
		       		{
		       			
		       			variableValuesN[j] = (variableValues[j] != null) 
		       				?
		       					this.modulesManager.getBoxModuleByProxy(variableValues[j])
		       				:
		       					null;
		       		}
		       	}
	       		
            	BoxModule clonedBox = (BoxModule)this.modulesManager.ProjectHelper.CloneBoxModuleWithChilds(box, addToProject, variablesN, variableValuesN);
            	return clonedBox.IceBoxModulePrx;
            }
			
			private ModulesManager modulesManager;
        }
    }
}
