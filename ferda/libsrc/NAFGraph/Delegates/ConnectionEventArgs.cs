using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;

	namespace Netron.GraphLib
	{
	
		/// <summary>
		/// Provides data for the new connection event
		/// </summary>
		public class ConnectionEventArgs : EventArgs
		{
			private Connector to;
			private Connector from;
			private Connection connection;
			private bool manual = false;
				

			/// <summary>
			/// Gets whether the new connection was created manually, i.e. via user interaction.
			/// If false it means that the connection was created programmatically.
			/// </summary>
			public bool Manual
			{
				get{return manual;}
			}

			/// <summary>
			/// Initializes a new instance of the ConnectionEventArgs class.
			/// </summary>			
			public ConnectionEventArgs(Connection connection)
			{
				this.connection=connection;
				this.to=connection.To;
				this.from=connection.From;
			}

			public ConnectionEventArgs(Connection connection, bool manual)
			{
				this.connection=connection;
				this.to=connection.To;
				this.from=connection.From;
				this.manual = manual;
			}

			/// <summary>
			/// Gets the newly created connection
			/// </summary> 
			public Connection Connection
			{
				get
				{ 
					return connection;
				}
			}

			/// <summary>
			/// Gets the 'to' connector of the connection
			/// </summary> 
			public Connector To
			{
				get
				{ 
					return to;
				}
			}
			/// <summary>
			/// Gets the 'from' connector of the connection
			/// </summary> 
			public Connector From
			{
				get
				{ 
					return from;
				}
			}
			
		}

	}


