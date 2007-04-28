// NetworkArchive.cs - project manager side of Network Archive
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

using Ferda.Modules;
using Ferda.ModulesManager;
using System.Collections.Generic;

namespace Ferda.ProjectManager
{
	/// <summary>
	/// Description of NetworkArchive.
	/// </summary>
	public class NetworkArchive
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="modulesManager">A Ferda.ModulesManager.ModulesManager</param>
		/// <param name="projectLoader">A Ferda.ProjectManager.IProjectLoader</param>
		public NetworkArchive(Ferda.ModulesManager.ModulesManager modulesManager, Ferda.ProjectManager.IProjectLoader projectLoader)
		{
			this.archive = modulesManager.Helper.NetworkArchive;
			this.projectLoader = projectLoader;
		}
		
		/// <summary>
		/// Adds connection of IBoxModules to network archive. It is representation of
		/// function. We simply call it Box, even if it is more IBoxModules connected together.
		/// In network archive will be stored <paramref name="boxModule"/> with all boxes connected
		/// to them.
		/// </summary>
		/// <param name="boxModule">Main IBoxModule</param>
		/// <param name="label">A string which represents label of </param>
		public void AddBox(IBoxModule boxModule, string label)
		{
			archive.AddBox(createBoxFromBoxModule(boxModule), label);
		}
		
		/// <summary>
		/// Removes connection of boxes from network archive
		/// </summary>
		/// <param name="label">A string representing name of connection</param>
		public void RemoveBox(string label)
		{
			archive.RemoveBox(label);
		}
		
		/// <summary>
		/// Creates a copy of connection stored in network archive to actual project
		/// (standard archive)
		/// </summary>
		/// <returns>An IBoxModule representing main box module added which is copied to
		/// actual project</returns>
		/// <param name="label">A string representing name of the connection</param>
		/// <param name="errors">A string representing errors from project loading,
		/// these errors aro not exception. If you store some connection in old version
		/// of Ferda and open it in new, it is possible that for example not all IBoxModuleFactoryCreators
		/// will still exist and that boxes will still have the same properties.</param>
		public IBoxModule GetBoxToProject(string label, out string errors)
		{
			Project.Box mainProjectBox;
			Ferda.ModulesManager.IBoxModule mainIBoxModule;
			Project project = Ferda.NetworkArchive.ProjectConverter.CreateProjectFromBox(archive.GetBox(label), out mainProjectBox);
			errors = projectLoader.ImportProject(project, mainProjectBox, out mainIBoxModule);
			return mainIBoxModule;
		}
		
		/// <summary>
		/// Gets IBoxModuleFactioryCreator of main box in stored connection. This
		/// method is usefull for displaying type of box in FrontEnd
		/// </summary>
		/// <returns>An IBoxModuleFactoryCreator of main box module</returns>
		/// <param name="label">A string representing user name of stored connection</param>
		public IBoxModuleFactoryCreator GetBoxModuleFactoryCreatorOfBox(string label)
		{
			return archive.GetBoxModeleFactoryCreatorOfBox(label);
		}
		
		/// <summary>
		/// User names of stored boxes with connections in archive
		/// </summary>
		public string[] Labels
		{
			get
			{
				return archive.Labels;
			}
		}
		
		private Ferda.NetworkArchive.Box createBoxFromBoxModule(IBoxModule boxModule)
		{
			Project project = projectLoader.SaveBoxModulesToProject(new IBoxModule[]{boxModule}, null);
			if(project.Boxes.Length > 0)
			{
				return Ferda.NetworkArchive.ProjectConverter.CreateBoxFromProject(project, project.Boxes[0].ProjectIdentifier);
			}
			else
			{
				return null;
			}
		}
		
		private Ferda.ModulesManager.NetworkArchive archive;
		private Ferda.ProjectManager.IProjectLoader projectLoader;
	}
}
