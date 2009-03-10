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
using Ferda.Modules.Boxes.DataPreparation;
using Ferda.Guha.Data;
using Ferda.Guha.Attribute;
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
            //TOHLE SE URCITE BUDE MENIT
            about = this.getPropertyAbout(valueBefore);

            //getting the localization
            string locale;
            try
            {
                locale = localePrefs[0];
                locale = "Ferda.FrontEnd.AddIns.EditFuzzyCategories.Localization_" + locale;
                resManager = new ResourceManager(locale, Assembly.GetExecutingAssembly());
            }
            catch { }

            //determining if there is column connected to the boxModule
            BoxModulePrx[] columns = boxModuleParam.getConnections("Column");
            if (columns == null || columns.Length == 0)
            {
                return valueBefore;
            }

            //retrieving the information about the column
            BoxModulePrx columnBox = columns[0];

            ColumnFunctionsPrx prx =
                ColumnFunctionsPrxHelper.checkedCast(columnBox.getFunctions());
            //tries to retrieve the column info
            ColumnInfo info;
            try
            {
                info = prx.getColumnInfo();
            }
            catch 
            {
                return valueBefore;
            }
            if (info.cardinality != CardinalityEnum.Cardinal)
            {
                Ferda.Modules.BadParamsError e = new BadParamsError("Column", 
                    "Fuzzy categories can be created only from cardinal columns. Set the semantics of the column to cardinal",
                    restrictionTypeEnum.OtherReason);
                ownerOfAddIn.ShowBoxException(e);
                return valueBefore;
            }
            if (!GenericColumn.GetIsNumericDataType(info.dataType))
            {
                Ferda.Modules.BadParamsError e = new BadParamsError("Column",
                    "The data type of the column is not supported for creation of fuzzy categories. Currently all numeric data types are supported.",
                    restrictionTypeEnum.OtherReason);
                ownerOfAddIn.ShowBoxException(e);
                return valueBefore;
            }
            ColumnStatistics stat = prx.getColumnStatistics();
            double minimum = Convert.ToDouble(stat.valueMin);
            double maximum = Convert.ToDouble(stat.valueMax);

            //checking, if the domain is
            PropertyValue p = boxModuleParam.getProperty("Domain");
            StringTI prop = p as StringTI;
            if (prop.stringValue != "WholeDomain")
            {
                p = boxModuleParam.getProperty("From");
                prop = p as StringTI;
                minimum = Convert.ToDouble(prop.stringValue);
                p = boxModuleParam.getProperty("To");
                prop = p as StringTI;
                maximum = Convert.ToDouble(prop.stringValue);
            }

            StringT vbefore = (StringT)valueBefore;
            TrapezoidalFuzzySets fuzzySets = TrapezoidalFuzzySets.Deserialize(vbefore.stringValue);

            MainWindow wind = new MainWindow(resManager, minimum, maximum, ownerOfAddIn, fuzzySets);
            DialogResult result = ownerOfAddIn.ShowDialog(wind);

            if (result == DialogResult.OK)
            {
                vbefore.stringValue = TrapezoidalFuzzySets.Serialize(wind.GetSets());
                return vbefore;
            }

            return valueBefore;
        }

        #endregion
    }
}
