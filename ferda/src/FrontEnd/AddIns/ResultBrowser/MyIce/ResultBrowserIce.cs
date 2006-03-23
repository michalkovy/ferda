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
            BoxType a = new BoxType();
            a.neededSockets = new NeededSocket[0];
            a.functionIceId = "::Ferda::Modules::Boxes::LISpMinerTasks::AbstractLMTask::AbstractLMTaskFunctions";
            return new BoxType[] { a };
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
            //getting proxy for task identifier
            Ice.ObjectPrx prx2 = boxModuleParam.getMyFactory();

            Modules.BoxModuleFactoryPrx tprx2 =
            Modules.BoxModuleFactoryPrxHelper.checkedCast(prx2);

            string taskType = tprx2.getMyFactoryCreator().getIdentifier();

            //getting proxy for hypotheses and quantifiers
            Ice.ObjectPrx prx = boxModuleParam.getFunctions();

            Modules.Boxes.LISpMinerTasks.AbstractLMTask.AbstractLMTaskFunctionsPrx tprx =
                Modules.Boxes.LISpMinerTasks.AbstractLMTask.AbstractLMTaskFunctionsPrxHelper.checkedCast(prx);
            Modules.HypothesisStruct[] hypotheses = tprx.getResult();

            Modules.Boxes.LISpMinerTasks.AbstractLMTask.QuantifierProvider[] used_quantifiers =
                tprx.getQuantifierProviders();


            Ice.ObjectPrx[] prxs =
                manager.getManagersLocator().findAllObjectsWithType("::Ferda::Statistics::StatisticsProvider");

            //get from task box
            // string taskType = "LISpMinerTasks.FFTask";
            string temp = "";

            List<Ferda.Statistics.StatisticsProviderPrx> proxies = new List<Ferda.Statistics.StatisticsProviderPrx>();

            foreach (Ice.ObjectPrx proxy in prxs)
            {
                Ferda.Statistics.StatisticsProviderPrx checkedProxy =
                Ferda.Statistics.StatisticsProviderPrxHelper.checkedCast(proxy);

                temp = checkedProxy.getTaskType();

                if (temp.CompareTo(taskType) == 0)
                {
                    proxies.Add(checkedProxy);
                }
            }

            try
            {
                FrontEnd.AddIns.ResultBrowser.FerdaResultBrowserControl control = new FrontEnd.AddIns.ResultBrowser.FerdaResultBrowserControl(localePrefs, hypotheses, used_quantifiers, this.Displayer, proxies, taskType, ownerOfAddIn);
                this.ownerOfAddIn.ShowDockableControl(control, resManager.GetString("ResultBrowserControl"));
            }
            catch (Ferda.Modules.NoConnectionInSocketError)
            {
                MessageBox.Show(resManager.GetString("BoxNotConnected"), resManager.GetString("Error"),
                           MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        #endregion
    }
}