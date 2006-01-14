using Ice;
using Ferda.ModulesManager;
using Ferda.ProjectManager;
using System.Resources;

namespace Ferda.FrontEnd
{
    /// <summary>
    /// Class that catches the exceptions comming from other layers of the system
    /// and displayes them to the user
    /// </summary>
	public class ActionExceptionCatcher : Ferda.Modules.AMI_BoxModule_runAction
    {
        #region Fields

        //the project manager of the Ferda
        protected ProjectManager.ProjectManager projectManager;
        //resource manager for localization
        protected ResourceManager resourceManager;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor for the class
        /// </summary>
        /// <param name="projManager">Project manager needs to be there for 
        /// displaying which box has thrown an exception
        /// </param>
        public ActionExceptionCatcher(ProjectManager.ProjectManager projManager, 
            ResourceManager resManager)
        {
            this.projectManager = projManager;
            this.resourceManager = resManager;
        }

        #endregion

        #region Methods

        /// <summary>
		/// Method ice_exception
		/// </summary>
		/// <param name="ex">An Ice.Exception</param>
		public override void ice_exception(Exception ex)
		{
			if(ex is Ferda.Modules.BoxRuntimeError)
			{
				Ferda.Modules.BoxRuntimeError error = (Ferda.Modules.BoxRuntimeError)ex;
				
                //getting info about the box that has created the exception
                IBoxModule box = 
                    projectManager.ModulesManager.GetIBoxModuleByIdentity(error.boxIdentity);

                //showing the dialog
                BoxExceptionDialog dialog = new BoxExceptionDialog(resourceManager,
                    box.UserName, error.userMessage);
                dialog.ShowDialog();
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
