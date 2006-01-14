using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Ferda.ProjectManager
{
	/// <sumary>
	/// Class for project serializating
	/// </sumary>
	public class Project
	{
		public class Box
		{
			public struct Connection
			{
				public string SocketName;
				public int BoxProjectIdentifier;
			}
			
			public struct PropertySet
			{

				
				public string PropertyName;
				public Ferda.Modules.ValueT Value;
			}
			
			public string CreatorIdentifier;
			public string UserName;
			public string UserHint;
			public int ProjectIdentifier;
			public Project.Box.Connection[] Connections;
			public Project.Box.PropertySet[] PropertySets;
		}
		
		public class View
		{
			public struct PositionSet
			{
				public int BoxProjectIdentifier;
				public System.Drawing.PointF Position;
			}
			
			public string Name;
			public Project.View.PositionSet[] PositionSets;
		}
		
		public Project.View[] Views;
		public Project.Box[] Boxes;
	}
}
