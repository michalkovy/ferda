// IContextHelpDisplayer.cs - Interface to implement the Ferda context help
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
using Ferda.ModulesManager;

namespace Ferda 
{
    namespace FrontEnd 
    {
        namespace ContextHelp 
        {

            ///<summary>
			///Each control that displays a Ferda context help should implement this
			///interface
			///</summary>
            public interface IContextHelpDisplayer
			{
                ///<summary>
                ///The box currently selected in the archive
                ///</summary>
				IBoxModule SelectedBox
                {
                    set;
                    //get;
                }

                ///<summary>
                ///Forces the control to refresh its state
                ///</summary>
                void Adapt();

				///<summary>
				///This function is called when the localization
				///of the application is changed - the whole menu needs to be redrawn
				///</summary>
				void ChangeLocalization();

                /// <summary>
                /// Resets the context help to be without any information
                /// </summary>
                void Reset();
			}
        }
    }
}
