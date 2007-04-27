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
using System.Collections.Generic;

namespace Ferda.NetworkArchive
{
	/// <summary>
	/// Description of NetworkArchiveI.
	/// </summary>
	public class ArchiveI : ArchiveDisp_
	{
		public override void addBox(Box boxValue, string label, Ice.Current current__)
		{
			lock(archive)
			{
				if(archive.ContainsKey(label))
				{
					throw new NameExistsError();
				}
				if(boxValue == null)
				{
					throw new NullParamError();
				}
				archive.Add(label, boxValue);
			}
		}
		
		public override void removeBox(string label, Ice.Current current__)
		{
			lock(archive)
			{
				if(!archive.ContainsKey(label))
				{
					throw new NameNotExistsError();
				}
				archive.Remove(label);
			}
		}
		
		public override Box getBox(string label, Ice.Current current__)
		{
			Box returnValue;
			lock(archive)
			{
				if(!archive.TryGetValue(label, out returnValue))
				{
					throw new NameNotExistsError();
				}
			}
			return returnValue;
		}
		
		public override string[] listLabels(Ice.Current current__)
		{
			lock(archive)
			{
				string[] result = new string[archive.Count];
				archive.Keys.CopyTo(result, 0);
				return result;
			}
		}
		
		Dictionary<string,Box> archive =
			new Dictionary<string,Box>();
	}
}
