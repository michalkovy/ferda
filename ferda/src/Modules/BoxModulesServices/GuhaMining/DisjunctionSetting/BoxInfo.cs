// BoxInfo.cs - BoxInfo class for the disjunction setting box
//
// Authors: Tomáš Kuchaø <tomas.kuchar@gmail.com>,
//          Martin Ralbovský <martin.ralbovsky@gmail.com> (modulesAFC)
//
// Copyright (c) 2006 Tomáš Kuchaø, Martin Ralbovský
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

using System.Collections.Generic;
using System;
using Ferda.Guha.MiningProcessor;
using Ferda.Guha.MiningProcessor.Formulas;
using Ferda.Modules.Helpers.Common;
using Object = Ice.Object;

namespace Ferda.Modules.Boxes.GuhaMining.DisjunctionSetting
{
    internal class BoxInfo : Boxes.BoxInfo
    {
        public override void CreateFunctions(BoxModuleI boxModule, out Object iceObject, out IFunctions functions)
        {
            Functions result = new Functions();
            iceObject = result;
            functions = result;
        }

        public override string[] GetBoxModuleFunctionsIceIds()
        {
            return Functions.ids__;
        }

        public override string GetDefaultUserLabel(BoxModuleI boxModule)
        {
            Functions Func = (Functions) boxModule.FunctionsIObj;

            string[] inputBoxesLabels = Func.GetInputBoxesLabels();

            if (inputBoxesLabels.Length == 0)
                return null;
            else
            {
                List<string> result = new List<string>();
                foreach (string s in inputBoxesLabels)
                {
                    if (s.Contains(FormulaHelper.SeparatorAnd))
                        result.Add("(" + s + ")");
                    else
                        result.Add(s);
                }
                return Print.SequenceToString(result, FormulaHelper.SeparatorOr);
            }
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
        public override ModulesAskingForCreation[] GetModulesAskingForCreation(string[] localePrefs,
                                                                               BoxModuleI boxModule)
        {
            //getting the information what is in the config files
            Dictionary<string, ModulesAskingForCreation> modulesAFC =
                getModulesAskingForCreationNonDynamic(localePrefs);
            //creating the structure that will be returned
            List<ModulesAskingForCreation> result =
                new List<ModulesAskingForCreation>();

            ModulesConnection moduleConnection;
            ModuleAskingForCreation singleModule;

            foreach (string moduleAFCname in modulesAFC.Keys)
            {
                singleModule = new ModuleAskingForCreation();
                moduleConnection = new ModulesConnection();
                //no need to set any property
                singleModule.propertySetting = new PropertySetting[] { };

                switch (moduleAFCname)
                {
                    case "ConjunctionSetting":
                        //creating the info about the connections of the new module
                        moduleConnection.socketName =
                            ConjunctionSetting.Functions.SockBooleanAttributeSetting;
                        moduleConnection.boxModuleParam = boxModule.MyProxy;

                        //creating the new (single) module
                        singleModule.modulesConnection =
                            new ModulesConnection[] { moduleConnection };
                        singleModule.newBoxModuleIdentifier =
                            ConjunctionSetting.BoxInfo.typeIdentifier;
                        break;

                    case "DisjunctionSetting":
                        //creating the info about the connections of the new module
                        moduleConnection.socketName =
                            DisjunctionSetting.Functions.SockBooleanAttributeSetting;
                        moduleConnection.boxModuleParam = boxModule.MyProxy;

                        //creating the new (single) module
                        singleModule.modulesConnection =
                            new ModulesConnection[] { moduleConnection };
                        singleModule.newBoxModuleIdentifier =
                            DisjunctionSetting.BoxInfo.typeIdentifier;
                        break;

                    case "Sign":
                        //creating the info about the connections of the new module
                        moduleConnection.socketName =
                            Sign.Functions.SockBooleanAttributeSetting;
                        moduleConnection.boxModuleParam = boxModule.MyProxy;

                        //creating the new (single) module
                        singleModule.modulesConnection =
                            new ModulesConnection[] { moduleConnection };
                        singleModule.newBoxModuleIdentifier =
                            Sign.BoxInfo.typeIdentifier;
                        break;

                    case "ClassOfEquivalence":
                        //creating the info about the connections of the new module
                        moduleConnection.socketName =
                            ClassOfEquivalence.Functions.SockBooleanAttributeSetting;
                        moduleConnection.boxModuleParam = boxModule.MyProxy;

                        //creating the new (single) module
                        singleModule.modulesConnection =
                            new ModulesConnection[] { moduleConnection };
                        singleModule.newBoxModuleIdentifier =
                            ClassOfEquivalence.BoxInfo.typeIdentifier;
                        break;

                    default:
                        throw new NotImplementedException();
                }

                //setting the newModules property of each modules for intearction
                modulesAFC[moduleAFCname].newModules =
                    new ModuleAskingForCreation[] { singleModule };
                result.Add(modulesAFC[moduleAFCname]);
            }

            return result.ToArray();
        }

        public override SelectString[] GetPropertyOptions(string propertyName, BoxModuleI boxModule)
        {
            return null;
        }

        public const string typeIdentifier = "GuhaMining.DisjunctionSetting";

        protected override string identifier
        {
            get { return typeIdentifier; }
        }

        public override void Validate(BoxModuleI boxModule)
        {
            Functions Func = (Functions)boxModule.FunctionsIObj;

            // try to invoke methods
            IEntitySetting myES = Func.GetEntitySetting(true);
            object dummy = Func.GetAttributeNames();
            dummy = Func.GetSourceDataTableId();

            List<BooleanAttributeSettingFunctionsPrx> booleanAttributes = Func.GetBooleanAttributeSettingFunctionsPrxs(true);
            if (booleanAttributes != null)
            {
                bool notOnlyAuxiliary = false;
                bool containsForced = false;
                foreach (BooleanAttributeSettingFunctionsPrx prx in booleanAttributes)
                {
                    IEntitySetting eS = prx.GetEntitySetting();
                    if (eS.importance != ImportanceEnum.Auxiliary)
                        notOnlyAuxiliary = true;
                    if (eS.importance == ImportanceEnum.Forced)
                        containsForced = true;
                }

                // inner entity can not be auxiliary
                if (!notOnlyAuxiliary)
                    throw Exceptions.BadValueError(
                        null,
                        boxModule.StringIceIdentity,
                        "Inner boolean attribute settings can not be only auxiliary.",
                        new string[] { Functions.SockBooleanAttributeSetting },
                        restrictionTypeEnum.OtherReason
                        );
                if (containsForced &&
                    myES.importance != ImportanceEnum.Forced)
                    throw Exceptions.BadValueError(
                        null,
                        boxModule.StringIceIdentity,
                        "Some inner boolean attribute setting is forced, therefore current has to be also forced.",
                        new string[] { Functions.SockBooleanAttributeSetting },
                        restrictionTypeEnum.OtherReason
                        );
            }
        }
    }
}