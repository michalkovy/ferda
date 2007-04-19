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

using System;
using Ferda.ModulesManager;
using System.Collections.Generic;

namespace Ferda.ProjectManager
{
	/// <summary>
	/// Description of NetworkArchive.
	/// </summary>
	public class NetworkArchive
	{
		public NetworkArchive(Ferda.ModulesManager.ModulesManager modulesManager)
		{
			this.modulesManager = modulesManager;
			this.archive = modulesManager.Helper.NetworkArchive;
		}
		
		public void AddBox(IBoxModule boxModule, string label)
		{
			archive.AddBox(createBoxFromBoxModule(boxModule), label);
		}
		
		public void RemoveBox(string label)
		{
			archive.RemoveBox(label);
		}
		
		public IBoxModule[] GetBox(string label)
		{
			return createBoxModulesFromBox(archive.GetBox(label));
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
			return null;
		}
		
		private IBoxModule[] createBoxModulesFromBox(Ferda.NetworkArchive.Box box)
		{
			return null;
		}
		
		private Project createProjectBoxFromBox(Ferda.NetworkArchive.Box box)
		{
			Dictionary<Ferda.NetworkArchive.Box, int> projectIdentifiers =
				new Dictionary<Ferda.NetworkArchive.Box, int>();
			List<Ferda.NetworkArchive.Box> undoneBoxes =
				new List<Ferda.NetworkArchive.Box>(new Ferda.NetworkArchive.Box[]{box});
			int lastProjectIdentifier =  1;
			projectIdentifiers.Add(box, lastProjectIdentifier);
			
			List<Project.Box> projectBoxes = new List<Project.Box>();
			
			while(undoneBoxes.Count > 0)
			{
				addBoxToProjectBoxes(undoneBoxes[0], projectBoxes, projectIdentifiers, undoneBoxes, ref lastProjectIdentifier);
			}
			
			Project project = new Project();
			project.Boxes = projectBoxes.ToArray();
			project.Views = new Project.View[0];
			
			return project;
		}
		
		private static void addBoxToProjectBoxes(Ferda.NetworkArchive.Box box, List<Project.Box> projectBoxes, Dictionary<Ferda.NetworkArchive.Box, int> projectIdentifiers, List<Ferda.NetworkArchive.Box> undoneBoxes, ref int lastProjectIdentifier)
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
				//projectBox.PropertySets[i].Value =  (Ferda.Modules.ValueT)box.PropertySets[i].value;
			}
			projectBoxes.Add(projectBox);
			undoneBoxes.Remove(box);
		}
		
		private Ferda.ModulesManager.ModulesManager modulesManager;
		private Ferda.ModulesManager.NetworkArchive archive;
	}
}
