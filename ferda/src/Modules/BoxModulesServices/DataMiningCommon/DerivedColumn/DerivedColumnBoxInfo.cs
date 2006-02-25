using System;
using System.Collections.Generic;

namespace Ferda.Modules.Boxes.DataMiningCommon.DerivedColumn
{
    class DerivedColumnBoxInfo : Ferda.Modules.Boxes.BoxInfo
    {
        public const string typeIdentifier =
            "DataMiningCommon.DerivedColumn";

        protected override string identifier
        {
            get { return typeIdentifier; }
        }

        public override void CreateFunctions(BoxModuleI boxModule, out Ice.Object iceObject, out IFunctions functions)
        {
            DerivedColumnFunctionsI result = new DerivedColumnFunctionsI();
            iceObject = (Ice.Object)result;
            functions = (IFunctions)result;
        }

        public override string[] GetBoxModuleFunctionsIceIds()
        {
            return DerivedColumnFunctionsI.ids__;
        }

        /// <summary>
        /// Gets default value for box module user label.
        /// </summary>
        public override string GetDefaultUserLabel(BoxModuleI boxModule)
        {
            return boxModule.GetPropertyString("Formula");
        }

        /// <summary>
        /// Gets array of <see cref="T:Ferda.Modules.SelectString"/> as
        /// options for property, whose options are dynamically variable.
        /// </summary>
        public override SelectString[] GetPropertyOptions(String propertyName, BoxModuleI boxModule)
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
            PropertySetting propertySetting;
            Ferda.ModulesManager.BoxModuleProjectInformationPrx projectInfoPrx = boxModule.Manager.getProjectInformation();
            string label = projectInfoPrx.getUserLabel(boxModule.StringIceIdentity);
            ValueSubTypeEnum columnValueSubType = (ValueSubTypeEnum)(Enum.Parse(typeof(ValueSubTypeEnum), boxModule.GetPropertyString("ValueSubType")));
            foreach (string moduleAFCName in modulesAFC.Keys)
            {
                moduleAFC = modulesAFC[moduleAFCName];
                moduleConnection = new ModulesConnection();
                singleModuleAFC = new ModuleAskingForCreation();
                switch (moduleAFCName)
                {
                    case "Attribute":
                    case "EachValueOneCategoryAttribute":
                    case "EquidistantIntervalsAttribute":
                    case "EquifrequencyIntervalsAttribute":
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
                singleModuleAFC.newBoxModuleUserLabel = new string[] { label };
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
            DerivedColumnFunctionsI Func = (DerivedColumnFunctionsI)boxModule.FunctionsIObj;
            switch (propertyName)
            {
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
            DerivedColumnFunctionsI Func = (DerivedColumnFunctionsI)boxModule.FunctionsIObj;
            if (Func.TestColumnSelectExpressionAction())
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
    }
}