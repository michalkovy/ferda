// Project.cs - Definition of class for project serialization
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

using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Ferda.ProjectManager
{
    /// <summary>
    /// Class for project serialization
    /// </summary>
    public class Project
	{
        /// <summary>
        /// Class representing a box in serialized structure
        /// </summary>
		public class Box
		{
            /// <summary>
            /// Connection between boxes
            /// </summary>
			public struct Connection
			{
                /// <summary>
                /// Name of socket
                /// </summary>
				public string SocketName;

                /// <summary>
                /// Unicate identifier of box in project
                /// </summary>
				public int BoxProjectIdentifier;
			}
			
            /// <summary>
            /// Property with specified value
            /// </summary>
			public struct PropertySet
			{
                /// <summary>
                /// Name of property
                /// </summary>
				public string PropertyName;

                /// <summary>
                /// Value of property
                /// </summary>
				public Ferda.Modules.ValueT Value;
			}
			
            /// <summary>
            /// Identifier of creator of factory for box
            /// </summary>
			public string CreatorIdentifier;

            /// <summary>
            /// Name of box instance defined by user
            /// </summary>
			public string UserName;

            /// <summary>
            /// User notes for box instance
            /// </summary>
			public string UserHint;

            /// <summary>
            /// Project identifier of box instance
            /// </summary>
			public int ProjectIdentifier;

            /// <summary>
            /// What is connected in sockets
            /// </summary>
			public Project.Box.Connection[] Connections;

            /// <summary>
            /// How are properties set
            /// </summary>
			public Project.Box.PropertySet[] PropertySets;
		}
		
        /// <summary>
        /// Class representing a view in serialized structure
        /// </summary>
		public class View
		{
            /// <summary>
            /// Position of box in view
            /// </summary>
			public struct PositionSet
			{
                /// <summary>
                /// Identifier of box instance
                /// </summary>
				public int BoxProjectIdentifier;

                /// <summary>
                /// Where is box in view
                /// </summary>
				public System.Drawing.PointF Position;
			}
			
            /// <summary>
            /// Name of view defined by user
            /// </summary>
			public string Name;

            /// <summary>
            /// Which boxes are in view and where
            /// </summary>
			public Project.View.PositionSet[] PositionSets;
		}
		
        /// <summary>
        /// Views of boxes which are in project archive
        /// </summary>
		public Project.View[] Views;

        /// <summary>
        /// Boxes in archive
        /// </summary>
		public Project.Box[] Boxes;
	}
}
