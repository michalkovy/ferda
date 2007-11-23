// FerdaServiceI.cs - half implementation of IceBox service
//
// Authors: 
//   Michal KovĂˇÄŤ <michal.kovac.develop@centrum.cz>
//   TomĂˇĹˇ KuchaĹ™ <tomas.kuchar@gmail.com>
//
// Copyright (c) 2005 Michal KovĂˇÄŤ, TomĂˇĹˇ KuchaĹ™ 
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
using System.Collections.Generic;
using Ferda.Modules.Boxes;
using Ice;
using IceBox;

namespace Ferda.Modules
{
    /// <summary>
    /// Represents a IceBox service, is created for inheriting
    /// </summary>
    public abstract class FerdaServiceI : LocalObjectImpl, Service
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
            _adapter = communicator.createObjectAdapter(name);
            ObjectFactoryForPropertyTypes factory =
                new ObjectFactoryForPropertyTypes();
            ObjectFactoryForPropertyTypes.addFactoryToCommunicator(
                communicator, factory);
            Debug.WriteLine("Activating adapter...");
            _adapter.activate();

            reaper = new ReapThread();
            reaperThread = new Thread(new ThreadStart(reaper.Run));
            reaperThread.Start();

            Debug.WriteLine("Registering boxes...");
            registerBoxes();

            if (havePropertyBoxes)
            {
                propertyReaper = new PropertyReapThread();
                propertyReaperThread = new Thread(new ThreadStart(propertyReaper.Run));
                propertyReaperThread.Start();

                Debug.WriteLine("Registering property boxes...");
                registerPropertyBoxes();
            }
        }


        /// <summary>
        /// Adds the custom factories to the comunicator.
        /// </summary>
        /// <param name="communicator">The communicator.</param>
        public virtual void AddCustomFactoriesToComunicator(Communicator communicator)
        {
        }

        /// <summary>
        /// This will be executed for stopping the service
        /// </summary>
        public void stop()
        {
            _adapter.deactivate();
            Debug.WriteLine("Adapter has deactivated");
            reaper.Terminate();
            reaperThread.Join();
            if (havePropertyBoxes)
            {
                propertyReaper.Terminate();
                propertyReaperThread.Join();
            }
            Debug.WriteLine("Service has stoped...");
        }

        /// <summary>
        /// Helper method for registering property box to Ice Object Adapter
        /// </summary>
        /// <param name="type">Name of box</param>
        /// <param name="defaultValue">Default value of box</param>
        /// <param name="valFromPrx">Method which converts property value proxy to property value object</param>
        /// <param name="settingModuleIdentifier">Identifier of setting module the only property of the property box</param>
        public void registerPropertyBox(string type, PropertyValue defaultValue,
                                        PropertyBoxModuleFactoryCreatorI.ValueFromPrx valFromPrx,
                                        string settingModuleIdentifier)
        {
        	List<string> iceIds = new List<string>();
        	string mainIceId = "::Ferda::Modules::" + type + "Interface";
        	iceIds.Add(mainIceId);
        	foreach(string iceId in defaultValue.ice_ids())
        	{
        		if ( (iceId.EndsWith("Interface") || iceId == "::Ice::Object") &&
        			mainIceId != iceId)
        		iceIds.Add(iceId);
        	}
        	
            new PropertyBoxModuleFactoryCreatorI(defaultValue.ice_id(),
                                                 iceIds.ToArray(),
                                                 mainIceId,
                                                 type,
                                                 defaultValue,
                                                 valFromPrx,
                                                 propertyReaper,
                                                 _adapter,
                                                 settingModuleIdentifier);
        }

        /// <summary>
        /// You have to implement there registering of property boxes 
        /// </summary>
        /// <seealso cref="M:Ferda.Modules.FerdaServiceI.registerPropertyBox(System.String,Ferda.Modules.PropertyValue,Ferda.Modules.PropertyBoxModuleFactoryCreatorI.ValueFromPrx,System.String)"/>
        /// <seealso cref="M:Ferda.Modules.FerdaServiceI.havePropertyBoxes()"/>
        protected virtual void registerPropertyBoxes()
        {
        }

        /// <summary>
        /// Helper method for registering standard box to Ice Object Adapter
        /// </summary>
        /// <param name="identity">Name of box</param>
        /// <param name="boxInfo">An <see cref="T:Ferda.Modules.Boxes.IBoxInfo"/> implementation for this box</param>
        public void registerBox(string identity, IBoxInfo boxInfo)
        {
            Debug.WriteLine("Registering " + identity + "...");
            BoxModuleFactoryCreatorI boxModuleFactoryCreator = new BoxModuleFactoryCreatorI(boxInfo, reaper);
            _adapter.add(boxModuleFactoryCreator, Util.stringToIdentity(identity));
        }

        /// <summary>
        /// Register box modules to Ice.ObjectAdapter.
        /// </summary>
        /// <remarks>
        /// Remember if you are adding registering of new box module,
        /// you must also change application.xml file in config directory.
        /// </remarks>
        /// <example>
        /// Sample implementation of this function.
        /// <code>
        /// namespace SampleBoxModules
        /// {
        ///     public class Service : Ferda.Modules.FerdaServiceI
        ///     {
        ///         protected override void registerBoxes()
        ///         {
        ///             // For registering is needed some unique string identifier
        ///             // of the box. The same identifier will be used in application.xml
        ///             // as object identity.
        ///             this.registerBox("SomeUniqueStringIdentifier", new SampleBoxes.SampleBoxModule.SampleBoxModuleBoxInfo());
        ///         }
        ///     }
        /// }
        /// </code>
        /// A part of the application.xml file.
        /// <code>
        /// &lt;icegrid&gt;
        ///     &lt;application name="..."&gt;
        ///         &lt;replica-group id="..." &gt;
        ///             ...
        ///             &lt;object identity="SomeUniqueStringIdentifier" type="::Ferda::Modules::BoxModuleFactoryCreator"/&gt;
        ///             ...
        ///         &lt;/replica-group&gt;
        ///         ...
        ///     &lt;/application&gt;
        /// &lt;/icegrid&gt;
        /// </code>
        /// </example>
        protected abstract void registerBoxes();

        private ObjectAdapter _adapter;

        /// <summary>
        /// Method saying if this service have registered some property boxes
        /// </summary>
        protected virtual bool havePropertyBoxes
        {
            get { return false; }
        }

        private PropertyReapThread propertyReaper;
        private ReapThread reaper;
        private Thread reaperThread;
        private Thread propertyReaperThread;
    }
}