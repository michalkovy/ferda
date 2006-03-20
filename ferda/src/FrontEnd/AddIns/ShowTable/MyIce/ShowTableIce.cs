// ShowTableIce.cs - class for ice communication
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
using System.Resources;
using System.Reflection;
using System.Windows.Forms;

namespace Ferda.FrontEnd.AddIns.ShowTable.MyIce
{
    /// <summary>
    /// Clas for ice communication
    /// </summary>
    class ShowTableIce : Ferda.Modules.ModuleForInteractionDisp_
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

        #endregion


        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="ownerOfAddIn">Owner of addin</param>
        public ShowTableIce(Ferda.FrontEnd.AddIns.IOwnerOfAddIn ownerOfAddIn)
        {
            this.ownerOfAddIn = ownerOfAddIn;
            //setting the ResManager resource manager and localization string
            resManager = new ResourceManager("Ferda.FrontEnd.AddIns.ShowTable.Localization_en-US",
            Assembly.GetExecutingAssembly());
            localizationString = "en-US";
        }

        #endregion


        #region Other ice

        /// <summary>
        /// Gets accepted box types
        /// </summary>
        /// <param name="current__">Ice context</param>
        /// <returns>Array of accepted box types</returns>
        public override Ferda.Modules.BoxType[] getAcceptedBoxTypes(Ice.Current current__)
        {
            Modules.BoxType boxType = new Modules.BoxType();
            boxType.neededSockets = new Modules.NeededSocket[0];
            boxType.functionIceId = "::Ferda::Modules::Boxes::DataMiningCommon::DataMatrix::DataMatrixFunctions";
            return new Modules.BoxType[] { boxType };
        }

        public override Ferda.Modules.DynamicHelpItem[] getDynamicHelpItems(string[] localePrefs, Ice.Current current__)
        {
            return null;
        }

        public override byte[] getHelpFile(string identifier, Ice.Current current__)
        {
            return null;
        }

        public override Ferda.Modules.HelpFileInfo[] getHelpFileInfoSeq(string[] localePrefs, Ice.Current current__)
        {
            return null;
        }

        public override string getHint(string[] localePrefs, Ice.Current current__)
        {
            string locale;
            try
            {
                locale = localePrefs[0];
                localizationString = locale;
                locale = "Ferda.FrontEnd.AddIns.ShowTable.Localization_" + locale;
                resManager = new ResourceManager(locale, Assembly.GetExecutingAssembly());
            }
            catch
            {
            }
            return resManager.GetString("ShowTableModule");
        }

        public override byte[] getIcon(Ice.Current current__)
        {
            return null;
        }

        public override string getLabel(string[] localePrefs, Ice.Current current__)
        {
            string locale;
            try
            {
                locale = localePrefs[0];
                localizationString = locale;
                locale = "Ferda.FrontEnd.AddIns.ShowTable.Localization_" + locale;
                resManager = new ResourceManager(locale, Assembly.GetExecutingAssembly());
            }
            catch
            {
            }
            return resManager.GetString("ShowTableModule");
        }

        public override string[] getNeededConnectedSockets(Ice.Current current__)
        {
            return new string[0];
        }

        #endregion


        #region IceRun

        /// <summary>
        /// IceRUn
        /// </summary>
        /// <param name="boxModuleParam">boxmoduleparams</param>
        /// <param name="localePrefs">localeprefs</param>
        /// <param name="manager">Manager prx</param>
        /// <param name="current__">Ice context</param>
        public override void run(Ferda.Modules.BoxModulePrx boxModuleParam, string[] localePrefs, Ferda.ModulesManager.ManagersEnginePrx manager, Ice.Current current__)
        {
            string locale;
            try
            {
                locale = localePrefs[0];
                localizationString = locale;
                locale = "Ferda.FrontEnd.AddIns.ShowTable.Localization_" + locale;
                resManager = new ResourceManager(locale, Assembly.GetExecutingAssembly());
            }
            catch
            {
            }
            Ferda.Modules.Boxes.DataMiningCommon.DataMatrix.DataMatrixFunctionsPrx prx =
                    Ferda.Modules.Boxes.DataMiningCommon.DataMatrix.DataMatrixFunctionsPrxHelper.checkedCast(boxModuleParam.getFunctions());
            try
            {
                string label = manager.getProjectInformation().getUserLabel(Ice.Util.identityToString(boxModuleParam.ice_getIdentity()));
                Ferda.FrontEnd.AddIns.ShowTable.ShowTableControl control = new ShowTableControl(localePrefs, prx.getColumnsNames(), prx.getDataMatrixInfo());
                this.ownerOfAddIn.ShowDockableControl(control, resManager.GetString("Show") + " " + label);
            }
            catch (Ferda.Modules.NoConnectionInSocketError)
            {
                MessageBox.Show(resManager.GetString("BoxNotConnected"), resManager.GetString("Error"),
                           MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
            }
        }

        #endregion
    }
}
