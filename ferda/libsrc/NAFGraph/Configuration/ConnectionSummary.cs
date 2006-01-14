using System;

namespace Netron.GraphLib.Configuration
{
	/// <summary>
	/// 
	/// </summary>
	public class ConnectionSummary
	{
		
		#region Fields
		
		protected string mConnectionName;		
		protected string mLibPath;
		protected string mReflectionName;
		protected string mDescription = "No description available.";


		#endregion

		#region Properties
		public string ReflectionName
		{
			get{return mReflectionName;}
			set{mReflectionName = value;}
		}

		public string LibPath
		{
			get{return mLibPath;}
			set{mLibPath = value;}
		}
		public string Description
		{
			get{return mDescription;}
			set{mDescription = value;}
		}

		
		public string ConnectionName
		{
			get{return mConnectionName;}
			set{mConnectionName = value;}
		}

		
		#endregion

		#region Constructors
		public ConnectionSummary()
		{
			
		}
		public ConnectionSummary(string libraryPath,  string connectionName,  string reflectionName)
		{
			this.mLibPath=libraryPath;			
			this.mConnectionName = connectionName;			
			this.mReflectionName = reflectionName;
		}
		public ConnectionSummary(string libraryPath,  string connectionName,  string reflectionName, string description)
		{
			this.mLibPath=libraryPath;			
			this.mConnectionName = connectionName;			
			this.mReflectionName = reflectionName;
			this.mDescription = description;
		}

		#endregion
	}
}
