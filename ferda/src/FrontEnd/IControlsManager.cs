using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.FrontEnd
{
    /// <summary>
    /// Interface to do global adapts, setting the caption of the application
    /// or writing messages on the status bar
    /// </summary>
    public interface IControlsManager
    {
        /// <summary>
        /// If there is an opened project, this property shows the name of the opened
        /// project, otherwise it is a string.Empty
        /// </summary>
        string ProjectName
        {
            set;
            get;
        }

        /// <summary>
        /// Forces all the relevant controls to adapt
        /// </summary>
        void GlobalAdapt();

        /// <summary>
        /// Adds a new desktop to the FrontEnd (new view to the project)
        /// </summary>
        void NewDesktop();

        /// <summary>
        /// Method that fills the project manager with a new view. It is used
        /// when a new project is created
        /// </summary>
        void FillPM();

        /// <summary>
        /// It is used when a new project is created. Any modules for interaction that
        /// are docked in the center of the screen should be removed.
        /// </summary>
        void ClearDocking();

        /// <summary>
        /// Function that returns the names of the views that are opened in the
        /// FrontEnd
        /// </summary>
        /// <returns>Names of opened views in the FrontEnd</returns>
        List<string> OpenedViews();

        /// <summary>
        /// Opens a view that is present in the project but was closed by the user
        /// </summary>
        /// <param name="name">name of the view</param>
        void OpenView(string name);

        /// <summary>
        /// Closes a view that is opened in the FerdaForm
        /// </summary>
        /// <param name="name">name of the view</param>
        void CloseView(string name);
    }
}
