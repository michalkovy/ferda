// IUserNoteDisplayer.cs - class implementing this interfrace should
// be able to edit a user note
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
using System.Collections.Generic;
using System.Text;

namespace Ferda.FrontEnd.UserNote
{
    /// <summary>
    /// Class implementing this interface should be able to edit a user note
    /// of a selected box. 
    /// </summary>
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
