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
using Ferda.Guha.Data;
using System.Data;

namespace Ferda.FrontEnd.AddIns.EditCategories
{
    /// <summary>
    /// Class for ice communication
    /// </summary>
    class EditCategoriesIce : Ferda.Modules.SettingModuleDisp_
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
        string categories;

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
            return "EditCategories";
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
        public override PropertyValue run(PropertyValue valueBefore, string propertyName, BoxModulePrx boxModuleParam, string[] localePrefs, ManagersEnginePrx manager, out string about, Ice.Current current__)
        {
            PropertyValue propertyValue = valueBefore;
            // Box
            StringTI pv = new StringTI();
            pv = (StringTI)propertyValue;
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
            DbDataTypeEnum columnDataType = new DbDataTypeEnum();
            PropertyValue returnValue = new PropertyValue();
            DataTable table = new DataTable();

            try
            {
                //if attribute is connected to static attribute
                try
                {
                    BoxModulePrx boxModuleParamNew = boxModuleParam.getConnections("BitStringGenerator")[0];
                    BoxModulePrx boxModuleParam1 = boxModuleParamNew.getConnections("Column")[0];
                    BoxModulePrx boxModuleParam2 = boxModuleParam1.getConnections("DataTable")[0];

                    Modules.Boxes.DataPreparation.ColumnFunctionsPrx prx2 =
                        Modules.Boxes.DataPreparation.ColumnFunctionsPrxHelper.checkedCast(
                        boxModuleParam1.getFunctions());
                    //      Modules.Boxes.DataPreparation.AttributeFunctionsPrx prx1 =
                    //        Modules.Boxes.DataPreparation.AttributeFunctionsPrxHelper.checkedCast(
                    //      boxModuleParamNew.getFunctions());

                    Modules.Boxes.DataPreparation.DataTableFunctionsPrx prx3 =
                        Modules.Boxes.DataPreparation.DataTableFunctionsPrxHelper.checkedCast(
                        boxModuleParam2.getFunctions());

                    Modules.Boxes.DataPreparation.ColumnInfo info = prx2.getColumnInfo();
                    columnDataType = info.dataType;
                    DatabaseConnectionSettingHelper connSetting = 
                        new DatabaseConnectionSettingHelper(
                        info.dataTable.databaseConnectionSetting);
                    GenericDataTable genericDataTable =
                        GenericDatabaseCache.GetGenericDatabase(
                        connSetting)[info.dataTable.dataTableName];
                    table = genericDataTable.Select();
                }

                catch
                {
                    //static attribute is connected to column
                    try
                    {
                        BoxModulePrx boxModuleParam1 = boxModuleParam.getConnections("Column")[0];
                        BoxModulePrx boxModuleParam2 = boxModuleParam1.getConnections("DataTable")[0];

                        Modules.Boxes.DataPreparation.ColumnFunctionsPrx prx2 =
                            Modules.Boxes.DataPreparation.ColumnFunctionsPrxHelper.checkedCast(
                            boxModuleParam1.getFunctions());

                        Modules.Boxes.DataPreparation.DataTableFunctionsPrx prx3 =
                            Modules.Boxes.DataPreparation.DataTableFunctionsPrxHelper.checkedCast(
                            boxModuleParam2.getFunctions());

                        Modules.Boxes.DataPreparation.ColumnInfo info = prx2.getColumnInfo();
                        columnDataType = info.dataType;
                        DatabaseConnectionSettingHelper connSetting =
                            new DatabaseConnectionSettingHelper(
                            info.dataTable.databaseConnectionSetting);
                        GenericDataTable genericDataTable =
                            GenericDatabaseCache.GetGenericDatabase(
                            connSetting)[info.dataTable.dataTableName];
                        table = genericDataTable.Select();
                    }
                    catch(Exception e)
                    {
                        //static attribute is not connected to anything
                        throw e;
                    }
                }

                Ferda.FrontEnd.AddIns.EditCategories.MainListView listView
                    = new Ferda.FrontEnd.AddIns.EditCategories.MainListView(
                    localePrefs, pv, table, columnDataType, ownerOfAddIn);
                listView.ShowInTaskbar = false;
                listView.Disposed += new EventHandler(SetCategories);
                System.Windows.Forms.DialogResult result = this.ownerOfAddIn.ShowDialog(listView);
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    PropertyValue resultValue = new StringTI(this.categories);
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
                if (ex.restrictionType == Ferda.Modules.restrictionTypeEnum.DbConnectionStringError)
                {
                    MessageBox.Show(resManager.GetString("BadConnectionString"), resManager.GetString("Error"),
                               MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (ex.restrictionType == Ferda.Modules.restrictionTypeEnum.DbDataTableNameError)
                {
                    MessageBox.Show(resManager.GetString("NoDataMatrix"), resManager.GetString("Error"),
                               MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (ex.restrictionType == Ferda.Modules.restrictionTypeEnum.DbColumnNameError)
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