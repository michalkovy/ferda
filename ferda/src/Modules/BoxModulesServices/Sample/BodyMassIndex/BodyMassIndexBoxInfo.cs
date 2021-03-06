// BodyMassIndexBoxInfo.cs - box info for body mass index box module
//
// Author: TomĂˇĹˇ KuchaĹ™ <tomas.kuchar@gmail.com>
//
// Copyright (c) 2005 TomĂˇĹˇ KuchaĹ™
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
using Object=Ice.Object;

namespace Ferda.Modules.Boxes.Sample.BodyMassIndex
{
    /// <summary>
    /// Implementation of <see cref="T:Ferda.Modules.Boxes.IBoxInfo"/>
    /// using inheritence of abstract class <see cref="T:Ferda.Modules.Boxes.BoxInfo"/>.
    /// This class has to be registered in <see cref="T:Ferda.Modules.FerdaServiceI"/> class ... 
    /// please see <see cref="T:Ferda.Modules.Boxes.Sample.Service"/> and its
    /// method <see cref="M:Ferda.Modules.Boxes.Sample.Service.registerBoxes()"/>.
    /// </summary>
    public class BodyMassIndexBoxInfo : BoxInfo
    {
        /// <summary>
        /// Creates the functions.
        /// </summary>
        /// <param name="boxModule">The box module.</param>
        /// <param name="iceObject">The ice object.</param>
        /// <param name="functions">The functions.</param>
        public override void CreateFunctions(BoxModuleI boxModule, out Object iceObject, out IFunctions functions)
        {
            BodyMassIndexFunctionsI result = new BodyMassIndexFunctionsI();
            iceObject = (Object) result;
            functions = (IFunctions) result;
        }

        /// <summary>
        /// Gets function`s Ice identifiers of the box module.
        /// </summary>
        /// <returns>
        /// An array of strings representing Ice identifiers
        /// of the box module`s functions.
        /// </returns>
        /// <example>
        /// Please see an example for <see cref="T:Ferda.Modules.Boxes.IBoxInfo">IBoxInfo`s</see>
        /// 	<see cref="M:Ferda.Modules.Boxes.IBoxInfo.GetBoxModuleFunctionsIceIds()"/>.
        /// </example>
        public override string[] GetBoxModuleFunctionsIceIds()
        {
            var f = new BodyMassIndexFunctionsI();
            return f.ice_ids();
        }

        /// <summary>
        /// Gets default value for box module user label.
        /// </summary>
        public override string GetDefaultUserLabel(BoxModuleI boxModule)
        {
            BodyMassIndexFunctionsI functionsObject = (BodyMassIndexFunctionsI) boxModule.FunctionsIObj;
            return functionsObject.GetDefaultUserLabel(false);
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
            Dictionary<string, ModulesAskingForCreation> modulesAFC = getModulesAskingForCreationNonDynamic(localePrefs);
            List<ModulesAskingForCreation> result = new List<ModulesAskingForCreation>();
            ModulesAskingForCreation moduleAFC;
            ModulesConnection moduleConnection;
            ModuleAskingForCreation singleModuleAFC;
            foreach (string moduleAFCName in modulesAFC.Keys)
            {
                moduleAFC = modulesAFC[moduleAFCName];
                moduleConnection = new ModulesConnection();
                singleModuleAFC = new ModuleAskingForCreation();
                switch (moduleAFCName)
                {
                    case "Attribute":
                        moduleConnection.socketName = "ColumnOrDerivedColumn";
                        singleModuleAFC.newBoxModuleIdentifier = "DataMiningCommon.Attributes.Attribute";
                        // == Ferda.Modules.Boxes.DataMiningCommon.Attributes.Attribute.AttributeBoxInfo.typeIdentifier;
                        break;
                    default:
                        throw new NotImplementedException();
                }
                moduleConnection.boxModuleParam = boxModule.MyProxy;
                singleModuleAFC.modulesConnection = new ModulesConnection[] {moduleConnection};
                moduleAFC.newModules = new ModuleAskingForCreation[] {singleModuleAFC};
                result.Add(moduleAFC);
            }
            return result.ToArray();
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
        /// This is recomended (not required) to have <c>public const string</c> 
        /// field in the BoxInfo implementation which holds the identifier 
        /// of type of the box module.
        /// </summary>
        public const string typeIdentifier = "Sample.BodyMassIndex";

        /// <summary>
        /// Unique identifier of type of Box module
        /// </summary>
        /// <value></value>
        /// <remarks>
        /// This string identifier is parsed i.e. dots (".") are
        /// replaced by <see cref="P:System.IO.Path.DirectorySeparatorChar"/>.
        /// Returned path is combined with application directory`s
        /// <see cref="F:Ferda.Modules.Boxes.BoxInfo.configFilesFolderName">subdirectory</see>
        /// and final path is used for getting stored configuration [localization] XML files.
        /// </remarks>
        protected override string identifier
        {
            get { return typeIdentifier; }
        }
    }
}