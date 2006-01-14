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
			if(path == null)
			{
				path = Path.GetFullPath(Path.Combine(savePath,identifier + ".pdf"));
			}
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
