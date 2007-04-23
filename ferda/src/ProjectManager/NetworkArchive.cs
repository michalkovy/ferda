// please change this line
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
		public NetworkArchive(Ferda.ModulesManager.ModulesManager modulesManager, Ferda.ProjectManager.IProjectLoader projectLoader)
		{
			this.archive = modulesManager.Helper.NetworkArchive;
			this.projectLoader = projectLoader;
		}
		
		public void AddBox(IBoxModule boxModule, string label)
		{
			archive.AddBox(createBoxFromBoxModule(boxModule), label);
		}
		
		public void RemoveBox(string label)
		{
			archive.RemoveBox(label);
		}
		
		public IBoxModule GetBoxToProject(string label, out string errors)
		{
			Project.Box mainProjectBox;
			Ferda.ModulesManager.IBoxModule mainIBoxModule;
			Project project = createProjectFromBox(archive.GetBox(label), out mainProjectBox);
			errors = projectLoader.ImportProject(project, mainProjectBox, out mainIBoxModule);
			return mainIBoxModule;
		}
		
		public IBoxModuleFactoryCreator GetBoxModeleFactoryCreatorOfBox(string label)
		{
			return archive.GetBoxModeleFactoryCreatorOfBox(label);
		}
		
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
			return createBoxFromProject(project);
		}
		
		private Project createProjectFromBox(Ferda.NetworkArchive.Box box, out Project.Box mainProjectBox)
		{
			Dictionary<Ferda.NetworkArchive.Box, int> projectIdentifiers =
				new Dictionary<Ferda.NetworkArchive.Box, int>();
			List<Ferda.NetworkArchive.Box> undoneBoxes =
				new List<Ferda.NetworkArchive.Box>(new Ferda.NetworkArchive.Box[]{box});
			int lastProjectIdentifier =  1;
						
			projectIdentifiers.Add(box, lastProjectIdentifier);
			
			List<Project.Box> projectBoxes = new List<Project.Box>();
			
			mainProjectBox = addBoxToProjectBoxes(box, projectBoxes, projectIdentifiers, undoneBoxes, ref lastProjectIdentifier);
			
			while(undoneBoxes.Count > 0)
			{
				addBoxToProjectBoxes(undoneBoxes[0], projectBoxes, projectIdentifiers, undoneBoxes, ref lastProjectIdentifier);
			}
			
			Project project = new Project();
			project.Boxes = projectBoxes.ToArray();
			project.Views = new Project.View[0];
			
			return project;
		}
		
		private static Project.Box addBoxToProjectBoxes(Ferda.NetworkArchive.Box box, List<Project.Box> projectBoxes, Dictionary<Ferda.NetworkArchive.Box, int> projectIdentifiers, List<Ferda.NetworkArchive.Box> undoneBoxes, ref int lastProjectIdentifier)
		{
			Project.Box projectBox = new Project.Box();
			projectBox.Connections = new Project.Box.Connection[box.Connections.Length];
			for(int i = 0; i < box.Connections.Length; i++)
			{
				projectBox.Connections[i].SocketName = box.Connections[i].socketName;
				if(!projectIdentifiers.TryGetValue(box.Connections[i].boxValue, out projectBox.Connections[i].BoxProjectIdentifier))
				{
					projectBox.Connections[i].BoxProjectIdentifier = ++lastProjectIdentifier;
					projectIdentifiers.Add(box, lastProjectIdentifier);
					undoneBoxes.Add(box);
				}
			}
			projectBox.PropertySets = new Project.Box.PropertySet[box.PropertySets.Length];
			for(int i = 0; i < box.PropertySets.Length; i++)
			{
				projectBox.PropertySets[i].PropertyName = box.PropertySets[i].propertyName;
				projectBox.PropertySets[i].Value =  ((Ferda.Modules.IValue)box.PropertySets[i].value).getValueT();
			}
			projectBoxes.Add(projectBox);
			undoneBoxes.Remove(box);
			return projectBox;
		}
		
		private static Ferda.NetworkArchive.Box createBoxFromProject(Project project)
		{
			Dictionary<int,Ferda.NetworkArchive.Box> identiferToBoxMap = new Dictionary<int,Ferda.NetworkArchive.Box>();
			foreach(Project.Box box in project.Boxes)
			{
				identiferToBoxMap.Add(box.ProjectIdentifier,
									  new Ferda.NetworkArchive.Box(
										  box.CreatorIdentifier,
										  box.UserName,
										  box.UserHint,
										  null,
										  null));
			}
			foreach(Project.Box box in project.Boxes)
			{
				List<Ferda.NetworkArchive.Connection> boxConnections =
					new List<Ferda.NetworkArchive.Connection>();
				Ferda.NetworkArchive.Box networkBox = identiferToBoxMap[box.ProjectIdentifier];
				
				foreach(Project.Box.Connection projectConnection in box.Connections)
				{
					Ferda.NetworkArchive.Connection boxConnection =
						new Ferda.NetworkArchive.Connection(
							projectConnection.SocketName,
							identiferToBoxMap[projectConnection.BoxProjectIdentifier]);
					boxConnections.Add(boxConnection);
				}
				networkBox.Connections = boxConnections.ToArray();
				
				networkBox.PropertySets = new PropertySetting[box.PropertySets.Length];
				for(int i = 0; i < box.PropertySets.Length; i++)
				{
					networkBox.PropertySets[i].propertyName = box.PropertySets[i].PropertyName;
					networkBox.PropertySets[i].value =  box.PropertySets[i].Value.GetPropertyValue();
				}
			}
			return identiferToBoxMap[project.Boxes[0].ProjectIdentifier];
		}
		
		private Ferda.ModulesManager.NetworkArchive archive;
		private Ferda.ProjectManager.IProjectLoader projectLoader;
	}
}
