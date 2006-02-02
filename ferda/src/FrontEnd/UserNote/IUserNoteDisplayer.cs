using System;
using System.Collections.Generic;
using System.Text;

namespace Ferda.FrontEnd.UserNote
{
    public interface IUserNoteDisplayer
    {
        ///<summary>
        ///The box currently selected in the archive
        ///</summary>
        Ferda.ModulesManager.IBoxModule SelectedBox
        {
            set;
            //get;
        }

        /// <summary>
        /// Adapts the control (loads the user note of the selected box)
        /// </summary>
        void Adapt();

        /// <summary>
        /// Resets the user note to be without any information
        /// </summary>
        void Reset();
    }
}
