// IArchiveDisplayer.cs - Interface of the Ferda Archive
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
using System.Windows.Forms;

namespace Ferda.FrontEnd.Archive
{
    ///<summary>
	///Each control that displays a Ferda archive should implement this
	///interface in order to cooperate with other controls.
	///</summary>
    public interface IArchiveDisplayer
	{
		///<summary>
		///Localizes view in parameter in the archive
		///</summary>
		///<remarks>
		///Jeste nevim, jestli tam ma ten parametr smysl (kdyby se pouzival
		///IDisplayViewer - tak tam byt nemusi)
		///</remarks>
		void LocalizeInArchive(IBoxModule box);

        ///<summary>
        ///The box currently selected in the archive
        ///</summary>
		IBoxModule SelectedBox
        {
            //set;
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
        ///Forces the control to refresh its state
        ///</summary>
        void Adapt();

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
        /// Because there are problems with sharing the clicking actions on the menu
        /// with other controls (ToolBox), this method raises the action that was
        /// clicked on the toolbar
        /// </summary>
        /// <param name="sender">sender of the method</param>
        void RaiseToolBarAction(object sender);
	}
}
