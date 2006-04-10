// AttributeBoxInfo.cs - box info for attribute box module
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

namespace Ferda.Modules.Boxes.DataMiningCommon.Attributes.Attribute
{
	class AttributeBoxInfo : Ferda.Modules.Boxes.DataMiningCommon.Attributes.AbstractAttributeBoxInfo
	{
		public const string typeIdentifier = 
			"DataMiningCommon.Attributes.Attribute";

		protected override string identifier
		{
			get { return typeIdentifier; }
		}

		public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
		{
			AttributeFunctionsI result = new AttributeFunctionsI();
			iceObject = (Ice.Object)result;
			functions = (IFunctions)result;
		}

		public override string[] GetBoxModuleFunctionsIceIds()
		{
			return AttributeFunctionsI.ids__;
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
			foreach (string moduleAFCName in modulesAFC.Keys)
			{
				moduleAFC = modulesAFC[moduleAFCName];
				moduleConnection = new ModulesConnection();
				singleModuleAFC = new ModuleAskingForCreation();
				switch (moduleAFCName)
				{
					case "AtomSetting":
						moduleConnection.socketName = "Attribute";
						moduleConnection.boxModuleParam = boxModule.MyProxy;
						singleModuleAFC.newBoxModuleIdentifier =
							Ferda.Modules.Boxes.DataMiningCommon.AtomSetting.AtomSettingBoxInfo.typeIdentifier;
						break;
					case "EquivalenceClass":
						moduleConnection.socketName = "LiteralSettingOrAttribute";
						moduleConnection.boxModuleParam = boxModule.MyProxy;
						singleModuleAFC.newBoxModuleIdentifier =
							Ferda.Modules.Boxes.DataMiningCommon.EquivalenceClass.EquivalenceClassBoxInfo.typeIdentifier;
						break;
					case "CategorialPartialCedentSetting":
						moduleConnection.socketName = "Attribute";
						moduleConnection.boxModuleParam = boxModule.MyProxy;
						singleModuleAFC.newBoxModuleIdentifier =
							Ferda.Modules.Boxes.DataMiningCommon.CategorialPartialCedentSetting.CategorialPartialCedentSettingBoxInfo.typeIdentifier;
						break;
					default:
						throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(moduleAFCName);
				}
				singleModuleAFC.modulesConnection = new ModulesConnection[] { moduleConnection };
				moduleAFC.newModules = new ModuleAskingForCreation[] { singleModuleAFC };
				result.Add(moduleAFC);
			}
			return result.ToArray();
		}

        /// <summary>
        /// Gets value of readonly property value.
        /// </summary>
        /// <param name="propertyName">Name of readonly property.</param>
        /// <param name="boxModule">Box module.</param>
        /// <returns>
        /// A <see cref="T:Ferda.Modules.PropertyValue"/> of
        /// readonly property named <c>propertyName</c>.
        /// </returns>
		public override PropertyValue GetReadOnlyPropertyValue(String propertyName, BoxModuleI boxModule)
		{
			AttributeFunctionsI Func = (AttributeFunctionsI)boxModule.FunctionsIObj;
			switch (propertyName)
			{
				case "CountOfCategories":
					return new Ferda.Modules.LongTI(Func.CountOfCategories());
				default:
					throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(propertyName);
			}
		}

		protected override IAbstractAttribute getFuncIAbstractAttribute(BoxModuleI boxModule)
		{
			AttributeFunctionsI Func = (AttributeFunctionsI)boxModule.FunctionsIObj;
			return Func;
		}
	}
}