// DatabaseInfoIce.cs - class for ice communication
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
using System.Windows.Forms;
using Ferda.ModulesManager;
using Ferda.FrontEnd.AddIns;
using Ferda.Modules.Boxes.DataMiningCommon.DataMatrix;
using Ferda.FrontEnd.AddIns.DatabaseInfo;
using System.Resources;
using System.Reflection;

namespace Ferda.FrontEnd.AddIns.DatabaseInfo.MyIce
{
    /// <summary>
    /// Class for ice communication
    /// </summary>
    public class DatabaseInfoIce : Ferda.Modules.ModuleForInteractionDisp_
    {
        /// <summary>
        /// Owner of addin
        /// </summary>
        Ferda.FrontEnd.AddIns.IOwnerOfAddIn ownerOfAddIn;

        /// <summary>
        /// Resource manager
        /// </summary>
        private ResourceManager resManager;

        /// <summary>
        /// Localization string
        /// </summary>
        private string localizationString;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="ownerOfAddIn">Owner of addin</param>
        public DatabaseInfoIce(Ferda.FrontEnd.AddIns.IOwnerOfAddIn ownerOfAddIn)
        {
            this.ownerOfAddIn = ownerOfAddIn;

            //setting the ResManager resource manager and localization string
            resManager = new ResourceManager("Ferda.FrontEnd.AddIns.DataBaseInfo.Localization_en-US",
            Assembly.GetExecutingAssembly());
            localizationString = "en-US";
        }

        /// <summary>
        /// Gets accepted box types
        /// </summary>
        /// <param name="__current">Ice context</param>
        /// <returns>Array of boxtypes</returns>
        public override Ferda.Modules.BoxType[] getAcceptedBoxTypes(Ice.Current __current)
        {
            Modules.BoxType boxType = new Modules.BoxType();
            boxType.neededSockets = new Modules.NeededSocket[0];
            boxType.functionIceId = "::Ferda::Modules::Boxes::DataMiningCommon::Database::DatabaseFunctions";
            return new Modules.BoxType[] { boxType };
        }

        public override Ferda.Modules.DynamicHelpItem[] getDynamicHelpItems(string[] localePrefs, Ice.Current __current)
        {
            return null;
        }

        public override byte[] getHelpFile(string identifier, Ice.Current __current)
        {
            return null;
        }

        public override Ferda.Modules.HelpFileInfo[] getHelpFileInfoSeq(string[] localePrefs, Ice.Current __current)
        {
            return null;
        }

        public override string getHint(string[] localePrefs, Ice.Current __current)
        {
            string locale;
            try
            {
                locale = localePrefs[0];
                localizationString = locale;
                locale = "Ferda.FrontEnd.AddIns.DataBaseInfo.Localization_" + locale;
                resManager = new ResourceManager(locale, Assembly.GetExecutingAssembly());
            }

            catch
            {
            }
            return resManager.GetString("DataBaseInfoModule");
        }

        public override byte[] getIcon(Ice.Current __current)
        {
            return null;
        }

        public override string getLabel(string[] localePrefs, Ice.Current __current)
        {
            string locale;
            try
            {
                locale = localePrefs[0];
                localizationString = locale;
                locale = "Ferda.FrontEnd.AddIns.DataBaseInfo.Localization_" + locale;
                resManager = new ResourceManager(locale, Assembly.GetExecutingAssembly());
            }

            catch
            {
            }
            return resManager.GetString("DataBaseInfoModule");

        }

        public override string[] getNeededConnectedSockets(Ice.Current __current)
        {
            return new string[0];
        }

        /// <summary>
        /// Run method
        /// </summary>
        /// <param name="boxModuleParam"></param>
        /// <param name="localePrefs">Locale prefs</param>
        /// <param name="manager">Manager proxy</param>
        /// <param name="__current">Ice context</param>
        public override void run(Ferda.Modules.BoxModulePrx boxModuleParam, string[] localePrefs, ManagersEnginePrx manager, Ice.Current __current)
        {
            string locale;
            try
            {
                locale = localePrefs[0];
                localizationString = locale;
                locale = "Ferda.FrontEnd.AddIns.DataBaseInfo.Localization_" + locale;
                resManager = new ResourceManager(locale, Assembly.GetExecutingAssembly());
            }

            catch
            {
            }
            Ferda.Modules.Boxes.DataMiningCommon.Database.DatabaseFunctionsPrx prx =
                Ferda.Modules.Boxes.DataMiningCommon.Database.DatabaseFunctionsPrxHelper.checkedCast(boxModuleParam.getFunctions());

            try
            {

                string label = manager.getProjectInformation().getUserLabel(Ice.Util.identityToString(boxModuleParam.ice_getIdentity()));
                Ferda.FrontEnd.AddIns.DatabaseInfo.DataBaseInfo control = new DataBaseInfo(localePrefs, prx.explain());
                this.ownerOfAddIn.ShowDockableControl(control, label + " " + resManager.GetString("DataBaseInfo"));
            }

            catch (Ferda.Modules.BadParamsError ex)
            {
                if (ex.restrictionType == Ferda.Modules.restrictionTypeEnum.DbConnectionString)
                {
                    MessageBox.Show(resManager.GetString("BadConnectionString"), resManager.GetString("Error"),
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

            catch (Ferda.Modules.NoConnectionInSocketError)
            {
                MessageBox.Show(resManager.GetString("BoxNotConnected"), resManager.GetString("Error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
