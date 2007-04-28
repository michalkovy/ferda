namespace Ferda.NetworkArchive
{
	/// <summary>
	/// Baby which contains network boxes. Is there for serialization.
	/// </summary>
	public class ArchiveBaby
	{
		/// <summary>
		/// Network box in archive for serialization
		/// </summary>
		public class BoxInArchive
		{
			/// <summary>
			/// Defalt constructor
			/// </summary>
			public BoxInArchive()
			{
			}
			
			/// <summary>
			/// Constructor
			/// </summary>
			/// <param name="label">A  string representing user name of the box</param>
			/// <param name="projectIdentifierOfFirstBox">An int representing project identifier of first box in project</param>
			/// <param name="value">A  Ferda.ProjectManager.Project representing network box</param>
			public BoxInArchive(string label, int projectIdentifierOfFirstBox, Ferda.ProjectManager.Project value)
			{
				this.Label = label;
				this.ProjectIdentifierOfFirstBox = projectIdentifierOfFirstBox;
				this.Value = value;
			}
			
			/// <summary>
			/// User name of the box
			/// </summary>
			public string Label;
			
			/// <summary>
			/// Project identifier of box in project which has to represent main box
			/// </summary>
			public int ProjectIdentifierOfFirstBox;
			
			/// <summary>
			/// Project representing network box
			/// </summary>
			public Ferda.ProjectManager.Project Value;
		}
		
		/// <summary>
		/// Array with network boxes represented by BoxInArchive class
		/// </summary>
		public BoxInArchive[] BoxesInArchive;
	}
}
