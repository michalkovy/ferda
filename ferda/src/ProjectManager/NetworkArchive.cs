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
		
		public IBoxModule GetBox(string label)
		{
			return createBoxModuleFromBox(archive.GetBox(label));
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
		
		private IBoxModule createBoxModuleFromBox(Ferda.NetworkArchive.Box box)
		{
			return null;
		}
		
		private Ferda.ModulesManager.ModulesManager modulesManager;
		private Ferda.ModulesManager.NetworkArchive archive;
	}
}
