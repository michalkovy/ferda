using Ferda.ProjectManager;
using System.Collections.Generic;
using Ferda.Modules;

namespace Ferda.NetworkArchive
{
	/// <summary>
	/// Convertor of network box to project and from project to network box
	/// </summary>
	public static class ProjectConverter
	{
		/// <summary>
		/// Creates project from network box
		/// </summary>
		/// <returns>A Project</returns>
		/// <param name="box">A  Ferda.NetworkArchive.Box</param>
		/// <param name="mainProjectBox">A  Project.Box representing main box in project,
		/// that is representation of main box in network box (network box also
		/// contains boxes connected to this main box (transitively))</param>
		public static Project CreateProjectFromBox(Ferda.NetworkArchive.Box box, out Project.Box mainProjectBox)
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
			projectBox.CreatorIdentifier = box.creatorIdentifier;
			projectBox.UserHint = box.userHint;
			projectBox.UserName = box.userName;
			projectBox.Connections = new Project.Box.Connection[box.Connections.Length];
			projectBox.ProjectIdentifier = projectIdentifiers[box];
			for(int i = 0; i < box.Connections.Length; i++)
			{
				projectBox.Connections[i].SocketName = box.Connections[i].socketName;
				if(!projectIdentifiers.TryGetValue(box.Connections[i].boxValue, out projectBox.Connections[i].BoxProjectIdentifier))
				{
					++lastProjectIdentifier;
					projectBox.Connections[i].BoxProjectIdentifier = lastProjectIdentifier;
					projectIdentifiers.Add(box.Connections[i].boxValue, lastProjectIdentifier);
					undoneBoxes.Add(box.Connections[i].boxValue);
				}
			}
			projectBox.PropertySets = new Project.Box.PropertySet[box.PropertySets.Length];
			for(int i = 0; i < box.PropertySets.Length; i++)
			{
				projectBox.PropertySets[i].PropertyName = box.PropertySets[i].propertyName;
				projectBox.PropertySets[i].Value =  ((Ferda.Modules.IValue)box.PropertySets[i].value).getValueT();
			}
			projectBox.SockedProperties = box.SockedProperties;
			projectBoxes.Add(projectBox);
			undoneBoxes.Remove(box);
			return projectBox;
		}
		
		/// <summary>
		/// Creates network box from project
		/// </summary>
		/// <returns>A Ferda.NetworkArchive.Box - new network box</returns>
		/// <param name="project">A Project which has to be converted</param>
		/// <param name="projectIdentifierOfFirstBox">An int representing project identifier
		/// of main box in project</param>
		public static Ferda.NetworkArchive.Box CreateBoxFromProject(Project project, int projectIdentifierOfFirstBox)
		{
			if((project == null) || (project.Boxes.Length == 0))
				return null;
			
			Dictionary<int,Ferda.NetworkArchive.Box> identiferToBoxMap = new Dictionary<int,Ferda.NetworkArchive.Box>();
			foreach(Project.Box box in project.Boxes)
			{
				identiferToBoxMap.Add(box.ProjectIdentifier,
									  new Ferda.NetworkArchive.Box(
										  box.CreatorIdentifier,
										  box.UserName,
										  box.UserHint,
										  null,
										  null,
										  box.SockedProperties));
			}
			foreach(Project.Box box in project.Boxes)
			{
				List<Ferda.NetworkArchive.Connection> boxConnections =
					new List<Ferda.NetworkArchive.Connection>();
				Ferda.NetworkArchive.Box networkBox = identiferToBoxMap[box.ProjectIdentifier];
				
				foreach(Project.Box.Connection projectConnection in box.Connections)
				{
					Ferda.NetworkArchive.Box otherBox = null;
					if(!identiferToBoxMap.TryGetValue(projectConnection.BoxProjectIdentifier, out otherBox))
					{
						throw new System.Exception(System.String.Format("Can not find box {0}",projectConnection.BoxProjectIdentifier));
					}
					Ferda.NetworkArchive.Connection boxConnection =
						new Ferda.NetworkArchive.Connection(
						projectConnection.SocketName,
						otherBox);
					boxConnections.Add(boxConnection);
				}
				networkBox.Connections = boxConnections.ToArray();
				
				List<PropertySetting> propertySettings = new List<PropertySetting>((box.PropertySets == null) ? 0 : box.PropertySets.Length);
				foreach(Project.Box.PropertySet s in box.PropertySets)
				{
					PropertySetting propertySetting = new PropertySetting(s.PropertyName, (s.Value == null) ? null : s.Value.GetPropertyValue());
					propertySettings.Add(propertySetting);
				}
				networkBox.PropertySets = propertySettings.ToArray();
			}
			return identiferToBoxMap[projectIdentifierOfFirstBox];
		}
	}
}
