// 
//
// Author: Michal Kováč <michal.kovac.develop@centrum.cz>
//
// Copyright (c) 2005 Michal Kováč 
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

using System.Collections.Generic;
using System.IO;

namespace Ferda.ModulesManager
{
	public class HelpFiles
	{
		struct HelpFileManagersInfo
		{
			private string identifier;
			private int version;
			private string path;
			
			///<summary>
			/// Constructor
			/// </summary>
			/// <param name="identifier">A string</param>
			/// <param name="version">An int</param>
			/// <param name="path">A string</param>
			public HelpFileManagersInfo(string identifier, int version, string path)
			{
				this.identifier = identifier;
				this.version = version;
				this.path = path;
			}
			
			public string Identifier
			{
				set {
					identifier = value;
				}
				
				get {
					return identifier;
				}
			}
			
			public int Version
			{
				set {
					version = value;
				}
				
				get {
					return version;
				}
			}
			
			public string Path
			{
				set {
					path = value;
				}
				
				get {
					return path;
				}
			}
		}
		
		public string GetHelpFilePath(string identifier)
		{
			HelpFileManagersInfo fileNfo;
			if(this.heplFiles.TryGetValue(identifier, out fileNfo))
			{
				return fileNfo.Path;
			}
			return null;
		}
		
		public int GetHelpFileVersion(string identifier)
		{
			HelpFileManagersInfo fileNfo;
			if(this.heplFiles.TryGetValue(identifier, out fileNfo))
			{
				return fileNfo.Version;
			}
			return -1;
		}
		
		public void SaveHelpFile(string identifier, int version ,byte[] file)
		{
			string path = GetHelpFilePath(identifier);
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
                File.Delete(path);
			path = Path.GetFullPath(Path.Combine(savePath,identifier + ".pdf"));
			FileStream stream = new FileStream(path,FileMode.Create);
			try{
				stream.Write(file, 0, file.Length);
			}
			finally
			{
				stream.Close();
			}
			heplFiles[identifier] = new HelpFileManagersInfo(identifier,
															 version,
															 path);
		}
		
		private Dictionary<string,HelpFileManagersInfo> heplFiles =
			new Dictionary<string,HelpFileManagersInfo>();
		const string savePath = "HelpFiles";
	}
}
