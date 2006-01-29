using Ice;
using Ferda.ModulesManager;
using Ferda.ProjectManager;
using System.Resources;
using System.Threading;

namespace Ferda.FrontEnd
{
    /// <summary>
    /// Class for displaying the BoxExceptionDialog to the user in another thread
    /// </summary>
    public class BoxExceptionThreadClass
    {
        /// <summary>
        /// Resource manager
        /// </summary>
        protected ResourceManager resourceManager;
        /// <summary>
        /// Box that has thrown the exception
        /// </summary>
        protected IBoxModule box;
        /// <summary>
        /// Message to be displayed to the user
        /// </summary>
        protected string userMessage;
        /// <summary>
        /// Modules manager of the application
        /// </summary>
        protected ModulesManager.ModulesManager modulesManager;

        /// <summary>
        /// Default constructor of the class
        /// </summary>
        /// <param name="mod">Modules manager of the application</param>
        /// <param name="resManager">Resource manager</param>
        /// <param name="box">Box that has thrown the exception</param>
        /// <param name="userMessage">Message to be displayed to the user</param>
        public BoxExceptionThreadClass(ModulesManager.ModulesManager mod,
            ResourceManager resManager, IBoxModule box, string userMessage)
        {
            this.resourceManager = resManager;
            this.box = box;
            this.userMessage = userMessage;
            this.modulesManager = mod;
        }

        /// <summary>
        /// The procedure for the new thread
        /// </summary>
        public void ThreadStart()
        {
            modulesManager.UnlockAllBoxes();
            BoxExceptionDialog dialog = new BoxExceptionDialog(resourceManager,
                box.UserName, userMessage);
            dialog.ShowDialog();
        }
    }
}
