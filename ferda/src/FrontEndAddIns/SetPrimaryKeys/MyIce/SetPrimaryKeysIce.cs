// SelectTablesIce.cs - class for ice communication
//
// Author: Alexander Kuzmin <alexander.kuzmin@gmail.com>
//
// Copyright (c) 2006 Alexander Kuzmin
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
using Ferda.Modules;
using Ferda.Modules.Boxes;
using Ferda.ModulesManager;
using Ferda.FrontEnd.AddIns;
using System.Resources;
using System.Reflection;
using PropertyInfo=Ferda.Modules.PropertyInfo;

namespace Ferda.FrontEnd.AddIns.SetPrimaryKeys.MyIce
{
    /// <summary>
    /// Class for ice communication
    /// </summary>
    class SetPrimaryKeysIce : SettingModuleDisp_
    {
        #region Private variables

        /// <summary>
        /// Owner of addin
        /// </summary>
        Ferda.FrontEnd.AddIns.IOwnerOfAddIn ownerOfAddIn;

        /// <summary>
        /// L10n resource manager
        /// </summary>
        private ResourceManager resManager;

        /// <summary>
        /// L10n string, for now en-US or cs-CZ
        /// </summary>
        private string localizationString;

        /// <summary>
        /// Resulting string with selected tables
        /// </summary>
        string [] returnStrings;

        #endregion


        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="ownerOfAddIn">Owner of addin</param>
        public SetPrimaryKeysIce(Ferda.FrontEnd.AddIns.IOwnerOfAddIn ownerOfAddIn)
        {
            this.ownerOfAddIn = ownerOfAddIn;

            //setting the ResManager resource manager and localization string
            resManager = new ResourceManager("Ferda.FrontEnd.AddIns.SetPrimaryKeys.Localization_en-US",
            Assembly.GetExecutingAssembly());
            localizationString = "en-US";
        }

        #endregion


        #region Other ice


        public override string getIdentifier(Ice.Current current__)
        {
            return "SetPrimaryKeys";
        }

        public override string getLabel(string[] localePrefs, Ice.Current current__)
        {
            string locale;
            try
            {
                locale = localePrefs[0];
                localizationString = locale;
                locale = "Ferda.FrontEnd.AddIns.SetPrimaryKeys.Localization_" + locale;
                resManager = new ResourceManager(locale, Assembly.GetExecutingAssembly());
            }
            catch { }
            return resManager.GetString("SetPrimaryKeys");
        }

        public override string getPropertyAbout(PropertyValue value, Ice.Current current__)
        {
            return "";
        }

        #endregion


        #region Icerun

        /// <summary>
        /// Ice run
        /// </summary>
        /// <param name="valueBefore">Previous value</param>
        /// <param name="boxModuleParam">boxmoduleparam</param>
        /// <param name="localePrefs">localeprefs</param>
        /// <param name="manager">Manager proxy</param>
        /// <param name="about"></param>
        /// <param name="current__">Ice context</param>
        /// <returns>Modified property value</returns>
        public override PropertyValue run(PropertyValue valueBefore, string propertyName, BoxModulePrx boxModuleParam, string[] localePrefs, ManagersEnginePrx manager, out string about, Ice.Current current__)
        {
            string locale;
            try
            {
                locale = localePrefs[0];
                localizationString = locale;
                locale = "Ferda.FrontEnd.AddIns.SetPrimaryKeys.Localization_" + locale;
                resManager = new ResourceManager(locale, Assembly.GetExecutingAssembly());
            }
            catch {}

            StringSeqT selectedStrings = (StringSeqT)valueBefore;
            PropertyValue propertyValue = valueBefore;
            PropertyValue returnValue = new PropertyValue();

            // getting the structure with the data strings
            SelectString[] allStrings = boxModuleParam.getPropertyOptions(propertyName);

            SetPrimaryKeys.SetPrimaryKeysControl listView =
                new SetPrimaryKeys.SetPrimaryKeysControl(localePrefs,
                ownerOfAddIn,
                allStrings,
                selectedStrings.stringSeqValue
                );

            listView.ShowInTaskbar = false;
            listView.Disposed += new EventHandler(listView_Disposed);

            System.Windows.Forms.DialogResult result = this.ownerOfAddIn.ShowDialog(listView);
            
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                selectedStrings.stringSeqValue = this.returnStrings;
                PropertyValue resultValue = selectedStrings;
                about = this.getPropertyAbout(resultValue, current__);
                propertyValue = resultValue;
                return resultValue;
            }
            else
            {
                about = this.getPropertyAbout(valueBefore, current__);
                return valueBefore;
            }

            return propertyValue;
        }

        #endregion


        #region Other private methods

        /// <summary>
        /// Handler of disposing event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void listView_Disposed(object sender, EventArgs e)
        {
            Ferda.FrontEnd.AddIns.SetPrimaryKeys.SetPrimaryKeysControl listView =
                (Ferda.FrontEnd.AddIns.SetPrimaryKeys.SetPrimaryKeysControl)sender;

            this.returnStrings = listView.ReturnStrings;
        }

        #endregion
    }
}
