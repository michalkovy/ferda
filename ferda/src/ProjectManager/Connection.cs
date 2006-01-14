using Ferda.ModulesManager;

namespace Ferda.ProjectManager
{
	/// <summary>
	/// Connection between boxes
	/// </summary>
	public struct Connection
	{
		private IBoxModule fromBox;
		private IBoxModule toBox;
		private string toSocket;
		
		///<summary>
		/// Constructs new connection
		/// </summary>
		/// <param name="fromBox">An <see cref="T:Ferda.ModulesManager.IBoxModule"/> from which connection is</param>
		/// <param name="toBox">An <see cref="T:Ferda.ModulesManager.IBoxModule"/> to which connection goes</param>
		/// <param name="toSocket">A string representing name of socket of <paramref name="toBox"/> to which connection goes</param>
		public Connection(IBoxModule fromBox, IBoxModule toBox, string toSocket)
		{
			this.fromBox = fromBox;
			this.toBox = toBox;
			this.toSocket = toSocket;
		}
		
		/// <summary>Box from which connection is</summary>
		/// <value>An <see cref="T:Ferda.ModulesManager.IBoxModule"/>
		/// from which connection is</value>
		public IBoxModule FromBox
		{
			set {
				fromBox = value;
			}
			
			get {
				return fromBox;
			}
		}
		
		/// <summary>Box to which connection goes</summary>
		/// <value>An <see cref="T:Ferda.ModulesManager.IBoxModule"/>
		/// to which connection goes</value>
		public IBoxModule ToBox
		{
			set {
				toBox = value;
			}
			
			get {
				return toBox;
			}
		}
		
		/// <summary>Socket to which connection goes</summary>
		/// <value>A string representing name of socket of
		/// <see cref="P:Ferda.ProjectManager.Connection.ToBox"/>
		/// to which connection goes</value>
		public string ToSocket
		{
			set {
				toSocket = value;
			}
			
			get {
				return toSocket;
			}
		}
	}
}
