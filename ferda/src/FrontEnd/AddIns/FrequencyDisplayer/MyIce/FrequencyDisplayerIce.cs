// FrequencyDisplayerIce.cs - class for ice communication for the
//  Column Frequency module for interaction
//
// Authors:   Alexander Kuzmin <alexander.kuzmin@gmail.com>
//           Martin Ralbovsky <martin.ralbovsky@gmail.com>
//
// Copyright (c) 2005 Alexander Kuzmin, Martin Ralbovsky
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
using Ferda.Guha.Data;
using Ferda.FrontEnd.AddIns.Common.MyIce;
using Ferda.Modules.Boxes.DataPreparation;
using System.Resources;
using System.Reflection;

namespace Ferda.FrontEnd.AddIns.FrequencyDisplayer.MyIce
{
    /// <summary>
    /// Class for communication with ice for the
    /// Column Frequency module for interaction.
    /// </summary>
    public class FrequencyDisplayerIce : ModuleForInteractionIce
    {
        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="ownerOfAddIn">Owner of addin</param>
        public FrequencyDisplayerIce(Ferda.FrontEnd.AddIns.IOwnerOfAddIn ownerOfAddIn):
            base(ownerOfAddIn, null)
        {
            resManager = new ResourceManager("Ferda.FrontEnd.AddIns.FrequencyDisplayer.Localization_en-US",
            Assembly.GetExecutingAssembly());
        }
        #endregion

        #region Other ice

        /// <summary>
        /// Gets a list of sockets needed to be connected in order for the module to
        /// work
        /// </summary>
        /// <param name="__current">ICE stuff</param>
        /// <returns>List of socket names for module to work propertly</returns>
        public override string[] getNeededConnectedSockets(Ice.Current __current)
        {
            return new string[] { };
        }

        /// <summary>
        /// Gets accepted box types
        /// </summary>
        /// <param name="__current">Ice context</param>
        /// <returns>Array of boxtypes</returns>
        public override Ferda.Modules.BoxType[] getAcceptedBoxTypes(Ice.Current __current)
        {
            Modules.NeededSocket sock1;
            Modules.NeededSocket sock2;

            Modules.BoxType column = new Modules.BoxType();
            sock1 = new Ferda.Modules.NeededSocket();
            sock1.socketName = "DataTable";
            sock2 = new Ferda.Modules.NeededSocket();
            sock2.socketName = "SelectExpression";
            column.neededSockets = new Modules.NeededSocket[] { sock1, sock2};
            column.functionIceId = "::Ferda::Modules::Boxes::DataPreparation::ColumnFunctions";

            Modules.BoxType attribute = new Modules.BoxType();
            attribute.neededSockets = new Modules.NeededSocket[0];
            attribute.functionIceId = "::Ferda::Modules::Boxes::DataPreparation::AttributeFunctions";

            return new Modules.BoxType[] { attribute, column };
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
            return resManager.GetString("ColumnFrequency");
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
            return resManager.GetString("ColumnFrequencyModule");
        }

        #endregion

        #region IceRun

        /// <summary>
        /// Run method
        /// </summary>
        /// <param name="boxModuleParam"></param>
        /// <param name="localePrefs">Locale prefs</param>
        /// <param name="manager">Manager proxy</param>
        /// <param name="__current">Ice context</param>
        public override void run(Ferda.Modules.BoxModulePrx boxModuleParam, 
            string[] localePrefs, ManagersEnginePrx manager, Ice.Current __current)
        {
            Localize(localePrefs);

            ColumnFunctionsPrx prx = 
                ColumnFunctionsPrxHelper.checkedCast(boxModuleParam.getFunctions());

            //TODO: V pripade, ze se to bude pouzivat aj pro atribut, tak se tady
            //musi osetrit neco ve styly "GetNeededConnectedSockets" programove.
            try
            {
                ValuesAndFrequencies valfreq = prx.getDistinctsAndFrequencies();
                long rowCount = prx.getColumnInfo().dataTable.recordsCount;
                string label = manager.getProjectInformation().getUserLabel(
                    Ice.Util.identityToString(boxModuleParam.ice_getIdentity()));
                Ferda.FrontEnd.AddIns.FrequencyDisplayer.FrequencyDisplayer control =
                    new FrequencyDisplayer(resManager, valfreq, ownerOfAddIn, rowCount);
                this.ownerOfAddIn.ShowDockableControl(control, label + " " +
                    resManager.GetString("ColumnFrequency"));
            }

            catch (Ferda.Modules.BoxRuntimeError e)
            {
                MessageBox.Show(e.Message,
                        resManager.GetString("Error"), MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
            }
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
            //poznamka - tohle cele bylo ve try bloku... uvidime jak to bude
            //fungovat bez neho
            string locale;
            locale = localePrefs[0];
            localizationString = locale;
            locale = "Ferda.FrontEnd.AddIns.FrequencyDisplayer.Localization_" + locale;
            resManager = new ResourceManager(locale, Assembly.GetExecutingAssembly());
        }

        #endregion
    }
}
