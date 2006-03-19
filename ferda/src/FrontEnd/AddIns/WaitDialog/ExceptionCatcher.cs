// ExceptionCatcher.cs - class catches exceptions comming from other application layers
//
// Author: Alexander Kuzmin <alexander.kuzmin@gmail.com>
//
// Copyright (c) 2005 Alexander Kuzmin
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

namespace Ferda.FrontEnd.AddIns.WaitDialog
{
    /// <summary>
    /// Class that catches the exceptions comming from other layers of the system
    /// and displayes them to the user
    /// </summary>
    internal class ExceptionCatcher : Ferda.Modules.Boxes.LISpMinerTasks.AbstractLMTask.AMI_AbstractLMTaskFunctions_runAction
    {
        #region Fields

        ///<summary>
        /// the project manager of the Ferda
        ///</summary>
        protected ProjectManager.ProjectManager projectManager;

        /// <summary>
        /// Owner of addin
        /// </summary>
        Ferda.FrontEnd.AddIns.IOwnerOfAddIn ownerOfAddIn;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor for the class
        /// </summary>
        /// <param name="ownerOfAddIn">OwnerofAddIn</param>
        /// <param name="form">Parent form</param>
        public ExceptionCatcher(Ferda.FrontEnd.AddIns.IOwnerOfAddIn ownerOfAddIn)
        {
            this.projectManager = ownerOfAddIn.ProjectManager;
            this.ownerOfAddIn = ownerOfAddIn;
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

                BoxExceptionClass c = new BoxExceptionClass(box, error.userMessage, ownerOfAddIn);
                Thread th = new Thread(new ThreadStart(c.ThreadStart));
                th.Start();
            }
            //TODO: other exceptions from IBoxModule.RunAction()
        }

        /// <summary>
        /// Method ice_response - refreshes the property grid after the action
        /// is done
        /// </summary>
        public override void ice_response()
        {
            ownerOfAddIn.AsyncAdapt();
            OnThreadCompleted();
        }

        public event ThreadCompleted Completed;
        public void OnThreadCompleted()
        {
            if (Completed != null)
            {
                Completed();
            }
        }


        #endregion
    }
}
