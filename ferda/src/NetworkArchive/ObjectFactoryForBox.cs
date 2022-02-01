// ObjectFactoryForBox.cs
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

namespace Ferda.NetworkArchive
{
	/// <summary>
	/// Object factory for network archive box
	/// </summary>
	public class ObjectFactoryForBox : Ice.ObjectFactory
	{
		/// <summary>
		/// Creates network archive box
		/// </summary>
		/// <returns>An Ice.Object representing network archive box</returns>
		/// <param name="type">A  string with type</param>
		public Ice.Object create(string type)
		{
			switch(type)
			{
				case "::Ferda::NetworkArchive::Box":
					return new Box();
			}
			System.Diagnostics.Debug.Assert(false);
			return null;
		}
		
		/// <summary>
		/// Destroys objct factory
		/// </summary>
		public void destroy()
		{
			// Nothing to do
		}

		/// <summary>
		/// Adds factory to ice comunicator
		/// </summary>
		/// <param name="communicator">An Ice.Communicator</param>
		/// <param name="factory">An ObjectFactoryForBox</param>
		public static void addFactoryToCommunicator(Ice.Communicator communicator,
				ObjectFactoryForBox factory)
		{
            lock (communicator)
            {
                if(communicator.findObjectFactory("::Ferda::NetworkArchive::Box")==null)
                    communicator.addObjectFactory(factory, "::Ferda::NetworkArchive::Box");
            }
		}
	}
}
