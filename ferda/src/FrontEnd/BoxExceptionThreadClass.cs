// BoxExceptionThreadClass.cs - Class for displaying the BoxExceptionDialog to the user in another thread
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
    /// Class for displaying the BoxExceptionDialog to the user in another thread
    /// </summary>
    internal class BoxExceptionThreadClass
    {
        /// <summary>
        /// Resource manager
        /// </summary>
        protected ResourceManager resourceManager;
        /// <summary>
        /// The name of box which failed.
        /// </summary>
        protected string faultBoxName;
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
        /// <param name="modulesManager">The Modules Manager.</param>
        /// <param name="resManager">Resource manager</param>
        /// <param name="faultBoxName">Name of the fault box.</param>
        /// <param name="userMessage">Message to be displayed to the user</param>
        public BoxExceptionThreadClass(ModulesManager.ModulesManager modulesManager,
            ResourceManager resManager, string faultBoxName, string userMessage)
        {
            this.resourceManager = resManager;
            this.faultBoxName = faultBoxName;
            this.userMessage = userMessage;
            this.modulesManager = modulesManager;
        }

        /// <summary>
        /// The procedure for the new thread
        /// </summary>
        public void ThreadStart()
        {
            modulesManager.UnlockAllBoxes();
            BoxExceptionDialog dialog;
            faultBoxName = FrontEndCommon.GetFaultBoxName(faultBoxName, resourceManager);
            dialog = new BoxExceptionDialog(resourceManager,
                faultBoxName, userMessage);
            dialog.ShowDialog();
        }
    }
}
