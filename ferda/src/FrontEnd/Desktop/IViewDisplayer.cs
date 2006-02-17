// IViewDisplayer.cs - interface needed for Ferda desktop control
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
using System.Windows.Forms;

namespace Ferda.FrontEnd.Desktop
{
    ///<summary>
    ///Each control that displays a Ferda view should implement this
    ///interface
    ///</summary>
    public interface IViewDisplayer
	{
        /// <summary>
        /// Name of the ViewDisplayer
        /// </summary>
        string Name
        {
            set;
            get;
        }

        /// <summary>
        /// View of the desktop (to run modules asking for creation)
        /// </summary>
        ProjectManager.View View
        {
            get;
        }

        ///<summary>
        ///Determines which box is selected in the control
        ///</summary>
        List <ModulesManager.IBoxModule> SelectedBoxes
        {
            get;
        }

        ///<summary>
        ///Determines, wheather only one box is selected in the control
        ///(and thus all the context menu of box module actions ... can
        ///be displayed)
        ///</summary>
		bool IsBoxSelected
        {
            get;
        }

        /// <summary>
        /// Context menu for the edit part of the main menu
        /// </summary>
        ContextMenuStrip EditMenu
        {
            get;
        }

        ///<summary>
        ///The method retrieves locations of all the boxes from the view
        ///and updates their locations in the IView
        ///</summary>
        void SetLocations();

        ///<summary>
        ///Redraws all shapes and connections according to a new state in
        ///the view object
        ///</summary>
        ///<remarks>
        ///Dont know if the control will remember previous shapes and connections
        ///and ask for the changed or write the entire structure over again...
        ///</remarks>
        void Adapt();

        /// <summary>
        /// Removes a box from the desktop
        /// </summary>
        /// <param name="box">IBoxmodule to be removed</param>
        void RemoveBox(IBoxModule box);

        /// <summary>
        /// Adds a box to the desktop
        /// </summary>
        /// <param name="box">Box to be added</param>
        void AddBox(IBoxModule box);

		///<summary>
		///This function is called when the localization
		///of the application is changed - the whole menu needs to be redrawn
		///</summary>
		void ChangeLocalization();

        /// <summary>
        /// This function is called by the property grid when a property is changed -
        /// that can only mean the change of the user name of the box.
        /// </summary>
        void RefreshBoxNames();

        /// <summary>
        /// When a box is selected in the archive, it should also be selected on the 
        /// view. This function selects the box in the desktop
        /// </summary>
        /// <param name="box">Box to be selected</param>
        void SelectBox(IBoxModule box);

        /// <summary>
        /// Because there are problems with sharing the clicking actions on the menu
        /// with other controls (ToolBox), this method raises the action that was
        /// clicked on the toolbar
        /// </summary>
        /// <param name="sender">sender of the method</param>
        void RaiseToolBarAction(object sender);
	}
}
