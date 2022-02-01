using Ice;

namespace Ferda.Modules.Boxes.OntologyRelated
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
            registerBox(Ontology.BoxInfo.typeIdentifier, new Ontology.BoxInfo());
            registerBox(OntologyMapping.BoxInfo.typeIdentifier, new OntologyMapping.BoxInfo());
        }

        /// <summary>
        /// Says that this service has property boxes
        /// </summary>
            
        //část kódu z jiného Service.cs - je to relevantní? nebo smazat?
        //protected override bool havePropertyBoxes
        //{
        //    get { return true; }
        //}
    }
}
