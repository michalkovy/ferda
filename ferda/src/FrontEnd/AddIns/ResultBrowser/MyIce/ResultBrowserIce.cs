// ResultBrowserIce.cs - class for ice communication
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
using System.Resources;
using System.Reflection;
using System.Windows.Forms;
using Ferda.Guha.Math.Quantifiers;
using Ferda.Guha.MiningProcessor;
using Ferda.Guha.MiningProcessor.QuantifierEvaluator;

namespace Ferda.FrontEnd.AddIns.ResultBrowser
{
    /// <summary>
    /// Class for communication with ice
    /// </summary>
    class ResultBrowserIce : Ferda.Modules.ModuleForInteractionDisp_
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

        private Ferda.FrontEnd.Properties.IOtherObjectDisplayer Displayer;

        #endregion


        #region Constructor

        public ResultBrowserIce(Ferda.FrontEnd.AddIns.IOwnerOfAddIn ownerOfAddIn, Ferda.FrontEnd.Properties.IOtherObjectDisplayer Displayer)
        {
            this.ownerOfAddIn = ownerOfAddIn;
            //setting the ResManager resource manager and localization string
            resManager = new ResourceManager("Ferda.FrontEnd.AddIns.ResultBrowser.Localization_en-US",
            Assembly.GetExecutingAssembly());
            localizationString = "en-US";
            this.Displayer = Displayer;
        }

        #endregion


        #region Other ice

        public override Ferda.Modules.BoxType[] getAcceptedBoxTypes(Ice.Current __current)
        {
            BoxType a1 = new BoxType();
            a1.neededSockets = new NeededSocket[0];
            a1.functionIceId = "::GuhaMining::Tasks::FourFold";

            BoxType a2 = new BoxType();
            a2.neededSockets = new NeededSocket[0];
            a2.functionIceId = "::GuhaMining::Tasks::SDFourFold";

            BoxType a3 = new BoxType();
            a3.neededSockets = new NeededSocket[0];
            a3.functionIceId = "::GuhaMining::Tasks::SingleDimensional";

            BoxType a4 = new BoxType();
            a4.neededSockets = new NeededSocket[0];
            a4.functionIceId = "::GuhaMining::Tasks::SDSingleDimensional";

            BoxType a5 = new BoxType();
            a5.neededSockets = new NeededSocket[0];
            a5.functionIceId = "::GuhaMining::Tasks::TwoDimensional";

            BoxType a6 = new BoxType();
            a6.neededSockets = new NeededSocket[0];
            a6.functionIceId = "::GuhaMining::Tasks::SDTwoDimensional";

            return new BoxType[] { a1, a2, a3, a4, a5, a6 };
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
                locale = "Ferda.FrontEnd.AddIns.ResultBrowser.Localization_" + locale;
                resManager = new ResourceManager(locale, Assembly.GetExecutingAssembly());
            }
            catch
            {
            }
            return resManager.GetString("ResultBrowserModule");
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
                locale = "Ferda.FrontEnd.AddIns.ResultBrowser.Localization_" + locale;
                resManager = new ResourceManager(locale, Assembly.GetExecutingAssembly());
            }
            catch
            {

            }
            return resManager.GetString("ResultBrowserModule");
        }

        public override string[] getNeededConnectedSockets(Ice.Current __current)
        {
            return new string[0];
        }

        #endregion


        #region Run

        public override void run(Ferda.Modules.BoxModulePrx boxModuleParam, string[] localePrefs, Ferda.ModulesManager.ManagersEnginePrx manager, Ice.Current __current)
        {
            string locale;
            try
            {
                locale = localePrefs[0];
                localizationString = locale;
                locale = "Ferda.FrontEnd.AddIns.ResultBrowser.Localization_" + locale;
                resManager = new ResourceManager(locale, Assembly.GetExecutingAssembly());
            }
            catch
            {
            }
            BitStringGeneratorProviderPrx  taskProxy =
                BitStringGeneratorProviderPrxHelper.checkedCast(boxModuleParam.getFunctions());
            
            MiningTaskFunctionsPrx taskProxy1 =
                MiningTaskFunctionsPrxHelper.checkedCast(boxModuleParam.getFunctions());
            string statistics = String.Empty;

            Quantifiers quantifiers = new Quantifiers(taskProxy1.GetQuantifiers(), taskProxy, localePrefs);

            try
            {
                FrontEnd.AddIns.ResultBrowser.FerdaResultBrowserControl control = new FrontEnd.AddIns.ResultBrowser.FerdaResultBrowserControl(localePrefs, taskProxy1.GetResult(out statistics), quantifiers, this.Displayer, ownerOfAddIn);
                this.ownerOfAddIn.ShowDockableControl(control, resManager.GetString("ResultBrowserControl"));
            }
            catch (Ferda.Modules.NoConnectionInSocketError)
            {
                MessageBox.Show(resManager.GetString("BoxNotConnected"), resManager.GetString("Error"),
                           MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            catch (Ferda.Modules.BadValueError)
            {
                MessageBox.Show(resManager.GetString("EmptyResult"), resManager.GetString("Error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        #endregion
    }
}