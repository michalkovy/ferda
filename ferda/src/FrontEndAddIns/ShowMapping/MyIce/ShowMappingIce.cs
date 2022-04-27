// ShowMappingIce.cs - class for ice communication for the ShowMapping module
// for interaction
//
// Authors:  Martin Zeman <martinzeman@email.cz>
//
// Copyright (c) 2007 Martin Zeman
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
using System.Windows.Forms;
using Ferda.ModulesManager;
using Ferda.FrontEnd.AddIns;
using Ferda.FrontEnd.AddIns.Common.MyIce;
using Ferda.Modules.Boxes.OntologyRelated.OntologyMapping;
using System.Resources;
using System.Reflection;

namespace Ferda.FrontEnd.AddIns.ShowMapping.MyIce
{
    /// <summary>
    /// Class for Ice communication for the ShowMapping module for interaction
    /// </summary>
    public class ShowMappingIce : ModuleForInteractionIce
    {
        #region Constructor

        /// <summary>
        /// Default constructor for the class
        /// </summary>
        /// <param name="ownerOfAddIn">Owner of this addIn</param>
        public ShowMappingIce(Ferda.FrontEnd.AddIns.IOwnerOfAddIn ownerOfAddIn) :
            base(ownerOfAddIn, null)
        {
            //setting the resource manager
            resManager = new ResourceManager("Ferda.FrontEnd.AddIns.ShowMapping.Localization_en-US",
            Assembly.GetExecutingAssembly());
        }

        #endregion

        #region Other Ice functions

        /// <summary>
        /// Gets a list of sockets needed to be connected in order for the module to
        /// work
        /// </summary>
        /// <param name="__current">ICE stuff</param>
        /// <returns>List of socket names for module to work propertly</returns>
        public override string[] getNeededConnectedSockets(Ice.Current __current)
        {
            //there has to be a database connected to the data table and ontology
            return new string[] { "Database", "Ontology" };
        }

        /// <summary>
        /// Gets accepted box type by this module for interaction
        /// </summary>
        /// <param name="__current">Ice context</param>
        /// <returns>Accpeted box types array</returns>
        public override Ferda.Modules.BoxType[] getAcceptedBoxTypes(Ice.Current __current)
        {
            Modules.BoxType boxType = new Modules.BoxType();
            boxType.neededSockets = new Modules.NeededSocket[0];
            boxType.functionIceId = "::Ferda::Modules::Boxes::OntologyRelated::OntologyMapping::OntologyMappingFunctions";
            return new Modules.BoxType[] { boxType };
        }

        /// <summary>
        /// Gets hint to the module according for a specified localization
        /// </summary>
        /// <param name="localePrefs">Localization preferences</param>
        /// <param name="__current">Some ICE stuff</param>
        /// <returns>Localized hint</returns>
        public override string getHint(string[] localePrefs, Ice.Current __current)
        {
            Localize(localePrefs);
            return resManager.GetString("ShowMappingModule");
        }

        /// <summary>
        /// Gets label of the module according to the localization
        /// </summary>
        /// <param name="localePrefs">Localization specification</param>
        /// <param name="__current">Some ICE stuff</param>
        /// <returns>Localized label</returns>
        public override string getLabel(string[] localePrefs, Ice.Current __current)
        {
            Localize(localePrefs);
            return resManager.GetString("ShowMappingModule");
        }

        #endregion

        #region Run function

        /// <summary>
        /// The method is called by te Ice framework when a module for interaction
        /// is to be displayed
        /// </summary>
        /// <param name="boxModuleParam">Box that is executing this module for interaction</param>
        /// <param name="localePrefs">Localization preferences</param>
        /// <param name="manager">Proxy address of the modules manager</param>
        /// <param name="__current">Ice context</param>
        public override void run(Ferda.Modules.BoxModulePrx boxModuleParam,
            string[] localePrefs, ManagersEnginePrx manager, Ice.Current __current)
        {
            //checking the validity of the box
            try
            {
                boxModuleParam.validate();
            }
            catch (Ferda.Modules.BoxRuntimeError e)
            {
                ownerOfAddIn.ShowBoxException(e);
                return;
            }

            Localize(localePrefs);

            OntologyMappingFunctionsPrx prx =
                OntologyMappingFunctionsPrxHelper.checkedCast(boxModuleParam.getFunctions());

            //getting the label
            string label = manager.getProjectInformation().
                getUserLabel(Ice.Util.identityToString(boxModuleParam.ice_getIdentity()));
            ShowMappingControl control =
                new ShowMappingControl(resManager, ownerOfAddIn, prx);
            ownerOfAddIn.ShowDockableControl(control, 
                resManager.GetString("Show") + " " + label);
        }

        #endregion

        #region Other functions

        /// <summary>
        /// Adjusts the resource manager according to the recent localization
        /// </summary>
        /// <param name="localePrefs">string array determining the localization
        /// preferences</param>
        private void Localize(string[] localePrefs)
        {
            string locale;
            locale = localePrefs[0];
            localizationString = locale;
            locale = "Ferda.FrontEnd.AddIns.ShowMapping.Localization_" + locale;
            resManager = new ResourceManager(locale, Assembly.GetExecutingAssembly());
        }

        #endregion
    }
}