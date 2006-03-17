// HelpFiles.cs - saving, loading and working with help files
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
using System.Xml.Serialization;

namespace Ferda.ModulesManager
{
    /// <summary>
    /// Works with files with help for boxes. Is made for savig and loading files with help
    /// from modules depending on help files version information.
    /// </summary>
	public class HelpFiles
	{
        /// <summary>
        /// Creates new class and loads information about old saved help files from config file
        /// </summary>
        public HelpFiles()
        {
            if (File.Exists(System.IO.Path.Combine(savePath, helpFilesConfigFileName)))
                this.LoadHelpFilesConfig();
        }

        /// <summary>
        /// Struct with information about version of help file loaded to local disk
        /// </summary>
		public struct HelpFileManagersInfo
		{
			private string identifier;
			private int version;
			private string path;
			
			///<summary>
			/// Constructor
			/// </summary>
			/// <param name="identifier">An identifier of help file</param>
			/// <param name="version">Version of help file</param>
			/// <param name="path">Path where is file saved</param>
			public HelpFileManagersInfo(string identifier, int version, string path)
			{
				this.identifier = identifier;
				this.version = version;
				this.path = path;
			}
			
            /// <summary>
            /// An identifier of help file
            /// </summary>
			public string Identifier
			{
				set {
					identifier = value;
				}
				
				get {
					return identifier;
				}
			}
			
            /// <summary>
            /// Version of help file
            /// </summary>
			public int Version
			{
				set {
					version = value;
				}
				
				get {
					return version;
				}
			}
			
            /// <summary>
            /// Path where is file saved
            /// </summary>
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

        /// <summary>
        /// Special struct created because of xml serialization problem with generic dictionary
        /// </summary>
        public struct HelpKeyValue
        {
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="key">Key</param>
            /// <param name="value">Value</param>
            public HelpKeyValue(string key, HelpFileManagersInfo value)
            {
                Key = key;
                Value = value;
            }

            /// <summary>
            /// Key
            /// </summary>
            public string Key;

            /// <summary>
            /// Value
            /// </summary>
            public HelpFileManagersInfo Value;
        }
		
        /// <summary>
        /// Gets path of help with identifier <paramref name="identifier"/>
        /// </summary>
        /// <param name="identifier">An identifier of help file</param>
        /// <returns>String representing path. If string is null, there is no
        /// saved help file with identifier <paramref name="identifier"/></returns>
		public string GetHelpFilePath(string identifier)
		{
			HelpFileManagersInfo fileNfo;
			if(this.helpFiles.TryGetValue(identifier, out fileNfo))
			{
				return fileNfo.Path;
			}
			return null;
		}

        /// <summary>
        /// Gets version of saved help with identifier <paramref name="identifier"/>
        /// </summary>
        /// <param name="identifier">An identifier of help file</param>
        /// <returns>Int representign version.. If return value is null, there is no
        /// saved help file with identifier <paramref name="identifier"/></returns>
		public int? GetHelpFileVersion(string identifier)
		{
			HelpFileManagersInfo fileNfo;
			if(this.helpFiles.TryGetValue(identifier, out fileNfo))
			{
				return fileNfo.Version;
			}
			return null;
		}
		
        /// <summary>
        /// Saves help file
        /// </summary>
        /// <param name="identifier">An identifier of help file</param>
        /// <param name="version">Version of help file</param>
        /// <param name="file">Data with help file</param>
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
			helpFiles[identifier] = new HelpFileManagersInfo(identifier,
															 version,
															 path);
		}

        /// <summary>
        /// Loads information about saved help files from config file.
        /// </summary>
        public void LoadHelpFilesConfig()
        {

            XmlSerializer s = new XmlSerializer(typeof(HelpKeyValue[]));
            TextReader r = new StreamReader(System.IO.Path.Combine(savePath, helpFilesConfigFileName));
            HelpKeyValue[] helpFilesCopy = (HelpKeyValue[])s.Deserialize(r);
            foreach (HelpKeyValue pair in helpFilesCopy)
                helpFiles[pair.Key] = pair.Value;
            r.Close();
        }

        /// <summary>
        /// Saves information about saved help files to config file.
        /// </summary>
        public void SaveHelpFilesConfig()
        {
            // TODO uncomment

            //List<HelpKeyValue> helpFilesCopy =
            //    new List<HelpKeyValue>(helpFiles.Count);
            //foreach(KeyValuePair<string, HelpFileManagersInfo> pair in helpFiles)
            //{
            //    helpFilesCopy.Add(new HelpKeyValue(pair.Key, pair.Value));
            //}
            //XmlSerializer s = new XmlSerializer(typeof(HelpKeyValue[]));
            //TextWriter w = new StreamWriter(System.IO.Path.Combine(savePath, helpFilesConfigFileName));
            //s.Serialize(w, helpFilesCopy.ToArray());
            //w.Close();
        }
		
		private Dictionary<string,HelpFileManagersInfo> helpFiles =
			new Dictionary<string,HelpFileManagersInfo>();
		const string savePath = "HelpFiles";
        const string helpFilesConfigFileName = "HelpFilesConfig.xml";
	}
}
