// ShowTableIce.cs - class for ice communication for the ShowTable module for 
// interaction
//
// Authors: Alexander Kuzmin <alexander.kuzmin@gmail.com>
//          Martin Ralbovský <martin.ralbovsky@gmail.com>            
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
using System.Resources;
using System.Reflection;
using System.Windows.Forms;
using Ferda.FrontEnd.AddIns.Common.MyIce;
using Ferda.Modules.Boxes.DataPreparation;

namespace Ferda.FrontEnd.AddIns.ShowTable.MyIce
{
    /// <summary>
    /// Clas for ice communication for the ExplainTable module for interaction
    /// </summary>
    class ShowTableIce : ModuleForInteractionIce
    {
        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="ownerOfAddIn">Owner of addin</param>
        public ShowTableIce(IOwnerOfAddIn ownerOfAddIn) : base(ownerOfAddIn, null)
        {
            //setting the ResManager resource manager and localization string
            resManager = new ResourceManager("Ferda.FrontEnd.AddIns.ShowTable.Localization_en-US",
            Assembly.GetExecutingAssembly());
        }

        #endregion

        #region Other ice

        /// <summary>
        /// Gets accepted box type by this module for interaction
        /// </summary>
        /// <param name="__current">Ice context</param>
        /// <returns>Accpeted box types array</returns>
        public override Ferda.Modules.BoxType[] getAcceptedBoxTypes(Ice.Current current__)
        {
            Modules.BoxType boxType = new Modules.BoxType();
            boxType.neededSockets = new Modules.NeededSocket[0];
            boxType.functionIceId = "::Ferda::Modules::Boxes::DataPreparation::DataTableFunctions";
            return new Modules.BoxType[] { boxType };
        }

        /// <summary>
        /// Gets hint to the module according for a specified localization
        /// </summary>
        /// <param name="localePrefs">Localization preferences</param>
        /// <param name="__current">Some ICE stuff</param>
        /// <returns>Localized hint</returns>
        public override string getHint(string[] localePrefs, Ice.Current current__)
        {
            Localize(localePrefs);
            return resManager.GetString("ShowTableModule");
        }

        /// <summary>
        /// Gets label of the module according to the localization
        /// </summary>
        /// <param name="localePrefs">Localization specification</param>
        /// <param name="__current">Some ICE stuff</param>
        /// <returns>Localized label</returns>
        public override string getLabel(string[] localePrefs, Ice.Current current__)
        {
            Localize(localePrefs);
            return resManager.GetString("ShowTableModule");
        }

        /// <summary>
        /// Gets a list of sockets needed to be connected in order for the module to
        /// work
        /// </summary>
        /// <param name="__current">ICE stuff</param>
        /// <returns>List of socket names for module to work propertly</returns>
        public override string[] getNeededConnectedSockets(Ice.Current current__)
        {
            //there has to be a database connected to the data table and a name 
            //needs to be selected
            return new string[] { "Database", "Name" };
        }

        #endregion

        #region IceRun

        /// <summary>
        /// The method is called by te Ice framework when a module for interaction
        /// is to be displayed
        /// </summary>
        /// <param name="boxModuleParam">Box that is executing this module for interaction</param>
        /// <param name="localePrefs">Localization preferences</param>
        /// <param name="manager">Proxy address of the modules manager</param>
        /// <param name="__current">Ice context</param>
        public override void run(Ferda.Modules.BoxModulePrx boxModuleParam, 
            string[] localePrefs, Ferda.ModulesManager.ManagersEnginePrx manager, 
            Ice.Current current__)
        {
            //checking the validity of the box
            try
            {
                boxModuleParam.validate();
            }
            catch (Ferda.Modules.BoxRuntimeError e)
            {
                ownerOfAddIn.ShowBoxException(e);
                return;
            }

            Localize(localePrefs);

            DataTableFunctionsPrx prx =
                DataTableFunctionsPrxHelper.checkedCast(boxModuleParam.getFunctions());

            string label =
                manager.getProjectInformation().getUserLabel(
                Ice.Util.identityToString(boxModuleParam.ice_getIdentity()));
            ShowTableControl control = new ShowTableControl(resManager,
                prx.getColumnsNames(), prx.getDataTableInfo(), ownerOfAddIn);
            this.ownerOfAddIn.ShowDockableControl(control, 
                resManager.GetString("Show") + " " + label);
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
            locale = "Ferda.FrontEnd.AddIns.ShowTable.Localization_" + locale;
            resManager = new ResourceManager(locale, Assembly.GetExecutingAssembly());
        }

        #endregion
    }
}

/* OLD SHOWTABLE IMPLEMENTATION - RUN FUNCTION
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
        Ferda.FrontEnd.AddIns.ShowTable.ShowTableControl control = new ShowTableControl(localePrefs, prx.getColumnsNames(), prx.getDataMatrixInfo(), ownerOfAddIn);
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
*/