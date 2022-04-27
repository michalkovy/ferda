// IProjectHelper.cs - interface of project manager for modules manager
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

namespace Ferda.ModulesManager
{
	/// <summary>
	/// Base interface for box modules. Implemented by classes that represents box.
	/// </summary>
	public interface IProjectHelper {
	
		/// <summary>
		/// Clone box boxModule with all boxes connected to it.
		/// </summary>
        /// <param name="boxModule">The parent box module which we want to clone with all child box modules</param>
        /// <returns>The cloned version of box module boxModule</returns>
		IBoxModule CloneBoxModuleWithChilds(IBoxModule boxModule, bool addToProject, IBoxModule[][] variables, IBoxModule[] variableValues);
	}
}