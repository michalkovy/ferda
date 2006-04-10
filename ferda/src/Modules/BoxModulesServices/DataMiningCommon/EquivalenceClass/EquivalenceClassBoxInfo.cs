// EquivalenceClassBoxInfo.cs - box info for equivalence class box module
//
// Author: Tomáš Kuchař <tomas.kuchar@gmail.com>
//
// Copyright (c) 2005 Tomáš Kuchař
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

namespace Ferda.Modules.Boxes.DataMiningCommon.EquivalenceClass
{
    class EquivalenceClassBoxInfo : Ferda.Modules.Boxes.BoxInfo
    {
        public const string typeIdentifier =
            "DataMiningCommon.EquivalenceClass";

        protected override string identifier
        {
            get { return typeIdentifier; }
        }

        public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
        {
            EquivalenceClassFunctionsI result = new EquivalenceClassFunctionsI();
            iceObject = (Ice.Object)result;
            functions = (IFunctions)result;
        }

        public override string[] GetBoxModuleFunctionsIceIds()
        {
            return EquivalenceClassFunctionsI.ids__;
        }

        /// <summary>
        /// Gets default value for box module user label.
        /// </summary>
        public override string GetDefaultUserLabel(BoxModuleI boxModule)
        {
            return null;
        }

        /// <summary>
        /// Gets array of <see cref="T:Ferda.Modules.SelectString"/> as
        /// options for property, whose options are dynamically variable.
        /// </summary>
        public override SelectString[] GetPropertyOptions(string propertyName, BoxModuleI boxModule)
        {
            return null;
        }

        /// <summary>
        /// Gets the box modules asking for creation.
        /// </summary>
        /// <param name="localePrefs">The localization preferences.</param>
        /// <param name="boxModule">The box module.</param>
        /// <returns>
        /// Array of <see cref="T:Ferda.Modules.ModuleAskingForCreation">
        /// Modules Asking For Creation</see>.
        /// </returns>
        public override ModulesAskingForCreation[] GetModulesAskingForCreation(string[] localePrefs, BoxModuleI boxModule)
        {
            Dictionary<string, ModulesAskingForCreation> modulesAFC = this.getModulesAskingForCreationNonDynamic(localePrefs);
            List<ModulesAskingForCreation> result = new List<ModulesAskingForCreation>();
            ModulesAskingForCreation moduleAFC;
            ModulesConnection moduleConnection;
            ModuleAskingForCreation singleModuleAFC;
            //PropertySetting propertySetting;
            //Ferda.ModulesManager.BoxModuleProjectInformationPrx projectInfoPrx = boxModule.Manager.getProjectInformation();
            //string label = projectInfoPrx.getUserLabel(boxModule.StringIceIdentity);
            foreach (string moduleAFCName in modulesAFC.Keys)
            {
                moduleAFC = modulesAFC[moduleAFCName];
                moduleConnection = new ModulesConnection();
                singleModuleAFC = new ModuleAskingForCreation();
                switch (moduleAFCName)
                {
                    case "BooleanPartialCedentSetting":
                        moduleConnection.socketName = "EquivalenceClass";
                        singleModuleAFC.newBoxModuleIdentifier =
                            Ferda.Modules.Boxes.DataMiningCommon.BooleanPartialCedentSetting.BooleanPartialCedentSettingBoxInfo.typeIdentifier;
                        break;
                    default:
                        throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(moduleAFCName);
                }
                moduleConnection.boxModuleParam = boxModule.MyProxy;
                singleModuleAFC.modulesConnection = new ModulesConnection[] { moduleConnection };
                moduleAFC.newModules = new ModuleAskingForCreation[] { singleModuleAFC };
                result.Add(moduleAFC);
            }
            return result.ToArray();
        }
    }
}