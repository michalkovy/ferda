// ResultBrowserIce.cs - class for ice communication
//
// Author:   Alexander Kuzmin <alexander.kuzmin@gmail.com>
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
using Ferda.Modules;
using System.Resources;
using System.Reflection;
using System.Windows.Forms;
using Ferda.Guha.Math.Quantifiers;
using Ferda.Guha.MiningProcessor;
using Ferda.Guha.MiningProcessor.Formulas;
using Ferda.Guha.MiningProcessor.QuantifierEvaluator;
using Ferda.Guha.MiningProcessor.Results;
using Ferda.FrontEnd.AddIns.Common.MyIce;

namespace Ferda.FrontEnd.AddIns.ResultBrowser
{
    /// <summary>
    /// Class for communication with ice or the ResultBrowser module
    /// for interaction
    /// </summary>
    class ResultBrowserIce : ModuleForInteractionIce
    {
        #region Constructor

        /// <summary>
        /// Default constructor of the class
        /// </summary>
        /// <param name="ownerOfAddIn">Owner of addin</param>
        /// <param name="Displayer">
        /// The displayer of object properties, normally the FrontEnd with
        /// the PropertyGrid
        /// </param>
        public ResultBrowserIce(Ferda.FrontEnd.AddIns.IOwnerOfAddIn ownerOfAddIn,
            Ferda.FrontEnd.Properties.IOtherObjectDisplayer Displayer) :
            base(ownerOfAddIn, Displayer)
        {
            resManager = new ResourceManager("Ferda.FrontEnd.AddIns.ResultBrowser.Localization_en-US",
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
            return new string[0];
        }

        /// <summary>
        /// Gets accepted box types (the Result Browser MFI can be used with these
        /// types)
        /// </summary>
        /// <param name="__current">Ice context</param>
        /// <returns>Array of boxtypes</returns>
        public override Ferda.Modules.BoxType[] getAcceptedBoxTypes(Ice.Current __current)
        {
            BoxType a1 = new BoxType();
            a1.neededSockets = new NeededSocket[0];
            a1.functionIceId = "::Ferda::Guha::MiningProcessor::MiningTaskFunctions";

            return new BoxType[] { a1 };
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
            return resManager.GetString("ResultBrowserModule");
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
            return resManager.GetString("ResultBrowserModule");
        }

        #endregion

        #region Run

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
            Ice.Current __current)
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
            
            string taskLabel = boxModuleParam.getMyFactory().getMyFactoryCreator().getLabel(localePrefs);
            BitStringGeneratorProviderPrx taskProxy =
                BitStringGeneratorProviderPrxHelper.checkedCast(boxModuleParam.getFunctions());
            
            MiningTaskFunctionsPrx taskProxy1 =
                MiningTaskFunctionsPrxHelper.checkedCast(boxModuleParam.getFunctions());
        
            AttributeNameProviderPrx nameProvider =
                AttributeNameProviderPrxHelper.checkedCast(boxModuleParam.getFunctions());
            AttributeNameInLiteralsProvider.Init(nameProvider);

            BitStringGeneratorProviderPrx bitStringProvider =
                BitStringGeneratorProviderPrxHelper.checkedCast(boxModuleParam.getFunctions());

            //getting the serialized task result (in order to avoid tasks with no results)
            string statistics = String.Empty;
            string serializedResult = taskProxy1.GetResult(out statistics);

            if (string.IsNullOrEmpty(serializedResult))
            {
                Ferda.Modules.BadParamsError e = new BadParamsError();
                e.userMessage = resManager.GetString("NoHypotheses");
                ownerOfAddIn.ShowBoxException(e);
                return;
            }

            //getting the information about the quantifiers from the task
            Quantifiers quantifiers = new Quantifiers(taskProxy1.GetQuantifiers(), taskProxy, localePrefs);

            if (IsETreeTask(serializedResult))
            {
                FrontEnd.AddIns.ResultBrowser.DecisionTreeBrowser control =
                    new DecisionTreeBrowser(serializedResult, resManager, quantifiers);

                this.ownerOfAddIn.ShowDockableControl(control,
                    taskLabel + " - " + resManager.GetString("ResultBrowserControl"));
            }
            else
            {
                FrontEnd.AddIns.ResultBrowser.FerdaResultBrowserControl control =
                    new FrontEnd.AddIns.ResultBrowser.FerdaResultBrowserControl(
                    resManager, serializedResult, quantifiers,
                    taskProxy, displayer, ownerOfAddIn, bitStringProvider);

                this.ownerOfAddIn.ShowDockableControl(control,
                    taskLabel + " - " + resManager.GetString("ResultBrowserControl"));
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
            locale = "Ferda.FrontEnd.AddIns.ResultBrowser.Localization_" + locale;
            resManager = new ResourceManager(locale, Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// Determines from the serialized result string, if the result
        /// is from ETree task or from other GUHA tasks.
        /// </summary>
        /// <param name="serializedResult">The serialized result</param>
        /// <returns>If the task is ETree task</returns>
        private bool IsETreeTask(string serializedResult)
        {
            try
            {
                DecisionTreeResult result =
                    DecisionTreeResult.Deserialize(serializedResult);
            }
            catch
            {
                return false;
            }
            return true;
        }

        #endregion
    }
}