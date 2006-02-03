using System;
using System.Windows.Forms;

namespace Ferda.FrontEnd.Menu
{
    /// <summary>
    /// Interface to communicate with the Ferda menu. A class wanting to
    /// display the menu should implement this interface. 
    /// </summary>
    public interface IMenuDisplayer
    {
        ///<summary>
        ///This function is called when the localization
        ///of the application is changed - the whole menu needs to be redrawn
        ///</summary>
        void ChangeLocalization();

        ///<summary>
        ///Forces the whole menu to redefine itself accroding to the new
        ///state in the ProjectManager (enabled/disabled items, actions of
        ///the selected box, etc.)
        ///</summary>
		void Adapt();

        /// <summary>
        /// Determines in the FerdaMenu, which control is now
        /// focused. The Other option stays for ModulesForInteraction and/or
        /// other controls.
        /// </summary>
        Control ControlHasFocus
        {
            set;
            get;
        }
    }
}
