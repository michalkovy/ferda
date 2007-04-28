// ArchiveI.cs - implementation of network archive service
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
using System.Xml.Serialization;
using System.IO;
using Ferda.ProjectManager;

namespace Ferda.NetworkArchive
{
	/// <summary>
	/// Implementation of network archive service
	/// </summary>
	public sealed class ArchiveI : ArchiveDisp_
	{
		static readonly ArchiveI instance = new ArchiveI();
		
		private const string fileNameForStore = "NetworkArchive.xml";
		private const string fileNameForStoreTemporaryNew = "NetworkArchive.xml.new";
		private const string fileNameForStoreTemporaryBackup = "NetworkArchive.xml.bak";
		
		/// <summary>
		/// Singleton instance
		/// </summary>
		public static ArchiveI Instance
		{
			get
			{
				return instance;
			}
		}
		
		/// <summary>
		/// Loads data from stored file
		/// </summary>
		static ArchiveI()
		{
			if(System.IO.File.Exists(fileNameForStore))
			{
				XmlSerializer s = new XmlSerializer( typeof( ArchiveBaby ) );
				System.IO.FileStream fs = new System.IO.FileStream(fileNameForStore, System.IO.FileMode.Open);
				ArchiveBaby archiveBaby = (ArchiveBaby)s.Deserialize( fs );
				fs.Close();
				foreach(ArchiveBaby.BoxInArchive boxInArchive in archiveBaby.BoxesInArchive)
				{
					Box box = ProjectConverter.CreateBoxFromProject(boxInArchive.Value, boxInArchive.ProjectIdentifierOfFirstBox);
					archive.Add(boxInArchive.Label, box);
				}
			}
		}
		
		private static void serialize()
		{
			XmlSerializer s = new XmlSerializer( typeof( ArchiveBaby ) );
            System.IO.FileStream fs = new System.IO.FileStream(fileNameForStoreTemporaryNew, System.IO.FileMode.Create);
			ArchiveBaby archiveBaby = new ArchiveBaby();
			archiveBaby.BoxesInArchive = new ArchiveBaby.BoxInArchive[archive.Count];
			int i = 0;
			foreach(KeyValuePair<string, Box> pair in archive)
			{
				Ferda.ProjectManager.Project.Box projectBox = null;
				archiveBaby.BoxesInArchive[i] = new ArchiveBaby.BoxInArchive();
				archiveBaby.BoxesInArchive[i].Label = pair.Key;
				archiveBaby.BoxesInArchive[i].Value = ProjectConverter.CreateProjectFromBox(pair.Value, out projectBox);
				archiveBaby.BoxesInArchive[i].ProjectIdentifierOfFirstBox = projectBox.ProjectIdentifier;
				i++;
			}
			
			s.Serialize( fs, archiveBaby );
            fs.Close();
			if(System.IO.File.Exists(fileNameForStore))
				System.IO.File.Move(fileNameForStore, fileNameForStoreTemporaryBackup);
			System.IO.File.Move(fileNameForStoreTemporaryNew, fileNameForStore);
			if(System.IO.File.Exists(fileNameForStoreTemporaryBackup))
				System.IO.File.Delete(fileNameForStoreTemporaryBackup);
		}
		
		/*
		private static Box CreateBoxesFromArchiveBaby(Dictionary<ArchiveBaby.Box, Box> doneBoxes, ArchiveBaby.Box babyBox)
		{
			Box result = new Box();
			doneBoxes.Add(babyBox, result);
			
			result.creatorIdentifier = babyBox.CreatorIdentifier;
			result.userHint          = babyBox.UserHint;
			result.userName          = babyBox.UserName;
			
			List<Connection> connections = new List<Connection>(babyBox.Connections.Length);
			foreach(ArchiveBaby.Box.Connection babyConnection in babyBox.Connections)
			{
				Connection connection = new Connection();
				connection.socketName = babyConnection.SocketName;
				if(!doneBoxes.TryGetValue(babyConnection.BoxValue, out connection.boxValue))
				{
					connection.boxValue = CreateBoxesFromArchiveBaby(doneBoxes, babyConnection.BoxValue);
				}
				connections.Add(connection);
			}
			result.Connections = connections.ToArray();
			
			List<Ferda.Modules.PropertySetting> propertySettings = new List<Ferda.Modules.PropertySetting>((babyBox.PropertySets == null) ? 0 : babyBox.PropertySets.Length);
			foreach(ArchiveBaby.Box.PropertySet s in babyBox.PropertySets)
			{
				Ferda.Modules.PropertySetting propertySetting = new Ferda.Modules.PropertySetting(s.PropertyName, (s.Value == null) ? null : s.Value.GetPropertyValue());
				propertySettings.Add(propertySetting);
			}
			result.PropertySets = propertySettings.ToArray();
			return result;
		}
		
		static void Serialize()
		{
			XmlSerializer s = new XmlSerializer( typeof( ArchiveBaby ) );
            System.IO.FileStream fs = new System.IO.FileStream( "NetworkArchive.xml.new", System.IO.FileMode.Create);
			List<ArchiveBaby.StringBoxPair> newPairs = new List<ArchiveBaby.StringBoxPair>(archive.Count);
			foreach(KeyValuePair<string, Box> pair in archive)
			{
				Dictionary<Box, ArchiveBaby.Box> doneBoxes = new Dictionary<Box, ArchiveBaby.Box>();
				newPairs.Add(new ArchiveBaby.StringBoxPair(pair.Key, CreateBabyBoxesFromBox(doneBoxes, pair.Value)));
			}
			ArchiveBaby archiveBaby = new ArchiveBaby();
			archiveBaby.Pairs = newPairs.ToArray();
			
			s.Serialize( fs, archiveBaby );
            fs.Close();
			if(System.IO.File.Exists("NetworkArchive.xml"))
				System.IO.File.Move("NetworkArchive.xml", "NetworkArchive.xml.bak");
			System.IO.File.Move("NetworkArchive.xml.new", "NetworkArchive.xml");
			if(System.IO.File.Exists("NetworkArchive.xml.bak"))
				System.IO.File.Delete("NetworkArchive.xml.bak");
		}
		
		private static ArchiveBaby.Box CreateBabyBoxesFromBox(Dictionary<Box, ArchiveBaby.Box> doneBoxes, Box box)
		{
			ArchiveBaby.Box result = new ArchiveBaby.Box();
			doneBoxes.Add(box, result);
			
			result.CreatorIdentifier = box.creatorIdentifier;
			result.UserHint          = box.userHint;
			result.UserName          = box.userName;
			
			List<ArchiveBaby.Box.Connection> babyConnections = new List<ArchiveBaby.Box.Connection>(box.Connections.Length);
			foreach(Connection connection in box.Connections)
			{
				ArchiveBaby.Box.Connection babyConnection = new ArchiveBaby.Box.Connection();
				babyConnection.SocketName = connection.socketName;
				if(!doneBoxes.TryGetValue(connection.boxValue, out babyConnection.BoxValue))
				{
					babyConnection.BoxValue = CreateBabyBoxesFromBox(doneBoxes, connection.boxValue);
				}
				babyConnections.Add(babyConnection);
			}
			result.Connections = babyConnections.ToArray();
			
			result.PropertySets = new ArchiveBaby.Box.PropertySet[box.PropertySets.Length];
			for(int i = 0; i < box.PropertySets.Length; i++)
			{
				result.PropertySets[i].PropertyName = box.PropertySets[i].propertyName;
				result.PropertySets[i].Value =  ((Ferda.Modules.IValue)box.PropertySets[i].value).getValueT();
			}
			return result;
		}*/
		
		/// <summary>
		/// Adds box to network archive
		/// </summary>
		/// <param name="boxValue">A  Box</param>
		/// <param name="label">A  string representing user name of box</param>
		/// <param name="current__">An Ice.Current</param>
		public override void addBox(Box boxValue, string label, Ice.Current current__)
		{
			lock(this)
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
				serialize();
			}
		}
		
		/// <summary>
		/// Removes network box from archive
		/// </summary>
		/// <param name="label">A  string representing user name of box for removal</param>
		/// <param name="current__">An Ice.Current</param>
		public override void removeBox(string label, Ice.Current current__)
		{
			lock(this)
			{
				if(!archive.ContainsKey(label))
				{
					throw new NameNotExistsError();
				}
				archive.Remove(label);
				serialize();
			}
		}
		
		/// <summary>
		/// Returns stored box in archive
		/// </summary>
		/// <returns>A Box</returns>
		/// <param name="label">A  string representing box for returning</param>
		/// <param name="current__">An Ice.Current</param>
		public override Box getBox(string label, Ice.Current current__)
		{
			Box returnValue;
			lock(this)
			{
				if(!archive.TryGetValue(label, out returnValue))
				{
					throw new NameNotExistsError();
				}
			}
			return returnValue;
		}
		
		/// <summary>
		/// Get user names of boxes stored in network archive. These names
		/// are keys for returning boxes
		/// </summary>
		/// <returns>A string[] with names of box</returns>
		/// <param name="current__">An Ice.Current</param>
		public override string[] listLabels(Ice.Current current__)
		{
			lock(this)
			{
				string[] result = new string[archive.Count];
				archive.Keys.CopyTo(result, 0);
				return result;
			}
		}
		
		private static Dictionary<string,Box> archive =
			new Dictionary<string,Box>();
	}
}
