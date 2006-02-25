using System;
using System.Collections.Generic;
using System.Text;
using IceBox;
using System.Diagnostics;
using System.Threading;

namespace Ferda.Modules
{
    /// <summary>
    /// TODO Michal ... kompletne zdokumentovat celou tridu
    /// </summary>
    public abstract class FerdaServiceI : Ice.LocalObjectImpl, IceBox.Service 
    {
        /// <summary>
        /// TODO Michal
        /// </summary>
        /// <param name="name"></param>
        /// <param name="communicator"></param>
        /// <param name="args"></param>
        public void start(string name, Ice.Communicator communicator, string[] args)
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
        /// TODO Michal
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
        /// TODO Michal
        /// </summary>
        /// <param name="type"></param>
        /// <param name="defaultValue"></param>
        /// <param name="valFromPrx"></param>
        public void registerPropertyBox(string type, PropertyValue defaultValue, PropertyBoxModuleFactoryCreatorI.ValueFromPrx valFromPrx)
        {
            PropertyBoxModuleFactoryCreatorI newCreator = new PropertyBoxModuleFactoryCreatorI("::Ferda::Modules::" + type,
                                                           new string[] { "::Ferda::Modules::" + type + "Interface" },
                                                           type,
                                                           defaultValue,
                                                           valFromPrx,
                                                           propertyReaper,
                                                           _adapter);
        }

        /// <summary>
        /// TODO Michal
        /// </summary>
        protected virtual void registerPropertyBoxes() { }

        /// <summary>
        /// TODO Michal
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="boxInfo"></param>
        public void registerBox(string identity, Ferda.Modules.Boxes.IBoxInfo boxInfo)
        {
            Debug.WriteLine("Registering " + identity + "...");
            BoxModuleFactoryCreatorI boxModuleFactoryCreator = new BoxModuleFactoryCreatorI(boxInfo, reaper);
            Ice.ObjectPrx newPrx = _adapter.add(boxModuleFactoryCreator, Ice.Util.stringToIdentity(identity));
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

        private Ice.ObjectAdapter _adapter;

        /// <summary>
        /// TODO Michal
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
