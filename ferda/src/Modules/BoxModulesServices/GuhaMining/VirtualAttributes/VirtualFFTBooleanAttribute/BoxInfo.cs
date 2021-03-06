// BoxInfo.cs - box functions for VirtualFFBooleanAttribute box
//
// Author: Alexander Kuzmin <alexander.kuzmin@gmail.com>
//
// Copyright (c) 2007 Alexander Kuzmin
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
using Ferda.Guha.Data;
using Object=Ice.Object;
using Ferda.Modules.Boxes.DataPreparation;

namespace Ferda.Modules.Boxes.GuhaMining.VirtualAttributes.VirtualFFTBooleanAttribute
{
    internal class BoxInfo : Boxes.BoxInfo
    {
        /// <summary>
        /// Creates Functions object
        /// </summary>
        /// <param name="boxModule">The boxmodule</param>
        /// <param name="iceObject">The ice object</param>
        /// <param name="functions">Functions</param>
        public override void CreateFunctions(BoxModuleI boxModule, out Object iceObject, out IFunctions functions)
        {
            Functions result = new Functions();
            iceObject = result;
            functions = result;
        }

        /// <summary>
        /// Gets functions ids
        /// </summary>
        /// <returns></returns>
        public override string[] GetBoxModuleFunctionsIceIds()
        {
            var f = new Functions();
            return f.ice_ids();
        }

        /// <summary>
        /// Displaying default user label for the box, if it is not customized.
        /// Here the default name from .xml config files is used
        /// </summary>
        /// <param name="boxModule">The boxmodule</param>
        /// <returns>Box label</returns>
        public override string GetDefaultUserLabel(BoxModuleI boxModule)
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

        /// <summary>
        /// Gets property options.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="boxModule"></param>
        /// <returns></returns>
        public override SelectString[] GetPropertyOptions(string propertyName, BoxModuleI boxModule)
        {
            Functions Func = (Functions)boxModule.FunctionsIObj;

            switch (propertyName)
            {
                default:
                    return null;
            }
        }

        /// <summary>
        /// Returns read-only property values for displaying in propertygrid
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="boxModule">The boxmodule.</param>
        /// <returns></returns>
        public override PropertyValue GetReadOnlyPropertyValue(string propertyName, BoxModuleI boxModule)
        {
            Functions Func = (Functions)boxModule.FunctionsIObj;
            switch (propertyName)
            {
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Box identifier
        /// </summary>
        public const string typeIdentifier = "GuhaMining.VirtualAttributes.VirtualFFTBooleanAttribute";

        /// <summary>
        /// Validation of the box
        /// </summary>
        /// <param name="boxModule">The boxmodule</param>
        public override void Validate(BoxModuleI boxModule)
        {
            Functions Func = (Functions)boxModule.FunctionsIObj;

            object dummy = Func.GetSourceDataTableId();

            dummy = 
                Ferda.Modules.Boxes.GuhaMining.Tasks.Common.GetBooleanAttributes(boxModule, Func);

            dummy = 
                Ferda.Modules.Boxes.GuhaMining.Tasks.Common.GetQuantifierBaseFunctions(boxModule, true);

            DataTableFunctionsPrx _dtPrx = Func.GetMasterDataTableFunctionsPrx(true);
            string[] _primaryKeyColumns = _dtPrx.getDataTableInfo().primaryKeyColumns;
            if (_primaryKeyColumns.Length < 1)
            {
                throw Exceptions.BoxRuntimeError(null, boxModule.StringIceIdentity, "No unique key selected");
            }

            if (Func.CountVector == null)
            {
                throw Exceptions.BoxRuntimeError(null, boxModule.StringIceIdentity, "Unable to get count vector");
            }
            
        }

        /// <summary>
        /// Box identifier
        /// </summary>
        protected override string identifier
        {
            get { return typeIdentifier; }
        }

    }
}