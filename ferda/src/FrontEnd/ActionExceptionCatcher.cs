// ActionExceptionCatcher.cs - class catches exceptions comming from other application layers
//
// Author: Martin Ralbovský <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2005 Martin Ralbovský
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
    internal class ActionExceptionCatcher : Ferda.Modules.AMI_BoxModule_runAction
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
        /// <summary>
        /// Property grid to be refreshed after the action is carried out
        /// </summary>
        protected Properties.IPropertiesDisplayer propertiesDisplayer;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor for the class
        /// </summary>
        /// <param name="projManager">Project manager needs to be there for 
        /// displaying which box has thrown an exception
        /// </param>
        /// <param name="resManager">Manager of the resources</param>
        /// <param name="selector">If a control has the ability to select
        /// a box that has thrown the exception, it can be also passed
        /// as a parameter</param>
        /// <param name="propGrid">
        /// Property grid of the application</param>
        public ActionExceptionCatcher(ProjectManager.ProjectManager projManager,
            ResourceManager resManager, IBoxSelector selector, 
            Properties.IPropertiesDisplayer propGrid)
        {
            this.projectManager = projManager;
            this.resourceManager = resManager;
            this.selector = selector;
            this.propertiesDisplayer = propGrid;
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
            if (ex is Ferda.Modules.BadParamsError)
            {
                Ferda.Modules.BadParamsError error =
                    (Ferda.Modules.BadParamsError)ex;

                //getting info about the box that has created the exception
                IBoxModule box =
                    projectManager.ModulesManager.GetIBoxModuleByIdentity(error.boxIdentity);

                BoxExceptionThreadClass c = new BoxExceptionThreadClass(projectManager.ModulesManager, resourceManager, box, error.userMessage);
                Thread th = new Thread(new ThreadStart(c.ThreadStart));
                th.Start();
            }
        }

        /// <summary>
        /// Method ice_response - refreshes the property grid after the action
        /// is done
        /// </summary>
        public override void ice_response()
        {
            propertiesDisplayer.AsyncAdapt();
        }

        #endregion
    }
}
