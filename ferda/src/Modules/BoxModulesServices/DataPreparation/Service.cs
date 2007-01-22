//#define CONSOLE_APPLICATION_FOR_SERVICE_STATARTUP_DEBUGGING
namespace Ferda.Modules.Boxes.DataPreparation
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
            registerBox(Datasource.Database.BoxInfo.typeIdentifier,
                        new Datasource.Database.BoxInfo());
            registerBox(Datasource.DataTable.BoxInfo.typeIdentifier,
                        new Datasource.DataTable.BoxInfo());
            registerBox(Datasource.Column.BoxInfo.typeIdentifier,
                        new Datasource.Column.BoxInfo());
            
            
            registerBox(DataPreparation.Categorization.EquidistantIntervals.BoxInfo.typeIdentifier,
                        new DataPreparation.Categorization.EquidistantIntervals.BoxInfo());
            registerBox(DataPreparation.Categorization.EquifrequencyIntervals.BoxInfo.typeIdentifier,
                        new DataPreparation.Categorization.EquifrequencyIntervals.BoxInfo());
            registerBox(Categorization.EachValueOneCategory.BoxInfo.typeIdentifier,
                       new Categorization.EachValueOneCategory.BoxInfo());
            registerBox(Categorization.StaticAttribute.BoxInfo.typeIdentifier,
                       new Categorization.StaticAttribute.BoxInfo());
            registerBox(Datasource.VirtualColumn.BoxInfo.typeIdentifier,
                        new Datasource.VirtualColumn.BoxInfo());

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
            BoxInfoLoadTesting.Test(boxInfo, new string[] { "en-US" });
        }

        public static void Main(string[] args)
        {
            Service service = new Service();
            service.registerBoxes();
        }
#endif
    }
}