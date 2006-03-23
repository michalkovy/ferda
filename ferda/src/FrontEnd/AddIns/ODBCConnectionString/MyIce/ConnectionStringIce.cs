// ConnectionStringIce.cs - class for ice communication
//
// Author: Alexander Kuzmin <alexander.kuzmin@gmail.com>
//
// Copyright (c) 2005 Alexander Kuzmin
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
using System.Resources;
using System.Reflection;

namespace Ferda.FrontEnd.AddIns.ODBCConnectionString.MyIce
{
    /// <summary>
    /// Class for ice communication
    /// </summary>
    class ConnectionStringIce : SettingModuleWithStringAbilityDisp_
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
        /// Resulting DSN string
        /// </summary>
        string returnString;

        #endregion


        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="ownerOfAddIn">Owner of addin</param>
        public ConnectionStringIce(Ferda.FrontEnd.AddIns.IOwnerOfAddIn ownerOfAddIn)
        {
            this.ownerOfAddIn = ownerOfAddIn;

            //setting the ResManager resource manager and localization string
            resManager = new ResourceManager("Ferda.FrontEnd.AddIns.ODBCConnectionString.Localization_en-US",
            Assembly.GetExecutingAssembly());
            localizationString = "en-US";
        }

        #endregion


        #region Other ice

        public override string getLabel(string[] localePrefs, global::Ice.Current current__)
        {
            string locale;
            try
            {
                locale = localePrefs[0];

                localizationString = locale;

                locale = "Ferda.FrontEnd.AddIns.ODBCConnectionString.Localization_" + locale;

                resManager = new ResourceManager(locale,
            Assembly.GetExecutingAssembly());

            }

            catch
            {
            }
            return resManager.GetString("ConnectionString");
        }

        public override string getPropertyAbout(PropertyValue value, global::Ice.Current current__)
        {
            return ((StringT)value).getStringValue();
        }

        public override string getIdentifier(global::Ice.Current current__)
        {
            return "ODBCConnectionString";
        }

        public override PropertyValue convertFromStringAbout(string about, string[] localePrefs, Ice.Current current__)
        {
            return new StringTI(about);
        }

        #endregion


        #region IceRun

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
        public override PropertyValue run(PropertyValue valueBefore, BoxModulePrx boxModuleParam, string[] localePrefs, ManagersEnginePrx manager, out string about, global::Ice.Current current__)
        {
            string locale;
            try
            {
                locale = localePrefs[0];
                localizationString = locale;
                locale = "Ferda.FrontEnd.AddIns.ODBCConnectionString.Localization_" + locale;
                resManager = new ResourceManager(locale, Assembly.GetExecutingAssembly());
            }
            catch
            {
            }

            about = resManager.GetString("ConnectionStringAbout");
            StringT connectionString = (StringT)valueBefore;
            PropertyValue returnValue = new PropertyValue();
            PropertyValue propertyValue = valueBefore;
            Ferda.FrontEnd.AddIns.ODBCConnectionString.ODBCConnectionStringControl listView = new Ferda.FrontEnd.AddIns.ODBCConnectionString.ODBCConnectionStringControl(localePrefs, connectionString.getStringValue(),ownerOfAddIn);
            listView.ShowInTaskbar = false;
            listView.Disposed += new EventHandler(listView_Disposed);
            System.Windows.Forms.DialogResult result = this.ownerOfAddIn.ShowDialog(listView);
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                connectionString.stringValue = this.returnString;
                PropertyValue resultValue = connectionString;
                about = this.getPropertyAbout(resultValue);
                propertyValue = resultValue;
            }
            else
            {
                about = this.getPropertyAbout(valueBefore);
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
            Ferda.FrontEnd.AddIns.ODBCConnectionString.ODBCConnectionStringControl listView =
                (Ferda.FrontEnd.AddIns.ODBCConnectionString.ODBCConnectionStringControl)sender;

            this.returnString = listView.ReturnString;
        }

        #endregion
    }
}
