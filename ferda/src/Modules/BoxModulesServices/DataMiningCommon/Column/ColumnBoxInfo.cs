// ColumnBoxInfo.cs - box info for column box module
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

namespace Ferda.Modules.Boxes.DataMiningCommon.Column
{
    class ColumnBoxInfo : Ferda.Modules.Boxes.BoxInfo
    {
        public const string typeIdentifier =
            "DataMiningCommon.Column";

        protected override string identifier
        {
            get { return typeIdentifier; }
        }

        public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
        {
            ColumnFunctionsI result = new ColumnFunctionsI();
            iceObject = (Ice.Object)result;
            functions = (IFunctions)result;
        }

        public override string[] GetBoxModuleFunctionsIceIds()
        {
            return ColumnFunctionsI.ids__;
        }

        /// <summary>
        /// Gets default value for box module user label.
        /// </summary>
        public override string GetDefaultUserLabel(BoxModuleI boxModule)
        {
            return boxModule.GetPropertyString(ColumnSelectExpressionPropertyName);
        }

        /// <summary>
        /// Gets array of <see cref="T:Ferda.Modules.SelectString"/> as
        /// options for property, whose options are dynamically variable.
        /// </summary>
        public override SelectString[] GetPropertyOptions(String propertyName, BoxModuleI boxModule)
        {
            switch (propertyName)
            {
                case "Name":
                    return BoxInfoHelper.StringArrayToSelectStringArray(
                        ((ColumnFunctionsI)boxModule.FunctionsIObj).GetColumnsNames()
                        );
                default:
                    return null;
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
        public override ModulesAskingForCreation[] GetModulesAskingForCreation(string[] localePrefs, BoxModuleI boxModule)
        {
            Dictionary<string, ModulesAskingForCreation> modulesAFC = this.getModulesAskingForCreationNonDynamic(localePrefs);
            List<ModulesAskingForCreation> result = new List<ModulesAskingForCreation>();
            ModulesAskingForCreation moduleAFC;
            ModulesConnection moduleConnection;
            ModuleAskingForCreation singleModuleAFC;
            PropertySetting propertySetting;
            Ferda.ModulesManager.BoxModuleProjectInformationPrx projectInfoPrx = boxModule.Manager.getProjectInformation();
            string label = projectInfoPrx.getUserLabel(boxModule.StringIceIdentity);

            //TODO switch column type:
            ValueSubTypeEnum columnValueSubType = ((ColumnFunctionsI)boxModule.FunctionsIObj).GetColumnSubType();
            //ValueSubTypeEnum columnValueSubType = (ValueSubTypeEnum)(Enum.Parse(typeof(ValueSubTypeEnum), boxModule.GetPropertyString("ValueSubType")));


            foreach (string moduleAFCName in modulesAFC.Keys)
            {
                moduleAFC = modulesAFC[moduleAFCName];
                moduleConnection = new ModulesConnection();
                singleModuleAFC = new ModuleAskingForCreation();
                switch (moduleAFCName)
                {
                    case "MultiColumn":
                        //TODO MultiColumn is not implemented yet.

                        //moduleConnection.socketName = "Column";
                        //singleModuleAFC.newBoxModuleIdentifier =
                        //	Ferda.Modules.Boxes.DataMiningCommon.MultiColumn.MultiColumnBoxInfo.typeIdentifier;
                        continue;
                    case "Attribute":
                    case "EachValueOneCategoryAttribute":
                    case "EquifrequencyIntervalsAttribute":
                    case "EquidistantIntervalsAttribute":
                        if (moduleAFCName == "Attribute")
                        {
                            singleModuleAFC.newBoxModuleIdentifier =
                                Ferda.Modules.Boxes.DataMiningCommon.Attributes.Attribute.AttributeBoxInfo.typeIdentifier;
                        }
                        if (moduleAFCName == "EachValueOneCategoryAttribute")
                        {
                            singleModuleAFC.newBoxModuleIdentifier =
                                Ferda.Modules.Boxes.DataMiningCommon.Attributes.EachValueOneCategoryAttribute.EachValueOneCategoryAttributeBoxInfo.typeIdentifier;
                        }
                        if (moduleAFCName == "EquifrequencyIntervalsAttribute")
                        {
                            if (!Ferda.Modules.Helpers.Data.Column.IsColumSubTypeCardinal(columnValueSubType)
                                || columnValueSubType == ValueSubTypeEnum.DateTimeType)
                                continue;
                            singleModuleAFC.newBoxModuleIdentifier =
                                Ferda.Modules.Boxes.DataMiningCommon.Attributes.EquifrequencyIntervalsAttribute.EquifrequencyIntervalsAttributeBoxInfo.typeIdentifier;
                        }
                        if (moduleAFCName == "EquidistantIntervalsAttribute")
                        {
                            if ((columnValueSubType == ValueSubTypeEnum.Unknown)
                                || (columnValueSubType == ValueSubTypeEnum.DateTimeType)
                                //|| (columnValueSubType == ValueSubTypeEnum.TimeType)
                                || (columnValueSubType == ValueSubTypeEnum.BooleanType))
                                continue;
                            singleModuleAFC.newBoxModuleIdentifier =
                                Ferda.Modules.Boxes.DataMiningCommon.Attributes.EquidistantIntervalsAttribute.EquidistantIntervalsAttributeBoxInfo.typeIdentifier;
                        }
                        moduleConnection.socketName = "ColumnOrDerivedColumn";
                        propertySetting = new PropertySetting();
                        propertySetting.propertyName = "NameInLiterals";
                        propertySetting.value = new Ferda.Modules.StringTI(label);
                        singleModuleAFC.propertySetting = new PropertySetting[] { propertySetting };
                        break;
                    default:
                        throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(moduleAFCName);
                }
                moduleConnection.boxModuleParam = boxModule.MyProxy;
                singleModuleAFC.modulesConnection = new ModulesConnection[] { moduleConnection };
                singleModuleAFC.newBoxModuleUserLabel = new string[] { label };
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
            ColumnFunctionsI Func = (ColumnFunctionsI)boxModule.FunctionsIObj;
            switch (propertyName)
            {
                case "ValueSubType":
                    //TODO for derived column
                    return new Ferda.Modules.StringTI(Func.GetColumnSubType().ToString());
                case "ValueMin":
                    return new Ferda.Modules.StringTI(Func.GetStatistics().ValueMin);
                case "ValueMax":
                    return new Ferda.Modules.StringTI(Func.GetStatistics().ValueMax);
                case "ValueAverage":
                    return new Ferda.Modules.StringTI(Func.GetStatistics().ValueAverage);
                case "ValueVariability":
                    return new Ferda.Modules.DoubleTI(Func.GetStatistics().ValueVariability);
                case "ValueStandardDeviation":
                    return new Ferda.Modules.DoubleTI(Func.GetStatistics().ValueStandardDeviation);
                case "ValueDistincts":
                    return new Ferda.Modules.LongTI(Func.GetStatistics().ValueDistincts);
                default:
                    throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(propertyName);
            }
        }

        /// <summary>
        /// Executes (runs) action specified by <c>actionName</c>.
        /// </summary>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="boxModule">The Box module.</param>
        /// <exception cref="T:Ferda.Modules.NameNotExistError">Thrown if action named <c>actionName</c> doesn`t exist.</exception>
        /// <exception cref="T:Ferda.Modules.BoxRuntimeError">Thrown if any runtime error occured while executing the action.</exception>
        public override void RunAction(string actionName, BoxModuleI boxModule)
        {
            switch (actionName)
            {
                case "TestColumnSelectExpression":
                    this.TestColumnSelectExpressionAction(boxModule);
                    break;
                default:
                    throw Ferda.Modules.Exceptions.SwitchCaseNotImplementedError(actionName);
            }
        }

        private void TestColumnSelectExpressionAction(BoxModuleI boxModule)
        {
            ColumnFunctionsI functionsIObj = (ColumnFunctionsI)boxModule.FunctionsIObj;
            bool isColumnSelectExpressionValid = false;
            try
            {
                DataMatrix.DataMatrixInfo dataMatrixInfo = functionsIObj.GetDataMatrixFunctionsPrx().getDataMatrixInfo();
                Ferda.Modules.Helpers.Data.Column.TestColumnSelectExpression(
                    dataMatrixInfo.database.odbcConnectionString,
                    dataMatrixInfo.dataMatrixName,
                    functionsIObj.ColumnSelectExpression,
                    boxModule.StringIceIdentity);
                isColumnSelectExpressionValid = true;
            }
            catch (Ferda.Modules.BoxRuntimeError) { }

            if (isColumnSelectExpressionValid)
                // test succeed
                boxModule.OutputMessage(
                    Ferda.ModulesManager.MsgType.Info,
                    "TestColumnSelectExpression",
                    "TestColumnSelectExpressionSucceed");
            else
                // test failed
                boxModule.OutputMessage(
                    Ferda.ModulesManager.MsgType.Warning,
                    "TestColumnSelectExpression",
                    "TestColumnSelectExpressionFailed");
        }

        public override void Validate(BoxModuleI boxModule)
        {
            ColumnFunctionsI functionsIObj = (ColumnFunctionsI)boxModule.FunctionsIObj;
            DataMatrix.DataMatrixInfo dataMatrixInfo = functionsIObj.GetDataMatrixFunctionsPrx().getDataMatrixInfo();
            Ferda.Modules.Helpers.Data.Column.TestColumnSelectExpression(
                dataMatrixInfo.database.odbcConnectionString,
                dataMatrixInfo.dataMatrixName,
                functionsIObj.ColumnSelectExpression,
                boxModule.StringIceIdentity);
        }

        #region Names of Properties
        public const string ColumnSelectExpressionPropertyName = "Name";
        #endregion
    }
}