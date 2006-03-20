// EditCategoriesIce.cs - class for ice communication
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
using System.Windows.Forms;

namespace Ferda.FrontEnd.AddIns.EditCategories
{
    /// <summary>
    /// Class for ice communication
    /// </summary>
    class EditCategoriesIce : SettingModuleDisp_
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
        /// Categories structure
        /// </summary>
        CategoriesStruct categories;

        #endregion


        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="ownerOfAddIn">Owner of addin</param>
        public EditCategoriesIce(Ferda.FrontEnd.AddIns.IOwnerOfAddIn ownerOfAddIn)
        {
            this.ownerOfAddIn = ownerOfAddIn;
            //setting the ResManager resource manager and localization string
            resManager = new ResourceManager("Ferda.FrontEnd.AddIns.EditCategories.Localization_en-US",
            Assembly.GetExecutingAssembly());
            localizationString = "en-US";
        }

        #endregion


        #region Other ice

        public override string getLabel(string[] localePrefs, Ice.Current __current)
        {

            string locale;

            try
            {
                locale = localePrefs[0];
                localizationString = locale;
                locale = "Ferda.FrontEnd.AddIns.EditCategories.Localization_" + locale;
                resManager = new ResourceManager(locale, Assembly.GetExecutingAssembly());
            }
            catch
            {
            }
            return resManager.GetString("EditCategories");
        }

        public override string getPropertyAbout(PropertyValue value, Ice.Current __current)
        {
            return resManager.GetString("EditCategoriesAbout");
        }

        public override string getIdentifier(Ice.Current current__)
        {
            return "CategoriesT";
        }

        #endregion


        #region Run

        /// <summary>
        /// Ice run method
        /// </summary>
        /// <param name="valueBefore">Previous parameter value</param>
        /// <param name="boxModuleParam"></param>
        /// <param name="localePrefs"></param>
        /// <param name="manager"></param>
        /// <param name="about"></param>
        /// <param name="__current"></param>
        /// <returns>New property value</returns>
        public override PropertyValue run(PropertyValue valueBefore, BoxModulePrx boxModuleParam, string[] localePrefs, ManagersEnginePrx manager, out string about, Ice.Current __current)
        {
            PropertyValue propertyValue = valueBefore;
            string locale;
            try
            {
                locale = localePrefs[0];
                localizationString = locale;
                locale = "Ferda.FrontEnd.AddIns.EditCategories.Localization_" + locale;
                resManager = new ResourceManager(locale, Assembly.GetExecutingAssembly());
            }
            catch
            {
            }
            about = resManager.GetString("EditCategoriesAbout");
            try
            {
                Ferda.Modules.Boxes.DataMiningCommon.Attributes.AbstractAttributeFunctionsPrx prx =
               Ferda.Modules.Boxes.DataMiningCommon.Attributes.AbstractAttributeFunctionsPrxHelper.checkedCast(boxModuleParam.getFunctions());
                String[] distinctValues;
                PropertyValue returnValue = new PropertyValue();
                CategoriesT categories = (CategoriesT)valueBefore;
                try
                {
                    BoxModulePrx boxModuleParamNew = boxModuleParam.getConnections("ColumnOrDerivedColumn")[0];
                    Ferda.Modules.Boxes.DataMiningCommon.Column.ColumnFunctionsPrx prx1 =
                        Ferda.Modules.Boxes.DataMiningCommon.Column.ColumnFunctionsPrxHelper.checkedCast(boxModuleParamNew.getFunctions());
                    distinctValues = prx1.getDistinctValues();
                }
                catch
                {
                    distinctValues = null;
                }
                Ferda.FrontEnd.AddIns.EditCategories.MainListView listView = new Ferda.FrontEnd.AddIns.EditCategories.MainListView(localePrefs, categories.categoriesValue, distinctValues);
                listView.ShowInTaskbar = false;
                listView.Disposed += new EventHandler(SetCategories);
                System.Windows.Forms.DialogResult result = this.ownerOfAddIn.ShowDialog(listView);
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    PropertyValue resultValue = new CategoriesTI(this.categories);
                    about = this.getPropertyAbout(resultValue);
                    propertyValue = resultValue;
                }
                else
                {
                    about = this.getPropertyAbout(valueBefore);
                }
            }
            catch (Ferda.Modules.BadParamsError ex)
            {
                if (ex.restrictionType == Ferda.Modules.restrictionTypeEnum.DbConnectionString)
                {
                    MessageBox.Show(resManager.GetString("BadConnectionString"), resManager.GetString("Error"),
                               MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (ex.restrictionType == Ferda.Modules.restrictionTypeEnum.DbTable)
                {
                    MessageBox.Show(resManager.GetString("NoDataMatrix"), resManager.GetString("Error"),
                               MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (ex.restrictionType == Ferda.Modules.restrictionTypeEnum.DbColumn)
                {
                    MessageBox.Show(resManager.GetString("BadColumnSelectExpression"), resManager.GetString("Error"),
                               MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    MessageBox.Show(resManager.GetString("InvalidParameters"), resManager.GetString("Error"),
                               MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Ferda.Modules.NoConnectionInSocketError)
            {
                MessageBox.Show(resManager.GetString("BoxNotConnected"), resManager.GetString("Error"),
                           MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return propertyValue;
        }

        #endregion


        #region Private methods

        /// <summary>
        /// Handler of EditCategoriesList disposing; retrieves edited categories.
        /// </summary>
        /// <param name="sender">EditCategoriesListView</param>
        /// <param name="e"></param>
        private void SetCategories(object sender, EventArgs e)
        {
            Ferda.FrontEnd.AddIns.EditCategories.MainListView listView = (Ferda.FrontEnd.AddIns.EditCategories.MainListView)sender;
            this.categories = listView.GetUpdatedCategories();
        }

        #endregion
    }
}