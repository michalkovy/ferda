using Ferda.Guha.MiningProcessor;
using Ferda.Guha.MiningProcessor.Formulas;
using Ice;

namespace Ferda.Modules.Boxes.GuhaMining.Sign
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

            string inputBoxLabel = Func.GetInputBoxLabel();

            if (inputBoxLabel == null)
                return null;
            else if (inputBoxLabel.Contains(FormulaHelper.SeparatorAnd) || inputBoxLabel.Contains(FormulaHelper.SeparatorOr))
                return FormulaHelper.NegationSign + "(" + inputBoxLabel + ")";
            else
                return FormulaHelper.NegationSign + inputBoxLabel;
        }

        public override ModulesAskingForCreation[] GetModulesAskingForCreation(string[] localePrefs,
                                                                               BoxModuleI boxModule)
        {
            return new ModulesAskingForCreation[0];
        }

        public override SelectString[] GetPropertyOptions(string propertyName, BoxModuleI boxModule)
        {
            return null;
        }

        public const string typeIdentifier = "GuhaMining.Sign";

        protected override string identifier
        {
            get { return typeIdentifier; }
        }

        public override void Validate(BoxModuleI boxModule)
        {
            Functions Func = (Functions) boxModule.FunctionsIObj;

            // try to invoke methods
            IEntitySetting myES = Func.GetEntitySetting(true);
            object dummy = Func.GetAttributeNames();
            
            BooleanAttributeSettingFunctionsPrx booleanAttribute = Func.GetBitStringGeneratorPrx(true);
            if (booleanAttribute != null)
            {
                IEntitySetting eS = booleanAttribute.GetEntitySetting();
                // inner entity can not be auxiliary
                if (eS.importance == ImportanceEnum.Auxiliary)
                    throw Exceptions.BadValueError(
                        null,
                        boxModule.StringIceIdentity,
                        "Inner boolean attribute setting can not be auxiliary.",
                        new string[] {Functions.SockBooleanAttributeSetting},
                        restrictionTypeEnum.OtherReason
                        );
                if (eS.importance == ImportanceEnum.Forced &&
                    myES.importance != ImportanceEnum.Forced)
                    throw Exceptions.BadValueError(
                        null,
                        boxModule.StringIceIdentity,
                        "Inner boolean attribute setting is forced, therefore current has to be also forced.",
                        new string[] { Functions.SockBooleanAttributeSetting },
                        restrictionTypeEnum.OtherReason
                        );
            }
        }
    }
}