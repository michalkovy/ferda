/* Generated by Together */

using System;
using Ferda.ModulesManager;
using System.Windows.Forms;

namespace Ferda.FrontEnd.Archive
{
    ///<summary>
	///Each control that displays a Ferda archive should implement this
	///interface
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
