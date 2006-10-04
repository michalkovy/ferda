// ModuleForInteractionIce.cs - base class for all the modules for interaction
// to communicate with the Ice layer of Ferda system
//
// Author: Martin Ralbovsky <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2006 Martin Ralbovsky
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
using System.Resources;
using System.Reflection;
using Ferda.Modules;
using Ferda.ModulesManager;
using Ferda.FrontEnd.Properties;


namespace Ferda.FrontEnd.AddIns.Common.MyIce
{
    /// <summary>
    /// Base class for all the modules for interaction to communicate with the
    /// Ice layer of the system. It overrides the 
    /// Ferda.Modules.ModuleForInteractionDisp_ class and provides all the
    /// necessary functionality that all the modules for interaction share.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Functions in the slice design not currently used by the modules for
    /// interaction : getDynamicHelpItems, getHelpFile, getHelpFileInfoSeq
    /// </para>
    /// <para>
    /// Functions in use by te modules for interaction (should be overriden):
    /// getNeededConnectedSockets, getLabel, getHint, getAcceptedBoxTypes
    /// </para>
    /// </remarks>
    public abstract class ModuleForInteractionIce : ModuleForInteractionDisp_
    {
        #region Public fields

        /// <summary>
        /// The owner of addin - it is usually a form that can display the addin
        /// within its docking environment (the FerdaForm of the FrontEnd).
        /// </summary>
        public Ferda.FrontEnd.AddIns.IOwnerOfAddIn ownerOfAddIn;

        /// <summary>
        /// Reference to the IOtherObjectDisplayer - in FrontEnd a property grid
        /// capable of displaying properties of an object
        /// </summary>
        public Ferda.FrontEnd.Properties.IOtherObjectDisplayer displayer;

        /// <summary>
        /// L10n string, for now en-US or cs-CZ
        /// </summary>
        public string localizationString;

        /// <summary>
        /// The resource manager for the module for interaction
        /// </summary>
        public ResourceManager resManager;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor for the class
        /// </summary>
        /// <param name="ownerOfAddIn">Owner of this addIn</param>
        /// <param name="displayer">Displayer of some objects properties</param>
        public ModuleForInteractionIce(IOwnerOfAddIn ownerOfAddIn,
            IOtherObjectDisplayer displayer)
        {
            this.displayer = displayer;
            this.ownerOfAddIn = ownerOfAddIn;
            //setting the ResManager resource manager and localization string
            resManager = null;
            localizationString = "en-US";
        }

        #endregion

        #region The Run function

        /// <summary>
        /// Function that runs the module for interaction (should be called from the
        /// FrontEnd).
        /// </summary>
        /// <param name="boxModuleParam">Proxy of the box module, over which this 
        /// module for interaction runs.</param>
        /// <param name="localePrefs">Localization preferences</param>
        /// <param name="manager">Proxy of the modules manager</param>
        /// <param name="__current">Some Ice stuff</param>
        //public override void run(Ferda.Modules.BoxModulePrx boxModuleParam, 
        //    string[] localePrefs, ManagersEnginePrx manager, Ice.Current __current)
        //{
        //}

        #endregion

        #region Other Ice functions

        /// <summary>
        /// Dynamically gets help items for this module for interaction.
        /// </summary>
        /// <param name="localePrefs">Localization preferences</param>
        /// <param name="__current">some Ice stuff</param>
        /// <returns>Array of help items that can be displayed in the 
        /// context help component of the FrontEnd</returns>
        public override Ferda.Modules.DynamicHelpItem[] getDynamicHelpItems(
            string[] localePrefs, Ice.Current __current)
        {
            return null;
        }

        /// <summary>
        /// Gets the help file associated with this module for interaction by 
        /// name
        /// </summary>
        /// <param name="identifier">Help file identifier</param>
        /// <param name="__current">Some Ice stuff</param>
        /// <returns>The byte sequence of the help file</returns>
        public override byte[] getHelpFile(string identifier, Ice.Current __current)
        {
            return null;
        }

        /// <summary>
        /// Gets information about help files for this module for interaction
        /// </summary>
        /// <param name="localePrefs">localization preferences</param>
        /// <param name="__current">some ice stuff</param>
        /// <returns>Sequence of informations about help files</returns>
        public override Ferda.Modules.HelpFileInfo[] getHelpFileInfoSeq(
            string[] localePrefs, Ice.Current __current)
        {
            return null;
        }

        /// <summary>
        /// Gets the icon for the module for interaction
        /// </summary>
        /// <param name="__current">Some Ice stuff</param>
        /// <returns>Icon for the module as sequence of bytes</returns>
        public override byte[] getIcon(Ice.Current __current)
        {
            return null;
        }

        #endregion
    }
}
