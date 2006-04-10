// AbstractDynamicAttributeBoxInfo.cs - box info for dynamic attributes box modules
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
using System.Text;

namespace Ferda.Modules.Boxes.DataMiningCommon.Attributes
{
    /// <summary>
    /// Interface which should be implemented by all dynamic attribute
    /// functionsI objects.
    /// </summary>
	public interface IAbstractDynamicAttribute
	{

        /// <summary>
        /// Gets the column box module proxy.
        /// </summary>
        /// <returns>Proxy of Column box module.</returns>
        BoxModulePrx GetColumnBoxModulePrx();

        /// <summary>
        /// Gets the setting for new attribute box.
        /// </summary>
        /// <returns>Setting for new attribute box.</returns>
        PropertySetting[] GetSettingForNewAttributeBox();
	}

    /// <summary>
    /// Abstract <see cref="T:Ferda.Modules.Boxes.BoxInfo"/> 
    /// class for all dynamic attribute box modules.
    /// </summary>
	public abstract class AbstractDynamicAttributeBoxInfo : AbstractAttributeBoxInfo
	{
        /// <summary>
        /// Gets the function object of abstract attribute.
        /// </summary>
        /// <param name="boxModule">The box module.</param>
        /// <returns>FunctionsI object.</returns>
		public abstract IAbstractDynamicAttribute getFuncIAbstractDynamicAttribute(BoxModuleI boxModule);
		//EachValueOneCategoryAttributeFunctionsI Func = (EachValueOneCategoryAttributeFunctionsI)boxModule.FunctionsIObj;

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
			//Ferda.ModulesManager.BoxModuleProjectInformationPrx projectInfoPrx = boxModule.Manager.getProjectInformation();
			foreach (string moduleAFCName in modulesAFC.Keys)
			{
				moduleAFC = modulesAFC[moduleAFCName];
				moduleConnection = new ModulesConnection();
				singleModuleAFC = new ModuleAskingForCreation();
				switch (moduleAFCName)
				{
					case "AtomSetting":
						moduleConnection.socketName = "Attribute";
						//moduleAFC.newBoxModuleUserLabel = new string[] { projectInfoPrx.getUserLabel(boxModule.StringIceIdentity) };
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
						//moduleAFC.newBoxModuleUserLabel = new string[] { projectInfoPrx.getUserLabel(boxModule.StringIceIdentity) };
						moduleConnection.boxModuleParam = boxModule.MyProxy;
						singleModuleAFC.newBoxModuleIdentifier =
							Ferda.Modules.Boxes.DataMiningCommon.CategorialPartialCedentSetting.CategorialPartialCedentSettingBoxInfo.typeIdentifier;
						break;
					case "Attribute":
						moduleConnection.socketName = "ColumnOrDerivedColumn";
						//moduleAFC.newBoxModuleUserLabel = new string[] { projectInfoPrx.getUserLabel(boxModule.StringIceIdentity) };
						singleModuleAFC.newBoxModuleIdentifier =
							Ferda.Modules.Boxes.DataMiningCommon.Attributes.Attribute.AttributeBoxInfo.typeIdentifier;
						try
						{
							IAbstractDynamicAttribute func = this.getFuncIAbstractDynamicAttribute(boxModule);
							moduleConnection.boxModuleParam = func.GetColumnBoxModulePrx();
							singleModuleAFC.propertySetting = func.GetSettingForNewAttributeBox();
						}
                        catch (Ferda.Modules.BoxRuntimeError) { continue; }
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
	}
}
