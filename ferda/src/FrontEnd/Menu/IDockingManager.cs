// IDockingManager.cs - Implementation of the docking functions of the
// application
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

namespace Ferda 
{
    namespace FrontEnd 
    {
        namespace Menu 
        {

            ///<summary>
            ///Implements the docking functions of the application, shows
            ///the controls
            ///</summary>
            public interface IDockingManager
		    {
                ///<summary>
                ///Shows the application's archive
                ///</summary>
                void ShowArchive();

                ///<summary>
                ///Shows the application's dynamic help
                ///</summary>
                void ShowContextHelp();

                ///<summary>
                ///Shows the application's property grid
                ///</summary>
                void ShowPropertyGrid();

                /// <summary>
                /// Shows the newBox treeview
                /// </summary>
                void ShowNewBox();

                /// <summary>
                /// Shows the user note control
                /// </summary>
                void ShowUserNote();

                /* I decided that this is not necessary for the application
                ///<summary>
                ///Shows the application's tooooooooolbar
                ///</summary>
			    void ShowToolBar();
                */
		    }
        }
    }
}
