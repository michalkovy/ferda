// IProjectLoader.cs - interface for loading and saving project
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
using Ferda.ModulesManager;

namespace Ferda.ProjectManager
{
	/// <summary>
	/// Interface for saving and loading of project
	/// </summary>
	public interface IProjectLoader
	{
		/// <summary>
		/// Imports project to allready opened one
		/// </summary>
		/// <returns>A string representing errors of import</returns>
		/// <param name="p">A  Ferda.ProjectManager.Project representing project which have to be imported</param>
		/// <param name="mainProjectBox">A  Project.Box representing some box in project,
		/// this method will return in <paramref name="mainIBoxModule"/> new IBoxModule
		/// representation of this box</param>
		/// <param name="addToProject">If false new modules will not be added to archive.</param>
		/// <param name="mainIBoxModule">New Ferda.ModulesManager.IBoxModule representation
		/// of <paramref name="mainProjectBox"/></param>
		string ImportProject(Ferda.ProjectManager.Project p, Project.Box mainProjectBox, bool addToProject, out Ferda.ModulesManager.IBoxModule mainIBoxModule);
		
		/// <summary>
		/// Saves box modules to project
		/// </summary>
		/// <returns>A Project with boxes from <paramref name="boxModules"/> or which are connected
		/// to these and not included in view <paramref name="view"/></returns>
		/// <param name="boxModules">A  Ferda.ModulesManager.IBoxModule[]</param>
		/// <param name="view">A  View</param>
		/// <param name="variables">Variables</param>
		/// <param name="variableValues">values of variables</param>
		Project SaveBoxModulesToProject(IBoxModule[] boxModules, View view, IBoxModule[][] variables, IBoxModule[] variableValues);
	}
}
