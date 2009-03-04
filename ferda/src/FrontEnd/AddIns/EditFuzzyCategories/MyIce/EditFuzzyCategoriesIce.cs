// EditFuzzyCategoriesIce.cs - the EditFuzzyCategories ice communications class
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
using System.Reflection;
using System.Resources;
using System.Windows.Forms;
using Ferda.Modules;
using Ferda.ModulesManager;
using Ferda.FrontEnd.AddIns;
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
        IOwnerOfAddIn ownerOfAddIn;

        /// <summary>
        /// L10n resource manager
        /// </summary>
        private ResourceManager resManager;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="ownerOfAddIn">Owner of addin</param>
        public EditFuzzyCategoriesIce(IOwnerOfAddIn ownerOfAddIn)
        {
            this.ownerOfAddIn = ownerOfAddIn;
            resManager = new ResourceManager("Ferda.FrontEnd.AddIns.EditFuzzyCategories.Localization_en-US",
            Assembly.GetExecutingAssembly());
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
            string locale;
            try
            {
                locale = localePrefs[0];
                locale = "Ferda.FrontEnd.AddIns.EditFuzzyCategories.Localization_" + locale;
                resManager = new ResourceManager(locale, Assembly.GetExecutingAssembly());
            }
            catch { }
            return resManager.GetString("EditFuzzyCategories");
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
            return resManager.GetString("EditFuzzyCategoriesAbout");
        }

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
            string locale;
            try
            {
                locale = localePrefs[0];
                locale = "Ferda.FrontEnd.AddIns.EditFuzzyCategories.Localization_" + locale;
                resManager = new ResourceManager(locale, Assembly.GetExecutingAssembly());
            }
            catch { }


            MainWindow wind = new MainWindow(resManager);
            DialogResult result = ownerOfAddIn.ShowDialog(wind);

            about = "neco";
            return valueBefore;
        }

        #endregion
    }
}
