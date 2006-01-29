using Ice;
using Ferda.ModulesManager;
using Ferda.ProjectManager;
using System.Resources;
using System.Threading;

namespace Ferda.FrontEnd
{
    /// <summary>
    /// Class that catches the exceptions comming from other layers of the system
    /// and displayes them to the user
    /// </summary>
    public class ActionExceptionCatcher : Ferda.Modules.AMI_BoxModule_runAction
    {
        #region Fields

        ///<summary>
        /// the project manager of the Ferda
        ///</summary>
        protected ProjectManager.ProjectManager projectManager;
        ///<summary>
        /// resource manager for localization
        ///</summary>
        protected ResourceManager resourceManager;
        /// <summary>
        /// Control to select the box
        /// </summary>
        protected IBoxSelector selector;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor for the class
        /// </summary>
        /// <param name="projManager">Project manager needs to be there for 
        /// displaying which box has thrown an exception
        /// </param>
        /// <param name="resManager">Manager of the resources</param>
        public ActionExceptionCatcher(ProjectManager.ProjectManager projManager,
            ResourceManager resManager, IBoxSelector selector)
        {
            this.projectManager = projManager;
            this.resourceManager = resManager;
            this.selector = selector;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Method ice_exception
        /// </summary>
        /// <param name="ex">An Ice.Exception</param>
        public override void ice_exception(Exception ex)
        {
            if (ex is Ferda.Modules.BoxRuntimeError)
            {
                Ferda.Modules.BoxRuntimeError error = (Ferda.Modules.BoxRuntimeError)ex;

                //getting info about the box that has created the exception
                IBoxModule box =
                    projectManager.ModulesManager.GetIBoxModuleByIdentity(error.boxIdentity);
                
                //selecting the box that has thrown the exception
                //selector.SelectBox(box);

                BoxExceptionThreadClass c = new BoxExceptionThreadClass(projectManager.ModulesManager, resourceManager, box, error.userMessage);
                Thread th = new Thread(new ThreadStart(c.ThreadStart));
                th.Start();
            }
            //TODO: other exceptions from IBoxModule.RunAction()
        }

        /// <summary>
        /// Method ice_response
        /// </summary>
        public override void ice_response()
        {
            //TODO: refresh property grid
        }

        #endregion
    }
}
