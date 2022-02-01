// Service.cs - IceBox service with network archive
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
using System.Diagnostics;
using System.Threading;
using Ice;
using IceBox;

namespace Ferda.NetworkArchive
{
    /// <summary>
    /// Represents a IceBox service, is created for inheriting
    /// </summary>
    public class Service : IceBox.Service
    {
        /// <summary>
        /// Service execution method
        /// </summary>
        /// <param name="name">Name of service</param>
        /// <param name="communicator">Ice communicator</param>
        /// <param name="args">Arguments from command line</param>
        public void start(string name, Communicator communicator, string[] args)
        {
            Debug.Listeners.Clear();
            Debug.Listeners.Add(new TextWriterTraceListener(name + ".log"));
            Debug.AutoFlush = true;
            Debug.WriteLine("Starting service...");
            _adapter = communicator.createObjectAdapter(name + "Adapter");
            ObjectFactoryForBox factory =
                new ObjectFactoryForBox();
            ObjectFactoryForBox.addFactoryToCommunicator(
                communicator, factory);
			Ferda.Modules.ObjectFactoryForPropertyTypes factoryPropertyTypes =
				new Ferda.Modules.ObjectFactoryForPropertyTypes();
			Ferda.Modules.ObjectFactoryForPropertyTypes.addFactoryToCommunicator(
				communicator, factoryPropertyTypes);
			
			ArchiveI archive = ArchiveI.Instance;
            _adapter.add(archive, Util.stringToIdentity("Ferda.NetworkArchive.Archive"));
			
            Debug.WriteLine("Activating adapter...");
            _adapter.activate();
			
            Debug.WriteLine("NetworkArchive service has started");
        }

        /// <summary>
        /// This will be executed for stopping the service
        /// </summary>
        public void stop()
        {
            _adapter.deactivate();
            Debug.WriteLine("Adapter has deactivated");
            Debug.WriteLine("Service has stoped...");
        }

        private ObjectAdapter _adapter;
    }
}
