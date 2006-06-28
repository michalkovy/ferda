//#define CONSOLE_APPLICATION_FOR_SERVICE_STATARTUP_DEBUGGING
using Ice;

namespace Ferda.Modules.Boxes.GuhaMining
{
    /// <summary>
    /// Represents a IceBox service for common boxes for data mining
    /// </summary>
    public class Service : FerdaServiceI
    {
        /// <summary>
        /// Register box modules to Ice.ObjectAdapter.
        /// </summary>
        /// <remarks>
        /// Remember if you are adding registering of new box module,
        /// you must also change application.xml filePath in config directory.
        /// </remarks>
        protected override void registerBoxes()
        {
            // guha mining setting
            registerBox(AtomSetting.BoxInfo.typeIdentifier, new AtomSetting.BoxInfo());
            registerBox(ClassOfEquivalence.BoxInfo.typeIdentifier, new ClassOfEquivalence.BoxInfo());
            registerBox(ConjunctionSetting.BoxInfo.typeIdentifier, new ConjunctionSetting.BoxInfo());
            registerBox(DisjunctionSetting.BoxInfo.typeIdentifier, new DisjunctionSetting.BoxInfo());
            registerBox(FixedAtom.BoxInfo.typeIdentifier, new FixedAtom.BoxInfo());
            registerBox(Sign.BoxInfo.typeIdentifier, new Sign.BoxInfo());
            
            // guha task boxes
            registerBox(Tasks.FourFold.BoxInfo.typeIdentifier, new Tasks.FourFold.BoxInfo());
            
            // guha quantifiers boxes
        }

        public override void AddCustomFactoriesToComunicator(Communicator communicator)
        {
            Ferda.Guha.MiningProcessor.ObjectFactory factory = new Ferda.Guha.MiningProcessor.ObjectFactory();
            Ferda.Guha.MiningProcessor.ObjectFactory.addFactoryToCommunicator(communicator, factory);
        }


        /// <summary>
        /// Says that this service has property boxes
        /// </summary>
        protected override bool havePropertyBoxes
        {
            get { return true; }
        }


#if CONSOLE_APPLICATION_FOR_SERVICE_STATARTUP_DEBUGGING
        private void registerBox(string factoryIdentifier, BoxInfo boxInfo)
        {
        }

        public static void Main(string[] args)
        {
            Service service = new Service();
            service.registerBoxes();
        }
#endif
    }
}