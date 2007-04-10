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

namespace Ferda.ModulesManager
{
	/// <summary>
	/// Description of NetworkArchive.
	/// </summary>
	public class NetworkArchive
	{
		public NetworkArchive(Ice.ObjectAdapter adapter)
		{
			Ice.Communicator communicator =
					adapter.getCommunicator();
			archive = Ferda.NetworkArchive.ArchivePrxHelper.checkedCast(
				communicator.stringToProxy("Ferda.NetworkArchive.Archive"));
		}
		
		public void AddBox(IBoxModule boxModule, string label)
		{
			if(boxModule == null)
			{
				throw new Ferda.NetworkArchive.NullParamError();
			}
			archive.addBox(null, label);
		}
		
		public void RemoveBox(string label)
		{
			archive.removeBox(label);
		}
		
		public string[] Labels
		{
			get
			{
				return archive.listLabels();
			}
		}
		
		Ferda.NetworkArchive.ArchivePrx archive;
	}
}
