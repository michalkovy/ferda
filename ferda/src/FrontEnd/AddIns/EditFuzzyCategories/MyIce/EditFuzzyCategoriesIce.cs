﻿// EditFuzzyCategoriesIce.cs - the EditFuzzyCategories ice communications class
//
// Author: Martin Ralbovsky <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2009 Martin Ralbovsky
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
using System.Linq;
using System.Text;
using Ferda.Modules;
using Ferda.ModulesManager;
using Ice;

namespace Ferda.FrontEnd.AddIns.EditFuzzyCategories.MyIce
{
    /// <summary>
    /// Class for ice communication
    /// </summary>
    class EditFuzzyCategoriesIce : SettingModuleDisp_
    {
        #region Private variables

        /// <summary>
        /// Owner of addin
        /// </summary>
        Ferda.FrontEnd.AddIns.IOwnerOfAddIn ownerOfAddIn;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="ownerOfAddIn">Owner of addin</param>
        public EditFuzzyCategoriesIce(Ferda.FrontEnd.AddIns.IOwnerOfAddIn ownerOfAddIn)
        {
            this.ownerOfAddIn = ownerOfAddIn;
            //resManager = new ResourceManager("Ferda.FrontEnd.AddIns.FrequencyDisplayer.Localization_en-US",
            //Assembly.GetExecutingAssembly());
        }

        #endregion

        #region Other Ice

        /// <summary>
        /// Gets the localized label of the setting module
        /// </summary>
        /// <param name="localePrefs">Localization preferences</param>
        /// <param name="current__">Ice stuff</param>
        /// <returns>Localized label of the setting module</returns>
        public override string getLabel(string[] localePrefs, Current current__)
        {
            return "EditFuzzyCategories ble ble ble";
        }

        /// <summary>
        /// Gets the identifier of the setting module (used in the boxes.xml file)
        /// </summary>
        /// <param name="current__">Ice stuff</param>
        /// <returns>The identifier of the setting module</returns>
        public override string getIdentifier(Current current__)
        {
            return "EditFuzzyCategories";
        }

        /// <summary>
        /// ???
        /// </summary>
        /// <param name="value">The value to be converted</param>
        /// <param name="current__">Ice stuff</param>
        /// <returns>String representation of the property value</returns>
        public override string getPropertyAbout(PropertyValue value, Current current__)
        {
            return "EditFuzzyCategories ble ble ble";
        }

        /// <summary>
        /// Converts to the property value from the string representation and local 
        /// preferences
        /// </summary>
        /// <param name="about">The string representation of the property</param>
        /// <param name="localePrefs">Localization preferences</param>
        /// <param name="current__">Ice stuff</param>
        /// <returns>The property value representation</returns>
        //public override PropertyValue convertFromStringAbout(string about, string[] localePrefs, Ice.Current current__)
        //{
        //    return new StringTI(about);
        //}

        #endregion

        #region Ice Run

        /// <summary>
        /// Function that runs the module for interaction (should be called from the
        /// FrontEnd).
        /// </summary>
        /// <param name="valueBefore">Previous value</param>
        /// <param name="boxModuleParam">boxmoduleparam</param>
        /// <param name="localePrefs">localeprefs</param>
        /// <param name="manager">Manager proxy</param>
        /// <param name="about"></param>
        /// <param name="current__">Ice context</param>
        /// <returns>Modified property value</returns>
        public override PropertyValue run(PropertyValue valueBefore, 
            string propertyName, 
            BoxModulePrx boxModuleParam, 
            string[] localePrefs, 
            ManagersEnginePrx manager, 
            out string about, 
            Current current__)
        {
            about = "neco";
            return null;
        }

        #endregion
    }
}
