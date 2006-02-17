// IPropertiesDisplayer.cs - interface to display properties there
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
using Ferda.ModulesManager;


namespace Ferda.FrontEnd.Properties
{
	///<summary>
	///Each control that displays a Ferda property grid should implement this
	///interface
	///</summary>
	public interface IPropertiesDisplayer
    {
		///<summary>
		///The control should refresh its properties according to the
        ///objects in SelectecBox(ex) and IsOneBoxSelected property
		///</summary>
        void Adapt();

		///<summary>
		///This function is called when the localization
		///of the application is changed - the whole menu needs to be redrawn
		///</summary>
		void ChangeLocalization();

        /// <summary>
        /// Resets the propetry grid to be without properties
        /// </summary>
        void Reset();

		///<summary>
		///Determines which boxes are selected in the control
		///</summary>
		///<remarks>
		///Tady nekde by mel oznamit vsem ostatnim kontrolum, ze doslo ke
		///zmene...
		///</remarks>
        List<IBoxModule> SelectedBoxes
        {
            set;
            //get
        }

		///<summary>
		///Determines which box is selected in the control
		///</summary>
		///<remarks>
		///Tady nekde by mel oznamit vsem ostatnim kontrolum, ze doslo ke
		///zmene...
		///</remarks>
		IBoxModule SelectedBox
		{
			set;
			//get;
		}

		///<summary>
		///Determines, wheather only one box is selected in the control
		///(and thus all the context menu of box module actions ... can
		///be displayed)
		///</summary>
		bool IsOneBoxSelected
		{
			set;
			//get;
		}
	}
}
