// IMenuDisplayer.cs - Interface to communicate with the Ferda menu 
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
